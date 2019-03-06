using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace RestartPlexWPF
{
    public partial class MainWindow : Window
    {
        AppWatcher MainAppWatcher = new AppWatcher();
        string AppPath = CurrentAppPath.CurrentAppFilePath();

        public MainWindow()
        {
            InitializeComponent();
            var currentProcesses = Process.GetProcesses();
            foreach (var process in currentProcesses.OrderBy(x => x.ProcessName))
            {
                var tempCheckbox = new CheckBox();
                tempCheckbox.Name = $"ProcessID{process.Id.ToString()}";
                tempCheckbox.Content = process.ProcessName;
                tempCheckbox.ToolTip = process.Id.ToString();
                CurrentProcessesLB.Items.Add(tempCheckbox);
            }
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            var processIDs = new List<int>();
            var checkedBoxesNames = CurrentProcessesLB.Items.OfType<CheckBox>().Where(x => x.IsChecked == true).ToList();

            foreach (var cb in checkedBoxesNames)
            {
                processIDs.Add(Convert.ToInt32(cb.Name.ToSt.Substring(8)));
            }

            Console.WriteLine(processIDs.Count());
            StatusCheck statusCheck = new StatusCheck
            {
                AppIDs = processIDs,
                KeepThreadRunning = true,
                AppPath = AppPath,
                AppRunning = false,
                ThreadSleepTime = 5000
            };
            StartStopProcess(statusCheck);
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            StatusCheck statusCheck = new StatusCheck
            {
                KeepThreadRunning = false,
                AppPath = AppPath,
                AppRunning = false,
                ThreadSleepTime = 5000
            };
            StartStopProcess(statusCheck);
        }

        public void StartStopProcess(StatusCheck statusCheck)
        {
            MainAppWatcher.AppWatch(statusCheck, "Plex Media Server");
        }
    }
}
