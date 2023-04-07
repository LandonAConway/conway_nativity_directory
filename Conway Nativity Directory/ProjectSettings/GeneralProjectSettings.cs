using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Conway_Nativity_Directory
{
    public class GeneralProjectSettings : ProjectSettingsBase
    {


        #region Dependency Properties


        public static readonly DependencyProperty ProjectNameProperty =
            DependencyProperty.Register("ProjectName", typeof(string), typeof(GeneralProjectSettings),
                new PropertyMetadata(String.Empty, new PropertyChangedCallback(ProjectNameChanged)));

        private static void ProjectNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            App.MainWindow.Title = String.IsNullOrWhiteSpace((string)e.NewValue) ?
                "Conway Nativity Directory" :
                "Conway Nativity Directory - " + (string)e.NewValue;
        }

        public string ProjectName
        {
            get { return (string)GetValue(ProjectNameProperty); }
            set { SetValue(ProjectNameProperty, value); }
        }


        #endregion


        #region IProjectSettings


        public override string Title { get { return "General"; } }

        public override bool IsHidden { get { return false; } }

        public override bool ApplyOnProjectSave { get { return false; } }

        public override void Apply()
        {
            this.ProjectName = ui.projectNameTextBox.Text;
        }

        public override void Exceptions() { }

        public override void Reset() { }

        public override ProjectSettingData Save(ProjectSettingData data)
        {
            data["ProjectName"] = this.ProjectName;

            return data;
        }

        private GeneralProjectSettingsUI ui;
        public GeneralProjectSettingsUI UI { get { return ui; } }
        public override UIElement ShowUI()
        {
            if (ui == null)
                ui = new GeneralProjectSettingsUI(this);

            return ui;
        }


        #endregion


        #region Static Methods


        public static GeneralProjectSettings Load(ProjectSettingData data)
        {
            GeneralProjectSettings setting = new GeneralProjectSettings();
            setting.ProjectName = data["ProjectName"];

            return setting;
        }


        #endregion


    }
}
