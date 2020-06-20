#pragma checksum "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Login.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "b3152e57a3e0b86928191e26978829cfdfb157e4"
// <auto-generated/>
#pragma warning disable 1591
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
            __builder.OpenElement(0, "div");
            __builder.AddMarkupContent(1, "\r\n    ");
            __builder.OpenElement(2, "div");
            __builder.AddAttribute(3, "class", "jumbotron");
            __builder.AddAttribute(4, "style", "padding: 15px 20px 15px 20px; position: absolute; width: 400px; height: 310px; vertical-align: middle; margin: auto; top: 0px; right: 0px; bottom: 0px; left: 0px;");
            __builder.AddMarkupContent(5, "\r\n        ");
            __builder.AddMarkupContent(6, "<label style=\"font-size:x-large\">IMS Login</label>\r\n        <br>\r\n        <br>\r\n        ");
            __builder.AddMarkupContent(7, "<label style=\"font-size:medium\">Username</label>\r\n        <br>\r\n        ");
            __builder.OpenElement(8, "input");
            __builder.AddAttribute(9, "class", "form-control");
            __builder.AddAttribute(10, "style", "width:100%");
            __builder.AddAttribute(11, "value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 21 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Login.razor"
                                                              Username

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(12, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => Username = __value, Username));
            __builder.SetUpdatesAttributeName("value");
            __builder.CloseElement();
            __builder.AddMarkupContent(13, "\r\n        <br>\r\n        ");
            __builder.AddMarkupContent(14, "<label style=\"font-size:medium\">Password</label>\r\n        <br>\r\n        ");
            __builder.OpenElement(15, "input");
            __builder.AddAttribute(16, "class", "form-control");
            __builder.AddAttribute(17, "style", "width:100%");
            __builder.AddAttribute(18, "type", "password");
            __builder.AddAttribute(19, "onkeyup", "if (event.keyCode === 13) { document.getElementById(\'LoginButton\').click(); }");
            __builder.AddAttribute(20, "value", Microsoft.AspNetCore.Components.BindConverter.FormatValue(
#nullable restore
#line 25 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Login.razor"
                                                                              Password

#line default
#line hidden
#nullable disable
            ));
            __builder.AddAttribute(21, "onchange", Microsoft.AspNetCore.Components.EventCallback.Factory.CreateBinder(this, __value => Password = __value, Password));
            __builder.SetUpdatesAttributeName("value");
            __builder.CloseElement();
            __builder.AddMarkupContent(22, "\r\n        <br>\r\n        ");
            __builder.OpenElement(23, "button");
            __builder.AddAttribute(24, "class", "btn btn-primary");
            __builder.AddAttribute(25, "id", "LoginButton");
            __builder.AddAttribute(26, "type", "submit");
            __builder.AddAttribute(27, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 27 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Login.razor"
                                                                                 AttemptLoginAsync

#line default
#line hidden
#nullable disable
            ));
            __builder.AddContent(28, "Login");
            __builder.CloseElement();
            __builder.AddMarkupContent(29, "\r\n        ");
            __builder.OpenElement(30, "button");
            __builder.AddAttribute(31, "class", "btn btn-primary");
            __builder.AddAttribute(32, "style", "margin-left:10px; background-color:transparent; color:#23527c");
            __builder.AddAttribute(33, "onclick", Microsoft.AspNetCore.Components.EventCallback.Factory.Create<Microsoft.AspNetCore.Components.Web.MouseEventArgs>(this, 
#nullable restore
#line 28 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Login.razor"
                                                                                                                        RequestLoginReset

#line default
#line hidden
#nullable disable
            ));
            __builder.AddContent(34, "Reset username/password");
            __builder.CloseElement();
            __builder.AddMarkupContent(35, "\r\n        <br>\r\n        ");
            __builder.OpenElement(36, "div");
            __builder.AddAttribute(37, "style", "margin-top:10px");
            __builder.AddMarkupContent(38, "\r\n            ");
            __builder.OpenElement(39, "label");
            __builder.AddAttribute(40, "class", "alert-danger");
            __builder.AddAttribute(41, "style", "background-color:transparent; font-weight:normal;");
            __builder.AddContent(42, 
#nullable restore
#line 31 "D:\Projects\IntegratedMinecraftServer\IMS-Interface\Pages\Login.razor"
                                                                                                   ErrorText

#line default
#line hidden
#nullable disable
            );
            __builder.CloseElement();
            __builder.AddMarkupContent(43, "\r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(44, "\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(45, "\r\n");
            __builder.CloseElement();
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