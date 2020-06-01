using System;
using System.Security.Cryptography;
using System.Text;

namespace Medical_Profile
{
  namespace Enc
  {
    internal class Rsa
    {
      public static Tuple<string, string> GetKeys(int Length = 2048)
      {
        string K1 = null;
        string K2 = null;

        try
        {
          using (RSACng rsa1 = new RSACng(Length))
          {
            K1 = rsa1.ToXmlString(false);
            K2 = rsa1.ToXmlString(true);

            return (new Tuple<string, string>(K1, K2));
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine("Exception generating a new key pair! More info:");
          Console.WriteLine(ex.Message);
        }

        return (new Tuple<string, string>(null, null));
      }

      public static string Encrypt(string plainText, string Key, int Keybits = 2048)
      {
        byte[] plainBytes = null;
        byte[] encryptedBytes = null;

        try
        {
          using (RSACng rsa = new RSACng(Keybits))
          {
            rsa.FromXmlString(Key);
            plainBytes = Encoding.Unicode.GetBytes(plainText);
            encryptedBytes = rsa.Encrypt(plainBytes, RSAEncryptionPadding.Pkcs1);
            return Convert.ToBase64String(encryptedBytes);
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine("Exception encrypting file! More info:");
          Console.WriteLine(ex.Message);
        }
        finally
        {
        }

        return null;
      }

      public static string Decrypt(string B64text, string Key, int Keybits = 2048)
      {
        string plainText = "";
        byte[] encryptedBytes = null;
        byte[] plainBytes = null;

        try
        {
          using (RSACng rsa = new RSACng(Keybits))
          {
            rsa.FromXmlString(Key);
            encryptedBytes = Convert.FromBase64String(B64text);
            plainBytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1);
            plainText = Encoding.Unicode.GetString(plainBytes);
          }
          return plainText;
        }
        catch (Exception ex)
        {
          Console.WriteLine("Exception decrypting file! More info:");
          Console.WriteLine(ex.Message);
        }
        finally
        {
        }

        return null;
      }
    }
  }
}
