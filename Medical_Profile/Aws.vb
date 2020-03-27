Imports Flurl.Http
Imports Newtonsoft.Json
Imports System.Collections.Generic
Imports System.IO
Imports System.Threading.Tasks

Friend Module Aws
  Async Function Get_Level1_aysnc(ByVal Base As String, ByVal Key As String, ByVal Salt As String, ByVal Claims As Dictionary(Of String, Object), ByVal bp As Dictionary(Of String, Object)) As Task(Of Level1_Return)

    Dim l1 As Level1_Return = New Level1_Return

    If System.IO.File.Exists("l1ret.json") Then
      Using sr As New StreamReader("l1ret.json")
        Dim l1d As String = sr.ReadToEnd()
        l1 = JsonConvert.DeserializeObject(Of Level1_Return)(l1d)
        Return l1
      End Using
    End If
    Dim m1 As Mret
    Dim bpl As Dictionary(Of String, Object) = New Dictionary(Of String, Object)(bp)
    bpl("Call_vector") = 5000
    Dim Token As String = Enc.Enc256.Encode(Key, Salt, Claims)

    Using cli As FlurlClient = New FlurlClient(Base).WithHeader("X-Auth", Token)
      '  m1 = Await cli.Request().PostJsonAsync(bpl).ReceiveJson(Of Mret)()
      m1 = Await cli.Request.PostJsonAsync(bpl).ReceiveJson(Of Mret)()
      If m1.status = 200 Then
        l1 = JsonConvert.DeserializeObject(Of Level1_Return)(m1.body)
      Else
        l1.code = m1.status
        l1.message = m1.message
      End If
    End Using

    Return l1
  End Function

 Async Function Save_exception(ByVal Base As String, ByVal Key As String, ByVal Salt As String, ByVal Claims As Dictionary(Of String, Object), ByVal bp As Dictionary(Of String, Object)) As Task(Of Dsave_return)
  Dim Dr As Dsave_return = New Dsave_return
  Dim M1 As Mret = Nothing
  Dim Bpl As Dictionary(Of String, Object) = New Dictionary(Of String, Object)(bp)
  Bpl("Call_vector") = 6725
  Dim Token As String = Enc.Enc256.Encode(Key, Salt, Claims)
  Using Cli = New FlurlClient(Base).WithHeader("X-Auth", Token)
   Try
    M1 = Await Cli.Request.PostJsonAsync(Bpl).ReceiveJson(Of Mret)()
    If M1.status = 200 Then
     Dr = JsonConvert.DeserializeObject(Of Dsave_return)(M1.body)
    Else
     Dr.code = M1.status
     Dr.message = M1.message
    End If
   Catch ex As FlurlHttpException
    Dim i As Integer = 1
   End Try
  End Using

  Return Dr
 End Function

 Async Function Pat_delete_aysnc(ByVal Base As String, ByVal Key As String, ByVal Salt As String, ByVal Claims As Dictionary(Of String, Object), ByVal bp As Dictionary(Of String, Object)) As Task(Of Dsave_return)
    Dim Dr As Dsave_return = New Dsave_return
    Dim M1 As Mret = Nothing
    Dim Bpl As Dictionary(Of String, Object) = New Dictionary(Of String, Object)(bp)
    Bpl("Call_vector") = 5184
    Dim Token As String = Enc.Enc256.Encode(Key, Salt, Claims)
    Using Cli = New FlurlClient(Base).WithHeader("X-Auth", Token)
      Try
        ' M1 = Await Cli.Request().PostJsonAsync(Bpl).ReceiveJson(Of Mret)()
        M1 = Await Cli.Request.PostJsonAsync(Bpl).ReceiveJson(Of Mret)()
        If M1.status = 200 Then
          Dr = JsonConvert.DeserializeObject(Of Dsave_return)(M1.body)
        Else
          Dr.code = M1.status
          Dr.message = M1.message
        End If
      Catch ex As FlurlHttpException
        Dim i As Integer = 1
      End Try
    End Using

    Return Dr
  End Function
  Async Function Pat_save_aysnc(ByVal Base As String, ByVal Key As String, ByVal Salt As String, ByVal Claims As Dictionary(Of String, Object), ByVal bp As Dictionary(Of String, Object)) As Task(Of Dsave_return)
    Dim Dr As Dsave_return = New Dsave_return
    Dim M1 As Mret = Nothing
    Dim Bpl As Dictionary(Of String, Object) = New Dictionary(Of String, Object)(bp)
    Bpl("Call_vector") = 5163
    Dim Token As String = Enc.Enc256.Encode(Key, Salt, Claims)
    Using Cli = New FlurlClient(Base).WithHeader("X-Auth", Token)
      Try
        '  M1 = Await Cli.Request().PostJsonAsync(Bpl).ReceiveJson(Of Mret)()
        M1 = Await Cli.Request.PostJsonAsync(Bpl).ReceiveJson(Of Mret)()
        If M1.status = 200 Then
          Dr = JsonConvert.DeserializeObject(Of Dsave_return)(M1.body)
        Else
          Dr.code = M1.status
          Dr.message = M1.message
        End If
      Catch ex As FlurlHttpException
        Dim i As Integer = 1
      End Try
    End Using

    Return Dr
  End Function

  Async Function Dsave_get_aysnc(ByVal Base As String, ByVal Key As String, ByVal Salt As String, ByVal Claims As Dictionary(Of String, Object), ByVal bp As Dictionary(Of String, Object)) As Task(Of Dsave_return)
    Dim Dvr As Dsave_return = New Dsave_return
    Dim M1 As Mret = Nothing
    Dim Bpl As Dictionary(Of String, Object) = New Dictionary(Of String, Object)(bp)

    If System.IO.File.Exists("dsave_get_return.json") Then
      Using sr As New StreamReader("dsave_get_return.json")
        Dim l1d As String = sr.ReadToEnd()
        Dvr = JsonConvert.DeserializeObject(Of Dsave_return)(l1d)
        Return Dvr
      End Using
    End If

    Bpl("Call_vector") = 7943
    Dim Token As String = Enc.Enc256.Encode(Key, Salt, Claims)
    Using Cli = New FlurlClient(Base).WithHeader("X-Auth", Token)
      Try
        '   M1 = Await Cli.Request().PostJsonAsync(Bpl).ReceiveJson(Of Mret)()
        M1 = Await Cli.Request.PostJsonAsync(Bpl).ReceiveJson(Of Mret)()
        If M1.status = 200 Then
          Dvr = JsonConvert.DeserializeObject(Of Dsave_return)(M1.body)
        Else
          Dvr.code = M1.status
          Dvr.message = M1.message
        End If
      Catch ex As FlurlHttpException
        Dim i As Integer = 1
      End Try
    End Using

    Return Dvr
  End Function

  Async Function Get_Level2_aysnc(ByVal Base As String, ByVal Key As String, ByVal Salt As String, ByVal Claims As Dictionary(Of String, Object), ByVal bp As Dictionary(Of String, Object)) As Task(Of Level2_Return)
    Dim l2 As Level2_Return = New Level2_Return

    If System.IO.File.Exists("l2ret.json") Then
      Dim m2 As Mret = Nothing
      Using sr As New StreamReader("l2ret.json")
        Dim l2d As String = sr.ReadToEnd()
    '    l2d = l2d.Replace("\", String.Empty)
    '   m2 = JsonConvert.DeserializeObject(Of mret)(l2d)
    l2 = JsonConvert.DeserializeObject(Of Level2_Return)(l2d)
        Return l2
      End Using
    End If

    Dim m1 As Mret = Nothing
    Dim bpl As Dictionary(Of String, Object) = New Dictionary(Of String, Object)(bp)
    bpl("Call_vector") = 6000
    Dim Token As String = Enc.Enc256.Encode(Key, Salt, Claims)
    Using cli = New FlurlClient(Base).WithHeader("X-Auth", Token)
      Try
        '   m1 = Await cli.Request().PostJsonAsync(bpl).ReceiveJson(Of Mret)()
        m1 = Await cli.Request.PostJsonAsync(bpl).ReceiveJson(Of Mret)()
        If m1.status = 200 Then
          l2 = JsonConvert.DeserializeObject(Of Level2_Return)(m1.body)
        Else
          l2.code = m1.status
          l2.message = m1.message
        End If
      Catch ex As FlurlHttpException
        Dim i As Integer = 1
      End Try
    End Using

    Return l2
  End Function

  Async Function Register_aysnc(ByVal Base As String, ByVal Key As String, ByVal Salt As String, ByVal Claims As Dictionary(Of String, Object), ByVal bp As Dictionary(Of String, Object)) As Task(Of Register_Return)
    Dim Rv As Register_Return = Nothing
    Dim m1 As Mret = Nothing

    Dim bpl As Dictionary(Of String, Object) = New Dictionary(Of String, Object)(bp)

    bpl("Call_vector") = 4152

    Dim Token As String = Enc.Enc256.Encode(Key, Salt, Claims)

    Using cli = New FlurlClient(Base).WithHeader("X-Auth", Token)
      '  m1 = Await cli.Request().PostJsonAsync(bpl).ReceiveJson(Of Mret)()
      m1 = Await cli.Request.PostJsonAsync(bpl).ReceiveJson(Of Mret)()
    End Using

    If m1.status = 200 Then
      Rv = JsonConvert.DeserializeObject(Of Register_Return)(m1.body)
    Else
      Rv.code = m1.status
      Rv.message = m1.message
    End If
    Return Rv
  End Function
End Module
