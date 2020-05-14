using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Library
{
    [Serializable]
    public struct BanIPTag
    {
        public string ip;
        public string created;
        public string source;
        public string expires;
        public string reason;
    }
}
