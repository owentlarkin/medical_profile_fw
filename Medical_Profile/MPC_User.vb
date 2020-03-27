
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks


Friend Class Blk_entry
  Public Property State As Integer
  Public Property Num As Integer
  Public Property Gb As GroupBox
End Class

#If DEBUG Then
Friend Class MPC_User
  Public Property User_Number As Integer
  Public Property Mkey As String
  Public Property Token As String
  Public Property Disk_Label As String
  Public Property Practice As String
  Public Property Secret1 As String
  Public Property Secret2 As String
  Public Property K1 As String
  Public Property K2 As String
  Public Property cid As String
  Public Property Salt As String
  Public Property Url As String
  Public Property Fname As String
  Public Property APIVersion As String
  Public Property Ftime As Long
  Public Property Email As String
  Public Property Iterations As Integer
  Public Property Phone As String
  Public Property Lines As String
  Public Property Blocks As String
  Public Property Sec_visible As String
  Public Property Sptitle As String
  Public Property Points As String
  Public Property Labels As String
  Public Property Comment As String
  Public Property Version As String
  Public Property Minimum_blocks As String
  Public Property Expiration As Date
  Public Property Blocklist As List(Of String)
End Class
#End If

Public Class MPC_key
 Public Property Mkey As String
 Public Property Email As String
 Public Property K1 As String
 Public Property Secret As String
 Public Property Dlab As String
 Public Property Url As String
 Public Property Salt As String
 Public Property Iterations As Integer
 Public Property Lines As String
 Public Property Blocks As String
 Public Property Sec_visible As Boolean
 Public Property Sptitle As String
 Public Property Points As String
 Public Property Labels As String
 Public Property Version As String
 Public Property Minimum_blocks As String
 Public Property Blocklist As List(Of String)
End Class

Friend Class MPC_type
  Public Property Akey As String
  Public Property F1 As String
End Class