Imports System.Security.Cryptography
Imports System.Text

Namespace Enc
  Friend Class Rsa
    Public Shared Function GetKeys(ByVal Optional Length As Integer = 2048) As Tuple(Of String, String)
      Dim K1 As String = Nothing
      Dim K2 As String = Nothing
      Dim Rsa As Rsa = Nothing

      Try
        Using rsa1 As RSACng = New RSACng(Length)
          K1 = rsa1.ToXmlString(False)
          K2 = rsa1.ToXmlString(True)

          Return (New Tuple(Of String, String)(K1, K2))
        End Using
      Catch ex As Exception
        Console.WriteLine("Exception generating a new key pair! More info:")
        Console.WriteLine(ex.Message)
      End Try

      Return (New Tuple(Of String, String)(Nothing, Nothing))
    End Function

    Public Shared Function Encrypt(ByVal plainText As String, ByVal Key As String, ByVal Optional Keybits As Integer = 2048) As String
      Dim plainBytes As Byte() = Nothing
      Dim encryptedBytes As Byte() = Nothing

      Try

        Using rsa As RSACng = New RSACng(Keybits)
          rsa.FromXmlString(Key)
          plainBytes = Encoding.Unicode.GetBytes(plainText)
          encryptedBytes = rsa.Encrypt(plainBytes, RSAEncryptionPadding.Pkcs1)
          Return Convert.ToBase64String(encryptedBytes)
        End Using

      Catch ex As Exception
        Console.WriteLine("Exception encrypting file! More info:")
        Console.WriteLine(ex.Message)
      Finally
      End Try

      Return Nothing
    End Function

    Public Shared Function Decrypt(ByVal B64text As String, ByVal Key As String, ByVal Optional Keybits As Integer = 2048) As String
      Dim plainText As String = ""
      Dim encryptedBytes As Byte() = Nothing
      Dim plainBytes As Byte() = Nothing

      Try
        Using rsa As RSACng = New RSACng(Keybits)
          rsa.FromXmlString(Key)
          encryptedBytes = Convert.FromBase64String(B64text)
          plainBytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1)
          plainText = Encoding.Unicode.GetString(plainBytes)
        End Using
        Return plainText

      Catch ex As Exception
        Console.WriteLine("Exception decrypting file! More info:")
        Console.WriteLine(ex.Message)
      Finally
      End Try

      Return Nothing
    End Function


  End Class
End Namespace
