using System.Collections.Generic;

namespace Medical_Profile
{
 public class Ath_block
 {
  public int num = -1;
  public int max_lines = 10;
  public string head = null;
  public string hkey = null;
  public string bkey = null;
  public string[] sortd = null;
  public string endpoint = null;
 }

 public class Blk_info
 {
  public int num = default;
  public int lines = default;
  public int hv = default;
  public int lv = default;
  public int[] ll = null;
 }

 public class Save_blk
 {
  public string Practice;
  public string Patient = null;
  public string Patient_id = null;
  public string Address = null;
  public string DOB = null;
  public string Insurance = null;
  public string Phone = null;
  public string Department;
  public string Sptitle = null;
  public string Secph = null;
  public string Priph = null;
  public string Prtitle = null;
  public string Emergency_contact = null;
  public bool Sec_visible;
  public List<Sblock> Blk_list = new List<Sblock>();
 }

 public class Sblock
 {
  public int num;
  public string header;
  public string body;
 }
}