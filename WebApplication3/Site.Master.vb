Imports Microsoft.AspNet.Identity
Imports Microsoft.Owin.Security
Imports Microsoft.Owin.Security.OpenIdConnect

Public Class SiteMaster
    Inherits MasterPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs)

        If Not (Request.IsAuthenticated) Then
            HttpContext.Current.GetOwinContext().Authentication.Challenge(New AuthenticationProperties() With {
                                                              .RedirectUri = "/"},
                                                              OpenIdConnectAuthenticationDefaults.AuthenticationType)
        End If

    End Sub

    Protected Sub Unnamed_LoggingOut(sender As Object, e As LoginCancelEventArgs)
        Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie)
    End Sub
End Class