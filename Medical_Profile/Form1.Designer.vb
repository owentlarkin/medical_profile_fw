<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class form1
  Inherits System.Windows.Forms.Form

  'Form overrides dispose to clean up the component list.
  <System.Diagnostics.DebuggerNonUserCode()>
  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    Try
      If disposing AndAlso components IsNot Nothing Then
        components.Dispose()
      End If
    Finally
      MyBase.Dispose(disposing)
    End Try
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  <System.Diagnostics.DebuggerStepThrough()>
  Private Sub InitializeComponent()
  Me.components = New System.ComponentModel.Container()
  Me.Dsbox = New System.Windows.Forms.GroupBox()
  Me.dsaves = New System.Windows.Forms.ComboBox()
  Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
  Me.GroupBox2 = New System.Windows.Forms.GroupBox()
  Me.Tgp = New System.Windows.Forms.GroupBox()
  Me.Ettb = New System.Windows.Forms.TextBox()
  Me.dest = New System.Windows.Forms.TextBox()
  Me.Label4 = New System.Windows.Forms.Label()
  Me.Label2 = New System.Windows.Forms.Label()
  Me.Label3 = New System.Windows.Forms.Label()
  Me.pid_label = New System.Windows.Forms.Label()
  Me.patient_label = New System.Windows.Forms.Label()
  Me.Patientid = New System.Windows.Forms.TextBox()
  Me.address = New System.Windows.Forms.RichTextBox()
  Me.Phone = New System.Windows.Forms.TextBox()
  Me.DOB = New System.Windows.Forms.TextBox()
  Me.Patient = New System.Windows.Forms.TextBox()
  Me.Printbox = New System.Windows.Forms.GroupBox()
  Me.Printers = New System.Windows.Forms.ComboBox()
  Me.OpenFileDialog2 = New System.Windows.Forms.OpenFileDialog()
  Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
  Me.ppgb = New System.Windows.Forms.GroupBox()
  Me.prv_combo = New System.Windows.Forms.ComboBox()
  Me.priph = New System.Windows.Forms.TextBox()
  Me.GroupBox8 = New System.Windows.Forms.GroupBox()
  Me.ins = New System.Windows.Forms.TextBox()
  Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
  Me.Editmenuitem = New System.Windows.Forms.ToolStripMenuItem()
  Me.Savemi = New System.Windows.Forms.ToolStripMenuItem()
  Me.Deletemi = New System.Windows.Forms.ToolStripMenuItem()
  Me.Previewmenuitem = New System.Windows.Forms.ToolStripMenuItem()
  Me.Printmenuitem = New System.Windows.Forms.ToolStripMenuItem()
  Me.Clrmenuitem = New System.Windows.Forms.ToolStripMenuItem()
  Me.Notesmenuitem = New System.Windows.Forms.ToolStripMenuItem()
  Me.Addpagemenuitem = New System.Windows.Forms.ToolStripMenuItem()
  Me.Aspecmenuitem = New System.Windows.Forms.ToolStripMenuItem()
  Me.Resetchg = New System.Windows.Forms.ToolStripMenuItem()
  Me.exmenuitem = New System.Windows.Forms.ToolStripMenuItem()
  Me.DeveloperToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
  Me.ColorDialogToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
  Me.sp = New System.Windows.Forms.GroupBox()
  Me.secph = New System.Windows.Forms.TextBox()
  Me.GroupBox3 = New System.Windows.Forms.GroupBox()
  Me.econtact = New System.Windows.Forms.TextBox()
  Me.sptitle = New System.Windows.Forms.TextBox()
  Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
  Me.Message_label = New System.Windows.Forms.Label()
  Me.ContextMenuStrip2 = New System.Windows.Forms.ContextMenuStrip(Me.components)
  Me.Practbox = New System.Windows.Forms.GroupBox()
  Me.Practice = New System.Windows.Forms.TextBox()
  Me.Deptbox = New System.Windows.Forms.GroupBox()
  Me.dept_tbox = New System.Windows.Forms.TextBox()
  Me.Department = New System.Windows.Forms.ComboBox()
  Me.Dsbox.SuspendLayout()
  Me.GroupBox2.SuspendLayout()
  Me.Tgp.SuspendLayout()
  Me.Printbox.SuspendLayout()
  Me.ppgb.SuspendLayout()
  Me.GroupBox8.SuspendLayout()
  Me.MenuStrip1.SuspendLayout()
  Me.sp.SuspendLayout()
  Me.GroupBox3.SuspendLayout()
  Me.Practbox.SuspendLayout()
  Me.Deptbox.SuspendLayout()
  Me.SuspendLayout()
  '
  'Dsbox
  '
  Me.Dsbox.Controls.Add(Me.dsaves)
  Me.Dsbox.Enabled = False
  Me.Dsbox.Location = New System.Drawing.Point(18, 210)
  Me.Dsbox.Name = "Dsbox"
  Me.Dsbox.Size = New System.Drawing.Size(365, 53)
  Me.Dsbox.TabIndex = 0
  Me.Dsbox.TabStop = False
  Me.Dsbox.Text = "Saved Items"
  Me.Dsbox.Visible = False
  '
  'dsaves
  '
  Me.dsaves.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
  Me.dsaves.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
  Me.dsaves.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
  Me.dsaves.Enabled = False
  Me.dsaves.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
  Me.dsaves.FormattingEnabled = True
  Me.dsaves.Location = New System.Drawing.Point(6, 21)
  Me.dsaves.Name = "dsaves"
  Me.dsaves.Size = New System.Drawing.Size(353, 22)
  Me.dsaves.TabIndex = 2
  '
  'OpenFileDialog1
  '
  Me.OpenFileDialog1.FileName = "OpenFileDialog1"
  Me.OpenFileDialog1.Filter = "Patient Reports | *.doc;*.docx;*.xps|All Files|*.*"
  '
  'GroupBox2
  '
  Me.GroupBox2.Controls.Add(Me.Tgp)
  Me.GroupBox2.Controls.Add(Me.dest)
  Me.GroupBox2.Controls.Add(Me.Label4)
  Me.GroupBox2.Controls.Add(Me.Label2)
  Me.GroupBox2.Controls.Add(Me.Label3)
  Me.GroupBox2.Controls.Add(Me.pid_label)
  Me.GroupBox2.Controls.Add(Me.patient_label)
  Me.GroupBox2.Controls.Add(Me.Patientid)
  Me.GroupBox2.Controls.Add(Me.address)
  Me.GroupBox2.Controls.Add(Me.Phone)
  Me.GroupBox2.Controls.Add(Me.DOB)
  Me.GroupBox2.Controls.Add(Me.Patient)
  Me.GroupBox2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
  Me.GroupBox2.Location = New System.Drawing.Point(401, 27)
  Me.GroupBox2.Name = "GroupBox2"
  Me.GroupBox2.Size = New System.Drawing.Size(365, 236)
  Me.GroupBox2.TabIndex = 1
  Me.GroupBox2.TabStop = False
  '
  'Tgp
  '
  Me.Tgp.Controls.Add(Me.Ettb)
  Me.Tgp.Location = New System.Drawing.Point(6, 182)
  Me.Tgp.Name = "Tgp"
  Me.Tgp.Size = New System.Drawing.Size(198, 43)
  Me.Tgp.TabIndex = 13
  Me.Tgp.TabStop = False
  Me.Tgp.Text = "Elapsed ms"
  Me.Tgp.Visible = False
  '
  'Ettb
  '
  Me.Ettb.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
  Me.Ettb.Location = New System.Drawing.Point(6, 19)
  Me.Ettb.Name = "Ettb"
  Me.Ettb.Size = New System.Drawing.Size(186, 21)
  Me.Ettb.TabIndex = 0
  Me.Ettb.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
  '
  'dest
  '
  Me.dest.Location = New System.Drawing.Point(223, 183)
  Me.dest.Name = "dest"
  Me.dest.Size = New System.Drawing.Size(100, 20)
  Me.dest.TabIndex = 12
  Me.dest.Visible = False
  '
  'Label4
  '
  Me.Label4.AutoSize = True
  Me.Label4.Location = New System.Drawing.Point(220, 103)
  Me.Label4.Name = "Label4"
  Me.Label4.Size = New System.Drawing.Size(38, 13)
  Me.Label4.TabIndex = 8
  Me.Label4.Text = "Phone"
  '
  'Label2
  '
  Me.Label2.AutoSize = True
  Me.Label2.Location = New System.Drawing.Point(7, 52)
  Me.Label2.Name = "Label2"
  Me.Label2.Size = New System.Drawing.Size(45, 13)
  Me.Label2.TabIndex = 6
  Me.Label2.Text = "Address"
  '
  'Label3
  '
  Me.Label3.AutoSize = True
  Me.Label3.Location = New System.Drawing.Point(220, 52)
  Me.Label3.Name = "Label3"
  Me.Label3.Size = New System.Drawing.Size(30, 13)
  Me.Label3.TabIndex = 7
  Me.Label3.Text = "DOB"
  '
  'pid_label
  '
  Me.pid_label.AutoSize = True
  Me.pid_label.Location = New System.Drawing.Point(220, 1)
  Me.pid_label.Name = "pid_label"
  Me.pid_label.Size = New System.Drawing.Size(51, 13)
  Me.pid_label.TabIndex = 10
  Me.pid_label.Text = "Patient id"
  '
  'patient_label
  '
  Me.patient_label.AutoSize = True
  Me.patient_label.Location = New System.Drawing.Point(6, 1)
  Me.patient_label.Name = "patient_label"
  Me.patient_label.Size = New System.Drawing.Size(40, 13)
  Me.patient_label.TabIndex = 5
  Me.patient_label.Text = "Patient"
  '
  'Patientid
  '
  Me.Patientid.AcceptsTab = True
  Me.Patientid.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
  Me.Patientid.Location = New System.Drawing.Point(220, 23)
  Me.Patientid.Multiline = True
  Me.Patientid.Name = "Patientid"
  Me.Patientid.Size = New System.Drawing.Size(125, 20)
  Me.Patientid.TabIndex = 11
  Me.Patientid.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
  '
  'address
  '
  Me.address.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
  Me.address.Location = New System.Drawing.Point(6, 67)
  Me.address.Name = "address"
  Me.address.Size = New System.Drawing.Size(198, 109)
  Me.address.TabIndex = 4
  Me.address.Text = ""
  '
  'Phone
  '
  Me.Phone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
  Me.Phone.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
  Me.Phone.Location = New System.Drawing.Point(220, 123)
  Me.Phone.Name = "Phone"
  Me.Phone.Size = New System.Drawing.Size(125, 21)
  Me.Phone.TabIndex = 3
  '
  'DOB
  '
  Me.DOB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
  Me.DOB.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
  Me.DOB.Location = New System.Drawing.Point(220, 74)
  Me.DOB.Name = "DOB"
  Me.DOB.Size = New System.Drawing.Size(125, 26)
  Me.DOB.TabIndex = 1
  '
  'Patient
  '
  Me.Patient.AcceptsTab = True
  Me.Patient.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
  Me.Patient.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
  Me.Patient.Location = New System.Drawing.Point(6, 23)
  Me.Patient.Multiline = True
  Me.Patient.Name = "Patient"
  Me.Patient.Size = New System.Drawing.Size(198, 20)
  Me.Patient.TabIndex = 0
  Me.Patient.Tag = "Patient"
  '
  'Printbox
  '
  Me.Printbox.Controls.Add(Me.Printers)
  Me.Printbox.Location = New System.Drawing.Point(18, 150)
  Me.Printbox.Name = "Printbox"
  Me.Printbox.Size = New System.Drawing.Size(365, 53)
  Me.Printbox.TabIndex = 10
  Me.Printbox.TabStop = False
  Me.Printbox.Text = "Printer"
  '
  'Printers
  '
  Me.Printers.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
  Me.Printers.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
  Me.Printers.FormattingEnabled = True
  Me.Printers.Location = New System.Drawing.Point(6, 19)
  Me.Printers.Name = "Printers"
  Me.Printers.Size = New System.Drawing.Size(353, 22)
  Me.Printers.TabIndex = 0
  '
  'OpenFileDialog2
  '
  Me.OpenFileDialog2.FileName = "OpenFileDialog2"
  '
  'FolderBrowserDialog1
  '
  Me.FolderBrowserDialog1.ShowNewFolderButton = False
  '
  'ppgb
  '
  Me.ppgb.Controls.Add(Me.prv_combo)
  Me.ppgb.Controls.Add(Me.priph)
  Me.ppgb.Location = New System.Drawing.Point(784, 27)
  Me.ppgb.Name = "ppgb"
  Me.ppgb.Size = New System.Drawing.Size(365, 53)
  Me.ppgb.TabIndex = 13
  Me.ppgb.TabStop = False
  Me.ppgb.Text = "Primary Physician"
  '
  'prv_combo
  '
  Me.prv_combo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
  Me.prv_combo.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
  Me.prv_combo.FormattingEnabled = True
  Me.prv_combo.Location = New System.Drawing.Point(7, 20)
  Me.prv_combo.Name = "prv_combo"
  Me.prv_combo.Size = New System.Drawing.Size(353, 22)
  Me.prv_combo.TabIndex = 1
  Me.prv_combo.Visible = False
  '
  'priph
  '
  Me.priph.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
  Me.priph.Location = New System.Drawing.Point(7, 20)
  Me.priph.Name = "priph"
  Me.priph.Size = New System.Drawing.Size(353, 21)
  Me.priph.TabIndex = 0
  '
  'GroupBox8
  '
  Me.GroupBox8.Controls.Add(Me.ins)
  Me.GroupBox8.Location = New System.Drawing.Point(784, 90)
  Me.GroupBox8.Name = "GroupBox8"
  Me.GroupBox8.Size = New System.Drawing.Size(365, 53)
  Me.GroupBox8.TabIndex = 14
  Me.GroupBox8.TabStop = False
  Me.GroupBox8.Text = "Insurance"
  '
  'ins
  '
  Me.ins.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
  Me.ins.Location = New System.Drawing.Point(7, 20)
  Me.ins.Name = "ins"
  Me.ins.Size = New System.Drawing.Size(352, 21)
  Me.ins.TabIndex = 0
  '
  'MenuStrip1
  '
  Me.MenuStrip1.Dock = System.Windows.Forms.DockStyle.None
  Me.MenuStrip1.Font = New System.Drawing.Font("Calibri", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
  Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Editmenuitem, Me.Savemi, Me.Deletemi, Me.Previewmenuitem, Me.Printmenuitem, Me.Clrmenuitem, Me.Notesmenuitem, Me.Addpagemenuitem, Me.Aspecmenuitem, Me.Resetchg, Me.exmenuitem, Me.DeveloperToolStripMenuItem})
  Me.MenuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
  Me.MenuStrip1.Location = New System.Drawing.Point(9, -1)
  Me.MenuStrip1.Name = "MenuStrip1"
  Me.MenuStrip1.Size = New System.Drawing.Size(563, 25)
  Me.MenuStrip1.TabIndex = 15
  Me.MenuStrip1.Text = "MenuStrip1"
  '
  'Editmenuitem
  '
  Me.Editmenuitem.Checked = True
  Me.Editmenuitem.CheckState = System.Windows.Forms.CheckState.Checked
  Me.Editmenuitem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
  Me.Editmenuitem.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
  Me.Editmenuitem.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
  Me.Editmenuitem.Name = "Editmenuitem"
  Me.Editmenuitem.Size = New System.Drawing.Size(43, 21)
  Me.Editmenuitem.Text = "Edit"
  '
  'Savemi
  '
  Me.Savemi.Name = "Savemi"
  Me.Savemi.Size = New System.Drawing.Size(44, 21)
  Me.Savemi.Text = "Save"
  '
  'Deletemi
  '
  Me.Deletemi.Name = "Deletemi"
  Me.Deletemi.Size = New System.Drawing.Size(56, 21)
  Me.Deletemi.Text = "Delete"
  '
  'Previewmenuitem
  '
  Me.Previewmenuitem.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
  Me.Previewmenuitem.Name = "Previewmenuitem"
  Me.Previewmenuitem.Size = New System.Drawing.Size(64, 21)
  Me.Previewmenuitem.Text = "Preview"
  '
  'Printmenuitem
  '
  Me.Printmenuitem.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
  Me.Printmenuitem.Name = "Printmenuitem"
  Me.Printmenuitem.Size = New System.Drawing.Size(46, 21)
  Me.Printmenuitem.Text = "Print"
  '
  'Clrmenuitem
  '
  Me.Clrmenuitem.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
  Me.Clrmenuitem.Name = "Clrmenuitem"
  Me.Clrmenuitem.Size = New System.Drawing.Size(47, 21)
  Me.Clrmenuitem.Text = "Clear"
  '
  'Notesmenuitem
  '
  Me.Notesmenuitem.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
  Me.Notesmenuitem.Name = "Notesmenuitem"
  Me.Notesmenuitem.Size = New System.Drawing.Size(52, 21)
  Me.Notesmenuitem.Text = "Notes"
  '
  'Addpagemenuitem
  '
  Me.Addpagemenuitem.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
  Me.Addpagemenuitem.Name = "Addpagemenuitem"
  Me.Addpagemenuitem.Size = New System.Drawing.Size(70, 21)
  Me.Addpagemenuitem.Text = "Add Page"
  '
  'Aspecmenuitem
  '
  Me.Aspecmenuitem.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
  Me.Aspecmenuitem.Name = "Aspecmenuitem"
  Me.Aspecmenuitem.Size = New System.Drawing.Size(94, 21)
  Me.Aspecmenuitem.Text = "Add Specialist"
  '
  'Resetchg
  '
  Me.Resetchg.Name = "Resetchg"
  Me.Resetchg.Size = New System.Drawing.Size(98, 21)
  Me.Resetchg.Text = "Reset Changes"
  Me.Resetchg.Visible = False
  '
  'exmenuitem
  '
  Me.exmenuitem.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
  Me.exmenuitem.Name = "exmenuitem"
  Me.exmenuitem.Size = New System.Drawing.Size(39, 21)
  Me.exmenuitem.Text = "Exit"
  '
  'DeveloperToolStripMenuItem
  '
  Me.DeveloperToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ColorDialogToolStripMenuItem})
  Me.DeveloperToolStripMenuItem.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
  Me.DeveloperToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
  Me.DeveloperToolStripMenuItem.Name = "DeveloperToolStripMenuItem"
  Me.DeveloperToolStripMenuItem.Size = New System.Drawing.Size(73, 21)
  Me.DeveloperToolStripMenuItem.Text = "Developer"
  Me.DeveloperToolStripMenuItem.Visible = False
  '
  'ColorDialogToolStripMenuItem
  '
  Me.ColorDialogToolStripMenuItem.Name = "ColorDialogToolStripMenuItem"
  Me.ColorDialogToolStripMenuItem.Size = New System.Drawing.Size(140, 22)
  Me.ColorDialogToolStripMenuItem.Text = "Color Dialog"
  '
  'sp
  '
  Me.sp.Controls.Add(Me.secph)
  Me.sp.Location = New System.Drawing.Point(784, 150)
  Me.sp.Name = "sp"
  Me.sp.Size = New System.Drawing.Size(365, 53)
  Me.sp.TabIndex = 16
  Me.sp.TabStop = False
  '
  'secph
  '
  Me.secph.Enabled = False
  Me.secph.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
  Me.secph.Location = New System.Drawing.Point(7, 20)
  Me.secph.Name = "secph"
  Me.secph.Size = New System.Drawing.Size(352, 21)
  Me.secph.TabIndex = 0
  Me.secph.Visible = False
  '
  'GroupBox3
  '
  Me.GroupBox3.Controls.Add(Me.econtact)
  Me.GroupBox3.Location = New System.Drawing.Point(784, 210)
  Me.GroupBox3.Name = "GroupBox3"
  Me.GroupBox3.Size = New System.Drawing.Size(365, 53)
  Me.GroupBox3.TabIndex = 17
  Me.GroupBox3.TabStop = False
  Me.GroupBox3.Text = "Emergency Contact"
  '
  'econtact
  '
  Me.econtact.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
  Me.econtact.Location = New System.Drawing.Point(7, 20)
  Me.econtact.Name = "econtact"
  Me.econtact.Size = New System.Drawing.Size(352, 21)
  Me.econtact.TabIndex = 0
  '
  'sptitle
  '
  Me.sptitle.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
  Me.sptitle.Location = New System.Drawing.Point(791, 146)
  Me.sptitle.Name = "sptitle"
  Me.sptitle.Size = New System.Drawing.Size(352, 21)
  Me.sptitle.TabIndex = 9
  Me.sptitle.Visible = False
  '
  'ContextMenuStrip1
  '
  Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
  Me.ContextMenuStrip1.Size = New System.Drawing.Size(61, 4)
  '
  'Message_label
  '
  Me.Message_label.AutoSize = True
  Me.Message_label.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
  Me.Message_label.Location = New System.Drawing.Point(520, 8)
  Me.Message_label.Name = "Message_label"
  Me.Message_label.Size = New System.Drawing.Size(0, 13)
  Me.Message_label.TabIndex = 19
  '
  'ContextMenuStrip2
  '
  Me.ContextMenuStrip2.Name = "ContextMenuStrip2"
  Me.ContextMenuStrip2.Size = New System.Drawing.Size(61, 4)
  '
  'Practbox
  '
  Me.Practbox.BackColor = System.Drawing.SystemColors.Control
  Me.Practbox.Controls.Add(Me.Practice)
  Me.Practbox.Location = New System.Drawing.Point(18, 27)
  Me.Practbox.Name = "Practbox"
  Me.Practbox.Size = New System.Drawing.Size(365, 53)
  Me.Practbox.TabIndex = 20
  Me.Practbox.TabStop = False
  Me.Practbox.Text = "Practice"
  '
  'Practice
  '
  Me.Practice.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
  Me.Practice.Location = New System.Drawing.Point(6, 21)
  Me.Practice.Name = "Practice"
  Me.Practice.Size = New System.Drawing.Size(353, 21)
  Me.Practice.TabIndex = 0
  Me.Practice.TabStop = False
  '
  'Deptbox
  '
  Me.Deptbox.Controls.Add(Me.dept_tbox)
  Me.Deptbox.Controls.Add(Me.Department)
  Me.Deptbox.Location = New System.Drawing.Point(18, 90)
  Me.Deptbox.Name = "Deptbox"
  Me.Deptbox.Size = New System.Drawing.Size(365, 53)
  Me.Deptbox.TabIndex = 0
  Me.Deptbox.TabStop = False
  Me.Deptbox.Text = "Department"
  '
  'dept_tbox
  '
  Me.dept_tbox.Anchor = System.Windows.Forms.AnchorStyles.Left
  Me.dept_tbox.Font = New System.Drawing.Font("Calibri", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
  Me.dept_tbox.Location = New System.Drawing.Point(30, -62)
  Me.dept_tbox.Name = "dept_tbox"
  Me.dept_tbox.Size = New System.Drawing.Size(353, 21)
  Me.dept_tbox.TabIndex = 1
  '
  'Department
  '
  Me.Department.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
  Me.Department.FormattingEnabled = True
  Me.Department.Location = New System.Drawing.Point(6, 21)
  Me.Department.Name = "Department"
  Me.Department.Size = New System.Drawing.Size(353, 21)
  Me.Department.Sorted = True
  Me.Department.TabIndex = 0
  '
  'form1
  '
  Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
  Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
  Me.AutoScroll = True
  Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
  Me.ClientSize = New System.Drawing.Size(1168, 325)
  Me.Controls.Add(Me.Deptbox)
  Me.Controls.Add(Me.Practbox)
  Me.Controls.Add(Me.Message_label)
  Me.Controls.Add(Me.sptitle)
  Me.Controls.Add(Me.GroupBox3)
  Me.Controls.Add(Me.sp)
  Me.Controls.Add(Me.GroupBox8)
  Me.Controls.Add(Me.ppgb)
  Me.Controls.Add(Me.Printbox)
  Me.Controls.Add(Me.Dsbox)
  Me.Controls.Add(Me.MenuStrip1)
  Me.Controls.Add(Me.GroupBox2)
  Me.ForeColor = System.Drawing.SystemColors.ButtonFace
  Me.MainMenuStrip = Me.MenuStrip1
  Me.Name = "form1"
  Me.Text = "Medical Profile Card"
  Me.Dsbox.ResumeLayout(False)
  Me.GroupBox2.ResumeLayout(False)
  Me.GroupBox2.PerformLayout()
  Me.Tgp.ResumeLayout(False)
  Me.Tgp.PerformLayout()
  Me.Printbox.ResumeLayout(False)
  Me.ppgb.ResumeLayout(False)
  Me.ppgb.PerformLayout()
  Me.GroupBox8.ResumeLayout(False)
  Me.GroupBox8.PerformLayout()
  Me.MenuStrip1.ResumeLayout(False)
  Me.MenuStrip1.PerformLayout()
  Me.sp.ResumeLayout(False)
  Me.sp.PerformLayout()
  Me.GroupBox3.ResumeLayout(False)
  Me.GroupBox3.PerformLayout()
  Me.Practbox.ResumeLayout(False)
  Me.Practbox.PerformLayout()
  Me.Deptbox.ResumeLayout(False)
  Me.Deptbox.PerformLayout()
  Me.ResumeLayout(False)
  Me.PerformLayout()

 End Sub
 Friend WithEvents Dsbox As System.Windows.Forms.GroupBox
  Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
  Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
  Friend WithEvents Patient As System.Windows.Forms.TextBox
  Friend WithEvents Phone As System.Windows.Forms.TextBox
  Friend WithEvents DOB As System.Windows.Forms.TextBox
  Friend WithEvents Printbox As System.Windows.Forms.GroupBox
  Friend WithEvents Printers As System.Windows.Forms.ComboBox
  Friend WithEvents address As System.Windows.Forms.RichTextBox
  Friend WithEvents OpenFileDialog2 As System.Windows.Forms.OpenFileDialog
  Friend WithEvents FolderBrowserDialog1 As System.Windows.Forms.FolderBrowserDialog
  Friend WithEvents dsaves As System.Windows.Forms.ComboBox
  Friend WithEvents ppgb As System.Windows.Forms.GroupBox
  Friend WithEvents priph As System.Windows.Forms.TextBox
  Friend WithEvents GroupBox8 As System.Windows.Forms.GroupBox
  Friend WithEvents ins As System.Windows.Forms.TextBox
  Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
  Friend WithEvents sp As GroupBox
  Friend WithEvents secph As TextBox
  Friend WithEvents GroupBox3 As GroupBox
  Friend WithEvents econtact As TextBox
 Friend WithEvents Label4 As Label
 Friend WithEvents Label3 As Label
  Friend WithEvents Label2 As Label
  Friend WithEvents patient_label As Label
  Friend WithEvents sptitle As TextBox
  Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
  Friend WithEvents Message_label As Label
  Friend WithEvents ContextMenuStrip2 As ContextMenuStrip
  Friend WithEvents DeveloperToolStripMenuItem As ToolStripMenuItem
  Friend WithEvents ColorDialogToolStripMenuItem As ToolStripMenuItem
  Friend WithEvents Practbox As GroupBox
  Friend WithEvents Practice As TextBox
  Friend WithEvents Deptbox As GroupBox
  Friend WithEvents Department As ComboBox
  Friend WithEvents pid_label As Label
  Friend WithEvents dept_tbox As TextBox
  Friend WithEvents Patientid As TextBox
  Friend WithEvents prv_combo As ComboBox
  Friend WithEvents Editmenuitem As ToolStripMenuItem
  Friend WithEvents Previewmenuitem As ToolStripMenuItem
  Friend WithEvents Printmenuitem As ToolStripMenuItem
  Friend WithEvents Clrmenuitem As ToolStripMenuItem
  Friend WithEvents Notesmenuitem As ToolStripMenuItem
  Friend WithEvents Addpagemenuitem As ToolStripMenuItem
  Friend WithEvents Aspecmenuitem As ToolStripMenuItem
  Friend WithEvents exmenuitem As ToolStripMenuItem
  Friend WithEvents Resetchg As ToolStripMenuItem
  Friend WithEvents dest As TextBox
  Friend WithEvents Savemi As ToolStripMenuItem
  Friend WithEvents Deletemi As ToolStripMenuItem
 Friend WithEvents Tgp As GroupBox
 Friend WithEvents Ettb As TextBox
End Class
