using KinglyStudios.Knetworking;
using Newtonsoft.Json;
using RoyalXML;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace IMS_Library
{
    public class JavaServer : ServerProxy
    {
        public JavaServerConfiguration ServerPreferences;
        public override ServerConfiguration CurrentConfiguration { get => ServerPreferences; set {
                if (State != ServerState.Disabled)
                {
                    throw new InvalidOperationException("Cannot change server settings while the server is running");
                }
                else
                {
                    ServerPreferences = (JavaServerConfiguration)value;
                }
            }
        }

        public override bool SupportsWhitelist => true;

        public override bool WhitelistEnabled { get => ServerPreferences.UseWhitelist; set => SendUncheckedConsoleCommand("whitelist " + (value ? "on" : "off")); }

        public override bool SupportsBanning => true;

        public override bool SupportsBanningIPs => true;

        public override bool SupportsOps => true;

        public override bool SupportsIPs => true;

        public override bool SupportsKicking => true;

        private Process ServerProcess;

        //Stores UUIDs if online or names if offline
        protected List<OpTag> OpList = new List<OpTag>();
        protected List<BanInformation> BanList = new List<BanInformation>();
        protected Dictionary<string, BanIPTag> BanIPList = new Dictionary<string, BanIPTag>();
        //Stores UUIDs if online or names if offline
        protected List<WhitelistTag> Whitelist = new List<WhitelistTag>();

        protected Dictionary<string, MinecraftPlayer> OnlineUsers = new Dictionary<string, MinecraftPlayer>();
        protected Dictionary<string, MinecraftPlayer> AllUsers = new Dictionary<string, MinecraftPlayer>();
        protected MemoryCache UUIDCache;

        protected List<string> ConsoleText = new List<string>();

        protected bool AutomaticSavingPlayerEnabled = true;
        protected bool HasCompletedAutomaticSave = true;
        protected string WorldLocation { get => ServerPreferences.GetServerFolderLocation() + "/" + ServerPreferences.LevelName; }

        public JavaServer(Guid id, JavaServerConfiguration configuration) : base(id)
        {
            ServerPreferences = configuration;
            UUIDCache = new MemoryCache("IMS", null, true);
            State = ServerState.Disabled;
        }

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

        public override IEnumerable<MinecraftPlayer> GetOnlinePlayers()
        {
            return OnlineUsers.Values;
        }

        public override string GetConsoleText()
        {
            string toReturn = string.Concat(ConsoleText.ToArray());
            if (toReturn.EndsWith("\n"))
            {
                toReturn = toReturn.Remove(toReturn.Length - 1);
            }
            return toReturn;
        }

        public void OnServerProcessDie()
        {
            if(ServerProcess != null && !ServerProcess.EnableRaisingEvents)
            {
                return;
            }
            if(State != ServerState.Stopping)
            {
                if (State == ServerState.Running)
                {
                    Logger.WriteError("Server " + ID + " crashed.  Attempting restart...");
                    State = ServerState.Disabled;
                    Start();
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

        public override void OpPlayer(string name)
        {
            if(State == ServerState.Disabled)
            {
                List<OpTag> tags = JsonConvert.DeserializeObject<OpTag[]>(File.ReadAllText(ServerPreferences.GetServerFolderLocation() + "/ops.json")).ToList();
                if(ServerPreferences.OnlineMode)
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

        public override async Task BackupToLocationAsync(string location)
        {
            if(!HasCompletedAutomaticSave)
            {
                return;
            }
            HasCompletedAutomaticSave = false;
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
                IMS.AsThreadSafe(() => {
                    if (AutomaticSavingPlayerEnabled)
                    {
                        SendUncheckedConsoleCommand("save-on");
                    }
                    location = null;
                });
            });
        }

        public override void Restart()
        {
            Stop();
            Start();
        }

        public override void SendConsoleCommand(string command)
        {
            if (command == "stop")
            {
                ServerPreferences.IsEnabled = false;
                Stop();
            }
            else
            {
                SendUncheckedConsoleCommand(command);
            }
        }

        protected void SendUncheckedConsoleCommand(string command)
        {
            ServerProcess.StandardInput.WriteLine(command);
        }

        protected void ReloadWhitelistJSON()
        {
            Whitelist.Clear();
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
                    if(player is null)
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

        protected void AddPlayerToAllPlayersList(MinecraftPlayer player)
        {
            if(ServerPreferences.OnlineMode)
            {
                AllUsers[player.UUID] = player;
            }
            else
            {
                AllUsers[player.Username] = player;
            }
        }

        protected void ReloadOpJSON()
        {
            OpList.Clear();
            foreach(MinecraftPlayer player in AllUsers.Values)
            {
                player.PermissionLevel = 0;
            }
            if (File.Exists(ServerPreferences.GetServerFolderLocation() + "/ops.json"))
            {
                OpTag[] tags = JsonConvert.DeserializeObject<OpTag[]>(File.ReadAllText(ServerPreferences.GetServerFolderLocation() + "/ops.json"));
                foreach(OpTag tag in tags)
                {
                    MinecraftPlayer player = GetPlayerByIdentifier(tag.name, tag.uuid);
                    if(player is null)
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

        protected MinecraftPlayer GetPlayerByIdentifier(string name, string uuid)
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

        protected MinecraftPlayer GetPlayerOrDefaultByIdentifier(string name, string uuid)
        {
            MinecraftPlayer toReturn = GetPlayerByIdentifier(name, uuid);
            if(toReturn is null)
            {
                toReturn = new MinecraftPlayer();
                toReturn.Username = name;
                toReturn.UUID = uuid;
                if(ServerPreferences.OnlineMode)
                {
                    toReturn.IsWhitelisted = Whitelist.FindIndex(x => x.uuid == uuid) >= 0;
                }
                else
                {
                    toReturn.IsWhitelisted = Whitelist.FindIndex(x => x.uuid == uuid) >= 0;
                }
                OpTag? op = null;
                if (ServerPreferences.OnlineMode)
                {
                    op = OpList.Find(x => x.uuid == uuid);
                }
                else
                {
                    op = OpList.Find(x => x.name == name);
                }
                if (op != null)
                {
                    toReturn.PermissionLevel = op.Value.level;
                }
            }
            return toReturn;
        }

        protected void ReloadBanJSON()
        {
            BanList.Clear();
            if (File.Exists(ServerPreferences.GetServerFolderLocation() + "/ops.json"))
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
                    if(information.Player is null)
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

        protected void ReloadBanIPJSON()
        {
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

        public int GetOperatorStatusOfPlayerByUUID(string uuid)
        {
            return AllUsers[uuid].PermissionLevel;
        }

        public override void Start()
        {
            if (State != ServerState.Disabled)
            {
                throw new Exception("Cannot stop a server that isn't running!");
            }
            World world = IMS.Instance.WorldManager.GetWorldByID(CurrentConfiguration.WorldID);
            if (world is null)
            {
                world = new World(Guid.NewGuid());
                world.Name = CurrentConfiguration.ServerName + " World";
                world.WorldEdition = World.WorldType.Java;
                ServerPreferences.WorldID = world.ID;
                IMS.Instance.WorldManager.AddWorldToRegistry(world);
            }
            JunctionPoint.Create(WorldLocation, world.WorldPath, true);
            if (File.Exists(ServerPreferences.GetServerFolderLocation() + "/logs/latest.log"))
            {
                File.Copy(ServerPreferences.GetServerFolderLocation() + "/logs/latest.log", ServerPreferences.GetServerFolderLocation() + "/logs/" + File.GetCreationTime(ServerPreferences.GetServerFolderLocation() + "/logs/latest.log").ToString("yyyy-dd-M--HH-mm-ss") + ".loge", true);
                File.Delete(ServerPreferences.GetServerFolderLocation() + "/logs/latest.log");
            }
            if (File.Exists(ServerPreferences.GetServerFolderLocation() + "/usercache.xml"))
            {
                foreach (MinecraftPlayer player in RoyalSerializer.XMLToObject<MinecraftPlayer[]>(File.ReadAllText(ServerPreferences.GetServerFolderLocation() + "/usercache.xml")))
                {
                    if(ServerPreferences.OnlineMode) {
                        AllUsers[player.UUID] = player;
                    }
                    else
                    {
                        AllUsers[player.Username] = player;
                    }                    
                }
            }
            if (!VerifyJavaVersion())
            {
                Logger.WriteError("This computer does not have the proper Java runtime installed.  Please install the latest version of Java in order to run Minecraft");
                //todo: use packaged java version to ensure compatability
                ServerPreferences.IsEnabled = false;
                return;
            }

            GetLogFiles();

            MinecraftConfigurationWriter.WriteServerPropertiesFile("server.properties", ServerPreferences);
            MinecraftConfigurationWriter.WriteEULA("eula.txt", ServerPreferences);

            ReloadWhitelistJSON();
            ReloadOpJSON();
            ReloadBanJSON();
            ReloadBanIPJSON();

            ServerProcess = new Process();
            ServerProcess.StartInfo = new ProcessStartInfo();
            if (ServerPreferences.UseJITCompiler)
            {
                ServerProcess.StartInfo.FileName = Constants.ExecutionPath + Constants.GraalVMJavaFilePath;
            }
            else
            {
                ServerProcess.StartInfo.FileName = "java.exe";
            }
            ServerProcess.StartInfo.Arguments = "-Xms" + ServerPreferences.MinimumMemoryMB + "M -Xmx" + ServerPreferences.MaximumMemoryMB + "M " + ServerPreferences.JavaArguments + " -jar \"" + (ServerPreferences.GetServerFolderLocation() + "/server.jar").Replace("/", "\\") + "\" nogui";
            ServerProcess.StartInfo.WorkingDirectory = ServerPreferences.GetServerFolderLocation();
            ServerProcess.StartInfo.LoadUserProfile = false;
            ServerProcess.StartInfo.UseShellExecute = false;
            ServerProcess.StartInfo.CreateNoWindow = true;

            ServerProcess.StartInfo.RedirectStandardInput = true;
            ServerProcess.StartInfo.RedirectStandardOutput = true;
            ServerProcess.StartInfo.RedirectStandardError = true;

            ServerProcess.OutputDataReceived += IMS.AsThreadSafeDataEvent(OnServerConsoleDataReceived);
            ServerProcess.ErrorDataReceived += IMS.AsThreadSafeDataEvent(OnServerConsoleDataReceived);

            ServerProcess.EnableRaisingEvents = true;
            ServerProcess.Exited += IMS.AsThreadSafeEvent(OnServerProcessDie);

            ServerProcess.Start();

            State = ServerState.Starting;

            ServerProcess.BeginOutputReadLine();
            ServerProcess.BeginErrorReadLine();
            ChildProcessTracker.AddProcess(ServerProcess);
        }

        protected void OnServerConsoleDataReceived(object sender, DataReceivedEventArgs args)
        {
            if(args.Data is null)
            {
                return;
            }

            ConsoleText.Add(args.Data + "\n");
            while(ConsoleText.Count > 100)
            {
                ConsoleText.RemoveAt(0);
            }

            string data = StripServerLogOfTimestamp(args.Data);
            if(data.StartsWith("[Server thread/INFO]: "))
            {
                data = data.Substring("Server thread/INFO]: ".Length + 1);
                Match regexMatch = null;
                //Manage server starting
                if (State == ServerState.Starting && data.StartsWith("Done"))
                {
                    State = ServerState.Running;
                }
                //Manage players joining
                else if (CheckRegexMatch(data, "^([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+)\\[\\/([0123456789.)]+):[0123456789]+] logged in with entity id [0123456789]+", out regexMatch))
                {
                    MinecraftPlayer loggingInPlayer = null;
                    string playerName = regexMatch.Groups[1].Value;
                    string uuid = null;
                    try
                    {
                        uuid = (string)UUIDCache[playerName];
                    } catch { }
                    if (ServerPreferences.OnlineMode)
                    {
                        if (AllUsers.ContainsKey(uuid))
                        {
                            loggingInPlayer = AllUsers[uuid];
                        }
                    }
                    else
                    {
                        if(AllUsers.ContainsKey(playerName))
                        {
                            loggingInPlayer = AllUsers[playerName];
                        }
                    }
                    if(loggingInPlayer is null)
                    {
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
                else if (CheckRegexMatch(data, "^([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+) left the game$", out regexMatch))
                {
                    MinecraftPlayer leavingPlayer = GetPlayerInformationByUsername(regexMatch.Groups[1].Value);
                    leavingPlayer.LastConnectionEvent = DateTime.Now;
                    if(ServerPreferences.OnlineMode)
                    {
                        OnlineUsers.Remove(leavingPlayer.UUID);
                    }
                    else
                    {
                        OnlineUsers.Remove(leavingPlayer.Username);
                    }                    
                }
                //Manage players using /op
                else if (CheckRegexMatch(data, "^\\[[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+: Made ([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+) a server operator]$", out regexMatch))
                {
                    ReloadOpJSON();
                }
                //Manage players using /ban
                else if(CheckRegexMatch(data, "^\\[([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+): Banned ([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+): (.*)(?=\\])]$", out regexMatch))
                {
                    ReloadBanJSON();
                }
                //Manage players using /ban-ip
                else if (CheckRegexMatch(data, "^\\[([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+): Banned IP ([0123456789.]+): (.*)(?=\\])]$", out regexMatch))
                {
                    ReloadBanIPJSON();
                }
                //Manage players using /pardon
                else if (CheckRegexMatch(data, "^\\[([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+): Unbanned ([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+)]$", out regexMatch))
                {
                    ReloadBanJSON();
                }
                //Manage players using /pardon-ip
                else if (CheckRegexMatch(data, "^\\[([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+): Unbanned IP ([0123456789.]+)]$", out regexMatch))
                {
                    ReloadBanIPJSON();
                }
                //Manage players using /deop
                else if (CheckRegexMatch(data, "^\\[[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+: Made ([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+) no longer a server operator]$", out regexMatch))
                {
                    ReloadOpJSON();
                }
                //Manage players using /whitelist reload
                else if (CheckRegexMatch(data, "^\\[[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+: Reloaded the whitelist]$", out regexMatch))
                {
                    ReloadWhitelistJSON();
                }
                //Manage players using /whitelist add
                else if(CheckRegexMatch(data, "^\\[[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+: Added ([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+) to the whitelist]$", out regexMatch))
                {
                    ReloadWhitelistJSON();
                }
                //Manage players using /whitelist remove
                else if (CheckRegexMatch(data, "^\\[[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+: Removed ([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+) from the whitelist]$", out regexMatch))
                {
                    ReloadWhitelistJSON();
                }
                //Manage players using /whitelist on
                else if (CheckRegexMatch(data, "^\\[[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+: Whitelist is now turned on]$", out regexMatch))
                {
                    ServerPreferences.UseWhitelist = true;
                }
                //Manage players using /whitelist off
                else if (CheckRegexMatch(data, "^\\[[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+: Whitelist is now turned off]$", out regexMatch))
                {
                    ServerPreferences.UseWhitelist = false;
                }
                //Manage SERVER
                //Manage server using /op
                else if (CheckRegexMatch(data, "^Made ([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+) a server operator$", out regexMatch))
                {
                    ReloadOpJSON();
                }
                //Manage server using /ban
                else if (CheckRegexMatch(data, "^Banned ([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+): (.*)$", out regexMatch))
                {
                    ReloadBanJSON();
                }
                //Manage server using /ban-ip
                else if (CheckRegexMatch(data, "^Banned IP ([0123456789.]+): (.*)$", out regexMatch))
                {
                    ReloadBanIPJSON();
                }
                //Manage server using /pardon
                else if (CheckRegexMatch(data, "^Unbanned ([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+)$", out regexMatch))
                {
                    ReloadBanJSON();
                }
                //Manage server using /pardon-ip
                else if (CheckRegexMatch(data, "^Unbanned IP ([0123456789.]+)$", out regexMatch))
                {
                    ReloadBanIPJSON();
                }
                //Manage server using /deop
                else if (CheckRegexMatch(data, "^Made ([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+) no longer a server operator$", out regexMatch))
                {
                    ReloadOpJSON();
                }
                //Manage server using /whitelist reload
                else if (CheckRegexMatch(data, "^Reloaded the whitelist$", out regexMatch))
                {
                    ReloadWhitelistJSON();
                }
                //Manage server using /whitelist add
                else if (CheckRegexMatch(data, "^Added ([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+) to the whitelist$", out regexMatch))
                {
                    ReloadWhitelistJSON();
                }
                //Manage server using /whitelist remove
                else if (CheckRegexMatch(data, "^Removed ([abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+) from the whitelist$", out regexMatch))
                {
                    ReloadWhitelistJSON();
                }
                //Manage server using /whitelist on
                else if (CheckRegexMatch(data, "^Whitelist is now turned on$", out regexMatch))
                {
                    ServerPreferences.UseWhitelist = true;
                }
                //Manage server using /whitelist off
                else if (CheckRegexMatch(data, "^[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+: Whitelist is now turned off$", out regexMatch))
                {
                    ServerPreferences.UseWhitelist = false;
                }
                //Manage server using /save on
                else if (CheckRegexMatch(data, "^Automatic saving is now enabled$", out regexMatch))
                {
                    AutomaticSavingPlayerEnabled = true;
                }
                //Manage server using /save off
                else if (CheckRegexMatch(data, "^Automatic saving is now disabled$", out regexMatch))
                {
                    if(HasCompletedAutomaticSave)
                    {
                        AutomaticSavingPlayerEnabled = false;
                    }
                }
                else if(data == "Saved the game")
                {
                    HasCompletedAutomaticSave = true;
                }
                //end
                //Manage players stopping the server
                else if (CheckRegexMatch(data, "^\\[[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+: Stopping the server]$", out regexMatch))
                {
                    OnlineUsers.Clear();
                    ServerPreferences.IsEnabled = false;
                    State = ServerState.Stopping;
                }
                //Manage players using /save on
                else if (CheckRegexMatch(data, "^\\[[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+: Automatic saving is now enabled]$", out regexMatch))
                {
                    AutomaticSavingPlayerEnabled = true;
                }
                //Manage players using /save off
                else if (CheckRegexMatch(data, "^\\[[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_]+: Automatic saving is now disabled]$", out regexMatch))
                {
                    AutomaticSavingPlayerEnabled = false;
                }
            }
            else if (data.StartsWith("[User Authenticator #"))
            {
                UUIDCache.Add(data.Substring(data.IndexOf("UUID of player ") + "UUID of player ".Length, data.IndexOf(" is ") - (data.IndexOf("UUID of player ") + "UUID of player ".Length)), data.Substring(data.IndexOf(" is ") + 4), new CacheItemPolicy() { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(20) });
            }
        }

        private string StripServerLogOfTimestamp(string logLine)
        {
            return logLine.Substring(11);
        }

        public override void Stop()
        {
            if(State == ServerState.Disabled) { return; }
            State = ServerState.Stopping;
            ServerProcess.EnableRaisingEvents = false;
            SendUncheckedConsoleCommand("stop");
            OnlineUsers.Clear();
            File.WriteAllText(ServerPreferences.GetServerFolderLocation() + "/usercache.xml", RoyalSerializer.ObjectToXML(AllUsers.Values.ToArray()));
            while(!ServerProcess.HasExited) { Thread.Sleep(5); }
            if(File.Exists(ServerPreferences.GetServerFolderLocation() + "/logs/latest.log"))
            {
                File.Copy(ServerPreferences.GetServerFolderLocation() + "/logs/latest.log", ServerPreferences.GetServerFolderLocation() + "/logs/" + File.GetCreationTime(ServerPreferences.GetServerFolderLocation() + "/logs/latest.log").ToString("yyyy-dd-M--HH-mm-ss") + ".log");
                File.Delete(ServerPreferences.GetServerFolderLocation() + "/logs/latest.log");
            }
            State = ServerState.Disabled;
        }

        private bool CheckRegexMatch(string input, string pattern, out Match match)
        {
            Regex regex = new Regex(pattern);
            match = regex.Match(input);
            return match.Success;
        }

        private bool VerifyJavaVersion()
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "java.exe";
                psi.Arguments = " -version";
                psi.RedirectStandardError = true;
                psi.UseShellExecute = false;
                psi.CreateNoWindow = true;

                Process pr = Process.Start(psi);
                string strOutput = pr.StandardError.ReadLine().Split(' ')[2].Replace("\"", "");

                return strOutput.StartsWith("1.8.");
            }
            catch
            {
                return false;
            }
        }

        public override void BanPlayer(string name, string reason)
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

        public override void BanIP(string ip, string reason)
        {
            if (State == ServerState.Disabled)
            {
                List<BanIPTag> tags = JsonConvert.DeserializeObject<BanIPTag[]>(File.ReadAllText(ServerPreferences.GetServerFolderLocation() + "/banned-ips.json")).ToList();
                tags.Add(new BanIPTag { ip = ip, created = DateTime.Now.ToString(), expires = "forever", reason = string.IsNullOrEmpty(reason) ? "Banned by an operator." : reason, source = "Server" });
                File.WriteAllText(ServerPreferences.GetServerFolderLocation() + "/banned-ips.json", JsonConvert.SerializeObject(tags.ToArray()));
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

        public override void UnbanPlayer(string name)
        {
            if (State == ServerState.Disabled)
            {
                List<BanTag> tags = JsonConvert.DeserializeObject<BanTag[]>(File.ReadAllText(ServerPreferences.GetServerFolderLocation() + "/banned-players.json")).ToList();
                int position = tags.FindIndex(x => x.name == name);
                if (position >= 0)
                {
                    tags.RemoveAt(position);
                }
                File.WriteAllText(ServerPreferences.GetServerFolderLocation() + "/banned-players.json", JsonConvert.SerializeObject(tags.ToArray()));
            }
            else
            {
                SendUncheckedConsoleCommand("pardon " + name);
            }
        }

        public override void UnbanIP(string ip)
        {
            if (State == ServerState.Disabled)
            {
                List<BanIPTag> tags = JsonConvert.DeserializeObject<BanIPTag[]>(File.ReadAllText(ServerPreferences.GetServerFolderLocation() + "/banned-ips.json")).ToList();
                int position = tags.FindIndex(x => x.ip == ip);
                if (position >= 0)
                {
                    tags.RemoveAt(position);
                }
                File.WriteAllText(ServerPreferences.GetServerFolderLocation() + "/banned-ips.json", JsonConvert.SerializeObject(tags.ToArray()));
            }
            else
            {
                SendUncheckedConsoleCommand("pardon-ip " + ip);
            }
        }

        public override void DeopPlayer(string name)
        {
            if (State == ServerState.Disabled)
            {
                List<OpTag> tags = JsonConvert.DeserializeObject<OpTag[]>(File.ReadAllText(ServerPreferences.GetServerFolderLocation() + "/ops.json")).ToList();
                int position = tags.FindIndex(x => x.name == name);
                if(position >= 0)
                {
                    tags.RemoveAt(position);
                }
                File.WriteAllText(ServerPreferences.GetServerFolderLocation() + "/ops.json", JsonConvert.SerializeObject(tags.ToArray()));
            }
            else
            {
                SendUncheckedConsoleCommand("deop " + name);
            }
        }

        public override void WhitelistPlayer(string name)
        {
            if (State == ServerState.Disabled)
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
            else
            {
                SendUncheckedConsoleCommand("whitelist add " + name);
            }
        }

        public override void RemoveWhitelistPlayer(string name)
        {
            if (State == ServerState.Disabled)
            {
                List<WhitelistTag> tags = JsonConvert.DeserializeObject<WhitelistTag[]>(File.ReadAllText(ServerPreferences.GetServerFolderLocation() + "/whitelist.json")).ToList();
                int position = tags.FindIndex(x => x.name == name);
                if (position >= 0)
                {
                    tags.RemoveAt(position);
                }
                File.WriteAllText(ServerPreferences.GetServerFolderLocation() + "/whitelist.json", JsonConvert.SerializeObject(tags.ToArray()));
            }
            else
            {
                SendUncheckedConsoleCommand("whitelist remove " + name);
            }
        }

        public override IEnumerable<MinecraftPlayer> GetAllPlayers()
        {
            return from x in AllUsers.Values where x.LastConnectionEvent != default select x;
        }

        public override IEnumerable<MinecraftPlayer> GetAllOps()
        {
            return from x in AllUsers.Values where x.PermissionLevel > 0 select x;
        }

        public override List<BanInformation> GetAllBans()
        {
            return BanList;
        }

        public override List<BanIPTag> GetAllBannedIPs()
        {
            return BanIPList.Values.ToList();
        }

        public override void ReloadServerPermissions()
        {
            throw new NotSupportedException("Java edition servers cannot reload player permissions.");
        }

        public override List<LogFileInformation> GetLogFiles()
        {
            List<LogFileInformation> files = new List<LogFileInformation>();
            foreach(string file in Directory.EnumerateFiles(ServerPreferences.GetServerFolderLocation() + "/logs", "*.log", SearchOption.TopDirectoryOnly)) {
                files.Add(new LogFileInformation { Name = Path.GetFileNameWithoutExtension(file), CleanExit = true });
            }
            foreach (string file in Directory.EnumerateFiles(ServerPreferences.GetServerFolderLocation() + "/logs", "*.loge", SearchOption.TopDirectoryOnly))
            {
                files.Add(new LogFileInformation { Name = Path.GetFileNameWithoutExtension(file), CleanExit = false });
            }
            files = files.OrderBy(info => info.Name).ToList();
            return files;
        }

        public override string GetLogFile(string name)
        {
            if(File.Exists(ServerPreferences.GetServerFolderLocation() + "/logs/" + name + ".log"))
            {
                return File.ReadAllText(ServerPreferences.GetServerFolderLocation() + "/logs/" + name + ".log");
            }
            else
            {
                return File.ReadAllText(ServerPreferences.GetServerFolderLocation() + "/logs/" + name + ".loge");
            }
        }

        public override IEnumerable<MinecraftPlayer> GetAllWhitelistedPlayers()
        {
            return from x in AllUsers.Values where x.IsWhitelisted select x;
        }

        public override MinecraftPlayer GetPlayerInformationByUsername(string username)
        {
            if(ServerPreferences.OnlineMode)
            {
                foreach(MinecraftPlayer player in AllUsers.Values)
                {
                    if(player.Username == username)
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

        public override MinecraftPlayer GetPlayerInformationByUUID(string uuid)
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
}
