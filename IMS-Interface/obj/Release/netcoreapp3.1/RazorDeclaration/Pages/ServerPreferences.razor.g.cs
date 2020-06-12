#pragma checksum "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\ServerPreferences.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0471067e404c254719fa58997a3bcf84d5343fcc"
// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace IMS_Interface
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\_Imports.razor"
using IMS_Interface;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\_Imports.razor"
using IMS_Interface.Data;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\_Imports.razor"
using IMS_Interface.Player;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\_Imports.razor"
using IMS_Interface.World;

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\_Imports.razor"
using IMS_Interface.Preferences;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\ServerPreferences.razor"
using IMS_Library;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/ServerPreferences")]
    public partial class ServerPreferences : Microsoft.AspNetCore.Components.ComponentBase, IDisposable
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 14 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\ServerPreferences.razor"
       
    public PreferenceEditor Editor;
    public List<PreferenceDisplay> PreferenceLayout = new List<PreferenceDisplay>();

    protected string ApplyText { get => ServerSelector.CurrentServer.State == ServerProxy.ServerState.Disabled ? "Apply" : "Apply and Restart"; }

    protected Task UpdateSettingsTask = null;
    protected IMSConfiguration PreferenceConfiguration { get; set; }

    static ServerPreferences()
    {
        Func<List<PreferenceDisplay>> layoutGenerator = () =>
        {
            List<PreferenceDisplay> layout = new List<PreferenceDisplay>();

            layout.Add(new MOTDDisplay("MessageOfTheDay", "Message of the day", "This setting determines what text appears on clients' server selection screen."));
            layout.Add(new MinecraftVersionDisplayView.MinecraftVersionDisplay("ServerVersion", "Server version", "This setting determines what version of Minecraft the server runs."));
            layout.Add(new IntegerDisplay("MinimumMemoryMB", "Minimum memory", "This setting determines the minimum amount of RAM in MB that Java reserves for the server.", 0, int.MaxValue));
            layout.Add(new IntegerDisplay("MaximumMemoryMB", "Maximum memory", "This setting determines the maximum amount of RAM in MB that Java will use for the server.", 0, int.MaxValue));
            layout.Add(new StringDisplay("JavaArguments", "JVM arguments", "This setting determines what command-line arguments are passed to the Java/GraalVM runtime."));

            layout.Add(new BooleanDisplay("AllowFlight", "Enable flight", "This setting determines whether the server kicks survival players for fly-hacking.", "Yes", "No"));
            layout.Add(new BooleanDisplay("AllowNether", "Enable Nether", "This setting determines whether players can enter the Nether.", "Yes", "No"));
            layout.Add(new BooleanDisplay("BroadcastConsoleToOps", "Broadcast console to ops", "This setting determines whether ops are notified of commands executed in the server console.", "Yes", "No"));
            layout.Add(new BooleanDisplay("BroadcastRCONToOps", "Broadcast RCON commands to ops", "This setting determines whether ops are notified of commands executed over RCON.", "Yes", "No"));
            layout.Add(new HardcoreDisplay("Difficulty", "HardcoreMode", "Difficulty", "This setting controls the difficulty of the server.", new[] { "Peaceful", "Easy", "Normal", "Hard", "Hardcore" }));
            layout.Add(new BooleanDisplay("EnableCommandBlocks", "Enable command blocks", "This setting determines whether command blocks execute on the server.", "Yes", "No"));
            layout.Add(new BooleanDisplay("EnableRCON", "Enable RCON", "This setting determines whether the server accepts RCON (remote console) requests.", "Yes", "No"));
            layout.Add(new ConditionalDisplay(new StringDisplay("RCONPassword", "RCON Password", "This setting determines what passwords users must enter to authenticate with RCON."), config => ((JavaServerConfiguration)config).EnableRCON));
            layout.Add(new ConditionalDisplay(new PortDisplay("RCONPort", "RCON Port", "This setting determines on which port RCON commands are listened for."), config => ((JavaServerConfiguration)config).EnableRCON));
            layout.Add(new BooleanDisplay("SyncChunkWrites", "Enable synchronous chunk writes", "This setting determines whether the server writes world data on the same thread that processes main game logic.  This setting may cause slowdowns when enabled, but can prevent world corruption upon crashing.", "Yes", "No"));
            layout.Add(new BooleanDisplay("EnableQuery", "Enable query", "This setting enables external programs to query the server about things like number of players, host IP/port, or message of the day.", "Yes", "No"));
            layout.Add(new ConditionalDisplay(new PortDisplay("QueryPort", "Query port", "This setting determines which port the server listens on for external queries."), config => ((JavaServerConfiguration)config).EnableQuery));
            layout.Add(new BooleanDisplay("ForceGamemode", "Force default gamemode", "When enabled, this setting causes the server to set players' gamemode to default when they log in.", "Yes", "No"));
            layout.Add(new MultiToggleDisplay("FunctionPermissionLevel", "Function permission level", "This setting determines which commands functions may use during their execution.  Enabling a setting like \"singleplayer commands\" also causes inheritance of the bypass spawn protection option.", new[] { "Bypass spawn protection", "Singleplayer commands", "Multiplayer exclusive commands", "Server console commands" }) { IndexOffset = 1 }); ;
            layout.Add(new MultiToggleDisplay("Gamemode", "Gamemode", "This setting determines the default gamemode that new players spawn with.", new[] { "Survival", "Creative", "Adventure", "Spectator" }));
            layout.Add(new IntegerDisplay("MaxBuildHeight", "Maximum build height", "This setting controls the maximum height at which players can build.  It does not prevent terrain from generating above the height limit.", 0, 256));
            layout.Add(new IntegerDisplay("MaxPlayers", "Maximum player count", "This setting determines the maximum number of players that can be on the server at once.", 0, int.MaxValue));
            layout.Add(new IntegerDisplay("MaxTickTime", "Maximum tick time", "This setting determines how many milliseconds the server will wait for a tick (game logic cycle) to finish before deciding the server has crashed.", 0, int.MaxValue));
            layout.Add(new IntegerDisplay("MaxWorldSize", "Maximum world size", "This setting determines the maximum distance from the center of the world to the world border.  Setting this limit changes the world border length.", 1, 29999984));
            layout.Add(new NetworkCompressionThresholdDisplay("NetworkCompressionThreshold", "Network compression threshold", "This setting controls how many bytes a packet must be for the server to compress it."));
            layout.Add(new BooleanDisplay("OnlineMode", "Online mode", "This setting determines whether the server attempts to authenticate users with Mojang's services.  If set to false, clients can claim that their username is anything they want, resulting in people able to log in as other users.  Only set this to false if you are certain of what it does.", "Yes", "No"));
            layout.Add(new ConditionalDisplay(new BooleanDisplay("PreventProxyConnections", "Prevent proxy connections", "This setting ensures that users log into the server using the same IP address as their Mojang session.", "Yes", "No"), config => ((JavaServerConfiguration)config).OnlineMode));
            layout.Add(new PlayerIdleTimeoutDisplay("PlayerIdleTimeout", "Player idle timeout", "This setting determines how many minutes a player may be idle before the server kicks them."));
            layout.Add(new MultiToggleDisplay("DefaultOpPermissionLevel", "Default op permission level", "This setting determines which commands ops may use by default.  Enabling a setting like \"singleplayer commands\" also causes inheritance of the bypass spawn protection option.", new[] { "Bypass spawn protection", "Singleplayer commands", "Multiplayer exclusive commands", "Server console commands" }) { IndexOffset = 1 });
            layout.Add(new BooleanDisplay("EnablePVP", "Enable PVP", "This setting determines whether players can attack/kill one another.", "Yes", "No"));
            layout.Add(new StringDisplay("ResourcePack", "Resource pack URL", "This setting controls what resource pack the server sends to the client"));
            layout.Add(new StringDisplay("ResourcePackSHA1", "Resource pack SHA1 hash", "This setting is used for verifying the resource pack's integrity by comparing it to the SHA1 hash client side."));
            layout.Add(new StringDisplay("ServerIP", "Bound server IP", "This setting determines whether the server listens on one IP address, or on all possible addresses."));
            layout.Add(new PortDisplay("ServerPort", "Server port", "This setting controls which port the server listens on."));
            layout.Add(new BooleanDisplay("SnooperEnabled", "Snooper enabled", "This setting determines whether the server sends statistical data to Mojang.", "Yes", "No"));
            layout.Add(new BooleanDisplay("SpawnAnimals", "Spawn animals", "This setting determines whether passive mobs (like cows or sheep) can spawn.", "Yes", "No"));
            layout.Add(new BooleanDisplay("SpawnMonsters", "Spawn monsters", "This setting determines whether hostile mobs (like zombies or creepers) can spawn.", "Yes", "No"));
            layout.Add(new BooleanDisplay("SpawnVillagers", "Spawn villagers", "This setting determines whether villagers can spawn.", "Yes", "No"));
            layout.Add(new IntegerDisplay("SpawnProtection", "Spawn protection size", "This setting determines the bounding-box size of spawn protection, where non-op players cannot break blocks.", 0, int.MaxValue));
            layout.Add(new IntegerDisplay("ViewDistance", "Player view distance", "This setting determines the distance (in chunks) that the server will load for any one client.", 0, int.MaxValue));
            layout.Add(new BooleanDisplay("EnforceWhitelist", "Enforce whitelist", "This setting determines whether the server will kick players who are online but not whitelisted when the whitelist is enabled.", "Yes", "No"));
            return layout;
        };
        Provider.ServerPreferenceDisplayBinding.Add(new ConfigurationPreferenceDisplayBinding(typeof(JavaServerConfiguration), layoutGenerator));

        layoutGenerator = () =>
        {
            List<PreferenceDisplay> layout = new List<PreferenceDisplay>();
            layout.Add(new StringDisplay("ServerDisplayName", "Server display name", "This setting determines the name of the server that appears on players' title screens."));
            layout.Add(new BooleanDisplay("AllowCheats", "Allow cheats", "This setting determines whether server operators can use commands in-game.", "Yes", "No"));
            layout.Add(new MultiToggleDisplay("Gamemode", "Gamemode", "This setting determines the default gamemode that new players spawn with.", new[] { "Survival", "Creative", "Adventure", "Spectator" }));
            layout.Add(new MultiToggleDisplay("Difficulty", "Difficulty", "This setting controls the difficulty of the server.", new[] { "Peaceful", "Easy", "Normal", "Hard" }));
            layout.Add(new IntegerDisplay("MaxPlayers", "Maximum player count", "This setting determines the maximum number of players that can be on the server at once.", 0, int.MaxValue));
            layout.Add(new BooleanDisplay("OnlineMode", "Online mode", "This setting determines whether the server attempts to authenticate users with Mojang's services.  If set to false, clients can claim that their username is anything they want, resulting in people able to log in as other users.  Only set this to false if you are certain of what it does.", "Yes", "No"));
            layout.Add(new IntegerDisplay("ViewDistance", "Player view distance", "This setting determines the distance (in chunks) that the server will load for any one client.", 0, int.MaxValue));
            layout.Add(new PlayerIdleTimeoutDisplay("PlayerIdleTimeout", "Player idle timeout", "This setting determines how many minutes a player may be idle before the server kicks them."));
            layout.Add(new IntegerDisplay("MaximumThreads", "Maximum server threads", "This setting determines the maximum number of worker threads that the server will allocate.", 0, int.MaxValue));
            layout.Add(new IntegerDisplay("TickDistance", "Player tick distance", "This setting determines the distance (in chunks) that the server will tick for any one client.", 0, int.MaxValue));
            layout.Add(new BooleanDisplay("TexturePackRequired", "Texture pack required", "This setting determines whether players must download a texture pack in order to join the server.", "Yes", "No"));
            layout.Add(new BooleanDisplay("ContentLogging", "Content logging", "This setting determines whether the server will log content errors to a file.", "Enabled", "Disabled"));
            layout.Add(new NetworkCompressionThresholdDisplay("NetworkCompressionThreshold", "Network compression threshold", "This setting controls how many bytes a packet must be for the server to compress it."));
            layout.Add(new BooleanDisplay("ServerAuthoritativeMovement", "Server authoritative movement", "This setting determines whether the server will analyze player movement to prevent cheating.", "Enabled", "Disabled"));
            layout.Add(new ConditionalDisplay(new IntegerDisplay("PlayerMovementScoreThreshold", "Abnormal player movement score threshold", "This setting determines the number of times that abnormal movement of a player can occur before it is reported.", 1, int.MaxValue), config => ((BedrockServerConfiguration)config).ServerAuthoritativeMovement));
            layout.Add(new ConditionalDisplay(new DoubleDisplay("PlayerMovementDistanceThreshold", "Abnormal player movement distance threshold", "This setting determines the number of times that abnormal behavior of a client can occur before it is reported.", 0, double.MaxValue), config => ((BedrockServerConfiguration)config).ServerAuthoritativeMovement));
            layout.Add(new ConditionalDisplay(new IntegerDisplay("PlayerMovementDurationThreshold", "Abnormal player movement duration threshold", "This setting determines the difference between server and player-calculated positions, in blocks, that must be exceeded before abnormal movement is detected.", 1, int.MaxValue), config => ((BedrockServerConfiguration)config).ServerAuthoritativeMovement));
            layout.Add(new ConditionalDisplay(new BooleanDisplay("CorrectPlayerMovement", "Player movement correction", "This setting determines whether the server should correct players' position when their movement score exceeds the threshold.", "Enabled", "Disabled"), config => ((BedrockServerConfiguration)config).ServerAuthoritativeMovement));
            return layout;
        };
        Provider.ServerPreferenceDisplayBinding.Add(new ConfigurationPreferenceDisplayBinding(typeof(BedrockServerConfiguration), layoutGenerator));
    }

    protected override void OnInitialized()
    {
        ServerSelector.OnServerSelectionChange += ChangeServerSelection;
        PreferenceConfiguration = ServerSelector.CurrentServer.CurrentConfiguration;
        SetPreferencesBasedOnPreferenceType(PreferenceConfiguration.GetType());
    }

    private void ChangeServerSelection()
    {
        PreferenceConfiguration = ServerSelector.CurrentServer.CurrentConfiguration;
        SetPreferencesBasedOnPreferenceType(PreferenceConfiguration.GetType());
        StateHasChanged();
    }

    public void Dispose()
    {
        ServerSelector.OnServerSelectionChange -= ChangeServerSelection;
    }

    private void SetPreferencesBasedOnPreferenceType(Type type)
    {
        int inheritanceDepth = int.MaxValue;
        ConfigurationPreferenceDisplayBinding bestBinding = null;
        foreach(ConfigurationPreferenceDisplayBinding binding in Provider.ServerPreferenceDisplayBinding)
        {
            if(type == binding.PreferenceType)
            {
                PreferenceLayout = binding.Layout();
                return;
            }
            if(type.IsSubclassOf(binding.PreferenceType)) {
                int depth = 0;
                Type baseType = type.BaseType;
                while(baseType != binding.PreferenceType)
                {
                    baseType = baseType.BaseType;
                    depth++;
                }
                if(depth < inheritanceDepth)
                {
                    inheritanceDepth = depth;
                    bestBinding = binding;
                }
            }
        }
        if(bestBinding != null)
        {
            PreferenceLayout = bestBinding.Layout();
        }
    }

    protected void ApplySettings(IMSConfiguration configuration)
    {
        PreferenceConfiguration = configuration;
        if (UpdateSettingsTask != null) { return; }
        ServerProxy toRestart = ServerSelector.CurrentServer;
        ServerConfiguration serverConfig = configuration as ServerConfiguration;
        serverConfig.ID = toRestart.CurrentConfiguration.ID;
        serverConfig.IsEnabled = toRestart.CurrentConfiguration.IsEnabled;
        serverConfig.ServerName = toRestart.CurrentConfiguration.ServerName;
        serverConfig.WorldID = toRestart.CurrentConfiguration.WorldID;
        if (toRestart.State == ServerProxy.ServerState.Disabled)
        {
            toRestart.CurrentConfiguration = (ServerConfiguration)configuration;
            Editor.ErrorText = "Settings applied successfully at " + DateTime.Now + ".";
        }
        else
        {
            Editor.ErrorText = "Restarting server...";
            UpdateSettingsTask = Task.Run(async () =>
            {
                await toRestart.StopAsync();
                toRestart.CurrentConfiguration = (ServerConfiguration)configuration;
                toRestart.StartAsync();
                InvokeAsync(() =>
                {
                    Editor.ErrorText = "Settings applied successfully at " + DateTime.Now + ".";
                    Editor.NotifyStateChanged(true);
                    UpdateSettingsTask = null;
                });
            });
        }
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ServerProvider ServerSelector { get; set; }
    }
}
#pragma warning restore 1591
