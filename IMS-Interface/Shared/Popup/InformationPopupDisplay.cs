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
        public Action<int> OnUserSubmit;
        public string[] Buttons;

        public InformationPopupDisplay(string info, string title, Action<int> onUserSubmit, params string[] buttonText)
        {
            if(buttonText.Length == 0)
            {
                buttonText = new[] { "Okay" };
            }
            Information = info;
            Title = title;
            OnUserSubmit = onUserSubmit;
            Buttons = buttonText;
        }
    }
}
