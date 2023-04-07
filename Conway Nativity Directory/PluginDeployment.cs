using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConwayNativityDirectory.PluginApi.Deployment;
using ConwayNativityDirectory.PluginApi.PluginDB;

namespace Conway_Nativity_Directory
{
    public static class PluginDeployment
    {
        static string pluginsDir = AppDomain.CurrentDomain.BaseDirectory + @"\bin\plugins\";
        public static readonly PluginDeploymentQueue Queue = PluginDeploymentQueue.OpenOrCreate(
            pluginsDir + @"queue.txt",
            pluginsDir);

        public static void Run()
        {
            Queue.Run();
            Queue.Clear();
            Queue.SaveToFile();
            foreach (var file in System.IO.Directory.GetFiles(pluginsDir + @"\downloads"))
                System.IO.File.Delete(file);
        }
    }
}
