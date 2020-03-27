
namespace Medical_Profile
{
 public class Practices_Return
 {
  public string token { get; set; }
  public string next { get; set; }
  public int totalcount { get; set; }
  public Practiceinfo_return[] practiceinfo { get; set; }
 }

 public class Practiceinfo_return
 {
  public string name { get; set; }
  public string[] publicnames { get; set; }
 }
}