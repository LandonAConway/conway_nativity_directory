using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ConwayNativityDirectory.PluginApi;
using ConwayNativityDirectory.PluginApi.Primitives;

namespace Conway_Nativity_Directory
{
    public class Nativity : NativityBase, IListItem
    {


        #region Constructor

        public Nativity()
        {
            information = new NativityInformation(this);
            Tags = new ObservableCollection<string>();
            GeographicalOrigins = new ObservableCollection<string>();
        }

        /// <summary>
        /// Initializes <see cref="Nativity"/> with a file path. To load a nativity use <see cref="Nativity.Load())"/>
        /// </summary>
        /// <param name="filePath"></param>
        public Nativity(string filePath)
        {
            this.filePath = filePath;
            information = new NativityInformation(this);
            Tags = new ObservableCollection<string>();
            GeographicalOrigins = new ObservableCollection<string>();
        }

        #endregion


        #region Dependency Properties

        //When a dependency property changes...
        private static void DependencyPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Enum.TryParse(e.Property.Name, out NativityModificationType modificationType);

            NativityModifiedEventArgs _e = new NativityModifiedEventArgs(
                (NativityBase)sender,
                modificationType,
                e.Property.PropertyType,
                e.OldValue,
                e.NewValue);

            Nativity nativity = (Nativity)sender;
            if (App.MainWindow != null && App.MainWindow.IsLoaded &&
                App.Project != null && !App.Project.isWorking ||
                App.MainWindow != null && App.Project == null)
            {
                nativity.SetDateModified(DateTime.Now);
            }

            if (nativity.NativityModified != null)
                nativity.NativityModified.Invoke(sender, _e);
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(Nativity));

        /// <summary>
        /// Gets or sets IsSelected.
        /// </summary>
        public override bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }


        /// <summary>
        /// Identifies the <see cref="Nativity.Id"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register("Id", typeof(int), typeof(Nativity),
                new PropertyMetadata(DependencyPropertyChanged));
        
        /// <summary>
        /// Gets or sets Id.
        /// </summary>
        public override int Id
        {
            get { return (int)GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }


        /// <summary>
        /// Identifies the <see cref="Nativity.Title"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(Nativity),
                new PropertyMetadata(DependencyPropertyChanged));

        /// <summary>
        /// Gets or sets Title.
        /// </summary>
        public override string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }


        /// <summary>
        /// Identifies the <see cref="Nativity.Origin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OriginProperty =
            DependencyProperty.Register("Origin", typeof(string), typeof(Nativity),
                new PropertyMetadata(DependencyPropertyChanged));

        /// <summary>
        /// Gets or sets Origin.
        /// </summary>
        public override string Origin
        {
            get { return (string)GetValue(OriginProperty); }
            set { SetValue(OriginProperty, value); }
        }


        /// <summary>
        /// Identifies the <see cref="Nativity.Acquired"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty AcquiredProperty =
            DependencyProperty.Register("Acquired", typeof(string), typeof(Nativity),
                new PropertyMetadata(DependencyPropertyChanged));

        /// <summary>
        /// Gets or sets Acquired.
        /// </summary>
        public override string Acquired
        {
            get { return (string)GetValue(AcquiredProperty); }
            set { SetValue(AcquiredProperty, value); }
        }


        /// <summary>
        /// Identifies the <see cref="Nativity.From"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty FromProperty =
            DependencyProperty.Register("From", typeof(string), typeof(Nativity),
                new PropertyMetadata(DependencyPropertyChanged));

        /// <summary>
        /// Gets or sets From.
        /// </summary>
        public override string From
        {
            get { return (string)GetValue(FromProperty); }
            set { SetValue(FromProperty, value); }
        }


        /// <summary>
        /// Identifies the <see cref="Nativity.Location"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty LocationProperty =
            DependencyProperty.Register("Location", typeof(string), typeof(Nativity),
                new PropertyMetadata(DependencyPropertyChanged));

        /// <summary>
        /// Gets or sets Location.
        /// </summary>
        public override string Location
        {
            get { return (string)GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
        }


        /// <summary>
        /// Identifies the <see cref="Nativity.Cost"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CostProperty =
            DependencyProperty.Register("Cost", typeof(double), typeof(Nativity),
                new PropertyMetadata(DependencyPropertyChanged));

        /// <summary>
        /// Gets or sets Cost.
        /// </summary>
        public override double Cost
        {
            get { return (double)GetValue(CostProperty); }
            set { SetValue(CostProperty, value); }
        }


        /// <summary>
        /// Identifies the <see cref="Nativity.Description"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(Nativity),
                new PropertyMetadata(DependencyPropertyChanged));

        /// <summary>
        /// Gets or sets Description.
        /// </summary>
        public override string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Nativity.Tags"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TagsProperty =
            DependencyProperty.Register("Tags", typeof(ObservableCollection<string>), typeof(Nativity),
                new PropertyMetadata(new ObservableCollection<string>(), new PropertyChangedCallback(DependencyPropertyChanged)));

        /// <summary>
        /// Gets or sets Tags.
        /// </summary>
        public override ObservableCollection<string> Tags
        {
            get { return (ObservableCollection<string>)GetValue(TagsProperty); }
            set { SetValue(TagsProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Nativity.GeographicalOrigins"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty GeographicalOriginsProperty =
            DependencyProperty.Register("GeographicalOrigins", typeof(ObservableCollection<string>), typeof(Nativity),
                new PropertyMetadata(new ObservableCollection<string>(), new PropertyChangedCallback(DependencyPropertyChanged)));

        /// <summary>
        /// Gets or sets GeographicalOrigins.
        /// </summary>
        public override ObservableCollection<string> GeographicalOrigins
        {
            get { return (ObservableCollection<string>)GetValue(GeographicalOriginsProperty); }
            set { SetValue(GeographicalOriginsProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Nativity.Images"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ImagesProperty =
            DependencyProperty.Register("Images", typeof(ObservableCollection<ImageInfo>), typeof(Nativity),
                new PropertyMetadata(new ObservableCollection<ImageInfo>()));

        /// <summary>
        /// Gets or sets Images.
        /// </summary>
        public ObservableCollection<ImageInfo> Images
        {
            get { return (ObservableCollection<ImageInfo>)GetValue(ImagesProperty); }
            set { SetValue(ImagesProperty, value); }
        }


        /// <summary>
        /// Identifies the <see cref="Nativity.ImageRotation"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ImageRotationProperty =
            DependencyProperty.Register("ImageRotation", typeof(System.Windows.Media.Imaging.Rotation), typeof(Nativity),
                new PropertyMetadata(System.Windows.Media.Imaging.Rotation.Rotate0, new PropertyChangedCallback(DependencyPropertyChanged)));

        /// <summary>
        /// Gets or sets ImageRotation.
        /// </summary>
        public override System.Windows.Media.Imaging.Rotation ImageRotation
        {
            get { return (System.Windows.Media.Imaging.Rotation)GetValue(ImageRotationProperty); }
            set { SetValue(ImageRotationProperty, value); }
        }


        #region Read-only

        private static readonly DependencyPropertyKey HasImagePropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(HasImage), typeof(bool), typeof(Nativity),
                new PropertyMetadata(false, new PropertyChangedCallback(DependencyPropertyChanged)));

        public static readonly DependencyProperty HasImageProperty =
            HasImagePropertyKey.DependencyProperty;

        public override bool HasImage
        {
            get { return (bool)GetValue(HasImageProperty); }
            protected set { SetValue(HasImagePropertyKey, value); }
        }


        private static readonly DependencyPropertyKey ImagePathPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(ImagePath), typeof(string), typeof(Nativity),
                new PropertyMetadata("", new PropertyChangedCallback(DependencyPropertyChanged)));

        public static readonly DependencyProperty ImagePathProperty =
            ImagePathPropertyKey.DependencyProperty;
        
        public string ImagePath
        {
            get { return (string)GetValue(ImagePathProperty); }
            protected set { SetValue(ImagePathPropertyKey, value); }
        }


        private static readonly DependencyPropertyKey ImageModePropertyKey =
            DependencyProperty.RegisterReadOnly("ImageMode", typeof(ImageMode), typeof(Nativity),
                new PropertyMetadata(ImageMode.Linked, new PropertyChangedCallback(DependencyPropertyChanged)));

        public static readonly DependencyProperty ImageModeProperty =
            ImageModePropertyKey.DependencyProperty;

        public override ImageMode ImageMode
        {
            get { return (ImageMode)GetValue(ImageModeProperty); }
            protected set { SetValue(ImageModePropertyKey, value); }
        }


        private static readonly DependencyPropertyKey DateModifiedPropertyKey =
            DependencyProperty.RegisterReadOnly("DateModified", typeof(DateTime), typeof(Nativity),
                new PropertyMetadata(DateTime.Now));

        public static readonly DependencyProperty DateModifiedProperty =
            DateModifiedPropertyKey.DependencyProperty;

        public override DateTime DateModified
        {
            get { return (DateTime)GetValue(DateModifiedProperty); }
        }

        private void SetDateModified(DateTime value)
        {
            SetValue(DateModifiedPropertyKey, value);
        }


        private static readonly DependencyPropertyKey DateModifiedInLastSessionPropertyKey =
            DependencyProperty.RegisterReadOnly("DateModifiedInLastSession", typeof(DateTime), typeof(Nativity),
                new PropertyMetadata(DateTime.Now));

        public static readonly DependencyProperty DateModifiedInLastSessionProperty =
            DateModifiedInLastSessionPropertyKey.DependencyProperty;

        public override DateTime DateModifiedInLastSession
        {
            get { return (DateTime)GetValue(DateModifiedInLastSessionProperty); }
        }

        private void SetDateModifiedInLastSession(DateTime value)
        {
            SetValue(DateModifiedInLastSessionPropertyKey, value);
        }


        private static readonly DependencyPropertyKey DateImageModifiedPropertyKey =
            DependencyProperty.RegisterReadOnly("DateImageModified", typeof(DateTime), typeof(Nativity),
                new PropertyMetadata(DateTime.Now));

        public static readonly DependencyProperty DateImageModifiedProperty =
            DateImageModifiedPropertyKey.DependencyProperty;

        public override DateTime DateImageModified
        {
            get { return (DateTime)GetValue(DateImageModifiedProperty); }
        }

        private void SetDateImageModified(DateTime value)
        {
            SetValue(DateImageModifiedPropertyKey, value);
        }


        private static readonly DependencyPropertyKey DateImageModifiedInLastSessionPropertyKey =
            DependencyProperty.RegisterReadOnly("DateImageModifiedInLastSession", typeof(DateTime), typeof(Nativity),
                new PropertyMetadata(DateTime.Now));

        public static readonly DependencyProperty DateImageModifiedInLastSessionProperty =
            DateImageModifiedInLastSessionPropertyKey.DependencyProperty;

        public override DateTime DateImageModifiedInLastSession
        {
            get { return (DateTime)GetValue(DateImageModifiedInLastSessionProperty); }
        }

        private void SetDateImageModifiedInLastSession(DateTime value)
        {
            SetValue(DateImageModifiedInLastSessionPropertyKey, value);
        }


        #endregion

        #endregion


        #region Event Handlers

        public override event NativityModifiedEventHandler NativityModified;

        #endregion


        #region Public Properties


        private string filePath;
        /// <summary>
        /// Gets the file path of the nativity data file.
        /// </summary>
        public string FilePath { get { return filePath; } }


        private NativityInformation information;
        /// <summary>
        /// Gets the information UI.
        /// </summary>
        public NativityInformation Information { get { return information; } }


        private string originalImagePath;
        /// <summary>
        /// Gets the original path of the image before being linked.
        /// </summary>
        public string OriginalImagePath { get { return originalImagePath; } }

        #endregion


        #region Private Properties


        #region General



        #endregion


        #endregion


        #region Public Methods

        /// <summary>
        /// Saves the nativity's file data.
        /// </summary>
        public void Save(string fileName)
        {
            CachePreference cache = (CachePreference)App.Preferences.GetPreference(@"Performance\Cache");

            string directory = System.IO.Path.GetDirectoryName(fileName);
            string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fileName);

            if (!Directory.Exists(directory + @"\descriptions"))
            {
                Directory.CreateDirectory(directory + @"\descriptions");
            }

            string contents = "Id, " + Id.ToString() + '\n' +
                "Title, " + Title + '\n' +
                "Origin, " + Origin + '\n' +
                "Acquired, " + Acquired + '\n' +
                "From, " + From + '\n' +
                "Cost, " + Cost.ToString() + '\n' +
                "Location, " + Location + '\n' +
                "ImagePath, " + OriginalImagePath + '\n' +
                "ImageMode, " + ImageMode.ToString() + '\n' +
                "ImageRotation, " + ImageRotation.ToString() + '\n' +
                "DateModifiedInLastSession, " + DateModified.ToString() + '\n' +
                "DateImageModifiedInLastSession, " + DateImageModified.ToString();

            File.WriteAllText(fileName, contents);
            File.WriteAllText(directory + @"\" + fileNameWithoutExtension + "_tags.txt",
                String.Join(Environment.NewLine, Tags));
            File.WriteAllText(directory + @"\" + fileNameWithoutExtension + "_geographical_origins.txt",
                String.Join(Environment.NewLine, GeographicalOrigins));
            File.WriteAllText(directory + @"\descriptions\" + fileNameWithoutExtension + "_desc.txt", Description);

            string imageDirectory = cache.CacheFolder + @"\content\images\";

            if (HasImage && ImageMode == ImageMode.Embedded)
            {
                File.Copy(ImagePath, directory + @"\" + fileNameWithoutExtension + "_image.jpg");
            }

            //Meta
            string metaPath = directory + @"\" + fileNameWithoutExtension + ".meta";
            var fs = new MetaStorageFileSystem(meta);
            fs.SaveToFile(metaPath);
        }

        /// <summary>
        /// Notifies that the image is not found.
        /// </summary>
        public bool NotifyImageNotFound()
        {
            //checks if the image is linked, and is set, and does not exist.
            //this returns false if an image has not been added.
            if (ImageMode == ImageMode.Linked && !String.IsNullOrEmpty(OriginalImagePath) &&
                !File.Exists(OriginalImagePath))
            {

                var p = (OptimizationNativityPreference)App.MainWindow.Preferences.GetPreference(@"Optimization\Nativity");
                if (p.SearchForImages)
                {
                    string fileName = Path.GetFileName(OriginalImagePath);
                    string newImagePath = OriginalImagePath;
                    foreach (string dir in p.ImageSearchFolders)
                    {
                        string possibleImagePath = dir + @"\" + fileName;
                        if (File.Exists(possibleImagePath))
                        {
                            newImagePath = possibleImagePath;
                            break;
                        }
                    }
                    ImagePath = newImagePath;
                }
                //Return false since the image search was canceled by the user.
                HasImage = File.Exists(ImagePath);
                return ImagePath != OriginalImagePath;
            }
            //There is no need to search for a new image. return true.
            HasImage = true;
            return true;
        }

        /// <summary>
        /// Adds an image. Images are linked by default when added.
        /// </summary>
        /// <param name="fileName"></param>
        public override void AddImage(string fileName)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException("The file name does not exist.", fileName);

            if (HasImage)
                RemoveImage();

            HasImage = true;
            ImagePath = fileName;
            originalImagePath = fileName;
            ImageMode = ImageMode.Linked;
            ImageRotation = Rotation.Rotate0;
            SetDateImageModified(DateTime.Now);
        }

        /// <summary>
        /// Removes the image.
        /// </summary>
        public override void RemoveImage()
        {
            if (HasImage && ImageMode == ImageMode.Embedded)
            {
                File.Delete(ImagePath);
            }

            HasImage = false;
            ImagePath = null;
            originalImagePath = null;
            ImageMode = ImageMode.Linked;
            SetDateImageModified(DateTime.Now);
        }

        /// <summary>
        /// Embedds the image so it is saved with the project file.
        /// </summary>
        public override void EmbeddImage()
        {
            CachePreference cache = (CachePreference)App.Preferences.GetPreference(@"Performance\Cache");

            //Paths
            string newImagePathWithoutExtention = cache.CacheFolder + @"\" +
                System.IO.Path.GetFileNameWithoutExtension(filePath) + "_image";

            string newImagePath = newImagePathWithoutExtention + ".jpg";

            int count = 0;
            do
            {
                newImagePath = newImagePathWithoutExtention + "_" + count.ToString() + ".jpg";
                count++;
            } while (System.IO.File.Exists(newImagePath));

            File.Copy(originalImagePath, newImagePath);

            HasImage = true;
            ImagePath = newImagePath;
            ImageMode = ImageMode.Embedded;
        }

        /// <summary>
        /// Relinks the image to a new image path, and adds the new image.
        /// </summary>
        /// <param name="fileName"></param>
        public override void RelinkImage(string fileName)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException("The file \"" + fileName + "\" was not found.");

            RemoveImage();

            HasImage = true;
            ImagePath = fileName;
            originalImagePath = fileName;
            ImageMode = ImageMode.Linked;
        }

        /// <summary>
        /// Attempts to relink the image to the original file that was used to get the current bitmap data.
        /// This method will throw an error if the file doesn't exist anymore.
        /// </summary>
        public override void RelinkImage()
        {
            RelinkImage(OriginalImagePath);
        }

        /// <summary>
        /// Sets the image path. Can only be set one time.
        /// </summary>
        /// <param name="fileName"></param>
        public void SetImagePath(string fileName)
        {
            if (ImagePath == null || ImagePath == "")
            {
                ImagePath = fileName;
            }
        }

        /// <summary>
        /// Gets the image path. This is implemented for NativityBase to be used by PluginApi.
        /// </summary>
        public override string GetImagePath()
        {
            return ImagePath;
        }

        /// <summary>
        /// Rotates the image to the right.
        /// </summary>
        public override void RotateImageRight()
        {
            Information.RotateImageRight();
        }

        /// <summary>
        /// Rotates the image to the left.
        /// </summary>
        public override void RotateImageLeft()
        {
            Information.RotateImageLeft();
        }

        /// <summary>
        /// Refreshes the image in the information panel.
        /// </summary>
        public override void RefreshImage()
        {
            Information.RefreshImage();
        }

        /// <summary>
        /// Unloads the image from cache.
        /// </summary>
        public override void UnloadImage()
        {
            Information.UnloadImage();
        }

        /// <summary>
        /// Loads the image into cache.
        /// </summary>
        public override void LoadImage()
        {
            Information.LoadImage();
        }

        /// <summary>
        /// Updates the bindings of the UI in the information panel.
        /// </summary>
        public void UpdateUI()
        {
            information.UpdateBindings();
        }

        public override void RefreshUI()
        {
            UpdateUI();
        }

        MetaStorage meta = new MetaStorage();
        public override MetaStorage GetMeta()
        {
            return meta;
        }

        MetaTagCollection metaTags = new MetaTagCollection();
        public override MetaTagCollection GetMetaTags()
        {
            return metaTags;
        }

        #endregion


        #region Public Static Methods

        /// <summary>
        /// Loads a nativity from a file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Nativity Load(string filePath)
        {
            CachePreference cache = (CachePreference)App.Preferences.GetPreference(@"Performance\Cache");
            Nativity nativity = new Nativity(filePath);


            //Paths
            string directory = System.IO.Path.GetDirectoryName(filePath);
            string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);


            //Read file
            string[] lines = File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                if (line.StartsWith("Id, "))
                {
                    nativity.Id = Convert.ToInt32(line.Substring("Id, ".Length));
                }

                else if (line.StartsWith("Title, "))
                {
                    nativity.Title = line.Substring("Title, ".Length);
                }

                else if (line.StartsWith("Origin, "))
                {
                    nativity.Origin = line.Substring("Origin, ".Length);
                }

                else if (line.StartsWith("Acquired, "))
                {
                    nativity.Acquired = line.Substring("Acquired, ".Length);
                }

                else if (line.StartsWith("From, "))
                {
                    nativity.From = line.Substring("From, ".Length);
                }

                else if (line.StartsWith("Location, "))
                {
                    nativity.Location = line.Substring("Location, ".Length);
                }

                else if (line.StartsWith("Cost, "))
                {
                    nativity.Cost = Convert.ToDouble(line.Substring("Cost, ".Length));
                }

                else if (line.StartsWith("ImagePath, "))
                {
                    nativity.originalImagePath = line.Substring("ImagePath, ".Length);
                }

                else if (line.StartsWith("ImageMode, "))
                {
                    nativity.ImageMode = (ImageMode)Enum.Parse(
                        typeof(ImageMode), line.Substring("ImageMode, ".Length));
                }

                else if (line.StartsWith("ImageRotation, "))
                {
                    nativity.ImageRotation = (System.Windows.Media.Imaging.Rotation)Enum.Parse(
                        typeof(System.Windows.Media.Imaging.Rotation), line.Substring("ImageRotation, ".Length));
                }

                else if (line.StartsWith("DateModifiedInLastSession, "))
                {
                    DateTime dateModifiedInLastSession = DateTime.Now;
                    try { dateModifiedInLastSession = DateTime.Parse(line.Substring("DateModifiedInLastSession, ".Length)); } catch { }
                    nativity.SetDateModifiedInLastSession(dateModifiedInLastSession);
                }

                else if (line.StartsWith("DateImageModifiedInLastSession, "))
                {
                    DateTime dateImageModifiedInLastSession = nativity.DateModifiedInLastSession;
                    try { dateImageModifiedInLastSession = DateTime.Parse(line.Substring("DateImageModifiedInLastSession, ".Length)); } catch { }
                    nativity.SetDateImageModifiedInLastSession(dateImageModifiedInLastSession);
                }
            }
            
            nativity.Description = File.ReadAllText(directory + @"\descriptions\" + fileName + "_desc.txt");
            nativity.Tags = new ObservableCollection<string>(File.ReadAllLines(directory + @"\" + fileName + "_tags.txt"));
            nativity.GeographicalOrigins = new ObservableCollection<string>(File.ReadAllLines(directory + @"\" + fileName + "_geographical_origins.txt"));


            //Images
            string imageDirectory = cache.CacheFolder + @"\content\images\";
            if (!Directory.Exists(imageDirectory))
            {
                Directory.CreateDirectory(imageDirectory);
            }

            string imagePath = directory + @"\" + fileName + "_image.jpg";

            //if the imagePath exists then it is embedded.
            //this will convert nativities from projects saved by an older version.
            if (File.Exists(imagePath))
                nativity.ImageMode = ImageMode.Embedded;

            if (nativity.ImageMode == ImageMode.Embedded && File.Exists(imagePath))
            {
                nativity.SetImagePath(imageDirectory + @"\" + fileName + "_image.jpg");
                File.Copy(imagePath, imageDirectory + @"\" + fileName + "_image.jpg");
                nativity.HasImage = true;
            }

            else if (nativity.ImageMode == ImageMode.Linked && File.Exists(nativity.originalImagePath))
            {
                nativity.SetImagePath(nativity.originalImagePath);
                nativity.HasImage = true;
            }


            //Meta
            string metaPath = Path.GetDirectoryName(filePath) + @"\" + fileName + @".meta";
            var fs = new MetaStorageFileSystem(nativity.meta);
            if (File.Exists(metaPath))
                fs.LoadFromFile(metaPath);

            //Make sure the DateModified is equel to DateModifiedInLastSession
            nativity.SetDateModified(nativity.DateModifiedInLastSession);
            nativity.SetDateImageModified(nativity.DateImageModifiedInLastSession);

            //If DateImageModifiedInLastSession was not stored in the save file then make it equel to DateModified
            if (!lines.Where(l => l.Contains("DateImageModifiedInLastSession, ")).Any())
            {
                nativity.SetDateImageModifiedInLastSession(nativity.DateModifiedInLastSession);
                nativity.SetDateImageModified(nativity.DateModifiedInLastSession);
            }

            return nativity;
        }

        public List<Nativity> LoadNativities(string dir)
        {
            List<Nativity> result = new List<Nativity>();

            foreach (string file in Directory.GetFiles(dir, "*.nty", SearchOption.TopDirectoryOnly))
            {
                result.Add(Nativity.Load(file));
            }

            return result;
        }

        #endregion


    }

    public abstract class NativityWarning
    {
        public abstract string Title { get; }
        public abstract string Description { get; }
        public abstract void Fix();
    }



    //This would be an atempt to allow for the support of having multiple images per-nativity. However, multiple image support is
    //going to be abandoned for awhile since there really is no purpose for it, and it is dificult to implement at this point of
    //development. There is no longer a plan to continue this kind of feature in the near future.
    public class ImageInfo
    {
        private BitmapImage bitmapImage;
        public BitmapImage BitmapImage 
        { 
            get { return bitmapImage; } 
        }

        private bool bitmapImageIsLoaded;
        public bool BitmapImageIsLoaded
        {
            get { return bitmapImageIsLoaded; }
        }

        public string Path { get; private set; }
        public ImageMode Mode { get; set; }
        public Rotation Rotation { get; set; }

        public ImageInfo(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("File does not exist.");

            Path = path;
            Mode = ImageMode.Linked;
        }

        public void LoadBitmapImage()
        {
            bitmapImage = GetBitmapImage();
            bitmapImageIsLoaded = false;
        }

        public void UnloadBitmapImage()
        {
            bitmapImage = null;
            bitmapImageIsLoaded = false;
        }

        public void Embedd()
        {
        }

        public void Relink()
        {
        }

        public void RotateRight()
        {
            if (Rotation == Rotation.Rotate0)
                Rotation = Rotation.Rotate90;
            else if (Rotation == Rotation.Rotate90)
                Rotation = Rotation.Rotate90;
            else if (Rotation == Rotation.Rotate180)
                Rotation = Rotation.Rotate270;
            else if (Rotation == Rotation.Rotate270)
                Rotation = Rotation.Rotate0;
        }

        public void RotateLeft()
        {
            if (Rotation == Rotation.Rotate0)
                Rotation = Rotation.Rotate270;
            else if (Rotation == Rotation.Rotate270)
                Rotation = Rotation.Rotate180;
            else if (Rotation == Rotation.Rotate180)
                Rotation = Rotation.Rotate90;
            else if (Rotation == Rotation.Rotate90)
                Rotation = Rotation.Rotate0;
        }

        public BitmapImage GetBitmapImage()
        {
            if (File.Exists(Path))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.None;
                bitmapImage.UriCachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.BypassCache);
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bitmapImage.Rotation = Rotation;
                bitmapImage.UriSource = new Uri(Path, UriKind.Absolute);
                bitmapImage.EndInit();
                return bitmapImage;
            }

            else
                return null;
        }

        public string Serialize()
        {
            string data = Path + ":" +
                Mode.ToString() + ":" +
                Rotation.ToString();

            return data;
        }

        public static ImageInfo Deserialize(string data)
        {
            string[] _data = data.Split(':');

            ImageInfo info = new ImageInfo(_data[0]);
            info.Mode = (ImageMode)Enum.Parse(typeof(ImageMode), _data[1]);
            info.Rotation = (Rotation)Enum.Parse(typeof(Rotation), _data[2]);

            return info;
        }
    }

    public enum SortingMode { Ascending, Descending }
    public enum SortingType { Id, Title, Origin, Acquired, From, Cost, Location, Tags, GeographicalOrigins }
}
