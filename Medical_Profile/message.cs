using System;
using System.Runtime.InteropServices;

namespace Medical_Profile
{
 public partial class Form1
 {
  private const int WM_SETREDRAW = 11;

  [DllImport("user32", EntryPoint = "SendMessageA")]
  private static extern int SendMessage(int hWnd, int wMsg, int wParam, ref int lParam);

  public int Stop_redraw(int target)
  {
   int arglParam = 0;
   int ret = SendMessage(target, WM_SETREDRAW, Convert.ToInt32(false), ref arglParam);
   return ret;
  }

  public int start_redraw(int target)
  {
   int arglParam = 0;
   int ret = SendMessage(target, WM_SETREDRAW, Convert.ToInt32(true), ref arglParam);
   return ret;
  }
 }
}