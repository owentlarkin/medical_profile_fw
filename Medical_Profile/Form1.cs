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
//using global::DymoSDK.Implementations;
//using global::DYMO.Label.Framework;
using global::JWT;
using global::Microsoft.Win32;
using global::Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using JR.Utils.GUI.Forms;
using Awsstd;
using DymoSDK.Implementations;

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
  //  private float ll = (float)(3600 / 15.0);
  private int nlead = 191;
  private int ylimit = 2386;
  int Xsize = 365;
  int Ysize = 192;
  int Blockno = 1;
  private delegate void cb();

  private delegate void InvokeDelegate();

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

    DymoSDK.App.Init();
    var xx = DymoLabel.Instance;

    try
    {
     xx.LoadLabelFromFilePath( Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + @"first.dymo");
    }
    catch (Exception Ex)
    {
     _ = Ex;
    }
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
   drive_label_encoded = null;
   eval_encoded = null;
   ath_blist.Clear();

   if (Rkeyvals.ContainsKey("Version"))
    installed_version = (string)Rkeyvals["Version"];

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


    if (eval_encoded is object)
    {
     file_access = true;
    }

    if (!file_access)
    {
     if (Rkeyvals.ContainsKey("Label"))
      drive_label_encoded = (string)Rkeyvals["Label"];

     if (drive_label_encoded is null)
     {
      using (Form f4 = new Formaik(cv))
      {
       f4.ShowDialog();

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

   List<string>  Printersx = Dlab.Get_Printer_Names();

   if (Printersx != null && Printersx.Count() > 0)
   {
    foreach (string P in Printersx)
    {
     Printers.Items.Add(P);
    }

    if (Printers.Items.Count > 0)
    {
     Printers.SelectedIndex = 0;
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
   aws_body["ukey"] = Ede.Encrypt(cid + Mpck.Dlab, cid + Mpck.Dlab, Mpck.Iterations);
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
    string New_Eval = Ede.Decrypt(L1_ret.Eval, Ede.Scramble(enck), 18926);
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
   Max_rec[Convert.ToInt32(tsi.Name)] = tsi.Value;
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
 //  Reset_labels();
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

  
  private void Form1_Resizeend(object sender, EventArgs e)
  {
   Control cntrl = (Control)sender;
   Form_Height = cntrl.Height;
   Scsiz(Width);
   Set_empgb();
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
     fpr.Text = "Labels Preview";
     // Preview = true;
     List<Byte[]> Imgs = Call_Generate(true);

     if (Imgs.Count < 1)
      return;

     tbl.Name = "Panel_Table";

     pccnt = Imgs.Count < 2 ? 1 : 2;
     prcnt = (Imgs.Count + 1) / pccnt;
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
       if (ind > Imgs.Count - 1)
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
        SizeMode = PictureBoxSizeMode.CenterImage,
        Anchor = AnchorStyles.None
       };
       using (var MS = new MemoryStream(Imgs[i * pccnt + j]))
       {
        i1 = (Bitmap)Image.FromStream(MS);
        pb.Image = i1;
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
   Call_Generate(false);
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
  }

  private async void Deletemi_Click(object sender, EventArgs e)
  {
   Dsave dsv = (Dsave)dsaves.SelectedValue;
   KeyValuePair<string, Dsave> Dsvsv = (KeyValuePair<string, Dsave>)dsaves.SelectedItem;
   var claims = Gen_Claims();
   Dsave_return Dr;
   Reset_fields();
   aws_body.Clear();
   aws_body["ukey"] = Ede.Encrypt(cid + Mpck.Dlab, cid + Mpck.Dlab, Mpck.Iterations);
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
   aws_body["ukey"] = Ede.Encrypt(cid + Mpck.Dlab, cid + Mpck.Dlab, Mpck.Iterations);
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
       js = Ede.Decrypt(Dvr.Dsave_value, Ede.Iterscramble(cid), Convert.ToInt32(Mpck.Iterations % 10 + 2));
       break;
      }

     default:
      {
       js = Ede.Decrypt(Dvr.Dsave_value, Ede.Iterscramble(cid, Mpck.Iterations), Convert.ToInt32(Mpck.Iterations / (double)3));
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