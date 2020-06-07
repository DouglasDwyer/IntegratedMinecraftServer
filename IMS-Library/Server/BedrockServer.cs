using Newtonsoft.Json;
using RoyalXML;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace IMS_Library
{
    public class BedrockServer : ServerProxy
    {
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

        public override bool SupportsWhitelist => true;

        public override bool WhitelistEnabled { get => ServerPreferences.Whitelist;
            set {
                //SendUncheckedConsoleCommand("whitelist " + (value ? "on" : "off"));
                ServerPreferences.Whitelist = value;
                RestartAsync();
            }
        }

        public override bool SupportsBanning => false;

        public override bool SupportsBanningIPs => false;

        public override bool SupportsOps => true;

        public override bool SupportsIPs => false;

        public override bool SupportsKicking => true;

        public override MinecraftEdition SupportedEdition => MinecraftEdition.Bedrock;

        protected string ServerLocation => ServerPreferences.GetServerFolderLocation();
        protected string LogFolderLocation => ServerLocation + "/log/";
        protected string WorldLocation => ServerLocation + "/worlds/" + ServerPreferences.LevelName;

        protected object Locker = new object();

        protected Dictionary<string, MinecraftPlayer> AllUsers = new Dictionary<string, MinecraftPlayer>();
        protected Dictionary<string, MinecraftPlayer> OnlineUsers = new Dictionary<string, MinecraftPlayer>();

        protected Process ServerProcess;
        protected BedrockServerConfiguration ServerPreferences;

        protected FileSystemWatcher WhitelistWatcher, PermissionsWatcher;
        protected StreamWriter LogWriter;
        protected List<string> ConsoleText = new List<string>();

        protected BackupState CurrentBackupState = BackupState.None;

        public BedrockServer(Guid id, BedrockServerConfiguration configuration) : base(id)
        {
            ServerPreferences = configuration;
        }

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

        public override void BanIP(string ip, string reason)
        {
            throw new NotImplementedException();
        }

        public override void BanPlayer(string name, string reason)
        {
            throw new NotImplementedException();
        }

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

        public override List<BanIPTag> GetAllBannedIPs()
        {
            throw new NotImplementedException();
        }

        public override List<BanInformation> GetAllBans()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<MinecraftPlayer> GetAllOps()
        {
            lock(Locker)
            {
                return from player in AllUsers.Values where player.PermissionLevel > 0 select player;
            }
        }

        public override IEnumerable<MinecraftPlayer> GetAllPlayers()
        {
            lock(Locker)
            {
                return AllUsers.Values.Where(x => x.LastConnectionEvent != default);
            }
        }

        public override IEnumerable<MinecraftPlayer> GetAllWhitelistedPlayers()
        {
            lock (Locker)
            {
                return from player in AllUsers.Values where player.IsWhitelisted select player;
            }
        }

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

        public override string GetLogFile(string name)
        {
            throw new NotImplementedException();
        }

        public override List<LogFileInformation> GetLogFiles()
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<MinecraftPlayer> GetOnlinePlayers()
        {
            lock (Locker)
            {
                return OnlineUsers.Values.ToArray();
            }
        }

        public override MinecraftPlayer GetPlayerInformationByUsername(string username)
        {
            lock (Locker)
            {
                return AllUsers.ContainsKey(username) ? AllUsers[username] : null;
            }
        }

        public override MinecraftPlayer GetPlayerInformationByUUID(string uuid)
        {
            lock(Locker)
            {
                return AllUsers.Values.Where(x => x.UUID == uuid).FirstOrDefault();
            }
        }

        public override void KickPlayer(string name, string reason)
        {
            SendUncheckedConsoleCommand("kick " + name + " " + reason);
        }

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

        public override void ReloadServerPermissions()
        {
            SendUncheckedConsoleCommand("permission reload");
        }

        public override void ReloadServerWhitelist()
        {
            SendUncheckedConsoleCommand("whitelist reload");
        }

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

        public override async Task RestartAsync()
        {
            await StopAsync();
            await StartAsync();
        }

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

        public override async Task StartAsync()
        {
            lock (Locker)
            {
                if (State != ServerState.Disabled)
                {
                    throw new InvalidOperationException("Cannot start a server that's already running!");
                }
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

                    JunctionPoint.CreateHardLink(Constants.ExecutionPath + Constants.ServerBinariesFolderLocation + "/Bedrock.exe", ServerLocation + "/Bedrock.exe");

                    ServerProcess.StartInfo.FileName = ServerLocation + "/Bedrock.exe";

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
                    ServerProcess.Exited += (x, y) => OnServerProcessDie(local);

                    IMS.Instance.FirewallManager.CreateFirewallExecutableException("Server" + ID, ServerProcess.StartInfo.FileName);

                    State = ServerState.Starting;
                    ServerProcess.Start();

                    ServerProcess.BeginOutputReadLine();
                    ServerProcess.BeginErrorReadLine();
                    ChildProcessTracker.AddProcess(ServerProcess);
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

        public override async Task StopAsync()
        {
            lock (Locker)
            {
                if (State == ServerState.Disabled || State == ServerState.Stopping) { return; }
                State = ServerState.Stopping;
            }

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
            IMS.Instance.FirewallManager.RemoveFirewallExecutableException("Server" + ID);
            while(!ServerProcess.HasExited) { await Task.Delay(1); }

            LogWriter.Close();
            File.Copy(LogFolderLocation + "/latest.log", LogFolderLocation + "/" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".log");
            File.Delete(LogFolderLocation + "/latest.log");

            State = ServerState.Disabled;
            ServerProcess = null;
        }

        public override void UnbanIP(string ip)
        {
            throw new NotImplementedException();
        }

        public override void UnbanPlayer(string name)
        {
            throw new NotImplementedException();
        }

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

        protected void ReloadWhitelistJSON()
        {
            lock (Locker) {
                try
                {
                    FileInfo file = new FileInfo(ServerLocation + "/whitelist.json");
                    while(file.IsFileLocked()) { Thread.Sleep(1); }
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

        protected void OnServerConsoleDataReceived(object sender, DataReceivedEventArgs args)
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

        protected bool MatchRegex(string data, string pattern, out Match match)
        {
            match = Regex.Match(data, pattern);
            return match.Success;
        }

        protected void OnServerProcessDie(Process process)
        {
            lock (Locker)
            {
                if (process.EnableRaisingEvents && process == ServerProcess)
                {
                    OnlineUsers.Clear();
                    if (State != ServerState.Stopping)
                    {
                        if (State == ServerState.Running)
                        {
                            Logger.WriteError("Server " + ID + " crashed.  Attempting restart...");
                            State = ServerState.Disabled;
                            StartAsync();
                        }
                        else
                        {
                            Logger.WriteError("Server " + ID + " crashed on startup.");
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
        }

        protected void SendUncheckedConsoleCommand(string command)
        {
            lock(Locker)
            {
                ServerProcess?.StandardInput.WriteLine(command);
            }
        }

        protected enum BackupState { None, Saving, Copying }
    }
}
