#pragma checksum "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\BooleanDisplayView.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e41093df0ac1a54c07c2bf185a2bb75fe29aa8b2"
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
#line 2 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\BooleanDisplayView.razor"
using System.Reflection;

#line default
#line hidden
#nullable disable
    public partial class BooleanDisplayView : PreferenceDisplayView
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "label");
            __builder.AddContent(1, 
#nullable restore
#line 4 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\BooleanDisplayView.razor"
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
#line 8 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\BooleanDisplayView.razor"
                  DisplayData.Description

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(7, "style", "display:inline-block; margin:5px 0px");
            __builder.AddMarkupContent(8, "\r\n    ");
            __builder.OpenComponent<IMS_Interface.Shared.MultiToggle>(9);
            __builder.AddAttribute(10, "Options", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.String[]>(
#nullable restore
#line 9 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\BooleanDisplayView.razor"
                          GetOptions()

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(11, "SelectedIndex", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Int32>(
#nullable restore
#line 10 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\BooleanDisplayView.razor"
                                 SelectedItem

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(12, "OnSelectionChange", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<System.Int32>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<System.Int32>(this, 
#nullable restore
#line 11 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\BooleanDisplayView.razor"
                                      x => SelectedItem = x

#line default
#line hidden
#nullable disable
            )));
            __builder.CloseComponent();
            __builder.AddMarkupContent(13, "\r\n    <br>\r\n");
            __builder.CloseElement();
            __builder.AddMarkupContent(14, "\r\n<br>");
        }
        #pragma warning restore 1998
#nullable restore
#line 16 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\BooleanDisplayView.razor"
       
    public virtual int SelectedItem
    {
        get
        {
            return (bool)DisplayData.ParentDisplay.CurrentConfiguration.GetType()
                .GetField(DisplayData.FieldName, BindingFlags.Public | BindingFlags.Instance)
                .GetValue(DisplayData.ParentDisplay.CurrentConfiguration) ? 0 : 1;
        }
        set
        {
            if(value == 0 || value == 1)
            {
                DisplayData.ParentDisplay.CurrentConfiguration.GetType().GetField(DisplayData.FieldName, BindingFlags.Public | BindingFlags.Instance).SetValue(DisplayData.ParentDisplay.CurrentConfiguration, value == 0);
            }
            DisplayData.ParentDisplay.NotifyStateChanged();
        }
    }

    protected string[] GetOptions() {
        return new[] { (DisplayData as BooleanDisplay).WhenTrue, (DisplayData as BooleanDisplay).WhenFalse };
    }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
