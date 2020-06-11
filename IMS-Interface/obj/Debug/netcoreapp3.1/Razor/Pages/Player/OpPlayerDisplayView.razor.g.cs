#pragma checksum "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "471c724f35dd30e2cbd592493b96912794fd1d49"
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
#line 3 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
using IMS_Library;

#line default
#line hidden
#nullable disable
    public partial class OpPlayerDisplayView : PlayerDisplayView
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
#line 36 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
  
    IEnumerable<MinecraftPlayer> players = CurrentServer.GetAllOps();
    if (players.Count() == 0)
    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(1, "        ");
            __builder.AddMarkupContent(2, "<label style=\"padding-top:3px; font-weight:normal\"><i>There are currently no server operators.</i></label>\r\n");
#nullable restore
#line 41 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
        return;
    }

#line default
#line hidden
#nullable disable
            __builder.OpenElement(3, "table");
            __builder.AddAttribute(4, "class", "playertable table table-striped");
            __builder.AddMarkupContent(5, "\r\n");
#nullable restore
#line 45 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
      
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
#line 63 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
             if (renderIPs)
            {

#line default
#line hidden
#nullable disable
            __builder.AddContent(14, "                ");
            __builder.AddMarkupContent(15, "<th>\r\n                    IP\r\n                </th>\r\n");
#nullable restore
#line 68 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
            }

#line default
#line hidden
#nullable disable
            __builder.AddContent(16, "            ");
            __builder.AddMarkupContent(17, "<th>\r\n                Last seen\r\n            </th>\r\n            <th></th>\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(18, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(19, "\r\n    ");
            __builder.OpenElement(20, "tbody");
            __builder.AddMarkupContent(21, "\r\n");
#nullable restore
#line 76 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
          
            IEnumerable<MinecraftPlayer> onlinePlayers = CurrentServer.GetOnlinePlayers();
            List<BanInformation> banPlayers = renderBans ? CurrentServer.GetAllBans() : null;
            List<BanIPTag> banIPs = renderBanIPs ? CurrentServer.GetAllBannedIPs() : null;
        

#line default
#line hidden
#nullable disable
#nullable restore
#line 81 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
         foreach (MinecraftPlayer player in players)
        {

#line default
#line hidden
#nullable disable
            __builder.AddContent(22, "            ");
            __builder.OpenElement(23, "tr");
            __builder.AddMarkupContent(24, "\r\n                ");
            __builder.OpenElement(25, "td");
            __builder.AddAttribute(26, "style", "width:32px");
            __builder.AddMarkupContent(27, "\r\n                    ");
            __builder.OpenElement(28, "div");
            __builder.AddAttribute(29, "class", "playerheadicon");
            __builder.AddAttribute(30, "style", "background-image:url(" + (
#nullable restore
#line 85 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                                                                              "https://crafatar.com/avatars/" + player.UUID + "?overlay"

#line default
#line hidden
#nullable disable
            ) + ")");
            __builder.AddMarkupContent(31, "\r\n                    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(32, "\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(33, "\r\n                ");
            __builder.OpenElement(34, "th");
            __builder.AddMarkupContent(35, "\r\n                    ");
            __builder.AddContent(36, 
#nullable restore
#line 89 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                     player.Username

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(37, "\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(38, "\r\n                ");
            __builder.OpenElement(39, "td");
            __builder.AddMarkupContent(40, "\r\n                    ");
            __builder.AddContent(41, 
#nullable restore
#line 92 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                     player.UUID

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(42, "\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(43, "\r\n");
#nullable restore
#line 94 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                 if (renderIPs)
                {

#line default
#line hidden
#nullable disable
            __builder.AddContent(44, "                    ");
            __builder.OpenElement(45, "td");
            __builder.AddMarkupContent(46, "\r\n                        ");
            __builder.AddContent(47, 
#nullable restore
#line 97 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                         player.IP

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(48, "\r\n                    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(49, "\r\n");
#nullable restore
#line 99 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                }

#line default
#line hidden
#nullable disable
            __builder.AddContent(50, "                ");
            __builder.OpenElement(51, "td");
            __builder.AddMarkupContent(52, "\r\n");
#nullable restore
#line 101 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                     if (onlinePlayers.Contains(player))
                    {
                        

#line default
#line hidden
#nullable disable
            __builder.AddContent(53, 
#nullable restore
#line 103 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                          "Online for " + GetHoursAndMinutesOfTimespan(DateTime.Now - player.LastConnectionEvent)

#line default
#line hidden
#nullable disable
            );
#nullable restore
#line 103 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                                                                                                                  
                    }
                    else
                    {
                        if (player.LastConnectionEvent == default)
                        {
                            

#line default
#line hidden
#nullable disable
            __builder.AddContent(54, 
#nullable restore
#line 109 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                              "Never"

#line default
#line hidden
#nullable disable
            );
#nullable restore
#line 109 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                                      
                        }
                        else
                        {
                            

#line default
#line hidden
#nullable disable
            __builder.AddContent(55, 
#nullable restore
#line 113 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                             player.LastConnectionEvent

#line default
#line hidden
#nullable disable
            );
#nullable restore
#line 113 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                                                       
                        }
                    }

#line default
#line hidden
#nullable disable
            __builder.AddContent(56, "                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(57, "\r\n                ");
            __builder.OpenElement(58, "td");
            __builder.AddAttribute(59, "style", "text-align:right");
            __builder.AddMarkupContent(60, "\r\n                    ");
            __builder.OpenElement(61, "div");
            __builder.AddAttribute(62, "style", "display:inline");
            __builder.AddMarkupContent(63, "\r\n");
#nullable restore
#line 119 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                           bool somethingPrevious = false; 

#line default
#line hidden
#nullable disable
#nullable restore
#line 120 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                         if (renderWhitelist)
                        {
                            somethingPrevious = true;
                            

#line default
#line hidden
#nullable disable
#nullable restore
#line 123 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                             if (player.IsWhitelisted)
                            {

#line default
#line hidden
#nullable disable
            __builder.AddContent(64, "                                ");
            __builder.OpenElement(65, "button");
            __builder.AddAttribute(66, "class", "linkbutton");
            __builder.AddAttribute(67, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 125 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                                                                     x => UnwhitelistPlayer(player.Username)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddContent(68, "Unwhitelist");
            __builder.CloseElement();
            __builder.AddMarkupContent(69, "\r\n");
#nullable restore
#line 126 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                            }
                            else
                            {

#line default
#line hidden
#nullable disable
            __builder.AddContent(70, "                                ");
            __builder.OpenElement(71, "button");
            __builder.AddAttribute(72, "class", "linkbutton");
            __builder.AddAttribute(73, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 129 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                                                                     x => WhitelistPlayer(player.Username)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddContent(74, "Whitelist");
            __builder.CloseElement();
            __builder.AddMarkupContent(75, "\r\n");
#nullable restore
#line 130 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                            }

#line default
#line hidden
#nullable disable
#nullable restore
#line 130 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                             
                        }

#line default
#line hidden
#nullable disable
#nullable restore
#line 132 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                         if (renderOps)
                        {
                            if (somethingPrevious)
                            {
                                

#line default
#line hidden
#nullable disable
            __builder.AddContent(76, 
#nullable restore
#line 136 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                                  "|"

#line default
#line hidden
#nullable disable
            );
#nullable restore
#line 136 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                                      
                            }
                            somethingPrevious = true;
                            

#line default
#line hidden
#nullable disable
#nullable restore
#line 139 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                             if (player.PermissionLevel > 0)
                            {

#line default
#line hidden
#nullable disable
            __builder.AddContent(77, "                                ");
            __builder.OpenElement(78, "button");
            __builder.AddAttribute(79, "class", "linkbutton");
            __builder.AddAttribute(80, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 141 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                                                                     x => DeopPlayer(player.Username)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddContent(81, "Deop");
            __builder.CloseElement();
            __builder.AddMarkupContent(82, "\r\n");
#nullable restore
#line 142 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                            }
                            else
                            {

#line default
#line hidden
#nullable disable
            __builder.AddContent(83, "                                ");
            __builder.OpenElement(84, "button");
            __builder.AddAttribute(85, "class", "linkbutton");
            __builder.AddAttribute(86, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 145 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                                                                     x => OpPlayer(player.Username)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddContent(87, "Op");
            __builder.CloseElement();
            __builder.AddMarkupContent(88, "\r\n");
#nullable restore
#line 146 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                            }

#line default
#line hidden
#nullable disable
#nullable restore
#line 146 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                             
                        }

#line default
#line hidden
#nullable disable
#nullable restore
#line 148 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                         if (renderKicking && onlinePlayers.Contains(player))
                        {
                            if (somethingPrevious)
                            {
                                

#line default
#line hidden
#nullable disable
            __builder.AddContent(89, 
#nullable restore
#line 152 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                                  "|"

#line default
#line hidden
#nullable disable
            );
#nullable restore
#line 152 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                                      
                            }
                            somethingPrevious = true;

#line default
#line hidden
#nullable disable
            __builder.AddContent(90, "                            ");
            __builder.OpenElement(91, "button");
            __builder.AddAttribute(92, "class", "linkbutton");
            __builder.AddAttribute(93, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 155 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                                                                 x => KickPlayer(player.Username)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddContent(94, "Kick");
            __builder.CloseElement();
            __builder.AddMarkupContent(95, "\r\n");
#nullable restore
#line 156 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                        }

#line default
#line hidden
#nullable disable
#nullable restore
#line 157 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                         if (renderBans)
                        {
                            if (somethingPrevious)
                            {
                                

#line default
#line hidden
#nullable disable
            __builder.AddContent(96, 
#nullable restore
#line 161 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                                  "|"

#line default
#line hidden
#nullable disable
            );
#nullable restore
#line 161 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                                      
                            }
                            somethingPrevious = true;
                            if (banPlayers.FindIndex(x => x.Player == player) < 0)
                            {

#line default
#line hidden
#nullable disable
            __builder.AddContent(97, "                                ");
            __builder.OpenElement(98, "button");
            __builder.AddAttribute(99, "class", "linkbutton");
            __builder.AddAttribute(100, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 166 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                                                                     x => BanPlayer(player.Username)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddContent(101, "Ban");
            __builder.CloseElement();
            __builder.AddMarkupContent(102, "\r\n");
#nullable restore
#line 167 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                            }
                            else
                            {

#line default
#line hidden
#nullable disable
            __builder.AddContent(103, "                                ");
            __builder.OpenElement(104, "button");
            __builder.AddAttribute(105, "class", "linkbutton");
            __builder.AddAttribute(106, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 170 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                                                                     x => UnbanPlayer(player.Username)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddContent(107, "Unban");
            __builder.CloseElement();
            __builder.AddMarkupContent(108, "\r\n");
#nullable restore
#line 171 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                            }
                        }

#line default
#line hidden
#nullable disable
#nullable restore
#line 173 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                         if (renderBanIPs && !string.IsNullOrEmpty(player.IP))
                        {
                            if (somethingPrevious)
                            {
                                

#line default
#line hidden
#nullable disable
            __builder.AddContent(109, 
#nullable restore
#line 177 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                                  "|"

#line default
#line hidden
#nullable disable
            );
#nullable restore
#line 177 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                                      
                            }
                            somethingPrevious = true;
                            if (banIPs.FindIndex(x => x.ip == player.IP) < 0)
                            {

#line default
#line hidden
#nullable disable
            __builder.AddContent(110, "                                ");
            __builder.OpenElement(111, "button");
            __builder.AddAttribute(112, "class", "linkbutton");
            __builder.AddAttribute(113, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 182 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                                                                     x => BanIP(player.IP)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddContent(114, "Ban IP");
            __builder.CloseElement();
            __builder.AddMarkupContent(115, "\r\n");
#nullable restore
#line 183 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                            }
                            else
                            {

#line default
#line hidden
#nullable disable
            __builder.AddContent(116, "                                ");
            __builder.OpenElement(117, "button");
            __builder.AddAttribute(118, "class", "linkbutton");
            __builder.AddAttribute(119, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 186 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                                                                     x => UnbanIP(player.IP)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddContent(120, "Unban IP");
            __builder.CloseElement();
            __builder.AddMarkupContent(121, "\r\n");
#nullable restore
#line 187 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
                            }
                        }

#line default
#line hidden
#nullable disable
            __builder.AddContent(122, "                    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(123, "\r\n                ");
            __builder.CloseElement();
            __builder.AddMarkupContent(124, "\r\n            ");
            __builder.CloseElement();
            __builder.AddMarkupContent(125, "\r\n");
#nullable restore
#line 192 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
        }

#line default
#line hidden
#nullable disable
            __builder.AddContent(126, "    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(127, "\r\n");
            __builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line 196 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\OpPlayerDisplayView.razor"
       
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
