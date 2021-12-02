using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DymoSDK.Implementations;
using System.Xml;
using System.Xml.Serialization;
using Dymodkl;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Forms;
using DymoSDK.Interfaces;

public class Lab : ILabint
{
 public string Practice { get; set; }
 public string Department { get; set; }
 public string Printer { get; set; }
 public string Patient { get; set; }
 public List<string> Address { get; set; }
 public string DOB { get; set; }
 public string Phone { get; set; }
 public string PPtitle { get; set; }
 public string PPname { get; set; }
 public string PPphone { get; set; }
 public string SPtitle { get; set; }
 public string SPname { get; set; }
 public string Insurance { get; set; }
 public string Econtact { get; set; }
 public List<List<string>> Blocks { get; set; }
 public List<Byte[]> Images { get; set; } = new List<byte[]>();

 private List<DymoLabel> Labels = new List<DymoLabel>();
 private List<Dymodkl.TextObject> Lt = new List<Dymodkl.TextObject>();
 private int Lno = 0;
 private decimal Ltop = 0m;
 private decimal Lbottom = 0m;

 private decimal Pl = 0.159722m;
 private decimal Yadj = 0m;
 private decimal Dll = 2.5m;
 private decimal Dcy = 0m;
 private decimal Lm = 0.2201389m;

 private IDymolab Dlab = null;

 private FontInfo Rfont = new FontInfo
 {
  FontBrush = new FontBrush()
  {
   SolidColorBrush = new SolidColorBrush
   {
    Color = new Color
    {
     A = "1",
     B = "0",
     G = "0",
     R = "0"
    }
   }
  },
  FontName = "Calibri",
  FontSize = "8",
  IsBold = "false",
  IsItalic = "false",
  IsUnderline = "false"
 };

 public Lab()
 {
  Dlab = Labfactory.GetIdymolab();
 }
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

  Dymodkl.TextObject Newto = mapper.Map(Dlab.GetRto());

  Newto.Name = Name;
  
  Newto.ObjectLayout.DPOINT.X = x.ToString();
  Newto.ObjectLayout.DPOINT.Y = y.ToString();

  Newto.ObjectLayout.Size.Width = w.ToString();
  Newto.ObjectLayout.Size.Height = h.ToString();

  if (Newto.FMTTEXT.LTSS.Count > 0)
   Newto.FMTTEXT.LTSS.Clear();

  Newto.FMTTEXT.LTSS.Add(new LineTextSpan());

  if (!string.IsNullOrEmpty(Btext))
  {
   S = new TextSpan
   {
    FONT = mapper.Map(Dlab.GetRfont())
   };
   S.FONT.FontSize = pts.ToString();
   S.FONT.IsBold = "true";
   S.Text = Btext;
   Newto.FMTTEXT.LTSS[0].TSPANS.Add(S);
  }

  if (!string.IsNullOrEmpty(Rtext))
  {
   S = new TextSpan
   {
    FONT = mapper.Map(Dlab.GetRfont())
   };
   S.FONT.FontSize = pts.ToString();
   S.FONT.IsBold = "false";
   S.Text = Rtext;
   Newto.FMTTEXT.LTSS[0].TSPANS.Add(S);
  }

  Lt.Add(Newto);
  return y + h;
 }

 private void Finish_label(DesktopLabel Lbl, int N)
 {
        IDymoLabel DLbl = DymoLabel.Instance;
  string X1 = Dlab.ToXml(Lbl, false);
  DLbl.LoadLabelFromXML(X1);
  var x = DLbl.GetPreviewLabel();
  Labels.Add((DymoLabel)DLbl);
  Images.Add(DLbl.Preview);

#if DEBUG
  string F1 = Dlab.ToXml(Lbl, true);
  string lns = "Label" + N.ToString() + ".dymo";
  using (StreamWriter Sw = new StreamWriter(lns))
  {
   Sw.Write(F1);
  }
#endif

 }

 private DesktopLabel Openlabel(string LabelName)
 {
        IDymoLabel D1 = DymoLabel.Instance;
  Dymodkl.TextObject T = null;
  D1.LoadLabelFromFilePath(LabelName);
  DesktopLabel D2 = Dlab.FromXml(D1.XMLContent);
  Lt = D2.DYMOLabel.DLM.LOS.LTOS;
  List<string> Ln = (List<string>)(from DymoSDK.Interfaces.ILabelObject d in D1.GetLabelObjects() select d.Name as string).ToList();

  if (Ln.Contains("dob"))
   Setlo("dob", "DOB: " + DOB);

  if (Ln.Contains("printed"))
   Setlo("printed", DateTime.Now.ToString("MM/dd/yyyy"), 0, 1);

  if (Ln.Contains("Date_Printed"))
   Setlo("Date_Printed", "Printed: " + DateTime.Now.ToString("MM/dd/yyyy"), 0, 0);

  Dcy = Setlo("name", Patient);

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
 public int Generate(int labels_number, bool Preview = false)
 {
  decimal Nlead = 0.1326389m;
  decimal Y_space_needed;
  StringBuilder Sb = new StringBuilder();
  int ltline = 0;
  DesktopLabel DK1;

  Images.Clear();

  Lno = 0;
  DK1 = Openlabel("first.dymo");

  Sb.Clear();

  ltline = 0;

  for (int i = 0, loopTo = Address.Count() - 1; i <= loopTo; i++)
  {
   if (!((Address[i] ?? "") == (string.Empty ?? "")))
   {
    if (i > 0)
    {
     Sb.Append("\n");
    }

    Sb.Append(Address[i]);
    ltline++;
   }
  }

  if (Sb.Length > 0)
   Dcy = Add_to("address", null, Sb.ToString(), Lm, Dcy, Dll, ltline * Nlead);

  if (!string.IsNullOrEmpty(Phone))
   Dcy = Add_to("phone", "Phone:", Phone, Lm, Dcy, Dll, Nlead);

  if (!((Econtact ?? "") == (string.Empty ?? "")))
   Dcy = Add_to("econtact", "Emergency Contact: ", Econtact, Lm, Dcy, Dll, Nlead);

  Pl = 0.159722m;
  Yadj = 0.020833m;


  if (!string.IsNullOrEmpty(PPphone))
  {
   Pl = 2 * Pl;
   PPname = PPname + @"\n" + PPphone;
  }

  Dcy = Add_to("priphys", PPtitle, PPname, Lm, Dcy + Yadj, Dll, Pl, 9);


  if (!string.IsNullOrEmpty(SPname))
  {
   Dcy = Add_to("spec1", SPtitle, SPname, Lm, Dcy + Yadj, Dll, Pl);
   Yadj = 0m;
  }

  if (!string.IsNullOrEmpty(Insurance))
  {
   Dcy = Add_to("insurance", "Insurance: ", Insurance, Lm, Dcy + Yadj, Dll, Nlead);
   Yadj = 0;
  }

  Finish_label(DK1, Lno);
  DK1 = Openlabel("next.dymo");
  Lno++;

  foreach (List<string> K in Blocks)
  {
   if (K.Count < 1)
    continue;

   Y_space_needed = 0m;

   if (DK1 is null)
   {
    DK1 = Openlabel("next.dymo");
    Lno++;
   }

   if (Lno > labels_number)
    break;

   if (!string.IsNullOrEmpty(K[0]))
   {
    Y_space_needed += Nlead;


    if (K.Count > 1)
     Y_space_needed += Nlead;

    if (Dcy > Ltop & K.Count > 1)
     Y_space_needed += Nlead / 2m;

    if (Lbottom - Dcy < Y_space_needed)
    {
     Finish_label(DK1, Lno);
     DK1 = Openlabel("next.dymo");

     Lno++;

     if (Lno > labels_number)
      break;
    }

    if (Dcy > Ltop & !string.IsNullOrEmpty(K[0]))
     Dcy += Nlead / 2;

    if (!string.IsNullOrEmpty(K[0]))
     Dcy = Add_to("L" + Dcy.ToString(), K[0], null, Lm, Dcy, Dll, Nlead);
   }

   for (int I = 1; I < K.Count; I++)
   {

    if (string.IsNullOrEmpty(K[I]))
     Dcy += Nlead;
    else
     Dcy = Add_to("L" + Dcy.ToString(), null, K[I], Lm, Dcy, Dll, Nlead);

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

  if (Dcy > Ltop)
   Finish_label(DK1, Lno);

  if (!Preview)
   DymoPrinter.Instance.PrintLabel(Labels, Printer, 1, barcodeGraphsQuality: true);

  return 0;
 }
}
