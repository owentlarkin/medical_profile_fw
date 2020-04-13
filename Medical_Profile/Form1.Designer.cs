using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace Medical_Profile
{
// [DesignerGenerated()]
 public partial class Form1 : Form
 {

  // Form overrides dispose to clean up the component list.
 // [DebuggerNonUserCode()]
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
   this.components = new System.ComponentModel.Container();
   this.Dsbox = new System.Windows.Forms.GroupBox();
   this.dsaves = new System.Windows.Forms.ComboBox();
   this.OpenFileDialog1 = new System.Windows.Forms.OpenFileDialog();
   this.GroupBox2 = new System.Windows.Forms.GroupBox();
   this.Tgp = new System.Windows.Forms.GroupBox();
   this.Ettb = new System.Windows.Forms.TextBox();
   this.dest = new System.Windows.Forms.TextBox();
   this.Label4 = new System.Windows.Forms.Label();
   this.Label2 = new System.Windows.Forms.Label();
   this.Label3 = new System.Windows.Forms.Label();
   this.pid_label = new System.Windows.Forms.Label();
   this.patient_label = new System.Windows.Forms.Label();
   this.Patientid = new System.Windows.Forms.TextBox();
   this.address = new System.Windows.Forms.RichTextBox();
   this.Phone = new System.Windows.Forms.TextBox();
   this.DOB = new System.Windows.Forms.TextBox();
   this.Patient = new System.Windows.Forms.TextBox();
   this.Printbox = new System.Windows.Forms.GroupBox();
   this.Printers = new System.Windows.Forms.ComboBox();
   this.OpenFileDialog2 = new System.Windows.Forms.OpenFileDialog();
   this.FolderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
   this.ppgb = new System.Windows.Forms.GroupBox();
   this.prv_combo = new System.Windows.Forms.ComboBox();
   this.priph = new System.Windows.Forms.TextBox();
   this.GroupBox8 = new System.Windows.Forms.GroupBox();
   this.ins = new System.Windows.Forms.TextBox();
   this.MenuStrip1 = new System.Windows.Forms.MenuStrip();
   this.Editmenuitem = new System.Windows.Forms.ToolStripMenuItem();
   this.Savemi = new System.Windows.Forms.ToolStripMenuItem();
   this.Deletemi = new System.Windows.Forms.ToolStripMenuItem();
   this.Previewmenuitem = new System.Windows.Forms.ToolStripMenuItem();
   this.Printmenuitem = new System.Windows.Forms.ToolStripMenuItem();
   this.Clrmenuitem = new System.Windows.Forms.ToolStripMenuItem();
   this.Notesmenuitem = new System.Windows.Forms.ToolStripMenuItem();
   this.Addpagemenuitem = new System.Windows.Forms.ToolStripMenuItem();
   this.Aspecmenuitem = new System.Windows.Forms.ToolStripMenuItem();
   this.Resetchg = new System.Windows.Forms.ToolStripMenuItem();
   this.exmenuitem = new System.Windows.Forms.ToolStripMenuItem();
   this.DeveloperToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
   this.ColorDialogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
   this.sp = new System.Windows.Forms.GroupBox();
   this.secph = new System.Windows.Forms.TextBox();
   this.GroupBox3 = new System.Windows.Forms.GroupBox();
   this.econtact = new System.Windows.Forms.TextBox();
   this.sptitle = new System.Windows.Forms.TextBox();
   this.ContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
   this.Message_label = new System.Windows.Forms.Label();
   this.ContextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
   this.Practbox = new System.Windows.Forms.GroupBox();
   this.Practice = new System.Windows.Forms.TextBox();
   this.Deptbox = new System.Windows.Forms.GroupBox();
   this.dept_tbox = new System.Windows.Forms.TextBox();
   this.Department = new System.Windows.Forms.ComboBox();
   this.Dsbox.SuspendLayout();
   this.GroupBox2.SuspendLayout();
   this.Tgp.SuspendLayout();
   this.Printbox.SuspendLayout();
   this.ppgb.SuspendLayout();
   this.GroupBox8.SuspendLayout();
   this.MenuStrip1.SuspendLayout();
   this.sp.SuspendLayout();
   this.GroupBox3.SuspendLayout();
   this.Practbox.SuspendLayout();
   this.Deptbox.SuspendLayout();
   this.SuspendLayout();
   // 
   // Dsbox
   // 
   this.Dsbox.Controls.Add(this.dsaves);
   this.Dsbox.Enabled = false;
   this.Dsbox.Location = new System.Drawing.Point(18, 210);
   this.Dsbox.Name = "Dsbox";
   this.Dsbox.Size = new System.Drawing.Size(365, 53);
   this.Dsbox.TabIndex = 0;
   this.Dsbox.TabStop = false;
   this.Dsbox.Text = "Saved Items";
   this.Dsbox.Visible = false;
   // 
   // dsaves
   // 
   this.dsaves.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
   this.dsaves.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
   this.dsaves.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
   this.dsaves.Enabled = false;
   this.dsaves.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
   this.dsaves.FormattingEnabled = true;
   this.dsaves.Location = new System.Drawing.Point(6, 21);
   this.dsaves.Name = "dsaves";
   this.dsaves.Size = new System.Drawing.Size(353, 22);
   this.dsaves.TabIndex = 2;
   this.dsaves.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.Cb_DrawItem);
   this.dsaves.DropDown += new System.EventHandler(this.Cb_DropDown);
   this.dsaves.SelectedIndexChanged += new System.EventHandler(this.Dsaves_SelectedIndexChanged);
   this.dsaves.TextUpdate += new System.EventHandler(this.Cb_TextChanged);
   this.dsaves.DropDownClosed += new System.EventHandler(this.Cb_DropDownClosed);
   this.dsaves.Click += new System.EventHandler(this.Cb_Enter);
   this.dsaves.Enter += new System.EventHandler(this.Cb_Enter);
   this.dsaves.Validated += new System.EventHandler(this.Cb_Validated);
   // 
   // OpenFileDialog1
   // 
   this.OpenFileDialog1.FileName = "OpenFileDialog1";
   this.OpenFileDialog1.Filter = "Patient Reports | *.doc;*.docx;*.xps|All Files|*.*";
   // 
   // GroupBox2
   // 
   this.GroupBox2.Controls.Add(this.Tgp);
   this.GroupBox2.Controls.Add(this.dest);
   this.GroupBox2.Controls.Add(this.Label4);
   this.GroupBox2.Controls.Add(this.Label2);
   this.GroupBox2.Controls.Add(this.Label3);
   this.GroupBox2.Controls.Add(this.pid_label);
   this.GroupBox2.Controls.Add(this.patient_label);
   this.GroupBox2.Controls.Add(this.Patientid);
   this.GroupBox2.Controls.Add(this.address);
   this.GroupBox2.Controls.Add(this.Phone);
   this.GroupBox2.Controls.Add(this.DOB);
   this.GroupBox2.Controls.Add(this.Patient);
   this.GroupBox2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
   this.GroupBox2.Location = new System.Drawing.Point(401, 27);
   this.GroupBox2.Name = "GroupBox2";
   this.GroupBox2.Size = new System.Drawing.Size(365, 236);
   this.GroupBox2.TabIndex = 1;
   this.GroupBox2.TabStop = false;
   // 
   // Tgp
   // 
   this.Tgp.Controls.Add(this.Ettb);
   this.Tgp.Location = new System.Drawing.Point(6, 182);
   this.Tgp.Name = "Tgp";
   this.Tgp.Size = new System.Drawing.Size(198, 43);
   this.Tgp.TabIndex = 13;
   this.Tgp.TabStop = false;
   this.Tgp.Text = "Elapsed ms";
   this.Tgp.Visible = false;
   // 
   // Ettb
   // 
   this.Ettb.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
   this.Ettb.Location = new System.Drawing.Point(6, 19);
   this.Ettb.Name = "Ettb";
   this.Ettb.Size = new System.Drawing.Size(186, 21);
   this.Ettb.TabIndex = 0;
   this.Ettb.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
   // 
   // dest
   // 
   this.dest.Location = new System.Drawing.Point(223, 183);
   this.dest.Name = "dest";
   this.dest.Size = new System.Drawing.Size(100, 20);
   this.dest.TabIndex = 12;
   this.dest.Visible = false;
   // 
   // Label4
   // 
   this.Label4.AutoSize = true;
   this.Label4.Location = new System.Drawing.Point(220, 103);
   this.Label4.Name = "Label4";
   this.Label4.Size = new System.Drawing.Size(38, 13);
   this.Label4.TabIndex = 8;
   this.Label4.Text = "Phone";
   // 
   // Label2
   // 
   this.Label2.AutoSize = true;
   this.Label2.Location = new System.Drawing.Point(7, 52);
   this.Label2.Name = "Label2";
   this.Label2.Size = new System.Drawing.Size(45, 13);
   this.Label2.TabIndex = 6;
   this.Label2.Text = "Address";
   // 
   // Label3
   // 
   this.Label3.AutoSize = true;
   this.Label3.Location = new System.Drawing.Point(220, 52);
   this.Label3.Name = "Label3";
   this.Label3.Size = new System.Drawing.Size(30, 13);
   this.Label3.TabIndex = 7;
   this.Label3.Text = "DOB";
   // 
   // pid_label
   // 
   this.pid_label.AutoSize = true;
   this.pid_label.Location = new System.Drawing.Point(220, 1);
   this.pid_label.Name = "pid_label";
   this.pid_label.Size = new System.Drawing.Size(51, 13);
   this.pid_label.TabIndex = 10;
   this.pid_label.Text = "Patient id";
   // 
   // patient_label
   // 
   this.patient_label.AutoSize = true;
   this.patient_label.Location = new System.Drawing.Point(6, 1);
   this.patient_label.Name = "patient_label";
   this.patient_label.Size = new System.Drawing.Size(40, 13);
   this.patient_label.TabIndex = 5;
   this.patient_label.Text = "Patient";
   // 
   // Patientid
   // 
   this.Patientid.AcceptsTab = true;
   this.Patientid.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
   this.Patientid.Location = new System.Drawing.Point(220, 23);
   this.Patientid.Multiline = true;
   this.Patientid.Name = "Patientid";
   this.Patientid.Size = new System.Drawing.Size(125, 20);
   this.Patientid.TabIndex = 11;
   this.Patientid.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
   this.Patientid.Click += new System.EventHandler(this.Patientid_Click);
   this.Patientid.Enter += new System.EventHandler(this.Patientid_Enter);
   this.Patientid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Patientid_KeyPress);
   this.Patientid.Validating += new System.ComponentModel.CancelEventHandler(this.Patientid_Validating);
   this.Patientid.Validated += new System.EventHandler(this.Patientid_Validated);
   // 
   // address
   // 
   this.address.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
   this.address.Location = new System.Drawing.Point(6, 67);
   this.address.Name = "address";
   this.address.Size = new System.Drawing.Size(198, 109);
   this.address.TabIndex = 4;
   this.address.Text = "";
   this.address.TextChanged += new System.EventHandler(this.Address_TextChanged);
   this.address.Enter += new System.EventHandler(this.Rtbbox_enter);
   this.address.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Edit_Keypress);
   // 
   // Phone
   // 
   this.Phone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
   this.Phone.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
   this.Phone.Location = new System.Drawing.Point(220, 123);
   this.Phone.Name = "Phone";
   this.Phone.Size = new System.Drawing.Size(125, 21);
   this.Phone.TabIndex = 3;
   this.Phone.TextChanged += new System.EventHandler(this.Phone_TextChanged);
   this.Phone.Enter += new System.EventHandler(this.Textbox_enter);
   this.Phone.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Edit_Keypress);
   // 
   // DOB
   // 
   this.DOB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
   this.DOB.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
   this.DOB.Location = new System.Drawing.Point(220, 74);
   this.DOB.Name = "DOB";
   this.DOB.Size = new System.Drawing.Size(125, 26);
   this.DOB.TabIndex = 1;
   this.DOB.TextChanged += new System.EventHandler(this.DOB_TextChanged);
   this.DOB.Enter += new System.EventHandler(this.Textbox_enter);
   this.DOB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Edit_Keypress);
   // 
   // Patient
   // 
   this.Patient.AcceptsTab = true;
   this.Patient.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
   this.Patient.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
   this.Patient.Location = new System.Drawing.Point(6, 23);
   this.Patient.Multiline = true;
   this.Patient.Name = "Patient";
   this.Patient.Size = new System.Drawing.Size(198, 20);
   this.Patient.TabIndex = 0;
   this.Patient.Tag = "Patient";
   this.Patient.Click += new System.EventHandler(this.Patient_Click);
   this.Patient.Enter += new System.EventHandler(this.Patient_Enter);
   this.Patient.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Patient_KeyPress);
   this.Patient.Validating += new System.ComponentModel.CancelEventHandler(this.Patient_Validating);
   this.Patient.Validated += new System.EventHandler(this.Patient_Validated);
   // 
   // Printbox
   // 
   this.Printbox.Controls.Add(this.Printers);
   this.Printbox.Location = new System.Drawing.Point(18, 150);
   this.Printbox.Name = "Printbox";
   this.Printbox.Size = new System.Drawing.Size(365, 53);
   this.Printbox.TabIndex = 10;
   this.Printbox.TabStop = false;
   this.Printbox.Text = "Printer";
   // 
   // Printers
   // 
   this.Printers.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
   this.Printers.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
   this.Printers.FormattingEnabled = true;
   this.Printers.Location = new System.Drawing.Point(6, 19);
   this.Printers.Name = "Printers";
   this.Printers.Size = new System.Drawing.Size(353, 22);
   this.Printers.TabIndex = 0;
   this.Printers.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.Cb_DrawItem);
   this.Printers.DropDown += new System.EventHandler(this.Cb_DropDown);
   this.Printers.SelectedIndexChanged += new System.EventHandler(this.Printers_SelectedIndexChanged);
   this.Printers.TextUpdate += new System.EventHandler(this.Cb_TextChanged);
   this.Printers.DropDownClosed += new System.EventHandler(this.Cb_DropDownClosed);
   this.Printers.Click += new System.EventHandler(this.Cb_Enter);
   this.Printers.Enter += new System.EventHandler(this.Cb_Enter);
   this.Printers.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Printers_KeyPress);
   this.Printers.Validated += new System.EventHandler(this.Cb_Validated);
   // 
   // OpenFileDialog2
   // 
   this.OpenFileDialog2.FileName = "OpenFileDialog2";
   // 
   // FolderBrowserDialog1
   // 
   this.FolderBrowserDialog1.ShowNewFolderButton = false;
   // 
   // ppgb
   // 
   this.ppgb.Controls.Add(this.prv_combo);
   this.ppgb.Controls.Add(this.priph);
   this.ppgb.Location = new System.Drawing.Point(784, 27);
   this.ppgb.Name = "ppgb";
   this.ppgb.Size = new System.Drawing.Size(365, 53);
   this.ppgb.TabIndex = 13;
   this.ppgb.TabStop = false;
   this.ppgb.Text = "Primary Physician";
   // 
   // prv_combo
   // 
   this.prv_combo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
   this.prv_combo.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
   this.prv_combo.FormattingEnabled = true;
   this.prv_combo.Location = new System.Drawing.Point(7, 20);
   this.prv_combo.Name = "prv_combo";
   this.prv_combo.Size = new System.Drawing.Size(353, 22);
   this.prv_combo.TabIndex = 1;
   this.prv_combo.Visible = false;
   this.prv_combo.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.Cb_DrawItem);
   this.prv_combo.SelectedIndexChanged += new System.EventHandler(this.prv_combo_SelectedIndexChanged);
   this.prv_combo.DropDownClosed += new System.EventHandler(this.Cb_DropDownClosed);
   this.prv_combo.TextChanged += new System.EventHandler(this.prv_combo_TextChanged);
   this.prv_combo.Click += new System.EventHandler(this.Cb_Enter);
   this.prv_combo.Enter += new System.EventHandler(this.Cb_Enter);
   this.prv_combo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Prv_combo_KeyPress);
   this.prv_combo.Validated += new System.EventHandler(this.Cb_Validated);
   // 
   // priph
   // 
   this.priph.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
   this.priph.Location = new System.Drawing.Point(7, 20);
   this.priph.Name = "priph";
   this.priph.Size = new System.Drawing.Size(353, 21);
   this.priph.TabIndex = 0;
   this.priph.TextChanged += new System.EventHandler(this.Priph_TextChanged);
   this.priph.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Edit_Keypress);
   // 
   // GroupBox8
   // 
   this.GroupBox8.Controls.Add(this.ins);
   this.GroupBox8.Location = new System.Drawing.Point(784, 90);
   this.GroupBox8.Name = "GroupBox8";
   this.GroupBox8.Size = new System.Drawing.Size(365, 53);
   this.GroupBox8.TabIndex = 14;
   this.GroupBox8.TabStop = false;
   this.GroupBox8.Text = "Insurance";
   // 
   // ins
   // 
   this.ins.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
   this.ins.Location = new System.Drawing.Point(7, 20);
   this.ins.Name = "ins";
   this.ins.Size = new System.Drawing.Size(352, 21);
   this.ins.TabIndex = 0;
   this.ins.TextChanged += new System.EventHandler(this.Ins_TextChanged);
   this.ins.Enter += new System.EventHandler(this.Textbox_enter);
   this.ins.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Edit_Keypress);
   // 
   // MenuStrip1
   // 
   this.MenuStrip1.Dock = System.Windows.Forms.DockStyle.None;
   this.MenuStrip1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
   this.MenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Editmenuitem,
            this.Savemi,
            this.Deletemi,
            this.Previewmenuitem,
            this.Printmenuitem,
            this.Clrmenuitem,
            this.Notesmenuitem,
            this.Addpagemenuitem,
            this.Aspecmenuitem,
            this.Resetchg,
            this.exmenuitem,
            this.DeveloperToolStripMenuItem});
   this.MenuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
   this.MenuStrip1.Location = new System.Drawing.Point(9, -1);
   this.MenuStrip1.Name = "MenuStrip1";
   this.MenuStrip1.Size = new System.Drawing.Size(563, 25);
   this.MenuStrip1.TabIndex = 15;
   this.MenuStrip1.Text = "MenuStrip1";
   // 
   // Editmenuitem
   // 
   this.Editmenuitem.Checked = true;
   this.Editmenuitem.CheckState = System.Windows.Forms.CheckState.Checked;
   this.Editmenuitem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
   this.Editmenuitem.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
   this.Editmenuitem.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
   this.Editmenuitem.Name = "Editmenuitem";
   this.Editmenuitem.Size = new System.Drawing.Size(43, 21);
   this.Editmenuitem.Text = "Edit";
   this.Editmenuitem.Click += new System.EventHandler(this.Editmenuitem_Click);
   // 
   // Savemi
   // 
   this.Savemi.Name = "Savemi";
   this.Savemi.Size = new System.Drawing.Size(44, 21);
   this.Savemi.Text = "Save";
   this.Savemi.Click += new System.EventHandler(this.Savemi_Click);
   // 
   // Deletemi
   // 
   this.Deletemi.Name = "Deletemi";
   this.Deletemi.Size = new System.Drawing.Size(56, 21);
   this.Deletemi.Text = "Delete";
   this.Deletemi.Click += new System.EventHandler(this.Deletemi_Click);
   // 
   // Previewmenuitem
   // 
   this.Previewmenuitem.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
   this.Previewmenuitem.Name = "Previewmenuitem";
   this.Previewmenuitem.Size = new System.Drawing.Size(64, 21);
   this.Previewmenuitem.Text = "Preview";
   this.Previewmenuitem.Click += new System.EventHandler(this.Previewmenuitem_Click);
   // 
   // Printmenuitem
   // 
   this.Printmenuitem.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
   this.Printmenuitem.Name = "Printmenuitem";
   this.Printmenuitem.Size = new System.Drawing.Size(46, 21);
   this.Printmenuitem.Text = "Print";
   this.Printmenuitem.Click += new System.EventHandler(this.Printmenuitem_Click);
   // 
   // Clrmenuitem
   // 
   this.Clrmenuitem.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
   this.Clrmenuitem.Name = "Clrmenuitem";
   this.Clrmenuitem.Size = new System.Drawing.Size(47, 21);
   this.Clrmenuitem.Text = "Clear";
   this.Clrmenuitem.Click += new System.EventHandler(this.ClearMenuItem_Click);
   // 
   // Notesmenuitem
   // 
   this.Notesmenuitem.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
   this.Notesmenuitem.Name = "Notesmenuitem";
   this.Notesmenuitem.Size = new System.Drawing.Size(52, 21);
   this.Notesmenuitem.Text = "Notes";
   this.Notesmenuitem.Click += new System.EventHandler(this.Notesmenuitem_Click);
   // 
   // Addpagemenuitem
   // 
   this.Addpagemenuitem.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
   this.Addpagemenuitem.Name = "Addpagemenuitem";
   this.Addpagemenuitem.Size = new System.Drawing.Size(70, 21);
   this.Addpagemenuitem.Text = "Add Page";
   this.Addpagemenuitem.Click += new System.EventHandler(this.AddBlockMenuItem_Click);
   // 
   // Aspecmenuitem
   // 
   this.Aspecmenuitem.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
   this.Aspecmenuitem.Name = "Aspecmenuitem";
   this.Aspecmenuitem.Size = new System.Drawing.Size(94, 21);
   this.Aspecmenuitem.Text = "Add Specialist";
   this.Aspecmenuitem.Click += new System.EventHandler(this.Sp_Title_Click);
   // 
   // Resetchg
   // 
   this.Resetchg.Name = "Resetchg";
   this.Resetchg.Size = new System.Drawing.Size(98, 21);
   this.Resetchg.Text = "Reset Changes";
   this.Resetchg.Visible = false;
   // 
   // exmenuitem
   // 
   this.exmenuitem.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
   this.exmenuitem.Name = "exmenuitem";
   this.exmenuitem.Size = new System.Drawing.Size(39, 21);
   this.exmenuitem.Text = "Exit";
   this.exmenuitem.Click += new System.EventHandler(this.ExitMenuItem_Click);
   // 
   // DeveloperToolStripMenuItem
   // 
   this.DeveloperToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ColorDialogToolStripMenuItem});
   this.DeveloperToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
   this.DeveloperToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
   this.DeveloperToolStripMenuItem.Name = "DeveloperToolStripMenuItem";
   this.DeveloperToolStripMenuItem.Size = new System.Drawing.Size(73, 21);
   this.DeveloperToolStripMenuItem.Text = "Developer";
   this.DeveloperToolStripMenuItem.Visible = false;
   // 
   // ColorDialogToolStripMenuItem
   // 
   this.ColorDialogToolStripMenuItem.Name = "ColorDialogToolStripMenuItem";
   this.ColorDialogToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
   this.ColorDialogToolStripMenuItem.Text = "Color Dialog";
   // 
   // sp
   // 
   this.sp.Controls.Add(this.secph);
   this.sp.Location = new System.Drawing.Point(784, 150);
   this.sp.Name = "sp";
   this.sp.Size = new System.Drawing.Size(365, 53);
   this.sp.TabIndex = 16;
   this.sp.TabStop = false;
   // 
   // secph
   // 
   this.secph.Enabled = false;
   this.secph.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
   this.secph.Location = new System.Drawing.Point(7, 20);
   this.secph.Name = "secph";
   this.secph.Size = new System.Drawing.Size(352, 21);
   this.secph.TabIndex = 0;
   this.secph.Visible = false;
   this.secph.TextChanged += new System.EventHandler(this.Secph_TextChanged);
   this.secph.Enter += new System.EventHandler(this.Textbox_enter);
   // 
   // GroupBox3
   // 
   this.GroupBox3.Controls.Add(this.econtact);
   this.GroupBox3.Location = new System.Drawing.Point(784, 210);
   this.GroupBox3.Name = "GroupBox3";
   this.GroupBox3.Size = new System.Drawing.Size(365, 53);
   this.GroupBox3.TabIndex = 17;
   this.GroupBox3.TabStop = false;
   this.GroupBox3.Text = "Emergency Contact";
   // 
   // econtact
   // 
   this.econtact.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
   this.econtact.Location = new System.Drawing.Point(7, 20);
   this.econtact.Name = "econtact";
   this.econtact.Size = new System.Drawing.Size(352, 21);
   this.econtact.TabIndex = 0;
   this.econtact.TextChanged += new System.EventHandler(this.Econtact_TextChanged);
   this.econtact.Enter += new System.EventHandler(this.Textbox_enter);
   this.econtact.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Edit_Keypress);
   // 
   // sptitle
   // 
   this.sptitle.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
   this.sptitle.Location = new System.Drawing.Point(791, 146);
   this.sptitle.Name = "sptitle";
   this.sptitle.Size = new System.Drawing.Size(352, 21);
   this.sptitle.TabIndex = 9;
   this.sptitle.Visible = false;
   this.sptitle.Enter += new System.EventHandler(this.Textbox_enter);
   this.sptitle.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Sptitle_KeyPress);
   this.sptitle.Leave += new System.EventHandler(this.Sptitle_Leave);
   // 
   // ContextMenuStrip1
   // 
   this.ContextMenuStrip1.Name = "ContextMenuStrip1";
   this.ContextMenuStrip1.Size = new System.Drawing.Size(61, 4);
   // 
   // Message_label
   // 
   this.Message_label.AutoSize = true;
   this.Message_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
   this.Message_label.Location = new System.Drawing.Point(520, 8);
   this.Message_label.Name = "Message_label";
   this.Message_label.Size = new System.Drawing.Size(0, 13);
   this.Message_label.TabIndex = 19;
   // 
   // ContextMenuStrip2
   // 
   this.ContextMenuStrip2.Name = "ContextMenuStrip2";
   this.ContextMenuStrip2.Size = new System.Drawing.Size(61, 4);
   // 
   // Practbox
   // 
   this.Practbox.BackColor = System.Drawing.SystemColors.Control;
   this.Practbox.Controls.Add(this.Practice);
   this.Practbox.Location = new System.Drawing.Point(18, 27);
   this.Practbox.Name = "Practbox";
   this.Practbox.Size = new System.Drawing.Size(365, 53);
   this.Practbox.TabIndex = 20;
   this.Practbox.TabStop = false;
   this.Practbox.Text = "Practice";
   // 
   // Practice
   // 
   this.Practice.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
   this.Practice.Location = new System.Drawing.Point(6, 21);
   this.Practice.Name = "Practice";
   this.Practice.Size = new System.Drawing.Size(353, 21);
   this.Practice.TabIndex = 0;
   this.Practice.TabStop = false;
   this.Practice.Enter += new System.EventHandler(this.Textbox_enter);
   this.Practice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Edit_Keypress);
   // 
   // Deptbox
   // 
   this.Deptbox.Controls.Add(this.dept_tbox);
   this.Deptbox.Controls.Add(this.Department);
   this.Deptbox.Location = new System.Drawing.Point(18, 90);
   this.Deptbox.Name = "Deptbox";
   this.Deptbox.Size = new System.Drawing.Size(365, 53);
   this.Deptbox.TabIndex = 0;
   this.Deptbox.TabStop = false;
   this.Deptbox.Text = "Department";
   // 
   // dept_tbox
   // 
   this.dept_tbox.Anchor = System.Windows.Forms.AnchorStyles.Left;
   this.dept_tbox.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
   this.dept_tbox.Location = new System.Drawing.Point(30, -62);
   this.dept_tbox.Name = "dept_tbox";
   this.dept_tbox.Size = new System.Drawing.Size(353, 21);
   this.dept_tbox.TabIndex = 1;
   this.dept_tbox.Click += new System.EventHandler(this.Textbox_enter);
   this.dept_tbox.Enter += new System.EventHandler(this.Textbox_enter);
   this.dept_tbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Dept_tbox_KeyPress);
   this.dept_tbox.MouseEnter += new System.EventHandler(this.dept_tbox_MouseEnter);
   // 
   // Department
   // 
   this.Department.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
   this.Department.FormattingEnabled = true;
   this.Department.Location = new System.Drawing.Point(6, 21);
   this.Department.Name = "Department";
   this.Department.Size = new System.Drawing.Size(353, 21);
   this.Department.Sorted = true;
   this.Department.TabIndex = 0;
   this.Department.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.Cb_DrawItem);
   this.Department.DropDown += new System.EventHandler(this.Cb_DropDown);
   this.Department.SelectedIndexChanged += new System.EventHandler(this.Department_SelectedIndexChanged);
   this.Department.TextUpdate += new System.EventHandler(this.Cb_TextChanged);
   this.Department.DropDownClosed += new System.EventHandler(this.Cb_DropDownClosed);
   this.Department.TextChanged += new System.EventHandler(this.Cb_TextChanged);
   this.Department.Click += new System.EventHandler(this.Cb_Enter);
   this.Department.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Department_KeyPress);
   this.Department.Validated += new System.EventHandler(this.Cb_Validated);
   // 
   // Form1
   // 
   this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
   this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
   this.AutoScroll = true;
   this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
   this.ClientSize = new System.Drawing.Size(1168, 325);
   this.Controls.Add(this.Deptbox);
   this.Controls.Add(this.Practbox);
   this.Controls.Add(this.Message_label);
   this.Controls.Add(this.sptitle);
   this.Controls.Add(this.GroupBox3);
   this.Controls.Add(this.sp);
   this.Controls.Add(this.GroupBox8);
   this.Controls.Add(this.ppgb);
   this.Controls.Add(this.Printbox);
   this.Controls.Add(this.Dsbox);
   this.Controls.Add(this.MenuStrip1);
   this.Controls.Add(this.GroupBox2);
   this.ForeColor = System.Drawing.SystemColors.ButtonFace;
   this.MainMenuStrip = this.MenuStrip1;
   this.Name = "Form1";
   this.Text = "Medical Profile Card";
   this.Load += new System.EventHandler(this.Form1_LoadAsync);
   this.Dsbox.ResumeLayout(false);
   this.GroupBox2.ResumeLayout(false);
   this.GroupBox2.PerformLayout();
   this.Tgp.ResumeLayout(false);
   this.Tgp.PerformLayout();
   this.Printbox.ResumeLayout(false);
   this.ppgb.ResumeLayout(false);
   this.ppgb.PerformLayout();
   this.GroupBox8.ResumeLayout(false);
   this.GroupBox8.PerformLayout();
   this.MenuStrip1.ResumeLayout(false);
   this.MenuStrip1.PerformLayout();
   this.sp.ResumeLayout(false);
   this.sp.PerformLayout();
   this.GroupBox3.ResumeLayout(false);
   this.GroupBox3.PerformLayout();
   this.Practbox.ResumeLayout(false);
   this.Practbox.PerformLayout();
   this.Deptbox.ResumeLayout(false);
   this.Deptbox.PerformLayout();
   this.ResumeLayout(false);
   this.PerformLayout();

  }

  private GroupBox Dsbox;

  //internal GroupBox Dsbox
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _Dsbox;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_Dsbox != null)
  //  {
  //  }

  //  _Dsbox = value;
  //  if (_Dsbox != null)
  //  {
  //  }
  // }
  //}

  private OpenFileDialog OpenFileDialog1;

  //internal OpenFileDialog OpenFileDialog1
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _OpenFileDialog1;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_OpenFileDialog1 != null)
  //  {
  //  }

  //  _OpenFileDialog1 = value;
  //  if (_OpenFileDialog1 != null)
  //  {
  //  }
  // }
  //}

  private GroupBox GroupBox2;

  //internal GroupBox GroupBox2
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _GroupBox2;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_GroupBox2 != null)
  //  {
  //  }

  //  _GroupBox2 = value;
  //  if (_GroupBox2 != null)
  //  {
  //  }
  // }
  //}

  private TextBox Patient;

  //internal TextBox Patient
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _Patient;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_Patient != null)
  //  {
  //   _Patient.Validated -= Patient_Validated;
  //   _Patient.Validating -= Patient_Validating;
  //   _Patient.Enter -= Patient_Enter;
  //   _Patient.Click -= Patient_Click;
  //   _Patient.KeyPress -= Patient_KeyPress;
  //  }

  //  _Patient = value;
  //  if (_Patient != null)
  //  {
  //   _Patient.Validated += Patient_Validated;
  //   _Patient.Validating += Patient_Validating;
  //   _Patient.Enter += Patient_Enter;
  //   _Patient.Click += Patient_Click;
  //   _Patient.KeyPress += Patient_KeyPress;
  //  }
  // }
  //}

  private TextBox Phone;

  //internal TextBox Phone
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _Phone;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_Phone != null)
  //  {
  //   _Phone.TextChanged -= Phone_TextChanged;
  //   _Phone.KeyPress -= Edit_Keypress;
  //   _Phone.Enter -= Textbox_enter;
  //  }

  //  _Phone = value;
  //  if (_Phone != null)
  //  {
  //   _Phone.TextChanged += Phone_TextChanged;
  //   _Phone.KeyPress += Edit_Keypress;
  //   _Phone.Enter += Textbox_enter;
  //  }
  // }
  //}

  private TextBox DOB;

  //internal TextBox DOB
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _DOB;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_DOB != null)
  //  {
  //   _DOB.TextChanged -= DOB_TextChanged;
  //   _DOB.KeyPress -= Edit_Keypress;
  //   _DOB.Enter -= Textbox_enter;
  //  }

  //  _DOB = value;
  //  if (_DOB != null)
  //  {
  //   _DOB.TextChanged += DOB_TextChanged;
  //   _DOB.KeyPress += Edit_Keypress;
  //   _DOB.Enter += Textbox_enter;
  //  }
  // }
  //}

  private GroupBox Printbox;

  //internal GroupBox Printbox
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _Printbox;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_Printbox != null)
  //  {
  //  }

  //  _Printbox = value;
  //  if (_Printbox != null)
  //  {
  //  }
  // }
  //}

  private ComboBox Printers;

  //internal ComboBox Printers
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _Printers;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_Printers != null)
  //  {
  //   _Printers.KeyPress -= Printers_KeyPress;
  //   _Printers.Click -= Cb_Enter;
  //   _Printers.Enter -= Cb_Enter;
  //   _Printers.DropDown -= Cb_DropDown;
  //   _Printers.DrawItem -= Cb_DrawItem;
  //   _Printers.TextUpdate -= Cb_TextChanged;
  //   _Printers.DropDownClosed -= Cb_DropDownClosed;
  //   _Printers.SelectedIndexChanged -= Printers_SelectedIndexChanged;
  //   _Printers.Validated -= Cb_Validated;
  //  }

  //  _Printers = value;
  //  if (_Printers != null)
  //  {
  //   _Printers.KeyPress += Printers_KeyPress;
  //   _Printers.Click += Cb_Enter;
  //   _Printers.Enter += Cb_Enter;
  //   _Printers.DropDown += Cb_DropDown;
  //   _Printers.DrawItem += Cb_DrawItem;
  //   _Printers.TextUpdate += Cb_TextChanged;
  //   _Printers.DropDownClosed += Cb_DropDownClosed;
  //   _Printers.SelectedIndexChanged += Printers_SelectedIndexChanged;
  //   _Printers.Validated += Cb_Validated;
  //  }
  // }
  //}

  private RichTextBox address;

  //internal RichTextBox address
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _address;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_address != null)
  //  {
  //   _address.TextChanged -= Address_TextChanged;
  //   _address.KeyPress -= Edit_Keypress;
  //   _address.Enter -= Rtbbox_enter;
  //  }

  //  _address = value;
  //  if (_address != null)
  //  {
  //   _address.TextChanged += Address_TextChanged;
  //   _address.KeyPress += Edit_Keypress;
  //   _address.Enter += Rtbbox_enter;
  //  }
  // }
  //}

  private OpenFileDialog OpenFileDialog2;

  //internal OpenFileDialog OpenFileDialog2
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _OpenFileDialog2;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_OpenFileDialog2 != null)
  //  {
  //  }

  //  _OpenFileDialog2 = value;
  //  if (_OpenFileDialog2 != null)
  //  {
  //  }
  // }
  //}

  private FolderBrowserDialog FolderBrowserDialog1;

  //internal FolderBrowserDialog FolderBrowserDialog1
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _FolderBrowserDialog1;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_FolderBrowserDialog1 != null)
  //  {
  //  }

  //  _FolderBrowserDialog1 = value;
  //  if (_FolderBrowserDialog1 != null)
  //  {
  //  }
  // }
  //}

  private ComboBox dsaves;

  //internal ComboBox xdsaves
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return dsaves;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (dsaves != null)
  //  {
  //   dsaves.SelectedIndexChanged -= Dsaves_SelectedIndexChanged;
  //   dsaves.Enter -= Cb_Enter;
  //   dsaves.Click -= Cb_Enter;
  //   dsaves.DropDown -= Cb_DropDown;
  //   dsaves.DrawItem -= Cb_DrawItem;
  //   dsaves.TextUpdate -= Cb_TextChanged;
  //   dsaves.DropDownClosed -= Cb_DropDownClosed;
  //   dsaves.Validated -= Cb_Validated;
  //  }

  //  dsaves = value;
  //  if (dsaves != null)
  //  {
  //   dsaves.SelectedIndexChanged += Dsaves_SelectedIndexChanged;
  //   dsaves.Enter += Cb_Enter;
  //   dsaves.Click += Cb_Enter;
  //   dsaves.DropDown += Cb_DropDown;
  //   dsaves.DrawItem += Cb_DrawItem;
  //   dsaves.TextUpdate += Cb_TextChanged;
  //   dsaves.DropDownClosed += Cb_DropDownClosed;
  //   dsaves.Validated += Cb_Validated;
  //  }
  // }
  //}

  private GroupBox ppgb;

  //internal GroupBox ppgb
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _ppgb;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_ppgb != null)
  //  {
  //  }

  //  _ppgb = value;
  //  if (_ppgb != null)
  //  {
  //  }
  // }
  //}

  private TextBox priph;

  //internal TextBox priph
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _priph;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_priph != null)
  //  {
  //   _priph.TextChanged -= Priph_TextChanged;
  //   _priph.KeyPress -= Edit_Keypress;
  //  }

  //  _priph = value;
  //  if (_priph != null)
  //  {
  //   _priph.TextChanged += Priph_TextChanged;
  //   _priph.KeyPress += Edit_Keypress;
  //  }
  // }
  //}

  private GroupBox GroupBox8;

  //internal GroupBox GroupBox8
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _GroupBox8;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_GroupBox8 != null)
  //  {
  //  }

  //  _GroupBox8 = value;
  //  if (_GroupBox8 != null)
  //  {
  //  }
  // }
  //}

  private TextBox ins;

  //internal TextBox ins
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _ins;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_ins != null)
  //  {
  //   _ins.TextChanged -= Ins_TextChanged;
  //   _ins.KeyPress -= Edit_Keypress;
  //   _ins.Enter -= Textbox_enter;
  //  }

  //  _ins = value;
  //  if (_ins != null)
  //  {
  //   _ins.TextChanged += Ins_TextChanged;
  //   _ins.KeyPress += Edit_Keypress;
  //   _ins.Enter += Textbox_enter;
  //  }
  // }
  //}

  private MenuStrip MenuStrip1;

  //internal MenuStrip MenuStrip1
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _MenuStrip1;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_MenuStrip1 != null)
  //  {
  //  }

  //  _MenuStrip1 = value;
  //  if (_MenuStrip1 != null)
  //  {
  //  }
  // }
  //}

  private GroupBox sp;

  //internal GroupBox sp
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _sp;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_sp != null)
  //  {
  //  }

  //  _sp = value;
  //  if (_sp != null)
  //  {
  //  }
  // }
  //}

  private TextBox secph;

  //internal TextBox secph
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _secph;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_secph != null)
  //  {
  //   _secph.TextChanged -= Secph_TextChanged;
  //   _secph.Enter -= Textbox_enter;
  //  }

  //  _secph = value;
  //  if (_secph != null)
  //  {
  //   _secph.TextChanged += Secph_TextChanged;
  //   _secph.Enter += Textbox_enter;
  //  }
  // }
  //}

  private GroupBox GroupBox3;

  //internal GroupBox GroupBox3
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _GroupBox3;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_GroupBox3 != null)
  //  {
  //  }

  //  _GroupBox3 = value;
  //  if (_GroupBox3 != null)
  //  {
  //  }
  // }
  //}

  private TextBox econtact;

  //internal TextBox econtact
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _econtact;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_econtact != null)
  //  {
  //   _econtact.TextChanged -= Econtact_TextChanged;
  //   _econtact.KeyPress -= Edit_Keypress;
  //   _econtact.Enter -= Textbox_enter;
  //  }

  //  _econtact = value;
  //  if (_econtact != null)
  //  {
  //   _econtact.TextChanged += Econtact_TextChanged;
  //   _econtact.KeyPress += Edit_Keypress;
  //   _econtact.Enter += Textbox_enter;
  //  }
  // }
  //}

  private Label Label4;

  //internal Label Label4
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _Label4;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_Label4 != null)
  //  {
  //  }

  //  _Label4 = value;
  //  if (_Label4 != null)
  //  {
  //  }
  // }
  //}

  private Label Label3;

  //internal Label Label3
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _Label3;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_Label3 != null)
  //  {
  //  }

  //  _Label3 = value;
  //  if (_Label3 != null)
  //  {
  //  }
  // }
  //}

  private Label Label2;

  //internal Label Label2
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _Label2;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_Label2 != null)
  //  {
  //  }

  //  _Label2 = value;
  //  if (_Label2 != null)
  //  {
  //  }
  // }
  //}

  private Label patient_label;

  //internal Label patient_label
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _patient_label;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_patient_label != null)
  //  {
  //  }

  //  _patient_label = value;
  //  if (_patient_label != null)
  //  {
  //  }
  // }
  //}

  private TextBox sptitle;

  //internal TextBox sptitle
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return sptitle;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (sptitle != null)
  //  {
  //   sptitle.Leave -= Sptitle_Leave;
  //   sptitle.KeyPress -= Sptitle_KeyPress;
  //   sptitle.Enter -= Textbox_enter;
  //  }

  //  sptitle = value;
  //  if (sptitle != null)
  //  {
  //   sptitle.Leave += Sptitle_Leave;
  //   sptitle.KeyPress += Sptitle_KeyPress;
  //   sptitle.Enter += Textbox_enter;
  //  }
  // }
  //}

  private ContextMenuStrip ContextMenuStrip1;

  //internal ContextMenuStrip ContextMenuStrip1
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _ContextMenuStrip1;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_ContextMenuStrip1 != null)
  //  {
  //  }

  //  _ContextMenuStrip1 = value;
  //  if (_ContextMenuStrip1 != null)
  //  {
  //  }
  // }
  //}

  private Label Message_label;

  //internal Label Message_label
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _Message_label;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_Message_label != null)
  //  {
  //  }

  //  _Message_label = value;
  //  if (_Message_label != null)
  //  {
  //  }
  // }
  //}

  private ContextMenuStrip ContextMenuStrip2;

  //internal ContextMenuStrip ContextMenuStrip2
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _ContextMenuStrip2;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_ContextMenuStrip2 != null)
  //  {
  //  }

  //  _ContextMenuStrip2 = value;
  //  if (_ContextMenuStrip2 != null)
  //  {
  //  }
  // }
  //}

  private ToolStripMenuItem DeveloperToolStripMenuItem;

  //internal ToolStripMenuItem DeveloperToolStripMenuItem
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _DeveloperToolStripMenuItem;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_DeveloperToolStripMenuItem != null)
  //  {
  //  }

  //  _DeveloperToolStripMenuItem = value;
  //  if (_DeveloperToolStripMenuItem != null)
  //  {
  //  }
  // }
  //}

  private ToolStripMenuItem ColorDialogToolStripMenuItem;

  //internal ToolStripMenuItem ColorDialogToolStripMenuItem
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _ColorDialogToolStripMenuItem;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_ColorDialogToolStripMenuItem != null)
  //  {
  //  }

  //  _ColorDialogToolStripMenuItem = value;
  //  if (_ColorDialogToolStripMenuItem != null)
  //  {
  //  }
  // }
  //}

  private GroupBox Practbox;

  //internal GroupBox Practbox
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return Practbox;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (Practbox != null)
  //  {
  //  }

  //  Practbox = value;
  //  if (Practbox != null)
  //  {
  //  }
  // }
  //}

  private TextBox Practice;

  //internal TextBox Practice
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return Practice;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (Practice != null)
  //  {
  //   Practice.KeyPress -= Edit_Keypress;
  //   Practice.Enter -= Textbox_enter;
  //  }

  //  Practice = value;
  //  if (Practice != null)
  //  {
  //   Practice.KeyPress += Edit_Keypress;
  //   Practice.Enter += Textbox_enter;
  //  }
  // }
  //}

  private GroupBox Deptbox;

  //internal GroupBox Deptbox
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return Deptbox;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (Deptbox != null)
  //  {
  //  }

  //  Deptbox = value;
  //  if (Deptbox != null)
  //  {
  //  }
  // }
  //}

  private ComboBox Department;

  //internal ComboBox Department
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _Department;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_Department != null)
  //  {
  //   _Department.KeyPress -= Department_KeyPress;
  //   _Department.Click -= Cb_Enter;
  //   _Department.DropDown -= Cb_DropDown;
  //   _Department.DrawItem -= Cb_DrawItem;
  //   _Department.TextUpdate -= Cb_TextChanged;
  //   _Department.TextChanged -= Cb_TextChanged;
  //   _Department.DropDownClosed -= Cb_DropDownClosed;
  //   _Department.SelectedIndexChanged -= Department_SelectedIndexChanged;
  //   _Department.Validated -= Cb_Validated;
  //  }

  //  _Department = value;
  //  if (_Department != null)
  //  {
  //   _Department.KeyPress += Department_KeyPress;
  //   _Department.Click += Cb_Enter;
  //   _Department.DropDown += Cb_DropDown;
  //   _Department.DrawItem += Cb_DrawItem;
  //   _Department.TextUpdate += Cb_TextChanged;
  //   _Department.TextChanged += Cb_TextChanged;
  //   _Department.DropDownClosed += Cb_DropDownClosed;
  //   _Department.SelectedIndexChanged += Department_SelectedIndexChanged;
  //   _Department.Validated += Cb_Validated;
  //  }
  // }
  //}

  private Label pid_label;

  //internal Label pid_label
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _pid_label;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_pid_label != null)
  //  {
  //  }

  //  _pid_label = value;
  //  if (_pid_label != null)
  //  {
  //  }
  // }
  //}

  private TextBox dept_tbox;

  //internal TextBox dept_tbox
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _dept_tbox;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_dept_tbox != null)
  //  {
  //   _dept_tbox.KeyPress -= Dept_tbox_KeyPress;
  //   _dept_tbox.Enter -= Textbox_enter;
  //   _dept_tbox.Click -= Textbox_enter;
  //   _dept_tbox.MouseEnter -= dept_tbox_MouseEnter;
  //  }

  //  _dept_tbox = value;
  //  if (_dept_tbox != null)
  //  {
  //   _dept_tbox.KeyPress += Dept_tbox_KeyPress;
  //   _dept_tbox.Enter += Textbox_enter;
  //   _dept_tbox.Click += Textbox_enter;
  //   _dept_tbox.MouseEnter += dept_tbox_MouseEnter;
  //  }
  // }
  //}

  private TextBox Patientid;

  //internal TextBox Patientid
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _Patientid;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_Patientid != null)
  //  {
  //   _Patientid.Click -= Patientid_Click;
  //   _Patientid.Enter -= Patientid_Enter;
  //   _Patientid.KeyPress -= Patientid_KeyPress;
  //   _Patientid.Validating -= Patientid_Validating;
  //   _Patientid.Validated -= Patientid_Validated;
  //  }

  //  _Patientid = value;
  //  if (_Patientid != null)
  //  {
  //   _Patientid.Click += Patientid_Click;
  //   _Patientid.Enter += Patientid_Enter;
  //   _Patientid.KeyPress += Patientid_KeyPress;
  //   _Patientid.Validating += Patientid_Validating;
  //   _Patientid.Validated += Patientid_Validated;
  //  }
  // }
  //}

  private ComboBox prv_combo;

  //internal ComboBox prv_combo
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _prv_combo;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_prv_combo != null)
  //  {
  //   _prv_combo.KeyPress -= Prv_combo_KeyPress;
  //   _prv_combo.TextChanged -= prv_combo_TextChanged;
  //   _prv_combo.Enter -= Cb_Enter;
  //   _prv_combo.Click -= Cb_Enter;
  //   _prv_combo.DrawItem -= Cb_DrawItem;
  //   _prv_combo.DropDownClosed -= Cb_DropDownClosed;
  //   _prv_combo.SelectedIndexChanged -= prv_combo_SelectedIndexChanged;
  //   _prv_combo.Validated -= Cb_Validated;
  //  }

  //  _prv_combo = value;
  //  if (_prv_combo != null)
  //  {
  //   _prv_combo.KeyPress += Prv_combo_KeyPress;
  //   _prv_combo.TextChanged += prv_combo_TextChanged;
  //   _prv_combo.Enter += Cb_Enter;
  //   _prv_combo.Click += Cb_Enter;
  //   _prv_combo.DrawItem += Cb_DrawItem;
  //   _prv_combo.DropDownClosed += Cb_DropDownClosed;
  //   _prv_combo.SelectedIndexChanged += prv_combo_SelectedIndexChanged;
  //   _prv_combo.Validated += Cb_Validated;
  //  }
  // }
  //}

  private ToolStripMenuItem Editmenuitem;

  //internal ToolStripMenuItem Editmenuitem
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _Editmenuitem;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_Editmenuitem != null)
  //  {
  //   _Editmenuitem.Click -= Editmenuitem_Click;
  //  }

  //  _Editmenuitem = value;
  //  if (_Editmenuitem != null)
  //  {
  //   _Editmenuitem.Click += Editmenuitem_Click;
  //  }
  // }
  //}

  private ToolStripMenuItem Previewmenuitem;

  //internal ToolStripMenuItem Previewmenuitem
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _Previewmenuitem;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_Previewmenuitem != null)
  //  {
  //   _Previewmenuitem.Click -= Previewmenuitem_Click;
  //  }

  //  _Previewmenuitem = value;
  //  if (_Previewmenuitem != null)
  //  {
  //   _Previewmenuitem.Click += Previewmenuitem_Click;
  //  }
  // }
  //}

  private ToolStripMenuItem Printmenuitem;

  //internal ToolStripMenuItem Printmenuitem
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _Printmenuitem;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_Printmenuitem != null)
  //  {
  //   _Printmenuitem.Click -= Printmenuitem_Click;
  //  }

  //  _Printmenuitem = value;
  //  if (_Printmenuitem != null)
  //  {
  //   _Printmenuitem.Click += Printmenuitem_Click;
  //  }
  // }
  //}

  private ToolStripMenuItem Clrmenuitem;

  //internal ToolStripMenuItem Clrmenuitem
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _Clrmenuitem;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_Clrmenuitem != null)
  //  {
  //   _Clrmenuitem.Click -= ClearMenuItem_Click;
  //  }

  //  _Clrmenuitem = value;
  //  if (_Clrmenuitem != null)
  //  {
  //   _Clrmenuitem.Click += ClearMenuItem_Click;
  //  }
  // }
  //}

  private ToolStripMenuItem Notesmenuitem;

  //internal ToolStripMenuItem Notesmenuitem
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _Notesmenuitem;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_Notesmenuitem != null)
  //  {
  //   _Notesmenuitem.Click -= Notesmenuitem_Click;
  //  }

  //  _Notesmenuitem = value;
  //  if (_Notesmenuitem != null)
  //  {
  //   _Notesmenuitem.Click += Notesmenuitem_Click;
  //  }
  // }
  //}

  private ToolStripMenuItem Addpagemenuitem;

  //internal ToolStripMenuItem Addpagemenuitem
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _Addpagemenuitem;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_Addpagemenuitem != null)
  //  {
  //   _Addpagemenuitem.Click -= AddBlockMenuItem_Click;
  //  }

  //  _Addpagemenuitem = value;
  //  if (_Addpagemenuitem != null)
  //  {
  //   _Addpagemenuitem.Click += AddBlockMenuItem_Click;
  //  }
  // }
  //}

  private ToolStripMenuItem Aspecmenuitem;

  //internal ToolStripMenuItem Aspecmenuitem
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _Aspecmenuitem;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_Aspecmenuitem != null)
  //  {
  //   _Aspecmenuitem.Click -= Sp_Title_Click;
  //  }

  //  _Aspecmenuitem = value;
  //  if (_Aspecmenuitem != null)
  //  {
  //   _Aspecmenuitem.Click += Sp_Title_Click;
  //  }
  // }
  //}

  private ToolStripMenuItem exmenuitem;

  //internal ToolStripMenuItem exmenuitem
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _exmenuitem;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_exmenuitem != null)
  //  {
  //   _exmenuitem.Click -= ExitMenuItem_Click;
  //  }

  //  _exmenuitem = value;
  //  if (_exmenuitem != null)
  //  {
  //   _exmenuitem.Click += ExitMenuItem_Click;
  //  }
  // }
  //}

  private ToolStripMenuItem Resetchg;

  //internal ToolStripMenuItem Resetchg
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _Resetchg;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_Resetchg != null)
  //  {
  //  }

  //  _Resetchg = value;
  //  if (_Resetchg != null)
  //  {
  //  }
  // }
  //}

  private TextBox dest;

  //internal TextBox dest
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _dest;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_dest != null)
  //  {
  //  }

  //  _dest = value;
  //  if (_dest != null)
  //  {
  //  }
  // }
  //}

  private ToolStripMenuItem Savemi;

  //internal ToolStripMenuItem Savemi
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _Savemi;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_Savemi != null)
  //  {
  //   _Savemi.Click -= Savemi_Click;
  //  }

  //  _Savemi = value;
  //  if (_Savemi != null)
  //  {
  //   _Savemi.Click += Savemi_Click;
  //  }
  // }
  //}

  private ToolStripMenuItem Deletemi;

  //internal ToolStripMenuItem Deletemi
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _Deletemi;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_Deletemi != null)
  //  {
  //   _Deletemi.Click -= Deletemi_Click;
  //  }

  //  _Deletemi = value;
  //  if (_Deletemi != null)
  //  {
  //   _Deletemi.Click += Deletemi_Click;
  //  }
  // }
  //}

  private GroupBox Tgp;

  //internal GroupBox Tgp
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _Tgp;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_Tgp != null)
  //  {
  //  }

  //  _Tgp = value;
  //  if (_Tgp != null)
  //  {
  //  }
  // }
  //}

  private TextBox Ettb;

  //internal TextBox Ettb
  //{
  // [MethodImpl(MethodImplOptions.Synchronized)]
  // get
  // {
  //  return _Ettb;
  // }

  // [MethodImpl(MethodImplOptions.Synchronized)]
  // set
  // {
  //  if (_Ettb != null)
  //  {
  //  }

  //  _Ettb = value;
  //  if (_Ettb != null)
  //  {
  //  }
  // }
  //}
 }
}