using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ConwayNativityDirectory.PluginApi;
using ConwayNativityDirectory.PluginApi.Deployment;
using ConwayNativityDirectory.PluginApi.PluginDB;

namespace Conway_Nativity_Directory
{
    /// <summary>
    /// Interaction logic for ManagePluginsWindow.xaml
    /// </summary>
    public partial class ManagePluginsWindow : Window
    {
        public ManagePluginsWindow()
        {
            InitializeComponent();
            SetAuthorizationCodes();
            FillOnlinePlugins();
            FillInstalledPlugins();
        }

        static readonly DependencyPropertyKey SelectedItemPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(SelectedItem), typeof(PluginListViewItem), typeof(ManagePluginsWindow),
            new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedItemProperty = SelectedItemPropertyKey.DependencyProperty;

        public PluginListViewItem SelectedItem
        {
            get { return (PluginListViewItem)GetValue(SelectedItemProperty); }
            private set { SetValue(SelectedItemPropertyKey, value); }
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listView = (ListView)sender;
            SelectedItem = listView.SelectedItem as PluginListViewItem;
            if (listView.Name == "installedItemsListView")
                onlineItemsListView.UnselectAll();
            else
                installedItemsListView.UnselectAll();
        }

        static readonly List<PluginActionData> installedPluginActionDatas = new List<PluginActionData>();
        void FillInstalledPlugins()
        {
            installedItemsListView.Items.Clear();
            foreach (var item in PluginDatabaseMain.LoadedPlugins)
            {
                InstalledPluginItem ipi = new InstalledPluginItem(item.Plugin.Name);
                ipi.PluginIsEnabled = item.PluginSettings.IsEnabled;

                var pad = installedPluginActionDatas.Where(x => x.Name == item.Plugin.Name).FirstOrDefault();
                if (pad != null)
                    ipi.ActionIndex = pad.Index;

                installedItemsListView.Items.Add(ipi);
            }
        }

        static readonly List<PluginActionData> onlinePluginActionDatas = new List<PluginActionData>();
        void FillOnlinePlugins()
        {
            onlineItemsListView.Items.Clear();
            foreach (var item in new SharedPluginCollection(AuthorizationCodes))
            {
                if (PluginDatabaseMain.LoadedPlugins.Where(a => a.Plugin.Name == item.Name).Count() <= 0
                    && item.RequiredVersion <= (System.Version)App.Version)
                {
                    SharedPluginItem spi = new SharedPluginItem(item);

                    var pad = onlinePluginActionDatas.Where(x => x.Name == item.Name).FirstOrDefault();
                    if (pad != null)
                        spi.ActionIndex = pad.Index;

                    onlineItemsListView.Items.Add(spi);
                }
            }
        }

        void SetAuthorizationCodes()
        {
            string arrJson = App.GlobalMeta["pluginAuthorizationCodes"];
            if (arrJson == null || arrJson == String.Empty)
                arrJson = "[]";
            AuthorizationCodes = JsonSerializer.Deserialize<string[]>(arrJson);
        }

        //This is not the same as filling.
        void RefreshOnlinePlugins()
        {
            var newItems = new List<SharedPluginItem>();
            foreach (var item in new SharedPluginCollection(AuthorizationCodes))
            {
                if (PluginDatabaseMain.LoadedPlugins.Where(a => a.Plugin.Name == item.Name).Count() <= 0
                    && item.RequiredVersion <= (System.Version)App.Version)
                {
                    SharedPluginItem spi = new SharedPluginItem(item);
                    spi.Title = item.Name;
                    spi.Version = item.Version;
                    newItems.Add(spi);
                }
            }

            foreach (var plugin in onlineItemsListView.Items.Cast<SharedPluginItem>())
            {
                var item = newItems.Where(x => x.Plugin.Name == plugin.Plugin.Name).FirstOrDefault();
                if (item != null)
                    item.ActionIndex = plugin.ActionIndex;
            }

            var selectedIndex = onlineItemsListView.SelectedIndex;
            onlineItemsListView.Items.Clear();

            foreach (var plugin in newItems)
                onlineItemsListView.Items.Add(plugin);

            if (selectedIndex > 0)
                onlineItemsListView.SelectedIndex = selectedIndex;
        }

        void SaveActions()
        {
            PluginDeploymentQueue queue = PluginDeployment.Queue;
            queue.Clear();
            onlinePluginActionDatas.Clear();
            installedPluginActionDatas.Clear();
            foreach (var plugin in onlineItemsListView.Items.Cast<SharedPluginItem>())
            {
                if (plugin.ActionIndex == 1)
                {
                    string packagePath = AppDomain.CurrentDomain.BaseDirectory + @"\bin\plugins\downloads\" + plugin.Plugin.Name + ".cndp";
                    if (System.IO.File.Exists(packagePath))
                        System.IO.File.Delete(packagePath);
                    plugin.Plugin.AuthorizationCodes = AuthorizationCodes.ToList();
                    plugin.Plugin.Download(packagePath);
                    queue.AddItem(PluginDeploymentType.Install, packagePath);
                }

                onlinePluginActionDatas.Add(new PluginActionData() { Name = plugin.Plugin.Name, Index = plugin.ActionIndex });
            }

            foreach (var plugin in installedItemsListView.Items.Cast<InstalledPluginItem>())
            {
                string packagePath = AppDomain.CurrentDomain.BaseDirectory + @"\bin\plugins\installed\" + plugin.Plugin.Name + ".cndp";
                if (plugin.ActionIndex == 1)
                    queue.AddItem(PluginDeploymentType.Uninstall, packagePath);
                else if (plugin.ActionIndex == 2)
                {
                    string downloadedPackagePath = AppDomain.CurrentDomain.BaseDirectory + @"\bin\plugins\downloads\" + plugin.Plugin.Name + ".cndp";
                    var sharedPlugin = new SharedPluginCollection(AuthorizationCodes).Where(p => p.Name == plugin.Plugin.Name).FirstOrDefault();
                    if (sharedPlugin != null)
                    {
                        if (System.IO.File.Exists(downloadedPackagePath))
                            System.IO.File.Delete(downloadedPackagePath);
                        sharedPlugin.Download(downloadedPackagePath);
                        queue.AddItem(PluginDeploymentType.Update, downloadedPackagePath);
                    }
                }

                installedPluginActionDatas.Add(new PluginActionData() { Name = plugin.Plugin.Name, Index = plugin.ActionIndex });
            }

            queue.SaveToFile();
        }

        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            RefreshOnlinePlugins();
        }

        string[] AuthorizationCodes = new string[] { };
        private void manageAuthorizationCodes_Click(object sender, RoutedEventArgs e)
        {
            ManagePluginAuthorizationCodesWindow mpacw = new ManagePluginAuthorizationCodesWindow();
            mpacw.AuthorizationCodes = new System.Collections.ObjectModel.ObservableCollection<string>(AuthorizationCodes);
            mpacw.ShowDialog();
            AuthorizationCodes = mpacw.AuthorizationCodes.ToArray();
            App.GlobalMeta["pluginAuthorizationCodes"] = JsonSerializer.Serialize(AuthorizationCodes);
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Please wait while the changes are processed. This may take a few minutes. " +
                "Changes will be complete after restarting Conway Nativity Directory.");
            var wf = _ConwayNativityDirectory.CreateWaitForm();
            wf.Title = "Conway Nativity Directory";
            wf.Description = "Please wait...";
            wf.Show();
            SaveActions();
            wf.Close();
            this.Close();
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    public class VersionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((System.Version)value).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new System.Version(value.ToString());
        }
    }

    internal class PluginActionData
    {
        public string Name { get; set; }
        public int Index { get; set; }
    }
}
