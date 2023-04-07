using ConwayNativityDirectory.PluginApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Conway_Nativity_Directory
{
    public static class WorkspacesMain
    {
        static string currentWorkspaceKey;
        public static string CurrentWorkspaceKey
        {
            get { return currentWorkspaceKey; }
            set
            {
                if (Workspaces[value] == null)
                    return;

                var oldValue = currentWorkspaceKey;
                currentWorkspaceKey = value;

                ShowWorkspace(value);
                PluginDatabaseMain.CNDWrapper.InvokeSelectedWorkspaceKeyChangedEvent(
                    new WorkspaceKeyChangedEventArgs(oldValue, value));
            }
        }

        private static void ShowWorkspace(string workspaceKey)
        {
            try
            {
                var mw = App.MainWindow;
                var workspace = Workspaces[workspaceKey];

                if (workspace.ViewMode == WorkspaceViewMode.Custom)
                {
                    mw.workspaceViewMode_custom_contentControl.Content = workspace.CustomViewObject;
                    mw.workspaceViewMode_custom.Visibility = Visibility.Visible;
                    mw.workspaceViewMode_panes.Visibility = Visibility.Collapsed;
                }

                else if (workspace.ViewMode == WorkspaceViewMode.Panes)
                {
                    mw.workspaceViewMode_custom_contentControl.Content = null;
                    mw.workspaceViewMode_panes.Visibility = Visibility.Visible;
                    mw.workspaceViewMode_custom.Visibility = Visibility.Collapsed;
                }

                foreach (Pane pane in mw.viewPanesGrid.Children)
                {
                    if (pane.WorkspaceKey == workspaceKey)
                        pane.Visibility = Visibility.Visible;
                    else
                        pane.Visibility = Visibility.Collapsed;
                }

                foreach (Pane pane in mw.infoPanesGrid.Children)
                {
                    if (pane.WorkspaceKey == workspaceKey)
                        pane.Visibility = Visibility.Visible;
                    else
                        pane.Visibility = Visibility.Collapsed;
                }

                if (workspace.WorkspacePlugin != null)
                    workspace.WorkspacePlugin.OnShow();
            }

            catch { }
        }

        public static void CreateMainWorkspace()
        {
            MainWindow mw = App.MainWindow;

            Workspace workspace = new Workspace("main", "Main");
            workspace.viewPane = mw.mainViewPane;
            workspace.infoPane = mw.mainInfoPane;
            Workspaces.Insert(0, workspace);

            //test
            //Workspace workspaceTest = new Workspace("test", "Test");
            //Workspaces.Add(workspaceTest);

            //Grid grid = new Grid();
            //grid.Background = Brushes.Blue;

            //PaneItem paneItem = new PaneItem();
            //paneItem.Header = "Test";
            //paneItem.Content = grid;

            //workspaceTest.viewPane.Items.Add(paneItem);
        }

        public static void LoadWorkspaces()
        {
            MainWindow mw = App.MainWindow;
            foreach (var ws in Workspaces.Reverse())
            {
                if (ws.Key != "main")
                {
                    mw.viewPanesGrid.Children.Add(ws.viewPane);
                    mw.infoPanesGrid.Children.Add(ws.InfoPane);
                }

                MenuItem menuItem = new MenuItem();
                menuItem.Header = ws.Title;
                menuItem.Tag = ws.Key;
                menuItem.Click += MenuItem_Click;
                mw.workspacesMenuItem.Items.Add(menuItem);
            }

            WorkspacesMain.CurrentWorkspaceKey = "main";
        }

        private static void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            CurrentWorkspaceKey = ((MenuItem)sender).Tag as string;
        }

        public static readonly WorkspaceCollection Workspaces = new WorkspaceCollection() { };
    }

    public class Workspace : DependencyObject
    {
        public Workspace(string key, string title)
        {
            this.key = key;
            this.title = title;
            viewPane = new Pane();
            viewPane.WorkspaceKey = key;
            infoPane = new Pane();
            infoPane.WorkspaceKey = key;
        }

        public Workspace(string key, string title, WorkspaceViewMode viewMode, object customViewObject)
        {
            this.viewMode = viewMode;
            this.customViewObject = customViewObject;
            this.key = key;
            this.title = title;
            viewPane = new Pane();
            viewPane.WorkspaceKey = key;
            infoPane = new Pane();
            infoPane.WorkspaceKey = key;
        }

        WorkspacePlugin workspacePlugin;
        public WorkspacePlugin WorkspacePlugin => workspacePlugin;

        WorkspaceViewMode viewMode = WorkspaceViewMode.Panes;
        public WorkspaceViewMode ViewMode => viewMode;

        object customViewObject;
        public object CustomViewObject => customViewObject;

        string key;
        public string Key { get { return key; } }

        string title;
        public string Title { get { return title; } }

        //The only purpose to set this is so the 'main' workspace can be set manually, otherwise the builtin MainWindow UI will act strange.
        internal Pane viewPane;
        public Pane Pane { get { return viewPane; } }

        //Same principle applies to the purpose of making this internal
        internal Pane infoPane;
        public Pane InfoPane { get { return infoPane; } }


        public static Workspace FromWorkspacePlugin(WorkspacePlugin plugin)
        {
            var workspace = new Workspace(plugin.Key, plugin.Title, plugin.ViewMode, plugin.CustomViewContent);
            workspace.workspacePlugin = plugin;
            return workspace;
        }
    }

    public class WorkspaceCollection : IEnumerable<Workspace>, ICollection<Workspace>, IList<Workspace>
    {
        List<Workspace> workspaces = new List<Workspace>();

        public Workspace this[string key]
        {
            get
            {
                return workspaces.Where(a => a.Key == key).FirstOrDefault();
            }
        }

        public Workspace this[int index]
        {
            get => workspaces[index];
            set => workspaces[index] = value;
        }

        public int Count => workspaces.Count;

        public bool IsReadOnly => false;

        public void Add(Workspace workspace)
        {
            if (this[workspace.Key] == null)
                workspaces.Add(workspace);
            else
                throw new InvalidOperationException("A workspace with the key \"" + workspace.Key + "\" already exists.");
        }

        public void Clear() => workspaces.Clear();

        public bool Contains(Workspace item) => workspaces.Contains(item);

        public void CopyTo(Workspace[] array, int arrayIndex) => workspaces.CopyTo(array, arrayIndex);

        public IEnumerator<Workspace> GetEnumerator()
        {
            return workspaces.GetEnumerator();
        }

        public int IndexOf(Workspace item) => workspaces.IndexOf(item);

        public void Insert(int index, Workspace item) => workspaces.Insert(index, item);

        public bool Remove(Workspace item) => workspaces.Remove(item);

        public void RemoveAt(int index) => workspaces.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public class Pane : TabControl
    {
        public string WorkspaceKey { get; set; }
    }

    public enum PaneItemType { View, Info }
}