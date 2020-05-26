using IMS_Library;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.IO.Compression;

namespace IMS_Interface.Data
{
    public class DownloadManager
    {
        protected ConcurrentDictionary<string, DateTime> QueuedFiles = new ConcurrentDictionary<string, DateTime>();
        protected Task RemovingFilesTask = null;

        public DownloadManager()
        {
            if (!Directory.Exists(Constants.ExecutionPath + "/wwwroot/Download/"))
            {
                Directory.CreateDirectory(Constants.ExecutionPath + "/wwwroot/Download/");
            }
            RemoveUsedFilesAsync();
        }

        public async Task DownloadFileAsync(NavigationManager navigator, string file)
        {
            RemoveUsedFilesAsync();
            string id = Guid.NewGuid().ToString();
            string ext = new FileInfo(file).Extension;
            string newName = id + (string.IsNullOrEmpty(ext) ? "" : "." + ext);
            QueuedFiles[id] = DateTime.MaxValue;
            await Task.Run(() => File.Copy(file, Constants.ExecutionPath + "/wwwroot/Download/" + newName));
            QueuedFiles[id] = DateTime.Now;
            navigator.NavigateTo("/Download/" + newName, true);
        }

        public async Task DownloadServerWorldAsync(NavigationManager navigator, ServerProxy server)
        {
            RemoveUsedFilesAsync();
            string id = Guid.NewGuid().ToString();
            QueuedFiles[id] = DateTime.MaxValue;
            await Task.Run(async () => await IMS.AsThreadSafe(() => server.BackupToZipFileAsync(Constants.ExecutionPath + "/wwwroot/Download/" + id + ".zip")));
            QueuedFiles[id] = DateTime.Now;
            navigator.NavigateTo("/Download/" + id + ".zip", true);
        }

        public async Task ZipAndDownloadFolderAsync(NavigationManager navigator, string file)
        {
            RemoveUsedFilesAsync();
            string id = Guid.NewGuid().ToString();
            QueuedFiles[id] = DateTime.MaxValue;
            await Task.Run(() => ZipFile.CreateFromDirectory(file, Constants.ExecutionPath + "/wwwroot/Download/" + id + ".zip"));
            QueuedFiles[id] = DateTime.Now;
            navigator.NavigateTo("/Download/" + id + ".zip", true);
        }

        public Task RemoveUsedFilesAsync()
        {
            lock(this)
            {
                if(RemovingFilesTask is null)
                {
                    return RemovingFilesTask = DeleteUsedFilesAsync();
                }
                else
                {
                    return RemovingFilesTask;
                }
            }
        }

        private async Task DeleteUsedFilesAsync()
        {
            string[] files = Directory.GetFiles(Constants.ExecutionPath + "/wwwroot/Download/");
            foreach(string file in files)
            {
                FileInfo info = new FileInfo(file);
                string fileName = Path.GetFileNameWithoutExtension(file);
                if (!QueuedFiles.ContainsKey(fileName) || QueuedFiles[fileName] + new TimeSpan(0, 1, 0) < DateTime.Now && !Extensions.IsFileLocked(info))
                {
                    await Task.Run(() => info.Delete());
                    DateTime outer;
                    QueuedFiles.TryRemove(fileName, out outer);
                }
            }
        }
    }
}
