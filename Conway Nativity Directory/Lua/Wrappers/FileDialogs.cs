using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conway_Nativity_Directory.Lua_Sandbox
{
    public class OpenFileDialog
    {
        public OpenFileDialog()
        {
            dialog = new System.Windows.Forms.OpenFileDialog();
        }
        
        private System.Windows.Forms.OpenFileDialog dialog;

        public string GetTitle() => dialog.Title;
        public void SetTitle(string title) => dialog.Title = title;

        public string GetInitialDirectory() => dialog.InitialDirectory;
        public void SetInitialDirectory(string initialDirectory) => dialog.InitialDirectory = initialDirectory;

        public string GetFileName() => dialog.FileName;
        public void SetFileName(string fileName) => dialog.FileName = fileName;

        public string GetSafeFileName() => dialog.SafeFileName;

        public string[] GetFileNames() => dialog.FileNames;

        public string[] GetSafeFileNames() => dialog.SafeFileNames;

        public string GetFiter() => dialog.Filter;
        public void SetFilter(string filter) => dialog.Filter = filter;

        public int GetFiterIndex() => dialog.FilterIndex;
        public void SetFilterIndex(int filterIndex) => dialog.FilterIndex = filterIndex;

        public bool GetMultiselect() => dialog.Multiselect;
        public void SetMultiselect(bool value) => dialog.Multiselect = value;

        public string ShowDialog() => dialog.ShowDialog().ToString();

        public void Dispose() => dialog.Dispose();
    }

    public class SaveFileDialog
    {
        public SaveFileDialog()
        {
            dialog = new System.Windows.Forms.SaveFileDialog();
        }

        private System.Windows.Forms.SaveFileDialog dialog;

        public string GetTitle() => dialog.Title;
        public void SetTitle(string title) => dialog.Title = title;

        public string GetInitialDirectory() => dialog.InitialDirectory;
        public void SetInitialDirectory(string initialDirectory) => dialog.InitialDirectory = initialDirectory;

        public string GetFileName() => dialog.FileName;
        public void SetFileName(string fileName) => dialog.FileName = fileName;

        public string[] GetFileNames() => dialog.FileNames;

        public string GetFiter() => dialog.Filter;
        public void SetFilter(string filter) => dialog.Filter = filter;

        public int GetFiterIndex() => dialog.FilterIndex;
        public void SetFilterIndex(int filterIndex) => dialog.FilterIndex = filterIndex;

        public string ShowDialog() => dialog.ShowDialog().ToString();

        public void Dispose() => dialog.Dispose();
    }

    public class FolderBrowserDialog
    {
        public FolderBrowserDialog()
        {
            dialog = new System.Windows.Forms.FolderBrowserDialog();
        }

        private System.Windows.Forms.FolderBrowserDialog dialog;

        public string GetSelectedPath() => dialog.SelectedPath;
        public void SetSelectedPath(string selectedPath) => dialog.SelectedPath = selectedPath;

        public string GetDescription() => dialog.Description;
        public void SetDescription(string description) => dialog.Description = description;

        public string ShowDialog() => dialog.ShowDialog().ToString();

        public void Dispose() => dialog.Dispose();
    }
}
