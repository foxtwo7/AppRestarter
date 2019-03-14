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
        List<CheckBox> ListBoxCheckboxes = new List<CheckBox>();

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
                //tempCheckbox.Content = process;
                ListBoxCheckboxes.Add(tempCheckbox);
                
            }
            CurrentProcessesLB.ItemsSource = ListBoxCheckboxes;
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            var processIDs = new List<int>();
            var checkedBoxesNames = CurrentProcessesLB.Items.OfType<CheckBox>().Where(x => x.IsChecked == true).ToList();

            foreach (var cb in checkedBoxesNames)
            {
                var processIDName = cb.Name.ToString().Substring(9);
                int processID = Convert.ToInt32(processIDName);
                processIDs.Add(processID);
            }

            Console.WriteLine(processIDs.Count());
            var checkedProcesses = Process.GetProcesses().Where(x => processIDs.Contains(x.Id));

            StatusCheck statusCheck = new StatusCheck
            {
                KeepAllAppsRunning = true,
                ThreadSleepTime = 5000
            };
            checkedProcesses.ToList().ForEach(x => statusCheck.AppInfo.Add(new AppInfo { AppId = x.Id, AppName = x.ProcessName, AppPath = x.MainModule.FileName, AppProcess = x, KeepRunning = true }));
            StartStopProcess(statusCheck);
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            StatusCheck statusCheck = new StatusCheck
            {
                KeepAllAppsRunning = false,
                ThreadSleepTime = 5000
            };
            StartStopProcess(statusCheck);
        }

        public void StartStopProcess(StatusCheck statusCheck)
        {
            MainAppWatcher.AppWatch(statusCheck);
        }

        private void FilterBox_KeyUp(object sender, KeyEventArgs e)
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
                var a = ListBoxCheckboxes.Where(x => x.Content.ToString().ToUpperInvariant().Contains(FilterBox.Text.ToUpperInvariant()));
                CurrentProcessesLB.ItemsSource = ListBoxCheckboxes.Where(x => x.Content.ToString().ToUpperInvariant().Contains(FilterBox.Text.ToUpperInvariant()));
            }
            
        }
    }
}
