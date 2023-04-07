using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Conway_Nativity_Directory
{
    static class CustomCommands
    {
        //Main Window
        public static RoutedCommand New = new RoutedCommand();
        public static RoutedCommand Open = new RoutedCommand();
        public static RoutedCommand Close = new RoutedCommand();
        public static RoutedCommand Save = new RoutedCommand();
        public static RoutedCommand SaveAs = new RoutedCommand();
        public static RoutedCommand SaveACopy = new RoutedCommand();
        public static RoutedCommand Login = new RoutedCommand();
        public static RoutedCommand Logout = new RoutedCommand();
        public static RoutedCommand Undo = new RoutedCommand();
        public static RoutedCommand Redo = new RoutedCommand();
        public static RoutedCommand RotateImageRight = new RoutedCommand();
        public static RoutedCommand RotateImageLeft = new RoutedCommand();
        public static RoutedCommand AdvancedSearch = new RoutedCommand();
        public static RoutedCommand Find = new RoutedCommand();
        public static RoutedCommand GoTo = new RoutedCommand();
        public static RoutedCommand Add = new RoutedCommand();
        public static RoutedCommand Remove = new RoutedCommand();
        public static RoutedCommand SelectAll = new RoutedCommand();
        public static RoutedCommand SelectNone = new RoutedCommand();
        public static RoutedCommand SelectInvert = new RoutedCommand();
        public static RoutedCommand CreateTagForSelected = new RoutedCommand();
        public static RoutedCommand RemoveTagFromSelected = new RoutedCommand();
        public static RoutedCommand CreateGeographicalOriginForSelected = new RoutedCommand();
        public static RoutedCommand RemoveGeographicalOriginFromSelected = new RoutedCommand();
        public static RoutedCommand OrganizeTagsAndGeographicalOrigins = new RoutedCommand();
        public static RoutedCommand Commands = new RoutedCommand();
        public static RoutedCommand Exit = new RoutedCommand();

        //Global
        public static RoutedCommand CloseWindow = new RoutedCommand();
        public static RoutedCommand Ok = new RoutedCommand();
        public static RoutedCommand Cancel = new RoutedCommand();
        public static RoutedCommand Confirm = new RoutedCommand();
        public static RoutedCommand Up = new RoutedCommand();
        public static RoutedCommand Down = new RoutedCommand();
        public static RoutedCommand Right = new RoutedCommand();
        public static RoutedCommand Left = new RoutedCommand();
        public static RoutedCommand Delete = new RoutedCommand();
    }
}
