using System;

namespace Medical_Profile.My
{
 // The following events are available for MyApplication:
 // Startup: Raised when the application starts, before the startup form is created.
 // Shutdown: Raised after all application forms are closed.  This event is not raised if the application terminates abnormally.
 // UnhandledException: Raised if the application encounters an unhandled exception.
 // StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
 // NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
 internal partial class MyApplication
 {
  private async void MyApplication_UnhandledException(object sender, Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs e)
  {
   string Header = "Unhandled Exception";
   string s = MyProject.Forms.Form1.Fe(e.Exception, Header);
   int rc;
   rc = await MyProject.Forms.Form1.Write_exception(s, true);
   Environment.Exit(0);
  }
 }
}