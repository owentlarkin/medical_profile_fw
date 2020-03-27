﻿using DYMO.Label.Framework;
using JR.Utils.GUI.Forms;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Medical_Profile
{
 public partial class Form1
 {
  private IDieCutLabel Label;
  private StyledTextBuilder stb = new StyledTextBuilder();
  public StyledTextBuilder htb = new StyledTextBuilder();
  public StyledTextBlock lt;
  public int ltline;
  public int labelno = 0;
  public FontInfo reg_font;
  public FontInfo rp1_font;
  public FontInfo bld_font;
  public FontInfo bp1_font;
  public FontInfo itl_font;
  public int lines = 12;
  public double points = 6.0;
  public string printerName;
  public IPrinters pnames;
  public IPrinter printer;
  public ILabelWriterPrinter labelWriterPrinter;
  public ILabelWriterPrintParams lprintparams;
  public IPrintParams printParams;
  public IPrintJob pjob;
  public float name_length = 0.0F;
  public List<FileInfo> fi = new List<FileInfo>();
  public FontInfo nfnt = new FontInfo("Calibri", 12.0, DYMO.Label.Framework.FontStyle.Bold);
  public FontInfo lfnt = new FontInfo("Calibri", 9.0, DYMO.Label.Framework.FontStyle.Bold);
  public string flsort = "Date";
  public string enck = null;
  public string read_patient;

  public static void Main()
  {
   try
   {
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);
    Application.Run(new Form1());
   }
   catch (Exception ex)
   {
    string s = Format_exception(ex);
    Interaction.MsgBox("Exception", MsgBoxStyle.OkOnly, s);
    Application.Exit();
   }
  }

  public bool Check_altered()
  {
   DialogResult res;
   string msg = "You have unprocessed changes" + Constants.vbCrLf + "do you want to save these changes?";
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
      ds.Name = Enc.Enc256.Decrypt(ds.Skey, Enc.Enc256.Iterscramble(cid + Mpck.Dlab), Mpck.Iterations % 10 + 2);
      break;
     }

    default:
     {
      ds.Name = Enc.Enc256.Decrypt(ds.Skey, Enc.Enc256.Iterscramble(cid + Mpck.Dlab, Mpck.Iterations));
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
    dsaves.SelectedIndex = dsaves.FindStringExact(ds.Name);
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

  public async Task<int> Write_exception(string Ex, bool show = false)
  {
   string Wrtim = DateTime.Now.ToString("yyyyMMddHHmmss");
   var claims = Gen_Claims();
   Dsave_return Dr;
   aws_body.Clear();
   aws_body["ukey"] = Enc.Enc256.Encrypt(cid + Mpck.Dlab, cid + Mpck.Dlab, Mpck.Iterations);
   aws_body["wrtim"] = Wrtim;
   aws_body["exception"] = Ex;
   Application.UseWaitCursor = true;
   Dr = await Aws.Save_exception(Mpck.Url, enck, Mpck.Salt, claims, aws_body);
   Application.UseWaitCursor = false;
   if (show)
   {
    FlexibleMessageBox.Show(Ex, "Unhandled Exception");
   }

   return Dr.code;
  }

  public string Fe(Exception e, string header = "Unhandled Exception")
  {
   var erm = new StringBuilder();
   erm.Append(header);
   erm.Append(Constants.vbLf);
   erm.Append("Installed Version ");
   erm.Append(installed_version);
   erm.Append(Constants.vbLf);
   if (e is AggregateException)
   {
    AggregateException ae = e as AggregateException;
    erm.Append("One or more errors have occured in a background process." + Constants.vbLf);
    foreach (var e1 in ae.Flatten().InnerExceptions)
    {
     erm.Append(Constants.vbLf);
     erm.Append(e1.Message);
     if (e1.StackTrace is object)
     {
      erm.Append(Constants.vbLf + "Stack Trace:" + Constants.vbLf);
      erm.Append(e1.StackTrace);
     }
    }
   }
   else
   {
    erm.Append(e.Message);
    if (e.StackTrace is object)
    {
     erm.Append(Constants.vbLf + "Stack Trace:" + Constants.vbLf);
     erm.Append(e.StackTrace);
    }
   }

   return erm.ToString();
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
        dec_name = Enc.Enc256.Decrypt(d.Skey, Enc.Enc256.Iterscramble(cid + Mpck.Dlab), Mpck.Iterations % 10 + 2);
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
        dec_name = Enc.Enc256.Decrypt(d.Skey, Enc.Enc256.Iterscramble(cid + Mpck.Dlab, Mpck.Iterations));
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

     Console.WriteLine(sb.ToString());
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

  public void Load_Department(Level1_Return l1)
  {
   Department.Enabled = false;
   Department.Items.Clear();
   departments.Clear();
   atdp.Clear();
   for (int i = 0, loopTo = l1.Dpt.Length - 1; i <= loopTo; i++)
   {
    departments[Conversions.ToInteger(l1.Dpt[i].departmentid)] = l1.Dpt[i];
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

    if ((My.MySettingsProperty.Settings.department ?? "") == (k.Value.name ?? ""))
    {
     dfound = k.Value.name;
    }
   }

   if (!((My.MySettingsProperty.Settings.department ?? "") == (dfound ?? "")))
   {
    My.MySettingsProperty.Settings.department = dfound;
    My.MySettingsProperty.Settings.Save();
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
   Scsiz(Width, originaly);
   Invalidate();
   Update();
   Reset_fields();
   dsaves.SelectedIndexChanged -= Dsaves_SelectedIndexChanged;
   dsaves.SelectedItem = null;
   dsaves.SelectedIndexChanged += Dsaves_SelectedIndexChanged;
   foreach (int k in blocks.Keys)
   {
    max_rec[k] = 0;
   }

   foreach (Ath_block b in ath_blist)
   {
    if (b.max_lines > 0)
    {
     max_rec[b.num] = b.max_lines;
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
    term = Constants.vbCrLf;
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

   foreach (string b in Endpoints_in_use)
   {
    if (l2.blks.ContainsKey(b) && l2.blks[b].Count > 0)
    {
     int bnum = bl_available.First().Key;
     bl_used[bl_available.First().Key] = bl_available.First().Value;
     bl_available.Remove(bl_available.First().Key);
     Controls[gpn[bnum]].Visible = false;
     Controls[gpn[bnum]].Controls[0].Text = b;
     Controls[gpn[bnum]].Controls[0].Tag = b;
     Controls[gpn[bnum]].Controls[0].Enabled = true;
     TextBox tb = (TextBox)Controls[gpn[bnum]].Controls[0];
     tb.ReadOnly = true;
     Controls[gpn[bnum]].Controls[1].Text = string.Join(Constants.vbCrLf, l2.blks[b]);
     Controls[gpn[bnum]].Controls[1].Enabled = true;
     RichTextBox rb = (RichTextBox)Controls[gpn[bnum]].Controls[1];
     rb.ReadOnly = true;
     rb.BackColor = System.Drawing.Color.White;
     Ath_setcolor(bnum);
     labgb[gph[bnum]] = b;
     labgb[gpb[bnum]] = string.Join(Constants.vbCrLf, l2.blks[b]);
     bl_loaded[b].Num = bnum;
     bl_loaded[b].State = (int)Load_state.loaded;
    }
   }

   if (bl_used.Count > 0)
   {
    Scsiz(Width);
   }

   Update();
   foreach (KeyValuePair<int, string> k in bl_used)
   {
    Controls[k.Value].Visible = true;
   }

   // If pr_practice Then
   Savemi.Visible = true;
   Savemi.Enabled = true;
   dsaves.Enabled = true;
   // End If

   Set_empgb();
   Data_altered = false;
   // WireAllEvents(dsaves)
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
     Controls[gpn[bnum]].Visible = false;
     Controls[gpn[bnum]].Controls[0].Text = sb.header;
     Controls[gpn[bnum]].Controls[0].Tag = sb.header;
     Controls[gpn[bnum]].Controls[0].Enabled = true;
     TextBox tb = (TextBox)Controls[gpn[bnum]].Controls[0];
     tb.ReadOnly = true;
     Controls[gpn[bnum]].Controls[1].Text = sb.body;
     Controls[gpn[bnum]].Controls[1].Enabled = true;
     RichTextBox rb = (RichTextBox)Controls[gpn[bnum]].Controls[1];
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
   Deletemi.Visible = true;
   if (bl_used.Count > 0)
   {
    Scsiz(Width);
   }

   Update();
   foreach (KeyValuePair<int, string> k in bl_used)
   {
    Controls[k.Value].Visible = true;
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
    foreach (KeyValuePair<string, Blk_entry> k in bl_loaded)
    {
     k.Value.State = (int)Load_state.not_in_use;
     k.Value.Num = 0;
    }

    foreach (KeyValuePair<int, string> k in bl_used)
    {
     bl_available[k.Key] = k.Value;
    }

    bl_used.Clear();
    foreach (KeyValuePair<int, string> s in bl_available)
    {
     Controls[s.Value].Visible = false;
     Controls[s.Value].Controls[0].Text = string.Empty;
     Controls[s.Value].Controls[1].Text = string.Empty;
    }

    currenty = originaly;
    this.SuspendPaint();
    for (int i = 1, loopTo = Endpoints_in_use.Count; i <= loopTo; i++)
    {
     gb_name = bl_available[i];
     labgb[gph[i]] = string.Empty;
     labgb[gpb[i]] = string.Empty;
     currenty = Setcy((GroupBox)Controls[gb_name]);
    }

    Reset_labels();
    Scsiz(Width, originaly);
    this.ResumePaint();
   }
   catch (Exception ex)
   {
    string s = Format_exception(ex);
    Interaction.MsgBox("Exception", MsgBoxStyle.OkOnly, s);
    Application.Exit();
   }

   Data_altered = Save_da;
  }

  public string Trimc(ref string s1, string crm)
  {
   string sl = s1;
   foreach (char c in crm)
   {
    sl = sl.Replace(Conversions.ToString(c), "");
   }

   return sl;
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
    logfile = My.MySettingsProperty.Settings.start_dir + @"\medical_profile.log";
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

  public static string Format_exception(Exception e, string header = "Error obtaining data")
  {
   var erm = new StringBuilder();
   erm.Append(header);
   erm.Append(Constants.vbLf);
   if (e is AggregateException)
   {
    AggregateException ae = e as AggregateException;
    erm.Append("One or more errors have occured in a background process." + Constants.vbLf);
    foreach (var e1 in ae.Flatten().InnerExceptions)
    {
     erm.Append(Constants.vbLf);
     erm.Append(e1.Message);
     if (e1.StackTrace is object)
     {
      erm.Append(Constants.vbLf + "Stack Trace:" + Constants.vbLf);
      erm.Append(e1.StackTrace);
     }
    }
   }
   else
   {
    erm.Append(e.Message);
    if (e.StackTrace is object)
    {
     erm.Append(Constants.vbLf + "Stack Trace:" + Constants.vbLf);
     erm.Append(e.StackTrace);
    }
   }

   return erm.ToString();
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
    wl +=Convert.ToSingle(cwdb[s[i]] * pt1);
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

   for(int il = 0;il <lsa.Length ;il++)
   { 
    lsa[il] = lsa[il].TrimEnd(Conversions.ToChar(Constants.vbCrLf), Conversions.ToChar(Constants.vbCr), Conversions.ToChar(Constants.vbLf));
   lsa[il] = lsa[il].TrimStart(Conversions.ToChar(Constants.vbCrLf), Conversions.ToChar(Constants.vbCr), Conversions.ToChar(Constants.vbLf));
   }

   lst = lsa.ToList();
   i = 0;
   j = lst.Count - 1;
   while (i <= j)
   {
    chng = false;
    lst[i] = lst[i].TrimStart();
    lst[i] = lst[i].TrimEnd(Conversions.ToChar(Constants.vbVerticalTab));
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

   return string.Join(Constants.vbCrLf, lst);
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

  /* TODO ERROR: Skipped RegionDirectiveTrivia */
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

  /* TODO ERROR: Skipped EndRegionDirectiveTrivia */
  public string Ucfc(string value)
  {
   if (string.IsNullOrEmpty(value))
   {
    return string.Empty;
   }

   return Conversions.ToString(char.ToUpper(value[0])) + value.Substring(1).ToLower();
  }

  public byte[] Render(ILabel label)
  {
   ILabelRenderParams renderparams = new LabelRenderParams();
   renderparams.FlowDirection = DYMO.Label.Framework.FlowDirection.LeftToRight;
   renderparams.LabelColor = Colors.White;
   renderparams.ShadowColor = Colors.DarkGray;
   renderparams.ShadowDepth = 3;
   renderparams.PngUseDisplayResolution = false;
   var pngdata = label.RenderAsPng(printer, renderparams);
   return pngdata;
  }
 }
}