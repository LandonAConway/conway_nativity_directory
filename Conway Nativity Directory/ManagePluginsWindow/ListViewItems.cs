using ConwayNativityDirectory.PluginApi.Deployment;
using ConwayNativityDirectory.PluginApi.PluginDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Conway_Nativity_Directory
{
    public class InstalledPluginItem : PluginListViewItem
    {
        PluginPackage plugin;
        public PluginPackage Plugin { get { return plugin; } }
        public InstalledPluginItem(PluginPackage plugin)
        {
            this.plugin = plugin;
            SetProperties();
        }

        public InstalledPluginItem(string pluginName)
        {
            this.plugin = PluginPackage.Open(AppDomain.CurrentDomain.BaseDirectory + @"\bin\plugins\installed\" + pluginName + ".cndp");
            SetProperties();
        }

        void SetProperties()
        {
            PName = plugin.Name;
            Title = plugin.Title;
            Description = plugin.Description;
            Version = plugin.Version;
            RequiredVersion = plugin.RequiredVersion;
            Author = plugin.Author;
            Website = plugin.Website;
        }

        static void PluginIsEnabledChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PluginInfo plugin = PluginDatabaseMain.LoadedPlugins.Where(p => p.Plugin.Name == ((InstalledPluginItem)sender).Plugin.Name).FirstOrDefault();
            if (plugin != null)
            {
                plugin.PluginSettings.IsEnabled = (bool)e.NewValue;
                plugin.SavePluginSettings();
            }
        }

        public static DependencyProperty PluginIsEnabledProperty = DependencyProperty.Register(
            nameof(PluginIsEnabled), typeof(bool), typeof(InstalledPluginItem),
            new PropertyMetadata(true, PluginIsEnabledChanged));

        public bool PluginIsEnabled
        {
            get => (bool)GetValue(PluginIsEnabledProperty);
            set => SetValue(PluginIsEnabledProperty, value);
        }

        public static DependencyProperty ActionIndexProperty = DependencyProperty.Register(
            nameof(ActionIndex), typeof(int), typeof(InstalledPluginItem),
            new PropertyMetadata(0));

        public int ActionIndex
        {
            get => (int)GetValue(ActionIndexProperty);
            set => SetValue(ActionIndexProperty, value);
        }
    }

    public class SharedPluginItem : PluginListViewItem
    {
        SharedPluginPackage plugin;
        public SharedPluginPackage Plugin { get { return plugin; } }
        public SharedPluginItem(SharedPluginPackage plugin)
        {
            this.plugin = plugin;
            SetProperties();
        }

        void SetProperties()
        {
            PName = plugin.Name;
            Title = plugin.Title;
            Description = plugin.Description;
            Version = plugin.Version;
            RequiredVersion = plugin.RequiredVersion;
            Author = plugin.Author;
            Website = plugin.Website;
        }

        public static DependencyProperty ActionIndexProperty = DependencyProperty.Register(
            nameof(ActionIndex), typeof(int), typeof(SharedPluginItem),
            new PropertyMetadata(0));

        public int ActionIndex
        {
            get => (int)GetValue(ActionIndexProperty);
            set => SetValue(ActionIndexProperty, value);
        }
    }

    public abstract class PluginListViewItem : ListViewItem
    {

        public static DependencyProperty PNameProperty = DependencyProperty.Register(
            nameof(PName), typeof(string), typeof(InstalledPluginItem));

        public string PName
        {
            get => (string)GetValue(PNameProperty);
            set => SetValue(PNameProperty, value);
        }

        public static DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title), typeof(string), typeof(InstalledPluginItem));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static DependencyProperty DescriptionProperty = DependencyProperty.Register(
            nameof(Description), typeof(string), typeof(InstalledPluginItem));

        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public static DependencyProperty VersionProperty = DependencyProperty.Register(
            nameof(Version), typeof(System.Version), typeof(InstalledPluginItem));

        public System.Version Version
        {
            get => (System.Version)GetValue(VersionProperty);
            set => SetValue(VersionProperty, value);
        }

        public static DependencyProperty RequiredVersionProperty = DependencyProperty.Register(
            nameof(RequiredVersion), typeof(System.Version), typeof(InstalledPluginItem));

        public System.Version RequiredVersion
        {
            get => (System.Version)GetValue(RequiredVersionProperty);
            set => SetValue(RequiredVersionProperty, value);
        }

        public static DependencyProperty AuthorProperty = DependencyProperty.Register(
            nameof(Author), typeof(string), typeof(InstalledPluginItem));

        public string Author
        {
            get => (string)GetValue(AuthorProperty);
            set => SetValue(AuthorProperty, value);
        }

        public static DependencyProperty WebsiteProperty = DependencyProperty.Register(
            nameof(Website), typeof(string), typeof(InstalledPluginItem));

        public string Website
        {
            get => (string)GetValue(WebsiteProperty);
            set => SetValue(WebsiteProperty, value);
        }
    }
}
