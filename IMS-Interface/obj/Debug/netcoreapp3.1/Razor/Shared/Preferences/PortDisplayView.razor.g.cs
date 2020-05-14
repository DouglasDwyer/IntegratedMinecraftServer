#pragma checksum "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PortDisplayView.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "54180a9e68d3b7f253fd73eda4af1e81118d30bc"
// <auto-generated/>
#pragma warning disable 1591
namespace IMS_Interface.Shared.Preferences
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
using IMS_Interface.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\_Imports.razor"
using IMS_Interface.Shared.Preferences;

#line default
#line hidden
#nullable disable
#nullable restore
#line 11 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\_Imports.razor"
using IMS_Interface.Data;

#line default
#line hidden
#nullable disable
#nullable restore
#line 12 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\_Imports.razor"
using IMS_Interface.Pages.Player;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PortDisplayView.razor"
using System.Reflection;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PortDisplayView.razor"
using IMS_Library;

#line default
#line hidden
#nullable disable
    public partial class PortDisplayView : PreferenceDisplayView
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "label");
            __builder.AddContent(1, 
#nullable restore
#line 6 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PortDisplayView.razor"
        DisplayData.DisplayName

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(2, "\r\n<br>\r\n");
            __builder.OpenElement(3, "input");
            __builder.AddAttribute(4, "class", "form-control");
            __builder.AddAttribute(5, "style", "margin-top:5px;margin-bottom:5px; width:200px; display:unset");
            __builder.AddAttribute(6, "data-toggle", "tooltip");
            __builder.AddAttribute(7, "data-placement", "right");
            __builder.AddAttribute(8, "data-title", 
#nullable restore
#line 12 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PortDisplayView.razor"
                    DisplayData.Description

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(9, "type", "number");
            __builder.AddAttribute(10, "min", "0");
            __builder.AddAttribute(11, "max", "65535");
            __builder.AddAttribute(12, "value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 16 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PortDisplayView.razor"
              PortValue

#line default
#line hidden
#nullable disable
            , culture: global::System.Globalization.CultureInfo.InvariantCulture));
            __builder.AddAttribute(13, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => PortValue = __value, PortValue, culture: global::System.Globalization.CultureInfo.InvariantCulture));
            __builder.SetUpdatesAttributeName("value");
            __builder.CloseElement();
            __builder.AddMarkupContent(14, "\r\n<br>\r\n");
            __builder.OpenComponent<IMS_Interface.Shared.MultiToggle>(15);
            __builder.AddAttribute(16, "Options", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.String[]>(
#nullable restore
#line 18 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PortDisplayView.razor"
                      Options

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(17, "SelectedIndex", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Int32>(
#nullable restore
#line 18 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PortDisplayView.razor"
                                               ForwardPort

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(18, "OnSelectionChange", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<System.Int32>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<System.Int32>(this, 
#nullable restore
#line 18 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PortDisplayView.razor"
                                                                                 x => ForwardPort = x

#line default
#line hidden
#nullable disable
            )));
            __builder.CloseComponent();
        }
        #pragma warning restore 1998
#nullable restore
#line 20 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PortDisplayView.razor"
       
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
            if(value == 0 || value == 1)
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
