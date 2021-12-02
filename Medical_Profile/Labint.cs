using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


interface ILabint
{
 string Practice { get; set; }
 string Department { get; set; }
 string Printer { get; set; }
 string Patient { get; set; }
 List<string> Address { get; set; }
 string DOB { get; set; }
 string Phone { get; set; }
 string PPtitle { get; set; }
 string PPname { get; set; }
 string PPphone { get; set; }
 string SPtitle { get; set; }
 string SPname { get; set; }
 string Insurance { get; set; }
 string Econtact { get; set; }
 List<List<string>> Blocks { get; set; }
 List<Byte[]> Images { get; set; }

 int Generate(int labels_number, bool Preview = false);
}
