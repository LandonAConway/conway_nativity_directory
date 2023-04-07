using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Conway_Nativity_Directory
{
    /// <summary>
    /// Interaction logic for About_Box.xaml
    /// </summary>
    public partial class About_Box : Window
    {
        public About_Box()
        {
            InitializeComponent();

            try
            {
                versionTextBox.Text = "Version " + App.Version.ToString();
            }

            catch { }

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.None;
            bitmapImage.UriCachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.BypassCache);
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            bitmapImage.UriSource = new Uri(AppDomain.CurrentDomain.BaseDirectory + @"\bin\icons\icon_help.png", UriKind.Absolute);
            bitmapImage.EndInit();

            iconImage.Source = bitmapImage;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    public partial class App : Application
    {
        public static readonly Version Version =
            new Version(2, 3, 1, 0);
    }

    public struct Version
    {
        private ushort major;
        public ushort Major { get { return major; } }

        private ushort minor;
        public ushort Minor {  get {  return minor; } }

        private ushort build;
        public ushort Build {  get {  return build; } }

        private ushort patch;
        public ushort Patch {  get {  return patch; } }

        public Version(ushort major, ushort minor, ushort build, ushort patch)
        {
            this.major = major;
            this.minor = minor;
            this.build = build;
            this.patch = patch;
        }

        public Version(ushort major, ushort minor, ushort build)
        {
            this.major = major;
            this.minor = minor;
            this.build = build;
            this.patch = 0;
        }

        public Version(ushort major, ushort minor)
        {
            this.major = major;
            this.minor = minor;
            this.build = 0;
            this.patch = 0;
        }

        public Version(ushort major)
        {
            this.major = major;
            this.minor = 0;
            this.build = 0;
            this.patch = 0;
        }

        public override string ToString()
        {
            return major.ToString() + "."
                + minor.ToString() + "."
                + build.ToString() + "."
                + patch;
        }

        public static Version Parse(string value)
        {
            ushort _major = 0;
            ushort _minor = 0;
            ushort _build = 0;
            ushort _patch = 0;

            try
            {
                string[] parts = value.Split('.');

                if (parts.Count() > 4)
                    throw new Exception();

                string partMajor = parts.ElementAtOrDefault(0);
                string partMinor = parts.ElementAtOrDefault(1);
                string partBuild = parts.ElementAtOrDefault(2);
                string partPatch = parts.ElementAtOrDefault(3);

                _major = Convert.ToUInt16(partMajor);
                _minor = Convert.ToUInt16(partMinor);
                _build = Convert.ToUInt16(partBuild);
                _patch = Convert.ToUInt16(partPatch);
            }

            catch
            {
                throw new FormatException("The value was in the wrong format.");
            }

            return new Version(_major, _minor, _build, _patch);
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                return false;
            else
            {
                Version v = (Version)obj;
                if (major == v.major
                    && minor == v.minor
                    && build == v.build
                    && patch == v.patch)
                    return true;
                else
                    return false;
            }
        }

        public override int GetHashCode()
        {
            return major.GetHashCode() ^ minor.GetHashCode() ^ build.GetHashCode() ^ patch.GetHashCode();
        }

        public System.Version ToSystem()
        {
            return new System.Version(this.major, this.minor, this.build, this.patch);
        }

        public static Version FromSystem(System.Version version)
        {
            return new Version(
                    Convert.ToUInt16(version.Major),
                    Convert.ToUInt16(version.Minor),
                    Convert.ToUInt16(version.Build),
                    Convert.ToUInt16(version.Revision)
            );
        }

        public static bool operator ==(Version v1, Version v2)
        {
            return v1.Major == v2.Major &&
                v1.Minor == v2.Minor &&
                v1.Build == v2.Build &&
                v1.Patch == v2.Patch;
        }

        public static bool operator !=(Version v1, Version v2)
        {
            return !(v1.Major == v2.Major &&
                v1.Minor == v2.Minor &&
                v1.Build == v2.Build &&
                v1.Patch == v2.Patch);
        }

        public static bool operator >(Version v1, Version v2)
        {
            ushort[] v1_parts = new ushort[] {v1.Major, v1.Minor, v1.build, v1.patch};
            ushort[] v2_parts = new ushort[] {v2.Major, v2.Minor, v2.build, v2.patch};

            bool result = false;

            for (int i = 0; i <= 3; i++)
            {
                ushort p1 = v1_parts[i];
                ushort p2 = v2_parts[i];
                if (p1 > p2)
                    return true;
                else if (p1 < p2)
                    return false;
                else if (p1 == p2)
                    result = false;
            }

            return result;
        }

        public static bool operator >=(Version v1, Version v2)
        {
            return v1 > v2 || v1 == v2;
        }

        public static bool operator <(Version v1, Version v2)
        {
            ushort[] v1_parts = new ushort[] { v1.Major, v1.Minor, v1.build, v1.patch };
            ushort[] v2_parts = new ushort[] { v2.Major, v2.Minor, v2.build, v2.patch };

            bool result = false;

            for (int i = 0; i <= 3; i++)
            {
                ushort p1 = v1_parts[i];
                ushort p2 = v2_parts[i];
                if (p1 < p2)
                    return true;
                else if (p1 > p2)
                    return false;
                else if (p1 == p2)
                    result = false;
            }

            return result;
        }

        public static bool operator <=(Version v1, Version v2)
        {
            return v1 < v2 || v1 == v2;
        }

        public static implicit operator System.Version(Version version) => version.ToSystem();
        public static implicit operator Version(System.Version version) => Version.FromSystem(version);
    }
}
