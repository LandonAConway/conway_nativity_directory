using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConwayNativityDirectory.PluginApi;
using NLua;

namespace Conway_Nativity_Directory.Lua_Sandbox
{
    internal class ProgressBarModalObj
    {
        public ProgressBarModalObj()
        {
            progressBarModal = new ProgressBarModal();
            progressBarModal.Process = new ProgressBarModalProcess((ProgressBarModalRef sender) =>
            {
                if (Process != null)
                    Process.Call(this);
            });
        }

        private ProgressBarModal progressBarModal;

        string name;
        public string GetName() => name;
        public void SetName(string value) => name = value;

        public string GetTitle() => progressBarModal.Title;
        public void SetTitle(string value) => progressBarModal.Title = value;

        public string GetInfo() => progressBarModal.Info;
        public void SetInfo(string value) => progressBarModal.Info = value;

        public double GetValue() => progressBarModal.Value;
        public void SetValue(double value) => progressBarModal.Value = value;

        public double GetMax() => progressBarModal.Max;
        public void SetMax(double value) => progressBarModal.Max = value;

        public LuaFunction Process { get; set; }

        public void Increment() => progressBarModal.Increment();
    }
}
