using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace RestartPlexWPF
{
    public static class CurrentAppPath
    {
        public static string CurrentAppFilePath()
        {
            Process[] currentProcesses = Process.GetProcesses();
            Process appProcess = currentProcesses.FirstOrDefault(x => x.ProcessName.StartsWith("Plex Media Server", StringComparison.OrdinalIgnoreCase));
            string appPath = appProcess.MainModule.FileName;
            return appPath;
        }
    }
}
