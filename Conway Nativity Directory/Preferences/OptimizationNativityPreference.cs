using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Xml;
using System.Collections.Generic;
using System.Text.Json;
using System.Linq;
using System.IO;

namespace Conway_Nativity_Directory
{
    public class OptimizationNativityPreference : TreeItem, IPreference, ITreeItem
    {
        public OptimizationNativityPreference()
        {
            ui = new OptimizationNativityPreferenceUI(this);
        }


        #region Public Properties

        /// <summary>
        /// Gets the title of the preference.
        /// </summary>
        public string Title { get { return "Nativity"; } }

        /// <summary>
        /// Indicates whether the preference is shown in a <see cref="PreferencesWindow"/> or not.
        /// </summary>
        public bool Visible { get { return true; } }

        /// <summary>
        /// Indicates wether or not any changes will take effect the next time the application is restarted.
        /// </summary>
        public bool EffectiveImmediately { get { return true; } }

        #endregion


        #region Dependency Properties


        public static readonly DependencyProperty CopyImagesToFolderProperty = DependencyProperty.Register(
            nameof(CopyImagesToFolder), typeof(bool), typeof(OptimizationNativityPreference),
            new PropertyMetadata(false));

        public bool CopyImagesToFolder
        {
            get => (bool)GetValue(CopyImagesToFolderProperty);
            set => SetValue(CopyImagesToFolderProperty, value);
        }


        public static readonly DependencyProperty CopyImagesFolderPathProperty = DependencyProperty.Register(
            nameof(CopyImagesFolderPath), typeof(string), typeof(OptimizationNativityPreference),
            new PropertyMetadata(null));

        public string CopyImagesFolderPath
        {
            get => (string)GetValue(CopyImagesFolderPathProperty);
            set => SetValue(CopyImagesFolderPathProperty, value);
        }


        public static readonly DependencyProperty SearchForImagesProperty = DependencyProperty.Register(
            nameof(SearchForImages), typeof(bool), typeof(OptimizationNativityPreference),
            new PropertyMetadata(false));

        public bool SearchForImages
        {
            get => (bool)GetValue(SearchForImagesProperty);
            set => SetValue(SearchForImagesProperty, value);
        }


        public static readonly DependencyProperty ImageSearchFoldersProperty = DependencyProperty.Register(
            nameof(ImageSearchFolders), typeof(ObservableCollection<string>), typeof(OptimizationNativityPreference),
            new PropertyMetadata(new ObservableCollection<string>()));

        public ObservableCollection<string> ImageSearchFolders
        {
            get => (ObservableCollection<string>)GetValue(ImageSearchFoldersProperty);
            set => SetValue(ImageSearchFoldersProperty, value);
        }


        #endregion


        #region Public Methods

        public void Exceptions()
        {
            if (ui.CopyImagesToFolder && !Directory.Exists(ui.CopyImagesFolderPath))
                throw new PreferenceException("The specified \"Auto Save Folder\" is not an existing folder path.", this);

            foreach (IPreference preference in Items)
            {
                preference.Exceptions();
            }
        }

        public void Apply()
        {
            CopyImagesToFolder = ui.CopyImagesToFolder;
            CopyImagesFolderPath = ui.CopyImagesFolderPath;
            SearchForImages = ui.SearchForImages;
            ImageSearchFolders = ui.ImageSearchFolders;

            foreach (IPreference preference in Items)
            {
                preference.Apply();
            }
        }

        public void Save(XmlWriter writer)
        {
            writer.WriteStartElement(nameof(OptimizationNativityPreference));
            writer.WriteAttributeString("type", "preference");

            //settings
            writer.WriteAttributeString(nameof(CopyImagesToFolder), CopyImagesToFolder.ToString());
            writer.WriteAttributeString(nameof(CopyImagesFolderPath), CopyImagesFolderPath);
            writer.WriteAttributeString(nameof(SearchForImages), SearchForImages.ToString());
            writer.WriteAttributeString(nameof(ImageSearchFolders), JsonSerializer.Serialize(ImageSearchFolders.ToArray()));

            //Save Items
            foreach (IPreference preference in Items)
            {
                preference.Save(writer);
            }

            writer.WriteEndElement();
        }

        public void Reset()
        {
            ui.CopyImagesToFolder = false;
            ui.SearchForImages = false;
        }

        public void ResetAll()
        {
            Reset();
            foreach (IPreference preference in Items)
            {
                preference.ResetAll();
            }
        }

        private OptimizationNativityPreferenceUI ui;
        public UIElement ShowUI()
        {
            ui.CopyImagesFolderPath = CopyImagesFolderPath;
            ui.CopyImagesToFolder = CopyImagesToFolder;
            ui.SearchForImages = SearchForImages;
            ui.ImageSearchFolders = ImageSearchFolders;
            return ui;
        }

        public void CloseUI(bool recrusive)
        {
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

        public static OptimizationNativityPreference Load(XmlNode node)
        {
            OptimizationNativityPreference preference = new OptimizationNativityPreference();

            foreach (XmlAttribute attribute in node.Attributes)
            {
                //Load the settings of this preference from attributes.
                if (attribute.LocalName == nameof(CopyImagesToFolder))
                    preference.CopyImagesToFolder = bool.Parse(attribute.Value);
                else if (attribute.LocalName == nameof(CopyImagesFolderPath))
                    preference.CopyImagesFolderPath = attribute.Value;
                else if (attribute.LocalName == nameof(SearchForImages))
                    preference.SearchForImages = bool.Parse(attribute.Value);
                else if (attribute.LocalName == nameof(ImageSearchFolders))
                    preference.ImageSearchFolders = new ObservableCollection<string>(JsonSerializer.Deserialize<string[]>(attribute.Value));
            }

            List<string> localNames = new List<string>();
            foreach (XmlNode child in node.ChildNodes)
            {
                localNames.Add(child.LocalName);
                preference.Items.Add(Preferences.LoadPreference(child));
            }

            return preference;
        }

        #endregion
    }
}