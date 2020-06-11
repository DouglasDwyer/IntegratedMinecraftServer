using KinglyStudios.Knetworking;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Library
{
    /// <summary>
    /// This class represents a Minecraft server wrapper, and provides functionality for server management.
    /// </summary>
    public abstract class ServerProxy
    {
        /// <summary>
        /// The unique identifier of the server.
        /// </summary>
        public Guid ID { get; protected set; }
        /// <summary>
        /// The current state of the internal server process.
        /// </summary>
        public ServerState State { get; protected set; }
        /// <summary>
        /// The current server settings.
        /// </summary>
        public abstract ServerConfiguration CurrentConfiguration { get; set; }

        /// <summary>
        /// Whether the server supports whitelisting players.
        /// </summary>
        public abstract bool SupportsWhitelist { get; }
        /// <summary>
        /// Whether the server's whitelist is currently enabled.
        /// </summary>
        public abstract bool WhitelistEnabled { get; set; }
        /// <summary>
        /// Whether the server supports banning players.
        /// </summary>
        public abstract bool SupportsBanning { get; }
        /// <summary>
        /// Whether the server supports banning by IP address.
        /// </summary>
        public abstract bool SupportsBanningIPs { get; }
        /// <summary>
        /// Whether the server supports player permissions.
        /// </summary>
        public abstract bool SupportsOps { get; }
        /// <summary>
        /// Whether the server supports gathering player IP addresses.
        /// </summary>
        public abstract bool SupportsIPs { get; }
        /// <summary>
        /// Whether the server supports kicking online players.
        /// </summary>
        public abstract bool SupportsKicking { get; }
        /// <summary>
        /// The version of Minecraft that this server is targeted toward.
        /// </summary>
        public abstract MinecraftEdition SupportedEdition { get; }

        /// <summary>
        /// Creates a new <see cref="ServerProxy"/> instance with the specified unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier that the server should be associated with.</param>
        public ServerProxy(Guid guid) {
            ID = guid;
        }

        /// <summary>
        /// Starts the Minecraft server.  This call does not complete until the Minecraft server is in the <see cref="ServerState.Running"/> phase.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents the state of the start operation.</returns>
        public abstract Task StartAsync();
        /// <summary>
        /// Stops the Minecraft server.  This call does not complete until the Minecraft server is in the <see cref="ServerState.Disabled"/> phase.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents the state of the stop operation.</returns>
        public abstract Task StopAsync();
        /// <summary>
        /// Restarts the Minecraft server.  This call does not complete until the Minecraft server has restarted and is in the <see cref="ServerState.Running"/> phase.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that represents the state of the restart operation.</returns>
        public abstract Task RestartAsync();
        /// <summary>
        /// Retrieves a Minecraft player's data by their username.
        /// </summary>
        /// <param name="username">The username of the player to find.</param>
        /// <returns>An object with information about the player, or null if no player was found..</returns>
        public abstract MinecraftPlayer GetPlayerInformationByUsername(string username);
        /// <summary>
        /// Retrieves a Minecraft player's data by their UUID.
        /// </summary>
        /// <param name="uuid">The unique identifier of the player to find.</param>
        /// <returns>An object with information about the player, or null if no player was found.</returns>
        public abstract MinecraftPlayer GetPlayerInformationByUUID(string uuid);
        /// <summary>
        /// Retrieves a list of players currently logged onto the Minecraft server.
        /// </summary>
        /// <returns>A list of players.</returns>
        public abstract IEnumerable<MinecraftPlayer> GetOnlinePlayers();
        /// <summary>
        /// Retrieves a list of all players who have logged onto the Minecraft server.
        /// </summary>
        /// <returns>A list of players.</returns>
        public abstract IEnumerable<MinecraftPlayer> GetAllPlayers();
        /// <summary>
        /// Retrieves a list of all server operators registered on the server.
        /// </summary>
        /// <returns>A list of players.</returns>
        public abstract IEnumerable<MinecraftPlayer> GetAllOps();
        /// <summary>
        /// Retrieves a list of all players banned from the server.
        /// </summary>
        /// <returns>A list of players.</returns>
        public abstract List<BanInformation> GetAllBans();
        /// <summary>
        /// Retrieves a list of all IPs banned from the server.
        /// </summary>
        /// <returns>A list of IP ban information.</returns>
        public abstract List<BanIPTag> GetAllBannedIPs();
        /// <summary>
        /// Retrieves a list of all players currently whitelisted on the server.
        /// </summary>
        /// <returns>A list of players.</returns>
        public abstract IEnumerable<MinecraftPlayer> GetAllWhitelistedPlayers();
        /// <summary>
        /// Adds a player to the server whitelist.
        /// </summary>
        /// <param name="name">The name of the player to whitelist.</param>
        public abstract void WhitelistPlayer(string name);
        /// <summary>
        /// Removes a player from the server whitelist.
        /// </summary>
        /// <param name="name">The name of the player to remove.</param>
        public abstract void RemoveWhitelistPlayer(string name);
        /// <summary>
        /// Makes a player a server operator.
        /// </summary>
        /// <param name="name">The name of the player to promote.</param>
        public abstract void OpPlayer(string name);
        /// <summary>
        /// Revokes a player's server operator status.
        /// </summary>
        /// <param name="name">The name of the player to demote.</param>
        public abstract void DeopPlayer(string name);
        /// <summary>
        /// Kicks a player from the Minecraft server.
        /// </summary>
        /// <param name="name">The name of the player to disconnect.</param>
        /// <param name="reason">The disconnection reason that should display on the player's screen.</param>
        public abstract void KickPlayer(string name, string reason);
        /// <summary>
        /// Permanently bans a player from the server.
        /// </summary>
        /// <param name="name">The name of the player to banish.</param>
        /// <param name="reason">The reason for banning that should display on the player's screen.</param>
        public abstract void BanPlayer(string name, string reason);
        /// <summary>
        /// Permanently bans an IP address from the server.
        /// </summary>
        /// <param name="ip">The IP to ban.</param>
        /// <param name="reason">The reason for banning that should display on the players' screens who have this IP.</param>
        public abstract void BanIP(string ip, string reason);
        /// <summary>
        /// Lifts a player's ban, allowing them to rejoin the server.
        /// </summary>
        /// <param name="name">The name of the player to unban.</param>
        public abstract void UnbanPlayer(string name);
        /// <summary>
        /// Lifts an IP address ban, allowing those with the address to rejoin the server.
        /// </summary>
        /// <param name="ip">The IP address to unban.</param>
        public abstract void UnbanIP(string ip);
        /// <summary>
        /// Sends a command to the server's input console.
        /// </summary>
        /// <param name="command">The command to send.</param>
        public abstract void SendConsoleCommand(string command);
        /// <summary>
        /// Retrieves a reasonably large number of lines of recent output from the Minecraft server console.
        /// </summary>
        /// <returns>A block of recent server console text.</returns>
        public abstract string GetConsoleText();
        /// <summary>
        /// Reloads the server whitelist from the server whitelist file.
        /// </summary>
        public abstract void ReloadServerWhitelist();
        /// <summary>
        /// Reloads the server permissions list from the server whitelist file.
        /// </summary>
        public abstract void ReloadServerPermissions();
        /// <summary>
        /// Retrieves a list of all currently-existing Minecraft server logfiles.
        /// </summary>
        /// <returns>A list containing information about each logfile.</returns>
        public abstract List<LogFileInformation> GetLogFiles();
        /// <summary>
        /// Retrieves the text of a logfile based on its name.
        /// </summary>
        /// <param name="name">The name of the logfile to retrieve.</param>
        /// <returns>The text inside the logfile.</returns>
        public abstract string GetLogFile(string name);
        /// <summary>
        /// Copies the current server world to a specified location on disk.
        /// </summary>
        /// <param name="location">The absolute path of the folder to copy the current Minecraft world to.</param>
        /// <returns>A <see cref="Task"/> object representing the current state of the backup operation.</returns>
        public abstract Task BackupToLocationAsync(string location);
        /// <summary>
        /// Copies the current server world to a zip file.
        /// </summary>
        /// <param name="file">The absolute path of the zip file to make of the current Minecraft world.</param>
        /// <returns>A <see cref="Task"/> object representing the current state of the backup operation.</returns>
        public abstract Task BackupToZipFileAsync(string file);

        /// <summary>
        /// Loads the server's previous settings from disk.
        /// </summary>
        /// <returns>A configuration object representing the loaded server settings.</returns>
        public virtual ServerConfiguration GetDefaultServerConfiguration()
        {
            return new ServerConfiguration(ID).FromConfiguration();
        }

        /// <summary>
        /// This enum is used to represent the current state of the internal server process.
        /// </summary>
        public enum ServerState {
            /// <summary>
            /// The server is not currently running.
            /// </summary>
            Disabled,
            /// <summary>
            /// The server is loading files from disk, but is not ready to accept players yet.
            /// </summary>
            Starting,
            /// <summary>
            /// The server is running and players can join/interact with the Minecraft world.
            /// </summary>
            Running,
            /// <summary>
            /// The server is saving its files and shutting down.
            /// </summary>
            Stopping
        }
    }
}
