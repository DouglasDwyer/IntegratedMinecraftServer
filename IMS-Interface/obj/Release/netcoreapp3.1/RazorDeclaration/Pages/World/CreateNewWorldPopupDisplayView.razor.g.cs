#pragma checksum "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\World\CreateNewWorldPopupDisplayView.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "590e9a76cd9961437b1736aeb268d4643234a8ae"
// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

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
#line 3 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\World\CreateNewWorldPopupDisplayView.razor"
using IMS_Library;

#line default
#line hidden
#nullable disable
    public partial class CreateNewWorldPopupDisplayView : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 35 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\World\CreateNewWorldPopupDisplayView.razor"
       

    public string WorldName { get; set; }
    public string Seed { get; set; }
    public string LevelType { get; set; } = "default";

    protected void OnSelection(int selection)
    {
        LevelType = WorldOptions.ElementAt(selection).Value;
    }

    protected Dictionary<string, string> WorldOptions
    {
        get
        {
            if (DisplayData.Edition == MinecraftEdition.Java)
            {
                return new Dictionary<string, string> { { "Default", "default" }, { "Flat", "flat" }, { "Large biomes", "largeBiomes" }, { "Amplified", "amplified" }, { "Buffet", "buffet" } };
            }
            else
            {
                return new Dictionary<string, string> { { "Default", "DEFAULT" }, { "Flat", "FLAT" }, { "Legacy", "LEGACY" } };
            }
        }
    }

    [Parameter]
    public CreateNewWorldPopupDisplay DisplayData { get; set; }

    public class CreateNewWorldPopupDisplay : PopupDisplay<CreateNewWorldPopupDisplayView>
    {

        public MinecraftEdition Edition;
        public Action<string, string, string> OnUserSubmit;

        public CreateNewWorldPopupDisplay(MinecraftEdition edition, Action<string, string, string> onUserSubmit)
        {
            Edition = edition;
            OnUserSubmit = onUserSubmit;
        }
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private PopupProvider PopupManager { get; set; }
    }
}
#pragma warning restore 1591
