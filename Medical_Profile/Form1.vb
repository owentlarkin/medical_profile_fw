Imports System.ComponentModel
Imports System.IO
Imports System.Text.RegularExpressions
Imports Amazon
Imports Amazon.DynamoDBv2
Imports Amazon.DynamoDBv2.DocumentModel
Imports DYMO.Label.Framework
Imports JWT
Imports Microsoft.Win32
Imports Newtonsoft.Json
'Imports JR.Utils.GUI.Forms
'Imports Medical_Profile.JR.Utils.GUI.Forms
'Imports System.ServiceModel
'Imports System.Windows.Threading
Imports System.Reflection.Emit

Partial Class form1

 Dim Endpoints_in_use As Specialized.StringCollection = New Specialized.StringCollection
 Dim drive_label As String = Nothing
 Dim usb_file As String = Nothing
 Public Shared cid As String = Nothing
 Dim ftime As String = Nothing
 Dim pngdata As Object
 Dim iLock As New Object
 Dim line As String
 Dim Folder As String = ""
 Dim icalled As Boolean = False
 Dim dnh As Boolean = True
 Dim cv(16) As Char
 Dim Wanted_hgt As Integer = 0
 Dim o1 As Object = Nothing
 Dim margin1 As Integer = 6
 Dim margin2 As Integer = 18
 Dim margin3 As Integer = 10

 Dim label_header As Integer = 345
 Dim ll As Single = 3600 / 15.0
 Dim nlead As Integer = 191
 Dim ylimit As Integer = 2386

 Delegate Sub cb()

 Delegate Sub InvokeDelegate()

 Dim Preview As Boolean = False

 Dim Develop As Boolean = False

 Public Shared Mpck As MPC_key = Nothing

 Private _timer As System.Timers.Timer

 Public Enum Load_state
  not_loaded
  loaded
  not_loaded_byc
  loaded_byc
  not_in_use
 End Enum

 Public Enum Data_state
  Edit_mode
  NoEdit
  Loaded_by_id
  Loaded_by_name
 End Enum

 Public Dstate As Data_state = Data_state.NoEdit

 Public Sub New()
  Dim fnam As String = Nothing
  Dim fs As String = Nothing
  Try
   SetStyle(ControlStyles.OptimizedDoubleBuffer, True)

   InitializeComponent()
   Me.dept_tbox.Visible = False

   Me.Visible = False

   Dim myScreen As Screen = Screen.FromControl(Me)
   Dim area As Rectangle = myScreen.WorkingArea

   Screen_width = area.Size.Width

   Screen_height = area.Size.Height

   cv(5) = Chr(Asc(title(19)) + 16)

   cv(6) = Chr(Asc(title(9)))
   cv(7) = Chr(Asc(title(33)))

   cv(8) = Chr(Asc(title(13)) - 8)
   cv(10) = Chr(Asc(title(1)))
   cv(11) = Chr(Asc(title(36)))

   If My.Settings.first_run_flag Then
    first_run = True
    My.Settings.first_run_flag = False
    My.Settings.Save()
   Else
    first_run = False
   End If

   cv(12) = Chr(Asc(title(31)))
   cv(13) = Chr(Asc(title(9)) + 10)
   cv(15) = Chr(Asc(title(33)) + 5)
   cv(3) = Chr(Asc(title(3)) + 6)
   cv(1) = Chr(Asc(title(10)))
   cv(2) = Chr(Asc(title(18)))

   If first_run Then
    Dim pi1 As New ProcessStartInfo With {
      .FileName = "cscript.exe",
      .Arguments = "pin_start.vbs",
      .WindowStyle = ProcessWindowStyle.Hidden
    }
    Dim p1 As Process = Process.Start(pi1)
    p1.WaitForExit()

    Dim pi2 As New ProcessStartInfo With {
      .FileName = "cscript.exe",
      .Arguments = "pin_start_cm.vbs",
      .WindowStyle = ProcessWindowStyle.Hidden
    }
    Dim p2 As Process = Process.Start(pi2)

    p2.WaitForExit()

    My.Settings.Save()

    Application.Exit()
   End If
  Catch ex As Exception
   MessageBox.Show(ex.Message)
  End Try

  cv(9) = Chr(Asc(title(0)) + 32)
  cv(16) = Chr(Asc(title(26)) - 48)
  cv(0) = Chr(Asc(title(2)) - 32)
  cv(4) = Chr(Asc(title(5)) - 64)
  cv(14) = Chr(Asc(title(23)) + 16)

  Dim key As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Medical_Profile", True)

  If key IsNot Nothing Then
   cid = key.GetValue("Cid", Nothing)

   If cid = Nothing Then
    cid = Guid.NewGuid.ToString()
    key.SetValue("Cid", cid)
   End If
  End If

 End Sub

 Public Function Handle_file(eval As String) As String
  Dim mty As MPC_type = Nothing

  Dim rjson As String = Enc.Enc256.Decrypt(eval, enck, 17531)

  If rjson = Nothing Then
   Return 1
  End If

  mty = JsonConvert.DeserializeObject(Of MPC_type)(rjson)

  If Not mty.Akey = Nothing Then
   drive_label = mty.Akey
  End If

  If Not mty.F1 = Nothing Then
   rjson = Enc.Enc256.Decrypt(mty.F1, enck, mty.Akey)

   If rjson Is Nothing Then
    MessageBox.Show("Key file is incorrect.", "Error", MessageBoxButtons.OK)
    Application.Exit()
   End If
   Mpck = JsonConvert.DeserializeObject(Of MPC_key)(rjson)
  End If
  Return 0
 End Function

 Private Function Set_key(ByVal s1 As String, ByVal s2 As String) As String
  Dim v1 As Integer = 0

  For i As Integer = 3 To s2.Length - 1
   v1 += Convert.ToInt32(s2.Substring(i, 1))
  Next

  Dim insert_loc As Integer = v1 Mod (s2.Length - 4)
  Return s1.Substring(0, insert_loc) & s2 & s1.Substring(insert_loc)
 End Function

#If DEBUG Then
 Public Function Handle_testmode(un As String) As Integer
  Dim n As Integer = Nothing
  Dim mpu As MPC_User
  Dim ds As String
  Dim item As Document
  Dim table As Table

  Using client As AmazonDynamoDBClient = New AmazonDynamoDBClient(RegionEndpoint.USEast2)
   table = Table.LoadTable(client, "mpc_users")
   item = table.GetItem(Convert.ToInt32(un))
   ds = item.ToJsonPretty
   mpu = JsonConvert.DeserializeObject(Of MPC_User)(ds)
  End Using

  Mpck = New MPC_key
  With Mpck
   .Url = mpu.Url
   .Mkey = mpu.Mkey
   .Salt = mpu.Salt
   .Email = mpu.Email
   .Dlab = mpu.Disk_Label
   .Secret = mpu.Secret1
   .Iterations = mpu.Iterations
   .Blocks = mpu.Blocks
   .Blocklist = mpu.Blocklist
   .K1 = mpu.K1
   .Labels = mpu.Labels
   .Lines = mpu.Lines
   .Minimum_blocks = mpu.Minimum_blocks
   .Points = mpu.Points
   .Sec_visible = mpu.Sec_visible
   .Sptitle = mpu.Sptitle
   .Version = mpu.Version
  End With

  cid = mpu.cid

  drive_label = mpu.Disk_Label

  file_access = True

  Return 0
 End Function
#End If

 Public Function Handle_usb(dle As String) As Integer

  Dim n As Integer = Nothing
  Dim fs As String
  Dim it As String
  Dim d1 As String = Nothing

  Dim drivesrem As DriveInfo() = DriveInfo.GetDrives().Where(Function(drive) drive.DriveType = DriveType.Removable).ToArray()

  For Each d As DriveInfo In drivesrem
   If d.VolumeLabel.StartsWith("MPC") Then
    d1 = Enc.Enc256.Decrypt(dle, enck, d.VolumeLabel)
    If d1 IsNot Nothing AndAlso d1 = d.VolumeLabel Then
     it = d.VolumeLabel.Substring(3)
     USB_Fname = d.Name & "F" & it & ".fil"
     drive_label = d.VolumeLabel
    End If
   End If
  Next

  If USB_Fname IsNot Nothing Then
   If System.IO.File.Exists(USB_Fname) Then
    Try
     USB_Ftime = File.GetLastWriteTimeUtc(USB_Fname)
     Using sr As New StreamReader(USB_Fname)
      fs = sr.ReadToEnd()

      Dim rjson As String = Enc.Enc256.Decrypt(fs, enck, drive_label)

      If rjson Is Nothing Then
       MessageBox.Show("Key file is incorrect.", "Error", MessageBoxButtons.OK)
       Application.Exit()
      End If
      Mpck = JsonConvert.DeserializeObject(Of MPC_key)(rjson)
     End Using
    Catch ex As Exception
     Console.WriteLine("Key file could not be read:")
     Console.WriteLine(ex.Message)
     Return 1
    End Try
   Else
    MessageBox.Show("Unable to find the key file.", "Error", MessageBoxButtons.OK)
    Return 1
   End If
  Else
   MessageBox.Show("Unable to find the usb key drive.", "Error", MessageBoxButtons.OK)
   Return 1
  End If
  Return 0
 End Function

 Dim bl As Single

 Dim tb As DYMO.Label.Framework.TextObject

 Dim bn As Integer

 Private Function New_label(ByRef lbl As DYMO.Label.Framework.DieCutLabel, ByRef ln As Integer) As Integer
  Dim nlen As Single
  Dim rlen As Single

  lbl = Framework.Open("mpcb.label")

  tb = lbl.GetObjectByName("labtext")
  lbl.DeleteObject(tb)

  tb = lbl.GetObjectByName("header")

  stb = New DYMO.Label.Framework.StyledTextBuilder

  htb = New DYMO.Label.Framework.StyledTextBuilder

  If Not String.Compare(Me.DOB.Text, "") = 0 Then
   stb.Append("DOB: ", lfnt, Colors.Black)
   stb.Append(Me.DOB.Text, lfnt, Colors.Black)
   stb.Append(" ", lfnt, Colors.Black)
  End If

  rlen = Wlengb(stb.StyledText.Text, 9)

  nlen = Wlengb(Me.Patient.Text + "    ", 9)

  bl = Wlengb("", 9)

  bn = (192.375 - (rlen + nlen - bl)) / bl

  htb.Append(Me.Patient.Text + "     ", lfnt, Colors.Black)
  htb.Append(StrDup(bn, " "), lfnt, Colors.Black)
  htb.Append(stb.StyledText.Text, lfnt, Colors.Black)

  tb.StyledText = htb.StyledText

  '  ltl = 1

  ln += 1

  Return label_header
 End Function

 Private Sub Output_label(ByRef pjob As PrintJob, ByRef label As DieCutLabel, lno As Integer, Optional sb As StyledTextBuilder = Nothing)
  If sb IsNot Nothing Then
   Dim tb As TextObject = label.GetObjectByName("labtext")
   tb.StyledText = sb.StyledText
  End If

  If Preview Then
   Pnglablist.Add(Render(label))
   Dim lns As String = "Label" + lno.ToString()
   label.SaveToFile(lns + ".label")
  Else
   pjob.AddLabel(label)
  End If
  sb = Nothing
  label = Nothing
 End Sub

 Private Function Check_lines() As Boolean
  Dim lc As Integer = 0
  Dim li() As String
  Dim lis As String
  Dim lin As List(Of String) = New List(Of String)
  Dim lno As Integer = 0
  Dim yl As Integer = 0
  Dim lin_count As Integer = 0
  Dim y_space_needed As Integer = 0
  Dim elines As Dictionary(Of Integer, Integer) = New Dictionary(Of Integer, Integer)

  yl = label_header

  For Each cb As Control In Me.Controls
   If Not cb.Name.StartsWith("GB") Then
    Continue For
   End If

   If String.IsNullOrEmpty(cb.Controls(0).Text) Or String.IsNullOrEmpty(cb.Controls(1).Text) Then
    Continue For
   End If

   Dim bn As Integer = (From kvp As KeyValuePair(Of Integer, String) In gpn Where kvp.Value = cb.Name Select kvp).First().Key

   y_space_needed = nlead * 2

   If currenty > label_header Then
    y_space_needed += nlead / 2
   End If

   If (ylimit - currenty) < y_space_needed Then
    lno += 1
    yl = label_header
   End If

   If currenty > label_header Then
    yl += nlead / 2
   End If

   yl += nlead

   lis = cb.Controls(1).Text

   elines.Clear()

   If lines_setting = "Label" Then
    lis = Adjust_lines(lis, "File")
   End If

   lis = Adjust_lines(lis, "Label", elines)

   li = Regex.Split(lis, "\r\n|\n")

   Dim lic As Integer = li.Count

   If max_rec(bn) > 0 And lic > max_rec(bn) Then
    lic = max_rec(bn)
    Dim iadj As Integer = 0
    For Each kvp As KeyValuePair(Of Integer, Integer) In elines
     If kvp.Key <= lic Then
      iadj = iadj + kvp.Value
     End If
    Next
    lic = lic + iadj
   End If

   For i As Integer = 0 To li.Count - 1 Step 1
    yl += nlead
    lin_count += 1
    If (ylimit - yl) < nlead Then
     lno += 1
     yl = label_header
     If lno > labels_number Then
      Exit For
     End If
    End If
   Next
  Next

  If (lin_count > total_lines) Then
   Dim Response As DialogResult

   Response = MessageBox.Show _
           ((lin_count - total_lines).ToString + " lines will not fit on the labels",
                                                    "Continue With Print",
                                                    MessageBoxButtons.OKCancel,
                                                    MessageBoxIcon.Exclamation,
                                                    MessageBoxDefaultButton.Button1)

   If Response = DialogResult.Cancel Then
    Return False
   Else
    Return True
   End If
  Else
   Return True
  End If
 End Function

 Private Sub Reset_labels()
  Dim lmni As List(Of ToolStripMenuItem) = New List(Of ToolStripMenuItem)

  f3s.Clear()

  lmni.Clear()

  For Each ts As ToolStripMenuItem In MenuStrip1.Items
   If ts.Name.StartsWith("Label") Then
    lmni.Add(ts)
   End If
  Next

  For Each ts As ToolStripMenuItem In lmni
   MenuStrip1.Items.Remove(ts)
  Next
 End Sub

 Public Function Lab_field(Name As String, ByVal val As StyledTextBuilder, x As Single, y As Integer, w As Single, h As Integer) As Integer
  Dim Tb As TextObject

  Label.AddObject(New TextObject(Name), New Rect(x, y / 15.0, w, h / 15.0))

  Tb = Label.GetObjectByName(Name)

  Tb.StyledText = val.StyledText

  Return y + h
 End Function

 Public Function Lab_field(Name As String, ByVal val As StyledTextBuilder, x As Integer, y As Integer, w As Single, h As Integer) As Integer
  Dim Tb As TextObject

  Label.AddObject(New TextObject(Name), New Rect(x / 15.0, y / 15.0, w, h / 15.0))

  Tb = Label.GetObjectByName(Name)

  Tb.StyledText = val.StyledText

  Return y + h
 End Function

 Public Function Lab_field(Name As String, val As String, x As Single, y As Integer, w As Single, h As Integer) As Integer
  Dim Tb As TextObject

  Label.AddObject(New TextObject(Name), New Rect(x, y / 15.0, w, h / 15.0))

  Tb = Label.GetObjectByName(Name)

  Tb.Text = val

  Return y + h
 End Function

 Public Function Lab_field(Name As String, val As String, x As Integer, y As Integer, w As Single, h As Integer) As Integer
  Dim Tb As TextObject

  Label.AddObject(New TextObject(Name), New Rect(x / 15.0, y / 15.0, w, h / 15.0))

  Tb = Label.GetObjectByName(Name)

  Tb.Text = val

  Return y + h
 End Function

 Public Sub Generate_Labels()
  Dim P1 As String = "(\(??\d\d\d\)??[\s|-]\d\d\d-\d\d\d\d)"
  Dim P2 As String = "(\d\d\d-\d\d\d\d)"
  Dim M1 As Match
  Dim li() As String = Nothing
  Dim lis As String
  Dim yadj As Integer
  Dim stb As StyledTextBuilder = New StyledTextBuilder
  Dim stx As StyledTextBuilder = New StyledTextBuilder
  Dim flen As Single
  Dim nlen As Single
  Dim dobst As Single
  Dim lpht As Single
  Dim cy_save As Integer
  Dim y_space_needed As Integer
  Dim tb As TextObject
  Dim Phone_number As String

  Dim elines As Dictionary(Of Integer, Integer) = New Dictionary(Of Integer, Integer)

  Reset_labels()

  If Not Check_lines() Then
   Return
  End If

  labelno = 0

  Pnglablist.Clear()

  If TypeOf printer Is ILabelWriterPrinter Then
   pjob = printer.CreatePrintJob(lprintparams)
  Else
   pjob = printer.CreatePrintJob(printParams)
  End If

  Me.pname = Me.Patient.Text
  Me.pdob = Me.DOB.Text

  Label = Framework.Open("mpc1.label")

  tb = Label.GetObjectByName("name")
  Label.DeleteObject(tb)

  tb = Label.GetObjectByName("labtext")
  Label.DeleteObject(tb)

  stb = New StyledTextBuilder

  flen = Wlengb("DOB: ", 12)

  stb.Append("DOB: ", nfnt, Colors.Black)
  stb.Append(Me.DOB.Text, nfnt, Colors.Black)

  flen = flen + Wleng(Me.DOB.Text, 12)

  dobst = 3900.0 / 15.0 - flen

  Label.AddObject(New TextObject("dob"), New Rect(dobst, 480 / 15.0, flen, 210 / 15.0))

  tb = Label.GetObjectByName("dob")

  tb.HorizontalAlignment = TextAlignment.Right

  tb.StyledText = stb.StyledText

  nlen = 3900.0 / 15.0 - flen - 317 / 15.0

  name_length = Wlengb(Me.pname, 12)

  stb = New StyledTextBuilder
  stb.Append(Me.pname, nfnt, Colors.Black)

  currenty = Lab_field("name", stb, 317, 480, nlen, 210)

  If Me.address.Lines.Count > 0 Then
   stb = New StyledTextBuilder
   ltline = 0
   li = Me.address.Lines

   For i = 0 To li.Count - 1
    If Not li(i) = String.Empty Then
     If i > 0 Then
      stb.Append(vbCrLf, reg_font, Colors.Black)
     End If
     stb.Append(li(i), reg_font, Colors.Black)
     ltline += 1
    End If
   Next
   currenty = Lab_field("address", stb, 317, currenty, ll, ltline * 191)
  End If

  If Not Me.Phone.Text = String.Empty Then
   stb = New StyledTextBuilder
   stb.Append("Phone: ", bld_font, Colors.Black)
   stb.Append(Me.Phone.Text, reg_font, Colors.Black)
   currenty = Lab_field("phone", stb, 317, currenty, ll, 191)
  End If

  If Not Me.econtact.Text = String.Empty Then
   stb = New StyledTextBuilder
   stb.Append("Emergency Contact: ", bld_font, Colors.Black)
   stb.Append(Me.econtact.Text, reg_font, Colors.Black)
   currenty = Lab_field("econtact", stb, 317, currenty, ll, 191)
  End If

  Dim dname As String = priph.Text

  If prv_combo.Items.Count > 0 Then
   dname = prv_combo.Text
  End If

  If Not dname = String.Empty Then
   stb = New StyledTextBuilder
   stb.Append(lab1("priph_title:"), bp1_font, Colors.Black)
   Dim s1 As String = lab1("priph_title:")
   If Not lab1("priph_title:").EndsWith(":") Then
    stb.Append(":", bp1_font, Colors.Black)
    s1 = s1 & ":"
   End If

   lpht = Wlengb(s1, 9) * 1.33

   cy_save = currenty

   Dim pl As Integer = 230

   yadj = 30

   currenty = Lab_field("priphyst", stb, 317, currenty + yadj, lpht, pl)

   lpht = lpht + 317 / 15.0

   Phone_number = String.Empty

   M1 = Regex.Match(dname, P1)

   If M1.Success Then
    Phone_number = M1.Value
    dname = dname.Replace(Phone_number, String.Empty)
   End If

   If Not M1.Success Then
    M1 = Regex.Match(dname, P2)
    If M1.Success Then
     Phone_number = M1.Value
     dname = dname.Replace(Phone_number, String.Empty)
    End If
   End If

   stb = New StyledTextBuilder

   stb.Append(dname, rp1_font, Colors.Black)
   If Not String.IsNullOrEmpty(Phone_number) Then
    pl = 2 * pl
    stb.Append(vbCrLf, rp1_font, Colors.Black)
    stb.Append("  " + Phone_number, rp1_font, Colors.Black)
   End If

   nlen = (3900 / 15.0) - lpht

   currenty = Lab_field("priphys", stb, lpht, cy_save + yadj, nlen, pl)
  End If

  If lab1.ContainsKey("secph:") Then
   stb = New StyledTextBuilder
   stb.Append(lab1("secph_title:"), bld_font, Colors.Black)
   If Not lab1("secph_title:").EndsWith(":") Then
    stb.Append(": ", bld_font, Colors.Black)
   End If
   stb.Append(Me.secph.Text, reg_font, Colors.Black)
   currenty = Lab_field("spec1", stb, 317, currenty + yadj, ll, 191)
   yadj = 0
  End If

  If Not Me.ins.Text = String.Empty Then
   stb = New StyledTextBuilder
   stb.Append("Insurance: ", bld_font, Colors.Black)
   stb.Append(Me.ins.Text, reg_font, Colors.Black)
   currenty = Lab_field("insurance", stb, 317, currenty + yadj, ll, 191)
   yadj = 0
  End If

  Output_label(pjob, Label, labelno)
  currenty = New_label(Label, labelno)

  'stb = New StyledTextBuilder

  For Each cb As Control In Me.Controls
   If Not cb.Name.StartsWith("GB") Then
    Continue For
   End If

   ' If String.IsNullOrEmpty(cb.Controls(0).Text) Or String.IsNullOrEmpty(cb.Controls(1).Text) Then
   If String.IsNullOrEmpty(cb.Controls(1).Text) Then
    Continue For
   End If

   Dim bn As Integer = (From kvp As KeyValuePair(Of Integer, String) In gpn Where kvp.Value = cb.Name Select kvp).First().Key

   If Label Is Nothing Then
    currenty = New_label(Label, labelno)
   End If

   If labelno > labels_number Then
    Exit For
   End If

   If String.IsNullOrEmpty(cb.Controls(0).Text) Then
    y_space_needed = nlead
   Else
    y_space_needed = nlead * 2
   End If

   If currenty > label_header And Not String.IsNullOrEmpty(cb.Controls(0).Text) Then
    y_space_needed += nlead / 2
   End If

   If (ylimit - currenty) < y_space_needed Then
    Output_label(pjob, Label, labelno)
    currenty = New_label(Label, labelno)
    If labelno > labels_number Then
     Exit For
    End If
   End If

   If currenty > label_header And Not String.IsNullOrEmpty(cb.Controls(0).Text) Then
    currenty += nlead / 2
   End If

   stb = New StyledTextBuilder

   If Not String.IsNullOrEmpty(cb.Controls(0).Text) Then
    stb.Append(cb.Controls(0).Text, bld_font, Colors.Black)
    currenty = Lab_field("L" & currenty.ToString, stb, 317, currenty, ll, nlead)
   Else
    currenty += nlead
   End If

   lis = cb.Controls(1).Text

   elines.Clear()

   If lines_setting = "Label" Then
    lis = Adjust_lines(lis, "File")
   End If

   lis = Adjust_lines(lis, "Label", elines)

   li = Regex.Split(lis, "\r\n|\n")

   Dim lic As Integer = li.Count

   If max_rec(bn) > 0 And lic > max_rec(bn) Then
    lic = max_rec(bn)
    Dim iadj As Integer = 0
    For Each kvp As KeyValuePair(Of Integer, Integer) In elines
     If kvp.Key <= lic Then
      iadj = iadj + kvp.Value
     End If
    Next
    lic = lic + iadj
   End If

   For i As Integer = 0 To li.Count - 1 Step 1
    If String.IsNullOrEmpty(li(i)) Then
     currenty += nlead
    Else
     stb = New StyledTextBuilder
     stb.Append(li(i), reg_font, Colors.Black)
     currenty = Lab_field("L" & currenty.ToString, stb, 317, currenty, ll, nlead)
    End If

    If (ylimit - currenty) < nlead Then
     Output_label(pjob, Label, labelno)
     currenty = New_label(Label, labelno)
     If labelno > labels_number Then
      Exit For
     End If
    End If
   Next
  Next

  If currenty > label_header Then
   Output_label(pjob, Label, labelno)
  End If

  If Not Preview Then
   pjob.Print()
  Else
   pjob = Nothing
  End If

 End Sub

 Private Function Setcy(gb As GroupBox) As Integer
  Dim br As Integer = gb.Location.Y + gb.Height + 76
  Dim cy As Integer = currenty
  If br > cy Then
   cy = br
  End If

  Return cy
 End Function

 Public Sub Setupdn(ByRef cm As ContextMenuStrip, bn As Integer, ni As Integer)
  Dim ctb As TSnumud = New TSnumud
  Dim a1 As Ath_block = Nothing
  Dim tsi As ToolStripMenuItem = New ToolStripMenuItem
  Dim tss As ToolStripSeparator = New ToolStripSeparator
  Dim tsl As ToolStripLabel = New ToolStripLabel("Lines to print")

  tsi.Text = "Activate max lines"
  tsi.Name = bn.ToString
  tsi.Tag = ni + 3
  AddHandler tsi.Click, AddressOf Cud_activate
  cm.Items.Add(tsi)
  cm.Items.Add(tss)
  cm.Items.Add(tsl)

  ctb.Name = bn.ToString
  ctb.Size = New System.Drawing.Size(100, 25)
  ctb.udl = ni + 3
  ctb.aml = ni

  a1 = ath_blist.Find(Function(p) p.num = bn)
  If Not a1 Is Nothing Then
   max_rec(a1.num) = a1.max_lines
   ctb.Value = a1.max_lines
  End If

  cm.Items.Add(ctb)

  AddHandler ctb.ValueChanged, AddressOf Cudchange
 End Sub

 Public Async Sub Form1_LoadAsync(sender As Object, e As EventArgs) Handles MyBase.Load

  Dim xsize As Integer = 365
  Dim ysize As Integer = 192
  Dim tboxsz As Integer = 20

  Dim blockno As Integer = 1
  Dim i As Integer
  Dim k As Integer
  Dim fs As String = Nothing
  Dim tsi As ToolStripMenuItem
  Dim ins As ToolStripMenuItem
  Dim swp As ToolStripMenuItem
  Dim cm As ContextMenuStrip
  Dim obox As GroupBox
  Dim o1 As Object = Nothing
  Dim fnam As String = Nothing
  Dim abp As Ath_block = Nothing
  Dim l1cfn As String = Nothing

  loading = True

  If run_timer Then
   Tgp.Visible = True
  End If

  MyBase.Visible = False

  Me.DoubleBuffered = True

  file_access = False

  enck = New String(cv)

  Dim key As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Medical_Profile")

  drive_label_encoded = Nothing

  eval_encoded = Nothing

  fnam = Nothing

  ath_blist.Clear()

  installed_version = key.GetValue("version", Nothing)


#If DEBUG Then
  If Not My.Settings.User_number = String.Empty Then
   testmode = True
   Handle_testmode(My.Settings.User_number)
  End If
#End If

  If Not testmode Then
   eval_encoded = key.GetValue("eval", Nothing)

   If eval_encoded IsNot Nothing Then
    file_access = True
   End If

   If Not file_access Then
    drive_label_encoded = key.GetValue("Label", Nothing)
    If drive_label_encoded Is Nothing Then
     Using f4 As Form = New Formaik(cv)
      f4.ShowDialog()
      eval_encoded = key.GetValue("eval", Nothing)
      If Not eval_encoded = Nothing Then
       file_access = True
      End If
     End Using
     If Not file_access Then
      MsgBox("Application can not run", MsgBoxStyle.OkOnly, "Registry entry is missing")
      Me.Close()
      End
     End If
    End If
   End If

   If file_access Then
    If Handle_file(eval_encoded) <> 0 Then
     Application.Exit()
     End
    End If
   Else
    If Handle_usb(drive_label_encoded) <> 0 Then
     Application.Exit()
     End
    End If
   End If
  End If

  If My.Settings.endpoints Is Nothing Then
   My.Settings.endpoints = New Specialized.StringCollection
  End If

  If My.Settings.endpoints.Count = 0 Then
   Dim json As String = JsonConvert.SerializeObject(Mpck, Formatting.Indented)

   For Each s As String In Mpck.Blocklist
    My.Settings.endpoints.Add(s)
   Next
   My.Settings.Save()
  End If

  For Each s As String In My.Settings.endpoints
   Endpoints_in_use.Add(s)
  Next

  secph.Text = ""
  sp.Text = ""

  If String.IsNullOrEmpty(installed_version) Then
   installed_version = Mpck.Version
  End If

  If Mpck.Sec_visible = "True" Then
   sp.Visible = True
   If Not String.IsNullOrEmpty(Mpck.Sptitle) Then
    sp.Text = Mpck.Sptitle
   Else
    sp.Text = "Specialist"
   End If
   secph.Visible = True
   secph.Enabled = True
   secph.BackColor = System.Drawing.Color.White
  Else
   sp.Visible = False
   secph.Visible = False
   secph.Enabled = False
  End If

  minimum_blocks = Mpck.Minimum_blocks
  blocks_number = Mpck.Blocks
  blocks_number = 9
  labels_number = Mpck.Labels
  lines_number = Mpck.Lines

  lines_number = 10
  labels_number = 5

  total_lines = lines_number * labels_number

  Patientid.Text = ""

  Dim ystart As Integer = Me.GroupBox3.Location.Y + Me.GroupBox3.Size.Height + margin2

  originaly = Me.Size.Height - 36

  MyBase.SuspendPaint()

  For i = ystart To 3 * ystart + ysize + margin1 Step ysize + margin1
   For k = margin2 To margin2 + 3 * xsize + 2 * margin2 Step xsize + margin2

    Dim bi As Blk_info = New Blk_info With {
      .num = blockno
    }

    blocks(blockno) = bi

    max_rec(blockno) = 0

    fieldsmr("MR" & blockno.ToString & ":") = blockno

    obox = New GroupBox With {
      .Size = New Size(xsize, ysize),
      .Location = New Point(k, i),
      .Name = "GB" + blockno.ToString(),
      .Tag = blockno
    }
    gpn(blockno) = obox.Name
    cm = New ContextMenuStrip

    AddHandler cm.Leave, AddressOf Cm_leave

    tsi = New ToolStripMenuItem With {
      .Text = "Drop This Block",
      .Name = blockno.ToString,
      .Tag = 0
    }
    AddHandler tsi.Click, AddressOf Menudrp_Click
    cm.Items.Add(tsi)
    ins = New ToolStripMenuItem With {
      .Text = "Insert Block Before",
      .Name = blockno.ToString,
      .Tag = 1
    }
    AddHandler ins.Click, AddressOf Menuins_Click
    cm.Items.Add(ins)
    swp = New ToolStripMenuItem With {
      .Text = "Swap Block Before",
      .Name = blockno.ToString,
      .Tag = 2
    }
    AddHandler swp.Click, AddressOf Menuswp_Click
    cm.Items.Add(swp)
    obox.ContextMenuStrip = cm
    Setupdn(cm, blockno, cm.Items.Count)

    Dim hbox As TextBox = New TextBox With {
      .Text = " ",
      .Size = New Size(xsize - 2 * margin1, tboxsz),
      .Location = New Point(margin1, margin3),
      .Name = obox.Name + "H:",
      .Tag = blockno,
      .Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte)),
      .Enabled = False
    }
    gph(blockno) = hbox.Name
    fieldsgb.Add(hbox.Name)
    AddHandler hbox.Enter, AddressOf Textbox_enter
    AddHandler hbox.Leave, AddressOf Leave_fld
    cm = New ContextMenuStrip

    AddHandler cm.Leave, AddressOf Cm_leave

    tsi = New ToolStripMenuItem With {
      .Text = "Drop This Block",
      .Name = blockno.ToString
    }
    AddHandler tsi.Click, AddressOf Menudrp_Click
    cm.Items.Add(tsi)
    ins = New ToolStripMenuItem With {
      .Text = "Insert Block Before",
      .Name = blockno.ToString
    }
    AddHandler ins.Click, AddressOf Menuins_Click
    cm.Items.Add(ins)
    swp = New ToolStripMenuItem With {
      .Text = "Swap Block Before",
      .Name = blockno.ToString
    }
    AddHandler swp.Click, AddressOf Menuswp_Click
    cm.Items.Add(swp)

    Setupdn(cm, blockno, cm.Items.Count)

    hbox.ContextMenuStrip = cm
    obox.Controls.Add(hbox)

    Dim rbox As RichTextBox = New RichTextBox With {
      .Text = " ",
      .WordWrap = False,
      .Size = New Size(xsize - 2 * margin1, ysize - 2 * margin1 - margin3 - tboxsz),
      .Location = New Point(margin1, tboxsz + margin1 + margin3),
      .Name = obox.Name + "B:",
      .Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte)),
      .Enabled = False
    }
    gpb(blockno) = rbox.Name
    fieldsgb.Add(rbox.Name)
    AddHandler rbox.Enter, AddressOf Rtbbox_enter
    AddHandler rbox.Leave, AddressOf Leave_fld
    cm = New ContextMenuStrip

    AddHandler cm.Leave, AddressOf Cm_leave

    tsi = New ToolStripMenuItem With {
      .Text = "Drop This Block",
      .Name = blockno.ToString
    }
    AddHandler tsi.Click, AddressOf Menudrp_Click
    cm.Items.Add(tsi)
    ins = New ToolStripMenuItem With {
      .Text = "Insert Block Before",
      .Name = blockno.ToString
    }
    AddHandler ins.Click, AddressOf Menuins_Click
    cm.Items.Add(ins)
    swp = New ToolStripMenuItem With {
      .Text = "Swap Block Before",
      .Name = blockno.ToString
    }
    AddHandler swp.Click, AddressOf Menuswp_Click
    cm.Items.Add(swp)

    Setupdn(cm, blockno, cm.Items.Count)

    rbox.ContextMenuStrip = cm
    obox.Controls.Add(rbox)

    Me.Controls.Add(obox)

    bl_available(blockno) = obox.Name

    blockno += 1
   Next
  Next

  cm = New ContextMenuStrip

  Dim Eti As ToolStripMenuItem = New ToolStripMenuItem With
  {
   .Text = "Use Timer",
   .Name = "Eltimermi"
  }

  AddHandler Eti.Click, AddressOf Eltimer_set

  cm.Items.Add(Eti)

  GroupBox2.ContextMenuStrip = cm

  points = 8

  reg_font = New FontInfo("Calibri", points, FontStyle.None)

  rp1_font = New FontInfo("Calibri", points + 1, FontStyle.None)

  bld_font = New FontInfo("Calibri", points, FontStyle.Bold)

  bp1_font = New FontInfo("Calibri", points + 1, FontStyle.Bold)

  itl_font = New FontInfo("Calibri Light Italic", points, FontStyle.Italic)

  Try
   Label = DYMO.Label.Framework.Framework.Open("mpc1.label")

   For Each o As String In Label.ObjectNames
    If Not String.IsNullOrEmpty(o) Then
     lab1(o) = Label.GetObjectText(o)
    End If
   Next
  Catch ex As Exception
   MsgBox("xxx", ex.Message)
  End Try

  Printers.Items.Clear()

  pnames = Framework.GetPrinters

  For Each p In pnames
   Printers.Items.Add(p.Name)
  Next

  If Printers.Items.Count > 0 Then
   Printers.SelectedIndex = 0
  End If

  printer = pnames.GetPrinterByName(Printers.SelectedItem)

  If TypeOf printer Is ILabelWriterPrinter Then
   labelWriterPrinter = printer
   lprintparams = New LabelWriterPrintParams With {
     .Copies = 1,
     .FlowDirection = FlowDirection.LeftToRight,
     .RollSelection = RollSelection.Auto
   }
  End If

  Reset_fields()

  MyBase.ResumePaint()

  If My.Settings.first_run_flag Then
   My.Settings.first_run_flag = False
   My.Settings.Save()
   Close()
  End If

  priph.Visible = True
  prv_combo.Visible = False

  Me.Text = "Medical Profile Card (" + installed_version + ")"

  Dim claims = Gen_Claims()

  aws_body.Clear()

  aws_body("ukey") = Enc.Encrypt(cid + Mpck.Dlab, cid + Mpck.Dlab, Mpck.Iterations)

  Me.Text = "Medical Profile Card (" + installed_version + ")" + " - Loading Practice Information"
  Me.Update()

  If run_timer Then
   Ettb.Text = start_timer(SW)
  End If

  Application.UseWaitCursor = True
  L1_ret = Await Aws.Get_Level1_aysnc(Mpck.Url, enck, Mpck.Salt, claims, aws_body)
  Application.UseWaitCursor = False

  If L1_ret.code <> 200 Then
   MsgBox("Returned Code(" & L1_ret.code.ToString & ") - " & L1_ret.message, MsgBoxStyle.OkOnly, "Error getting practice information")
   Application.Exit()
  End If

  Me.Text = "Medical Profile Card (" + installed_version + ")"

  Me.Update()

  If L1_ret.Prc Is Nothing Then
   MsgBox("Unable to get practice information", MsgBoxStyle.OkOnly, "Error in information returned")
   Application.Exit()
  End If

  Dstate = Data_state.Edit_mode

  Me.Practice.Text = L1_ret.Prc.name

  Load_Department(L1_ret)

  Load_providers(L1_ret)

  For Each s As String In L1_ret.Block_names
   Dim b As Blk_entry = New Blk_entry With {
     .State = Load_state.not_in_use,
     .Num = -1
   }
   bl_loaded(s) = b
  Next

  Set_saved_items(L1_ret.dsl)

  Me.Patientid.Select()

  loading = False
  Data_altered = False

  Dstate = Data_state.NoEdit

  priph.Visible = True
  prv_combo.Visible = False

  If run_timer Then
   Ettb.Text = stop_timer(SW)
  End If
 End Sub

 Private Function Gen_Claims() As Dictionary(Of String, Object)
  Dim nw As DateTimeOffset = DateTimeOffset.UtcNow
  Dim ew As DateTimeOffset = nw.AddMinutes(5)
  'Dim ew As DateTimeOffset = nw.AddYears(1)
  Dim provider As IDateTimeProvider = New UtcDateTimeProvider()
  Dim payload = New Dictionary(Of String, Object) From
                         {
                            {"aud", "http: //medicalprofilecard.com"},
                            {"exp", ew.ToUnixTimeSeconds()},
                            {"iss", Mpck.Email},
                            {"label", drive_label},
                            {"key", Mpck.Mkey}
                         }

  If file_access Then
   payload("cid") = Enc.Enc256.Encrypt(cid, Set_key(Mpck.Secret, Mpck.Dlab), Convert.ToInt32(Mpck.Dlab.Substring(3)) + Mpck.Iterations)

   payload("f1t") = USB_Ftime.ToUnixTimeSeconds()
  End If

  If ftime IsNot Nothing AndAlso ftime > 0 <> 0 Then
   payload("f1t") = ftime
  End If

  Return payload
 End Function

 Private Sub Printers_SelectionChangeCommitted(sender As Object, e As EventArgs)

 End Sub

 Private Sub ExitMenuItem_Click(sender As Object, e As EventArgs) Handles exmenuitem.Click
  Close()
 End Sub

 Public Sub Set_ll(rtb As RichTextBox, bn As Integer)
  Dim ln As Integer = 0
  Dim bi As Blk_info = blocks(bn)

  For i As Integer = 0 To bi.lines - 1 Step 1
   bi.ll(i) = ln
   ln = ln + rtb.Lines(i).Length + 1
  Next
 End Sub

 Private Sub Redo_blocks(ls As String)
  Dim lnum As Integer = 0
  Dim c0l As Single
  Dim c1l As Single
  Dim mrkl As Integer
  Dim pline As Integer
  Dim rtb As RichTextBox
  Dim tb As TextBox
  Dim ln As Integer = 0
  Dim bi As Blk_info = Nothing
  Dim bn As Integer = 0

  For Each cb As Control In Me.Controls
   If (cb.Name.StartsWith("GB") AndAlso cb.Visible) Then

    bn = (From kvp As KeyValuePair(Of Integer, String) In gpn Where kvp.Value = cb.Name Select kvp).First().Key

    rtb = cb.Controls(1)
    tb = cb.Controls(0)

    bi = blocks(bn)

    bi.lines = rtb.Lines.Length

    bi.ll = New Integer(bi.lines) {}

    ln = 0

    Set_ll(rtb, bn)

    If ls = "File" Then
     rtb.ReadOnly = False
     tb.ReadOnly = False
     tb.Enabled = True

     rtb.Text = labgb(cb.Name + "B:")

     tb.Text = labgb(cb.Name + "H:")

     bi.lines = rtb.Lines.Length

     bi.ll = New Integer(bi.lines) {}

     Set_ll(rtb, bn)

     Reset_color(bn)

     Fe_setcolor(bn)
    Else
     labgb(cb.Name + "B:") = rtb.Text
     labgb(cb.Name + "H:") = tb.Text

     rtb.Text = Adjust_lines(rtb.Text, ls)

     bi.lines = rtb.Lines.Length

     bi.ll = New Integer(bi.lines) {}

     Set_ll(rtb, bn)

     Reset_color(bn)

     mrkl = Fe_setcolor(bn)

     If (cb.Controls(0).Text.Length > 0) Then
      c0l = Wlengb(cb.Controls(0).Text.ToString())
     Else
      c0l = 0.0
     End If

     If rtb.Lines.Count > 0 Then
      c1l = Wleng(rtb.Lines(0))
     Else
      c1l = 0.0
     End If

     If label_length < (c0l + c1l + Wleng(" ")) Then
      lnum += 1
     End If

     If total_lines > lnum + mrkl Then
      lnum += mrkl
     Else
      pline = total_lines - lnum
      If pline < 0 Then
       pline = 0
      End If
      Setcolor(bn, pline)
      lnum += mrkl
     End If

     rtb.ReadOnly = True
     tb.ReadOnly = True
     tb.Enabled = False
    End If
   End If
  Next
 End Sub

 Private Sub DocumentMenuItem_Click(sender As Object, e As EventArgs)
  lines_setting = "File"
  My.Settings.lines_setting = lines_setting
  My.Settings.Save()
  Redo_blocks(lines_setting)
 End Sub

 Private Sub LabelMenuItem_Click(sender As Object, e As EventArgs)
  lines_setting = "Label"
  My.Settings.lines_setting = lines_setting
  My.Settings.Save()
  Redo_blocks(lines_setting)
 End Sub

 Private Sub LblPrintMenuItem_Click(sender As Object, e As EventArgs)
  Reset_labels()
 End Sub

 Private Sub LblDrawMenuItem_Click(sender As Object, e As EventArgs)
  Reset_labels()
 End Sub

 Private Sub Leave_fld(sender As Object, e As EventArgs)
  Dim rtb As RichTextBox
  Dim tb As TextBox

  Dim field As String = sender.name

  If Not field.StartsWith("GB") Then
   Return
  End If

  If sender.GetType().Name = "RichTextBox" Then
   rtb = sender
   tb = rtb.Parent.Controls(0)
   If labgb.ContainsKey(field) Then
    If Not labgb(field) = rtb.Text Then
     labgb(field) = rtb.Text
     If bl_loaded.ContainsKey(tb.Tag) Then
      bl_loaded(tb.Tag).Num = Nothing
      bl_loaded(tb.Tag).State = Load_state.not_loaded_byc
     End If
     Set_empgb()
    End If
   Else
    labgb(field) = rtb.Text
   End If
  Else
   tb = sender
   If tb.Tag.ToString() <> tb.Text Then
    If bl_loaded.ContainsKey(tb.Tag) Then
     bl_loaded(tb.Tag).Num = Nothing
     bl_loaded(tb.Tag).State = Load_state.not_loaded_byc
    End If
    Set_empgb()
   End If
   If labgb.ContainsKey(field) Then
    If Not labgb(field) = tb.Text Then
     labgb(field) = tb.Text
     '  Data_altered = True
    End If
   Else
    labgb(field) = tb.Text
    '   Data_altered = True
   End If
  End If
 End Sub

 Public Sub Load_block_click(sender As Object, e As EventArgs)

  Dim item As ToolStripMenuItem = DirectCast(sender, ToolStripMenuItem)
  Dim blk As String = DirectCast(item.Tag, String)

  If bl_used.Count = blocks_number Then
   MsgBox("Unable to load " & blk, MsgBoxStyle.Information, "All Blocks are in use")
  Else
   If L2_ret.blks.ContainsKey(blk) AndAlso L2_ret.blks(blk).Count > 0 Then
    Dim fnum As Integer = bl_available.First.Key

    bl_used(fnum) = bl_available.First.Value

    Dim lnum As Integer = bl_used.Getlkey

    bl_available.Remove(fnum)

    Me.Controls(gpn(lnum)).Visible = False

    Me.Controls(gpn(lnum)).Controls(0).Text = blk
    Me.Controls(gpn(lnum)).Controls(0).Tag = blk
    Me.Controls(gpn(lnum)).Controls(0).Enabled = True

    Me.Controls(gpn(lnum)).Controls(1).Text = String.Join(vbCrLf, L2_ret.blks(blk))
    Me.Controls(gpn(lnum)).Controls(1).Enabled = True

    Ath_setcolor(lnum)

    Me.Controls(gpn(lnum)).Visible = True

    labgb(gph(lnum)) = blk
    labgb(gpb(lnum)) = String.Join(vbCrLf, L2_ret.blks(blk))

    bl_loaded(blk).Num = lnum

    bl_loaded(blk).State = Load_state.loaded

   End If

   Scsiz(Me.Width)

   Me.Update()

   Set_empgb()
  End If
 End Sub

 Private Sub Menudrp_Click(sender As Object, e As EventArgs)

  Dim objn As Integer = CType(sender, ToolStripMenuItem).Name

  Dim gb As GroupBox = Me.Controls(gpn(objn))

  Dim Tag As String = gb.Controls(0).Tag

  If bl_loaded.ContainsKey(Tag) Then
   If bl_loaded(Tag).State = Load_state.loaded_byc Then
    bl_loaded(Tag).State = Load_state.not_loaded_byc
   End If

   If bl_loaded(Tag).State = Load_state.loaded Then
    bl_loaded(Tag).State = Load_state.not_loaded
   End If
  End If

  For ib As Integer = objn To bl_used.Getlkey - 1 Step 1
   Move_blk(ib + 1, ib)
  Next

  labgb(gph(bl_used.Getlkey)) = String.Empty
  labgb(gpb(bl_used.Getlkey)) = String.Empty

  Me.Controls(bl_used.Getlval).Visible = False

  bl_available(bl_used.Getlkey) = bl_used.Getlval

  bl_used.Remove(bl_used.Getlkey)

  Set_empgb()

  Scsiz(Me.Width)
 End Sub

 Private Sub Move_blk(fbn As Integer, tbn As Integer)
  Dim gbf As GroupBox = Me.Controls(gpn(fbn))
  Dim gbt As GroupBox = Me.Controls(gpn(tbn))

  gbt.Controls(0).Tag = gbf.Controls(0).Tag
  gbt.Enabled = True

  If bl_loaded.ContainsKey(gbt.Controls(0).Tag) Then
   bl_loaded(gbt.Controls(0).Tag).Num = tbn
   bl_loaded(gbt.Controls(0).Tag).State = Load_state.loaded
  End If

  gbt.Controls(0).Enabled = True
  gbt.Controls(1).Enabled = True

  labgb(gph(tbn)) = labgb(gph(fbn))
  labgb(gpb(tbn)) = labgb(gpb(fbn))

  gbt.Controls(0).Text = gbf.Controls(0).Text
  gbt.Controls(1).Text = gbf.Controls(1).Text

  gbf.Controls(0).Tag = gbf.Tag

  gbf.Controls(0).Text = String.Empty
  gbf.Controls(1).Text = String.Empty

  gbf.Controls(0).Enabled = False
  gbf.Controls(1).Enabled = False

  Set_ro(gbt.Controls(0))
  Set_ro(gbt.Controls(1))

 End Sub

 Private Sub Menuins_Click(sender As Object, e As EventArgs)

  Dim objn As Integer = CType(sender, ToolStripMenuItem).Name

  Dim gb As GroupBox = Me.Controls(gpn(objn))

  If bl_used.Count = blocks_number Then
   MsgBox("Unable to insert block", MsgBoxStyle.OkOnly, "All blocks are in use")
   Return
  End If

  bl_used(bl_available.First.Key) = bl_available.First.Value
  bl_available.Remove(bl_available.First.Key)

  For ii As Integer = bl_used.Getlkey - 1 To objn Step -1
   Move_blk(ii, ii + 1)
  Next

  labgb(gph(objn)) = String.Empty
  labgb(gpb(objn)) = String.Empty

  gb.Controls(0).Text = String.Empty
  gb.Controls(1).Text = String.Empty

  gb.Controls(0).Enabled = True
  gb.Controls(1).Enabled = True

  gb.Controls(0).Tag = gb.Tag

  Me.Controls(gpn(bl_used.Getlkey)).Visible = True

  Set_empgb()

  Scsiz(Me.Width)

 End Sub

 Private Sub Menuswp_Click(sender As Object, e As EventArgs)

  Dim objn As Integer = CType(sender, ToolStripMenuItem).Name

  If objn = 1 Then
   Return
  End If

  Dim gb1 As GroupBox = Me.Controls(gpn(objn))
  Dim gb2 As GroupBox = Me.Controls(gpn(objn - 1))

  Dim n1 As String = gb1.Tag
  Dim n2 As String = gb2.Tag

  Dim tag1 As String = gb1.Controls(0).Tag
  Dim tag2 As String = gb2.Controls(0).Tag

  If bl_loaded.ContainsKey(tag2) Then
   bl_loaded(tag2).Num = objn
  End If

  If bl_loaded.ContainsKey(tag1) Then
   bl_loaded(tag1).Num = objn - 1
  End If

  Dim sh As String = labgb(gph(objn - 1))
  Dim sb As String = labgb(gpb(objn - 1))

  labgb(gph(objn - 1)) = labgb(gph(objn))
  labgb(gpb(objn - 1)) = labgb(gpb(objn))

  labgb(gph(objn)) = sh
  labgb(gpb(objn)) = sb

  Me.Controls(gpn(objn - 1)).Controls(0).Text = labgb(gph(objn - 1))
  Me.Controls(gpn(objn - 1)).Controls(1).Text = labgb(gpb(objn - 1))

  Set_ro(Me.Controls(gpn(objn - 1)).Controls(0))
  Set_ro(Me.Controls(gpn(objn - 1)).Controls(1))

  Me.Controls(gpn(objn)).Controls(0).Text = labgb(gph(objn))
  Me.Controls(gpn(objn)).Controls(1).Text = labgb(gpb(objn))

  Set_ro(Me.Controls(gpn(objn)).Controls(0))
  Set_ro(Me.Controls(gpn(objn)).Controls(1))

  Set_empgb()

 End Sub

 Private Sub Showlabel(sender As Object, e As EventArgs)
  Dim bn As String = CType(sender, ToolStripMenuItem).Name
  If f3s.ContainsKey(bn) Then
   f3s(bn).ShowDialog()
  End If
 End Sub

 Private Sub Cudchange(sender As Object, e As EventArgs)
  Dim objn As Integer = CType(sender, TSnumud).Name
  Dim tsi As TSnumud = sender
  Dim tg1 As Integer = tsi.aml
  Dim tgno As Integer = tsi.udl

  max_rec(tsi.Name) = tsi.Value

  Redo_blocks(lines_setting)

  If tsi.Value = 0 Then
   tsi.Owner.Items(tsi.udl - 2).Enabled = False
   tsi.Owner.Items(tsi.udl - 2).Visible = False
   tsi.Owner.Items(tsi.udl - 1).Enabled = False
   tsi.Owner.Items(tsi.udl - 1).Visible = False
   tsi.Enabled = False
   tsi.Visible = False
   tsi.Owner.Items(tsi.aml).Enabled = True
   tsi.Owner.Items(tsi.aml).Visible = True
  End If
 End Sub

 Private Sub Cm_leave(sender As Object, e As EventArgs)
  Dim cm As ContextMenuStrip = sender

  cm.Close()

 End Sub

 Private Sub Cm_leave_m(sender As Object, e As EventArgs)
  Dim cm As ContextMenuStrip = sender

  cm.Close()

 End Sub

 Private Sub Cud_activate(sender As Object, e As EventArgs)
  Dim tsi As ToolStripMenuItem
  Dim tud As TSnumud

  tsi = sender
  tud = tsi.Owner.Items(tsi.Tag)

  tsi.Owner.Items(tud.udl - 2).Enabled = True
  tsi.Owner.Items(tud.udl - 2).Visible = True
  tsi.Owner.Items(tud.udl - 1).Enabled = True
  tsi.Owner.Items(tud.udl - 1).Visible = True

  tud.Enabled = True
  tud.Visible = True
  tsi.Enabled = False
  tsi.Visible = False
 End Sub

 Private Sub Set_ro(ByVal ctrl As Windows.Forms.Control)
  Dim tb As TextBox
  Dim rb As RichTextBox

  If TypeOf ctrl Is TextBox Then
   tb = ctrl
   If Editmenuitem.Text = "Edit" Then
    tb.ReadOnly = True
    tb.BackColor = System.Drawing.Color.White
   Else
    tb.ReadOnly = False
   End If
  End If

  If TypeOf ctrl Is RichTextBox Then
   rb = ctrl
   If Editmenuitem.Text = "Edit" Then
    rb.ReadOnly = True
    rb.BackColor = System.Drawing.Color.White
   Else
    rb.ReadOnly = False
   End If
  End If
 End Sub

 Private Sub AddBlockMenuItem_Click(sender As Object, e As EventArgs) Handles Addpagemenuitem.Click
  Dim bn As Integer

  If bl_used.Count = blocks_number Then
   MsgBox("Unable to add page", MsgBoxStyle.Information, "All are in use")
  Else
   bn = bl_available.First.Key

   bl_available.Remove(bn)

   bl_used(bn) = gpn(bn)

   Me.Controls(gpn(bn)).Controls(0).Text = String.Empty
   Me.Controls(gpn(bn)).Controls(1).Text = String.Empty

   Me.Controls(gpn(bn)).Controls(0).Enabled = True
   Me.Controls(gpn(bn)).Controls(1).Enabled = True

   Me.Controls(gpn(bn)).Visible = True

   Set_ro(Me.Controls(gpn(bn)).Controls(0))
   Set_ro(Me.Controls(gpn(bn)).Controls(1))

   labgb(gph(bn)) = String.Empty
   labgb(gpb(bn)) = String.Empty

   Set_empgb()

   Scsiz(Me.Width)

  End If
 End Sub

 Private Sub Clear_set()
  For Each cb As Control In Me.Controls
   If TypeOf (cb) Is GroupBox Then
    For Each cb1 As Control In cb.Controls
     If TypeOf (cb1) Is TextBox Or TypeOf (cb1) Is RichTextBox Then
      If cb1.Font.Italic Then
       cb1.Text = String.Empty
       RemoveHandler cb1.Enter, AddressOf Field_click
       If bfont(cb1.Name) Then
        cb1.Font = New Font("Calibri", 8.25F, Drawing.FontStyle.Bold)
       Else
        cb1.Font = New Font("Calibri", 8.25F, Drawing.FontStyle.Regular)
       End If
      End If
     End If
    Next
   End If
  Next
 End Sub

 Private Sub Set_field(blk As Object, val As String)

  blk.Text = val

  If blk.Font.Bold Then
   bfont(blk.Name) = True
  Else
   bfont(blk.Name) = False
  End If

  If TypeOf (blk) Is TextBox Then
   Dim blk1 As TextBox = blk
   AddHandler blk1.Enter, AddressOf Field_click
  End If

  If TypeOf (blk) Is RichTextBox Then
   Dim blk1 As RichTextBox = blk
   AddHandler blk1.Enter, AddressOf Field_click
  End If

  blk.Font = New Font("Calibri Light", 8.25F, Drawing.FontStyle.Italic)

 End Sub

 Private Sub Field_click(ByVal sender As Object, ByVal e As System.EventArgs)
  Dim blk As TextBox = CType(sender, TextBox)
  If blk.Font.Italic Then
   blk.Text = String.Empty
   RemoveHandler blk.Enter, AddressOf Field_click
   If bfont(blk.Name) Then
    blk.Font = New Font("Calibri", 8.25F, Drawing.FontStyle.Bold)
   Else
    blk.Font = New Font("Calibri", 8.25F, Drawing.FontStyle.Regular)
   End If
  End If
 End Sub

 Private Sub ClearMenuItem_Click(sender As Object, e As EventArgs) Handles Clrmenuitem.Click
  Me.SuspendPaint()

  file_clear = True

  Reset_labels()

  Reset_fields()

  Patient.Text = ""

  Patientid.Text = ""

  Department.SelectedText = Nothing
  Department.Text = Nothing

  Data_altered = False

  RemoveHandler dsaves.SelectedIndexChanged, AddressOf Dsaves_SelectedIndexChanged
  dsaves.SelectedItem = Nothing
  dsaves.SelectedText = ""
  AddHandler dsaves.SelectedIndexChanged, AddressOf Dsaves_SelectedIndexChanged

  Me.ResumePaint()
 End Sub

 Private Sub CheckMenuItem(ByVal mnu As ToolStripMenuItem, ByVal checked_item As ToolStripMenuItem)
  For Each item As ToolStripItem In mnu.DropDownItems
   If (TypeOf item Is ToolStripMenuItem) Then
    Dim menu_item As ToolStripMenuItem = DirectCast(item, ToolStripMenuItem)
    menu_item.Checked = (menu_item Is checked_item)
   End If
  Next item
 End Sub

 Private Sub Econtact_Leave(sender As Object, e As EventArgs)
  lab1("Emergency Contact:") = Me.econtact.Text
 End Sub

 Private Sub Patient_TextChanged(sender As Object, e As EventArgs)
  lab1("Patient Name:") = Me.Patient.Text
 End Sub

 Private Sub DOB_TextChanged(sender As Object, e As EventArgs) Handles DOB.TextChanged
  lab1("DOB:") = Me.DOB.Text
 End Sub

 Private Sub Econtact_TextChanged(sender As Object, e As EventArgs) Handles econtact.TextChanged
  lab1("Emergency Contact:") = Me.econtact.Text
 End Sub

 Private Sub Phone_TextChanged(sender As Object, e As EventArgs) Handles Phone.TextChanged
  lab1("Patient Phone:") = Me.Phone.Text
 End Sub

 Private Sub Priph_TextChanged(sender As Object, e As EventArgs) Handles priph.TextChanged
  lab1("Primary Physician:") = Me.priph.Text
 End Sub

 Private Sub Ins_TextChanged(sender As Object, e As EventArgs) Handles ins.TextChanged
  lab1("Insurance:") = Me.ins.Text
 End Sub

 Private Sub Secph_TextChanged(sender As Object, e As EventArgs) Handles secph.TextChanged
  lab1("secph:") = Me.secph.Text
 End Sub

 Private Sub Address_TextChanged(sender As Object, e As EventArgs) Handles address.TextChanged
  lab1("Patient Address:") = Me.address.Text
 End Sub

 Private Sub Sp_Title_Click(sender As Object, e As EventArgs) Handles Aspecmenuitem.Click
  If (sptitle.Visible) Then
   sptitle.Visible = False
  Else
   sptitle.Text = sp.Text
   sptitle.Visible = True
  End If
 End Sub

 Private Sub Sptitle_Leave(sender As Object, e As EventArgs) Handles sptitle.Leave
  If sptitle.Text.Length > 0 Then
   sp.Text = sptitle.Text
   sp.Visible = True
   lab1("secph_title:") = sp.Text
   secph.Enabled = True
   secph.Visible = True
   sptitle.Visible = False
  End If
 End Sub

 Private Sub Sptitle_KeyPress(sender As Object, e As KeyPressEventArgs) Handles sptitle.KeyPress
  If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Return) Then
   SendKeys.Send("{TAB}")
   e.Handled = True
  End If
 End Sub

 Public Sub Reset_color(bn As Integer)
  Dim bx As RichTextBox = Nothing
  Dim tb As TextBox
  Dim bi As Blk_info = Nothing

  tb = Me.Controls(gpn(bn)).Controls(0)
  bx = Me.Controls(gpn(bn)).Controls(1)

  bi = blocks(bn)

  bx.SelectAll()
  bx.SelectionBackColor = System.Drawing.Color.White
  bx.Select(0, 0)

  tb.BackColor = System.Drawing.Color.White

  bi.hv = Nothing
  bi.lv = Nothing

  bi.lines = bx.Lines.Length
 End Sub

 Public Function Fe_setcolor(bn As Integer) As Integer
  Dim bx As RichTextBox = Nothing
  Dim bi As Blk_info = Nothing

  bx = Me.Controls(gpn(bn)).Controls(1)
  bi = blocks(bn)

  If Not gpn.ContainsKey(bn) Then
   Return bi.lines
  End If

  If max_rec(bn) <= 0 Or bi.ll.Length <= max_rec(bn) Then
   Return bi.lines
  End If

  bx.Select(0, bi.ll(max_rec(bn)))

  bx.SelectionBackColor = mp_backcolor

  Application.DoEvents()

  bx.SelectionLength = 0
  bx.Select(0, 0)

  Return max_rec(bn)
 End Function

 Public Sub ChangeMySelectionColor()

 End Sub

 Private Function Ath_setcolor(bn As Integer) As Integer
  Dim ln As Integer = 0
  Dim lines As Integer = Nothing
  Dim ll() As Integer = Nothing
  Dim hx As TextBox = Nothing
  Dim bx As RichTextBox = Nothing

  hx = Me.Controls(gpn(bn)).Controls(0)
  bx = Me.Controls(gpn(bn)).Controls(1)

  ll = New Integer(bx.Lines.Length) {}

  lines = bx.Lines.Length

  If Not gpn.ContainsKey(bn) Then
   Return lines
  End If

  bx.SelectAll()
  bx.SelectionBackColor = System.Drawing.Color.White
  bx.Select(0, 0)

  hx.BackColor = System.Drawing.Color.White

  If Not max_rec.ContainsKey(bn) Then
   Return lines
  End If

  If max_rec(bn) <= 0 Or ll.Length <= max_rec(bn) Then
   Return lines
  End If

  For i As Integer = 0 To lines - 1 Step 1
   ll(i) = ln
   ln = ln + bx.Lines(i).Length + 1
  Next

  Application.DoEvents()

  bx.Select(0, ll(max_rec(bn)))

  bx.SelectionBackColor = mp_backcolor

  Application.DoEvents()

  bx.SelectionLength = 0
  bx.Select(0, 0)

  Return max_rec(bn)

 End Function

 Public Sub Add_cm(name As String, cm As ContextMenuStrip, ts1 As ToolStripMenuItem, ts2 As ToolStripMenuItem)
  ts1 = New ToolStripMenuItem With {
    .Name = "Block" + name,
    .Text = "Show " + name,
    .Tag = name
  }
  AddHandler ts1.Click, AddressOf Load_block_click

  ts2 = New ToolStripMenuItem With {
    .Name = "Block" + name,
    .Text = "Show " + name,
    .Tag = name
  }
  AddHandler ts2.Click, AddressOf Load_block_click
  cm.Items.Add(ts2)
 End Sub

 Public Sub Set_empgb(Optional res_endpoints As Boolean = False)
  Dim gb As GroupBox = Nothing
  Dim lb As GroupBox = Nothing

  Dim cm As ContextMenuStrip = New ContextMenuStrip()
  Dim ts1 As ToolStripMenuItem = Nothing
  Dim ts2 As ToolStripMenuItem = Nothing

  Dim pnxstart As Integer = 0
  Dim pnystart As Integer = 0
  Dim pnxsize As Integer = 0
  Dim pnysize As Integer = 0

  Dim bnum As Integer = 0

  Dim bmni As List(Of ToolStripMenuItem) = New List(Of ToolStripMenuItem)

  bmni.Clear()

  If emppn IsNot Nothing Then
   Me.Controls.Remove(emppn)
   emppn.Dispose()
  End If

  For Each tsi As ToolStripMenuItem In bmni
  Next

  emppn = Nothing

  If bl_used.Count Mod 3 <> 0 Then
   lb = Me.Controls(bl_used.Getlval)
   emppn = New Panel With {
     .Name = "emppn"
   }
   pnxstart = lb.Location.X + lb.Width + 1
   If (bl_used.Count < 3) Then
    pnystart = lb.Location.Y - 2 * margin1 + 1
    gb = Me.Controls("GB3")
   Else
    gb = Me.Controls(("GB" & (bl_used.Count \ 3) * 3).ToString)
    pnystart = gb.Location.Y + gb.Height + 1
   End If
   pnxsize = (gb.Location.X + gb.Width) - pnxstart
   pnysize = (lb.Location.Y + lb.Height) - pnystart
   emppn.Location = New Point(pnxstart, pnystart)
   emppn.Size = New Size(pnxsize, pnysize)
  End If

  Dim Rebuild_Endpoints As Boolean = False

  For Each k As KeyValuePair(Of String, Blk_entry) In bl_loaded
   Dim b As Blk_entry = k.Value

   If L2_ret Is Nothing Then
    Continue For

   End If
   If Not L2_ret.blks.ContainsKey(k.Key) Then
    Continue For
   End If

   If L2_ret.blks(k.Key).Count = 0 Then
    Continue For
   End If

   If b.State = Load_state.not_loaded Or b.State = Load_state.not_loaded_byc Or b.State = Load_state.not_in_use Then
    Add_cm(k.Key, cm, ts1, ts2)
    Continue For
   End If

   If Me.Controls(gpn(b.Num)).Controls(1).Text = "" Then
    Continue For
   End If
  Next

  If emppn IsNot Nothing Then
   emppn.ContextMenuStrip = cm
   Me.Controls.Add(emppn)
  End If

  If res_endpoints Then
   Dim tbiu As Specialized.StringCollection = New Specialized.StringCollection

   For Each k As KeyValuePair(Of String, Blk_entry) In bl_loaded
    If k.Value.State <> Load_state.not_loaded Then
     tbiu.Add(k.Key)
    End If
   Next

   If tbiu.Count <> Endpoints_in_use.Count Then
    My.Settings.endpoints = New Specialized.StringCollection
    For Each s As String In tbiu
     My.Settings.endpoints.Add(s)
    Next
    My.Settings.Save()
   End If
   Me.Update()
  End If
  Scsiz(Me.Width)
 End Sub

 Private Async Sub Patientid_Leave(sender As Object, e As EventArgs)
  Dim claims = Gen_Claims()
  Dim fs As String = Nothing
  Dim fn As String = Nothing
  aws_body.Clear()
  If Editmenuitem.Text = "End Edit" Then
   Return
  End If

  If Patientid.Text.Length = 0 Then
   Patient.ReadOnly = False
  End If

  If Not Regex.IsMatch(Patientid.Text, "^[0-9 ]+$") Then
   Return
  End If

  If Not rethit And Not tabhit Then
   Patientid.Text = ""
   Return
  End If

  aws_body("patientid") = Me.Patientid.Text

  aws_body("departmentid") = atdp(Me.Department.SelectedItem.ToString())

  Reset_fields()

  Me.Text = "Medical Profile Card (" + installed_version + ")" + " - Loading Patient Information"
  Me.Update()

  Application.UseWaitCursor = True
  L2_ret = Await Aws.Get_Level2_aysnc(Mpck.Url, enck, Mpck.Salt, claims, aws_body)
  Application.UseWaitCursor = False

  If L2_ret.code <> 200 Then
   MsgBox(L2_ret.message, MsgBoxStyle.OkOnly, "Medical Profile Card")
   Reset_fields()
   Me.Patientid.Text = ""
   Patientid.Select()
   Return
  End If

  Me.Text = "Medical Profile Card (" + installed_version + ")"
  Me.Update()

  Load_level2(L2_ret)
 End Sub

 Private Async Sub Patientid_Click(sender As Object, e As MouseEventArgs) Handles Patientid.Click
  If Editmenuitem.Text = "End Edit" Then
   Return
  End If

  If Check_altered() Then
   Await save_blk()
   Application.DoEvents()
  End If

  Data_altered = False
  Dsi = New KeyValuePair(Of String, Dsave)

  If inpid Then
   Return
  End If

  inpid = True
  inpat = False

  L2_ret = Nothing

  Patient.ReadOnly = True
  Patient.Enabled = False
  Patient.BackColor = System.Drawing.Color.White
  Patient.Text = ""
  patep.SetError(Patient, "")

  Patientid.ReadOnly = False
  Patientid.Enabled = True
  Patientid.BackColor = System.Drawing.Color.White

  rethit = False
  tabhit = False

  If Not inpid Then
   Patientid.Text = ""
  End If

  Reset_fields()

  Deletemi.Enabled = False
  Savemi.Enabled = False

  RemoveHandler dsaves.SelectedIndexChanged, AddressOf Dsaves_SelectedIndexChanged
  dsaves.SelectedItem = Nothing
  AddHandler dsaves.SelectedIndexChanged, AddressOf Dsaves_SelectedIndexChanged

  Scsiz(Me.Width, originaly)

  Me.Invalidate()
  Me.Update()
 End Sub

 Private Async Sub Patientid_Enter(sender As Object, e As EventArgs) Handles Patientid.Enter
  If Editmenuitem.Text = "End Edit" Then
   Return
  End If

  If Check_altered() Then
   Await save_blk()
   Application.DoEvents()
  End If

  Data_altered = False

  If inpid Then
   Return
  End If

  Patientid.SelectionStart = 0

  inpid = True

  L2_ret = Nothing

  inpat = False

  Patient.ReadOnly = True
  Patient.BackColor = System.Drawing.Color.White
  Patient.Text = ""
  patep.SetError(Patient, "")

  Patientid.ReadOnly = False

  rethit = False
  tabhit = False
  If valid_patientid Then
   Patientid.Text = ""
  End If
  Reset_fields()
  Scsiz(Me.Width, originaly)
  Me.Invalidate()
  Me.Update()
 End Sub

 Private Sub Cnt_set(ctrl As Control, enb As Boolean, ro As Boolean)
  Dim tb As TextBox
  Dim rb As RichTextBox

  If TypeOf ctrl Is TextBox Then
   tb = ctrl
   If ro Then
    tb.ReadOnly = True
   Else
    tb.ReadOnly = False
   End If
   tb.BackColor = System.Drawing.Color.White
  End If

  If TypeOf ctrl Is RichTextBox Then
   rb = ctrl
   If ro Then
    rb.ReadOnly = True
   Else
    rb.ReadOnly = False
   End If
   rb.BackColor = System.Drawing.Color.White
  End If

  If enb Then
   ctrl.Enabled = True
  Else
   ctrl.Enabled = False
  End If

 End Sub

 Private Sub Patientid_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Patientid.KeyPress
  If Editmenuitem.Text = "End Edit" Then
   Data_altered = True
   Return
  End If

  If Patientid.Text.Length = 0 Then
   pidep.SetError(Patientid, "")
  End If

  If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Return) Then
   rethit = True
   SendKeys.Send("{TAB}")
   e.Handled = True
  End If

  If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Tab) Then
   tabhit = True
   dest.Select()
   e.Handled = True
  End If
 End Sub

 Private Sub ResetToolStripMenuItem_Click(sender As Object, e As EventArgs)

  For Each k As KeyValuePair(Of Integer, String) In bl_used
   bl_available(k.Key) = k.Value
  Next

  bl_used.Clear()

  For Each k As KeyValuePair(Of String, Blk_entry) In bl_loaded
   k.Value.State = Load_state.not_loaded
   k.Value.Num = 0
  Next

  My.Settings.endpoints = Nothing

  For Each s As KeyValuePair(Of Integer, String) In bl_available
   Me.Controls(s.Value).Visible = False

   Me.Controls(s.Value).Controls(0).Text = String.Empty
   Me.Controls(s.Value).Controls(1).Text = String.Empty
  Next

  Scsiz(Me.Width, originaly)

  My.Settings.endpoints = New Specialized.StringCollection

  For Each s As String In Mpck.Blocklist
   My.Settings.endpoints.Add(s)
  Next

  My.Settings.Save()

  Endpoints_in_use = My.Settings.endpoints

  If L2_ret IsNot Nothing Then

   For Each b As String In Endpoints_in_use

    If L2_ret.blks.ContainsKey(b) AndAlso L2_ret.blks(b).Count > 0 Then
     Dim bnum As Integer = bl_available.First.Key

     bl_used(bl_available.First.Key) = bl_available.First.Value
     bl_available.Remove(bl_available.First.Key)

     Me.Controls(gpn(bnum)).Visible = False

     Me.Controls(gpn(bnum)).Controls(0).Text = b
     Me.Controls(gpn(bnum)).Controls(0).Tag = b
     Me.Controls(gpn(bnum)).Controls(0).Enabled = True

     bl_loaded(b).State = Load_state.loaded
     bl_loaded(b).Num = bnum

     Me.Controls(gpn(bnum)).Controls(1).Text = String.Join(vbCrLf, L2_ret.blks(b))
     Me.Controls(gpn(bnum)).Controls(1).Enabled = True

     Ath_setcolor(bnum)

     Me.Controls(gpn(bnum)).Visible = True

     labgb(gph(bnum)) = b
     labgb(gpb(bnum)) = String.Join(vbCrLf, L2_ret.blks(b))
    End If
   Next
  End If

  Scsiz(Me.Width)

  Set_empgb()

 End Sub

 Public Sub Scsiz(Width As Integer, Optional Height As Integer = -1)
  Dim lb As GroupBox
  Dim br As Integer

  If Height <> -1 Then
   br = Height
  Else
   If bl_used.Count = 0 Then
    br = originaly
   Else
    lb = Me.Controls(bl_used.Getlval)
    br = lb.Location.Y + lb.Height + 76
    If bl_used.Count > 3 Then
     If (Me.Top + br) > Screen_height Then
      dnh = False
      Wanted_hgt = br
      lb = Me.Controls(bl_used(bl_used.Count - 3))
      br = lb.Location.Y + lb.Height + 76
     Else
      dnh = True
      Wanted_hgt = 0
     End If
    End If
   End If
  End If
  Me.Size = New System.Drawing.Size(Width, br)
  Me.Update()
 End Sub

 Private Sub Previewmenuitem_Click(sender As Object, e As EventArgs) Handles Previewmenuitem.Click
  Dim pfrm As Form = New Form()
  Dim pccnt As Integer
  Dim prcnt As Integer
  Dim pb As PictureBox
  Dim gblabels() As String = {"First Label", "Second Label", "Third Label", "Fourth Label", "Fifth Label", "Sixth Label"}
  Dim pnl As Panel
  Dim Cms As ContextMenuStrip = Nothing
  Dim Tsi As ToolStripMenuItem = Nothing

  Using fpr As Form = New Form3

   Using tbl As TableLayoutPanel = New TableLayoutPanel()

    Dim i1 As System.Drawing.Bitmap
    Dim i2 As System.Drawing.Bitmap

    fpr.Text = "Labels Preview"

    Pnglablist.Clear()

    Preview = True

    Generate_Labels()

    If Pnglablist.Count < 1 Then
     Return
    End If

    pccnt = If(Pnglablist.Count < 2, 1, 2)

    prcnt = (Pnglablist.Count + 1) \ pccnt

    tbl.RowCount = prcnt
    tbl.ColumnCount = pccnt

    tbl.Width = 504 * pccnt
    tbl.Height = 337 * prcnt

    fpr.Width = tbl.Width
    fpr.Height = tbl.Height

    For i As Integer = 0 To prcnt - 1 Step 1
     For j As Integer = 0 To pccnt - 1 Step 1
      Dim ind As Integer = i * pccnt + j
      If (ind) > (Pnglablist.Count - 1) Then
       Exit For
      End If

      pnl = New Panel

      pb = New PictureBox With
      {
        .Width = 464,
        .Height = 277,
        .Left = 15,
        .Top = 15,
        .Anchor = AnchorStyles.None
      }

      Using MS As New MemoryStream(Pnglablist(i * pccnt + j))
       i1 = Image.FromStream(MS)
       i2 = ResizeImage(i1, i1.Width / 2, i1.Height / 2)
       pb.Image = i2
      End Using

      pnl.Width = 504
      pnl.Height = 337

      Dim lab As System.Windows.Forms.Label = New System.Windows.Forms.Label With
      {
        .Text = gblabels(ind)
      }
      pnl.Controls.Add(lab)
      pnl.Controls(0).Left = 20
      pnl.Controls(0).Top = 20

      pnl.Controls.Add(pb)
      pb.CenterControl()
      pnl.Controls(1).Left = 20
      pnl.Controls(1).Top = 45

      Cms = New ContextMenuStrip()

      Tsi = New ToolStripMenuItem With
      {
       .Text = "Print",
       .Name = gblabels(ind)
      }

      AddHandler Tsi.Click, AddressOf Form3.Print_Panel

      Cms.Items.Add(Tsi)

      pnl.ContextMenuStrip = Cms

      tbl.Controls.Add(pnl, j, i)
     Next
    Next

    fpr.Controls.Add(tbl)

    fpr.ShowDialog()
   End Using
  End Using

  Preview = False
 End Sub

 Private Sub Editmenuitem_Click(sender As Object, e As EventArgs) Handles Editmenuitem.Click
  Dim bset As Boolean

  If Me.Editmenuitem.Text = "Edit" Then
   Me.Editmenuitem.Text = "End Edit"
   bset = False
   Dstate = Data_state.Edit_mode
   Me.dsaves.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None
   Me.dsaves.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None
   Patientid.Enabled = True
   Patientid.ReadOnly = False
   Patientid.BackColor = System.Drawing.Color.White
   Patient.Enabled = True
   Patient.ReadOnly = False
   Patient.BackColor = System.Drawing.Color.White
   inpat = False
   inpid = False
  Else
   Me.Editmenuitem.Text = "Edit"
   bset = True
   Dstate = Data_state.NoEdit
   Me.dsaves.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
   Me.dsaves.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
   Patientid.Enabled = False
   Patientid.ReadOnly = True
   Patientid.BackColor = System.Drawing.Color.White
   Patient.Enabled = False
   Patient.ReadOnly = True
   Patient.BackColor = System.Drawing.Color.White
  End If

  'If prv_combo.Items.Count > 1 Then
  ' priph.Visible = False
  ' prv_combo.Visible = True
  'Else
  ' prv_combo.Visible = False
  ' priph.Visible = True
  'End If

  If bset And prv_combo.Items.Count > 1 Then
   prv_combo.Visible = False
   priph.Visible = True
  Else
   prv_combo.Visible = True
   priph.Visible = False
  End If

  Me.Patientid.Enabled = True
  Me.ins.ReadOnly = bset
  Me.Patient.ReadOnly = bset
  Me.Practice.ReadOnly = bset
  Me.Patient.Enabled = True
  Me.Phone.ReadOnly = bset
  Me.DOB.ReadOnly = bset
  Me.address.ReadOnly = bset
  Me.econtact.ReadOnly = bset

  If sp.Visible Then
   Me.secph.ReadOnly = bset
  End If

  For Each k As KeyValuePair(Of Integer, String) In bl_used
   Dim tb As TextBox = Me.Controls(k.Value).Controls(0)
   tb.ReadOnly = bset
   Dim rb As RichTextBox = Me.Controls(k.Value).Controls(1)
   rb.ReadOnly = bset
   Me.Refresh()
  Next
 End Sub

 Private Sub Notesmenuitem_Click(sender As Object, e As EventArgs) Handles Notesmenuitem.Click
  Dim bn As Integer

  If bl_used.Count = blocks_number Then
   MsgBox("Unable to Notes Page", MsgBoxStyle.Information, "All are in use")
  Else
   bn = bl_available.First.Key

   bl_available.Remove(bn)

   bl_used(bn) = gpn(bn)

   Me.Controls(gpn(bn)).Controls(0).Text = String.Empty
   Me.Controls(gpn(bn)).Controls(1).Text = String.Empty

   Me.Controls(gpn(bn)).Controls(0).Enabled = True
   Me.Controls(gpn(bn)).Controls(1).Enabled = True

   Me.Controls(gpn(bn)).Controls(0).Text = "Notes"

   Me.Controls(gpn(bn)).Visible = True

   Set_ro(Me.Controls(gpn(bn)).Controls(0))
   Set_ro(Me.Controls(gpn(bn)).Controls(1))

   labgb(gph(bn)) = Me.Controls(gpn(bn)).Controls(0).Text
   labgb(gpb(bn)) = String.Empty

   Set_empgb()

   Scsiz(Me.Width)

  End If
 End Sub

 Private Sub Eltimer_set(sender As Object, e As EventArgs)
  Dim T As ToolStripMenuItem = sender
  If run_timer Then
   T.Text = "Use timer"
   run_timer = False
   Tgp.Visible = False
  Else
   T.Text = "Stop using timer"
   run_timer = True
   Tgp.Visible = True
  End If
 End Sub

 Private Sub Printmenuitem_Click(sender As Object, e As EventArgs) Handles Printmenuitem.Click
  Generate_Labels()
 End Sub

 Private Sub Edit_Keypress(sender As Object, e As KeyPressEventArgs) Handles priph.KeyPress, Phone.KeyPress, ins.KeyPress, econtact.KeyPress, DOB.KeyPress, address.KeyPress, Practice.KeyPress
  If Not Editmenuitem.Text = "End Edit" Then
   e.Handled = True
  Else
   Data_altered = True
  End If
 End Sub

 Private Sub Prv_combo_KeyPress(sender As Object, e As KeyPressEventArgs) Handles prv_combo.KeyPress
  If Not Editmenuitem.Text = "End Edit" Then
   e.Handled = True
  End If
  Data_altered = True
 End Sub

 Private Sub Printers_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Printers.KeyPress
  If Not Editmenuitem.Text = "End Edit" Then
   e.Handled = True
  Else
   Data_altered = True
  End If
 End Sub

 Private Sub Practice_KeyPress(sender As Object, e As KeyPressEventArgs)
  If Not Editmenuitem.Text = "End Edit" Then
   e.Handled = True
  Else
   Data_altered = True
  End If
 End Sub

 Private Sub Department_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Department.KeyPress
  If Not Editmenuitem.Text = "End Edit" Then
   e.Handled = True
  Else
   Data_altered = True
  End If
 End Sub

 Private Sub Dept_tbox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles dept_tbox.KeyPress
  If Not Editmenuitem.Text = "End Edit" Then
   e.Handled = True
  Else
   Data_altered = True
  End If
 End Sub

 Private Sub Patientid_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Patientid.Validating
  If Editmenuitem.Text = "End Edit" Then
   Return
  End If

  If Not tabhit And Not rethit Then
   Return
  End If

  If Data_altered Then
   Return
  End If

  valid_patientid = True

  If Patientid.Text.Length = 0 Then
   Patient.ReadOnly = False
   Return
  End If

  If Not Regex.IsMatch(Patientid.Text, "^[0-9 ]+$") Then
   valid_patientid = False
  End If

  If Not valid_patientid Then
   e.Cancel = True
   Patientid.Select(0, Patientid.Text.Length)
   pidep.SetError(Patientid, "A Patient id must consist of only numbers")
  End If
 End Sub

 Private Async Sub Patientid_Validated(sender As Object, e As EventArgs) Handles Patientid.Validated
  Dim claims = Gen_Claims()
  Dim fs As String = Nothing
  Dim fn As String = Nothing

  If Editmenuitem.Text = "End Edit" Then
   Return
  End If

  If Data_altered Then
   Return
  End If

  If Not tabhit And Not rethit Then
   Return
  End If

  If valid_patientid Then
   pidep.SetError(Patientid, "")

   If Patientid.Text.Length = 0 Then
    Return
   End If

   aws_body.Clear()

   aws_body("patientid") = Me.Patientid.Text

   If Me.Department.SelectedIndex > 0 Then
    aws_body("departmentid") = atdp(Me.Department.SelectedItem.ToString())
   End If

   Reset_fields()

   Me.Text = "Medical Profile Card (" + installed_version + ")" + " - Loading Patient Information"

   Me.Update()

   If run_timer Then
    Ettb.Text = start_timer(SW)
   End If

   Application.UseWaitCursor = True
   L2_ret = Await Aws.Get_Level2_aysnc(Mpck.Url, enck, Mpck.Salt, claims, aws_body)
   Application.UseWaitCursor = False

   If L2_ret.code <> 200 Then
    MsgBox(L2_ret.message, MsgBoxStyle.OkOnly, "Medical Profile Card")
    Reset_fields()
    pidep.SetError(Patientid, "Patient not found")
    tabhit = False
    rethit = False
    Patientid.Select(0, Patientid.Text.Length)
    Patientid.HideSelection = False
    Patientid.Focus()
    Return
   End If

   Me.Text = "Medical Profile Card (" + installed_version + ")"

   Me.Update()

   Load_level2(L2_ret)
   If run_timer Then
    Ettb.Text = stop_timer(SW)
   End If
   inpid = False
  End If
 End Sub

 Private Async Sub Patient_Validated(sender As Object, e As EventArgs) Handles Patient.Validated
  Dim claims = Gen_Claims()
  Dim fs As String = Nothing
  Dim fn As String = Nothing

  If Editmenuitem.Text = "End Edit" Then
   Return
  End If

  If Not tabhit And Not rethit Then
   Return
  End If

  If Data_altered Then
   Return
  End If

  If valid_patient Then
   patep.SetError(Patient, "")

   aws_body.Clear()

   aws_body("firstname") = firstname

   aws_body("lastname") = lastname

   If Not Me.Department.SelectedIndex = 0 Then
    aws_body("departmentid") = atdp(Me.Department.SelectedItem.ToString())
   End If

   Me.Text = "Medical Profile Card (" + installed_version + ")" + " - Loading Patient Information"

   Me.Update()

   If run_timer Then
    Ettb.Text = start_timer(SW)
   End If

   Application.UseWaitCursor = True
   L2_ret = Await Aws.Get_Level2_aysnc(Mpck.Url, enck, Mpck.Salt, claims, aws_body)
   Application.UseWaitCursor = False

   If L2_ret.code <> 200 Then
    MsgBox(L2_ret.message, MsgBoxStyle.OkOnly, "Medical Profile Card")
    Reset_fields()
    patep.SetError(Patient, "Patient not found")
    tabhit = False
    rethit = False
    Patient.Select(0, Patient.Text.Length)
    Patient.HideSelection = False
    Patient.Focus()
    Return
   End If

   Me.Text = "Medical Profile Card (" + installed_version + ")"

   Me.Update()

   Load_level2(L2_ret)

   If run_timer Then
    Ettb.Text = stop_timer(SW)
   End If

   inpat = False
  End If
 End Sub

 Private Sub Patient_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Patient.Validating
  Dim np As String()
  Dim nl As String()

  firstname = ""
  lastname = ""

  If Editmenuitem.Text = "End Edit" Then
   Return
  End If

  If Not tabhit And Not rethit Then
   Return
  End If

  If Data_altered Then
   Return
  End If

  If Patient.Text.Length = 0 Then
   Return
  End If

  np = Patient.Text.Split(New Char() {","c})

  nl = Patient.Text.Split(New Char() {" "c})

  If np.Count = 2 Then
   lastname = np(0)
   firstname = np(1)
  Else
   If nl.Count = 2 Then
    firstname = nl(0)
    lastname = nl(1)
   End If
  End If

  If lastname = "" Or firstname = "" Then
   valid_patient = False
  Else
   valid_patient = True
  End If

  If Not valid_patient Then
   e.Cancel = True
   Patient.Select(0, Patient.Text.Length)
   patep.SetError(Patient, "A patient name must be either <first> <last> or <last>,<first>")
  End If
 End Sub

 Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles MyBase.Closing
  e.Cancel = False
 End Sub

 Private Sub Rtbbox_enter(sender As Object, e As EventArgs) Handles address.Enter
  Dim rtb As RichTextBox
  rtb = sender
  rtb.SelectionStart = 0
 End Sub

 Private Sub Textbox_enter(sender As Object, e As EventArgs) Handles sptitle.Enter, secph.Enter, Practice.Enter, Phone.Enter, ins.Enter, econtact.Enter, DOB.Enter, dept_tbox.Enter, dept_tbox.Click
  Dim tb As TextBox
  tb = sender
  tb.SelectionStart = tb.Text.Length
  tb.SelectionLength = 0
 End Sub

 Private Async Sub Patient_Enter(sender As Object, e As EventArgs) Handles Patient.Enter

  Dim tb As TextBox

  If Editmenuitem.Text = "End Edit" Then
   Return
  End If

  If Check_altered() Then
   Await save_blk()
   Application.DoEvents()
   Reset_fields()
  End If

  Data_altered = False
  Dsi = New KeyValuePair(Of String, Dsave)

  tb = sender

  tb.SelectionStart = 0

  If inpat Then
   Return
  End If

  valid_patient = False

  patep.SetError(Patient, "")

  L2_ret = Nothing

  inpid = False

  Patientid.ReadOnly = True
  Patientid.BackColor = System.Drawing.Color.White
  Patientid.Text = ""
  pidep.SetError(Patientid, "")

  Patient.ReadOnly = False

  rethit = False
  tabhit = False

  If Not inpat Then
   Patient.Text = ""
  End If

  valid_patient = False
  inpat = True

  Reset_fields()

  Deletemi.Enabled = False
  Savemi.Enabled = False

  RemoveHandler dsaves.SelectedIndexChanged, AddressOf Dsaves_SelectedIndexChanged
  dsaves.SelectedItem = Nothing
  AddHandler dsaves.SelectedIndexChanged, AddressOf Dsaves_SelectedIndexChanged

  Scsiz(Me.Width, originaly)
  Me.Invalidate()
  Me.Update()
 End Sub

 Private Async Sub Patient_Click(sender As Object, e As EventArgs) Handles Patient.Click
  If Editmenuitem.Text = "End Edit" Then
   Return
  End If

  If Check_altered() Then
   Await save_blk()
   Application.DoEvents()
  End If

  Dsi = New KeyValuePair(Of String, Dsave)

  Data_altered = False

  L2_ret = Nothing

  If inpat Then
   Return
  End If

  inpat = True
  inpid = False

  Patientid.ReadOnly = True
  Patientid.Enabled = False
  Patientid.BackColor = System.Drawing.Color.White
  Patientid.Text = ""

  pidep.SetError(Patientid, "")

  Patient.ReadOnly = False
  Patient.Enabled = True
  Patient.BackColor = System.Drawing.Color.White

  If Not inpat Then
   Patient.Text = ""
  End If

  rethit = False
  tabhit = False

  Reset_fields()
  Scsiz(Me.Width, originaly)
  Me.Invalidate()
  Me.Update()
 End Sub

 Private Sub Patient_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Patient.KeyPress

  If Editmenuitem.Text = "End Edit" Then
   Data_altered = True
   Return
  End If

  patep.SetError(Patient, "")

  If Patient.Text.Length = 0 Then
   patep.SetError(Patient, "")
  End If

  If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Return) Then
   rethit = True
   SendKeys.Send("{TAB}")
   e.Handled = True
  End If

  If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Tab) Then
   tabhit = True
   dest.Select()
   e.Handled = True
  End If

  valid_patient = False
 End Sub

 Private Async Sub Savemi_Click(sender As Object, e As EventArgs) Handles Savemi.Click

  If run_timer Then
   Ettb.Text = start_timer(SW)
  End If

  Dim claims = Gen_Claims()
  Dim Wrtim = DateTime.Now.ToString("yyyyMMddHHmmss")
  Dim Sblk As Save_blk = New Save_blk
  Dim Sb As Sblock
  Dim js As String
  Dim jse As String
  Dim dr As Dsave_return

  'Dim dsi As KeyValuePair(Of String, Dsave) = Nothing
  ' Dim ds As Dsave = Nothing

  If saved_type Then
   ' Dsi = dsaves.SelectedItem
   Ds = Dsi.Value
   If String.IsNullOrEmpty(Patient.Text) Then
    Patient.Text = Ds.Name
   End If
  End If

  If String.IsNullOrEmpty(Patient.Text) Then
   Patient.Text = "Unknown_" & Wrtim
  End If

  If Not String.IsNullOrEmpty(Patient.Text) AndAlso Not Ds Is Nothing AndAlso Not String.IsNullOrEmpty(Ds.Name) Then
   If Not Patient.Text = Ds.Name Then
    saved_type = False
   End If
  End If

  Sblk.Practice = Practice.Text

  If Department.Items.Count > 1 Then
   Sblk.Department = Department.Text
  Else
   Sblk.Department = dept_tbox.Text
  End If

  Sblk.Patient = Me.Patient.Text
  Sblk.Patient_id = Me.Patientid.Text
  Sblk.Address = Me.address.Text
  Sblk.DOB = Me.DOB.Text
  Sblk.Emergency_contact = Me.econtact.Text
  Sblk.Insurance = Me.ins.Text

  If atpv.Count > 1 Then
   Sblk.Priph = prv_combo.Text
  Else
   Sblk.Priph = Me.priph.Text
  End If

  Sblk.Phone = Phone.Text

  Sblk.Prtitle = ppgb.Text

  Sblk.Sptitle = Me.sptitle.Text
  Sblk.Secph = Me.secph.Text
  Sblk.Sec_visible = Mpck.Sec_visible

  For Each s As KeyValuePair(Of Integer, String) In bl_used
   Sb = New Sblock
   With Sb
    .num = s.Key
    .header = Me.Controls(s.Value).Controls(0).Text
    .body = Me.Controls(s.Value).Controls(1).Text
   End With
   Sblk.Blk_list.Add(Sb)
  Next

  aws_body.Clear()

  js = JsonConvert.SerializeObject(Sblk, Formatting.Indented)

  aws_body("skey") = Enc.Encrypt(Sblk.Patient, Enc.Iterscramble(cid + Mpck.Dlab, Mpck.Iterations))

  Select Case save_ver
   Case 1
    jse = Enc.Encrypt(js, Enc.Iterscramble(cid), Convert.ToInt32((Mpck.Iterations Mod 10) + 2))
    aws_body("skey") = Enc.Encrypt(Sblk.Patient, Enc.Iterscramble(cid + Mpck.Dlab), (Mpck.Iterations Mod 10 + 2))
   Case Else
    jse = Enc.Encrypt(js, Enc.Iterscramble(cid, Mpck.Iterations), Convert.ToInt32(Mpck.Iterations / 3))
    aws_body("skey") = Enc.Encrypt(Sblk.Patient, Enc.Iterscramble(cid + Mpck.Dlab, Mpck.Iterations))
  End Select

  aws_body("ukey") = Enc.Encrypt(cid + Mpck.Dlab, cid + Mpck.Dlab, Mpck.Iterations)

  If saved_type Then
   If String.IsNullOrEmpty(Ds.lwtim) Then
    aws_body("wrtim") = Wrtim
    aws_body("lwtim") = Wrtim
   Else
    aws_body("wrtim") = Ds.wrtim
    aws_body("lwtim") = Wrtim
   End If
  Else
   aws_body("wrtim") = Wrtim
   aws_body("lwtim") = Wrtim
  End If

  aws_body("vers") = save_ver.ToString()

  aws_body("value") = jse

  Application.UseWaitCursor = True
  dr = Await Aws.Pat_save_aysnc(Mpck.Url, enck, Mpck.Salt, claims, aws_body)
  Application.UseWaitCursor = False

  Add_saved_item(dr.ds(0))

  Data_altered = False

  If run_timer Then
   Ettb.Text = stop_timer(SW)
  End If

 End Sub

 Private Async Sub Deletemi_Click(sender As Object, e As EventArgs) Handles Deletemi.Click
  Dim dsv As Dsave = dsaves.SelectedValue
  Dim Dsvsv As KeyValuePair(Of String, Dsave) = dsaves.SelectedItem
  Dim claims = Gen_Claims()
  Dim Dr As Dsave_return

  Reset_fields()

  aws_body.Clear()
  aws_body("ukey") = Enc.Encrypt(cid + Mpck.Dlab, cid + Mpck.Dlab, Mpck.Iterations)
  aws_body("wrtim") = dsv.wrtim

  Application.UseWaitCursor = True
  Dr = Await Aws.Pat_delete_aysnc(Mpck.Url, enck, Mpck.Salt, claims, aws_body)
  Application.UseWaitCursor = False

  Delete_saved_item(Dsvsv.Key)

  Data_altered = False

  Me.Patient.Text = Nothing
  Me.Patientid.Text = ""
  Me.Patientid.Focus()
 End Sub

 Private Sub WireAllEvents(ByVal obj As Object)
  Dim parameterTypes() As Type = {GetType(System.Object), GetType(System.EventArgs)}
  Dim Events = obj.GetType().GetEvents()
  For Each ev In Events
   If ev.Name.StartsWith("Format") Then
    Continue For
   End If
   If ev.Name.StartsWith("Mouse") Then
    Continue For
   End If
   Dim handler As New DynamicMethod("", Nothing, parameterTypes, GetType(Form1))
   Dim ilgen As ILGenerator = handler.GetILGenerator()
   ilgen.EmitWriteLine("Event Name: " + ev.Name)
   ilgen.Emit(OpCodes.Ret)
   ev.AddEventHandler(obj, handler.CreateDelegate(ev.EventHandlerType))
  Next
 End Sub

 Public Async Sub Dsaves_SelectedIndexChanged(sender As Object, e As EventArgs) Handles dsaves.SelectedIndexChanged
  Dim cb As ComboBox = sender

  dsaves.SelectionStart = dsaves.Text.Length

  If Not String.IsNullOrEmpty(Dsi.Key) Then
   Dim Ti As KeyValuePair(Of String, Dsave) = dsaves.SelectedItem
   If Dsi.Key = Ti.Key Then
    dsaves.SelectionStart = dsaves.Text.Length
    dest.Select()
    Return
   End If
  End If

  If run_timer Then
   Ettb.Text = start_timer(SW)
  End If

  If Data_altered Then
   If Check_altered() Then
    Dim ds_index As Integer = dsaves.SelectedIndex
    Await save_blk()
    Application.DoEvents()
    RemoveHandler dsaves.SelectedIndexChanged, AddressOf Dsaves_SelectedIndexChanged
    dsaves.SelectedIndex = ds_index
    AddHandler dsaves.SelectedIndexChanged, AddressOf Dsaves_SelectedIndexChanged
   End If
  End If

  Dim dsv As Dsave = dsaves.SelectedValue
  Dim claims = Gen_Claims()
  Dim Dvr As Dsave_return
  Dim Sblk As Save_blk = New Save_blk
  Dim js As String = Nothing

  Dim svsel As Object = dsaves.SelectedItem

  Reset_fields(True)

  dest.Select()

  RemoveHandler dsaves.SelectedIndexChanged, AddressOf Dsaves_SelectedIndexChanged
  dsaves.SelectedItem = svsel
  dsaves.SelectionStart = dsaves.SelectionLength
  AddHandler dsaves.SelectedIndexChanged, AddressOf Dsaves_SelectedIndexChanged

  Patient.Text = Nothing
  Patient.ReadOnly = True
  Patientid.BackColor = System.Drawing.Color.White

  Patientid.Text = ""
  Patientid.ReadOnly = True

  inpat = False
  inpid = False

  aws_body.Clear()
  aws_body("ukey") = Enc.Encrypt(cid + Mpck.Dlab, cid + Mpck.Dlab, Mpck.Iterations)
  aws_body("skey") = dsv.Skey
  aws_body("wrtim") = dsv.wrtim

  Application.UseWaitCursor = True
  Dvr = Await Aws.Dsave_get_aysnc(Mpck.Url, enck, Mpck.Salt, claims, aws_body)
  Application.UseWaitCursor = False

  If Dvr.Dsave_value IsNot Nothing Then
   Select Case dsv.vers
    Case 1
     js = Enc.Decrypt(Dvr.Dsave_value, Enc.Iterscramble(cid), Convert.ToInt32((Mpck.Iterations Mod 10) + 2))
    Case Else
     js = Enc.Decrypt(Dvr.Dsave_value, Enc.Iterscramble(cid, Mpck.Iterations), Convert.ToInt32(Mpck.Iterations / 3))
   End Select

   If js = Nothing Then
    MessageBox.Show("Unable to load saved data", "Error", MessageBoxButtons.OK)
    Return
   End If

   Sblk = JsonConvert.DeserializeObject(Of Save_blk)(js)

   If Sblk Is Nothing Then
    MessageBox.Show("Unable to load saved data", "Error", MessageBoxButtons.OK)
    Return
   End If

   Load_saved(Sblk)

   Deletemi.Enabled = True
  End If

  If run_timer Then
   Ettb.Text = stop_timer(SW)
  End If

 End Sub

 Private Sub prv_combo_TextChanged(sender As Object, e As EventArgs) Handles prv_combo.TextChanged
  prv_combo.SelectionLength = 0
 End Sub

 Private Sub dept_tbox_MouseEnter(sender As Object, e As EventArgs) Handles dept_tbox.MouseEnter
  dept_tbox.SelectionStart = 0
 End Sub

 Private Sub Printers_MouseEnter(sender As Object, e As EventArgs)
  Printers.SelectionStart = 0
 End Sub

 Private Sub dsaves_MouseEnter(sender As Object, e As EventArgs)
  dsaves.SelectionStart = 0
 End Sub

 Private Sub dsaves_Enter(sender As Object, e As EventArgs)
  dsaves.SelectionStart = 0
 End Sub

 Private Sub Cb_Enter(sender As Object, e As EventArgs) Handles dsaves.Enter, dsaves.Click, prv_combo.Enter, prv_combo.Click, Printers.Click, Printers.Enter, Department.Click
  Dim Cb As ComboBox = sender
  Cb.SelectionStart = Cb.Text.Length
  ' Cb.SelectionLength = 0
 End Sub

 Private Sub Cb_DropDown(sender As Object, e As EventArgs) Handles dsaves.DropDown, Printers.DropDown, Department.DropDown
  Dim Cb As ComboBox = sender
  Cb.SelectionLength = 0

  If Cb.Name = "dsaves" Then
   Console.WriteLine("dsaves selected[" & dsaves.SelectedText & "]")
  End If

  Dim St As String = Cb.Text
 End Sub

 Private Sub Cb_DrawItem(sender As Object, e As DrawItemEventArgs) Handles dsaves.DrawItem, Printers.DrawItem, prv_combo.DrawItem, Department.DrawItem
  Dim str As String
  Dim Cb As ComboBox = sender
  Cb.SelectionStart = Cb.SelectionLength
  Dim G As Graphics = e.Graphics
  Dim R As Rectangle = e.Bounds
  Dim bc As System.Drawing.Color = System.Drawing.Color.FromArgb(225, 227, 225)
  If e.Index < 0 Then
   Return
  End If

  If Cb.Name = "dsaves" Then
   str = DirectCast(Cb.Items(e.Index), KeyValuePair(Of String, Dsave)).Key
  Else
   str = DirectCast(Cb.Items(e.Index), String)
  End If

  e.Graphics.FillRectangle(New SolidBrush(bc), R)

  If e.State And DrawItemState.Selected Then
   e.Graphics.DrawString(str, scb_font, System.Drawing.Brushes.Black, R)
  Else
   e.Graphics.DrawString(str, scr_font, System.Drawing.Brushes.Black, R)
  End If

  e.DrawFocusRectangle()
 End Sub

 Private Sub Cb_TextChanged(sender As Object, e As EventArgs) Handles dsaves.TextUpdate, Printers.TextUpdate, Department.TextUpdate, Department.TextChanged
  Dim Cb As ComboBox = sender
  Cb.SelectionStart = Cb.Text.Length
 End Sub

 Private Sub Cb_DropDownClosed(sender As Object, e As EventArgs) Handles dsaves.DropDownClosed, prv_combo.DropDownClosed, Printers.DropDownClosed, Department.DropDownClosed
  Dim Cb As ComboBox = sender
  Cb.SelectionStart = Cb.Text.Length
 End Sub

 Private Sub prv_combo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles prv_combo.SelectedIndexChanged
  Dim Cb As ComboBox = sender
  priph.Text = prv_combo.Text
  dest.Select()
 End Sub

 Private Sub Printers_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Printers.SelectedIndexChanged
  Dim Cb As ComboBox = sender
  Cb.SelectionStart = Cb.Text.Length
  dest.Select()
 End Sub

 Private Sub Department_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Department.SelectedIndexChanged
  Dim Cb As ComboBox = sender
  Cb.SelectionStart = Cb.Text.Length
  dest.Select()
 End Sub

 Private Sub Cb_Validated(sender As Object, e As EventArgs) Handles prv_combo.Validated, Printers.Validated, dsaves.Validated, Department.Validated
  Dim Cb As ComboBox = sender
  Cb.SelectionStart = Cb.Text.Length
 End Sub

 Private Sub dest_Enter(sender As Object, e As EventArgs)
  MsgBox("Dest", MsgBoxStyle.OkOnly, "Dest Enter")
 End Sub
End Class