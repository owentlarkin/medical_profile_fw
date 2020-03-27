Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Security.Cryptography
Imports System.Xml
Imports System.Runtime.CompilerServices

Namespace Enc_Extensions
    Friend Module RSAKeyExtensions
    <Extension()>
    Sub ImpString(ByVal rsa As RSA, ByVal xmlString As String)
      Dim parameters As RSAParameters = New RSAParameters()
      Dim xmlDoc As XmlDocument = New XmlDocument()
      xmlDoc.LoadXml(xmlString)

      If xmlDoc.DocumentElement.Name.Equals("RSAKeyValue") Then

        For Each node As XmlNode In xmlDoc.DocumentElement.ChildNodes

          Select Case node.Name
            Case "Modulus"
              parameters.Modulus = (If(String.IsNullOrEmpty(node.InnerText), Nothing, Convert.FromBase64String(node.InnerText)))
            Case "Exponent"
              parameters.Exponent = (If(String.IsNullOrEmpty(node.InnerText), Nothing, Convert.FromBase64String(node.InnerText)))
            Case "P"
              parameters.P = (If(String.IsNullOrEmpty(node.InnerText), Nothing, Convert.FromBase64String(node.InnerText)))
            Case "Q"
              parameters.Q = (If(String.IsNullOrEmpty(node.InnerText), Nothing, Convert.FromBase64String(node.InnerText)))
            Case "DP"
              parameters.DP = (If(String.IsNullOrEmpty(node.InnerText), Nothing, Convert.FromBase64String(node.InnerText)))
            Case "DQ"
              parameters.DQ = (If(String.IsNullOrEmpty(node.InnerText), Nothing, Convert.FromBase64String(node.InnerText)))
            Case "InverseQ"
              parameters.InverseQ = (If(String.IsNullOrEmpty(node.InnerText), Nothing, Convert.FromBase64String(node.InnerText)))
            Case "D"
              parameters.D = (If(String.IsNullOrEmpty(node.InnerText), Nothing, Convert.FromBase64String(node.InnerText)))
          End Select
        Next
      Else
        Throw New Exception("Invalid XML RSA key.")
      End If

      rsa.ImportParameters(parameters)
    End Sub

    <Extension()>
        Function ExpString(ByVal rsa As RSA, ByVal includePrivateParameters As Boolean) As String
            Dim parameters As RSAParameters = rsa.ExportParameters(includePrivateParameters)
            Return String.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>", If(parameters.Modulus IsNot Nothing, Convert.ToBase64String(parameters.Modulus), Nothing), If(parameters.Exponent IsNot Nothing, Convert.ToBase64String(parameters.Exponent), Nothing), If(parameters.P IsNot Nothing, Convert.ToBase64String(parameters.P), Nothing), If(parameters.Q IsNot Nothing, Convert.ToBase64String(parameters.Q), Nothing), If(parameters.DP IsNot Nothing, Convert.ToBase64String(parameters.DP), Nothing), If(parameters.DQ IsNot Nothing, Convert.ToBase64String(parameters.DQ), Nothing), If(parameters.InverseQ IsNot Nothing, Convert.ToBase64String(parameters.InverseQ), Nothing), If(parameters.D IsNot Nothing, Convert.ToBase64String(parameters.D), Nothing))
        End Function
    End Module
End Namespace
