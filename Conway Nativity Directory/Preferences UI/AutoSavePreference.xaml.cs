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
    /// Interaction logic for AutoSavePreferenceUI.xaml
    /// </summary>
    public partial class AutoSavePreferenceUI : UserControl
    {
        private AutoSavePreference autoSavePreference;
        public AutoSavePreference AutoSavePreference { get { return autoSavePreference; } }
        public AutoSavePreferenceUI(AutoSavePreference autoSavePreference)
        {
            InitializeComponent();
            this.autoSavePreference = autoSavePreference;
            SetBindings();
            AutoSaveAgeLimitChanged(AutoSaveAgeLimit, AutoSaveAgeLimit);
        }


        public static readonly DependencyProperty AutoSaveAgeLimitProperty =
            DependencyProperty.Register("AutoSaveAgeLimit", typeof(TimeSpan), typeof(AutoSavePreferenceUI),
                new PropertyMetadata(TimeSpan.FromMinutes(30),
                    new PropertyChangedCallback(_AutoSaveAgeLimitChanged)));

        private static void _AutoSaveAgeLimitChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            (sender as AutoSavePreferenceUI).AutoSaveAgeLimitChanged((TimeSpan)e.OldValue, (TimeSpan)e.NewValue);
        }

        public TimeSpan AutoSaveAgeLimit
        {
            get { return (TimeSpan)GetValue(AutoSaveAgeLimitProperty); }
            set { SetValue(AutoSaveAgeLimitProperty, value); }
        }

        bool changing = false;
        private void AutoSaveAgeLimitChanged(TimeSpan oldValue, TimeSpan newValue)
        {
            if (!changing)
            {
                if (newValue.TotalMinutes < 1)
                {
                    autoSaveAgeLimitValue.Value = newValue.TotalSeconds;
                    autoSaveAgeLimitType.SelectedIndex = 0;
                }

                else if (newValue.TotalHours < 1)
                {
                    autoSaveAgeLimitValue.Value = newValue.TotalMinutes;
                    autoSaveAgeLimitType.SelectedIndex = 1;
                }

                else if (newValue.TotalDays < 1)
                {
                    autoSaveAgeLimitValue.Value = newValue.TotalHours;
                    autoSaveAgeLimitType.SelectedIndex = 2;
                }

                else
                {
                    autoSaveAgeLimitValue.Value = newValue.TotalDays;
                    autoSaveAgeLimitType.SelectedIndex = 3;
                }
            }
        }

        private void autoSaveAgeLimitValue_ValueChanged(object sender, CustRes.ValueChangedEventArgs e)
        {
            handlechange();
        }

        private void autoSaveAgeLimitType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            handlechange();
        }

        void handlechange()
        {
            changing = true;
            if (autoSaveAgeLimitType.SelectedIndex == 0)
                AutoSaveAgeLimit = TimeSpan.FromSeconds(autoSaveAgeLimitValue.Value);
            else if (autoSaveAgeLimitType.SelectedIndex == 1)
                AutoSaveAgeLimit = TimeSpan.FromMinutes(autoSaveAgeLimitValue.Value);
            else if (autoSaveAgeLimitType.SelectedIndex == 2)
                AutoSaveAgeLimit = TimeSpan.FromHours(autoSaveAgeLimitValue.Value);
            else if (autoSaveAgeLimitType.SelectedIndex == 3)
                AutoSaveAgeLimit = TimeSpan.FromDays(autoSaveAgeLimitValue.Value);
            changing = false;
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.SelectedPath = autoSaveFolderTextBox.Text;
            
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                autoSaveFolderTextBox.Text = fbd.SelectedPath;
            }
        }

        private void SetBindings()
        {
            App.SetBinding("AutoSaveEnabled", AutoSavePreference, autoSaveEnabledCheckBox, CheckBox.IsCheckedProperty,
                BindingMode.OneWay, UpdateSourceTrigger.Default);
            App.SetBinding("AutoSaveFolder", autoSavePreference, autoSaveFolderTextBox, TextBox.TextProperty,
                BindingMode.OneWay, UpdateSourceTrigger.Default);
            App.SetBinding("AutoSaveIncrement", autoSavePreference, autoSaveIncrement, CustRes.NumericUpDown.ValueProperty,
                BindingMode.OneWay, UpdateSourceTrigger.Default);
            App.SetBinding("AutoSaveAgeLimit", autoSavePreference, this, AutoSavePreferenceUI.AutoSaveAgeLimitProperty,
                BindingMode.OneWay, UpdateSourceTrigger.Default);
        }
    }
}
