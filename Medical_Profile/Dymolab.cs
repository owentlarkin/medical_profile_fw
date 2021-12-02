using Dymodkl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

public class Dymolab :IDymolab
{
 private static FontInfo Rfont = new FontInfo
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

 private static TextObject Rto = new TextObject();

 private static XmlSerializer DLSerializer = new XmlSerializer(typeof(DesktopLabel));

 private static UltraMapper.Mapper mapper = new UltraMapper.Mapper();
 public Dymolab()
 {
  
 }

 public DesktopLabel FromXml(string Xml)
 {
  DesktopLabel Dl = new DesktopLabel();
  using (TextReader reader = new StringReader(Xml))
  {
   Dl = (DesktopLabel)DLSerializer.Deserialize(reader);
  }
  return Dl;
 }

 public TextObject GetRto()
 {
  return Rto;
 }
 public FontInfo GetRfont()
 {
  return Rfont;
 }

 public DesktopLabel OpenLabel(string Filename)
 {
  string Xml;
  DesktopLabel D1;// = new DesktopLabel();
  using (StreamReader sr = new StreamReader(Filename))
  {
   Xml = sr.ReadToEnd();
  }

  D1 = FromXml(Xml);

  return D1;
 }
 public string ToXml(DesktopLabel D1, bool bomflag = true)
 {
  string Xmlstr = "";

  XmlWriterSettings settings = new XmlWriterSettings();
  settings.Indent = true;
  settings.Encoding = new UTF8Encoding(bomflag);
  settings.NewLineChars = Environment.NewLine;
  settings.ConformanceLevel = ConformanceLevel.Document;

  var encb = new UTF8Encoding(true);

  using (var stream = new MemoryStream())
  {
   using (XmlWriter writer = XmlWriter.Create(stream, settings))
   {
    DLSerializer.Serialize(new XmlWriterEE(writer), D1);
   }

   byte[] v1 = stream.ToArray();

   Xmlstr = encb.GetString(v1);

   int pos = Xmlstr.IndexOf(@"></DYMOThickness>");

   if (pos > 0)
    Xmlstr = Xmlstr.Replace(@"></DYMOThickness>", @" />");

   return Xmlstr;
  }
 }
}
public class XmlWriterEE : XmlWriter
{
 private XmlWriter baseWriter;

 public XmlWriterEE(XmlWriter w)
 {
  baseWriter = w;
 }

 //Force WriteEndElement to use WriteFullEndElement
 public override void WriteEndElement() { baseWriter.WriteFullEndElement(); }

 public override void WriteFullEndElement()
 {
  baseWriter.WriteFullEndElement();
 }

 public override void Close()
 {
  baseWriter.Close();
 }

 public override void Flush()
 {
  baseWriter.Flush();
 }

 public override string LookupPrefix(string ns)
 {
  return (baseWriter.LookupPrefix(ns));
 }

 public override void WriteBase64(byte[] buffer, int index, int count)
 {
  baseWriter.WriteBase64(buffer, index, count);
 }

 public override void WriteCData(string text)
 {
  baseWriter.WriteCData(text);
 }

 public override void WriteCharEntity(char ch)
 {
  baseWriter.WriteCharEntity(ch);
 }

 public override void WriteChars(char[] buffer, int index, int count)
 {
  baseWriter.WriteChars(buffer, index, count);
 }

 public override void WriteComment(string text)
 {
  baseWriter.WriteComment(text);
 }

 public override void WriteDocType(string name, string pubid, string sysid, string subset)
 {
  baseWriter.WriteDocType(name, pubid, sysid, subset);
 }

 public override void WriteEndAttribute()
 {
  baseWriter.WriteEndAttribute();
 }

 public override void WriteEndDocument()
 {
  baseWriter.WriteEndDocument();
 }

 public override void WriteEntityRef(string name)
 {
  baseWriter.WriteEntityRef(name);
 }

 public override void WriteProcessingInstruction(string name, string text)
 {
  baseWriter.WriteProcessingInstruction(name, text);
 }

 public override void WriteRaw(string data)
 {
  baseWriter.WriteRaw(data);
 }

 public override void WriteRaw(char[] buffer, int index, int count)
 {
  baseWriter.WriteRaw(buffer, index, count);
 }

 public override void WriteStartAttribute(string prefix, string localName, string ns)
 {
  baseWriter.WriteStartAttribute(prefix, localName, ns);
 }

 public override void WriteStartDocument(bool standalone)
 {
  baseWriter.WriteStartDocument(standalone);
 }

 public override void WriteStartDocument()
 {
  baseWriter.WriteStartDocument();
 }

 public override void WriteStartElement(string prefix, string localName, string ns)
 {
  baseWriter.WriteStartElement(prefix, localName, ns);
 }

 public override WriteState WriteState
 {
  get { return baseWriter.WriteState; }
 }

 public override void WriteString(string text)
 {
  baseWriter.WriteString(text);
 }

 public override void WriteSurrogateCharEntity(char lowChar, char highChar)
 {
  baseWriter.WriteSurrogateCharEntity(lowChar, highChar);
 }

 public override void WriteWhitespace(string ws)
 {
  baseWriter.WriteWhitespace(ws);
 }
}

