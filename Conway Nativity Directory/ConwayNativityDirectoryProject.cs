using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using ConwayNativityDirectory.PluginApi;
using ConwayNativityDirectory.PluginApi.Primitives;

namespace Conway_Nativity_Directory
{
    public class ConwayNativityDirectoryProject : IConwayNativityDirectoryProject
    {


        #region Constructor

        private MainWindow mainWindow;
        public MainWindow MainWindow { get { return mainWindow; } }
        public ConwayNativityDirectoryProject(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            this.Close();
            SetBindings();
            SetFilter();

            //keeping track of the the selection changed
            mainWindow.nativityListView.SelectionChanged += NativityListView_SelectionChanged;
        }

        #endregion


        #region Public Properties

        /// <summary>
        /// Returns true if the project is open.
        /// </summary>
        public bool IsOpen { get { return isOpen; } }

        /// <summary>
        /// Gets the file name of the project.
        /// </summary>
        public string FileName { get { return fileName; } }

        /// <summary>
        /// Gets the unique identifier of the project.
        /// </summary>
        public string UID { get { return uid; } }

        /// <summary>
        /// Gets the settings of the project.
        /// </summary>
        public ProjectSettings ProjectSettings { get { return projectSettings; } }

        /// <summary>
        /// Gets the collection of nativities.
        /// </summary>
        public ObservableCollection<object> Nativities { get { return nativities; } }

        /// <summary>
        /// Gets the collection of nativities in the current search results.
        /// </summary>
        public List<object> SearchResults
        {
            get { return (searchResults ?? new CollectionView(new List<object>())).Cast<object>().ToList(); }
        }

        public IEnumerable<NativityBase> SelectedNativities
        {
            get
            {
                return nativities.Cast<NativityBase>().Where(a => a.IsSelected);
            }
        }

        public NativityBase SelectedNativity
        {
            get { return mainWindow.nativityListView.SelectedItem as NativityBase; }
        }

        public int SelectedNativityIndex
        {
            get { return mainWindow.nativityListView.SelectedIndex; }
        }

        public ActionStack Actions { get { return actions; } }

        #endregion


        #region Public Events

        public event EventHandler PreviewNewProject;
        public event EventHandler PreviewOpenProject;
        public event EventHandler PreviewSaveProject;
        public event EventHandler PreviewCloseProject;
        public event EventHandler OnNewProject;
        public event EventHandler OnOpenProject;
        public event EventHandler OnSaveProject;
        public event EventHandler OnCloseProject;
        public event EventHandler SelectedNativitiesChanged;

        #endregion


        #region Other Private Properties


        #region General

        /// <summary>
        /// Returns true if the project has been saved on the disk.
        /// </summary>
        private bool HasFileName
        {
            get
            {
                if (!System.String.IsNullOrEmpty(FileName) && !System.String.IsNullOrWhiteSpace(FileName))
                {
                    return true;
                }

                return false;
            }
        }

        MetaStorage meta = new MetaStorage();
        MetaStorage localMeta = new MetaStorage();

        internal bool isWorking = false;

        #endregion


        #region For public get-only properties

        private bool isOpen;
        private string fileName;
        private string uid;
        private ProjectSettings projectSettings;
        private ObservableCollection<object> nativities = new ObservableCollection<object>();
        private CollectionView searchResults;
        private ActionStack actions = new ActionStack();

        #endregion


        #endregion


        #region Project


        #region ProgressBarModal

        private ProgressBarModal _ProgressBarModal = new ProgressBarModal();
        private string _ProgressBarModalInfoText = "";

        private void _ProgressBarModal_Update()
        {
            _ProgressBarModal_UpdateInfo();
            _ProgressBarModal.Increment();
        }

        private void _ProgressBarModal_UpdateInfo()
        {
            double p = Math.Round((_ProgressBarModal.Value / _ProgressBarModal.Max) * 100, 2);
            _ProgressBarModal.Info = "Progress: " + p.ToString() + "% - " + _ProgressBarModalInfoText;
        }

        #endregion


        /// <summary>
        /// Creates a new project.
        /// </summary>
        public void New()
        {
            isWorking = true;

            //Event
            EventHandler previewHandler = PreviewNewProject;
            if (previewHandler != null)
                previewHandler(this, new EventArgs());

            //Close project
            Close();
            isWorking = true;

            //UI changes - menu items
            mainWindow.closeMenuItem.IsEnabled = true;
            mainWindow.saveMenuItem.IsEnabled = false;
            mainWindow.saveAsMenuItem.IsEnabled = true;
            mainWindow.saveACopyMenuItem.IsEnabled = true;
            mainWindow.importMenuItem.IsEnabled = true;
            mainWindow.exportMenuItem.IsEnabled = true;
            mainWindow.projectMenuItem.IsEnabled = true;
            mainWindow.undoMenuItem.IsEnabled = true;
            mainWindow.redoMenuItem.IsEnabled = true;
            mainWindow.assignImagesByIdMenuItem.IsEnabled = true;
            mainWindow.removeImagesMenuItem.IsEnabled = true;
            mainWindow.imageMenuItem.IsEnabled = true;
            mainWindow.taggingMenuItem.IsEnabled = true;
            mainWindow.nativitiesMenuItem.IsEnabled = true;
            mainWindow.searchMenuItem.IsEnabled = true;
            mainWindow.viewMenuItem.IsEnabled = true;
            mainWindow.ConfigureColumns();

            //UI changes - grid
            mainWindow.projectGrid.Visibility = System.Windows.Visibility.Visible;

            //Open project
            this.isOpen = true;

            //Other important stuff
            uid = GenerateUID();
            this.projectSettings = ProjectSettings.New();
            foreach (IProjectSettings settings in projectSettings)
                settings.OnNewProject();

            //Start auto-save
            AutoSaving.StopTimer();
            AutoSaving.StartTimer();

            //Log
            App.Log("Action", "NewProject", "A new project was started.");

            //Event
            EventHandler handler = OnNewProject;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }

            isWorking = false;
        }

        /// <summary>
        /// Opens a project from a file.
        /// </summary>
        /// <param name="fileName">The file to open.</param>
        /// <param name="projectFileName">Sets <see cref="ConwayNativityDirectoryProject.FileName"/></param>
        public void Open(string fileName, string projectFileName)
        {
            isWorking = true;

            _ProgressBarModalInfoText = "Please wait...";
            _ProgressBarModal_UpdateInfo();

            //Event
            EventHandler previewHandler = PreviewOpenProject;
            if (previewHandler != null)
                previewHandler(this, new EventArgs());

            //Close project
            this.Close();
            isWorking = true;

            //Save file name
            this.fileName = projectFileName;

            //Extract contents
            string temporarySaveDest = CreateProjectDirectory();
            ZipFile.ExtractToDirectory(fileName, temporarySaveDest);

            //Modal stuff
            string[] filePaths = Directory.GetFiles(temporarySaveDest, "*.nty", SearchOption.TopDirectoryOnly);
            _ProgressBarModal.Max = filePaths.Length + 4;

            //Load contents
            foreach (string filePath in filePaths)
            {
                var nativity = Nativity.Load(filePath);
                Nativities.Add(nativity);
                _ProgressBarModalInfoText = "Loading Nativity #" + nativity.Id;
                _ProgressBarModal_Update();
            }
            _ProgressBarModalInfoText = "Please wait...";
            _ProgressBarModal_UpdateInfo();

            //Data file
            mainWindow.ExpandColumns(double.NaN);
            string dataFileDest = temporarySaveDest + @"\data.dat";
            if (File.Exists(dataFileDest))
            {
                string[] dataFileLines = File.ReadAllLines(dataFileDest);
                foreach (string line in dataFileLines)
                {
                    if (line.StartsWith("uid="))
                        this.uid = line.Substring(4);
                }
            }

            else
            {
                //Defaults
                if (String.IsNullOrEmpty(this.uid))
                    this.uid = GenerateUID();
            }

            //Load meta
            meta.Clear();
            var metaPath = temporarySaveDest + @"\project.meta";
            var fs = new MetaStorageFileSystem(meta);
            if (File.Exists(metaPath))
                fs.LoadFromFile(metaPath);
            _ProgressBarModal_Update();

            //Load local meta
            var localMetaPath = AppDomain.CurrentDomain.BaseDirectory + @"\bin\storage\project\" + UID + @"\project-local.meta";
            if (File.Exists(localMetaPath))
            {
                var lmfs = new MetaStorageFileSystem(localMeta);
                if (File.Exists(localMetaPath))
                    lmfs.LoadFromFile(localMetaPath);
            }

            //Other important stuff
            string projectSettingsPath = temporarySaveDest + @"\projset.dat";
            if (File.Exists(projectSettingsPath))
                this.projectSettings = ProjectSettings.Load(projectSettingsPath);
            else
                this.projectSettings = ProjectSettings.New();

            foreach (IProjectSettings settings in projectSettings)
                settings.OnOpenProject();

            Directory.Delete(temporarySaveDest, true);
            _ProgressBarModal_Update();

            //UI changes - menu items
            mainWindow.closeMenuItem.IsEnabled = true;
            mainWindow.saveMenuItem.IsEnabled = true;
            mainWindow.saveAsMenuItem.IsEnabled = true;
            mainWindow.saveACopyMenuItem.IsEnabled = true;
            mainWindow.importMenuItem.IsEnabled = true;
            mainWindow.exportMenuItem.IsEnabled = true;
            mainWindow.projectMenuItem.IsEnabled = true;
            mainWindow.undoMenuItem.IsEnabled = true;
            mainWindow.redoMenuItem.IsEnabled = true;
            mainWindow.assignImagesByIdMenuItem.IsEnabled = true;
            mainWindow.removeImagesMenuItem.IsEnabled = true;
            mainWindow.imageMenuItem.IsEnabled = true;
            mainWindow.taggingMenuItem.IsEnabled = true;
            mainWindow.nativitiesMenuItem.IsEnabled = true;
            mainWindow.searchMenuItem.IsEnabled = true;
            mainWindow.viewMenuItem.IsEnabled = true;

            var gps = (GeneralProjectSettings)ProjectSettings["General"];
            mainWindow.Title = String.IsNullOrWhiteSpace(gps.ProjectName) ?
                "Conway Nativity Directory" :
                "Conway Nativity Directory - " + gps.ProjectName;
            mainWindow.ConfigureColumns();

            //UI changes - grid
            mainWindow.projectGrid.Visibility = System.Windows.Visibility.Visible;

            //Open project
            this.isOpen = true;

            //Start auto-save
            AutoSaving.StopTimer();
            AutoSaving.StartTimer();

            //Log
            bool isAutoSave = fileName != projectFileName;
            string name = "OpenProject";
            if (isAutoSave)
                name = "OpenAutoSaveProject";

            App.Log("Action", name, "A project was opened; path=\"" + projectFileName + "\"");

            //Event
            EventHandler handler = OnOpenProject;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
            _ProgressBarModal_Update();

            isWorking = false;
        }

        /// <summary>
        /// Opens a project from a file.
        /// </summary>
        /// <param name="fileName">The file to open.</param>
        /// <param name="projectFileName">Sets <see cref="ConwayNativityDirectoryProject.FileName"/></param>
        /// <param name="showMessage">Shows a message durring the saving process.</param>
        public void Open(string fileName, string projectFileName, bool showMessage)
        {
            if (showMessage)
            {
                //WaitForm f = new WaitForm("Loading...");
                //f.Show();
                //f.Invalidate();
                //f.Update();
                //Open(fileName, realFileName);
                //f.Close();

                _ProgressBarModal.Process = new ProgressBarModalProcess((ProgressBarModalRef p) =>
                {
                    Open(fileName, projectFileName);
                });
                _ProgressBarModal.Title = "Open Project";
                _ProgressBarModal.Info = "Please wait...";
                _ProgressBarModal.Show();
            }

            else
            {
                Open(fileName, projectFileName);
            }
        }

        /// <summary>
        /// Opens a project from a file.
        /// </summary>
        /// <param name="fileName">The file to open.</param>
        /// <param name="showMessage">Shows a message durring the saving process.</param>
        public void Open(string fileName, bool showMessage)
        {
            Open(fileName, fileName, showMessage);
        }


        /// <summary>
        /// Opens a project from a file.
        /// </summary>
        /// <param name="fileName">The file to open.</param>
        public void Open(string fileName)
        {
            Open(fileName, fileName, false);
        }

        /// <summary>
        /// Closes the current project.
        /// </summary>
        public void Close()
        {
            isWorking = true;

            //Event
            EventHandler previewHandler = PreviewCloseProject;
            if (previewHandler != null)
                previewHandler(this, new EventArgs());

            CachePreference cache = (CachePreference)App.Preferences.GetPreference(@"Performance\Cache");

            //Unload images
            foreach (Nativity nativity in Nativities)
                if (nativity.HasImage)
                    nativity.Information.UnloadImage();

            //Reset things
            fileName = null;
            uid = String.Empty;
            Nativities.Clear();
            Actions.Clear();
            meta.Clear();
            mainWindow.informationContent.Content = null;

            //UI changes - menu items
            mainWindow.closeMenuItem.IsEnabled = false;
            mainWindow.saveMenuItem.IsEnabled = false;
            mainWindow.saveAsMenuItem.IsEnabled = false;
            mainWindow.saveACopyMenuItem.IsEnabled = false;
            mainWindow.importMenuItem.IsEnabled = false;
            mainWindow.exportMenuItem.IsEnabled = false;
            mainWindow.projectMenuItem.IsEnabled = false;
            mainWindow.undoMenuItem.IsEnabled = false;
            mainWindow.redoMenuItem.IsEnabled = false;
            mainWindow.assignImagesByIdMenuItem.IsEnabled = false;
            mainWindow.removeImagesMenuItem.IsEnabled = false;
            mainWindow.imageMenuItem.IsEnabled = false;
            mainWindow.taggingMenuItem.IsEnabled = false;
            mainWindow.nativitiesMenuItem.IsEnabled = false;
            mainWindow.searchMenuItem.IsEnabled = false;
            mainWindow.viewMenuItem.IsEnabled = false;

            //UI changes - grid
            mainWindow.projectGrid.Visibility = System.Windows.Visibility.Collapsed;
            mainWindow.Title = "Conway Nativity Directory";

            //File system changes
            string contentPath = cache.CacheFolder + @"\content\";
            if (Directory.Exists(contentPath))
            {
                System.IO.Directory.Delete(contentPath, true);
            }

            //Close project
            this.isOpen = false;

            //Other important stuff
            if (projectSettings != null)
                foreach (IProjectSettings settings in projectSettings)
                    settings.OnCloseProject();
            this.projectSettings = null;

            //Stop auto-save
            AutoSaving.StopTimer();

            //Log
            App.Log("Action", "ProjectClosed", "A project was closed; path=\"" + FileName + "\"");

            //Event
            EventHandler handler = OnCloseProject;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }

            isWorking = false;
        }

        /// <summary>
        /// Saves the current project if a file name in known.
        /// </summary>
        public void Save()
        {
            this.SaveAs(FileName, FileName);
        }


        /// <summary>
        /// Saves the current project if a file name in known.
        /// </summary>
        /// <param name="showMessage">Choose to show message.</param>
        public void Save(bool showMessage)
        {
            this.SaveAs(FileName, FileName, showMessage);
        }

        /// <summary>
        /// Saves the current project with a new file name.
        /// </summary>
        /// <param name="fileName"></param>
        public void SaveAs(string fileName, string projectFileName)
        {
            isWorking = true;

            //ProgressBarModal
            _ProgressBarModalInfoText = "Please wait...";
            _ProgressBarModal.Max = Nativities.Count + 6;
            _ProgressBarModal_UpdateInfo();

            //Event
            EventHandler previewHandler = PreviewSaveProject;
            if (previewHandler != null)
                previewHandler(this, new EventArgs());

            //Save file name
            this.fileName = projectFileName;

            //Create contents
            string temporarySaveDest = CreateProjectDirectory();
            string descriptionsDest = temporarySaveDest + @"\Descriptions";
            Directory.CreateDirectory(temporarySaveDest);
            Directory.CreateDirectory(descriptionsDest);
            _ProgressBarModal_Update();

            //Data file
            string dataFileDest = temporarySaveDest + @"\data.dat";
            string dataFileContents =
                "uid=" + this.uid;
            File.WriteAllText(dataFileDest, dataFileContents);
            _ProgressBarModal_Update();

            foreach (Nativity nativity in Nativities)
            {
                string _fileName = GetValidFileName(nativity.Id.ToString() + "_" + nativity.Title) + 0.ToString() + ".nty";
                int count = 0;
                do
                {
                    _fileName = GetValidFileName(nativity.Id.ToString() + "_" + nativity.Title) + count + ".nty";
                    count++;
                } while (File.Exists(_fileName));

                nativity.Save(temporarySaveDest + @"\" + _fileName);
                _ProgressBarModalInfoText = "Saving Nativity #" + nativity.Id.ToString();
                _ProgressBarModal_Update();
            }
            _ProgressBarModalInfoText = "Please wait...";
            _ProgressBarModal_UpdateInfo();

            //Project settings
            string projectSettingsPath = temporarySaveDest + @"\projset.dat";

            foreach (IProjectSettings settings in this.ProjectSettings)
                if (settings.ApplyOnProjectSave)
                    settings.Apply();

            this.ProjectSettings.Save(projectSettingsPath);

            foreach (IProjectSettings settings in projectSettings)
            {
                settings.OnSaveProject();
            }
            _ProgressBarModal_Update();

            //Meta
            string metaPath = temporarySaveDest + @"\project.meta";
            var fs = new MetaStorageFileSystem(meta);
            fs.SaveToFile(metaPath);
            _ProgressBarModal_Update();

            //Local Meta
            var localMetaPath = AppDomain.CurrentDomain.BaseDirectory + @"\bin\storage\project\" + UID + @"\project-local.meta";
            if (!Directory.Exists(Path.GetDirectoryName(localMetaPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(localMetaPath));
            var lmfs = new MetaStorageFileSystem(localMeta);
            lmfs.SaveToFile(localMetaPath);

            //Save contents
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            ZipFile.CreateFromDirectory(temporarySaveDest, fileName);
            Directory.Delete(temporarySaveDest, true);
            _ProgressBarModal_Update();

            //UI changes - menu items
            mainWindow.saveMenuItem.IsEnabled = true;

            //Log
            bool isAutoSave = fileName != projectFileName;
            string name = "SaveProject";
            if (isAutoSave)
                name = "AutoSaveProject";

            App.Log("Action", name, "A project was saved; path=\"" + projectFileName + "\"");

            //Event
            EventHandler handler = OnSaveProject;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
            _ProgressBarModal_Update();

            isWorking = false;
        }

        /// <summary>
        /// Saves the current project with a new file name.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="showMessage">Choose to show message.</param>
        public void SaveAs(string fileName, string projectFileName, bool showMessage)
        {
            if (showMessage)
            {
                _ProgressBarModal.Process = new ProgressBarModalProcess((ProgressBarModalRef p) =>
                {
                    SaveAs(fileName, projectFileName);
                });
                _ProgressBarModal.Title = "Save Project";
                _ProgressBarModal.Info = "Please wait...";
                _ProgressBarModal.Show();
            }

            else
            {
                SaveAs(fileName, projectFileName);
            }
        }

        public void SaveAs(string fileName, bool showMessage)
        {
            SaveAs(fileName, fileName, showMessage);
        }

        public void SaveAs(string fileName)
        {
            SaveAs(fileName, fileName, false);
        }

        /// <summary>
        /// Saves the project to the auto-save folder.
        /// </summary>
        public void AutoSave()
        {
            AutoSave(false);
        }

        /// <summary>
        /// Saves the project to the auto-save folder.
        /// </summary>
        /// <param name="showMessage">Shows a message during the saving process.</param>
        public void AutoSave(bool showMessage)
        {
            AutoSavePreference preference = (AutoSavePreference)App.Preferences.GetPreference(@"Optimization\Auto-Save");

            if (IsOpen)
            {
                //File naming
                string timestamp = DateTime.Now.ToString("MM-dd-yyyy-HH-mm-ss-tt");
                string filedir = preference.AutoSaveFolder + @"\" + uid;
                string filename = preference.AutoSaveFolder + @"\" + uid + @"\" +
                    timestamp + @".cnb";

                //If the project has already been saved, then save over the actual project
                if (File.Exists(FileName))
                    filename = FileName;

                //Create folder - only if the project has not yet been saved.
                if (!Directory.Exists(filedir) && !File.Exists(filename))
                    Directory.CreateDirectory(filedir);

                //Delete expired files
                foreach (string path in Directory.GetFiles(filedir, "*.cnb"))
                {
                    DateTime saveDate = new FileInfo(path).CreationTime;
                    TimeSpan time = DateTime.Now.Subtract(saveDate);

                    if (time.TotalSeconds > preference.AutoSaveAgeLimit.TotalSeconds)
                        File.Delete(path);
                }

                //Save project to dest
                if (showMessage)
                {
                    _ProgressBarModal.Process = new ProgressBarModalProcess((ProgressBarModalRef p) =>
                    {
                        SaveAs(filename, FileName);
                    });
                    _ProgressBarModal.Title = "Auto Save";
                    _ProgressBarModal.Info = "Please wait...";
                    _ProgressBarModal.Show();
                }

                else
                {
                    SaveAs(filename, FileName);
                }
            }
        }

        /// <summary>
        /// Gets the latest automatically saved file associated with the currently opened project.
        /// </summary>
        /// <returns></returns>
        public AutoSaveInfo? GetLatestAutoSave()
        {
            AutoSaveInfo[] autoSaveInfos = GetAutoSaves();

            if (autoSaveInfos != null && autoSaveInfos.Count() > 0)
                return autoSaveInfos.FirstOrDefault();
            else
                return null;
        }

        public AutoSaveInfo[] GetAutoSaves()
        {
            if (IsOpen)
            {
                AutoSavePreference preference = (AutoSavePreference)App.Preferences.GetPreference(@"Optimization\Auto-Save");
                string filedir = preference.AutoSaveFolder + @"\" + uid;

                List<AutoSaveInfo> infos = new List<AutoSaveInfo>();
                if (Directory.Exists(filedir))
                {
                    foreach (string path in Directory.GetFiles(filedir, "*.cnb"))
                        infos.Add(new AutoSaveInfo(path));
                }

                infos = infos.OrderByDescending(i => i.Created).ToList();
                return infos.ToArray();
            }

            else
                return null;
        }

        #endregion


        #region Public Methods

        /// <summary>
        /// Sets the <see cref="IFilter"/> to use for searching.
        /// </summary>
        /// <param name="filter"></param>
        public void SetSearchFilter(IFilter filter)
        {
            searchResults.Filter = filter.Filter;
        }

        /// <summary>
        /// Refreshes the seach results.
        /// </summary>
        public void Search()
        {
            CollectionViewSource.GetDefaultView(mainWindow.nativityListView.ItemsSource).Refresh();
        }

        public MetaStorage GetMeta()
        {
            return this.meta;
        }

        public MetaStorage GetLocalMeta()
        {
            return this.localMeta;
        }

        #endregion


        #region Private Methods

        private void SetBindings()
        {
            //Bind the ItemsSource property of mainWindow.luaFiles to LuaFiles
            App.SetBinding("Nativities", this, mainWindow.nativityListView, ListView.ItemsSourceProperty);

            Binding countBinding = new Binding("Count");
            countBinding.Source = this.mainWindow.nativityListView.Items;
            countBinding.Converter = new NativityCountConverter(this);
            countBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

            App.SetBinding(countBinding, mainWindow.nativityCountTextBlock, TextBlock.TextProperty);
        }

        private string GenerateUID()
        {
            Random random = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyz" +
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ" + "0123456789";
            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void SetFilter()
        {
            searchResults = (CollectionView)CollectionViewSource.GetDefaultView(mainWindow.nativityListView.ItemsSource);
        }

        private string CreateProjectDirectory()
        {
            CachePreference cache = (CachePreference)App.Preferences.GetPreference(@"Performance\Cache");

            int count = 0;
            string path = cache.CacheFolder + @"\Project";

            do
            {
                path = cache.CacheFolder + @"\Project" + count.ToString();
                count++;
            } while (System.IO.Directory.Exists(path));

            return path;
        }

        private static string GetValidFileName(string fileName)
        {
            string invalid = new string(Path.GetInvalidFileNameChars());
            string result = fileName;

            foreach (char c in invalid)
            {
                result = result.Replace(c, '_');
            }

            return result;
        }

        private void NativityListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsOpen)
            {
                EventHandler handler = SelectedNativitiesChanged;
                if (handler != null)
                {
                    handler(this, new EventArgs());
                }
            }
        }

        #endregion


        #region Events

        #endregion


    }

    public struct AutoSaveInfo
    {
        private string fileName;
        public string FileName { get { return fileName; } }

        private DateTime created;
        public DateTime Created {  get {  return created; } }

        public AutoSaveInfo(string fileName)
        {
            this.fileName = fileName;
            string date = Path.GetFileNameWithoutExtension(fileName);
            this.created = new FileInfo(fileName).CreationTime;
        }
    }

    public class NativityCountConverter : IValueConverter
    {
        private ConwayNativityDirectoryProject project;
        public NativityCountConverter(ConwayNativityDirectoryProject project)
        {
            this.project = project;
        }

        public object Convert(object value, Type targetType, object target, System.Globalization.CultureInfo culture)
        {
            int count = (int)value;
            string plural = "";

            if (count != 1)
                plural = "Nativities";
            else
                plural = "Nativity";

            if (count == project.Nativities.Count)
                return count.ToString() + " " + plural;
            else
                return count.ToString() + " of " + project.Nativities.Count + " " + plural;
        }

        public object ConvertBack(object value, Type targetType, object target, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
