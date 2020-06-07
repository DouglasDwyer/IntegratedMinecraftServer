using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace IMS_Library
{
    public static class Extensions
    {
        public static bool Remove<K, V>(this ConcurrentDictionary<K,V> dictionary, K key)
        {
            V value;
            return dictionary.TryRemove(key, out value);
        }

        public static bool IsFileLocked(this FileInfo file)
        {
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }

            //file is not locked
            return false;
        }

        public static void CopyFolder(string sourceDirectory, string targetDirectory)
        {
            var diSource = new DirectoryInfo(sourceDirectory);
            var diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        public static void HoboCopyFolder(string sourceDirectory, string targetDirectory)
        {
            HoboCopyAll(sourceDirectory, targetDirectory);
            foreach(DirectoryInfo directory in new DirectoryInfo(sourceDirectory).GetDirectories())
            {
                HoboCopyFolder(directory.FullName, Path.Combine(targetDirectory, directory.Name));
            }
        }

        private static void HoboCopyAll(string sourceDirectory, string targetDirectory)
        {
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo();
            process.StartInfo.FileName = Constants.ExecutionPath + Constants.BinariesFolderLocation + "/HoboCopy.exe";
            process.StartInfo.Arguments = sourceDirectory + " " + targetDirectory;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
        }
    }
}
