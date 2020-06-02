using IMS_Interface.Pages.World;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMS_Interface.Data
{
    public class PopupProvider
    {
        public RenderFragment CurrentPopupFragment { get; protected set; }
        public Action OnPopupChange;

        public void ShowPopup<T>(PopupDisplay<T> display) where T : ComponentBase
        {
            lock (this)
            {
                if (display is null)
                {
                    CurrentPopupFragment = null;
                    return;
                }
                Type component = display.GetComponentType();
                CurrentPopupFragment = builder =>
                {
                    builder.OpenComponent(0, component);
                    if (component.GetProperty("DisplayData") != null)
                    {
                        builder.AddAttribute(0, "DisplayData", display);
                    }
                    builder.CloseComponent();
                };
                OnPopupChange?.Invoke();
            }
        }

        public void ClosePopup()
        {
            lock(this)
            {
                CurrentPopupFragment = null;
                OnPopupChange?.Invoke();
            }
        }
    }
}
