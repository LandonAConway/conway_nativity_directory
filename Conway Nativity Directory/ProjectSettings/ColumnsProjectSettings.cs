using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Conway_Nativity_Directory
{
    public class ColumnsProjectSettings : ProjectSettingsBase
    {


        #region Properties


        public double IdColumnWidth { get; set; }
        public double TitleColumnWidth { get; set; }
        public double OriginColumnWidth { get; set; }
        public double AcquiredColumnWidth { get; set; }
        public double FromColumnWidth { get; set; }
        public double CostColumnWidth { get; set; }
        public double LocationColumnWidth { get; set; }
        public double TagsColumnWidth { get; set; }
        public double GeographicalOriginsColumnWidth { get; set; }



        #endregion


        #region IProjectSettings


        public override string Title { get { return "Columns"; } }

        public override bool IsHidden { get { return true; } }

        public override bool ApplyOnProjectSave { get { return true; } }

        public override void Apply()
        {
            this.IdColumnWidth = GetWidth(App.MainWindow.idGridViewColumn);
            this.TitleColumnWidth = GetWidth(App.MainWindow.titleGridViewColumn);
            this.OriginColumnWidth = GetWidth(App.MainWindow.originGridViewColumn);
            this.AcquiredColumnWidth = GetWidth(App.MainWindow.acquiredGridViewColumn);
            this.FromColumnWidth = GetWidth(App.MainWindow.fromGridViewColumn);
            this.CostColumnWidth = GetWidth(App.MainWindow.costGridViewColumn);
            this.LocationColumnWidth = GetWidth(App.MainWindow.locationGridViewColumn);
            this.TagsColumnWidth = GetWidth(App.MainWindow.tagsGridViewColumn);
            this.GeographicalOriginsColumnWidth = GetWidth(App.MainWindow.geographicalOriginsGridViewColumn);
        }

        private double GetWidth(System.Windows.Controls.GridViewColumn gridViewColumn)
        {
            if (!Double.IsNaN(gridViewColumn.Width))
                return gridViewColumn.Width;
            else
                return gridViewColumn.ActualWidth;
        }

        public override void Exceptions() { }

        public override void Reset() { }

        public override ProjectSettingData Save(ProjectSettingData data)
        {
            data["IdColumnWidth"] = IdColumnWidth.ToString();
            data["TitleColumnWidth"] = TitleColumnWidth.ToString();
            data["OriginColumnWidth"] = OriginColumnWidth.ToString();
            data["AcquiredColumnWidth"] = AcquiredColumnWidth.ToString();
            data["FromColumnWidth"] = FromColumnWidth.ToString();
            data["CostColumnWidth"] = CostColumnWidth.ToString();
            data["LocationColumnWidth"] = LocationColumnWidth.ToString();
            data["TagsColumnWidth"] = TagsColumnWidth.ToString();
            data["GeographicalOriginsColumnWidth"] = GeographicalOriginsColumnWidth.ToString();

            return data;
        }

        public override UIElement ShowUI()
        {
            return null;
        }

        public override void OnOpenProject()
        {
            App.MainWindow.idGridViewColumn.Width = this.IdColumnWidth;
            App.MainWindow.titleGridViewColumn.Width = this.TitleColumnWidth;
            App.MainWindow.originGridViewColumn.Width = this.OriginColumnWidth;
            App.MainWindow.acquiredGridViewColumn.Width = this.AcquiredColumnWidth;
            App.MainWindow.fromGridViewColumn.Width = this.FromColumnWidth;
            App.MainWindow.costGridViewColumn.Width = this.CostColumnWidth;
            App.MainWindow.locationGridViewColumn.Width = this.LocationColumnWidth;
            App.MainWindow.tagsGridViewColumn.Width = this.TagsColumnWidth;
            App.MainWindow.geographicalOriginsGridViewColumn.Width = this.GeographicalOriginsColumnWidth;
        }


        #endregion


        #region Static Methods


        public static ColumnsProjectSettings Load(ProjectSettingData data)
        {
            ColumnsProjectSettings setting = new ColumnsProjectSettings();

            setting.IdColumnWidth = convertToDouble(data["IdColumnWidth"]);
            setting.TitleColumnWidth = convertToDouble(data["TitleColumnWidth"]);
            setting.OriginColumnWidth = convertToDouble(data["OriginColumnWidth"]);
            setting.AcquiredColumnWidth = convertToDouble(data["AcquiredColumnWidth"]);
            setting.FromColumnWidth = convertToDouble(data["FromColumnWidth"]);
            setting.CostColumnWidth = convertToDouble(data["CostColumnWidth"]);
            setting.LocationColumnWidth = convertToDouble(data["LocationColumnWidth"]);
            setting.TagsColumnWidth = convertToDouble(data["TagsColumnWidth"]);
            setting.GeographicalOriginsColumnWidth = convertToDouble(data["GeographicalOriginsColumnWidth"]);

            return setting;
        }

        private static double convertToDouble(string value)
        {
            try
            {
                return double.Parse(value);
            }

            catch
            {
                return double.NaN;
            }
        }


        #endregion


    }
}
