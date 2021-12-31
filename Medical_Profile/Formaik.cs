using System;
using System.Collections.Generic;
using System.Windows.Forms;
using global::JWT;
using global::Microsoft.Win32;

namespace Medical_Profile
{
 public partial class Formaik
 {
 private static Enc256.Ienc256 Ede = Enc256.Factory.Getinstance();
  public Formaik(char[] cv)
  {
   base.Load += Formaik_Load;
   keys = new string(cv);
   SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
   InitializeComponent();
  }

  private string keys;
  private string key_encrypted;
  private string installation_url = "https://2xi513e85m.execute-api.us-east-2.amazonaws.com/Dev";

  private static Iaws Aws = Awsstd.Factory.Getinstance();
  private void Done_Click(object sender, EventArgs e)
  {
   Close();
  }

  private void Formaik_Load(object sender, EventArgs e)
  {
  }
  
  private async void Register_Click(object sender, EventArgs e)
  {
   var nw = DateTimeOffset.UtcNow;
   //var ew = nw.AddYears(1);
   var ew = nw.AddMinutes(5);

   IDateTimeProvider provider = new UtcDateTimeProvider();
   string salt;
   string Cid = null;
   string mtyenc;
   Register_Return mcd;
   var aws_body = new Dictionary<string, object>();

   using (RegistryKey Rkey = Registry.CurrentUser.OpenSubKey(@"Software\Medical_Profile", true))
   {
    Cid = Rkey.GetValue("Cid", null).ToString();

    if (Key.Text is object && !((Key.Text ?? "") == (string.Empty ?? "")))
    {
     key_encrypted = Ede.Encrypt(Key.Text, Ede.Scramble(keys));
     salt = Ede.Getsalt(key_encrypted);

     var payload = new Dictionary<string, object>() { { "aud", "http://medicalprofilecard.com" }, { "exp", ew.ToUnixTimeSeconds() }, { "register", key_encrypted } };

     payload["cid"] = Cid;
     payload["User_Name"] = Environment.UserName;
     payload["Machine_Name"] = Environment.MachineName;

     aws_body["vector_code"] = "4152";

     Application.UseWaitCursor = true;
     mcd = await Aws.Register_aysnc(installation_url, keys, salt, payload, aws_body);
     Application.UseWaitCursor = false;
     mtyenc = Ede.Decrypt(mcd.eval, Ede.Scramble(keys), 18926);

     if (mtyenc is object)
     {
      Rkey.SetValue("eval", mtyenc);
     }
    }
   }

   Close();
  }
 }
}