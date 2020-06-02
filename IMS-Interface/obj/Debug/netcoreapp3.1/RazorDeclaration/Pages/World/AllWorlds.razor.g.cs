#pragma checksum "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\World\AllWorlds.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "420c46eefc826ea3356c8a02b8062c688fda3901"
// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace IMS_Interface.Pages.World
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
#line 1 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\World\AllWorlds.razor"
using IMS_Library;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\World\AllWorlds.razor"
using System.IO;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\World\AllWorlds.razor"
using System.Threading;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\World\AllWorlds.razor"
using Blazor.FileReader;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/AllWorlds")]
    public partial class AllWorlds : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 103 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\World\AllWorlds.razor"
       
    protected static bool IsExecutingTask { get => CurrentTask != null && !CurrentTask.IsCompleted; }
    protected static Task CurrentTask;

    protected ElementReference NewWorldInput;
    protected WorldUpload CurrentWorldUpload = null;

    [CascadingParameter(Name = "CurrentServer")]
    protected Guid CurrentServerID { get; set; }
    protected int WorldSelection { get => 0; set { return; } }

    protected string GetRelativeWorldIcon(World world)
    {
        if (File.Exists(world.IconPath))
        {
            return Cache.CacheFile(world.IconPath);
        }
        else
        {
            return world.Edition == World.WorldType.Java ? "/img/grass_block.png" : "/img/bedrock.png";
        }
    }

    protected void OnCreateNewWorld()
    {
        ServerProxy server = IMS.AsThreadSafe(() => IMS.Instance.ServerManager.GetServer(CurrentServerID));
        PopupManager.ShowPopup(new CreateNewWorldPopupDisplayView.CreateNewWorldPopupDisplay(server.SupportedEdition, (name, seed, parameters) => {
            StateHasChanged();
            Task.Run(() => {
                World world = new World(Guid.NewGuid());
                world.Name = name;
                world.Edition = server.CurrentConfiguration.Edition;
                bool startServer = false;
                IMS.AsThreadSafe(() =>
                {
                    IMS.Instance.WorldManager.AddWorldToRegistry(world);
                    if(server.State != ServerProxy.ServerState.Disabled)
                    {
                        server.Stop();
                        startServer = true;
                    }
                    server.CurrentConfiguration.WorldID = world.ID;
                    if(server.CurrentConfiguration is JavaServerConfiguration)
                    {
                        (server.CurrentConfiguration as JavaServerConfiguration).LevelSeed = seed;
                        (server.CurrentConfiguration as JavaServerConfiguration).LevelType = parameters;
                    }
                });
                if (startServer)
                {
                    while(server.State != ServerProxy.ServerState.Disabled) { Thread.Sleep(1); }
                    server.Start();
                }
                InvokeAsync(() =>
                {
                    PopupManager.ClosePopup();
                    StateHasChanged();
                });
            });
        }));
    }

    protected void BeginWorldUpload()
    {
        CurrentTask = UploadWorldAsync();
    }

    protected async Task UploadWorldAsync()
    {
        await Task.Run(() =>
        {
            try
            {
                IEnumerable<IFileReference> fileUpload = FileReader.CreateReference(NewWorldInput).EnumerateFilesAsync().Result;
                IEnumerable<IFileInfo> fileInfo = fileUpload.Select(x => x.ReadFileInfoAsync().Result);
                if (fileInfo.Where(x => x.Name == "level.dat").Count() == 0)
                {
                    Console.WriteLine("Invalid world");
                }
                else
                {
                    CurrentWorldUpload = new WorldUpload();
                    foreach (IFileInfo file in fileInfo)
                    {
                        CurrentWorldUpload.TotalSize += file.Size;
                    }
                    World world = new World(Guid.NewGuid());
                    world.Name = fileInfo.First().WebkitRelativePath.Remove(fileInfo.First().WebkitRelativePath.IndexOf("/"));
                    if (fileInfo.Where(x => x.Name == "levelname.txt").Count() > 0)
                    {
                        world.Edition = World.WorldType.Bedrock;
                    }
                    else
                    {
                        world.Edition = World.WorldType.Java;
                    }
                    Directory.CreateDirectory(world.WorldPath);
                    int i = 0;
                    foreach (IFileReference entry in fileUpload)
                    {
                        IFileInfo info = fileInfo.ElementAt(i);
                        string entryLocation = world.WorldPath + "/" + info.WebkitRelativePath.Substring(info.WebkitRelativePath.IndexOf("/") + 1);
                        string entryPath = new FileInfo(entryLocation).Directory.FullName;
                        if (!Directory.Exists(entryPath))
                        {
                            Directory.CreateDirectory(entryPath);
                        }
                        using (Stream fileStream = File.Create(entryLocation))
                        using (MemoryStream memoryStream = entry.CreateMemoryStreamAsync(1024 * 1024).Result)
                        {
                            memoryStream.CopyToAsync(fileStream).Wait();
                        }
                        CurrentWorldUpload.UploadedSize += info.Size;
                        InvokeAsync(StateHasChanged);
                        i++;
                    }
                    CurrentWorldUpload = null;
                    IMS.AsThreadSafe(() => IMS.Instance.WorldManager.AddWorldToRegistry(world));
                    world.SaveConfiguration();
                    CurrentTask = null;
                    InvokeAsync(StateHasChanged);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        });
    }

    protected sealed class WorldUpload
    {
        public long TotalSize;
        public long UploadedSize;
    }

    protected void TakeActionOnWorld(string action, World world, ServerProxy mountedServer)
    {
        switch(action) {
            case "Rename":
                PopupManager.ShowPopup(new RenameWorldDisplayView.RenameWorldDisplay(world, StateHasChanged));
                break;
            case "Delete":
                PopupManager.ShowPopup(new DeleteWorldDisplayView.DeleteWorldDisplay(world, mountedServer is null ? null : mountedServer.CurrentConfiguration.ServerName, StateHasChanged));
                break;
            case "Change world icon":
                PopupManager.ShowPopup(new ChangeWorldIconDisplayView.ChangeWorldIconDisplay(world, mountedServer is null ? null : mountedServer.CurrentConfiguration.ServerName, StateHasChanged));
                break;
            case "Set as server world":
                PopupManager.ShowPopup(new ChangingWorldDisplayView.ChangingWorldDisplay(world, IMS.AsThreadSafe(() => IMS.Instance.ServerManager.GetServer(CurrentServerID)), StateHasChanged));
                break;
            default:
                throw new InvalidOperationException();
        }
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private PopupProvider PopupManager { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IFileReaderService FileReader { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private FileCache Cache { get; set; }
    }
}
#pragma warning restore 1591
