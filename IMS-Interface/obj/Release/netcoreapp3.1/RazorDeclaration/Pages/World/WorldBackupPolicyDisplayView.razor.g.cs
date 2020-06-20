#pragma checksum "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\World\WorldBackupPolicyDisplayView.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "02628d2400928f0b03b1edb98a54f38a01efe8f9"
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
#line 3 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\World\WorldBackupPolicyDisplayView.razor"
using IMS_Library;

#line default
#line hidden
#nullable disable
    public partial class WorldBackupPolicyDisplayView : WorldDisplayView, IDisposable
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 70 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\World\WorldBackupPolicyDisplayView.razor"
       
    protected int AddPolicyDropdownValue { get => 0; set => OnUserAddPolicy(value); }

    static WorldBackupPolicyDisplayView()
    {
        Provider.BackupPolicyDisplayBinding[typeof(BackupAfterTimeIntervalPolicy)] = new BackupPolicyDisplay("Backup after time interval...", typeof(BackupAfterTimeIntervalPolicyDisplayView));
        Provider.BackupPolicyDisplayBinding[typeof(RemoveBackupAfterTimeIntervalPolicy)] = new BackupPolicyDisplay("Delete backups based on date...", typeof(RemoveBackupAfterTimeIntervalPolicyDisplayView));
    }

    protected void OnUserAddPolicy(int selectionIndex)
    {
        IBackupPolicy policy = (IBackupPolicy)Activator.CreateInstance(Provider.BackupPolicyDisplayBinding.Keys.ElementAt(selectionIndex - 1));
        CurrentWorld.BackupPolicies.Add(policy);
    }

    protected void DeleteBackupPolicy(IBackupPolicy policy)
    {
        CurrentWorld.BackupPolicies.Remove(policy);
    }

    public void Dispose()
    {
        CurrentWorld.SaveConfiguration();
    }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591