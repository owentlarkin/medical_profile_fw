Public Class Practices_Return
    Public Property token As String
    Public Property [next] As String
    Public Property totalcount As Integer
    Public Property practiceinfo As Practiceinfo_return()
End Class

Public Class Practiceinfo_return
    Public Property name As String
    Public Property publicnames As String()
End Class
