#pragma checksum "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\IMSManagement.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f2d6cd4f20f17de68fd166b6043508a9a7e8b516"
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
#line 3 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\IMSManagement.razor"
using System.Reflection;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\IMSManagement.razor"
using IMS_Library;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/IMSManagement")]
    public partial class IMSManagement : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 39 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\IMSManagement.razor"
       
    protected void AttemptIMSUpdate()
    {
        PopupManager.ShowPopup(
            new InformationPopupDisplay(
                "There is a new update for IMS that is ready to install.  Would you like to restart IMS now?",
                "Update IMS",
                x =>
                {
                    if (x == 0)
                    {
                        IMS.Instance.UpdateManager.UpdateAndRestart();
                    }
                },
                "Yes",
                "No"
        ));
    }

    protected void StopIMS()
    {
        PopupManager.ShowPopup(
            new InformationPopupDisplay(
                "Are you certain you would like to shut down IMS?  This will stop any running servers.",
                "Shut down",
                x =>
                {
                    if (x == 0)
                    {
                        IMS.Instance.Stop();
                    }
                },
                "Yes",
                "No"
        ));
    }

    protected void RestartIMS()
    {
        PopupManager.ShowPopup(
            new InformationPopupDisplay(
                "Are you certain you would like to restart IMS?  This will restart any running servers.",
                "Restart",
                x =>
                {
                    if (x == 0)
                    {
                        IMS.Instance.Restart();
                    }
                },
                "Yes",
                "No"
        ));
    }

    protected async void CheckForUpdates()
    {
        PopupManager.ShowPopup(new LoadingPopupDisplay("Checking for new updates..."));
        await IMS.Instance.UpdateManager.DownloadUpdatesAsync();
        if (IMS.Instance.UpdateManager.UpdatesReadyForInstallation)
        {
            AttemptIMSUpdate();
        }
        else
        {
            PopupManager.ShowPopup(new InformationPopupDisplay("No new updates were found for IMS.", "IMS up-to-date", x => { }));
        }
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private PopupProvider PopupManager { get; set; }
    }
}
#pragma warning restore 1591
