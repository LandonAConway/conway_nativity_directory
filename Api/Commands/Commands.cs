using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwayNativityDirectory.PluginApi
{

    //Seperate from ICommand for technicalities.
    public interface ICommandPluginFeature : IPluginFeature, ICommand { }

    public interface ICommand
    {
        bool RequiresProject { get; }
        bool RequiresLogin { get; }
        string Command { get; }
        string Description { get; }
        string Params { get; }
        void Execute(string text);
    }

    public static class CommandConsole
    {
        public static void Log(object value) => PluginDatabase.ConwayNativityDirectory.CommandConsoleLog(value);
        public static void Clear() => PluginDatabase.ConwayNativityDirectory.CommandConsoleClear();
    }
}
