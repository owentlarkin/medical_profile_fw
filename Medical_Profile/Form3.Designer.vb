<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form3
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
  Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
  Me.ClosePreviewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
  Me.PrintFormToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
  Me.PrintDoc = New System.Drawing.Printing.PrintDocument()
  Me.PrintDocument1 = New System.Drawing.Printing.PrintDocument()
  Me.PrintLabelsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
  Me.PD2 = New System.Drawing.Printing.PrintDocument()
  Me.MenuStrip1.SuspendLayout()
  Me.SuspendLayout()
  '
  'MenuStrip1
  '
  Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ClosePreviewToolStripMenuItem, Me.PrintFormToolStripMenuItem, Me.PrintLabelsToolStripMenuItem})
  Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
  Me.MenuStrip1.Name = "MenuStrip1"
  Me.MenuStrip1.Size = New System.Drawing.Size(1033, 24)
  Me.MenuStrip1.TabIndex = 1
  Me.MenuStrip1.Text = "MenuStrip1"
  '
  'ClosePreviewToolStripMenuItem
  '
  Me.ClosePreviewToolStripMenuItem.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
  Me.ClosePreviewToolStripMenuItem.Name = "ClosePreviewToolStripMenuItem"
  Me.ClosePreviewToolStripMenuItem.Size = New System.Drawing.Size(97, 20)
  Me.ClosePreviewToolStripMenuItem.Text = "Close Preview"
  '
  'PrintFormToolStripMenuItem
  '
  Me.PrintFormToolStripMenuItem.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
  Me.PrintFormToolStripMenuItem.Name = "PrintFormToolStripMenuItem"
  Me.PrintFormToolStripMenuItem.Size = New System.Drawing.Size(78, 20)
  Me.PrintFormToolStripMenuItem.Text = "Print Form"
  '
  'PrintDoc
  '
  '
  'PrintDocument1
  '
  '
  'PrintLabelsToolStripMenuItem
  '
  Me.PrintLabelsToolStripMenuItem.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
  Me.PrintLabelsToolStripMenuItem.Name = "PrintLabelsToolStripMenuItem"
  Me.PrintLabelsToolStripMenuItem.Size = New System.Drawing.Size(83, 20)
  Me.PrintLabelsToolStripMenuItem.Text = "Print Labels"
  '
  'Form3
  '
  Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
  Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
  Me.AutoScroll = True
  Me.AutoSize = True
  Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
  Me.ClientSize = New System.Drawing.Size(1033, 623)
  Me.Controls.Add(Me.MenuStrip1)
  Me.MainMenuStrip = Me.MenuStrip1
  Me.Name = "Form3"
  Me.ShowIcon = False
  Me.Text = "Form3"
  Me.MenuStrip1.ResumeLayout(False)
  Me.MenuStrip1.PerformLayout()
  Me.ResumeLayout(False)
  Me.PerformLayout()

 End Sub

 Friend WithEvents MenuStrip1 As MenuStrip
  Friend WithEvents ClosePreviewToolStripMenuItem As ToolStripMenuItem
  Friend WithEvents PrintFormToolStripMenuItem As ToolStripMenuItem
  Friend WithEvents PrintDoc As Drawing.Printing.PrintDocument
 Friend WithEvents PrintDocument1 As Drawing.Printing.PrintDocument
 Friend WithEvents PrintLabelsToolStripMenuItem As ToolStripMenuItem
 Friend WithEvents PD2 As Drawing.Printing.PrintDocument
End Class
