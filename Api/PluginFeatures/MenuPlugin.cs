using ConwayNativityDirectory.PluginApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ConwayNativityDirectory.PluginApi
{
    public abstract class MenuPlugin : PluginFeature
    {
        /// <summary>
        /// Called when the <see cref="MenuItem"/> is added to the main menu.
        /// </summary>
        /// <param name="menuItem">The <see cref="MenuItem"/> that is added to the main menu.</param>
        public virtual void OnLoad(MenuItem menuItem) { }
    }

    public abstract class ProjectMenuPlugin : MenuPlugin { }
    public abstract class EditMenuPlugin : MenuPlugin { }
    public abstract class NativitiesMenuPlugin : MenuPlugin { }
    public abstract class SearchMenuPlugin : MenuPlugin { }
    public abstract class SearchToolsMenuPlugin : MenuPlugin { }
    public abstract class ImportMenuPlugin : MenuPlugin { }
    public abstract class ExportMenuPlugin : MenuPlugin { }
}
