#pragma checksum "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\ReleaseNotes.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f06a38d09e8a298b2e20a8f2e7289c4582863c53"
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
#line 3 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\ReleaseNotes.razor"
using System.Reflection;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\ReleaseNotes.razor"
using IMS_Library;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/ReleaseNotes")]
    public partial class ReleaseNotes : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.AddMarkupContent(0, "<style>\r\n    .header-label {\r\n        font-weight: normal;\r\n        margin-top: 5px;\r\n    }\r\n</style>\r\n\r\n");
            __builder.AddMarkupContent(1, "<h3>Release Notes</h3>\r\n");
            __builder.OpenElement(2, "label");
            __builder.AddAttribute(3, "style", "font-weight:normal");
            __builder.OpenElement(4, "i");
            __builder.AddContent(5, "IMS v");
            __builder.AddContent(6, 
#nullable restore
#line 16 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\ReleaseNotes.razor"
                                            Assembly.GetAssembly(typeof(IMS)).GetName().Version

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.CloseElement();
            __builder.AddMarkupContent(7, "\r\n<br>\r\n<br>\r\n");
            __builder.AddMarkupContent(8, "<div style=\"max-width:500px\"><label class=\"header-label\"><b>Maintenance</b></label>\r\n    <ul><li>IMS updates are now downloaded from Git Large File Storage, allowing IMS distributions to be over 100 MB large.</li></ul></div>");
        }
        #pragma warning restore 1998
    }
}
#pragma warning restore 1591
