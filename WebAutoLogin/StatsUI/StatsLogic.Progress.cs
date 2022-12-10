using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WebAutoLogin.StatsUI;

public partial class StatsLogic
{
    internal void ProgressChanged()
        => Application.Current.Dispatcher.Invoke(ApplyProgressChange);

    private void ApplyProgressChange()
    {
        StatsData.Progress = _connectorService?.Data.Progress ?? 0;
    }
}
