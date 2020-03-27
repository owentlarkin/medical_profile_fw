using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;

namespace Medical_Profile
{
 [DesignerGenerated()]
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
  private System.ComponentModel.IContainer components;

  // NOTE: The following procedure is required by the Windows Form Designer
  // It can be modified using the Windows Form Designer.  
  // Do not modify it using the code editor.
  [DebuggerStepThrough()]
  private void InitializeComponent()
  {
   _MenuStrip1 = new MenuStrip();
   _ClosePreviewToolStripMenuItem = new ToolStripMenuItem();
   _ClosePreviewToolStripMenuItem.Click += new EventHandler(ClosePreviewToolStripMenuItem_Click);
   _PrintFormToolStripMenuItem = new ToolStripMenuItem();
   _PrintFormToolStripMenuItem.Click += new EventHandler(PrintFormToolStripMenuItem_Click);
   _PrintDoc = new System.Drawing.Printing.PrintDocument();
   _PrintDoc.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(PrintDoc_PrintPage);
   _PrintDocument1 = new System.Drawing.Printing.PrintDocument();
   _PrintDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(pd_PrintPage);
   _PrintLabelsToolStripMenuItem = new ToolStripMenuItem();
   _PrintLabelsToolStripMenuItem.Click += new EventHandler(PrintLabelsToolStripMenuItem_Click);
   _PD2 = new System.Drawing.Printing.PrintDocument();
   _PD2.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(PD2_PrintPage);
   _MenuStrip1.SuspendLayout();
   SuspendLayout();
   // 
   // MenuStrip1
   // 
   _MenuStrip1.Items.AddRange(new ToolStripItem[] { _ClosePreviewToolStripMenuItem, _PrintFormToolStripMenuItem, _PrintLabelsToolStripMenuItem });
   _MenuStrip1.Location = new Point(0, 0);
   _MenuStrip1.Name = "MenuStrip1";
   _MenuStrip1.Size = new Size(1033, 24);
   _MenuStrip1.TabIndex = 1;
   _MenuStrip1.Text = "MenuStrip1";
   // 
   // ClosePreviewToolStripMenuItem
   // 
   _ClosePreviewToolStripMenuItem.Font = new Font("Segoe UI", 9.0F, FontStyle.Bold, GraphicsUnit.Point, Conversions.ToByte(0));
   _ClosePreviewToolStripMenuItem.Name = "ClosePreviewToolStripMenuItem";
   _ClosePreviewToolStripMenuItem.Size = new Size(97, 20);
   _ClosePreviewToolStripMenuItem.Text = "Close Preview";
   // 
   // PrintFormToolStripMenuItem
   // 
   _PrintFormToolStripMenuItem.Font = new Font("Segoe UI", 9.0F, FontStyle.Bold, GraphicsUnit.Point, Conversions.ToByte(0));
   _PrintFormToolStripMenuItem.Name = "PrintFormToolStripMenuItem";
   _PrintFormToolStripMenuItem.Size = new Size(78, 20);
   _PrintFormToolStripMenuItem.Text = "Print Form";
   // 
   // PrintDoc
   // 
   // 
   // PrintDocument1
   // 
   // 
   // PrintLabelsToolStripMenuItem
   // 
   _PrintLabelsToolStripMenuItem.Font = new Font("Segoe UI", 9.0F, FontStyle.Bold, GraphicsUnit.Point, Conversions.ToByte(0));
   _PrintLabelsToolStripMenuItem.Name = "PrintLabelsToolStripMenuItem";
   _PrintLabelsToolStripMenuItem.Size = new Size(83, 20);
   _PrintLabelsToolStripMenuItem.Text = "Print Labels";
   // 
   // Form3
   // 
   AutoScaleDimensions = new SizeF(6.0F, 13.0F);
   AutoScaleMode = AutoScaleMode.Font;
   AutoScroll = true;
   AutoSize = true;
   AutoSizeMode = AutoSizeMode.GrowAndShrink;
   ClientSize = new Size(1033, 623);
   Controls.Add(_MenuStrip1);
   MainMenuStrip = _MenuStrip1;
   Name = "Form3";
   ShowIcon = false;
   Text = "Form3";
   _MenuStrip1.ResumeLayout(false);
   _MenuStrip1.PerformLayout();
   ResumeLayout(false);
   PerformLayout();
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