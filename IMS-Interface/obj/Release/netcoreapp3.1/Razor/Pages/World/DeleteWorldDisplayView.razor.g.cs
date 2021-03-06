#pragma checksum "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\World\DeleteWorldDisplayView.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7fa8eada1a467472a8dba019c1a65dba80258f0b"
// <auto-generated/>
#pragma warning disable 1591
namespace IMS_Interface.World
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
#line 3 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\World\DeleteWorldDisplayView.razor"
using IMS_Library;

#line default
#line hidden
#nullable disable
    public partial class DeleteWorldDisplayView : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.AddMarkupContent(0, "<style>\r\n    label {\r\n        font-weight: normal;\r\n    }\r\n</style>\r\n");
            __builder.AddMarkupContent(1, "<label><b>Delete world?</b></label>\r\n<br>\r\n");
            __builder.AddMarkupContent(2, "<label>This cannot be undone.</label>\r\n");
#nullable restore
#line 15 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\World\DeleteWorldDisplayView.razor"
 if (DisplayData.CurrentServerName != null)
{

#line default
#line hidden
#nullable disable
            __builder.AddContent(3, "    ");
            __builder.OpenElement(4, "label");
            __builder.AddContent(5, "This will also cause ");
            __builder.AddContent(6, 
#nullable restore
#line 17 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\World\DeleteWorldDisplayView.razor"
                                 DisplayData.CurrentServerName

#line default
#line hidden
#nullable disable
            );
            __builder.AddContent(7, " to generate a new world.");
            __builder.CloseElement();
            __builder.AddMarkupContent(8, "\r\n");
#nullable restore
#line 18 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\World\DeleteWorldDisplayView.razor"
}

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(9, "<br>\r\n<br>\r\n<label></label>\r\n");
            __builder.OpenElement(10, "div");
            __builder.AddAttribute(11, "style", "position:absolute;right:10px;bottom:10px;");
            __builder.AddMarkupContent(12, "\r\n    ");
            __builder.OpenElement(13, "button");
            __builder.AddAttribute(14, "class", "btn btn-danger");
            __builder.AddAttribute(15, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 23 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\World\DeleteWorldDisplayView.razor"
                                             Delete

#line default
#line hidden
#nullable disable
            ));
            __builder.AddContent(16, "Delete");
            __builder.CloseElement();
            __builder.AddMarkupContent(17, "\r\n    ");
            __builder.OpenElement(18, "button");
            __builder.AddAttribute(19, "class", "btn btn-default");
            __builder.AddAttribute(20, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 24 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\World\DeleteWorldDisplayView.razor"
                                              x => PopupManager.ClosePopup()

#line default
#line hidden
#nullable disable
            ));
            __builder.AddContent(21, "Cancel");
            __builder.CloseElement();
            __builder.AddMarkupContent(22, "\r\n");
            __builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line 27 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\World\DeleteWorldDisplayView.razor"
       

    [Parameter]
    public DeleteWorldDisplay DisplayData { get; set; }

    protected async void Delete()
    {
        PopupManager.ShowPopup(new LoadingPopupDisplay("Deleting world..."));
        ServerProxy server = IMS.Instance.WorldManager.GetServerOfWorld(DisplayData.CurrentWorld);
        if (server != null)
        {
            if (server.State == ServerProxy.ServerState.Disabled)
            {
                server = null;
            }
            else
            {
                await server.StopAsync();
                server.CurrentConfiguration.WorldID = Guid.Empty;
                if (server.CurrentConfiguration is JavaServerConfiguration config)
                {
                    config.LevelSeed = "";
                    config.LevelType = "default";
                }
            }
        }
        await IMS.Instance.WorldManager.DeleteWorldAsync(DisplayData.CurrentWorld);
        if (server != null)
        {
            server.StartAsync();
        }
        PopupManager.ClosePopup();
        DisplayData.OnDeleteOccur?.Invoke();
    }

    public class DeleteWorldDisplay : PopupDisplay<DeleteWorldDisplayView>
    {
        public World CurrentWorld;
        public string CurrentServerName;
        public Action OnDeleteOccur;

        public DeleteWorldDisplay(World world, string server, Action delete)
        {
            CurrentWorld = world;
            CurrentServerName = server;
            OnDeleteOccur = delete;
        }
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private PopupProvider PopupManager { get; set; }
    }
}
#pragma warning restore 1591
