using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace Medical_Profile
{
 [DesignerGenerated()]
 public partial class Formaik : Form
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
   var resources = new System.ComponentModel.ComponentResourceManager(typeof(Formaik));
   _Label1 = new Label();
   _Key = new TextBox();
   _Done = new Button();
   _Done.Click += new EventHandler(Done_Click);
   _Register = new Button();
   _Register.Click += new EventHandler(Register_Click);
   SuspendLayout();
   // 
   // Label1
   // 
   _Label1.AutoSize = true;
   _Label1.Location = new Point(13, 22);
   _Label1.Name = "Label1";
   _Label1.Size = new Size(297, 18);
   _Label1.TabIndex = 0;
   _Label1.Text = "Please enter the intallation key from the email";
   // 
   // Key
   // 
   _Key.Location = new Point(316, 19);
   _Key.Name = "Key";
   _Key.Size = new Size(324, 26);
   _Key.TabIndex = 1;
   // 
   // Done
   // 
   _Done.Location = new Point(385, 74);
   _Done.Name = "Done";
   _Done.Size = new Size(104, 33);
   _Done.TabIndex = 2;
   _Done.Text = "Done";
   _Done.UseVisualStyleBackColor = true;
   // 
   // Register
   // 
   _Register.Location = new Point(133, 74);
   _Register.Name = "Register";
   _Register.Size = new Size(104, 33);
   _Register.TabIndex = 3;
   _Register.Text = "Register";
   _Register.UseVisualStyleBackColor = true;
   // 
   // Formaik
   // 
   AutoScaleDimensions = new SizeF(8.0F, 18.0F);
   AutoScaleMode = AutoScaleMode.Font;
   ClientSize = new Size(654, 119);
   Controls.Add(_Register);
   Controls.Add(_Done);
   Controls.Add(_Key);
   Controls.Add(_Label1);
   Font = new Font("Calibri", 11.25F, FontStyle.Bold, GraphicsUnit.Point, Conversions.ToByte(0));
   Icon = (Icon)resources.GetObject("$this.Icon");
   Margin = new Padding(4);
   Name = "Formaik";
   StartPosition = FormStartPosition.CenterScreen;
   Text = "Medical Profile";
   ResumeLayout(false);
   PerformLayout();
  }

  private Label _Label1;

  internal Label Label1
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _Label1;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_Label1 != null)
    {
    }

    _Label1 = value;
    if (_Label1 != null)
    {
    }
   }
  }

  private TextBox _Key;

  internal TextBox Key
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _Key;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_Key != null)
    {
    }

    _Key = value;
    if (_Key != null)
    {
    }
   }
  }

  private Button _Done;

  internal Button Done
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _Done;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_Done != null)
    {
     _Done.Click -= Done_Click;
    }

    _Done = value;
    if (_Done != null)
    {
     _Done.Click += Done_Click;
    }
   }
  }

  private Button _Register;

  internal Button Register
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _Register;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_Register != null)
    {
     _Register.Click -= Register_Click;
    }

    _Register = value;
    if (_Register != null)
    {
     _Register.Click += Register_Click;
    }
   }
  }
 }
}