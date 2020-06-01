using System;
using System.Diagnostics;
using System.Windows.Forms;
using global::System.Windows.Forms.Design;
//using Microsoft.VisualBasic.CompilerServices;

namespace Medical_Profile
{
 [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip)]
 [DebuggerStepThrough()]
 public class TSnumud : ToolStripControlHost
 {
  public int aml = -1;
  public int udl = -1;

  public TSnumud() : base(new NumericUpDown())
  {
   // ToolStriptrackbarControl.BackColor = Color.Transparent
  }

  public NumericUpDown TSnumudControl
  {
   get
   {
    return Control as NumericUpDown;
   }
  }

  public ControlBindingsCollection databindings
  {
   get
   {
    return TSnumudControl.DataBindings;
   }
  }

  public int Value
  {
   get
   {
    return Convert.ToInt32(TSnumudControl.Value);
   }

   set
   {
    TSnumudControl.Value = value;
   }
  }

  public int Minimum
  {
   get
   {
    return Convert.ToInt32(TSnumudControl.Minimum);
   }

   set
   {
    TSnumudControl.Minimum = value;
   }
  }

  // Property Overrides Text As String
  // Get
  // Return TSnumudControl.Text
  // End Get
  // Set(ByVal value As String)
  // TSnumudControl.Text = value
  // End Set
  // End Property


  public int Maximum
  {
   get
   {
    return Convert.ToInt32(TSnumudControl.Maximum);
   }

   set
   {
    TSnumudControl.Maximum = value;
   }
  }

  public bool ToolStriptrackbarEnabled
  {
   get
   {
    return TSnumudControl.Enabled;
   }

   set
   {
    TSnumudControl.Enabled = value;
   }
  }

  protected override void OnSubscribeControlEvents(Control c)
  {

   // Call the base so the base events are connected.
   base.OnSubscribeControlEvents(c);

   // Cast the control to a MonthCalendar control.
   NumericUpDown TSnumudControl = (NumericUpDown)c;

   // Add the event.
   TSnumudControl.ValueChanged += HandleValueChanged;
  }
  // Protected Overrides Sub OnSubscribeControlEvents(ByVal c As Control)
  // MyBase.OnSubscribeControlEvents(c)
  // AddHandler DirectCast(c, CheckBox).CheckedChanged, AddressOf OnCheckedChanged
  // End Sub

  // Protected Overrides Sub OnUnsubscribeControlEvents(ByVal c As Control)
  // MyBase.OnUnsubscribeControlEvents(c)
  // RemoveHandler DirectCast(c, CheckBox).CheckedChanged, AddressOf OnCheckedChanged
  // End Sub

  public event EventHandler ValueChanged;

  private void HandleValueChanged(object sender, EventArgs e)
  {
   ValueChanged?.Invoke(this, e);
  }
 }
}