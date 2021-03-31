using System;
using System.Collections.Generic;
using global::System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using global::System.IO;
using System.Linq;
using global::System.Reflection.Emit;
using global::System.Text.RegularExpressions;
using System.Windows.Forms;
using global::Amazon;
using global::Amazon.DynamoDBv2;
using global::Amazon.DynamoDBv2.DocumentModel;
using global::DYMO.Label.Framework;
using global::JWT;
using global::Microsoft.Win32;
using global::Newtonsoft.Json;
using System.Text;
using Enc;
using System.Threading.Tasks;
using JR.Utils.GUI.Forms;

namespace Medical_Profile
{
 public partial class Form1 : Form
 {
  private System.Collections.Specialized.StringCollection Endpoints_in_use = new System.Collections.Specialized.StringCollection();
  private string drive_label = null;
  public string cid = null;
  private string ftime = null;
  private object iLock = new object();
  private char[] cv = new char[17];

  private RegistryKey Rkey = null;
  private List<string> Rkeynames = new List<string>();
  private Dictionary<string, object> Rkeyvals = new Dictionary<string, object>();

  private int Ymargin = 6;
  private int Xmargin = 18;
  private int margin3 = 10;
  private int label_header = 345;
  private float ll = (float)(3600 / 15.0);
  private int nlead = 191;
  private int ylimit = 2386;
  int Xsize = 365;
  int Ysize = 192;
  int Blockno = 1;
  private delegate void cb();

  private delegate void InvokeDelegate();

  private bool Preview = false;
  public MPC_key Mpck = null;

#if DEBUG
  public string User_Name;
  public string Machine_Name;
#endif
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

  public Form1()
  {
   try
   {
    SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
    InitializeComponent();
    Size = new Size(1188, Height);
    dept_tbox.Visible = false;
    Visible = false;

    var myScreen = Screen.FromControl(this);

    var area = myScreen.WorkingArea;

    Screen_width = area.Size.Width;
    Screen_height = area.Size.Height;

    cv[5] = Convert.ToChar(title[19] + 16);
    cv[6] = Convert.ToChar(title[9]);
    cv[7] = Convert.ToChar(title[33]);
    cv[8] = Convert.ToChar(title[13] - 8);
    cv[10] = Convert.ToChar(title[1]);
    cv[11] = Convert.ToChar(title[36]);

    if (Properties.Settings.Default.first_run_flag)
    {
     first_run = true;
     Properties.Settings.Default.first_run_flag = false;
     Properties.Settings.Default.Save();
    }
    else
    {
     first_run = false;
    }

    cv[12] = Convert.ToChar(title[31]);
    cv[13] = Convert.ToChar(title[9] + 10);
    cv[15] = Convert.ToChar(title[33] + 5);
    cv[3] = Convert.ToChar(title[3] + 6);
    cv[1] = Convert.ToChar(title[10]);
    cv[2] = Convert.ToChar(title[18]);

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
     Properties.Settings.Default.Save();
     Application.Exit();
    }
   }
   catch (Exception ex)
   {
    MessageBox.Show(ex.Message);
   }

   cv[9] = Convert.ToChar(title[0] + 32);
   cv[16] = Convert.ToChar(title[26] - 48);
   cv[0] = Convert.ToChar(title[2] - 32);
   cv[4] = Convert.ToChar(title[5] - 64);
   cv[14] = Convert.ToChar(title[23] + 16);

   Rkey = Registry.CurrentUser.OpenSubKey(@"Software\Medical_Profile", true);
   if (Rkey is object)
    Rkeynames = Rkey.GetValueNames().ToList();
   else
   {
    Rkey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\Medical_Profile");
    Rkey.SetValue("Path", Path.GetDirectoryName(Application.ExecutablePath));
    Rkey.SetValue("Version", "?.?.?");
    Rkey = Registry.CurrentUser.OpenSubKey(@"Software\Medical_Profile", true);
   }

   Rkeynames = Rkey.GetValueNames().ToList();

   foreach (string Vn in Rkeynames)
   {
    Rkeyvals[Vn] = Rkey.GetValue(Vn, null);
   }

   if (Rkeyvals.ContainsKey("Cid"))
    cid = (string)Rkeyvals["Cid"];

   if (cid == default)
   {
    cid = Guid.NewGuid().ToString();
    Rkey.SetValue("Cid", cid);
   }

   Rkey.Dispose();
  }

  private string Handle_file(string eval)
  {
   MPC_type mty = null;
   string rjson = Enc256.Decrypt(eval, enck, 17531);
   if (string.IsNullOrEmpty(rjson))
   {
    return "1";
   }

   mty = JsonConvert.DeserializeObject<MPC_type>(rjson);

   if (mty == null)
    return "1";

   if (string.IsNullOrEmpty(mty.F1))
    return "1";

   rjson = Enc256.Decrypt(mty.F1, enck, mty.Akey);

   if (string.IsNullOrEmpty(rjson))
    return "1";

   Mpck = JsonConvert.DeserializeObject<MPC_key>(rjson);

   if (Mpck == null)
    return "1";

   if (!string.IsNullOrEmpty(mty.Akey))
   {
    drive_label = mty.Akey;
   }

   if (!string.IsNullOrEmpty(mty.F1))
   {
    rjson = Enc256.Decrypt(mty.F1, enck, mty.Akey);
    if (rjson is null)
    {
     MessageBox.Show("Key file is incorrect.", "Error", MessageBoxButtons.OK);
     Application.Exit();
    }

    Mpck = JsonConvert.DeserializeObject<MPC_key>(rjson);
   }

   return "0";
  }

  private string Set_key(string s1, string s2)
  {
   int v1 = 0;
   for (int i = 3, loopTo = s2.Length - 1; i <= loopTo; i++)
    v1 += Convert.ToInt32(s2.Substring(i, 1));
   int insert_loc = v1 % (s2.Length - 4);
   return s1.Substring(0, insert_loc) + s2 + s1.Substring(insert_loc);
  }

#if DEBUG
  private int Handle_testmode(string un)
  {
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

   Mpck = new MPC_key()
   {
    Url = mpu.Url,
    Mkey = mpu.Mkey,
    Salt = mpu.Salt,
    Email = mpu.Email,
    Dlab = mpu.Disk_Label,
    Secret = mpu.Secret1,
    Iterations = mpu.Iterations,
    Blocklist = mpu.Blocklist,
    K1 = mpu.K1,
    Minimum_blocks = mpu.Minimum_blocks,
    Sec_visible = Convert.ToBoolean(mpu.Sec_visible),
    Sptitle = mpu.Sptitle,
    Version = mpu.Version
   };

   User_Name = mpu.UserName;
   Machine_Name = mpu.MachineName;
   cid = mpu.cid;
   drive_label = mpu.Disk_Label;
   file_access = true;
   return 0;
  }
#endif
  private int Handle_usb(string dle)
  {
   string fs;
   string it;
   string d1 = null;
   var drivesrem = DriveInfo.GetDrives().Where(drive => drive.DriveType == DriveType.Removable).ToArray();
   foreach (DriveInfo d in drivesrem)
   {
    if (d.VolumeLabel.StartsWith("MPC"))
    {
     d1 = Enc256.Decrypt(dle, enck, d.VolumeLabel);
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
       string rjson = Enc256.Decrypt(fs, enck, drive_label);
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
   htb.Append(new string(' ', bn), lfnt, Colors.Black);
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

#if DEBUG
    label.SaveToFile(lns + ".label");
#endif
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
     y_space_needed += nlead / 2;
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

   pname = Patient.Text;
   pdob = DOB.Text;
   Label = (DieCutLabel)Framework.Open("mpc1.label");
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
       stb.Append("\n", reg_font, Colors.Black);
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
     stb.Append("\n", rp1_font, Colors.Black);
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
   Output_label(ref pjob, ref Label, labelno);
   currenty = New_label(ref Label, ref labelno);

   stb = new StyledTextBuilder();

   foreach (KeyValuePair<int, string> K in bl_used)
   {
    int Bnum = K.Key;
    GroupBox Gb = Grpblocks[Bnum];

    if (Label is null)
    {
     currenty = New_label(ref Label, ref labelno);
    }

    if (labelno > labels_number)
    {
     break;
    }

    if (!string.IsNullOrEmpty(Gb.Controls[0].Text))
    {
     y_space_needed = 0;

     if (string.IsNullOrEmpty(Gb.Controls[0].Text))
     {
      y_space_needed += nlead;
     }

     if (string.IsNullOrEmpty(Gb.Controls[1].Text))
     {
      y_space_needed += nlead;
     }

     if (currenty > label_header & !string.IsNullOrEmpty(Gb.Controls[0].Text))
     {
      y_space_needed += nlead / 2;
     }

     if (ylimit - currenty < y_space_needed)
     {
      Output_label(ref pjob, ref Label, labelno);
      currenty = New_label(ref Label, ref labelno);
      if (labelno > labels_number)
      {
       break;
      }
     }

     if (currenty > label_header & !string.IsNullOrEmpty(Gb.Controls[0].Text))
     {
      currenty += nlead / 2;
     }

     stb = new StyledTextBuilder();
     if (!string.IsNullOrEmpty(Gb.Controls[0].Text))
     {
      stb.Append(Gb.Controls[0].Text, bld_font, Colors.Black);
      currenty = Lab_field("L" + currenty.ToString(), stb, 317, currenty, ll, nlead);
     }
    }

    if (!string.IsNullOrEmpty(Gb.Controls[1].Text))
    {
     lis = Gb.Controls[1].Text;
     elines.Clear();
     if ((lines_setting ?? "") == "Label")
     {
      lis = this.Adjust_lines(lis, "File");
     }

     lis = Adjust_lines(lis, "Label", elines);
     li = Regex.Split(lis, @"\r\n|\n");
     int lic = li.Count();
     if (max_rec[Bnum] > 0 & lic > max_rec[Bnum])
     {
      lic = max_rec[Bnum];
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

     for (int i = 0; i <= li.Count() - 1; i++)
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
       Output_label(ref pjob, ref Label, labelno);
       currenty = New_label(ref Label, ref labelno);
       if (labelno > labels_number)
       {
        break;
       }
      }
     }
    }
   }
   if (currenty > label_header)
   {
    Output_label(ref pjob, ref Label, labelno);
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

  private void Set_Size(int Xval, int Yval)
  {
   int Oldmax = MaximumSize.Height;
   MinimumSize = new Size(Xval, Originaly + H_delta);
   MaximumSize = new Size(0, 0);
   Size = new Size(Xval, Yval);
   if (Pe == null)
    MaximumSize = new Size(Xval, Yval);
   else
    MaximumSize = new Size(Xval, Pe.Bottom + H_delta + 2 * Ymargin);
  }
  private GroupBox gen_gb(int blockno, int xpos, int ypos)
  {
   int tboxsz = 20;

   ToolStripMenuItem tsi;
   ToolStripMenuItem ins;
   ToolStripMenuItem swp;
   ContextMenuStrip cm;
   GroupBox obox;

   var bi = new Blk_info() { num = blockno };

   blocks[blockno] = bi;
   max_rec[blockno] = 0;
   fieldsmr["MR" + blockno.ToString() + ":"] = blockno;

   obox = new GroupBox()
   {
    Size = new Size(Xsize, Ysize),
    Location = new Point(xpos, ypos),
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
    Size = new Size(Xsize - 2 * Ymargin, tboxsz),
    Location = new Point(Ymargin, margin3),
    Name = obox.Name + "H:",
    Tag = blockno,
    Font = new Font("Calibri", 8.25F, System.Drawing.FontStyle.Bold, GraphicsUnit.Point),
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
    Size = new Size(Xsize - 2 * Ymargin, Ysize - 2 * Ymargin - margin3 - tboxsz),
    Location = new Point(Ymargin, tboxsz + Ymargin + margin3),
    Name = obox.Name + "B:",
    Font = new Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, GraphicsUnit.Point),
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
   return obox;
  }
  public async Task<string> Get_Eval(MPC_key Mk)
  {
   DateTimeOffset nw = new DateTimeOffset();

   var key = Registry.CurrentUser.OpenSubKey(@"Software\Medical_Profile", true);
   string salt;
   string New_Eval = null;
   Ckup_Return mcd;
   var aws_body = new Dictionary<string, object>();

   nw = DateTime.UtcNow;
   nw = nw.AddYears(1);
   IDateTimeProvider provider = new UtcDateTimeProvider();

   string Cid = key.GetValue("Cid", null).ToString();

   if (string.IsNullOrEmpty(Cid))
    return null;

   string Cid_encrypted = Enc256.Encrypt(Cid, Enc256.Scramble(enck));
   salt = Enc256.Getsalt(Cid_encrypted);

   var payload = new Dictionary<string, object>() { { "aud", "http://medicalprofilecard.com" }, { "exp", nw.ToUnixTimeSeconds() }, { "update", Cid_encrypted } };

   payload["cid"] = Cid;
   payload["User_Name"] = Environment.UserName;
   payload["Machine_Name"] = Environment.MachineName;


   aws_body["vector_code"] = "4162";
   Application.UseWaitCursor = true;

   // Mk.Url = "https://pu0r0ghtw8.execute-api.us-east-2.amazonaws.com/dev";

   mcd = await Aws.Update_aysnc(Mk.Url, enck, salt, payload, aws_body);
   Application.UseWaitCursor = false;

   if (!string.IsNullOrEmpty(mcd.eval))
   {
    New_Eval = Enc256.Decrypt(mcd.eval, Enc256.Scramble(enck), 18926);
    if (!string.IsNullOrEmpty(New_Eval))
    {
     key.SetValue("eval", New_Eval);
    }
   }
   return New_Eval;
  }

  public async void Form1_LoadAsync(object sender, EventArgs e)
  {
   ContextMenuStrip cm;

   loading = true;

   if (run_timer)
   {
    Tgp.Visible = true;
   }

   Visible = false;
   DoubleBuffered = true;
   file_access = false;
   enck = new string(cv);



   //var key = Registry.CurrentUser.OpenSubKey(@"Software\Medical_Profile");

   drive_label_encoded = null;
   eval_encoded = null;
   ath_blist.Clear();

   if (Rkeyvals.ContainsKey("Version"))
    installed_version = (string)Rkeyvals["Version"];
   // if (key != null)
   // installed_version = key.GetValue("version", null).ToString();

#if DEBUG
   if (!string.IsNullOrEmpty(Properties.Settings.Default.User_number))
   {
    testmode = true;
    Handle_testmode(Properties.Settings.Default.User_number);
   }
#endif

   if (!testmode)
   {
    if (Rkeyvals.ContainsKey("eval"))
     eval_encoded = (string)Rkeyvals["eval"];

    //if (key != null)
    //{
    // eval_encoded = key.GetValue("eval", null)?.ToString();
    if (eval_encoded is object)
    {
     file_access = true;
    }
    //}

    if (!file_access)
    {
     //if (key != null)
     // drive_label_encoded = key.GetValue("Label", null)?.ToString();

     if (Rkeyvals.ContainsKey("Label"))
      drive_label_encoded = (string)Rkeyvals["Label"];

     if (drive_label_encoded is null)
     {
      using (Form f4 = new Formaik(cv))
      {
       f4.ShowDialog();

       //eval_encoded = key.GetValue("eval", null)?.ToString(); ;

       using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Medical_Profile"))
       {
        eval_encoded = key.GetValue("eval", null)?.ToString();
       }

       if (!string.IsNullOrEmpty(eval_encoded))
       {
        file_access = true;
       }
      }

      if (!file_access)
      {
       MessageBox.Show("Application can not run", "Registry entry is missing", MessageBoxButtons.OK);
       Close();
       Environment.Exit(0);
       return;
      }
     }
    }

    if (file_access)
    {
     if (Convert.ToDouble(Handle_file(eval_encoded)) != 0)
     {
      Application.Exit();
      Environment.Exit(0);
      return;
     }
    }
    else if (Handle_usb(drive_label_encoded) != 0)
    {
     Application.Exit();
     Environment.Exit(0);
     return;
    }
   }

   if (Properties.Settings.Default.endpoints is null)
   {
    Properties.Settings.Default.endpoints = new System.Collections.Specialized.StringCollection();
   }

   if (Properties.Settings.Default.endpoints.Count == 0)
   {
    string json = JsonConvert.SerializeObject(Mpck, Formatting.Indented);
    foreach (string s in Mpck.Blocklist)
     Properties.Settings.Default.endpoints.Add(s);
    Properties.Settings.Default.Save();
   }

   foreach (string s in Properties.Settings.Default.endpoints)
    Endpoints_in_use.Add(s);
   secph.Text = "";
   sp.Text = "";
   if (string.IsNullOrEmpty(installed_version))
   {
    installed_version = Mpck.Version;
   }

   if (Mpck.Sec_visible)
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

   minimum_blocks = Convert.ToInt32(Mpck.Minimum_blocks);
   blocks_number = 9;
   int BPR = 3;
   lines_number = 10;
   labels_number = 5;

   total_lines = lines_number * labels_number;

   Patientid.Text = "";

   Screen_Area = Screen.PrimaryScreen.WorkingArea;

   Originaly = ClientSize.Height;

   Ystart = Originaly;

   int Yval = Ymargin;

   for (int y = 1; y <= blocks_number / BPR; y++)
   {
    int Xval = Xmargin;
    for (int x = 1; x <= BPR; x++)
    {
     var gbg = gen_gb(Blockno, Xval, Yval);
     Grpblocks[Blockno] = gbg;
     bl_available[Blockno] = gbg.Name;
     Xval += Xsize + Xmargin;
     Blockno++;
    }
    Yval += Ysize + Ymargin;
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
    Label = (DieCutLabel)Framework.Open("mpc1.label");
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
    string S = Program.Format_exception(ex);

    FlexibleMessageBox.FONT = new Font("Calibri", 10, System.Drawing.FontStyle.Bold);
    FlexibleMessageBox.Show(S, "Exception Occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
    MessageBox.Show(ex.Message);
    Application.Exit();
    return;
   }

   Printers.Items.Clear();
   pnames = Framework.GetPrinters();
   if (pnames != null && pnames.Count() > 0)
   {
    foreach (var p in pnames)
     Printers.Items.Add(p.Name);
    if (Printers.Items.Count > 0)
    {
     Printers.SelectedIndex = 0;
    }

    printer = pnames.GetPrinterByName(Printers.SelectedItem.ToString());


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
   }
   else
   {
    Printmenuitem.Visible = false;
    Printmenuitem.Enabled = false;
   }

   Reset_fields();
   if (Properties.Settings.Default.first_run_flag)
   {
    Properties.Settings.Default.first_run_flag = false;
    Properties.Settings.Default.Save();
    Close();
   }

   priph.Visible = true;
   prv_combo.Visible = false;
   Text = "Medical Profile Card (" + installed_version + ")";
   var claims = Gen_Claims();
   aws_body.Clear();
   aws_body["ukey"] = Enc256.Encrypt(cid + Mpck.Dlab, cid + Mpck.Dlab, Mpck.Iterations);
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
    MessageBox.Show("Returned Code(" + L1_ret.code.ToString() + ") - " + L1_ret.message, "Error getting practice information", MessageBoxButtons.OK);
    System.Windows.Forms.Application.Exit();
    Application.DoEvents();
    this.Close();
    return;
   }

   Text = "Medical Profile Card (" + installed_version + ")";

   Update();

   if (!string.IsNullOrEmpty(L1_ret.Eval))
   {
    string New_Eval = Enc256.Decrypt(L1_ret.Eval, Enc256.Scramble(enck), 18926);
    if (!string.IsNullOrEmpty(New_Eval))
    {
     if (Handle_file(New_Eval) == "0")
     {
      var Knv = Registry.CurrentUser.OpenSubKey(@"Software\Medical_Profile", true);
      Knv.SetValue("eval", New_Eval);
      Knv.Close();
     }
    }
   }

   if (L1_ret?.Prc is null)
   {
    MessageBox.Show("Unable to get practice information", "Error in information returned");
    System.Windows.Forms.Application.Exit();
    Application.DoEvents();
    Close();
    return;
   }

   Dstate = Data_state.Edit_mode;
   Practice.Text = L1_ret?.Prc?.name;
   Load_Department(L1_ret);
   Load_providers(L1_ret);

   bl_loaded.Clear();

   Set_saved_items(L1_ret.dsl);

   Patientid.Select();

   Set_empgb();

   loading = false;
   Data_altered = false;
   Dstate = Data_state.NoEdit;
   if (run_timer)
   {
    Ettb.Text = stop_timer(SW);
   }
  }

  public Dictionary<string, object> Gen_Claims()
  {
   var nw = DateTimeOffset.UtcNow;

#if DEBUG
   var ew = nw.AddYears(1);
#else
   var ew = nw.AddMinutes(5);
#endif

   IDateTimeProvider provider = new UtcDateTimeProvider();
   var payload = new Dictionary<string, object>() { { "aud", "http: //medicalprofilecard.com" }, { "exp", ew.ToUnixTimeSeconds() }, { "iss", Mpck.Email }, { "label", drive_label }, { "key", Mpck.Mkey } };
   if (file_access)
   {
    payload["cid"] = Enc256.Encrypt(cid, Set_key(Mpck.Secret, Mpck.Dlab), Convert.ToInt32(Mpck.Dlab.Substring(3)) + Mpck.Iterations);
    payload["f1t"] = USB_Ftime.ToUnixTimeSeconds();
   }

   if (ftime is object && Convert.ToInt32(ftime) > 0)
   {
    payload["f1t"] = ftime;
   }

#if DEBUG
   if (testmode)
   {
    if (!string.IsNullOrEmpty(User_Name))
     payload["User_Name"] = User_Name;

    if (!string.IsNullOrEmpty(Machine_Name))
     payload["Machine_Name"] = Machine_Name;
   }
   else
   {
#endif
    payload["User_Name"] = Environment.UserName;
    payload["Machine_Name"] = Environment.MachineName;
#if DEBUG
   }
#endif
   return payload;
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

  private void Leave_fld(object sender, EventArgs e)
  {
   RichTextBox rtb;
   TextBox tb;

   Control C1 = sender as Control;

   String Name = C1.Name;

   string Type = C1.GetType().Name;

   if (!Name.StartsWith("GB"))
    return;

   int Num = Convert.ToInt32(Name.Substring(2, 1));

   if (Type == "RichTextBox")
   {
    rtb = C1 as RichTextBox;

    tb = rtb.Parent.Controls[0] as TextBox;

    if (labgb.ContainsKey(Name))
    {
     if (!((labgb[Name] ?? "") == (rtb.Text ?? "")))
     {
      labgb[Name] = rtb.Text;

      if (bl_loaded.ContainsKey(tb.Tag.ToString()))
      {
       bl_loaded[tb.Tag.ToString()].Num = default;
       bl_loaded[tb.Tag.ToString()].State = (int)Load_state.not_loaded_byc;
      }

      Set_empgb();
     }
    }
    else
    {
     labgb[Name] = rtb.Text;
    }
   }
   else
   {
    tb = C1 as TextBox;
    if ((tb.Tag.ToString() ?? "") != (tb.Text ?? ""))
    {
     if (bl_loaded.ContainsKey(tb.Tag.ToString()))
     {
      bl_loaded[tb.Tag.ToString()].Num = default;
      bl_loaded[tb.Tag.ToString()].State = (int)Load_state.not_loaded_byc;
     }

     Set_empgb();
    }

    if (labgb.ContainsKey(Name))
    {
     if (!((labgb[Name] ?? "") == (tb.Text ?? "")))
     {
      labgb[Name] = tb.Text;
     }
    }
    else
    {
     labgb[Name] = tb.Text;
    }
   }
  }

  private void Load_block_click(object sender, EventArgs e)
  {
   ToolStripMenuItem item = (ToolStripMenuItem)sender;
   string blk = item.Tag.ToString();
   if (bl_used.Count == blocks_number)
   {
    MessageBox.Show("Unable to load " + blk, "All Blocks are in use");
   }
   else
   {
    if (L2_ret.blks.ContainsKey(blk) && L2_ret.blks[blk].Count > 0)
    {
     int fnum = bl_available.First().Key;
     bl_used[fnum] = bl_available.First().Value;
     int lnum = bl_used.Getlkey();
     bl_available.Remove(fnum);
     Grpblocks[lnum].Visible = false;
     Grpblocks[lnum].Controls[0].Text = blk;
     Grpblocks[lnum].Controls[0].Tag = blk;
     Grpblocks[lnum].Controls[0].Enabled = true;
     Grpblocks[lnum].Controls[1].Text = string.Join("\n", L2_ret.blks[blk]);
     Grpblocks[lnum].Controls[1].Enabled = true;
     Ath_setcolor(lnum);
     Grpblocks[lnum].Visible = true;
     labgb[gph[lnum]] = blk.ToString();
     labgb[gpb[lnum]] = string.Join("\n", L2_ret.blks[blk]);
     var blke = new Blk_entry()
     {
      header = blk,
      State = (int)Load_state.loaded,
      Num = lnum
     };
     bl_loaded[blk] = blke;
    }

    Scsiz(Width);
    Update();
    Set_empgb();
   }
  }

  private void Menudrp_Click(object sender, EventArgs e)
  {
   int objn = Convert.ToInt32(((ToolStripMenuItem)sender).Name);
   GroupBox gb = Grpblocks[objn];
   gb.Visible = false;
   string Tag = gb.Controls[0].Tag.ToString();
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

   int Last_block = bl_used.Getlkey() - 1;

   for (int ib = objn; ib <= Last_block; ib += 1)
   {
    Move_blk(ib + 1, ib);
   }

   gb.Visible = true;

   labgb[gph[bl_used.Getlkey()]] = string.Empty;
   labgb[gpb[bl_used.Getlkey()]] = string.Empty;

   Grpblocks[bl_used.Getlkey()].Visible = false;

   bl_available[bl_used.Getlkey()] = bl_used.Getlval();
   bl_used.Remove(bl_used.Getlkey());

   Scsiz(Width);
   Set_empgb();
  }

  private void Move_blk(int fbn, int tbn)
  {
   GroupBox gbf = Grpblocks[fbn];
   GroupBox gbt = Grpblocks[tbn];
   gbt.Controls[0].Tag = gbf.Controls[0].Tag;
   gbt.Enabled = true;
   if (bl_loaded.ContainsKey(gbt.Controls[0].Tag.ToString()))
   {
    bl_loaded[gbt.Controls[0].Tag.ToString()].Num = tbn;
    bl_loaded[gbt.Controls[0].Tag.ToString()].State = (int)Load_state.loaded;
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
   int objn = Convert.ToInt32(((ToolStripMenuItem)sender).Name);
   GroupBox gb = (GroupBox)Grpblocks[objn];
   if (bl_used.Count == blocks_number)
   {
    MessageBox.Show("Unable to insert block", "All blocks are in use");
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
   Grpblocks[bl_used.Getlkey()].Visible = true;
   Set_empgb();
   Scsiz(Width);
  }

  private void Menuswp_Click(object sender, EventArgs e)
  {
   int objn = Convert.ToInt32(((ToolStripMenuItem)sender).Name);
   if (objn == 1)
   {
    return;
   }

   GroupBox gb1 = Grpblocks[objn];
   GroupBox gb2 = Grpblocks[objn - 1];
   string n1 = gb1.Tag.ToString();
   string n2 = gb2.Tag.ToString();
   string tag1 = gb1.Controls[0].Tag.ToString();
   string tag2 = gb2.Controls[0].Tag.ToString();
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
   Grpblocks[objn - 1].Controls[0].Text = labgb[gph[objn - 1]];
   Grpblocks[objn - 1].Controls[1].Text = labgb[gpb[objn - 1]];
   Set_ro(Grpblocks[objn - 1].Controls[0]);
   Set_ro(Grpblocks[objn - 1].Controls[1]);
   Grpblocks[objn].Controls[0].Text = labgb[gph[objn]];
   Grpblocks[objn].Controls[1].Text = labgb[gpb[objn]];
   Set_ro(Grpblocks[objn].Controls[0]);
   Set_ro(Grpblocks[objn].Controls[1]);
   Set_empgb();
  }

  private void Cudchange(object sender, EventArgs e)
  {
   int objn = Convert.ToInt32(((TSnumud)sender).Name);
   TSnumud tsi = (TSnumud)sender;
   int tg1 = tsi.aml;
   int tgno = tsi.udl;
   max_rec[Convert.ToInt32(tsi.Name)] = tsi.Value;
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

  private void Set_ro(Control ctrl)
  {
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
    MessageBox.Show("Unable to add page", "All are in use", MessageBoxButtons.OK, MessageBoxIcon.Information);
   }
   else
   {
    Panels_delete();
    bn = bl_available.First().Key;
    bl_available.Remove(bn);
    bl_used[bn] = gpn[bn];
    Grpblocks[bn].Controls[0].Text = string.Empty;
    Grpblocks[bn].Controls[1].Text = string.Empty;
    Grpblocks[bn].Controls[0].Enabled = true;
    Grpblocks[bn].Controls[1].Enabled = true;
    Grpblocks[bn].Visible = true;
    Set_ro(Grpblocks[bn].Controls[0]);
    Set_ro(Grpblocks[bn].Controls[1]);
    labgb[gph[bn]] = string.Empty;
    labgb[gpb[bn]] = string.Empty;
    Scsiz(Width);
    Set_empgb();
   }
  }

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
   Dsi = new KeyValuePair<string, Dsave>();
   this.ResumePaint();
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
   tb = (TextBox)Grpblocks[bn].Controls[0];
   bx = (RichTextBox)Grpblocks[bn].Controls[1];
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
   bx = (RichTextBox)Grpblocks[bn].Controls[1];
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
  private int Ath_setcolor(int bn)
  {
   int ln = 0;
   int lines = default;
   int[] ll = null;
   TextBox hx = null;
   RichTextBox bx = null;
   hx = (TextBox)Grpblocks[bn].Controls[0];
   bx = (RichTextBox)Grpblocks[bn].Controls[1];
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
   if (Pe == null)
    return;

   if (bl_used.Count == 0)
    return;

   GroupBox gb = null;
   GroupBox lb = null;
   GroupBox gb3 = null;

   Form_Bounds = Bounds;

   Size Cs = ClientSize;
   Size Fs = Size;

   if (Vsm)
   {
    Control c = Pe;
    var rect = c.RectangleToScreen(c.ClientRectangle);
    while (c != null)
    {
     rect = Rectangle.Intersect(rect, c.RectangleToScreen(c.ClientRectangle));
     c = c.Parent;
    }
    rect = Pe.RectangleToClient(rect);

    Sb = new VScrollBar()
    {
     Minimum = 0,
     Maximum = Pn.Height - rect.Height + 2 * Ymargin
    };

    if (Vertical_Scroll_Position >= Sb.Minimum && Vertical_Scroll_Position <= Sb.Maximum)
    {
     Sb.Value = Vertical_Scroll_Position;
    }

    if (Sb.Value != 0)
    {
     Pn.Top = -Sb.Value;
    }
    Sb.ValueChanged += Sb_changed;
    Pn.Top = -Sb.Value;
    Pn.SizeChanged += Pn_sizechanged;
    int M1 = Cs.Width - Pe.Width;

    if (Original_Margin == -1)
    {
     Original_Margin = M1;
    }

    if (M1 < Sb.Width)
    {
     M1 = Sb.Width - M1;
     Set_Size(Width += M1, Height);
    }

    Pe.Size = new Size(Pe.Width += Sb.Width, Pe.Height);
    Pe.Controls.Add(Sb);
    Sb.Location = new Point(Pn.Width, 0);
    Sb.Size = new Size(Sb.Width, rect.Height);
    Sb.Visible = true;
   }

   var cm = new ContextMenuStrip();

   ToolStripMenuItem ts1 = null;
   ToolStripMenuItem ts2 = null;

   int pnxstart = 0;
   int pnystart = 0;
   int pnxsize = 0;
   int pnysize = 0;

   var bmni = new List<ToolStripMenuItem>();

   bmni.Clear();

   if (emppn is object)
   {
    Pn.Controls.Remove(emppn);
    emppn.Dispose();
   }

   emppn = null;

   gb3 = Grpblocks[3];

   if (bl_used.Count % 3 != 0)
   {
    lb = Grpblocks[bl_used.Getlkey()];
    emppn = new Panel() { Name = "emppn" };
    pnxstart = lb.Location.X + lb.Width + 1;
    if (bl_used.Count < 3)
    {
     pnystart = lb.Location.Y - 2 * Ymargin + 1;
     gb = Grpblocks[3];
    }
    else
    {
     gb = Grpblocks[bl_used.Count / 3 * 3];
     pnystart = gb.Location.Y + gb.Height + 1;
    }

    pnxsize = gb.Location.X + gb.Width - pnxstart;
    pnysize = lb.Location.Y + lb.Height - pnystart;
    emppn.Location = new Point(pnxstart, pnystart);
    emppn.Size = new Size(pnxsize, pnysize);
   }

   foreach (KeyValuePair<string, Blk_entry> k in bl_loaded)
   {
    var b = k.Value;

    if (b.State == (int)Load_state.not_loaded | b.State == (int)Load_state.not_loaded_byc | b.State == (int)Load_state.not_in_use)
    {
     Add_cm(k.Value.header, cm, ts1, ts2);
     continue;
    }

    if (string.IsNullOrEmpty(Grpblocks[b.Num].Controls[1].Text))
    {
     continue;
    }
   }

   if (emppn is object)
   {
    emppn.ContextMenuStrip = cm;
    Pn.Controls.Add(emppn);
   }

   if (res_endpoints)
   {
    var tbiu = new System.Collections.Specialized.StringCollection();
    foreach (KeyValuePair<string, Blk_entry> k in bl_loaded)
    {
     if (k.Value.State != (int)Load_state.not_loaded)
     {
      tbiu.Add(k.Value.header);
     }
    }

    if (tbiu.Count != Endpoints_in_use.Count)
    {
     Properties.Settings.Default.endpoints = new System.Collections.Specialized.StringCollection();
     foreach (string s in tbiu)
      Properties.Settings.Default.endpoints.Add(s);
     Properties.Settings.Default.Save();
    }

    Update();
   }
  }

  private void Sb_changed(object sender, EventArgs e)
  {
   Pn.Top = -Sb.Value;
   Vertical_Scroll_Position = Sb.Value;
  }

  private void Pn_sizechanged(object sender, EventArgs e)
  {
   Sb.ValueChanged -= Sb_changed;
   Application.DoEvents();
   Sb.Maximum = Pn.Height;
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
   Scsiz(Width, Originaly);
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
   Scsiz(Width, Originaly);
   Invalidate();
   Update();
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

  private void Panels_delete()
  {
   if (Pn != null)
   {
    Pn.Controls.Clear();
   }

   if (Pe != null && Pe.Controls.Contains(Pn))
   {
    Pe.Controls.Remove(Pn);
   }
   Controls.Remove(Pe);
  }

  private Rectangle Panels_create(int Petop)
  {
   GroupBox B3 = Grpblocks[3];
   GroupBox Bl = Grpblocks[bl_used.Getlkey()];

   int Pn_width = B3.Left + B3.Width/* + Xmargin*/;
   int Pn_height = Bl.Top + Bl.Height/* + Ymargin*/;

   Pn = new Panel()
   {
    Name = "PnPanel",
    Location = new Point(0, 0),
    Width = Pn_width,
    Height = Pn_height,
    Padding = new Padding(0, 0, 0, 0),
    Margin = new Padding(0, 0, 0, 0),
    AutoSize = false,
    AutoScroll = false
   };

   foreach (KeyValuePair<int, string> s in bl_used)
   {
    Pn.Controls.Add(Grpblocks[s.Key]);
   }

   Pe = new Panel()
   {
    Name = "PePanel",
    Padding = new Padding(0, 0, 20, 0),
    Location = new Point(0, Petop),
    Width = Pn.Width/* += Xmargin*/,
    Height = Pn.Height/* += Ymargin*/,
    AutoSize = false,
    AutoScroll = false
   };

   Pe.Controls.Add(Pn);

   Controls.Add(Pe);

   return Pe.Bounds;
  }

  private void Form1_Resizeend(object sender, EventArgs e)
  {
   Control cntrl = (Control)sender;
   Form_Height = cntrl.Height;
   Scsiz(Width);
   Set_empgb();
  }

  private void Scsiz(int Width, int DHeight = -1)
  {
   int F_width = 0;
   int F_height = 0;
   int F_left = 0;
   int F_top = 0;

   if (DHeight == ClientSize.Height)
    return;

   Rectangle Form_Bounds = Bounds;
   Rectangle Screen_Area = Screen.PrimaryScreen.WorkingArea;

   Size Client_Size = ClientSize;
   Size Fs = Size;

   if (Pe != null)
   {
    Panels_delete();
   }

   if (Form_size.IsEmpty)
   {
    Form_size = Fs;
    Form_area = Client_Size;
   }

   W_delta = Fs.Width - Client_Size.Width;
   H_delta = Fs.Height - Client_Size.Height;

   if (DHeight != -1)
    F_height = DHeight + H_delta;
   else
    F_height = Originaly + H_delta;

   F_width = Fs.Width;
   F_left = Form_Bounds.X;
   F_top = Form_Bounds.Y;
   Vsm = false;

   if (bl_used.Count > 0)
   {
    Rectangle pb = Panels_create(Ystart + 2 * Ymargin);

    F_height = Pe.Top + Pe.Height + H_delta + 2 * Ymargin;
   }

   if (F_height == Form_Height)
    Form_Height = -1;

   if ((Form_Height != -1) && (F_height > Form_Height))
   {
    F_top = 20;
    F_height = Form_Height;
    Vsm = true;
   }
   else if (F_height > Screen_Area.Height)
   {
    F_top = 20;
    Form_Height = Screen_Area.Height;
    F_height = Form_Height;
    Vsm = true;
   }
   else
   {
    F_top = ((Screen_Area.Height / 2) - (F_height / 2)) / 2;
    Vsm = false;
    Vertical_Scroll_Position = 0;
   }

   F_left = (Screen_Area.Width / 2) - (Fs.Width / 2);

   StartPosition = FormStartPosition.Manual;

   Location = new Point(F_left, F_top);

   Set_Size(Fs.Width, F_height);

   Update();
  }

  private void Previewmenuitem_Click(object sender, EventArgs e)
  {
   int pccnt;
   int prcnt;
   PictureBox pb;
   var gblabels = new[] { "First Label", "Second Label", "Third Label", "Fourth Label", "Fifth Label", "Sixth Label" };
   Panel pnl;

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
     tbl.Name = "Panel_Table";
     pccnt = Pnglablist.Count < 2 ? 1 : 2;
     prcnt = (Pnglablist.Count + 1) / pccnt;
     tbl.RowCount = prcnt;
     tbl.ColumnCount = pccnt;
     tbl.Width = 504 * pccnt;
     tbl.Height = 337 * prcnt;
     fpr.Width = tbl.Width;
     fpr.Height = tbl.Height;
     for (int i = 0; i <= prcnt - 1; i++)
     {
      for (int j = 0; j <= pccnt - 1; j++)
      {
       int ind = i * pccnt + j;
       if (ind > Pnglablist.Count - 1)
       {
        break;
       }
       pnl = new Panel();
       pnl.Name = "Panel";
       pb = new PictureBox()
       {
        Width = 464,
        Height = 277,
        Left = 15,
        Top = 15,
        Name = "Picture",
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
       lab.Name = "Label";
       pnl.Controls.Add(lab);
       pnl.Controls[0].Left = 20;
       pnl.Controls[0].Top = 20;
       pnl.Controls.Add(pb);
       pb.CenterControl();
       pnl.Controls[1].Left = 20;
       pnl.Controls[1].Top = 45;
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

   if (prv_combo.Items.Count > 1)
   {
    priph.Visible = false;
    prv_combo.Visible = true;
   }
   else
   {
    prv_combo.Visible = false;
    priph.Visible = true;
   }

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
    TextBox tb = (TextBox)Grpblocks[k.Key].Controls[0];
    tb.ReadOnly = bset;
    RichTextBox rb = (RichTextBox)Grpblocks[k.Key].Controls[1];
    rb.ReadOnly = bset;
    Refresh();
   }
  }

  private void Notesmenuitem_Click(object sender, EventArgs e)
  {
   int bn;
   if (bl_used.Count == blocks_number)
   {
    MessageBox.Show("Unable to Notes Page", "All are in use", MessageBoxButtons.OK, MessageBoxIcon.Information);
   }
   else
   {
    bn = bl_available.First().Key;
    bl_available.Remove(bn);
    bl_used[bn] = gpn[bn];
    Grpblocks[bn].Controls[0].Text = string.Empty;
    Grpblocks[bn].Controls[1].Text = string.Empty;
    Grpblocks[bn].Controls[0].Enabled = true;
    Grpblocks[bn].Controls[1].Enabled = true;
    Grpblocks[bn].Controls[0].Text = "Notes";
    Grpblocks[bn].Visible = true;
    Set_ro(Grpblocks[bn].Controls[0]);
    Set_ro(Grpblocks[bn].Controls[1]);
    labgb[gph[bn]] = Grpblocks[bn].Controls[0].Text;
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
   Preview = false;

   if (printer is ILabelWriterPrinter)
   {
    pjob = (PrintJob)printer.CreatePrintJob(lprintparams);
   }
   else
   {
    pjob = (PrintJob)printer.CreatePrintJob(printParams);
   }
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
    if (!string.IsNullOrEmpty(Patient.Text))
    {
     Savemi.Enabled = true;
     Savemi.Visible = true;
    }
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
     MessageBox.Show(L2_ret.message, "Medical Profile Card");
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
     MessageBox.Show(L2_ret.message, "Medical Profile Card");
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
   Scsiz(Width, Originaly);
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
   Scsiz(Width, Originaly);
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

   if (saved_type)
   {
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
     withBlock.header = Grpblocks[s.Key].Controls[0].Text;
     withBlock.body = Grpblocks[s.Key].Controls[1].Text;
    }

    Sblk.Blk_list.Add(Sb);
   }

   aws_body.Clear();
   js = JsonConvert.SerializeObject(Sblk, Formatting.Indented);
   aws_body["skey"] = Enc256.Encrypt(Sblk.Patient, Enc256.Iterscramble(cid + Mpck.Dlab, Mpck.Iterations));
   var switchExpr = save_ver;
   switch (switchExpr)
   {
    case 1:
     {
      jse = Enc256.Encrypt(js, Enc256.Iterscramble(cid), Convert.ToInt32(Mpck.Iterations % 10 + 2));
      aws_body["skey"] = Enc256.Encrypt(Sblk.Patient, Enc256.Iterscramble(cid + Mpck.Dlab), Mpck.Iterations % 10 + 2);
      break;
     }

    default:
     {
      jse = Enc256.Encrypt(js, Enc256.Iterscramble(cid, Mpck.Iterations), Convert.ToInt32(Mpck.Iterations / (double)3));
      aws_body["skey"] = Enc256.Encrypt(Sblk.Patient, Enc256.Iterscramble(cid + Mpck.Dlab, Mpck.Iterations));
      break;
     }
   }

   aws_body["ukey"] = Enc256.Encrypt(cid + Mpck.Dlab, cid + Mpck.Dlab, Mpck.Iterations);
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
   aws_body["ukey"] = Enc256.Encrypt(cid + Mpck.Dlab, cid + Mpck.Dlab, Mpck.Iterations);
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
   aws_body["ukey"] = Enc256.Encrypt(cid + Mpck.Dlab, cid + Mpck.Dlab, Mpck.Iterations);
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
     case "1":
      {
       js = Enc256.Decrypt(Dvr.Dsave_value, Enc256.Iterscramble(cid), Convert.ToInt32(Mpck.Iterations % 10 + 2));
       break;
      }

     default:
      {
       js = Enc256.Decrypt(Dvr.Dsave_value, Enc256.Iterscramble(cid, Mpck.Iterations), Convert.ToInt32(Mpck.Iterations / (double)3));
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

  private void Cb_Enter(object sender, EventArgs e)
  {
   ComboBox Cb = (ComboBox)sender;
   Cb.SelectionStart = Cb.Text.Length;
  }

  private void Cb_DropDown(object sender, EventArgs e)
  {
   ComboBox Cb = (ComboBox)sender;
   Cb.SelectionLength = 0;

   string St = Cb.Text;
  }

  private void Cb_DrawItem(object sender, DrawItemEventArgs e)
  {
   string str;
   ComboBox Cb = (ComboBox)sender;
   Cb.SelectionStart = Cb.SelectionLength;
   var G = e.Graphics;
   var R = e.Bounds;
   var S = e.State;
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

   if ((S & DrawItemState.Selected) != 0)
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
 }
}