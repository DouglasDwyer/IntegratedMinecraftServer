#pragma checksum "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PreferenceEditor.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "cdc62a4648c6da0b3a054973087659e648a07780"
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
#line 3 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PreferenceEditor.razor"
using IMS_Library;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PreferenceEditor.razor"
using System.Reflection;

#line default
#line hidden
#nullable disable
    public partial class PreferenceEditor : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.AddMarkupContent(0, "<style>\r\n    label {\r\n        font-weight: 100;\r\n        margin-bottom: 0;\r\n    }\r\n</style>\r\n\r\n");
#nullable restore
#line 13 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PreferenceEditor.razor"
 foreach (PreferenceDisplay display in FieldDisplays)
{
    display.ParentDisplay = this;
    RenderFragment fragment = builder =>
    {
        builder.OpenComponent(0, display.GetComponentType());
        builder.AddAttribute(0, "DisplayData", display);
        builder.CloseComponent();
    };
    

#line default
#line hidden
#nullable disable
            __builder.AddContent(1, 
#nullable restore
#line 22 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PreferenceEditor.razor"
     fragment

#line default
#line hidden
#nullable disable
            );
#nullable restore
#line 22 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PreferenceEditor.razor"
             
    if (!(display is ConditionalDisplay))
    {

#line default
#line hidden
#nullable disable
            __builder.AddMarkupContent(2, "        <br>\r\n");
#nullable restore
#line 26 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PreferenceEditor.razor"
    }
}

#line default
#line hidden
#nullable disable
            __builder.OpenElement(3, "div");
            __builder.AddAttribute(4, "style", "padding: 6px;background-color: white;border-radius: 3px;float:right;position: sticky;bottom: 20px;z-index: 20;border: 1px solid #ddd;");
            __builder.AddMarkupContent(5, "\r\n");
#nullable restore
#line 29 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PreferenceEditor.razor"
     if (!string.IsNullOrEmpty(ErrorText))
    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(6, "        ");
            __builder.OpenElement(7, "label");
            __builder.AddAttribute(8, "class", "alert alert-success");
            __builder.AddAttribute(9, "style", "padding:6px; margin-right:8px; margin-bottom:unset");
            __builder.AddContent(10, 
#nullable restore
#line 31 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PreferenceEditor.razor"
                                                                                                       ErrorText

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(11, "\r\n");
#nullable restore
#line 32 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PreferenceEditor.razor"
    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 33 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PreferenceEditor.razor"
     if (!string.IsNullOrEmpty(ApplyText))
    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(12, "        ");
            __builder.OpenElement(13, "button");
            __builder.AddAttribute(14, "class", "btn btn-success");
            __builder.AddAttribute(15, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 35 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PreferenceEditor.razor"
                                                  Apply

#line default
#line hidden
#nullable disable
            ));
            __builder.AddContent(16, 
#nullable restore
#line 35 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PreferenceEditor.razor"
                                                          ApplyText

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(17, "\r\n");
#nullable restore
#line 36 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PreferenceEditor.razor"
    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 37 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PreferenceEditor.razor"
     if (!string.IsNullOrEmpty(RevertText))
    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(18, "        ");
            __builder.OpenElement(19, "button");
            __builder.AddAttribute(20, "class", "btn btn-warning");
            __builder.AddAttribute(21, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 39 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PreferenceEditor.razor"
                                                  Revert

#line default
#line hidden
#nullable disable
            ));
            __builder.AddContent(22, 
#nullable restore
#line 39 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PreferenceEditor.razor"
                                                           RevertText

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(23, "\r\n");
#nullable restore
#line 40 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PreferenceEditor.razor"
    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 41 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PreferenceEditor.razor"
     if (!string.IsNullOrEmpty(ResetText))
    {

#line default
#line hidden
#nullable disable
            __builder.AddContent(24, "        ");
            __builder.OpenElement(25, "button");
            __builder.AddAttribute(26, "class", "btn btn-danger");
            __builder.AddAttribute(27, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 43 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PreferenceEditor.razor"
                                                 Reset

#line default
#line hidden
#nullable disable
            ));
            __builder.AddContent(28, 
#nullable restore
#line 43 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PreferenceEditor.razor"
                                                         ResetText

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(29, "\r\n");
#nullable restore
#line 44 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PreferenceEditor.razor"
    }

#line default
#line hidden
#nullable disable
            __builder.CloseElement();
            __builder.AddMarkupContent(30, "\r\n<br>\r\n<br>");
        }
        #pragma warning restore 1998
#nullable restore
#line 49 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Preferences\PreferenceEditor.razor"
       

    [Parameter]
    public string ApplyText { get; set; } = "Apply";
    [Parameter]
    public string RevertText { get; set; } = "Revert";
    [Parameter]
    public string ResetText { get; set; } = "Reset to Default";

    public string ErrorText;

    [Parameter]
    public IMSConfiguration StartingConfiguration
    {
        set
        {
            CurrentConfiguration = value;
        }
    }

    public IMSConfiguration CurrentConfiguration
    {
        get
        {
            return (IMSConfiguration)LoadedConfiguration;
        }
        set
        {
            if (value is null)
            {
                LoadedConfiguration = InitialConfiguration = null;
            }
            else
            {
                LoadedConfiguration = (IMSConfiguration)value.Clone();
                InitialConfiguration = (IMSConfiguration)value.Clone();
            }
        }
    }
    [Parameter]
    public List<PreferenceDisplay> FieldDisplays { get; set; }
    [Parameter]
    public EventCallback<IMSConfiguration> OnApply { get; set; }
    [Parameter]
    public EventCallback<IMSConfiguration> OnRevert { get; set; }
    [Parameter]
    public EventCallback<IMSConfiguration> OnReset { get; set; }

    private IMSConfiguration LoadedConfiguration;
    private IMSConfiguration InitialConfiguration;

    public void NotifyStateChanged()
    {
        ErrorText = null;
        StateHasChanged();
    }

    public void NotifyStateChanged(bool maintainErrorText)
    {
        StateHasChanged();
    }

    protected async Task Apply()
    {
        InitialConfiguration = (IMSConfiguration)LoadedConfiguration.Clone();
        await OnApply.InvokeAsync((IMSConfiguration)CurrentConfiguration.Clone());
    }

    protected async Task Revert()
    {
        ErrorText = null;
        await OnRevert.InvokeAsync(CurrentConfiguration);
        CurrentConfiguration = InitialConfiguration;
    }

    protected async Task Reset()
    {
        ErrorText = null;
        await OnReset.InvokeAsync(CurrentConfiguration);
        LoadedConfiguration = (IMSConfiguration)Activator.CreateInstance(CurrentConfiguration.GetType());
    }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591