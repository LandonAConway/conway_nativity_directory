using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xaml;
using System.Xml;

namespace Conway_Nativity_Directory
{
    public class LoginSettingsPreference : TreeItem, IPreference, ITreeItem
    {


        #region Constructor

        public LoginSettingsPreference()
        {
            ui = new LoginSettingsPreferenceUI(this);
        }

        #endregion


        #region Dependency Properties

        private static readonly DependencyPropertyKey PasswordPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(Password), typeof(string), typeof(LoginSettingsPreference),
                new PropertyMetadata("", PasswordPropertyChangedCallback));

        public static readonly DependencyProperty PasswordProperty =
            PasswordPropertyKey.DependencyProperty;

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            protected set { SetValue(PasswordPropertyKey, value); }
        }


        private static readonly DependencyPropertyKey IsLoggedInPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(IsLoggedIn), typeof(bool), typeof(LoginSettingsPreference),
                new PropertyMetadata(false, IsLoggedInPropertyChangedCallback));

        public static readonly DependencyProperty IsLoggedInProperty =
            IsLoggedInPropertyKey.DependencyProperty;

        public bool IsLoggedIn
        {
            get { return (bool)GetValue(IsLoggedInProperty); }
            protected set { SetValue(IsLoggedInPropertyKey, value); }
        }


        private static readonly DependencyPropertyKey IsPasswordProtectedPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(IsPasswordProtected), typeof(bool), typeof(LoginSettingsPreference),
                new PropertyMetadata(false, IsPasswordProtectedPropertyChangedCallback));

        public static readonly DependencyProperty IsPasswordProtectedProperty =
            IsPasswordProtectedPropertyKey.DependencyProperty;

        public bool IsPasswordProtected
        {
            get { return (bool) GetValue(IsPasswordProtectedProperty); }
            protected set { SetValue(IsPasswordProtectedPropertyKey, value); }
        }


        private static readonly DependencyPropertyKey AllowEditingDependencyPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(AllowEditing), typeof(bool), typeof(LoginSettingsPreference),
                new PropertyMetadata(true));

        public static readonly DependencyProperty AllowEditingDependencyProperty =
            AllowEditingDependencyPropertyKey.DependencyProperty;

        public bool AllowEditing
        {
            get { return (bool) GetValue(AllowEditingDependencyProperty); }
            set { SetValue(AllowEditingDependencyPropertyKey, value); }
        }

        #region Callbacks

        private static void PasswordPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var loginSettings = (LoginSettingsPreference)d;
            loginSettings.IsPasswordProtected = ((string)e.NewValue == String.Empty || (string)e.NewValue == "") ? false : true;
        }
        
        private static void IsLoggedInPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var loginSettings = (LoginSettingsPreference) d;

            if ((bool) e.NewValue)
            {
                loginSettings.AllowEditing = true;
            }

            else if (!(bool) e.NewValue && !loginSettings.IsPasswordProtected)
            {
                loginSettings.AllowEditing = true;
            }

            else
            {
                loginSettings.AllowEditing = false;
            }
        }

        private static void IsPasswordProtectedPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var loginSettings = (LoginSettingsPreference) d;

            if (loginSettings.IsLoggedIn)
            {
                loginSettings.AllowEditing = true;
            }

            else if (!loginSettings.IsLoggedIn && !(bool) e.NewValue)
            {
                loginSettings.AllowEditing = true;
            }

            else
            {
                loginSettings.AllowEditing = false;
            }
        }

        #endregion

        #endregion


        #region Public Properties

        /// <summary>
        /// Gets the title of the preference.
        /// </summary>
        public string Title { get { return "Login Settings"; } }

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

        private LoginSettingsPreferenceUI ui;

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
            this.Password = ui.passwordTextBox.Text;
            Login(Password);

            foreach (IPreference preference in Items)
            {
                preference.Apply();
            }
        }

        public void Save(XmlWriter writer)
        {
            writer.WriteStartElement(nameof(LoginSettingsPreference));
            writer.WriteAttributeString("type", "preference");

            //settings
            writer.WriteAttributeString(nameof(Password), Password);

            //Save Items
            foreach (IPreference preference in Items)
            {
                preference.Save(writer);
            }

            writer.WriteEndElement();
        }

        public void Reset()
        {

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
            ui = new LoginSettingsPreferenceUI(this);

            if (recrusive)
            {
                foreach (IPreference preference in Items)
                {
                    preference.CloseUI(recrusive);
                }
            }
        }

        public bool Login(string password)
        {
            if (IsPasswordProtected)
            {
                if (password == Password)
                {
                    IsLoggedIn = true;
                    return true;
                }

                else
                {
                    IsLoggedIn = false;
                    return false;
                }
            }

            else
            {
                IsLoggedIn = false;
                return true;
            }
        }

        public void Logout()
        {
            IsLoggedIn = false;
        }

        #endregion


        #region Static Methods

        public static LoginSettingsPreference Load(XmlNode node)
        {
            LoginSettingsPreference preference = new LoginSettingsPreference();

            foreach (XmlAttribute attribute in node.Attributes)
            {
                if (attribute.LocalName == nameof(Password))
                    preference.Password = attribute.Value;
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