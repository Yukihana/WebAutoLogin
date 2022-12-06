using WALConnector.Helpers;
using WALConnector.Services.Configuration;
using WALConnector.Services.PingStats;
using WALConnector.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WALConnector.Services.Connector;

/// <summary>
/// The connection logic routine is coordinated here.
/// Don't clutter it up. Keep it concise. Handle boilerplates in separate files.
/// </summary>
public class ConnectorService
{
    private readonly ConnectorData _data = new();
    private CancellationTokenSource _ctSource = new();

    private Task? _task = null;

    public Action? OnPingsPolled;

    public Action? OnLoginAttempted;

    public Action? OnLoginSucceeded;

    public async Task<bool> Initialize()
    {
        if (await ConfigurationService.Load() is LoginConfig config)
        {
            _data.ApplyFrom(config);
            _data.Initialize();
            await TryStart().ConfigureAwait(false);
            return true;
        }
        return false;
    }
    public void Decouple()
    {
        OnPingsPolled = null;
        OnLoginAttempted = null;
        OnLoginSucceeded = null;
    }

    public async Task TryStart()
    {
        await EndRoutine().ConfigureAwait(false);
        await StartRoutine().ConfigureAwait(false);
    }

    public void TryEnd()
        => Task.Run(EndRoutine);

    // Rewrite this method to include initialize. Let it be big, unify config into it but also add breakout and terminate.
    private async Task StartRoutine()
    {
        var config = await ConfigurationService.Load();
        if (config == null)
            return;
        _data.ApplyFrom(config);
        if (_data.LoginBehaviour != LoginBehaviour.Disabled && !_data.Validate())
            return;
        _ctSource = new CancellationTokenSource();
        _task = Task.Factory.StartNew(async ()
            => await ManageAsync(_ctSource.Token).ConfigureAwait(false),
            TaskCreationOptions.LongRunning);
    }
    private async Task EndRoutine()
    {
        if (_task != null)
        {
            _ctSource.Cancel();
            await _task;
            _task = null;
        }
    }

    public async Task ManageAsync(CancellationToken token = default)
    {
        int pingInterval = _data.PingPollInterval;
        int loginMultiplier = _data.LoginPollMultiplier;
        int pingIntervalCounter = 0;
        int loginMultiplierCounter = 0;
        bool lastConnected, loginSuccess;

        while (!token.IsCancellationRequested)
        {
            // Ping wait cycle
            if (pingIntervalCounter < pingInterval)
            {
                await Task.Delay(1000, token);
                pingIntervalCounter++;
                continue;
            }
            pingIntervalCounter = 0;

            // Pings
            await PollPings(token);
            _ = Task.Run(()=>OnPingsPolled?.Invoke(), CancellationToken.None);

            // Login wait cycle
            if (loginMultiplierCounter < loginMultiplier)
            {
                loginMultiplierCounter++;
                continue;
            }
            loginMultiplierCounter = 0;

            // Logins
            if (_data.LoginBehaviour == LoginBehaviour.Disabled)
                continue;
            lastConnected = IsConnected();
            loginSuccess = false;
            if (!lastConnected || _data.LoginBehaviour == LoginBehaviour.Always)
            {
                loginSuccess = await AttemptLogin(token); //returns false if already logged in or failed
            }
            _ = Task.Run(() => OnLoginAttempted?.Invoke(), CancellationToken.None);

            // Poll destinations once more if connected state changed
            if (!lastConnected && loginSuccess)
            {
                await PollPings(token);
                _ = Task.Run(() => OnLoginSucceeded ?.Invoke(), CancellationToken.None);
            }
        }
    }

    private async Task PollPings(CancellationToken token = default)
    {
        List<Task> pingList = new();

        if (_data.Gateway != null)
            pingList.Add( _data.Gateway.SendPing(_data.Options, token));
        if (_data.Portal != null)
            pingList.Add(_data.Portal.SendPing(_data.Options, token));

        pingList.AddRange(_data.Destinations.Select(x => x.SendPing(_data.Options, token)));
        pingList.AddRange(_data.Nodes.Select(x => x.SendPing(_data.Options, token)));

        await Task.WhenAll(pingList);
    }

    private bool IsConnected() =>
        _data.Destinations.Any(x => x.PingLatency != -1);

    public async Task<bool> AttemptLogin(CancellationToken token)
    {
        if (_data.Portal == null || string.IsNullOrEmpty(_data.Portal.Address))
            return false;
        using HttpClient client = new();
        var responseString = await client.GetStringAsync(_data.Portal.Address, token).ConfigureAwait(false);

        // Return false if already logged in
        if (responseString.Contains(_data.ValidationString, StringComparison.OrdinalIgnoreCase))
            return false;

        // Do Login
        if (_data.LoginMethodIsPost)
        {
            var response = await client.PostAsync(_data.Portal.Address, _data.EncodedParams, token).ConfigureAwait(false);
            responseString = await response.Content.ReadAsStringAsync(token).ConfigureAwait(false);
        }
        else
        {
            responseString = await client.GetStringAsync(_data.Portal.Address + "?" + _data.EncodedParams, token);
        }

        // Determine if login succeeded, return
        return responseString.Contains(_data.ValidationString, StringComparison.OrdinalIgnoreCase);
    }
}