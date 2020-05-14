#pragma checksum "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Timer.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f676967a88006a98f3185a831102485164192c73"
// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace IMS_Interface.Shared
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
#line 1 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Timer.razor"
using System.Reflection;

#line default
#line hidden
#nullable disable
    public partial class Timer : Microsoft.AspNetCore.Components.ComponentBase, IDisposable
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 5 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Shared\Timer.razor"
       
    [Parameter]
    public double Interval {
        get {
            return InternalTimer.Interval;
        }
        set
        {
            InternalTimer.Interval = value;
        }
    }

    [Parameter]
    public ComponentBase UpdateScope { get; set; }

    public bool Enabled
    {
        get => InternalTimer.Enabled;
        set => InternalTimer.Enabled = value;
    }

    [Parameter]
    public Action Tick { get; set; }

    [Parameter]
    public bool UseSynchronousTick { get; set; } = true;
    [Parameter]
    public bool UpdateComponentOnTick { get; set; } = true;

    private System.Timers.ElapsedEventHandler tickAction;

    private System.Timers.Timer InternalTimer = new System.Timers.Timer();

    public Timer()
    {
        tickAction = (x, y) =>
        {
            if (UseSynchronousTick)
            {
                InvokeAsync(() =>
                {
                    Tick?.Invoke();
                    if (UpdateComponentOnTick)
                    {
                        MethodInfo updater = UpdateScope.GetType().GetMethod("StateHasChanged", BindingFlags.NonPublic | BindingFlags.Instance);
                        updater.Invoke(UpdateScope, new object[0]);
                    }
                });
            }
            else
            {
                Tick?.Invoke();
                if (UpdateComponentOnTick)
                {
                    MethodInfo updater = UpdateScope.GetType().GetMethod("StateHasChanged", BindingFlags.NonPublic | BindingFlags.Instance);
                    InvokeAsync(() => { updater.Invoke(UpdateScope, new object[0]); });
                }
            }
        };
        InternalTimer.Elapsed += tickAction;
    }

    public void Dispose()
    {
        Enabled = false;
    }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
