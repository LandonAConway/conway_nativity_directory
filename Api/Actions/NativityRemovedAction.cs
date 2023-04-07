using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConwayNativityDirectory.PluginApi.Primitives;

namespace ConwayNativityDirectory.PluginApi
{
    public class NativityRemovedAction : IAction
    {
        private IConwayNativityDirectoryProject project;
        public IConwayNativityDirectoryProject Project { get { return project; } }
        private IEnumerable<NativityBase> nativities;
        public IEnumerable<NativityBase> Nativities { get { return nativities;} }
        public NativityRemovedAction(IConwayNativityDirectoryProject project, IEnumerable<NativityBase> nativities)
        {
            this.project = project;
            this.nativities = nativities;
        }

        public string Description
        {
            get
            {
                if (nativities.Count() < 1 || nativities.Count() > 1)
                    return "Remove [" + nativities.Count().ToString() + "] Nativities";
                else
                    return "Remove [1] Nativity";
            }
        }

        public void Undo()
        {
            foreach (NativityBase nativity in Nativities)
            {
                Project.Nativities.Add(nativity);
            }
        }

        public void Redo()
        {
            foreach (NativityBase nativity in Nativities)
            {
                Project.Nativities.Remove(nativity);
            }
        }
    }
}
