using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using NLua;

namespace ConwayNativityDirectory.PluginApi
{
    public interface IPlugin
    {
        IEnumerable<string> GetDependencies();
        IEnumerable<IPluginFeature> GetPluginFeatures();
        bool IsLoaded { get; }
        void Load(string installationDirectory, MetaStorage pluginStorage);
    }

    public abstract class PluginBase : IPlugin
    {


        #region Basic Information


        public abstract Version RequiredVersion { get; }

        public abstract Version Version { get; }

        public abstract string Name { get; }

        public abstract string Title { get; }

        public virtual string Description { get { return ""; } }


        #endregion


        #region Dependencies & Features

        public virtual IEnumerable<string> GetDependencies()
        {
            return Enumerable.Empty<string>();
        }

        public virtual IEnumerable<IPluginFeature> GetPluginFeatures()
        {
            return Enumerable.Empty<IPluginFeature>();
        }

        #endregion


        #region Load

        bool isLoaded;
        public bool IsLoaded { get { return isLoaded; } }

        MetaStorage pluginStorage;
        public MetaStorage PluginStorage { get { return pluginStorage; } }

        string installationDirectory;
        public string InstallationDirectory { get { return installationDirectory; } }

        public void Load(string installationDirectory, MetaStorage pluginStorage)
        {
            if (!isLoaded)
            {
                this.installationDirectory = installationDirectory;
                LoadPluginStorage(pluginStorage);
                OnLoad();
                isLoaded = true;
                OnLoaded();
            }

            else
                throw new InvalidOperationException("The plugin can only be loaded once.");
        }

        public void NotifyAllPluginsLoaded()
        {
            if (isLoaded)
                OnAllPluginsLoaded();
        }

        private void LoadPluginStorage(MetaStorage pluginStorage)
        {
            this.pluginStorage = pluginStorage;
            OnPluginStorageLoaded(pluginStorage);
        }

        protected virtual void OnLoad() { }

        protected virtual void OnLoaded() { }

        protected virtual void OnPluginStorageLoaded(MetaStorage pluginStorage) { }

        protected virtual void OnAllPluginsLoaded() { }

        #endregion


        #region Lua Scripting


        bool luaScriptingIsLoaded = false;
        public void LoadLuaScripting(Lua engine)
        {
            if (!luaScriptingIsLoaded)
                this.OnLoadLuaScripting(engine);
            luaScriptingIsLoaded = true;
        }

        protected virtual void OnLoadLuaScripting(Lua engine) { }


        #endregion


    }

    public interface IPluginFeature { }
}
