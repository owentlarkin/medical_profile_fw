Imports System.Drawing
Imports System.Windows.Forms
''' <summary>
''' Prints a screengrab of the form
''' </summary>
''' <remarks></remarks>
Public Class PrintForm
    'USAGE:
    ' Dim pf As New PrintForm(Me)
    ' pf.PrintPreview()
    ' - or-
    ' pf.Print()
    '
    Private Declare Auto Function BitBlt Lib "gdi32.dll" (ByVal hDIDest As IntPtr, ByVal nXDest As Integer, ByVal nYDest As Integer, ByVal nWidth As Integer, ByVal nHeight As Integer, ByVal hdcSrc As IntPtr, ByVal nXSrc As Integer, ByVal nYSrc As Integer, ByVal dwRop As System.Int32) As Boolean ' API call to help generate final screenshot
    Private mbmpScreenshot As Bitmap ' Variable to store screenshot
    Private mblnLandscape As Boolean = False
    Public Enum PrintMode_ENUM As Integer
        [Default]
        FitToPage
    End Enum
    Private menuPrintMode As PrintMode_ENUM = PrintMode_ENUM.Default
    '
    Private mfrm As Form
    Public Sub New(ByVal frm As Form)
        mfrm = frm
        Call GrabScreen()
    End Sub
    '
    ''' <summary>
    ''' Determines page settings for current page e.g. Orientation 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub QueryPageSettings(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.QueryPageSettingsEventArgs)
        '
        Dim pgsTemp As System.Drawing.Printing.PageSettings = New System.Drawing.Printing.PageSettings()
        pgsTemp.Landscape = mblnLandscape
        e.PageSettings = pgsTemp
        '
    End Sub
    '
    Public Sub Print(landscape As Boolean, printMode As PrintMode_ENUM, Optional ByVal docname As String = "PrintForm", Optional ByVal PrinterName As String = "")
        mblnLandscape = landscape
        menuPrintMode = printMode
        'create the document object
        Using pdcNew As New Printing.PrintDocument
            '
            'wire up event handlers to handle pagination
            AddHandler pdcNew.PrintPage, AddressOf PrintPage
            AddHandler pdcNew.QueryPageSettings, AddressOf QueryPageSettings
            '
            Using docOutput As Printing.PrintDocument = pdcNew
                If PrinterName > "" Then
                    docOutput.PrinterSettings.PrinterName = PrinterName
                End If
                docOutput.DocumentName = docname
                docOutput.Print()
            End Using
        End Using
    End Sub
    '
    ''' <summary>
    ''' Preview the Report on screen
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub PrintPreview(landscape As Boolean, printMode As PrintMode_ENUM, Optional ByVal docname As String = "PrintForm", Optional ByVal Owner As Form = Nothing)
        mblnLandscape = landscape
        menuPrintMode = printMode
        '
        'create the document object
        Using pdcNew As New Printing.PrintDocument
            '
            'wire up event handlers to handle pagination
            AddHandler pdcNew.PrintPage, AddressOf PrintPage
            AddHandler pdcNew.QueryPageSettings, AddressOf QueryPageSettings
            '
            Using ppvPreview As New PrintPreviewDialog
                ppvPreview.Document = pdcNew
                ppvPreview.FindForm.WindowState = FormWindowState.Maximized
                If IsNothing(Owner) Then
                    ppvPreview.ShowDialog()
                Else
                    ppvPreview.ShowDialog(Owner)
                End If
            End Using
        End Using
    End Sub
    Sub PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        Dim g As Graphics = e.Graphics 'shortcut
        'g.DrawRectangle(Pens.Red, e.MarginBounds) 'DEBUG: use this line to check margins        
        '
        ' Method that handles the printing
        Using objImageToPrint As Graphics = e.Graphics
            Select Case menuPrintMode
                Case PrintMode_ENUM.FitToPage
                    Dim rctTarget As Rectangle
                    If (mbmpScreenshot.Width / mbmpScreenshot.Height) < (e.MarginBounds.Width / e.MarginBounds.Height) Then
                        'fit height
                        rctTarget = New Rectangle(e.MarginBounds.X, e.MarginBounds.Y, CInt(mbmpScreenshot.Width * e.MarginBounds.Height / mbmpScreenshot.Height), e.MarginBounds.Height)
                    Else
                        'fit width
                        rctTarget = New Rectangle(e.MarginBounds.X, e.MarginBounds.Y, e.MarginBounds.Width, CInt(mbmpScreenshot.Height * e.MarginBounds.Width / mbmpScreenshot.Width))
                    End If
                    'g.DrawRectangle(Pens.Blue, rctTarget) 'DEBUG: use this line to check target rectangle
                    objImageToPrint.DrawImage(mbmpScreenshot, rctTarget)
                Case Else 'default
                    objImageToPrint.DrawImage(mbmpScreenshot, 0, 0)
            End Select
        End Using
        '
        e.HasMorePages = False
    End Sub
    '
    Private Sub GrabScreen()
        ' Performs a screenshot, saving results to bmpScreenshot
        Dim objGraphics As Graphics = mfrm.CreateGraphics
        Dim rctForm As Rectangle = mfrm.ClientRectangle 'including the border is beyond the scope of this demo program. See http://support.microsoft.com/kb/84066 for GetSystemMetrics() API to get  size of border
        '
        Const SRCCOPY As Integer = &HCC0020
        mbmpScreenshot = New Bitmap(rctForm.Width, rctForm.Height, objGraphics)
        Dim objGraphics2 As Graphics = Graphics.FromImage(mbmpScreenshot)
        Dim deviceContext1 As IntPtr = objGraphics.GetHdc
        Dim deviceContext2 As IntPtr = objGraphics2.GetHdc
        '
        BitBlt(deviceContext2, rctForm.X, rctForm.Y, rctForm.Width, rctForm.Height, deviceContext1, 0, 0, SRCCOPY)
        objGraphics.ReleaseHdc(deviceContext1)
        objGraphics2.ReleaseHdc(deviceContext2)
    End Sub
    '
End Class
'
