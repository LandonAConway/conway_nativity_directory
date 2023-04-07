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
using Microsoft.Win32;

namespace Conway_Nativity_Directory
{
    /// <summary>
    /// Interaction logic for CachePreferenceUI.xaml
    /// </summary>
    public partial class StartupPreferenceUI : UserControl
    {
        private StartupPreference startupPreference;
        public StartupPreference StartupPreference { get { return startupPreference; } }
        public StartupPreferenceUI(StartupPreference startupPreference)
        {
            InitializeComponent();
            this.startupPreference = startupPreference;
            SetBindings();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = fileTextBox.Text;
            ofd.Filter = "Conway Nativity Directory Project (*.cnp)|*.cnp|Conway Nativity Directory Backup (*.cnb)|*.cnb";
            
            if (ofd.ShowDialog() == true)
            {
                fileTextBox.Text = ofd.FileName;
            }
        }

        private void SetBindings()
        {
            App.SetBinding("AutoLoadFile", startupPreference, fileTextBox, TextBox.TextProperty,
                BindingMode.OneWay, UpdateSourceTrigger.Default);
        }
    }
}
