using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConwayNativityDirectory.PluginApi;
using NLua;

namespace Conway_Nativity_Directory.Lua_Sandbox
{
    public class NativityObj
    {
        internal Nativity nativity;

        public NativityObj()
        {
            this.nativity = new Nativity();
        }

        internal NativityObj(Nativity nativity)
        {
            this.nativity = nativity;
        }

        public int GetId() => nativity.Id;
        public void SetId(int value) => nativity.Id = value;

        public string GetTitle() => nativity.Title;
        public void SetTitle(string value) => nativity.Title = value;

        public string GetOrigin() => nativity.Origin;
        public void SetOrigin(string value) => nativity.Origin = value;

        public int GetAcquired() => Convert.ToInt32(nativity.Acquired);
        public void SetAcquired(int value) => nativity.Acquired = value.ToString();

        public string GetFrom() => nativity.From;
        public void SetFrom(string value) => nativity.From = value;

        public double GetCost() => nativity.Cost;
        public void SetCost(double value) => nativity.Cost = value;

        public string GetLocation() => nativity.Location;
        public void SetLocation(string value) => nativity.Location = value;

        public string GetDescription() => nativity.Description;
        public void SetDescription(string value) => nativity.Description = value;

        public object GetTags()
        {
            LuaScripting.Engine.DoString("cnd._t.nativityTags = {}");
            foreach (var tag in nativity.Tags)
            {
                LuaScripting.Engine["cnd._t.nativityTag"] = tag;
                LuaScripting.Engine.DoString("table.insert(cnd._t.nativityTags, cnd._t.nativityTag) \n cnd._t.nativityTag = nil");
            }
            return LuaScripting.Engine.DoString("local nativityTags = cnd._t.nativityTags \n cnd._t.nativityTags = nil \n return nativityTags")[0];
        }

        public void SetTags(object value)
        {
            LuaScripting.Engine["cnd._t.nativityTags"] = value;
            LuaTable tags = LuaScripting.Engine.GetTable("cnd._t.nativityTags");
            nativity.Tags = new System.Collections.ObjectModel.ObservableCollection<string>();
            foreach (string tag in tags.Values)
                nativity.Tags.Add(tag);
            LuaScripting.Engine["cnd._t.nativityTags"] = null;
        }

        public object GetGeographicalOrigins()
        {
            LuaScripting.Engine.DoString("cnd._t.nativityGeographicalOrigins = {}");
            foreach (var GeographicalOrigin in nativity.GeographicalOrigins)
            {
                LuaScripting.Engine["cnd._t.nativityGeographicalOrigin"] = GeographicalOrigin;
                LuaScripting.Engine.DoString(@"
                    table.insert(cnd._t.nativityGeographicalOrigins,
                    cnd._t.nativityGeographicalOrigin)
                    cnd._t.nativityGeographicalOrigin = nil");
            }
            return LuaScripting.Engine.DoString(@"
                    local nativityGeographicalOrigins = cnd._t.nativityGeographicalOrigins
                    cnd._t.nativityGeographicalOrigins = nil
                    return nativityGeographicalOrigins")[0];
        }

        public void SetGeographicalOrigins(object value)
        {
            LuaScripting.Engine["cnd._t.nativityGeographicalOrigins"] = value;
            LuaTable GeographicalOrigins = LuaScripting.Engine.GetTable("cnd._t.nativityGeographicalOrigins");
            nativity.GeographicalOrigins.Clear();
            foreach (string GeographicalOrigin in GeographicalOrigins.Values)
                nativity.GeographicalOrigins.Add(GeographicalOrigin);
            LuaScripting.Engine["cnd._t.nativityGeographicalOrigins"] = null;
        }

        public string GetOriginalImagePath() => nativity.OriginalImagePath;

        public string GetImagePath() => nativity.ImagePath;

        public string GetImageMode() => nativity.ImageMode.ToString();

        public string GetImageRotation() => nativity.ImageRotation.ToString();

        public MetaStorageRef GetMeta() => new MetaStorageRef(nativity.GetMeta());

        public bool GetIsSelected() => nativity.IsSelected;
        public void SetIsSelected(bool value) => nativity.IsSelected = value;

        public void ScrollIntoView()
        {
            if (App.Project.IsOpen && App.Project.Nativities.Where(a => a == nativity).Any())
                App.MainWindow.nativityListView.ScrollIntoView(nativity);
        }
    }

    public class MetaStorageRef
    {
        private MetaStorage metaStorage;
        internal MetaStorageRef(MetaStorage metaStorage)
        {
            this.metaStorage = metaStorage;
        }

        public string GetValue(string key) => metaStorage.GetValue(key);
        public void SetValue(string key, string value) => metaStorage.SetValue(key, value);

        public object ToTable()
        {
            LuaScripting.Engine.DoString("cnd._t.msrfk = {}");
            foreach (var entry in metaStorage)
            {
                LuaScripting.Engine["cnd._t.msrfkKey"] = entry.Key;
                LuaScripting.Engine["cnd._t.msrfkValue"] = entry.Value;
                LuaScripting.Engine.DoString("cnd._t.msrfk[cnd._t.msrfkKey] = cnd._t.msrfkValue cnd._t.msrfkKey = nil cnd._t.msrfkValue = nil");
            }
            return LuaScripting.Engine.DoString("local msrfk = cnd._t.msrfk cnd._t.msrfk = nil return msrfk");
        }
    }
}
