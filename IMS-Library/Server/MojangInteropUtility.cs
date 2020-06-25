using Newtonsoft.Json;
using RoyalXML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Library
{
    /// <summary>
    /// This class provides methods for interacting with the Mojang web API.
    /// </summary>
    public static class MojangInteropUtility
    {
        /// <summary>
        /// Retrieves a JSON structure describing all existing versions of Minecraft.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that that represents the current state of the download operation.</returns>
        /// <exception cref="WebException">
        /// Thrown if the web API request times out.
        /// </exception>
        public static async Task<VersionInformationTag> GetAllJavaVersionInformation()
        {
            WebClientWithTimeout mojangRequest = new WebClientWithTimeout(1000);
            return await Task.Run(() => JsonConvert.DeserializeObject<VersionInformationTag>(mojangRequest.DownloadString("https://launchermeta.mojang.com/mc/game/version_manifest.json")));
        }

        /// <summary>
        /// Retrieves information about the latest version of the Minecraft: Bedrock Edition server.
        /// </summary>
        /// <returns>A <see cref="Task"/> object that that represents the current state of the download operation.</returns>
        /// <exception cref="WebException">
        /// Thrown if the web API request times out.
        /// </exception>
        public static async Task<ServerVersionInformation> GetCurrentBedrockServerVersionInformation()
        {
            WebClientWithTimeout mojangRequest = new WebClientWithTimeout(1000);
            return await Task.Run(() => RoyalSerializer.XMLToObject<ServerVersionInformation>(mojangRequest.DownloadString("http://raw.githubusercontent.com/DouglasDwyer/IntegratedMinecraftServer/master/IMS-Distribution/bedrock-version.xml")));
        }

        /// <summary>
        /// Retrieves a JSON structure describing data about a single version of Minecraft.
        /// </summary>
        /// <param name="version">The version to download additional information about.</param>
        /// <returns>A <see cref="Task"/> object that represents the current state of the download operation.</returns>
        /// <exception cref="WebException">
        /// Thrown if the web API request times out.
        /// </exception>
        public static async Task<VersionDataTag> GetVersionInformation(VersionMetadataTag version)
        {
            WebClientWithTimeout mojangRequest = new WebClientWithTimeout(1000);
            return await Task.Run(() => JsonConvert.DeserializeObject<VersionDataTag>(mojangRequest.DownloadString(version.url)));
        }

        /// <summary>
        /// Gets the UUID of a Minecraft: Java Edition player from their username.
        /// </summary>
        /// <param name="username">The username of the player to get information about.</param>
        /// <returns>A string that represents the player's UUID.</returns>
        public static string GetUUIDFromUsername(string username)
        {
            try
            {
                WebClientWithTimeout mojangRequest = new WebClientWithTimeout(1000);
                PlayerIDPairTag tag = JsonConvert.DeserializeObject<PlayerIDPairTag>(mojangRequest.DownloadString("https://api.mojang.com/users/profiles/minecraft/" + username));
                return tag.id;
            }
            catch
            {
                Logger.WriteWarning("Could not get player " + username + "'s UUID from Mojang servers!");
                return null;
            }
        }

        /// <summary>
        /// Gets the username of a Minecraft: Java Edition player from their UUID.
        /// </summary>
        /// <param name="uuid">The UUID of the player to get information about.</param>
        /// <returns>A string representing the player's username.</returns>
        public static string GetUsernameFromUUID(string uuid)
        {
            try
            {
                WebClientWithTimeout mojangRequest = new WebClientWithTimeout(1000);
                NameTimelineTag[] tag = JsonConvert.DeserializeObject<NameTimelineTag[]>(mojangRequest.DownloadString("https://api.mojang.com/user/profiles/" + uuid.Replace("-","") + "/names"));
                return tag.Last().name;
            }
            catch
            {
                Logger.WriteWarning("Could not get player " + uuid + "'s username from Mojang servers!");
                return null;
            }
        }

        private class WebClientWithTimeout : WebClient
        {
            public int Timeout;

            public WebClientWithTimeout(int time)
            {
                Timeout = time;
            }


            protected override WebRequest GetWebRequest(Uri address)
            {
                WebRequest wr = base.GetWebRequest(address);
                wr.Timeout = Timeout;
                return wr;
            }
        }
    }
}
