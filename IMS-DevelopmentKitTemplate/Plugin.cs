using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMS_Library;

namespace IMS_DevelopmentKit
{
    public class Plugin : IMSPluginBase
    {
        public override string Name => "New plugin";

        public override string Author => "Anonymous";

        public override string Description => "A new plugin.";

        public override void Start()
        {

        }

        public override void Stop()
        {
            
        }
    }
}
