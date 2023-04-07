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
using System.IO;

namespace Conway_Nativity_Directory
{
    /// <summary>
    /// Interaction logic for ChangelogsWindow.xaml
    /// </summary>
    public partial class ChangelogsWindow : Window
    {
        public ChangelogsWindow()
        {
            InitializeComponent();
            versionsListView.SelectionChanged += VersionsListView_SelectionChanged;
            Populate();

            if (versionsListView.Items.Count > 0)
                versionsListView.SelectedIndex = versionsListView.Items.Count - 1;
        }

        private void Populate()
        {
            string changelogDir = AppDomain.CurrentDomain.BaseDirectory + @"\changelogs";
            if (Directory.Exists(changelogDir))
            {
                foreach (string dir in Directory.EnumerateDirectories(changelogDir))
                {
                    string name = System.IO.Path.GetFileName(dir);
                    string changelog = File.ReadAllText(dir + @"\changelog.txt");

                    versionsListView.Items.Add(new ListViewItem()
                    {
                        Content = name,
                        Tag = changelog
                    });
                }
            }
        }

        private void VersionsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (versionsListView.Items.Count > 0)
                changeLog.Text = ((ListViewItem)versionsListView.SelectedItem).Tag.ToString();
        }
    }
}
