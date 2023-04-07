using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents.Serialization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ConwayNativityDirectory.PluginApi.Deployment
{
    public class PluginDeploymentQueue : IEnumerable<PluginDeploymentQueueItem>
    {
        internal PluginDeploymentQueue() { }

        List<PluginDeploymentQueueItem> items = new List<PluginDeploymentQueueItem>();

        string pluginsFolder;
        public string PluginsFolder { get { return pluginsFolder; } }

        string fileName;
        public string FileName { get { return fileName; } }

        public void AddItem(PluginDeploymentType deploymentType, string packageFileName)
        {
            var item = items.Where(x => x.DeploymentType == deploymentType &&
                x.PackageFileName == packageFileName).FirstOrDefault();
            if (item != null)
                items.Remove(item);
            items.Add(PluginDeploymentQueueItem.Create(this, deploymentType, packageFileName));
        }

        public void AddItem(PluginDeploymentQueueItem item) => AddItem(item.DeploymentType, item.PackageFileName);

        public void RemoveItem(PluginDeploymentQueueItem item) => items.Remove(item);

        public void RemoveItemAt(int index)
        {
            try
            {
                items.RemoveAt(index);
            }

            catch { }
        }

        public void Clear() => items.Clear();

        public void Run()
        {
            foreach (var item in items)
                item.Run();
        }

        public void SaveToFile(string fileName)
        {
            string data = "";
            foreach (var item in items)
                data += item.ToString() + "\n";
            data.TrimEnd('\n');
            File.WriteAllText(fileName, data);
        }

        public void SaveToFile() => SaveToFile(FileName);

        public static PluginDeploymentQueue OpenOrCreate(string fileName, string installationFolder)
        {
            var queue = new PluginDeploymentQueue();
            queue.pluginsFolder = installationFolder;
            queue.fileName = fileName;

            if (File.Exists(fileName))
                foreach (string line in File.ReadAllLines(fileName))
                    queue.items.Add(PluginDeploymentQueueItem.FromString(queue, line));

            return queue;
        }

        public IEnumerator<PluginDeploymentQueueItem> GetEnumerator() => items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class PluginDeploymentQueueItem
    {
        PluginDeploymentQueue queue;
        internal PluginDeploymentQueueItem(PluginDeploymentQueue queue)
        {
            this.queue = queue;
        }
        
        internal PluginDeploymentType deploymentType;
        public PluginDeploymentType DeploymentType { get { return deploymentType; } }

        string packageFileName;
        public string PackageFileName { get { return packageFileName; } }

        public void Run()
        {
            PluginPackage pluginPackage = PluginPackage.Open(packageFileName);
            if (DeploymentType == PluginDeploymentType.Install)
                pluginPackage.Install(queue.PluginsFolder);
            else if (DeploymentType == PluginDeploymentType.Uninstall)
                pluginPackage.Uninstall(queue.PluginsFolder);
            else if (DeploymentType == PluginDeploymentType.Update)
                pluginPackage.Update(queue.PluginsFolder);
        }

        internal static PluginDeploymentQueueItem Create(PluginDeploymentQueue queue, PluginDeploymentType deploymentType, string packageFileName)
        {
            PluginDeploymentQueueItem item = new PluginDeploymentQueueItem(queue);
            item.queue = queue;
            item.deploymentType = deploymentType;
            item.packageFileName = packageFileName;
            return item;
        }

        internal static PluginDeploymentQueueItem FromString(PluginDeploymentQueue queue, string data)
        {
            string[] _data = data.Split('|');
            PluginDeploymentQueueItem item = new PluginDeploymentQueueItem(queue);
            switch (_data[0])
            {
                case "Install":
                    item.deploymentType = PluginDeploymentType.Install;
                    break;
                case "Uninstall":
                    item.deploymentType = PluginDeploymentType.Uninstall;
                    break;
                case "Update":
                    item.deploymentType = PluginDeploymentType.Update;
                    break;
            }
            item.packageFileName = _data[1];
            return item;
        }

        public override string ToString()
        {
            return deploymentType.ToString() + "|" + packageFileName;
        }
    }

    public enum PluginDeploymentType { Install, Uninstall, Update }
}
