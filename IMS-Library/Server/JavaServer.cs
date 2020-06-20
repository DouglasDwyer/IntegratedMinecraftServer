using KinglyStudios.Knetworking;
using Newtonsoft.Json;
using RoyalXML;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace IMS_Library
{
    /// <summary>
    /// Allows for control over a Minecraft: Java Edition server process, doing things like managing crashes or maintaining server preferences.
    /// </summary>
    public class JavaServer : ServerProxy
    {
        /// <summary>
        /// The current server settings.  The server must be restarted for changes to take effect.
        /// </summary>
        protected JavaServerConfiguration ServerPreferences;
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
                    ServerPreferences = (JavaServerConfiguration)value;
                    ServerPreferences.SaveConfiguration();
                }
            }
        }

        /// <summary>
        /// Whether the server supports whitelisting players.
        /// </summary>
        public override bool SupportsWhitelist => true;

        /// <summary>
        /// Whether the server whitelist is currently enabled.
        /// </summary>
        public override bool WhitelistEnabled { get => ServerPreferences.UseWhitelist; set => SendUncheckedConsoleCommand("whitelist " + (value ? "on" : "off")); }

        /// <summary>
        /// Whether the server supports banning players.
        /// </summary>
        public override bool SupportsBanning => true;

        /// <summary>
        /// Whether the server supports banning players by IP address.
        /// </summary>
        public override bool SupportsBanningIPs => true;

        /// <summary>
        /// Whether the server supports designating players as server operators.
        /// </summary>
        public override bool SupportsOps => true;

        /// <summary>
        /// Whether the server has the ability to record player IP addresses.
        /// </summary>
        public override bool SupportsIPs => true;

        /// <summary>
        /// Whether the server supports kicking online players.
        /// </summary>
        public override bool SupportsKicking => true;

        /// <summary>
        /// The Mojang-assigned identifier for the version of Minecraft that the server is currently running.
        /// </summary>
        public override string ServerVersionID => ServerVersion;
        private string ServerVersion;
        /// <summary>
        /// This server supports Minecraft: Java Edition.
        /// </summary>
        public sealed override MinecraftEdition SupportedEdition => MinecraftEdition.Java;

        /// <summary>
        /// The absolute path of the server icon.  This property does not check whether the icon exists.
        /// </summary>
        public virtual string ServerIconLocation => Constants.ExecutionPath + Constants.ServerFolderLocation + "/" + ID + "/server-icon.png";

        /// <summary>
        /// The current internal Minecraft server process, or null if the server is not running.
        /// </summary>
        protected Process ServerProcess;

        /// <summary>
        /// This list contains information about all the players on the server who have been banned.
        /// </summary>
        protected List<BanInformation> BanList = new List<BanInformation>();
        /// <summary>
        /// This dictionary contains information about all the IP addresses that have been banned, indexed by IP address.
        /// </summary>
        protected Dictionary<string, BanIPTag> BanIPList = new Dictionary<string, BanIPTag>();

        /// <summary>
        /// This dictionary contains information about online players.  It is indexed by UUID if <see cref="JavaServerConfiguration.OnlineMode"/> is true, otherwise, it is indexed by username.
        /// </summary>
        protected Dictionary<string, MinecraftPlayer> OnlineUsers = new Dictionary<string, MinecraftPlayer>();
        /// <summary>
        /// This dictionary contains information about all known players, including players that have never joined the server (like a player that is whitelisted but has never played).  It is indexed by UUID if <see cref="JavaServerConfiguration.OnlineMode"/> is true, otherwise, it is indexed by username.
        /// </summary>
        protected Dictionary<string, MinecraftPlayer> AllUsers = new Dictionary<string, MinecraftPlayer>();
        private MemoryCache UUIDCache;

        private List<string> ConsoleText = new List<string>();

        private bool AutomaticSavingPlayerEnabled = true;
        private bool HasCompletedAutomaticSave = true;
        private Timer LogManagementTimer;

        /// <summary>
        /// The location of the server JAR file that should be executed.
        /// </summary>
        protected virtual string JarLocation => (string.IsNullOrEmpty(ServerPreferences.ServerVersion)
                            ? IMS.Instance.VersionManager.LatestRelease.PhysicalLocation : IMS.Instance.VersionManager.AvailableServerVersions[ServerPreferences.ServerVersion].PhysicalLocation).Replace("/", "\\");

        /// <summary>
        /// The absolute path of the server world.
        /// </summary>
        protected string WorldLocation { get => ServerPreferences.GetServerFolderLocation() + "/" + ServerPreferences.LevelName; }

        /// <summary>
        /// This object is used to provide thread-safety when interacting with the <see cref="JavaServer"/> instance.  It should be locked whenever a method is interacting with the object's non-public fields.
        /// </summary>
        protected object Locker = new object();

        /// <summary>
        /// Creates a new <see cref="JavaServer"/> instance with the specified unique identifier and server configuration data.
        /// </summary>
        /// <param name="id">The unique identifier that this server is associated with.</param>
        /// <param name="configuration">The settings that this server should use.</param>
        public JavaServer(Guid id, JavaServerConfiguration configuration) : base(id)
        {
            ServerPreferences = configuration;
            UUIDCache = new MemoryCache("IMS", null, true);
            State = ServerState.Disabled;
            SetupLogDeletionTimer();
        }

        /// <summary>
        /// Kicks a player from the server.
        /// </summary>
        /// <param name="name">The name of the player to disconnect.</param>
        /// <param name="reason">The reason for the player's disconnection.</param>
        public override void KickPlayer(string name, string reason)
        {
            if (string.IsNullOrEmpty(reason))
            {
                SendUncheckedConsoleCommand("kick " + name);
            }
            else
            {
                SendUncheckedConsoleCommand("kick " + name + " " + reason);
            }
        }

        /// <summary>
        /// Returns a string that describes the current object.
        /// </summary>
        /// <returns>A string with the name of this server type.</returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(ServerPreferences.ServerVersion))
            {
                return "Java " + IMS.Instance.VersionManager.LatestRelease.Name;
            }
            else
            {
                return "Java " + IMS.Instance.VersionManager.AvailableServerVersions[ServerPreferences.ServerVersion].Name;
            }
        }

        /// <summary>
        /// Retrieves a list of all online players.
        /// </summary>
        /// <returns>An enumerable of players.</returns>
        public override IEnumerable<MinecraftPlayer> GetOnlinePlayers()
        {
            lock (Locker)
            {
                return OnlineUsers.Values;
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
        /// This method is called when the server process quits.  If the server was in the <see cref="ServerProxy.ServerState.Running"/> state, the proxy will attempt to restart the server.
        /// </summary>
        protected virtual void OnServerProcessDie()
        {
            lock (Locker)
            {
                foreach (MinecraftPlayer player in OnlineUsers.Values)
                {
                    player.LastConnectionEvent = DateTime.Now;
                }
                OnlineUsers.Clear();
                if (State != ServerState.Stopping)
                {
                    if (State == ServerState.Running)
                    {
                        IMS.Instance.UserMessageManager.LogError("Server " + CurrentConfiguration.ServerName + " crashed.  Attempting restart...");
                        State = ServerState.Disabled;
                        StartAsync();
                    }
                    else
                    {
                        IMS.Instance.UserMessageManager.LogError("Server " + CurrentConfiguration.ServerName + " crashed on startup and was subsequently disabled.");
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
        /// Gives server operator status to the specified player.
        /// </summary>
        /// <param name="name">The player to make server operator.</param>
        public override void OpPlayer(string name)
        {
            lock (Locker)
            {
                if (State == ServerState.Disabled)
                {
                    List<OpTag> tags = JsonConvert.DeserializeObject<OpTag[]>(File.ReadAllText(ServerPreferences.GetServerFolderLocation() + "/ops.json")).ToList();
                    if (ServerPreferences.OnlineMode)
                    {
                        tags.Add(new OpTag { name = name, bypassesPlayerLimit = false, level = ServerPreferences.DefaultOpPermissionLevel, uuid = MojangInteropUtility.GetUUIDFromUsername(name) });
                    }
                    else
                    {
                        tags.Add(new OpTag { name = name, bypassesPlayerLimit = false, level = ServerPreferences.DefaultOpPermissionLevel });
                    }
                    File.WriteAllText(ServerPreferences.GetServerFolderLocation() + "/ops.json", JsonConvert.SerializeObject(tags.ToArray()));
                }
                else
                {
                    SendUncheckedConsoleCommand("op " + name);
                }
            }
        }

        /// <summary>
        /// Causes the server to make a snapshot of the current world, backing the world up to the specified location.
        /// </summary>
        /// <param name="location">The absolute path of the Minecraft world folder to back up to.</param>
        /// <returns>A <see cref="Task"/> object representing the current state of the backup operation.</returns>
        public override async Task BackupToLocationAsync(string location)
        {
            lock (Locker)
            {
                if (!HasCompletedAutomaticSave)
                {
                    return;
                }
                HasCompletedAutomaticSave = false;
            }
            if(AutomaticSavingPlayerEnabled)
            {
                SendUncheckedConsoleCommand("save-off");
            }
            SendUncheckedConsoleCommand("save-all");
            while(!HasCompletedAutomaticSave)
            {
                await Task.Delay(1);
            }
            await Task.Run(() => {
                Extensions.CopyFolder(WorldLocation, location);
                if (AutomaticSavingPlayerEnabled)
                {
                    SendUncheckedConsoleCommand("save-on");
                }
            });
        }

        /// <summary>
        /// Causes the server to make a snapshot of the current world, backing the world up to the specified zip file.
        /// </summary>
        /// <param name="file">The absolute path of the zip file to back up to.</param>
        /// <returns>A <see cref="Task"/> object representing the current state of the backup operation.</returns>
        public override async Task BackupToZipFileAsync(string file)
        {
            lock (Locker)
            {
                if (!HasCompletedAutomaticSave)
                {
                    return;
                }
                HasCompletedAutomaticSave = false;
            }
            if (AutomaticSavingPlayerEnabled)
            {
                SendUncheckedConsoleCommand("save-off");
            }
            SendUncheckedConsoleCommand("save-all");
            while (!HasCompletedAutomaticSave)
            {
                await Task.Delay(1);
            }
            await Task.Run(() => {
                string location = Path.GetTempPath() + "/IMS/" + Guid.NewGuid();
                Extensions.CopyFolder(WorldLocation, location);
                ZipFile.CreateFromDirectory(location, file);
                Directory.Delete(location, true);
                if (AutomaticSavingPlayerEnabled)
                {
                    SendUncheckedConsoleCommand("save-on");
                }
            });
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
            if (command == "stop")
            {
                ServerPreferences.IsEnabled = false;
                StopAsync();
            }
            else
            {
                SendUncheckedConsoleCommand(command);
            }
        }

        /// <summary>
        /// Sends a command to the Minecraft server console without checking the command beforehand.
        /// </summary>
        /// <param name="command">The command to send to the server console.</param>
        protected void SendUncheckedConsoleCommand(string command)
        {
            lock (ServerProcess)
            {
                ServerProcess.StandardInput.WriteLine(command);
            }
        }

        /// <summary>
        /// Reloads the server whitelist from its file on disk, updating IMS data accordingly.
        /// </summary>
        protected void ReloadWhitelistJSON()
        {
            lock (Locker) {
                foreach (MinecraftPlayer player in AllUsers.Values)
                {
                    player.IsWhitelisted = false;
                }
                if (File.Exists(ServerPreferences.GetServerFolderLocation() + "/ops.json"))
                {
                    WhitelistTag[] tags = JsonConvert.DeserializeObject<WhitelistTag[]>(File.ReadAllText(ServerPreferences.GetServerFolderLocation() + "/whitelist.json"));
                    foreach (WhitelistTag tag in tags)
                    {
                        MinecraftPlayer player = GetPlayerByIdentifier(tag.name, tag.uuid);
                        if (player is null)
                        {
                            player = new MinecraftPlayer();
                            player.Username = tag.name;
                            player.UUID = tag.uuid;
                            AddPlayerToAllPlayersList(player);
                        }
                        player.IsWhitelisted = true;
                    }
                }
            }
        }

        /// <summary>
        /// Registers a player as part of the all-players list.
        /// </summary>
        /// <param name="player">The player to add.  The player does not need to have played on the server before, but if that is the case, then <see cref="MinecraftPlayer.LastConnectionEvent"/> should be equal to <see cref="DateTime.MinValue"/>.</param>
        protected void AddPlayerToAllPlayersList(MinecraftPlayer player)
        {
            lock (Locker)
            {
                if (ServerPreferences.OnlineMode)
                {
                    AllUsers[player.UUID] = player;
                }
                else
                {
                    AllUsers[player.Username] = player;
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
                foreach (MinecraftPlayer player in AllUsers.Values)
                {
                    player.PermissionLevel = 0;
                }
                if (File.Exists(ServerPreferences.GetServerFolderLocation() + "/ops.json"))
                {
                    OpTag[] tags = JsonConvert.DeserializeObject<OpTag[]>(File.ReadAllText(ServerPreferences.GetServerFolderLocation() + "/ops.json"));
                    foreach (OpTag tag in tags)
                    {
                        MinecraftPlayer player = GetPlayerByIdentifier(tag.name, tag.uuid);
                        if (player is null)
                        {
                            player = new MinecraftPlayer();
                            player.Username = tag.name;
                            player.UUID = tag.uuid;
                            AddPlayerToAllPlayersList(player);
                        }
                        player.PermissionLevel = tag.level;
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves a player's information given both their name and UUID.
        /// </summary>
        /// <param name="name">The name of the player to find.</param>
        /// <param name="uuid">The UUID of the player to find.</param>
        /// <returns>The player's data, or null if no player was found.</returns>
        protected virtual MinecraftPlayer GetPlayerByIdentifier(string name, string uuid)
        {
            lock (Locker)
            {
                if (ServerPreferences.OnlineMode)
                {
                    if (AllUsers.ContainsKey(uuid))
                    {
                        return AllUsers[uuid];
                    }
                }
                else
                {
                    if (AllUsers.ContainsKey(name))
                    {
                        return AllUsers[name];
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Gets a player's information given both their name or UUID.
        /// </summary>
        /// <param name="name">The name of the player to find.</param>
        /// <param name="uuid">The UUID of the player to find.</param>
        /// <returns>The found player, or a new Minecraft player with the specified name and UUID if no player was found.</returns>
        protected virtual MinecraftPlayer GetPlayerOrDefaultByIdentifier(string name, string uuid)
        {
            lock (Locker)
            {
                MinecraftPlayer toReturn = GetPlayerByIdentifier(name, uuid);
                if (toReturn is null)
                {
                    toReturn = new MinecraftPlayer();
                    toReturn.Username = name;
                    toReturn.UUID = uuid;
                }
                return toReturn;
            }
        }

        /// <summary>
        /// Reloads the list of banned players from disk.
        /// </summary>
        protected void ReloadBanJSON()
        {
            lock (Locker)
            {
                BanList.Clear();
                if (File.Exists(ServerPreferences.GetServerFolderLocation() + "/banned-players.json"))
                {
                    BanTag[] tags = JsonConvert.DeserializeObject<BanTag[]>(File.ReadAllText(ServerPreferences.GetServerFolderLocation() + "/banned-players.json"));
                    foreach (BanTag tag in tags)
                    {
                        BanInformation information = new BanInformation();
                        information.Reason = tag.reason;
                        information.BanSource = tag.source;
                        information.CreatedDate = tag.created;
                        information.ExpirationDate = tag.expires;
                        information.Player = GetPlayerByIdentifier(tag.name, tag.uuid);
                        if (information.Player is null)
                        {
                            information.Player = new MinecraftPlayer();
                            information.Player.Username = tag.name;
                            information.Player.UUID = tag.uuid;
                            AddPlayerToAllPlayersList(information.Player);
                        }
                        BanList.Add(information);
                    }
                }
            }
        }

        /// <summary>
        /// Reloads the list of banned IPs from disk.
        /// </summary>
        protected void ReloadBanIPJSON()
        {
            lock(Locker) {
            BanIPList.Clear();
                if (File.Exists(ServerPreferences.GetServerFolderLocation() + "/ops.json"))
                {
                    BanIPTag[] tags = JsonConvert.DeserializeObject<BanIPTag[]>(File.ReadAllText(ServerPreferences.GetServerFolderLocation() + "/banned-ips.json"));
                    foreach (BanIPTag tag in tags)
                    {
                        BanIPList[tag.ip] = tag;
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves the permission level of a player from their UUID.
        /// </summary>
        /// <param name="uuid">The UUID of the player to get information about.</param>
        /// <returns>An <see cref="int"/> representing the player's permission level.</returns>
        public int GetOperatorStatusOfPlayerByUUID(string uuid)
        {
            lock (Locker)
            {
                return AllUsers[uuid].PermissionLevel;
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
                try
                {
                    World world = IMS.Instance.WorldManager.GetWorldByID(CurrentConfiguration.WorldID);
                    if (world is null)
                    {
                        world = new World(Guid.NewGuid());
                        world.Name = CurrentConfiguration.ServerName + " World";
                        world.Edition = MinecraftEdition.Java;
                        ServerPreferences.WorldID = world.ID;
                        IMS.Instance.WorldManager.AddWorldToRegistry(world);
                    }
                    JunctionPoint.Create(WorldLocation, world.WorldPath, true);
                    if (File.Exists(ServerPreferences.GetServerFolderLocation() + "/logs/latest.log"))
                    {
                        if (File.GetAttributes(ServerPreferences.GetServerFolderLocation() + "/logs/latest.log") != FileAttributes.Hidden)
                        {
                            File.Copy(ServerPreferences.GetServerFolderLocation() + "/logs/latest.log", ServerPreferences.GetServerFolderLocation() + "/logs/" + File.GetCreationTime(ServerPreferences.GetServerFolderLocation() + "/logs/latest.log").ToString("yyyy-dd-M--HH-mm-ss") + ".loge", true);
                        }
                        File.Delete(ServerPreferences.GetServerFolderLocation() + "/logs/latest.log");
                    }
                    if (File.Exists(ServerPreferences.GetServerFolderLocation() + "/usercache.xml"))
                    {
                        foreach (MinecraftPlayer player in RoyalSerializer.XMLToObject<MinecraftPlayer[]>(File.ReadAllText(ServerPreferences.GetServerFolderLocation() + "/usercache.xml")))
                        {
                            if (ServerPreferences.OnlineMode)
                            {
                                AllUsers[player.UUID] = player;
                            }
                            else
                            {
                                AllUsers[player.Username] = player;
                            }
                        }
                    }

                    MinecraftConfigurationWriter.WriteServerPropertiesFile("server.properties", ServerPreferences);
                    MinecraftConfigurationWriter.WriteEULA("eula.txt", ServerPreferences);

                    ReloadWhitelistJSON();
                    ReloadOpJSON();
                    ReloadBanJSON();
                    ReloadBanIPJSON();

                    ServerProcess = new Process();
                    ServerProcess.StartInfo = new ProcessStartInfo();

                    ServerProcess.StartInfo.FileName = Constants.ExecutionPath + Constants.JavaExecutableLocation;

                    ServerProcess.StartInfo.Arguments = "-Xms" + ServerPreferences.MinimumMemoryMB
                        + "M -Xmx" + ServerPreferences.MaximumMemoryMB
                        + "M " + ServerPreferences.JavaArguments
                        + " -jar \"" + JarLocation + "\" nogui";

                    ServerVersion = string.IsNullOrEmpty(ServerPreferences.ServerVersion) ? IMS.Instance.VersionManager.LatestRelease.Version : IMS.Instance.VersionManager.AvailableServerVersions[ServerPreferences.ServerVersion].Version;

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
                    ServerProcess.Exited += (x, y) =>
                    {
                        if (local.EnableRaisingEvents)
                        {
                            OnServerProcessDie();
                        }
                    };

                    IMS.Instance.FirewallManager.CreateFirewallExecutableException("Server" + ID, ServerProcess.StartInfo.FileName);
                    foreach(int port in ServerPreferences.GetPortsToForward())
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
                    CurrentConfiguration.IsEnabled = false;
                    State = ServerState.Disabled;
                }
            }
            while (State == ServerState.Starting) { await Task.Delay(1); }
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
                lock (Locker)
                {
                    if (args.Data is null)
                    {
                        return;
                    }

                    ConsoleText.Add(args.Data + "\n");
                    while (ConsoleText.Count > 100)
                    {
                        ConsoleText.RemoveAt(0);
                    }

                    //It might be worth refactoring these else-ifs at some point

                    string data = StripServerLogOfTimestamp(args.Data);
                    if (data.StartsWith("[Server thread/INFO]: "))
                    {
                        data = data.Substring("Server thread/INFO]: ".Length + 1);
                        Match regexMatch = null;
                        //Manage server starting
                        if (State == ServerState.Starting && data.StartsWith("Done"))
                        {
                            State = ServerState.Running;
                        }
                        //Manage players joining
                        else if (MatchRegex(data, "^([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+)\\[\\/([0123456789.)]+):[0123456789]+] logged in with entity id [0123456789]+", out regexMatch))
                        {
                            MinecraftPlayer loggingInPlayer = null;
                            string playerName = regexMatch.Groups[1].Value;
                            string uuid = null;
                            try
                            {
                                uuid = (string)UUIDCache[playerName];
                            }
                            catch { }
                            if (ServerPreferences.OnlineMode)
                            {
                                if (AllUsers.ContainsKey(uuid))
                                {
                                    loggingInPlayer = AllUsers[uuid];
                                }
                            }
                            else
                            {
                                if (AllUsers.ContainsKey(playerName))
                                {
                                    loggingInPlayer = AllUsers[playerName];
                                }
                            }
                            if (loggingInPlayer is null)
                            {
                                loggingInPlayer = new MinecraftPlayer();
                                loggingInPlayer.Username = playerName;
                                loggingInPlayer.UUID = uuid;
                                if (ServerPreferences.OnlineMode)
                                {
                                    AllUsers[loggingInPlayer.UUID] = loggingInPlayer;
                                }
                                else
                                {
                                    AllUsers[loggingInPlayer.Username] = loggingInPlayer;
                                }
                            }
                            loggingInPlayer.IP = regexMatch.Groups[2].Value;
                            loggingInPlayer.LastConnectionEvent = DateTime.Now;
                            if (ServerPreferences.OnlineMode)
                            {
                                OnlineUsers[loggingInPlayer.UUID] = loggingInPlayer;
                            }
                            else
                            {
                                OnlineUsers[loggingInPlayer.Username] = loggingInPlayer;
                            }
                        }
                        //Manage players leaving
                        else if (MatchRegex(data, "^([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+) left the game$", out regexMatch))
                        {
                            MinecraftPlayer leavingPlayer = GetPlayerInformationByUsername(regexMatch.Groups[1].Value);
                            leavingPlayer.LastConnectionEvent = DateTime.Now;
                            if (ServerPreferences.OnlineMode)
                            {
                                OnlineUsers.Remove(leavingPlayer.UUID);
                            }
                            else
                            {
                                OnlineUsers.Remove(leavingPlayer.Username);
                            }
                        }
                        //Manage players using /op
                        else if (MatchRegex(data, "^\\[[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+: Made ([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+) a server operator]$", out regexMatch))
                        {
                            ReloadOpJSON();
                        }
                        //Manage players using /ban
                        else if (MatchRegex(data, "^\\[([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+): Banned ([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+): (.*)(?=\\])]$", out regexMatch))
                        {
                            ReloadBanJSON();
                        }
                        //Manage players using /ban-ip
                        else if (MatchRegex(data, "^\\[([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+): Banned IP ([0123456789.]+): (.*)(?=\\])]$", out regexMatch))
                        {
                            ReloadBanIPJSON();
                        }
                        //Manage players using /pardon
                        else if (MatchRegex(data, "^\\[([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+): Unbanned ([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+)]$", out regexMatch))
                        {
                            ReloadBanJSON();
                        }
                        //Manage players using /pardon-ip
                        else if (MatchRegex(data, "^\\[([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+): Unbanned IP ([0123456789.]+)]$", out regexMatch))
                        {
                            ReloadBanIPJSON();
                        }
                        //Manage players using /deop
                        else if (MatchRegex(data, "^\\[[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+: Made ([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+) no longer a server operator]$", out regexMatch))
                        {
                            ReloadOpJSON();
                        }
                        //Manage players using /whitelist reload
                        else if (MatchRegex(data, "^\\[[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+: Reloaded the whitelist]$", out regexMatch))
                        {
                            ReloadWhitelistJSON();
                        }
                        //Manage players using /whitelist add
                        else if (MatchRegex(data, "^\\[[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+: Added ([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+) to the whitelist]$", out regexMatch))
                        {
                            ReloadWhitelistJSON();
                        }
                        //Manage players using /whitelist remove
                        else if (MatchRegex(data, "^\\[[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+: Removed ([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+) from the whitelist]$", out regexMatch))
                        {
                            ReloadWhitelistJSON();
                        }
                        //Manage players using /whitelist on
                        else if (MatchRegex(data, "^\\[[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+: Whitelist is now turned on]$", out regexMatch))
                        {
                            ServerPreferences.UseWhitelist = true;
                        }
                        //Manage players using /whitelist off
                        else if (MatchRegex(data, "^\\[[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+: Whitelist is now turned off]$", out regexMatch))
                        {
                            ServerPreferences.UseWhitelist = false;
                        }
                        //Manage SERVER
                        //Manage server using /op
                        else if (MatchRegex(data, "^Made ([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+) a server operator$", out regexMatch))
                        {
                            ReloadOpJSON();
                        }
                        //Manage server using /ban
                        else if (MatchRegex(data, "^Banned ([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+): (.*)$", out regexMatch))
                        {
                            ReloadBanJSON();
                        }
                        //Manage server using /ban-ip
                        else if (MatchRegex(data, "^Banned IP ([0123456789.]+): (.*)$", out regexMatch))
                        {
                            ReloadBanIPJSON();
                        }
                        //Manage server using /pardon
                        else if (MatchRegex(data, "^Unbanned ([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+)$", out regexMatch))
                        {
                            ReloadBanJSON();
                        }
                        //Manage server using /pardon-ip
                        else if (MatchRegex(data, "^Unbanned IP ([0123456789.]+)$", out regexMatch))
                        {
                            ReloadBanIPJSON();
                        }
                        //Manage server using /deop
                        else if (MatchRegex(data, "^Made ([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+) no longer a server operator$", out regexMatch))
                        {
                            ReloadOpJSON();
                        }
                        //Manage server using /whitelist reload
                        else if (MatchRegex(data, "^Reloaded the whitelist$", out regexMatch))
                        {
                            ReloadWhitelistJSON();
                        }
                        //Manage server using /whitelist add
                        else if (MatchRegex(data, "^Added ([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+) to the whitelist$", out regexMatch))
                        {
                            ReloadWhitelistJSON();
                        }
                        //Manage server using /whitelist remove
                        else if (MatchRegex(data, "^Removed ([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+) from the whitelist$", out regexMatch))
                        {
                            ReloadWhitelistJSON();
                        }
                        //Manage server using /whitelist on
                        else if (MatchRegex(data, "^Whitelist is now turned on$", out regexMatch))
                        {
                            ServerPreferences.UseWhitelist = true;
                        }
                        //Manage server using /whitelist off
                        else if (MatchRegex(data, "^[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+: Whitelist is now turned off$", out regexMatch))
                        {
                            ServerPreferences.UseWhitelist = false;
                        }
                        //Manage server using /save on
                        else if (MatchRegex(data, "^Automatic saving is now enabled$", out regexMatch))
                        {
                            AutomaticSavingPlayerEnabled = true;
                        }
                        //Manage server using /save off
                        else if (MatchRegex(data, "^Automatic saving is now disabled$", out regexMatch))
                        {
                            if (HasCompletedAutomaticSave)
                            {
                                AutomaticSavingPlayerEnabled = false;
                            }
                        }
                        else if (data == "Saved the game")
                        {
                            HasCompletedAutomaticSave = true;
                        }
                        //end
                        //Manage players stopping the server
                        else if (MatchRegex(data, "^\\[[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+: Stopping the server]$", out regexMatch))
                        {
                            OnlineUsers.Clear();
                            ServerPreferences.IsEnabled = false;
                            State = ServerState.Stopping;
                        }
                        //Manage players using /save on
                        else if (MatchRegex(data, "^\\[[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+: Automatic saving is now enabled]$", out regexMatch))
                        {
                            AutomaticSavingPlayerEnabled = true;
                        }
                        //Manage players using /save off
                        else if (MatchRegex(data, "^\\[[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+: Automatic saving is now disabled]$", out regexMatch))
                        {
                            AutomaticSavingPlayerEnabled = false;
                        }
                    }
                    else if (data.StartsWith("[User Authenticator #"))
                    {
                        UUIDCache.Add(data.Substring(data.IndexOf("UUID of player ") + "UUID of player ".Length, data.IndexOf(" is ") - (data.IndexOf("UUID of player ") + "UUID of player ".Length)), data.Substring(data.IndexOf(" is ") + 4), new CacheItemPolicy() { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(20) });
                    }
                }
            }
            catch(Exception e)
            {
                Logger.WriteError("Couldn't process console data for server " + ID + "!\n" + e);
            }
        }

        private string StripServerLogOfTimestamp(string logLine)
        {
            return logLine.Substring(11);
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
            ServerVersion = null;
            LogManagementTimer.Stop();
            ServerProcess.EnableRaisingEvents = false;
            SendUncheckedConsoleCommand("stop");

            foreach(MinecraftPlayer player in OnlineUsers.Values)
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

            while (!ServerProcess.HasExited) { await Task.Delay(1); }
            if (File.Exists(ServerPreferences.GetServerFolderLocation() + "/logs/latest.log"))
            {
                File.Copy(ServerPreferences.GetServerFolderLocation() + "/logs/latest.log", ServerPreferences.GetServerFolderLocation() + "/logs/" + File.GetCreationTime(ServerPreferences.GetServerFolderLocation() + "/logs/latest.log").ToString("yyyy-dd-M--HH-mm-ss") + ".log");
                try
                {
                    //Set an attribute because sometimes Windows thinks that the MC server is still using the file even when the process has exited
                    //We check for this attribute to identify whether a logfile was backed up properly or not
                    File.SetAttributes(ServerPreferences.GetServerFolderLocation() + "/logs/latest.log", FileAttributes.Hidden);
                    File.Delete(ServerPreferences.GetServerFolderLocation() + "/logs/latest.log");
                }
                catch (Exception e)
                {
                    Logger.WriteWarning("Couldn't delete logfile; it seems to still be in use by other process?\n" + e);
                }
            }
            State = ServerState.Disabled;
            ServerProcess = null;
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
            Regex regex = new Regex(pattern);
            match = regex.Match(data);
            return match.Success;
        }

        /// <summary>
        /// Bans a player from the server, kicking them from the game and preventing them from rejoining.
        /// </summary>
        /// <param name="name">The name of the player to ban.</param>
        /// <param name="reason">The reason the player is being banned.  This may be left null.</param>
        public override void BanPlayer(string name, string reason)
        {
            lock (Locker)
            {
                if (State == ServerState.Disabled)
                {
                    List<BanTag> tags = JsonConvert.DeserializeObject<BanTag[]>(File.ReadAllText(ServerPreferences.GetServerFolderLocation() + "/banned-players.json")).ToList();
                    if (ServerPreferences.OnlineMode)
                    {
                        tags.Add(new BanTag { name = name, uuid = MojangInteropUtility.GetUUIDFromUsername(name), source = "Server", reason = string.IsNullOrEmpty(reason) ? "Banned by an operator." : reason, expires = "forever", created = DateTime.Now.ToString() });
                    }
                    else
                    {
                        tags.Add(new BanTag { name = name, source = "Server", reason = string.IsNullOrEmpty(reason) ? "Banned by an operator." : reason, expires = "forever", created = DateTime.Now.ToString() });
                    }
                    File.WriteAllText(ServerPreferences.GetServerFolderLocation() + "/banned-players.json", JsonConvert.SerializeObject(tags.ToArray()));
                }
                else
                {
                    if (string.IsNullOrEmpty(reason))
                    {
                        SendUncheckedConsoleCommand("ban " + name);
                    }
                    else
                    {
                        SendUncheckedConsoleCommand("ban " + name + " " + reason);
                    }
                }
            }
        }

        /// <summary>
        /// Bans a player by IP address.
        /// </summary>
        /// <param name="ip">The IP address of the player to ban.</param>
        /// <param name="reason">The reason that the IP address is being banned.</param>
        public override void BanIP(string ip, string reason)
        {
            if (State == ServerState.Disabled)
            {
                lock (Locker)
                {
                    List<BanIPTag> tags = JsonConvert.DeserializeObject<BanIPTag[]>(File.ReadAllText(ServerPreferences.GetServerFolderLocation() + "/banned-ips.json")).ToList();
                    tags.Add(new BanIPTag { ip = ip, created = DateTime.Now.ToString(), expires = "forever", reason = string.IsNullOrEmpty(reason) ? "Banned by an operator." : reason, source = "Server" });
                    File.WriteAllText(ServerPreferences.GetServerFolderLocation() + "/banned-ips.json", JsonConvert.SerializeObject(tags.ToArray()));
                }
            }
            else
            {
                if (string.IsNullOrEmpty(reason))
                {
                    SendUncheckedConsoleCommand("ban-ip " + ip);
                }
                else
                {
                    SendUncheckedConsoleCommand("ban-ip " + ip + " " + reason);
                }
            }
        }

        /// <summary>
        /// Pardons a player, allowing them to rejoin the server after a ban.
        /// </summary>
        /// <param name="name">The name of the player to unban.</param>
        public override void UnbanPlayer(string name)
        {
            if (State == ServerState.Disabled)
            {
                lock (Locker)
                {
                    List<BanTag> tags = JsonConvert.DeserializeObject<BanTag[]>(File.ReadAllText(ServerPreferences.GetServerFolderLocation() + "/banned-players.json")).ToList();
                    int position = tags.FindIndex(x => x.name == name);
                    if (position >= 0)
                    {
                        tags.RemoveAt(position);
                    }
                    File.WriteAllText(ServerPreferences.GetServerFolderLocation() + "/banned-players.json", JsonConvert.SerializeObject(tags.ToArray()));
                }
            }
            else
            {
                SendUncheckedConsoleCommand("pardon " + name);
            }
        }

        /// <summary>
        /// Pardons an IP, allowing players with that IP to rejoin the server after an IP ban.
        /// </summary>
        /// <param name="ip">The IP address to unban.</param>
        public override void UnbanIP(string ip)
        {
            if (State == ServerState.Disabled)
            {
                lock (Locker)
                {
                    List<BanIPTag> tags = JsonConvert.DeserializeObject<BanIPTag[]>(File.ReadAllText(ServerPreferences.GetServerFolderLocation() + "/banned-ips.json")).ToList();
                    int position = tags.FindIndex(x => x.ip == ip);
                    if (position >= 0)
                    {
                        tags.RemoveAt(position);
                    }
                    File.WriteAllText(ServerPreferences.GetServerFolderLocation() + "/banned-ips.json", JsonConvert.SerializeObject(tags.ToArray()));
                }
            }
            else
            {
                SendUncheckedConsoleCommand("pardon-ip " + ip);
            }
        }

        /// <summary>
        /// Revokes a player's server operator status.
        /// </summary>
        /// <param name="name">The name of the player to demote.</param>
        public override void DeopPlayer(string name)
        {
            if (State == ServerState.Disabled)
            {
                lock (Locker)
                {
                    List<OpTag> tags = JsonConvert.DeserializeObject<OpTag[]>(File.ReadAllText(ServerPreferences.GetServerFolderLocation() + "/ops.json")).ToList();
                    int position = tags.FindIndex(x => x.name == name);
                    if (position >= 0)
                    {
                        tags.RemoveAt(position);
                    }
                    File.WriteAllText(ServerPreferences.GetServerFolderLocation() + "/ops.json", JsonConvert.SerializeObject(tags.ToArray()));
                }
            }
            else
            {
                SendUncheckedConsoleCommand("deop " + name);
            }
        }

        /// <summary>
        /// Adds a player to the server whitelist, allowing them to join the game while the whitelist is enabled.
        /// </summary>
        /// <param name="name">The name of the player to whitelist.</param>
        public override void WhitelistPlayer(string name)
        {
            if (State == ServerState.Disabled)
            {
                lock (Locker)
                {
                    List<WhitelistTag> tags = JsonConvert.DeserializeObject<WhitelistTag[]>(File.ReadAllText(ServerPreferences.GetServerFolderLocation() + "/whitelist.json")).ToList();
                    if (ServerPreferences.OnlineMode)
                    {
                        tags.Add(new WhitelistTag { name = name, uuid = MojangInteropUtility.GetUUIDFromUsername(name) });
                    }
                    else
                    {
                        tags.Add(new WhitelistTag { name = name });
                    }
                    File.WriteAllText(ServerPreferences.GetServerFolderLocation() + "/whitelist.json", JsonConvert.SerializeObject(tags.ToArray()));
                }
            }
            else
            {
                SendUncheckedConsoleCommand("whitelist add " + name);
            }
        }

        /// <summary>
        /// Removes a player from the server whitelist, preventing them from joining the game while the whitelist is enabled.  If <see cref="JavaServerConfiguration.EnforceWhitelist"/> is true and this player is currently online, they will be kicked from the game.
        /// </summary>
        /// <param name="name">The name of the player to remove from the whitelist.</param>
        public override void RemoveWhitelistPlayer(string name)
        {
            if (State == ServerState.Disabled)
            {
                lock (Locker)
                {
                    List<WhitelistTag> tags = JsonConvert.DeserializeObject<WhitelistTag[]>(File.ReadAllText(ServerPreferences.GetServerFolderLocation() + "/whitelist.json")).ToList();
                    int position = tags.FindIndex(x => x.name == name);
                    if (position >= 0)
                    {
                        tags.RemoveAt(position);
                    }
                    File.WriteAllText(ServerPreferences.GetServerFolderLocation() + "/whitelist.json", JsonConvert.SerializeObject(tags.ToArray()));
                }
            }
            else
            {
                SendUncheckedConsoleCommand("whitelist remove " + name);
            }
        }

        /// <summary>
        /// Retrieves a list of all players who have every logged onto the server.
        /// </summary>
        /// <returns>An enumerable of players.</returns>
        public override IEnumerable<MinecraftPlayer> GetAllPlayers()
        {
            lock (Locker)
            {
                return from x in AllUsers.Values where x.LastConnectionEvent != default select x;
            }
        }

        /// <summary>
        /// Retrieves a list of players on the server who have operator status.
        /// </summary>
        /// <returns>An enumerable of players.</returns>
        public override IEnumerable<MinecraftPlayer> GetAllOps()
        {
            lock (Locker)
            {
                return from x in AllUsers.Values where x.PermissionLevel > 0 select x;
            }
        }

        /// <summary>
        /// Retrieves a list of ban information about players who have been banned from the server.
        /// </summary>
        /// <returns>An enumerable of information about bans.</returns>
        public override List<BanInformation> GetAllBans()
        {
            lock (Locker)
            {
                return BanList;
            }
        }
        
        /// <summary>
        /// Retrieves a list of information about IPs that have been banned from the server.
        /// </summary>
        /// <returns>An enumerable of information about banned IPs.</returns>
        public override List<BanIPTag> GetAllBannedIPs()
        {
            lock (Locker)
            {
                return BanIPList.Values.ToList();
            }
        }

        /// <summary>
        /// Causes the internal Minecraft server to reload the whitelist from disk.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// Minecraft: Java Edition servers do not support this operation.
        /// </exception>
        public override void ReloadServerWhitelist()
        {
            throw new NotSupportedException("Java edition servers cannot reload player whitelist.");
        }

        /// <summary>
        /// Causes the internal Minecraft server to reload the server permissions file from disk.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// Minecraft: Java Edition servers do not support this operation.
        /// </exception>
        public override void ReloadServerPermissions()
        {
            throw new NotSupportedException("Java edition servers cannot reload player permissions.");
        }

        /// <summary>
        /// Retrieves a list of logfiles that this server has produced.
        /// </summary>
        /// <returns>A list with information about each logfile created by the Minecraft server.</returns>
        public override IEnumerable<LogFileInformation> GetAllLogFiles()
        {
            List<LogFileInformation> files = new List<LogFileInformation>();
            foreach(string file in Directory.EnumerateFiles(ServerPreferences.GetServerFolderLocation() + "/logs", "*.log", SearchOption.TopDirectoryOnly)) {
                if(new FileInfo(file).Name == "latest.log")
                {
                    continue;
                }
                files.Add(new LogFileInformation { Name = Path.GetFileNameWithoutExtension(file), CreationDate = File.GetCreationTime(file), CleanExit = true });
            }
            foreach (string file in Directory.EnumerateFiles(ServerPreferences.GetServerFolderLocation() + "/logs", "*.loge", SearchOption.TopDirectoryOnly))
            {
                files.Add(new LogFileInformation { Name = Path.GetFileNameWithoutExtension(file), CreationDate = File.GetCreationTime(file), CleanExit = false });
            }
            files = files.OrderBy(info => info.Name).ToList();
            return files;
        }

        /// <summary>
        /// Gets the text inside a specific logfile.
        /// </summary>
        /// <param name="info">The logfile to get information about.</param>
        /// <returns>The information that the logfile contains.</returns>
        public override string GetLogFile(LogFileInformation info)
        {
            if(File.Exists(ServerPreferences.GetServerFolderLocation() + "/logs/" + info.Name + ".log"))
            {
                return File.ReadAllText(ServerPreferences.GetServerFolderLocation() + "/logs/" + info.Name + ".log");
            }
            else
            {
                return File.ReadAllText(ServerPreferences.GetServerFolderLocation() + "/logs/" + info.Name + ".loge");
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
                return from x in AllUsers.Values where x.IsWhitelisted select x;
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
                if (ServerPreferences.OnlineMode)
                {
                    foreach (MinecraftPlayer player in AllUsers.Values)
                    {
                        if (player.Username == username)
                        {
                            return player;
                        }
                    }
                    return null;
                }
                else
                {
                    if (AllUsers.ContainsKey(username))
                    {
                        return AllUsers[username];
                    }
                    else
                    {
                        return null;
                    }
                }
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
                File.Delete(ServerPreferences.GetServerFolderLocation() + "/logs/" + info.Name + ".log");
            }
            else
            {
                File.Delete(ServerPreferences.GetServerFolderLocation() + "/logs/" + info.Name + ".loge");
            }
        }

        /// <summary>
        /// Get information about a player by their UUID.
        /// </summary>
        /// <param name="uuid">The UUID of the player to retrieve data about.</param>
        /// <returns>Information about the specified player, or null if no player was found.</returns>
        public override MinecraftPlayer GetPlayerInformationByUUID(string uuid)
        {
            lock (Locker)
            {
                if (ServerPreferences.OnlineMode)
                {
                    if (AllUsers.ContainsKey(uuid))
                    {
                        return AllUsers[uuid];
                    }
                    else
                    {
                        return null;
                    }

                }
                else
                {
                    foreach (MinecraftPlayer player in AllUsers.Values)
                    {
                        if (player.UUID == uuid)
                        {
                            return player;
                        }
                    }
                    return null;
                }
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
    }
}
