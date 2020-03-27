using System;
using global::System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Medical_Profile
{
 public static class Extensions
 {
  [DllImport("user32.dll")]
  private static extern int SendMessage(IntPtr hWnd, int Msg, bool wParam, IntPtr lParam);

  private const int WM_SETREDRAW = 11;

  // Extension methods for Control
  public static void ResumeDrawing(this Control Target, bool Redraw)
  {
   SendMessage(Target.Handle, WM_SETREDRAW, true, (IntPtr)0);
   if (Redraw)
   {
    Target.Refresh();
   }
  }

  public static void SuspendDrawing(this Control Target)
  {
   SendMessage(Target.Handle, WM_SETREDRAW, false, (IntPtr)0);
  }

  public static void ResumeDrawing(this Control Target)
  {
   Target.ResumeDrawing(true);
  }
 }
}