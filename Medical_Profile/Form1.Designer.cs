using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Medical_Profile
{
 public partial class Form1 : Form
 {
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
  
  private System.ComponentModel.IContainer components;

  
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
   this.exmenuitem = new System.Windows.Forms.ToolStripMenuItem();
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
   this.Dsbox.Location = new System.Drawing.Point(24, 258);
   this.Dsbox.Margin = new System.Windows.Forms.Padding(4);
   this.Dsbox.Name = "Dsbox";
   this.Dsbox.Padding = new System.Windows.Forms.Padding(4);
   this.Dsbox.Size = new System.Drawing.Size(487, 65);
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
   this.dsaves.Location = new System.Drawing.Point(8, 26);
   this.dsaves.Margin = new System.Windows.Forms.Padding(4);
   this.dsaves.Name = "dsaves";
   this.dsaves.Size = new System.Drawing.Size(469, 25);
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
   this.GroupBox2.Location = new System.Drawing.Point(535, 33);
   this.GroupBox2.Margin = new System.Windows.Forms.Padding(4);
   this.GroupBox2.Name = "GroupBox2";
   this.GroupBox2.Padding = new System.Windows.Forms.Padding(4);
   this.GroupBox2.Size = new System.Drawing.Size(487, 290);
   this.GroupBox2.TabIndex = 1;
   this.GroupBox2.TabStop = false;
   // 
   // Tgp
   // 
   this.Tgp.Controls.Add(this.Ettb);
   this.Tgp.Location = new System.Drawing.Point(8, 224);
   this.Tgp.Margin = new System.Windows.Forms.Padding(4);
   this.Tgp.Name = "Tgp";
   this.Tgp.Padding = new System.Windows.Forms.Padding(4);
   this.Tgp.Size = new System.Drawing.Size(264, 53);
   this.Tgp.TabIndex = 13;
   this.Tgp.TabStop = false;
   this.Tgp.Text = "Elapsed ms";
   this.Tgp.Visible = false;
   // 
   // Ettb
   // 
   this.Ettb.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
   this.Ettb.Location = new System.Drawing.Point(8, 23);
   this.Ettb.Margin = new System.Windows.Forms.Padding(4);
   this.Ettb.Name = "Ettb";
   this.Ettb.Size = new System.Drawing.Size(247, 24);
   this.Ettb.TabIndex = 0;
   this.Ettb.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
   // 
   // dest
   // 
   this.dest.Location = new System.Drawing.Point(297, 225);
   this.dest.Margin = new System.Windows.Forms.Padding(4);
   this.dest.Name = "dest";
   this.dest.Size = new System.Drawing.Size(132, 22);
   this.dest.TabIndex = 12;
   this.dest.Visible = false;
   // 
   // Label4
   // 
   this.Label4.AutoSize = true;
   this.Label4.Location = new System.Drawing.Point(293, 127);
   this.Label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
   this.Label4.Name = "Label4";
   this.Label4.Size = new System.Drawing.Size(49, 17);
   this.Label4.TabIndex = 8;
   this.Label4.Text = "Phone";
   // 
   // Label2
   // 
   this.Label2.AutoSize = true;
   this.Label2.Location = new System.Drawing.Point(9, 64);
   this.Label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
   this.Label2.Name = "Label2";
   this.Label2.Size = new System.Drawing.Size(60, 17);
   this.Label2.TabIndex = 6;
   this.Label2.Text = "Address";
   // 
   // Label3
   // 
   this.Label3.AutoSize = true;
   this.Label3.Location = new System.Drawing.Point(293, 64);
   this.Label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
   this.Label3.Name = "Label3";
   this.Label3.Size = new System.Drawing.Size(38, 17);
   this.Label3.TabIndex = 7;
   this.Label3.Text = "DOB";
   // 
   // pid_label
   // 
   this.pid_label.AutoSize = true;
   this.pid_label.Location = new System.Drawing.Point(293, 1);
   this.pid_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
   this.pid_label.Name = "pid_label";
   this.pid_label.Size = new System.Drawing.Size(67, 17);
   this.pid_label.TabIndex = 10;
   this.pid_label.Text = "Patient id";
   // 
   // patient_label
   // 
   this.patient_label.AutoSize = true;
   this.patient_label.Location = new System.Drawing.Point(8, 1);
   this.patient_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
   this.patient_label.Name = "patient_label";
   this.patient_label.Size = new System.Drawing.Size(52, 17);
   this.patient_label.TabIndex = 5;
   this.patient_label.Text = "Patient";
   // 
   // Patientid
   // 
   this.Patientid.AcceptsTab = true;
   this.Patientid.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
   this.Patientid.Location = new System.Drawing.Point(293, 28);
   this.Patientid.Margin = new System.Windows.Forms.Padding(4);
   this.Patientid.Multiline = true;
   this.Patientid.Name = "Patientid";
   this.Patientid.Size = new System.Drawing.Size(165, 24);
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
   this.address.Location = new System.Drawing.Point(8, 82);
   this.address.Margin = new System.Windows.Forms.Padding(4);
   this.address.Name = "address";
   this.address.Size = new System.Drawing.Size(263, 133);
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
   this.Phone.Location = new System.Drawing.Point(293, 151);
   this.Phone.Margin = new System.Windows.Forms.Padding(4);
   this.Phone.Name = "Phone";
   this.Phone.Size = new System.Drawing.Size(166, 24);
   this.Phone.TabIndex = 3;
   this.Phone.TextChanged += new System.EventHandler(this.Phone_TextChanged);
   this.Phone.Enter += new System.EventHandler(this.Textbox_enter);
   this.Phone.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Edit_Keypress);
   // 
   // DOB
   // 
   this.DOB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
   this.DOB.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
   this.DOB.Location = new System.Drawing.Point(293, 91);
   this.DOB.Margin = new System.Windows.Forms.Padding(4);
   this.DOB.Name = "DOB";
   this.DOB.Size = new System.Drawing.Size(166, 30);
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
   this.Patient.Location = new System.Drawing.Point(8, 28);
   this.Patient.Margin = new System.Windows.Forms.Padding(4);
   this.Patient.Multiline = true;
   this.Patient.Name = "Patient";
   this.Patient.Size = new System.Drawing.Size(263, 24);
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
   this.Printbox.Location = new System.Drawing.Point(24, 185);
   this.Printbox.Margin = new System.Windows.Forms.Padding(4);
   this.Printbox.Name = "Printbox";
   this.Printbox.Padding = new System.Windows.Forms.Padding(4);
   this.Printbox.Size = new System.Drawing.Size(487, 65);
   this.Printbox.TabIndex = 10;
   this.Printbox.TabStop = false;
   this.Printbox.Text = "Printer";
   // 
   // Printers
   // 
   this.Printers.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
   this.Printers.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
   this.Printers.FormattingEnabled = true;
   this.Printers.Location = new System.Drawing.Point(8, 23);
   this.Printers.Margin = new System.Windows.Forms.Padding(4);
   this.Printers.Name = "Printers";
   this.Printers.Size = new System.Drawing.Size(469, 25);
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
   this.ppgb.Location = new System.Drawing.Point(1045, 33);
   this.ppgb.Margin = new System.Windows.Forms.Padding(4);
   this.ppgb.Name = "ppgb";
   this.ppgb.Padding = new System.Windows.Forms.Padding(4);
   this.ppgb.Size = new System.Drawing.Size(487, 65);
   this.ppgb.TabIndex = 13;
   this.ppgb.TabStop = false;
   this.ppgb.Text = "Primary Physician";
   // 
   // prv_combo
   // 
   this.prv_combo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
   this.prv_combo.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
   this.prv_combo.FormattingEnabled = true;
   this.prv_combo.Location = new System.Drawing.Point(9, 25);
   this.prv_combo.Margin = new System.Windows.Forms.Padding(4);
   this.prv_combo.Name = "prv_combo";
   this.prv_combo.Size = new System.Drawing.Size(469, 25);
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
   this.priph.Location = new System.Drawing.Point(9, 25);
   this.priph.Margin = new System.Windows.Forms.Padding(4);
   this.priph.Name = "priph";
   this.priph.Size = new System.Drawing.Size(469, 24);
   this.priph.TabIndex = 0;
   this.priph.TextChanged += new System.EventHandler(this.Priph_TextChanged);
   this.priph.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Edit_Keypress);
   // 
   // GroupBox8
   // 
   this.GroupBox8.Controls.Add(this.ins);
   this.GroupBox8.Location = new System.Drawing.Point(1045, 111);
   this.GroupBox8.Margin = new System.Windows.Forms.Padding(4);
   this.GroupBox8.Name = "GroupBox8";
   this.GroupBox8.Padding = new System.Windows.Forms.Padding(4);
   this.GroupBox8.Size = new System.Drawing.Size(487, 65);
   this.GroupBox8.TabIndex = 14;
   this.GroupBox8.TabStop = false;
   this.GroupBox8.Text = "Insurance";
   // 
   // ins
   // 
   this.ins.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
   this.ins.Location = new System.Drawing.Point(9, 25);
   this.ins.Margin = new System.Windows.Forms.Padding(4);
   this.ins.Name = "ins";
   this.ins.Size = new System.Drawing.Size(468, 24);
   this.ins.TabIndex = 0;
   this.ins.TextChanged += new System.EventHandler(this.Ins_TextChanged);
   this.ins.Enter += new System.EventHandler(this.Textbox_enter);
   this.ins.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Edit_Keypress);
   // 
   // MenuStrip1
   // 
   this.MenuStrip1.Dock = System.Windows.Forms.DockStyle.None;
   this.MenuStrip1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
   this.MenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
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
            this.exmenuitem});
   this.MenuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
   this.MenuStrip1.Location = new System.Drawing.Point(12, -1);
   this.MenuStrip1.Name = "MenuStrip1";
   this.MenuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
   this.MenuStrip1.Size = new System.Drawing.Size(714, 31);
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
   this.Editmenuitem.Size = new System.Drawing.Size(50, 27);
   this.Editmenuitem.Text = "File";
   this.Editmenuitem.Click += new System.EventHandler(this.Editmenuitem_Click);
   // 
   // Savemi
   // 
   this.Savemi.Name = "Savemi";
   this.Savemi.Size = new System.Drawing.Size(57, 27);
   this.Savemi.Text = "Save";
   this.Savemi.Click += new System.EventHandler(this.Savemi_Click);
   // 
   // Deletemi
   // 
   this.Deletemi.Name = "Deletemi";
   this.Deletemi.Size = new System.Drawing.Size(72, 27);
   this.Deletemi.Text = "Delete";
   this.Deletemi.Click += new System.EventHandler(this.Deletemi_Click);
   // 
   // Previewmenuitem
   // 
   this.Previewmenuitem.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
   this.Previewmenuitem.Name = "Previewmenuitem";
   this.Previewmenuitem.Size = new System.Drawing.Size(82, 27);
   this.Previewmenuitem.Text = "Preview";
   this.Previewmenuitem.Click += new System.EventHandler(this.Previewmenuitem_Click);
   // 
   // Printmenuitem
   // 
   this.Printmenuitem.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
   this.Printmenuitem.Name = "Printmenuitem";
   this.Printmenuitem.Size = new System.Drawing.Size(58, 27);
   this.Printmenuitem.Text = "Print";
   this.Printmenuitem.Click += new System.EventHandler(this.Printmenuitem_Click);
   // 
   // Clrmenuitem
   // 
   this.Clrmenuitem.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
   this.Clrmenuitem.Name = "Clrmenuitem";
   this.Clrmenuitem.Size = new System.Drawing.Size(60, 27);
   this.Clrmenuitem.Text = "Clear";
   this.Clrmenuitem.Click += new System.EventHandler(this.ClearMenuItem_Click);
   // 
   // Notesmenuitem
   // 
   this.Notesmenuitem.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
   this.Notesmenuitem.Name = "Notesmenuitem";
   this.Notesmenuitem.Size = new System.Drawing.Size(66, 27);
   this.Notesmenuitem.Text = "Notes";
   this.Notesmenuitem.Click += new System.EventHandler(this.Notesmenuitem_Click);
   // 
   // Addpagemenuitem
   // 
   this.Addpagemenuitem.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
   this.Addpagemenuitem.Name = "Addpagemenuitem";
   this.Addpagemenuitem.Size = new System.Drawing.Size(90, 27);
   this.Addpagemenuitem.Text = "Add Page";
   this.Addpagemenuitem.Click += new System.EventHandler(this.AddBlockMenuItem_Click);
   // 
   // Aspecmenuitem
   // 
   this.Aspecmenuitem.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
   this.Aspecmenuitem.Name = "Aspecmenuitem";
   this.Aspecmenuitem.Size = new System.Drawing.Size(122, 27);
   this.Aspecmenuitem.Text = "Add Specialist";
   this.Aspecmenuitem.Click += new System.EventHandler(this.Sp_Title_Click);
   // 
   // exmenuitem
   // 
   this.exmenuitem.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
   this.exmenuitem.Name = "exmenuitem";
   this.exmenuitem.Size = new System.Drawing.Size(50, 27);
   this.exmenuitem.Text = "Exit";
   this.exmenuitem.Click += new System.EventHandler(this.ExitMenuItem_Click);
   // 
   // sp
   // 
   this.sp.Controls.Add(this.secph);
   this.sp.Location = new System.Drawing.Point(1045, 185);
   this.sp.Margin = new System.Windows.Forms.Padding(4);
   this.sp.Name = "sp";
   this.sp.Padding = new System.Windows.Forms.Padding(4);
   this.sp.Size = new System.Drawing.Size(487, 65);
   this.sp.TabIndex = 16;
   this.sp.TabStop = false;
   // 
   // secph
   // 
   this.secph.Enabled = false;
   this.secph.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
   this.secph.Location = new System.Drawing.Point(9, 25);
   this.secph.Margin = new System.Windows.Forms.Padding(4);
   this.secph.Name = "secph";
   this.secph.Size = new System.Drawing.Size(468, 24);
   this.secph.TabIndex = 0;
   this.secph.Visible = false;
   this.secph.TextChanged += new System.EventHandler(this.Secph_TextChanged);
   this.secph.Enter += new System.EventHandler(this.Textbox_enter);
   // 
   // GroupBox3
   // 
   this.GroupBox3.Controls.Add(this.econtact);
   this.GroupBox3.Location = new System.Drawing.Point(1045, 258);
   this.GroupBox3.Margin = new System.Windows.Forms.Padding(4);
   this.GroupBox3.Name = "GroupBox3";
   this.GroupBox3.Padding = new System.Windows.Forms.Padding(4);
   this.GroupBox3.Size = new System.Drawing.Size(487, 65);
   this.GroupBox3.TabIndex = 17;
   this.GroupBox3.TabStop = false;
   this.GroupBox3.Text = "Emergency Contact";
   // 
   // econtact
   // 
   this.econtact.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
   this.econtact.Location = new System.Drawing.Point(9, 25);
   this.econtact.Margin = new System.Windows.Forms.Padding(4);
   this.econtact.Name = "econtact";
   this.econtact.Size = new System.Drawing.Size(468, 24);
   this.econtact.TabIndex = 0;
   this.econtact.TextChanged += new System.EventHandler(this.Econtact_TextChanged);
   this.econtact.Enter += new System.EventHandler(this.Textbox_enter);
   this.econtact.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Edit_Keypress);
   // 
   // sptitle
   // 
   this.sptitle.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
   this.sptitle.Location = new System.Drawing.Point(1055, 180);
   this.sptitle.Margin = new System.Windows.Forms.Padding(4);
   this.sptitle.Name = "sptitle";
   this.sptitle.Size = new System.Drawing.Size(468, 24);
   this.sptitle.TabIndex = 9;
   this.sptitle.Visible = false;
   this.sptitle.Enter += new System.EventHandler(this.Textbox_enter);
   this.sptitle.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Sptitle_KeyPress);
   this.sptitle.Leave += new System.EventHandler(this.Sptitle_Leave);
   // 
   // ContextMenuStrip1
   // 
   this.ContextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
   this.ContextMenuStrip1.Name = "ContextMenuStrip1";
   this.ContextMenuStrip1.Size = new System.Drawing.Size(61, 4);
   // 
   // Message_label
   // 
   this.Message_label.AutoSize = true;
   this.Message_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
   this.Message_label.Location = new System.Drawing.Point(693, 10);
   this.Message_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
   this.Message_label.Name = "Message_label";
   this.Message_label.Size = new System.Drawing.Size(0, 17);
   this.Message_label.TabIndex = 19;
   // 
   // ContextMenuStrip2
   // 
   this.ContextMenuStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
   this.ContextMenuStrip2.Name = "ContextMenuStrip2";
   this.ContextMenuStrip2.Size = new System.Drawing.Size(61, 4);
   // 
   // Practbox
   // 
   this.Practbox.BackColor = System.Drawing.SystemColors.Control;
   this.Practbox.Controls.Add(this.Practice);
   this.Practbox.Location = new System.Drawing.Point(24, 33);
   this.Practbox.Margin = new System.Windows.Forms.Padding(4);
   this.Practbox.Name = "Practbox";
   this.Practbox.Padding = new System.Windows.Forms.Padding(4);
   this.Practbox.Size = new System.Drawing.Size(487, 65);
   this.Practbox.TabIndex = 20;
   this.Practbox.TabStop = false;
   this.Practbox.Text = "Practice";
   // 
   // Practice
   // 
   this.Practice.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
   this.Practice.Location = new System.Drawing.Point(8, 26);
   this.Practice.Margin = new System.Windows.Forms.Padding(4);
   this.Practice.Name = "Practice";
   this.Practice.Size = new System.Drawing.Size(469, 24);
   this.Practice.TabIndex = 0;
   this.Practice.TabStop = false;
   this.Practice.Enter += new System.EventHandler(this.Textbox_enter);
   this.Practice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Edit_Keypress);
   // 
   // Deptbox
   // 
   this.Deptbox.Controls.Add(this.dept_tbox);
   this.Deptbox.Controls.Add(this.Department);
   this.Deptbox.Location = new System.Drawing.Point(24, 111);
   this.Deptbox.Margin = new System.Windows.Forms.Padding(4);
   this.Deptbox.Name = "Deptbox";
   this.Deptbox.Padding = new System.Windows.Forms.Padding(4);
   this.Deptbox.Size = new System.Drawing.Size(487, 65);
   this.Deptbox.TabIndex = 0;
   this.Deptbox.TabStop = false;
   this.Deptbox.Text = "Department";
   // 
   // dept_tbox
   // 
   this.dept_tbox.Anchor = System.Windows.Forms.AnchorStyles.Left;
   this.dept_tbox.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
   this.dept_tbox.Location = new System.Drawing.Point(40, -76);
   this.dept_tbox.Margin = new System.Windows.Forms.Padding(4);
   this.dept_tbox.Name = "dept_tbox";
   this.dept_tbox.Size = new System.Drawing.Size(469, 24);
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
   this.Department.Location = new System.Drawing.Point(8, 26);
   this.Department.Margin = new System.Windows.Forms.Padding(4);
   this.Department.Name = "Department";
   this.Department.Size = new System.Drawing.Size(469, 23);
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
   this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
   this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
   this.ClientSize = new System.Drawing.Size(1546, 336);
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
   this.Margin = new System.Windows.Forms.Padding(4);
   this.Name = "Form1";
   this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
   this.Text = "Medical Profile Card";
   this.Load += new System.EventHandler(this.Form1_LoadAsync);
   this.ResizeEnd += new System.EventHandler(this.Form1_Resizeend);
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

  private OpenFileDialog OpenFileDialog1;

  private GroupBox GroupBox2;
  
  private TextBox Patient;
 
  private TextBox Phone;
 
  private TextBox DOB;

  private GroupBox Printbox;

  private ComboBox Printers;

  private RichTextBox address;

  private OpenFileDialog OpenFileDialog2;

  private FolderBrowserDialog FolderBrowserDialog1;

  private ComboBox dsaves;

  private GroupBox ppgb;

  private TextBox priph;

  private GroupBox GroupBox8;

  private TextBox ins;

  private MenuStrip MenuStrip1;

  private GroupBox sp;

  private TextBox secph;

  private GroupBox GroupBox3;

  private TextBox econtact;

  private Label Label4;

  private Label Label3;

  private Label Label2;

  private Label patient_label;

  private TextBox sptitle;

  private ContextMenuStrip ContextMenuStrip1;

  private Label Message_label;

  private ContextMenuStrip ContextMenuStrip2;

  private GroupBox Practbox;

  private TextBox Practice;

  private GroupBox Deptbox;

  private ComboBox Department;

  private Label pid_label;

  private TextBox dept_tbox;

  private TextBox Patientid;

  private ComboBox prv_combo;

  private ToolStripMenuItem Editmenuitem;

  private ToolStripMenuItem Previewmenuitem;

  private ToolStripMenuItem Printmenuitem;

  private ToolStripMenuItem Clrmenuitem;

  private ToolStripMenuItem Notesmenuitem;

  private ToolStripMenuItem Addpagemenuitem;

  private ToolStripMenuItem Aspecmenuitem;

  private ToolStripMenuItem exmenuitem;

  private TextBox dest;

  private ToolStripMenuItem Savemi;

  private ToolStripMenuItem Deletemi;

  private GroupBox Tgp;

  private TextBox Ettb;
 }
}