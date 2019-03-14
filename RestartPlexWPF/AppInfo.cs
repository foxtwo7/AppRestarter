using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace RestartPlexWPF
{
    public class AppInfo
    {
        public int AppId { get; set; }

        public string AppName { get; set; }

        public bool KeepRunning { get; set; }

        public Process AppProcess { get; set; }

        public string AppPath { get; set; }

    }
}

