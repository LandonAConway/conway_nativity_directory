using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;

namespace Conway_Nativity_Directory
{
    public class Preferences
    {
        /// <summary>
        /// Gets a collection of preferences.
        /// </summary>
        public ObservableCollection<object> Items { get { return items; } }
        private ObservableCollection<object> items = new ObservableCollection<object>();


        #region Public Methods

        /// <summary>
        /// Throws exceptions to prevent errors when applying and saving preferences.
        /// </summary>
        public void Exceptions()
        {
            foreach (IPreference preference in Items)
            {
                preference.Exceptions();
            }
        }

        /// <summary>
        /// Applies the preferences based on the interacted UI but does not save them to a file.
        /// </summary>
        public void Apply()
        {
            foreach (IPreference preference in Items)
            {
                preference.Apply();
            }
        }


        /// <summary>
        /// Saves preferences to a XML file.
        /// </summary>
        /// <param name="filePath">Specifies where to save the XML file.</param>
        public void Save(string filePath)
        {
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true
            };

            XmlWriter writer = XmlWriter.Create(filePath, settings);

            using (writer)
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Preferences");

                foreach (IPreference preference in Items)
                {
                    preference.Save(writer);
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        public void ResetAll()
        {
            foreach (IPreference preference in Items)
            {
                preference.ResetAll();
            }
        }


        public void CloseUI()
        {
            foreach (IPreference preference in Items)
            {
                preference.CloseUI(true);
            }
        }


        #region GetPreference

        /// <summary>
        /// Gets the preference from a path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public IPreference GetPreference(string path)
        {
            paths = new Dictionary<string, IPreference>();
            foreach (IPreference preference in Items)
            {
                BuildPath(preference);
                currentPath = String.Empty;
            }

            IPreference result = null;

            foreach (KeyValuePair<string, IPreference> pair in paths)
            {
                if (pair.Key == path || pair.Key == path + @"\")
                {
                    result = pair.Value;
                }
            }

            return result;
        }

        Dictionary<string, IPreference> paths = new Dictionary<string, IPreference>();

        string currentPath = String.Empty;
        List<string> pathSegments = new List<string>();

        private void BuildPath(IPreference preference)
        {
            currentPath = currentPath + preference.Title + @"\";
            pathSegments.Add(preference.Title);

            if (!paths.ContainsKey(currentPath))
                paths.Add(string.Join(@"\", pathSegments), preference);

            foreach (IPreference p in preference.Items)
            {
                BuildPath(p);
            }

            pathSegments.Remove(pathSegments[pathSegments.Count - 1]);
        }

        #endregion


        #endregion


        #region Static

        /// <summary>
        /// Loads preferences from a XML file.
        /// </summary>
        /// <param name="filePath">Specifies where to load the XML file.</param>
        /// <returns></returns>
        public static Preferences Load(string filePath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNode root = doc.SelectSingleNode("*");

            Preferences preferences = new Preferences();

            foreach (XmlNode node in root.ChildNodes)
            {
                preferences.Items.Add(LoadPreference(node));
            }

            return preferences;
        }

        public static IPreference LoadPreference(XmlNode node)
        {
            switch (node.LocalName)
            {
                case (nameof(SecurityPreference)):
                    return SecurityPreference.Load(node);

                case (nameof(LoginSettingsPreference)):
                    return LoginSettingsPreference.Load(node);

                case (nameof(OptimizationPreference)):
                    return OptimizationPreference.Load(node);

                case (nameof(OptimizationNativityPreference)):
                    return OptimizationNativityPreference.Load(node);

                case (nameof(StartupPreference)):
                    return StartupPreference.Load(node);

                case (nameof(AutoSavePreference)):
                    return AutoSavePreference.Load(node);

                case (nameof(InterfacePreference)):
                    return InterfacePreference.Load(node);

                case (nameof(InterfaceMainWindowPreference)):
                    return InterfaceMainWindowPreference.Load(node);

                case (nameof(ColumnsPreference)):
                    return ColumnsPreference.Load(node);

                case (nameof(PerformancePreference)):
                    return PerformancePreference.Load(node);

                case (nameof(CachePreference)):
                    return CachePreference.Load(node);

                default:
                    return null;
            }
        }

        #endregion

    }
}