
namespace Medical_Profile
{
 public class Departments_Return
 {
  public string token { get; set; }
  public string next { get; set; }
  public int totalcount { get; set; }
  public Dept_Return[] departments { get; set; }
 }

 public class Dept_Return
 {
  public string state { get; set; }
  public string departmentid { get; set; }
  public string address { get; set; }
  public string name { get; set; }
  public string zip { get; set; }
  public string city { get; set; }
  public string fax { get; set; }
  public string providergroupname { get; set; }
  public string phone { get; set; }
 }
}