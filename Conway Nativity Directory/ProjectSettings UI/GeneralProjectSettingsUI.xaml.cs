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

namespace Conway_Nativity_Directory
{
    /// <summary>
    /// Interaction logic for GeneralProjectSettingsUI.xaml
    /// </summary>
    public partial class GeneralProjectSettingsUI : UserControl
    {
        GeneralProjectSettings settings;
        public GeneralProjectSettings Settings { get { return settings; } }

        public GeneralProjectSettingsUI(GeneralProjectSettings settings)
        {
            InitializeComponent();
            this.settings = settings;
            SetBindings();
        }

        private void SetBindings()
        {
            App.SetBinding("ProjectName", settings, projectNameTextBox, TextBox.TextProperty,
                BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);
        }
    }
}
