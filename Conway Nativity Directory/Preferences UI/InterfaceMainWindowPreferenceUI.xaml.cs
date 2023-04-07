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
using CustRes;
using Microsoft.Win32;

namespace Conway_Nativity_Directory
{
    /// <summary>
    /// Interaction logic for InterfaceMainWindowPreferenceUI.xaml
    /// </summary>
    public partial class InterfaceMainWindowPreferenceUI : UserControl
    {
        private InterfaceMainWindowPreference interfaceMainWindowPreference;
        public InterfaceMainWindowPreference InterfaceMainWindowPreference { get { return interfaceMainWindowPreference; } }
        public InterfaceMainWindowPreferenceUI(InterfaceMainWindowPreference interfaceMainWindowPreference)
        {
            InitializeComponent();
            this.interfaceMainWindowPreference = interfaceMainWindowPreference;
            populateComboBox();
            SetBindings();
        }

        private void populateComboBox()
        {
            nativityInformationFontFamilyComboBox.ItemsSource = Fonts.SystemFontFamilies;
        }

        private void Border_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();

                if (((Border)sender).Background is SolidColorBrush)
                {
                    Color color = (((Border)sender).Background as SolidColorBrush).Color;
                    colorDialog.Color = System.Drawing.Color.FromArgb(color.R, color.G, color.B);
                }

                if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    System.Drawing.Color color = colorDialog.Color;
                    ((Border)sender).Background = new SolidColorBrush(Color.FromRgb(color.R, color.G, color.B));
                }
            }
        }

        private void SetBindings()
        {
            //This binding does not work properly because something is wrong in the CustRes library so I have to set it manually.
            //As a result, it will not update if the preference setting is updated
            //and will only load when the ui loads.
            App.SetBinding("NativityInformationFontSize", interfaceMainWindowPreference,
                nativityInformationFontSizeNumericUpDown, NumericUpDown.ValueProperty,
                BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            //setting it manually
            nativityInformationFontSizeNumericUpDown.Value = interfaceMainWindowPreference.NativityInformationFontSize;

            App.SetBinding("TNativityInformationFontFamily", interfaceMainWindowPreference,
                nativityInformationFontFamilyComboBox, ComboBox.TextProperty,
                BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            //tag colors
            App.SetBinding("TButtonsBrush", interfaceMainWindowPreference,
                tButtonsBorder, Border.BackgroundProperty,
                BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TButtonsMouseOverBrush", interfaceMainWindowPreference,
                tButtonsMouseOverBorder, Border.BackgroundProperty,
                BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TEditingButtonsBrush", interfaceMainWindowPreference,
                tEditingButtonsBorder, Border.BackgroundProperty,
                BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TEditingButtonsMouseOverBrush", interfaceMainWindowPreference,
                tEditingButtonsMouseOverBorder, Border.BackgroundProperty,
                BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TEditingTagsBrush", interfaceMainWindowPreference,
                tEditingTagsBorder, Border.BackgroundProperty,
                BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TEditingTagsMouseOverBrush", interfaceMainWindowPreference,
                tEditingTagsMouseOverBorder, Border.BackgroundProperty,
                BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TEditingForeground", interfaceMainWindowPreference,
                tEditingForegroundBorder, Border.BackgroundProperty,
                BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TEditingMouseOverForeground", interfaceMainWindowPreference,
                tEditingMouseOverForegroundBorder, Border.BackgroundProperty,
                BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TMovingTagsBorderBrush", interfaceMainWindowPreference,
                tMovingTagsBorder, Border.BackgroundProperty,
                BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TNewTagBrush", interfaceMainWindowPreference,
                tNewTagBorder, Border.BackgroundProperty,
                BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TNewTagButtonsBrush", interfaceMainWindowPreference,
                tNewTagButtonsBorder, Border.BackgroundProperty,
                BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TNewTagButtonsMouseOverBrush", interfaceMainWindowPreference,
                tNewTagButtonsMouseOverBorder, Border.BackgroundProperty,
                BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TNewTagForeground", interfaceMainWindowPreference,
                tNewTagForegroundBorder, Border.BackgroundProperty,
                BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TTagButtonsBrush", interfaceMainWindowPreference,
                tTagButtonsBorder, Border.BackgroundProperty,
                BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TTagButtonsMouseOverBrush", interfaceMainWindowPreference,
                tTagButtonsMouseOverBorder, Border.BackgroundProperty,
                BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TTagsBrush", interfaceMainWindowPreference,
                tTagsBorder, Border.BackgroundProperty,
                BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TTagsMouseOverBrush", interfaceMainWindowPreference,
                tTagsMouseOverBorder, Border.BackgroundProperty,
                BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("TForeground", interfaceMainWindowPreference,
                tForegroundBorder, Border.BackgroundProperty,
                BindingMode.OneWay, UpdateSourceTrigger.PropertyChanged);
        }

        //ButtonsBrush;
        //ButtonsMouseOverBrush;
        //EditingButtonsBrush;
        //EditingButtonsMouseOverBrush;
        //EditingTagsBrush;
        //EditingTagsMouseOverBrush;
        //MovingTagsBorderBrush;
        //NewTagBrush;
        //NewTagButtonsBrush;
        //NewTagButtonsMouseOverBrush;
        //TagButtonsBrush;
        //TagButtonsMouseOverBrush;
        //TagsBrush;
        //TagsMouseOverBrush;-->
    }
}
