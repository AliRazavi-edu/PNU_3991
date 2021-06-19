Imports System.Configuration

Public Class AppSettings

    Private Shared _cdnUrl As String = String.Empty
    Public Shared ReadOnly Property CdnUrl As String
        Get

            If String.IsNullOrWhiteSpace(_cdnUrl) Then
                Dim value = ConfigurationManager.AppSettings("CdnUrl")

                If String.IsNullOrEmpty(value) Then
                    Throw New Exception("CDN Url not defined!")
                End If

                If Not value.EndsWith("/") Then
                    value = value & "/"
                End If

                _cdnUrl = value
            End If

            Return _cdnUrl

        End Get
    End Property

    Private Shared _IsCsharp As Boolean? = Nothing
    Public Shared ReadOnly Property IsCsharp As Boolean
        Get

            If Not _IsCsharp.HasValue Then
                Dim value = ConfigurationManager.AppSettings("Csharp")

                If String.IsNullOrEmpty(value) Then
                    _IsCsharp = False
                    Return _IsCsharp
                End If
                If value.ToLower = "true" Then
                    _IsCsharp = True
                Else
                    _IsCsharp = False
                End If

            End If

            Return _IsCsharp

        End Get
    End Property

    Public Shared Function GetPathInCdn(ByVal path As String) As String
        If path.StartsWith("/") Then
            path = path.TrimStart("/"c)
        End If
        Return CdnUrl & path
    End Function

    Public Shared ReadOnly Property ScriptPath As String
        Get
            Return GetPathInCdn("js/")
        End Get
    End Property

    Public Shared ReadOnly Property LibraryPath(Optional ByVal path As String = "") As String
        Get
            Return GetPathInCdn("lib/") & path
        End Get
    End Property

    Public Shared ReadOnly Property ThemePath As String
        Get
            Return GetPathInCdn("theme_img/")
        End Get
    End Property

    Public Shared ReadOnly Property ImagesPath As String
        Get
            Return GetPathInCdn("img/")
        End Get
    End Property

    Public Shared ReadOnly Property PagesPath As String
        Get
            Return GetPathInCdn("pages/")
        End Get
    End Property

    Public Shared ReadOnly Property ControlImagesPath As String
        Get
            Return GetPathInCdn("img/")
        End Get
    End Property

    Public Shared ReadOnly Property CssPath As String
        Get
            Return CdnUrl & "css/"
        End Get
    End Property

    Public Shared ReadOnly Property Pages_UnsupportedBrowser As String
        Get
            Return PagesPath & "UnsupportedBrowser.html"
        End Get
    End Property

    Public Shared ReadOnly Property Pages_InvalidAccess As String
        Get
            Return PagesPath & "InvalidAccess.html"
        End Get
    End Property

    Public Shared ReadOnly Property Pages_MissingInformations As String
        Get
            Return PagesPath & "MissingInformations.html"
        End Get
    End Property

    Public Shared ReadOnly Property Pages_InsufficientPermission As String
        Get
            Return PagesPath & "InsufficientPremission.html"
        End Get
    End Property

    Private Shared _LogoutUrl As String = String.Empty
    Public Shared ReadOnly Property LogoutUrl As String
        Get
            If String.IsNullOrWhiteSpace(_LogoutUrl) Then
                _LogoutUrl = System.Configuration.ConfigurationManager.AppSettings("LogoutUrl")
                If String.IsNullOrWhiteSpace(_LogoutUrl) Then
                    Return RootDomain & "Logout.aspx"
                End If
            End If
            Return _LogoutUrl
        End Get
    End Property

    Private Shared _rootDomain As String = String.Empty
    Public Shared ReadOnly Property RootDomain As String
        Get
            If String.IsNullOrWhiteSpace(_rootDomain) Then
                If Not System.Configuration.ConfigurationManager.AppSettings.AllKeys.Contains("RootDomain") Then
                    Throw New Exception("دامنه اصلی سامانه تعریف نشده است." & vbNewLine & "web.config > appSetting > RootDomain")
                End If
                _rootDomain = System.Configuration.ConfigurationManager.AppSettings("RootDomain")
                If Not _rootDomain.EndsWith("/") Then
                    _rootDomain &= "/"
                End If
            End If
            Return _rootDomain
        End Get
    End Property

    Private Shared _password As String = String.Empty
    Public Shared ReadOnly Property Password As String
        Get
            If String.IsNullOrWhiteSpace(_password) Then
                _password = System.Configuration.ConfigurationManager.AppSettings("Password")
                If String.IsNullOrWhiteSpace(_password) Then
                    Throw New Exception("رمز عبور سامانه تعریف نشده است." & vbNewLine & "web.config > appSetting > Password")
                End If
            End If
            Return _password
        End Get
    End Property

    Private Shared _debugMode As Boolean? = Nothing
    Public Shared ReadOnly Property DebugMode As Boolean
        Get
            If Not _debugMode.HasValue Then
                _debugMode = False
                Dim value = System.Configuration.ConfigurationManager.AppSettings("DebugMode")
                Boolean.TryParse(value, _debugMode)
            End If
            Return _debugMode
        End Get
    End Property


    Private Shared _showSyncErrror As Boolean? = Nothing
    Public Shared ReadOnly Property ShowSyncErrror As Boolean
        Get
            If Not _showSyncErrror.HasValue Then
                _showSyncErrror = False
                Dim value = System.Configuration.ConfigurationManager.AppSettings("ShowSyncErrror")
                Boolean.TryParse(value, _showSyncErrror)
            End If
            Return _showSyncErrror
        End Get
    End Property



End Class
