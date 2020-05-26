using IMS_Library;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IMS_Interface.Data
{
    public class FileCache
    {
        protected ConcurrentDictionary<string, DateTime> CachedFiles = new ConcurrentDictionary<string, DateTime>();

        private const int CacheTime = 30; //s

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
            lock(this)
            {
                string fileName = Path.GetFileName(file);
                string finalName = "/Cache/" + fileName;
                if (!CachedFiles.ContainsKey(fileName))
                {
                    File.Copy(file, Constants.ExecutionPath + "/wwwroot/Cache/" + fileName);
                }
                CachedFiles[fileName] = DateTime.Now.AddSeconds(30);
                return finalName;
            }
        }

        public void RemoveUsedFiles()
        {
            lock(this)
            {
                foreach (string file in Directory.GetFiles(Constants.ExecutionPath + "/wwwroot/Cache")) {
                    string fileName = Path.GetFileName(file);
                    if(!CachedFiles.ContainsKey(fileName) || CachedFiles[fileName] < DateTime.Now)
                    {
                        File.Delete(fileName);
                        DateTime bad;
                        CachedFiles.TryRemove(fileName, out bad);
                    }
                }
            }
        }
    }
}
