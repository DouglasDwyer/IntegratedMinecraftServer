using IMS_Interface.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS_Interface
{
    public class InformationPopupDisplay : PopupDisplay<InformationPopupDisplayView>
    {
        public string Title;
        public string Information;
        public string ButtonText;

        public InformationPopupDisplay(string info, string title = "", string buttonText = "Okay")
        {
            Information = info;
            Title = title;
            ButtonText = buttonText;
        }
    }
}
