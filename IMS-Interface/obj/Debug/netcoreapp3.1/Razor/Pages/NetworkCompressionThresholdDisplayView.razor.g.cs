#pragma checksum "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\NetworkCompressionThresholdDisplayView.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "65d51176cf3da4534c09225b794e8216b1666942"
// <auto-generated/>
#pragma warning disable 1591
namespace IMS_Interface
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
#line 4 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\NetworkCompressionThresholdDisplayView.razor"
using System.Reflection;

#line default
#line hidden
#nullable disable
    public partial class NetworkCompressionThresholdDisplayView : PreferenceDisplayView
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "label");
            __builder.AddContent(1, 
#nullable restore
#line 6 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\NetworkCompressionThresholdDisplayView.razor"
        DisplayData.DisplayName

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(2, "\r\n<br>\r\n");
            __builder.OpenElement(3, "div");
            __builder.AddAttribute(4, "data-toggle", "tooltip");
            __builder.AddAttribute(5, "data-placement", "right");
            __builder.AddAttribute(6, "data-title", 
#nullable restore
#line 10 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\NetworkCompressionThresholdDisplayView.razor"
                  DisplayData.Description

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(7, "style", "display:inline-block; margin:5px 0px");
            __builder.AddMarkupContent(8, "\r\n    ");
            __builder.OpenComponent<IMS_Interface.MultiToggle>(9);
            __builder.AddAttribute(10, "Options", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.String[]>(
#nullable restore
#line 11 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\NetworkCompressionThresholdDisplayView.razor"
                          Values

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(11, "SelectedIndex", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Int32>(
#nullable restore
#line 12 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\NetworkCompressionThresholdDisplayView.razor"
                                SelectedItem

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(12, "OnSelectionChange", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<System.Int32>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<System.Int32>(this, 
#nullable restore
#line 13 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\NetworkCompressionThresholdDisplayView.razor"
                                      x => SelectedItem = x

#line default
#line hidden
#nullable disable
            )));
            __builder.CloseComponent();
            __builder.AddMarkupContent(13, "\r\n");
            __builder.CloseElement();
            __builder.AddMarkupContent(14, "\r\n<br>\r\n");
#nullable restore
#line 16 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\NetworkCompressionThresholdDisplayView.razor"
 if ((int)DisplayData.ParentDisplay.CurrentConfiguration.GetType().GetField(DisplayData.FieldName, BindingFlags.Public | BindingFlags.Instance).GetValue(DisplayData.ParentDisplay.CurrentConfiguration) > 0)
{

#line default
#line hidden
#nullable disable
            __builder.AddContent(15, "    ");
            __builder.OpenElement(16, "input");
            __builder.AddAttribute(17, "class", "form-control");
            __builder.AddAttribute(18, "style", "margin-top:5px;margin-bottom:5px; width:150px; display:unset");
            __builder.AddAttribute(19, "type", "number");
            __builder.AddAttribute(20, "min", "1");
            __builder.AddAttribute(21, "max", (
#nullable restore
#line 22 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\NetworkCompressionThresholdDisplayView.razor"
                 int.MaxValue

#line default
#line hidden
#nullable disable
            ) + ")");
            __builder.AddAttribute(22, "value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 23 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\NetworkCompressionThresholdDisplayView.razor"
                  Value

#line default
#line hidden
#nullable disable
            , culture: global::System.Globalization.CultureInfo.InvariantCulture));
            __builder.AddAttribute(23, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => Value = __value, Value, culture: global::System.Globalization.CultureInfo.InvariantCulture));
            __builder.SetUpdatesAttributeName("value");
            __builder.CloseElement();
            __builder.AddMarkupContent(24, "\r\n    <br>\r\n");
#nullable restore
#line 25 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\NetworkCompressionThresholdDisplayView.razor"
}

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
#nullable restore
#line 27 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\NetworkCompressionThresholdDisplayView.razor"
       
    private string[] Values = new[] { "No compression", "Compress everything", "Compression limit" };

    public string Value
    {
        get
        {
            return DisplayData.ParentDisplay.CurrentConfiguration.GetType().GetField(DisplayData.FieldName, BindingFlags.Public | BindingFlags.Instance).GetValue(DisplayData.ParentDisplay.CurrentConfiguration).ToString();
        }
        set
        {
            int val;
            if (int.TryParse((string)value, out val) && val >= 1 && val <= int.MaxValue)
            {
                DisplayData.ParentDisplay.CurrentConfiguration.GetType().GetField(DisplayData.FieldName, BindingFlags.Public | BindingFlags.Instance).SetValue(DisplayData.ParentDisplay.CurrentConfiguration, val);
            }

            DisplayData.ParentDisplay.NotifyStateChanged();
        }
    }

    public int SelectedItem
    {
        get
        {
            int selection = (int)DisplayData.ParentDisplay.CurrentConfiguration.GetType().GetField(DisplayData.FieldName, BindingFlags.Public | BindingFlags.Instance).GetValue(DisplayData.ParentDisplay.CurrentConfiguration);
            if (selection == -1)
            {
                return 0;
            }
            else if (selection == 0)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }
        set
        {
            if (value >= 0 && value < 3 && value != SelectedItem)
            {
                int toSet = value - 1;
                if (toSet == 1) { toSet = 256; }
                DisplayData.ParentDisplay.CurrentConfiguration.GetType().GetField(DisplayData.FieldName, BindingFlags.Public | BindingFlags.Instance).SetValue(DisplayData.ParentDisplay.CurrentConfiguration, toSet);
            }
            DisplayData.ParentDisplay.NotifyStateChanged();
        }
    }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
