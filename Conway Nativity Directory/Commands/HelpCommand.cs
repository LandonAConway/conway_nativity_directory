using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ConwayNativityDirectory.PluginApi;

namespace Conway_Nativity_Directory
{
    public class HelpCommand : ICommand
    {
        public bool RequiresProject => false;
        public bool RequiresLogin => false;
        public string Command => @"help";
        public string Description => "Shows commands.";
        public string Params => "<command>";

        public void Execute(string text)
        {
            var cmd = Commands.RegisteredCommands[text];
            if (text == "all")
                foreach (var command in Commands.RegisteredCommands)
                {
                    var p = "";
                    if (!String.IsNullOrWhiteSpace(command.Params))
                        p = " " + command.Params;
                    CommandConsole.Log(command.Command + p + " => " + command.Description);
                }
            else if (cmd != null)
            {
                var p = "";
                if (!String.IsNullOrWhiteSpace(cmd.Params))
                    p = " " + cmd.Params;
                CommandConsole.Log(cmd.Command + p + " => " + cmd.Description);
            }
            else
                CommandConsole.Log("The command '" + text + "' does not exist.");
        }
    }
}
