using IMS_Interface.Data;
using IMS_Interface.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IMS_Interface.Pages.Popup;

namespace IMS_Interface.Shared.Popup
{
    public class LoadingPopupDisplay : PopupDisplay<LoadingPopupDisplayView>
    {
        public string Text;

        public LoadingPopupDisplay(string text)
        {
            Text = text;
        }
    }
}
