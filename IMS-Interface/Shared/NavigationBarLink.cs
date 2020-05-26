using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS_Interface
{
    public struct NavigationBarLink
    {
        public string ImageLocation;
        public string Tooltip;
        public string URL;

        public NavigationBarLink(string url, string tooltip, string image) {
            ImageLocation = image;
            Tooltip = tooltip;
            URL = url;
        }
    }
}
