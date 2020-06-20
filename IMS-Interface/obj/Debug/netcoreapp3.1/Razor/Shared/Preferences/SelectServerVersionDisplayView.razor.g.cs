#pragma checksum "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\SelectServerVersionDisplayView.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "13daed6fc5ba21f7a52d835e96722753cf91c2ba"
// <auto-generated/>
#pragma warning disable 1591
namespace IMS_Interface.Preferences
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
#line 3 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\SelectServerVersionDisplayView.razor"
using IMS_Library;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\SelectServerVersionDisplayView.razor"
using System.Reflection;

#line default
#line hidden
#nullable disable
    public partial class SelectServerVersionDisplayView : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.AddMarkupContent(0, "<style>\r\n    label {\r\n        font-weight: normal;\r\n    }\r\n</style>\r\n");
            __builder.OpenElement(1, "label");
            __builder.AddAttribute(2, "style", "width:300px;");
            __builder.OpenElement(3, "b");
            __builder.AddContent(4, "Select ");
            __builder.AddContent(5, 
#nullable restore
#line 13 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\SelectServerVersionDisplayView.razor"
                                       DisplayData.Edition

#line default
#line hidden
#nullable disable
            );
            __builder.AddContent(6, " server version");
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(7, "\r\n<label></label>\r\n");
#nullable restore
#line 15 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\SelectServerVersionDisplayView.razor"
  
    Information = IMS.Instance.VersionManager.AvailableServerVersions.Values
        .Where(x => x.Edition == DisplayData.Edition && (ShowSnapshots || x.VersionType != ServerVersionInformation.ReleaseType.Snapshot))
        .OrderByDescending(x => x.ReleaseTime);

#line default
#line hidden
#nullable disable
            __builder.OpenElement(8, "select");
            __builder.AddAttribute(9, "disabled", 
#nullable restore
#line 20 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\SelectServerVersionDisplayView.razor"
                   IsWorking

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(10, "size", "8");
            __builder.AddAttribute(11, "style", "display:inherit;width:100%");
            __builder.AddAttribute(12, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.ChangeEventArgs>(this, 
#nullable restore
#line 20 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\SelectServerVersionDisplayView.razor"
                                                                                     x => SelectedVersion = Information.ElementAt(int.Parse((string)x.Value))

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(13, "\r\n");
#nullable restore
#line 21 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\SelectServerVersionDisplayView.razor"
     for (int x = 0; x < Information.Count(); x++)
    {
        int i = x;
        ServerVersionInformation info = Information.ElementAt(i);

#line default
#line hidden
#nullable disable
            __builder.AddContent(14, "        ");
            __builder.OpenElement(15, "option");
            __builder.AddAttribute(16, "value", 
#nullable restore
#line 25 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\SelectServerVersionDisplayView.razor"
                        i

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(17, "selected", 
#nullable restore
#line 25 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\SelectServerVersionDisplayView.razor"
                                       info == SelectedVersion

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(18, "\r\n            ");
            __builder.AddContent(19, 
#nullable restore
#line 26 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\SelectServerVersionDisplayView.razor"
             info.Name

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(20, "\r\n");
#nullable restore
#line 27 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\SelectServerVersionDisplayView.razor"
             if (info.PhysicalLocation != null)
            {
                

#line default
#line hidden
#nullable disable
            __builder.AddContent(21, 
#nullable restore
#line 29 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\SelectServerVersionDisplayView.razor"
                  "\U0001F4BE"

#line default
#line hidden
#nullable disable
            );
#nullable restore
#line 29 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\SelectServerVersionDisplayView.razor"
                               
            }

#line default
#line hidden
#nullable disable
            __builder.AddContent(22, "        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(23, "\r\n");
#nullable restore
#line 32 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\SelectServerVersionDisplayView.razor"
    }

#line default
#line hidden
#nullable disable
            __builder.CloseElement();
            __builder.AddMarkupContent(24, "\r\n");
            __builder.OpenElement(25, "input");
            __builder.AddAttribute(26, "id", "showsnapshotscheckbox");
            __builder.AddAttribute(27, "type", "checkbox");
            __builder.AddAttribute(28, "checked", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 34 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\SelectServerVersionDisplayView.razor"
                                                                 ShowSnapshots

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(29, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => ShowSnapshots = __value, ShowSnapshots));
            __builder.SetUpdatesAttributeName("checked");
            __builder.CloseElement();
            __builder.AddMarkupContent(30, "\r\n");
            __builder.AddMarkupContent(31, "<label for=\"showsnapshotscheckbox\">Show snapshots</label>\r\n<br>\r\n<br>\r\n<label></label>\r\n");
            __builder.OpenElement(32, "div");
            __builder.AddAttribute(33, "style", "position:absolute;right:10px;bottom:10px;");
            __builder.AddMarkupContent(34, "\r\n    ");
            __builder.OpenElement(35, "button");
            __builder.AddAttribute(36, "class", "btn btn-primary");
            __builder.AddAttribute(37, "disabled", 
#nullable restore
#line 40 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\SelectServerVersionDisplayView.razor"
                                               IsWorking

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(38, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 40 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\SelectServerVersionDisplayView.razor"
                                                                    x => BeginVersionDownloadAndSet(SelectedVersion.Version)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddMarkupContent(39, "\r\n");
#nullable restore
#line 41 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\SelectServerVersionDisplayView.razor"
         if (IsWorking)
        {
            

#line default
#line hidden
#nullable disable
            __builder.AddContent(40, 
#nullable restore
#line 43 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\SelectServerVersionDisplayView.razor"
              "Downloading..."

#line default
#line hidden
#nullable disable
            );
#nullable restore
#line 43 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\SelectServerVersionDisplayView.razor"
                               
        }
        else
        {
            if (SelectedVersion.PhysicalLocation is null)
            {
                

#line default
#line hidden
#nullable disable
            __builder.AddContent(41, 
#nullable restore
#line 49 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\SelectServerVersionDisplayView.razor"
                  "Select/download"

#line default
#line hidden
#nullable disable
            );
#nullable restore
#line 49 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\SelectServerVersionDisplayView.razor"
                                    
            }
            else
            {
                

#line default
#line hidden
#nullable disable
            __builder.AddContent(42, 
#nullable restore
#line 53 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\SelectServerVersionDisplayView.razor"
                  "Select"

#line default
#line hidden
#nullable disable
            );
#nullable restore
#line 53 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\SelectServerVersionDisplayView.razor"
                           
            }
        }

#line default
#line hidden
#nullable disable
            __builder.AddContent(43, "    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(44, "\r\n    ");
            __builder.OpenElement(45, "button");
            __builder.AddAttribute(46, "class", "btn btn-default");
            __builder.AddAttribute(47, "disabled", 
#nullable restore
#line 57 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\SelectServerVersionDisplayView.razor"
                                               IsWorking

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(48, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 57 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\SelectServerVersionDisplayView.razor"
                                                                    x => BeginVersionDownloadAndSet(null)

#line default
#line hidden
#nullable disable
            ));
            __builder.AddContent(49, "Use latest");
            __builder.CloseElement();
            __builder.AddMarkupContent(50, "\r\n    ");
            __builder.OpenElement(51, "button");
            __builder.AddAttribute(52, "class", "btn btn-default");
            __builder.AddAttribute(53, "disabled", 
#nullable restore
#line 58 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\SelectServerVersionDisplayView.razor"
                                               IsWorking

#line default
#line hidden
#nullable disable
            );
            __builder.AddAttribute(54, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 58 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\SelectServerVersionDisplayView.razor"
                                                                    x => PopupManager.ClosePopup()

#line default
#line hidden
#nullable disable
            ));
            __builder.AddContent(55, "Cancel");
            __builder.CloseElement();
            __builder.AddMarkupContent(56, "\r\n");
            __builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line 61 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\SelectServerVersionDisplayView.razor"
       

    [Parameter]
    public SelectServerVersionDisplay DisplayData { get; set; }

    protected bool IsWorking => CurrentTask != null;
    protected Task CurrentTask = null;

    protected IOrderedEnumerable<ServerVersionInformation> Information;
    protected ServerVersionInformation SelectedVersion;
    protected bool ShowSnapshots { get => Snapshots; set { Snapshots = value; EnsureSnapshotNotSelected(); } }
    private bool Snapshots;

    protected override void OnInitialized()
    {
        ShowSnapshots = !string.IsNullOrEmpty(DisplayData.CurrentServerVersion) && IMS.Instance.VersionManager.AvailableServerVersions[DisplayData.CurrentServerVersion].VersionType == ServerVersionInformation.ReleaseType.Snapshot;
        SelectedVersion = IMS.Instance.VersionManager.GetVersionInformationFromID(DisplayData.CurrentServerVersion);
    }

    protected void EnsureSnapshotNotSelected()
    {
        if (!ShowSnapshots && SelectedVersion != null && SelectedVersion.VersionType == ServerVersionInformation.ReleaseType.Snapshot)
        {
            SelectedVersion = IMS.Instance.VersionManager.GetVersionInformationFromID(DisplayData.CurrentServerVersion);
            StateHasChanged();
        }
    }

    protected bool IsVersionDownloaded(string version)
    {
        if (string.IsNullOrEmpty(version))
        {
            return IMS.Instance.VersionManager.LatestRelease.PhysicalLocation != null;
        }
        else
        {
            return IMS.Instance.VersionManager.AvailableServerVersions[version].PhysicalLocation != null;
        }
    }

    protected void BeginVersionDownloadAndSet(string version)
    {
        CurrentTask = DownloadVersionAndSetAsync(version);
    }

    protected async Task DownloadVersionAndSetAsync(string version)
    {
        ServerVersionInformation information = IMS.Instance.VersionManager.AvailableServerVersions.ContainsKey(version ?? "") ? IMS.Instance.VersionManager.AvailableServerVersions[version] : IMS.Instance.VersionManager.LatestRelease;
        if (information.PhysicalLocation is null)
        {
            try
            {
                await information.DownloadServerBinaryAsync();
            }
            catch (Exception e)
            {
                Logger.WriteWarning("Unable to download server binary for " + SelectedVersion.Name + "!\n" + e);
                PopupManager.ShowPopup(new InformationPopupDisplay("IMS was unable to download the server binary.  Check your internet connection or consult the IMS log for more details.", "Downloading error", null));
                return;
            }
        }
        SetServerVersion(version);
        PopupManager.ClosePopup();
        DisplayData.OnUserFinish?.Invoke();
    }

    protected void SetServerVersion(string version)
    {
        DisplayData.FieldToChange.SetValue(DisplayData.Configuration, version);
    }

    public class SelectServerVersionDisplay : PopupDisplay<SelectServerVersionDisplayView>
    {
        public string CurrentServerVersion;
        public MinecraftEdition Edition;
        public Action OnUserFinish;
        public IMSConfiguration Configuration;
        public FieldInfo FieldToChange;

        public SelectServerVersionDisplay(string serverVersion, MinecraftEdition edition, IMSConfiguration configuration, FieldInfo fieldToChange, Action finish)
        {
            CurrentServerVersion = serverVersion;
            Edition = edition;
            OnUserFinish = finish;
            Configuration = configuration;
            FieldToChange = fieldToChange;
        }
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private PopupProvider PopupManager { get; set; }
    }
}
#pragma warning restore 1591