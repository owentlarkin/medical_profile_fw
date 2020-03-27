Imports System

Public Class Departments_Return
    Public Property token As String
    Public Property [next] As String
    Public Property totalcount As Integer
    Public Property departments As Dept_Return()
End Class

Public Class Dept_Return
    Public Property state As String
    Public Property departmentid As String
    Public Property address As String
    Public Property name As String
    Public Property zip As String
    Public Property city As String
    Public Property fax As String
    Public Property providergroupname As String
    Public Property phone As String
End Class
