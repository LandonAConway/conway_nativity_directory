using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ConwayNativityDirectory.PluginApi
{
    public abstract class PaneItemPlugin : PluginFeature
    {
        internal PaneItemPlugin() { }

        public virtual string WorkspaceKey { get { return "main"; } }
    }

    //info panes
    public abstract class InfoPaneItemPlugin : PaneItemPlugin
    {
        /// <summary>
        /// Called when the <see cref="PaneItem"/> is added to the info pane.
        /// </summary>
        /// <param name="paneItem">The <see cref="PaneItem"/> that is added to the info pane.</param>
        /// 
        public virtual void OnLoad(PaneItem paneItem) { }

        /// <summary>
        /// Overrides <see cref="CreatePaneItem"/>.
        /// </summary>
        /// <returns>The <see cref="PaneItem"/> that will be added to the pane.</returns>
        protected virtual PaneItem CreatePaneItemOverride()
        {
            return null;
        }

        /// <summary>
        /// Creates the <see cref="PaneItem"/> that will be added to the pane.
        /// </summary>
        /// <returns>The <see cref="PaneItem"/> that will be added to the pane.</returns>
        public PaneItem CreatePaneItem()
        {
            PaneItem PaneItem = CreatePaneItemOverride();
            if (PaneItem == null)
                PaneItem = new PaneItem();
            return PaneItem;
        }
    }

    //info panes
    public abstract class ViewPaneItemPlugin : PaneItemPlugin
    {
        /// <summary>
        /// Called when the <see cref="PaneItem"/> is added to the view pane.
        /// </summary>
        /// <param name="paneItem">The <see cref="PaneItem"/> that is added to the info pane.</param>
        public virtual void OnLoad(PaneItem paneItem) { }

        /// <summary>
        /// Overrides <see cref="CreatePaneItem"/>.
        /// </summary>
        /// <returns>The <see cref="PaneItem"/> that will be added to the pane.</returns>
        protected virtual PaneItem CreatePaneItemOverride()
        {
            return null;
        }

        /// <summary>
        /// Creates the <see cref="PaneItem"/> that will be added to the pane.
        /// </summary>
        /// <returns>The <see cref="PaneItem"/> that will be added to the pane.</returns>
        public PaneItem CreatePaneItem()
        {
            PaneItem tabItem = CreatePaneItemOverride();
            if (tabItem == null)
                tabItem = new PaneItem();
            return tabItem;
        }
    }

    public class PaneItem : TabItem
    {
        /// <summary>
        /// Indicates the key that is used to know whcich <see cref="PaneItem"/> is selected.
        /// </summary>
        public string Key { get; set; }
    }
}
