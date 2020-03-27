Async Function Register_aysnc(ByVal Base As String, ByVal Key As String, ByVal Salt As String, ByVal Claims As Dictionary(Of String, Object), ByVal bp As Dictionary(Of String, Object)) As Task(Of Register_Return)
    Dim Rv As Register_Return = Nothing
    Dim m1 As mret = Nothing

    Dim bpl As Dictionary(Of String, Object) = New Dictionary(Of String, Object)(bp)

    bpl("Call_vector") = 4152

    Dim Token As String = Enc.Enc256.Encode(Key, Salt, Claims)

    Using cli = New FlurlClient(Base).WithHeader("X-Auth", Token)
      m1 = Await cli.Request().PostJsonAsync(bpl).ReceiveJson(Of mret)()
    End Using

    If m1.status = 200 Then
      Rv = JsonConvert.DeserializeObject(Of Register_Return)(m1.body)
    Else
      Rv.code = m1.status
      Rv.message = m1.message
    End If
    Return Rv
  End Function
  
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

Public Class Register_Return
  Public ex As Exception
  Public code As Integer = 200
  Public message As String
  Public eval As String
End Class

Public Class Dsave
  Public Ukey As String
  Public Wrtim As datetime
  Public Fname As String
  Public Lname As String
End Class

Public Class Dsave_return
  Public ex As Exception
  Public code As Integer = 200
  Public message As String
  Public ds As Dsave
End Class