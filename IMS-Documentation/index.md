# IntegratedMinecraftServer API Reference
---
The IntegratedMinecraftServer project seeks to provide a reliable, stable interface for managing dedicated Windows Minecraft servers.
IMS runs in the background as a Windows service, manages servers, restarts them upon crashing,
provides tools for optimization and management,and includes a remote web interface.

IMS also comes with an extensible C# plugin API that allows for the addition of third-party features.  This website contains guides to writing IMS plugins as well as the IMS API reference.

---
### Getting started with the IMS Development Kit
#### Downloading the IMSDK
In order to create plugins for IMS, it is necessary to download and install the IMS Development Kit (IMSDK).  The IMSDK is a Visual Studio Extension which comes with the IMS Plugin project template in addition to an experimental instance of IMS for testing.  The IMSDK can be installed by opening Visual Studio, going to `Extensions > Manage Extensions`, and then searching for "IMS Development Kit" on the Visual Studio Marketplace.  Alternatively, the IMSDK can be downloaded directly using [this link](https://marketplace.visualstudio.com/items?itemName=DouglasDwyer.IMSDevelopmentKit).  In order to properly use the IMSDK, it is also necessary to have support for .NET Core and Razor Web Development installed with Visual Studio.  If you do not have them, they can be installed by opening the Visual Studio Installer and selecting `Modify`.
#### Creating your first plugin
Once the IMSDK has been installed, the `IMS Plugin` project template will be available in the `Create New Project` menu.  Upon creation of a new IMS plugin, you will be presented with the following class:
``` c#
public class Plugin : IMSPluginBase
{
    public override string Name => "New plugin";

    public override string Author => "Anonymous";

    public override string Description => "A new plugin.";

    public override void Start()
    {
            
    }

    public override void Stop()
    {
            
    }
}
```
Several things are of note here.  The properties of `Name`, `Author`, and `Description` should be overwritten to properly describe the plugin.  The `Start` and `Stop` methods are called by IMS when the plugin is loaded and unloaded.  These methods will contain any code necessary for interfacing with IMS, like registering displays with `Provider`.  You should familiarize yourself with the `IMS` class its properties, because they contain all of the core methods necessary for interfacing with servers and worlds.  To access the current instance of `IMS`, you can use the `IMSPluginBase` property `Service` or use the static property `IMS.Instance`.
#### Debugging
Debugging IMS plugins is relatively straightforward and simple.  Clicking `Debug` in Visual Studio will open an experimental instance of IMS with the debug plugin already loaded.  Breakpoints work and may be set at any point in plugin code.  Visual Studio must be run in administrator mode to debug, though, because IMS is designed to require administrator privileges.
#### How IMS manages plugins
IMS keeps track of plugins using the `IMS.Instance.PluginManager` object, which stores a registry of all plugins.  Plugins are indexed by **assembly name**, meaning each plugin must have a unique assembly name and only one plugin may belong to any plugin assembly.
