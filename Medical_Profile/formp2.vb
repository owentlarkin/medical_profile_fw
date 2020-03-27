Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports DYMO.Label.Framework
Imports Medical_Profile.JR.Utils.GUI.Forms
Imports Microsoft.VisualBasic.ApplicationServices
Imports Newtonsoft.Json

Partial Class form1
 Public Label As IDieCutLabel
 Public stb As StyledTextBuilder = New StyledTextBuilder()
 Public htb As StyledTextBuilder = New StyledTextBuilder()
 Public lt As StyledTextBlock
 Public ltline As Integer
 Public labelno As Integer = 0
 Public reg_font As FontInfo
 Public rp1_font As FontInfo
 Public bld_font As FontInfo
 Public bp1_font As FontInfo
 Public itl_font As FontInfo
 Public lines As Integer = 12
 Public points As Double = 6.0
 Public printerName As String
 Public pnames As IPrinters
 Public printer As IPrinter
 Public labelWriterPrinter As ILabelWriterPrinter
 Public lprintparams As ILabelWriterPrintParams
 Public printParams As IPrintParams
 Public pjob As IPrintJob
 Public name_length As Single = 0.0
 Public fi As List(Of FileInfo) = New List(Of FileInfo)
 Public nfnt As FontInfo = New FontInfo("Calibri", 12.0, FontStyle.Bold)
 Public lfnt As FontInfo = New FontInfo("Calibri", 9.0, FontStyle.Bold)
 Public flsort As String = "Date"
 Public enck As String = Nothing
 Public read_patient As String

 Public Shared Sub Main()
  Try
   Application.EnableVisualStyles()
   Application.SetCompatibleTextRenderingDefault(False)
   Application.Run(New Form1)
  Catch ex As Exception
   Dim s As String = Format_exception(ex)
   MsgBox("Exception", MsgBoxStyle.OkOnly, s)
   Application.Exit()
  End Try
 End Sub

 Public Function Check_altered() As Boolean
  Dim res As DialogResult
  Dim msg As String = "You have unprocessed changes" & vbCrLf & "do you want to save these changes?"

  If Not Data_altered Then
   Return False
  End If

  If Dstate = Data_state.Edit_mode Then
   Return False
  End If

  FlexibleMessageBox.FONT = New System.Drawing.Font("Calibri", 10, System.Drawing.FontStyle.Bold)
  res = FlexibleMessageBox.Show(msg, "Data Altered", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)

  If res = DialogResult.Yes Then
   Return True
  Else
   Return False
  End If
 End Function

 Public Sub Delete_saved_item(dsk As String)
  RemoveHandler dsaves.SelectedIndexChanged, AddressOf Dsaves_SelectedIndexChanged

  saved_patients.Remove(dsk)

  dsaves.SelectedItem = Nothing
  If saved_patients.Count > 0 Then

   dsaves.DisplayMember = "key"
   dsaves.ValueMember = "Value"

   dsaves.DataSource = New BindingSource(saved_patients, Nothing)

   dsaves.Enabled = True
   dsaves.Visible = True

   Dsbox.Enabled = True
   Dsbox.Visible = True
  Else
   dsaves.Enabled = False
   dsaves.Visible = False

   Dsbox.Enabled = False
   Dsbox.Visible = False
  End If
  AddHandler dsaves.SelectedIndexChanged, AddressOf Dsaves_SelectedIndexChanged
 End Sub

 Public Sub Add_saved_item(ds As Dsave)
  Dim np As String() = Nothing
  Dim csep As Char() = {" "c}
  Dim pn As String = Nothing

  RemoveHandler dsaves.SelectedIndexChanged, AddressOf Dsaves_SelectedIndexChanged

  Select Case Convert.ToInt32(ds.vers)
   Case 1
    ds.Name = Enc.Decrypt(ds.Skey, Enc.Iterscramble(cid + Mpck.Dlab), (Mpck.Iterations Mod 10) + 2)
   Case Else
    ds.Name = Enc.Decrypt(ds.Skey, Enc.Iterscramble(cid + Mpck.Dlab, Mpck.Iterations))
  End Select

  If String.IsNullOrEmpty(ds.Name) Then
   ds.Name = "Unknown_" & If(String.IsNullOrEmpty(ds.lwtim), ds.wrtim, ds.lwtim)
  End If

  np = ds.Name.Split(csep)

  If np.Count > 1 Then
   pn = np(1) & "," & np(0)
  Else
   pn = ds.Name
  End If

  saved_patients(pn) = ds

  If saved_type Then
   dsaves.Text = pn
  End If

  If saved_patients.Count > 0 Then

   dsaves.DisplayMember = "key"
   dsaves.ValueMember = "Value"

   dsaves.DataSource = New BindingSource(saved_patients, Nothing)

   dsaves.SelectedIndex = dsaves.FindStringExact(ds.Name)

   dsaves.Enabled = True
   dsaves.Visible = True

   Dsbox.Enabled = True
   Dsbox.Visible = True
  Else
   dsaves.Enabled = False
   dsaves.Visible = False

   Dsbox.Enabled = False
   Dsbox.Visible = False
  End If
  AddHandler dsaves.SelectedIndexChanged, AddressOf Dsaves_SelectedIndexChanged
 End Sub

 Public Async Function Write_exception(Ex As String, Optional ByVal show As Boolean = False) As Task(Of Integer)

  Dim Wrtim = DateTime.Now.ToString("yyyyMMddHHmmss")
  Dim claims = Gen_Claims()
  Dim Dr As Dsave_return

  aws_body.Clear()
  aws_body("ukey") = Enc.Encrypt(cid + Mpck.Dlab, cid + Mpck.Dlab, Mpck.Iterations)
  aws_body("wrtim") = Wrtim
  aws_body("exception") = Ex

  Application.UseWaitCursor = True
  Dr = Await Aws.Save_exception(Mpck.Url, enck, Mpck.Salt, claims, aws_body)
  Application.UseWaitCursor = False

  If show Then
   FlexibleMessageBox.Show(Ex, "Unhandled Exception")
  End If

  Return Dr.code
 End Function

 Public Function Fe(ByVal e As Exception, ByVal Optional header As String = "Unhandled Exception") As String
  Dim erm As StringBuilder = New StringBuilder()
  erm.Append(header)
  erm.Append(vbLf)
  erm.Append("Installed Version ")
  erm.Append(Form1.installed_version)
  erm.Append(vbLf)

  If TypeOf e Is AggregateException Then
   Dim ae As AggregateException = TryCast(e, AggregateException)
   erm.Append("One or more errors have occured in a background process." & vbLf)

   For Each e1 In ae.Flatten().InnerExceptions
    erm.Append(vbLf)
    erm.Append(e1.Message)

    If e1.StackTrace IsNot Nothing Then
     erm.Append(vbLf & "Stack Trace:" & vbLf)
     erm.Append(e1.StackTrace)
    End If
   Next
  Else
   erm.Append(e.Message)

   If e.StackTrace IsNot Nothing Then
    erm.Append(vbLf & "Stack Trace:" & vbLf)
    erm.Append(e.StackTrace)
   End If
  End If

  Return (erm.ToString())
 End Function

 Public Sub Set_saved_items(ds As List(Of Dsave), Optional ByVal preserve As Boolean = False)
  Dim dss As String = Nothing
  Dim O1 As Object = Nothing
  Dim S1 As String = Nothing
  Dim np As String() = Nothing
  Dim csep As Char() = {" "c}
  Dim dec_name As String = Nothing

  RemoveHandler dsaves.SelectedIndexChanged, AddressOf Dsaves_SelectedIndexChanged

  If preserve Then
   O1 = dsaves.SelectedValue
   S1 = dsaves.SelectedText
  End If

  saved_patients.Clear()

  dsaves.SelectedItem = Nothing

  If ds IsNot Nothing AndAlso ds.Count > 0 Then
   For Each d As Dsave In ds
    Select Case Convert.ToInt32(d.vers)
     Case 1
      dec_name = Enc.Decrypt(d.Skey, Enc.Iterscramble(cid + Mpck.Dlab), (Mpck.Iterations Mod 10) + 2)
      If String.IsNullOrEmpty(dec_name) Then
       d.Name = "Unknown_" & If(String.IsNullOrEmpty(d.lwtim), d.wrtim, d.lwtim)
       dss = d.Name
      Else
       d.Name = dec_name
      End If
     Case Else
      dec_name = Enc.Decrypt(d.Skey, Enc.Iterscramble(cid + Mpck.Dlab, Mpck.Iterations))
      If String.IsNullOrEmpty(dec_name) Then
       d.Name = "Unknown_" & If(String.IsNullOrEmpty(d.lwtim), d.wrtim, d.lwtim)
       dss = d.Name
      Else
       d.Name = dec_name
      End If
    End Select

    np = d.Name.Split(csep)

    If np.Count > 1 Then
     dss = np(1) & "," & np(0)

     If String.IsNullOrEmpty(d.lwtim) Then
      dss = dss & String.Format(" {0}/{1}/{2} {3}:{4}:{5}", d.wrtim.Substring(0, 4), d.wrtim.Substring(4, 2), d.wrtim.Substring(6, 2),
                                       d.wrtim.Substring(8, 2), d.wrtim.Substring(10, 2), d.wrtim.Substring(12, 2))
     End If
    End If

    Dim sb As StringBuilder = New StringBuilder
    sb.Append("dss[" & dss & "] d.name[" & d.Name & "]")
    If Not String.IsNullOrEmpty(d.wrtim) Then
     sb.Append(" wrtim[" & d.wrtim & "]")
    End If
    If Not String.IsNullOrEmpty(d.lwtim) Then
     sb.Append(" lwtim[" & d.lwtim & "]")
    End If
    Console.WriteLine(sb.ToString())

    saved_patients(dss) = d
   Next

   dsaves.DisplayMember = "key"
   dsaves.ValueMember = "Value"

   dsaves.DataSource = New BindingSource(saved_patients, Nothing)

   dsaves.Enabled = True
   dsaves.Visible = True

   Dsbox.Enabled = True
   Dsbox.Visible = True

   If preserve Then
    dsaves.SelectedIndex = dsaves.FindStringExact(S1)
   Else
    dsaves.SelectedItem = Nothing
   End If

   AddHandler dsaves.SelectedIndexChanged, AddressOf Dsaves_SelectedIndexChanged
  Else
   dsaves.Enabled = False
   dsaves.Visible = False

   Dsbox.Enabled = False
   Dsbox.Visible = False
  End If
 End Sub

 Public Sub Load_Department(l1 As Level1_Return)

  Department.Enabled = False

  Department.Items.Clear()

  departments.Clear()
  atdp.Clear()

  For i As Integer = 0 To l1.Dpt.Length - 1
   departments(l1.Dpt(i).departmentid) = l1.Dpt(i)
  Next

  If departments.Count = 1 Then
   Me.Department.Visible = False
   Me.dept_tbox.Visible = True
   Me.dept_tbox.Text = departments(0).name
  Else
   Me.dept_tbox.Visible = False
  End If

  Dim dfound As String = ""

  Me.Department.Items.Add(String.Empty)

  For Each k As KeyValuePair(Of Integer, Dept_Return) In departments
   Me.Department.Items.Add(k.Value.name)
   atdp(k.Value.name) = k.Key

   If dfound = "" Then
    dfound = k.Value.name
   End If

   If My.Settings.department = k.Value.name Then
    dfound = k.Value.name
   End If
  Next

  If Not My.Settings.department = dfound Then
   My.Settings.department = dfound
   My.Settings.Save()
  End If

  Me.Department.SelectedIndex = 0

  Department.Enabled = True

 End Sub

 Private Async Function save_blk() As Task(Of Dsave_return)
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

  Return dr
 End Function

 Private Sub Load_providers(l1 As Level1_Return)
  Dim s1 As String
  Dim pphone As String = Nothing
  Dim p As Provider_return

  Me.Message_label.Text = "Loading Providers"

  Message_label.Update()

  providers.Clear()

  atpv.Clear()

  For i = 0 To l1.Prv.Count - 1

   Dim ad As StringBuilder = New StringBuilder

   providers(l1.Prv(i).Providerid) = l1.Prv(i)

   p = l1.Prv(i)

   s1 = ""

   If p.Displayname IsNot Nothing Then
    s1 = p.Displayname
   Else
    If p.Firstname IsNot Nothing AndAlso p.Lastname IsNot Nothing Then
     s1 = p.Firstname & " " & p.Lastname
    End If
   End If

   If p.Providertype IsNot Nothing AndAlso s1.IndexOf(",") = -1 AndAlso Not s1.EndsWith(p.Providertype) Then
    s1 = s1 & ", " & p.Providertype
   End If

   If p.Homedepartment IsNot Nothing AndAlso atdp.ContainsKey(p.Homedepartment) Then
    If departments(atdp(p.Homedepartment)).phone IsNot Nothing Then
     pphone = Trimc(departments(atdp(p.Homedepartment)).phone, ")(- ")
     s1 = s1 & " " & Format_phone(pphone)
    End If
   End If

   atpv(p.Providerid) = s1
   prv_combo.Items.Add(s1)
  Next

  Message_label.Text = Nothing
  Message_label.Update()

  If dsaves.Items.Count > 0 Then
   dsaves.Enabled = True
  End If

  Application.DoEvents()

  Return
 End Sub

 Sub Load_level2(l2 As Level2_Return)
  Dim pphone As String = Nothing
  Dim s1 As String
  Dim ad As StringBuilder = New StringBuilder
  Dim term As String = Nothing

  ppgb.Text = "Primary Physician"

  saved_type = False
  Dsi = New KeyValuePair(Of String, Dsave)

  Savemi.Enabled = False
  Deletemi.Enabled = False

  dsaves.Enabled = False

  Scsiz(Me.Width, originaly)
  Me.Invalidate()
  Me.Update()

  Reset_fields()

  RemoveHandler dsaves.SelectedIndexChanged, AddressOf Dsaves_SelectedIndexChanged
  dsaves.SelectedItem = Nothing
  AddHandler dsaves.SelectedIndexChanged, AddressOf Dsaves_SelectedIndexChanged

  For Each k As Integer In blocks.Keys
   max_rec(k) = 0
  Next

  For Each b As Ath_block In ath_blist
   If b.max_lines > 0 Then
    max_rec(b.num) = b.max_lines
   End If
  Next

  If l2.Pat.Patientid IsNot Nothing Then
   Me.Patientid.Text = l2.Pat.Patientid.ToString
  End If

  If l2.Pat.Mobilephone IsNot Nothing Then
   pphone = l2.Pat.Mobilephone
  End If

  If pphone Is Nothing AndAlso l2.Pat.Homephone IsNot Nothing Then
   pphone = l2.Pat.Homephone
  End If

  If l2.Pat.Firstname IsNot Nothing AndAlso l2.Pat.Lastname IsNot Nothing Then
   Me.Patient.Text = l2.Pat.Firstname & " " & l2.Pat.Lastname
  End If

  If l2.Pat.Dob IsNot Nothing Then
   Me.DOB.Text = l2.Pat.Dob
  End If

  If pphone IsNot Nothing Then
   pphone = Trimc(pphone, ")(- ")
   Phone.Text = Format_phone(pphone)
  End If

  If l2.Pat.Address1 IsNot Nothing Then
   ad.Append(l2.Pat.Address1)
   term = vbCrLf
  End If

  If l2.Pat.City IsNot Nothing Then
   ad.Append(term)
   ad.Append(l2.Pat.City)
   term = ","
  End If

  If l2.Pat.State IsNot Nothing Then
   ad.Append(term)
   ad.Append(l2.Pat.State)
   term = " "
   If l2.Pat.Zip IsNot Nothing Then
    ad.Append(term)
    ad.Append(l2.Pat.Zip)
   End If
  End If

  If ad.Length > 0 Then
   Me.address.Text = ad.ToString
  End If

  Me.address.BackColor = System.Drawing.Color.White

  If l2.Pat.Primaryproviderid <> -1 AndAlso atpv.ContainsKey(l2.Pat.Primaryproviderid) Then
   prv_combo.SelectedItem = atpv(l2.Pat.Primaryproviderid)
   priph.Text = atpv(l2.Pat.Primaryproviderid)
  End If

  s1 = Nothing
  pphone = Nothing

  If l2.Pat.Contactname IsNot Nothing Then
   s1 = l2.Pat.Contactname
  Else
   If l2.Pat.Contactrelationship IsNot Nothing Then
    s1 = Ucfc(l2.Pat.Contactrelationship)
   End If
  End If

  If s1 IsNot Nothing Then
   If l2.Pat.Contactmobilephone IsNot Nothing Then
    pphone = l2.Pat.Contactmobilephone
   Else
    pphone = l2.Pat.Contacthomephone
   End If
  End If

  If s1 IsNot Nothing Then
   If pphone IsNot Nothing Then
    s1 = s1 & " " & Format_phone(pphone)
   End If
   Me.econtact.Text = s1
  End If

  Me.econtact.BackColor = System.Drawing.Color.White

  Me.ins.Text = l2.Ins

  Me.ins.BackColor = System.Drawing.Color.White

  If Me.ppgb.Text.Length > 1 Then
   lab1("priph_title:") = Me.ppgb.Text
  End If

  If Me.sp.Text.Length > 1 Then
   lab1("secph_title:") = Me.sp.Text
  End If

  For Each b As String In Endpoints_in_use
   If l2.blks.ContainsKey(b) AndAlso l2.blks(b).Count > 0 Then
    Dim bnum As Integer = bl_available.First.Key

    bl_used(bl_available.First.Key) = bl_available.First.Value
    bl_available.Remove(bl_available.First.Key)

    Me.Controls(gpn(bnum)).Visible = False

    Me.Controls(gpn(bnum)).Controls(0).Text = b
    Me.Controls(gpn(bnum)).Controls(0).Tag = b
    Me.Controls(gpn(bnum)).Controls(0).Enabled = True

    Dim tb As TextBox = Me.Controls(gpn(bnum)).Controls(0)
    tb.ReadOnly = True

    Me.Controls(gpn(bnum)).Controls(1).Text = String.Join(vbCrLf, l2.blks(b))
    Me.Controls(gpn(bnum)).Controls(1).Enabled = True

    Dim rb As RichTextBox = Me.Controls(gpn(bnum)).Controls(1)

    rb.ReadOnly = True
    rb.BackColor = System.Drawing.Color.White

    Ath_setcolor(bnum)

    labgb(gph(bnum)) = b
    labgb(gpb(bnum)) = String.Join(vbCrLf, l2.blks(b))

    bl_loaded(b).Num = bnum
    bl_loaded(b).State = Load_state.loaded
   End If
  Next

  If bl_used.Count > 0 Then
   Scsiz(Me.Width)
  End If

  Me.Update()

  For Each k As KeyValuePair(Of Integer, String) In bl_used
   Me.Controls(k.Value).Visible = True
  Next

  ' If pr_practice Then
  Me.Savemi.Visible = True
  Me.Savemi.Enabled = True

  Me.dsaves.Enabled = True
  '   End If

  Set_empgb()

  Data_altered = False
  '  WireAllEvents(dsaves)
  Return
 End Sub

 Sub Load_saved(sblk As Save_blk)

  Dim bnum As Integer

  saved_type = True

  Me.Practice.Text = sblk.Practice

  Me.dept_tbox.Text = sblk.Department

  Me.Department.Text = sblk.Department

  Me.Patient.Text = sblk.Patient

  Me.Patientid.Text = sblk.Patient_id

  Me.Phone.Text = sblk.Phone

  Me.address.Text = sblk.Address
  Me.address.BackColor = System.Drawing.Color.White

  Me.DOB.Text = sblk.DOB

  Me.econtact.Text = sblk.Emergency_contact
  Me.econtact.BackColor = System.Drawing.Color.White

  Me.ins.Text = sblk.Insurance
  Me.ins.BackColor = System.Drawing.Color.White

  If Not String.IsNullOrEmpty(sblk.Prtitle) Then
   Me.ppgb.Text = sblk.Prtitle
   lab1("priph_title:") = Me.ppgb.Text
  End If

  prv_combo.Text = sblk.Priph
  priph.Text = sblk.Priph

  Me.secph.Text = sblk.Secph

  If Not String.IsNullOrEmpty(sblk.Sptitle) Or Not String.IsNullOrEmpty(Me.secph.Text) Then
   If String.IsNullOrEmpty(sblk.Sptitle) Then
    Me.sptitle.Text = Me.sp.Text
   Else
    Me.sptitle.Text = sblk.Sptitle
   End If

   Me.sp.Text = Me.sptitle.Text
   lab1("secph_title:") = Me.sp.Text
   Me.sp.Visible = True
   Me.secph.Visible = True
   Me.secph.Enabled = True
   Me.secph.BackColor = System.Drawing.Color.White
  End If

  For Each sb As Sblock In sblk.Blk_list
   bnum = sb.num
   If gpn.ContainsKey(bnum) Then
    Me.Controls(gpn(bnum)).Visible = False
    Me.Controls(gpn(bnum)).Controls(0).Text = sb.header
    Me.Controls(gpn(bnum)).Controls(0).Tag = sb.header
    Me.Controls(gpn(bnum)).Controls(0).Enabled = True

    Dim tb As TextBox = Me.Controls(gpn(bnum)).Controls(0)
    tb.ReadOnly = True

    Me.Controls(gpn(bnum)).Controls(1).Text = sb.body
    Me.Controls(gpn(bnum)).Controls(1).Enabled = True

    Dim rb As RichTextBox = Me.Controls(gpn(bnum)).Controls(1)
    rb.ReadOnly = True
    rb.BackColor = System.Drawing.Color.White

    Ath_setcolor(bnum)

    labgb(gph(bnum)) = sb.header
    labgb(gpb(bnum)) = sb.body

    bl_used(bnum) = bl_available(bnum)
    bl_available.Remove(bnum)
   End If
  Next

  Me.Savemi.Visible = True
  Me.Deletemi.Visible = True

  If bl_used.Count > 0 Then
   Scsiz(Me.Width)
  End If

  Me.Update()

  For Each k As KeyValuePair(Of Integer, String) In bl_used
   Me.Controls(k.Value).Visible = True
  Next

  Set_empgb()

  Data_altered = False

  Dsi = dsaves.SelectedItem

  Return
 End Sub

 Private Sub do_tbox(tb As TextBox)
  tb.Text = String.Empty
  tb.ReadOnly = True
  tb.BackColor = System.Drawing.Color.White
 End Sub

 Private Sub Reset_fields(Optional Skip_si As Boolean = False)
  Dim Save_da As Boolean = Data_altered

  Dim gb_name As String

  Me.Editmenuitem.Text = "Edit"
  Dstate = Data_state.NoEdit
  Dsbox.Enabled = True

  If Not L1_ret Is Nothing Then
   Practice.Text = L1_ret.Prc.name
   Practice.ReadOnly = True
   Practice.BackColor = System.Drawing.Color.White
  End If

  address.Text = String.Empty
  address.ReadOnly = True
  address.BackColor = System.Drawing.Color.White

  dept_tbox.Text = String.Empty

  Department.Text = ""

  ppgb.Text = "Primary Physician"

  do_tbox(DOB)
  do_tbox(Phone)
  do_tbox(priph)
  do_tbox(ins)
  do_tbox(econtact)
  do_tbox(secph)
  do_tbox(priph)
  do_tbox(dept_tbox)

  sp.Text = ""
  secph.Text = ""

  If Not Mpck.Sec_visible Then
   sp.Visible = False
   secph.Visible = False
   secph.Enabled = False
  Else
   sp.Visible = True
   secph.Visible = True
   secph.Enabled = True
   secph.BackColor = System.Drawing.Color.White
   If Not String.IsNullOrEmpty(Mpck.Sptitle) Then
    sp.Text = Mpck.Sptitle
   Else
    sp.Text = "Specialist"
   End If
  End If

  Me.Savemi.Visible = False
  Me.Deletemi.Visible = False

  RemoveHandler prv_combo.SelectedIndexChanged, AddressOf prv_combo_SelectedIndexChanged
  prv_combo.SelectedItem = Nothing
  prv_combo.Text = String.Empty
  AddHandler prv_combo.SelectedIndexChanged, AddressOf prv_combo_SelectedIndexChanged

  Ettb.Text = Nothing

  If Not Skip_si Then
   RemoveHandler dsaves.SelectedIndexChanged, AddressOf Dsaves_SelectedIndexChanged
   dsaves.SelectedItem = Nothing
   AddHandler dsaves.SelectedIndexChanged, AddressOf Dsaves_SelectedIndexChanged
  End If

  Try
   lab1.Clear()

   For Each k As KeyValuePair(Of String, Blk_entry) In bl_loaded
    k.Value.State = Load_state.not_in_use
    k.Value.Num = 0
   Next

   For Each k As KeyValuePair(Of Integer, String) In bl_used
    bl_available(k.Key) = k.Value
   Next

   bl_used.Clear()

   For Each s As KeyValuePair(Of Integer, String) In bl_available
    Me.Controls(s.Value).Visible = False

    Me.Controls(s.Value).Controls(0).Text = String.Empty
    Me.Controls(s.Value).Controls(1).Text = String.Empty
   Next

   currenty = originaly

   Me.SuspendPaint()

   For i As Integer = 1 To Endpoints_in_use.Count
    gb_name = bl_available(i)
    labgb(gph(i)) = String.Empty
    labgb(gpb(i)) = String.Empty
    currenty = Setcy(Me.Controls(gb_name))
   Next

   Reset_labels()

   Scsiz(Me.Width, originaly)

   Me.ResumePaint()
  Catch ex As Exception
   Dim s As String = Format_exception(ex)
   MsgBox("Exception", MsgBoxStyle.OkOnly, s)
   Application.Exit()
  End Try

  Data_altered = Save_da
 End Sub

 Public Function Trimc(ByRef s1 As String, ByVal crm As String) As String
  Dim sl As String = s1

  For Each c As Char In crm
   sl = sl.Replace(c, "")
  Next

  Return sl
 End Function

 Public Function Format_phone(ByRef s1 As String) As String
  Dim s2 As String

  Select Case s1.Length
   Case 10
    s2 = "(" & s1.Substring(0, 3) & ") " & s1.Substring(3, 3) & "-" & s1.Substring(6, 4)
   Case 7
    s2 = s1.Substring(0, 3) & "-" & s1.Substring(3, 4)
   Case Else
    s2 = s1
  End Select

  Return s2
 End Function

 Public Sub Logwr(lin As String)
  Dim format As String = "MM/dd/yyyy HH:mm:ss"

  If logfile = Nothing Then
   logfile = My.Settings.start_dir & "\medical_profile.log"
  End If

  If File.Exists(logfile) Then
   Using writer As New StreamWriter(logfile, True)
    writer.WriteLine("[" & DateTime.Now.ToString(format) & "] " & lin)
   End Using
  Else
   Using writer As New StreamWriter(logfile, False)
    writer.WriteLine("[" & DateTime.Now.ToString(format) & "] " & lin)
   End Using
  End If
 End Sub

 Public Shared Function start_timer(timer As Stopwatch) As String
  timer.Reset()
  timer.Start()
  Return ""
 End Function

 Public Shared Function stop_timer(timer As Stopwatch) As String
  Dim et As String = Nothing

  timer.Stop()

  et = timer.ElapsedMilliseconds.ToString()

  Return et
 End Function

 Public Shared Function Format_exception(ByVal e As Exception, ByVal Optional header As String = "Error obtaining data") As String
  Dim erm As StringBuilder = New StringBuilder()
  erm.Append(header)
  erm.Append(vbLf)

  If TypeOf e Is AggregateException Then
   Dim ae As AggregateException = TryCast(e, AggregateException)
   erm.Append("One or more errors have occured in a background process." & vbLf)

   For Each e1 In ae.Flatten().InnerExceptions
    erm.Append(vbLf)
    erm.Append(e1.Message)

    If e1.StackTrace IsNot Nothing Then
     erm.Append(vbLf & "Stack Trace:" & vbLf)
     erm.Append(e1.StackTrace)
    End If
   Next
  Else
   erm.Append(e.Message)

   If e.StackTrace IsNot Nothing Then
    erm.Append(vbLf & "Stack Trace:" & vbLf)
    erm.Append(e.StackTrace)
   End If
  End If

  Return (erm.ToString())
 End Function

 Private Function Wlengb(s As String, Optional pts As Double = Nothing) As Single
  Dim wl As Single = 0.0
  Dim pt1 As Double

  If pts = Nothing Then
   pt1 = points
  Else
   pt1 = pts
  End If

  For i As Integer = 0 To s.Length - 1
   wl += cwdb(Asc(s(i))) * pt1
  Next

  wl += cwdb(32) * pt1

  Return wl

 End Function

 Private Function Wleng(s As String, Optional pts As Double = Nothing) As Single
  Dim wl As Single = 0.0
  Dim pt1 As Double

  If pts = Nothing Then
   pt1 = points
  Else
   pt1 = pts
  End If

  For i As Integer = 0 To s.Length - 1
   wl += cwd(Asc(s(i))) * pt1
  Next

  wl += cwd(32) * pt1

  Return wl

 End Function

 Public Function Adjust_lines(lis As String, lstate As String, Optional ByRef adl As Dictionary(Of Integer, Integer) = Nothing) As String

  Dim li() As String = Regex.Split(lis, "\r\n|\n")

  Dim lict As Integer = li.Count - 1

  Dim lsa() As String = li

  Dim lsl() As String

  Dim lst As New List(Of String)

  Dim i, j, k As Integer

  Dim sb As New StringBuilder

  Dim sbps As Single

  Dim wds As Single

  Dim chng As Boolean

  For Each s As String In lsa
   s = s.TrimEnd(vbCrLf, vbCr, vbLf)
   s = s.TrimStart(vbCrLf, vbCr, vbLf)
  Next

  lst = lsa.ToList

  i = 0

  j = lst.Count - 1

  While (i <= j)
   chng = False
   lst(i) = lst(i).TrimStart
   lst(i) = lst(i).TrimEnd(vbVerticalTab)
   lst(i) = lst(i).TrimEnd
   If lst(i).StartsWith("Last Reviewed") Then
    lst.RemoveRange(i, 1)
    j -= 1
    If lst(i).Length() < 10 Then
     lst.RemoveRange(i, 1)
     j -= 1
    End If
    Continue While
   End If

   If lstate = "Label" Then
    wds = Wleng(lst(i))
    If wds > label_length Then
     chng = True
     sb.Clear()
     sbps = 0.0
     lsl = lst(i).ToString.Split(" ")
     lst.RemoveRange(i, 1)
     j -= 1
     For k = 0 To lsl.Count - 1
      wds = Wleng(lsl(k))
      If wds + sbps > label_length Then
       lst.Insert(i, sb.ToString)
       j = j + 1
       i = i + 1
       sb.Clear()
       sb.Append("..")
       If adl IsNot Nothing Then
        If adl.ContainsKey(i) Then
         adl(i) = adl(i) + 1
        Else
         adl(i) = 1
        End If
       End If
      End If

      sb.Append(lsl(k) + " ")
      sbps = Wleng(sb.ToString)
     Next
     lst.Insert(i, sb.ToString)
     i += 1
     j += 1
    End If
    If chng Then
     i = i - 1
     Continue While
    End If
   End If

   If lst(i).Length = 0 Then
    lst.RemoveRange(i, 1)
    j = j - 1
    Continue While
   End If

   i = i + 1
  End While

  Return String.Join(vbCrLf, lst)

 End Function

 Private Function Setcolor(bn As Integer, nl As Integer) As Integer
  Dim bx As RichTextBox = Nothing
  Dim hx As TextBox = Nothing
  Dim bi As Blk_info = Nothing

  hx = Me.Controls(gpn(bn)).Controls(0)
  bx = Me.Controls(gpn(bn)).Controls(1)
  bi = blocks(bn)

  Dim ln As Integer = 0
  Dim tsize As Integer = bx.Text.Length

  bx.Select(bi.ll(nl), tsize - 1)
  bx.SelectionBackColor = System.Drawing.Color.FromArgb(255, 196, 255, 255)
  hx.BackColor = System.Drawing.Color.FromArgb(255, 255, 210, 255)

  Application.DoEvents()

  bx.SelectionLength = 0
  bx.Select(0, 0)

  Return ln

 End Function

#Region " ResizeImage "

 Public Shared Function ResizeImage(bmSource As Drawing.Bitmap, TargetWidth As Int32, TargetHeight As Int32) As Drawing.Bitmap
  Dim bmDest As New Drawing.Bitmap(TargetWidth, TargetHeight, Drawing.Imaging.PixelFormat.Format32bppArgb)

  Dim nSourceAspectRatio = bmSource.Width / bmSource.Height
  Dim nDestAspectRatio = bmDest.Width / bmDest.Height

  Dim NewX = 0
  Dim NewY = 0
  Dim NewWidth = bmDest.Width
  Dim NewHeight = bmDest.Height

  If nDestAspectRatio = nSourceAspectRatio Then
   'same ratio
  ElseIf nDestAspectRatio > nSourceAspectRatio Then
   'Source is taller
   NewWidth = Convert.ToInt32(Math.Floor(nSourceAspectRatio * NewHeight))
   NewX = Convert.ToInt32(Math.Floor((bmDest.Width - NewWidth) / 2))
  Else
   'Source is wider
   NewHeight = Convert.ToInt32(Math.Floor((1 / nSourceAspectRatio) * NewWidth))
   NewY = Convert.ToInt32(Math.Floor((bmDest.Height - NewHeight) / 2))
  End If

  Using grDest = Drawing.Graphics.FromImage(bmDest)
   With grDest
    .CompositingQuality = Drawing.Drawing2D.CompositingQuality.HighQuality
    .InterpolationMode = Drawing.Drawing2D.InterpolationMode.HighQualityBicubic
    .PixelOffsetMode = Drawing.Drawing2D.PixelOffsetMode.HighQuality
    .SmoothingMode = Drawing.Drawing2D.SmoothingMode.AntiAlias
    .CompositingMode = Drawing.Drawing2D.CompositingMode.SourceOver

    .DrawImage(bmSource, NewX, NewY, NewWidth, NewHeight)
   End With
  End Using

  Return bmDest
 End Function

#End Region

 Public Function Ucfc(value As String) As String
  If String.IsNullOrEmpty(value) Then
   Return String.Empty
  End If
  Return Char.ToUpper(value(0)) & value.Substring(1).ToLower
 End Function

 Public Function Render(label As ILabel) As Byte()
  Dim renderparams As ILabelRenderParams = New LabelRenderParams()
  With renderparams
   .FlowDirection = FlowDirection.LeftToRight
   .LabelColor = Colors.White
   .ShadowColor = Colors.DarkGray
   .ShadowDepth = 3
   .PngUseDisplayResolution = False
  End With
  Dim pngdata() As Byte = label.RenderAsPng(printer, renderparams)
  Return pngdata
 End Function

End Class