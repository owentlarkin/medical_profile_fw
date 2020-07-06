using global::System;
using global::System.Collections.Generic;
using System.Windows.Forms;

namespace Medical_Profile
{
 internal class Blk_entry
 {
 public string header { get; set; }
  public int State { get; set; }
  public int Num { get; set; }
 }

 /* TODO ERROR: Skipped IfDirectiveTrivia */
 internal class MPC_User
 {
  public int User_Number { get; set; }
  public string Mkey { get; set; }
  public string Token { get; set; }
  public string Disk_Label { get; set; }
  public string Practice { get; set; }
  public string Secret1 { get; set; }
  public string Secret2 { get; set; }
  public string K1 { get; set; }
  public string K2 { get; set; }
  public string cid { get; set; }
  public string Salt { get; set; }
  public string Url { get; set; }
  public string Fname { get; set; }
  public string APIVersion { get; set; }
  public long Ftime { get; set; }
  public string Email { get; set; }
  public int Iterations { get; set; }
  public string Phone { get; set; }
//  public string Lines { get; set; }
//  public string Blocks { get; set; }
  public string Sec_visible { get; set; }
  public string Sptitle { get; set; }
//  public string Points { get; set; }
//  public string Labels { get; set; }
  public string Comment { get; set; }
  public string Version { get; set; }
  public string Minimum_blocks { get; set; }
  public DateTime Expiration { get; set; }
  public List<string> Blocklist { get; set; }
 }
 
 public class MPC_key
 {
  public string Mkey { get; set; }
  public string Email { get; set; }
  public string K1 { get; set; }
  public string Secret { get; set; }
  public string Dlab { get; set; }
  public string Url { get; set; }
  public string Salt { get; set; }
  public int Iterations { get; set; }
  public string Lines { get; set; }
  public string Blocks { get; set; }
  public bool Sec_visible { get; set; }
  public string Sptitle { get; set; }
  public string Points { get; set; }
  public string Labels { get; set; }
  public string Version { get; set; }
  public string Minimum_blocks { get; set; }
  public List<string> Blocklist { get; set; }
 }

 internal class MPC_type
 {
  public string Akey { get; set; }
  public string F1 { get; set; }
 }
}