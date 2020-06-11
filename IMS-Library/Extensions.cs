using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace IMS_Library
{
    /// <summary>
    /// Provides general-purpose extension methods for various operations.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Removes an object from a <see cref="ConcurrentDictionary{TKey, TValue}"/> using its key.
        /// </summary>
        /// <typeparam name="K">The type of the key.</typeparam>
        /// <typeparam name="V">The type of value that the dictionary stores.</typeparam>
        /// <param name="dictionary">The dictionary to remove an item from.</param>
        /// <param name="key">The key of the item to remove.</param>
        /// <returns>A <see cref="bool"/> that represents whether the item was found/removed from the dictionary successfully.</returns>
        public static bool Remove<K, V>(this ConcurrentDictionary<K,V> dictionary, K key)
        {
            V value;
            return dictionary.TryRemove(key, out value);
        }

        /// <summary>
        /// This method checks whether another program has a file lock on the specified file.
        /// </summary>
        /// <param name="file">The file to check.</param>
        /// <returns>Whether the file is locked or not.</returns>
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

        /// <summary>
        /// Copies an entire folder to a different directory, including subdirectories.
        /// </summary>
        /// <param name="sourceDirectory">The path of the directory to copy.</param>
        /// <param name="targetDirectory">The path of the newly-created clone directory.</param>
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
    }
}
