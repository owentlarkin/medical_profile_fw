using System;
using System.Collections.Generic;
using global::System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using global::System.IO;
using System.Linq;
// Imports JR.Utils.GUI.Forms
// Imports Medical_Profile.JR.Utils.GUI.Forms
// Imports System.ServiceModel
// Imports System.Windows.Threading
using global::System.Reflection.Emit;
using global::System.Text.RegularExpressions;
using System.Windows.Forms;
using global::Amazon;
using global::Amazon.DynamoDBv2;
using global::Amazon.DynamoDBv2.DocumentModel;
using global::DYMO.Label.Framework;
using global::JWT;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using global::Microsoft.Win32;
using global::Newtonsoft.Json;

namespace Medical_Profile
{
 public partial class Form1:Form
 {
  private System.Collections.Specialized.StringCollection Endpoints_in_use = new System.Collections.Specialized.StringCollection();
  private string drive_label = null;
  private string usb_file = null;
  private static string cid = null;
  private string ftime = null;
  private object pngdata;
  private object iLock = new object();
  private string line;
  private string Folder = "";
  private bool icalled = false;
  private bool dnh = true;
  private char[] cv = new char[17];
  private int Wanted_hgt = 0;
  private object o1 = null;
  private int margin1 = 6;
  private int margin2 = 18;
  private int margin3 = 10;
  private int label_header = 345;
  private float ll = (float)(3600 / 15.0);
  private int nlead = 191;
  private int ylimit = 2386;

  private delegate void cb();

  private delegate void InvokeDelegate();

  private bool Preview = false;
  private bool Develop = false;
  private static MPC_key Mpck = null;
  private System.Timers.Timer _timer;

  private enum Load_state
  {
   not_loaded,
   loaded,
   not_loaded_byc,
   loaded_byc,
   not_in_use
  }

  private enum Data_state
  {
   Edit_mode,
   NoEdit,
   Loaded_by_id,
   Loaded_by_name
  }

  private Data_state Dstate = Data_state.NoEdit;

  private Form1()
  {
   string fnam = null;
   string fs = null;
   try
   {
    SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
    InitializeComponent();
    dept_tbox.Visible = false;
    Visible = false;
    var myScreen = Screen.FromControl(this);
    var area = myScreen.WorkingArea;
    Screen_width = area.Size.Width;
    Screen_height = area.Size.Height;
    cv[5] = (char)(Strings.Asc(title[19]) + 16);
    cv[6] = (char)Strings.Asc(title[9]);
    cv[7] = (char)Strings.Asc(title[33]);
    cv[8] = (char)(Strings.Asc(title[13]) - 8);
    cv[10] = (char)Strings.Asc(title[1]);
    cv[11] = (char)Strings.Asc(title[36]);
    if (My.MySettingsProperty.Settings.first_run_flag)
    {
     first_run = true;
     My.MySettingsProperty.Settings.first_run_flag = false;
     My.MySettingsProperty.Settings.Save();
    }
    else
    {
     first_run = false;
    }

    cv[12] = (char)Strings.Asc(title[31]);
    cv[13] = (char)(Strings.Asc(title[9]) + 10);
    cv[15] = (char)(Strings.Asc(title[33]) + 5);
    cv[3] = (char)(Strings.Asc(title[3]) + 6);
    cv[1] = (char)Strings.Asc(title[10]);
    cv[2] = (char)Strings.Asc(title[18]);
    if (first_run)
    {
     var pi1 = new ProcessStartInfo()
     {
      FileName = "cscript.exe",
      Arguments = "pin_start.vbs",
      WindowStyle = ProcessWindowStyle.Hidden
     };
     var p1 = Process.Start(pi1);
     p1.WaitForExit();
     var pi2 = new ProcessStartInfo()
     {
      FileName = "cscript.exe",
      Arguments = "pin_start_cm.vbs",
      WindowStyle = ProcessWindowStyle.Hidden
     };
     var p2 = Process.Start(pi2);
     p2.WaitForExit();
     My.MySettingsProperty.Settings.Save();
     Application.Exit();
    }
   }
   catch (Exception ex)
   {
    MessageBox.Show(ex.Message);
   }

   cv[9] = (char)(Strings.Asc(title[0]) + 32);
   cv[16] = (char)(Strings.Asc(title[26]) - 48);
   cv[0] = (char)(Strings.Asc(title[2]) - 32);
   cv[4] = (char)(Strings.Asc(title[5]) - 64);
   cv[14] = (char)(Strings.Asc(title[23]) + 16);
   var key = Registry.CurrentUser.OpenSubKey(@"Software\Medical_Profile", true);
   if (key is object)
   {
    cid = Conversions.ToString(key.GetValue("Cid", null));
    if (cid == default)
    {
     cid = Guid.NewGuid().ToString();
     key.SetValue("Cid", cid);
    }
   }
  }

  private string Handle_file(string eval)
  {
   MPC_type mty = null;
   string rjson = Enc.Enc256.Decrypt(eval, enck, 17531);
   if (rjson == default)
   {
    return Conversions.ToString(1);
   }

   mty = JsonConvert.DeserializeObject<MPC_type>(rjson);
   if (mty.Akey != default)
   {
    drive_label = mty.Akey;
   }

   if (mty.F1 != default)
   {
    rjson = Enc.Enc256.Decrypt(mty.F1, enck, mty.Akey);
    if (rjson is null)
    {
     MessageBox.Show("Key file is incorrect.", "Error", MessageBoxButtons.OK);
     Application.Exit();
    }

    Mpck = JsonConvert.DeserializeObject<MPC_key>(rjson);
   }

   return Conversions.ToString(0);
  }

  private string Set_key(string s1, string s2)
  {
   int v1 = 0;
   for (int i = 3, loopTo = s2.Length - 1; i <= loopTo; i++)
    v1 += Convert.ToInt32(s2.Substring(i, 1));
   int insert_loc = v1 % (s2.Length - 4);
   return s1.Substring(0, insert_loc) + s2 + s1.Substring(insert_loc);
  }

 
  private int Handle_testmode(string un)
  {
   int n = default;
   MPC_User mpu;
   string ds;
   Document item;
   Table table;
   using (var client = new AmazonDynamoDBClient(RegionEndpoint.USEast2))
   {
    table = Table.LoadTable(client, "mpc_users");
    item = table.GetItem(Convert.ToInt32(un));
    ds = item.ToJsonPretty();
    mpu = JsonConvert.DeserializeObject<MPC_User>(ds);
   }

   Mpck = new MPC_key();
   {
    var withBlock = Mpck;
    withBlock.Url = mpu.Url;
    withBlock.Mkey = mpu.Mkey;
    withBlock.Salt = mpu.Salt;
    withBlock.Email = mpu.Email;
    withBlock.Dlab = mpu.Disk_Label;
    withBlock.Secret = mpu.Secret1;
    withBlock.Iterations = mpu.Iterations;
    withBlock.Blocks = mpu.Blocks;
    withBlock.Blocklist = mpu.Blocklist;
    withBlock.K1 = mpu.K1;
    withBlock.Labels = mpu.Labels;
    withBlock.Lines = mpu.Lines;
    withBlock.Minimum_blocks = mpu.Minimum_blocks;
    withBlock.Points = mpu.Points;
    withBlock.Sec_visible = Conversions.ToBoolean(mpu.Sec_visible);
    withBlock.Sptitle = mpu.Sptitle;
    withBlock.Version = mpu.Version;
   }

   cid = mpu.cid;
   drive_label = mpu.Disk_Label;
   file_access = true;
   return 0;
  }
  
  private int Handle_usb(string dle)
  {
   int n = default;
   string fs;
   string it;
   string d1 = null;
   var drivesrem = DriveInfo.GetDrives().Where(drive => drive.DriveType == DriveType.Removable).ToArray();
   foreach (DriveInfo d in drivesrem)
   {
    if (d.VolumeLabel.StartsWith("MPC"))
    {
     d1 = Enc.Enc256.Decrypt(dle, enck, d.VolumeLabel);
     if (d1 is object && (d1 ?? "") == (d.VolumeLabel ?? ""))
     {
      it = d.VolumeLabel.Substring(3);
      USB_Fname = d.Name + "F" + it + ".fil";
      drive_label = d.VolumeLabel;
     }
    }
   }

   if (USB_Fname is object)
   {
    if (File.Exists(USB_Fname))
    {
     try
     {
      USB_Ftime = File.GetLastWriteTimeUtc(USB_Fname);
      using (var sr = new StreamReader(USB_Fname))
      {
       fs = sr.ReadToEnd();
       string rjson = Enc.Enc256.Decrypt(fs, enck, drive_label);
       if (rjson is null)
       {
        MessageBox.Show("Key file is incorrect.", "Error", MessageBoxButtons.OK);
        Application.Exit();
       }

       Mpck = JsonConvert.DeserializeObject<MPC_key>(rjson);
      }
     }
     catch (Exception ex)
     {
      Console.WriteLine("Key file could not be read:");
      Console.WriteLine(ex.Message);
      return 1;
     }
    }
    else
    {
     MessageBox.Show("Unable to find the key file.", "Error", MessageBoxButtons.OK);
     return 1;
    }
   }
   else
   {
    MessageBox.Show("Unable to find the usb key drive.", "Error", MessageBoxButtons.OK);
    return 1;
   }

   return 0;
  }

  private float bl;
  private TextObject tb;
  private int bn;

  private int New_label(ref DieCutLabel lbl, ref int ln)
  {
   float nlen;
   float rlen;
   lbl = (DieCutLabel)Framework.Open("mpcb.label");
   tb = (TextObject)lbl.GetObjectByName("labtext");
   lbl.DeleteObject(tb);
   tb = (TextObject)lbl.GetObjectByName("header");
   stb = new StyledTextBuilder();
   htb = new StyledTextBuilder();
   if (!(string.Compare(DOB.Text, "") == 0))
   {
    stb.Append("DOB: ", lfnt, Colors.Black);
    stb.Append(DOB.Text, lfnt, Colors.Black);
    stb.Append(" ", lfnt, Colors.Black);
   }

   rlen = Wlengb(stb.StyledText.Text, 9);
   nlen = Wlengb(Patient.Text + "    ", 9);
   bl = Wlengb("", 9);
   bn = (int)((192.375 - (rlen + nlen - bl)) / bl);
   htb.Append(Patient.Text + "     ", lfnt, Colors.Black);
   htb.Append(Strings.StrDup(bn, " "), lfnt, Colors.Black);
   htb.Append(stb.StyledText.Text, lfnt, Colors.Black);
   tb.StyledText = htb.StyledText;

   // ltl = 1

   ln += 1;
   return label_header;
  }

  private void Output_label(ref PrintJob pjob, ref DieCutLabel label, int lno, StyledTextBuilder sb = null)
  {
   if (sb is object)
   {
    TextObject tb = (TextObject)label.GetObjectByName("labtext");
    tb.StyledText = sb.StyledText;
   }

   if (Preview)
   {
    Pnglablist.Add(Render(label));
    string lns = "Label" + lno.ToString();
    label.SaveToFile(lns + ".label");
   }
   else
   {
    pjob.AddLabel(label);
   }

   sb = null;
   label = null;
  }

  private bool Check_lines()
  {
   int lc = 0;
   string[] li;
   string lis;
   var lin = new List<string>();
   int lno = 0;
   int yl = 0;
   int lin_count = 0;
   int y_space_needed = 0;
   var elines = new Dictionary<int, int>();
   yl = label_header;
   foreach (Control cb in Controls)
   {
    if (!cb.Name.StartsWith("GB"))
    {
     continue;
    }

    if (string.IsNullOrEmpty(cb.Controls[0].Text) | string.IsNullOrEmpty(cb.Controls[1].Text))
    {
     continue;
    }

    int bn = (from kvp in gpn
              where (kvp.Value ?? "") == (cb.Name ?? "")
              select kvp).First().Key;
    y_space_needed = nlead * 2;
    if (currenty > label_header)
    {
     y_space_needed += nlead /2;
    }

    if (ylimit - currenty < y_space_needed)
    {
     lno += 1;
     yl = label_header;
    }

    if (currenty > label_header)
    {
     yl += nlead / 2;
    }

    yl += nlead;
    lis = cb.Controls[1].Text;
    elines.Clear();
    if ((lines_setting ?? "") == "Label")
    {
     lis = this.Adjust_lines(lis, "File");
    }

    lis = Adjust_lines(lis, "Label", elines);
    li = Regex.Split(lis, @"\r\n|\n");
    int lic = li.Count();
    if (max_rec[bn] > 0 & lic > max_rec[bn])
    {
     lic = max_rec[bn];
     int iadj = 0;
     foreach (KeyValuePair<int, int> kvp in elines)
     {
      if (kvp.Key <= lic)
      {
       iadj = iadj + kvp.Value;
      }
     }

     lic = lic + iadj;
    }

    for (int i = 0, loopTo = li.Count() - 1; i <= loopTo; i += 1)
    {
     yl += nlead;
     lin_count += 1;
     if (ylimit - yl < nlead)
     {
      lno += 1;
      yl = label_header;
      if (lno > labels_number)
      {
       break;
      }
     }
    }
   }

   if (lin_count > total_lines)
   {
    DialogResult Response;
    Response = MessageBox.Show((lin_count - total_lines).ToString() + " lines will not fit on the labels", "Continue With Print", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
    if (Response == DialogResult.Cancel)
    {
     return false;
    }
    else
    {
     return true;
    }
   }
   else
   {
    return true;
   }
  }

  private void Reset_labels()
  {
   var lmni = new List<ToolStripMenuItem>();
   f3s.Clear();
   lmni.Clear();
   foreach (ToolStripMenuItem ts in MenuStrip1.Items)
   {
    if (ts.Name.StartsWith("Label"))
    {
     lmni.Add(ts);
    }
   }

   foreach (ToolStripMenuItem ts in lmni)
    MenuStrip1.Items.Remove(ts);
  }

  private int Lab_field(string Name, StyledTextBuilder val, float x, int y, float w, int h)
  {
   TextObject Tb;
   Label.AddObject(new TextObject(Name), new Rect(x, y / 15.0, w, h / 15.0));
   Tb = (TextObject)Label.GetObjectByName(Name);
   Tb.StyledText = val.StyledText;
   return y + h;
  }

  private int Lab_field(string Name, StyledTextBuilder val, int x, int y, float w, int h)
  {
   TextObject Tb;
   Label.AddObject(new TextObject(Name), new Rect(x / 15.0, y / 15.0, w, h / 15.0));
   Tb = (TextObject)Label.GetObjectByName(Name);
   Tb.StyledText = val.StyledText;
   return y + h;
  }

  private int Lab_field(string Name, string val, float x, int y, float w, int h)
  {
   TextObject Tb;
   Label.AddObject(new TextObject(Name), new Rect(x, y / 15.0, w, h / 15.0));
   Tb = (TextObject)Label.GetObjectByName(Name);
   Tb.Text = val;
   return y + h;
  }

  private int Lab_field(string Name, string val, int x, int y, float w, int h)
  {
   TextObject Tb;
   Label.AddObject(new TextObject(Name), new Rect(x / 15.0, y / 15.0, w, h / 15.0));
   Tb = (TextObject)Label.GetObjectByName(Name);
   Tb.Text = val;
   return y + h;
  }

  private void Generate_Labels()
  {
   string P1 = @"(\(??\d\d\d\)??[\s|-]\d\d\d-\d\d\d\d)";
   string P2 = @"(\d\d\d-\d\d\d\d)";
   Match M1;
   string[] li = null;
   string lis;
   var yadj = default(int);
   var stb = new StyledTextBuilder();
   var stx = new StyledTextBuilder();
   float flen;
   float nlen;
   float dobst;
   float lpht;
   int cy_save;
   int y_space_needed;
   TextObject tb;
   string Phone_number;
   var elines = new Dictionary<int, int>();
   Reset_labels();
   if (!Check_lines())
   {
    return;
   }

   labelno = 0;
   Pnglablist.Clear();
   if (printer is ILabelWriterPrinter)
   {
    pjob = printer.CreatePrintJob(lprintparams);
   }
   else
   {
    pjob = printer.CreatePrintJob(printParams);
   }

   pname = Patient.Text;
   pdob = DOB.Text;
   Label = (IDieCutLabel)Framework.Open("mpc1.label");
   tb = (TextObject)Label.GetObjectByName("name");
   Label.DeleteObject(tb);
   tb = (TextObject)Label.GetObjectByName("labtext");
   Label.DeleteObject(tb);
   stb = new StyledTextBuilder();
   flen = Wlengb("DOB: ", 12);
   stb.Append("DOB: ", nfnt, Colors.Black);
   stb.Append(DOB.Text, nfnt, Colors.Black);
   flen = flen + Wleng(DOB.Text, 12);
   dobst = (float)(3900.0 / 15.0 - flen);
   Label.AddObject(new TextObject("dob"), new Rect(dobst, 480 / 15.0, flen, 210 / 15.0));
   tb = (TextObject)Label.GetObjectByName("dob");
   tb.HorizontalAlignment = TextAlignment.Right;
   tb.StyledText = stb.StyledText;
   nlen = (float)(3900.0 / 15.0 - flen - 317 / 15.0);
   name_length = Wlengb(pname, 12);
   stb = new StyledTextBuilder();
   stb.Append(pname, nfnt, Colors.Black);
   currenty = Lab_field("name", stb, 317, 480, nlen, 210);
   if (address.Lines.Count() > 0)
   {
    stb = new StyledTextBuilder();
    ltline = 0;
    li = address.Lines;
    for (int i = 0, loopTo = li.Count() - 1; i <= loopTo; i++)
    {
     if (!((li[i] ?? "") == (string.Empty ?? "")))
     {
      if (i > 0)
      {
       stb.Append(Constants.vbCrLf, reg_font, Colors.Black);
      }

      stb.Append(li[i], reg_font, Colors.Black);
      ltline += 1;
     }
    }

    currenty = Lab_field("address", stb, 317, currenty, ll, ltline * 191);
   }

   if (!((Phone.Text ?? "") == (string.Empty ?? "")))
   {
    stb = new StyledTextBuilder();
    stb.Append("Phone: ", bld_font, Colors.Black);
    stb.Append(Phone.Text, reg_font, Colors.Black);
    currenty = Lab_field("phone", stb, 317, currenty, ll, 191);
   }

   if (!((econtact.Text ?? "") == (string.Empty ?? "")))
   {
    stb = new StyledTextBuilder();
    stb.Append("Emergency Contact: ", bld_font, Colors.Black);
    stb.Append(econtact.Text, reg_font, Colors.Black);
    currenty = Lab_field("econtact", stb, 317, currenty, ll, 191);
   }

   string dname = priph.Text;
   if (prv_combo.Items.Count > 0)
   {
    dname = prv_combo.Text;
   }

   if (!((dname ?? "") == (string.Empty ?? "")))
   {
    stb = new StyledTextBuilder();
    stb.Append(lab1["priph_title:"], bp1_font, Colors.Black);
    string s1 = lab1["priph_title:"];
    if (!lab1["priph_title:"].EndsWith(":"))
    {
     stb.Append(":", bp1_font, Colors.Black);
     s1 = s1 + ":";
    }

    lpht = (float)(Wlengb(s1, 9) * 1.33);
    cy_save = currenty;
    int pl = 230;
    yadj = 30;
    currenty = Lab_field("priphyst", stb, 317, currenty + yadj, lpht, pl);
    lpht = (float)(lpht + 317 / 15.0);
    Phone_number = string.Empty;
    M1 = Regex.Match(dname, P1);
    if (M1.Success)
    {
     Phone_number = M1.Value;
     dname = dname.Replace(Phone_number, string.Empty);
    }

    if (!M1.Success)
    {
     M1 = Regex.Match(dname, P2);
     if (M1.Success)
     {
      Phone_number = M1.Value;
      dname = dname.Replace(Phone_number, string.Empty);
     }
    }

    stb = new StyledTextBuilder();
    stb.Append(dname, rp1_font, Colors.Black);
    if (!string.IsNullOrEmpty(Phone_number))
    {
     pl = 2 * pl;
     stb.Append(Constants.vbCrLf, rp1_font, Colors.Black);
     stb.Append("  " + Phone_number, rp1_font, Colors.Black);
    }

    nlen = (float)(3900 / 15.0 - lpht);
    currenty = Lab_field("priphys", stb, lpht, cy_save + yadj, nlen, pl);
   }

   if (lab1.ContainsKey("secph:"))
   {
    stb = new StyledTextBuilder();
    stb.Append(lab1["secph_title:"], bld_font, Colors.Black);
    if (!lab1["secph_title:"].EndsWith(":"))
    {
     stb.Append(": ", bld_font, Colors.Black);
    }

    stb.Append(secph.Text, reg_font, Colors.Black);
    currenty = Lab_field("spec1", stb, 317, currenty + yadj, ll, 191);
    yadj = 0;
   }

   if (!((ins.Text ?? "") == (string.Empty ?? "")))
   {
    stb = new StyledTextBuilder();
    stb.Append("Insurance: ", bld_font, Colors.Black);
    stb.Append(ins.Text, reg_font, Colors.Black);
    currenty = Lab_field("insurance", stb, 317, currenty + yadj, ll, 191);
    yadj = 0;
   }

   PrintJob argpjob = (PrintJob)pjob;
   DieCutLabel arglabel = (DieCutLabel)Label;
   Output_label(ref argpjob, ref arglabel, labelno);
   DieCutLabel arglbl = (DieCutLabel)Label;
   currenty = New_label(ref arglbl, ref labelno);

   // stb = New StyledTextBuilder

   foreach (Control cb in Controls)
   {
    if (!cb.Name.StartsWith("GB"))
    {
     continue;
    }

    // If String.IsNullOrEmpty(cb.Controls(0).Text) Or String.IsNullOrEmpty(cb.Controls(1).Text) Then
    if (string.IsNullOrEmpty(cb.Controls[1].Text))
    {
     continue;
    }

    int bn = (from kvp in gpn
              where (kvp.Value ?? "") == (cb.Name ?? "")
              select kvp).First().Key;
    if (Label is null)
    {
     DieCutLabel arglbl1 = (DieCutLabel)Label;
     currenty = New_label(ref arglbl1, ref labelno);
    }

    if (labelno > labels_number)
    {
     break;
    }

    if (string.IsNullOrEmpty(cb.Controls[0].Text))
    {
     y_space_needed = nlead;
    }
    else
    {
     y_space_needed = nlead * 2;
    }

    if (currenty > label_header & !string.IsNullOrEmpty(cb.Controls[0].Text))
    {
     y_space_needed += nlead / 2;
    }

    if (ylimit - currenty < y_space_needed)
    {
     PrintJob argpjob1 = (PrintJob)pjob;
     DieCutLabel arglabel1 = (DieCutLabel)Label;
     Output_label(ref argpjob1, ref arglabel1, labelno);
     DieCutLabel arglbl2 = (DieCutLabel)Label;
     currenty = New_label(ref arglbl2, ref labelno);
     if (labelno > labels_number)
     {
      break;
     }
    }

    if (currenty > label_header & !string.IsNullOrEmpty(cb.Controls[0].Text))
    {
     currenty += nlead / 2;
    }

    stb = new StyledTextBuilder();
    if (!string.IsNullOrEmpty(cb.Controls[0].Text))
    {
     stb.Append(cb.Controls[0].Text, bld_font, Colors.Black);
     currenty = Lab_field("L" + currenty.ToString(), stb, 317, currenty, ll, nlead);
    }
    else
    {
     currenty += nlead;
    }

    lis = cb.Controls[1].Text;
    elines.Clear();
    if ((lines_setting ?? "") == "Label")
    {
     lis = this.Adjust_lines(lis, "File");
    }

    lis = Adjust_lines(lis, "Label", elines);
    li = Regex.Split(lis, @"\r\n|\n");
    int lic = li.Count();
    if (max_rec[bn] > 0 & lic > max_rec[bn])
    {
     lic = max_rec[bn];
     int iadj = 0;
     foreach (KeyValuePair<int, int> kvp in elines)
     {
      if (kvp.Key <= lic)
      {
       iadj = iadj + kvp.Value;
      }
     }

     lic = lic + iadj;
    }

    for (int i = 0, loopTo1 = li.Count() - 1; i <= loopTo1; i += 1)
    {
     if (string.IsNullOrEmpty(li[i]))
     {
      currenty += nlead;
     }
     else
     {
      stb = new StyledTextBuilder();
      stb.Append(li[i], reg_font, Colors.Black);
      currenty = Lab_field("L" + currenty.ToString(), stb, 317, currenty, ll, nlead);
     }

     if (ylimit - currenty < nlead)
     {
      PrintJob argpjob2 = (PrintJob)pjob;
      DieCutLabel arglabel2 = (DieCutLabel)Label;
      Output_label(ref argpjob2, ref arglabel2, labelno);
      DieCutLabel arglbl3 = (DieCutLabel)Label;
      currenty = New_label(ref arglbl3, ref labelno);
      if (labelno > labels_number)
      {
       break;
      }
     }
    }
   }

   if (currenty > label_header)
   {
    PrintJob argpjob3 = (PrintJob)pjob;
    DieCutLabel arglabel3 = (DieCutLabel)Label;
    Output_label(ref argpjob3, ref arglabel3, labelno);
   }

   if (!Preview)
   {
    pjob.Print();
   }
   else
   {
    pjob = null;
   }
  }

  private int Setcy(GroupBox gb)
  {
   int br = gb.Location.Y + gb.Height + 76;
   int cy = currenty;
   if (br > cy)
   {
    cy = br;
   }

   return cy;
  }

  private void Setupdn(ref ContextMenuStrip cm, int bn, int ni)
  {
   var ctb = new TSnumud();
   Ath_block a1 = null;
   var tsi = new ToolStripMenuItem();
   var tss = new ToolStripSeparator();
   var tsl = new ToolStripLabel("Lines to print");
   tsi.Text = "Activate max lines";
   tsi.Name = bn.ToString();
   tsi.Tag = ni + 3;
   tsi.Click += Cud_activate;
   cm.Items.Add(tsi);
   cm.Items.Add(tss);
   cm.Items.Add(tsl);
   ctb.Name = bn.ToString();
   ctb.Size = new Size(100, 25);
   ctb.udl = ni + 3;
   ctb.aml = ni;
   a1 = ath_blist.Find(p => p.num == bn);
   if (a1 is object)
   {
    max_rec[a1.num] = a1.max_lines;
    ctb.Value = a1.max_lines;
   }

   cm.Items.Add(ctb);
   ctb.ValueChanged += Cudchange;
  }

  private async void Form1_LoadAsync(object sender, EventArgs e)
  {
   int xsize = 365;
   int ysize = 192;
   int tboxsz = 20;
   int blockno = 1;
   int i;
   int k;
   string fs = null;
   ToolStripMenuItem tsi;
   ToolStripMenuItem ins;
   ToolStripMenuItem swp;
   ContextMenuStrip cm;
   GroupBox obox;
   object o1 = null;
   string fnam = null;
   Ath_block abp = null;
   string l1cfn = null;
   loading = true;
   if (run_timer)
   {
    Tgp.Visible = true;
   }

   Visible = false;
   DoubleBuffered = true;
   file_access = false;
   enck = new string(cv);
   var key = Registry.CurrentUser.OpenSubKey(@"Software\Medical_Profile");
   drive_label_encoded = null;
   eval_encoded = null;
   fnam = null;
   ath_blist.Clear();
   installed_version = Conversions.ToString(key.GetValue("version", null));


   /* TODO ERROR: Skipped IfDirectiveTrivia */
   if (!((My.MySettingsProperty.Settings.User_number ?? "") == (string.Empty ?? "")))
   {
    testmode = true;
    Handle_testmode(My.MySettingsProperty.Settings.User_number);
   }
   /* TODO ERROR: Skipped EndIfDirectiveTrivia */
   if (!testmode)
   {
    eval_encoded = Conversions.ToString(key.GetValue("eval", null));
    if (eval_encoded is object)
    {
     file_access = true;
    }

    if (!file_access)
    {
     drive_label_encoded = Conversions.ToString(key.GetValue("Label", null));
     if (drive_label_encoded is null)
     {
      using (Form f4 = new Formaik(cv))
      {
       f4.ShowDialog();
       eval_encoded = Conversions.ToString(key.GetValue("eval", null));
       if (eval_encoded != default)
       {
        file_access = true;
       }
      }

      if (!file_access)
      {
       Interaction.MsgBox("Application can not run", MsgBoxStyle.OkOnly, "Registry entry is missing");
       Close();
       Environment.Exit(0);
      }
     }
    }

    if (file_access)
    {
     if (Conversions.ToDouble(Handle_file(eval_encoded)) != 0)
     {
      Application.Exit();
      Environment.Exit(0);
     }
    }
    else if (Handle_usb(drive_label_encoded) != 0)
    {
     Application.Exit();
     Environment.Exit(0);
    }
   }

   if (My.MySettingsProperty.Settings.endpoints is null)
   {
    My.MySettingsProperty.Settings.endpoints = new System.Collections.Specialized.StringCollection();
   }

   if (My.MySettingsProperty.Settings.endpoints.Count == 0)
   {
    string json = JsonConvert.SerializeObject(Mpck, Formatting.Indented);
    foreach (string s in Mpck.Blocklist)
     My.MySettingsProperty.Settings.endpoints.Add(s);
    My.MySettingsProperty.Settings.Save();
   }

   foreach (string s in My.MySettingsProperty.Settings.endpoints)
    Endpoints_in_use.Add(s);
   secph.Text = "";
   sp.Text = "";
   if (string.IsNullOrEmpty(installed_version))
   {
    installed_version = Mpck.Version;
   }

   if (Mpck.Sec_visible == Conversions.ToBoolean("True"))
   {
    sp.Visible = true;
    if (!string.IsNullOrEmpty(Mpck.Sptitle))
    {
     sp.Text = Mpck.Sptitle;
    }
    else
    {
     sp.Text = "Specialist";
    }

    secph.Visible = true;
    secph.Enabled = true;
    secph.BackColor = System.Drawing.Color.White;
   }
   else
   {
    sp.Visible = false;
    secph.Visible = false;
    secph.Enabled = false;
   }

   minimum_blocks = Conversions.ToInteger(Mpck.Minimum_blocks);
   blocks_number = Conversions.ToInteger(Mpck.Blocks);
   blocks_number = 9;
   labels_number = Conversions.ToInteger(Mpck.Labels);
   lines_number = Conversions.ToInteger(Mpck.Lines);
   lines_number = 10;
   labels_number = 5;
   total_lines = lines_number * labels_number;
   Patientid.Text = "";
   int ystart = GroupBox3.Location.Y + GroupBox3.Size.Height + margin2;
   originaly = Size.Height - 36;
   //base.SuspendPaint();
   var loopTo = 3 * ystart + ysize + margin1;
   for (i = ystart; i <= loopTo; i += ysize + margin1)
   {
    var loopTo1 = margin2 + 3 * xsize + 2 * margin2;
    for (k = margin2; k <= loopTo1; k += xsize + margin2)
    {
     var bi = new Blk_info() { num = blockno };
     blocks[blockno] = bi;
     max_rec[blockno] = 0;
     fieldsmr["MR" + blockno.ToString() + ":"] = blockno;
     obox = new GroupBox()
     {
      Size = new Size(xsize, ysize),
      Location = new Point(k, i),
      Name = "GB" + blockno.ToString(),
      Tag = blockno
     };
     gpn[blockno] = obox.Name;
     cm = new ContextMenuStrip();
     cm.Leave += Cm_leave;
     tsi = new ToolStripMenuItem()
     {
      Text = "Drop This Block",
      Name = blockno.ToString(),
      Tag = 0
     };
     tsi.Click += Menudrp_Click;
     cm.Items.Add(tsi);
     ins = new ToolStripMenuItem()
     {
      Text = "Insert Block Before",
      Name = blockno.ToString(),
      Tag = 1
     };
     ins.Click += Menuins_Click;
     cm.Items.Add(ins);
     swp = new ToolStripMenuItem()
     {
      Text = "Swap Block Before",
      Name = blockno.ToString(),
      Tag = 2
     };
     swp.Click += Menuswp_Click;
     cm.Items.Add(swp);
     obox.ContextMenuStrip = cm;
     Setupdn(ref cm, blockno, cm.Items.Count);
     var hbox = new TextBox()
     {
      Text = " ",
      Size = new Size(xsize - 2 * margin1, tboxsz),
      Location = new Point(margin1, margin3),
      Name = obox.Name + "H:",
      Tag = blockno,
      Font = new Font("Calibri", 8.25F, System.Drawing.FontStyle.Bold, GraphicsUnit.Point, Conversions.ToByte(0)),
      Enabled = false
     };
     gph[blockno] = hbox.Name;
     fieldsgb.Add(hbox.Name);
     hbox.Enter += Textbox_enter;
     hbox.Leave += Leave_fld;
     cm = new ContextMenuStrip();
     cm.Leave += Cm_leave;
     tsi = new ToolStripMenuItem()
     {
      Text = "Drop This Block",
      Name = blockno.ToString()
     };
     tsi.Click += Menudrp_Click;
     cm.Items.Add(tsi);
     ins = new ToolStripMenuItem()
     {
      Text = "Insert Block Before",
      Name = blockno.ToString()
     };
     ins.Click += Menuins_Click;
     cm.Items.Add(ins);
     swp = new ToolStripMenuItem()
     {
      Text = "Swap Block Before",
      Name = blockno.ToString()
     };
     swp.Click += Menuswp_Click;
     cm.Items.Add(swp);
     Setupdn(ref cm, blockno, cm.Items.Count);
     hbox.ContextMenuStrip = cm;
     obox.Controls.Add(hbox);
     var rbox = new RichTextBox()
     {
      Text = " ",
      WordWrap = false,
      Size = new Size(xsize - 2 * margin1, ysize - 2 * margin1 - margin3 - tboxsz),
      Location = new Point(margin1, tboxsz + margin1 + margin3),
      Name = obox.Name + "B:",
      Font = new Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, Conversions.ToByte(0)),
      Enabled = false
     };
     gpb[blockno] = rbox.Name;
     fieldsgb.Add(rbox.Name);
     rbox.Enter += Rtbbox_enter;
     rbox.Leave += Leave_fld;
     cm = new ContextMenuStrip();
     cm.Leave += Cm_leave;
     tsi = new ToolStripMenuItem()
     {
      Text = "Drop This Block",
      Name = blockno.ToString()
     };
     tsi.Click += Menudrp_Click;
     cm.Items.Add(tsi);
     ins = new ToolStripMenuItem()
     {
      Text = "Insert Block Before",
      Name = blockno.ToString()
     };
     ins.Click += Menuins_Click;
     cm.Items.Add(ins);
     swp = new ToolStripMenuItem()
     {
      Text = "Swap Block Before",
      Name = blockno.ToString()
     };
     swp.Click += Menuswp_Click;
     cm.Items.Add(swp);
     Setupdn(ref cm, blockno, cm.Items.Count);
     rbox.ContextMenuStrip = cm;
     obox.Controls.Add(rbox);
     Controls.Add(obox);
     bl_available[blockno] = obox.Name;
     blockno += 1;
    }
   }

   cm = new ContextMenuStrip();
   var Eti = new ToolStripMenuItem()
   {
    Text = "Use Timer",
    Name = "Eltimermi"
   };
   Eti.Click += Eltimer_set;
   cm.Items.Add(Eti);
   GroupBox2.ContextMenuStrip = cm;
   points = 8;
   reg_font = new FontInfo("Calibri", points, DYMO.Label.Framework.FontStyle.None);
   rp1_font = new FontInfo("Calibri", points + 1, DYMO.Label.Framework.FontStyle.None);
   bld_font = new FontInfo("Calibri", points, DYMO.Label.Framework.FontStyle.Bold);
   bp1_font = new FontInfo("Calibri", points + 1, DYMO.Label.Framework.FontStyle.Bold);
   itl_font = new FontInfo("Calibri Light Italic", points, DYMO.Label.Framework.FontStyle.Italic);
   try
   {
    Label = (IDieCutLabel)Framework.Open("mpc1.label");
    foreach (string o in Label.ObjectNames)
    {
     if (!string.IsNullOrEmpty(o))
     {
      lab1[o] = Label.GetObjectText(o);
     }
    }
   }
   catch (Exception ex)
   {
    MessageBox.Show(ex.Message);
   }

   Printers.Items.Clear();
   pnames = Framework.GetPrinters();
   foreach (var p in pnames)
    Printers.Items.Add(p.Name);
   if (Printers.Items.Count > 0)
   {
    Printers.SelectedIndex = 0;
   }

   printer = pnames.GetPrinterByName(Conversions.ToString(Printers.SelectedItem));
   if (printer is ILabelWriterPrinter)
   {
    labelWriterPrinter = (ILabelWriterPrinter)printer;
    lprintparams = new LabelWriterPrintParams()
    {
     Copies = 1,
     FlowDirection = DYMO.Label.Framework.FlowDirection.LeftToRight,
     RollSelection = RollSelection.Auto
    };
   }

   Reset_fields();
   //base.ResumePaint();
   if (My.MySettingsProperty.Settings.first_run_flag)
   {
    My.MySettingsProperty.Settings.first_run_flag = false;
    My.MySettingsProperty.Settings.Save();
    Close();
   }

   priph.Visible = true;
   prv_combo.Visible = false;
   Text = "Medical Profile Card (" + installed_version + ")";
   var claims = Gen_Claims();
   aws_body.Clear();
   aws_body["ukey"] = Enc.Enc256.Encrypt(cid + Mpck.Dlab, cid + Mpck.Dlab, Mpck.Iterations);
   Text = "Medical Profile Card (" + installed_version + ")" + " - Loading Practice Information";
   Update();
   if (run_timer)
   {
    Ettb.Text = start_timer(SW);
   }

   Application.UseWaitCursor = true;
   L1_ret = await Aws.Get_Level1_aysnc(Mpck.Url, enck, Mpck.Salt, claims, aws_body);
   Application.UseWaitCursor = false;
   if (L1_ret.code != 200)
   {
    Interaction.MsgBox("Returned Code(" + L1_ret.code.ToString() + ") - " + L1_ret.message, MsgBoxStyle.OkOnly, "Error getting practice information");
    Application.Exit();
   }

   Text = "Medical Profile Card (" + installed_version + ")";
   Update();
   if (L1_ret.Prc is null)
   {
    Interaction.MsgBox("Unable to get practice information", MsgBoxStyle.OkOnly, "Error in information returned");
    Application.Exit();
   }

   Dstate = Data_state.Edit_mode;
   Practice.Text = L1_ret.Prc.name;
   Load_Department(L1_ret);
   Load_providers(L1_ret);
   foreach (string s in L1_ret.Block_names)
   {
    var b = new Blk_entry()
    {
     State = (int)Load_state.not_in_use,
     Num = -1
    };
    bl_loaded[s] = b;
   }

   Set_saved_items(L1_ret.dsl);
   Patientid.Select();
   loading = false;
   Data_altered = false;
   Dstate = Data_state.NoEdit;
   priph.Visible = true;
   prv_combo.Visible = false;
   if (run_timer)
   {
    Ettb.Text = stop_timer(SW);
   }
  }

  private Dictionary<string, object> Gen_Claims()
  {
   var nw = DateTimeOffset.UtcNow;
   var ew = nw.AddMinutes(5);
   // Dim ew As DateTimeOffset = nw.AddYears(1)
   IDateTimeProvider provider = new UtcDateTimeProvider();
   var payload = new Dictionary<string, object>() { { "aud", "http: //medicalprofilecard.com" }, { "exp", ew.ToUnixTimeSeconds() }, { "iss", Mpck.Email }, { "label", drive_label }, { "key", Mpck.Mkey } };
   if (file_access)
   {
    payload["cid"] = Enc.Enc256.Encrypt(cid, Set_key(Mpck.Secret, Mpck.Dlab), Convert.ToInt32(Mpck.Dlab.Substring(3)) + Mpck.Iterations);
    payload["f1t"] = USB_Ftime.ToUnixTimeSeconds();
   }

   if (ftime is object && Conversions.ToInteger(Conversions.ToDouble(ftime) > 0) != 0)
   {
    payload["f1t"] = ftime;
   }

   return payload;
  }

  private void Printers_SelectionChangeCommitted(object sender, EventArgs e)
  {
  }

  private void ExitMenuItem_Click(object sender, EventArgs e)
  {
   Close();
  }

  private void Set_ll(RichTextBox rtb, int bn)
  {
   int ln = 0;
   var bi = blocks[bn];
   for (int i = 0, loopTo = bi.lines - 1; i <= loopTo; i += 1)
   {
    bi.ll[i] = ln;
    ln = ln + rtb.Lines[i].Length + 1;
   }
  }

  private void Redo_blocks(string ls)
  {
   int lnum = 0;
   float c0l;
   float c1l;
   int mrkl;
   int pline;
   RichTextBox rtb;
   TextBox tb;
   int ln = 0;
   Blk_info bi = null;
   int bn = 0;
   foreach (Control cb in Controls)
   {
    if (cb.Name.StartsWith("GB") && cb.Visible)
    {
     bn = (from kvp in gpn
           where (kvp.Value ?? "") == (cb.Name ?? "")
           select kvp).First().Key;
     rtb = (RichTextBox)cb.Controls[1];
     tb = (TextBox)cb.Controls[0];
     bi = blocks[bn];
     bi.lines = rtb.Lines.Length;
     bi.ll = new int[bi.lines + 1];
     ln = 0;
     Set_ll(rtb, bn);
     if ((ls ?? "") == "File")
     {
      rtb.ReadOnly = false;
      tb.ReadOnly = false;
      tb.Enabled = true;
      rtb.Text = labgb[cb.Name + "B:"];
      tb.Text = labgb[cb.Name + "H:"];
      bi.lines = rtb.Lines.Length;
      bi.ll = new int[bi.lines + 1];
      Set_ll(rtb, bn);
      Reset_color(bn);
      Fe_setcolor(bn);
     }
     else
     {
      labgb[cb.Name + "B:"] = rtb.Text;
      labgb[cb.Name + "H:"] = tb.Text;
      rtb.Text = this.Adjust_lines(rtb.Text, ls);
      bi.lines = rtb.Lines.Length;
      bi.ll = new int[bi.lines + 1];
      Set_ll(rtb, bn);
      Reset_color(bn);
      mrkl = Fe_setcolor(bn);
      if (cb.Controls[0].Text.Length > 0)
      {
       c0l = Wlengb(cb.Controls[0].Text.ToString());
      }
      else
      {
       c0l = 0.0F;
      }

      if (rtb.Lines.Count() > 0)
      {
       c1l = Wleng(rtb.Lines[0]);
      }
      else
      {
       c1l = 0.0F;
      }

      if (label_length < c0l + c1l + Wleng(" "))
      {
       lnum += 1;
      }

      if (total_lines > lnum + mrkl)
      {
       lnum += mrkl;
      }
      else
      {
       pline = total_lines - lnum;
       if (pline < 0)
       {
        pline = 0;
       }

       Setcolor(bn, pline);
       lnum += mrkl;
      }

      rtb.ReadOnly = true;
      tb.ReadOnly = true;
      tb.Enabled = false;
     }
    }
   }
  }

  private void DocumentMenuItem_Click(object sender, EventArgs e)
  {
   lines_setting = "File";
   My.MySettingsProperty.Settings.lines_setting = lines_setting;
   My.MySettingsProperty.Settings.Save();
   Redo_blocks(lines_setting);
  }

  private void LabelMenuItem_Click(object sender, EventArgs e)
  {
   lines_setting = "Label";
   My.MySettingsProperty.Settings.lines_setting = lines_setting;
   My.MySettingsProperty.Settings.Save();
   Redo_blocks(lines_setting);
  }

  private void LblPrintMenuItem_Click(object sender, EventArgs e)
  {
   Reset_labels();
  }

  private void LblDrawMenuItem_Click(object sender, EventArgs e)
  {
   Reset_labels();
  }

  private void Leave_fld(object sender, EventArgs e)
  {
   RichTextBox rtb;
   TextBox tb;
   GroupBox gb;
   string field = null;

   if ((sender.GetType().Name ?? "") == "GroupBox")
   {
    gb = (GroupBox)sender;
    field = gb.Name;
    if(!field.StartsWith("GB"))
     return;
   }

   if ((sender.GetType().Name ?? "") == "RichTextBox")
   {
    rtb = (RichTextBox)sender;
    tb = (TextBox)rtb.Parent.Controls[0];
    if (labgb.ContainsKey(field))
    {
     if (!((labgb[field] ?? "") == (rtb.Text ?? "")))
     {
      labgb[field] = rtb.Text;
      if (bl_loaded.ContainsKey(Conversions.ToString(tb.Tag)))
      {
       bl_loaded[Conversions.ToString(tb.Tag)].Num = default;
       bl_loaded[Conversions.ToString(tb.Tag)].State = (int)Load_state.not_loaded_byc;
      }

      Set_empgb();
     }
    }
    else
    {
     labgb[field] = rtb.Text;
    }
   }
   else
   {
    tb = (TextBox)sender;
    if ((tb.Tag.ToString() ?? "") != (tb.Text ?? ""))
    {
     if (bl_loaded.ContainsKey(Conversions.ToString(tb.Tag)))
     {
      bl_loaded[Conversions.ToString(tb.Tag)].Num = default;
      bl_loaded[Conversions.ToString(tb.Tag)].State = (int)Load_state.not_loaded_byc;
     }

     Set_empgb();
    }

    if (labgb.ContainsKey(field))
    {
     if (!((labgb[field] ?? "") == (tb.Text ?? "")))
     {
      labgb[field] = tb.Text;
      // Data_altered = True
     }
    }
    else
    {
     labgb[field] = tb.Text;
     // Data_altered = True
    }
   }
  }

  private void Load_block_click(object sender, EventArgs e)
  {
   ToolStripMenuItem item = (ToolStripMenuItem)sender;
   string blk = (string)item.Tag;
   if (bl_used.Count == blocks_number)
   {
    Interaction.MsgBox("Unable to load " + blk, MsgBoxStyle.Information, "All Blocks are in use");
   }
   else
   {
    if (L2_ret.blks.ContainsKey(blk) && L2_ret.blks[blk].Count > 0)
    {
     int fnum = bl_available.First().Key;
     bl_used[fnum] = bl_available.First().Value;
     int lnum = bl_used.Getlkey();
     bl_available.Remove(fnum);
     Controls[gpn[lnum]].Visible = false;
     Controls[gpn[lnum]].Controls[0].Text = blk;
     Controls[gpn[lnum]].Controls[0].Tag = blk;
     Controls[gpn[lnum]].Controls[0].Enabled = true;
     Controls[gpn[lnum]].Controls[1].Text = string.Join(Constants.vbCrLf, L2_ret.blks[blk]);
     Controls[gpn[lnum]].Controls[1].Enabled = true;
     Ath_setcolor(lnum);
     Controls[gpn[lnum]].Visible = true;
     labgb[gph[lnum]] = blk;
     labgb[gpb[lnum]] = string.Join(Constants.vbCrLf, L2_ret.blks[blk]);
     bl_loaded[blk].Num = lnum;
     bl_loaded[blk].State = (int)Load_state.loaded;
    }

    Scsiz(Width);
    Update();
    Set_empgb();
   }
  }

  private void Menudrp_Click(object sender, EventArgs e)
  {
   int objn = Conversions.ToInteger(((ToolStripMenuItem)sender).Name);
   GroupBox gb = (GroupBox)Controls[gpn[objn]];
   string Tag = Conversions.ToString(gb.Controls[0].Tag);
   if (bl_loaded.ContainsKey(Tag))
   {
    if (bl_loaded[Tag].State == (int)Load_state.loaded_byc)
    {
     bl_loaded[Tag].State = (int)Load_state.not_loaded_byc;
    }

    if (bl_loaded[Tag].State == (int)Load_state.loaded)
    {
     bl_loaded[Tag].State = (int)Load_state.not_loaded;
    }
   }

   for (int ib = objn, loopTo = bl_used.Getlkey() - 1; ib <= loopTo; ib += 1)
    Move_blk(ib + 1, ib);
   labgb[gph[bl_used.Getlkey()]] = string.Empty;
   labgb[gpb[bl_used.Getlkey()]] = string.Empty;
   Controls[bl_used.Getlval()].Visible = false;
   bl_available[bl_used.Getlkey()] = bl_used.Getlval();
   bl_used.Remove(bl_used.Getlkey());
   Set_empgb();
   Scsiz(Width);
  }

  private void Move_blk(int fbn, int tbn)
  {
   GroupBox gbf = (GroupBox)Controls[gpn[fbn]];
   GroupBox gbt = (GroupBox)Controls[gpn[tbn]];
   gbt.Controls[0].Tag = gbf.Controls[0].Tag;
   gbt.Enabled = true;
   if (bl_loaded.ContainsKey(Conversions.ToString(gbt.Controls[0].Tag)))
   {
    bl_loaded[Conversions.ToString(gbt.Controls[0].Tag)].Num = tbn;
    bl_loaded[Conversions.ToString(gbt.Controls[0].Tag)].State = (int)Load_state.loaded;
   }

   gbt.Controls[0].Enabled = true;
   gbt.Controls[1].Enabled = true;
   labgb[gph[tbn]] = labgb[gph[fbn]];
   labgb[gpb[tbn]] = labgb[gpb[fbn]];
   gbt.Controls[0].Text = gbf.Controls[0].Text;
   gbt.Controls[1].Text = gbf.Controls[1].Text;
   gbf.Controls[0].Tag = gbf.Tag;
   gbf.Controls[0].Text = string.Empty;
   gbf.Controls[1].Text = string.Empty;
   gbf.Controls[0].Enabled = false;
   gbf.Controls[1].Enabled = false;
   Set_ro(gbt.Controls[0]);
   Set_ro(gbt.Controls[1]);
  }

  private void Menuins_Click(object sender, EventArgs e)
  {
   int objn = Conversions.ToInteger(((ToolStripMenuItem)sender).Name);
   GroupBox gb = (GroupBox)Controls[gpn[objn]];
   if (bl_used.Count == blocks_number)
   {
    Interaction.MsgBox("Unable to insert block", MsgBoxStyle.OkOnly, "All blocks are in use");
    return;
   }

   bl_used[bl_available.First().Key] = bl_available.First().Value;
   bl_available.Remove(bl_available.First().Key);
   for (int ii = bl_used.Getlkey() - 1, loopTo = objn; ii >= loopTo; ii += -1)
    Move_blk(ii, ii + 1);
   labgb[gph[objn]] = string.Empty;
   labgb[gpb[objn]] = string.Empty;
   gb.Controls[0].Text = string.Empty;
   gb.Controls[1].Text = string.Empty;
   gb.Controls[0].Enabled = true;
   gb.Controls[1].Enabled = true;
   gb.Controls[0].Tag = gb.Tag;
   Controls[gpn[bl_used.Getlkey()]].Visible = true;
   Set_empgb();
   Scsiz(Width);
  }

  private void Menuswp_Click(object sender, EventArgs e)
  {
   int objn = Conversions.ToInteger(((ToolStripMenuItem)sender).Name);
   if (objn == 1)
   {
    return;
   }

   GroupBox gb1 = (GroupBox)Controls[gpn[objn]];
   GroupBox gb2 = (GroupBox)Controls[gpn[objn - 1]];
   string n1 = Conversions.ToString(gb1.Tag);
   string n2 = Conversions.ToString(gb2.Tag);
   string tag1 = Conversions.ToString(gb1.Controls[0].Tag);
   string tag2 = Conversions.ToString(gb2.Controls[0].Tag);
   if (bl_loaded.ContainsKey(tag2))
   {
    bl_loaded[tag2].Num = objn;
   }

   if (bl_loaded.ContainsKey(tag1))
   {
    bl_loaded[tag1].Num = objn - 1;
   }

   string sh = labgb[gph[objn - 1]];
   string sb = labgb[gpb[objn - 1]];
   labgb[gph[objn - 1]] = labgb[gph[objn]];
   labgb[gpb[objn - 1]] = labgb[gpb[objn]];
   labgb[gph[objn]] = sh;
   labgb[gpb[objn]] = sb;
   Controls[gpn[objn - 1]].Controls[0].Text = labgb[gph[objn - 1]];
   Controls[gpn[objn - 1]].Controls[1].Text = labgb[gpb[objn - 1]];
   Set_ro(Controls[gpn[objn - 1]].Controls[0]);
   Set_ro(Controls[gpn[objn - 1]].Controls[1]);
   Controls[gpn[objn]].Controls[0].Text = labgb[gph[objn]];
   Controls[gpn[objn]].Controls[1].Text = labgb[gpb[objn]];
   Set_ro(Controls[gpn[objn]].Controls[0]);
   Set_ro(Controls[gpn[objn]].Controls[1]);
   Set_empgb();
  }

  private void Showlabel(object sender, EventArgs e)
  {
   string bn = ((ToolStripMenuItem)sender).Name;
   if (f3s.ContainsKey(bn))
   {
    f3s[bn].ShowDialog();
   }
  }

  private void Cudchange(object sender, EventArgs e)
  {
   int objn = Conversions.ToInteger(((TSnumud)sender).Name);
   TSnumud tsi = (TSnumud)sender;
   int tg1 = tsi.aml;
   int tgno = tsi.udl;
   max_rec[Conversions.ToInteger(tsi.Name)] = tsi.Value;
   Redo_blocks(lines_setting);
   if (tsi.Value == 0)
   {
    tsi.Owner.Items[tsi.udl - 2].Enabled = false;
    tsi.Owner.Items[tsi.udl - 2].Visible = false;
    tsi.Owner.Items[tsi.udl - 1].Enabled = false;
    tsi.Owner.Items[tsi.udl - 1].Visible = false;
    tsi.Enabled = false;
    tsi.Visible = false;
    tsi.Owner.Items[tsi.aml].Enabled = true;
    tsi.Owner.Items[tsi.aml].Visible = true;
   }
  }

  private void Cm_leave(object sender, EventArgs e)
  {
   ContextMenuStrip cm = (ContextMenuStrip)sender;
   cm.Close();
  }

  private void Cm_leave_m(object sender, EventArgs e)
  {
   ContextMenuStrip cm = (ContextMenuStrip)sender;
   cm.Close();
  }

  private void Cud_activate(object sender, EventArgs e)
  {
   ToolStripMenuItem tsi;
   TSnumud tud;
   tsi = (ToolStripMenuItem)sender;
   ;

   tud = (TSnumud)tsi.Owner.Items[(int)tsi.Tag];

   tsi.Owner.Items[tud.udl - 2].Enabled = true;
   tsi.Owner.Items[tud.udl - 2].Visible = true;
   tsi.Owner.Items[tud.udl - 1].Enabled = true;
   tsi.Owner.Items[tud.udl - 1].Visible = true;
   tud.Enabled = true;
   tud.Visible = true;
   tsi.Enabled = false;
   tsi.Visible = false;
  }

  private void Set_ro(Control ctrl)  {
   TextBox tb;
   RichTextBox rb;
   if (ctrl is TextBox)
   {
    tb = (TextBox)ctrl;
    if ((Editmenuitem.Text ?? "") == "Edit")
    {
     tb.ReadOnly = true;
     tb.BackColor = System.Drawing.Color.White;
    }
    else
    {
     tb.ReadOnly = false;
    }
   }

   if (ctrl is RichTextBox)
   {
    rb = (RichTextBox)ctrl;
    if ((Editmenuitem.Text ?? "") == "Edit")
    {
     rb.ReadOnly = true;
     rb.BackColor = System.Drawing.Color.White;
    }
    else
    {
     rb.ReadOnly = false;
    }
   }
  }

  private void AddBlockMenuItem_Click(object sender, EventArgs e)
  {
   int bn;
   if (bl_used.Count == blocks_number)
   {
    Interaction.MsgBox("Unable to add page", MsgBoxStyle.Information, "All are in use");
   }
   else
   {
    bn = bl_available.First().Key;
    bl_available.Remove(bn);
    bl_used[bn] = gpn[bn];
    Controls[gpn[bn]].Controls[0].Text = string.Empty;
    Controls[gpn[bn]].Controls[1].Text = string.Empty;
    Controls[gpn[bn]].Controls[0].Enabled = true;
    Controls[gpn[bn]].Controls[1].Enabled = true;
    Controls[gpn[bn]].Visible = true;
    Set_ro(Controls[gpn[bn]].Controls[0]);
    Set_ro(Controls[gpn[bn]].Controls[1]);
    labgb[gph[bn]] = string.Empty;
    labgb[gpb[bn]] = string.Empty;
    Set_empgb();
    Scsiz(Width);
   }
  }

  private void Clear_set()
  {
   foreach (Control cb in Controls)
   {
    if (cb is GroupBox)
    {
     foreach (Control cb1 in cb.Controls)
     {
      if (cb1 is TextBox | cb1 is RichTextBox)
      {
       if (cb1.Font.Italic)
       {
        cb1.Text = string.Empty;
        cb1.Enter -= Field_click;
        if (bfont[cb1.Name])
        {
         cb1.Font = new Font("Calibri", 8.25F, System.Drawing.FontStyle.Bold);
        }
        else
        {
         cb1.Font = new Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular);
        }
       }
      }
     }
    }
   }
  }

  //private void Set_field(object blk, string val)
  //{
  // blk.Text = val;
  // if (Conversions.ToBoolean(blk.Font.Bold))
  // {
  //  bfont[Conversions.ToString(blk.Name)] = true;
  // }
  // else
  // {
  //  bfont[Conversions.ToString(blk.Name)] = false;
  // }

  // if (blk is TextBox)
  // {
  //  TextBox blk1 = (TextBox)blk;
  //  blk1.Enter += Field_click;
  // }

  // if (blk is RichTextBox)
  // {
  //  RichTextBox blk1 = (RichTextBox)blk;
  //  blk1.Enter += Field_click;
  // }

  // blk.Font = new Font("Calibri Light", 8.25F, System.Drawing.FontStyle.Italic);
  //}

  private void Field_click(object sender, EventArgs e)
  {
   TextBox blk = (TextBox)sender;
   if (blk.Font.Italic)
   {
    blk.Text = string.Empty;
    blk.Enter -= Field_click;
    if (bfont[blk.Name])
    {
     blk.Font = new Font("Calibri", 8.25F, System.Drawing.FontStyle.Bold);
    }
    else
    {
     blk.Font = new Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular);
    }
   }
  }

  private void ClearMenuItem_Click(object sender, EventArgs e)
  {
   this.SuspendPaint();
   file_clear = true;
   Reset_labels();
   Reset_fields();
   Patient.Text = "";
   Patientid.Text = "";
   Department.SelectedText = null;
   Department.Text = null;
   Data_altered = false;
   dsaves.SelectedIndexChanged -= Dsaves_SelectedIndexChanged;
   dsaves.SelectedItem = null;
   dsaves.SelectedText = "";
   dsaves.SelectedIndexChanged += Dsaves_SelectedIndexChanged;
   this.ResumePaint();
  }

  private void CheckMenuItem(ToolStripMenuItem mnu, ToolStripMenuItem checked_item)
  {
   foreach (ToolStripItem item in mnu.DropDownItems)
   {
    if (item is ToolStripMenuItem)
    {
     ToolStripMenuItem menu_item = (ToolStripMenuItem)item;
     menu_item.Checked = menu_item == checked_item;
    }
   }
  }

  private void Econtact_Leave(object sender, EventArgs e)
  {
   lab1["Emergency Contact:"] = econtact.Text;
  }

  private void Patient_TextChanged(object sender, EventArgs e)
  {
   lab1["Patient Name:"] = Patient.Text;
  }

  private void DOB_TextChanged(object sender, EventArgs e)
  {
   lab1["DOB:"] = DOB.Text;
  }

  private void Econtact_TextChanged(object sender, EventArgs e)
  {
   lab1["Emergency Contact:"] = econtact.Text;
  }

  private void Phone_TextChanged(object sender, EventArgs e)
  {
   lab1["Patient Phone:"] = Phone.Text;
  }

  private void Priph_TextChanged(object sender, EventArgs e)
  {
   lab1["Primary Physician:"] = priph.Text;
  }

  private void Ins_TextChanged(object sender, EventArgs e)
  {
   lab1["Insurance:"] = ins.Text;
  }

  private void Secph_TextChanged(object sender, EventArgs e)
  {
   lab1["secph:"] = secph.Text;
  }

  private void Address_TextChanged(object sender, EventArgs e)
  {
   lab1["Patient Address:"] = address.Text;
  }

  private void Sp_Title_Click(object sender, EventArgs e)
  {
   if (sptitle.Visible)
   {
    sptitle.Visible = false;
   }
   else
   {
    sptitle.Text = sp.Text;
    sptitle.Visible = true;
   }
  }

  private void Sptitle_Leave(object sender, EventArgs e)
  {
   if (sptitle.Text.Length > 0)
   {
    sp.Text = sptitle.Text;
    sp.Visible = true;
    lab1["secph_title:"] = sp.Text;
    secph.Enabled = true;
    secph.Visible = true;
    sptitle.Visible = false;
   }
  }

  private void Sptitle_KeyPress(object sender, KeyPressEventArgs e)
  {
   if (e.KeyChar == (char)(int)Keys.Return)
   {
    SendKeys.Send("{TAB}");
    e.Handled = true;
   }
  }

  private void Reset_color(int bn)
  {
   RichTextBox bx = null;
   TextBox tb;
   Blk_info bi = null;
   tb = (TextBox)Controls[gpn[bn]].Controls[0];
   bx = (RichTextBox)Controls[gpn[bn]].Controls[1];
   bi = blocks[bn];
   bx.SelectAll();
   bx.SelectionBackColor = System.Drawing.Color.White;
   bx.Select(0, 0);
   tb.BackColor = System.Drawing.Color.White;
   bi.hv = default;
   bi.lv = default;
   bi.lines = bx.Lines.Length;
  }

  private int Fe_setcolor(int bn)
  {
   RichTextBox bx = null;
   Blk_info bi = null;
   bx = (RichTextBox)Controls[gpn[bn]].Controls[1];
   bi = blocks[bn];
   if (!gpn.ContainsKey(bn))
   {
    return bi.lines;
   }

   if (max_rec[bn] <= 0 | bi.ll.Length <= max_rec[bn])
   {
    return bi.lines;
   }

   bx.Select(0, bi.ll[max_rec[bn]]);
   bx.SelectionBackColor = mp_backcolor;
   Application.DoEvents();
   bx.SelectionLength = 0;
   bx.Select(0, 0);
   return max_rec[bn];
  }

  private void ChangeMySelectionColor()
  {
  }

  private int Ath_setcolor(int bn)
  {
   int ln = 0;
   int lines = default;
   int[] ll = null;
   TextBox hx = null;
   RichTextBox bx = null;
   hx = (TextBox)Controls[gpn[bn]].Controls[0];
   bx = (RichTextBox)Controls[gpn[bn]].Controls[1];
   ll = new int[bx.Lines.Length + 1];
   lines = bx.Lines.Length;
   if (!gpn.ContainsKey(bn))
   {
    return lines;
   }

   bx.SelectAll();
   bx.SelectionBackColor = System.Drawing.Color.White;
   bx.Select(0, 0);
   hx.BackColor = System.Drawing.Color.White;
   if (!max_rec.ContainsKey(bn))
   {
    return lines;
   }

   if (max_rec[bn] <= 0 | ll.Length <= max_rec[bn])
   {
    return lines;
   }

   for (int i = 0, loopTo = lines - 1; i <= loopTo; i += 1)
   {
    ll[i] = ln;
    ln = ln + bx.Lines[i].Length + 1;
   }

   Application.DoEvents();
   bx.Select(0, ll[max_rec[bn]]);
   bx.SelectionBackColor = mp_backcolor;
   Application.DoEvents();
   bx.SelectionLength = 0;
   bx.Select(0, 0);
   return max_rec[bn];
  }

  private void Add_cm(string name, ContextMenuStrip cm, ToolStripMenuItem ts1, ToolStripMenuItem ts2)
  {
   ts1 = new ToolStripMenuItem()
   {
    Name = "Block" + name,
    Text = "Show " + name,
    Tag = name
   };
   ts1.Click += Load_block_click;
   ts2 = new ToolStripMenuItem()
   {
    Name = "Block" + name,
    Text = "Show " + name,
    Tag = name
   };
   ts2.Click += Load_block_click;
   cm.Items.Add(ts2);
  }

  private void Set_empgb(bool res_endpoints = false)
  {
   GroupBox gb = null;
   GroupBox lb = null;
   var cm = new ContextMenuStrip();
   ToolStripMenuItem ts1 = null;
   ToolStripMenuItem ts2 = null;
   int pnxstart = 0;
   int pnystart = 0;
   int pnxsize = 0;
   int pnysize = 0;
   int bnum = 0;
   var bmni = new List<ToolStripMenuItem>();
   bmni.Clear();
   if (emppn is object)
   {
    Controls.Remove(emppn);
    emppn.Dispose();
   }

   foreach (ToolStripMenuItem tsi in bmni)
   {
   }

   emppn = null;
   if (bl_used.Count % 3 != 0)
   {
    lb = (GroupBox)Controls[bl_used.Getlval()];
    emppn = new Panel() { Name = "emppn" };
    pnxstart = lb.Location.X + lb.Width + 1;
    if (bl_used.Count < 3)
    {
     pnystart = lb.Location.Y - 2 * margin1 + 1;
     gb = (GroupBox)Controls["GB3"];
    }
    else
    {
     gb = (GroupBox)Controls[("GB" + Conversions.ToString(bl_used.Count / 3 * 3)).ToString()];
     pnystart = gb.Location.Y + gb.Height + 1;
    }

    pnxsize = gb.Location.X + gb.Width - pnxstart;
    pnysize = lb.Location.Y + lb.Height - pnystart;
    emppn.Location = new Point(pnxstart, pnystart);
    emppn.Size = new Size(pnxsize, pnysize);
   }

   bool Rebuild_Endpoints = false;
   foreach (KeyValuePair<string, Blk_entry> k in bl_loaded)
   {
    var b = k.Value;
    if (L2_ret is null)
    {
     continue;
    }

    if (!L2_ret.blks.ContainsKey(k.Key))
    {
     continue;
    }

    if (L2_ret.blks[k.Key].Count == 0)
    {
     continue;
    }

    if (b.State == (int)Load_state.not_loaded | b.State == (int)Load_state.not_loaded_byc | b.State == (int)Load_state.not_in_use)
    {
     Add_cm(k.Key, cm, ts1, ts2);
     continue;
    }

    if (string.IsNullOrEmpty(Controls[gpn[b.Num]].Controls[1].Text))
    {
     continue;
    }
   }

   if (emppn is object)
   {
    emppn.ContextMenuStrip = cm;
    Controls.Add(emppn);
   }

   if (res_endpoints)
   {
    var tbiu = new System.Collections.Specialized.StringCollection();
    foreach (KeyValuePair<string, Blk_entry> k in bl_loaded)
    {
     if (k.Value.State != (int)Load_state.not_loaded)
     {
      tbiu.Add(k.Key);
     }
    }

    if (tbiu.Count != Endpoints_in_use.Count)
    {
     My.MySettingsProperty.Settings.endpoints = new System.Collections.Specialized.StringCollection();
     foreach (string s in tbiu)
      My.MySettingsProperty.Settings.endpoints.Add(s);
     My.MySettingsProperty.Settings.Save();
    }

    Update();
   }

   Scsiz(Width);
  }

  private async void Patientid_Leave(object sender, EventArgs e)
  {
   var claims = Gen_Claims();
   string fs = null;
   string fn = null;
   aws_body.Clear();
   if ((Editmenuitem.Text ?? "") == "End Edit")
   {
    return;
   }

   if (Patientid.Text.Length == 0)
   {
    Patient.ReadOnly = false;
   }

   if (!Regex.IsMatch(Patientid.Text, "^[0-9 ]+$"))
   {
    return;
   }

   if (!rethit & !tabhit)
   {
    Patientid.Text = "";
    return;
   }

   aws_body["patientid"] = Patientid.Text;
   aws_body["departmentid"] = atdp[Department.SelectedItem.ToString()];
   Reset_fields();
   Text = "Medical Profile Card (" + installed_version + ")" + " - Loading Patient Information";
   Update();
   Application.UseWaitCursor = true;
   L2_ret = await Aws.Get_Level2_aysnc(Mpck.Url, enck, Mpck.Salt, claims, aws_body);
   Application.UseWaitCursor = false;
   if (L2_ret.code != 200)
   {
    Interaction.MsgBox(L2_ret.message, MsgBoxStyle.OkOnly, "Medical Profile Card");
    Reset_fields();
    Patientid.Text = "";
    Patientid.Select();
    return;
   }

   Text = "Medical Profile Card (" + installed_version + ")";
   Update();
   Load_level2(L2_ret);
  }

  private async void Patientid_Click(object sender, EventArgs e)
  {
   if ((Editmenuitem.Text ?? "") == "End Edit")
   {
    return;
   }

   if (Check_altered())
   {
    await save_blk();
    Application.DoEvents();
   }

   Data_altered = false;
   Dsi = new KeyValuePair<string, Dsave>();
   if (inpid)
   {
    return;
   }

   inpid = true;
   inpat = false;
   L2_ret = null;
   Patient.ReadOnly = true;
   Patient.Enabled = false;
   Patient.BackColor = System.Drawing.Color.White;
   Patient.Text = "";
   patep.SetError(Patient, "");
   Patientid.ReadOnly = false;
   Patientid.Enabled = true;
   Patientid.BackColor = System.Drawing.Color.White;
   rethit = false;
   tabhit = false;
   if (!inpid)
   {
    Patientid.Text = "";
   }

   Reset_fields();
   Deletemi.Enabled = false;
   Savemi.Enabled = false;
   dsaves.SelectedIndexChanged -= Dsaves_SelectedIndexChanged;
   dsaves.SelectedItem = null;
   dsaves.SelectedIndexChanged += Dsaves_SelectedIndexChanged;
   Scsiz(Width, originaly);
   Invalidate();
   Update();
  }

  private async void Patientid_Enter(object sender, EventArgs e)
  {
   if ((Editmenuitem.Text ?? "") == "End Edit")
   {
    return;
   }

   if (Check_altered())
   {
    await save_blk();
    Application.DoEvents();
   }

   Data_altered = false;
   if (inpid)
   {
    return;
   }

   Patientid.SelectionStart = 0;
   inpid = true;
   L2_ret = null;
   inpat = false;
   Patient.ReadOnly = true;
   Patient.BackColor = System.Drawing.Color.White;
   Patient.Text = "";
   patep.SetError(Patient, "");
   Patientid.ReadOnly = false;
   rethit = false;
   tabhit = false;
   if (valid_patientid)
   {
    Patientid.Text = "";
   }

   Reset_fields();
   Scsiz(Width, originaly);
   Invalidate();
   Update();
  }

  private void Cnt_set(Control ctrl, bool enb, bool ro)
  {
   TextBox tb;
   RichTextBox rb;
   if (ctrl is TextBox)
   {
    tb = (TextBox)ctrl;
    if (ro)
    {
     tb.ReadOnly = true;
    }
    else
    {
     tb.ReadOnly = false;
    }

    tb.BackColor = System.Drawing.Color.White;
   }

   if (ctrl is RichTextBox)
   {
    rb = (RichTextBox)ctrl;
    if (ro)
    {
     rb.ReadOnly = true;
    }
    else
    {
     rb.ReadOnly = false;
    }

    rb.BackColor = System.Drawing.Color.White;
   }

   if (enb)
   {
    ctrl.Enabled = true;
   }
   else
   {
    ctrl.Enabled = false;
   }
  }

  private void Patientid_KeyPress(object sender, KeyPressEventArgs e)
  {
   if ((Editmenuitem.Text ?? "") == "End Edit")
   {
    Data_altered = true;
    return;
   }

   if (Patientid.Text.Length == 0)
   {
    pidep.SetError(Patientid, "");
   }

   if (e.KeyChar == (char)(int)Keys.Return)
   {
    rethit = true;
    SendKeys.Send("{TAB}");
    e.Handled = true;
   }

   if (e.KeyChar == (char)(int)Keys.Tab)
   {
    tabhit = true;
    dest.Select();
    e.Handled = true;
   }
  }

  private void ResetToolStripMenuItem_Click(object sender, EventArgs e)
  {
   foreach (KeyValuePair<int, string> k in bl_used)
    bl_available[k.Key] = k.Value;
   bl_used.Clear();
   foreach (KeyValuePair<string, Blk_entry> k in bl_loaded)
   {
    k.Value.State = (int)Load_state.not_loaded;
    k.Value.Num = 0;
   }

   My.MySettingsProperty.Settings.endpoints = null;
   foreach (KeyValuePair<int, string> s in bl_available)
   {
    Controls[s.Value].Visible = false;
    Controls[s.Value].Controls[0].Text = string.Empty;
    Controls[s.Value].Controls[1].Text = string.Empty;
   }

   Scsiz(Width, originaly);
   My.MySettingsProperty.Settings.endpoints = new System.Collections.Specialized.StringCollection();
   foreach (string s in Mpck.Blocklist)
    My.MySettingsProperty.Settings.endpoints.Add(s);
   My.MySettingsProperty.Settings.Save();
   Endpoints_in_use = My.MySettingsProperty.Settings.endpoints;
   if (L2_ret is object)
   {
    foreach (string b in Endpoints_in_use)
    {
     if (L2_ret.blks.ContainsKey(b) && L2_ret.blks[b].Count > 0)
     {
      int bnum = bl_available.First().Key;
      bl_used[bl_available.First().Key] = bl_available.First().Value;
      bl_available.Remove(bl_available.First().Key);
      Controls[gpn[bnum]].Visible = false;
      Controls[gpn[bnum]].Controls[0].Text = b;
      Controls[gpn[bnum]].Controls[0].Tag = b;
      Controls[gpn[bnum]].Controls[0].Enabled = true;
      bl_loaded[b].State = (int)Load_state.loaded;
      bl_loaded[b].Num = bnum;
      Controls[gpn[bnum]].Controls[1].Text = string.Join(Constants.vbCrLf, L2_ret.blks[b]);
      Controls[gpn[bnum]].Controls[1].Enabled = true;
      Ath_setcolor(bnum);
      Controls[gpn[bnum]].Visible = true;
      labgb[gph[bnum]] = b;
      labgb[gpb[bnum]] = string.Join(Constants.vbCrLf, L2_ret.blks[b]);
     }
    }
   }

   Scsiz(Width);
   Set_empgb();
  }

  private void Scsiz(int Width, int Height = -1)
  {
   GroupBox lb;
   int br;
   if (Height != -1)
   {
    br = Height;
   }
   else if (bl_used.Count == 0)
   {
    br = originaly;
   }
   else
   {
    lb = (GroupBox)Controls[bl_used.Getlval()];
    br = lb.Location.Y + lb.Height + 76;
    if (bl_used.Count > 3)
    {
     if (Top + br > Screen_height)
     {
      dnh = false;
      Wanted_hgt = br;
      lb = (GroupBox)Controls[bl_used[bl_used.Count - 3]];
      br = lb.Location.Y + lb.Height + 76;
     }
     else
     {
      dnh = true;
      Wanted_hgt = 0;
     }
    }
   }

   Size = new Size(Width, br);
   Update();
  }

  private void Previewmenuitem_Click(object sender, EventArgs e)
  {
   var pfrm = new Form();
   int pccnt;
   int prcnt;
   PictureBox pb;
   var gblabels = new[] { "First Label", "Second Label", "Third Label", "Fourth Label", "Fifth Label", "Sixth Label" };
   Panel pnl;
   ContextMenuStrip Cms = null;
   ToolStripMenuItem Tsi = null;
   using (Form fpr = new Form3())
   {
    using (var tbl = new TableLayoutPanel())
    {
     Bitmap i1;
     Bitmap i2;
     fpr.Text = "Labels Preview";
     Pnglablist.Clear();
     Preview = true;
     Generate_Labels();
     if (Pnglablist.Count < 1)
     {
      return;
     }

     pccnt = Pnglablist.Count < 2 ? 1 : 2;
     prcnt = (Pnglablist.Count + 1) / pccnt;
     tbl.RowCount = prcnt;
     tbl.ColumnCount = pccnt;
     tbl.Width = 504 * pccnt;
     tbl.Height = 337 * prcnt;
     fpr.Width = tbl.Width;
     fpr.Height = tbl.Height;
     for (int i = 0, loopTo = prcnt - 1; i <= loopTo; i += 1)
     {
      for (int j = 0, loopTo1 = pccnt - 1; j <= loopTo1; j += 1)
      {
       int ind = i * pccnt + j;
       if (ind > Pnglablist.Count - 1)
       {
        break;
       }

       pnl = new Panel();
       pb = new PictureBox()
       {
        Width = 464,
        Height = 277,
        Left = 15,
        Top = 15,
        Anchor = AnchorStyles.None
       };
       using (var MS = new MemoryStream(Pnglablist[i * pccnt + j]))
       {
        i1 = (Bitmap)Image.FromStream(MS);
        i2 = ResizeImage(i1, (int)(i1.Width / (double)2), (int)(i1.Height / (double)2));
        pb.Image = i2;
       }

       pnl.Width = 504;
       pnl.Height = 337;
       var lab = new System.Windows.Forms.Label() { Text = gblabels[ind] };
       pnl.Controls.Add(lab);
       pnl.Controls[0].Left = 20;
       pnl.Controls[0].Top = 20;
       pnl.Controls.Add(pb);
       pb.CenterControl();
       pnl.Controls[1].Left = 20;
       pnl.Controls[1].Top = 45;
       Cms = new ContextMenuStrip();
       Tsi = new ToolStripMenuItem()
       {
        Text = "Print",
        Name = gblabels[ind]
       };
       Tsi.Click += My.MyProject.Forms.Form3.Print_Panel;
       Cms.Items.Add(Tsi);
       pnl.ContextMenuStrip = Cms;
       tbl.Controls.Add(pnl, j, i);
      }
     }

     fpr.Controls.Add(tbl);
     fpr.ShowDialog();
    }
   }

   Preview = false;
  }

  private void Editmenuitem_Click(object sender, EventArgs e)
  {
   bool bset;
   if ((Editmenuitem.Text ?? "") == "Edit")
   {
    Editmenuitem.Text = "End Edit";
    bset = false;
    Dstate = Data_state.Edit_mode;
    dsaves.AutoCompleteMode = AutoCompleteMode.None;
    dsaves.AutoCompleteSource = AutoCompleteSource.None;
    Patientid.Enabled = true;
    Patientid.ReadOnly = false;
    Patientid.BackColor = System.Drawing.Color.White;
    Patient.Enabled = true;
    Patient.ReadOnly = false;
    Patient.BackColor = System.Drawing.Color.White;
    inpat = false;
    inpid = false;
   }
   else
   {
    Editmenuitem.Text = "Edit";
    bset = true;
    Dstate = Data_state.NoEdit;
    dsaves.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
    dsaves.AutoCompleteSource = AutoCompleteSource.ListItems;
    Patientid.Enabled = false;
    Patientid.ReadOnly = true;
    Patientid.BackColor = System.Drawing.Color.White;
    Patient.Enabled = false;
    Patient.ReadOnly = true;
    Patient.BackColor = System.Drawing.Color.White;
   }

   // If prv_combo.Items.Count > 1 Then
   // priph.Visible = False
   // prv_combo.Visible = True
   // Else
   // prv_combo.Visible = False
   // priph.Visible = True
   // End If

   if (bset & prv_combo.Items.Count > 1)
   {
    prv_combo.Visible = false;
    priph.Visible = true;
   }
   else
   {
    prv_combo.Visible = true;
    priph.Visible = false;
   }

   Patientid.Enabled = true;
   ins.ReadOnly = bset;
   Patient.ReadOnly = bset;
   Practice.ReadOnly = bset;
   Patient.Enabled = true;
   Phone.ReadOnly = bset;
   DOB.ReadOnly = bset;
   address.ReadOnly = bset;
   econtact.ReadOnly = bset;
   if (sp.Visible)
   {
    secph.ReadOnly = bset;
   }

   foreach (KeyValuePair<int, string> k in bl_used)
   {
    TextBox tb = (TextBox)Controls[k.Value].Controls[0];
    tb.ReadOnly = bset;
    RichTextBox rb = (RichTextBox)Controls[k.Value].Controls[1];
    rb.ReadOnly = bset;
    Refresh();
   }
  }

  private void Notesmenuitem_Click(object sender, EventArgs e)
  {
   int bn;
   if (bl_used.Count == blocks_number)
   {
    Interaction.MsgBox("Unable to Notes Page", MsgBoxStyle.Information, "All are in use");
   }
   else
   {
    bn = bl_available.First().Key;
    bl_available.Remove(bn);
    bl_used[bn] = gpn[bn];
    Controls[gpn[bn]].Controls[0].Text = string.Empty;
    Controls[gpn[bn]].Controls[1].Text = string.Empty;
    Controls[gpn[bn]].Controls[0].Enabled = true;
    Controls[gpn[bn]].Controls[1].Enabled = true;
    Controls[gpn[bn]].Controls[0].Text = "Notes";
    Controls[gpn[bn]].Visible = true;
    Set_ro(Controls[gpn[bn]].Controls[0]);
    Set_ro(Controls[gpn[bn]].Controls[1]);
    labgb[gph[bn]] = Controls[gpn[bn]].Controls[0].Text;
    labgb[gpb[bn]] = string.Empty;
    Set_empgb();
    Scsiz(Width);
   }
  }

  private void Eltimer_set(object sender, EventArgs e)
  {
   ToolStripMenuItem T = (ToolStripMenuItem)sender;
   if (run_timer)
   {
    T.Text = "Use timer";
    run_timer = false;
    Tgp.Visible = false;
   }
   else
   {
    T.Text = "Stop using timer";
    run_timer = true;
    Tgp.Visible = true;
   }
  }

  private void Printmenuitem_Click(object sender, EventArgs e)
  {
   Generate_Labels();
  }

  private void Edit_Keypress(object sender, KeyPressEventArgs e)
  {
   if (!((Editmenuitem.Text ?? "") == "End Edit"))
   {
    e.Handled = true;
   }
   else
   {
    Data_altered = true;
   }
  }

  private void Prv_combo_KeyPress(object sender, KeyPressEventArgs e)
  {
   if (!((Editmenuitem.Text ?? "") == "End Edit"))
   {
    e.Handled = true;
   }

   Data_altered = true;
  }

  private void Printers_KeyPress(object sender, KeyPressEventArgs e)
  {
   if (!((Editmenuitem.Text ?? "") == "End Edit"))
   {
    e.Handled = true;
   }
   else
   {
    Data_altered = true;
   }
  }

  private void Practice_KeyPress(object sender, KeyPressEventArgs e)
  {
   if (!((Editmenuitem.Text ?? "") == "End Edit"))
   {
    e.Handled = true;
   }
   else
   {
    Data_altered = true;
   }
  }

  private void Department_KeyPress(object sender, KeyPressEventArgs e)
  {
   if (!((Editmenuitem.Text ?? "") == "End Edit"))
   {
    e.Handled = true;
   }
   else
   {
    Data_altered = true;
   }
  }

  private void Dept_tbox_KeyPress(object sender, KeyPressEventArgs e)
  {
   if (!((Editmenuitem.Text ?? "") == "End Edit"))
   {
    e.Handled = true;
   }
   else
   {
    Data_altered = true;
   }
  }

  private void Patientid_Validating(object sender, CancelEventArgs e)
  {
   if ((Editmenuitem.Text ?? "") == "End Edit")
   {
    return;
   }

   if (!tabhit & !rethit)
   {
    return;
   }

   if (Data_altered)
   {
    return;
   }

   valid_patientid = true;
   if (Patientid.Text.Length == 0)
   {
    Patient.ReadOnly = false;
    return;
   }

   if (!Regex.IsMatch(Patientid.Text, "^[0-9 ]+$"))
   {
    valid_patientid = false;
   }

   if (!valid_patientid)
   {
    e.Cancel = true;
    Patientid.Select(0, Patientid.Text.Length);
    pidep.SetError(Patientid, "A Patient id must consist of only numbers");
   }
  }

  private async void Patientid_Validated(object sender, EventArgs e)
  {
   var claims = Gen_Claims();
   string fs = null;
   string fn = null;
   if ((Editmenuitem.Text ?? "") == "End Edit")
   {
    return;
   }

   if (Data_altered)
   {
    return;
   }

   if (!tabhit & !rethit)
   {
    return;
   }

   if (valid_patientid)
   {
    pidep.SetError(Patientid, "");
    if (Patientid.Text.Length == 0)
    {
     return;
    }

    aws_body.Clear();
    aws_body["patientid"] = Patientid.Text;
    if (Department.SelectedIndex > 0)
    {
     aws_body["departmentid"] = atdp[Department.SelectedItem.ToString()];
    }

    Reset_fields();
    Text = "Medical Profile Card (" + installed_version + ")" + " - Loading Patient Information";
    Update();
    if (run_timer)
    {
     Ettb.Text = start_timer(SW);
    }

    Application.UseWaitCursor = true;
    L2_ret = await Aws.Get_Level2_aysnc(Mpck.Url, enck, Mpck.Salt, claims, aws_body);
    Application.UseWaitCursor = false;
    if (L2_ret.code != 200)
    {
     Interaction.MsgBox(L2_ret.message, MsgBoxStyle.OkOnly, "Medical Profile Card");
     Reset_fields();
     pidep.SetError(Patientid, "Patient not found");
     tabhit = false;
     rethit = false;
     Patientid.Select(0, Patientid.Text.Length);
     Patientid.HideSelection = false;
     Patientid.Focus();
     return;
    }

    Text = "Medical Profile Card (" + installed_version + ")";
    Update();
    Load_level2(L2_ret);
    if (run_timer)
    {
     Ettb.Text = stop_timer(SW);
    }

    inpid = false;
   }
  }

  private async void Patient_Validated(object sender, EventArgs e)
  {
   var claims = Gen_Claims();
   string fs = null;
   string fn = null;
   if ((Editmenuitem.Text ?? "") == "End Edit")
   {
    return;
   }

   if (!tabhit & !rethit)
   {
    return;
   }

   if (Data_altered)
   {
    return;
   }

   if (valid_patient)
   {
    patep.SetError(Patient, "");
    aws_body.Clear();
    aws_body["firstname"] = firstname;
    aws_body["lastname"] = lastname;
    if (!(Department.SelectedIndex == 0))
    {
     aws_body["departmentid"] = atdp[Department.SelectedItem.ToString()];
    }

    Text = "Medical Profile Card (" + installed_version + ")" + " - Loading Patient Information";
    Update();
    if (run_timer)
    {
     Ettb.Text = start_timer(SW);
    }

    Application.UseWaitCursor = true;
    L2_ret = await Aws.Get_Level2_aysnc(Mpck.Url, enck, Mpck.Salt, claims, aws_body);
    Application.UseWaitCursor = false;
    if (L2_ret.code != 200)
    {
     Interaction.MsgBox(L2_ret.message, MsgBoxStyle.OkOnly, "Medical Profile Card");
     Reset_fields();
     patep.SetError(Patient, "Patient not found");
     tabhit = false;
     rethit = false;
     Patient.Select(0, Patient.Text.Length);
     Patient.HideSelection = false;
     Patient.Focus();
     return;
    }

    Text = "Medical Profile Card (" + installed_version + ")";
    Update();
    Load_level2(L2_ret);
    if (run_timer)
    {
     Ettb.Text = stop_timer(SW);
    }

    inpat = false;
   }
  }

  private void Patient_Validating(object sender, CancelEventArgs e)
  {
   string[] np;
   string[] nl;
   firstname = "";
   lastname = "";
   if ((Editmenuitem.Text ?? "") == "End Edit")
   {
    return;
   }

   if (!tabhit & !rethit)
   {
    return;
   }

   if (Data_altered)
   {
    return;
   }

   if (Patient.Text.Length == 0)
   {
    return;
   }

   np = Patient.Text.Split(new char[] { ',' });
   nl = Patient.Text.Split(new char[] { ' ' });
   if (np.Count() == 2)
   {
    lastname = np[0];
    firstname = np[1];
   }
   else if (nl.Count() == 2)
   {
    firstname = nl[0];
    lastname = nl[1];
   }

   if (string.IsNullOrEmpty(lastname) | string.IsNullOrEmpty(firstname))
   {
    valid_patient = false;
   }
   else
   {
    valid_patient = true;
   }

   if (!valid_patient)
   {
    e.Cancel = true;
    Patient.Select(0, Patient.Text.Length);
    patep.SetError(Patient, "A patient name must be either <first> <last> or <last>,<first>");
   }
  }

  private void Form1_Closing(object sender, CancelEventArgs e)
  {
   e.Cancel = false;
  }

  private void Rtbbox_enter(object sender, EventArgs e)
  {
   RichTextBox rtb;
   rtb = (RichTextBox)sender;
   rtb.SelectionStart = 0;
  }

  private void Textbox_enter(object sender, EventArgs e)
  {
   TextBox tb;
   tb = (TextBox)sender;
   tb.SelectionStart = tb.Text.Length;
   tb.SelectionLength = 0;
  }

  private async void Patient_Enter(object sender, EventArgs e)
  {
   TextBox tb;
   if ((Editmenuitem.Text ?? "") == "End Edit")
   {
    return;
   }

   if (Check_altered())
   {
    await save_blk();
    Application.DoEvents();
    Reset_fields();
   }

   Data_altered = false;
   Dsi = new KeyValuePair<string, Dsave>();
   tb = (TextBox)sender;
   tb.SelectionStart = 0;
   if (inpat)
   {
    return;
   }

   valid_patient = false;
   patep.SetError(Patient, "");
   L2_ret = null;
   inpid = false;
   Patientid.ReadOnly = true;
   Patientid.BackColor = System.Drawing.Color.White;
   Patientid.Text = "";
   pidep.SetError(Patientid, "");
   Patient.ReadOnly = false;
   rethit = false;
   tabhit = false;
   if (!inpat)
   {
    Patient.Text = "";
   }

   valid_patient = false;
   inpat = true;
   Reset_fields();
   Deletemi.Enabled = false;
   Savemi.Enabled = false;
   dsaves.SelectedIndexChanged -= Dsaves_SelectedIndexChanged;
   dsaves.SelectedItem = null;
   dsaves.SelectedIndexChanged += Dsaves_SelectedIndexChanged;
   Scsiz(Width, originaly);
   Invalidate();
   Update();
  }

  private async void Patient_Click(object sender, EventArgs e)
  {
   if ((Editmenuitem.Text ?? "") == "End Edit")
   {
    return;
   }

   if (Check_altered())
   {
    await save_blk();
    Application.DoEvents();
   }

   Dsi = new KeyValuePair<string, Dsave>();
   Data_altered = false;
   L2_ret = null;
   if (inpat)
   {
    return;
   }

   inpat = true;
   inpid = false;
   Patientid.ReadOnly = true;
   Patientid.Enabled = false;
   Patientid.BackColor = System.Drawing.Color.White;
   Patientid.Text = "";
   pidep.SetError(Patientid, "");
   Patient.ReadOnly = false;
   Patient.Enabled = true;
   Patient.BackColor = System.Drawing.Color.White;
   if (!inpat)
   {
    Patient.Text = "";
   }

   rethit = false;
   tabhit = false;
   Reset_fields();
   Scsiz(Width, originaly);
   Invalidate();
   Update();
  }

  private void Patient_KeyPress(object sender, KeyPressEventArgs e)
  {
   if ((Editmenuitem.Text ?? "") == "End Edit")
   {
    Data_altered = true;
    return;
   }

   patep.SetError(Patient, "");
   if (Patient.Text.Length == 0)
   {
    patep.SetError(Patient, "");
   }

   if (e.KeyChar == (char)(int)Keys.Return)
   {
    rethit = true;
    SendKeys.Send("{TAB}");
    e.Handled = true;
   }

   if (e.KeyChar == (char)(int)Keys.Tab)
   {
    tabhit = true;
    dest.Select();
    e.Handled = true;
   }

   valid_patient = false;
  }

  private async void Savemi_Click(object sender, EventArgs e)
  {
   if (run_timer)
   {
    Ettb.Text = start_timer(SW);
   }

   var claims = Gen_Claims();
   string Wrtim = DateTime.Now.ToString("yyyyMMddHHmmss");
   var Sblk = new Save_blk();
   Sblock Sb;
   string js;
   string jse;
   Dsave_return dr;

   // Dim dsi As KeyValuePair(Of String, Dsave) = Nothing
   // Dim ds As Dsave = Nothing

   if (saved_type)
   {
    // Dsi = dsaves.SelectedItem
    Ds = Dsi.Value;
    if (string.IsNullOrEmpty(Patient.Text))
    {
     Patient.Text = Ds.Name;
    }
   }

   if (string.IsNullOrEmpty(Patient.Text))
   {
    Patient.Text = "Unknown_" + Wrtim;
   }

   if (!string.IsNullOrEmpty(Patient.Text) && Ds is object && !string.IsNullOrEmpty(Ds.Name))
   {
    if (!((Patient.Text ?? "") == (Ds.Name ?? "")))
    {
     saved_type = false;
    }
   }

   Sblk.Practice = Practice.Text;
   if (Department.Items.Count > 1)
   {
    Sblk.Department = Department.Text;
   }
   else
   {
    Sblk.Department = dept_tbox.Text;
   }

   Sblk.Patient = Patient.Text;
   Sblk.Patient_id = Patientid.Text;
   Sblk.Address = address.Text;
   Sblk.DOB = DOB.Text;
   Sblk.Emergency_contact = econtact.Text;
   Sblk.Insurance = ins.Text;
   if (atpv.Count > 1)
   {
    Sblk.Priph = prv_combo.Text;
   }
   else
   {
    Sblk.Priph = priph.Text;
   }

   Sblk.Phone = Phone.Text;
   Sblk.Prtitle = ppgb.Text;
   Sblk.Sptitle = sptitle.Text;
   Sblk.Secph = secph.Text;
   Sblk.Sec_visible = Mpck.Sec_visible;
   foreach (KeyValuePair<int, string> s in bl_used)
   {
    Sb = new Sblock();
    {
     var withBlock = Sb;
     withBlock.num = s.Key;
     withBlock.header = Controls[s.Value].Controls[0].Text;
     withBlock.body = Controls[s.Value].Controls[1].Text;
    }

    Sblk.Blk_list.Add(Sb);
   }

   aws_body.Clear();
   js = JsonConvert.SerializeObject(Sblk, Formatting.Indented);
   aws_body["skey"] = Enc.Enc256.Encrypt(Sblk.Patient, Enc.Enc256.Iterscramble(cid + Mpck.Dlab, Mpck.Iterations));
   var switchExpr = save_ver;
   switch (switchExpr)
   {
    case 1:
     {
      jse = Enc.Enc256.Encrypt(js, Enc.Enc256.Iterscramble(cid), Convert.ToInt32(Mpck.Iterations % 10 + 2));
      aws_body["skey"] = Enc.Enc256.Encrypt(Sblk.Patient, Enc.Enc256.Iterscramble(cid + Mpck.Dlab), Mpck.Iterations % 10 + 2);
      break;
     }

    default:
     {
      jse = Enc.Enc256.Encrypt(js, Enc.Enc256.Iterscramble(cid, Mpck.Iterations), Convert.ToInt32(Mpck.Iterations / (double)3));
      aws_body["skey"] = Enc.Enc256.Encrypt(Sblk.Patient, Enc.Enc256.Iterscramble(cid + Mpck.Dlab, Mpck.Iterations));
      break;
     }
   }

   aws_body["ukey"] = Enc.Enc256.Encrypt(cid + Mpck.Dlab, cid + Mpck.Dlab, Mpck.Iterations);
   if (saved_type)
   {
    if (string.IsNullOrEmpty(Ds.lwtim))
    {
     aws_body["wrtim"] = Wrtim;
     aws_body["lwtim"] = Wrtim;
    }
    else
    {
     aws_body["wrtim"] = Ds.wrtim;
     aws_body["lwtim"] = Wrtim;
    }
   }
   else
   {
    aws_body["wrtim"] = Wrtim;
    aws_body["lwtim"] = Wrtim;
   }

   aws_body["vers"] = save_ver.ToString();
   aws_body["value"] = jse;
   Application.UseWaitCursor = true;
   dr = await Aws.Pat_save_aysnc(Mpck.Url, enck, Mpck.Salt, claims, aws_body);
   Application.UseWaitCursor = false;
   Add_saved_item(dr.ds[0]);
   Data_altered = false;
   if (run_timer)
   {
    Ettb.Text = stop_timer(SW);
   }
  }

  private async void Deletemi_Click(object sender, EventArgs e)
  {
   Dsave dsv = (Dsave)dsaves.SelectedValue;
   KeyValuePair<string, Dsave> Dsvsv = (KeyValuePair<string, Dsave>)dsaves.SelectedItem;
   var claims = Gen_Claims();
   Dsave_return Dr;
   Reset_fields();
   aws_body.Clear();
   aws_body["ukey"] = Enc.Enc256.Encrypt(cid + Mpck.Dlab, cid + Mpck.Dlab, Mpck.Iterations);
   aws_body["wrtim"] = dsv.wrtim;
   Application.UseWaitCursor = true;
   Dr = await Aws.Pat_delete_aysnc(Mpck.Url, enck, Mpck.Salt, claims, aws_body);
   Application.UseWaitCursor = false;
   Delete_saved_item(Dsvsv.Key);
   Data_altered = false;
   Patient.Text = null;
   Patientid.Text = "";
   Patientid.Focus();
  }

  private void WireAllEvents(object obj)
  {
   var parameterTypes = new[] { typeof(object), typeof(EventArgs) };
   var Events = obj.GetType().GetEvents();
   foreach (var ev in Events)
   {
    if (ev.Name.StartsWith("Format"))
    {
     continue;
    }

    if (ev.Name.StartsWith("Mouse"))
    {
     continue;
    }

    var handler = new DynamicMethod("", null, parameterTypes, typeof(Form1));
    var ilgen = handler.GetILGenerator();
    ilgen.EmitWriteLine("Event Name: " + ev.Name);
    ilgen.Emit(OpCodes.Ret);
    ev.AddEventHandler(obj, handler.CreateDelegate(ev.EventHandlerType));
   }
  }

  private async void Dsaves_SelectedIndexChanged(object sender, EventArgs e)
  {
   ComboBox cb = (ComboBox)sender;
   dsaves.SelectionStart = dsaves.Text.Length;
   if (!string.IsNullOrEmpty(Dsi.Key))
   {
    KeyValuePair<string, Dsave> Ti = (KeyValuePair<string, Dsave>)dsaves.SelectedItem;
    if ((Dsi.Key ?? "") == (Ti.Key ?? ""))
    {
     dsaves.SelectionStart = dsaves.Text.Length;
     dest.Select();
     return;
    }
   }

   if (run_timer)
   {
    Ettb.Text = start_timer(SW);
   }

   if (Data_altered)
   {
    if (Check_altered())
    {
     int ds_index = dsaves.SelectedIndex;
     await save_blk();
     Application.DoEvents();
     dsaves.SelectedIndexChanged -= Dsaves_SelectedIndexChanged;
     dsaves.SelectedIndex = ds_index;
     dsaves.SelectedIndexChanged += Dsaves_SelectedIndexChanged;
    }
   }

   Dsave dsv = (Dsave)dsaves.SelectedValue;
   var claims = Gen_Claims();
   Dsave_return Dvr;
   var Sblk = new Save_blk();
   string js = null;
   var svsel = dsaves.SelectedItem;
   Reset_fields(true);
   dest.Select();
   dsaves.SelectedIndexChanged -= Dsaves_SelectedIndexChanged;
   dsaves.SelectedItem = svsel;
   dsaves.SelectionStart = dsaves.SelectionLength;
   dsaves.SelectedIndexChanged += Dsaves_SelectedIndexChanged;
   Patient.Text = null;
   Patient.ReadOnly = true;
   Patientid.BackColor = System.Drawing.Color.White;
   Patientid.Text = "";
   Patientid.ReadOnly = true;
   inpat = false;
   inpid = false;
   aws_body.Clear();
   aws_body["ukey"] = Enc.Enc256.Encrypt(cid + Mpck.Dlab, cid + Mpck.Dlab, Mpck.Iterations);
   aws_body["skey"] = dsv.Skey;
   aws_body["wrtim"] = dsv.wrtim;
   Application.UseWaitCursor = true;
   Dvr = await Aws.Dsave_get_aysnc(Mpck.Url, enck, Mpck.Salt, claims, aws_body);
   Application.UseWaitCursor = false;
   if (Dvr.Dsave_value is object)
   {
    var switchExpr = dsv.vers;
    switch (switchExpr)
    {
     case var @case when @case == Conversions.ToString(1):
      {
       js = Enc.Enc256.Decrypt(Dvr.Dsave_value, Enc.Enc256.Iterscramble(cid), Convert.ToInt32(Mpck.Iterations % 10 + 2));
       break;
      }

     default:
      {
       js = Enc.Enc256.Decrypt(Dvr.Dsave_value, Enc.Enc256.Iterscramble(cid, Mpck.Iterations), Convert.ToInt32(Mpck.Iterations / (double)3));
       break;
      }
    }

    if (js == default)
    {
     MessageBox.Show("Unable to load saved data", "Error", MessageBoxButtons.OK);
     return;
    }

    Sblk = JsonConvert.DeserializeObject<Save_blk>(js);
    if (Sblk is null)
    {
     MessageBox.Show("Unable to load saved data", "Error", MessageBoxButtons.OK);
     return;
    }

    Load_saved(Sblk);
    Deletemi.Enabled = true;
   }

   if (run_timer)
   {
    Ettb.Text = stop_timer(SW);
   }
  }

  private void prv_combo_TextChanged(object sender, EventArgs e)
  {
   prv_combo.SelectionLength = 0;
  }

  private void dept_tbox_MouseEnter(object sender, EventArgs e)
  {
   dept_tbox.SelectionStart = 0;
  }

  private void Printers_MouseEnter(object sender, EventArgs e)
  {
   Printers.SelectionStart = 0;
  }

  private void dsaves_MouseEnter(object sender, EventArgs e)
  {
   dsaves.SelectionStart = 0;
  }

  private void dsaves_Enter(object sender, EventArgs e)
  {
   dsaves.SelectionStart = 0;
  }

  private void Cb_Enter(object sender, EventArgs e)
  {
   ComboBox Cb = (ComboBox)sender;
   Cb.SelectionStart = Cb.Text.Length;
   // Cb.SelectionLength = 0
  }

  private void Cb_DropDown(object sender, EventArgs e)
  {
   ComboBox Cb = (ComboBox)sender;
   Cb.SelectionLength = 0;
   if ((Cb.Name ?? "") == "dsaves")
   {
    Console.WriteLine("dsaves selected[" + dsaves.SelectedText + "]");
   }

   string St = Cb.Text;
  }

  private void Cb_DrawItem(object sender, DrawItemEventArgs e)
  {
   string str;
   ComboBox Cb = (ComboBox)sender;
   Cb.SelectionStart = Cb.SelectionLength;
   var G = e.Graphics;
   var R = e.Bounds;
   var bc = System.Drawing.Color.FromArgb(225, 227, 225);
   if (e.Index < 0)
   {
    return;
   }

   if ((Cb.Name ?? "") == "dsaves")
   {
    str = ((KeyValuePair<string, Dsave>)Cb.Items[e.Index]).Key;
   }
   else
   {
    str = (string)Cb.Items[e.Index];
   }

   e.Graphics.FillRectangle(new SolidBrush(bc), R);
   if (Conversions.ToBoolean(e.State & DrawItemState.Selected))
   {
    e.Graphics.DrawString(str, scb_font, Brushes.Black, R);
   }
   else
   {
    e.Graphics.DrawString(str, scr_font, Brushes.Black, R);
   }

   e.DrawFocusRectangle();
  }

  private void Cb_TextChanged(object sender, EventArgs e)
  {
   ComboBox Cb = (ComboBox)sender;
   Cb.SelectionStart = Cb.Text.Length;
  }

  private void Cb_DropDownClosed(object sender, EventArgs e)
  {
   ComboBox Cb = (ComboBox)sender;
   Cb.SelectionStart = Cb.Text.Length;
  }

  private void prv_combo_SelectedIndexChanged(object sender, EventArgs e)
  {
   ComboBox Cb = (ComboBox)sender;
   priph.Text = prv_combo.Text;
   dest.Select();
  }

  private void Printers_SelectedIndexChanged(object sender, EventArgs e)
  {
   ComboBox Cb = (ComboBox)sender;
   Cb.SelectionStart = Cb.Text.Length;
   dest.Select();
  }

  private void Department_SelectedIndexChanged(object sender, EventArgs e)
  {
   ComboBox Cb = (ComboBox)sender;
   Cb.SelectionStart = Cb.Text.Length;
   dest.Select();
  }

  private void Cb_Validated(object sender, EventArgs e)
  {
   ComboBox Cb = (ComboBox)sender;
   Cb.SelectionStart = Cb.Text.Length;
  }

  private void dest_Enter(object sender, EventArgs e)
  {
   Interaction.MsgBox("Dest", MsgBoxStyle.OkOnly, "Dest Enter");
  }
 }
}