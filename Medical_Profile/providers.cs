
namespace Medical_Profile
{
 public class Providers_return
 {
  public string Token { get; set; }
  public string next { get; set; }
  public Provider_return[] providers { get; set; }
  public int totalcount { get; set; }
 }

 public class Provider_return
 {
  public string Firstname { get; set; }
  public string Specialty { get; set; }
  public string Displayname { get; set; }
  public string Lastname { get; set; }
  public int Providerid { get; set; }
  public string Homedepartment { get; set; }
  public string Providerusername { get; set; }
  public string Providertype { get; set; }
 }
}