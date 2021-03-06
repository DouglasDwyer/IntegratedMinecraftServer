#pragma checksum "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e4672aae205d32b655cb68fb41045d209c37145a"
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
#line 3 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
using IMS_Library;

#line default
#line hidden
#nullable disable
    public partial class BanPlayerDisplayView : PlayerDisplayView
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.AddMarkupContent(0, @"<style>
    .playertable > thead > tr > th {
        vertical-align: middle;
    }

    .playertable > tbody > tr > th {
        vertical-align: middle;
    }

    .playertable > tbody > tr > td {
        vertical-align: middle;
    }

    .playerheadicon {
        width: 28px;
        height: 28px;
        background-size: 100%;
        image-rendering: pixelated;
    }

    .linkbutton {
        background: none !important;
        border: none;
        padding: 0 !important;
        color: #23527c;
    }
</style>
");
#nullable restore
#line 36 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
  
    IEnumerable<BanInformation> players = CurrentServer.GetAllBans();
    if (players.Count() == 0)
    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(1, "        ");
            __builder.AddMarkupContent(2, "<label style=\"padding-top:3px; font-weight:normal\"><i>There are currently no banned players.</i></label>\r\n");
#nullable restore
#line 41 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
        return;
    }

#line default
#line hidden
#nullable disable
            __builder.OpenElement(3, "table");
            __builder.AddAttribute(4, "class", "playertable table table-striped");
            __builder.AddMarkupContent(5, "\r\n");
#nullable restore
#line 45 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
      
        bool renderIPs = CurrentServer.SupportsIPs;
        bool renderBans = CurrentServer.SupportsBanning;
        bool renderBanIPs = CurrentServer.SupportsBanningIPs;
        bool renderWhitelist = CurrentServer.SupportsWhitelist;
        bool renderOps = CurrentServer.SupportsOps;
        bool renderKicking = CurrentServer.SupportsKicking;
    

#line default
#line hidden
#nullable disable
            __builder.AddContent(6, "    ");
            __builder.OpenElement(7, "thead");
            __builder.AddMarkupContent(8, "\r\n        ");
            __builder.OpenElement(9, "tr");
            __builder.AddMarkupContent(10, "\r\n            ");
            __builder.AddMarkupContent(11, "<th style=\"width:32px\">\r\n            </th>\r\n            ");
            __builder.AddMarkupContent(12, "<th>\r\n                Name\r\n            </th>\r\n            ");
            __builder.AddMarkupContent(13, "<th>\r\n                UUID\r\n            </th>\r\n");
#nullable restore
#line 63 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
             if (renderIPs)
            {

#line default
#line hidden
#nullable disable
            __builder.AddContent(14, "                ");
            __builder.AddMarkupContent(15, "<th>\r\n                    IP\r\n                </th>\r\n");
#nullable restore
#line 68 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
            }

#line default
#line hidden
#nullable disable
            __builder.AddContent(16, "            ");
            __builder.AddMarkupContent(17, "<th>\r\n                Banned on\r\n            </th>\r\n            ");
            __builder.AddMarkupContent(18, "<th>\r\n                Banned by\r\n            </th>\r\n            ");
            __builder.AddMarkupContent(19, "<th>\r\n                Reason\r\n            </th>\r\n            <th></th>\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(20, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(21, "\r\n    ");
            __builder.OpenElement(22, "tbody");
            __builder.AddMarkupContent(23, "\r\n");
#nullable restore
#line 82 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
          
            List<BanIPTag> banIPs = CurrentServer.GetAllBannedIPs();

        

#line default
#line hidden
#nullable disable
#nullable restore
#line 86 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
         foreach (BanInformation info in players)
        {

#line default
#line hidden
#nullable disable
            __builder.AddContent(24, "            ");
            __builder.OpenElement(25, "tr");
            __builder.AddMarkupContent(26, "\r\n                ");
            __builder.OpenElement(27, "td");
            __builder.AddAttribute(28, "style", "width:32px");
            __builder.AddMarkupContent(29, "\r\n                    ");
            __builder.OpenElement(30, "div");
            __builder.AddAttribute(31, "class", "playerheadicon");
            __builder.AddAttribute(32, "style", "background-image:url(" + (
#nullable restore
#line 90 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                                                                              "https://crafatar.com/avatars/" + info.Player.UUID + "?overlay"

#line default
#line hidden
#nullable disable
            ) + ")");
            __builder.AddMarkupContent(33, "\r\n                    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(34, "\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(35, "\r\n                ");
            __builder.OpenElement(36, "th");
            __builder.AddMarkupContent(37, "\r\n                    ");
            __builder.AddContent(38, 
#nullable restore
#line 94 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                     info.Player.Username

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(39, "\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(40, "\r\n                ");
            __builder.OpenElement(41, "td");
            __builder.AddMarkupContent(42, "\r\n                    ");
            __builder.AddContent(43, 
#nullable restore
#line 97 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                     info.Player.UUID

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(44, "\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(45, "\r\n");
#nullable restore
#line 99 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                 if (renderIPs)
                {

#line default
#line hidden
#nullable disable
            __builder.AddContent(46, "                    ");
            __builder.OpenElement(47, "td");
            __builder.AddMarkupContent(48, "\r\n                        ");
            __builder.AddContent(49, 
#nullable restore
#line 102 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                         info.Player.IP

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(50, "\r\n                    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(51, "\r\n");
#nullable restore
#line 104 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                }

#line default
#line hidden
#nullable disable
            __builder.AddContent(52, "                ");
            __builder.OpenElement(53, "td");
            __builder.AddMarkupContent(54, "\r\n                    ");
            __builder.AddContent(55, 
#nullable restore
#line 106 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                     DateTime.Parse(info.CreatedDate).ToString()

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(56, "\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(57, "\r\n                ");
            __builder.OpenElement(58, "td");
            __builder.AddMarkupContent(59, "\r\n                    ");
            __builder.AddContent(60, 
#nullable restore
#line 109 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                     info.BanSource

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(61, "\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(62, "\r\n                ");
            __builder.OpenElement(63, "td");
            __builder.AddMarkupContent(64, "\r\n                    ");
            __builder.AddContent(65, 
#nullable restore
#line 112 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                     info.Reason

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(66, "\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(67, "\r\n                ");
            __builder.OpenElement(68, "td");
            __builder.AddAttribute(69, "style", "text-align:right");
            __builder.AddMarkupContent(70, "\r\n                    ");
            __builder.OpenElement(71, "div");
            __builder.AddAttribute(72, "style", "display:inline");
            __builder.AddMarkupContent(73, "\r\n");
#nullable restore
#line 116 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                           bool somethingPrevious = false; 

#line default
#line hidden
#nullable disable
#nullable restore
#line 117 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                         if (renderWhitelist)
                        {
                            somethingPrevious = true;
                            

#line default
#line hidden
#nullable disable
#nullable restore
#line 120 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                             if (info.Player.IsWhitelisted)
                            {

#line default
#line hidden
#nullable disable
            __builder.AddContent(74, "                                ");
            __builder.OpenElement(75, "button");
            __builder.AddAttribute(76, "class", "linkbutton");
            __builder.AddAttribute(77, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 122 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                                                                     x => UnwhitelistPlayer(info.Player.Username)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddContent(78, "Unwhitelist");
            __builder.CloseElement();
            __builder.AddMarkupContent(79, "\r\n");
#nullable restore
#line 123 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                            }
                            else
                            {

#line default
#line hidden
#nullable disable
            __builder.AddContent(80, "                                ");
            __builder.OpenElement(81, "button");
            __builder.AddAttribute(82, "class", "linkbutton");
            __builder.AddAttribute(83, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 126 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                                                                     x => WhitelistPlayer(info.Player.Username)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddContent(84, "Whitelist");
            __builder.CloseElement();
            __builder.AddMarkupContent(85, "\r\n");
#nullable restore
#line 127 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                            }

#line default
#line hidden
#nullable disable
#nullable restore
#line 127 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                             
                        }

#line default
#line hidden
#nullable disable
#nullable restore
#line 129 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                         if (renderOps)
                        {
                            if (somethingPrevious)
                            {
                                

#line default
#line hidden
#nullable disable
            __builder.AddContent(86, 
#nullable restore
#line 133 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                                  "|"

#line default
#line hidden
#nullable disable
            );
#nullable restore
#line 133 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                                      
                            }
                            somethingPrevious = true;
                            

#line default
#line hidden
#nullable disable
#nullable restore
#line 136 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                             if (info.Player.PermissionLevel > 0)
                            {

#line default
#line hidden
#nullable disable
            __builder.AddContent(87, "                                ");
            __builder.OpenElement(88, "button");
            __builder.AddAttribute(89, "class", "linkbutton");
            __builder.AddAttribute(90, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 138 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                                                                     x => DeopPlayer(info.Player.Username)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddContent(91, "Deop");
            __builder.CloseElement();
            __builder.AddMarkupContent(92, "\r\n");
#nullable restore
#line 139 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                            }
                            else
                            {

#line default
#line hidden
#nullable disable
            __builder.AddContent(93, "                                ");
            __builder.OpenElement(94, "button");
            __builder.AddAttribute(95, "class", "linkbutton");
            __builder.AddAttribute(96, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 142 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                                                                     x => OpPlayer(info.Player.Username)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddContent(97, "Op");
            __builder.CloseElement();
            __builder.AddMarkupContent(98, "\r\n");
#nullable restore
#line 143 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                            }

#line default
#line hidden
#nullable disable
#nullable restore
#line 143 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                             
                        }

#line default
#line hidden
#nullable disable
#nullable restore
#line 145 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                         if (renderBans)
                        {
                            if (somethingPrevious)
                            {
                                

#line default
#line hidden
#nullable disable
            __builder.AddContent(99, 
#nullable restore
#line 149 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                                  "|"

#line default
#line hidden
#nullable disable
            );
#nullable restore
#line 149 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                                      
                            }
                            somethingPrevious = true;

#line default
#line hidden
#nullable disable
            __builder.AddContent(100, "                            ");
            __builder.OpenElement(101, "button");
            __builder.AddAttribute(102, "class", "linkbutton");
            __builder.AddAttribute(103, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 152 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                                                                 x => UnbanPlayer(info.Player.Username)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddContent(104, "Unban");
            __builder.CloseElement();
            __builder.AddMarkupContent(105, "\r\n");
#nullable restore
#line 153 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                        }

#line default
#line hidden
#nullable disable
#nullable restore
#line 154 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                         if (renderBanIPs && !string.IsNullOrEmpty(info.Player.IP))
                        {
                            if (somethingPrevious)
                            {
                                

#line default
#line hidden
#nullable disable
            __builder.AddContent(106, 
#nullable restore
#line 158 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                                  "|"

#line default
#line hidden
#nullable disable
            );
#nullable restore
#line 158 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                                      
                            }
                            somethingPrevious = true;
                            if (banIPs.FindIndex(x => x.ip == info.Player.IP) < 0)
                            {

#line default
#line hidden
#nullable disable
            __builder.AddContent(107, "                                ");
            __builder.OpenElement(108, "button");
            __builder.AddAttribute(109, "class", "linkbutton");
            __builder.AddAttribute(110, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 163 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                                                                     x => BanIP(info.Player.IP)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddContent(111, "Ban IP");
            __builder.CloseElement();
            __builder.AddMarkupContent(112, "\r\n");
#nullable restore
#line 164 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                            }
                            else
                            {

#line default
#line hidden
#nullable disable
            __builder.AddContent(113, "                                ");
            __builder.OpenElement(114, "button");
            __builder.AddAttribute(115, "class", "linkbutton");
            __builder.AddAttribute(116, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 167 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                                                                     x => UnbanIP(info.Player.IP)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddContent(117, "Unban IP");
            __builder.CloseElement();
            __builder.AddMarkupContent(118, "\r\n");
#nullable restore
#line 168 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
                            }
                        }

#line default
#line hidden
#nullable disable
            __builder.AddContent(119, "                    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(120, "\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(121, "\r\n            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(122, "\r\n");
#nullable restore
#line 173 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
        }

#line default
#line hidden
#nullable disable
            __builder.AddContent(123, "    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(124, "\r\n");
            __builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line 177 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\BanPlayerDisplayView.razor"
       
    protected string GetHoursAndMinutesOfTimespan(TimeSpan span)
    {
        string toReturn = "";
        toReturn += span.Minutes == 1 ? "1 minute" : span.Minutes + " minutes";
        if (span.TotalHours > 1)
        {
            if (Math.Round(span.TotalHours) == 1)
            {
                toReturn = "1 hour " + toReturn;
            }
            else
            {
                toReturn = Math.Round(span.TotalHours) + " hours " + toReturn;
            }
        }
        return toReturn;
    }

    protected void BanPlayer(string name)
    {
        CurrentServer.BanPlayer(name, "");
    }

    protected void UnbanPlayer(string name)
    {
        CurrentServer.UnbanPlayer(name);
    }

    protected void BanIP(string ip)
    {
        CurrentServer.BanIP(ip, "");
    }

    protected void UnbanIP(string ip)
    {
        CurrentServer.UnbanIP(ip);
    }

    protected void WhitelistPlayer(string name)
    {
        CurrentServer.WhitelistPlayer(name);
    }

    protected void UnwhitelistPlayer(string name)
    {
        CurrentServer.RemoveWhitelistPlayer(name);
    }

    protected void OpPlayer(string name)
    {
        CurrentServer.OpPlayer(name);
    }

    protected void DeopPlayer(string name)
    {
        CurrentServer.DeopPlayer(name);
    }

    protected void KickPlayer(string name)
    {
        CurrentServer.KickPlayer(name, "");
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime JSRuntime { get; set; }
    }
}
#pragma warning restore 1591
