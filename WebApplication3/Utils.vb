Imports System.IO
Imports System.Net
Imports System.Text.RegularExpressions
Imports System.Web.UI
Imports System.Web.Configuration
Imports System.Collections.Specialized

Public Class Utils
    Private Shared ReadOnly settings As NameValueCollection = WebConfigurationManager.AppSettings

    Public Shared Function IsFlagActive(flag As String) As Boolean
        Dim isActive As Boolean = False
        Dim flagValue As String = WebConfigurationManager.AppSettings($"flag:{flag}")
        Boolean.TryParse(flagValue, isActive)
        Return isActive
    End Function

    Public Shared Function GetOktaSetting(key As String) As String
        Dim value As Object = settings($"okta:{key}")
        If String.IsNullOrEmpty(value) Then Return Nothing
        Return value
    End Function
End Class
