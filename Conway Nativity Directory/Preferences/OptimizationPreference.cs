using System.Windows;
using System.Xml;
using System.Collections.Generic;

namespace Conway_Nativity_Directory
{
    public class OptimizationPreference : TreeItem, IPreference, ITreeItem
    {

        #region Public Properties

        /// <summary>
        /// Gets the title of the preference.
        /// </summary>
        public string Title { get { return "Optimization"; } }

        /// <summary>
        /// Indicates whether the preference is shown in a <see cref="PreferencesWindow"/> or not.
        /// </summary>
        public bool Visible { get { return true; } }

        /// <summary>
        /// Indicates wether or not any changes will take effect the next time the application is restarted.
        /// </summary>
        public bool EffectiveImmediately { get { return true; } }

        #endregion


        #region Private Properties

        #region For get-only properties


        #endregion

        #endregion


        #region Public Methods

        public void Exceptions()
        {
            foreach (IPreference preference in Items)
            {
                preference.Exceptions();
            }
        }

        public void Apply()
        {
            foreach (IPreference preference in Items)
            {
                preference.Apply();
            }
        }

        public void Save(XmlWriter writer)
        {
            writer.WriteStartElement(nameof(OptimizationPreference));
            writer.WriteAttributeString("type", "preference");

            //Save the settings of this preference as attributes.

            //Save Items
            foreach (IPreference preference in Items)
            {
                preference.Save(writer);
            }

            writer.WriteEndElement();
        }

        public void Reset()
        {
            //Reset everything inside this preference.
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
            return null;
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

        public static OptimizationPreference Load(XmlNode node)
        {
            OptimizationPreference preference = new OptimizationPreference();

            foreach (XmlAttribute attribute in node.Attributes)
            {
                //Load the settings of this preference from attributes.
            }

            List<string> localNames = new List<string>();
            foreach (XmlNode child in node.ChildNodes)
            {
                localNames.Add(child.LocalName);
                preference.Items.Add(Preferences.LoadPreference(child));
            }

            if (!localNames.Contains(nameof(OptimizationNativityPreference)))
                preference.Items.Add(new OptimizationNativityPreference());

            return preference;
        }

        #endregion
    }
}