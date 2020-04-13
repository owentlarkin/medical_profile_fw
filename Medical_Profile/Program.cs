using System;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Medical_Profile
{
 static class Program
 {
  public static Form1 Mpf { get; set; }
  [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
  [STAThread]
  static void Main()
  {
   AppDomain currentDomain = AppDomain.CurrentDomain;
   currentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
   Application.EnableVisualStyles();
   Application.SetCompatibleTextRenderingDefault(false);

   //Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
   //AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

   Application.Run(new Form1());
  }

  static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
  {
   MessageBox.Show((e.ExceptionObject as Exception).Message, "Unhandled UI Exception");
  }
 }
}
