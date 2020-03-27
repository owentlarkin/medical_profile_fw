Imports JWT
Imports Microsoft.Win32

Public Class Formaik
  Private keys As String
  Private key_encrypted As String

  Private installation_url As String = "https://2xi513e85m.execute-api.us-east-2.amazonaws.com/Dev"

  Public Sub New(cv() As Char)
    keys = New String(cv)
    SetStyle(ControlStyles.OptimizedDoubleBuffer, True)

    InitializeComponent()
  End Sub

  Private Sub Done_Click(sender As Object, e As EventArgs) Handles Done.Click
    Me.Close()
  End Sub

  Private Sub Formaik_Load(sender As Object, e As EventArgs) Handles MyBase.Load

  End Sub

  Private Async Sub Register_Click(sender As Object, e As EventArgs) Handles Register.Click
    Dim nw As DateTimeOffset = DateTimeOffset.UtcNow
    Dim ew As DateTimeOffset = nw.AddMinutes(5)
    ' Dim ew As DateTimeOffset = nw.AddYears(1)
    Dim provider As IDateTimeProvider = New UtcDateTimeProvider()
    Dim salt As String
    Dim mtyenc As String
    Dim mcd As Register_Return
    Dim aws_body As Dictionary(Of String, Object) = New Dictionary(Of String, Object)
    Dim Rkey As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Medical_Profile", True)

    If Key.Text IsNot Nothing AndAlso Not Key.Text = String.Empty Then
      key_encrypted = Enc.Enc256.Encrypt(Key.Text, Enc.Enc256.Scramble(keys))

      salt = Enc.Getsalt(key_encrypted)

      Dim payload = New Dictionary(Of String, Object) From
                             {
                                {"aud", "http://medicalprofilecard.com"},
                                {"exp", ew.ToUnixTimeSeconds()},
                                {"register", key_encrypted}
                             }
      aws_body("vector_code") = "4152"

      Application.UseWaitCursor = True
      mcd = Await Aws.Register_aysnc(installation_url, keys, salt, payload, aws_body)
      Application.UseWaitCursor = False

      mtyenc = Enc.Enc256.Decrypt(mcd.eval, Enc.Enc256.Scramble(keys), 18926)

      If mtyenc IsNot Nothing Then
        Rkey.SetValue("eval", mtyenc)
      End If

    End If

    Me.Close()
  End Sub
End Class