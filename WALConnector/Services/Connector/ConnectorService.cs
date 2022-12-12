using WALConnector.Helpers;
using WALConnector.Services.Configuration;
using WALConnector.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WALConnector.Services.Logger;
using System.Net.NetworkInformation;
using WALConnector.Services.LatencyAnalysis;

namespace WALConnector.Services.Connector;

/// <summary>
/// The connection logic routine is coordinated here.
/// Don't clutter it up. Keep it concise. Handle boilerplates in separate files.
/// </summary>
public class ConnectorService
{
    private readonly ConnectorData _data = new();
    public ConnectorData Data => _data;

    private readonly LoggerService _logger = new("log.txt");
    private CancellationTokenSource _ctSource = new();

    public bool IsDebug { get; set; } = true;

    private Task? _task = null;

    public Action? OnStarted;

    public Action? OnProgressChanged;

    public Action? OnPingsPolled;

    public Action? OnLoginAttempted;

    public Action? OnLoginSucceeded;

    public async Task<bool> Start()
    {
        var config = await ConfigurationService.Load();
        if (config == null)
            return false;

        // Stop running task
        await Stop().ConfigureAwait(false);

        // Load new config values
        _data.ApplyFrom(config);
        _data.Initialize();
        if (_data.LoginBehaviour == LoginBehaviour.Disabled && !_data.Validate())
            return false;

        // Setup new routine
        _ctSource = new CancellationTokenSource();
        _task = Task.Factory.StartNew(async ()
            => await ManageAsync(_ctSource.Token).ConfigureAwait(false),
            TaskCreationOptions.LongRunning);

        _logger.LogThis("Started the connector service.");
        return true;
    }
    public async Task Stop()
    {
        if (_task != null)
        {
            _ctSource.Cancel();
            await _task;
            _logger.LogThis("Stopped the connector service.");
            _task = null;
        }
    }
    public void Decouple()
    {
        OnStarted = null;
        OnProgressChanged = null;
        OnPingsPolled = null;
        OnLoginAttempted = null;
        OnLoginSucceeded = null;
    }

    public async Task ManageAsync(CancellationToken token = default)
    {
        int pingInterval = _data.PingPollInterval;
        int loginMultiplier = _data.LoginPollMultiplier;
        int pingIntervalCounter = int.MaxValue;
        int loginMultiplierCounter = int.MaxValue;
        bool lastConnected;
        bool? loginSuccess;

        while (!token.IsCancellationRequested)
        {
            _data.Progress = pingIntervalCounter / (float)pingInterval;
            _ = Task.Run(() => OnProgressChanged?.Invoke(), CancellationToken.None);

            // Ping wait cycle
            if (pingIntervalCounter < pingInterval)
            {
                await Task.Delay(1000, token);
                pingIntervalCounter++;
                continue;
            }
            pingIntervalCounter = 0;

            if (IsDebug)
                _logger.LogThis("Starting ping polling cycle");

            // Pings
            await PollAllTargets(token);
            _ = Task.Run(()=>OnPingsPolled?.Invoke(), CancellationToken.None);

            // Process Login conditions
            if (_data.LoginBehaviour == LoginBehaviour.Disabled)
                continue;
            lastConnected = IsConnected();
            if (lastConnected && _data.LoginBehaviour == LoginBehaviour.OnDisconnected)
                continue;

            // Login wait cycle
            if (loginMultiplierCounter < loginMultiplier)
            {
                loginMultiplierCounter++;
                continue;
            }
            loginMultiplierCounter = 0;

            // Logins
            loginSuccess = false;
            if (!lastConnected || _data.LoginBehaviour == LoginBehaviour.Always)
                loginSuccess = await AttemptLogin(token); //returns false if already logged in or failed
            _ = Task.Run(() => OnLoginAttempted?.Invoke(), CancellationToken.None);

            if(IsDebug && loginSuccess == null)
                _logger.LogThis("Login failed.");

            // Poll destinations once more if connected state changed
            if (!lastConnected && loginSuccess == true)
            {
                await PollAllTargets(token);
                _ = Task.Run(() => OnLoginSucceeded?.Invoke(), CancellationToken.None);
            }
        }
    }

    private async Task PollAllTargets(CancellationToken token = default)
    {
        if(token.IsCancellationRequested) return;

        List<LatencyStatistics> pingTargets = new();
        if (_data.Gateway != null)
            pingTargets.Add(_data.Gateway);
        if (_data.Portal != null)
            pingTargets.Add(_data.Portal);

        if (_data.Destinations.Count > 0)
        {
            if (_data.DestinationPollingMode == PingGroupPollingMode.SeriallyAlternated)
            {
                pingTargets.Add(_data.Destinations[_data.DestinationIndex]);

                _data.DestinationIndex++;
                if (_data.DestinationIndex >= _data.Destinations.Count)
                    _data.DestinationIndex = 0;
            }
            else
            {
                pingTargets.AddRange(_data.Destinations);
            }
        }

        pingTargets.AddRange(_data.Nodes);

        int timeout = _data.PingTimeout;
        int maxPings = _data.MaximumPingsCount;
        await Parallel.ForEachAsync(pingTargets, async (x, ctoken) => await PollTarget(x, timeout, maxPings, ctoken));
    }
    private async Task PollTarget(LatencyStatistics data, int timeout, int maxPings, CancellationToken token = default)
    {
        try
        {
            using Ping P = new();
            PingReply reply = await P.SendPingAsync(data.Address, timeout);
            await Task.Run(() => data.Append(reply, maxPings), token);

            if (IsDebug)
            {
                if (reply.Status == IPStatus.Success)
                    _logger.LogThis($"Ping {data.Address} [{reply.Address}] => {data.LastRoundTripTime} ms");
                else
                    _logger.LogThis($"Ping {data.Address} [{reply.Address}] :: {reply.Status}");
            }
        }
        catch
        {
            _logger.LogThis($"Ping ({data.Address}) => Error!!!");
        }
    }

    private bool IsConnected() =>
        _data.Destinations.Any(x => x.LastRoundTripTime != -1);

    public async Task<bool?> AttemptLogin(CancellationToken token)
    {
        try
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
        catch
        {
            return null;
        }
    }
}