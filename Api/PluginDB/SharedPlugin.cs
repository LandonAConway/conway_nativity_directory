using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections;
using System.Web;
using System.Net;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ConwayNativityDirectory.PluginApi.PluginDB
{
    public class SharedPluginPackage
    {
        internal SharedPluginPackage() { }

        internal string name;
        public string Name { get { return name; } }

        internal string title;
        public string Title { get { return title; } }

        internal string description;
        public string Description { get { return description; } }

        internal Version version;
        public Version Version { get { return version; } }

        internal Version requiredVersion;
        public Version RequiredVersion { get { return requiredVersion; } }

        internal string author;
        public string Author { get { return author; } }

        internal string website;
        public string Website { get { return website; } }

        public List<string> AuthorizationCodes { get; set; } = new List<string>();

        public void Refresh()
        {
            var sharedPlugin = SharedPluginPackage.Get(this.name, AuthorizationCodes);
            this.name = sharedPlugin.Name;
            this.title = sharedPlugin.Title;
            this.description = sharedPlugin.Description;
            this.version = sharedPlugin.Version;
            this.requiredVersion = sharedPlugin.RequiredVersion;
            this.author = sharedPlugin.Author;
            this.website = sharedPlugin.Website;
        }

        public void Download(string filePath)
        {
            Refresh();
            WebClient client = new WebClient();
            client.Headers = new WebHeaderCollection();
            client.Headers.Add("authcodes", JsonSerializer.Serialize(AuthorizationCodes.ToArray()));
            client.DownloadFile(this.GetDownloadUrl(), filePath);
        }

        string GetDownloadUrl() => "http://www.cnd-app.conwaynativities.com/plugins/" + this.Name + "/download";

        internal SharedPluginPackageJson ToJson()
        {
            return new SharedPluginPackageJson()
            {
                name = Name,
                title = Title,
                description = Description,
                version = Version.ToString(),
                requiredVersion = RequiredVersion.ToString(),
                author = Author,
                website = Website
            };
        }

        internal static SharedPluginPackage FromJson(SharedPluginPackageJson sharedPluginJson)
        {
            return new SharedPluginPackage()
            {
                name = sharedPluginJson.name,
                title = sharedPluginJson.title,
                description = sharedPluginJson.description,
                version = Version.Parse(sharedPluginJson.version),
                requiredVersion = Version.Parse(sharedPluginJson.requiredVersion),
                author = sharedPluginJson.author,
                website = sharedPluginJson.website,
            };
        }

        public static SharedPluginPackage Get(string name, IEnumerable<string> authorizationCodes)
        {
            HttpWebRequest httpWebRequest = HttpWebRequest.CreateHttp("http://www.cnd-app.conwaynativities.com/plugins/" + name + "/packageinfo");
            httpWebRequest.Headers = new WebHeaderCollection();
            httpWebRequest.Headers.Add("authcodes", JsonSerializer.Serialize(authorizationCodes));
            var response = httpWebRequest.GetResponse();
            var stream = response.GetResponseStream();
            var readStream = new StreamReader(stream, Encoding.UTF8);
            var json = readStream.ReadToEnd();
            if (json == "PLUGIN_NOT_FOUND") { throw new InvalidOperationException("The plugin does not exist."); }
            var spjson = JsonSerializer.Deserialize<SharedPluginPackageJson>(json);
            var spp = SharedPluginPackage.FromJson(spjson);
            spp.AuthorizationCodes = authorizationCodes.ToList();
            return spp;
        }

        public static SharedPluginPackage Get(string name) => Get(name, null);
    }

    internal class SharedPluginPackageJson
    {
        public string name { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string version { get; set; }
        public string requiredVersion { get; set; }
        public string author { get; set; }
        public string website { get; set; }
    }

    public class SharedPluginCollection : IEnumerable<SharedPluginPackage>
    {
        public List<SharedPluginPackage> items;

        public SharedPluginPackage this[int index] => items[index];

        public SharedPluginPackage this[string name]
        {
            get => items.Where(p => p.Name == name).FirstOrDefault();
        }

        public SharedPluginCollection()
        {
            Refresh();
        }

        public SharedPluginCollection(IEnumerable<string> authorizationCodes)
        {
            this.AuthorizationCodes = authorizationCodes.ToList();
            Refresh();
        }

        public List<string> AuthorizationCodes { get; set; } = new List<string>();

        public void Refresh()
        {
            HttpWebRequest httpWebRequest = HttpWebRequest.CreateHttp("http://www.cnd-app.conwaynativities.com/plugins/get");
            httpWebRequest.Headers = new WebHeaderCollection();
            httpWebRequest.Headers.Add("authcodes", JsonSerializer.Serialize(AuthorizationCodes.ToArray()));
            var response = httpWebRequest.GetResponse();
            var stream = response.GetResponseStream();
            var readStream = new StreamReader(stream, Encoding.UTF8);
            var _items = JsonSerializer.Deserialize<SharedPluginPackageJson[]>(readStream.ReadToEnd());
            items = new List<SharedPluginPackage>();
            foreach (var item in _items)
            {
                var sharedPlugin = SharedPluginPackage.FromJson(item);
                sharedPlugin.AuthorizationCodes = AuthorizationCodes;
                items.Add(sharedPlugin);
            }
        }

        public IEnumerator<SharedPluginPackage> GetEnumerator() => items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
