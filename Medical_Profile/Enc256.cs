using global::System;
using global::System.Collections.Generic;
using global::System.IO;
using global::System.Security.Cryptography;
using global::System.Text;
using global::JWT;
using global::JWT.Algorithms;
using global::JWT.Serializers;
using Microsoft.VisualBasic.CompilerServices;

namespace Medical_Profile.Enc
{
 internal static class Enc256
 {
  private readonly static RandomNumberGenerator Random = RandomNumberGenerator.Create();
  public readonly static int BlockBitSize = 128;
  public readonly static int KeyBitSize = 256;
  public readonly static int SaltBitSize = 64;
  public readonly static int Iterations = 10000;
  public readonly static int MinPasswordLength = 12;

  public static string Encode(string secret, string salt, Dictionary<string, object> payload, int iter)
  {
   IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
   IJsonSerializer serializer = new JsonNetSerializer();
   IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
   IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
   string token = encoder.Encode(payload, NewKey(secret, salt, iter));
   return token;
  }

  public static string Encode(string secret, string Salt, Dictionary<string, object> payload)
  {
   IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
   IJsonSerializer serializer = new JsonNetSerializer();
   IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
   IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
   var saltb = Convert.FromBase64String(Salt);
   int iter = Conversions.ToInteger(Math.Abs(Convert.ToInt64((BitConverter.ToUInt64(saltb, 0) + (decimal)10000) % 10000)));
   string token = encoder.Encode(payload, NewKey(secret, Salt, iter));
   token = Salt + "." + token;
   return token;
  }

  public static IDictionary<string, object> Decode(string token, string secret, string salt, int iter)
  {
   IDictionary<string, object> rdict = new Dictionary<string, object>();
   try
   {
    IJsonSerializer serializer = new JsonNetSerializer();
    IDateTimeProvider provider = new UtcDateTimeProvider();
    IJwtValidator validator = new JwtValidator(serializer, provider);
    IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
    IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
    var dict = decoder.DecodeToObject<IDictionary<string, object>>(token, NewKey(secret, salt, iter), verify: true);
    return dict;
   }
   catch (TokenExpiredException ex)
   {
    rdict["Error"] = "Token has expired";
    return rdict;
   }
   catch (SignatureVerificationException ex)
   {
    rdict["Error"] = "Token has invalid signature";
    return rdict;
   }
  }

  public static IDictionary<string, object> Decode(string token, string secret)
  {
   int i = token.IndexOf(".");
   string salt = token.Substring(0, i);
   string Stoken = token.Substring(i + 1);
   var saltb = Convert.FromBase64String(salt);
   int iter = Conversions.ToInteger(Math.Abs(Convert.ToInt64((BitConverter.ToUInt64(saltb, 0) + (decimal)10000) % 10000)));
   return Decode(Stoken, secret, salt, iter);
  }

  public static string NewKey(string password, string salt, int Iterations = 10000)
  {
   var saltb = Convert.FromBase64String(salt);
   var key = new byte[(int)(KeyBitSize / (double)8 - 1) + 1];
   using (var generator = new Rfc2898DeriveBytes(password, saltb, Iterations))
   {
    key = generator.GetBytes((int)(KeyBitSize / (double)8));
   }

   return Convert.ToBase64String(key, 0, key.Length);
  }

  public static int Next_open(byte[] tg, int st)
  {
   int p = st + 1;
   if (p >= tg.Length)
    p = 0;
   while (true)
   {
    if (tg[p] == 0xFF)
    {
     break;
    }

    p += 1;
    if (p == tg.Length)
    {
     p = 0;
    }
   }

   return p;
  }

  public static string Iterscramble(string inp, int iter = 1)
  {
   string pstring = inp;
   for (int i = 0, loopTo = iter; i <= loopTo; i++)
    pstring = Scramble(pstring);
   return pstring;
  }

  public static string Scramble(string inp)
  {
   var b1 = new byte[inp.Length];
   var b2 = new byte[inp.Length];
   var car = inp.ToCharArray();
   for (int i = 0, loopTo = b1.Length - 1; i <= loopTo; i++)
    b1[i] = Conversions.ToByte(i);
   for (int j = 0, loopTo1 = b2.Length - 1; j <= loopTo1; j++)
    b2[j] = 0xFF;
   for (int i = 0, loopTo2 = b2.Length - 1; i <= loopTo2; i++)
    b2[Next_open(b2, i + i)] = Convert.ToByte(car[i]);
   return Encoding.Default.GetString(b2);
  }

  public static string SimpleEncrypt(string secretMessage, byte[] cryptKey, byte[] authKey, byte[] nonSecretPayload = null)
  {
   if (string.IsNullOrEmpty(secretMessage))
   {
    throw new ArgumentException("Secret Message Required!", "secretMessage");
   }

   var plainText = Encoding.UTF8.GetBytes(secretMessage);
   var cipherText = SimpleEncrypt(plainText, cryptKey, authKey, nonSecretPayload);
   return Convert.ToBase64String(cipherText);
  }

  public static string SimpleDecrypt(string encryptedMessage, byte[] cryptKey, byte[] authKey, int nonSecretPayloadLength = 0)
  {
   if (string.IsNullOrWhiteSpace(encryptedMessage))
   {
    throw new ArgumentException("Encrypted Message Required!", "encryptedMessage");
   }

   var cipherText = Convert.FromBase64String(encryptedMessage);
   var plainText = SimpleDecrypt(cipherText, cryptKey, authKey, nonSecretPayloadLength);
   return plainText is null ? null : Encoding.UTF8.GetString(plainText);
  }

  public static string Encrypt(string secretmessage, string password, string Label)
  {
   int v1 = 0;
   int l1 = Convert.ToInt32(Label.Substring(3));
   for (int i = 3, loopTo = Label.Length - 1; i <= loopTo; i++)
    v1 += Convert.ToInt32(Label.Substring(i, 1));
   int insert_loc = v1 % (Label.Length - 4);
   string k1 = password.Substring(0, insert_loc) + Label + password.Substring(insert_loc);
   return Encrypt(secretmessage, k1, l1);
  }

  public static string Encrypt(string secretMessage, string password, int iter = 10000, byte[] nonSecretPayload = null)
  {
   if (string.IsNullOrEmpty(secretMessage))
   {
    throw new ArgumentException("Secret Message Required!", "secretMessage");
   }

   var plainText = Encoding.UTF8.GetBytes(secretMessage);
   var cipherText = SimpleEncryptWithPassword(plainText, password, iter, nonSecretPayload);
   return Convert.ToBase64String(cipherText);
  }

  public static string SimpleEncryptWithPassword(string secretMessage, string password, int iter, byte[] nonSecretPayload = null)
  {
   if (string.IsNullOrEmpty(secretMessage))
   {
    throw new ArgumentException("Secret Message Required!", "secretMessage");
   }

   var plainText = Encoding.UTF8.GetBytes(secretMessage);
   var cipherText = SimpleEncryptWithPassword(plainText, password, iter, nonSecretPayload);
   return Convert.ToBase64String(cipherText);
  }

  public static string Decrypt(string secretmessage, string password, string Label)
  {
   int v1 = 0;
   int l1 = Convert.ToInt32(Label.Substring(3));
   for (int i = 3, loopTo = Label.Length - 1; i <= loopTo; i++)
    v1 += Convert.ToInt32(Label.Substring(i, 1));
   int insert_loc = v1 % (Label.Length - 4);
   string k1 = password.Substring(0, insert_loc) + Label + password.Substring(insert_loc);
   return Decrypt(secretmessage, k1, l1);
  }

  public static string Decrypt(string encryptedMessage, string password, int iter = 10000, int nonSecretPayloadLength = 0)
  {
   if (string.IsNullOrWhiteSpace(encryptedMessage))
   {
    throw new ArgumentException("Encrypted Message Required!", "encryptedMessage");
   }

   var cipherText = Convert.FromBase64String(encryptedMessage);
   var plainText = SimpleDecryptWithPassword(cipherText, password, iter, nonSecretPayloadLength);
   return plainText is null ? null : Encoding.UTF8.GetString(plainText);
  }

  public static string SimpleDecryptWithPassword(string encryptedMessage, string password, int iter = 10000, int nonSecretPayloadLength = 0)
  {
   if (string.IsNullOrWhiteSpace(encryptedMessage))
   {
    throw new ArgumentException("Encrypted Message Required!", "encryptedMessage");
   }

   var cipherText = Convert.FromBase64String(encryptedMessage);
   var plainText = SimpleDecryptWithPassword(cipherText, password, nonSecretPayloadLength);
   return plainText is null ? null : Encoding.UTF8.GetString(plainText);
  }

  public static byte[] SimpleEncrypt(byte[] secretMessage, byte[] cryptKey, byte[] authKey, byte[] nonSecretPayload = null)
  {
   if (cryptKey is null || cryptKey.Length != KeyBitSize / Conversions.ToDouble(8))
   {
    throw new ArgumentException(string.Format("Key needs to be {0} bit!", KeyBitSize), "cryptKey");
   }

   if (authKey is null || authKey.Length != KeyBitSize / Conversions.ToDouble(8))
   {
    throw new ArgumentException(string.Format("Key needs to be {0} bit!", KeyBitSize), "authKey");
   }

   if (secretMessage is null || secretMessage.Length < 1)
   {
    throw new ArgumentException("Secret Message Required!", "secretMessage");
   }

   nonSecretPayload = nonSecretPayload ?? new byte[] { };
   byte[] cipherText;
   byte[] iv;
   using (var aes = new AesManaged()
   {
    KeySize = KeyBitSize,
    BlockSize = BlockBitSize,
    Mode = CipherMode.CBC,
    Padding = PaddingMode.PKCS7
   })
   {
    aes.GenerateIV();
    iv = aes.IV;
    using (var encrypter = aes.CreateEncryptor(cryptKey, iv))
    {
     using (var cipherStream = new MemoryStream())
     {
      using (var cryptoStream = new CryptoStream(cipherStream, encrypter, CryptoStreamMode.Write))
      {
       using (var binaryWriter = new BinaryWriter(cryptoStream))
       {
        binaryWriter.Write(secretMessage);
       }
      }

      cipherText = cipherStream.ToArray();
     }
    }
   }

   using (var hmac = new HMACSHA256(authKey))
   {
    using (var encryptedStream = new MemoryStream())
    {
     using (var binaryWriter = new BinaryWriter(encryptedStream))
     {
      binaryWriter.Write(nonSecretPayload);
      binaryWriter.Write(iv);
      binaryWriter.Write(cipherText);
      binaryWriter.Flush();
      var tag = hmac.ComputeHash(encryptedStream.ToArray());
      binaryWriter.Write(tag);
     }

     return encryptedStream.ToArray();
    }
   }
  }

  public static byte[] SimpleDecrypt(byte[] encryptedMessage, byte[] cryptKey, byte[] authKey, int nonSecretPayloadLength = 0)
  {
   if (cryptKey is null || cryptKey.Length != KeyBitSize / Conversions.ToDouble(8))
   {
    throw new ArgumentException(string.Format("CryptKey needs to be {0} bit!", KeyBitSize), "cryptKey");
   }

   if (authKey is null || authKey.Length != KeyBitSize / Conversions.ToDouble(8))
   {
    throw new ArgumentException(string.Format("AuthKey needs to be {0} bit!", KeyBitSize), "authKey");
   }

   if (encryptedMessage is null || encryptedMessage.Length == 0)
   {
    throw new ArgumentException("Encrypted Message Required!", "encryptedMessage");
   }

   using (var hmac = new HMACSHA256(authKey))
   {
    var sentTag = new byte[(int)(hmac.HashSize / (double)8 - 1 + 1 - 1) + 1];
    var calcTag = hmac.ComputeHash(encryptedMessage, 0, encryptedMessage.Length - sentTag.Length);
    double ivLength = BlockBitSize / (double)8;
    if (encryptedMessage.Length < sentTag.Length + nonSecretPayloadLength + ivLength)
    {
     return null;
    }

    Array.Copy(encryptedMessage, encryptedMessage.Length - sentTag.Length, sentTag, 0, sentTag.Length);
    int compare = 0;
    int loopTo = sentTag.Length - 1;
    for (int i = 0, loopTo1 = loopTo; i <= loopTo1; i++)
     compare = (compare | sentTag[i]) ^ calcTag[i];
    if (compare != 0)
    {
     return null;
    }

    using (var aes = new AesManaged()
    {
     KeySize = KeyBitSize,
     BlockSize = BlockBitSize,
     Mode = CipherMode.CBC,
     Padding = PaddingMode.PKCS7
    })
    {
     var iv = new byte[(int)(ivLength - 1 + 1 - 1) + 1];
     Array.Copy(encryptedMessage, nonSecretPayloadLength, iv, 0, iv.Length);
     using (var decrypter = aes.CreateDecryptor(cryptKey, iv))
     {
      using (var plainTextStream = new MemoryStream())
      {
       using (var decrypterStream = new CryptoStream(plainTextStream, decrypter, CryptoStreamMode.Write))
       {
        using (var binaryWriter = new BinaryWriter(decrypterStream))
        {
         binaryWriter.Write(encryptedMessage, nonSecretPayloadLength + iv.Length, encryptedMessage.Length - nonSecretPayloadLength - iv.Length - sentTag.Length);
        }
       }

       return plainTextStream.ToArray();
      }
     }
    }
   }
  }

  public static byte[] SimpleEncryptWithPassword(byte[] secretMessage, string password, int iter = 10000, byte[] nonSecretPayload = null)
  {
   nonSecretPayload = nonSecretPayload ?? new byte[] { };
   if (string.IsNullOrWhiteSpace(password) || password.Length < MinPasswordLength)
   {
    throw new ArgumentException(string.Format("Must have a password of at least {0} characters!", MinPasswordLength), "password");
   }

   if (secretMessage is null || secretMessage.Length == 0)
   {
    throw new ArgumentException("Secret Message Required!", "secretMessage");
   }

   var payload = new byte[(int)(SaltBitSize / (double)8 * 2 + nonSecretPayload.Length - 1 + 1 - 1) + 1];
   Array.Copy(nonSecretPayload, payload, nonSecretPayload.Length);
   int payloadIndex = nonSecretPayload.Length;
   byte[] cryptKey;
   byte[] authKey;
   using (var generator = new Rfc2898DeriveBytes(password, (int)(SaltBitSize / (double)8), Iterations))
   {
    var salt = generator.Salt;
    cryptKey = generator.GetBytes((int)(KeyBitSize / (double)8));
    Array.Copy(salt, 0, payload, payloadIndex, salt.Length);
    payloadIndex += salt.Length;
   }

   using (var generator = new Rfc2898DeriveBytes(password, (int)(SaltBitSize / (double)8), Iterations))
   {
    var salt = generator.Salt;
    authKey = generator.GetBytes((int)(KeyBitSize / (double)8));
    Array.Copy(salt, 0, payload, payloadIndex, salt.Length);
   }

   return SimpleEncrypt(secretMessage, cryptKey, authKey, payload);
  }

  public static string Getsalt(string message)
  {
   if (string.IsNullOrWhiteSpace(message))
   {
    throw new ArgumentException("Encrypted Message Required!", "message");
   }

   var cText = Convert.FromBase64String(message);
   var Salt = new byte[(int)(SaltBitSize / (double)8 - 1 + 1 - 1) + 1];
   Array.Copy(cText, 0, Salt, 0, Salt.Length);
   return Convert.ToBase64String(Salt, 0, Salt.Length);
  }

  public static byte[] SimpleDecryptWithPassword(byte[] encryptedMessage, string password, int iter = 10000, int nonSecretPayloadLength = 0)
  {
   if (string.IsNullOrWhiteSpace(password) || password.Length < MinPasswordLength)
   {
    throw new ArgumentException(string.Format("Must have a password of at least {0} characters!", MinPasswordLength), "password");
   }

   if (encryptedMessage is null || encryptedMessage.Length == 0)
   {
    throw new ArgumentException("Encrypted Message Required!", "encryptedMessage");
   }

   var cryptSalt = new byte[(int)(SaltBitSize / (double)8 - 1 + 1 - 1) + 1];
   var authSalt = new byte[(int)(SaltBitSize / (double)8 - 1 + 1 - 1) + 1];
   Array.Copy(encryptedMessage, nonSecretPayloadLength, cryptSalt, 0, cryptSalt.Length);
   Array.Copy(encryptedMessage, nonSecretPayloadLength + cryptSalt.Length, authSalt, 0, authSalt.Length);
   byte[] cryptKey;
   byte[] authKey;
   using (var generator = new Rfc2898DeriveBytes(password, cryptSalt, Iterations))
   {
    cryptKey = generator.GetBytes((int)(KeyBitSize / (double)8));
   }

   using (var generator = new Rfc2898DeriveBytes(password, authSalt, Iterations))
   {
    authKey = generator.GetBytes((int)(KeyBitSize / (double)8));
   }

   return SimpleDecrypt(encryptedMessage, cryptKey, authKey, cryptSalt.Length + authSalt.Length + nonSecretPayloadLength);
  }
 }
}