#pragma checksum "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Index.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f522df3fcf6eb1dd607d6265efea0cef26748d08"
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
#line 4 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Index.razor"
using IMS_Library;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/")]
    public partial class Index : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.AddMarkupContent(0, "<h3>IMS Overview</h3>\r\n");
            __builder.OpenElement(1, "div");
            __builder.AddAttribute(2, "style", "width:fit-content;height:fit-content;margin:auto");
            __builder.AddMarkupContent(3, "\r\n");
#nullable restore
#line 10 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Index.razor"
     foreach (ServerProxy server in IMS.Instance.ServerManager.Servers)
    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(4, "        ");
            __builder.OpenComponent<IMS_Interface.ServerOverviewWidget>(5);
            __builder.AddAttribute(6, "CurrentServer", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<IMS_Library.ServerProxy>(
#nullable restore
#line 12 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Index.razor"
                                             server

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(7, "OnDelete", new System.Action(
#nullable restore
#line 12 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Index.razor"
                                                               StateHasChanged

#line default
#line hidden
#nullable disable
            ));
            __builder.CloseComponent();
            __builder.AddMarkupContent(8, "\r\n        <br>\r\n");
#nullable restore
#line 14 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Index.razor"
    }

#line default
#line hidden
#nullable disable
            __builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line 17 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Index.razor"
       
    protected override void OnAfterRender(bool firstRender)
    {
        if(ServerSelector.CurrentServer is null)
        {
            Navigator.NavigateTo("/CreateNewServer");
        }
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ServerProvider ServerSelector { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private NavigationManager Navigator { get; set; }
    }
}
#pragma warning restore 1591