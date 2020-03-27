using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Medical_Profile
{
 static class ControlExtensions
 {
  private readonly static int WM_SETREDRAW = 11;

  public static void SuspendPaint(this Control ctrl)
  {
   var msgSuspendUpdate = Message.Create(ctrl.Handle, WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
   var window = NativeWindow.FromHandle(ctrl.Handle);
   window.DefWndProc(ref msgSuspendUpdate);
  }

  /// <summary>
 /// Resume from SuspendPaint method
 /// </summary>
 /// <param name="ctrl"></param>
 /// <remarks></remarks>
  public static void ResumePaint(this Control ctrl)
  {
   var wparam = new IntPtr(1);
   var msgResumeUpdate = Message.Create(ctrl.Handle, WM_SETREDRAW, wparam, IntPtr.Zero);
   var window = NativeWindow.FromHandle(ctrl.Handle);
   window.DefWndProc(ref msgResumeUpdate);
   ctrl.Refresh();
  }

  public static KeyValuePair<TKey, TValue>? Getlkvp<TKey, TValue>(this SortedDictionary<TKey, TValue> sdictionary)
  {
   if (sdictionary.Keys.Count == 0)
   {
    return default;
   }

   int ct = sdictionary.Count - 1;
   var k = sdictionary.Keys.ElementAtOrDefault(ct);
   var v = sdictionary[k];
   return new KeyValuePair<TKey, TValue>(k, v);
  }

  public static TKey Getlkey<TKey, TValue>(this SortedDictionary<TKey, TValue> sdictionary)
  {
   if (sdictionary.Keys.Count == 0)
   {
    return default;
   }

   int ct = sdictionary.Count - 1;
   var k = sdictionary.Keys.ElementAtOrDefault(ct);
   var v = sdictionary[k];
   return k;
  }

  public static TValue Getlval<TKey, TValue>(this SortedDictionary<TKey, TValue> sdictionary)
  {
   if (sdictionary.Keys.Count == 0)
   {
    return default;
   }

   int ct = sdictionary.Count - 1;
   var k = sdictionary.Keys.ElementAtOrDefault(ct);
   var v = sdictionary[k];
   return v;
  }

  public static void CenterControl(this Control ctrl)
  {
   ctrl.Top = (int)(ctrl.Parent.Height / (double)2 - ctrl.Height / (double)2);
   ctrl.Left = (int)(ctrl.Parent.Width / (double)2 - ctrl.Width / (double)2);
  }
 }
}