using IMS_Library;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Linq;

namespace IMS_Interface.Data
{
    public class FileCache
    {
        private ConcurrentDictionary<string, CachedFile> CachedFiles = new ConcurrentDictionary<string, CachedFile>();

        private static readonly TimeSpan CacheTime = TimeSpan.FromSeconds(30); //s

        public FileCache()
        {
            if(!Directory.Exists(Constants.ExecutionPath + "/wwwroot/Cache/"))
            {
                Directory.CreateDirectory(Constants.ExecutionPath + "/wwwroot/Cache/");
            }
            RemoveUsedFiles();
        }

        public string CacheFile(string file)
        {
            RemoveUsedFiles();
            lock(this)
            {
                file = Path.GetFullPath(file);
                string extension = Path.GetExtension(file);
                if (CachedFiles.ContainsKey(file))
                {
                    CachedFile cached = CachedFiles[file];
                    cached.CreationTime = DateTime.Now;
                    return "/Cache/" + cached.AssociatedID + extension;
                }
                else
                {
                    CachedFile cached = CachedFiles[file] = new CachedFile();
                    File.Copy(file, Constants.ExecutionPath + "/wwwroot/Cache/" + cached.AssociatedID + extension);
                    return "/Cache/" + cached.AssociatedID + extension;
                }
            }
        }

        public void RemoveUsedFiles()
        {
            lock(this)
            {
                foreach (string file in Directory.GetFiles(Constants.ExecutionPath + "/wwwroot/Cache")) {
                    Guid fileID = Guid.Parse(Path.GetFileNameWithoutExtension(file));
                    KeyValuePair<string, CachedFile> cached = CachedFiles.Where(x => x.Value.AssociatedID == fileID).FirstOrDefault();
                    if(cached.Value is null || cached.Value.CreationTime + CacheTime < DateTime.Now)
                    {
                        File.Delete(file);
                        CachedFile bad;
                        try
                        {
                            CachedFiles.TryRemove(cached.Key, out bad);
                        } catch { }
                    }
                }
            }
        }

        private sealed class CachedFile
        {
            public Guid AssociatedID;
            public DateTime CreationTime;

            public CachedFile()
            {
                AssociatedID = Guid.NewGuid();
                CreationTime = DateTime.Now;
            }
        }
    }
}
