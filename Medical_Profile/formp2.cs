//using DYMO.Label.Framework;
using JR.Utils.GUI.Forms;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using global::Amazon;
using global::Amazon.DynamoDBv2;
using global::Amazon.DynamoDBv2.DocumentModel;
using JWT;
using Microsoft.Win32;
using Awsstd;

namespace Medical_Profile
{
 public partial class Form1
 {
  //private DieCutLabel Label;
  //tyledTextBuilder stb = new StyledTextBuilder();
  //public StyledTextBuilder htb = new StyledTextBuilder();
 // public StyledTextBlock lt;
  public int ltline;
  public int labelno = 0;
  //public FontInfo reg_font;
  //public FontInfo rp1_font;
  //public FontInfo bld_font;
  //public FontInfo bp1_font;
  //public FontInfo itl_font;
  public int lines = 12;
  public double points = 6.0;
  public string printerName;
 // public IPrinters pnames;
//  public IPrinter printer;
  public float name_length = 0.0F;
  public List<FileInfo> fi = new List<FileInfo>();
  //public FontInfo nfnt = new FontInfo("Calibri", 12.0, DYMO.Label.Framework.FontStyle.Bold);
  //public FontInfo lfnt = new FontInfo("Calibri", 9.0, DYMO.Label.Framework.FontStyle.Bold);
  public string flsort = "Date";
  public string enck = null;
  public string read_patient;

  public List<Byte[]> Call_Generate(bool Print_flag = false)
  {
   string Phone_number = null;
   string P1 = @"(\(??\d\d\d\)??[\s|-]\d\d\d-\d\d\d\d)";
   string P2 = @"(\d\d\d-\d\d\d\d)";
   Match M1;

   foreach (int j in Dlab.Blocks.Keys)
   {
    Dlab.Blocks[j].Hdr = "";
    Dlab.Blocks[j].Bdy.Clear();
   }

   Dlab.Patient = Patient.Text;

   Dlab.Phone = Phone.Text ?? string.Empty;

   Dlab.Address.AddRange(address.Lines);

   Dlab.DOB = DOB.Text ?? string.Empty;

   string dname = priph.Text;

   if (prv_combo.Items.Count > 0)
    dname = prv_combo.Text;

   if (!string.IsNullOrEmpty(dname))
   {
    Dlab.PPtitle = lab1["priph_title:"];

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

    if (!string.IsNullOrEmpty(Phone_number))
    {
     Dlab.PPphone = Phone_number;
    }

    Dlab.PPname = dname;
   }

   Dlab.Insurance = ins.Text ?? string.Empty;

   if (lab1.ContainsKey("secph:"))
   {
    Dlab.SPtitle = lab1["secph_title:"];
    Dlab.SPname = secph.Text;
   }

   Dlab.Econtact = econtact.Text ?? string.Empty;

   Dlab.Printer = Printers.SelectedItem.ToString();

   foreach (KeyValuePair<int, string> K in bl_used)
   {
    int Bnum = K.Key;
    GroupBox Gb = Grpblocks[Bnum];

    if (!string.IsNullOrEmpty(Gb.Controls[0].Text))
     Dlab.Blocks[Bnum].Hdr = Gb.Controls[0].Text;

    RichTextBox Rb = (RichTextBox)Gb.Controls[1];

    if (Rb.Lines.Length > 0)
     Dlab.Blocks[Bnum].Bdy = Rb.Lines.ToList();
   }

   string logfile = @"c:\users\bud\medical_profile.log";
   using (var writer = new StreamWriter(logfile, true))
   {
    foreach (int K in Dlab.Blocks.Keys)
    {
     writer.WriteLine("[" + K.ToString() + "] Hdl[" + Dlab.Blocks[K].Hdr.Length.ToString() + "] [" + Dlab.Blocks[K].Hdr + "]");
     for (int l = 0; l < Dlab.Blocks[K].Bdy.Count; l++)
     {
      writer.WriteLine("   [" + l.ToString() + "] Bll[" + Dlab.Blocks[K].Bdy[l].Length.ToString() + "] [" + Dlab.Blocks[K].Bdy[l] + "]");
     }
    }
   }

   string Dsfile = @"c:\users\bud\Dlab.json";
   string Ds = JsonConvert.SerializeObject(Dlab, Formatting.Indented);
   using (var writer = new StreamWriter(Dsfile, true))
   {
    writer.WriteLine(Ds);
   }

    return Dlab.Generate(6, Print_flag);
  }

  public bool Check_altered()
  {
   DialogResult res;
   string msg = "You have unprocessed changes" + "\r\n" + "do you want to save these changes?";
   if (!Data_altered)
   {
    return false;
   }

   if (Dstate == Data_state.Edit_mode)
   {
    return false;
   }

   FlexibleMessageBox.FONT = new Font("Calibri", 10, System.Drawing.FontStyle.Bold);
   res = FlexibleMessageBox.Show(msg, "Data Altered", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
   if (res == DialogResult.Yes)
   {
    return true;
   }
   else
   {
    return false;
   }
  }

  public void Delete_saved_item(string dsk)
  {
   dsaves.SelectedIndexChanged -= Dsaves_SelectedIndexChanged;
   saved_patients.Remove(dsk);
   dsaves.SelectedItem = null;
   if (saved_patients.Count > 0)
   {
    dsaves.DisplayMember = "key";
    dsaves.ValueMember = "Value";
    dsaves.DataSource = new BindingSource(saved_patients, null);
    dsaves.Enabled = true;
    dsaves.Visible = true;
    Dsbox.Enabled = true;
    Dsbox.Visible = true;
   }
   else
   {
    dsaves.Enabled = false;
    dsaves.Visible = false;
    Dsbox.Enabled = false;
    Dsbox.Visible = false;
   }

   dsaves.SelectedIndexChanged += Dsaves_SelectedIndexChanged;
  }

  public void Add_saved_item(Dsave ds)
  {
   string[] np = null;
   var csep = new[] { ' ' };
   string pn = null;
   dsaves.SelectedIndexChanged -= Dsaves_SelectedIndexChanged;
   var switchExpr = Convert.ToInt32(ds.vers);
   switch (switchExpr)
   {
    case 1:
     {
      ds.Name = Ede.Decrypt(ds.Skey, Ede.Iterscramble(cid + Mpck.Dlab), Mpck.Iterations % 10 + 2);
      break;
     }

    default:
     {
      ds.Name = Ede.Decrypt(ds.Skey, Ede.Iterscramble(cid + Mpck.Dlab, Mpck.Iterations));
      break;
     }
   }

   if (string.IsNullOrEmpty(ds.Name))
   {
    ds.Name = "Unknown_" + (string.IsNullOrEmpty(ds.lwtim) ? ds.wrtim : ds.lwtim);
   }

   np = ds.Name.Split(csep);
   if (np.Count() > 1)
   {
    pn = np[1] + "," + np[0];
   }
   else
   {
    pn = ds.Name;
   }

   saved_patients[pn] = ds;
   if (saved_type)
   {
    dsaves.Text = pn;
   }

   if (saved_patients.Count > 0)
   {
    dsaves.DisplayMember = "key";
    dsaves.ValueMember = "Value";
    dsaves.DataSource = new BindingSource(saved_patients, null);
    dsaves.SelectedIndex = dsaves.FindStringExact(pn);
    dsaves.Enabled = true;
    dsaves.Visible = true;
    Dsbox.Enabled = true;
    Dsbox.Visible = true;
   }
   else
   {
    dsaves.Enabled = false;
    dsaves.Visible = false;
    Dsbox.Enabled = false;
    Dsbox.Visible = false;
   }

   dsaves.SelectedIndexChanged += Dsaves_SelectedIndexChanged;
  }

  public void Set_saved_items(List<Dsave> ds, bool preserve = false)
  {
   string dss = null;
   object O1 = null;
   string S1 = null;
   string[] np = null;
   var csep = new[] { ' ' };
   string dec_name = null;
   dsaves.SelectedIndexChanged -= Dsaves_SelectedIndexChanged;
   if (preserve)
   {
    O1 = dsaves.SelectedValue;
    S1 = dsaves.SelectedText;
   }

   saved_patients.Clear();
   dsaves.SelectedItem = null;
   if (ds is object && ds.Count > 0)
   {
    foreach (Dsave d in ds)
    {
     var switchExpr = Convert.ToInt32(d.vers);
     switch (switchExpr)
     {
      case 1:
       {
        dec_name = Ede.Decrypt(d.Skey, Ede.Iterscramble(cid + Mpck.Dlab), Mpck.Iterations % 10 + 2);
        if (string.IsNullOrEmpty(dec_name))
        {
         d.Name = "Unknown_" + (string.IsNullOrEmpty(d.lwtim) ? d.wrtim : d.lwtim);
         dss = d.Name;
        }
        else
        {
         d.Name = dec_name;
        }

        break;
       }

      default:
       {
        dec_name = Ede.Decrypt(d.Skey, Ede.Iterscramble(cid + Mpck.Dlab, Mpck.Iterations));
        if (string.IsNullOrEmpty(dec_name))
        {
         d.Name = "Unknown_" + (string.IsNullOrEmpty(d.lwtim) ? d.wrtim : d.lwtim);
         dss = d.Name;
        }
        else
        {
         d.Name = dec_name;
        }

        break;
       }
     }

     np = d.Name.Split(csep);
     if (np.Count() > 1)
     {
      dss = np[1] + "," + np[0];
      if (string.IsNullOrEmpty(d.lwtim))
      {
       dss = dss + string.Format(" {0}/{1}/{2} {3}:{4}:{5}", d.wrtim.Substring(0, 4), d.wrtim.Substring(4, 2), d.wrtim.Substring(6, 2), d.wrtim.Substring(8, 2), d.wrtim.Substring(10, 2), d.wrtim.Substring(12, 2));
      }
     }

     var sb = new StringBuilder();
     sb.Append("dss[" + dss + "] d.name[" + d.Name + "]");
     if (!string.IsNullOrEmpty(d.wrtim))
     {
      sb.Append(" wrtim[" + d.wrtim + "]");
     }

     if (!string.IsNullOrEmpty(d.lwtim))
     {
      sb.Append(" lwtim[" + d.lwtim + "]");
     }

     saved_patients[dss] = d;
    }

    dsaves.DisplayMember = "key";
    dsaves.ValueMember = "Value";
    dsaves.DataSource = new BindingSource(saved_patients, null);
    dsaves.Enabled = true;
    dsaves.Visible = true;
    Dsbox.Enabled = true;
    Dsbox.Visible = true;
    if (preserve)
    {
     dsaves.SelectedIndex = dsaves.FindStringExact(S1);
    }
    else
    {
     dsaves.SelectedItem = null;
    }

    dsaves.SelectedIndexChanged += Dsaves_SelectedIndexChanged;
   }
   else
   {
    dsaves.Enabled = false;
    dsaves.Visible = false;
    Dsbox.Enabled = false;
    Dsbox.Visible = false;
   }
  }
  private string Handle_file(string eval)
  {
   MPC_type mty = null;
   string rjson = Ede.Decrypt(eval, enck, 17531);
   if (string.IsNullOrEmpty(rjson))
   {
    return "1";
   }

   mty = JsonConvert.DeserializeObject<MPC_type>(rjson);

   if (mty == null)
    return "1";

   if (string.IsNullOrEmpty(mty.F1))
    return "1";

   rjson = Ede.Decrypt(mty.F1, enck, mty.Akey);

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
    rjson = Ede.Decrypt(mty.F1, enck, mty.Akey);
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
     d1 = Ede.Decrypt(dle, enck, d.VolumeLabel);
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
       string rjson = Ede.Decrypt(fs, enck, drive_label);
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
  public void Load_Department(Level1_Return l1)
  {
   Department.Enabled = false;
   Department.Items.Clear();
   departments.Clear();
   atdp.Clear();
   for (int i = 0, loopTo = l1.Dpt.Length - 1; i <= loopTo; i++)
   {
    departments[Convert.ToInt32(l1.Dpt[i].departmentid)] = l1.Dpt[i];
   }

   if (departments.Count == 1)
   {
    Department.Visible = false;
    dept_tbox.Visible = true;
    dept_tbox.Text = departments[0].name;
   }
   else
   {
    dept_tbox.Visible = false;
   }

   string dfound = "";
   Department.Items.Add(string.Empty);
   foreach (KeyValuePair<int, Dept_Return> k in departments)
   {
    Department.Items.Add(k.Value.name);
    atdp[k.Value.name] = k.Key;
    if (string.IsNullOrEmpty(dfound))
    {
     dfound = k.Value.name;
    }

    if ((Properties.Settings.Default.department ?? "") == (k.Value.name ?? ""))
    {
     dfound = k.Value.name;
    }
   }

   if (!((Properties.Settings.Default.department ?? "") == (dfound ?? "")))
   {
    Properties.Settings.Default.department = dfound;
    Properties.Settings.Default.Save();
   }

   Department.SelectedIndex = 0;
   Department.Enabled = true;
  }

  private async Task<Dsave_return> save_blk()
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
   aws_body["skey"] = Ede.Encrypt(Sblk.Patient, Ede.Iterscramble(cid + Mpck.Dlab, Mpck.Iterations));
   var switchExpr = save_ver;
   switch (switchExpr)
   {
    case 1:
     {
      jse = Ede.Encrypt(js, Ede.Iterscramble(cid), Convert.ToInt32(Mpck.Iterations % 10 + 2));
      aws_body["skey"] = Ede.Encrypt(Sblk.Patient, Ede.Iterscramble(cid + Mpck.Dlab), Mpck.Iterations % 10 + 2);
      break;
     }

    default:
     {
      jse = Ede.Encrypt(js, Ede.Iterscramble(cid, Mpck.Iterations), Convert.ToInt32(Mpck.Iterations / (double)3));
      aws_body["skey"] = Ede.Encrypt(Sblk.Patient, Ede.Iterscramble(cid + Mpck.Dlab, Mpck.Iterations));
      break;
     }
   }

   aws_body["ukey"] = Ede.Encrypt(cid + Mpck.Dlab, cid + Mpck.Dlab, Mpck.Iterations);
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

   return dr;
  }

  private void Load_providers(Level1_Return l1)
  {
   string s1;
   string pphone = null;
   Provider_return p;
   Message_label.Text = "Loading Providers";
   Message_label.Update();
   providers.Clear();
   atpv.Clear();
   for (int i = 0, loopTo = l1.Prv.Count() - 1; i <= loopTo; i++)
   {
    var ad = new StringBuilder();
    providers[l1.Prv[i].Providerid] = l1.Prv[i];
    p = l1.Prv[i];
    s1 = "";
    if (p.Displayname is object)
    {
     s1 = p.Displayname;
    }
    else if (p.Firstname is object && p.Lastname is object)
    {
     s1 = p.Firstname + " " + p.Lastname;
    }

    if (p.Providertype is object && s1.IndexOf(",") == -1 && !s1.EndsWith(p.Providertype))
    {
     s1 = s1 + ", " + p.Providertype;
    }

    if (p.Homedepartment is object && atdp.ContainsKey(p.Homedepartment))
    {
     if (departments[atdp[p.Homedepartment]].phone is object)
     {
      string args1 = departments[atdp[p.Homedepartment]].phone;
      pphone = Trimc(ref args1, ")(- ");
      s1 = s1 + " " + Format_phone(ref pphone);
     }
    }

    atpv[p.Providerid] = s1;
    prv_combo.Items.Add(s1);
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

   Message_label.Text = null;
   Message_label.Update();
   if (dsaves.Items.Count > 0)
   {
    dsaves.Enabled = true;
   }

   Application.DoEvents();
   return;
  }

  public void Load_level2(Level2_Return l2)
  {
   string pphone = null;
   string s1;
   var ad = new StringBuilder();
   string term = null;
   ppgb.Text = "Primary Physician";
   saved_type = false;
   Dsi = new KeyValuePair<string, Dsave>();
   Savemi.Enabled = false;
   Deletemi.Enabled = false;
   dsaves.Enabled = false;
   Scsiz(Width, Originaly);
   Invalidate();
   Update();
   Reset_fields();
   dsaves.SelectedIndexChanged -= Dsaves_SelectedIndexChanged;
   dsaves.SelectedItem = null;
   dsaves.SelectedIndexChanged += Dsaves_SelectedIndexChanged;
   foreach (int k in blocks.Keys)
   {
    Max_rec[k] = 0;
   }

   foreach (Ath_block b in ath_blist)
   {
    if (b.max_lines > 0)
    {
     Max_rec[b.num] = b.max_lines;
    }
   }

   if (l2.Pat.Patientid is object)
   {
    Patientid.Text = l2.Pat.Patientid.ToString();
   }

   if (l2.Pat.Mobilephone is object)
   {
    pphone = l2.Pat.Mobilephone;
   }

   if (pphone is null && l2.Pat.Homephone is object)
   {
    pphone = l2.Pat.Homephone;
   }

   if (l2.Pat.Firstname is object && l2.Pat.Lastname is object)
   {
    Patient.Text = l2.Pat.Firstname + " " + l2.Pat.Lastname;
   }

   if (l2.Pat.Dob is object)
   {
    DOB.Text = l2.Pat.Dob;
   }

   if (pphone is object)
   {
    pphone = Trimc(ref pphone, ")(- ");
    Phone.Text = Format_phone(ref pphone);
   }

   if (l2.Pat.Address1 is object)
   {
    ad.Append(l2.Pat.Address1);
    term = "\r\n";
   }

   if (l2.Pat.City is object)
   {
    ad.Append(term);
    ad.Append(l2.Pat.City);
    term = ",";
   }

   if (l2.Pat.State is object)
   {
    ad.Append(term);
    ad.Append(l2.Pat.State);
    term = " ";
    if (l2.Pat.Zip is object)
    {
     ad.Append(term);
     ad.Append(l2.Pat.Zip);
    }
   }

   if (ad.Length > 0)
   {
    address.Text = ad.ToString();
   }

   address.BackColor = System.Drawing.Color.White;
   if (l2.Pat.Primaryproviderid != -1 && atpv.ContainsKey((int)l2.Pat.Primaryproviderid))
   {
    prv_combo.SelectedItem = atpv[(int)l2.Pat.Primaryproviderid];
    priph.Text = atpv[(int)l2.Pat.Primaryproviderid];
   }

   s1 = null;
   pphone = null;
   if (l2.Pat.Contactname is object)
   {
    s1 = l2.Pat.Contactname;
   }
   else if (l2.Pat.Contactrelationship is object)
   {
    s1 = Ucfc(l2.Pat.Contactrelationship);
   }

   if (s1 is object)
   {
    if (l2.Pat.Contactmobilephone is object)
    {
     pphone = l2.Pat.Contactmobilephone;
    }
    else
    {
     pphone = l2.Pat.Contacthomephone;
    }
   }

   if (s1 is object)
   {
    if (pphone is object)
    {
     s1 = s1 + " " + Format_phone(ref pphone);
    }

    econtact.Text = s1;
   }

   econtact.BackColor = System.Drawing.Color.White;
   ins.Text = l2.Ins;
   ins.BackColor = System.Drawing.Color.White;
   if (ppgb.Text.Length > 1)
   {
    lab1["priph_title:"] = ppgb.Text;
   }

   if (sp.Text.Length > 1)
   {
    lab1["secph_title:"] = sp.Text;
   }

   foreach (KeyValuePair<string, List<string>> K in l2.blks)
   {
    var blk = new Blk_entry()
    {
     header = K.Key,
     State = (int)Load_state.not_loaded
    };

    if (Endpoints_in_use.Contains(blk.header))
    {
     blk.State = (int)Load_state.not_loaded_byc;
     int bnum = bl_available.First().Key;
     bl_used[bl_available.First().Key] = bl_available.First().Value;
     bl_available.Remove(bl_available.First().Key);
     Grpblocks[bnum].Visible = false;
     Grpblocks[bnum].Controls[0].Text = K.Key;
     Grpblocks[bnum].Controls[0].Tag = K.Key;
     Grpblocks[bnum].Controls[0].Enabled = true;
     TextBox tb = (TextBox)Grpblocks[bnum].Controls[0];
     tb.ReadOnly = true;
     Grpblocks[bnum].Controls[1].Text = string.Join("\r\n", K.Value);
     Grpblocks[bnum].Controls[1].Enabled = true;
     Grpblocks[bnum].Controls[1].Tag = K.Key;
     RichTextBox rb = (RichTextBox)Grpblocks[bnum].Controls[1];
     rb.ReadOnly = true;
     rb.BackColor = System.Drawing.Color.White;
     Ath_setcolor(bnum);
     labgb[gph[bnum]] = K.Key;
     labgb[gpb[bnum]] = string.Join("\r\n", K.Value);
     blk.State = (int)Load_state.loaded;
     blk.Num = bnum;
    }
    bl_loaded[K.Key] = blk;
   }

   if (bl_used.Count > 0)
   {
    Scsiz(Width);
   }

   Update();

   foreach (KeyValuePair<string, Blk_entry> k in bl_loaded)
   {
    if (k.Value.State == (int)Load_state.loaded)
     Grpblocks[k.Value.Num].Visible = true;
   }

   Savemi.Visible = true;
   Savemi.Enabled = true;
   dsaves.Enabled = true;

   Set_empgb();
   Data_altered = false;
   //   Generate_labels();
   return;
  }

  public void Load_saved(Save_blk sblk)
  {
   int bnum;
   saved_type = true;
   Practice.Text = sblk.Practice;
   dept_tbox.Text = sblk.Department;
   Department.Text = sblk.Department;
   Patient.Text = sblk.Patient;
   Patientid.Text = sblk.Patient_id;
   Phone.Text = sblk.Phone;
   address.Text = sblk.Address;
   address.BackColor = System.Drawing.Color.White;
   DOB.Text = sblk.DOB;
   econtact.Text = sblk.Emergency_contact;
   econtact.BackColor = System.Drawing.Color.White;
   ins.Text = sblk.Insurance;
   ins.BackColor = System.Drawing.Color.White;
   if (!string.IsNullOrEmpty(sblk.Prtitle))
   {
    ppgb.Text = sblk.Prtitle;
    lab1["priph_title:"] = ppgb.Text;
   }

   prv_combo.Text = sblk.Priph;
   priph.Text = sblk.Priph;
   secph.Text = sblk.Secph;
   if (!string.IsNullOrEmpty(sblk.Sptitle) | !string.IsNullOrEmpty(secph.Text))
   {
    if (string.IsNullOrEmpty(sblk.Sptitle))
    {
     sptitle.Text = sp.Text;
    }
    else
    {
     sptitle.Text = sblk.Sptitle;
    }

    sp.Text = sptitle.Text;
    lab1["secph_title:"] = sp.Text;
    sp.Visible = true;
    secph.Visible = true;
    secph.Enabled = true;
    secph.BackColor = System.Drawing.Color.White;
   }

   foreach (Sblock sb in sblk.Blk_list)
   {
    bnum = sb.num;
    if (gpn.ContainsKey(bnum))
    {
     Grpblocks[bnum].Visible = false;
     Grpblocks[bnum].Controls[0].Text = sb.header;
     Grpblocks[bnum].Controls[0].Tag = sb.header;
     Grpblocks[bnum].Controls[0].Enabled = true;
     TextBox tb = (TextBox)Grpblocks[bnum].Controls[0];
     tb.ReadOnly = true;
     Grpblocks[bnum].Controls[1].Text = sb.body;
     Grpblocks[bnum].Controls[1].Enabled = true;
     RichTextBox rb = (RichTextBox)Grpblocks[bnum].Controls[1];
     rb.ReadOnly = true;
     rb.BackColor = System.Drawing.Color.White;
     Ath_setcolor(bnum);
     labgb[gph[bnum]] = sb.header;
     labgb[gpb[bnum]] = sb.body;
     bl_used[bnum] = bl_available[bnum];
     bl_available.Remove(bnum);
    }
   }

   Savemi.Visible = true;
   Savemi.Enabled = true;
   Deletemi.Visible = true;
   if (bl_used.Count > 0)
   {
    Scsiz(Width);
   }

   Update();
   foreach (KeyValuePair<int, string> k in bl_used)
   {
    Grpblocks[k.Key].Visible = true;
   }

   Set_empgb();
   Data_altered = false;
   Dsi = (KeyValuePair<string, Dsave>)dsaves.SelectedItem;
   return;
  }

  private void do_tbox(TextBox tb)
  {
   tb.Text = string.Empty;
   tb.ReadOnly = true;
   tb.BackColor = System.Drawing.Color.White;
  }

  private void Reset_fields(bool Skip_si = false)
  {
   bool Save_da = Data_altered;
   string gb_name;
   Editmenuitem.Text = "Edit";
   Dstate = Data_state.NoEdit;
   Dsbox.Enabled = true;
   if (L1_ret is object)
   {
    Practice.Text = L1_ret.Prc.name;
    Practice.ReadOnly = true;
    Practice.BackColor = System.Drawing.Color.White;
   }

   address.Text = string.Empty;
   address.ReadOnly = true;
   address.BackColor = System.Drawing.Color.White;
   dept_tbox.Text = string.Empty;
   Department.Text = "";
   ppgb.Text = "Primary Physician";
   do_tbox(DOB);
   do_tbox(Phone);
   do_tbox(priph);
   do_tbox(ins);
   do_tbox(econtact);
   do_tbox(secph);
   do_tbox(priph);
   do_tbox(dept_tbox);
   sp.Text = "";
   secph.Text = "";
   if (!Mpck.Sec_visible)
   {
    sp.Visible = false;
    secph.Visible = false;
    secph.Enabled = false;
   }
   else
   {
    sp.Visible = true;
    secph.Visible = true;
    secph.Enabled = true;
    secph.BackColor = System.Drawing.Color.White;
    if (!string.IsNullOrEmpty(Mpck.Sptitle))
    {
     sp.Text = Mpck.Sptitle;
    }
    else
    {
     sp.Text = "Specialist";
    }
   }

   Savemi.Visible = false;
   Deletemi.Visible = false;
   prv_combo.SelectedIndexChanged -= prv_combo_SelectedIndexChanged;
   prv_combo.SelectedItem = null;
   prv_combo.Text = string.Empty;
   prv_combo.SelectedIndexChanged += prv_combo_SelectedIndexChanged;
   Ettb.Text = null;
   if (!Skip_si)
   {
    dsaves.SelectedIndexChanged -= Dsaves_SelectedIndexChanged;
    dsaves.SelectedItem = null;
    dsaves.SelectedIndexChanged += Dsaves_SelectedIndexChanged;
   }

   try
   {
    lab1.Clear();

    bl_loaded.Clear();

    foreach (KeyValuePair<int, string> k in bl_used)
    {
     bl_available[k.Key] = k.Value;
    }

    bl_used.Clear();

    foreach (KeyValuePair<int, string> s in bl_available)
    {
     Grpblocks[s.Key].Visible = false;
     Grpblocks[s.Key].Controls[0].Text = string.Empty;
     Grpblocks[s.Key].Controls[1].Text = string.Empty;
    }

    currenty = Originaly;

    this.SuspendPaint();
    for (int i = 1, loopTo = Endpoints_in_use.Count; i <= loopTo; i++)
    {
     gb_name = bl_available[i];
     labgb[gph[i]] = string.Empty;
     labgb[gpb[i]] = string.Empty;
     currenty = Setcy(Grpblocks[i]);
    }
    Panels_delete();

    //   Reset_labels();

    Scsiz(Width, Originaly);

    this.ResumePaint();
   }
   catch (Exception ex)
   {
    string s = Program.Format_exception(ex);
    MessageBox.Show("Exception", s, MessageBoxButtons.OK);
    Application.Exit();
   }

   Data_altered = Save_da;
  }

  public string Trimc(ref string s1, string crm)
  {
   string sl = s1;
   for (int i = 0; i < crm.Length; i++)
   {
    sl = sl.Replace(crm.Substring(i, 1), "");
   }

   return sl;
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

   if (Max_rec[bn] <= 0 | bi.ll.Length <= Max_rec[bn])
   {
    return bi.lines;
   }

   bx.Select(0, bi.ll[Max_rec[bn]]);
   bx.SelectionBackColor = mp_backcolor;
   Application.DoEvents();
   bx.SelectionLength = 0;
   bx.Select(0, 0);
   return Max_rec[bn];
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
   if (!Max_rec.ContainsKey(bn))
   {
    return lines;
   }

   if (Max_rec[bn] <= 0 | ll.Length <= Max_rec[bn])
   {
    return lines;
   }

   for (int i = 0, loopTo = lines - 1; i <= loopTo; i += 1)
   {
    ll[i] = ln;
    ln = ln + bx.Lines[i].Length + 1;
   }

   Application.DoEvents();
   bx.Select(0, ll[Max_rec[bn]]);
   bx.SelectionBackColor = mp_backcolor;
   Application.DoEvents();
   bx.SelectionLength = 0;
   bx.Select(0, 0);
   return Max_rec[bn];
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
    Max_rec[a1.num] = a1.max_lines;
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
   Max_rec[blockno] = 0;
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
    if (Max_rec[bn] > 0 & lic > Max_rec[bn])
    {
     lic = Max_rec[bn];
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

   string Cid_encrypted = Ede.Encrypt(Cid, Ede.Scramble(enck));
   salt = Ede.Getsalt(Cid_encrypted);

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
    New_Eval = Ede.Decrypt(mcd.eval, Ede.Scramble(enck), 18926);
    if (!string.IsNullOrEmpty(New_Eval))
    {
     key.SetValue("eval", New_Eval);
    }
   }
   return New_Eval;
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
    payload["cid"] = Ede.Encrypt(cid, Set_key(Mpck.Secret, Mpck.Dlab), Convert.ToInt32(Mpck.Dlab.Substring(3)) + Mpck.Iterations);
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

  public string Format_phone(ref string s1)
  {
   string s2;
   var switchExpr = s1.Length;
   switch (switchExpr)
   {
    case 10:
     {
      s2 = "(" + s1.Substring(0, 3) + ") " + s1.Substring(3, 3) + "-" + s1.Substring(6, 4);
      break;
     }

    case 7:
     {
      s2 = s1.Substring(0, 3) + "-" + s1.Substring(3, 4);
      break;
     }

    default:
     {
      s2 = s1;
      break;
     }
   }

   return s2;
  }

  public void Logwr(string lin)
  {
   string format = "MM/dd/yyyy HH:mm:ss";
   if (logfile == default)
   {
    logfile = Properties.Settings.Default.start_dir + @"\medical_profile.log";
   }

   if (File.Exists(logfile))
   {
    using (var writer = new StreamWriter(logfile, true))
    {
     writer.WriteLine("[" + DateTime.Now.ToString(format) + "] " + lin);
    }
   }
   else
   {
    using (var writer = new StreamWriter(logfile, false))
    {
     writer.WriteLine("[" + DateTime.Now.ToString(format) + "] " + lin);
    }
   }
  }

  public static string start_timer(Stopwatch timer)
  {
   timer.Reset();
   timer.Start();
   return "";
  }

  public static string stop_timer(Stopwatch timer)
  {
   string et = null;
   timer.Stop();
   et = timer.ElapsedMilliseconds.ToString();
   return et;
  }

  private float Wlengb(string s, double pts = default)
  {
   float wl = 0.0F;
   double pt1;
   if (pts == default)
   {
    pt1 = points;
   }
   else
   {
    pt1 = pts;
   }

   for (int i = 0, loopTo = s.Length - 1; i <= loopTo; i++)
   {
    wl += Convert.ToSingle(cwdb[s[i]] * pt1);
   }

   wl += Convert.ToSingle(cwdb[32] * pt1);
   return wl;
  }

  private float Wleng(string s, double pts = default)
  {
   float wl = 0.0F;
   double pt1;
   if (pts == default)
   {
    pt1 = points;
   }
   else
   {
    pt1 = pts;
   }

   for (int i = 0, loopTo = s.Length - 1; i <= loopTo; i++)
   {
    wl += Convert.ToSingle(cwd[s[i]] * pt1);
   }

   wl += Convert.ToSingle(cwd[32] * pt1);
   return wl;
  }

  public string Adjust_lines(string lis, string lstate, Dictionary<int, int> adl = null)
  {
   var li = Regex.Split(lis, @"\r\n|\n");
   int lict = li.Count() - 1;
   var lsa = li;
   string[] lsl;
   var lst = new List<string>();
   int i, j, k;
   var sb = new StringBuilder();
   float sbps;
   float wds;
   bool chng;

   for (int il = 0; il < lsa.Length; il++)
   {
    lsa[il] = lsa[il].TrimEnd('\r', '\n');
    lsa[il] = lsa[il].TrimStart('\r', '\n');
   }

   lst = lsa.ToList();
   i = 0;
   j = lst.Count - 1;
   while (i <= j)
   {
    chng = false;
    lst[i] = lst[i].TrimStart();
    lst[i] = lst[i].TrimEnd('\v');
    lst[i] = lst[i].TrimEnd();
    if (lst[i].StartsWith("Last Reviewed"))
    {
     lst.RemoveRange(i, 1);
     j -= 1;
     if (lst[i].Length < 10)
     {
      lst.RemoveRange(i, 1);
      j -= 1;
     }

     continue;
    }

    if ((lstate ?? "") == "Label")
    {
     wds = Wleng(lst[i]);
     if (wds > label_length)
     {
      chng = true;
      sb.Clear();
      sbps = 0.0F;
      lsl = lst[i].ToString().Split(' ');
      lst.RemoveRange(i, 1);
      j -= 1;
      var loopTo = lsl.Count() - 1;
      for (k = 0; k <= loopTo; k++)
      {
       wds = Wleng(lsl[k]);
       if (wds + sbps > label_length)
       {
        lst.Insert(i, sb.ToString());
        j = j + 1;
        i = i + 1;
        sb.Clear();
        sb.Append("..");
        if (adl is object)
        {
         if (adl.ContainsKey(i))
         {
          adl[i] = adl[i] + 1;
         }
         else
         {
          adl[i] = 1;
         }
        }
       }

       sb.Append(lsl[k] + " ");
       sbps = Wleng(sb.ToString());
      }

      lst.Insert(i, sb.ToString());
      i += 1;
      j += 1;
     }

     if (chng)
     {
      i = i - 1;
      continue;
     }
    }

    if (lst[i].Length == 0)
    {
     lst.RemoveRange(i, 1);
     j = j - 1;
     continue;
    }

    i = i + 1;
   }

   return string.Join("\r\n", lst);
  }

  private int Setcolor(int bn, int nl)
  {
   RichTextBox bx = null;
   TextBox hx = null;
   Blk_info bi = null;
   hx = (TextBox)Controls[gpn[bn]].Controls[0];
   bx = (RichTextBox)Controls[gpn[bn]].Controls[1];
   bi = blocks[bn];
   int ln = 0;
   int tsize = bx.Text.Length;
   bx.Select(bi.ll[nl], tsize - 1);
   bx.SelectionBackColor = System.Drawing.Color.FromArgb(255, 196, 255, 255);
   hx.BackColor = System.Drawing.Color.FromArgb(255, 255, 210, 255);
   Application.DoEvents();
   bx.SelectionLength = 0;
   bx.Select(0, 0);
   return ln;
  }
  public static Bitmap ResizeImage(Bitmap bmSource, int TargetWidth, int TargetHeight)
  {
   var bmDest = new Bitmap(TargetWidth, TargetHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
   double nSourceAspectRatio = bmSource.Width / (double)bmSource.Height;
   double nDestAspectRatio = bmDest.Width / (double)bmDest.Height;
   int NewX = 0;
   int NewY = 0;
   int NewWidth = bmDest.Width;
   int NewHeight = bmDest.Height;
   if (nDestAspectRatio == nSourceAspectRatio)
   {
   }
   // same ratio
   else if (nDestAspectRatio > nSourceAspectRatio)
   {
    // Source is taller
    NewWidth = Convert.ToInt32(Math.Floor(nSourceAspectRatio * NewHeight));
    NewX = Convert.ToInt32(Math.Floor((bmDest.Width - NewWidth) / (double)2));
   }
   else
   {
    // Source is wider
    NewHeight = Convert.ToInt32(Math.Floor(1 / nSourceAspectRatio * NewWidth));
    NewY = Convert.ToInt32(Math.Floor((bmDest.Height - NewHeight) / (double)2));
   }

   using (var grDest = Graphics.FromImage(bmDest))
   {
    grDest.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
    grDest.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
    grDest.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
    grDest.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
    grDest.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
    grDest.DrawImage(bmSource, NewX, NewY, NewWidth, NewHeight);
   }

   return bmDest;
  }

  public string Ucfc(string value)
  {
   if (string.IsNullOrEmpty(value))
   {
    return string.Empty;
   }

   return Convert.ToString(char.ToUpper(value[0])) + value.Substring(1).ToLower();
  }

  //public byte[] Render(ILabel label)
  //{
  // ILabelRenderParams renderparams = new LabelRenderParams();
  // renderparams.FlowDirection = DYMO.Label.Framework.FlowDirection.LeftToRight;
  // renderparams.LabelColor = Colors.White;
  // renderparams.ShadowColor = Colors.DarkGray;
  // renderparams.ShadowDepth = 3;
  // renderparams.PngUseDisplayResolution = false;
  // var pngdata = label.RenderAsPng(printer, renderparams);
  // return pngdata;
  //}
 }
}