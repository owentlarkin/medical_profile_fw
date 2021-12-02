using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;
using DymoSDK.Implementations;

namespace Dymodkl
{ 
 [XmlRoot(ElementName = "DYMOPoint")]
 public class DYMOPoint
 {
  [XmlElement(ElementName = "X")]
  public string X { get; set; } = "0";
  [XmlElement(ElementName = "Y")]
  public string Y { get; set; } = "0";
 }

 [XmlRoot(ElementName = "Size")]
 public class Size
 {
  [XmlElement(ElementName = "Width")]
  public string Width { get; set; } = "0";
  [XmlElement(ElementName = "Height")]
  public string Height { get; set; } = "0";
 }

 [XmlRoot(ElementName = "DYMORect")]
 public class DYMORect
 {
  [XmlElement(ElementName = "DYMOPoint")]
  public DYMOPoint DPt { get; set; }
  [XmlElement(ElementName = "Size")]
  public Size Size { get; set; }
 }

 [XmlRoot(ElementName = "Color")]
 public class Color
 {
  public Color()
  {
  }
  public Color(string a, string r = "0", string g = "0", string b = "0")
  {
   A = a;
   R = r;
   G = g;
   B = b;
  }

  [XmlAttribute(AttributeName = "A")]
  public string A { get; set; }
  [XmlAttribute(AttributeName = "R")]
  public string R { get; set; }
  [XmlAttribute(AttributeName = "G")]
  public string G { get; set; }
  [XmlAttribute(AttributeName = "B")]
  public string B { get; set; }
 }
         
 [XmlRoot(ElementName = "SolidColorBrush")]
 public class SolidColorBrush
 {
  [XmlElement(ElementName = "Color")]
  public Color Color { get; set; } = new Color { A = "1", R = "0", G = "0", B = "0" };
 }

 [XmlRoot(ElementName = "BorderColor")]
 public class BorderColor
 {
  [XmlElement(ElementName = "SolidColorBrush")]
  public SolidColorBrush SolidColorBrush { get; set; } = new SolidColorBrush { Color = new Color { A = "1", R = "0", G = "0", B = "0" } };
 }

 [XmlRoot(ElementName = "BackgroundBrush")]
 public class BackgroundBrush
 {
  [XmlElement(ElementName = "SolidColorBrush")]
  public SolidColorBrush SolidColorBrush { get; set; } = new SolidColorBrush { Color = new Color { A = "0", R = "1", G = "1", B = "1" } };
 }

 [XmlRoot(ElementName = "BorderBrush")]
 public class BorderBrush
 {
  [XmlElement(ElementName = "SolidColorBrush")]
  public SolidColorBrush SolidColorBrush { get; set; } = new SolidColorBrush { Color = new Color { A = "1", R = "0", G = "0", B = "0" } };
 }

 [XmlRoot(ElementName = "StrokeBrush")]
 public class StrokeBrush
 {
  [XmlElement(ElementName = "SolidColorBrush")]
  public SolidColorBrush SolidColorBrush { get; set; } = new SolidColorBrush { Color = new Color { A = "1", R = "0", G = "0", B = "0" } };
 }

 [XmlRoot(ElementName = "FillBrush")]
 public class FillBrush
 {
  [XmlElement(ElementName = "SolidColorBrush")]
  public SolidColorBrush SolidColorBrush { get; set; } = new SolidColorBrush { Color = new Color { A = "0", R = "0", G = "0", B = "0" } };
 }

 [XmlRoot(ElementName = "Brushes")]
 public class Brushes
 {  
  [XmlElement(ElementName = "BackgroundBrush")]
  public BackgroundBrush BackgroundBrush { get; set; } = new BackgroundBrush();
  [XmlElement(ElementName = "BorderBrush")]
  public BorderBrush BorderBrush { get; set; } = new BorderBrush();
  [XmlElement(ElementName = "StrokeBrush")]
  public StrokeBrush StrokeBrush { get; set; } = new StrokeBrush();
  [XmlElement(ElementName = "FillBrush")]
  public FillBrush FillBrush { get; set; } = new FillBrush();
 }

 [XmlRoot(ElementName = "DYMOThickness")]
 public class DYMOThickness
 {
  public DYMOThickness()
  {
  }

  public DYMOThickness(string left, string top, string right, string bottom)
  {
   Left = left;
   Top = top;
   Right = right;
   Bottom = bottom;
  }

  [XmlAttribute(AttributeName = "Left")]
  public string Left { get; set; }
  [XmlAttribute(AttributeName = "Top")]
  public string Top { get; set; }
  [XmlAttribute(AttributeName = "Right")]
  public string Right { get; set; }
  [XmlAttribute(AttributeName = "Bottom")]
  public string Bottom { get; set; }
 }

 [XmlRoot(ElementName = "Margin")]
 public class Margin
 {
  [XmlElement(ElementName = "DYMOThickness")]
  public DYMOThickness DYMOThickness { get; set; }
 }

 [XmlRoot(ElementName = "FontBrush")]
 public class FontBrush
 {
  [XmlElement(ElementName = "SolidColorBrush")]
  public SolidColorBrush SolidColorBrush { get; set; }
 }

 [XmlRoot(ElementName = "FontInfo")]
 public class FontInfo
 {
  [XmlElement(ElementName = "FontName")]
  public string FontName { get; set; }
  [XmlElement(ElementName = "FontSize")]
  public string FontSize { get; set; }
  [XmlElement(ElementName = "IsBold")]
  public string IsBold { get; set; }
  [XmlElement(ElementName = "IsItalic")]
  public string IsItalic { get; set; }
  [XmlElement(ElementName = "IsUnderline")]
  public string IsUnderline { get; set; }
  [XmlElement(ElementName = "FontBrush")]
  public FontBrush FontBrush { get; set; }
 }

 [XmlRoot(ElementName = "TextSpan")]
 public class TextSpan
 {
  [XmlElement(ElementName = "Text")]
  public string Text { get; set; }
  [XmlElement(ElementName = "FontInfo")]
  public FontInfo FONT { get; set; }
 }

 [XmlRoot(ElementName = "LineTextSpan")]
 public class LineTextSpan
 {
  [XmlElement(ElementName = "TextSpan")]
  public List<TextSpan> TSPANS { get; set; } = new List<TextSpan>();
  [XmlElement(ElementName = "FixedTextSpan")]
  public FixedTextSpan FTSPAN { get; set; }
 }

 [XmlRoot(ElementName = "FormattedText")]
 public class FormattedText
 {
  [XmlElement(ElementName = "FitMode")]
  public string FitMode { get; set; } = "None";
  [XmlElement(ElementName = "HorizontalAlignment")]
  public string HorizontalAlignment { get; set; } = "Left";
  [XmlElement(ElementName = "VerticalAlignment")]
  public string VerticalAlignment { get; set; } = "Top";
  [XmlElement(ElementName = "IsVertical")]
  public string IsVertical { get; set; } = "False";
  [XmlElement(ElementName = "LineTextSpan")]
  public List<LineTextSpan> LTSS { get; set; } = new List<LineTextSpan>();
 }

 [XmlRoot(ElementName = "ObjectLayout")]
 public class ObjectLayout
 {
  public ObjectLayout()
  {
  }

  public ObjectLayout(DYMOPoint dPOINT, Size size)
  {
   DPOINT = dPOINT;
   Size = size;
  }

  [XmlElement(ElementName = "DYMOPoint")]
  public DYMOPoint DPOINT { get; set; }
  [XmlElement(ElementName = "Size")]
  public Size Size { get; set; }
 }

 [XmlRoot(ElementName = "TextObject")]
 public class TextObject
 {
  [XmlElement(ElementName = "Name")]
  public string Name { get; set; }
  [XmlElement(ElementName = "Brushes")]
  public Brushes Brushes { get; set; } = new Brushes();
  [XmlElement(ElementName = "Rotation")]
  public string Rotation { get; set; } = "Rotation0";
  [XmlElement(ElementName = "OutlineThickness")]
  public string OutlineThickness { get; set; } = "1";
  [XmlElement(ElementName = "IsOutlined")]
  public string IsOutlined { get; set; } = "False";
  [XmlElement(ElementName = "BorderStyle")]
  public string BorderStyle { get; set; } = "SolidLine";
  [XmlElement(ElementName = "Margin")]
  public Margin Margin { get; set; } = new Margin { DYMOThickness = new DYMOThickness("0", "0", "0", "0") };
  [XmlElement(ElementName = "HorizontalAlignment")]
  public string HorizontalAlignment { get; set; } = "Left";
  [XmlElement(ElementName = "VerticalAlignment")]
  public string VerticalAlignment { get; set; } = "Top";
  [XmlElement(ElementName = "FitMode")]
  public string FitMode { get; set; } = "None";
  [XmlElement(ElementName = "IsVertical")]
  public string IsVertical { get; set; } = "False";
  [XmlElement(ElementName = "FormattedText")]
  public FormattedText FMTTEXT { get; set; } = new FormattedText();
  [XmlElement(ElementName = "ObjectLayout")]
  public ObjectLayout ObjectLayout { get; set; } = new ObjectLayout { DPOINT = new DYMOPoint(), Size = new Size() };
 }

 [XmlRoot(ElementName = "FixedTextSpan")]
 public class FixedTextSpan
 {
  [XmlElement(ElementName = "Text")]
  public string Text { get; set; }
  [XmlElement(ElementName = "FontInfo")]
  public FontInfo FONT { get; set; }
 }

 [XmlRoot(ElementName = "DateTimeObject")]
 public class DateTimeObject
 {
  [XmlElement(ElementName = "Name")]
  public string Name { get; set; }
  [XmlElement(ElementName = "Brushes")]
  public Brushes Brushes { get; set; }
  [XmlElement(ElementName = "Rotation")]
  public string Rotation { get; set; }
  [XmlElement(ElementName = "OutlineThickness")]
  public string OutlineThickness { get; set; }
  [XmlElement(ElementName = "IsOutlined")]
  public string IsOutlined { get; set; }
  [XmlElement(ElementName = "BorderStyle")]
  public string BorderStyle { get; set; }
  [XmlElement(ElementName = "Margin")]
  public Margin Margin { get; set; }
  [XmlElement(ElementName = "HorizontalAlignment")]
  public string HorizontalAlignment { get; set; }
  [XmlElement(ElementName = "VerticalAlignment")]
  public string VerticalAlignment { get; set; }
  [XmlElement(ElementName = "FitMode")]
  public string FitMode { get; set; }
  [XmlElement(ElementName = "IsVertical")]
  public string IsVertical { get; set; }
  [XmlElement(ElementName = "FormattedText")]
  public FormattedText FMTTEXT { get; set; }
  [XmlElement(ElementName = "Prefix")]
  public string Prefix { get; set; }
  [XmlElement(ElementName = "Postfix")]
  public string Postfix { get; set; }
  [XmlElement(ElementName = "Format")]
  public string Format { get; set; }
  [XmlElement(ElementName = "Use24HourTime")]
  public string Use24HourTime { get; set; }
  [XmlElement(ElementName = "TimeFormat")]
  public string TimeFormat { get; set; }
  [XmlElement(ElementName = "ObjectLayout")]
  public ObjectLayout ObjectLayout { get; set; }
 }

 [XmlRoot(ElementName = "LabelObjects")]
 public class LabelObjects
 {
  [XmlElement(ElementName = "TextObject")]
  public List<TextObject> LTOS { get; set; } = new List<TextObject>();
  [XmlElement(ElementName = "DateTimeObject")]
  public DateTimeObject DTO { get; set; }
 }

 [XmlRoot(ElementName = "DynamicLayoutManager")]
 public class DynamicLayoutManager
 {
  [XmlElement(ElementName = "RotationBehavior")]
  public string RotationBehavior { get; set; }
  [XmlElement(ElementName = "LabelObjects")]
  public LabelObjects LOS { get; set; }
 }

 [XmlRoot(ElementName = "DYMOLabel")]
 public class DYMOLabel
 {
  [XmlElement(ElementName = "Description")]
  public string Description { get; set; }
  [XmlElement(ElementName = "Orientation")]
  public string Orientation { get; set; }
  [XmlElement(ElementName = "LabelName")]
  public string LabelName { get; set; }
  [XmlElement(ElementName = "InitialLength")]
  public string InitialLength { get; set; }
  [XmlElement(ElementName = "BorderStyle")]
  public string BorderStyle { get; set; }
  [XmlElement(ElementName = "DYMORect")]
  public DYMORect RECT { get; set; }
  [XmlElement(ElementName = "BorderColor")]
  public BorderColor BorderColor { get; set; }
  [XmlElement(ElementName = "BorderThickness")]
  public string BorderThickness { get; set; }
  [XmlElement(ElementName = "Show_Border")]
  public string Show_Border { get; set; }
  [XmlElement(ElementName = "DynamicLayoutManager")]
  public DynamicLayoutManager DLM { get; set; }
  [XmlAttribute(AttributeName = "Version")]
  public string Version { get; set; }
 }

 [XmlRoot(ElementName = "DataTable")]
 public class DataTable
 {
  [XmlElement(ElementName = "Columns")]
  public string Columns { get; set; }
  [XmlElement(ElementName = "Rows")]
  public string Rows { get; set; }
 }

 [XmlRoot(ElementName = "DesktopLabel")]
 public class DesktopLabel
 {
  [XmlElement(ElementName = "DYMOLabel")]
  public DYMOLabel DYMOLabel { get; set; }
  [XmlElement(ElementName = "LabelApplication")]
  public string Application { get; set; }
  [XmlElement(ElementName = "DataTable")]
  public DataTable DataTable { get; set; }
  [XmlAttribute(AttributeName = "Version")]
  public string Version { get; set; }
 }
}

