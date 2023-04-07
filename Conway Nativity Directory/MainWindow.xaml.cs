using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using ConwayNativityDirectory.PluginApi;
using Application = System.Windows.Application;
using MenuItem = System.Windows.Controls.MenuItem;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using FolderBrowserDialog = System.Windows.Forms.FolderBrowserDialog;
using TextBox = System.Windows.Controls.TextBox;
using System.Security;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using WPFListViewItem = System.Windows.Controls.ListViewItem;
using System.Globalization;
using System.Windows.Threading;
using System.Text.RegularExpressions;
using NLua.Exceptions;

namespace Conway_Nativity_Directory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        #region MainWindow

        public MainWindow()
        {
            InitializeComponent();

            App.LoadAssemblyResolve();

            SplashScreen splashScreen = new SplashScreen(@"splash.png");
            splashScreen.Show(false, true);

            App.SetMainWindow(this);

            LoadGlobalMeta();

            CreatePreferencesFile();
            preferences = Preferences.Load(AppDomain.CurrentDomain.BaseDirectory + @"\settings\preferences.xml");
            App.SetPreferences(this.Preferences);
            this.LoginSettings = (LoginSettingsPreference)Preferences.GetPreference(@"Security\Login Settings");
            ViewColumnMenuItemInvokeCheckedUnchecked();

            Commands.RegisterInternalCommands();

            project = new ConwayNativityDirectoryProject(this);
            App.SetProject(this.Project);

            SetBindings();

            this.PreviewMouseDown += MainWindow_PreviewMouseDown;
            this.nativityListView.PreviewMouseDown += MainWindow_PreviewMouseDown;
            this.Loaded += MainWindow_Loaded;
            this.Closing += Window_Closing;

            WorkspacesMain.CreateMainWorkspace();

            PluginDeployment.Run();
            PluginDatabaseMain.LoadPlugins();

            LuaScripting.Load();

            AutoSaving.Load();

            Thread.Sleep(TimeSpan.FromMilliseconds(3000));
            splashScreen.Close(TimeSpan.FromMilliseconds(500));
        }

        internal bool isLoaded = false;
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            bool abruptlyClosed = LoadAfterAbruptlyClosed();
            if (!abruptlyClosed)
            {
                App.CheckForUpdates();
                LoadChangelogsWindow();

                if (System.IO.File.Exists(App.AssociatedFileOnStartup))
                    Project.Open(App.AssociatedFileOnStartup, true);
                else
                    AutoLoadProject();

                CheckForRecentAutoSave();
            }

            this.RefreshSort();

            if (bringIntoViewOnLoad > -1 && bringIntoViewOnLoad < nativityListView.Items.Count)
                nativityListView.ScrollIntoView(Project.Nativities[bringIntoViewOnLoad]);

            WorkspacesMain.LoadWorkspaces();
            //LuaScripting.LoadScripts();

            isLoaded = true;
        }

        bool AbruptlyClosing = false;
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!AbruptlyClosing)
            {
                if (AskToExit() == false)
                {
                    e.Cancel = true;
                    return;
                }

                if (AdvancedSearch != null)
                    AdvancedSearch.Close();

                if (Project.IsOpen)
                    Project.Close();

                PluginDatabaseMain.SavePluginStorage();
                SaveGlobalMeta();

                Application.Current.Shutdown();
            }
        }

        private void LoadGlobalMeta()
        {
            MetaStorage ms = new MetaStorage();
            App.GlobalMeta = ms;
            MetaStorageFileSystem fs = new MetaStorageFileSystem(ms);

            string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\bin\globalmeta.meta";
            if (System.IO.File.Exists(filePath))
                fs.LoadFromFile(filePath);
        }

        private void SaveGlobalMeta()
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\bin\globalmeta.meta";
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
            MetaStorageFileSystem fs = new MetaStorageFileSystem(App.GlobalMeta);
            fs.SaveToFile(filePath);
        }

        private void LoadChangelogsWindow()
        {
            string firstInstancePath = AppDomain.CurrentDomain.BaseDirectory + @"\first-instance";
            if (System.IO.File.Exists(firstInstancePath))
            {
                System.IO.File.Delete(firstInstancePath);
                MessageBox.Show("Welcome to Conway Nativity Directory v" + App.Version.ToString() + "." + '\n' +
                    "Please see the \"Changelogs\" window to view all changes that were made from the previous version of Conway Nativity Directory.");
                ChangelogsWindow cl = new ChangelogsWindow();
                cl.ShowDialog();
            }
        }

        private void CreatePreferencesFile()
        {
            string preferencesDefaultXML = AppDomain.CurrentDomain.BaseDirectory + @"\settings\preferences_default.xml";
            string preferencesXML = AppDomain.CurrentDomain.BaseDirectory + @"\settings\preferences.xml";

            if (!System.IO.File.Exists(preferencesXML))
            {
                if (System.IO.File.Exists(preferencesDefaultXML))
                    System.IO.File.Copy(preferencesDefaultXML, preferencesXML);
                else
                {
                    throw new System.IO.FileNotFoundException("\"preferences.xml\" could not be created.");
                }
            }

        }

        private int bringIntoViewOnLoad;
        private bool LoadAfterAbruptlyClosed()
        {
            string abruptInfoFile = AppDomain.CurrentDomain.BaseDirectory + @"\abrupt.txt";
            string[] data = null;
            if (System.IO.File.Exists(abruptInfoFile))
            {
                data = System.IO.File.ReadAllLines(abruptInfoFile);
                System.IO.File.Delete(abruptInfoFile);
            }

            if (data != null)
            {
                bringIntoViewOnLoad = Convert.ToInt32(data[0]);
                string fileName = data[1];
                string realFileName = data.ElementAtOrDefault(2);

                if (String.IsNullOrEmpty(realFileName))
                    realFileName = fileName;

                Project.Open(fileName, realFileName, true);

                return true;
            }

            return false;
        }

        private void AutoLoadProject()
        {
            StartupPreference sp = (StartupPreference)Preferences.GetPreference(@"Optimization\Startup");

            if (System.IO.File.Exists(sp.AutoLoadFile))
            {
                MessageBoxResult mbr = MessageBox.Show("Would you like to open the 'Auto-Load' file specified in Preferences?",
                    "Conway Nativity Directory", MessageBoxButton.YesNo);
                if (mbr == MessageBoxResult.Yes)
                {
                    Project.Open(sp.AutoLoadFile, true);
                }
            }
        }

        internal void ConfigureColumns()
        {
            var columns = App.Preferences.GetPreference(@"Interface\Columns") as ColumnsPreference;

            if (!columns.ShowId)
                idGridViewColumn.Width = 0;
            if (!columns.ShowTitle)
                titleGridViewColumn.Width = 0;
            if (!columns.ShowOrigin)
                originGridViewColumn.Width = 0;
            if (!columns.ShowAcquired)
                acquiredGridViewColumn.Width = 0;
            if (!columns.ShowFrom)
                fromGridViewColumn.Width = 0;
            if (!columns.ShowCost)
                costGridViewColumn.Width = 0;
            if (!columns.ShowLocation)
                locationGridViewColumn.Width = 0;
            if (!columns.ShowTags)
                tagsGridViewColumn.Width = 0;
            if (!columns.ShowGeographicalOrigins)
                geographicalOriginsGridViewColumn.Width = 0;
        }



        #endregion


        #region Project 

        private ConwayNativityDirectoryProject project;
        public ConwayNativityDirectoryProject Project { get { return project; } }

        #endregion


        #region Preferences

        private Preferences preferences;
        public Preferences Preferences { get { return preferences; } }
        
        private static readonly DependencyPropertyKey LoginSettingsPropertyKey =
            DependencyProperty.RegisterReadOnly("LoginSettings", typeof(LoginSettingsPreference), typeof(MainWindow),
                new PropertyMetadata(null));

        public static readonly DependencyProperty LoginSettingsProperty =
            LoginSettingsPropertyKey.DependencyProperty;

        public LoginSettingsPreference LoginSettings
        {
            get { return (LoginSettingsPreference) GetValue(LoginSettingsProperty); }
            set { SetValue(LoginSettingsPropertyKey, value); }
        }

        #endregion


        #region Allowing Editing
        
        public static readonly DependencyProperty AllowEditingTimeStampProperty =
            DependencyProperty.Register(nameof(AllowEditingTimeStamp), typeof(DateTime), typeof(MainWindow),
                new PropertyMetadata(null));

        public DateTime AllowEditingTimeStamp
        {
            get { return (DateTime)GetValue(AllowEditingTimeStampProperty); }
            set { SetValue(AllowEditingTimeStampProperty, value); }
        }


        public bool AllowEditing
        {
            get { return ((LoginSettingsPreference) Preferences.GetPreference(@"Security\Login Settings")).AllowEditing; }
        }

        #endregion


        #region Sorting

        public static readonly DependencyProperty SortingModeProperty =
            DependencyProperty.Register("SortingMode", typeof(SortingMode), typeof(MainWindow), new PropertyMetadata(SortingMode.Ascending));

        public SortingMode SortingMode {
            get { return (SortingMode)GetValue(SortingModeProperty); }
            set { SetValue(SortingModeProperty, value); }
        }

        public static readonly DependencyProperty SortingTypeProperty =
            DependencyProperty.Register("SortingType", typeof(SortingType), typeof(MainWindow), new PropertyMetadata(SortingType.Id,
                new PropertyChangedCallback(SortingTypeChangedCallback)));

        private static void SortingTypeChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)sender;

            var list = SortNativities(mainWindow.Project, mainWindow.SortingMode, (SortingType)e.NewValue).ToList();
            _ = ResetListViewAsync(mainWindow, list);
        }

        private static async Task ResetListViewAsync(MainWindow mainWindow, List<Nativity> list)
        {
            await App.Current.Dispatcher.BeginInvoke(DispatcherPriority.Input,
                new Action(delegate () {
                    mainWindow.Project.Nativities.Clear();
                    for (int i = 0; i < list.Count(); i++)
                        mainWindow.Project.Nativities.Add(list[i]);
                }));
        }

        public SortingType SortingType
        {
            get { return (SortingType)GetValue(SortingTypeProperty); }
            set
            {
                SetValue(SortingTypeProperty, value);
            }
        }

        public static IEnumerable<Nativity> SortNativities(ConwayNativityDirectoryProject project, SortingMode sortingMode, SortingType sortingType)
        {
            switch (sortingType)
            {
                case SortingType.Id:
                    if (sortingMode == SortingMode.Ascending)
                        return project.Nativities.Cast<Nativity>().OrderBy(o => o.Id);

                    else if (sortingMode == SortingMode.Descending)
                        return project.Nativities.Cast<Nativity>().OrderByDescending(o => o.Id);

                    break;

                case SortingType.Title:
                    if (sortingMode == SortingMode.Ascending)
                        return project.Nativities.Cast<Nativity>().OrderBy(o => RemoveSpecialChars(o.Title));

                    else if (sortingMode == SortingMode.Descending)
                        return project.Nativities.Cast<Nativity>().OrderByDescending(o => RemoveSpecialChars(o.Title));

                    break;

                case SortingType.Origin:
                    if (sortingMode == SortingMode.Ascending)
                        return project.Nativities.Cast<Nativity>().OrderBy(o => RemoveSpecialChars(o.Origin));

                    else if (sortingMode == SortingMode.Descending)
                        return project.Nativities.Cast<Nativity>().OrderByDescending(o => RemoveSpecialChars(o.Origin));

                    break;

                case SortingType.Acquired:
                    if (sortingMode == SortingMode.Ascending)
                        return project.Nativities.Cast<Nativity>().OrderBy(o => o.Acquired);

                    else if (sortingMode == SortingMode.Descending)
                        return project.Nativities.Cast<Nativity>().OrderByDescending(o => o.Acquired);

                    break;

                case SortingType.From:
                    if (sortingMode == SortingMode.Ascending)
                        return project.Nativities.Cast<Nativity>().OrderBy(o => RemoveSpecialChars(o.From));

                    else if (sortingMode == SortingMode.Descending)
                        return project.Nativities.Cast<Nativity>().OrderByDescending(o => RemoveSpecialChars(o.From));

                    break;

                case SortingType.Cost:
                    if (sortingMode == SortingMode.Ascending)
                        return project.Nativities.Cast<Nativity>().OrderBy(o => o.Cost);

                    else if (sortingMode == SortingMode.Descending)
                        return project.Nativities.Cast<Nativity>().OrderByDescending(o => o.Cost);

                    break;

                case SortingType.Location:
                    if (sortingMode == SortingMode.Ascending)
                        return project.Nativities.Cast<Nativity>().OrderBy(o => RemoveSpecialChars(o.Location));

                    else if (sortingMode == SortingMode.Descending)
                        return project.Nativities.Cast<Nativity>().OrderByDescending(o => RemoveSpecialChars(o.Location));

                    break;

                case SortingType.Tags:
                    if (sortingMode == SortingMode.Ascending)
                        return project.Nativities.Cast<Nativity>().OrderBy(
                            o => RemoveSpecialChars(string.Join(String.Empty, o.Tags)));

                    else if (sortingMode == SortingMode.Descending)
                        return project.Nativities.Cast<Nativity>().OrderByDescending(
                            o => RemoveSpecialChars(string.Join(String.Empty, o.Tags)));

                    break;

                case SortingType.GeographicalOrigins:
                    if (sortingMode == SortingMode.Ascending)
                        return project.Nativities.Cast<Nativity>().OrderBy(
                            o => RemoveSpecialChars(string.Join(String.Empty, o.GeographicalOrigins)));

                    else if (sortingMode == SortingMode.Descending)
                        return project.Nativities.Cast<Nativity>().OrderByDescending(
                            o => RemoveSpecialChars(string.Join(String.Empty, o.GeographicalOrigins)));

                    break;

                default:
                    if (sortingMode == SortingMode.Ascending)
                        return project.Nativities.Cast<Nativity>().OrderBy(o => o.Id);

                    else if (sortingMode == SortingMode.Descending)
                        return project.Nativities.Cast<Nativity>().OrderByDescending(o => o.Id);

                    break;
            }

            return project.Nativities.Cast<Nativity>();
        }

        private void ChangeSortingMode(SortingType newSortingType)
        {
            if (SortingType == newSortingType)
            {
                if (SortingMode == SortingMode.Ascending)
                    SortingMode = SortingMode.Descending;

                else if (SortingMode == SortingMode.Descending)
                    SortingMode = SortingMode.Ascending;

                RefreshSort();
            }
        }


        private static string RemoveSpecialChars(string value)
        {
            return System.Text.RegularExpressions.Regex.Replace(value, @"[^0-9a-zA-Z]+", string.Empty);
        }

        public void RefreshSort()
        {
            SortingTypeChangedCallback(this, new DependencyPropertyChangedEventArgs(SortingTypeProperty, SortingType, SortingType));
        }


        #region Click Events

        private void IdColumn_Click(object sender, RoutedEventArgs e)
        {
            ChangeSortingMode(SortingType.Id);
            SortingType = SortingType.Id;
        }

        private void TitleColumn_Click(object sender, RoutedEventArgs e)
        {
            ChangeSortingMode(SortingType.Title);
            SortingType = SortingType.Title;
        }

        private void OriginColumn_Click(object sender, RoutedEventArgs e)
        {
            ChangeSortingMode(SortingType.Origin);
            SortingType = SortingType.Origin;
        }

        private void AcquiredColumn_Click(object sender, RoutedEventArgs e)
        {
            ChangeSortingMode(SortingType.Acquired);
            SortingType = SortingType.Acquired;
        }

        private void FromColumn_Click(object sender, RoutedEventArgs e)
        {
            ChangeSortingMode(SortingType.From);
            SortingType = SortingType.From;
        }

        private void CostColumn_Click(object sender, RoutedEventArgs e)
        {
            ChangeSortingMode(SortingType.Cost);
            SortingType = SortingType.Cost;
        }

        private void LocationColumn_Click(object sender, RoutedEventArgs e)
        {
            ChangeSortingMode(SortingType.Location);
            SortingType = SortingType.Location;
        }

        private void TagsColumn_Click(object sender, RoutedEventArgs e)
        {
            ChangeSortingMode(SortingType.Tags);
            SortingType = SortingType.Tags;
        }

        private void GeographicalOriginsColumn_Click(object sender, RoutedEventArgs e)
        {
            ChangeSortingMode(SortingType.GeographicalOrigins);
            SortingType = SortingType.GeographicalOrigins;
        }

        #endregion


        #endregion


        #region Dependency Properties


        static DependencyPropertyKey MultipleNativitiesSelectedPropertyKey = DependencyProperty.RegisterReadOnly(
            "MultipleNativitiesSelected", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

        public static readonly DependencyProperty MultipleNativitiesSelectedProperty =
            MultipleNativitiesSelectedPropertyKey.DependencyProperty;

        public bool MultipleNativitiesSelected
        {
            get { return (bool)GetValue(MultipleNativitiesSelectedProperty); }
            private set { SetValue(MultipleNativitiesSelectedPropertyKey, value); }
        }

        #endregion


        #region Public Properties



        #endregion


        #region Private Properties

        #region For get-only public properties




        #endregion


        #region Other

        private Nativity LastSelectedNativity;

        #endregion

        #endregion


        #region Public Methods


        public void ExpandColumns(double width)
        {
            var p = (ColumnsPreference)Preferences.GetPreference(@"Interface\Columns");

            if (p.ShowId)
                idGridViewColumn.Width = width;
            if (p.ShowTitle)
                titleGridViewColumn.Width = width;
            if (p.ShowOrigin)
                originGridViewColumn.Width = width;
            if (p.ShowAcquired)
                acquiredGridViewColumn.Width = width;
            if (p.ShowFrom)
                fromGridViewColumn.Width = width;
            if (p.ShowCost)
                costGridViewColumn.Width = width;
            if (p.ShowLocation)
                locationGridViewColumn.Width = width;
            if (p.ShowTags)
                tagsGridViewColumn.Width = width;
            if (p.ShowGeographicalOrigins)
                geographicalOriginsGridViewColumn.Width = width;
        }


        #endregion


        #region Private Methods


        #region Events


        #region Menu


        #region File

        private void NewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Project.IsOpen)
            {
                MessageBoxResult mbr = MessageBox.Show(
                    "Are you sure you would like to create a new project while another project is already opened? " +
                    "The current project will be lost unless it is saved.", "Conway Nativity Directory",
                    MessageBoxButton.YesNo);

                if (mbr == MessageBoxResult.Yes)
                {
                    Project.New();
                }
            }

            else
            {
                Project.New();
            }
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            bool allow = true;

            if (Project.IsOpen)
            {
                allow = false;

                MessageBoxResult mbr = MessageBox.Show(
                    "Are you sure you would like to open project while another project is already opened? " +
                    "The current project will be lost unless it is saved.", "Conway Nativity Directory",
                    MessageBoxButton.YesNo);

                if (mbr == MessageBoxResult.Yes)
                {
                    allow = true;
                }
            }

            if (allow)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Title = "Open";
                ofd.Filter = "Conway Nativity Directory Project (*.cnp)|*.cnp|Conway Nativity Directory Backup (*.cnb)|*.cnb";

                if (ofd.ShowDialog() == true)
                {
                    Project.Open(ofd.FileName, true);
                    CheckForRecentAutoSave();
                    SortingMode = SortingMode.Ascending;
                    SortingType = SortingType.Id;
                    RefreshSort();
                }
            }
        }

        private void CloseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            bool allow = true;

            if (Project.IsOpen)
            {
                allow = false;

                MessageBoxResult mbr = MessageBox.Show(
                    "Are you sure you would like to close the current project? " +
                    "The current project will be lost unless it is saved.", "Conway Nativity Directory",
                    MessageBoxButton.YesNo);

                if (mbr == MessageBoxResult.Yes)
                {
                    allow = true;
                }
            }

            if (allow)
                Project.Close();
        }

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Project.IsOpen && System.IO.File.Exists(Project.FileName))
                Project.Save(true);
        }

        private void SaveAsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Project.IsOpen)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Save As";
                sfd.Filter = "Conway Nativity Directory Project (*.cnp)|*.cnp|Conway Nativity Directory Backup (*.cnb)|*.cnb";

                if (sfd.ShowDialog() == true)
                {
                    Project.SaveAs(sfd.FileName, true);
                }
            }
        }

        private void SaveACopyMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Project.IsOpen)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Save a Copy";
                sfd.Filter = "Conway Nativity Directory Project (*.cnp)|*.cnp|Conway Nativity Directory Backup (*.cnb)|*.cnb";

                if (sfd.ShowDialog() == true)
                {
                    Project.SaveAs(sfd.FileName, Project.FileName, true);
                }
            }
        }

        private void scriptsManagerMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ScriptsManagerWindow smw = new ScriptsManagerWindow();
            smw.ShowDialog();
        }

        public void executeScriptMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var menuItem = (ScriptMenuItem)sender;
                LuaScripting.Engine.DoFile(menuItem.FileName);
            }

            catch (Exception ex)
            {
                if (ex is FileNotFoundException)
                    MessageBox.Show("Failed to run the script because the file does not exist.");
                else
                {
                    LuaScriptException lse = ex as LuaScriptException;
                    if (lse != null && lse.InnerException != null)
                        MessageBox.Show("Failed to run script: " + ex.InnerException.Message);
                    else
                        MessageBox.Show("Failed to run script: " + ex.Message);
                }
            }
        }

        private void parseNarrativesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ImportNarrativesDialog ind = new ImportNarrativesDialog();
            ind.ShowDialog();
        }

        private void exportNarrativesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text Documents (*.txt)|*.txt";
            if (sfd.ShowDialog() == true)
            {
                WaitForm waitForm = new WaitForm("Please Wait...");
                waitForm.Text = "Export Narratives";
                waitForm.Show();
                waitForm.Update();

                string content = "";
                foreach (Nativity nativity in Project.Nativities)
                    content = content + nativity.Id + ". " + nativity.Title + "\n" + nativity.Description + "\n";
                content.TrimEnd('\n');
                File.WriteAllText(sfd.FileName, content, Encoding.UTF8);

                waitForm.Close();
            }
        }

        private void LoginMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (LoginSettings.IsPasswordProtected && !LoginSettings.IsLoggedIn)
            {
                var inputDialog = new InputDialog();
                inputDialog.ShowDialog();

                if (inputDialog.Successful)
                {
                    bool wasCorrect =
                        ((LoginSettingsPreference)Preferences.GetPreference(@"Security\Login Settings")).Login(inputDialog
                            .Response);

                    if (wasCorrect)
                        MessageBox.Show("Login was successful.");
                    else
                        MessageBox.Show("Password was incorrect.");

                }
            }
        }

        private void LogoutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (LoginSettings.IsLoggedIn)
            {
                ((LoginSettingsPreference)Preferences.GetPreference(@"Security\Login Settings")).Logout();
            }
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(1);
        }

        #endregion


        #region Project

        private void SettingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ProjectSettingsWindow psw = new ProjectSettingsWindow();
            psw.ShowDialog();
        }

        #endregion


        #region Edit

        private void EditMenuItem_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            int count = Project.Actions.Count;
            int undoIndex = Project.Actions.SelectedIndex;
            int redoIndex = Project.Actions.SelectedIndex + 1;

            if (undoIndex > -1 && undoIndex < count)
                undoMenuItem.IsEnabled = true;
            else
                undoMenuItem.IsEnabled = false;

            if (redoIndex > -1 && redoIndex < count)
                redoMenuItem.IsEnabled = true;
            else
                redoMenuItem.IsEnabled = false;

            setDescriptions();
        }

        private void UndoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Project.Actions.Undo();
            setDescriptions();
            //RefreshSort();
        }

        private void RedoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Project.Actions.Redo();
            setDescriptions();
            //RefreshSort();
        }

        private void setDescriptions()
        {
            setDescription(undoMenuItem);
            setDescription(redoMenuItem);
        }

        private void setDescription(MenuItem menuItem)
        {
            string operation = "Undo";
            int offset = 0;
            if (menuItem.Name == "redoMenuItem")
            {
                offset = 1;
                operation = "Redo";
            }

            int index = Project.Actions.SelectedIndex + offset;
            if (index >= 0 && index < Project.Actions.Count)
            {
                IAction action = Project.Actions[index];
                prepareActionMenuItem(menuItem, operation, action.Description);
            }

            else
            {
                prepareActionMenuItem(menuItem, operation);
            }
        }

        private void prepareActionMenuItem(MenuItem menuItem, string operation, string description = "")
        {
            Brush brush = new SolidColorBrush(SystemColors.ControlTextColor);
            if (!menuItem.IsEnabled)
                brush = new SolidColorBrush(SystemColors.GrayTextColor);

            ContentControl _operation = new ContentControl();
            _operation.Foreground = brush;
            _operation.Width = 40;
            _operation.Content = operation;

            ContentControl _description = new ContentControl();
            _description.Content = description;

            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = System.Windows.Controls.Orientation.Horizontal;
            stackPanel.Children.Add(_operation);
            if (!String.IsNullOrEmpty(description))
                stackPanel.Children.Add(_description);

            Grid grid = new Grid();
            grid.Children.Add(stackPanel);

            menuItem.Header = grid;
        }


        private void AssignImagesByIdMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Files must be in JPEG format and can only contain numbers and a file extension to be imported. For example \"1.jpeg\".", "Assign Images By Id");
            MessageBoxResult mbr = MessageBox.Show("This process can take a while. Would you like to continue?", "Assign Images By Id", MessageBoxButton.YesNo);
            if (mbr == MessageBoxResult.Yes)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {

                    WaitForm f = new WaitForm("Assigning Images...");
                    f.Show();

                    int count = 0;
                    foreach (Nativity nativity in Project.Nativities)
                    {
                        string jpeg = fbd.SelectedPath + @"\" + nativity.Id.ToString() + ".jpeg";
                        string jpg = fbd.SelectedPath + @"\" + nativity.Id.ToString() + ".jpg";

                        if (System.IO.File.Exists(jpeg))
                        {
                            nativity.AddImage(jpeg);
                            count++;
                        }

                        else if (System.IO.File.Exists(jpg))
                        {
                            nativity.AddImage(jpg);
                            count++;
                        }
                    }

                    f.Close();

                    MessageBox.Show("Images were added to " + count.ToString() + " nativity(ies).");
                }
            }
        }

        private void RemoveImagesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbr = MessageBox.Show("Are you sure you want to remove images from the selected nativity(ies)?", "Remove Images", MessageBoxButton.YesNo);
            if (mbr == MessageBoxResult.Yes)
            {
                foreach (Nativity nativity in Project.Nativities)
                {
                    if (nativity.IsSelected)
                    {
                        nativity.RemoveImage();
                    }
                }
            }
        }

        private void RotateRightMenuItem_Click(object sender, RoutedEventArgs e)
        {
            foreach (Nativity nativity in Project.Nativities)
            {
                if (nativity.IsSelected)
                {
                    nativity.RotateImageRight();
                }
            }
        }

        private void RotateLeftMenuItem_Click(object sender, RoutedEventArgs e)
        {
            foreach (Nativity nativity in Project.Nativities)
            {
                if (nativity.IsSelected)
                {
                    nativity.RotateImageLeft();
                }
            }
        }

        private void EmbeddImagesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string grammar = "all";
            if (nativityListView.SelectedItems.Count > 0)
                grammar = "the selected";
            MessageBoxResult mbr = MessageBox.Show("Are you sure you want to embedd the images of " + grammar +
                " nativity(ies)?", "Embedd Images", MessageBoxButton.YesNo);

            if (mbr == MessageBoxResult.Yes)
            {
                WaitForm f = new WaitForm("Embedding Images...");
                f.Show();

                int count = 0;
                foreach (Nativity nativity in Project.Nativities)
                {
                    if (nativityListView.SelectedItems.Count > 0)
                    {
                        if (nativity.IsSelected && nativity.ImageMode == ImageMode.Linked)
                        {
                            nativity.EmbeddImage();
                            count++;
                        }
                    }

                    else
                    {
                        if (nativity.ImageMode == ImageMode.Linked)
                        {
                            nativity.EmbeddImage();
                            count++;
                        }
                    }
                }

                f.Close();
                MessageBox.Show("Images of " + count.ToString() + " nativity(ies) were embedded.");
            }
        }

        private void RelinkImagesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //simple grammar fix.
            string grammar = "all";
            if (nativityListView.SelectedItems.Count > 0)
                grammar = "the selected";

            //ask to relink the images
            MessageBoxResult mbr = MessageBox.Show("Are you sure you want to relink the images of " + grammar +
                " nativity(ies)?", "Relink Images", MessageBoxButton.YesNo);

            //count missing image links and ask to copy them if there are any missing image links.
            int missing = 0;
            List<Nativity> selected = new List<Nativity>();
            foreach (Nativity nativity in nativityListView.SelectedItems)
                selected.Add(nativity);

            if (selected.Count > 0)
                missing = countNativitiesWithMissingImageLinks(selected);
            else
                missing = countNativitiesWithMissingImageLinks(Project.Nativities);

            string copyToFolderPath = @"";
            MessageBoxResult relinkMissing = MessageBoxResult.No;
            if (missing > 0)
                relinkMissing = MessageBox.Show(missing.ToString() + " image(s) could not be relinked to their original files." +
                    "Would you like to specify a folder to save the images in then relink them? (File names will be automically generated.)",
                    "Relink Images", MessageBoxButton.YesNo);

            if (relinkMissing == MessageBoxResult.Yes)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();

                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    copyToFolderPath = fbd.SelectedPath;
            }


            //if the user wants to relink the images, do it.
            if (mbr == MessageBoxResult.Yes)
            {
                WaitForm f = new WaitForm("Relinking Images...");
                f.Show();

                int count = 0;
                foreach (Nativity nativity in Project.Nativities)
                {
                    if (nativityListView.SelectedItems.Count > 0)
                    {
                        if (nativity.IsSelected && nativity.ImageMode == ImageMode.Embedded)
                        {
                            if (relinkImage(nativity, copyToFolderPath))
                                count++;
                        }
                    }

                    else
                    {
                        if (nativity.ImageMode == ImageMode.Embedded)
                        {
                            if (relinkImage(nativity, copyToFolderPath))
                                count++;
                        }
                    }
                }

                f.Close();
                MessageBox.Show("Images of " + count.ToString() + " nativity(ies) were relinked.");
            }
        }

        //tries to relink the images of a nativity and returns a boolean indicating if it was successful.
        //if copyToFolderPath is not an existing folder, the image will not be relinked.
        private static bool relinkImage(Nativity nativity, string copyToFolderPath)
        {
            if (System.IO.File.Exists(nativity.OriginalImagePath))
            {
                nativity.RelinkImage();
                return true;
            }

            else if (System.IO.Directory.Exists(copyToFolderPath))
            {
                //generate a name and make sure the file name is not already used.
                string newImagePath = copyToFolderPath + @"\" + nativity.Id.ToString() + ".jpg";

                do
                {
                    Random random = new Random();
                    newImagePath = copyToFolderPath + @"\" + nativity.Id.ToString() + "_" + random.Next(0, 10000) + ".jpg";
                } while (System.IO.File.Exists(newImagePath));

                System.IO.File.Copy(nativity.ImagePath, newImagePath);
                nativity.RelinkImage(newImagePath);
                return true;
            }

            return false;
        }

        private static int countNativitiesWithMissingImageLinks(IEnumerable<object> objs)
        {
            List<Nativity> nativities = new List<Nativity>();
            foreach (object nativity in objs)
            {
                nativities.Add((Nativity)nativity);
            }

            return countNativitiesWithMissingImageLinks(nativities);
        }

        private static int countNativitiesWithMissingImageLinks(IEnumerable<Nativity> nativities)
        {
            int count = 0;
            foreach (Nativity nativity in nativities)
            {
                if (!System.IO.File.Exists(nativity.OriginalImagePath))
                    count++;
            }
            return count;
        }

        private void OrganizeSelectedTagsAndGeographicalOrigins(object sender, RoutedEventArgs e)
        {
            foreach (Nativity nativity in Project.Nativities)
            {
                if (nativity.IsSelected)
                {
                    List<string> tags = new List<string>(nativity.Tags.OrderBy(a => a).ToList());
                    List<string> geographical_origins = new List<string>(nativity.GeographicalOrigins.OrderBy(a => a).ToList());

                    nativity.Tags.Clear();
                    nativity.GeographicalOrigins.Clear();

                    foreach (string a in tags)
                    {
                        nativity.Tags.Add(a);
                    }

                    foreach (string a in geographical_origins)
                    {
                        nativity.GeographicalOrigins.Add(a);
                    }
                }
            }
        }

        private void AddTagToSelectionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            InputDialog id = new InputDialog() {FocusOnLoad = true};
            id.Title = "Add Tag";
            id.ShowDialog();

            if (id.Successful)
            {
                if (!String.IsNullOrEmpty(id.Response) && !String.IsNullOrWhiteSpace(id.Response))
                {
                    var infos = new List<NativityPropertyChangeInfo>();

                    foreach (Nativity nativity in nativityListView.SelectedItems)
                    {
                        var oldTags = new ObservableCollection<string>(nativity.Tags.ToArray());
                        var newTags = new ObservableCollection<string>(nativity.Tags.ToArray());
                        newTags.Add(id.Response);

                        nativity.Tags.Add(id.Response);

                        infos.Add(new NativityPropertyChangeInfo(nativity, oldTags, newTags));
                    }

                    if (nativityListView.SelectedItems.Count > 0)
                        Project.Actions.Add(new NativityEditedAction(infos, NativityProperty.Tags));
                }
            }
        }

        private void RemoveTagFromSelectionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            InputDialog id = new InputDialog() { FocusOnLoad = true };
            id.Title = "Remove Tag";
            id.ShowDialog();

            if (id.Successful)
            {
                if (!String.IsNullOrEmpty(id.Response) && !String.IsNullOrWhiteSpace(id.Response))
                {
                    var infos = new List<NativityPropertyChangeInfo>();

                    foreach (Nativity nativity in nativityListView.SelectedItems)
                    {
                        var oldTags = new ObservableCollection<string>(nativity.Tags.ToArray());
                        var newTags = new ObservableCollection<string>(nativity.Tags.ToArray());
                        newTags.Remove(id.Response);

                        nativity.Tags.Remove(id.Response);

                        infos.Add(new NativityPropertyChangeInfo(nativity, oldTags, newTags));
                    }

                    if (nativityListView.SelectedItems.Count > 0)
                        Project.Actions.Add(new NativityEditedAction(infos, NativityProperty.Tags));
                }
            }
        }

        private void AddGeographicalOriginToSelectionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            InputDialog id = new InputDialog() { FocusOnLoad = true };
            id.Title = "Add Geographical Origin";
            id.ShowDialog();

            if (id.Successful)
            {
                if (!String.IsNullOrEmpty(id.Response) && !String.IsNullOrWhiteSpace(id.Response))
                {
                    var infos = new List<NativityPropertyChangeInfo>();

                    foreach (Nativity nativity in nativityListView.SelectedItems)
                    {
                        var oldTags = new ObservableCollection<string>(nativity.GeographicalOrigins.ToArray());
                        var newTags = new ObservableCollection<string>(nativity.GeographicalOrigins.ToArray());
                        newTags.Add(id.Response);

                        nativity.GeographicalOrigins.Add(id.Response);

                        infos.Add(new NativityPropertyChangeInfo(nativity, oldTags, newTags));
                    }

                    if (nativityListView.SelectedItems.Count > 0)
                        Project.Actions.Add(new NativityEditedAction(infos, NativityProperty.GeographicalOrigins));
                }
            }
        }

        private void RemoveGeographicalOriginFromSelectionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            InputDialog id = new InputDialog() { FocusOnLoad = true };
            id.Title = "Remove Geographical Origin";
            id.ShowDialog();

            if (id.Successful)
            {
                if (!String.IsNullOrEmpty(id.Response) && !String.IsNullOrWhiteSpace(id.Response))
                {
                    var infos = new List<NativityPropertyChangeInfo>();

                    foreach (Nativity nativity in nativityListView.SelectedItems)
                    {
                        var oldTags = new ObservableCollection<string>(nativity.GeographicalOrigins.ToArray());
                        var newTags = new ObservableCollection<string>(nativity.GeographicalOrigins.ToArray());
                        newTags.Remove(id.Response);

                        nativity.GeographicalOrigins.Remove(id.Response);

                        infos.Add(new NativityPropertyChangeInfo(nativity, oldTags, newTags));
                    }

                    if (nativityListView.SelectedItems.Count > 0)
                        Project.Actions.Add(new NativityEditedAction(infos, NativityProperty.GeographicalOrigins));
                }
            }
        }

        private void uppercaseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            NamingOptionsDialog nod = new NamingOptionsDialog();
            nod.idOption.Visibility = Visibility.Collapsed;
            nod.acquiredOption.Visibility = Visibility.Collapsed;
            nod.costOption.Visibility = Visibility.Collapsed;
            nod.ShowDialog();

            if (nod.Successful)
            {
                NativitiesEditedAction action = new NativitiesEditedAction();
                action.Description = "Edited " + _nativityPlural(nativityListView.SelectedItems.Count) + " [Uppercase]";
                foreach (Nativity nativity in nativityListView.SelectedItems)
                {
                    NativityEditedInfo info = new NativityEditedInfo(nativity);
                    info.CopyCurrentValuesToOldValues();

                    if (nod.titleOption.IsChecked == true)
                        nativity.Title = nativity.Title.ToUpper();
                    if (nod.originOption.IsChecked == true)
                        nativity.Origin = nativity.Origin.ToUpper();
                    if (nod.fromOption.IsChecked == true)
                        nativity.From = nativity.From.ToUpper();
                    if (nod.locationOption.IsChecked == true)
                        nativity.Location = nativity.Location.ToUpper();
                    if (nod.tagsOption.IsChecked == true)
                    {
                        List<string> newTags = new List<string>();
                        foreach (var tag in nativity.Tags)
                            newTags.Add(tag.ToUpper());
                        nativity.Tags.Clear();
                        foreach (var tag in newTags)
                            nativity.Tags.Add(tag);
                    }
                    if (nod.geographicalOriginsOption.IsChecked == true)
                    {
                        List<string> newGeoOrigins = new List<string>();
                        foreach (var geoOrigin in nativity.GeographicalOrigins)
                            newGeoOrigins.Add(geoOrigin.ToUpper());
                        nativity.GeographicalOrigins.Clear();
                        foreach (var geoOrigin in newGeoOrigins)
                            nativity.GeographicalOrigins.Add(geoOrigin);
                    }

                    info.CopyCurrentValuesToNewValues();
                    action.Nativities.Add(info);
                }

                if (nativityListView.SelectedItems.Count > 0)
                    Project.Actions.Add(action);
            }
        }

        private void lowercaseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            NamingOptionsDialog nod = new NamingOptionsDialog();
            nod.idOption.Visibility = Visibility.Collapsed;
            nod.acquiredOption.Visibility = Visibility.Collapsed;
            nod.costOption.Visibility = Visibility.Collapsed;
            nod.ShowDialog();

            if (nod.Successful)
            {
                NativitiesEditedAction action = new NativitiesEditedAction();
                action.Description = "Edited " + _nativityPlural(nativityListView.SelectedItems.Count) + " [Lowercase]";
                foreach (Nativity nativity in nativityListView.SelectedItems)
                {
                    NativityEditedInfo info = new NativityEditedInfo(nativity);
                    info.CopyCurrentValuesToOldValues();

                    if (nod.titleOption.IsChecked == true)
                        nativity.Title = nativity.Title.ToLower();
                    if (nod.originOption.IsChecked == true)
                        nativity.Origin = nativity.Origin.ToLower();
                    if (nod.fromOption.IsChecked == true)
                        nativity.From = nativity.From.ToLower();
                    if (nod.locationOption.IsChecked == true)
                        nativity.Location = nativity.Location.ToLower();
                    if (nod.tagsOption.IsChecked == true)
                    {
                        List<string> newTags = new List<string>();
                        foreach (var tag in nativity.Tags)
                            newTags.Add(tag.ToLower());
                        nativity.Tags.Clear();
                        foreach (var tag in newTags)
                            nativity.Tags.Add(tag);
                    }
                    if (nod.geographicalOriginsOption.IsChecked == true)
                    {
                        List<string> newGeoOrigins = new List<string>();
                        foreach (var geoOrigin in nativity.GeographicalOrigins)
                            newGeoOrigins.Add(geoOrigin.ToLower());
                        nativity.GeographicalOrigins.Clear();
                        foreach (var geoOrigin in newGeoOrigins)
                            nativity.GeographicalOrigins.Add(geoOrigin);
                    }

                    info.CopyCurrentValuesToNewValues();
                    action.Nativities.Add(info);
                }

                if (nativityListView.SelectedItems.Count > 0)
                    Project.Actions.Add(action);
            }
        }

        private void fixCapitalizationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            NamingOptionsDialog nod = new NamingOptionsDialog();
            nod.idOption.Visibility = Visibility.Collapsed;
            nod.acquiredOption.Visibility = Visibility.Collapsed;
            nod.costOption.Visibility = Visibility.Collapsed;
            nod.ShowDialog();

            if (nod.Successful)
            {
                NativitiesEditedAction action = new NativitiesEditedAction();
                action.Description = "Edited " + _nativityPlural(nativityListView.SelectedItems.Count) + " [Fix Capitalization]";
                foreach (Nativity nativity in nativityListView.SelectedItems)
                {
                    NativityEditedInfo info = new NativityEditedInfo(nativity);
                    info.CopyCurrentValuesToOldValues();

                    if (nod.titleOption.IsChecked == true)
                        nativity.Title = capitalize(nativity.Title);
                    if (nod.originOption.IsChecked == true)
                        nativity.Origin = capitalize(nativity.Origin);
                    if (nod.fromOption.IsChecked == true)
                        nativity.From = capitalize(nativity.From);
                    if (nod.locationOption.IsChecked == true)
                        nativity.Location = capitalize(nativity.Location);
                    if (nod.tagsOption.IsChecked == true)
                    {
                        List<string> newTags = new List<string>();
                        foreach (var tag in nativity.Tags)
                            newTags.Add(capitalize(tag));
                        nativity.Tags.Clear();
                        foreach (var tag in newTags)
                            nativity.Tags.Add(tag);
                    }
                    if (nod.geographicalOriginsOption.IsChecked == true)
                    {
                        List<string> newGeoOrigins = new List<string>();
                        foreach (var geoOrigin in nativity.GeographicalOrigins)
                            newGeoOrigins.Add(capitalize(geoOrigin));
                        nativity.GeographicalOrigins.Clear();
                        foreach (var geoOrigin in newGeoOrigins)
                            nativity.GeographicalOrigins.Add(geoOrigin);
                    }

                    info.CopyCurrentValuesToNewValues();
                    action.Nativities.Add(info);
                }

                if (nativityListView.SelectedItems.Count > 0)
                    Project.Actions.Add(action);
            }
        }

        private string capitalize(string value)
        {
            string[] words = value.Split(' ');
            List<string> newWords = new List<string>();
            foreach (string word in words)
            {
                string build = "";
                int index = 0;
                foreach (char c in word)
                {
                    if (index == 0)
                        build = build + c.ToString().ToUpper();
                    else
                        build = build + c.ToString().ToLower();
                    index++;
                }
                newWords.Add(build);
            }
            return String.Join(" ", newWords);
        }

        private void NamingReplaceTextMenuItemClick(object sender, RoutedEventArgs e)
        {
            NamingOptionsDialog nod = new NamingOptionsDialog();
            nod.idOption.Visibility = Visibility.Collapsed;
            nod.acquiredOption.Visibility = Visibility.Collapsed;
            nod.costOption.Visibility = Visibility.Collapsed;
            nod.ShowDialog();
            if (nod.Successful)
            {
                DoubleInputDialog input = new DoubleInputDialog();
                input.Title = "Replace Text";
                input.ShowDialog();
                if (input.Successful)
                {
                    NativitiesEditedAction action = new NativitiesEditedAction();
                    action.Description = "Edited " + _nativityPlural(nativityListView.SelectedItems.Count) + " [Replace Text]";
                    foreach (Nativity nativity in nativityListView.SelectedItems)
                    {
                        NativityEditedInfo info = new NativityEditedInfo(nativity);
                        info.CopyCurrentValuesToOldValues();

                        if (nod.titleOption.IsChecked == true)
                            nativity.Title = nativity.Title.Replace(input.Response1, input.Response2);
                        if (nod.originOption.IsChecked == true)
                            nativity.Origin = nativity.Origin.Replace(input.Response1, input.Response2);
                        if (nod.fromOption.IsChecked == true)
                            nativity.From = nativity.From.Replace(input.Response1, input.Response2);
                        if (nod.locationOption.IsChecked == true)
                            nativity.Location = nativity.Location.Replace(input.Response1, input.Response2);
                        if (nod.tagsOption.IsChecked == true)
                        {
                            List<string> newTags = new List<string>();
                            foreach (var tag in nativity.Tags)
                                newTags.Add(tag.Replace(input.Response1, input.Response2));
                            nativity.Tags.Clear();
                            foreach (var tag in newTags)
                                nativity.Tags.Add(tag);
                        }
                        if (nod.geographicalOriginsOption.IsChecked == true)
                        {
                            List<string> newGeoOrigins = new List<string>();
                            foreach (var geoOrigin in nativity.GeographicalOrigins)
                                newGeoOrigins.Add(geoOrigin.Replace(input.Response1, input.Response2));
                            nativity.GeographicalOrigins.Clear();
                            foreach (var geoOrigin in newGeoOrigins)
                                nativity.GeographicalOrigins.Add(geoOrigin);
                        }

                        info.CopyCurrentValuesToNewValues();
                        action.Nativities.Add(info);
                    }

                    if (nativityListView.SelectedItems.Count > 0)
                        Project.Actions.Add(action);
                }
            }
        }

        private void NamingReplaceTextRegexMenuItem_Click(object sender, RoutedEventArgs e)
        {
            NamingOptionsDialog nod = new NamingOptionsDialog();
            nod.idOption.Visibility = Visibility.Collapsed;
            nod.acquiredOption.Visibility = Visibility.Collapsed;
            nod.costOption.Visibility = Visibility.Collapsed;
            nod.ShowDialog();

            if (nod.Successful)
                DoNamingRegex(nod);
        }

        private void DoNamingRegex(NamingOptionsDialog nod)
        {
            DoubleInputDialog input = new DoubleInputDialog();
            input.Title = "Replace Text (Regex)";
            input.ShowDialog();
            if (input.Successful)
            {
                try
                {
                    NativitiesEditedAction action = new NativitiesEditedAction();
                    action.Description = "Edited " + _nativityPlural(nativityListView.SelectedItems.Count) + " [Replace Text (Regex)]";
                    foreach (Nativity nativity in nativityListView.SelectedItems)
                    {
                        NativityEditedInfo info = new NativityEditedInfo(nativity);
                        info.CopyCurrentValuesToOldValues();

                        if (nod.titleOption.IsChecked == true)
                            nativity.Title = Regex.Replace(nativity.Title, input.Response1, input.Response2);
                        if (nod.originOption.IsChecked == true)
                            nativity.Origin = Regex.Replace(nativity.Origin, input.Response1, input.Response2);
                        if (nod.fromOption.IsChecked == true)
                            nativity.From = Regex.Replace(nativity.From, input.Response1, input.Response2);
                        if (nod.locationOption.IsChecked == true)
                            nativity.Location = Regex.Replace (nativity.Location, input.Response1, input.Response2);
                        if (nod.tagsOption.IsChecked == true)
                        {
                            List<string> newTags = new List<string>();
                            foreach (var tag in nativity.Tags)
                                newTags.Add(Regex.Replace(tag, input.Response1, input.Response2));
                            nativity.Tags.Clear();
                            foreach (var tag in newTags)
                                nativity.Tags.Add(tag);
                        }
                        if (nod.geographicalOriginsOption.IsChecked == true)
                        {
                            List<string> newGeoOrigins = new List<string>();
                            foreach (var geoOrigin in nativity.GeographicalOrigins)
                                newGeoOrigins.Add(Regex.Replace(geoOrigin, input.Response1, input.Response2));
                            nativity.GeographicalOrigins.Clear();
                            foreach (var geoOrigin in newGeoOrigins)
                                nativity.GeographicalOrigins.Add(geoOrigin);
                        }

                        info.CopyCurrentValuesToNewValues();
                        action.Nativities.Add(info);
                    }

                    if (nativityListView.SelectedItems.Count > 0)
                        Project.Actions.Add(action);
                }

                catch
                {
                    MessageBox.Show("The regex pattern used is incorrect.");
                    DoNamingRegex(nod);
                }
            }
        }

        private string _nativityPlural(int count)
        {
            if (count < 1 || count > 1)
                return "Nativities";
            else
                return "Nativity";
        }

        private void PreferencesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            PreferencesWindow pw = new PreferencesWindow();
            pw.ShowDialog();
        }

        #endregion


        #region Nativities

        private void AddNativityMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AddNativity an = new AddNativity();
            an.ShowDialog();

            if (an.Successful)
            {
                Project.Nativities.Add(an.Nativity);
                Project.Actions.Add(new NativityAddedAction(Project, new List<Nativity>() { an.Nativity }));
                RefreshSort();
                _ = ShowNativityAsync(an.Nativity);
            }
        }

        private async Task ShowNativityAsync(Nativity nativity)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.25));
            await Dispatcher.BeginInvoke(DispatcherPriority.Input,
            new Action(delegate () {
                if (nativity != null)
                {
                    nativityListView.ScrollIntoView(nativity);
                    nativityListView.UnselectAll();
                    nativity.IsSelected = true;
                }
            }));
        }

        private void RemoveNativityMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbr = MessageBox.Show("Are you sure you want to remove the selected nativities?", "Remove Nativity", MessageBoxButton.YesNo);

            if (mbr == MessageBoxResult.Yes)
            {
                List<Nativity> nativities = new List<Nativity>(nativityListView.SelectedItems.Cast<Nativity>().ToList());

                foreach (Nativity nativity in nativities)
                {
                    Project.Nativities.Remove(nativity);
                }

                Project.Actions.Add(new NativityRemovedAction(Project, nativities));

                RefreshSort();
            }
        }

        private void SelectAllMenuItemOnClick(object sender, RoutedEventArgs e)
        {
            nativityListView.SelectAll();
        }

        private void SelectNoneMenuItemOnClick(object sender, RoutedEventArgs e)
        {
            nativityListView.SelectedItems.Clear();
            nativityListView.SelectionMode = System.Windows.Controls.SelectionMode.Single;
            nativityListView.SelectionMode = System.Windows.Controls.SelectionMode.Extended;
        }

        private void SelectInvertMenuItemOnClick(object sender, RoutedEventArgs e)
        {
            foreach (Nativity nativity in nativityListView.Items)
            {
                nativity.IsSelected = !nativity.IsSelected;
            }
        }

        #endregion


        #region Search

        private void FindMenuItem_Click(object sender, RoutedEventArgs e)
        {
            FindDialog fd = new FindDialog();
            fd.Closed += (_sender, _e) =>
            {
                findMenuItem.IsEnabled = true;
            };

            findMenuItem.IsEnabled = false;
            fd.Show();

        }

        private void GoToMenuItem_Click(object sender, RoutedEventArgs e)
        {
            InputDialog id = new InputDialog() {FocusOnLoad = true};
            id.Title = "Go To";
            id.ShowDialog();

            if (id.Successful)
            {
                int response = 0;
                if (int.TryParse(id.Response, out response))
                {
                    Nativity target = Project.Nativities.Cast<Nativity>().Where(a => a.Id == response).FirstOrDefault();
                    if (target == null)
                    {
                        int attempts = 0;
                        do
                        {
                            attempts++;
                            target = Project.Nativities.Cast<Nativity>().Where(a => a.Id == (response - attempts)).FirstOrDefault();
                        }

                        while (target == null && attempts <= 15);
                    }

                    if (target != null)
                    {
                        int index = Project.Nativities.IndexOf(target);
                        nativityListView.UnselectAll();
                        target.IsSelected = true;
                        nativityListView.ScrollIntoView(Project.Nativities[Project.Nativities.Count - 1]);
                        nativityListView.ScrollIntoView(Project.Nativities[index]);
                    }
                }
            }
        }

        private void FindMissingNativityIdsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            List<string> ids = new List<string>();
            List<Nativity> nativities = new List<Nativity>();

            foreach (Nativity nativity in Project.Nativities)
            {
                nativities.Add(nativity);
            }

            for (int i = 1; i <= ((Nativity)Project.Nativities.Last()).Id; i++)
            {
                if (nativities.Where(o => o.Id == i).LastOrDefault() == null)
                {
                    ids.Add(i.ToString());
                }
            }

            MessageBox.Show("There are no nativities with these Ids: " + string.Join(", ", ids));
        }

        private void FindNativitiesWithoutImagesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            List<string> ids = new List<string>();

            foreach (Nativity n in Project.Nativities)
            {
                if (!n.HasImage)
                {
                    ids.Add(n.Id.ToString());
                }
            }

            MessageBox.Show("These nativities do not have images: " + string.Join(", ", ids) + Environment.NewLine +
                            Environment.NewLine +
                            "With Images: " + (Project.Nativities.Count - ids.Count).ToString() + " nativity(ies)" + Environment.NewLine +
                            "Without Images: " + ids.Count + " nativity(ies)");
        }

        private void HighlightToolMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion


        #region View
        #endregion


        #region Plugins

        private void managePluginsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ManagePluginsWindow mpw = new ManagePluginsWindow();
            mpw.ShowDialog();
        }

        #endregion


        #region Help

        private void Changelogs_Click(object sender, RoutedEventArgs e)
        {
            ChangelogsWindow cw = new ChangelogsWindow();
            cw.ShowDialog();
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            About_Box ab = new About_Box();
            ab.ShowDialog();
        }

        #endregion


        #endregion


        #region Nativity Context Menus

        private void addImageSingleNativityContextMenu_Click(object sender, RoutedEventArgs e)
        {
            var nativity = nativityListView.SelectedItem as Nativity;
            if (nativity != null)
            {
                if (nativity.HasImage)
                    MessageBox.Show("This nativity already has an image. Please remove it, then try adding an image.", "Conway Nativity Directory",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    nativity.Information.ChooseImageButton_Click(sender, e);
            }
        }

        private void removeImageSingleNativityContextMenu_Click(object sender, RoutedEventArgs e)
        {
            RemoveImagesMenuItem_Click(sender, e);
            var nativity = nativityListView.SelectedItem as Nativity;
            if (nativity != null)
                nativity.Information.UnloadImage();
        }

        #endregion


        #region Searching

        private void SearchTextBox_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Project.SetSearchFilter(new TextBoxFilter(searchTextBox));
                Project.Search();
            }
        }

        private void ClearSeachButton_Click(object sender, RoutedEventArgs e)
        {
            Project.SetSearchFilter(new TextBoxFilter(searchTextBox));
            searchTextBox.Text = "";
            Project.Search();
            AdvancedSearchIsActive = false;
        }

        #endregion


        #region Nativities

        List<Nativity> nativityCache = new List<Nativity>();

        private void NativityListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (nativityListView.Items.Count > 0)
            {
                if (nativityListView.SelectedItem != null)
                {
                    NativityInformation ni = (nativityListView.SelectedItems[0] as Nativity).Information;
                    ni.LoadImage();
                    informationContent.Content = ni;

                    if (AdvancedSearchIsActive)
                    {
                        if (AdvancedSearch.highlightCheckBox.IsChecked == true)
                        {
                            foreach (SearchTerm searchTerm in AdvancedSearch.termsListView.Items)
                            {
                                if (searchTerm.IsEnabled)
                                {
                                    System.Globalization.CompareOptions compareOptions = System.Globalization.CompareOptions.None;
                                    if (!searchTerm.CaseSensitive)
                                    {
                                        compareOptions = System.Globalization.CompareOptions.IgnoreCase;
                                    }

                                    ni.HighlightKeyword(searchTerm.Brush, searchTerm.Text, compareOptions);
                                }
                            }
                        }
                    }
                }

                else
                {
                    informationContent.Content = null;
                }

                if (LastSelectedNativity != null)
                {
                    LastSelectedNativity.Information.EndDescriptionEdit();
                    LastSelectedNativity.UnloadImage();

                    if (AdvancedSearchIsActive)
                    {
                        if (AdvancedSearch.highlightCheckBox.IsChecked == true)
                        {
                            LastSelectedNativity.Information.ClearHighlights();
                        }
                    }
                }

                if (nativityListView.SelectedItem != null)
                {
                    //Set last selected nativity. Do anything with last selected nativity before this line of code,
                    //otherwise you will be using the currently selected nativity.
                    LastSelectedNativity = nativityListView.SelectedItems[0] as Nativity;

                    //also add it to the nativity cache.
                    if (!nativityCache.Contains(nativityListView.SelectedItem as Nativity))
                        nativityCache.Add(nativityListView.SelectedItem as Nativity);

                    //the software will run out of memory so it must be restarted.
                    if (nativityCache.Count > 350)
                    {
                        MessageBox.Show("Too many nativities have been loaded into the memory cache." +
                            " To prevent an error from occuring, Conway Nativity Directory must restart to clear the cache.",
                            "Conway Nativity Directory", MessageBoxButton.OK, MessageBoxImage.Warning);

                        Project.AutoSave(true);

                        //Notify the next process that the software closed abruptly.
                        //Provide information about where to open the last project.

                        string contents = nativityListView.SelectedIndex.ToString() + Environment.NewLine +
                            Project.GetLatestAutoSave().Value.FileName;

                        if (System.IO.File.Exists(Project.FileName))
                            contents = contents + Environment.NewLine +
                                Project.FileName;


                        System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory +
                            @"\abrupt.txt", contents);

                        System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                        AbruptlyClosing = true;
                        Application.Current.Shutdown();
                    }
                }
            }

            MultipleNativitiesSelected = nativityListView.SelectedItems.Count > 1;
        }

        private void columnTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            var _sender = (FrameworkElement)sender;
            var nativity = (Nativity)_sender.Tag;

            if (_sender.Name == "idTextBox")
                NativityColumnElements.SetIdColumn(nativity, _sender);
            else if (_sender.Name == "titleTextBox")
                NativityColumnElements.SetTitleColumn(nativity, _sender);
            else if (_sender.Name == "originTextBox")
                NativityColumnElements.SetOriginColumn(nativity, _sender);
            else if (_sender.Name == "acquiredTextBox")
                NativityColumnElements.SetAcquiredColumn(nativity, _sender);
            else if (_sender.Name == "fromTextBox")
                NativityColumnElements.SetFromColumn(nativity, _sender);
            else if (_sender.Name == "costTextBox")
                NativityColumnElements.SetCostColumn(nativity, _sender);
            else if (_sender.Name == "locationTextBox")
                NativityColumnElements.SetLocationColumn(nativity, _sender);
            else if (_sender.Name == "tagsTagEditor")
                NativityColumnElements.SetTagsColumn(nativity, _sender);
            else if (_sender.Name == "geographicalOriginsTagEditor")
                NativityColumnElements.SetGeographicalOriginsColumn(nativity, _sender);
        }

        #endregion


        #region Nativity Undo/Redo


        private void nativityTextBox_UserStoppedTyping(object sender, UserStoppedTypingEventArgs e)
        {
            NativityProperty property = ParseNativityPropertyFromTextBoxName((sender as TextBox).Name);
            var info = new NativityPropertyChangeInfo((sender as TextBox).Tag as Nativity, e.OldText, e.NewText);

            NativityEditedAction action = new NativityEditedAction(new NativityPropertyChangeInfo[] { info },
                property);
            Project.Actions.Add(action);
        }

        private void T_TagEditor_UserChangedTags(object sender, TagsChangedEventArgs e)
        {
            var oldTags = new ObservableCollection<string>(e.OldTags);
            var newTags = new ObservableCollection<string>(e.NewTags);
            var info = new NativityPropertyChangeInfo((sender as TagEditor).Tag as Nativity, oldTags, newTags);

            NativityEditedAction action = new NativityEditedAction(new NativityPropertyChangeInfo[] { info },
                NativityProperty.Tags);
            Project.Actions.Add(action);
        }

        private void GO_TagEditor_UserChangedTags(object sender, TagsChangedEventArgs e)
        {
            var oldTags = new ObservableCollection<string>(e.OldTags);
            var newTags = new ObservableCollection<string>(e.NewTags);
            var info = new NativityPropertyChangeInfo((sender as TagEditor).Tag as Nativity, oldTags, newTags);

            NativityEditedAction action = new NativityEditedAction(new NativityPropertyChangeInfo[] { info },
                NativityProperty.GeographicalOrigins);
            Project.Actions.Add(action);
        }

        private NativityProperty ParseNativityPropertyFromTextBoxName(string textBoxName)
        {
            if (textBoxName == "idTextBox")
                return NativityProperty.Id;
            else if (textBoxName == "titleTextBox")
                return NativityProperty.Title;
            else if (textBoxName == "originTextBox")
                return NativityProperty.Origin;
            else if (textBoxName == "acquiredTextBox")
                return NativityProperty.Acquired;
            else if (textBoxName == "fromTextBox")
                return NativityProperty.From;
            else if (textBoxName == "costTextBox")
                return NativityProperty.Cost;
            else
                return NativityProperty.Location;
        }

        private object GetNativityValue(NativityProperty property, Nativity nativity)
        {
            if (property == NativityProperty.Id)
                return nativity.Id;
            else if (property == NativityProperty.Title)
                return nativity.Title;
            else if (property == NativityProperty.Origin)
                return nativity.Origin;
            else if (property == NativityProperty.Acquired)
                return nativity.Acquired;
            else if (property == NativityProperty.From)
                return nativity.From;
            else if (property == NativityProperty.Cost)
                return nativity.Cost;
            else
                return nativity.Location;
        }


        #endregion


        #endregion


        #region Other

        private bool AskToExit()
        {
            if (Project.IsOpen)
            {
                MessageBoxResult saveMbr = MessageBox.Show("Would you like to save before exiting?",
                    "Conway Nativity Directory", MessageBoxButton.YesNo);

                if (saveMbr == MessageBoxResult.Yes)
                {
                    if (System.IO.File.Exists(project.FileName))
                        Project.Save(true);
                    else
                    {
                        MessageBox.Show("It seems like the current project has never been saved. Please choose a location to save the project.");

                        SaveFileDialog sfd = new SaveFileDialog();
                        sfd.Filter = "Conway Nativity Directory Project (*.cnp)|*.cnp|Conway Nativity Directory Backup (*.cnb)|*.cnb";

                        if (sfd.ShowDialog() == true)
                            Project.SaveAs(sfd.FileName, true);
                        else
                            MessageBox.Show("A location was not choosen, and the project was not saved.");
                    }
                }

                else
                {
                    MessageBoxResult mbr = MessageBox.Show("Are you sure you would like to exit? Any unsaved changes will be lost.",
                            "Conway Nativity Directory", MessageBoxButton.YesNo);

                    if (mbr == MessageBoxResult.No)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void CheckForRecentAutoSave()
        {
            if (Project.IsOpen)
            {
                var lastestAutoSave = Project.GetLatestAutoSave();

                if (lastestAutoSave != null)
                {
                    DateTime d1 = new System.IO.FileInfo(Project.FileName).CreationTime;
                    DateTime d2 = lastestAutoSave.Value.Created;

                    if (d2 > d1)
                    {
                        MessageBoxResult mbr = MessageBox.Show("An auto-saved file was found. It was saved after the currently opened project. " +
                            "Would you like to open it?", "Conway Nativity Directory", MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (mbr == MessageBoxResult.Yes)
                        {
                            string oldFileName = Project.FileName;
                            Project.Close();
                            Project.Open(lastestAutoSave.Value.FileName, oldFileName, true);
                        }
                    }
                }
            }
        }

        private void SetBindings()
        {
            //Sorting modes
            App.SetBinding("SortingMode", this, idArrow, ColumnHeaderArrow.SortingModeProperty);
            App.SetBinding("SortingMode", this, titleArrow, ColumnHeaderArrow.SortingModeProperty);
            App.SetBinding("SortingMode", this, originArrow, ColumnHeaderArrow.SortingModeProperty);
            App.SetBinding("SortingMode", this, acquiredArrow, ColumnHeaderArrow.SortingModeProperty);
            App.SetBinding("SortingMode", this, fromArrow, ColumnHeaderArrow.SortingModeProperty);
            App.SetBinding("SortingMode", this, costArrow, ColumnHeaderArrow.SortingModeProperty);
            App.SetBinding("SortingMode", this, locationArrow, ColumnHeaderArrow.SortingModeProperty);
            App.SetBinding("SortingMode", this, tagsArrow, ColumnHeaderArrow.SortingModeProperty);
            App.SetBinding("SortingMode", this, geographicalOriginsArrow, ColumnHeaderArrow.SortingModeProperty);
            
            App.SetBinding("SortingType", this, idArrow, ColumnHeaderArrow.VisibilityProperty,
                new SortingTypeToVisibilityConverter() { SortingType = SortingType.Id });
            App.SetBinding("SortingType", this, titleArrow, ColumnHeaderArrow.VisibilityProperty,
                new SortingTypeToVisibilityConverter() { SortingType = SortingType.Title });
            App.SetBinding("SortingType", this, originArrow, ColumnHeaderArrow.VisibilityProperty,
                new SortingTypeToVisibilityConverter() { SortingType = SortingType.Origin });
            App.SetBinding("SortingType", this, acquiredArrow, ColumnHeaderArrow.VisibilityProperty,
                new SortingTypeToVisibilityConverter() { SortingType = SortingType.Acquired });
            App.SetBinding("SortingType", this, fromArrow, ColumnHeaderArrow.VisibilityProperty,
                new SortingTypeToVisibilityConverter() { SortingType = SortingType.From });
            App.SetBinding("SortingType", this, costArrow, ColumnHeaderArrow.VisibilityProperty,
                new SortingTypeToVisibilityConverter() { SortingType = SortingType.Cost });
            App.SetBinding("SortingType", this, locationArrow, ColumnHeaderArrow.VisibilityProperty,
                new SortingTypeToVisibilityConverter() { SortingType = SortingType.Location });
            App.SetBinding("SortingType", this, tagsArrow, ColumnHeaderArrow.VisibilityProperty,
                new SortingTypeToVisibilityConverter() { SortingType = SortingType.Tags });
            App.SetBinding("SortingType", this, geographicalOriginsArrow, ColumnHeaderArrow.VisibilityProperty,
                new SortingTypeToVisibilityConverter() { SortingType = SortingType.GeographicalOrigins });


            //Views
            ColumnsPreference preference = (ColumnsPreference)Preferences.GetPreference(@"Interface\Columns");

            App.SetBinding("ShowId", preference, viewIdColumnMenuItem, MenuItem.IsCheckedProperty,
                BindingMode.TwoWay, UpdateSourceTrigger.PropertyChanged);
            App.SetBinding("ShowTitle", preference, viewTitleColumnMenuItem, MenuItem.IsCheckedProperty,
                BindingMode.TwoWay, UpdateSourceTrigger.PropertyChanged);
            App.SetBinding("ShowOrigin", preference, viewOriginColumnMenuItem, MenuItem.IsCheckedProperty,
                BindingMode.TwoWay, UpdateSourceTrigger.PropertyChanged);
            App.SetBinding("ShowAcquired", preference, viewAcquiredColumnMenuItem, MenuItem.IsCheckedProperty,
                BindingMode.TwoWay, UpdateSourceTrigger.PropertyChanged);
            App.SetBinding("ShowFrom", preference, viewFromColumnMenuItem, MenuItem.IsCheckedProperty,
                BindingMode.TwoWay, UpdateSourceTrigger.PropertyChanged);
            App.SetBinding("ShowCost", preference, viewCostColumnMenuItem, MenuItem.IsCheckedProperty,
                BindingMode.TwoWay, UpdateSourceTrigger.PropertyChanged);
            App.SetBinding("ShowLocation", preference, viewLocationColumnMenuItem, MenuItem.IsCheckedProperty,
                BindingMode.TwoWay, UpdateSourceTrigger.PropertyChanged);
            App.SetBinding("ShowTags", preference, viewTagsColumnMenuItem, MenuItem.IsCheckedProperty,
                BindingMode.TwoWay, UpdateSourceTrigger.PropertyChanged);
            App.SetBinding("ShowGeographicalOrigins", preference, viewGeographicalOriginsColumnMenuItem, MenuItem.IsCheckedProperty,
                BindingMode.TwoWay, UpdateSourceTrigger.PropertyChanged);

            //Login
            var loginSettings = (LoginSettingsPreference) Preferences.GetPreference(@"Security\Login Settings");



            App.SetBinding("IsLoggedIn", loginSettings, loginMenuItem, MenuItem.VisibilityProperty,
                BindingMode.OneWay, UpdateSourceTrigger.Default,
                new BooleanToVisibilityConverter() {Invert = true});
            App.SetBinding("IsPasswordProtected", loginSettings, loginMenuItem, MenuItem.IsEnabledProperty,
                BindingMode.OneWay, UpdateSourceTrigger.Default);
            App.SetBinding("IsLoggedIn", loginSettings, logoutMenuItem, MenuItem.VisibilityProperty,
                BindingMode.OneWay, UpdateSourceTrigger.Default, new BooleanToVisibilityConverter());
            App.SetBinding("AllowEditing", loginSettings, this, MainWindow.AllowEditingTimeStampProperty,
                new AllowEditingTimeStampConverter()
                    {Mode = AllowEditingTimeStampConverter.ConversionMode.ConvertToTimestamp});
        }


        #endregion


        #endregion


        #region Hiding Columns

        private void ViewColumnMenuItem_Checked(object sender, RoutedEventArgs e)
        {
            if (sender == viewIdColumnMenuItem)
            {
                idGridViewColumn.Width = double.NaN;
                IdColumn.IsEnabled = true;
            }

            else if (sender == viewTitleColumnMenuItem)
            {
                titleGridViewColumn.Width = double.NaN;
                titleColumn.IsEnabled = true;
            }

            else if (sender == viewOriginColumnMenuItem)
            {
                originGridViewColumn.Width = double.NaN;
                originColumn.IsEnabled = true;
            }

            else if (sender == viewAcquiredColumnMenuItem)
            {
                acquiredGridViewColumn.Width = double.NaN;
                acquiredColumn.IsEnabled = true;
            }

            else if (sender == viewFromColumnMenuItem)
            {
                fromGridViewColumn.Width = double.NaN;
                fromColumn.IsEnabled = true;
            }

            else if (sender == viewCostColumnMenuItem)
            {
                costGridViewColumn.Width = double.NaN;
                costColumn.IsEnabled = true;
            }

            else if (sender == viewLocationColumnMenuItem)
            {
                locationGridViewColumn.Width = double.NaN;
                locationColumn.IsEnabled = true;
            }

            else if (sender == viewTagsColumnMenuItem)
            {
                tagsGridViewColumn.Width = double.NaN;
                tagsColumn.IsEnabled = true;
            }

            else if (sender == viewGeographicalOriginsColumnMenuItem)
            {
                geographicalOriginsGridViewColumn.Width = double.NaN;
                geographicalOriginsColumn.IsEnabled = true;
            }
        }

        private void ViewColumnMenuItem_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender == viewIdColumnMenuItem)
            {
                idGridViewColumn.Width = 0;
                IdColumn.IsEnabled = false;
            }

            else if (sender == viewTitleColumnMenuItem)
            {
                titleGridViewColumn.Width = 0;
                titleColumn.IsEnabled = false;
            }

            else if (sender == viewOriginColumnMenuItem)
            {
                originGridViewColumn.Width = 0;
                originColumn.IsEnabled = false;
            }

            else if (sender == viewAcquiredColumnMenuItem)
            {
                acquiredGridViewColumn.Width = 0;
                acquiredColumn.IsEnabled = false;
            }

            else if (sender == viewFromColumnMenuItem)
            {
                fromGridViewColumn.Width = 0;
                fromColumn.IsEnabled = false;
            }

            else if (sender == viewCostColumnMenuItem)
            {
                costGridViewColumn.Width = 0;
                costColumn.IsEnabled = false;
            }

            else if (sender == viewLocationColumnMenuItem)
            {
                locationGridViewColumn.Width = 0;
                locationColumn.IsEnabled = false;
            }

            else if (sender == viewTagsColumnMenuItem)
            {
                tagsGridViewColumn.Width = 0;
                tagsColumn.IsEnabled = false;
            }

            else if (sender == viewGeographicalOriginsColumnMenuItem)
            {
                geographicalOriginsGridViewColumn.Width = 0;
                geographicalOriginsColumn.IsEnabled = false;
            }
        }

        private void ViewColumnMenuItemInvokeCheckedUnchecked()
        {
            MenuItem[] menuItems = new MenuItem[] {
                viewIdColumnMenuItem,
                viewTitleColumnMenuItem,
                viewOriginColumnMenuItem,
                viewAcquiredColumnMenuItem,
                viewFromColumnMenuItem,
                viewCostColumnMenuItem,
                viewLocationColumnMenuItem,
                viewTagsColumnMenuItem,
                viewGeographicalOriginsColumnMenuItem
            };

            foreach (MenuItem item in menuItems)
            {
                if (item.IsChecked)
                    ViewColumnMenuItem_Checked(item, new RoutedEventArgs());
                else
                    ViewColumnMenuItem_Unchecked(item, new RoutedEventArgs());
            }
        }

        #endregion


        #region Focusing ListView Elements

        private object LastClickedObject;
        private void MainWindow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source != nativityListView)
            {
                if (LastClickedObject != null)
                {
                    if (LastClickedObject != e.Source)
                    {
                        if (LastClickedObject is TagEditor tagEditor)
                        {
                            tagEditor.EndEdit();
                        }
                    }
                }

                LastClickedObject = e.Source;
            }
        }

        #endregion


        #region Advanced Search

        public AdvancedSearch AdvancedSearch { get; set; }


        private static readonly DependencyPropertyKey AdvancedSearchIsActivePropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(AdvancedSearchIsActive), typeof(bool), typeof(MainWindow),
                new PropertyMetadata(false, AdvancedSearchIsActiveCallback));

        public static readonly DependencyProperty AdvancedSearchIsActiveProperty =
            AdvancedSearchIsActivePropertyKey.DependencyProperty;

        public bool AdvancedSearchIsActive
        {
            get { return (bool)GetValue(AdvancedSearchIsActiveProperty); }
            protected set { SetValue(AdvancedSearchIsActivePropertyKey, value); }
        }

        private static void AdvancedSearchIsActiveCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MainWindow mainWindow = App.MainWindow;

            if ((bool)e.NewValue)
            {
                mainWindow.AdvancedSearch = new AdvancedSearch(mainWindow);
                mainWindow.AdvancedSearch.Closed += AdvancedSearch_Closed;
                mainWindow.AdvancedSearch.Topmost = true;
                mainWindow.AdvancedSearch.Show();
            }

            else
            {
                if (mainWindow.AdvancedSearch != null)
                {
                    mainWindow.AdvancedSearch.Close();
                    mainWindow.AdvancedSearch = null;

                    mainWindow.Project.SetSearchFilter(new TextBoxFilter(mainWindow.searchTextBox));
                    mainWindow.searchTextBox.Text = "";
                    mainWindow.Project.Search();
                }
            }
        }


        private void AdvancedSearchMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AdvancedSearchIsActive = !AdvancedSearchIsActive;
        }

        private void AdvancedSearch_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (AdvancedSearch != null)
            {
                AdvancedSearch.Show();
            }
        }

        private static void AdvancedSearch_Closed(object sender, EventArgs e)
        {
            MainWindow mainWindow = App.MainWindow;
            mainWindow.AdvancedSearchIsActive = false;
        }


        #endregion


        #region Commands

        internal bool CanExecuteCommandWithCurrentLogin
        {
            get
            {
                if (!LoginSettings.IsPasswordProtected)
                    return true;
                else
                    return LoginSettings.IsLoggedIn;
            }
        }

        private void CommandsPrompt(object sender, ExecutedRoutedEventArgs e)
        {
            CommandConsoleWindow ccw = new CommandConsoleWindow();
            ccw.Show();
        }


        #endregion


    }

    public class AllowEditingTimeStampConverter : IValueConverter
    {
        public enum ConversionMode { ConvertToBool, ConvertToTimestamp }

        public ConversionMode Mode { get; set; }

        public AllowEditingTimeStampConverter()
        {
            Mode = ConversionMode.ConvertToBool;
        }

        public object Convert(object value, Type targetType, object target,
            System.Globalization.CultureInfo culture)
        {
            if (Mode == ConversionMode.ConvertToBool)
                return App.MainWindow.AllowEditing;

            else
                return DateTime.Now;
        }

        public object ConvertBack(object value, Type targetType, object target,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("Cannot convert back.");
        }
    }

    public class BooleanInverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object target, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object target, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }
    }

    public class SortingTypeToVisibilityConverter : IValueConverter
    {
        public SortingType SortingType { get; set; }

        public object Convert(object value, Type targetType, object target, System.Globalization.CultureInfo culture)
        {
            SortingType sortingType = (SortingType)value;

            if (SortingType == sortingType)
                return Visibility.Visible;
            else
                return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object target, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class NullableBooleanToBooleanConverter : IValueConverter
    {
        public bool Reverse = false;

        public object Convert(object value, Type targetType, object target, System.Globalization.CultureInfo culture)
        {
            if (!Reverse)
                return NullableToBoolean((bool?)value);
            else
                return BooleanToNullable((bool)value);
        }

        public object ConvertBack(object value, Type targetType, object target, System.Globalization.CultureInfo culture)
        {
            if (!Reverse)
                return BooleanToNullable((bool)value);
            else
                return NullableToBoolean((bool?)value);
        }

        private bool NullableToBoolean(bool? value)
        {
            if (value == true)
                return true;
            else
                return false;
        }

        private bool? BooleanToNullable(bool value)
        {
            if (value)
                return true;
            else
                return false;
        }
    }

    public class TextBoxFilter : IFilter
    {
        private TextBox textBox;
        public TextBoxFilter(TextBox textBox)
        {
            this.textBox = textBox;
        }

        public bool Filter(object obj)
        {
            Nativity nativity = (Nativity)obj;

            if (String.IsNullOrEmpty(textBox.Text))
                return true;
            else
                return Contains(nativity);
        }

        private bool Contains(Nativity nativity)
        {
            bool result = false;

            if (nativity.Id.ToString().Contains(textBox.Text, System.Globalization.CompareOptions.IgnoreCase))
                result = true;
            else if (nativity.Title.Contains(textBox.Text, System.Globalization.CompareOptions.IgnoreCase))
                result = true;
            else if (nativity.Origin.Contains(textBox.Text, System.Globalization.CompareOptions.IgnoreCase))
                result = true;
            else if (nativity.Acquired.Contains(textBox.Text, System.Globalization.CompareOptions.IgnoreCase))
                result = true;
            else if (nativity.From.Contains(textBox.Text, System.Globalization.CompareOptions.IgnoreCase))
                result = true;
            else if (nativity.Cost.ToString().Contains(textBox.Text, System.Globalization.CompareOptions.IgnoreCase))
                result = true;
            else if (nativity.Location.Contains(textBox.Text, System.Globalization.CompareOptions.IgnoreCase))
                result = true;
            else if (CollectionContainsPartOf(nativity.Tags, textBox.Text))
                result = true;
            else if (CollectionContainsPartOf(nativity.GeographicalOrigins, textBox.Text))
                result = true;
            else if (nativity.Description.Contains(textBox.Text, System.Globalization.CompareOptions.IgnoreCase))
                result = true;

            return result;
        }

        private bool CollectionContainsPartOf(IEnumerable<string> list, string value)
        {
            foreach (string @string in list)
            {
                if (@string.Contains(value, System.Globalization.CompareOptions.IgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }

    public static class Extentions
    {
        public static bool Contains(this String s, string value, System.Globalization.CompareOptions compareOptions)
        {
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("es-ES", false);
            return culture.CompareInfo.IndexOf(s, value, compareOptions) >= 0;
        }
    }
}