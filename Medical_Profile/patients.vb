Imports System

Public Class Patients_Return
 Public Property Token As String
 Public Property [Next] As String
 Public Property Patients As Patient_Return()
 Public Property Totalcount As Integer
End Class

Public Class Patient_Return
 Public Property Preferredname As String
 Public Property Email As String
 Public Property Suffix As String
 Public Property Departmentid As Integer?
 Public Property Primaryproviderid As Integer?
 Public Property Primarydepartmentid As Integer?
 Public Property Homephone As String
 Public Property Insurances As Insurance_Return()
 Public Property Zip As String
 Public Property Lastname As String
 Public Property Address1 As String
 Public Property City As String
 Public Property Lastappointment As DateTime
 Public Property Middlename As String
 Public Property Contactname As String
 Public Property Mobilephone As String
 Public Property Firstname As String
 Public Property State As String
 Public Property Patientid As Integer?
 Public Property Dob As String
 Public Property Contacthomephone As String
 Public Property Contactmobilephone As String
 Public Property Contactrelationship As String
 Public Property Insurance_string As String
End Class

Public Class Insurances_Return
 Public Property Token As String
 Public Property Insurances As Insurance_Return()
 Public Property Totalcount As Integer
End Class

Public Class Insurance_Return
 Public Property Sequencenumber As Integer
 Public Property Insuranceplanname As String
 Public Property Insurancephone As String
End Class
