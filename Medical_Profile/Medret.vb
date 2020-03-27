Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Text

Public Class Alret
 Public status As Integer = 0
 Public message As String = Nothing
 Public token As String = ""
 Public al As ArrayList = New ArrayList()
End Class

Public Class Stret
 Public status As Integer = 0
 Public message As String = Nothing
 Public token As String = ""
 Public head As String = Nothing
 Public s1 As List(Of String) = New List(Of String)()
End Class

Public Class Mret
 Public status As Integer = 0
 Public message As String = ""
 Public body As String
End Class


Public Class Level1_Return
 Public code As Integer = 200
 Public message As String = ""
 Public Preview_practice As Boolean = False
 Public Prc As Practiceinfo_return
 Public Dpt As Dept_Return()
 Public Prv As Provider_return()
 Public Block_names As String()
 Public dsl As List(Of Dsave)
End Class

Public Class Match_Return
 Public Property Departmentid As String
 Public Property Lastname As String
 Public Property Firstname As String
 Public Property Patientid As String
 Public Property Dob As String
End Class

Public Class Level2_Return
 Public ex As Exception
 Public code As Integer = 200
 Public message As String
 Public Pat As Patient_Return
 Public mtr As List(Of Match_Return) = New List(Of Match_Return)()
 Public Ins As String = Nothing
 Public blks As Dictionary(Of String, List(Of String)) = New Dictionary(Of String, List(Of String))()
End Class

Public Class Register_Return
 Public ex As Exception
 Public code As Integer = 200
 Public message As String
 Public eval As String
End Class

Public Class Dsave_value_return
 Public ex As Exception
 Public code As Integer = 200
 Public message As String
 Public Dsave_value As String
End Class

Public Class Dsave
 Public Ukey As String
 Public Skey As String
 Public wrtim As String
 Public Name As String
 Public lwtim As String
 Public vers As String
End Class

Public Class Dsave_return
 Public ex As Exception
 Public code As Integer = 200
 Public message As String
 Public ds As List(Of Dsave)
 Public Dsave_value As String
End Class

Public Class Dsave_display
 Public Name As String
 Public Wrtim As String
 Public Skey As String
End Class
