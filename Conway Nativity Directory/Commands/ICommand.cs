using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ConwayNativityDirectory.PluginApi;

namespace Conway_Nativity_Directory
{
    //The ICommand interface is implemented in the PluginApi library.

    public static class Commands
    {
        public static readonly CommandMap RegisteredCommands = new CommandMap();

        internal static void RegisterInternalCommands()
        {
            RegisteredCommands.Register(new HelpCommand());
            RegisteredCommands.Register(new ClearCommand());
            RegisteredCommands.Register(new LuaCommand());
            RegisteredCommands.Register(new TotalCommand());
        }
    }

    public sealed class CommandMap : IEnumerable<ICommand>
    {
        private List<ICommand> commands = new List<ICommand>();

        public IEnumerator<ICommand> GetEnumerator()
        {
            return commands.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public ICommand this[string command]
        {
            get { return commands.Where(c => c.Command == command).FirstOrDefault(); }
        }

        public ICommand this[int index]
        {
            get { return commands[index]; }
        }

        public void Register(ICommand command)
        {
            if (!commands.Any(c => c.GetType() == command.GetType()))
                commands.Add(command);
            else
                throw new InvalidOperationException("There can only be one command instance per type.");
        }

        public void Unregister(ICommand command)
        {
            commands.Remove(command);
        }

        public void UnregisterAll()
        {
            commands.Clear();
        }
    }
}
