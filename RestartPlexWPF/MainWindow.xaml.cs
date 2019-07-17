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

namespace AppRestarter
{
    public partial class MainWindow : Window
    {
        private const string title = "App Watcher";
        AppWatcher MainAppWatcher = new AppWatcher();
        List<CheckBox> ListBoxCheckboxes = new List<CheckBox>();
        Process[] currentProcesses = Process.GetProcesses();
        StatusCheck statusCheck = new StatusCheck
        {
            KeepAllAppsRunning = true,
            ThreadSleepTime = 5000
        };

        public MainWindow()
        {
            InitializeComponent();
            Title = title;
            FillOutListBoxWithCheckboxes();
        }

        private void FillOutListBoxWithCheckboxes()
        {
            ListBoxCheckboxes.Clear();

            foreach (var process in currentProcesses.OrderBy(x => x.ProcessName))
            {                
                var tempCheckbox = new CheckBox
                {
                    Name = $"ProcessID{process.Id.ToString()}",
                    Content = process.ProcessName,
                    ToolTip = process.Id.ToString()
                };
                ListBoxCheckboxes.Add(tempCheckbox);

            }
            CurrentProcessesLB.ItemsSource = ListBoxCheckboxes;
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            var processIDs = new List<int>();
            var checkedBoxesNames = CurrentProcessesLB.Items.OfType<CheckBox>().Where(x => x.IsChecked == true).ToList();
            //statusCheck.AppInfo.Clear();
            var tempString = "";
            foreach (var cb in checkedBoxesNames)
            {
                var processIDName = cb.Name.ToString().Substring(9);
                int processID = Convert.ToInt32(processIDName);
                processIDs.Add(processID);
                
            }

            var checkedProcesses = Process.GetProcesses().Where(x => processIDs.Contains(x.Id));
            foreach (var process in checkedProcesses)
            {
                var tempAppInfo = new AppInfo
                {
                    AppProcess = process,
                    AppId = process.Id,
                    AppName = process.ProcessName,
                    AppPath = process.MainModule.FileName,
                    KeepRunning = true
                };

                if (!statusCheck.AppInfo.Contains(tempAppInfo))
                {
                    statusCheck.AppInfo.Add(tempAppInfo);
                    tempString += $"{tempAppInfo.AppName} - ID: {tempAppInfo.AppId} {Environment.NewLine}";
                }
            }
            //checkedProcesses.ToList().ForEach(x => statusCheck.AppInfo.Add(new AppInfo { AppId = x.Id,AppName = x.ProcessName, AppPath = x.MainModule.FileName, AppProcess = x, KeepRunning = true }));
            StartStopProcess(statusCheck);
            AppWatcherWindow.Title = title + "Watching Applications";
            WatchingLabel.Content = tempString;
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            statusCheck.AppInfo.Clear();
            StartStopProcess(statusCheck);
            AppWatcherWindow.Title = title;
            WatchingLabel.Content = null;
        }

        public async void StartStopProcess(StatusCheck statusCheck)
        {
            await MainAppWatcher.AppWatch(statusCheck);
        }

        private void FilterBox_KeyUp(object sender, KeyEventArgs e)
        {
            BoxFilter();
        }

        private void BoxFilter()
        {
            if (string.IsNullOrWhiteSpace(FilterBox.Text))
            {
                CurrentProcessesLB.ItemsSource = ListBoxCheckboxes;                
            }
            else if (FilterBox.Text.Count() > 1 && FilterBox.Text.Count() < 2)
            {
                CurrentProcessesLB.ItemsSource = ListBoxCheckboxes.Where(x => x.Content.ToString().StartsWith(FilterBox.Text, StringComparison.OrdinalIgnoreCase));
            }
            else if (FilterBox.Text.Count() >= 2)
            {
                CurrentProcessesLB.ItemsSource = ListBoxCheckboxes.Where(x => x.Content.ToString().ToUpperInvariant().Contains(FilterBox.Text.ToUpperInvariant()));
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            currentProcesses = Process.GetProcesses();
            FillOutListBoxWithCheckboxes();
            BoxFilter();
        }
    }
}
