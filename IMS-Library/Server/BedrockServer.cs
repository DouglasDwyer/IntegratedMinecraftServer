using Newtonsoft.Json;
using RoyalXML;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace IMS_Library
{
    /// <summary>
    /// Allows for control over a Minecraft: Bedrock Edition server process, doing things like managing crashes or maintaining server preferences.
    /// </summary>
    public class BedrockServer : ServerProxy
    {
        /// <summary>
        /// The current server settings.  The server must be restarted for changes to take effect.
        /// </summary>
        public override ServerConfiguration CurrentConfiguration { get => ServerPreferences; set {
                if (State != ServerState.Disabled)
                {
                    throw new InvalidOperationException("Cannot change server settings while the server is running");
                }
                else
                {
                    ServerPreferences = (BedrockServerConfiguration)value;
                    ServerPreferences.SaveConfiguration();
                }
            }
        }

        /// <summary>
        /// The Mojang-assigned identifier for the version of Minecraft that the server is currently running.
        /// </summary>
        public override string ServerVersionID => throw new NotImplementedException();

        /// <summary>
        /// Whether the server supports whitelisting players.
        /// </summary>
        public override bool SupportsWhitelist => true;

        /// <summary>
        /// Whether the server whitelist is currently enabled.
        /// </summary>
        public override bool WhitelistEnabled { get => ServerPreferences.Whitelist;
            set {
                //SendUncheckedConsoleCommand("whitelist " + (value ? "on" : "off"));
                ServerPreferences.Whitelist = value;
                RestartAsync();
            }
        }

        /// <summary>
        /// Whether the server supports banning players.
        /// </summary>
        public override bool SupportsBanning => false;

        /// <summary>
        /// Whether the server supports banning players by IP address.
        /// </summary>
        public override bool SupportsBanningIPs => false;

        /// <summary>
        /// Whether the server supports designating players as server operators.
        /// </summary>
        public override bool SupportsOps => true;

        /// <summary>
        /// Whether the server has the ability to record player IP addresses.
        /// </summary>
        public override bool SupportsIPs => false;

        /// <summary>
        /// Whether the server supports kicking online players.
        /// </summary>
        public override bool SupportsKicking => true;

        /// <summary>
        /// This server supports Minecraft: Bedrock Edition.
        /// </summary>
        public sealed override MinecraftEdition SupportedEdition => MinecraftEdition.Bedrock;

        /// <summary>
        /// Returns the absolute path of the Minecraft server's data folder.
        /// </summary>
        protected string ServerLocation => ServerPreferences.GetServerFolderLocation();
        /// <summary>
        /// Returns the absolute path of the Minecraft server's log folder.
        /// </summary>
        protected string LogFolderLocation => ServerLocation + "/log/";
        /// <summary>
        /// Returns the absolute path of the Minecraft world folder that this server is currently using.
        /// </summary>
        protected string WorldLocation => ServerLocation + "/worlds/" + ServerPreferences.LevelName;

        /// <summary>
        /// This object is used to provide thread-safety when interacting with the <see cref="JavaServer"/> instance.  It should be locked whenever a method is interacting with the object's non-public fields.
        /// </summary>
        protected object Locker = new object();

        /// <summary>
        /// This dictionary contains information about all known players, including players that have never joined the server (like a player that is whitelisted but has never played).  It is indexed by username.
        /// </summary>
        protected Dictionary<string, MinecraftPlayer> AllUsers = new Dictionary<string, MinecraftPlayer>();
        /// <summary>
        /// This dictionary contains information about online players.  It is indexed by username.
        /// </summary>
        protected Dictionary<string, MinecraftPlayer> OnlineUsers = new Dictionary<string, MinecraftPlayer>();

        /// <summary>
        /// The location of the server EXE file that should be executed.
        /// </summary>
        protected virtual string ExeLocation => IMS.Instance.VersionManager.LatestBedrockRelease.PhysicalLocation;

        /// <summary>
        /// The current internal Minecraft server process, or null if the server is not running.
        /// </summary>
        protected Process ServerProcess;
        /// <summary>
        /// The current server settings.  The server must be restarted for changes to take effect.
        /// </summary>
        protected BedrockServerConfiguration ServerPreferences;

        
        private FileSystemWatcher WhitelistWatcher, PermissionsWatcher;
        /// <summary>
        /// This object writes console data to a server log, as Bedrock servers currently do not make saved console logs.
        /// </summary>
        protected StreamWriter LogWriter;
        private List<string> ConsoleText = new List<string>();

        private BackupState CurrentBackupState = BackupState.None;
        private Timer LogManagementTimer;

        /// <summary>
        /// Creates a new <see cref="BedrockServer"/> instance with the specified unique identifier and server configuration data.
        /// </summary>
        /// <param name="id">The unique identifier that this server is associated with.</param>
        /// <param name="configuration">The settings that this server should use.</param>
        public BedrockServer(Guid id, BedrockServerConfiguration configuration) : base(id)
        {
            ServerPreferences = configuration;
            SetupLogDeletionTimer();
        }

        /// <summary>
        /// Causes the server to make a snapshot of the current world, backing the world up to the specified location.  Currently, the server must be turned off to access world files, so this method causes the server to restart.
        /// </summary>
        /// <param name="location">The absolute path of the Minecraft world folder to back up to.</param>
        /// <returns>A <see cref="Task"/> object representing the current state of the backup operation.</returns>
        public override async Task BackupToLocationAsync(string location)
        {
            await StopAsync();
            await Task.Run(() =>
            {
                Extensions.CopyFolder(WorldLocation, location);
            });
            StartAsync();
            return;
            //so this is what we would do normally, but there's a bug where the BDS takes out an exclusive file lock on the world files, meaning we have to shut down to backup
            try
            {
                lock (Locker)
                {
                    if (CurrentBackupState != BackupState.None)
                    {
                        return;
                    }
                    CurrentBackupState = BackupState.Copying;
                }
                SendUncheckedConsoleCommand("save hold");
                while (CurrentBackupState == BackupState.Saving)
                {
                    await Task.Delay(250);
                    SendUncheckedConsoleCommand("save query");
                }
                await Task.Run(() =>
                {
                    Extensions.CopyFolder(WorldLocation, location);
                });
            }
            finally
            {
                SendUncheckedConsoleCommand("save resume");
                CurrentBackupState = BackupState.None;
            }
        }

        /// <summary>
        /// Causes the server to make a snapshot of the current world, backing the world up to the specified zip file.  Currently, the server must be turned off to access the world files, so this method causes the server to restart.
        /// </summary>
        /// <param name="file">The absolute path of the zip file to back up to.</param>
        /// <returns>A <see cref="Task"/> object representing the current state of the backup operation.</returns>
        public override async Task BackupToZipFileAsync(string file)
        {
            await StopAsync();
            await Task.Run(() =>
            {
                ZipFile.CreateFromDirectory(WorldLocation, file);
            });
            StartAsync();
            return;
            return;
            try
            {
                lock (Locker)
                {
                    if (CurrentBackupState != BackupState.None)
                    {
                        return;
                    }
                    CurrentBackupState = BackupState.Saving;
                }
                SendUncheckedConsoleCommand("save hold");
                while (CurrentBackupState == BackupState.Saving)
                {
                    await Task.Delay(250);
                    SendUncheckedConsoleCommand("save query");
                }
                await Task.Run(() =>
                {
                    string location = Path.GetTempPath() + "/IMS/" + Guid.NewGuid();
                    Extensions.CopyFolder(WorldLocation, location);
                    ZipFile.CreateFromDirectory(location, file);
                    Directory.Delete(location, true);
                });
            }
            finally
            {
                SendUncheckedConsoleCommand("save resume");
                CurrentBackupState = BackupState.None;
            }
        }

        /// <summary>
        /// Bans a player by IP address.
        /// </summary>
        /// <param name="ip">The IP address of the player to ban.</param>
        /// <param name="reason">The reason that the IP address is being banned.</param>
        /// <exception cref="NotSupportedException">
        /// Minecraft: Bedrock Edition servers do not currently support banning IP addresses.
        /// </exception>
        public override void BanIP(string ip, string reason)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Bans a player from the server, kicking them from the game and preventing them from rejoining.
        /// </summary>
        /// <param name="name">The name of the player to ban.</param>
        /// <param name="reason">The reason the player is being banned.  This may be left null.</param>
        /// <exception cref="NotSupportedException">
        /// Minecraft: Bedrock Edition servers do not currently support banning players.
        /// </exception>
        public override void BanPlayer(string name, string reason)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Revokes a player's server operator status.
        /// </summary>
        /// <param name="name">The name of the player to demote.</param>
        public override void DeopPlayer(string name)
        {
            lock (Locker)
            {
                if (AllUsers.ContainsKey(name))
                {
                    BedrockOpTag[] tags = JsonConvert.DeserializeObject<BedrockOpTag[]>(File.ReadAllText(ServerLocation + "/permissions.json"));
                    MinecraftPlayer player = AllUsers[name];
                    for (int i = 0; i < tags.Length; i++)
                    {
                        if (tags[i].xuid == player.UUID)
                        {
                            tags[i].permission = "member";
                        }
                    }
                    player.PermissionLevel = 0;
                    File.WriteAllText(ServerLocation + "/permissions.json", JsonConvert.SerializeObject(tags));
                    ReloadServerPermissions();
                }
            }
        }

        /// <summary>
        /// Retrieves a list of information about IPs that have been banned from the server.
        /// </summary>
        /// <returns>An enumerable of information about banned IPs.</returns>
        /// <exception cref="NotSupportedException">
        /// Minecraft: Bedrock Edition servers do not currently support banning IP addresses.
        /// </exception>
        public override List<BanIPTag> GetAllBannedIPs()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves a list of ban information about players who have been banned from the server.
        /// </summary>
        /// <returns>An enumerable of information about bans.</returns>
        /// <exception cref="NotSupportedException">
        /// Minecraft: Bedrock Edition servers do not currently support banning players.
        /// </exception>
        public override List<BanInformation> GetAllBans()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves a list of players on the server who have operator status.
        /// </summary>
        /// <returns>An enumerable of players.</returns>
        public override IEnumerable<MinecraftPlayer> GetAllOps()
        {
            lock(Locker)
            {
                return from player in AllUsers.Values where player.PermissionLevel > 0 select player;
            }
        }

        /// <summary>
        /// Retrieves a list of all players who have every logged onto the server.
        /// </summary>
        /// <returns>An enumerable of players.</returns>
        public override IEnumerable<MinecraftPlayer> GetAllPlayers()
        {
            lock(Locker)
            {
                return AllUsers.Values.Where(x => x.LastConnectionEvent != default);
            }
        }

        /// <summary>
        /// Retrieves a list of all players with whitelist status on the server.
        /// </summary>
        /// <returns>An enumerable of players.</returns>
        public override IEnumerable<MinecraftPlayer> GetAllWhitelistedPlayers()
        {
            lock (Locker)
            {
                return from player in AllUsers.Values where player.IsWhitelisted select player;
            }
        }

        /// <summary>
        /// Retrieves a reasonably-large block of recent text from the Minecraft server console.
        /// </summary>
        /// <returns>A block of recent server console text.</returns>
        public override string GetConsoleText()
        {
            lock (Locker)
            {
                string toReturn = string.Concat(ConsoleText.ToArray());
                if (toReturn.EndsWith("\n"))
                {
                    toReturn = toReturn.Remove(toReturn.Length - 1);
                }
                return toReturn;
            }
        }

        /// <summary>
        /// Gets the text inside a specific logfile.
        /// </summary>
        /// <param name="info">The logfile to get information about.</param>
        /// <returns>The information that the logfile contains.</returns>
        public override string GetLogFile(LogFileInformation info)
        {
            if (info.CleanExit)
            {
                return File.ReadAllText(LogFolderLocation + "/" + info.Name + ".log");
            }
            else
            {
                return File.ReadAllText(LogFolderLocation + "/" + info.Name + ".loge");
            }
        }

        /// <summary>
        /// Retrieves a list of logfiles that this server has produced.
        /// </summary>
        /// <returns>A list with information about each logfile created by the Minecraft server.</returns>
        public override IEnumerable<LogFileInformation> GetAllLogFiles()
        {
            List<LogFileInformation> files = new List<LogFileInformation>();
            foreach (string file in Directory.EnumerateFiles(LogFolderLocation, "*.log", SearchOption.TopDirectoryOnly))
            {
                if(new FileInfo(file).Name == "latest.log")
                {
                    continue;
                }
                files.Add(new LogFileInformation { Name = Path.GetFileNameWithoutExtension(file), CreationDate = File.GetCreationTime(file), CleanExit = true });
            }
            foreach (string file in Directory.EnumerateFiles(LogFolderLocation, "*.loge", SearchOption.TopDirectoryOnly))
            {
                files.Add(new LogFileInformation { Name = Path.GetFileNameWithoutExtension(file), CreationDate = File.GetCreationTime(file), CleanExit = false });
            }
            files = files.OrderBy(info => info.Name).ToList();
            return files;
        }

        /// <summary>
        /// Retrieves a list of all online players.
        /// </summary>
        /// <returns>An enumerable of players.</returns>
        public override IEnumerable<MinecraftPlayer> GetOnlinePlayers()
        {
            lock (Locker)
            {
                return OnlineUsers.Values.ToArray();
            }
        }

        /// <summary>
        /// Get information about a player by their username.
        /// </summary>
        /// <param name="username">The username of the player to retrieve data about.</param>
        /// <returns>Information about the specified player, or null if no player was found.</returns>
        public override MinecraftPlayer GetPlayerInformationByUsername(string username)
        {
            lock (Locker)
            {
                return AllUsers.ContainsKey(username) ? AllUsers[username] : null;
            }
        }

        /// <summary>
        /// Get information about a player by their UUID.
        /// </summary>
        /// <param name="uuid">The UUID of the player to retrieve data about.</param>
        /// <returns>Information about the specified player, or null if no player was found.</returns>
        public override MinecraftPlayer GetPlayerInformationByUUID(string uuid)
        {
            lock(Locker)
            {
                return AllUsers.Values.Where(x => x.UUID == uuid).FirstOrDefault();
            }
        }

        /// <summary>
        /// Kicks a player from the server.
        /// </summary>
        /// <param name="name">The name of the player to disconnect.</param>
        /// <param name="reason">The reason for the player's disconnection.</param>
        public override void KickPlayer(string name, string reason)
        {
            SendUncheckedConsoleCommand("kick " + name + " " + reason);
        }

        /// <summary>
        /// Gives server operator status to the specified player.
        /// </summary>
        /// <param name="name">The player to make server operator.</param>
        public override void OpPlayer(string name)
        {
            lock (Locker)
            {
                if (AllUsers.ContainsKey(name))
                {
                    BedrockOpTag[] tags = JsonConvert.DeserializeObject<BedrockOpTag[]>(File.ReadAllText(ServerLocation + "/permissions.json"));
                    MinecraftPlayer player = AllUsers[name];
                    for (int i = 0; i < tags.Length; i++)
                    {
                        if (tags[i].xuid == player.UUID)
                        {
                            tags[i].permission = "operator";
                        }
                    }
                    player.PermissionLevel = 4;
                    File.WriteAllText(ServerLocation + "/permissions.json", JsonConvert.SerializeObject(tags));
                }
                ReloadServerPermissions();
            }
        }

        /// <summary>
        /// Causes the internal Minecraft server to reload the server permissions file from disk.
        /// </summary>
        public override void ReloadServerPermissions()
        {
            SendUncheckedConsoleCommand("permission reload");
        }

        /// <summary>
        /// Causes the internal Minecraft server to reload the whitelist from disk.
        /// </summary>
        public override void ReloadServerWhitelist()
        {
            SendUncheckedConsoleCommand("whitelist reload");
        }

        /// <summary>
        /// Removes a player from the server whitelist, preventing them from joining the game while the whitelist is enabled.  If <see cref="JavaServerConfiguration.EnforceWhitelist"/> is true and this player is currently online, they will be kicked from the game.
        /// </summary>
        /// <param name="name">The name of the player to remove from the whitelist.</param>
        public override void RemoveWhitelistPlayer(string name)
        {
            lock (Locker)
            {
                List<BedrockWhitelistTag> tags = new List<BedrockWhitelistTag>(JsonConvert.DeserializeObject<BedrockWhitelistTag[]>(File.ReadAllText(ServerLocation + "/whitelist.json")));
                int index = tags.FindIndex(x => x.name == name);
                if(index >= 0)
                {
                    tags.RemoveAt(index);
                }
                File.WriteAllText(ServerLocation + "/whitelist.json", JsonConvert.SerializeObject(tags.ToArray()));
                if (AllUsers.ContainsKey(name))
                {
                    AllUsers[name].IsWhitelisted = false;
                }
                ReloadServerWhitelist();
            }
        }

        /// <summary>
        /// Restarts the server, blocking until the server has entered the <see cref="ServerProxy.ServerState.Running"/> state.
        /// </summary>
        /// <returns>A <see cref="Task"/> object representing the current state of the restart operation.</returns>
        public override async Task RestartAsync()
        {
            await StopAsync();
            await StartAsync();
        }

        /// <summary>
        /// Sends a command to the server console.  If the command is "stop," shuts down the server and sets <see cref="ServerConfiguration.IsEnabled"/> to false.
        /// </summary>
        /// <param name="command">The command to send to the server console.</param>
        public override void SendConsoleCommand(string command)
        {
            if(command.Trim() == "stop")
            {
                StopAsync();
            }
            else
            {
                SendUncheckedConsoleCommand(command);
            }
        }

        /// <summary>
        /// Starts the internal Minecraft server process.  This call does not complete until the server leaves the <see cref="ServerProxy.ServerState.Starting"/> state.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents the current start operation.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the server is already running when this method is called.
        /// </exception>
        public override async Task StartAsync()
        {
            lock (Locker)
            {
                if (State != ServerState.Disabled)
                {
                    throw new InvalidOperationException("Cannot start a server that's already running!");
                }
                State = ServerState.Starting;
            }
            ServerVersionInformation info = IMS.Instance.VersionManager.LatestBedrockRelease;
            if(info.PhysicalLocation is null)
            {
                await info.DownloadServerBinaryAsync();
            }
            await Task.Run(() => EnsureBedrockTemplateFilesExist(info));
            lock (Locker) {
                try
                {
                    World world = IMS.Instance.WorldManager.GetWorldByID(CurrentConfiguration.WorldID);
                    if (world is null)
                    {
                        world = new World(Guid.NewGuid());
                        world.Name = CurrentConfiguration.ServerName + " World";
                        world.Edition = MinecraftEdition.Bedrock;
                        ServerPreferences.WorldID = world.ID;
                        IMS.Instance.WorldManager.AddWorldToRegistry(world);
                    }
                    JunctionPoint.Create(WorldLocation, world.WorldPath, true);
                    if (File.Exists(LogFolderLocation + "/latest.log"))
                    {
                        if (File.GetAttributes(LogFolderLocation + "/latest.log") != FileAttributes.Hidden)
                        {
                            File.Copy(LogFolderLocation + "/latest.log", LogFolderLocation + "/" + File.GetCreationTime(ServerPreferences.GetServerFolderLocation() + "/logs/latest.log").ToString("yyyy-dd-M--HH-mm-ss") + ".loge", true);
                        }
                        File.Delete(LogFolderLocation + "/latest.log");
                    }
                    if (File.Exists(ServerPreferences.GetServerFolderLocation() + "/usercache.xml"))
                    {
                        foreach (MinecraftPlayer player in RoyalSerializer.XMLToObject<MinecraftPlayer[]>(File.ReadAllText(ServerPreferences.GetServerFolderLocation() + "/usercache.xml")))
                        {
                            AllUsers[player.Username] = player;
                        }
                    }

                    if(!Directory.Exists(LogFolderLocation))
                    {
                        Directory.CreateDirectory(LogFolderLocation);
                    }
                    LogWriter = new StreamWriter(LogFolderLocation + "/latest.log");
                    LogWriter.AutoFlush = true;

                    MinecraftConfigurationWriter.WriteServerPropertiesFile("server.properties", ServerPreferences);

                    WhitelistWatcher = new FileSystemWatcher(ServerLocation, "whitelist.json");
                    WhitelistWatcher.NotifyFilter = NotifyFilters.LastWrite;
                    WhitelistWatcher.Changed += (x, y) => ReloadWhitelistJSON();
                    PermissionsWatcher = new FileSystemWatcher(ServerLocation, "permissions.json");
                    PermissionsWatcher.NotifyFilter = NotifyFilters.LastWrite;
                    PermissionsWatcher.Changed += (x, y) => ReloadOpJSON();

                    WhitelistWatcher.EnableRaisingEvents = true;
                    PermissionsWatcher.EnableRaisingEvents = true;

                    ReloadWhitelistJSON();
                    ReloadOpJSON();

                    ServerProcess = new Process();
                    ServerProcess.StartInfo = new ProcessStartInfo();

                    if(File.Exists(ServerLocation + "/Bedrock.exe"))
                    {
                        File.Delete(ServerLocation + "/Bedrock.exe");
                    }

                    JunctionPoint.CreateHardLink(Constants.ExecutionPath + Constants.JavaBinariesFolderLocation + "/Bedrock.exe", ServerLocation + "/Bedrock.exe");

                    ServerProcess.StartInfo.FileName = ExeLocation;

                    ServerProcess.StartInfo.WorkingDirectory = ServerPreferences.GetServerFolderLocation();
                    ServerProcess.StartInfo.LoadUserProfile = false;
                    ServerProcess.StartInfo.UseShellExecute = false;
                    ServerProcess.StartInfo.CreateNoWindow = true;

                    ServerProcess.StartInfo.RedirectStandardInput = true;
                    ServerProcess.StartInfo.RedirectStandardOutput = true;
                    ServerProcess.StartInfo.RedirectStandardError = true;

                    ServerProcess.OutputDataReceived += OnServerConsoleDataReceived;
                    ServerProcess.ErrorDataReceived += OnServerConsoleDataReceived;

                    ServerProcess.EnableRaisingEvents = true;

                    Process local = ServerProcess;
                    ServerProcess.Exited += (x, y) => {
                        if (local.EnableRaisingEvents)
                        {
                            OnServerProcessDie();
                        }
                    };

                    IMS.Instance.FirewallManager.CreateFirewallExecutableException("Server" + ID, ServerProcess.StartInfo.FileName);
                    foreach (int port in ServerPreferences.GetPortsToForward())
                    {
                        IMS.Instance.PortManager.ForwardPort(port);
                    }

                    ServerProcess.Start();

                    ServerProcess.BeginOutputReadLine();
                    ServerProcess.BeginErrorReadLine();
                    ChildProcessTracker.AddProcess(ServerProcess);
                    LogManagementTimer.Start();
                }
                catch (Exception e)
                {
                    Logger.WriteWarning("Server " + ID + " was unable to start.\n" + e);
                    State = ServerState.Disabled;
                    CurrentConfiguration.IsEnabled = false;
                }
            }
            while (State == ServerState.Starting) { await Task.Delay(1); }
        }

        /// <summary>
        /// Returns a string that describes the current object.
        /// </summary>
        /// <returns>A string with the name of this server type.</returns>
        public override string ToString()
        {
            return "Bedrock";
        }

        /// <summary>
        /// Stops the internal server process.  This call does not complete until the server has reached the <see cref="ServerProxy.ServerState.Disabled"/> state.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents the current state of the stop operation.</returns>
        public override async Task StopAsync()
        {
            lock (Locker)
            {
                if (State == ServerState.Disabled || State == ServerState.Stopping) { return; }
                State = ServerState.Stopping;
            }

            LogManagementTimer.Stop();

            WhitelistWatcher.EnableRaisingEvents = false;
            WhitelistWatcher.Dispose();
            PermissionsWatcher.EnableRaisingEvents = false;
            PermissionsWatcher.Dispose();

            ServerProcess.EnableRaisingEvents = false;
            SendUncheckedConsoleCommand("stop");
            foreach (MinecraftPlayer player in OnlineUsers.Values)
            {
                player.LastConnectionEvent = DateTime.Now;
            }
            OnlineUsers.Clear();
            File.WriteAllText(ServerPreferences.GetServerFolderLocation() + "/usercache.xml", RoyalSerializer.ObjectToXML(AllUsers.Values.ToArray()));

            foreach (int port in ServerPreferences.GetPortsToForward())
            {
                IMS.Instance.PortManager.RemovePort(port);
            }
            IMS.Instance.FirewallManager.RemoveFirewallExecutableException("Server" + ID);

            while(!ServerProcess.HasExited) { await Task.Delay(1); }

            LogWriter.Close();
            File.Copy(LogFolderLocation + "/latest.log", LogFolderLocation + "/" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".log");
            File.Delete(LogFolderLocation + "/latest.log");

            State = ServerState.Disabled;
            ServerProcess = null;
        }

        /// <summary>
        /// Pardons an IP, allowing players with that IP to rejoin the server after an IP ban.
        /// </summary>
        /// <param name="ip">The IP address to unban.</param>
        /// <exception cref="NotSupportedException">
        /// Minecraft: Bedrock Edition servers do not currently support banning IP addresses.
        /// </exception>
        public override void UnbanIP(string ip)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Pardons a player, allowing them to rejoin the server after a ban.
        /// </summary>
        /// <param name="name">The name of the player to unban.</param>
        /// <exception cref="NotSupportedException">
        /// Minecraft: Bedrock Edition servers do not currently support banning players.
        /// </exception>
        public override void UnbanPlayer(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a player to the server whitelist, allowing them to join the game while the whitelist is enabled.
        /// </summary>
        /// <param name="name">The name of the player to whitelist.</param>
        public override void WhitelistPlayer(string name)
        {
            lock (Locker)
            {
                List<BedrockWhitelistTag> tags = new List<BedrockWhitelistTag>(JsonConvert.DeserializeObject<BedrockWhitelistTag[]>(File.ReadAllText(ServerLocation + "/whitelist.json")));
                if(tags.FindIndex(x => x.name == name) < 0)
                {
                    tags.Add(new BedrockWhitelistTag { name = name });
                    File.WriteAllText(ServerLocation + "/whitelist.json", JsonConvert.SerializeObject(tags.ToArray()));
                    if (AllUsers.ContainsKey(name))
                    {
                        AllUsers[name].IsWhitelisted = false;
                    }
                    ReloadServerWhitelist();
                }
            }
        }

        /// <summary>
        /// Reloads the server whitelist from its file on disk, updating IMS data accordingly.
        /// </summary>
        protected void ReloadWhitelistJSON()
        {
            lock (Locker) {
                try
                {
                    FileInfo file = new FileInfo(ServerLocation + "/whitelist.json");
                    while (file.IsFileLocked()) { Thread.Sleep(1); }
                    BedrockWhitelistTag[] tags = JsonConvert.DeserializeObject<BedrockWhitelistTag[]>(File.ReadAllText(file.FullName));
                    foreach (MinecraftPlayer player in AllUsers.Values)
                    {
                        player.IsWhitelisted = false;
                    }
                    foreach (BedrockWhitelistTag tag in tags)
                    {
                        if (AllUsers.ContainsKey(tag.name))
                        {
                            AllUsers[tag.name].IsWhitelisted = true;
                        }
                        else
                        {
                            AllUsers[tag.name] = new MinecraftPlayer() { Username = tag.name };
                        }
                    }
                }
                catch(Exception e)
                {
                    Logger.WriteError("Couldn't reload whitelist JSON for server " + ID + "!\n" + e);
                }
            }
        }

        /// <summary>
        /// Reloads server operator information from disk, updating IMS data accordingly.
        /// </summary>
        protected void ReloadOpJSON()
        {
            lock (Locker)
            {
                try
                {
                    FileInfo file = new FileInfo(ServerLocation + "/permissions.json");
                    while(file.IsFileLocked()) { Thread.Sleep(1); }
                    BedrockOpTag[] tags = JsonConvert.DeserializeObject<BedrockOpTag[]>(File.ReadAllText(file.FullName));
                    foreach (BedrockOpTag tag in tags)
                    {
                        MinecraftPlayer player = AllUsers.Values.Where(x => x.UUID == tag.xuid).FirstOrDefault();
                        if (player != null)
                        {
                            player.PermissionLevel = tag.permission == "operator" ? 4 : 0;
                        }
                    }
                }
                catch(Exception e)
                {
                    Logger.WriteError("Couldn't reload op JSON for server " + ID + "!\n" + e);
                }
            }
        }

        /// <summary>
        /// This method is called when the internal Minecraft server posts data to its console.
        /// </summary>
        /// <param name="sender">The process which invoked this event handler.</param>
        /// <param name="args">The data that the server posted to its console.</param>
        protected virtual void OnServerConsoleDataReceived(object sender, DataReceivedEventArgs args)
        {
            try
            {
                if (args.Data is null)
                {
                    return;
                }

                lock (Locker)
                {
                    ConsoleText.Add(args.Data + "\n");
                    if (ConsoleText.Count > 100)
                    {
                        ConsoleText.RemoveAt(0);
                    }
                    LogWriter.WriteLine(args.Data);

                    Match regexMatch;
                    if (MatchRegex(args.Data, @"^\[[0-9-]* [0-9:]* INFO] (.*)$", out regexMatch))
                    {
                        string data = regexMatch.Groups[1].Value;
                        if (MatchRegex(data, @"^Player connected: ([^,;]+), xuid: ([0-9]*)$", out regexMatch))
                        {
                            string name = regexMatch.Groups[1].Value, uuid = regexMatch.Groups[2].Value;
                            MinecraftPlayer player;
                            if (AllUsers.ContainsKey(name))
                            {
                                player = AllUsers[name];
                                if (player.PermissionLevel > 0)
                                {
                                    SendUncheckedConsoleCommand("op " + player.Username);
                                }
                            }
                            else
                            {
                                player = new MinecraftPlayer();
                                player.Username = name;
                                AllUsers[name] = player;
                            }
                            player.UUID = uuid;
                            player.LastConnectionEvent = DateTime.Now;
                            OnlineUsers[name] = player;
                        }
                        else if (MatchRegex(data, @"^Player disconnected: ([^,;]+), xuid: [0-9]*$", out regexMatch))
                        {
                            string name = regexMatch.Groups[1].Value;
                            if (OnlineUsers.ContainsKey(name))
                            {
                                OnlineUsers[name].LastConnectionEvent = DateTime.Now;
                                OnlineUsers.Remove(name);
                            }
                        }
                        else if (MatchRegex(data, @"^Server started.$", out regexMatch))
                        {
                            State = ServerState.Running;
                        }
                        else if (MatchRegex(data, @"Data saved. Files are now ready to be copied.", out regexMatch))
                        {
                            CurrentBackupState = BackupState.Copying;
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Logger.WriteError("Couldn't process console data for server " + ID + "!\n" + e);
            }
        }

        /// <summary>
        /// This method checks whether an input matches a Regex pattern, and if it does, saves that data to <paramref name="match"/>.
        /// </summary>
        /// <param name="data">The input string to parse with the Regex pattern.</param>
        /// <param name="pattern">The Regex pattern to use in parsing.</param>
        /// <param name="match">The data object that will describe the result of the parse operation once this method returns.</param>
        /// <returns>A <see cref="bool"/> which indicates whether the Regex pattern matched successfully.</returns>
        protected bool MatchRegex(string data, string pattern, out Match match)
        {
            match = Regex.Match(data, pattern);
            return match.Success;
        }

        /// <summary>
        /// This method is called when the server process quits.  If the server was in the <see cref="ServerProxy.ServerState.Running"/> state, the proxy will attempt to restart the server.
        /// </summary>
        protected virtual void OnServerProcessDie()
        {
            lock (Locker)
            {
                OnlineUsers.Clear();
                if (State != ServerState.Stopping)
                {
                    try
                    {
                        LogWriter.Close();
                        File.Copy(LogFolderLocation + "/latest.log", LogFolderLocation + "/" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".loge");
                        File.Delete(LogFolderLocation + "/latest.log");
                    }
                    catch { }
                    if (State == ServerState.Running)
                    {
                        IMS.Instance.UserMessageManager.LogError("Server " + ID + " crashed.  Attempting restart...");
                        State = ServerState.Disabled;
                        StartAsync();
                    }
                    else
                    {
                        IMS.Instance.UserMessageManager.LogError("Server " + ID + " crashed on startup and was subsequently disabled.");
                        State = ServerState.Disabled;
                        ServerPreferences.IsEnabled = false;
                    }
                }
                else
                {
                    State = ServerState.Disabled;
                }
            }
        }

        /// <summary>
        /// Sends a command to the Minecraft server console without checking the command beforehand.
        /// </summary>
        /// <param name="command">The command to send to the server console.</param>
        protected void SendUncheckedConsoleCommand(string command)
        {
            lock(Locker)
            {
                ServerProcess?.StandardInput.WriteLine(command);
            }
        }

        /// <summary>
        /// Deletes the specified logfile, removing it from disk.
        /// </summary>
        /// <param name="info">The logfile to delete.</param>
        public override void DeleteLogFile(LogFileInformation info)
        {
            if (info.CleanExit)
            {
                File.Delete(LogFolderLocation + "/" + info.Name + ".log");
            }
            else
            {
                File.Delete(LogFolderLocation + "/" + info.Name + ".loge");
            }
        }

        private void SetupLogDeletionTimer()
        {
            LogManagementTimer = new Timer();
            LogManagementTimer.Interval = 10 * 60 * 1000;
            LogManagementTimer.Elapsed += (x, y) => {
                if (ServerPreferences.LogDeletionInterval == default)
                {
                    return;
                }
                foreach (LogFileInformation info in GetAllLogFiles())
                {
                    if (info.CreationDate + ServerPreferences.LogDeletionInterval < DateTime.Now)
                    {
                        DeleteLogFile(info);
                    }
                }
            };
        }

        private void EnsureBedrockTemplateFilesExist(ServerVersionInformation info)
        {
            try
            {
                foreach (string file in Directory.GetFiles(Path.GetDirectoryName(info.PhysicalLocation)))
                {
                    string fileName = Path.GetFileName(file);
                    string newFile = Path.Combine(ServerLocation, fileName);
                    if (!File.Exists(newFile))
                    {
                        if (fileName == "bedrock_server.exe" || fileName == "bedrock_server.pdb" || fileName == "data.zip")
                        {
                            continue;
                        }
                        else
                        {
                            File.Copy(file, newFile);
                        }
                    }
                }
                foreach (string directory in Directory.GetDirectories(Path.GetDirectoryName(info.PhysicalLocation)))
                {
                    string folderName = new DirectoryInfo(directory).Name;
                    string newFolder = ServerLocation + "/" + folderName;
                    if (!Directory.Exists(newFolder))
                    {
                        if (folderName == "worlds")
                        {
                            Directory.CreateDirectory(newFolder);
                        }
                        else if (folderName == "internalStorage")
                        {
                            Extensions.CopyFolder(directory, newFolder);
                        }
                        else
                        {
                            JunctionPoint.Create(newFolder, directory, true);
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Logger.WriteError("Couldn't correctly copy Bedrock files!\n" + e);
                throw;
            }
        }

        private enum BackupState { None, Saving, Copying }
    }
}
