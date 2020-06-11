#pragma checksum "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\IMSPreferences.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "a63382d4d4ec49461a640f956abafc26832c319a"
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
#line 5 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\IMSPreferences.razor"
using IMS_Library;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.LayoutAttribute(typeof(MainLayout))]
    [Microsoft.AspNetCore.Components.RouteAttribute("/IMSPreferences")]
    public partial class IMSPreferences : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.AddMarkupContent(0, "<h3>IMS Preferences</h3>\r\n<br>\r\n");
            __builder.OpenComponent<IMS_Interface.Preferences.PreferenceEditor>(1);
            __builder.AddAttribute(2, "FieldDisplays", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Collections.Generic.List<IMS_Interface.Preferences.PreferenceDisplay>>(
#nullable restore
#line 9 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\IMSPreferences.razor"
                                 PreferenceLayout

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(3, "StartingConfiguration", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<IMS_Library.IMSConfiguration>(
#nullable restore
#line 9 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\IMSPreferences.razor"
                                                                          GetCurrentPreferences()

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(4, "OnApply", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<Microsoft.AspNetCore.Components.EventCallback<IMS_Library.IMSConfiguration>>(Microsoft.AspNetCore.Components.EventCallback.Factory.Create<IMS_Library.IMSConfiguration>(this, 
#nullable restore
#line 9 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\IMSPreferences.razor"
                                                                                                            ApplySettings

#line default
#line hidden
#nullable disable
            )));
            __builder.AddComponentReferenceCapture(5, (__value) => {
#nullable restore
#line 9 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\IMSPreferences.razor"
                                                                                                                                 Editor = (IMS_Interface.Preferences.PreferenceEditor)__value;

#line default
#line hidden
#nullable disable
            }
            );
            __builder.CloseComponent();
        }
        #pragma warning restore 1998
#nullable restore
#line 11 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\IMSPreferences.razor"
       
    public List<PreferenceDisplay> PreferenceLayout = new List<PreferenceDisplay>();
    public PreferenceEditor Editor;

    public IMSPreferences()
    {
        PreferenceLayout.Add(new PortDisplay("ManagementPort", "Management Port", "This is the port which the IMS web interface runs on."));
        PreferenceLayout.Add(new BooleanDisplay("RunIMSOnStartup", "Run on startup", "This setting determines whether IMS runs when the computer boots.", "Run", "Don't run"));
    }

    public IMSSettings GetCurrentPreferences()
    {
        return IMS.Instance.CurrentSettings;
    }

    public void ApplySettings(IMSConfiguration configuration)
    {
        Editor.ErrorText = "Settings applied successfully at " + DateTime.Now + ".";
        IMS.Instance.ChangeSettings((IMSSettings)configuration);
    }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
