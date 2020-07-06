using global::System;
using global::System.Collections;
using global::System.Collections.Generic;

namespace Medical_Profile
{
 public class Alret
 {
  public int status = 0;
  public string message = null;
  public string token = "";
  public ArrayList al = new ArrayList();
 }

 public class Stret
 {
  public int status = 0;
  public string message = null;
  public string token = "";
  public string head = null;
  public List<string> s1 = new List<string>();
 }

 public class Mret
 {
  public int status = 0;
  public string message = "";
  public string body;
 }

 public class Level1_Return
 {
  public int code = 200;
  public string message = "";
  public bool Preview_practice = false;
  public Practiceinfo_return Prc;
  public string Eval;
  public Dept_Return[] Dpt;
  public Provider_return[] Prv;
  public string[] Block_names;
  public List<Dsave> dsl;
 }

 public class Match_Return
 {
  public string Departmentid { get; set; }
  public string Lastname { get; set; }
  public string Firstname { get; set; }
  public string Patientid { get; set; }
  public string Dob { get; set; }
 }

 public class Level2_Return
 {
  public Exception ex;
  public int code = 200;
  public string message;
  public Patient_Return Pat;
  public List<Match_Return> mtr = new List<Match_Return>();
  public string Ins = null;
  public Dictionary<string, List<string>> blks = new Dictionary<string, List<string>>();
 }

 public class Register_Return
 {
  public Exception ex;
  public int code = 200;
  public string message;
  public string eval;
 }

 public class Ckup_Return
 {
  public Exception ex;
  public int code = 200;
  public string message;
  public string eval;
 }
 public class Dsave_value_return
 {
  public Exception ex;
  public int code = 200;
  public string message;
  public string Dsave_value;
 }

 public class Dsave
 {
  public string Ukey;
  public string Skey;
  public string wrtim;
  public string Name;
  public string lwtim;
  public string vers;
 }

 public class Dsave_return
 {
  public Exception ex;
  public int code = 200;
  public string message;
  public List<Dsave> ds;
  public string Dsave_value;
 }

 public class Dsave_display
 {
  public string Name;
  public string Wrtim;
  public string Skey;
 }
}