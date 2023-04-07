using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using Conway_Nativity_Directory;

namespace Conway_Nativity_Directory
{
    /// <summary>
    /// Interaction logic for PreferencesWindow.xaml
    /// </summary>
    public partial class PreferencesWindow : Window
    {


        #region PreferencesWindow

        public PreferencesWindow()
        {
            InitializeComponent();
            this.preferences = App.Preferences;
            this.warningIcon.Source = IconUtilities.ToImageSource(System.Drawing.SystemIcons.Information);

            SetBindings();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Preferences.CloseUI();
        }

        #endregion


        #region Preferences

        private Preferences preferences;
        public Preferences Preferences { get { return preferences; } }

        #endregion


        #region Events
        
        private void PreferencesTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (preferencesTreeView.SelectedItem != null)
            {
                IPreference preference = preferencesTreeView.SelectedItem as IPreference;
                titleTextBlock.Text = preference.Title;
                uiContentControl.Content = preference.ShowUI();

                if (preference.EffectiveImmediately)
                    effectiveImmediatelyBorder.Visibility = Visibility.Collapsed;
                else
                    effectiveImmediatelyBorder.Visibility = Visibility.Visible;
            }

            else
            {
                titleTextBlock.Text = String.Empty;
                uiContentControl.Content = null;
                effectiveImmediatelyBorder.Visibility = Visibility.Collapsed;
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            if (preferencesTreeView.SelectedItem != null)
            {
                (preferencesTreeView.SelectedItem as IPreference).Reset();
            }
        }

        private void ResetAllButton_Click(object sender, RoutedEventArgs e)
        {
            Preferences.ResetAll();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            ApplyPreferences(false);
            this.Close();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            ApplyPreferences(true);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion


        #region Private Methods


        private void ApplyPreferences(bool showMsg)
        {
            bool canApply = true;

            try { Preferences.Exceptions(); }
            catch (PreferenceException ex)
            {
                MessageBox.Show(ex.Message, ex.Preference.Title);
                canApply = false;
            }

            if (canApply)
            {
                string saveDir = AppDomain.CurrentDomain.BaseDirectory + @"\settings";
                if (!System.IO.Directory.Exists(saveDir))
                {
                    System.IO.Directory.CreateDirectory(saveDir);
                }

                Preferences.Apply();
                Preferences.Save(saveDir + @"\preferences.xml");

                if (showMsg)
                {
                    MessageBox.Show("Successful!");
                }
            }
        }

        public void SetBindings()
        {
            App.SetBinding("Items", Preferences, preferencesTreeView, TreeView.ItemsSourceProperty);
        }



        #endregion

        
    }

    public class BooleanToVisibilityConverter : IValueConverter
    {
        private bool invert = false;
        public bool Invert
        {
            get { return invert; }
            set { invert = value; }
        }

        public object Convert(object value, Type targetType, object target, System.Globalization.CultureInfo culture)
        {
            if (!Invert)
            {
                if ((bool)value)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }

            else
            {
                if ((bool)value)
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object target, System.Globalization.CultureInfo culture)
        {
            if (!Invert)
            {
                if ((Visibility)value == Visibility.Visible)
                    return true;
                else
                    return false;
            }

            else
            {
                if ((Visibility)value == Visibility.Collapsed)
                    return false;
                else
                    return true;
            }
        }
    }

    public class ShowPreferencesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object target, System.Globalization.CultureInfo culture)
        {
            System.Collections.ObjectModel.ObservableCollection<object> collection =
                (System.Collections.ObjectModel.ObservableCollection<object>)value;

            bool show = false;

            foreach (IPreference preference in collection)
            {
                if (preference.Visible)
                {
                    show = true;
                }
            }

            return show;
        }

        public object ConvertBack(object value, Type targetType, object target, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("Cannot convert back!");
        }
    }

    public static class IconUtilities
    {
        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);

        public static ImageSource ToImageSource(this System.Drawing.Icon icon)
        {
            System.Drawing.Bitmap bitmap = icon.ToBitmap();
            IntPtr hBitmap = bitmap.GetHbitmap();

            ImageSource wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(
                hBitmap,
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            if (!DeleteObject(hBitmap))
            {
                throw new Win32Exception();
            }

            return wpfBitmap;
        }
    }
}

namespace System.Windows
{
    public static class SystemIcons
    {
        public static ImageSource Application = IconUtilities.ToImageSource(System.Drawing.SystemIcons.Application);
        public static ImageSource Asterisk = IconUtilities.ToImageSource(System.Drawing.SystemIcons.Asterisk);
        public static ImageSource Error = IconUtilities.ToImageSource(System.Drawing.SystemIcons.Error);
        public static ImageSource Exclamation = IconUtilities.ToImageSource(System.Drawing.SystemIcons.Exclamation);
        public static ImageSource Hand = IconUtilities.ToImageSource(System.Drawing.SystemIcons.Hand);
        public static ImageSource Information = IconUtilities.ToImageSource(System.Drawing.SystemIcons.Information);
        public static ImageSource Question = IconUtilities.ToImageSource(System.Drawing.SystemIcons.Question);
        public static ImageSource Shield = IconUtilities.ToImageSource(System.Drawing.SystemIcons.Shield);
        public static ImageSource Warning = IconUtilities.ToImageSource(System.Drawing.SystemIcons.Warning);
        public static ImageSource WinLogo = IconUtilities.ToImageSource(System.Drawing.SystemIcons.WinLogo);
    }
}