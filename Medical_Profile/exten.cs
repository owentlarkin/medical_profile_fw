using global::System;
using global::System.Security.Cryptography;
using global::System.Xml;

namespace Medical_Profile.Enc_Extensions
{
 internal static class RSAKeyExtensions
 {
  public static void ImpString(this RSA rsa, string xmlString)
  {
   var parameters = new RSAParameters();
   var xmlDoc = new XmlDocument();
   xmlDoc.LoadXml(xmlString);
   if (xmlDoc.DocumentElement.Name.Equals("RSAKeyValue"))
   {
    foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
    {
     var switchExpr = node.Name;
     switch (switchExpr)
     {
      case "Modulus":
       {
        parameters.Modulus = string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText);
        break;
       }

      case "Exponent":
       {
        parameters.Exponent = string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText);
        break;
       }

      case "P":
       {
        parameters.P = string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText);
        break;
       }

      case "Q":
       {
        parameters.Q = string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText);
        break;
       }

      case "DP":
       {
        parameters.DP = string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText);
        break;
       }

      case "DQ":
       {
        parameters.DQ = string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText);
        break;
       }

      case "InverseQ":
       {
        parameters.InverseQ = string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText);
        break;
       }

      case "D":
       {
        parameters.D = string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText);
        break;
       }
     }
    }
   }
   else
   {
    throw new Exception("Invalid XML RSA key.");
   }

   rsa.ImportParameters(parameters);
  }

  public static string ExpString(this RSA rsa, bool includePrivateParameters)
  {
   var parameters = rsa.ExportParameters(includePrivateParameters);
   return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>", parameters.Modulus is object ? Convert.ToBase64String(parameters.Modulus) : null, parameters.Exponent is object ? Convert.ToBase64String(parameters.Exponent) : null, parameters.P is object ? Convert.ToBase64String(parameters.P) : null, parameters.Q is object ? Convert.ToBase64String(parameters.Q) : null, parameters.DP is object ? Convert.ToBase64String(parameters.DP) : null, parameters.DQ is object ? Convert.ToBase64String(parameters.DQ) : null, parameters.InverseQ is object ? Convert.ToBase64String(parameters.InverseQ) : null, parameters.D is object ? Convert.ToBase64String(parameters.D) : null);
  }
 }
}