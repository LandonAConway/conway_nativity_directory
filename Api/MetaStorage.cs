using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace ConwayNativityDirectory.PluginApi
{
    public class MetaStorage : IEnumerable<MetaStorageItem>
    {
        List<MetaStorageItem> items;
        public MetaStorage()
        {
            items = new List<MetaStorageItem>();
        }


        public MetaStorageItemChangedEventHandler ValueChanged;
        public MetaStorageItemChangedEventHandler ValueCreated;
        public MetaStorageItemChangedEventHandler ValueDeleted;
        public EventHandler Cleared;


        public MetaStorageItem this[int index] => items[index];
        public int Count => items.Count;
        public void Clear()
        { 
            items.Clear();
            Cleared?.Invoke(this, EventArgs.Empty);
        }

        public string this[string key]
        {
            get { return GetValue(key); }
            set { SetValue(key, value); }
        }

        public string GetValue(string key)
        {
            MetaStorageItem item = items.Where(a => a.Key == key).FirstOrDefault();
            if (item != null)
                return item.Value;
            return String.Empty;
        }

        public void SetValue(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            MetaStorageItem item = items.Where(a => a.Key == key).FirstOrDefault();

            if (String.IsNullOrEmpty(value))
            {
                items.Remove(item);
                if (item != null)
                    ValueDeleted?.Invoke(this, new MetaStorageItemChangedEventArgs(this, key, item.Value, null));
                return;
            }

            if (item != null)
            {
                var oldValue = item.Value;
                item.Value = value;
                ValueChanged?.Invoke(this, new MetaStorageItemChangedEventArgs(this, key, oldValue, value));
            }

            else
            {
                items.Add(new MetaStorageItem(key, value));
                ValueCreated?.Invoke(this, new MetaStorageItemChangedEventArgs(this, key, null, value));
            }
        }

        public void SetValue(string key)
        {
            SetValue(key, String.Empty);
        }

        public IEnumerator<MetaStorageItem> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class MetaStorageItem
    {
        public MetaStorageItem(string key, string value)
        {
            this.key = key;
            this.Value = value;
        }

        string key;
        public string Key { get { return key; } }

        public string Value { get; set; }
    }

    public delegate void MetaStorageItemChangedEventHandler(object sender, MetaStorageItemChangedEventArgs e);

    public class MetaStorageItemChangedEventArgs : EventArgs
    {
        MetaStorage metaStorage;
        /// <summary>
        /// Gets the <see cref="ConwayNativityDirectory.PluginApi.MetaStorage"/> object that contains the item which changed.
        /// </summary>
        public MetaStorage MetaStorage => metaStorage;

        string key;
        /// <summary>
        /// Gets the Key.
        /// </summary>
        public string Key => key;

        string oldValue;
        /// <summary>
        /// Gets the old value.
        /// </summary>
        public string OldValue => oldValue;

        string newValue;
        /// <summary>
        /// Gets the new value.
        /// </summary>
        public string Value => newValue;

        /// <summary>
        /// Indicates if the value was deleted.
        /// </summary>
        public bool WasDeleted => newValue == null;

        /// <summary>
        /// Indicates if the value was created.
        /// </summary>
        public bool WasCreated => oldValue == null;

        /// <summary>
        /// Indicates if the value was not deleted or created but modified.
        /// </summary>
        public bool WasChanged => newValue != null && oldValue != null;

        public MetaStorageItemChangedEventArgs(MetaStorage metaStorage, string key, string oldValue, string newValue)
        {
            this.metaStorage = metaStorage;
            this.key = key;
            this.oldValue = oldValue;
            this.newValue = newValue;
        }
    }

    public class MetaStorageFileSystem
    {
        MetaStorage metaStorage;
        public MetaStorage MetaStorage { get { return metaStorage; } }

        public MetaStorageFileSystem(MetaStorage metaStorage)
        {
            this.metaStorage = metaStorage;
        }

        public void SaveToFile(string path)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (var item in MetaStorage)
                dictionary.Add(item.Key, item.Value);

            string json = JsonSerializer.Serialize(dictionary);

            File.WriteAllText(path, json);
        }

        public void LoadFromFile(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(path);

            MetaStorage.Clear();
            Dictionary<string, string> dictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(path));
            foreach (var item in dictionary)
            {
                MetaStorage[item.Key] = item.Value;
            }
        }
    }
}
