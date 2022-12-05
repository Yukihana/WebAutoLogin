using System;
using System.Drawing;
using NotifyIcon = Hardcodet.Wpf.TaskbarNotification.TaskbarIcon;

namespace Autologin.TrayHandler;

internal class TrayAgent
{
    private NotifyIcon TrayIcon { get; set; }

    internal TrayAgent()
    {
        TrayIcon = GenerateTrayAgent();
    }

    internal void Unload()
    {
        if (TrayIcon != null)
        {
            //TrayAgent. IsVisible = false;
        }
    }

    private NotifyIcon GenerateTrayAgent()
    {
        var trayAgent = new NotifyIcon()
        {
            ContextMenu = new()
        };

        try
        {
            trayAgent.Icon = Icon.ExtractAssociatedIcon(Environment.ProcessPath ?? throw new NullReferenceException());
        }
        catch { }

        return trayAgent;
    }
}