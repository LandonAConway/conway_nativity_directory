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

namespace Conway_Nativity_Directory
{
    /// <summary>
    /// Interaction logic for NamingOptionsDialog.xaml
    /// </summary>
    public partial class NamingOptionsDialog : Window
    {
        public NamingOptionsDialog()
        {
            InitializeComponent();
            LoadCheckstates();
        }

        bool successful = false;
        public bool Successful { get { return successful; } }
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.successful = true;
            this.Close();
        }

        public void CheckAll()
        {
            this.idOption.IsChecked = true;
            this.titleOption.IsChecked = true;
            this.originOption.IsChecked = true;
            this.acquiredOption.IsChecked = true;
            this.fromOption.IsChecked = true;
            this.costOption.IsChecked = true;
            this.locationOption.IsChecked = true;
            this.tagsOption.IsChecked = true;
            this.geographicalOriginsOption.IsChecked = true;
        }

        public void UncheckAll()
        {
            this.idOption.IsChecked = false;
            this.titleOption.IsChecked = false;
            this.originOption.IsChecked = false;
            this.acquiredOption.IsChecked = false;
            this.fromOption.IsChecked = false;
            this.costOption.IsChecked = false;
            this.locationOption.IsChecked = false;
            this.tagsOption.IsChecked = false;
            this.geographicalOriginsOption.IsChecked = false;
        }

        private void LoadCheckstates()
        {
            Dictionary<string, CheckBox> properties = new Dictionary<string, CheckBox>();
            properties.Add("id", idOption);
            properties.Add("title", titleOption);
            properties.Add("origin", originOption);
            properties.Add("acquired", acquiredOption);
            properties.Add("from", fromOption);
            properties.Add("cost", costOption);
            properties.Add("location", locationOption);
            properties.Add("tags", tagsOption);
            properties.Add("geographicalOrigins", geographicalOriginsOption);

            foreach (var item in properties)
            {
                bool state = getBool(App.GlobalMeta["namingOptionsDialog_option_" + item.Key]);
                item.Value.IsChecked = state;
            }
        }

        private void SaveCheckstates()
        {
            Dictionary<string, CheckBox> properties = new Dictionary<string, CheckBox>();
            properties.Add("id", idOption);
            properties.Add("title", titleOption);
            properties.Add("origin", originOption);
            properties.Add("acquired", acquiredOption);
            properties.Add("from", fromOption);
            properties.Add("cost", costOption);
            properties.Add("location", locationOption);
            properties.Add("tags", tagsOption);
            properties.Add("geographicalOrigins", geographicalOriginsOption);

            foreach (var item in properties)
                App.GlobalMeta["namingOptionsDialog_option_" + item.Key] = item.Value.IsChecked.ToString();
        }

        private bool getBool(string value)
        {
            if (value.ToLower() == "true")
                return true;
            return false;
        }

        private void checkstateChanged(object sender, RoutedEventArgs e)
        {
            SaveCheckstates();
        }
    }
}
