Imports Microsoft.AspNet.Identity
Imports Microsoft.Owin.Security
Imports Microsoft.Owin.Security.OpenIdConnect
Public Class Contact
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not (Request.IsAuthenticated) Then
            HttpContext.Current.GetOwinContext().Authentication.Challenge(New AuthenticationProperties() With {
                                                              .RedirectUri = "/"},
                                                              OpenIdConnectAuthenticationDefaults.AuthenticationType)
        End If
    End Sub
End Class