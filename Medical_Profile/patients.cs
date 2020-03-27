using global::System;

namespace Medical_Profile
{
 public class Patients_Return
 {
  public string Token { get; set; }
  public string Next { get; set; }
  public Patient_Return[] Patients { get; set; }
  public int Totalcount { get; set; }
 }

 public class Patient_Return
 {
  public string Preferredname { get; set; }
  public string Email { get; set; }
  public string Suffix { get; set; }
  public int? Departmentid { get; set; }
  public int? Primaryproviderid { get; set; }
  public int? Primarydepartmentid { get; set; }
  public string Homephone { get; set; }
  public Insurance_Return[] Insurances { get; set; }
  public string Zip { get; set; }
  public string Lastname { get; set; }
  public string Address1 { get; set; }
  public string City { get; set; }
  public DateTime Lastappointment { get; set; }
  public string Middlename { get; set; }
  public string Contactname { get; set; }
  public string Mobilephone { get; set; }
  public string Firstname { get; set; }
  public string State { get; set; }
  public int? Patientid { get; set; }
  public string Dob { get; set; }
  public string Contacthomephone { get; set; }
  public string Contactmobilephone { get; set; }
  public string Contactrelationship { get; set; }
  public string Insurance_string { get; set; }
 }

 public class Insurances_Return
 {
  public string Token { get; set; }
  public Insurance_Return[] Insurances { get; set; }
  public int Totalcount { get; set; }
 }

 public class Insurance_Return
 {
  public int Sequencenumber { get; set; }
  public string Insuranceplanname { get; set; }
  public string Insurancephone { get; set; }
 }
}