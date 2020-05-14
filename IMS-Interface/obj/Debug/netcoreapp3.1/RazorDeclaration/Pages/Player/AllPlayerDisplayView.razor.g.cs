#pragma checksum "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\AllPlayerDisplayView.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "cd84229f2685430e9c906238d501425792ef3193"
// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace IMS_Interface.Pages.Player
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
#line 1 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\AllPlayerDisplayView.razor"
using IMS_Library;

#line default
#line hidden
#nullable disable
    public partial class AllPlayerDisplayView : PlayerDisplayView
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 187 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Player\AllPlayerDisplayView.razor"
       
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
        IMS.AsThreadSafe(() => CurrentServer.BanPlayer(name, ""));
    }

    protected void UnbanPlayer(string name)
    {
        IMS.AsThreadSafe(() => CurrentServer.UnbanPlayer(name));
    }

    protected void BanIP(string ip)
    {
        IMS.AsThreadSafe(() => CurrentServer.BanIP(ip, ""));
    }

    protected void UnbanIP(string ip)
    {
        IMS.AsThreadSafe(() => CurrentServer.UnbanIP(ip));
    }

    protected void WhitelistPlayer(string name)
    {
        IMS.AsThreadSafe(() => CurrentServer.WhitelistPlayer(name));
    }

    protected void UnwhitelistPlayer(string name)
    {
        IMS.AsThreadSafe(() => CurrentServer.RemoveWhitelistPlayer(name));
    }

    protected void OpPlayer(string name)
    {
        IMS.AsThreadSafe(() => CurrentServer.OpPlayer(name));
    }

    protected void DeopPlayer(string name)
    {
        IMS.AsThreadSafe(() => CurrentServer.DeopPlayer(name));
    }

    protected void KickPlayer(string name)
    {
        IMS.AsThreadSafe(() => CurrentServer.KickPlayer(name, ""));
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime JSRuntime { get; set; }
    }
}
#pragma warning restore 1591
