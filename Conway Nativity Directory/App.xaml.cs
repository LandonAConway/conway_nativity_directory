using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Reflection;
using System.IO;
using System.IO.Compression;
using System.Collections.Specialized;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Text;
using System.Text.Json;
using ConwayNativityDirectory.PluginApi;
using ConwayNativityDirectory.PluginApi.Deployment;

namespace Conway_Nativity_Directory
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {


        #region Resolve Dependencies
        public static void LoadAssemblyResolve()
        {
            //This must be done to prevent errors with loading assemblies.
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
        }

        private static Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string assemblyName = new AssemblyName(args.Name).Name;

            string basicPath = baseDir + @"\lib\" + assemblyName + @".dll";
            if (File.Exists(basicPath))
                return Assembly.LoadFile(basicPath);

            //search for assembly in a plugin's dependency folder
            string pluginsDir = baseDir + @"\bin\plugins\installed\";
            foreach (string pluginPath in Directory.GetDirectories(pluginsDir))
            {
                string dpPath = pluginPath + @"\dependencies\" + assemblyName + @".dll";
                if (File.Exists(dpPath))
                    return Assembly.LoadFile(dpPath);
            }

            return null;
        }

        #endregion

        public static void SetBinding(Binding binding, FrameworkElement frameworkElement, DependencyProperty dependencyProperty)
        {
            frameworkElement.SetBinding(dependencyProperty, binding);
        }

        public static void SetBinding(string path, object source, FrameworkElement frameworkElement, DependencyProperty dependencyProperty)
        {
            Binding binding = new Binding(path);
            binding.Source = source;
            frameworkElement.SetBinding(dependencyProperty, binding);
        }

        public static void SetBinding(string path, object source, FrameworkElement frameworkElement, DependencyProperty dependencyProperty,
            BindingMode bindingMode, UpdateSourceTrigger updateSourceTrigger)
        {
            Binding binding = new Binding(path);
            binding.Source = source;
            binding.Mode = bindingMode;
            binding.UpdateSourceTrigger = updateSourceTrigger;
            frameworkElement.SetBinding(dependencyProperty, binding);
        }

        public static void SetBinding(string path, object source, FrameworkElement frameworkElement, DependencyProperty dependencyProperty,
            IValueConverter valueConverter)
        {
            Binding binding = new Binding(path);
            binding.Source = source;
            binding.Converter = valueConverter;
            frameworkElement.SetBinding(dependencyProperty, binding);
        }

        public static void SetBinding(string path, object source, FrameworkElement frameworkElement, DependencyProperty dependencyProperty,
            BindingMode bindingMode, UpdateSourceTrigger updateSourceTrigger, IValueConverter valueConverter)
        {
            Binding binding = new Binding(path);
            binding.Source = source;
            binding.Mode = bindingMode;
            binding.UpdateSourceTrigger = updateSourceTrigger;
            binding.Converter = valueConverter;
            frameworkElement.SetBinding(dependencyProperty, binding);
        }


        #region Public Properties

        private static MainWindow _mainWindow;
        public static new MainWindow MainWindow { get { return _mainWindow; } }

        /// <summary>
        /// Sets the main MainWindow.
        /// </summary>
        /// <param name="mainWindow"></param>
        public static void SetMainWindow(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }


        private static ConwayNativityDirectoryProject _project;
        public static ConwayNativityDirectoryProject Project { get { return _project; } }

        /// <summary>
        /// Sets the main Project.
        /// </summary>
        /// <param name="project"></param>
        public static void SetProject(ConwayNativityDirectoryProject project)
        {
            _project = project;
        }


        private static Preferences _preferences;
        public static Preferences Preferences { get { return _preferences; } }

        /// <summary>
        /// Sets the main Preferences.
        /// </summary>
        /// <param name="preferences"></param>
        public static void SetPreferences(Preferences preferences)
        {
            _preferences = preferences;
        }

        public static MetaStorage GlobalMeta { get; internal set; }

        #endregion


        #region Types

        static Type[] types;
        public static Type[] Types { get { return types; } }


        public static void LoadTypes()
        {
            List<Type> allTypes = new List<Type>();
            List<Type> _types = new List<Type>();
            _types.AddRange(Assembly.GetExecutingAssembly().GetTypes());
            _types.AddRange(Assembly.GetExecutingAssembly().GetReferencedAssemblies()
                .SelectMany(r => Assembly.Load(r).GetTypes()));

            foreach (Type type in _types)
                if (!allTypes.Contains(type))
                    allTypes.Add(type);

            types = allTypes.ToArray();
        }


        #endregion


        #region Plugin Installation


        public StringDictionary InstallPlugin(string filePath)
        {
            StringDictionary results = new StringDictionary();
            results["path"] = filePath;

            string installationDir = AppDomain.CurrentDomain.BaseDirectory + @"\bin\plugins\";
            PluginPackage pckg = PluginPackage.Open(filePath);
            bool pluginInstalled = pckg.IsInstalled(installationDir);

            if (pluginInstalled)
            {
                MessageBoxResult mbr = MessageBox.Show("The plugin \"" + pckg.Title + "\" is already installed. " +
                    "Would you like to uninstall it or update it? [Yes] to uninstall, [No] to update, [Cancel] to do nothing.",
                    "Conway Nativity Directory", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

                if (mbr == MessageBoxResult.Yes)
                {
                    pckg.Uninstall(installationDir);
                    results["action"] = "uninstalled";
                }

                else if (mbr == MessageBoxResult.No)
                {
                    if (pckg.RequiredVersion <= (System.Version)App.Version)
                    {
                        pckg.Uninstall(installationDir);
                        pckg.Install(installationDir);
                    }

                    else
                        MessageBox.Show("The plugin could not be installed because Conway Nativity Directory needs to be updated.",
                            "Conway Nativity Directory", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            else
            {
                MessageBoxResult mbr = MessageBox.Show("The plugin \"" + pckg.Title + "\" is not installed. " +
                    "Would you like to install it?", "Conway Nativity Directory", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (mbr == MessageBoxResult.Yes)
                {
                    if (pckg.RequiredVersion <= (System.Version)App.Version)
                    {
                        pckg.Install(installationDir);
                        results["action"] = "installed";
                    }

                    else
                        MessageBox.Show("The plugin could not be installed because Conway Nativity Directory needs to be updated.",
                            "Conway Nativity Directory", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            return results;
        }

        static public void CopyFolder(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);
            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                string dest = Path.Combine(destFolder, name);
                File.Copy(file, dest);
            }
            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(destFolder, name);
                CopyFolder(folder, dest);
            }
        }

        private string extractPlugin(string filePath)
        {
            string cache = AppDomain.CurrentDomain.BaseDirectory + @"\bin\cache\";

            Random random = new Random();
            string finalDest = cache + @"\plugin";

            do { finalDest = cache + @"\plugin" + random.Next(8); }
            while (Directory.Exists(finalDest));

            ZipFile.ExtractToDirectory(filePath, finalDest);

            return finalDest;
        }

        #endregion


        #region Updates


        internal static void CheckForUpdates()
        {
            if (Internet.HasConnection(@"https://www.cnd-app.conwaynativities.com/"))
            {
                UpdateInfo update = GetLatestUpdateInfo();
                if (update.Version > App.Version)
                {
                    MessageBoxResult mbr = MessageBox.Show("A newer version of Conway Nativity Directory is available. (v" + update.Version.ToString() + ") " +
                        "Would you like to download and install it now? (Recomended)", "Conway Nativity Directory",
                        MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (mbr == MessageBoxResult.Yes)
                    {
                        string folder = AppDomain.CurrentDomain.BaseDirectory +
                            @"\bin\updates\downloads";

                        WaitForm waitForm = new WaitForm("Downloading Update...");
                        waitForm.Show();

                        if (Directory.Exists(folder))
                            Directory.Delete(folder, true);
                        Directory.CreateDirectory(folder);

                        update.DownloadUpdate(folder);
                        waitForm.Close();

                        MessageBox.Show("The update has been downloaded and installation will now begin." +
                            " Conway Nativity Directory will now close the installation setup will appear. Follow the instructions" +
                            " of the installation setup to install the latest version of Conway Nativity Directory.");

                        if (File.Exists(folder + @"Setup.msi"))
                            System.Diagnostics.Process.Start(folder + @"\Setup.msi");
                        else if (File.Exists(folder + @"Installer.exe"))
                            System.Diagnostics.Process.Start(folder + @"\Installer.exe");

                        Application.Current.Shutdown(1);
                    }
                }
            }
        }

        internal static UpdateInfo GetLatestUpdateInfo()
        {
            var localFolder = AppDomain.CurrentDomain.BaseDirectory +
                @"\bin\updates";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.cnd-app.conwaynativities.com/versions/download-latest-version-info");
            request.Timeout = Convert.ToInt32(TimeSpan.FromSeconds(8).TotalMilliseconds);
            var res = request.GetResponse();
            var _stream = res.GetResponseStream();
            var _readStream = new StreamReader(_stream, Encoding.UTF8);
            string jsonText = _readStream.ReadToEnd();

            var json = JsonDocument.Parse(jsonText);
            return new UpdateInfo(
                    Version.Parse(json.RootElement.GetProperty("Version").ToString()),
                    DateTime.Parse(json.RootElement.GetProperty("ReleaseDate").ToString())
            );
        }

        internal static void DownloadFromGithub(string path, string url)
        {
            var githubToken = "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";

            using (var client = new System.Net.Http.HttpClient())
            {
                var credentials = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}:", githubToken);
                credentials = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(credentials));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
                var contents = client.GetByteArrayAsync(url).Result;
                System.IO.File.WriteAllBytes(path, contents);
            }
        }

        public static void DownloadLatestUpdateZip(string locaZip)
        {
            using (WebClient webClient = new WebClient())
                webClient.DownloadFile("https://www.cnd-app.conwaynativities.com/versions/download-latest-package", locaZip);
        }

        #endregion
    }

    public struct UpdateInfo
    {
        Version version;
        public Version Version { get { return version; } }

        DateTime uploaded;
        public DateTime Uploaded { get { return uploaded; } }

        public UpdateInfo(Version version, DateTime uploaded)
        {
            this.version = version;
            this.uploaded = uploaded;
        }

        public void DownloadUpdate(string localFolder)
        {
            //download
            string localZip = localFolder + @"\update.zip";
            App.DownloadLatestUpdateZip(localZip);

            //extract
            string extractFolder = localFolder + @"\update";
            Directory.CreateDirectory(extractFolder);
            ZipFile.ExtractToDirectory(localZip, extractFolder);

            //get setups
            string setupsFolder = extractFolder + @"";
            foreach (string path in Directory.GetFiles(setupsFolder))
                File.Copy(path, localFolder + @"\" + Path.GetFileName(path));

            //Delete cache
            File.Delete(localZip);
            Directory.Delete(extractFolder, true);
        }
    }

    public static class Internet
    {
        public static bool HasConnection(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = Convert.ToInt32(TimeSpan.FromSeconds(2).TotalMilliseconds);
            request.Method = "GET";
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    return response.StatusCode == HttpStatusCode.OK;
            }

            catch
            {
                return false;
            }
        }
    }
}
