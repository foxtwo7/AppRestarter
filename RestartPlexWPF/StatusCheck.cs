using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppRestarter
{
    public class StatusCheck
    {
        public int ThreadSleepTime { get; set; }

        public bool KeepAllAppsRunning { get; set; }

        public List<AppInfo> AppInfo { get; set; } = new List<AppInfo>();
    }
}
