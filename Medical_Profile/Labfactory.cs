using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


static class Labfactory
{
 private static IDymolab _Idymolab = null;
 private static ILabint _Lab = null;
 public static ILabint GetLab()
 {
  _Lab = new Lab();

  _Lab.Address = new List<string>();
  _Lab.Blocks = new List<List<string>>();
  _Lab.Images = new List<byte[]>();

  return _Lab;
 }

 public static IDymolab GetIdymolab()
 {
  if (_Idymolab != null)
   return _Idymolab;

  _Idymolab = new Dymolab();
  return _Idymolab;
 }
}
