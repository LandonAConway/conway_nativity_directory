using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwayNativityDirectory.PluginApi
{
    public delegate void WorkspaceKeyChangedEventHandler(WorkspaceKeyChangedEventArgs args);

    public class WorkspaceKeyChangedEventArgs : EventArgs
    {
        public WorkspaceKeyChangedEventArgs(string oldWorkspaceKey, string newWorkspaceKey)
        {
            this.oldWorkspaceKey = oldWorkspaceKey;
            this.newWorkspaceKey = newWorkspaceKey;
        }

        string oldWorkspaceKey;
        public string OldWorkspaceKey { get { return oldWorkspaceKey; } }

        string newWorkspaceKey;
        public string NewWorkspaceKey { get { return newWorkspaceKey; } }
    }

    public class WorkspaceKeyNamingException : Exception
    {
        public WorkspaceKeyNamingException(string key)
        {
            this.key = key;
        }

        public WorkspaceKeyNamingException(string message, string key) : base(message)
        {
            this.key = key;
        }

        public WorkspaceKeyNamingException(string message, string key, Exception innerException) : base(message, innerException)
        {
            this.key = key;
        }

        string key;
        public string Key { get { return key; } }
    }

    public enum WorkspaceViewMode { Custom, Panes }
}
