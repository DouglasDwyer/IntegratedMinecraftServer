#pragma checksum "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PortDisplayView.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d58ff113d1ad47f129f80eb65928b214abadbf87"
// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace IMS_Interface.Preferences
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\_Imports.razor"
using Microsoft.AspNetCore.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\_Imports.razor"
using Microsoft.AspNetCore.Components.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\_Imports.razor"
using IMS_Interface;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\_Imports.razor"
using IMS_Interface.Data;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\_Imports.razor"
using IMS_Interface.Player;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\_Imports.razor"
using IMS_Interface.World;

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\_Imports.razor"
using IMS_Interface.Preferences;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PortDisplayView.razor"
using System.Reflection;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PortDisplayView.razor"
using IMS_Library;

#line default
#line hidden
#nullable disable
    public partial class PortDisplayView : PreferenceDisplayView
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 27 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PortDisplayView.razor"
       
    private static readonly string[] Options = new[] { "Forward", "Don't forward" };

    public virtual string PortValue
    {
        get
        {
            return CurrentPort.Port.ToString();
        }
        set
        {
            int val;
            if (int.TryParse((string)value, out val) && val >= 0 && val <= 65535)
            {
                WebPort port = CurrentPort;
                port.Port = val;
                DisplayData.ParentDisplay.CurrentConfiguration.GetType().GetField(DisplayData.FieldName, BindingFlags.Public | BindingFlags.Instance).SetValue(DisplayData.ParentDisplay.CurrentConfiguration, port);
            }
            DisplayData.ParentDisplay.NotifyStateChanged();
        }
    }
    public virtual int ForwardPort
    {
        get
        {
            return CurrentPort.AttemptUPnPForwarding ? 0 : 1;
        }
        set
        {
            if (value == 0 || value == 1)
            {
                WebPort port = CurrentPort;
                port.AttemptUPnPForwarding = value == 0;
                DisplayData.ParentDisplay.CurrentConfiguration.GetType().GetField(DisplayData.FieldName, BindingFlags.Public | BindingFlags.Instance).SetValue(DisplayData.ParentDisplay.CurrentConfiguration, port);
            }
            DisplayData.ParentDisplay.NotifyStateChanged();
        }
    }
    protected WebPort CurrentPort
    {
        get
        {
            return (WebPort)DisplayData.ParentDisplay.CurrentConfiguration.GetType().GetField(DisplayData.FieldName, BindingFlags.Public | BindingFlags.Instance).GetValue(DisplayData.ParentDisplay.CurrentConfiguration);
        }
    }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
