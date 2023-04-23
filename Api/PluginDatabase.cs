using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ConwayNativityDirectory.PluginApi.Primitives;
using ConwayNativityDirectory.PluginApi.Lua_Sandbox;

namespace ConwayNativityDirectory.PluginApi
{
    public interface IConwayNativityDirectory
    {
        Version Version { get; }
        string[] EnabledPlugins { get; }
        MetaStorage AppStorage { get; }
        MetaTagCollection MetaTags { get; }
        IConwayNativityDirectoryProject GetProject();
        NativityBase CreateNativity();
        event WorkspaceKeyChangedEventHandler SelectedWorkspaceKeyChanged;
        string SelectedWorkspaceKey { get; }
        string SelectedViewPaneKey { get; }
        string SelectedInfoPaneKey { get; }
        void Log(Exception exception);
        void Log(string level, string description);
        void Log(string level, string name, string description);
        void CommandConsoleLog(object value);
        void CommandConsoleClear();
        string GetInput(string caption, string defaultResponse);
        WaitFormRef CreateWaitForm();
        ProgressBarModalRef CreateProgressBarModal();
        NativityObjBase CreateNativityObjBase();
        NativityObjBase GetNativityObjBase(NativityBase nativityBase);
    }

    public static class PluginDatabase
    {
        private static IConwayNativityDirectory _conwayNativityDirectory;
        internal static IConwayNativityDirectory ConwayNativityDirectory { get { return _conwayNativityDirectory; } }

        public static void SetConwayNativityDirectory(IConwayNativityDirectory conwayNativityDirectory)
        {
            if (_conwayNativityDirectory == null)
                _conwayNativityDirectory = conwayNativityDirectory;
            else
                throw new InvalidOperationException("\"ConwayNativityDirectory\" can only be set once.");

            _conwayNativityDirectory.SelectedWorkspaceKeyChanged += _conwayNativityDirectory_SelectedWorkspaceKeyChanged;
        }

        private static void _conwayNativityDirectory_SelectedWorkspaceKeyChanged(WorkspaceKeyChangedEventArgs args)
        {
            _ConwayNativityDirectory.InvokeSelectedWorkspaceKeyChangedEvent(args);
        }
    }

    public static class _ConwayNativityDirectory
    {
        public static Version Version { get { return _cnd.Version; } }
        public static string[] EnabledPlugins { get { return _cnd.EnabledPlugins; } }
        public static MetaStorage AppMeta => _cnd.AppStorage;
        public static MetaTagCollection MetaTags { get { return _cnd.MetaTags; } }
        private static IConwayNativityDirectory _cnd
        {
            get { return PluginDatabase.ConwayNativityDirectory; }
        }

        public static IConwayNativityDirectoryProject GetProject() { return _cnd.GetProject(); }

        public static string SelectedWorkspaceKey { get { return _cnd.SelectedWorkspaceKey; } }
        public static string SelectedViewPaneKey { get { return _cnd.SelectedViewPaneKey; } }
        public static string SelectedInfoPaneKey { get { return _cnd.SelectedInfoPaneKey; } }

        public static event WorkspaceKeyChangedEventHandler SelectedWorkspaceKeyChanged;

        public static void Log(Exception exception)
        {
            _cnd.Log(exception);
        }

        public static void Log(string level, string description)
        {
            _cnd.Log(level, description);
        }

        public static void Log(string level, string name, string description)
        {
            _cnd.Log(level, name, description);
        }

        public static WaitFormRef CreateWaitForm()
        {
            return _cnd.CreateWaitForm();
        }

        public static ProgressBarModalRef CreateProgressBarModal()
        {
            return _cnd.CreateProgressBarModal();
        }

        public static string GetInput() => _cnd.GetInput("Conway Nativity Directory", string.Empty);
        public static string GetInput(string caption) => _cnd.GetInput(caption, string.Empty);
        public static string GetInput(string caption, string defaultResponse) => _cnd.GetInput(caption, defaultResponse);

        #region internal stuff

        internal static void InvokeSelectedWorkspaceKeyChangedEvent(WorkspaceKeyChangedEventArgs args)
        {
            if (SelectedWorkspaceKeyChanged != null)
                SelectedWorkspaceKeyChanged.Invoke(args);
        }

        #endregion
    }
}
