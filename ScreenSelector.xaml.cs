using Microsoft.Win32;
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
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace FocusMode
{
    /// <summary>
    /// Interaction logic for ScreenSelector.xaml
    /// </summary>
    public partial class ScreenSelector : Window
    {

        public string SelectedScreen { get; private set; } = "";
        public ScreenSelector()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            SelectedScreen = MainWindow.GetCurrentScreen(this).DeviceName.ToString();

                using (RegistryKey? regEntry = Registry.CurrentUser.OpenSubKey(@"Software\\FocusMode", true))
                {
                    regEntry?.SetValue("PrimaryScreen", SelectedScreen);

                }
                MessageBox.Show(this, $"Primary screen has now been selected.\nYou can change this at anytime by in the tray menu.",
                    "Primary Screen Set",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            this.Close();
        }        
    }
}
