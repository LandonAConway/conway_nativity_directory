using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for OptimizationNativityPreferenceUI.xaml
    /// </summary>
    public partial class OptimizationNativityPreferenceUI : UserControl
    {
        private static readonly DependencyPropertyKey OptimizationNativityPreferencePropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(OptimizationNativityPreference), typeof(OptimizationNativityPreference), typeof(OptimizationNativityPreferenceUI),
            new PropertyMetadata(null));

        public static readonly DependencyProperty OptimizationNativityPreferenceProperty =
            OptimizationNativityPreferencePropertyKey.DependencyProperty;

        public OptimizationNativityPreference OptimizationNativityPreference
        {
            get => (OptimizationNativityPreference)GetValue(OptimizationNativityPreferenceProperty);
            private set => SetValue(OptimizationNativityPreferencePropertyKey, value);
        }

        public OptimizationNativityPreferenceUI(OptimizationNativityPreference optimizationNativityPreference)
        {
            InitializeComponent();
            this.OptimizationNativityPreference = optimizationNativityPreference;
        }

        private void browseImagesFolderPathBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                CopyImagesFolderPath = fbd.SelectedPath;
        }

        private void addFolderBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                ImageSearchFolders.Add(fbd.SelectedPath);
        }

        private void removeFolderBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var folder in imageSearchFoldersListView.SelectedItems.Cast<string>().ToList())
                ImageSearchFolders.Remove(folder);
        }


        #region Dependency Properties


        public static readonly DependencyProperty CopyImagesToFolderProperty = DependencyProperty.Register(
            nameof(CopyImagesToFolder), typeof(bool), typeof(OptimizationNativityPreferenceUI),
            new PropertyMetadata(false));

        public bool CopyImagesToFolder
        {
            get => (bool)GetValue(CopyImagesToFolderProperty);
            set => SetValue(CopyImagesToFolderProperty, value);
        }


        public static readonly DependencyProperty CopyImagesFolderPathProperty = DependencyProperty.Register(
            nameof(CopyImagesFolderPath), typeof(string), typeof(OptimizationNativityPreferenceUI),
            new PropertyMetadata(null));

        public string CopyImagesFolderPath
        {
            get => (string)GetValue(CopyImagesFolderPathProperty);
            set => SetValue(CopyImagesFolderPathProperty, value);
        }


        public static readonly DependencyProperty SearchForImagesProperty = DependencyProperty.Register(
            nameof(SearchForImages), typeof(bool), typeof(OptimizationNativityPreferenceUI),
            new PropertyMetadata(false));

        public bool SearchForImages
        {
            get => (bool)GetValue(SearchForImagesProperty);
            set => SetValue(SearchForImagesProperty, value);
        }


        public static readonly DependencyProperty ImageSearchFoldersProperty = DependencyProperty.Register(
            nameof(ImageSearchFolders), typeof(ObservableCollection<string>), typeof(OptimizationNativityPreferenceUI),
            new PropertyMetadata(new ObservableCollection<string>()));

        public ObservableCollection<string> ImageSearchFolders
        {
            get => (ObservableCollection<string>)GetValue(ImageSearchFoldersProperty);
            set => SetValue(ImageSearchFoldersProperty, value);
        }


        #endregion
    }
}
