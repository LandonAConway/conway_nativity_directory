using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace ConwayNativityDirectory.PluginApi.Deployment
{
    public class PluginPackage
    {
        internal PluginPackage() { }

        string name;
        public string Name { get { return name; } }

        string title;
        public string Title { get { return title; } }

        string description;
        public string Description { get { return description; } }

        Version version;
        public Version Version { get { return version; } }

        Version requiredVersion;
        public Version RequiredVersion { get { return requiredVersion; } }

        string website;
        public string Website { get { return website; } }

        string author;
        public string Author { get { return author; } }

        string fileName;
        string FileName { get { return fileName; } }

        public void Install(string folderPath)
        {
            var pluginInstalledFolderPath = folderPath + @"\installed\" + name;
            var pluginDataFolderPath = folderPath + @"\data\" + name;
            var tempExtractionPath = folderPath + @"\temp_" + name;

            Uninstall(folderPath);

            //installed
            Directory.CreateDirectory(tempExtractionPath);
            ZipFile.ExtractToDirectory(fileName, tempExtractionPath); //
            Directory.Move(tempExtractionPath + @"\installation", pluginInstalledFolderPath);

            //data
            if (!Directory.Exists(pluginDataFolderPath))
                Directory.CreateDirectory(pluginDataFolderPath);
            if (!File.Exists(pluginDataFolderPath + @"\data.json"))
                File.WriteAllText(pluginDataFolderPath + @"\data.json", "{\"IsEnabled\":\"True\"");

            //.cndp file
            var cndp = pluginInstalledFolderPath + ".cndp";
            if (File.Exists(cndp))
                File.Delete(cndp);
            File.Copy(FileName, cndp);

            //cleanup
            Directory.Delete(tempExtractionPath, true);
        }

        public void Install() => Install(AppDomain.CurrentDomain.BaseDirectory + @"\bin\plugins"); //

        public void Uninstall(string folderPath)
        {
            var pluginFolderPath = folderPath + @"\installed\" + name;
            if (IsInstalled(folderPath))
            {
                Directory.Delete(pluginFolderPath, true);
                File.Delete(pluginFolderPath + ".cndp");
            }
        }

        public void Uninstall() => Uninstall(AppDomain.CurrentDomain.BaseDirectory + @"\bin\plugins\");

        public bool IsInstalled(string folderPath) => Directory.Exists(folderPath + @"\installed\" + name);
        public bool IsInstalled() => IsInstalled(AppDomain.CurrentDomain.BaseDirectory + @"\bin\plugins\");

        public void Update(string folderPath)
        {
            if (IsInstalled())
            {
                var currentPackagePath = folderPath + @"\installed\" + name + ".cndp";
                var currentPackage = PluginPackage.Open(currentPackagePath);
                if (FileName == currentPackagePath)
                    throw new InvalidOperationException("The package being installed cannot be located at the same place as the " +
                        "already installed package.");
                if (this.Version > currentPackage.Version)
                    Install();
            }
        }

        public void Update() => Update(AppDomain.CurrentDomain.BaseDirectory + @"\bin\plugins\");

        public static PluginPackage Open(string fileName)
        {
            string json = "";
            using (ZipArchive zip = ZipFile.Open(fileName, ZipArchiveMode.Read))
                foreach (ZipArchiveEntry entry in zip.Entries)
                    if (entry.Name == @"packageinfo.json")
                    {
                        StreamReader reader = new StreamReader(entry.Open());
                        json = reader.ReadToEnd();
                    }

            var pij = JsonSerializer.Deserialize<PluginPackageJson>(json);
            PluginPackage package = new PluginPackage();
            package.fileName = fileName;
            package.name = pij.name;
            package.title = pij.title;
            package.description = pij.description;
            package.version = new Version(pij.version);
            package.requiredVersion = new Version(pij.version);
            package.author = pij.author;
            package.website = pij.website;
            return package;
        }
    }

    internal class PluginPackageJson
    {
        public string name { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string version { get; set; }
        public string requiredVersion { get; set; }
        public string website { get; set; }
        public string author { get; set; }
    }
}
