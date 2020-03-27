
Public Class Ath_block
  Public num As Integer = -1
  Public max_lines As Integer = 10
  Public head As String = Nothing
  Public hkey As String = Nothing
  Public bkey As String = Nothing
  Public sortd() As String = Nothing
  Public endpoint As String = Nothing
End Class

Public Class Blk_info
  Public num As Integer = Nothing
  Public lines As Integer = Nothing
  Public hv As Integer = Nothing
  Public lv As Integer = Nothing
  Public ll() As Integer = Nothing
End Class

Public Class Save_blk
  Public Practice As String
  Public Patient As String = Nothing
  Public Patient_id As String = Nothing
  Public Address As String = Nothing
  Public DOB As String = Nothing
  Public Insurance As String = Nothing
  Public Phone As String = Nothing
  Public Department As String
  Public Sptitle As String = Nothing
  Public Secph As String = Nothing
  Public Priph As String = Nothing
  Public Prtitle As String = Nothing
  Public Emergency_contact As String = Nothing
  Public Sec_visible As Boolean
  Public Blk_list As List(Of Sblock) = New List(Of Sblock)
End Class

Public Class Sblock
  Public num As Integer
  Public header As String
  Public body As String
End Class
