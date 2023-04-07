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
    public partial class CachePreferenceUI : UserControl
    {
        private CachePreference cachePreference;
        public CachePreference CachePreference { get { return cachePreference; } }
        public CachePreferenceUI(CachePreference cachePreference)
        {
            InitializeComponent();
            this.cachePreference = cachePreference;
            SetBindings();
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.SelectedPath = cacheFolderTextBox.Text;
            
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                cacheFolderTextBox.Text = fbd.SelectedPath;
            }
        }

        private void SetBindings()
        {
            App.SetBinding("CacheFolder", cachePreference, cacheFolderTextBox, TextBox.TextProperty,
                BindingMode.OneWay, UpdateSourceTrigger.Default);
        }
    }
}
