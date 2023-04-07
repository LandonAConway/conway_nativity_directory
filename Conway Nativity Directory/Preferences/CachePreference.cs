using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml;

namespace Conway_Nativity_Directory
{
    public class CachePreference : TreeItem, IPreference, ITreeItem
    {


        #region Constructor

        public CachePreference()
        {
            ui = new CachePreferenceUI(this);
        }

        #endregion


        #region Dependency Properties

        public static readonly DependencyProperty CacheFolderProperty =
            DependencyProperty.Register("CacheFolder", typeof(string), typeof(CachePreference),
                new PropertyMetadata(AppDomain.CurrentDomain.BaseDirectory + @"\bin\cache\"));

        public string CacheFolder
        {
            get { return (string)GetValue(CacheFolderProperty); }
            set { SetValue(CacheFolderProperty, value); }
        }

        #endregion


        #region Public Properties

        /// <summary>
        /// Gets the title of the preference.
        /// </summary>
        public string Title { get { return "Cache"; } }

        /// <summary>
        /// Indicates whether the preference is shown in a <see cref="PreferencesWindow"/> or not.
        /// </summary>
        public bool Visible { get { return true; } }

        /// <summary>
        /// Indicates wether or not any changes will take effect the next time the application is restarted.
        /// </summary>
        public bool EffectiveImmediately { get { return false; } }

        #endregion


        #region Private Properties

        #region For get-only properties

        private UIElement ui;

        #endregion

        #endregion


        #region Public Methods

        public void Exceptions()
        {
            CachePreferenceUI _ui = (CachePreferenceUI)ui;

            string oldCacheDest = CacheFolder;
            string newCacheDest = _ui.cacheFolderTextBox.Text;

            int count = 0;

            if (Directory.Exists(newCacheDest))
            {
                count = Directory.GetDirectories(newCacheDest, "*").Count() +
                        Directory.GetFiles(newCacheDest, "*.*").Count();
            }

            string path1 = Path.GetFullPath(oldCacheDest);
            string path2 = Path.GetFullPath(newCacheDest);

            if (path1 != path2)
            {
                if (count > 0)
                {
                    throw new PreferenceException("Cache folder must be empty.", this);
                }
            }

            foreach (IPreference preference in Items)
            {
                preference.Exceptions();
            }
        }

        public void Apply()
        {
            CachePreferenceUI _ui = (CachePreferenceUI)ui;

            foreach (IPreference preference in Items)
            {
                preference.Apply();
            }
        }

        public void Save(XmlWriter writer)
        {
            writer.WriteStartElement(nameof(CachePreference));
            writer.WriteAttributeString("type", "preference");

            //settings
            CachePreferenceUI _ui = (CachePreferenceUI)ui;
            writer.WriteAttributeString("cacheFolder", _ui.cacheFolderTextBox.Text);

            //Save Items
            foreach (IPreference preference in Items)
            {
                preference.Save(writer);
            }

            writer.WriteEndElement();
        }

        public void Reset()
        {
            CachePreferenceUI _ui = (CachePreferenceUI)ui;
            _ui.cacheFolderTextBox.Text = (string)CachePreference.CacheFolderProperty.DefaultMetadata.DefaultValue;
        }

        public void ResetAll()
        {
            Reset();
            foreach (IPreference preference in Items)
            {
                preference.ResetAll();
            }
        }

        public UIElement ShowUI()
        {
            return ui;
        }

        public void CloseUI(bool recrusive)
        {
            ui = new CachePreferenceUI(this);

            if (recrusive)
            {
                foreach (IPreference preference in Items)
                {
                    preference.CloseUI(recrusive);
                }
            }
        }

        #endregion


        #region Static Methods

        public static CachePreference Load(XmlNode node)
        {
            CachePreference preference = new CachePreference();

            foreach (XmlAttribute attribute in node.Attributes)
            {
                if (attribute.LocalName == "cacheFolder")
                {
                    preference.CacheFolder = attribute.Value;
                }
            }

            foreach (XmlNode child in node.ChildNodes)
            {
                preference.Items.Add(Preferences.LoadPreference(child));
            }

            return preference;
        }

        #endregion
    }
}