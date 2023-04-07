using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ConwayNativityDirectory.PluginApi;
using NLua;
using NLua.Exceptions;

namespace Conway_Nativity_Directory
{
    public class LuaCommand : ICommand
    {
        public bool RequiresProject => false;
        public bool RequiresLogin => false;
        public string Command => @"lua";
        public string Description => "Runs lua code.";
        public string Params => "<code>";

        public void Execute(string text)
        {
            try
            {
                LuaScripting.Engine.DoString(text);
            }

            catch (Exception ex)
            {
                LuaScriptException lse = ex as LuaScriptException;
                if (lse != null && lse.InnerException != null)
                    CommandConsole.Log("Error: " + lse.InnerException.Message);
                else
                    CommandConsole.Log("Error: " + ex.Message);
            }
        }
    }
}
