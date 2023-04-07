using ConwayNativityDirectory.PluginApi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.ComponentModel;
using Microsoft.Win32;
using System.Diagnostics;
using CustRes.Docking;
using System.Text.Json;
using System.Security.Cryptography;
using System.Windows.Forms.VisualStyles;
using SixLabors.ImageSharp;
using WpfImage = System.Windows.Controls.Image;
using IsImage = SixLabors.ImageSharp.Image;
using SixLabors.ImageSharp.Processing;

namespace Conway_Nativity_Directory
{
    /// <summary>
    /// Interaction logic for NativityInformation.xaml
    /// </summary>
    public partial class NativityInformation : UserControl
    {


        #region Constructor

        private Nativity nativity;
        public Nativity Nativity { get { return nativity; } }
        public NativityInformation(Nativity nativity)
        {
            InitializeComponent();
            this.nativity = nativity;

            LoadPreferences();
            SetBindings();
        }

        #endregion


        #region Preferences

        private static readonly DependencyPropertyKey InterfaceMainWindowPropertyKey =
            DependencyProperty.RegisterReadOnly("InterfaceMainWindow", typeof(InterfaceMainWindowPreference), typeof(MainWindow),
                new PropertyMetadata(null));

        public static readonly DependencyProperty InterfaceMainWindowProperty =
            InterfaceMainWindowPropertyKey.DependencyProperty;

        public InterfaceMainWindowPreference InterfaceMainWindow
        {
            get { return (InterfaceMainWindowPreference)GetValue(InterfaceMainWindowProperty); }
            set { SetValue(InterfaceMainWindowPropertyKey, value); }
        }

        private void LoadPreferences()
        {
            InterfaceMainWindow = (InterfaceMainWindowPreference)App.Preferences.GetPreference(@"Interface\Main Window");
        }

        #endregion


        #region Description Property

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(NativityInformation),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(DescriptionChanged)));

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        private static void DescriptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NativityInformation ni = (NativityInformation)d;
            ni.descriptionRichTextBox.Text = e.NewValue as string;
        }

        #endregion


        #region Private Methods


        #region Events

        private void ContextMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (Nativity.ImageMode == ImageMode.Embedded)
            {
                ((MenuItem)((WpfImage)sender).ContextMenu.Items[0]).Visibility = Visibility.Collapsed;
                ((MenuItem)((WpfImage)sender).ContextMenu.Items[1]).Visibility = Visibility.Visible;
            }

            else if (Nativity.ImageMode == ImageMode.Linked)
            {
                ((MenuItem)((WpfImage)sender).ContextMenu.Items[0]).Visibility = Visibility.Visible;
                ((MenuItem)((WpfImage)sender).ContextMenu.Items[1]).Visibility = Visibility.Collapsed;
            }
        }

        private void Embedd_Click(object sender, RoutedEventArgs e)
        {
            Nativity.EmbeddImage();
        }

        private void Relink_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Nativity.OriginalImagePath))
                Nativity.RelinkImage();
            else
            {
                MessageBoxResult mbr = MessageBox.Show("The original image file could not be found. Would you like to save the embedded image?",
                    "Conway Nativity Directory", MessageBoxButton.OKCancel);

                if (mbr == MessageBoxResult.OK)
                {
                    Microsoft.Win32.SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "JPEG (*.jpg)|*.jpg";

                    if (sfd.ShowDialog() == true)
                    {
                        File.Copy(Nativity.GetImagePath(), sfd.FileName);
                        Nativity.RelinkImage(sfd.FileName);
                    }
                }
            }
        }

        private void RotateRight_Click(object sender, RoutedEventArgs e)
        {
            RotateImageRight();
        }

        private void RotateLeft_Click(object sender, RoutedEventArgs e)
        {
            RotateImageLeft();
        }

        private void refreshMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Nativity.UnloadImage();
            Nativity.LoadImage();
        }

        private void openWithMenuItem_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!(sender is MenuItem)) return;
            if ((sender as MenuItem).Name != "openWithMenuItem") return;

            var openWithMenuItem = (MenuItem)sender;
            var openWithBrowseMenuItem = openWithMenuItem.Items[0];
            List<object> items = new List<object>();
            foreach (object item in openWithMenuItem.Items)
            {
                if (item != openWithBrowseMenuItem)
                    items.Add(item);
            }

            foreach (object item in items)
                openWithMenuItem.Items.Remove(item);

            var meta = App.GlobalMeta["ImageEditors"];
            List<ImageEditor> imageEditors;
            if (String.IsNullOrEmpty(meta))
                imageEditors = new List<ImageEditor>();
            else
                imageEditors = JsonSerializer.Deserialize<List<ImageEditor>>(meta);

            if (imageEditors.Count > 0)
                openWithMenuItem.Items.Add(new Separator());
            foreach (ImageEditor imageEditor in imageEditors)
            {
                MenuItem menuItem = new MenuItem();
                menuItem.Header = imageEditor.Title;
                menuItem.Click += (_sender, _e) =>
                {
                    OpenImageWithProcess(imageEditor.Path);
                };
                openWithMenuItem.Items.Add(menuItem);
            }
        }

        private void showInFileExplorer_Click(object sender, RoutedEventArgs e)
        {
            string cmd = "explorer.exe";
            string arg = "/select, " + Nativity.GetImagePath();
            Process.Start(cmd, arg);
        }

        FileWatcher fileWatcher1;
        private void openWithBrowseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Executable Files (*.exe)|*.exe|All files (*.*)|*.*";
            if (ofd.ShowDialog() == true)
            {
                var meta = App.GlobalMeta["ImageEditors"];
                List<ImageEditor> imageEditors;
                if (String.IsNullOrEmpty(meta))
                    imageEditors = new List<ImageEditor>();
                else
                    imageEditors = JsonSerializer.Deserialize<List<ImageEditor>>(meta);
                ImageEditor imageEditor = new ImageEditor();
                imageEditor.Path = ofd.FileName;
                imageEditor.Title = System.IO.Path.GetFileNameWithoutExtension(ofd.FileName);
                if (!imageEditors.Where(a => a.Title == imageEditor.Title && a.Path == imageEditor.Path).Any())
                    imageEditors.Add(imageEditor);
                App.GlobalMeta["ImageEditors"] = JsonSerializer.Serialize(imageEditors);

                OpenImageWithProcess(ofd.FileName);
            }
        }

        private void OpenImageWithProcess(string path)
        {
            CachePreference cache = (CachePreference)App.Preferences.GetPreference(@"Performance\Cache");
            string divider = "";
            if (!cache.CacheFolder.EndsWith(@"\"))
                divider = @"\";
            string cacheFolder = cache.CacheFolder + divider + @"content\images-openwith";
            if (!Directory.Exists(cacheFolder))
                Directory.CreateDirectory(cacheFolder);

            string fileName = System.IO.Path.GetFileName(Nativity.GetImagePath());
            string copiedFilePath = cacheFolder + @"\" + fileName;

            Nativity.UnloadImage();
            if (File.Exists(copiedFilePath))
                File.Delete(copiedFilePath);
            File.Copy(Nativity.GetImagePath(), copiedFilePath);
            Nativity.LoadImage();

            copiedImagePath = copiedFilePath;
            if (fileWatcher1 != null)
                fileWatcher1.Stop();
            fileWatcher1 = new FileWatcher(copiedFilePath);
            fileWatcher1.FileModified += CopiedImageChanged;
            fileWatcher1.Start();

            Process process = new Process();
            process.StartInfo.FileName = path;
            process.StartInfo.Arguments = copiedFilePath;
            process.Start();
        }

        string copiedImagePath = "";
        private void CopiedImageChanged(object sender, EventArgs e)
        {
            Nativity.UnloadImage();
            File.Delete(Nativity.GetImagePath());
            File.Copy(((FileWatcher)sender).FileName, Nativity.GetImagePath());
            Nativity.LoadImage();
        }

        #endregion


        #region Other

        private void SetBindings()
        {
            App.SetBinding("Text", this.idTextBlock, nativity, Nativity.IdProperty, BindingMode.TwoWay, UpdateSourceTrigger.PropertyChanged);
            App.SetBinding("Text", this.titleTextBlock, nativity, Nativity.TitleProperty, BindingMode.TwoWay, UpdateSourceTrigger.PropertyChanged);

            //Description
            App.SetBinding("Description", this, nativity, Nativity.DescriptionProperty,
                BindingMode.TwoWay, UpdateSourceTrigger.PropertyChanged);

            //Setting UI from Preferences
            App.SetBinding("NativityInformationFontSize", InterfaceMainWindow,
                descriptionTextBox, TextBox.FontSizeProperty, BindingMode.OneWay,
                UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("NativityInformationFontFamily", InterfaceMainWindow,
                descriptionTextBox, TextBox.FontFamilyProperty, BindingMode.OneWay,
                UpdateSourceTrigger.PropertyChanged);


            App.SetBinding("NativityInformationFontSize", InterfaceMainWindow,
                descriptionRichTextBox, RichTextBox.FontSizeProperty, BindingMode.OneWay,
                UpdateSourceTrigger.PropertyChanged);

            App.SetBinding("NativityInformationFontFamily", InterfaceMainWindow,
                descriptionRichTextBox, RichTextBox.FontFamilyProperty, BindingMode.OneWay,
                UpdateSourceTrigger.PropertyChanged);
        }




        #endregion


        #endregion


        #region Public Methods


        public void UpdateBindings()
        {
            SetBindings();
        }


        #endregion


        #region Description Editing

        private static readonly DependencyPropertyKey EditingDescriptionPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(EditingDescription), typeof(bool), typeof(NativityInformation),
                new PropertyMetadata(false));

        public static readonly DependencyProperty EditingDescriptionProperty =
            EditingDescriptionPropertyKey.DependencyProperty;

        public bool EditingDescription
        {
            get { return (bool)GetValue(EditingDescriptionProperty); }
            set { SetValue(EditingDescriptionPropertyKey, value); }
        }


        private void EditDescriptionButton_Click(object sender, RoutedEventArgs e)
        {
            EditingDescription = true;
        }

        private void SaveDescriptionButton_Click(object sender, RoutedEventArgs e)
        {
            EditingDescription = false;
        }

        public void StartDescriptionEdit()
        {
            EditingDescription = true;
        }

        public void EndDescriptionEdit()
        {
            EditingDescription = false;
        }

        #endregion


        #region Image

        private static readonly DependencyPropertyKey ShowingImagePropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(ShowingImage), typeof(bool), typeof(NativityInformation), new PropertyMetadata(false));

        public static readonly DependencyProperty ShowingImageProperty =
            ShowingImagePropertyKey.DependencyProperty;

        public bool ShowingImage
        {
            get { return (bool)GetValue(ShowingImageProperty); }
            protected set { SetValue(ShowingImagePropertyKey, value); }
        }


        private static readonly DependencyPropertyKey DisplayingImageNotFoundPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(DisplayingImageNotFound), typeof(bool), typeof(NativityInformation),
                new PropertyMetadata(false));

        public static readonly DependencyProperty DisplayingImageNotFoundProperty =
            DisplayingImageNotFoundPropertyKey.DependencyProperty;

        public bool DisplayingImageNotFound
        {
            get { return (bool)GetValue(DisplayingImageNotFoundProperty); }
            private set { SetValue(DisplayingImageNotFoundPropertyKey, value); }
        }


        Stream mediaStream;
        private BitmapImage _bitmapImage;
        public void LoadImage()
        {
            //If the nativity's image exists, this will not generate a new image path.
            Nativity.NotifyImageNotFound();

            if (Nativity.HasImage && File.Exists(Nativity.ImagePath))
            {
                DisposeMediaStream();
                string imagePath = Nativity.GetImagePath();

                var bitmap = new BitmapImage();
                mediaStream = new FileStream(imagePath, FileMode.Open);

                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = mediaStream;
                //bitmap.Rotation = Nativity.ImageRotation;
                bitmap.EndInit();

                bitmap.Freeze();

                image.Source = bitmap;
                _bitmapImage = bitmap;

                ShowingImage = true;
            }

            if (Nativity.ImageMode == ImageMode.Linked && !File.Exists(Nativity.ImagePath) && !String.IsNullOrWhiteSpace(Nativity.ImagePath))
                DisplayingImageNotFound = true;
            else if (Nativity.ImageMode == ImageMode.Linked)
                DisplayingImageNotFound = false;
            else
                DisplayingImageNotFound = false;
        }

        public void UnloadImage()
        {
            //((BitmapImage)image.Source).StreamSource = null;
            image.Source = null;
            if (_bitmapImage != null)
            {
                DisposeMediaStream();
                _bitmapImage = null;
            }
            ShowingImage = false;
            DisplayingImageNotFound = false;
        }

        public void RefreshImage()
        {
            UnloadImage();
            LoadImage();
        }

        private void DisposeMediaStream()
        {
            if (mediaStream != null)
            {
                mediaStream.Close();
                mediaStream.Dispose();
                mediaStream = null;
                //GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
                GC.Collect();
            }
        }

        public void RotateImageRight()
        {
            if (Nativity.ImageRotation == Rotation.Rotate0)
                Nativity.ImageRotation = Rotation.Rotate90;
            else if (Nativity.ImageRotation == Rotation.Rotate90)
                Nativity.ImageRotation = Rotation.Rotate180;
            else if (Nativity.ImageRotation == Rotation.Rotate180)
                Nativity.ImageRotation = Rotation.Rotate270;
            else if (Nativity.ImageRotation == Rotation.Rotate270)
                Nativity.ImageRotation = Rotation.Rotate0;

            _ = SaveImageRotation(Direction.Right);

            UnloadImage();
            LoadImage();
        }

        public void RotateImageLeft()
        {
            if (Nativity.ImageRotation == Rotation.Rotate0)
                Nativity.ImageRotation = Rotation.Rotate270;
            else if (Nativity.ImageRotation == Rotation.Rotate270)
                Nativity.ImageRotation = Rotation.Rotate180;
            else if (Nativity.ImageRotation == Rotation.Rotate180)
                Nativity.ImageRotation = Rotation.Rotate90;
            else if (Nativity.ImageRotation == Rotation.Rotate90)
                Nativity.ImageRotation = Rotation.Rotate0;

            _ = SaveImageRotation(Direction.Left);

            UnloadImage();
            LoadImage();
        }

        enum Direction { Left, Right }
        private async Task SaveImageRotation(Direction direction)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.1));

            //If the nativity's image exists, this will not generate a new image path.
            Nativity.NotifyImageNotFound();

            if (Nativity.HasImage && File.Exists(Nativity.ImagePath))
            {
                string fileName = System.IO.Path.GetFileName(Nativity.GetImagePath());
                string fileExtension = System.IO.Path.GetExtension(Nativity.GetImagePath());
                string filePath = fileName + "0" + fileExtension;
                int count = 0;
                do
                {
                    filePath = fileName + count.ToString() + fileExtension;
                    count++;
                } while (File.Exists(filePath));

                Application.Current.Dispatcher.Invoke(new Action(() => { UnloadImage(); }));

                ImageRotater rotater = ImageRotater.Load(Nativity.GetImagePath());
                if (direction == Direction.Right)
                    rotater.RotateImageRight();
                else
                    rotater.RotateImageLeft();

                string _filePath = Nativity.GetImagePath();
                File.Delete(_filePath);
                File.Move(filePath, _filePath);

                Application.Current.Dispatcher.Invoke(new Action(() => { LoadImage(); }));
            }
        }

        public void AddImage(string fileName)
        {
            Nativity.AddImage(fileName);
        }

        public void RemoveImage()
        {
            UnloadImage();
            Nativity.RemoveImage();
        }

        //This method is made public so it can be used by the main window
        public void ChooseImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "JPG (*jpg)|*.jpg|JPEG (*.jpeg)|*.jpeg";
            if (Directory.Exists(App.GlobalMeta["chooseImageOfdPath"]))
                ofd.InitialDirectory = App.GlobalMeta["chooseImageOfdPath"];

            if (ofd.ShowDialog() == true)
            {
                if (App.Project.Nativities.Cast<Nativity>().Where(a => a.GetImagePath() == ofd.FileName).Any())
                {
                    MessageBox.Show("This image is already being used by another nativity. Please use a different image.");
                    ChooseImageButton_Click(sender, e);
                }
                else
                {
                    if (App.Project.Nativities.Cast<Nativity>().Where(a => a.GetImagePath() == ofd.FileName).Count() < 1)
                        AddImage(ofd.FileName);
                    else
                        MessageBox.Show("This image cannot be choosen because it is already being used for another nativity.");
                }
                App.GlobalMeta["chooseImageOfdPath"] = ofd.InitialDirectory;
            }

            LoadImage();
        }

        private void ClearImageButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbr = MessageBox.Show("Are you sure you would like to clear the image?", "Conway Nativity Directory",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (mbr == MessageBoxResult.Yes)
            {
                UnloadImage();
                RemoveImage();
            }
        }

        private void imageBorder_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Nativity.HasImage)
            {
                if (e.ClickCount == 2)
                    System.Diagnostics.Process.Start(Nativity.GetImagePath());
            }
        }

        #endregion


        #region Descrition Highlighting

        public void HighlightKeyword(Brush brush, string keyword, System.Globalization.CompareOptions compareOptions)
        {
            descriptionRichTextBox.HighlightKeyword(brush, keyword, compareOptions);
        }

        public void ClearHighlights()
        {
            descriptionRichTextBox.ClearHighlights();
        }



        #endregion


    }

    public class ImageRotater
    {
        private ImageRotater() { }

        string filePath;
        public string FilePath => filePath;

        BitmapImage bitmap;
        Rotation rotation;

        public void RotateImageRight()
        {
            //if (bitmap.Rotation == Rotation.Rotate0)
            //    bitmap.Rotation = Rotation.Rotate90;
            //else if (bitmap.Rotation == Rotation.Rotate90)
            //    bitmap.Rotation = Rotation.Rotate180;
            //else if (bitmap.Rotation == Rotation.Rotate180)
            //    bitmap.Rotation = Rotation.Rotate270;
            //else if (bitmap.Rotation == Rotation.Rotate270)
            //    bitmap.Rotation = Rotation.Rotate0;
            rotation = Rotation.Rotate90;
            Save();
        }

        public void RotateImageLeft()
        {
            //if (bitmap.Rotation == Rotation.Rotate0)
            //    bitmap.Rotation = Rotation.Rotate270;
            //else if (bitmap.Rotation == Rotation.Rotate270)
            //    bitmap.Rotation = Rotation.Rotate180;
            //else if (bitmap.Rotation == Rotation.Rotate180)
            //    bitmap.Rotation = Rotation.Rotate90;
            //else if (bitmap.Rotation == Rotation.Rotate90)
            //    bitmap.Rotation = Rotation.Rotate0;
            rotation = Rotation.Rotate270;
            Save();
        }

        void LoadBitmap()
        {
            byte[] bytes = File.ReadAllBytes(filePath);
            BitmapImage image = null;
            using (var ms = new System.IO.MemoryStream(bytes))
            {
                image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.Rotation = rotation;
                image.StreamSource = ms;
                image.EndInit();
            }
            this.bitmap = image;
        }

        void Save()
        {
            string fileName = System.IO.Path.GetFileName(FilePath);
            string fileExtension = System.IO.Path.GetExtension(FilePath);
            string filePath = fileName + "0" + fileExtension;
            int count = 0;
            do
            {
                filePath = fileName + count.ToString() + fileExtension;
                count++;
            } while (File.Exists(filePath));

            LoadBitmap();

            BitmapEncoder encoder = new JpegBitmapEncoder();
            if (fileExtension.ToUpper() == ".PNG")
                encoder = new PngBitmapEncoder();

            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }

        public static ImageRotater Load(string filePath)
        {
            ImageRotater result = new ImageRotater();
            result.filePath = filePath;

            return result;
        }
    }

    public class ImageEditor
    {
        public string Title { get; set; }
        public string Path { get; set; }
    }
}
