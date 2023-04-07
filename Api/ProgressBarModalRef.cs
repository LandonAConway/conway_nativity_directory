using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwayNativityDirectory.PluginApi
{
    public delegate void ProgressBarModalProcess(ProgressBarModalRef progressBarModal);

    public abstract class ProgressBarModalRef
    {
        public abstract string Title { get; set; }

        public abstract string Info { get; set; }

        public abstract double Value { get; set; }

        public abstract double Max { get; set; }

        public abstract ProgressBarModalProcess Process { get; set; }

        public abstract void Show();

        public abstract void Increment();
    }
}
