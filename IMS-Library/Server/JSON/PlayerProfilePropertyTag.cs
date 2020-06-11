using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Library
{
    internal struct PlayerProfilePropertyTag
    {
        public string name;
        public string value;

        public PlayerProfileValueTag GetDecodedValueTag()
        {
            return JsonConvert.DeserializeObject<PlayerProfileValueTag>(Encoding.UTF8.GetString(Convert.FromBase64String(value)));
        }
    }
}
