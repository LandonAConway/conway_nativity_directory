using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml;

namespace Conway_Nativity_Directory
{
    public class AutoSavePreference : TreeItem, IPreference, ITreeItem
    {


        #region Constructor

        public AutoSavePreference()
        {
            ui = new AutoSavePreferenceUI(this);
        }

        #endregion


        #region Dependency Properties


        public static readonly DependencyProperty AutoSaveEnabledProperty =
            DependencyProperty.Register("AutoSaveEnabled", typeof(bool), typeof(AutoSavePreference),
                new PropertyMetadata(false));

        public bool AutoSaveEnabled
        {
            get { return (bool)GetValue(AutoSaveEnabledProperty); }
            set { SetValue(AutoSaveEnabledProperty, value); }
        }


        public static readonly DependencyProperty AutoSaveFolderProperty =
            DependencyProperty.Register("AutoSaveFolder", typeof(string), typeof(AutoSavePreference),
                new PropertyMetadata(AppDomain.CurrentDomain.BaseDirectory +
                    @"\bin\auto-save"));

        public string AutoSaveFolder
        {
            get { return (string)GetValue(AutoSaveFolderProperty); }
            set { SetValue(AutoSaveFolderProperty, value); }
        }


        public static readonly DependencyProperty AutoSaveIncrementProperty =
            DependencyProperty.Register("AutoSaveIncrement", typeof(double), typeof(AutoSavePreference),
                new PropertyMetadata(5.0));

        public double AutoSaveIncrement
        {
            get {  return (double)GetValue(AutoSaveIncrementProperty); }
            set {  SetValue(AutoSaveIncrementProperty, value); }
        }


        public static readonly DependencyProperty AutoSaveAgeLimitProperty =
            DependencyProperty.Register("AutoSaveAgeLimit", typeof(TimeSpan), typeof(AutoSavePreference),
                new PropertyMetadata(TimeSpan.FromMinutes(30)));

        public TimeSpan AutoSaveAgeLimit
        {
            get {  return (TimeSpan)GetValue(AutoSaveAgeLimitProperty); }
            set {  SetValue(AutoSaveAgeLimitProperty, value); }
        }


        #endregion


        #region Public Properties

        /// <summary>
        /// Gets the title of the preference.
        /// </summary>
        public string Title { get { return "Auto-Save"; } }

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
            AutoSavePreferenceUI _ui = (AutoSavePreferenceUI)ui;

            if (!Directory.Exists(_ui.autoSaveFolderTextBox.Text))
                throw new PreferenceException("The specified \"Auto Save Folder\" is not an existing folder path.", this);

            foreach (IPreference preference in Items)
            {
                preference.Exceptions();
            }
        }

        public void Apply()
        {
            //StartupPreferenceUI _ui = (StartupPreferenceUI)ui;

            foreach (IPreference preference in Items)
            {
                preference.Apply();
            }
        }

        public void Save(XmlWriter writer)
        {
            writer.WriteStartElement(nameof(AutoSavePreference));
            writer.WriteAttributeString("type", "preference");

            //settings
            AutoSavePreferenceUI _ui = (AutoSavePreferenceUI)ui;
            writer.WriteAttributeString("autoSaveEnabled", _ui.autoSaveEnabledCheckBox.IsChecked.ToString());
            writer.WriteAttributeString("autoSaveFolder", _ui.autoSaveFolderTextBox.Text);
            writer.WriteAttributeString("autoSaveIncrement", _ui.autoSaveIncrement.Value.ToString());
            writer.WriteAttributeString("autoSaveAgeLimit", _ui.AutoSaveAgeLimit.TotalSeconds.ToString());

            //Save Items
            foreach (IPreference preference in Items)
            {
                preference.Save(writer);
            }

            writer.WriteEndElement();
        }

        public void Reset()
        {
            AutoSavePreferenceUI _ui = (AutoSavePreferenceUI)ui;
            _ui.autoSaveFolderTextBox.Text = (string)AutoSavePreference.AutoSaveFolderProperty.DefaultMetadata.DefaultValue;
            _ui.autoSaveIncrement.Value = (double)AutoSavePreference.AutoSaveIncrementProperty.DefaultMetadata.DefaultValue;
            _ui.AutoSaveAgeLimit = (TimeSpan)AutoSavePreference.AutoSaveAgeLimitProperty.DefaultMetadata.DefaultValue;
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
            ui = new AutoSavePreferenceUI(this);

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

        public static AutoSavePreference Load(XmlNode node)
        {
            AutoSavePreference preference = new AutoSavePreference();

            foreach (XmlAttribute attribute in node.Attributes)
            {
                if (attribute.LocalName == "autoSaveEnabled")
                    preference.AutoSaveEnabled = Boolean.Parse(attribute.Value);
                else if (attribute.LocalName == "autoSaveFolder")
                    preference.AutoSaveFolder = attribute.Value;
                else if (attribute.LocalName == "autoSaveIncrement")
                    preference.AutoSaveIncrement = Convert.ToDouble(attribute.Value);
                else if (attribute.LocalName == "autoSaveAgeLimit")
                    preference.AutoSaveAgeLimit = TimeSpan.FromSeconds(Convert.ToDouble(attribute.Value));
            }

            preference.ui = new AutoSavePreferenceUI(preference);

            foreach (XmlNode child in node.ChildNodes)
            {
                preference.Items.Add(Preferences.LoadPreference(child));
            }

            return preference;
        }

        #endregion


    }
}