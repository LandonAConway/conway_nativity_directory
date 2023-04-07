using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwayNativityDirectory.PluginApi
{
    public abstract class WaitFormRef
    {
        public abstract string Title { get; set; }
        public abstract string Description { get; set; }
        public abstract void Show();
        public abstract void Close();
    }
}
