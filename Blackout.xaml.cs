using Microsoft.Win32;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Animation;

namespace FocusMode
{
    public partial class Blackout : Window
    {
        // List to keep track of blacked out screens.
        private List<Window> blackoutWindows = new List<Window>();

        public Blackout()
        {
            InitializeComponent();
            Loaded += (s, e) => FadeIn();
        }

        // Fade-in effect when the window is loaded.
        private void FadeIn()
        {
            var fade = new DoubleAnimation
            {
                From = 0,
                To = 1.0,
                Duration = TimeSpan.FromMilliseconds(400),
                FillBehavior = FillBehavior.HoldEnd
            };
            this.BeginAnimation(Window.OpacityProperty, fade);
        }

        // Fade-out effect.
        public void FadeOutAndClose()
        {
            foreach (var window in blackoutWindows)
            {
                var fade = new DoubleAnimation
                {
                    From = window.Opacity,
                    To = 0,
                    Duration = TimeSpan.FromMilliseconds(400),
                    FillBehavior = FillBehavior.Stop
                };
                window.BeginAnimation(Window.OpacityProperty, fade);
                fade.Completed += (s, e) =>
                {
                    // Close the window after it fades out.
                    window.Close(); 
                };
            }

            // Remove all closed windows after fading out.
            blackoutWindows.Clear();
        }

        // Blackout all screens except the primary display.
        public void BlackoutScreen()
        {
            // Get primary screen devicename from registry.
            string? primaryScreen;
            using (RegistryKey? regEntry = Registry.CurrentUser.OpenSubKey(@"Software\\FocusMode", false))
            {
                primaryScreen = regEntry?.GetValue("PrimaryScreen")?.ToString();
            }

            // If registry entry does not exist, prompt user to select primary screen).
            if (string.IsNullOrEmpty(primaryScreen))
            {
                var dialog = new ScreenSelector();
                bool? result = dialog.ShowDialog();
            }

            // Blackout all screens but primary screen.
            else
            {
                foreach (var screen in Screen.AllScreens)
                {
                    string currScreen = screen.DeviceName;
                    if (currScreen != primaryScreen)
                    {
                        var blackout = new Blackout
                        {
                            WindowStartupLocation = WindowStartupLocation.Manual,
                            WindowStyle = WindowStyle.None,
                            ResizeMode = ResizeMode.NoResize,
                            Topmost = true,
                            ShowInTaskbar = false
                        };

                        var bounds = screen.Bounds;

                        // Convert screen coordinates (pixels) to WPF units and position the window.
                        blackout.SourceInitialized += (s, e) =>
                        {
                            var handle = new System.Windows.Interop.WindowInteropHelper(blackout).Handle;
                            NativeMethods.SetWindowPos(handle, IntPtr.Zero, bounds.X, bounds.Y, bounds.Width, bounds.Height, 0);
                        };

                        blackoutWindows.Add(blackout);
                        blackout.ShowActivated = true;
                        blackout.Show();
                    }
                }
            }
        }
    }
}
