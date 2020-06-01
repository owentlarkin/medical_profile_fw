using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Medical_Profile
{
 public partial class Form3 : Form
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
  private System.ComponentModel.IContainer components = null;

  // NOTE: The following procedure is required by the Windows Form Designer
  // It can be modified using the Windows Form Designer.  
  // Do not modify it using the code editor.
  [DebuggerStepThrough()]
  private void InitializeComponent()
  {
   this._MenuStrip1 = new System.Windows.Forms.MenuStrip();
   this._ClosePreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
   this._PrintFormToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
   this._PrintLabelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
   this._PrintDoc = new System.Drawing.Printing.PrintDocument();
   this._PrintDocument1 = new System.Drawing.Printing.PrintDocument();
   this._PD2 = new System.Drawing.Printing.PrintDocument();
   this._MenuStrip1.SuspendLayout();
   this.SuspendLayout();
   // 
   // _MenuStrip1
   // 
   this._MenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._ClosePreviewToolStripMenuItem,
            this._PrintFormToolStripMenuItem,
            this._PrintLabelsToolStripMenuItem});
   this._MenuStrip1.Location = new System.Drawing.Point(0, 0);
   this._MenuStrip1.Name = "_MenuStrip1";
   this._MenuStrip1.Size = new System.Drawing.Size(1033, 24);
   this._MenuStrip1.TabIndex = 1;
   this._MenuStrip1.Text = "MenuStrip1";
   // 
   // _ClosePreviewToolStripMenuItem
   // 
   this._ClosePreviewToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
   this._ClosePreviewToolStripMenuItem.Name = "_ClosePreviewToolStripMenuItem";
   this._ClosePreviewToolStripMenuItem.Size = new System.Drawing.Size(97, 20);
   this._ClosePreviewToolStripMenuItem.Text = "Close Preview";
   this._ClosePreviewToolStripMenuItem.Click += new System.EventHandler(this.ClosePreviewToolStripMenuItem_Click);
   // 
   // _PrintFormToolStripMenuItem
   // 
   this._PrintFormToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
   this._PrintFormToolStripMenuItem.Name = "_PrintFormToolStripMenuItem";
   this._PrintFormToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
   this._PrintFormToolStripMenuItem.Text = "Print Form";
   this._PrintFormToolStripMenuItem.Click += new System.EventHandler(this.PrintFormToolStripMenuItem_Click);
   // 
   // _PrintLabelsToolStripMenuItem
   // 
   this._PrintLabelsToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
   this._PrintLabelsToolStripMenuItem.Name = "_PrintLabelsToolStripMenuItem";
   this._PrintLabelsToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
   this._PrintLabelsToolStripMenuItem.Text = "Print Labels";
   this._PrintLabelsToolStripMenuItem.Click += new System.EventHandler(this.PrintLabelsToolStripMenuItem_Click);
   // 
   // _PrintDoc
   // 
   this._PrintDoc.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.PrintDoc_PrintPage);
   // 
   // _PrintDocument1
   // 
   this._PrintDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.pd_PrintPage);
   // 
   // _PD2
   // 
   this._PD2.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.PD2_PrintPage);
   // 
   // Form3
   // 
   this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
   this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
   this.AutoScroll = true;
   this.AutoSize = true;
   this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
   this.ClientSize = new System.Drawing.Size(1033, 623);
   this.Controls.Add(this._MenuStrip1);
   this.MainMenuStrip = this._MenuStrip1;
   this.Name = "Form3";
   this.ShowIcon = false;
   this.Text = "Form3";
   this.Load += new System.EventHandler(this.Form3_Load);
   this._MenuStrip1.ResumeLayout(false);
   this._MenuStrip1.PerformLayout();
   this.ResumeLayout(false);
   this.PerformLayout();

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

  private ToolStripMenuItem _ClosePreviewToolStripMenuItem;

  internal ToolStripMenuItem ClosePreviewToolStripMenuItem
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _ClosePreviewToolStripMenuItem;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_ClosePreviewToolStripMenuItem != null)
    {
     _ClosePreviewToolStripMenuItem.Click -= ClosePreviewToolStripMenuItem_Click;
    }

    _ClosePreviewToolStripMenuItem = value;
    if (_ClosePreviewToolStripMenuItem != null)
    {
     _ClosePreviewToolStripMenuItem.Click += ClosePreviewToolStripMenuItem_Click;
    }
   }
  }

  private ToolStripMenuItem _PrintFormToolStripMenuItem;

  internal ToolStripMenuItem PrintFormToolStripMenuItem
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _PrintFormToolStripMenuItem;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_PrintFormToolStripMenuItem != null)
    {
     _PrintFormToolStripMenuItem.Click -= PrintFormToolStripMenuItem_Click;
    }

    _PrintFormToolStripMenuItem = value;
    if (_PrintFormToolStripMenuItem != null)
    {
     _PrintFormToolStripMenuItem.Click += PrintFormToolStripMenuItem_Click;
    }
   }
  }

  private System.Drawing.Printing.PrintDocument _PrintDoc;

  internal System.Drawing.Printing.PrintDocument PrintDoc
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _PrintDoc;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_PrintDoc != null)
    {
     _PrintDoc.PrintPage -= PrintDoc_PrintPage;
    }

    _PrintDoc = value;
    if (_PrintDoc != null)
    {
     _PrintDoc.PrintPage += PrintDoc_PrintPage;
    }
   }
  }

  private System.Drawing.Printing.PrintDocument _PrintDocument1;

  internal System.Drawing.Printing.PrintDocument PrintDocument1
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _PrintDocument1;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_PrintDocument1 != null)
    {
     _PrintDocument1.PrintPage -= pd_PrintPage;
    }

    _PrintDocument1 = value;
    if (_PrintDocument1 != null)
    {
     _PrintDocument1.PrintPage += pd_PrintPage;
    }
   }
  }

  private ToolStripMenuItem _PrintLabelsToolStripMenuItem;

  internal ToolStripMenuItem PrintLabelsToolStripMenuItem
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _PrintLabelsToolStripMenuItem;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_PrintLabelsToolStripMenuItem != null)
    {
     _PrintLabelsToolStripMenuItem.Click -= PrintLabelsToolStripMenuItem_Click;
    }

    _PrintLabelsToolStripMenuItem = value;
    if (_PrintLabelsToolStripMenuItem != null)
    {
     _PrintLabelsToolStripMenuItem.Click += PrintLabelsToolStripMenuItem_Click;
    }
   }
  }

  private System.Drawing.Printing.PrintDocument _PD2;

  internal System.Drawing.Printing.PrintDocument PD2
  {
   [MethodImpl(MethodImplOptions.Synchronized)]
   get
   {
    return _PD2;
   }

   [MethodImpl(MethodImplOptions.Synchronized)]
   set
   {
    if (_PD2 != null)
    {
     _PD2.PrintPage -= PD2_PrintPage;
    }

    _PD2 = value;
    if (_PD2 != null)
    {
     _PD2.PrintPage += PD2_PrintPage;
    }
   }
  }
 }
}