Imports System.Collections.Generic
Imports System.Threading.Tasks
Imports IdentityModel.Client
Imports Microsoft.AspNet.Identity
Imports Microsoft.IdentityModel.Protocols.OpenIdConnect
Imports Microsoft.IdentityModel.Tokens
Imports Microsoft.Owin
Imports Microsoft.Owin.Security
Imports Microsoft.Owin.Security.Cookies
Imports Microsoft.Owin.Security.OpenIdConnect
Imports Microsoft.Owin.Security.Notifications
Imports Owin
Imports System.Security.Claims
Imports Microsoft.Owin.Host.SystemWeb

<Assembly: OwinStartup(GetType(Startup))>
Public Class Startup
    Private ReadOnly _authority As String = $"{Utils.GetOktaSetting("Domain")}/oauth2/default"
    Private ReadOnly _clientId As String = Utils.GetOktaSetting("ClientId")
    Private ReadOnly _clientSecret As String = Utils.GetOktaSetting("ClientSecret")
    Private ReadOnly _redirectUri As String = Utils.GetOktaSetting("RedirectUri")


    Public Sub Configuration(app As IAppBuilder)
        ConfigureAuth(app)
    End Sub

    Public Sub ConfigureAuth(app As IAppBuilder)
        app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie)
        app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType)
        app.UseCookieAuthentication(New CookieAuthenticationOptions With
            {
                .CookieManager = New SystemWebCookieManager()
            })

        app.UseOpenIdConnectAuthentication(New OpenIdConnectAuthenticationOptions With
            {
                .ClientId = _clientId,
                .ClientSecret = _clientSecret,
                .Authority = _authority,
                .RedirectUri = _redirectUri,
                .ResponseType = OpenIdConnectResponseType.CodeIdToken,
                .Scope = "openid profile email"
            })
            
    End Sub
End Class
