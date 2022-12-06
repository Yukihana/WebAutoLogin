using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

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
        _guid = Guid.NewGuid().ToString();
        _lock.ExitWriteLock();

        Task.Run(async () => await StartDelayedFlush(_guid));
    }

    private async Task StartDelayedFlush(string guid)
    {
        await Task.Delay(_delay);
        Task? task = null;

        try
        {
            _lock.EnterWriteLock();

            if (!guid.Equals(_guid))
                return;

            List<string> buffer = new(_logs);
            _logs.Clear();
            task = File.AppendAllLinesAsync(_logPath, buffer);
        }
        finally
        {
            _lock.ExitWriteLock();
        }

        if(task != null)
        {
            // awaited later to release the lock faster
            task.Start();
            await task;
        }
    }

    private static string GetTimeStamp()
        => DateTime.Now.ToString("yyyy.MM.dd_HH:mm:ss.fff");
}