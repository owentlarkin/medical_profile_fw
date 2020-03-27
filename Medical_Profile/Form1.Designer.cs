using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace Medical_Profile
{
 [DesignerGenerated()]
 public partial class Form1 : Form
 {

  // Form overrides dispose to clean up the component list.
  [DebuggerNonUserCode()]
  protected override void Dispose(bool disposing)
  {
   try
   {
    if (disposing && components is object)
    {
     components.Dispose();
    }
   }
   finally
   {
    base.Dispose(disposing);
   }
  }

  // Required by the Windows Form Designer
  private System.ComponentModel.IContainer components;

  // NOTE: The following procedure is required by the Windows Form Designer
  // It can be modified using the Windows Form Designer.  
  // Do not modify it using the code editor.
  [DebuggerStepThrough()]
  private void InitializeComponent()
  {
   components = new System.ComponentModel.Container();
   _Dsbox = new GroupBox();
   _dsaves = new ComboBox();
   _dsaves.SelectedIndexChanged += new EventHandler(Dsaves_SelectedIndexChanged);
   _dsaves.Enter += new EventHandler(Cb_Enter);
   _dsaves.Click += new EventHandler(Cb_Enter);
   _dsaves.Enter += new EventHandler(Cb_Enter);
   _dsaves.Click += new EventHandler(Cb_Enter);
   _dsaves.DropDown += new EventHandler(Cb_DropDown);
   _dsaves.DrawItem += new DrawItemEventHandler(Cb_DrawItem);
   _dsaves.TextUpdate += new EventHandler(Cb_TextChanged);
   _dsaves.DropDownClosed += new EventHandler(Cb_DropDownClosed);
   _dsaves.Validated += new EventHandler(Cb_Validated);
   _OpenFileDialog1 = new OpenFileDialog();
   _GroupBox2 = new GroupBox();
   _Tgp = new GroupBox();
   _Ettb = new TextBox();
   _dest = new TextBox();
   _Label4 = new Label();
   _Label2 = new Label();
   _Label3 = new Label();
   _pid_label = new Label();
   _patient_label = new Label();
   _Patientid = new TextBox();
   _Patientid.Click += new EventHandler(Patientid_Click);
   _Patientid.Enter += new EventHandler(Patientid_Enter);
   _Patientid.KeyPress += new KeyPressEventHandler(Patientid_KeyPress);
   _Patientid.Validating += new System.ComponentModel.CancelEventHandler(Patientid_Validating);
   _Patientid.Validated += new EventHandler(Patientid_Validated);
   _address = new RichTextBox();
   _address.TextChanged += new EventHandler(Address_TextChanged);
   _address.KeyPress += new KeyPressEventHandler(Edit_Keypress);
   _address.Enter += new EventHandler(Rtbbox_enter);
   _Phone = new TextBox();
   _Phone.TextChanged += new EventHandler(Phone_TextChanged);
   _Phone.KeyPress += new KeyPressEventHandler(Edit_Keypress);
   _Phone.Enter += new EventHandler(Textbox_enter);
   _DOB = new TextBox();
   _DOB.TextChanged += new EventHandler(DOB_TextChanged);
   _DOB.KeyPress += new KeyPressEventHandler(Edit_Keypress);
   _DOB.Enter += new EventHandler(Textbox_enter);
   _Patient = new TextBox();
   _Patient.Validated += new EventHandler(Patient_Validated);
   _Patient.Validating += new System.ComponentModel.CancelEventHandler(Patient_Validating);
   _Patient.Enter += new EventHandler(Patient_Enter);
   _Patient.Click += new EventHandler(Patient_Click);
   _Patient.KeyPress += new KeyPressEventHandler(Patient_KeyPress);
   _Printbox = new GroupBox();
   _Printers = new ComboBox();
   _Printers.KeyPress += new KeyPressEventHandler(Printers_KeyPress);
   _Printers.Click += new EventHandler(Cb_Enter);
   _Printers.Enter += new EventHandler(Cb_Enter);
   _Printers.Click += new EventHandler(Cb_Enter);
   _Printers.Enter += new EventHandler(Cb_Enter);
   _Printers.DropDown += new EventHandler(Cb_DropDown);
   _Printers.DrawItem += new DrawItemEventHandler(Cb_DrawItem);
   _Printers.TextUpdate += new EventHandler(Cb_TextChanged);
   _Printers.DropDownClosed += new EventHandler(Cb_DropDownClosed);
   _Printers.SelectedIndexChanged += new EventHandler(Printers_SelectedIndexChanged);
   _Printers.Validated += new EventHandler(Cb_Validated);
   _OpenFileDialog2 = new OpenFileDialog();
   _FolderBrowserDialog1 = new FolderBrowserDialog();
   _ppgb = new GroupBox();
   _prv_combo = new ComboBox();
   _prv_combo.KeyPress += new KeyPressEventHandler(Prv_combo_KeyPress);
   _prv_combo.TextChanged += new EventHandler(prv_combo_TextChanged);
   _prv_combo.Enter += new EventHandler(Cb_Enter);
   _prv_combo.Click += new EventHandler(Cb_Enter);
   _prv_combo.Enter += new EventHandler(Cb_Enter);
   _prv_combo.Click += new EventHandler(Cb_Enter);
   _prv_combo.DrawItem += new DrawItemEventHandler(Cb_DrawItem);
   _prv_combo.DropDownClosed += new EventHandler(Cb_DropDownClosed);
   _prv_combo.SelectedIndexChanged += new EventHandler(prv_combo_SelectedIndexChanged);
   _prv_combo.Validated += new EventHandler(Cb_Validated);
   _priph = new TextBox();
   _priph.TextChanged += new EventHandler(Priph_TextChanged);
   _priph.KeyPress += new KeyPressEventHandler(Edit_Keypress);
   _GroupBox8 = new GroupBox();
   _ins = new TextBox();
   _ins.TextChanged += new EventHandler(Ins_TextChanged);
   _ins.KeyPress += new KeyPressEventHandler(Edit_Keypress);
   _ins.Enter += new EventHandler(Textbox_enter);
   _MenuStrip1 = new MenuStrip();
   _Editmenuitem = new ToolStripMenuItem();
   _Editmenuitem.Click += new EventHandler(Editmenuitem_Click);
   _Savemi = new ToolStripMenuItem();
   _Savemi.Click += new EventHandler(Savemi_Click);
   _Deletemi = new ToolStripMenuItem();
   _Deletemi.Click += new EventHandler(Deletemi_Click);
   _Previewmenuitem = new ToolStripMenuItem();
   _Previewmenuitem.Click += new EventHandler(Previewmenuitem_Click);
   _Printmenuitem = new ToolStripMenuItem();
   _Printmenuitem.Click += new EventHandler(Printmenuitem_Click);
   _Clrmenuitem = new ToolStripMenuItem();
   _Clrmenuitem.Click += new EventHandler(ClearMenuItem_Click);
   _Notesmenuitem = new ToolStripMenuItem();
   _Notesmenuitem.Click += new EventHandler(Notesmenuitem_Click);
   _Addpagemenuitem = new ToolStripMenuItem();
   _Addpagemenuitem.Click += new EventHandler(AddBlockMenuItem_Click);
   _Aspecmenuitem = new ToolStripMenuItem();
   _Aspecmenuitem.Click += new EventHandler(Sp_Title_Click);
   _Resetchg = new ToolStripMenuItem();
   _exmenuitem = new ToolStripMenuItem();
   _exmenuitem.Click += new EventHandler(ExitMenuItem_Click);
   _DeveloperToolStripMenuItem = new ToolStripMenuItem();
   _ColorDialogToolStripMenuItem = new ToolStripMenuItem();
   _sp = new GroupBox();
   _secph = new TextBox();
   _secph.TextChanged += new EventHandler(Secph_TextChanged);
   _secph.Enter += new EventHandler(Textbox_enter);
   _GroupBox3 = new GroupBox();
   _econtact = new TextBox();
   _econtact.TextChanged += new EventHandler(Econtact_TextChanged);
   _econtact.KeyPress += new KeyPressEventHandler(Edit_Keypress);
   _econtact.Enter += new EventHandler(Textbox_enter);
   _sptitle = new TextBox();
   _sptitle.Leave += new EventHandler(Sptitle_Leave);
   _sptitle.KeyPress += new KeyPressEventHandler(Sptitle_KeyPress);
   _sptitle.Enter += new EventHandler(Textbox_enter);
   _ContextMenuStrip1 = new ContextMenuStrip(components);
   _Message_label = new Label();
   _ContextMenuStrip2 = new ContextMenuStrip(components);
   _Practbox = new GroupBox();
   _Practice = new TextBox();
   _Practice.KeyPress += new KeyPressEventHandler(Edit_Keypress);
   _Practice.Enter += new EventHandler(Textbox_enter);
   _Deptbox = new GroupBox();
   _dept_tbox = new TextBox();
   _dept_tbox.KeyPress += new KeyPressEventHandler(Dept_tbox_KeyPress);
   _dept_tbox.Enter += new EventHandler(Textbox_enter);
   _dept_tbox.Click += new EventHandler(Textbox_enter);
   _dept_tbox.Enter += new EventHandler(Textbox_enter);
   _dept_tbox.Click += new EventHandler(Textbox_enter);
   _dept_tbox.MouseEnter += new EventHandler(dept_tbox_MouseEnter);
   _Department = new ComboBox();
   _Department.KeyPress += new KeyPressEventHandler(Department_KeyPress);
   _Department.Click += new EventHandler(Cb_Enter);
   _Department.DropDown += new EventHandler(Cb_DropDown);
   _Department.DrawItem += new DrawItemEventHandler(Cb_DrawItem);
   _Department.TextUpdate += new EventHandler(Cb_TextChanged);
   _Department.TextChanged += new EventHandler(Cb_TextChanged);
   _Department.TextUpdate += new EventHandler(Cb_TextChanged);
   _Department.TextChanged += new EventHandler(Cb_TextChanged);
   _Department.DropDownClosed += new EventHandler(Cb_DropDownClosed);
   _Department.SelectedIndexChanged += new EventHandler(Department_SelectedIndexChanged);
   _Department.Validated += new EventHandler(Cb_Validated);
   _Dsbox.SuspendLayout();
   _GroupBox2.SuspendLayout();
   _Tgp.SuspendLayout();
   _Printbox.SuspendLayout();
   _ppgb.SuspendLayout();
   _GroupBox8.SuspendLayout();
   _MenuStrip1.SuspendLayout();
   _sp.SuspendLayout();
   _GroupBox3.SuspendLayout();
   _Practbox.SuspendLayout();
   _Deptbox.SuspendLayout();
   SuspendLayout();
   // 
   // Dsbox
   // 
   _Dsbox.Controls.Add(_dsaves);
   _Dsbox.Enabled = false;
   _Dsbox.Location = new Point(18, 210);
   _Dsbox.Name = "Dsbox";
   _Dsbox.Size = new Size(365, 53);
   _Dsbox.TabIndex = 0;
   _Dsbox.TabStop = false;
   _Dsbox.Text = "Saved Items";
   _Dsbox.Visible = false;
   // 
   // dsaves
   // 
   _dsaves.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
   _dsaves.AutoCompleteSource = AutoCompleteSource.ListItems;
   _dsaves.DrawMode = DrawMode.OwnerDrawFixed;
   _dsaves.Enabled = false;
   _dsaves.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
   _dsaves.FormattingEnabled = true;
   _dsaves.Location = new Point(6, 21);
   _dsaves.Name = "dsaves";
   _dsaves.Size = new Size(353, 22);
   _dsaves.TabIndex = 2;
   // 
   // OpenFileDialog1
   // 
   _OpenFileDialog1.FileName = "OpenFileDialog1";
   _OpenFileDialog1.Filter = "Patient Reports | *.doc;*.docx;*.xps|All Files|*.*";
   // 
   // GroupBox2
   // 
   _GroupBox2.Controls.Add(_Tgp);
   _GroupBox2.Controls.Add(_dest);
   _GroupBox2.Controls.Add(_Label4);
   _GroupBox2.Controls.Add(_Label2);
   _GroupBox2.Controls.Add(_Label3);
   _GroupBox2.Controls.Add(_pid_label);
   _GroupBox2.Controls.Add(_patient_label);
   _GroupBox2.Controls.Add(_Patientid);
   _GroupBox2.Controls.Add(_address);
   _GroupBox2.Controls.Add(_Phone);
   _GroupBox2.Controls.Add(_DOB);
   _GroupBox2.Controls.Add(_Patient);
   _GroupBox2.ForeColor = SystemColors.ActiveCaptionText;
   _GroupBox2.Location = new Point(401, 27);
   _GroupBox2.Name = "GroupBox2";
   _GroupBox2.Size = new Size(365, 236);
   _GroupBox2.TabIndex = 1;
   _GroupBox2.TabStop = false;
   // 
   // Tgp
   // 
   _Tgp.Controls.Add(_Ettb);
   _Tgp.Location = new Point(6, 182);
   _Tgp.Name = "Tgp";
   _Tgp.Size = new Size(198, 43);
   _Tgp.TabIndex = 13;
   _Tgp.TabStop = false;
   _Tgp.Text = "Elapsed ms";
   _Tgp.Visible = false;
   // 
   // Ettb
   // 
   _Ettb.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
   _Ettb.Location = new Point(6, 19);
   _Ettb.Name = "Ettb";
   _Ettb.Size = new Size(186, 21);
   _Ettb.TabIndex = 0;
   _Ettb.TextAlign = HorizontalAlignment.Right;
   // 
   // dest
   // 
   _dest.Location = new Point(223, 183);
   _dest.Name = "dest";
   _dest.Size = new Size(100, 20);
   _dest.TabIndex = 12;
   _dest.Visible = false;
   // 
   // Label4
   // 
   _Label4.AutoSize = true;
   _Label4.Location = new Point(220, 103);
   _Label4.Name = "Label4";
   _Label4.Size = new Size(38, 13);
   _Label4.TabIndex = 8;
   _Label4.Text = "Phone";
   // 
   // Label2
   // 
   _Label2.AutoSize = true;
   _Label2.Location = new Point(7, 52);
   _Label2.Name = "Label2";
   _Label2.Size = new Size(45, 13);
   _Label2.TabIndex = 6;
   _Label2.Text = "Address";
   // 
   // Label3
   // 
   _Label3.AutoSize = true;
   _Label3.Location = new Point(220, 52);
   _Label3.Name = "Label3";
   _Label3.Size = new Size(30, 13);
   _Label3.TabIndex = 7;
   _Label3.Text = "DOB";
   // 
   // pid_label
   // 
   _pid_label.AutoSize = true;
   _pid_label.Location = new Point(220, 1);
   _pid_label.Name = "pid_label";
   _pid_label.Size = new Size(51, 13);
   _pid_label.TabIndex = 10;
   _pid_label.Text = "Patient id";
   // 
   // patient_label
   // 
   _patient_label.AutoSize = true;
   _patient_label.Location = new Point(6, 1);
   _patient_label.Name = "patient_label";
   _patient_label.Size = new Size(40, 13);
   _patient_label.TabIndex = 5;
   _patient_label.Text = "Patient";
   // 
   // Patientid
   // 
   _Patientid.AcceptsTab = true;
   _Patientid.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
   _Patientid.Location = new Point(220, 23);
   _Patientid.Multiline = true;
   _Patientid.Name = "Patientid";
   _Patientid.Size = new Size(125, 20);
   _Patientid.TabIndex = 11;
   _Patientid.TextAlign = HorizontalAlignment.Right;
   // 
   // address
   // 
   _address.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
   _address.Location = new Point(6, 67);
   _address.Name = "address";
   _address.Size = new Size(198, 109);
   _address.TabIndex = 4;
   _address.Text = "";
   // 
   // Phone
   // 
   _Phone.BorderStyle = BorderStyle.FixedSingle;
   _Phone.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
   _Phone.Location = new Point(220, 123);
   _Phone.Name = "Phone";
   _Phone.Size = new Size(125, 21);
   _Phone.TabIndex = 3;
   // 
   // DOB
   // 
   _DOB.BorderStyle = BorderStyle.FixedSingle;
   _DOB.Font = new Font("Calibri", 11.25F, FontStyle.Bold, GraphicsUnit.Point, Conversions.ToByte(0));
   _DOB.Location = new Point(220, 74);
   _DOB.Name = "DOB";
   _DOB.Size = new Size(125, 26);
   _DOB.TabIndex = 1;
   // 
   // Patient
   // 
   _Patient.AcceptsTab = true;
   _Patient.BorderStyle = BorderStyle.FixedSingle;
   _Patient.Font = new Font("Calibri", 8.25F, FontStyle.Bold, GraphicsUnit.Point, Conversions.ToByte(0));
   _Patient.Location = new Point(6, 23);
   _Patient.Multiline = true;
   _Patient.Name = "Patient";
   _Patient.Size = new Size(198, 20);
   _Patient.TabIndex = 0;
   _Patient.Tag = "Patient";
   // 
   // Printbox
   // 
   _Printbox.Controls.Add(_Printers);
   _Printbox.Location = new Point(18, 150);
   _Printbox.Name = "Printbox";
   _Printbox.Size = new Size(365, 53);
   _Printbox.TabIndex = 10;
   _Printbox.TabStop = false;
   _Printbox.Text = "Printer";
   // 
   // Printers
   // 
   _Printers.DrawMode = DrawMode.OwnerDrawFixed;
   _Printers.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
   _Printers.FormattingEnabled = true;
   _Printers.Location = new Point(6, 19);
   _Printers.Name = "Printers";
   _Printers.Size = new Size(353, 22);
   _Printers.TabIndex = 0;
   // 
   // OpenFileDialog2
   // 
   _OpenFileDialog2.FileName = "OpenFileDialog2";
   // 
   // FolderBrowserDialog1
   // 
   _FolderBrowserDialog1.ShowNewFolderButton = false;
   // 
   // ppgb
   // 
   _ppgb.Controls.Add(_prv_combo);
   _ppgb.Controls.Add(_priph);
   _ppgb.Location = new Point(784, 27);
   _ppgb.Name = "ppgb";
   _ppgb.Size = new Size(365, 53);
   _ppgb.TabIndex = 13;
   _ppgb.TabStop = false;
   _ppgb.Text = "Primary Physician";
   // 
   // prv_combo
   // 
   _prv_combo.DrawMode = DrawMode.OwnerDrawFixed;
   _prv_combo.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
   _prv_combo.FormattingEnabled = true;
   _prv_combo.Location = new Point(7, 20);
   _prv_combo.Name = "prv_combo";
   _prv_combo.Size = new Size(353, 22);
   _prv_combo.TabIndex = 1;
   _prv_combo.Visible = false;
   // 
   // priph
   // 
   _priph.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
   _priph.Location = new Point(7, 20);
   _priph.Name = "priph";
   _priph.Size = new Size(353, 21);
   _priph.TabIndex = 0;
   // 
   // GroupBox8
   // 
   _GroupBox8.Controls.Add(_ins);
   _GroupBox8.Location = new Point(784, 90);
   _GroupBox8.Name = "GroupBox8";
   _GroupBox8.Size = new Size(365, 53);
   _GroupBox8.TabIndex = 14;
   _GroupBox8.TabStop = false;
   _GroupBox8.Text = "Insurance";
   // 
   // ins
   // 
   _ins.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
   _ins.Location = new Point(7, 20);
   _ins.Name = "ins";
   _ins.Size = new Size(352, 21);
   _ins.TabIndex = 0;
   // 
   // MenuStrip1
   // 
   _MenuStrip1.Dock = DockStyle.None;
   _MenuStrip1.Font = new Font("Calibri", 9.75F, FontStyle.Bold, GraphicsUnit.Point, Conversions.ToByte(0));
   _MenuStrip1.Items.AddRange(new ToolStripItem[] { _Editmenuitem, _Savemi, _Deletemi, _Previewmenuitem, _Printmenuitem, _Clrmenuitem, _Notesmenuitem, _Addpagemenuitem, _Aspecmenuitem, _Resetchg, _exmenuitem, _DeveloperToolStripMenuItem });
   _MenuStrip1.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
   _MenuStrip1.Location = new Point(9, -1);
   _MenuStrip1.Name = "MenuStrip1";
   _MenuStrip1.Size = new Size(563, 25);
   _MenuStrip1.TabIndex = 15;
   _MenuStrip1.Text = "MenuStrip1";
   // 
   // Editmenuitem
   // 
   _Editmenuitem.Checked = true;
   _Editmenuitem.CheckState = CheckState.Checked;
   _Editmenuitem.DisplayStyle = ToolStripItemDisplayStyle.Text;
   _Editmenuitem.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, Conversions.ToByte(0));
   _Editmenuitem.ForeColor = SystemColors.ActiveCaptionText;
   _Editmenuitem.Name = "Editmenuitem";
   _Editmenuitem.Size = new Size(43, 21);
   _Editmenuitem.Text = "Edit";
   // 
   // Savemi
   // 
   _Savemi.Name = "Savemi";
   _Savemi.Size = new Size(44, 21);
   _Savemi.Text = "Save";
   // 
   // Deletemi
   // 
   _Deletemi.Name = "Deletemi";
   _Deletemi.Size = new Size(56, 21);
   _Deletemi.Text = "Delete";
   // 
   // Previewmenuitem
   // 
   _Previewmenuitem.ForeColor = SystemColors.ActiveCaptionText;
   _Previewmenuitem.Name = "Previewmenuitem";
   _Previewmenuitem.Size = new Size(64, 21);
   _Previewmenuitem.Text = "Preview";
   // 
   // Printmenuitem
   // 
   _Printmenuitem.ForeColor = SystemColors.ActiveCaptionText;
   _Printmenuitem.Name = "Printmenuitem";
   _Printmenuitem.Size = new Size(46, 21);
   _Printmenuitem.Text = "Print";
   // 
   // Clrmenuitem
   // 
   _Clrmenuitem.ForeColor = SystemColors.ActiveCaptionText;
   _Clrmenuitem.Name = "Clrmenuitem";
   _Clrmenuitem.Size = new Size(47, 21);
   _Clrmenuitem.Text = "Clear";
   // 
   // Notesmenuitem
   // 
   _Notesmenuitem.ForeColor = SystemColors.ActiveCaptionText;
   _Notesmenuitem.Name = "Notesmenuitem";
   _Notesmenuitem.Size = new Size(52, 21);
   _Notesmenuitem.Text = "Notes";
   // 
   // Addpagemenuitem
   // 
   _Addpagemenuitem.ForeColor = SystemColors.ActiveCaptionText;
   _Addpagemenuitem.Name = "Addpagemenuitem";
   _Addpagemenuitem.Size = new Size(70, 21);
   _Addpagemenuitem.Text = "Add Page";
   // 
   // Aspecmenuitem
   // 
   _Aspecmenuitem.ForeColor = SystemColors.ActiveCaptionText;
   _Aspecmenuitem.Name = "Aspecmenuitem";
   _Aspecmenuitem.Size = new Size(94, 21);
   _Aspecmenuitem.Text = "Add Specialist";
   // 
   // Resetchg
   // 
   _Resetchg.Name = "Resetchg";
   _Resetchg.Size = new Size(98, 21);
   _Resetchg.Text = "Reset Changes";
   _Resetchg.Visible = false;
   // 
   // exmenuitem
   // 
   _exmenuitem.ForeColor = SystemColors.ActiveCaptionText;
   _exmenuitem.Name = "exmenuitem";
   _exmenuitem.Size = new Size(39, 21);
   _exmenuitem.Text = "Exit";
   // 
   // DeveloperToolStripMenuItem
   // 
   _DeveloperToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _ColorDialogToolStripMenuItem });
   _DeveloperToolStripMenuItem.Font = new Font("Segoe UI Semibold", 9.0F, FontStyle.Bold, GraphicsUnit.Point, Conversions.ToByte(0));
   _DeveloperToolStripMenuItem.ForeColor = SystemColors.ActiveCaptionText;
   _DeveloperToolStripMenuItem.Name = "DeveloperToolStripMenuItem";
   _DeveloperToolStripMenuItem.Size = new Size(73, 21);
   _DeveloperToolStripMenuItem.Text = "Developer";
   _DeveloperToolStripMenuItem.Visible = false;
   // 
   // ColorDialogToolStripMenuItem
   // 
   _ColorDialogToolStripMenuItem.Name = "ColorDialogToolStripMenuItem";
   _ColorDialogToolStripMenuItem.Size = new Size(140, 22);
   _ColorDialogToolStripMenuItem.Text = "Color Dialog";
   // 
   // sp
   // 
   _sp.Controls.Add(_secph);
   _sp.Location = new Point(784, 150);
   _sp.Name = "sp";
   _sp.Size = new Size(365, 53);
   _sp.TabIndex = 16;
   _sp.TabStop = false;
   // 
   // secph
   // 
   _secph.Enabled = false;
   _secph.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
   _secph.Location = new Point(7, 20);
   _secph.Name = "secph";
   _secph.Size = new Size(352, 21);
   _secph.TabIndex = 0;
   _secph.Visible = false;
   // 
   // GroupBox3
   // 
   _GroupBox3.Controls.Add(_econtact);
   _GroupBox3.Location = new Point(784, 210);
   _GroupBox3.Name = "GroupBox3";
   _GroupBox3.Size = new Size(365, 53);
   _GroupBox3.TabIndex = 17;
   _GroupBox3.TabStop = false;
   _GroupBox3.Text = "Emergency Contact";
   // 
   // econtact
   // 
   _econtact.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
   _econtact.Location = new Point(7, 20);
   _econtact.Name = "econtact";
   _econtact.Size = new Size(352, 21);
   _econtact.TabIndex = 0;
   // 
   // sptitle
   // 
   _sptitle.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
   _sptitle.Location = new Point(791, 146);
   _sptitle.Name = "sptitle";
   _sptitle.Size = new Size(352, 21);
   _sptitle.TabIndex = 9;
   _sptitle.Visible = false;
   // 
   // ContextMenuStrip1
   // 
   _ContextMenuStrip1.Name = "ContextMenuStrip1";
   _ContextMenuStrip1.Size = new Size(61, 4);
   // 
   // Message_label
   // 
   _Message_label.AutoSize = true;
   _Message_label.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, Conversions.ToByte(0));
   _Message_label.Location = new Point(520, 8);
   _Message_label.Name = "Message_label";
   _Message_label.Size = new Size(0, 13);
   _Message_label.TabIndex = 19;
   // 
   // ContextMenuStrip2
   // 
   _ContextMenuStrip2.Name = "ContextMenuStrip2";
   _ContextMenuStrip2.Size = new Size(61, 4);
   // 
   // Practbox
   // 
   _Practbox.BackColor = SystemColors.Control;
   _Practbox.Controls.Add(_Practice);
   _Practbox.Location = new Point(18, 27);
   _Practbox.Name = "Practbox";
   _Practbox.Size = new Size(365, 53);
   _Practbox.TabIndex = 20;
   _Practbox.TabStop = false;
   _Practbox.Text = "Practice";
   // 
   // Practice
   // 
   _Practice.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
   _Practice.Location = new Point(6, 21);
   _Practice.Name = "Practice";
   _Practice.Size = new Size(353, 21);
   _Practice.TabIndex = 0;
   _Practice.TabStop = false;
   // 
   // Deptbox
   // 
   _Deptbox.Controls.Add(_dept_tbox);
   _Deptbox.Controls.Add(_Department);
   _Deptbox.Location = new Point(18, 90);
   _Deptbox.Name = "Deptbox";
   _Deptbox.Size = new Size(365, 53);
   _Deptbox.TabIndex = 0;
   _Deptbox.TabStop = false;
   _Deptbox.Text = "Department";
   // 
   // dept_tbox
   // 
   _dept_tbox.Anchor = AnchorStyles.Left;
   _dept_tbox.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0));
   _dept_tbox.Location = new Point(30, -62);
   _dept_tbox.Name = "dept_tbox";
   _dept_tbox.Size = new Size(353, 21);
   _dept_tbox.TabIndex = 1;
   // 
   // Department
   // 
   _Department.DrawMode = DrawMode.OwnerDrawFixed;
   _Department.FormattingEnabled = true;
   _Department.Location = new Point(6, 21);
   _Department.Name = "Department";
   _Department.Size = new Size(353, 21);
   _Department.Sorted = true;
   _Department.TabIndex = 0;
   // 
   // form1
   // 
   AutoScaleDimensions = new SizeF(6.0F, 13.0F);
   AutoScaleMode = AutoScaleMode.Font;
   AutoScroll = true;
   AutoSizeMode = AutoSizeMode.GrowAndShrink;
   ClientSize = new Size(1168, 325);
   Controls.Add(_Deptbox);
   Controls.Add(_Practbox);
   Controls.Add(_Message_label);
   Controls.Add(_sptitle);
   Controls.Add(_GroupBox3);
   Controls.Add(_sp);
   Controls.Add(_GroupBox8);
   Controls.Add(_ppgb);
   Controls.Add(_Printbox);
   Controls.Add(_Dsbox);
   Controls.Add(_MenuStrip1);
   Controls.Add(_GroupBox2);
   ForeColor = SystemColors.ButtonFace;
   MainMenuStrip = _MenuStrip1;
   Name = "form1";
   Text = "Medical Profile Card";
   _Dsbox.ResumeLayout(false);
   _GroupBox2.ResumeLayout(false);
   _GroupBox2.PerformLayout();
   _Tgp.ResumeLayout(false);
   _Tgp.PerformLayout();
   _Printbox.ResumeLayout(false);
   _ppgb.ResumeLayout(false);
   _ppgb.PerformLayout();
   _GroupBox8.ResumeLayout(false);
   _GroupBox8.PerformLayout();
   _MenuStrip1.ResumeLayout(false);
   _MenuStrip1.PerformLayout();
   _sp.ResumeLayout(false);
   _sp.PerformLayout();
   _GroupBox3.ResumeLayout(false);
   _GroupBox3.PerformLayout();
   _Practbox.ResumeLayout(false);
   _Practbox.PerformLayout();
   _Deptbox.ResumeLayout(false);
   _Deptbox.PerformLayout();
   ResumeLayout(false);
   PerformLayout();
  }

  private GroupBox _Dsbox;

  internal GroupBox Dsbox
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _Dsbox;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_Dsbox != null)
    {
    }

    _Dsbox = value;
    if (_Dsbox != null)
    {
    }
   }
  }

  private OpenFileDialog _OpenFileDialog1;

  internal OpenFileDialog OpenFileDialog1
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _OpenFileDialog1;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_OpenFileDialog1 != null)
    {
    }

    _OpenFileDialog1 = value;
    if (_OpenFileDialog1 != null)
    {
    }
   }
  }

  private GroupBox _GroupBox2;

  internal GroupBox GroupBox2
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _GroupBox2;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_GroupBox2 != null)
    {
    }

    _GroupBox2 = value;
    if (_GroupBox2 != null)
    {
    }
   }
  }

  private TextBox _Patient;

  internal TextBox Patient
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _Patient;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_Patient != null)
    {
     _Patient.Validated -= Patient_Validated;
     _Patient.Validating -= Patient_Validating;
     _Patient.Enter -= Patient_Enter;
     _Patient.Click -= Patient_Click;
     _Patient.KeyPress -= Patient_KeyPress;
    }

    _Patient = value;
    if (_Patient != null)
    {
     _Patient.Validated += Patient_Validated;
     _Patient.Validating += Patient_Validating;
     _Patient.Enter += Patient_Enter;
     _Patient.Click += Patient_Click;
     _Patient.KeyPress += Patient_KeyPress;
    }
   }
  }

  private TextBox _Phone;

  internal TextBox Phone
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _Phone;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_Phone != null)
    {
     _Phone.TextChanged -= Phone_TextChanged;
     _Phone.KeyPress -= Edit_Keypress;
     _Phone.Enter -= Textbox_enter;
    }

    _Phone = value;
    if (_Phone != null)
    {
     _Phone.TextChanged += Phone_TextChanged;
     _Phone.KeyPress += Edit_Keypress;
     _Phone.Enter += Textbox_enter;
    }
   }
  }

  private TextBox _DOB;

  internal TextBox DOB
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _DOB;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_DOB != null)
    {
     _DOB.TextChanged -= DOB_TextChanged;
     _DOB.KeyPress -= Edit_Keypress;
     _DOB.Enter -= Textbox_enter;
    }

    _DOB = value;
    if (_DOB != null)
    {
     _DOB.TextChanged += DOB_TextChanged;
     _DOB.KeyPress += Edit_Keypress;
     _DOB.Enter += Textbox_enter;
    }
   }
  }

  private GroupBox _Printbox;

  internal GroupBox Printbox
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _Printbox;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_Printbox != null)
    {
    }

    _Printbox = value;
    if (_Printbox != null)
    {
    }
   }
  }

  private ComboBox _Printers;

  internal ComboBox Printers
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _Printers;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_Printers != null)
    {
     _Printers.KeyPress -= Printers_KeyPress;
     _Printers.Click -= Cb_Enter;
     _Printers.Enter -= Cb_Enter;
     _Printers.DropDown -= Cb_DropDown;
     _Printers.DrawItem -= Cb_DrawItem;
     _Printers.TextUpdate -= Cb_TextChanged;
     _Printers.DropDownClosed -= Cb_DropDownClosed;
     _Printers.SelectedIndexChanged -= Printers_SelectedIndexChanged;
     _Printers.Validated -= Cb_Validated;
    }

    _Printers = value;
    if (_Printers != null)
    {
     _Printers.KeyPress += Printers_KeyPress;
     _Printers.Click += Cb_Enter;
     _Printers.Enter += Cb_Enter;
     _Printers.DropDown += Cb_DropDown;
     _Printers.DrawItem += Cb_DrawItem;
     _Printers.TextUpdate += Cb_TextChanged;
     _Printers.DropDownClosed += Cb_DropDownClosed;
     _Printers.SelectedIndexChanged += Printers_SelectedIndexChanged;
     _Printers.Validated += Cb_Validated;
    }
   }
  }

  private RichTextBox _address;

  internal RichTextBox address
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _address;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_address != null)
    {
     _address.TextChanged -= Address_TextChanged;
     _address.KeyPress -= Edit_Keypress;
     _address.Enter -= Rtbbox_enter;
    }

    _address = value;
    if (_address != null)
    {
     _address.TextChanged += Address_TextChanged;
     _address.KeyPress += Edit_Keypress;
     _address.Enter += Rtbbox_enter;
    }
   }
  }

  private OpenFileDialog _OpenFileDialog2;

  internal OpenFileDialog OpenFileDialog2
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _OpenFileDialog2;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_OpenFileDialog2 != null)
    {
    }

    _OpenFileDialog2 = value;
    if (_OpenFileDialog2 != null)
    {
    }
   }
  }

  private FolderBrowserDialog _FolderBrowserDialog1;

  internal FolderBrowserDialog FolderBrowserDialog1
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _FolderBrowserDialog1;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_FolderBrowserDialog1 != null)
    {
    }

    _FolderBrowserDialog1 = value;
    if (_FolderBrowserDialog1 != null)
    {
    }
   }
  }

  private ComboBox _dsaves;

  internal ComboBox dsaves
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _dsaves;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_dsaves != null)
    {
     _dsaves.SelectedIndexChanged -= Dsaves_SelectedIndexChanged;
     _dsaves.Enter -= Cb_Enter;
     _dsaves.Click -= Cb_Enter;
     _dsaves.DropDown -= Cb_DropDown;
     _dsaves.DrawItem -= Cb_DrawItem;
     _dsaves.TextUpdate -= Cb_TextChanged;
     _dsaves.DropDownClosed -= Cb_DropDownClosed;
     _dsaves.Validated -= Cb_Validated;
    }

    _dsaves = value;
    if (_dsaves != null)
    {
     _dsaves.SelectedIndexChanged += Dsaves_SelectedIndexChanged;
     _dsaves.Enter += Cb_Enter;
     _dsaves.Click += Cb_Enter;
     _dsaves.DropDown += Cb_DropDown;
     _dsaves.DrawItem += Cb_DrawItem;
     _dsaves.TextUpdate += Cb_TextChanged;
     _dsaves.DropDownClosed += Cb_DropDownClosed;
     _dsaves.Validated += Cb_Validated;
    }
   }
  }

  private GroupBox _ppgb;

  internal GroupBox ppgb
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _ppgb;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_ppgb != null)
    {
    }

    _ppgb = value;
    if (_ppgb != null)
    {
    }
   }
  }

  private TextBox _priph;

  internal TextBox priph
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _priph;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_priph != null)
    {
     _priph.TextChanged -= Priph_TextChanged;
     _priph.KeyPress -= Edit_Keypress;
    }

    _priph = value;
    if (_priph != null)
    {
     _priph.TextChanged += Priph_TextChanged;
     _priph.KeyPress += Edit_Keypress;
    }
   }
  }

  private GroupBox _GroupBox8;

  internal GroupBox GroupBox8
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _GroupBox8;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_GroupBox8 != null)
    {
    }

    _GroupBox8 = value;
    if (_GroupBox8 != null)
    {
    }
   }
  }

  private TextBox _ins;

  internal TextBox ins
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _ins;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_ins != null)
    {
     _ins.TextChanged -= Ins_TextChanged;
     _ins.KeyPress -= Edit_Keypress;
     _ins.Enter -= Textbox_enter;
    }

    _ins = value;
    if (_ins != null)
    {
     _ins.TextChanged += Ins_TextChanged;
     _ins.KeyPress += Edit_Keypress;
     _ins.Enter += Textbox_enter;
    }
   }
  }

  private MenuStrip _MenuStrip1;

  internal MenuStrip MenuStrip1
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _MenuStrip1;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_MenuStrip1 != null)
    {
    }

    _MenuStrip1 = value;
    if (_MenuStrip1 != null)
    {
    }
   }
  }

  private GroupBox _sp;

  internal GroupBox sp
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _sp;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_sp != null)
    {
    }

    _sp = value;
    if (_sp != null)
    {
    }
   }
  }

  private TextBox _secph;

  internal TextBox secph
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _secph;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_secph != null)
    {
     _secph.TextChanged -= Secph_TextChanged;
     _secph.Enter -= Textbox_enter;
    }

    _secph = value;
    if (_secph != null)
    {
     _secph.TextChanged += Secph_TextChanged;
     _secph.Enter += Textbox_enter;
    }
   }
  }

  private GroupBox _GroupBox3;

  internal GroupBox GroupBox3
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _GroupBox3;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_GroupBox3 != null)
    {
    }

    _GroupBox3 = value;
    if (_GroupBox3 != null)
    {
    }
   }
  }

  private TextBox _econtact;

  internal TextBox econtact
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _econtact;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_econtact != null)
    {
     _econtact.TextChanged -= Econtact_TextChanged;
     _econtact.KeyPress -= Edit_Keypress;
     _econtact.Enter -= Textbox_enter;
    }

    _econtact = value;
    if (_econtact != null)
    {
     _econtact.TextChanged += Econtact_TextChanged;
     _econtact.KeyPress += Edit_Keypress;
     _econtact.Enter += Textbox_enter;
    }
   }
  }

  private Label _Label4;

  internal Label Label4
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _Label4;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_Label4 != null)
    {
    }

    _Label4 = value;
    if (_Label4 != null)
    {
    }
   }
  }

  private Label _Label3;

  internal Label Label3
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _Label3;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_Label3 != null)
    {
    }

    _Label3 = value;
    if (_Label3 != null)
    {
    }
   }
  }

  private Label _Label2;

  internal Label Label2
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _Label2;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_Label2 != null)
    {
    }

    _Label2 = value;
    if (_Label2 != null)
    {
    }
   }
  }

  private Label _patient_label;

  internal Label patient_label
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _patient_label;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_patient_label != null)
    {
    }

    _patient_label = value;
    if (_patient_label != null)
    {
    }
   }
  }

  private TextBox _sptitle;

  internal TextBox sptitle
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _sptitle;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_sptitle != null)
    {
     _sptitle.Leave -= Sptitle_Leave;
     _sptitle.KeyPress -= Sptitle_KeyPress;
     _sptitle.Enter -= Textbox_enter;
    }

    _sptitle = value;
    if (_sptitle != null)
    {
     _sptitle.Leave += Sptitle_Leave;
     _sptitle.KeyPress += Sptitle_KeyPress;
     _sptitle.Enter += Textbox_enter;
    }
   }
  }

  private ContextMenuStrip _ContextMenuStrip1;

  internal ContextMenuStrip ContextMenuStrip1
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _ContextMenuStrip1;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_ContextMenuStrip1 != null)
    {
    }

    _ContextMenuStrip1 = value;
    if (_ContextMenuStrip1 != null)
    {
    }
   }
  }

  private Label _Message_label;

  internal Label Message_label
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _Message_label;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_Message_label != null)
    {
    }

    _Message_label = value;
    if (_Message_label != null)
    {
    }
   }
  }

  private ContextMenuStrip _ContextMenuStrip2;

  internal ContextMenuStrip ContextMenuStrip2
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _ContextMenuStrip2;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_ContextMenuStrip2 != null)
    {
    }

    _ContextMenuStrip2 = value;
    if (_ContextMenuStrip2 != null)
    {
    }
   }
  }

  private ToolStripMenuItem _DeveloperToolStripMenuItem;

  internal ToolStripMenuItem DeveloperToolStripMenuItem
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _DeveloperToolStripMenuItem;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_DeveloperToolStripMenuItem != null)
    {
    }

    _DeveloperToolStripMenuItem = value;
    if (_DeveloperToolStripMenuItem != null)
    {
    }
   }
  }

  private ToolStripMenuItem _ColorDialogToolStripMenuItem;

  internal ToolStripMenuItem ColorDialogToolStripMenuItem
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _ColorDialogToolStripMenuItem;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_ColorDialogToolStripMenuItem != null)
    {
    }

    _ColorDialogToolStripMenuItem = value;
    if (_ColorDialogToolStripMenuItem != null)
    {
    }
   }
  }

  private GroupBox _Practbox;

  internal GroupBox Practbox
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _Practbox;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_Practbox != null)
    {
    }

    _Practbox = value;
    if (_Practbox != null)
    {
    }
   }
  }

  private TextBox _Practice;

  internal TextBox Practice
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _Practice;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_Practice != null)
    {
     _Practice.KeyPress -= Edit_Keypress;
     _Practice.Enter -= Textbox_enter;
    }

    _Practice = value;
    if (_Practice != null)
    {
     _Practice.KeyPress += Edit_Keypress;
     _Practice.Enter += Textbox_enter;
    }
   }
  }

  private GroupBox _Deptbox;

  internal GroupBox Deptbox
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _Deptbox;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_Deptbox != null)
    {
    }

    _Deptbox = value;
    if (_Deptbox != null)
    {
    }
   }
  }

  private ComboBox _Department;

  internal ComboBox Department
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _Department;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_Department != null)
    {
     _Department.KeyPress -= Department_KeyPress;
     _Department.Click -= Cb_Enter;
     _Department.DropDown -= Cb_DropDown;
     _Department.DrawItem -= Cb_DrawItem;
     _Department.TextUpdate -= Cb_TextChanged;
     _Department.TextChanged -= Cb_TextChanged;
     _Department.DropDownClosed -= Cb_DropDownClosed;
     _Department.SelectedIndexChanged -= Department_SelectedIndexChanged;
     _Department.Validated -= Cb_Validated;
    }

    _Department = value;
    if (_Department != null)
    {
     _Department.KeyPress += Department_KeyPress;
     _Department.Click += Cb_Enter;
     _Department.DropDown += Cb_DropDown;
     _Department.DrawItem += Cb_DrawItem;
     _Department.TextUpdate += Cb_TextChanged;
     _Department.TextChanged += Cb_TextChanged;
     _Department.DropDownClosed += Cb_DropDownClosed;
     _Department.SelectedIndexChanged += Department_SelectedIndexChanged;
     _Department.Validated += Cb_Validated;
    }
   }
  }

  private Label _pid_label;

  internal Label pid_label
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _pid_label;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_pid_label != null)
    {
    }

    _pid_label = value;
    if (_pid_label != null)
    {
    }
   }
  }

  private TextBox _dept_tbox;

  internal TextBox dept_tbox
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _dept_tbox;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_dept_tbox != null)
    {
     _dept_tbox.KeyPress -= Dept_tbox_KeyPress;
     _dept_tbox.Enter -= Textbox_enter;
     _dept_tbox.Click -= Textbox_enter;
     _dept_tbox.MouseEnter -= dept_tbox_MouseEnter;
    }

    _dept_tbox = value;
    if (_dept_tbox != null)
    {
     _dept_tbox.KeyPress += Dept_tbox_KeyPress;
     _dept_tbox.Enter += Textbox_enter;
     _dept_tbox.Click += Textbox_enter;
     _dept_tbox.MouseEnter += dept_tbox_MouseEnter;
    }
   }
  }

  private TextBox _Patientid;

  internal TextBox Patientid
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _Patientid;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_Patientid != null)
    {
     _Patientid.Click -= Patientid_Click;
     _Patientid.Enter -= Patientid_Enter;
     _Patientid.KeyPress -= Patientid_KeyPress;
     _Patientid.Validating -= Patientid_Validating;
     _Patientid.Validated -= Patientid_Validated;
    }

    _Patientid = value;
    if (_Patientid != null)
    {
     _Patientid.Click += Patientid_Click;
     _Patientid.Enter += Patientid_Enter;
     _Patientid.KeyPress += Patientid_KeyPress;
     _Patientid.Validating += Patientid_Validating;
     _Patientid.Validated += Patientid_Validated;
    }
   }
  }

  private ComboBox _prv_combo;

  internal ComboBox prv_combo
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _prv_combo;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_prv_combo != null)
    {
     _prv_combo.KeyPress -= Prv_combo_KeyPress;
     _prv_combo.TextChanged -= prv_combo_TextChanged;
     _prv_combo.Enter -= Cb_Enter;
     _prv_combo.Click -= Cb_Enter;
     _prv_combo.DrawItem -= Cb_DrawItem;
     _prv_combo.DropDownClosed -= Cb_DropDownClosed;
     _prv_combo.SelectedIndexChanged -= prv_combo_SelectedIndexChanged;
     _prv_combo.Validated -= Cb_Validated;
    }

    _prv_combo = value;
    if (_prv_combo != null)
    {
     _prv_combo.KeyPress += Prv_combo_KeyPress;
     _prv_combo.TextChanged += prv_combo_TextChanged;
     _prv_combo.Enter += Cb_Enter;
     _prv_combo.Click += Cb_Enter;
     _prv_combo.DrawItem += Cb_DrawItem;
     _prv_combo.DropDownClosed += Cb_DropDownClosed;
     _prv_combo.SelectedIndexChanged += prv_combo_SelectedIndexChanged;
     _prv_combo.Validated += Cb_Validated;
    }
   }
  }

  private ToolStripMenuItem _Editmenuitem;

  internal ToolStripMenuItem Editmenuitem
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _Editmenuitem;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_Editmenuitem != null)
    {
     _Editmenuitem.Click -= Editmenuitem_Click;
    }

    _Editmenuitem = value;
    if (_Editmenuitem != null)
    {
     _Editmenuitem.Click += Editmenuitem_Click;
    }
   }
  }

  private ToolStripMenuItem _Previewmenuitem;

  internal ToolStripMenuItem Previewmenuitem
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _Previewmenuitem;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_Previewmenuitem != null)
    {
     _Previewmenuitem.Click -= Previewmenuitem_Click;
    }

    _Previewmenuitem = value;
    if (_Previewmenuitem != null)
    {
     _Previewmenuitem.Click += Previewmenuitem_Click;
    }
   }
  }

  private ToolStripMenuItem _Printmenuitem;

  internal ToolStripMenuItem Printmenuitem
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _Printmenuitem;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_Printmenuitem != null)
    {
     _Printmenuitem.Click -= Printmenuitem_Click;
    }

    _Printmenuitem = value;
    if (_Printmenuitem != null)
    {
     _Printmenuitem.Click += Printmenuitem_Click;
    }
   }
  }

  private ToolStripMenuItem _Clrmenuitem;

  internal ToolStripMenuItem Clrmenuitem
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _Clrmenuitem;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_Clrmenuitem != null)
    {
     _Clrmenuitem.Click -= ClearMenuItem_Click;
    }

    _Clrmenuitem = value;
    if (_Clrmenuitem != null)
    {
     _Clrmenuitem.Click += ClearMenuItem_Click;
    }
   }
  }

  private ToolStripMenuItem _Notesmenuitem;

  internal ToolStripMenuItem Notesmenuitem
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _Notesmenuitem;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_Notesmenuitem != null)
    {
     _Notesmenuitem.Click -= Notesmenuitem_Click;
    }

    _Notesmenuitem = value;
    if (_Notesmenuitem != null)
    {
     _Notesmenuitem.Click += Notesmenuitem_Click;
    }
   }
  }

  private ToolStripMenuItem _Addpagemenuitem;

  internal ToolStripMenuItem Addpagemenuitem
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _Addpagemenuitem;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_Addpagemenuitem != null)
    {
     _Addpagemenuitem.Click -= AddBlockMenuItem_Click;
    }

    _Addpagemenuitem = value;
    if (_Addpagemenuitem != null)
    {
     _Addpagemenuitem.Click += AddBlockMenuItem_Click;
    }
   }
  }

  private ToolStripMenuItem _Aspecmenuitem;

  internal ToolStripMenuItem Aspecmenuitem
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _Aspecmenuitem;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_Aspecmenuitem != null)
    {
     _Aspecmenuitem.Click -= Sp_Title_Click;
    }

    _Aspecmenuitem = value;
    if (_Aspecmenuitem != null)
    {
     _Aspecmenuitem.Click += Sp_Title_Click;
    }
   }
  }

  private ToolStripMenuItem _exmenuitem;

  internal ToolStripMenuItem exmenuitem
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _exmenuitem;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_exmenuitem != null)
    {
     _exmenuitem.Click -= ExitMenuItem_Click;
    }

    _exmenuitem = value;
    if (_exmenuitem != null)
    {
     _exmenuitem.Click += ExitMenuItem_Click;
    }
   }
  }

  private ToolStripMenuItem _Resetchg;

  internal ToolStripMenuItem Resetchg
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _Resetchg;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_Resetchg != null)
    {
    }

    _Resetchg = value;
    if (_Resetchg != null)
    {
    }
   }
  }

  private TextBox _dest;

  internal TextBox dest
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _dest;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_dest != null)
    {
    }

    _dest = value;
    if (_dest != null)
    {
    }
   }
  }

  private ToolStripMenuItem _Savemi;

  internal ToolStripMenuItem Savemi
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _Savemi;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_Savemi != null)
    {
     _Savemi.Click -= Savemi_Click;
    }

    _Savemi = value;
    if (_Savemi != null)
    {
     _Savemi.Click += Savemi_Click;
    }
   }
  }

  private ToolStripMenuItem _Deletemi;

  internal ToolStripMenuItem Deletemi
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _Deletemi;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_Deletemi != null)
    {
     _Deletemi.Click -= Deletemi_Click;
    }

    _Deletemi = value;
    if (_Deletemi != null)
    {
     _Deletemi.Click += Deletemi_Click;
    }
   }
  }

  private GroupBox _Tgp;

  internal GroupBox Tgp
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _Tgp;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_Tgp != null)
    {
    }

    _Tgp = value;
    if (_Tgp != null)
    {
    }
   }
  }

  private TextBox _Ettb;

  internal TextBox Ettb
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _Ettb;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_Ettb != null)
    {
    }

    _Ettb = value;
    if (_Ettb != null)
    {
    }
   }
  }
 }
}