using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace IMS_Library
{
    /// <summary>
    /// This controller is in charge of keeping IMS up-to-date by downloading IMS version information and update packages as necessary.
    /// </summary>
    public sealed class UpdateController
    {
        /// <summary>
        /// The version of IMS that is currently running.
        /// </summary>
        public Version CurrentVersion => Assembly.GetAssembly(typeof(IMS)).GetName().Version;
        /// <summary>
        /// Whether there are new IMS updates ready for installation.
        /// </summary>
        public bool UpdatesReadyForInstallation => File.Exists(UpdateFile) && !IsUpdating;

        /// <summary>
        /// The URL which contains data about the latest version of IMS.  Fetching this URL should return a string that can be parsed into a <see cref="Version"/> object.
        /// </summary>
        public const string VersionDataURL = "http://raw.githubusercontent.com/DouglasDwyer/IntegratedMinecraftServer/master/IMS-Distribution/lfs-version.txt";
        /// <summary>
        /// The URL which contains a zipfile of the latest version of IMS.
        /// </summary>
        public const string LatestUpdateURL = "https://github.com/DouglasDwyer/IntegratedMinecraftServer/raw/master/IMS-Distribution/latest-update.zip";

        private static string UpdateFile => Constants.ExecutionPath + "/latest-update.zip";
        
        private bool IsUpdating = false;
        private object Locker = new object();
        private Timer UpdateCheckTimer = new Timer();

        /// <summary>
        /// Starts the controller, automatically checking to see whether updates are available.  If new updates have been downloaded, IMS may update and restart.
        /// </summary>
        public void Start()
        {
            string extJavaPath = Path.GetDirectoryName(Constants.ExecutionPath + Constants.JavaExecutableLocation) + "/../lib/";
            string extJavaFolder = extJavaPath + "ext";
            if (Directory.Exists(extJavaFolder))
            {
                Directory.Move(extJavaFolder, extJavaPath + "ext.old");
            }

            if(IMS.Instance.IsDevelopmentMode)
            {
                return;
            }
            lock (Locker)
            {
                if (UpdatesReadyForInstallation)
                {
                    UpdateAndRestart();
                }
                else
                {
                    DownloadUpdatesAsync();
                }
                UpdateCheckTimer.Interval = 30 * 60 * 1000;
                UpdateCheckTimer.Elapsed += async (x, y) =>
                {
                    await DownloadUpdatesAsync();
                    if(IMS.Instance.CurrentSettings.IMSAutoUpdateTime != null && IMS.Instance.CurrentSettings.IMSAutoUpdateTime.Value < DateTime.Now)
                    {
                        IMS.Instance.CurrentSettings.IMSAutoUpdateTime = DateTime.Today.AddDays(1) + IMS.Instance.CurrentSettings.IMSAutoUpdateTime.Value.TimeOfDay;
                        if(UpdatesReadyForInstallation)
                        {
                            UpdateAndRestart();
                        }
                        else
                        {
                            IMS.Instance.VersionManager.RestartUpdatedServers();
                        }
                    }
                };
                UpdateCheckTimer.Start();
            }
        }

        /// <summary>
        /// Stops the controller.
        /// </summary>
        public void Stop()
        {
            lock(Locker)
            {
                UpdateCheckTimer.Enabled = false;
            }
        }

        /// <summary>
        /// Causes IMS to shut down, copies the queued update files over the current IMS installation, and then starts IMS again.
        /// </summary>
        public void UpdateAndRestart()
        {

            Process process = new Process();
            process.StartInfo = new ProcessStartInfo();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.WorkingDirectory = Constants.ExecutionPath;
            process.StartInfo.Arguments = "/C Update.bat latest-update.zip";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            IMS.Instance.Stop();
        }

        /// <summary>
        /// Gets version data from the internet about the latest copy of IMS, and downloads the latest version if IMS is out-of-date.
        /// </summary>
        /// <returns>A <see cref="Task"/> object representing the current state of the download operation.</returns>
        public async Task DownloadUpdatesAsync()
        {
            lock(Locker)
            {
                if (UpdatesReadyForInstallation || IsUpdating)
                {
                    return;
                }
                IsUpdating = true;
            }            
            try
            {
                using WebClient client = new WebClient();
                Version newVersion;
                if (Version.TryParse(await client.DownloadStringTaskAsync(VersionDataURL), out newVersion))
                {
                    if(newVersion > CurrentVersion)
                    {
                        await client.DownloadFileTaskAsync(LatestUpdateURL, UpdateFile);
                        IMS.Instance.UserMessageManager.LogInfo("A new version of IMS (v" + newVersion + ") is available for download.", false);
                    }
                }
                else
                {
                    IMS.Instance.UserMessageManager.LogError("Unable to fetch IMS version data from " + VersionDataURL + "!  The version string was not in the correct format.  To update IMS, a reinstall may be required.");
                }
            }
            catch(Exception e)
            {
                Logger.WriteWarning("Unable to fetch IMS version data from " + VersionDataURL + "!\n" + e);
            }
            IsUpdating = false;
        }
    }
}
