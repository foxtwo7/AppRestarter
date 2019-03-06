using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace RestartPlexWPF
{
    public class AppWatcher
    {
        public async void AppWatch(StatusCheck statusCheck, string appName)
        {
            var result = await Task.Run(() => Execute(statusCheck, appName));
        }

        private int Execute(StatusCheck statusCheck, string appName)
        {
            while (statusCheck.KeepThreadRunning)
            {
                statusCheck.AppProcess = CheckAppStatus(appName);
                if (statusCheck.AppProcess == null)
                {
                    Process.Start(statusCheck.AppPath);
                }
                Thread.Sleep(statusCheck.ThreadSleepTime);
            }
            return 0;
        }

        private static Process CheckAppStatus(string appName)
        {
            var currentProcesses = Process.GetProcesses();
            var plexMediaServer = currentProcesses.FirstOrDefault(x => x.ProcessName.StartsWith(appName, StringComparison.OrdinalIgnoreCase));
            return plexMediaServer;
        }
    }
}
