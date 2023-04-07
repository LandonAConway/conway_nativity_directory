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
    /// Interaction logic for ProjectSettingsWindow.xaml
    /// </summary>
    public partial class ProjectSettingsWindow : Window
    {
        public ProjectSettingsWindow()
        {
            InitializeComponent();
            PopulateListView();
            settingsItems.SelectionChanged += SettingsItems_SelectionChanged;
        }

        public void PopulateListView()
        {
            if (App.Project.IsOpen)
            {
                foreach (IProjectSettings settings in App.Project.ProjectSettings)
                {
                    if (!settings.IsHidden)
                    {
                        var lvi = new ListViewItem() { Content = settings.Title, Tag = settings };
                        settingsItems.Items.Add(lvi);
                    }
                }
            }
        }

        private IProjectSettings selectedSettings
        {
            get
            {
                if (settingsItems.Items.Count > 0)
                    return (IProjectSettings)((ListViewItem)settingsItems.SelectedItem).Tag;
                else
                    return null;
            }
        }

        private void SettingsItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (selectedSettings != null)
            {
                settingsUIPanel.Content = selectedSettings.ShowUI();
                settingsTitle.Text = selectedSettings.Title;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (selectedSettings != null)
                selectedSettings.Apply();
        }
    }
}
