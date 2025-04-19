using Microsoft.Win32;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using MessageBox = System.Windows.MessageBox;
using Application = System.Windows.Application;


namespace FocusMode
{
    public partial class MainWindow : Window
    {
        

        private Blackout _blackout;
        private bool _startupChecked;
        
        public bool StartupChecked
        {
            get => _startupChecked;
            set
            {
                if (_startupChecked != value)
                {
                    _startupChecked = value;
                    OnPropertyChanged(nameof(StartupChecked));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            FirstRunCheck();
            Check_RunOnStartup();
            _blackout = new Blackout();
        }

        public static Screen GetCurrentScreen(Window window)
        {
            // Return current screen.
            var windowInteropHelper = new WindowInteropHelper(window);
            var handle = windowInteropHelper.Handle;
            return Screen.FromHandle(handle);
        }

        private void SetPrimaryDisplay()
        {
            // Launch dialog to select primary display.
            var dialog = new ScreenSelector();
            bool? result = dialog.ShowDialog();
        }

        private void FirstRunCheck()
        {
            using (RegistryKey? regEntry = Registry.CurrentUser.OpenSubKey(@"Software\\FocusMode", true))
            {
                if (regEntry == null)
                {
                    using (RegistryKey newEntry = Registry.CurrentUser.CreateSubKey(@"Software\\FocusMode"))
                    {
                        newEntry?.SetValue("FirstRun", "1");
                    }

                    MessageBox.Show(this, "Focus Mode is now running and will continue to run in the System Tray, simply right click the icon to view options.\n\nYou won't see the message in future.",
                        "First Run",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);

                    SetPrimaryDisplay();
                }
            }
        }
        private void Activate_Click(object sender, RoutedEventArgs e)
        {
            // Blackout all screens but primary.
            _blackout.BlackoutScreen();
        }

        private void Deactivate_Click(object sender, RoutedEventArgs e)
        {
            // Fadeout and close blacked out screens.
            _blackout.FadeOutAndClose();
        }
        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this, "A simple app that blacks out all screens but primary at the tap of a button!",
                            "About", 
                            MessageBoxButton.OK, 
                            MessageBoxImage.Information);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            // Confirm exit.
            if (MessageBox.Show(this, "Really exit?",
                    "Exit",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
        private void RunOnStartup_Click(object sender, RoutedEventArgs e)
        {
            // Enable/disable run on startup through registry entry.
            using (RegistryKey? regEntry = Registry.CurrentUser.OpenSubKey(@"Software\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                if (_startupChecked)
                {
                    regEntry?.SetValue("FocusMode", $"\"{Process.GetCurrentProcess().MainModule?.FileName}\"");
                }
                else
                {
                    regEntry?.DeleteValue("FocusMode", false);
                }
            }
        }

        private void Check_RunOnStartup()
        {
            using (RegistryKey? regEntry = Registry.CurrentUser.OpenSubKey(@"Software\\Microsoft\\Windows\\CurrentVersion\\Run", false))
            {
                _startupChecked = regEntry?.GetValue("FocusMode") != null;
            }
        }
        private void SetPrimaryScreen_Click(object sender, RoutedEventArgs e)
        {
            SetPrimaryDisplay();
        }
    }
}
