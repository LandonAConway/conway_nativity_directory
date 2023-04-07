using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Reflection;
using ConwayNativityDirectory.PluginApi;
using ConwayNativityDirectory.PluginApi.Primitives;
using System.Collections;
using System.Text.Json.Nodes;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Conway_Nativity_Directory
{
    public class PluginDatabaseMain
    {

        #region Plugin Feature Implementations

        private static int ProjectMenuCount = 0;
        private static int EditMenuCount = 0;
        private static int NativitiesMenuCount = 0;
        private static int SearchMenuCount = 0;
        private static int SearchToolsMenuCount = 0;

        public static void RegisterFeature(IPluginFeature plugin)
        {
            if (!PluginDatabaseMain.PluginsLoaded)
            {
                if (plugin is ICommandPluginFeature a)
                    PluginDatabaseMain.RegisterFeature(a);

                else if (plugin is ImportMenuPlugin b)
                    PluginDatabaseMain.RegisterFeature(b);

                else if (plugin is ExportMenuPlugin c)
                    PluginDatabaseMain.RegisterFeature(c);

                else if (plugin is ProjectMenuPlugin d)
                    PluginDatabaseMain.RegisterFeature(d);

                else if (plugin is EditMenuPlugin e)
                    PluginDatabaseMain.RegisterFeature(e);

                else if (plugin is NativitiesMenuPlugin f)
                    PluginDatabaseMain.RegisterFeature(f);

                else if (plugin is SearchMenuPlugin g)
                    PluginDatabaseMain.RegisterFeature(g);

                else if (plugin is SearchToolsMenuPlugin h)
                    PluginDatabaseMain.RegisterFeature(h);

                else if (plugin is ViewPaneItemPlugin i)
                    PluginDatabaseMain.RegisterFeature(i);

                else if (plugin is InfoPaneItemPlugin j)
                    PluginDatabaseMain.RegisterFeature(j);

                else if (plugin is WorkspacePlugin k)
                    PluginDatabaseMain.RegisterFeature(k);
            }

            else
                throw new InvalidOperationException("Plugin features can only be registered when loading.");
        }

        internal static void RegisterFeature(ICommandPluginFeature plugin)
        {
            Commands.RegisteredCommands.Register(plugin);
        }

        internal static void RegisterFeature(ImportMenuPlugin plugin)
        {
            MainWindow mw = App.MainWindow;

            MenuItem menuItem = new MenuItem();
            plugin.OnLoad(menuItem);

            mw.importMenuItem.Items.Add(menuItem);
        }

        internal static void RegisterFeature(ExportMenuPlugin plugin)
        {
            MainWindow mw = App.MainWindow;

            MenuItem menuItem = new MenuItem();
            plugin.OnLoad(menuItem);

            mw.exportMenuItem.Items.Add(menuItem);
        }

        internal static void RegisterFeature(ProjectMenuPlugin plugin)
        {
            MainWindow mw = App.MainWindow;
            if (ProjectMenuCount < 1)
                mw.projectMenuItem.Items.Add(new Separator());

            MenuItem menuItem = new MenuItem();
            plugin.OnLoad(menuItem);

            mw.projectMenuItem.Items.Add(menuItem);

            ProjectMenuCount++;
        }

        internal static void RegisterFeature(EditMenuPlugin plugin)
        {
            MainWindow mw = App.MainWindow;
            if (EditMenuCount < 1)
                mw.editMenuItem.Items.Add(new Separator());

            MenuItem menuItem = new MenuItem();
            plugin.OnLoad(menuItem);

            mw.editMenuItem.Items.Add(menuItem);

            EditMenuCount++;
        }

        internal static void RegisterFeature(NativitiesMenuPlugin plugin)
        {
            MainWindow mw = App.MainWindow;
            if (NativitiesMenuCount < 1)
                mw.nativitiesMenuItem.Items.Add(new Separator());

            MenuItem menuItem = new MenuItem();
            plugin.OnLoad(menuItem);

            mw.nativitiesMenuItem.Items.Add(menuItem);

            NativitiesMenuCount++;
        }

        internal static void RegisterFeature(SearchMenuPlugin plugin)
        {
            MainWindow mw = App.MainWindow;
            if (SearchMenuCount < 1)
                mw.searchMenuItem.Items.Add(new Separator());

            MenuItem menuItem = new MenuItem();
            plugin.OnLoad(menuItem);

            mw.searchMenuItem.Items.Add(menuItem);

            SearchMenuCount++;
        }

        internal static void RegisterFeature(SearchToolsMenuPlugin plugin)
        {
            MainWindow mw = App.MainWindow;
            if (SearchToolsMenuCount < 1)
                mw.searchToolsMenuItem.Items.Add(new Separator());

            MenuItem menuItem = new MenuItem();
            plugin.OnLoad(menuItem);

            mw.searchToolsMenuItem.Items.Add(menuItem);

            SearchToolsMenuCount++;
        }

        internal static void RegisterFeature(ViewPaneItemPlugin plugin)
        {
            PaneItem paneItem = plugin.CreatePaneItem();
            plugin.OnLoad(paneItem);

            var workspaceKey = plugin.WorkspaceKey;
            var workspace = WorkspacesMain.Workspaces[workspaceKey];
            if (workspace == null)
                WorkspacesMain.Workspaces["main"].viewPane.Items.Add(paneItem);
            else
                workspace.viewPane.Items.Add(paneItem);
        }

        internal static void RegisterFeature(InfoPaneItemPlugin plugin)
        {
            PaneItem paneItem = plugin.CreatePaneItem();
            plugin.OnLoad(paneItem);

            var workspaceKey = plugin.WorkspaceKey;
            var workspace = WorkspacesMain.Workspaces[workspaceKey];
            if (workspace == null)
                WorkspacesMain.Workspaces["main"].infoPane.Items.Add(paneItem);
            else
                workspace.infoPane.Items.Add(paneItem);
        }

        internal static void RegisterFeature(WorkspacePlugin plugin)
        {
            Workspace workspace = Workspace.FromWorkspacePlugin(plugin);
            WorkspacesMain.Workspaces.Add(workspace);
        }

        #endregion


        #region Plugin Loading

        static bool pluginsLoaded = false;
        public static bool PluginsLoaded { get { return pluginsLoaded; } }
        public static ConwayNativityDirectoryWrapper CNDWrapper;
        public static void LoadPlugins()
        {
            var wrapper = new ConwayNativityDirectoryWrapper();
            CNDWrapper = wrapper;
            PluginDatabase.SetConwayNativityDirectory(wrapper);
            LoadPluginsIntoSystem();
            pluginsLoaded = true;
        }

        private static List<Assembly> assemblies = new List<Assembly>();
        public static IEnumerable<Assembly> Assemblies { get { return assemblies; } }

        private static void LoadPluginsIntoSystem()
        {
            string[] pluginFolders = Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory + @"\bin\plugins\installed");
            List<Assembly> loadedAssemblies = new List<Assembly>();
            List<string> allPluginDlls = new List<string>();

            foreach (string pluginFolder in pluginFolders)
            {
                var pluginDll = pluginFolder + @"\" + Path.GetFileName(pluginFolder) + @".dll";
                try
                {
                    Assembly pluginAssembly = Assembly.LoadFile(pluginDll);
                    loadedAssemblies.Add(pluginAssembly);
                    allPluginDlls.Add(pluginDll);
                    
                    //foreach (var refAssembly in pluginAssembly.GetReferencedAssemblies())
                    //{
                    //    Assembly.LoadFile(Path.Combine(pluginFolder, refAssembly.Name + ".dll"));
                    //}
                }

                catch (Exception ex)
                {
                    App.Log("Error", "LoadPluginAssembly", "A plugin could not be loaded. path=\"" + pluginDll +
                        "\" ExceptionType: \"" + ex.GetType().ToString() + "\" Exception: \"" + ex.Message + "\" \"");
                }
            }

            //First load the information about the plugins
            foreach (Assembly assembly in loadedAssemblies)
            {
                try
                {
                    Type[] types = assembly.GetTypes();
                    string mainTypeNameOption1 = types.Where(x => x.Namespace == assembly.GetName().Name &&
                        x.Name == "Main").FirstOrDefault()?.Namespace + ".Main";
                    string mainTypeNameOption2 = types.Where(x => x.Name == "Main").FirstOrDefault()?.Namespace + ".Main";

                    PluginBase pluginOption1 = assembly.CreateInstance(mainTypeNameOption1) as PluginBase;
                    PluginBase pluginOption2 = assembly.CreateInstance(mainTypeNameOption2) as PluginBase;
                    PluginBase plugin = null;

                    if (pluginOption1 != null)
                        plugin = pluginOption1;
                    else
                        plugin = pluginOption2;

                    if (plugin != null)
                    {
                        assemblies.Add(assembly);
                        LoadPluginInfo(plugin, allPluginDlls[assemblies.IndexOf(assembly)]);
                        App.Log("Event", "LoadPlugin", "A plugin was loaded. path=\"" + allPluginDlls[assemblies.IndexOf(assembly)] + "\"");
                    }

                    else
                        throw new Exception("The plugin did not load becuase the following type names did not exist: \n" +
                            mainTypeNameOption1 + "\n" +
                            mainTypeNameOption2);
                }

                catch (Exception ex)
                {
                    App.Log("Error", "LoadPlugin", "A plugin could not be loaded. path=\"" + allPluginDlls[assemblies.IndexOf(assembly)] +
                        "\" ExceptionType: \"" + ex.GetType().ToString() + "\" Exception: \"" + ex.Message + "\" \"");
                }
            }

            //Then load the actual plugin itself. (initialize it). This is where the plugins actually get
            //loaded into the software so they are available to the user.
            foreach (var pluginInfo in EnabledPlugins)
            {
                string pluginStoragePath = AppDomain.CurrentDomain.BaseDirectory + @"\bin\plugins\data\" +
                        Path.GetFileName(pluginInfo.InstallationPath) + @"\storage.meta";
                MetaStorageFileSystem metaStorageFileSystem = new MetaStorageFileSystem(new MetaStorage());
                if (Directory.Exists(Path.GetDirectoryName(pluginStoragePath)) && File.Exists(pluginStoragePath))
                    metaStorageFileSystem.LoadFromFile(pluginStoragePath);

                pluginInfo.Plugin.Load(pluginInfo.InstallationPath, metaStorageFileSystem.MetaStorage);

                foreach (IPluginFeature pluginFeature in pluginInfo.Plugin.GetPluginFeatures())
                    RegisterFeature(pluginFeature);
            }

            //After doing everything, notify all the plugins that they have been loaded into the UI
            foreach (var pluginInfo in EnabledPlugins)
                pluginInfo.Plugin.NotifyAllPluginsLoaded();
        }

        private static List<PluginInfo> loadedPlugins = new List<PluginInfo>();
        public static IEnumerable<PluginInfo> LoadedPlugins { get { return loadedPlugins.ToArray(); } }
        public static IEnumerable<PluginInfo> EnabledPlugins { get { return loadedPlugins.Where(p => p.WasEnabledOnStartup).ToArray(); } }

        private static void LoadPluginInfo(PluginBase plugin, string pluginFileName)
        {
            foreach (string dll in plugin.GetDependencies())
            {
                string fileName = Path.GetFileName(dll);
                string path = AppDomain.CurrentDomain.BaseDirectory + @"\bin\lib\" + fileName;

                if (File.Exists(path))
                    Assembly.LoadFile(path);
                else
                    App.Log("Error", "LoadPlugin", "Plugin dependency was not found. plugin=\"" + pluginFileName + "\", " +
                        "dependency=\"" + fileName);
            }

            var pluginInfo = new PluginInfo(Path.GetDirectoryName(pluginFileName), plugin);
            pluginInfo.LoadPluginSettings();
            loadedPlugins.Add(pluginInfo);
        }

        public static void SavePluginStorage()
        {
            foreach (PluginInfo pluginInfo in EnabledPlugins)
            {
                string destDir = AppDomain.CurrentDomain.BaseDirectory + @"\bin\plugins\data\" + 
                    Path.GetFileName(pluginInfo.InstallationPath);
                string destPath = destDir + @"\storage.meta";

                if (!Directory.Exists(destDir))
                    Directory.CreateDirectory(destDir);

                MetaStorageFileSystem metaStorageFileSystem = new MetaStorageFileSystem(pluginInfo.Plugin.PluginStorage);
                metaStorageFileSystem.SaveToFile(destPath);
            }
        }

        #endregion
    }

    public class PluginInfo
    {
        public PluginInfo(string installationPath, PluginBase plugin)
        {
            this.installationPath = installationPath;
            this.plugin = plugin;
        }

        string installationPath;
        public string InstallationPath { get { return installationPath; } }

        bool wasEnabledOnStartup;
        public bool WasEnabledOnStartup { get { return wasEnabledOnStartup; } }

        PluginBase plugin;
        public PluginBase Plugin { get { return plugin; } }

        PluginSettings pluginSettings;
        public PluginSettings PluginSettings { get { return pluginSettings; } }

        public void LoadPluginSettings()
        {
            pluginSettings = new PluginSettings();
            var path = GetDataPath() + @"\settings.json";
            if (File.Exists(path))
            {
                var json = JsonDocument.Parse(File.ReadAllText(path)).RootElement;
                pluginSettings.IsEnabled = Convert.ToBoolean(json.GetProperty("IsEnabled").ToString());
                wasEnabledOnStartup = pluginSettings.IsEnabled;
            }
        }

        public void SavePluginSettings()
        {
            var path = GetDataPath() + @"\settings.json";
            File.WriteAllText(path, JsonSerializer.Serialize(pluginSettings));
        }

        public string GetDataPath()
        {
            var result = AppDomain.CurrentDomain.BaseDirectory + @"\bin\plugins\data\" +
                    Path.GetFileName(this.InstallationPath);

            if (!Directory.Exists(result))
                Directory.CreateDirectory(result);
            return result;
        }
    }

    public class PluginSettings
    {
        public bool IsEnabled { get; set; } = true;
    }

    public class ConwayNativityDirectoryWrapper : IConwayNativityDirectory
    {
        public System.Version Version { get { return App.Version.ToSystem(); } }
        public string[] EnabledPlugins { get { return PluginDatabaseMain.EnabledPlugins.Select(a => a.Plugin.Name).ToArray(); } }

        public MetaStorage AppStorage => App.GlobalMeta;

        MetaTagCollection metaTags = new MetaTagCollection();
        public MetaTagCollection MetaTags { get { return metaTags; } }

        public NativityBase CreateNativity()
        {
            return new Nativity();
        }

        public IConwayNativityDirectoryProject GetProject()
        {
            return App.Project;
        }

        public string GetCurrentWorkspaceKey()
        {
            return WorkspacesMain.CurrentWorkspaceKey;
        }

        public event WorkspaceKeyChangedEventHandler SelectedWorkspaceKeyChanged;

        public string SelectedWorkspaceKey
        {
            get
            {
                return WorkspacesMain.CurrentWorkspaceKey;
            }
        }

        public string SelectedViewPaneKey
        {
            get
            {
                var currentWorkspace = WorkspacesMain.Workspaces[WorkspacesMain.CurrentWorkspaceKey];
                if (currentWorkspace != null)
                {
                    var paneItem = currentWorkspace.viewPane.SelectedContent as PaneItem;
                    if (paneItem != null)
                        return paneItem.Key;
                }

                return null;
            }
        }

        public string SelectedInfoPaneKey
        {
            get
            {
                var currentWorkspace = WorkspacesMain.Workspaces[WorkspacesMain.CurrentWorkspaceKey];
                if (currentWorkspace != null)
                {
                    var paneItem = currentWorkspace.infoPane.SelectedContent as PaneItem;
                    if (paneItem != null)
                        return paneItem.Key;
                }
                return null;
            }
        }

        public void Log(Exception exception)
        {
            App.Log(exception);
        }

        public void Log(string level, string description)
        {
            App.Log(level, description);
        }

        public void Log(string level, string name, string description)
        {
            App.Log(level, name, description);
        }

        public void CommandConsoleLog(object value)
        {
            CommandConsoleWindow.Log(value);
        }

        public void CommandConsoleClear()
        {
            CommandConsoleWindow.Clear();
        }

        public string GetInput(string caption, string defaultResponse)
        {
            InputDialog inputDialog = new InputDialog();
            inputDialog.Title = caption;
            inputDialog.Response = defaultResponse;
            if (inputDialog.ShowDialog() == true)
                return inputDialog.Response;
            return null;
        }

        public WaitFormRef CreateWaitForm()
        {
            return new WaitFormWrapper();
        }

        public ProgressBarModalRef CreateProgressBarModal()
        {
            return new ProgressBarModal();
        }

        #region internal stuff

        internal void InvokeSelectedWorkspaceKeyChangedEvent(WorkspaceKeyChangedEventArgs args)
        {
            if (SelectedWorkspaceKeyChanged != null)
                SelectedWorkspaceKeyChanged.Invoke(args);
        }

        #endregion
    }

    public class WaitFormWrapper : WaitFormRef
    {
        WaitForm waitForm;
        public WaitFormWrapper()
        {
            this.waitForm = new WaitForm("");
        }

        public override string Title
        {
            get { return this.waitForm.Text; }
            set { this.waitForm.Text = value; }
        }

        public override string Description
        {
            get { return this.waitForm.LabelText; }
            set { this.waitForm.LabelText = value; }
        }

        public override void Show()
        {
            this.waitForm.Show();
            waitForm.Invalidate();
            waitForm.Update();
        }

        public override void Close()
        {
            this.waitForm.Close();
        }
    }
}