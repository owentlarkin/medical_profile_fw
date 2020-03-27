<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Formaik
  Inherits System.Windows.Forms.Form

  'Form overrides dispose to clean up the component list.
  <System.Diagnostics.DebuggerNonUserCode()> _
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
  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
    Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Formaik))
    Me.Label1 = New System.Windows.Forms.Label()
    Me.Key = New System.Windows.Forms.TextBox()
    Me.Done = New System.Windows.Forms.Button()
    Me.Register = New System.Windows.Forms.Button()
    Me.SuspendLayout()
    '
    'Label1
    '
    Me.Label1.AutoSize = True
    Me.Label1.Location = New System.Drawing.Point(13, 22)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(297, 18)
    Me.Label1.TabIndex = 0
    Me.Label1.Text = "Please enter the intallation key from the email"
    '
    'Key
    '
    Me.Key.Location = New System.Drawing.Point(316, 19)
    Me.Key.Name = "Key"
    Me.Key.Size = New System.Drawing.Size(324, 26)
    Me.Key.TabIndex = 1
    '
    'Done
    '
    Me.Done.Location = New System.Drawing.Point(385, 74)
    Me.Done.Name = "Done"
    Me.Done.Size = New System.Drawing.Size(104, 33)
    Me.Done.TabIndex = 2
    Me.Done.Text = "Done"
    Me.Done.UseVisualStyleBackColor = True
    '
    'Register
    '
    Me.Register.Location = New System.Drawing.Point(133, 74)
    Me.Register.Name = "Register"
    Me.Register.Size = New System.Drawing.Size(104, 33)
    Me.Register.TabIndex = 3
    Me.Register.Text = "Register"
    Me.Register.UseVisualStyleBackColor = True
    '
    'Formaik
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 18.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.ClientSize = New System.Drawing.Size(654, 119)
    Me.Controls.Add(Me.Register)
    Me.Controls.Add(Me.Done)
    Me.Controls.Add(Me.Key)
    Me.Controls.Add(Me.Label1)
    Me.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Margin = New System.Windows.Forms.Padding(4)
    Me.Name = "Formaik"
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
    Me.Text = "Medical Profile"
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub

  Friend WithEvents Label1 As Label
  Friend WithEvents Key As TextBox
  Friend WithEvents Done As Button
  Friend WithEvents Register As Button
End Class
