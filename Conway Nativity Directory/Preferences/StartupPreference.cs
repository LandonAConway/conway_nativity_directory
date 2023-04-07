using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml;

namespace Conway_Nativity_Directory
{
    public class StartupPreference : TreeItem, IPreference, ITreeItem
    {


        #region Constructor

        public StartupPreference()
        {
            ui = new StartupPreferenceUI(this);
        }

        #endregion


        #region Dependency Properties

        public static readonly DependencyProperty AutoLoadFileProperty =
            DependencyProperty.Register("AutoLoadFile", typeof(string), typeof(StartupPreference),
                new PropertyMetadata());

        public string AutoLoadFile
        {
            get { return (string)GetValue(AutoLoadFileProperty); }
            set { SetValue(AutoLoadFileProperty, value); }
        }

        #endregion


        #region Public Properties

        /// <summary>
        /// Gets the title of the preference.
        /// </summary>
        public string Title { get { return "Startup"; } }

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
            //StartupPreferenceUI _ui = (StartupPreferenceUI)ui;

            if (!String.IsNullOrEmpty(AutoLoadFile) && String.IsNullOrWhiteSpace(AutoLoadFile))
            {
                if (!File.Exists(AutoLoadFile))
                    throw new FileNotFoundException("Auto-Load file path does not exist.");
            }

            foreach (IPreference preference in Items)
            {
                preference.Exceptions();
            }
        }

        public void Apply()
        {
            StartupPreferenceUI _ui = (StartupPreferenceUI)ui;

            foreach (IPreference preference in Items)
            {
                preference.Apply();
            }
        }

        public void Save(XmlWriter writer)
        {
            writer.WriteStartElement(nameof(StartupPreference));
            writer.WriteAttributeString("type", "preference");

            //settings
            StartupPreferenceUI _ui = (StartupPreferenceUI)ui;
            writer.WriteAttributeString("autoLoadFile", _ui.fileTextBox.Text);

            //Save Items
            foreach (IPreference preference in Items)
            {
                preference.Save(writer);
            }

            writer.WriteEndElement();
        }

        public void Reset()
        {
            StartupPreference _ui = (StartupPreference)ui;
            //_ui.cacheFolderTextBox.Text = (string)StartupPreference.AutoLoadFileProperty.DefaultMetadata.DefaultValue;
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
            //ui = new StartupPreferenceUI();

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

        public static StartupPreference Load(XmlNode node)
        {
            StartupPreference preference = new StartupPreference();

            foreach (XmlAttribute attribute in node.Attributes)
            {
                if (attribute.LocalName == "autoLoadFile")
                {
                    preference.AutoLoadFile = attribute.Value;
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