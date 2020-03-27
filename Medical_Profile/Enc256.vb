Imports JWT
Imports JWT.Algorithms
Imports JWT.Serializers
Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text

Namespace Enc
 Friend Module Enc256
  Private ReadOnly Random As RandomNumberGenerator = RandomNumberGenerator.Create()
  Public ReadOnly BlockBitSize As Integer = 128
  Public ReadOnly KeyBitSize As Integer = 256
  Public ReadOnly SaltBitSize As Integer = 64
  Public ReadOnly Iterations As Integer = 10000
  Public ReadOnly MinPasswordLength As Integer = 12

  Function Encode(ByVal secret As String, ByVal salt As String, ByVal payload As Dictionary(Of String, Object), ByVal iter As Integer) As String
   Dim algorithm As IJwtAlgorithm = New HMACSHA256Algorithm()
   Dim serializer As IJsonSerializer = New JsonNetSerializer()
   Dim urlEncoder As IBase64UrlEncoder = New JwtBase64UrlEncoder()
   Dim encoder As IJwtEncoder = New JwtEncoder(algorithm, serializer, urlEncoder)
   Dim token = encoder.Encode(payload, NewKey(secret, salt, iter))
   Return (token)
  End Function

  Function Encode(ByVal secret As String, ByVal Salt As String, ByVal payload As Dictionary(Of String, Object)) As String
   Dim algorithm As IJwtAlgorithm = New HMACSHA256Algorithm()
   Dim serializer As IJsonSerializer = New JsonNetSerializer()
   Dim urlEncoder As IBase64UrlEncoder = New JwtBase64UrlEncoder()
   Dim encoder As IJwtEncoder = New JwtEncoder(algorithm, serializer, urlEncoder)
   Dim saltb As Byte() = Convert.FromBase64String(Salt)
   Dim iter As Integer = CInt(Math.Abs(Convert.ToInt64((BitConverter.ToUInt64(saltb, 0) + 10000) Mod 10000)))
   Dim token = encoder.Encode(payload, NewKey(secret, Salt, iter))
   token = Salt & "." & token
   Return (token)
  End Function

  Function Decode(ByVal token As String, ByVal secret As String, ByVal salt As String, ByVal iter As Integer) As IDictionary(Of String, Object)
   Dim rdict As IDictionary(Of String, Object) = New Dictionary(Of String, Object)()

   Try
    Dim serializer As IJsonSerializer = New JsonNetSerializer()
    Dim provider As IDateTimeProvider = New UtcDateTimeProvider()
    Dim validator As IJwtValidator = New JwtValidator(serializer, provider)
    Dim urlEncoder As IBase64UrlEncoder = New JwtBase64UrlEncoder()
    Dim decoder As IJwtDecoder = New JwtDecoder(serializer, validator, urlEncoder)
    Dim dict = decoder.DecodeToObject(Of IDictionary(Of String, Object))(token, NewKey(secret, salt, iter), verify:=True)
    Return (dict)
   Catch __unusedTokenExpiredException1__ As TokenExpiredException
    rdict("Error") = "Token has expired"
    Return rdict
   Catch __unusedSignatureVerificationException2__ As SignatureVerificationException
    rdict("Error") = "Token has invalid signature"
    Return rdict
   End Try
  End Function

  Function Decode(ByVal token As String, ByVal secret As String) As IDictionary(Of String, Object)
   Dim i = token.IndexOf(".")
   Dim salt As String = token.Substring(0, i)
   Dim Stoken As String = token.Substring(i + 1)
   Dim saltb As Byte() = Convert.FromBase64String(salt)
   Dim iter As Integer = CInt(Math.Abs(Convert.ToInt64((BitConverter.ToUInt64(saltb, 0) + 10000) Mod 10000)))
   Return (Decode(Stoken, secret, salt, iter))
  End Function

    Function NewKey(ByVal password As String, ByVal salt As String, ByVal Optional Iterations As Integer = 10000) As String
      Dim saltb = Convert.FromBase64String(salt)
      Dim key As Byte() = New Byte(KeyBitSize / 8 - 1) {}

      Using generator = New Rfc2898DeriveBytes(password, saltb, Iterations)
        key = generator.GetBytes(KeyBitSize / 8)
      End Using

      Return Convert.ToBase64String(key, 0, key.Length)
    End Function

    Function Next_open(ByVal tg As Byte(), ByVal st As Integer) As Integer
      Dim p As Integer = st + 1
      If p >= tg.Length Then p = 0

      While True

        If tg(p) = &HFF Then
          Exit While
        End If

        p += 1

        If p = tg.Length Then
          p = 0
        End If
      End While

      Return p
    End Function

    Function Iterscramble(ByVal inp As String, Optional ByVal iter As Integer = 1) As String
      Dim pstring As String = inp
      For i As Integer = 0 To iter
        pstring = Scramble(pstring)
      Next
      Return pstring
    End Function

    Function Scramble(ByVal inp As String) As String
      Dim b1 As Byte() = New Byte(inp.Length - 1) {}
      Dim b2 As Byte() = New Byte(inp.Length - 1) {}
      Dim car() As Char = inp.ToCharArray

      For i As Integer = 0 To b1.Length - 1
        b1(i) = CByte(i)
      Next

      For j As Integer = 0 To b2.Length - 1
        b2(j) = &HFF
      Next

      For i As Integer = 0 To b2.Length - 1
        b2(Next_open(b2, i + i)) = Convert.ToByte(car(i))
      Next

      Return System.Text.Encoding.[Default].GetString(b2)
    End Function

    Function SimpleEncrypt(ByVal secretMessage As String, ByVal cryptKey As Byte(), ByVal authKey As Byte(), ByVal Optional nonSecretPayload As Byte() = Nothing) As String
   If String.IsNullOrEmpty(secretMessage) Then
    Throw New ArgumentException("Secret Message Required!", "secretMessage")
   End If

   Dim plainText = Encoding.UTF8.GetBytes(secretMessage)
   Dim cipherText = SimpleEncrypt(plainText, cryptKey, authKey, nonSecretPayload)
   Return Convert.ToBase64String(cipherText)
  End Function

  Function SimpleDecrypt(ByVal encryptedMessage As String, ByVal cryptKey As Byte(), ByVal authKey As Byte(), ByVal Optional nonSecretPayloadLength As Integer = 0) As String
   If String.IsNullOrWhiteSpace(encryptedMessage) Then
    Throw New ArgumentException("Encrypted Message Required!", "encryptedMessage")
   End If

   Dim cipherText = Convert.FromBase64String(encryptedMessage)
   Dim plainText = SimpleDecrypt(cipherText, cryptKey, authKey, nonSecretPayloadLength)
   Return If(plainText Is Nothing, Nothing, Encoding.UTF8.GetString(plainText))
  End Function

  Function Encrypt(ByVal secretmessage As String, ByVal password As String, ByVal Label As String) As String
   Dim v1 As Integer = 0
   Dim l1 As Integer = Convert.ToInt32(Label.Substring(3))

   For i As Integer = 3 To Label.Length - 1
    v1 += Convert.ToInt32(Label.Substring(i, 1))
   Next

   Dim insert_loc As Integer = v1 Mod (Label.Length - 4)
   Dim k1 As String = password.Substring(0, insert_loc) & Label & password.Substring(insert_loc)
   Return (Encrypt(secretmessage, k1, l1))
  End Function

  Function Encrypt(ByVal secretMessage As String, ByVal password As String, ByVal Optional iter As Integer = 10000, ByVal Optional nonSecretPayload As Byte() = Nothing) As String
   If String.IsNullOrEmpty(secretMessage) Then
    Throw New ArgumentException("Secret Message Required!", "secretMessage")
   End If

   Dim plainText = Encoding.UTF8.GetBytes(secretMessage)
   Dim cipherText = SimpleEncryptWithPassword(plainText, password, iter, nonSecretPayload)
   Return Convert.ToBase64String(cipherText)
  End Function

  Function SimpleEncryptWithPassword(ByVal secretMessage As String, ByVal password As String, ByVal iter As Integer, ByVal Optional nonSecretPayload As Byte() = Nothing) As String
   If String.IsNullOrEmpty(secretMessage) Then
    Throw New ArgumentException("Secret Message Required!", "secretMessage")
   End If

   Dim plainText = Encoding.UTF8.GetBytes(secretMessage)
   Dim cipherText = SimpleEncryptWithPassword(plainText, password, iter, nonSecretPayload)
   Return Convert.ToBase64String(cipherText)
  End Function

  Function Decrypt(ByVal secretmessage As String, ByVal password As String, ByVal Label As String) As String
   Dim v1 As Integer = 0
   Dim l1 As Integer = Convert.ToInt32(Label.Substring(3))

   For i As Integer = 3 To Label.Length - 1
    v1 += Convert.ToInt32(Label.Substring(i, 1))
   Next

   Dim insert_loc As Integer = v1 Mod (Label.Length - 4)
   Dim k1 As String = password.Substring(0, insert_loc) & Label & password.Substring(insert_loc)
   Return (Decrypt(secretmessage, k1, l1))
  End Function

  Function Decrypt(ByVal encryptedMessage As String, ByVal password As String, ByVal Optional iter As Integer = 10000, ByVal Optional nonSecretPayloadLength As Integer = 0) As String
   If String.IsNullOrWhiteSpace(encryptedMessage) Then
    Throw New ArgumentException("Encrypted Message Required!", "encryptedMessage")
   End If

   Dim cipherText = Convert.FromBase64String(encryptedMessage)
   Dim plainText = SimpleDecryptWithPassword(cipherText, password, iter, nonSecretPayloadLength)
   Return If(plainText Is Nothing, Nothing, Encoding.UTF8.GetString(plainText))
  End Function

  Function SimpleDecryptWithPassword(ByVal encryptedMessage As String, ByVal password As String, ByVal Optional iter As Integer = 10000, ByVal Optional nonSecretPayloadLength As Integer = 0) As String
   If String.IsNullOrWhiteSpace(encryptedMessage) Then
    Throw New ArgumentException("Encrypted Message Required!", "encryptedMessage")
   End If

   Dim cipherText = Convert.FromBase64String(encryptedMessage)
   Dim plainText = SimpleDecryptWithPassword(cipherText, password, nonSecretPayloadLength)
   Return If(plainText Is Nothing, Nothing, Encoding.UTF8.GetString(plainText))
  End Function

  Function SimpleEncrypt(ByVal secretMessage As Byte(), ByVal cryptKey As Byte(), ByVal authKey As Byte(), ByVal Optional nonSecretPayload As Byte() = Nothing) As Byte()
   If cryptKey Is Nothing OrElse cryptKey.Length <> KeyBitSize / CDbl(8) Then
    Throw New ArgumentException(String.Format("Key needs to be {0} bit!", KeyBitSize), "cryptKey")
   End If

   If authKey Is Nothing OrElse authKey.Length <> KeyBitSize / CDbl(8) Then
    Throw New ArgumentException(String.Format("Key needs to be {0} bit!", KeyBitSize), "authKey")
   End If

   If secretMessage Is Nothing OrElse secretMessage.Length < 1 Then
    Throw New ArgumentException("Secret Message Required!", "secretMessage")
   End If

   nonSecretPayload = If(nonSecretPayload, New Byte() {})
   Dim cipherText As Byte()
   Dim iv As Byte()

   Using aes = New AesManaged() With {
       .KeySize = KeyBitSize,
       .BlockSize = BlockBitSize,
       .Mode = CipherMode.CBC,
       .Padding = PaddingMode.PKCS7
   }
    aes.GenerateIV()
    iv = aes.IV

    Using encrypter = aes.CreateEncryptor(cryptKey, iv)

     Using cipherStream = New MemoryStream()

      Using cryptoStream = New CryptoStream(cipherStream, encrypter, CryptoStreamMode.Write)

       Using binaryWriter = New BinaryWriter(cryptoStream)
        binaryWriter.Write(secretMessage)
       End Using
      End Using

      cipherText = cipherStream.ToArray()
     End Using
    End Using
   End Using

   Using hmac = New HMACSHA256(authKey)

    Using encryptedStream = New MemoryStream()

     Using binaryWriter = New BinaryWriter(encryptedStream)
      binaryWriter.Write(nonSecretPayload)
      binaryWriter.Write(iv)
      binaryWriter.Write(cipherText)
      binaryWriter.Flush()
      Dim tag = hmac.ComputeHash(encryptedStream.ToArray())
      binaryWriter.Write(tag)
     End Using

     Return encryptedStream.ToArray()
    End Using
   End Using
  End Function

  Function SimpleDecrypt(ByVal encryptedMessage As Byte(), ByVal cryptKey As Byte(), ByVal authKey As Byte(), ByVal Optional nonSecretPayloadLength As Integer = 0) As Byte()
   If cryptKey Is Nothing OrElse cryptKey.Length <> KeyBitSize / CDbl(8) Then
    Throw New ArgumentException(String.Format("CryptKey needs to be {0} bit!", KeyBitSize), "cryptKey")
   End If

   If authKey Is Nothing OrElse authKey.Length <> KeyBitSize / CDbl(8) Then
    Throw New ArgumentException(String.Format("AuthKey needs to be {0} bit!", KeyBitSize), "authKey")
   End If

   If encryptedMessage Is Nothing OrElse encryptedMessage.Length = 0 Then
    Throw New ArgumentException("Encrypted Message Required!", "encryptedMessage")
   End If

   Using hmac = New HMACSHA256(authKey)
    Dim sentTag = New Byte(hmac.HashSize / 8 - 1 + 1 - 1) {}
    Dim calcTag = hmac.ComputeHash(encryptedMessage, 0, encryptedMessage.Length - sentTag.Length)
    Dim ivLength = (BlockBitSize / 8)

    If encryptedMessage.Length < sentTag.Length + nonSecretPayloadLength + ivLength Then
     Return Nothing
    End If

    Array.Copy(encryptedMessage, encryptedMessage.Length - sentTag.Length, sentTag, 0, sentTag.Length)
    Dim compare = 0
    Dim loopTo = sentTag.Length - 1

    For i = 0 To loopTo
     compare = compare Or sentTag(i) Xor calcTag(i)
    Next

    If compare <> 0 Then
     Return Nothing
    End If

    Using aes = New AesManaged() With {
        .KeySize = KeyBitSize,
        .BlockSize = BlockBitSize,
        .Mode = CipherMode.CBC,
        .Padding = PaddingMode.PKCS7
    }
     Dim iv = New Byte(ivLength - 1 + 1 - 1) {}
     Array.Copy(encryptedMessage, nonSecretPayloadLength, iv, 0, iv.Length)

     Using decrypter = aes.CreateDecryptor(cryptKey, iv)

      Using plainTextStream = New MemoryStream()

       Using decrypterStream = New CryptoStream(plainTextStream, decrypter, CryptoStreamMode.Write)

        Using binaryWriter = New BinaryWriter(decrypterStream)
         binaryWriter.Write(encryptedMessage, nonSecretPayloadLength + iv.Length, encryptedMessage.Length - nonSecretPayloadLength - iv.Length - sentTag.Length)
        End Using
       End Using

       Return plainTextStream.ToArray()
      End Using
     End Using
    End Using
   End Using
  End Function

  Function SimpleEncryptWithPassword(ByVal secretMessage As Byte(), ByVal password As String, ByVal Optional iter As Integer = 10000, ByVal Optional nonSecretPayload As Byte() = Nothing) As Byte()
   nonSecretPayload = If(nonSecretPayload, New Byte() {})

   If String.IsNullOrWhiteSpace(password) OrElse password.Length < MinPasswordLength Then
    Throw New ArgumentException(String.Format("Must have a password of at least {0} characters!", MinPasswordLength), "password")
   End If

   If secretMessage Is Nothing OrElse secretMessage.Length = 0 Then
    Throw New ArgumentException("Secret Message Required!", "secretMessage")
   End If

   Dim payload = New Byte(((SaltBitSize / 8) * 2) + nonSecretPayload.Length - 1 + 1 - 1) {}
   Array.Copy(nonSecretPayload, payload, nonSecretPayload.Length)
   Dim payloadIndex As Integer = nonSecretPayload.Length
   Dim cryptKey As Byte()
   Dim authKey As Byte()

   Using generator = New Rfc2898DeriveBytes(password, SaltBitSize / 8, Iterations)
    Dim salt = generator.Salt
    cryptKey = generator.GetBytes(KeyBitSize / 8)
    Array.Copy(salt, 0, payload, payloadIndex, salt.Length)
    payloadIndex += salt.Length
   End Using

   Using generator = New Rfc2898DeriveBytes(password, SaltBitSize / 8, Iterations)
    Dim salt = generator.Salt
    authKey = generator.GetBytes(KeyBitSize / 8)
    Array.Copy(salt, 0, payload, payloadIndex, salt.Length)
   End Using

   Return SimpleEncrypt(secretMessage, cryptKey, authKey, payload)
  End Function

  Function Getsalt(ByVal message As String) As String
   If String.IsNullOrWhiteSpace(message) Then
    Throw New ArgumentException("Encrypted Message Required!", "message")
   End If

   Dim cText = Convert.FromBase64String(message)
   Dim Salt = New Byte(SaltBitSize / 8 - 1 + 1 - 1) {}
   Array.Copy(cText, 0, Salt, 0, Salt.Length)
   Return Convert.ToBase64String(Salt, 0, Salt.Length)
  End Function

  Function SimpleDecryptWithPassword(ByVal encryptedMessage As Byte(), ByVal password As String, ByVal Optional iter As Integer = 10000, ByVal Optional nonSecretPayloadLength As Integer = 0) As Byte()
   If String.IsNullOrWhiteSpace(password) OrElse password.Length < MinPasswordLength Then
    Throw New ArgumentException(String.Format("Must have a password of at least {0} characters!", MinPasswordLength), "password")
   End If

   If encryptedMessage Is Nothing OrElse encryptedMessage.Length = 0 Then
    Throw New ArgumentException("Encrypted Message Required!", "encryptedMessage")
   End If

   Dim cryptSalt = New Byte(SaltBitSize / 8 - 1 + 1 - 1) {}
   Dim authSalt = New Byte(SaltBitSize / 8 - 1 + 1 - 1) {}
   Array.Copy(encryptedMessage, nonSecretPayloadLength, cryptSalt, 0, cryptSalt.Length)
   Array.Copy(encryptedMessage, nonSecretPayloadLength + cryptSalt.Length, authSalt, 0, authSalt.Length)
   Dim cryptKey As Byte()
   Dim authKey As Byte()

   Using generator = New Rfc2898DeriveBytes(password, cryptSalt, Iterations)
    cryptKey = generator.GetBytes(KeyBitSize / 8)
   End Using

   Using generator = New Rfc2898DeriveBytes(password, authSalt, Iterations)
    authKey = generator.GetBytes(KeyBitSize / 8)
   End Using

   Return SimpleDecrypt(encryptedMessage, cryptKey, authKey, cryptSalt.Length + authSalt.Length + nonSecretPayloadLength)
  End Function
 End Module
End Namespace

