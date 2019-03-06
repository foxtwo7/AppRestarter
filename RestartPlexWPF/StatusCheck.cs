using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestartPlexWPF
{
    public class StatusCheck
    {
        public bool KeepThreadRunning { get; set; }

        public bool AppStarted { get; set; }

        public bool AppRunning { get; set; }

        public int ThreadSleepTime { get; set; }

        public System.Diagnostics.Process AppProcess { get; set; }

        public string AppPath { get; set; }

        public List<int> AppIDs { get; set; } = new List<int>();
    }
}
