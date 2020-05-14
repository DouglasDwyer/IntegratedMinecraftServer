using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Library
{
    public static class MojangInteropUtility
    {
        public static MemoryCache SkinLinkCache = new MemoryCache("IMS", null, true);

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

        [Obsolete("This method is blocking and takes extremely long.")]
        public static string GetLinkToPlayerSkin(string uuid)
        {
            return "https://crafatar.com/avatars/" + uuid;

            //https://sessionserver.mojang.com/session/minecraft/profile/
            if (uuid is null)
            {
                return null;
            }
            string cachedLink = (string)SkinLinkCache.Get(uuid);
            if(cachedLink != null)
            {
                if(cachedLink == "")
                {
                    cachedLink = null;
                }
                return cachedLink;
            }
            try
            {
                WebClientWithTimeout mojangRequest = new WebClientWithTimeout(250);
                PlayerProfileTag tag = JsonConvert.DeserializeObject<PlayerProfileTag>(mojangRequest.DownloadString("https://sessionserver.mojang.com/session/minecraft/profile/" + uuid.Replace("-","")));
                string toReturn = tag.properties[0].GetDecodedValueTag().textures.SKIN.url;
                SkinLinkCache.Add(uuid, toReturn, new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(10 * 60) });
                return toReturn;
            }
            catch
            {
                Logger.WriteWarning("Could not get player " + uuid + "'s profile from Mojang servers!");
                SkinLinkCache.Add(uuid, "", new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(61) });
                return null;
            }
        }

        protected class WebClientWithTimeout : WebClient
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
