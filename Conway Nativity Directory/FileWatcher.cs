using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.IO;
using System.Windows.Forms;
using System.Collections;
using System.Reflection;

namespace Conway_Nativity_Directory
{
    public class FileWatcher
    {
        public FileWatcher(string fileName)
        {
            FileName = fileName;
        }

        public string FileName { get; set; }

        public void Start()
        {
            fileExists = File.Exists(FileName);
            running = true;

            if (fileExists)
            {
                creationTime = new FileInfo(FileName).CreationTime;
            }

            Task.Run(() => WatchAsync());
        }

        public void Stop()
        {
            running = false;
        }

        public event EventHandler FileCreated;
        public event EventHandler FileDeleted;
        public event EventHandler FileModified;

        private async Task InvokeFileCreatedAsync()
        {
            App.Current.Dispatcher.Invoke(new Action(() => { 
                if (FileCreated != null)
                    FileCreated.Invoke(this, EventArgs.Empty);
            }));
            await Task.Delay(1);
        }
        private async Task InvokeFileDeletedAsync()
        {
            App.Current.Dispatcher.Invoke(new Action(() => {
                if (FileDeleted != null)
                    FileDeleted.Invoke(this, EventArgs.Empty);
            }));
            await Task.Delay(1);
        }
        private async Task InvokeFileModifiedAsync()
        {
            App.Current.Dispatcher.Invoke(new Action(() => {
                if (FileModified != null)
                    FileModified.Invoke(this, EventArgs.Empty);
            }));
            await Task.Delay(1);
        }

        bool fileExists = false;
        DateTime creationTime = DateTime.Now;
        bool running = false;
        private async Task WatchAsync()
        {
            do
            {
                bool newFileExists = File.Exists(FileName);
                if (newFileExists != fileExists)
                {
                    fileExists = newFileExists;
                    if (newFileExists)
                        await InvokeFileCreatedAsync();
                    else if (!newFileExists)
                        await InvokeFileDeletedAsync();
                }

                if (newFileExists)
                {
                    FileInfo fileInfo = new FileInfo(FileName);
                    if (fileInfo.CreationTime != creationTime)
                        await InvokeFileModifiedAsync();

                    creationTime = fileInfo.CreationTime;
                }
            }

            while (running == true);

            await Task.Delay(1);
        }
    }

    internal class MultiFileWatcherFileData
    {
        string fileName;
        public string FileName { get { return fileName; } }

        bool fileExists;
        public bool FileExists { get { return fileExists; } }

        DateTime creationTime;
        public DateTime CreationTime { get { return creationTime; } }

        public MultiFileWatcherFileData(string fileName)
        {
            this.fileName = fileName;
            Update();
        }

        public void Update()
        {
            this.fileExists = File.Exists(FileName);
            this.creationTime = new FileInfo(FileName).CreationTime;
        }
    }

    public class MultiFileWatcher : IEnumerable<string>
    {
        private List<MultiFileWatcherFileData> fileDatas = new List<MultiFileWatcherFileData>();
        private List<string> files { get { return fileDatas.Select(x => x.FileName).ToList(); } }

        public void AddFile(string fileName)
        {
            Path.GetFullPath(fileName); //Throws an error if the path is not a valid path. Irrelevent to wether it exist or not.
            if (files.Where(f => f == fileName).FirstOrDefault() != null)
                fileDatas.Add(new MultiFileWatcherFileData(fileName));
        }

        public void RemoveFile(string fileName)
        {
            try
            {
                fileDatas.Remove(fileDatas.Where(f => f.FileName == fileName).FirstOrDefault());
            }

            catch { }
        }
        public void Start()
        {
            running = true;
            Task.Run(() => WatchAsync());
        }

        public void Stop()
        {
            running = false;
        }

        bool running = false;
        private async Task WatchAsync()
        {
            do
            {
                foreach(MultiFileWatcherFileData outdated in fileDatas)
                {
                    var updated = new MultiFileWatcherFileData(outdated.FileName);

                    if (updated.FileExists != outdated.FileExists)
                    {
                        if (updated.FileExists)
                            await InvokeFileChangedAsync(new MultiFileWatcherEventArgs(this, outdated.FileName, MultiFileWatcherEventType.FileCreated));
                        else if (!updated.FileExists)
                            await InvokeFileChangedAsync(new MultiFileWatcherEventArgs(this, outdated.FileName, MultiFileWatcherEventType.FileCreated));
                    }

                    if (updated.CreationTime != outdated.CreationTime)
                        await InvokeFileChangedAsync(new MultiFileWatcherEventArgs(this, outdated.FileName, MultiFileWatcherEventType.FileModified));

                    outdated.Update();
                }
            }

            while (running == true);

            await Task.Delay(1);
        }

        public event MultiFileWatcherEventHandler FileChanged;

        private async Task InvokeFileChangedAsync(MultiFileWatcherEventArgs e)
        {
            App.Current.Dispatcher.Invoke(new Action(() => {
                if (FileChanged != null)
                    FileChanged.Invoke(this, e);
            }));
            await Task.Delay(1);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return files.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public delegate void MultiFileWatcherEventHandler(object sender, MultiFileWatcherEventArgs e);

    public class MultiFileWatcherEventArgs : EventArgs
    {
        MultiFileWatcher sender;
        public MultiFileWatcher Sender { get { return sender; } }

        string fileName;
        public string FileName { get { return fileName; } }

        MultiFileWatcherEventType eventType;
        public MultiFileWatcherEventType EventType { get { return eventType; } }

        public MultiFileWatcherEventArgs(MultiFileWatcher sender, string fileName, MultiFileWatcherEventType eventType)
        {
            this.sender = sender;
            this.fileName = fileName;
            this.eventType = eventType;
        }
    }

    public enum MultiFileWatcherEventType { FileCreated, FileDeleted, FileModified };
}
