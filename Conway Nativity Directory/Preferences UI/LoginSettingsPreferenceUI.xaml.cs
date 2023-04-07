using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace Conway_Nativity_Directory
{
    /// <summary>
    /// Interaction logic for LoginSettingsPreferenceUI.xaml
    /// </summary>
    public partial class LoginSettingsPreferenceUI : UserControl
    {
        private LoginSettingsPreference loginSettingsPreference;
        public LoginSettingsPreference LoginSettingsPreference { get { return loginSettingsPreference; } }
        public LoginSettingsPreferenceUI(LoginSettingsPreference loginSettingsPreference)
        {
            InitializeComponent();
            this.loginSettingsPreference = loginSettingsPreference;
            SetBindings();
        }

        private void SetBindings()
        {
            App.SetBinding("Password", loginSettingsPreference, passwordTextBox, TextBox.TextProperty,
                BindingMode.OneWay, UpdateSourceTrigger.Default);
        }
    }
}
