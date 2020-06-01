using System;
using System.Collections.Generic;
using System.Drawing;
using global::System.Drawing.Printing;
using global::System.Windows.Forms;

namespace Medical_Profile
{
 public partial class Form3 : Form
 {
  public Form3()
  {
   InitializeComponent();
  }

  private ToolStripMenuItem Tsi = null;
  private ContextMenuStrip Cms = null;
  private Panel Pnl = null;
  private PrintDialog Mpd = null;
  private Bitmap Mi = null;
  private Color Sdcb = default;
  private Queue<Panel> Pnlq = null;
  private TableLayoutPanel Tbl = null;

  //private void BtnPrint_Click(object sender, EventArgs e)
  //{
  // var pf = new PrintForm(this);
  // pf.Print(true, PrintForm.PrintMode_ENUM.FitToPage);
  //}

  private void ClosePreviewToolStripMenuItem_Click(object sender, EventArgs e)
  {
   Close();
  }

  private void PrintFormToolStripMenuItem_Click(object sender, EventArgs e)
  {
   var pf = new PrintForm(this);
   pf.Print(true, PrintForm.PrintMode_ENUM.FitToPage);
  }

  private void PrintDoc_PrintPage(object sender, PrintPageEventArgs e)
  {
   var pf = new PrintForm(this);
   pf.Print(true, PrintForm.PrintMode_ENUM.FitToPage);
  }

  public void Print_Panel(object sender, EventArgs e)
  {
   Tsi = (ToolStripMenuItem)sender;
   Cms = (ContextMenuStrip)Tsi.Owner;
   Pnl = (Panel)Cms.SourceControl;
   Sdcb = Pnl.BackColor;
   int Pw;
   int Lm;
   Pnl.BackColor = Color.FromArgb(220, 220, 220);
   Mpd = new PrintDialog();
   Mi = new Bitmap(Pnl.Width, Pnl.Height);
   Pnl.DrawToBitmap(Mi, Pnl.ClientRectangle);
   Pnl.BackColor = Sdcb;
   if (Mpd.ShowDialog() == DialogResult.OK)
   {
    var values = new PrinterSettings();
    PrintDocument1.PrinterSettings = Mpd.PrinterSettings;
    Mpd.Document = PrintDocument1;
    PrintDocument1.PrintController = new StandardPrintController();
    Pw = PrintDocument1.PrinterSettings.DefaultPageSettings.Bounds.Width;
    Lm = (int)(Pw / 2.0 - Pnl.Width / 2.0);
    PrintDocument1.PrinterSettings.DefaultPageSettings.Margins.Left = Lm;
    PrintDocument1.Print();
   }

   PrintDocument1.Dispose();
  }

  private void pd_PrintPage(object sender, PrintPageEventArgs e)
  {
   e.Graphics.DrawImage(Mi, new Rectangle(e.MarginBounds.X, e.MarginBounds.Y, Mi.Width, Mi.Height));
   e.Graphics.DrawRectangle(Pens.Black, new Rectangle(e.MarginBounds.X, e.MarginBounds.Y, Mi.Width - 1, Mi.Height - 1));
  }

  private void PrintLabelsToolStripMenuItem_Click(object sender, EventArgs e)
  {
   int Pw;
   int Lm;
   Panel p;
   var Pnlw = default(int);
   Tbl = (TableLayoutPanel)Controls[1];
   Pnlq = new Queue<Panel>();
   for (int i = 0; i <= Tbl.RowCount - 1; i++)
   {
    for (int j = 0; j <= Tbl.ColumnCount - 1; j++)
    {
     p = (Panel)Tbl.GetControlFromPosition(j, i);
     if (p != null)
     {
      Pnlw = p.Width;
      Pnlq.Enqueue(p);
     }
    }
   }

   Mpd = new PrintDialog();
   if (Mpd.ShowDialog() == DialogResult.OK)
   {
    var values = new PrinterSettings();
    PD2.PrinterSettings = Mpd.PrinterSettings;
    Mpd.Document = PD2;
    PD2.PrintController = new StandardPrintController();
    Pw = PD2.PrinterSettings.DefaultPageSettings.Bounds.Width;
    Lm = (int)(Pw / 2.0 - Pnlw / 2.0);
    PD2.PrinterSettings.DefaultPageSettings.Margins.Left = Lm;
    PD2.Print();
   }

   PD2.Dispose();
  }

  private void PD2_PrintPage(object sender, PrintPageEventArgs e)
  {
   var p2 = Pnlq.Dequeue();
   Sdcb = p2.BackColor;
   p2.BackColor = Color.FromArgb(220, 220, 220);
   Mi = new Bitmap(p2.Width, p2.Height);
   p2.DrawToBitmap(Mi, p2.ClientRectangle);
   p2.BackColor = Sdcb;
   e.Graphics.DrawImage(Mi, new Rectangle(e.MarginBounds.X, e.MarginBounds.Y, Mi.Width, Mi.Height));
   e.Graphics.DrawRectangle(Pens.Black, new Rectangle(e.MarginBounds.X, e.MarginBounds.Y, Mi.Width - 1, Mi.Height - 1));
   if (Pnlq.Count > 0)
   {
    e.HasMorePages = true;
   }
   else
   {
    e.HasMorePages = false;
   }
  }

  private void Form3_Load(object sender, EventArgs e)
  {
   Panel Pnl = null; ;
   ContextMenuStrip Cms = null;
   ToolStripMenuItem Tsi = null;

   var gblabels = new[] { "First Label", "Second Label", "Third Label", "Fourth Label", "Fifth Label", "Sixth Label", "Seventh Label", "Eigth Label", "Ninth Label" };

   TableLayoutPanel Tbl = (TableLayoutPanel)Controls["Panel_Table"];

   for (int i = 0; i <= Tbl.Controls.Count - 1; i++)
   {
    Pnl = (Panel)Tbl.Controls[i];
    Cms = new ContextMenuStrip();
    Tsi = new ToolStripMenuItem()
    {
     Text = "Print Label",
     Name = gblabels[i]
    };

    Tsi.Click += Print_Panel;
    Cms.Items.Add(Tsi);
    Pnl.ContextMenuStrip = Cms;
   }
  }
 }
}