Imports System.Runtime.CompilerServices
Imports System.Text

Module ControlExtensions
  ReadOnly WM_SETREDRAW As Integer = 11

  <Extension()>
 Public Sub SuspendPaint(ByVal ctrl As Windows.Forms.Control)

  Dim msgSuspendUpdate As Windows.Forms.Message = Windows.Forms.Message.Create(ctrl.Handle, WM_SETREDRAW, System.IntPtr.Zero, System.IntPtr.Zero)

  Dim window As Windows.Forms.NativeWindow = Windows.Forms.NativeWindow.FromHandle(ctrl.Handle)

  window.DefWndProc(msgSuspendUpdate)

 End Sub

 ''' <summary>
 ''' Resume from SuspendPaint method
 ''' </summary>
 ''' <param name="ctrl"></param>
 ''' <remarks></remarks>
 <Extension()>
 Public Sub ResumePaint(ByVal ctrl As Windows.Forms.Control)

  Dim wparam As New System.IntPtr(1)
  Dim msgResumeUpdate As Windows.Forms.Message = Windows.Forms.Message.Create(ctrl.Handle, WM_SETREDRAW, wparam, System.IntPtr.Zero)

  Dim window As Windows.Forms.NativeWindow = Windows.Forms.NativeWindow.FromHandle(ctrl.Handle)

  window.DefWndProc(msgResumeUpdate)

  ctrl.Refresh()
 End Sub


  <Extension()>
 Public Function Getlkvp(Of TKey, TValue)(sdictionary As SortedDictionary(Of TKey, TValue)) As KeyValuePair(Of TKey, TValue)?
  If sdictionary.Keys.Count = 0 Then
   Return Nothing
  End If
  Dim ct As Integer = sdictionary.Count - 1
  Dim k As TKey = sdictionary.Keys(ct)
  Dim v As TValue = sdictionary(k)
  Return New KeyValuePair(Of TKey, TValue)(k, v)
 End Function


 <Extension()>
 Public Function Getlkey(Of TKey, TValue)(sdictionary As SortedDictionary(Of TKey, TValue)) As TKey
  If sdictionary.Keys.Count = 0 Then
   Return Nothing
  End If
  Dim ct As Integer = sdictionary.Count - 1
  Dim k As TKey = sdictionary.Keys(ct)
  Dim v As TValue = sdictionary(k)
  Return k
 End Function

 <Extension()>
 Public Function Getlval(Of TKey, TValue)(sdictionary As SortedDictionary(Of TKey, TValue)) As TValue
  If sdictionary.Keys.Count = 0 Then
   Return Nothing
  End If
  Dim ct As Integer = sdictionary.Count - 1
  Dim k As TKey = sdictionary.Keys(ct)
  Dim v As TValue = sdictionary(k)
  Return v
 End Function

  <Extension()>
  Public Sub CenterControl(ByVal ctrl As Windows.Forms.Control)
    ctrl.Top = (ctrl.Parent.Height / 2) - (ctrl.Height / 2)
    ctrl.Left = (ctrl.Parent.Width / 2) - (ctrl.Width / 2)
  End Sub
End Module
