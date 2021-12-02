using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dymodkl;
 interface IDymolab
 {
 FontInfo GetRfont();
 TextObject GetRto();
  DesktopLabel FromXml(string Xml);
  DesktopLabel OpenLabel(string Filename);
  string ToXml(DesktopLabel D1, bool bomflag = true);
 }
