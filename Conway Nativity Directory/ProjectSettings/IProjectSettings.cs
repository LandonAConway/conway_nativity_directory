using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Reflection;

namespace Conway_Nativity_Directory
{
    public class ProjectSettings : IEnumerable<IProjectSettings>
    {


        #region Settings


        List<IProjectSettings> settings = new List<IProjectSettings>();

        public IProjectSettings this[string settingsName]
        {
            get
            {
                return settings.Where(i => i.Title == settingsName).FirstOrDefault();
            }
        }

        public IEnumerator<IProjectSettings> GetEnumerator()
        {
            return settings.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        #endregion


        #region Public Methods

        public void Save(string fileName)
        {
            List<string> lines = new List<string>();

            foreach (IProjectSettings setting in settings)
            {
                ProjectSettingData settingData = setting.Save(new ProjectSettingData(setting.GetType()));
                foreach (string[] data in settingData)
                    lines.Add("[" + setting.GetType().Name + "]" +
                        data[0] + "|" + data[1]);
            }

            File.WriteAllText(fileName, String.Join(Environment.NewLine, lines.ToArray()));
        }


        #endregion


        #region Static Methods


        public static ProjectSettings New()
        {
            var projectSettings = new ProjectSettings();

            var type = typeof(IProjectSettings);
            var types = App.Types
                .Where(t => type.IsAssignableFrom(t) && t.IsClass)
                .ToList();

            foreach (var t in types)
            {
                var methodInfo = t.GetMethod("Load", BindingFlags.Public | BindingFlags.Static);
                if (methodInfo != null)
                {
                    object obj = methodInfo.Invoke(null, new object[] { new ProjectSettingData(t) });
                    if (obj != null && type.IsAssignableFrom(obj.GetType()))
                        projectSettings.settings.Add((IProjectSettings)obj);
                }
            }

            return projectSettings;
        }

        public static ProjectSettings Load(string fileName)
        {
            var projectSettings = new ProjectSettings();

            var type = typeof(IProjectSettings);
            var types = App.Types
                .Where(t => type.IsAssignableFrom(t) && t.IsClass)
                .ToList();

            string[] lines = File.ReadAllLines(fileName);
            List<ProjectSettingData> settingDatas = new List<ProjectSettingData>();

            foreach (Type t in types)
            {
                string[] settingLines = lines.Where(l => l.StartsWith("[" + t.Name + "]")).ToArray();
                if (settingLines.Count() > 0)
                {
                    var projectSettingData = new ProjectSettingData(t);
                    foreach (string line in settingLines)
                    {
                        var settingData = line.Substring(t.Name.Length + 2)
                            .Split('|');

                        projectSettingData[settingData[0]] = settingData[1];
                    }

                    settingDatas.Add(projectSettingData);
                }

                else
                    settingDatas.Add(new ProjectSettingData(t));
            }

            foreach (ProjectSettingData settingData in settingDatas)
            {
                var methodInfo = settingData.SettingType.GetMethod("Load", BindingFlags.Static | BindingFlags.Public);
                if (methodInfo != null)
                {
                    object obj = methodInfo.Invoke(null, new object[] { settingData });
                    if (obj != null && obj.GetType() == settingData.SettingType)
                        projectSettings.settings.Add((IProjectSettings)obj);
                }
            }

            return projectSettings;
        }


        #endregion


    }

    public interface IProjectSettings
    {
        string Title { get; }
        bool IsHidden { get; }
        bool ApplyOnProjectSave { get; }
        void Exceptions();
        void Apply();
        ProjectSettingData Save(ProjectSettingData data);
        void Reset();
        UIElement ShowUI();
        void OnNewProject();
        void OnOpenProject();
        void OnSaveProject();
        void OnCloseProject();
    }

    public abstract class ProjectSettingsBase : DependencyObject, IProjectSettings
    {
        /// <summary>
        /// Gets the title of the settings. Default is an empty string.
        /// </summary>
        public virtual string Title { get { return String.Empty; } }

        /// <summary>
        /// Indicates if the settings are hidden in the UI. Default is true.
        /// </summary>
        public virtual bool IsHidden { get { return true; } }

        /// <summary>
        /// Indicates if the settings should be applied before they are saved to a file. Default is false.
        /// </summary>
        public virtual bool ApplyOnProjectSave { get { return false; } }

        /// <summary>
        /// Applies the settings when clicking the 'Apply' button, or when the project is saved.
        /// </summary>
        public virtual void Apply() { }

        /// <summary>
        /// Throws exceptions while 'applying' the settings.
        /// </summary>
        public virtual void Exceptions() { }

        /// <summary>
        /// Resets the settings to default.
        /// </summary>
        public virtual void Reset() { }

        /// <summary>
        /// Gets the settings to be saved to a file.
        /// </summary>
        public virtual ProjectSettingData Save(ProjectSettingData data)
        {
            return data;
        }

        /// <summary>
        /// Shows the UI of the settings.
        /// </summary>
        /// <returns></returns>
        public virtual UIElement ShowUI()
        {
            return null;
        }

        public virtual void OnNewProject() { }
        public virtual void OnOpenProject() { }
        public virtual void OnSaveProject() { }
        public virtual void OnCloseProject() { }
    }

    public class ProjectSettingData : IEnumerable<string[]>
    {
        Type settingType;
        public Type SettingType { get { return settingType; } }

        public ProjectSettingData(Type settingType)
        {
            if (typeof(IProjectSettings).IsAssignableFrom(settingType))
                this.settingType = settingType;
            else
                throw new InvalidCastException("The type must implement the IProjectSettings interface.");
        }

        Dictionary<string, string> settings = new Dictionary<string, string>();

        public int Count { get { return settings.Count; } }

        public string[] this[int index]
        {
            get
            {
                if (index > -1 && index < this.Count)
                {
                    KeyValuePair<string, string> pair = settings.ElementAt(index);
                    return new string[] { pair.Key, pair.Value };
                }

                else
                    throw new IndexOutOfRangeException("Index was out of range. Must be non-negative " +
                        "and less than the size of the collection.");
            }
        }

        public string this[string setting]
        {
            get
            {
                if (settings.ContainsKey(setting))
                    return settings[setting];
                else
                    return String.Empty;
            }

            set
            {
                if (!settings.ContainsKey(setting))
                    settings[setting] = value;
                else
                {
                    settings.Remove(setting);
                    settings[setting] = value;
                }
            }
        }

        public bool SettingExists(string setting)
        {
            return settings.ContainsKey(setting);
        }

        public IEnumerator<string[]> GetEnumerator()
        {
            List<string[]> list = new List<string[]>();
            foreach (KeyValuePair<string, string> setting in settings)
                list.Add(new string[] { setting.Key, setting.Value });
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
