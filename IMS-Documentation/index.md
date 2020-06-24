# IntegratedMinecraftServer API Reference
---
The IntegratedMinecraftServer project seeks to provide a reliable, stable interface for managing dedicated Windows Minecraft servers.
IMS runs in the background as a Windows service, manages servers, restarts them upon crashing,
provides tools for optimization and management,and includes a remote web interface.

IMS also comes with an extensible plugin API that allows for the addition of third-party features.  This website contains guides to writing IMS plugins as well as the IMS API reference.  IMS, along with its plugin API, is written in C#.  IMS uses Blazor to render the admin console.

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
Several things are of note here.  The properties of `Name`, `Author`, and `Description` should be overwritten to properly describe the plugin.  The `Start` and `Stop` methods are called by IMS when the plugin is loaded and unloaded.  These methods will contain any code necessary for interfacing with IMS, like registering displays with `Provider`.  You should familiarize yourself with the `IMS` class and its properties, because they contain all of the core methods necessary for interfacing with servers and worlds.  To access the current instance of `IMS`, you can use the `IMSPluginBase` property `Service` or use the static property `IMS.Instance`.
#### Debugging
Debugging IMS plugins is relatively straightforward and simple.  Clicking `Debug` in Visual Studio will open an experimental instance of IMS with the debug plugin already loaded.  Breakpoints work and may be set at any point in plugin code.  Visual Studio must be run in administrator mode to debug, though, because IMS is designed to require administrator privileges.
#### How IMS manages plugins
IMS keeps track of plugins using the `IMS.Instance.PluginManager` object, which stores a registry of all plugins.  Plugins are indexed by **assembly name**, meaning each plugin must have a unique assembly name and only one plugin may belong to any plugin assembly.
### Adding new GUIs to IMS
IMS uses Blazor, a C#-based web framework, for rendering the IMS admin console.  There are two different ways to add new Blazor components to IMS.  You can use the `Provider` class to add new displays and displayviews to existing interface webpages, or have your plugin class implement `IWebpageProvider` to add completely new webpages to IMS.
#### Provider, displays, and displayviews
Multiple parts of the IMS web interface offer tabs and components that users can interact with.  These GUIs are easily extensible, and each one has a list of its "displays" in the `Provider` class.  IMS separates these component-based interfaces into two parts - displays, which contain data about the displayview, such as its name or whether it should be rendered, and displayviews, which are Blazor components that will actually be rendered as part of the IMS interface.
As an example, the code for `BooleanDisplay` is shown below.  `BooleanDisplay` is used as a part of `PreferenceEditor` instances to render a boolean toggle that allows the user to select one of two options:
``` c#
public class BooleanDisplay : PreferenceDisplay
{
    public string WhenTrue, WhenFalse;

    public BooleanDisplay(string name, string display, string description, string whenTrue, string whenFalse) : base(name, display, description)
    {
        WhenTrue = whenTrue;
        WhenFalse = whenFalse;
    }

    public override Type GetComponentType()
    {
        return typeof(BooleanDisplayView);
    }
}
```
The `BooleanDisplayView` class is a Blazor component which renders itself using the data `BooleanDisplay` provides.  This pattern allows for extensibility and ease-of-use, as multiple Blazor components may be instantiated, dynamically rendered, and have access to the same rendering data.
#### Custom webpages
Custom webpages may be implemented in IMS through the use of the `IWebpageProvider` interface.  IMS checks for plugins that have this interface when the user attempts to access a custom webpage, and loads the page accordingly.  Plugin webpages have the following special URL, relative to the admin console's base URL: `/Plugin/{PluginAssemblyName}/{PluginPage}`, where `{PluginAssemblyName}` is the name of the plugin that owns the custom webpage and `{PluginPage}` is the page name.  The implementation of this using the `IWebpageProvider` interface is as follows:
``` c#
public class Plugin : IMSPluginBase, IWebpageProvider
{
    public Dictionary<string, Type> GetPageRoutings()
    {
        Dictionary<string, Type> routings = new Dictionary<string, Type>();
        routings.Add("PageName", typeof(PageComponent));
        return routings;
    }
}
```
This code tells IMS that the Blazor component type `PageComponent` should render when the custom webpage `PageName` is accessed by a user.

---
### Further reading
This website contains the IMS API documentation, which may be used to look up information regarding the interfaces that IMS exposes.  In addition, the source code of IMS is available [on GitHub](https://github.com/DouglasDwyer/IntegratedMinecraftServer/).
