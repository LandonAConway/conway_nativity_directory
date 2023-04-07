using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ConwayNativityDirectory.PluginApi;

namespace Conway_Nativity_Directory
{
    public class ClearCommand : ICommand
    {
        public bool RequiresProject => false;
        public bool RequiresLogin => false;
        public string Command => @"clear";
        public string Description => "Clears the console window.";
        public string Params => "";

        public void Execute(string text)
        {
            CommandConsoleWindow.Clear();
        }
    }
}
