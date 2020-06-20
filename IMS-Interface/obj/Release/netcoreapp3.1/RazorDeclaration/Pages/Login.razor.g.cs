#pragma checksum "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Login.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b3152e57a3e0b86928191e26978829cfdfb157e4"
// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace IMS_Interface
{
    #line hidden
    using System.Collections.Generic;
    using System.Linq;
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
#line 6 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Login.razor"
using System;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Login.razor"
using IMS_Library;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Login.razor"
using System.Text;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Login.razor"
using System.Threading.Tasks;

#line default
#line hidden
#nullable disable
#nullable restore
#line 10 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Login.razor"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.LayoutAttribute(typeof(EmptyLayout))]
    [Microsoft.AspNetCore.Components.RouteAttribute("/Login")]
    public partial class Login : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 36 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Login.razor"
       
    [Inject]
    protected NavigationManager Navigator { get; set; }

    public string ErrorText;

    public string Username = "";
    public string Password = "";

    private static string RealUsername => IMS.Instance.CurrentSettings.Username;
    private static byte[] RealPassword => IMS.Instance.CurrentSettings.PasswordHash;

    public static ILocalCredentialsResetter CredentialsResetter;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (HasValidLoginCookie(JSRuntime, Context))
        {
            Navigator.NavigateTo("/", true);
        }
    }

    public static bool IsLoginRequired => !(RealUsername is null && RealPassword is null);

    public async Task AttemptLoginAsync()
    {
        if (!IsLoginRequired)
        {
            Navigator.NavigateTo("/");
        }
        if (Username == RealUsername && Enumerable.SequenceEqual(Encryption.HashBytes(Encoding.UTF8.GetBytes(Password)), RealPassword))
        {
            await WriteLoginCookie(JSRuntime, Context);
            Navigator.NavigateTo("/");
        }
        else
        {
            ErrorText = "Incorrect username or password.";
        }
    }

    public void RequestLoginReset()
    {
        if (CredentialsResetter is null)
        {
            ErrorText = "Couldn't make a local credential reset request.";
        }
        else
        {
            CredentialsResetter.ResetCredentialsAsync();
            ErrorText = "A reset request was made on the host computer.";
        }
    }

    public static bool HasValidLoginCookie(IJSRuntime runtime, IHttpContextAccessor context)
    {
        if (RealUsername is null && RealPassword is null)
        {
            return true;
        }
        string toCompare = ByteArrayToString(Encryption.HashBytes(Concat(Encoding.ASCII.GetBytes(RealUsername + context.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString()), RealPassword)));
        return toCompare == context.HttpContext.Request.Cookies["LoginKey"];
    }

    public static async Task WriteLoginCookie(IJSRuntime runtime, IHttpContextAccessor context)
    {
        await runtime.InvokeVoidAsync("setCookie", "LoginKey", ByteArrayToString(Encryption.HashBytes(Concat(Encoding.ASCII.GetBytes(RealUsername + context.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString()), RealPassword))), 36500);
    }

    public static async Task DeleteLoginCookie(IJSRuntime runtime)
    {
        await runtime.InvokeVoidAsync("deleteCookie", "LoginKey");
    }

    private static string ByteArrayToString(byte[] ba)
    {
        StringBuilder hex = new StringBuilder(ba.Length * 2);
        foreach (byte b in ba)
            hex.AppendFormat("{0:x2}", b);
        return hex.ToString();
    }

    private static T[] Concat<T>(T[] x, T[] y)
    {
        if (x == null) x = new T[0];
        if (y == null) y = new T[0];
        int oldLen = x.Length;
        Array.Resize<T>(ref x, x.Length + y.Length);
        Array.Copy(y, 0, x, oldLen, y.Length);
        return x;
    }

    public interface ILocalCredentialsResetter
    {
        Task ResetCredentialsAsync();
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IHttpContextAccessor Context { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private IJSRuntime JSRuntime { get; set; }
    }
}
#pragma warning restore 1591