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
        public async Task<int> AppWatch(StatusCheck statusCheck)
        {
            var result = await Execute(statusCheck);
            return result;
        }

        private async Task<int> Execute(StatusCheck statusCheck)
        {
            while (statusCheck.KeepAllAppsRunning)
            {
                var appsToWatch = statusCheck.AppInfo.Where(x => x.KeepRunning);
                foreach (var app in appsToWatch)
                {
                    app.AppProcess = await CheckAppStatus(app.AppId);
                    if (app.AppProcess == null)
                    {
                        var newProcess = Process.Start(app.AppPath);
                        app.AppId = newProcess.Id;
                        await Log(app.AppName + " Failed and was restarted.");
                        continue;
                    }

                    await Log(app.AppName);
                }

                await Task.Delay(statusCheck.ThreadSleepTime);
            }
            return 0;
        }

        private async Task Log(string logInfo)
        {
            using (StreamWriter writer = new StreamWriter(@"C:\temp\AppWatcherLog.txt"))
            {
                writer.WriteLine($"Watching App: {logInfo}");
            }
        }

        private async Task<Process> CheckAppStatus(int appId)
        {
            var currentProcesses = Process.GetProcesses();
            var appProcess = currentProcesses.FirstOrDefault(x => x.Id.Equals(appId));
            return appProcess;
        }
    }
}
