using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DymoSDK.Implementations;
using System.Xml;
using System.Xml.Serialization;
using Dymodkl;
using DYMO.Label.Framework;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Forms;

namespace Medical_Profile
{
 public partial class Form1
 {
  private List<DymoLabel> Labels = new List<DymoLabel>();
  private List<Dymodkl.TextObject> Lt = new List<Dymodkl.TextObject>();
  private int Lno = 0;
  private decimal Ltop = 0m;
  private decimal Lbottom = 0m;

  private decimal Pl = 0.159722m;
  private decimal Cy_save;
  private decimal Yadj = 0m;
  private decimal Dll = 2.5m;
  private decimal Dcy = 0m;
  private decimal Lm = 0.2201389m;
  private Dymodkl.TextObject Get_to(string Name)
  {
   foreach (Dymodkl.TextObject T in Lt)
   {
    if (T.Name == Name)
     return T;
   }
   return null;
  }

  private decimal Setlo(string Name, string Val = "", int Ltnum = 0, int Tnum = 0)
  {
   Dymodkl.TextObject T0 = Get_to(Name);

   if (T0 != null)
   {
    T0.FMTTEXT.LTSS[Ltnum].TSPANS[Tnum].Text = Val;

    decimal x = Convert.ToDecimal(T0.ObjectLayout.DPOINT.Y) + Convert.ToDecimal(T0.ObjectLayout.Size.Height);

    return x;

   }

   return 0;
  }
 
  private decimal Add_to(string Name, string Btext, string Rtext, decimal x, decimal y, decimal w, decimal h, decimal pts = 8)
  {
   UltraMapper.Mapper mapper = new UltraMapper.Mapper();
   TextSpan S = null;

   Dymodkl.TextObject Newto = mapper.Map(Dds.Rto);

   Newto.Name = Name;

   Newto.ObjectLayout.DPOINT.X = x.ToString();
   Newto.ObjectLayout.DPOINT.Y = y.ToString();

   Newto.ObjectLayout.Size.Width = w.ToString();
   Newto.ObjectLayout.Size.Height = h.ToString();

   var TS = Newto.FMTTEXT.LTSS[0].TSPANS;

   TS.Clear();

   if (!string.IsNullOrEmpty(Btext))
   {
    S = new TextSpan();
    S.FONT = mapper.Map(Dds.F8b);
    S.FONT.FontSize = pts.ToString();
    S.Text = Btext;
    TS.Add(S);
   }

   if (!string.IsNullOrEmpty(Rtext))
   {
    S = new TextSpan();
    S.FONT = mapper.Map(Dds.F8);
    S.FONT.FontSize = pts.ToString();
    S.Text = Rtext;
    TS.Add(S);
   }

   Lt.Add(Newto);
   return y + h;
  }
  
  public void Finish_label(DesktopLabel Lbl, int N)
  {
   DymoLabel DLbl = new DymoLabel();
   string X1 = Dds.Serdtl(Lbl, false);
   DLbl.LoadLabelFromXML(X1);
   var x = DLbl.GetPreviewLabel();
   Labels.Add(DLbl);

#if DEBUG
   string F1 = Dds.Serdtl(Lbl, true);
   string lns = "Label" + N.ToString() + ".dymo";
   using (StreamWriter Sw = new StreamWriter(lns))
   {
    Sw.Write(F1);
   }
#endif

  }

  public DesktopLabel Openlabel(string LabelName)
  {
   DymoSDK.Implementations.DymoLabel D1 = new DymoLabel();
   Dymodkl.TextObject T = null;
   D1.LoadLabelFromFilePath(LabelName);
   DesktopLabel D2 = Dds.Destr(D1.XMLContent);
   Lt = D2.DYMOLabel.DLM.LOS.LTOS;
   List<string> Ln = (List<string>)(from DymoSDK.Interfaces.ILabelObject d in D1.GetLabelObjects() select d.Name as string).ToList();

   if (Ln.Contains("dob"))
    Setlo("dob", "DOB: " + DOB.Text);

   if (Ln.Contains("DOB"))
    Setlo("DOB", "DOB: " + DOB.Text);

   if (Ln.Contains("printed"))
    Setlo("printed", DateTime.Now.ToString("MM/dd/yyyy"), 0, 1);

   if (Ln.Contains("Date_Printed"))
    Setlo("Date_Printed", "Printed: " + DateTime.Now.ToString("MM/dd/yyyy"), 0, 0);

   Dcy = Setlo("name", Patient.Text);

   if (Ln.Contains("firstline"))
   {
    T = Get_to("firstline");
    Ltop = Convert.ToDecimal(T.ObjectLayout.DPOINT.Y);
    Dcy = Ltop;
    Lt.Remove(T);
   }

   if (Ln.Contains("lastline"))
   {
    T = Get_to("lastline");
    Lbottom = Convert.ToDecimal(T.ObjectLayout.DPOINT.Y);
    Lt.Remove(T);
   }

   return D2;
  }
  public void Generate_labels()
  {
   var Elines = new Dictionary<int, int>();
   decimal Nlead = 0.1326389m;
   decimal Y_space_needed;
   string Phone_number = null;
   StringBuilder Sb = new StringBuilder();
   string[] Li = null;
   string Lis;
   string P1 = @"(\(??\d\d\d\)??[\s|-]\d\d\d-\d\d\d\d)";
   string P2 = @"(\d\d\d-\d\d\d\d)";
   string S1 = null;
   Match M1;
   DesktopLabel DK1 = null;

   Labels.Clear();

   Lno = 0;
   DK1 = Openlabel("first.dymo");

   Li = address.Lines;
   Sb.Clear(); ;
   ltline = 0;

   for (int i = 0, loopTo = Li.Count() - 1; i <= loopTo; i++)
   {
    if (!((Li[i] ?? "") == (string.Empty ?? "")))
    {
     if (i > 0)
     {
      Sb.Append("\n");
     }

     Sb.Append(Li[i]);
     ltline++;
    }
   }

   if (Sb.Length > 0)
    Dcy = Add_to("address", null, Sb.ToString(), Lm, Dcy, Dll, ltline * Nlead);

   if (!string.IsNullOrEmpty(Phone.Text))
    Dcy = Add_to("phone", "Phone:", Phone.Text, Lm, Dcy, Dll, Nlead);

   if (!((econtact.Text ?? "") == (string.Empty ?? "")))
    Dcy = Add_to("econtact", "Emergency Contact: ", econtact.Text, Lm, Dcy, Dll, Nlead);

   string dname = priph.Text;

   if (prv_combo.Items.Count > 0)
    dname = prv_combo.Text;

   if (!string.IsNullOrEmpty(dname))
   {
    S1 = lab1["priph_title:"];
    if (!lab1["priph_title:"].EndsWith(":"))
     S1 = S1 + ":";
   }

   S1 = S1 + " ";

   Cy_save = Dcy;
   Pl = 0.159722m;
   Yadj = 0.020833m;
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
    Pl = 2 * Pl;
    dname = dname + @"\n" + Phone_number;
   }

   Dcy = Add_to("priphys", S1, dname, Lm, Dcy + Yadj, Dll, Pl, 9);

   S1 = "";

   if (lab1.ContainsKey("secph:"))
   {
    S1 = lab1["secph_title:"];
    if (!S1.EndsWith(":"))
     S1 = S1 + ":";
    Dcy = Add_to("spec1", S1, secph.Text, Lm, Dcy + Yadj, Dll, Pl);
    Yadj = 0m;
   }

   if (!((ins.Text ?? "") == (string.Empty ?? "")))
   {
    Dcy = Add_to("insurance", "Insurance: ", ins.Text, Lm, Dcy + Yadj, Dll, Nlead);
    Yadj = 0;
   }

   Finish_label(DK1, Lno);
   DK1 = Openlabel("next.dymo");
   Lno++;

   foreach (KeyValuePair<int, string> K in bl_used)
   {
    int Bnum = K.Key;
    GroupBox Gb = Grpblocks[Bnum];

    Y_space_needed = 0m;

    if (DK1 is null)
    {
     DK1 = Openlabel("next.dymo");
     Lno++;
    }

    if (Lno > labels_number)
     break;

    if (!string.IsNullOrEmpty(Gb.Controls[0].Text))
    {
     Y_space_needed += Nlead;


     if (string.IsNullOrEmpty(Gb.Controls[1].Text))
      Y_space_needed += Nlead;

     if (Dcy > Ltop & !string.IsNullOrEmpty(Gb.Controls[0].Text))
      Y_space_needed += Nlead / 2m;

     if (Lbottom - Dcy < Y_space_needed)
     {
      Finish_label(DK1, Lno);
      DK1 = Openlabel("next.dymo");
      Lno++;
      if (Lno > labels_number)
       break;
     }

     if (Dcy > Ltop & !string.IsNullOrEmpty(Gb.Controls[0].Text))
      Dcy += Nlead / 2;

     if (!string.IsNullOrEmpty(Gb.Controls[0].Text))
      Dcy = Add_to("L" + Dcy.ToString(), Gb.Controls[0].Text, null, Lm, Dcy, Dll, Nlead);
    }

    if (!string.IsNullOrEmpty(Gb.Controls[1].Text))
    {
     Lis = Gb.Controls[1].Text;
     Elines.Clear();
     if ((lines_setting ?? "") == "Label")
      Lis = Adjust_lines(Lis, "File");

     Lis = Adjust_lines(Lis, "Label", Elines);
     Li = Regex.Split(Lis, @"\r\n|\n");
     int Lic = Li.Count();
     if (Max_rec[Bnum] > 0 & Lic > Max_rec[Bnum])
     {
      Lic = Max_rec[Bnum];
      int Iadj = 0;
      foreach (KeyValuePair<int, int> Kvp in Elines)
      {
       if (Kvp.Key <= Lic)
        Iadj = Iadj + Kvp.Value;
      }

      Lic = Lic + Iadj;
     }

     for (int i = 0; i <= Li.Count() - 1; i++)
     {
      if (string.IsNullOrEmpty(Li[i]))
       Dcy += Nlead;
      else
       Dcy = Add_to("L" + Dcy.ToString(), null, Li[i], Lm, Dcy, Dll, Nlead);

      if (Lbottom - Dcy < Nlead)
      {
       Finish_label(DK1, Lno);
       DK1 = Openlabel("next.dymo");
       Lno++;
       if (Lno > labels_number)
       {
        break;
       }
      }
     }
    }
   }
   if (Dcy > Ltop)
    Finish_label(DK1, Lno);

   if (!Preview)
    DymoPrinter.Instance.PrintLabel(Labels, Printers.SelectedItem.ToString(), 1, barcodeGraphsQuality: true);
  }
 }
}
