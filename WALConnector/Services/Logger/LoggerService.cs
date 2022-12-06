using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WALConnector.Helpers;

namespace WALConnector.Services.Logger;

internal class LoggerService
{
    private readonly ReaderWriterLockSlim _lock = new();

    private readonly string _logPath;
    private readonly int _delay;

    private string _guid = string.Empty;
    private readonly List<string> _logs = new();

    public LoggerService(string logPath, int delay = 50)
    {
        _logPath = logPath;
        _delay = delay;
    }

    public void LogThis(string message)
    {
        _lock.EnterWriteLock();
        _logs.Add(GetTimeStamp() + " : " + message);
        var guid = Guid.NewGuid().ToString();
        _guid = guid.DeepClone();
        _lock.ExitWriteLock();

        Task.Run(async () => await StartDelayedLogger(guid));
    }

    private async Task StartDelayedLogger(string guid)
    {
        _lock.EnterWriteLock();
        await Task.Delay(_delay);
        if(guid.Equals(_guid, StringComparison.OrdinalIgnoreCase))
        {
            await File.AppendAllLinesAsync(_logPath, _logs);
            _logs.Clear();
        }
        _lock.ExitWriteLock();
    }

    private static string GetTimeStamp()
    {
        return DateTime.Now.ToString();
    }
}