using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ConwayNativityDirectory.PluginApi.Primitives
{
    public abstract class NativityBase : FrameworkElement
    {
        public virtual int Id { get; set; }

        public virtual string Title { get; set; }

        public virtual string Origin { get; set; }

        public virtual string Acquired { get; set; }

        public virtual string From { get; set; }

        public virtual double Cost { get; set; }

        public virtual string Location { get; set; }

        public virtual ObservableCollection<string> Tags {  get; set; }

        public virtual ObservableCollection<string> GeographicalOrigins { get; set; }

        public virtual string Description { get; set; }

        public virtual bool IsSelected { get; set; }

        public abstract ImageMode ImageMode { get; protected set;  }

        public abstract bool HasImage { get; protected set; }

        public abstract System.Windows.Media.Imaging.Rotation ImageRotation { get; set; }

        public abstract string GetImagePath();

        public abstract void AddImage(string fileName);

        public abstract void RemoveImage();

        public abstract void EmbeddImage();

        public abstract void RelinkImage(string fileName);

        public abstract void RelinkImage();

        public abstract void RotateImageRight();

        public abstract void RotateImageLeft();

        public abstract void RefreshImage();

        public abstract void UnloadImage();

        public abstract void LoadImage();

        public abstract void RefreshUI();

        public abstract MetaStorage GetMeta();

        public abstract MetaTagCollection GetMetaTags();

        public abstract DateTime DateModified { get; }

        public abstract DateTime DateModifiedInLastSession { get; }

        public abstract DateTime DateImageModified { get; }

        public abstract DateTime DateImageModifiedInLastSession { get; }

        public abstract event NativityModifiedEventHandler NativityModified;

        public static NativityBase CreateNativity()
        {
            return PluginDatabase.ConwayNativityDirectory.CreateNativity();
        }
    }

    public delegate void NativityModifiedEventHandler(object sender, NativityModifiedEventArgs e);

    public class NativityModifiedEventArgs : EventArgs
    {
        NativityBase nativity;
        public NativityBase Nativity { get { return nativity; } }

        NativityModificationType modificationType;
        public NativityModificationType ModificationType { get { return modificationType; } }

        Type valueType;
        public Type ValueType { get { return valueType; } }

        object oldValue;
        public object OldValue { get { return oldValue; } }

        object newValue;
        public object NewValue { get { return newValue; } }

        public NativityModifiedEventArgs(NativityBase nativity, NativityModificationType modificationType,
            Type valueType, object oldValue, object newValue)
        {
            this.nativity = nativity;
            this.modificationType = modificationType;
            this.valueType = valueType;
            this.oldValue = oldValue;
            this.newValue = newValue;
        }
    }

    public enum NativityModificationType {
        Id,
        Title,
        Origin,
        Acquired,
        From,
        Cost,
        Location,
        Tags,
        GeographicalOrigins,
        Description,
        ImagePath,
        ImageMode,
        HasImage,
        ImageRotation
    }

    public class MetaTagCollection : IEnumerable<MetaTagCollectionItem>
    {
        List<MetaTagCollectionItem> items;
        public MetaTagCollection()
        {
            items = new List<MetaTagCollectionItem>();
        }

        public MetaTagCollectionItem this[int index] => items[index];
        public int Count => items.Count;
        public void Clear() => items.Clear();

        public object this[string key]
        {
            get { return GetValue(key); }
            set { SetValue(key, value); }
        }

        public object GetValue(string key)
        {
            MetaTagCollectionItem item = items.Where(a => a.Key == key).FirstOrDefault();
            if (item != null)
                return item.Value;
            return String.Empty;
        }

        public void SetValue(string key, object value)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));

            MetaTagCollectionItem item = items.Where(a => a.Key == key).FirstOrDefault();

            if (value == null)
            {
                items.Remove(item);
                return;
            }

            if (item != null)
                item.Value = value;
            else
                items.Add(new MetaTagCollectionItem(key, value));
        }

        public void SetValue(string key)
        {
            SetValue(key, String.Empty);
        }

        public IEnumerator<MetaTagCollectionItem> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class MetaTagCollectionItem
    {
        public MetaTagCollectionItem(string key, object value)
        {
            this.key = key;
            this.Value = value;
        }

        string key;
        public string Key { get { return key; } }

        public object Value { get; set; }
    }
}

namespace ConwayNativityDirectory.PluginApi
{
    public enum ImageMode { Linked, Embedded }
}
