using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ManagePluginAuthorizationCodesWindow.xaml
    /// </summary>
    public partial class ManagePluginAuthorizationCodesWindow : Window
    {
        public ManagePluginAuthorizationCodesWindow()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty AuthorizationCodesProperty = DependencyProperty.Register(
            nameof(AuthorizationCodes), typeof(ObservableCollection<string>), typeof(ManagePluginAuthorizationCodesWindow),
            new PropertyMetadata(new ObservableCollection<string>()));

        public ObservableCollection<string> AuthorizationCodes
        {
            get { return (ObservableCollection<string>)GetValue(AuthorizationCodesProperty); }
            set { SetValue(AuthorizationCodesProperty, value); }
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            InputDialog id = new InputDialog();
            id.Title = "Authorization Code";
            id.ShowDialog();
            if (id.Successful && id.Response != String.Empty)
            {
                AuthorizationCodes.Add(id.Response);
            }
        }

        private void removeBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in authorizationCodesListView.SelectedItems.Cast<string>().ToArray())
                AuthorizationCodes.Remove(item);
        }
    }
}
