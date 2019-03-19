using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace AppRestarter
{
    public class AppWatcher
    {
        public async void AppWatch(StatusCheck statusCheck)
        {
            var result = await Task.Run(() => Execute(statusCheck));
        }

        private int Execute(StatusCheck statusCheck)
        {
            while (statusCheck.KeepAllAppsRunning)
            {
                var appsToWatch = statusCheck.AppInfo.Where(x => x.KeepRunning);
                foreach (var app in appsToWatch)
                {
                    app.AppProcess = CheckAppStatus(app.AppName);
                    if (app.AppProcess == null)
                    {
                        Process.Start(app.AppPath);
                        Log(app.AppName + " Failed and was restarted.");
                        continue;
                    }

                    Log(app.AppName);
                }

                Thread.Sleep(statusCheck.ThreadSleepTime);
            }
            return 0;
        }

        private static void Log(string logInfo)
        {
            using (StreamWriter writer = new StreamWriter(@"C:\temp\AppWatcherLog.txt"))
            {
                writer.WriteLine($"Watching App: {logInfo}");
            }
        }

        private static Process CheckAppStatus(string appName)
        {
            var currentProcesses = Process.GetProcesses();
            var appProcess = currentProcesses.FirstOrDefault(x => x.ProcessName.StartsWith(appName, StringComparison.OrdinalIgnoreCase));
            return appProcess;
        }
    }
}
