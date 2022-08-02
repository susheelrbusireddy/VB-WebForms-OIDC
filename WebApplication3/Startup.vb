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

<Assembly: OwinStartup(GetType(Startup))>
Public Class Startup
    Private ReadOnly _authority As String = $"{Utils.GetOktaSetting("Domain")}/oauth2/default"
    Private ReadOnly _clientId As String = Utils.GetOktaSetting("ClientId")
    Private ReadOnly _clientSecret As String = Utils.GetOktaSetting("ClientSecret")
    Private ReadOnly _redirectUri As String = Utils.GetOktaSetting("RedirectUri")

    <Obsolete>
    Public Sub Configuration(app As IAppBuilder)
        ConfigureAuth(app)
    End Sub

    <Obsolete>
    Public Sub ConfigureAuth(app As IAppBuilder)
        app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie)
        app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType)
        app.UseCookieAuthentication(New CookieAuthenticationOptions())

        app.UseOpenIdConnectAuthentication(New OpenIdConnectAuthenticationOptions With
            {
                .ClientId = _clientId,
                .ClientSecret = _clientSecret,
                .Authority = _authority,
                .RedirectUri = _redirectUri,
                .ResponseType = OpenIdConnectResponseType.CodeToken,
                .Scope = OpenIdConnectScope.OpenIdProfile,
                .SaveTokens = True,
                .RedeemCode = True,
                .TokenValidationParameters = New TokenValidationParameters With {.ValidateIssuer = False},
                .Notifications = New OpenIdConnectAuthenticationNotifications With
                    {
                        .AuthorizationCodeReceived = Async Function(n)
                                                         ' Exchange code for access And ID tokens
                                                         Dim tokenClient As New TokenClient($"{_authority}/v1/token", _clientId, _clientSecret)
                                                         Dim TokenResponse As TokenResponse = Await tokenClient.RequestAuthorizationCodeAsync(n.Code, _redirectUri)
                                                         If (TokenResponse.IsError) Then
                                                             Throw New Exception(TokenResponse.Error)
                                                         End If

                                                         Dim userInfoClient As New UserInfoClient($"{_authority}/v1/userinfo")
                                                         Dim UserInfoResponse As UserInfoResponse = Await userInfoClient.GetAsync(TokenResponse.AccessToken)

                                                         Dim claims As New List(Of System.Security.Claims.Claim)(UserInfoResponse.Claims) From {
                                                             New Claim("id_token", TokenResponse.IdentityToken),
                                                             New Claim("access_token", TokenResponse.AccessToken)
                                                         }

                                                         n.AuthenticationTicket.Identity.AddClaims(claims)                                                        

                                                     End Function
                    }
            })

    End Sub
End Class
