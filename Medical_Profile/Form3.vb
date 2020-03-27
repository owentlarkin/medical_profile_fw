Imports System.Drawing.Printing
Imports System.Windows.Forms
Imports System.Drawing.Imaging

Public Class Form3
 Dim Tsi As ToolStripMenuItem = Nothing
 Dim Cms As ContextMenuStrip = Nothing
 Dim Pnl As Panel = Nothing
 Dim Mpd As PrintDialog = Nothing
 Dim Mi As Bitmap = Nothing
 Dim Sdcb As System.Drawing.Color = Nothing
 Dim Pnlq As Queue(Of Panel) = Nothing
 Dim Tbl As TableLayoutPanel = Nothing

 Private Sub BtnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
  Dim pf As New PrintForm(Me)
  pf.Print(True, PrintForm.PrintMode_ENUM.FitToPage)
 End Sub

 Private Sub ClosePreviewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ClosePreviewToolStripMenuItem.Click
  Me.Close()
 End Sub

 Private Sub PrintFormToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PrintFormToolStripMenuItem.Click
  Dim pf As New PrintForm(Me)
  pf.Print(True, PrintForm.PrintMode_ENUM.FitToPage)
 End Sub

 Private Sub PrintDoc_PrintPage(sender As Object, e As PrintPageEventArgs) Handles PrintDoc.PrintPage
  Dim pf As New PrintForm(Me)
  pf.Print(True, PrintForm.PrintMode_ENUM.FitToPage)
 End Sub

 Public Sub Print_Panel(sender As Object, e As EventArgs)
  Tsi = sender
  Cms = Tsi.Owner
  Pnl = Cms.SourceControl
  Sdcb = Pnl.BackColor
  Dim Pw As Integer
  Dim Lm As Integer

  Pnl.BackColor = System.Drawing.Color.FromArgb(220, 220, 220)

  Mpd = New PrintDialog()

  Mi = New Bitmap(Pnl.Width, Pnl.Height)

  Pnl.DrawToBitmap(Mi, Pnl.ClientRectangle)

  Pnl.BackColor = Sdcb

  If Mpd.ShowDialog() = DialogResult.OK Then
   Dim values As PrinterSettings = New PrinterSettings()
   PrintDocument1.PrinterSettings = Mpd.PrinterSettings
   Mpd.Document = PrintDocument1
   PrintDocument1.PrintController = New StandardPrintController()
   Pw = PrintDocument1.PrinterSettings.DefaultPageSettings.Bounds.Width
   Lm = (Pw / 2.0) - (Pnl.Width / 2.0)
   PrintDocument1.PrinterSettings.DefaultPageSettings.Margins.Left = Lm
   PrintDocument1.Print()
  End If
  PrintDocument1.Dispose()
 End Sub

 Private Sub pd_PrintPage(sender As Object, e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
  e.Graphics.DrawImage(Mi, New Rectangle(e.MarginBounds.X, e.MarginBounds.Y, Mi.Width, Mi.Height))
  e.Graphics.DrawRectangle(Pens.Black, New Rectangle(e.MarginBounds.X, e.MarginBounds.Y, Mi.Width - 1, Mi.Height - 1))
 End Sub

 Private Sub PrintLabelsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PrintLabelsToolStripMenuItem.Click
  Dim Pw As Integer
  Dim Lm As Integer
  Dim p As Panel
  Dim Pnlw As Integer

  Tbl = Me.Controls(1)

  Pnlq = New Queue(Of Panel)

  For i As Integer = 0 To Tbl.RowCount - 1 Step 1
   For j As Integer = 0 To Tbl.ColumnCount - 1 Step 1
    p = Tbl.GetControlFromPosition(j, i)
    Pnlw = p.Width
    Pnlq.Enqueue(p)
   Next
  Next

  Mpd = New PrintDialog()

  If Mpd.ShowDialog() = DialogResult.OK Then
   Dim values As PrinterSettings = New PrinterSettings()
   PD2.PrinterSettings = Mpd.PrinterSettings
   Mpd.Document = PD2
   PD2.PrintController = New StandardPrintController()
   Pw = PD2.PrinterSettings.DefaultPageSettings.Bounds.Width
   Lm = (Pw / 2.0) - (Pnlw / 2.0)
   PD2.PrinterSettings.DefaultPageSettings.Margins.Left = Lm
   PD2.Print()
  End If
  PD2.Dispose()

 End Sub

 Private Sub PD2_PrintPage(sender As Object, e As System.Drawing.Printing.PrintPageEventArgs) Handles PD2.PrintPage
  Dim p2 As Panel = Pnlq.Dequeue()
  Sdcb = p2.BackColor
  p2.BackColor = System.Drawing.Color.FromArgb(220, 220, 220)

  Mi = New Bitmap(p2.Width, p2.Height)

  p2.DrawToBitmap(Mi, p2.ClientRectangle)

  p2.BackColor = Sdcb

  e.Graphics.DrawImage(Mi, New Rectangle(e.MarginBounds.X, e.MarginBounds.Y, Mi.Width, Mi.Height))
  e.Graphics.DrawRectangle(Pens.Black, New Rectangle(e.MarginBounds.X, e.MarginBounds.Y, Mi.Width - 1, Mi.Height - 1))

  If Pnlq.Count > 0 Then
   e.HasMorePages = True
  Else
   e.HasMorePages = False
  End If

 End Sub
End Class