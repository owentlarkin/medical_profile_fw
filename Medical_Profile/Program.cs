using Enc;
using JR.Utils.GUI.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dymodkl;

namespace Medical_Profile
{
 public static class Program
 {
  public static Form1 Mpf { get; set; }

  [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
  [STAThread]

  static void Main()
  {
   AppDomain currentDomain = AppDomain.CurrentDomain;

   Application.EnableVisualStyles();
   Application.SetCompatibleTextRenderingDefault(false);

   Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

   currentDomain.UnhandledException += new UnhandledExceptionEventHandler(UHException);

   Application.ThreadException += new ThreadExceptionEventHandler(UIException);

   Mpf = new Form1();

   //Dds.Init();

   Application.Run(Mpf);
  }

  public static async void UIException(object sender, ThreadExceptionEventArgs t)
  {
   try
   {
    Exception E = (Exception)t.Exception;
    string S = Format_exception(E, "Unhandled UI Exception");

    if (Mpf != null && Mpf.Mpck != null)
     await Write_exception(S);

    FlexibleMessageBox.FONT = new Font("Calibri", 10, System.Drawing.FontStyle.Bold);
    FlexibleMessageBox.Show(S, "Exception Occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
   }
   catch
   {
     MessageBox.Show("Fatal Windows Forms Error",
         "Fatal Windows Forms Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);    
   }
   finally
   {
    Application.Exit();
   }
  }

  static async void UHException(object sender, UnhandledExceptionEventArgs ue)
  {
   try
   {
    Exception E = (Exception)ue.ExceptionObject;

    string S = Format_exception(E, "Unhandled Exception");

    if (Mpf != null && Mpf.Mpck != null)
     await Write_exception(S);

    FlexibleMessageBox.FONT = new Font("Calibri", 10, System.Drawing.FontStyle.Bold);
    FlexibleMessageBox.Show(S, "Exception Occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
   }
   catch
   {
    MessageBox.Show("Fatal Windows Forms Error",
        "Fatal Windows Forms Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
   }
   finally
   {
    Application.Exit();
   }
  }

  public static string Format_exception(Exception e, string header = "Error obtaining data")
  {
   var erm = new StringBuilder();
   erm.Append(header);
   erm.Append("\n");


   if (e is AggregateException)
   {
    AggregateException ae = e as AggregateException;
    erm.Append("One or more errors have occured in a background process." + "\n");
    foreach (var e1 in ae.Flatten().InnerExceptions)
    {
     erm.Append("\n");
     erm.Append(e1.Message);
     if (e1.StackTrace is object)
     {
      erm.Append("\n" + "Stack Trace:" + "\n");
      erm.Append(e1.StackTrace);
     }
    }
   }
   else
   {
    erm.Append(e.Message);
    if (e.StackTrace is object)
    {
     erm.Append("\n" + "Stack Trace:" + "\n");
     erm.Append(e.StackTrace);
    }
   }

   return erm.ToString();
  }

  static async Task<int> Write_exception(string Ex, bool show = false)
  {
   Dictionary<string, object> aws_body = new Dictionary<string, object>();
   string Wrtim = DateTime.Now.ToString("yyyyMMddHHmmss");
   var claims = Mpf.Gen_Claims();
   Dsave_return Dr;
   aws_body.Clear();
   aws_body["ukey"] = Enc256.Encrypt(Mpf.cid + Mpf.Mpck.Dlab, Mpf.cid + Mpf.Mpck.Dlab, Mpf.Mpck.Iterations);
   aws_body["wrtim"] = Wrtim;
   aws_body["exception"] = Ex;
   Application.UseWaitCursor = true;
   Dr = await Aws.Save_exception(Mpf.Mpck.Url, Mpf.enck, Mpf.Mpck.Salt, claims, aws_body);
   Application.UseWaitCursor = false;
   if (show)
   {
    FlexibleMessageBox.Show(Ex, "Unhandled Exception");
   }

   return Dr.code;
  }

 }
}
