using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS_Library
{
    [Serializable]
    public struct OpTag
    {
        public string uuid;
        public string name;
        public int level;
        public bool bypassesPlayerLimit;
    }
}
