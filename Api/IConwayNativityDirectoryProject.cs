using ConwayNativityDirectory.PluginApi.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwayNativityDirectory.PluginApi
{
    public interface IConwayNativityDirectoryProject
    {
        /// <summary>
        /// Indicates if the project is currently opened.
        /// </summary>
        bool IsOpen { get; }

        /// <summary>
        /// Indicates the filename of the project that is open.
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// Gets all nativities in the current project.
        /// </summary>
        ObservableCollection<object> Nativities { get; }

        /// <summary>
        /// Gets all selected nativities.
        /// </summary>
        IEnumerable<NativityBase> SelectedNativities { get; }

        /// <summary>
        /// Gets the main selected nativity.
        /// </summary>
        NativityBase SelectedNativity { get; }

        /// <summary>
        /// Gets the main selected nativities index.
        /// </summary>
        int SelectedNativityIndex { get; }

        /// <summary>
        /// Gets the nativities that are in the current search results.
        /// </summary>
        List<object> SearchResults { get; }

        /// <summary>
        /// Gets the stack of actions performed by the user.
        /// </summary>
        ActionStack Actions { get; }

        event EventHandler PreviewNewProject;
        event EventHandler PreviewOpenProject;
        event EventHandler PreviewSaveProject;
        event EventHandler PreviewCloseProject;
        event EventHandler OnNewProject;
        event EventHandler OnOpenProject;
        event EventHandler OnSaveProject;
        event EventHandler OnCloseProject;
        event EventHandler SelectedNativitiesChanged;

        /// <summary>
        /// Starts a new project.
        /// </summary>
        void New();

        /// <summary>
        /// Opens a project.
        /// </summary>
        /// <param name="fileName"></param>
        void Open(string fileName);

        /// <summary>
        /// Saves the project.
        /// </summary>
        void Save();

        /// <summary>
        /// Saves a project to another destination and sets <see cref="IConwayNativityDirectoryProject.FileName"/> property to that destination.
        /// </summary>
        /// <param name="fileName"></param>
        void SaveAs(string fileName);

        /// <summary>
        /// Closes the current project.
        /// </summary>
        void Close();

        /// <summary>
        /// Gets the storage of the project.
        /// </summary>
        /// <returns></returns>
        MetaStorage GetMeta();

        /// <summary>
        /// Gets the local storage of the project. This data is not transfered in the project file but is only accessed by the current project.
        /// </summary>
        /// <returns></returns>
        MetaStorage GetLocalMeta();
    }
}
