using System;
using System.Windows;
using System.Xml;

namespace Conway_Nativity_Directory
{
    public class ColumnsPreference : TreeItem, IPreference, ITreeItem
    {


        #region Dependency Properties

        public static readonly DependencyProperty ShowIdProperty =
            DependencyProperty.Register(nameof(ShowId), typeof(bool), typeof(ColumnsPreference),
                new PropertyMetadata(true));

        public bool ShowId
        {
            get { return (bool)GetValue(ShowIdProperty); }
            set { SetValue(ShowIdProperty, value); }
        }


        public static readonly DependencyProperty ShowTitleProperty =
            DependencyProperty.Register(nameof(ShowTitle), typeof(bool), typeof(ColumnsPreference),
                new PropertyMetadata(true));

        public bool ShowTitle
        {
            get { return (bool)GetValue(ShowTitleProperty); }
            set { SetValue(ShowTitleProperty, value); }
        }


        public static readonly DependencyProperty ShowOriginProperty =
            DependencyProperty.Register(nameof(ShowOrigin), typeof(bool), typeof(ColumnsPreference),
                new PropertyMetadata(true));

        public bool ShowOrigin
        {
            get { return (bool)GetValue(ShowOriginProperty); }
            set { SetValue(ShowOriginProperty, value); }
        }


        public static readonly DependencyProperty ShowAcquiredProperty =
            DependencyProperty.Register(nameof(ShowAcquired), typeof(bool), typeof(ColumnsPreference),
                new PropertyMetadata(true));

        public bool ShowAcquired
        {
            get { return (bool)GetValue(ShowAcquiredProperty); }
            set { SetValue(ShowAcquiredProperty, value); }
        }


        public static readonly DependencyProperty ShowFromProperty =
            DependencyProperty.Register(nameof(ShowFrom), typeof(bool), typeof(ColumnsPreference),
                new PropertyMetadata(true));

        public bool ShowFrom
        {
            get { return (bool)GetValue(ShowFromProperty); }
            set { SetValue(ShowFromProperty, value); }
        }


        public static readonly DependencyProperty ShowCostProperty =
            DependencyProperty.Register(nameof(ShowCost), typeof(bool), typeof(ColumnsPreference),
                new PropertyMetadata(true));

        public bool ShowCost
        {
            get { return (bool)GetValue(ShowCostProperty); }
            set { SetValue(ShowCostProperty, value); }
        }


        public static readonly DependencyProperty ShowLocationProperty =
            DependencyProperty.Register(nameof(ShowLocation), typeof(bool), typeof(ColumnsPreference),
                new PropertyMetadata(true));

        public bool ShowLocation
        {
            get { return (bool)GetValue(ShowLocationProperty); }
            set { SetValue(ShowLocationProperty, value); }
        }


        public static readonly DependencyProperty ShowTagsProperty =
            DependencyProperty.Register(nameof(ShowTags), typeof(bool), typeof(ColumnsPreference),
                new PropertyMetadata(true));

        public bool ShowTags
        {
            get { return (bool)GetValue(ShowTagsProperty); }
            set { SetValue(ShowTagsProperty, value); }
        }


        public static readonly DependencyProperty ShowGeographicalOriginsProperty =
            DependencyProperty.Register(nameof(ShowGeographicalOrigins), typeof(bool), typeof(ColumnsPreference),
                new PropertyMetadata(true));

        public bool ShowGeographicalOrigins
        {
            get { return (bool)GetValue(ShowGeographicalOriginsProperty); }
            set { SetValue(ShowGeographicalOriginsProperty, value); }
        }

        #endregion


        #region Public Properties

        /// <summary>
        /// Gets the title of the preference.
        /// </summary>
        public string Title { get { return "Columns"; } }

        /// <summary>
        /// Indicates whether the preference is shown in a <see cref="PreferencesWindow"/> or not.
        /// </summary>
        public bool Visible { get { return false; } }

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
            writer.WriteStartElement(nameof(ColumnsPreference));
            writer.WriteAttributeString("type", "preference");

            writer.WriteAttributeString(nameof(ShowId), ShowId.ToString());
            writer.WriteAttributeString(nameof(ShowTitle), ShowTitle.ToString());
            writer.WriteAttributeString(nameof(ShowOrigin), ShowOrigin.ToString());
            writer.WriteAttributeString(nameof(ShowAcquired), ShowAcquired.ToString());
            writer.WriteAttributeString(nameof(ShowFrom), ShowFrom.ToString());
            writer.WriteAttributeString(nameof(ShowCost), ShowCost.ToString());
            writer.WriteAttributeString(nameof(ShowLocation), ShowLocation.ToString());
            writer.WriteAttributeString(nameof(ShowTags), ShowTags.ToString());
            writer.WriteAttributeString(nameof(ShowGeographicalOrigins), ShowGeographicalOrigins.ToString());

            //Save Items
            foreach (IPreference preference in Items)
            {
                preference.Save(writer);
            }

            writer.WriteEndElement();
        }

        public void Reset()
        {
            ShowId = true;
            ShowTitle = true;
            ShowOrigin = true;
            ShowAcquired = true;
            ShowFrom = true;
            ShowCost = true;
            ShowLocation = true;
            ShowTags = true;
            ShowGeographicalOrigins = true;
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

        public static ColumnsPreference Load(XmlNode node)
        {
            ColumnsPreference preference = new ColumnsPreference();

            foreach (XmlAttribute attribute in node.Attributes)
            {
                switch (attribute.LocalName)
                {
                    case nameof(ShowId):
                        preference.ShowId = Boolean.Parse(attribute.Value);
                        break;

                    case nameof(ShowTitle):
                        preference.ShowTitle = Boolean.Parse(attribute.Value);
                        break;

                    case nameof(ShowOrigin):
                        preference.ShowOrigin = Boolean.Parse(attribute.Value);
                        break;

                    case nameof(ShowAcquired):
                        preference.ShowAcquired = Boolean.Parse(attribute.Value);
                        break;

                    case nameof(ShowFrom):
                        preference.ShowFrom = Boolean.Parse(attribute.Value);
                        break;

                    case nameof(ShowCost):
                        preference.ShowCost = Boolean.Parse(attribute.Value);
                        break;

                    case nameof(ShowLocation):
                        preference.ShowLocation = Boolean.Parse(attribute.Value);
                        break;

                    case nameof(ShowTags):
                        preference.ShowTags = Boolean.Parse(attribute.Value);
                        break;

                    case nameof(ShowGeographicalOrigins):
                        preference.ShowGeographicalOrigins = Boolean.Parse(attribute.Value);
                        break;
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