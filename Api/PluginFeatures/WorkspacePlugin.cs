using ConwayNativityDirectory.PluginApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwayNativityDirectory.PluginApi
{
    public abstract class WorkspacePlugin : PluginFeature
    {
        public virtual WorkspaceViewMode ViewMode => WorkspaceViewMode.Panes;

        public virtual object CustomViewContent => null;

        public abstract string Key { get; }

        public abstract string Title { get; }

        public virtual void OnLoad() { }

        public virtual void OnShow() { }
    }
}
