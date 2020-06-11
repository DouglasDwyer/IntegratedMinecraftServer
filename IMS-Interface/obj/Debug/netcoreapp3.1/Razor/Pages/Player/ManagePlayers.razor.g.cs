#pragma checksum "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\ManagePlayers.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4b201e3e17933a75f3eb906f96ff44d5c99c2323"
// <auto-generated/>
#pragma warning disable 1591
namespace IMS_Interface.Player
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
#line 3 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\ManagePlayers.razor"
using System.Reflection;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\ManagePlayers.razor"
using IMS_Library;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/ManagePlayers")]
    public partial class ManagePlayers : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.AddMarkupContent(0, @"<style>
    .tab-ul {
        list-style-type: none;
        margin: 0;
        padding: 0;
        overflow: hidden;
    }

        .tab-ul > li {
            float: left;
        }

            .tab-ul > li > a {
                display: block;
                text-align: center;
                padding: 16px;
                text-decoration: none;
            }

            .tab-ul > li a:hover {
                background-color: #eeeeee;
            }

    .nav-link {
        background: transparent;
        border: 1px solid transparent;
        color: #23527c;
        font-weight: normal;
        border-radius: 3px;
        margin-left: 2px;
        margin-right: 2px;
        padding: 5px;
    }

    .nav-link-active {
        border: 1px solid #23527c;
        background: transparent;
        color: #23527c;
        font-weight: normal;
        border-radius: 3px;
        margin-left: 2px;
        margin-right: 2px;
        padding: 5px 6px 5px 6px;
    }

    .nav-link:hover, .nav-link:focus {
        background: rgba(200, 200, 200, 0.25);
    }
</style>

");
            __builder.AddMarkupContent(1, "<h3>Manage Players</h3>\r\n<br>\r\n");
            __builder.OpenElement(2, "table");
            __builder.AddAttribute(3, "class", "table table-bordered");
            __builder.AddMarkupContent(4, "\r\n    ");
            __builder.OpenElement(5, "thead");
            __builder.AddMarkupContent(6, "\r\n        ");
            __builder.OpenElement(7, "tr");
            __builder.AddMarkupContent(8, "\r\n            ");
            __builder.OpenElement(9, "th");
            __builder.AddMarkupContent(10, "\r\n                ");
            __builder.OpenElement(11, "div");
            __builder.AddAttribute(12, "style", "display:table; margin:0 0");
            __builder.AddMarkupContent(13, "\r\n                    ");
            __builder.OpenElement(14, "ul");
            __builder.AddAttribute(15, "class", "nav tab-ul");
            __builder.AddAttribute(16, "style", "\r\n                        list-style: none;\r\n                        margin-bottom: unset;\r\n                        padding-left: unset;");
            __builder.AddMarkupContent(17, "\r\n");
#nullable restore
#line 70 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\ManagePlayers.razor"
                         foreach (PlayerDisplay display in Provider.PlayerManagerDisplays)
                        {
                            if (display.ShouldDisplayFor(ServerSelector.CurrentServer))
                            {

#line default
#line hidden
#nullable disable
            __builder.AddContent(18, "                                ");
            __builder.OpenElement(19, "li");
            __builder.AddAttribute(20, "style", "display:inline; position:static");
            __builder.AddMarkupContent(21, "\r\n                                    ");
            __builder.OpenElement(22, "button");
            __builder.AddAttribute(23, "class", 
#nullable restore
#line 75 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\ManagePlayers.razor"
                                                     display == SelectedView ? "nav-link-active" : "nav-link"

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(24, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 75 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\ManagePlayers.razor"
                                                                                                                          x => SelectedView = display

#line default
#line hidden
#nullable disable
            ));
            __builder.AddContent(25, 
#nullable restore
#line 75 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\ManagePlayers.razor"
                                                                                                                                                        display.Name

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(26, "\r\n                                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(27, "\r\n");
#nullable restore
#line 77 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\ManagePlayers.razor"
                            }
                        }

#line default
#line hidden
#nullable disable
            __builder.AddContent(28, "                    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(29, "\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(30, "\r\n            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(31, "\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(32, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(33, "\r\n    ");
            __builder.OpenElement(34, "tbody");
            __builder.AddMarkupContent(35, "\r\n        ");
            __builder.OpenElement(36, "tr");
            __builder.AddMarkupContent(37, "\r\n            ");
            __builder.OpenElement(38, "td");
            __builder.AddMarkupContent(39, "\r\n");
#nullable restore
#line 87 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\ManagePlayers.razor"
                 if (SelectedView != null)
                {
                    RenderFragment fragment = builder =>
                    {
                        builder.OpenComponent(0, SelectedView.GetComponentType());
                        builder.AddAttribute(0, "DisplayData", SelectedView);
                        builder.CloseComponent();
                    };
                    

#line default
#line hidden
#nullable disable
            __builder.AddContent(40, 
#nullable restore
#line 95 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\ManagePlayers.razor"
                     fragment

#line default
#line hidden
#nullable disable
            );
#nullable restore
#line 95 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\ManagePlayers.razor"
                             
                }

#line default
#line hidden
#nullable disable
            __builder.AddContent(41, "            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(42, "\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(43, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(44, "\r\n");
            __builder.CloseElement();
            __builder.AddMarkupContent(45, "\r\n\r\n");
            __builder.OpenComponent<IMS_Interface.Timer>(46);
            __builder.AddAttribute(47, "Interval", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Double>(
#nullable restore
#line 102 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\ManagePlayers.razor"
                                    250

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(48, "UpdateScope", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.ComponentBase>(
#nullable restore
#line 102 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\ManagePlayers.razor"
                                                     this

#line default
#line hidden
#nullable disable
            ));
            __builder.AddComponentReferenceCapture(49, (__value) => {
#nullable restore
#line 102 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\ManagePlayers.razor"
             UpdateTimer = (IMS_Interface.Timer)__value;

#line default
#line hidden
#nullable disable
            }
            );
            __builder.CloseComponent();
        }
        #pragma warning restore 1998
#nullable restore
#line 104 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\ManagePlayers.razor"
       
    protected Timer UpdateTimer;
    protected PlayerDisplay SelectedView { get => View.ShouldDisplayFor(ServerSelector.CurrentServer) ? View : View = Provider.PlayerManagerDisplays.First(); set => View = value; }
    protected PlayerDisplay View;

    static ManagePlayers()
    {
        Provider.PlayerManagerDisplays.Add(new OnlinePlayerDisplay());
        Provider.PlayerManagerDisplays.Add(new AllPlayerDisplay());
        Provider.PlayerManagerDisplays.Add(new WhitelistPlayerDisplay());
        Provider.PlayerManagerDisplays.Add(new OpPlayerDisplay());
        Provider.PlayerManagerDisplays.Add(new BanPlayerDisplay());
        Provider.PlayerManagerDisplays.Add(new BanIPDisplay());
    }

    public ManagePlayers()
    {
        SelectedView = Provider.PlayerManagerDisplays.First();
    }

    protected override void OnAfterRender(bool firstRender)
    {
        UpdateTimer.Enabled = true;
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ServerProvider ServerSelector { get; set; }
    }
}
#pragma warning restore 1591
