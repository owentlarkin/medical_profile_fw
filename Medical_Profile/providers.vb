Imports System

Public Class Providers_return
  Public Property Token As String
  Public Property [next] As String
 Public Property providers As Provider_return()
 Public Property totalcount As Integer
End Class

Public Class Provider_return
  Public Property Firstname As String
  Public Property Specialty As String
  Public Property Displayname As String
  Public Property Lastname As String
  Public Property Providerid As Integer
  Public Property Homedepartment As String
  Public Property Providerusername As String
  Public Property Providertype As String
End Class
