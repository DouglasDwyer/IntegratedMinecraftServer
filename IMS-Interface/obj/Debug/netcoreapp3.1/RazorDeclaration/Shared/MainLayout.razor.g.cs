#pragma checksum "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\MainLayout.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e634f8bc8b472b666f26343b60524948ae4dadc6"
// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

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
#line 4 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\MainLayout.razor"
using IMS_Library;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\MainLayout.razor"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
    public partial class MainLayout : LayoutComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 210 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\MainLayout.razor"
       
    [Inject]
    protected NavigationManager Navigator { get; set; }

    protected Timer MainTimer;

    protected bool ShowInformationBox = false;

    protected string CurrentServerBoxText
    {
        get
        {
            return ServerSelector.CurrentServerID.ToString();
        }
        set
        {
            if (value == "NEW")
            {
                Navigator.NavigateTo("/CreateNewServer", false);
            }
            else
            {
                ServerSelector.CurrentServerID = Guid.Parse(value);
            }
        }
    }

    protected override void OnInitialized()
    {
        if (Navigator.ToBaseRelativePath(Navigator.Uri) != "Login" && !Login.HasValidLoginCookie(JSRuntime, Context))
        {
            Navigator.NavigateTo("/Login", true);
        }
        NavigateToNewServerPage();
    }

    protected void NavigateToNewServerPage()
    {
        if (IMS.Instance.ServerManager.Servers.Count == 0 && Navigator.ToBaseRelativePath(Navigator.Uri) != "CreateNewServer")
        {
            Navigator.NavigateTo("/CreateNewServer", true);
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await JSRuntime.InvokeVoidAsync("pageLoad");
        MainTimer.Enabled = true;
    }

    protected async Task LogoutAsync()
    {
        await Login.DeleteLoginCookie(JSRuntime);
        Navigator.NavigateTo("/Login", true);
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private ServerProvider ServerSelector { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private PopupProvider PopupManager { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IHttpContextAccessor Context { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime JSRuntime { get; set; }
    }
}
#pragma warning restore 1591
