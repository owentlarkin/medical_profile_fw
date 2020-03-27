Imports System.Windows.Forms.Design

<ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip), DebuggerStepThrough()>
Public Class TSnumud
    Inherits ToolStripControlHost

    Public aml As Integer = -1
    Public udl As Integer = -1

    Public Sub New()
        MyBase.New(New System.Windows.Forms.NumericUpDown())
        '     ToolStriptrackbarControl.BackColor = Color.Transparent
    End Sub

    Public ReadOnly Property TSnumudControl() As NumericUpDown
        Get
            Return TryCast(Control, NumericUpDown)
        End Get
    End Property

    Public ReadOnly Property databindings As ControlBindingsCollection
        Get
            Return TSnumudControl.DataBindings
        End Get
    End Property

    Public Property Value As Integer
        Get
            Return TSnumudControl.Value
        End Get
        Set
            TSnumudControl.Value = Value
        End Set
    End Property

    Property Minimum As Integer
        Get
            Return TSnumudControl.Minimum
        End Get
        Set(ByVal value As Integer)
            TSnumudControl.Minimum = value
        End Set
    End Property

    ' Property Overrides Text As String
    'Get
    'Return TSnumudControl.Text
    'End Get
    'Set(ByVal value As String)
    '       TSnumudControl.Text = value
    'End Set
    'End Property


    Public Property Maximum As Integer
        Get
            Return TSnumudControl.Maximum
        End Get
        Set
            TSnumudControl.Maximum = Value
        End Set
    End Property

    Public Property ToolStriptrackbarEnabled() As Boolean
        Get
            Return TSnumudControl.Enabled
        End Get
        Set(ByVal value As Boolean)
            TSnumudControl.Enabled = value
        End Set
    End Property

    Protected Overrides Sub OnSubscribeControlEvents(ByVal c As Control)

        ' Call the base so the base events are connected.
        MyBase.OnSubscribeControlEvents(c)

        ' Cast the control to a MonthCalendar control.
        Dim TSnumudControl As NumericUpDown =
            CType(c, NumericUpDown)

        ' Add the event.
        AddHandler TSnumudControl.ValueChanged,
            AddressOf HandleValueChanged

    End Sub
    '   Protected Overrides Sub OnSubscribeControlEvents(ByVal c As Control)
    '  MyBase.OnSubscribeControlEvents(c)
    ' AddHandler DirectCast(c, CheckBox).CheckedChanged, AddressOf OnCheckedChanged
    'End Sub

    ' Protected Overrides Sub OnUnsubscribeControlEvents(ByVal c As Control)
    'MyBase.OnUnsubscribeControlEvents(c)
    'RemoveHandler DirectCast(c, CheckBox).CheckedChanged, AddressOf OnCheckedChanged
    'End Sub

    Public Event ValueChanged As EventHandler

    Private Sub HandleValueChanged(ByVal sender As Object, ByVal e As EventArgs)
        RaiseEvent ValueChanged(Me, e)
    End Sub
End Class