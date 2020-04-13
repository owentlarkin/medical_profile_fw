using System;
using global::System.Drawing;
using System.Runtime.InteropServices;
using global::System.Windows.Forms;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;

namespace Medical_Profile
{
 /// <summary>
/// Prints a screengrab of the form
/// </summary>
/// <remarks></remarks>
 public class PrintForm
 {
  // USAGE:
  // Dim pf As New PrintForm(Me)
  // pf.PrintPreview()
  // - or-
  // pf.Print()
  // 
  [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
  private static extern bool BitBlt(IntPtr hDIDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop); // API call to help generate final screenshot

  private Bitmap mbmpScreenshot; // Variable to store screenshot
  private bool mblnLandscape = false;

  public enum PrintMode_ENUM : int
  {
   Default,
   FitToPage
  }

  private PrintMode_ENUM menuPrintMode = PrintMode_ENUM.Default;
  // 
  private Form mfrm;

  public PrintForm(Form frm)
  {
   mfrm = frm;
   GrabScreen();
  }
  // 
  /// <summary>
    /// Determines page settings for current page e.g. Orientation
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <remarks></remarks>
  private void QueryPageSettings(object sender, System.Drawing.Printing.QueryPageSettingsEventArgs e)
  {
   // 
   var pgsTemp = new System.Drawing.Printing.PageSettings();
   pgsTemp.Landscape = mblnLandscape;
   e.PageSettings = pgsTemp;
   // 
  }
  // 
  public void Print(bool landscape, PrintMode_ENUM printMode, string docname = "PrintForm", string PrinterName = "")
  {
   mblnLandscape = landscape;
   menuPrintMode = printMode;
   // create the document object
   using (var pdcNew = new System.Drawing.Printing.PrintDocument())
   {
    // 
    // wire up event handlers to handle pagination
    pdcNew.PrintPage += PrintPage;
    pdcNew.QueryPageSettings += QueryPageSettings;
    // 
    using (var docOutput = pdcNew)
    {
     if (PrinterName != "")
     {
      docOutput.PrinterSettings.PrinterName = PrinterName;
     }

     docOutput.DocumentName = docname;
     docOutput.Print();
    }
   }
  }
  // 
  /// <summary>
    /// Preview the Report on screen
    /// </summary>
    /// <remarks></remarks>
  public void PrintPreview(bool landscape, PrintMode_ENUM printMode, string docname = "PrintForm", Form Owner = null)
  {
   mblnLandscape = landscape;
   menuPrintMode = printMode;
   // 
   // create the document object
   using (var pdcNew = new System.Drawing.Printing.PrintDocument())
   {
    // 
    // wire up event handlers to handle pagination
    pdcNew.PrintPage += PrintPage;
    pdcNew.QueryPageSettings += QueryPageSettings;
    // 
    using (var ppvPreview = new PrintPreviewDialog())
    {
     ppvPreview.Document = pdcNew;
     ppvPreview.FindForm().WindowState = FormWindowState.Maximized;
     if (Information.IsNothing(Owner))
     {
      ppvPreview.ShowDialog();
     }
     else
     {
      ppvPreview.ShowDialog(Owner);
     }
    }
   }
  }

  public void PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
  {
   var g = e.Graphics; // shortcut
                       // g.DrawRectangle(Pens.Red, e.MarginBounds) 'DEBUG: use this line to check margins        
                       // 
                       // Method that handles the printing
   using (var objImageToPrint = e.Graphics)
   {
    var switchExpr = menuPrintMode;
    switch (switchExpr)
    {
     case PrintMode_ENUM.FitToPage:
      {
       Rectangle rctTarget;
       if (mbmpScreenshot.Width / (double)mbmpScreenshot.Height < e.MarginBounds.Width / (double)e.MarginBounds.Height)
       {
        // fit height
        rctTarget = new Rectangle(e.MarginBounds.X, e.MarginBounds.Y, Conversions.ToInteger(mbmpScreenshot.Width * e.MarginBounds.Height / (double)mbmpScreenshot.Height), e.MarginBounds.Height);
       }
       else
       {
        // fit width
        rctTarget = new Rectangle(e.MarginBounds.X, e.MarginBounds.Y, e.MarginBounds.Width, Conversions.ToInteger(mbmpScreenshot.Height * e.MarginBounds.Width / (double)mbmpScreenshot.Width));
       }
       // g.DrawRectangle(Pens.Blue, rctTarget) 'DEBUG: use this line to check target rectangle
       objImageToPrint.DrawImage(mbmpScreenshot, rctTarget); // default
       break;
      }

     default:
      {
       objImageToPrint.DrawImage(mbmpScreenshot, 0, 0);
       break;
      }
    }
   }
   // 
   e.HasMorePages = false;
  }
  // 
  private void GrabScreen()
  {
   // Performs a screenshot, saving results to bmpScreenshot
   var objGraphics = mfrm.CreateGraphics();
   var rctForm = mfrm.ClientRectangle; // including the border is beyond the scope of this demo program. See http://support.microsoft.com/kb/84066 for GetSystemMetrics() API to get  size of border
                                       // 
   const int SRCCOPY = 0xCC0020;
   mbmpScreenshot = new Bitmap(rctForm.Width, rctForm.Height, objGraphics);
   var objGraphics2 = Graphics.FromImage(mbmpScreenshot);
   var deviceContext1 = objGraphics.GetHdc();
   var deviceContext2 = objGraphics2.GetHdc();
   // 
   BitBlt(deviceContext2, rctForm.X, rctForm.Y, rctForm.Width, rctForm.Height, deviceContext1, 0, 0, SRCCOPY);
   objGraphics.ReleaseHdc(deviceContext1);
   objGraphics2.ReleaseHdc(deviceContext2);
  }
  // 
 }
}
// 
