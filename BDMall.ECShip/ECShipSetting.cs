using BDMall.ECShip.Model;
using BDMall.Utility;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace BDMall.ECShip
{
    public class ECShipSetting
    {
        public static string NS_WSSE = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
        public static string NS_WSU = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";
        public static string PASSWORDTYPE = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordDigest";
        public static string NONCETYPE = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary";
        public static string SOAPENV = "http://schemas.xmlsoap.org/soap/envelope/";
        public static string WEB = "http://webservice.integrator.hkpost.com";
        public static string OBJ = "http://object.integrator.hkpost.com";
        public static string WS = "http://ws.emte.hkpost.com";
        //public static string GetIntegratorUserName()
        //{
        //    return "techptxhk";
        //}

        //public static string GetECShipName()
        //{
        //    return "ttechptxhk";
        //}
        //private static string GetECShipPassword()
        //{
        //    return "17924f27-5009-47a7-92b4-434b056ce50b";
        //}

        /// <summary>
        /// 创建调用ECShipAPI
        /// </summary>
        /// <returns></returns>
        public static XmlNode GenPostingXMLHeader(ECShipAccountInfo account, XmlDocument xmlDoc)
        {
            var created = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ssZ");
            var nonce = StringUtil.GenerateRandomNum(6);
            var encoded_nonce = Convert.ToBase64String(ECShipSetting.pack("H*", nonce));
            var password = "";
            var ecshipPassword = account.Password;

            var r = pack("H*", nonce).Concat(pack("a*", created)).Concat(pack("a*", ecshipPassword)).ToArray();
            r = Sha1(r);

            password = Convert.ToBase64String(r);

            //XmlDocument xmldoc = new XmlDocument();
            var root = xmlDoc.CreateElement("root");

            var header = xmlDoc.CreateNode(XmlNodeType.Element, "soapenv:Header", SOAPENV);
            root.AppendChild(header);

            var securityElement = xmlDoc.CreateElement("wsse:Security", NS_WSSE);
            header.AppendChild(securityElement);

            var usernameTokenElement = xmlDoc.CreateElement("wsse:UsernameToken", NS_WSSE);
            usernameTokenElement.SetAttribute("xmlns:wsu", NS_WSU);
            usernameTokenElement.SetAttribute("wsu:Id", "UsernameToken-1");
            securityElement.AppendChild(usernameTokenElement);

            var usernameNode = xmlDoc.CreateNode(XmlNodeType.Element, "wsse:Username", NS_WSSE);
            var username = xmlDoc.CreateTextNode(account.IntegratorUserName);
            usernameNode.AppendChild(username);
            usernameTokenElement.AppendChild(usernameNode);

            var passwordNode = xmlDoc.CreateNode(XmlNodeType.Element, "wsse:Password", NS_WSSE);
            var pw = xmlDoc.CreateTextNode(password);
            var pwType = xmlDoc.CreateAttribute("Type");
            pwType.Value = PASSWORDTYPE;
            passwordNode.Attributes.Append(pwType);
            passwordNode.AppendChild(pw);
            usernameTokenElement.AppendChild(passwordNode);

            var nonceNode = xmlDoc.CreateNode(XmlNodeType.Element, "wsse:Nonce", NS_WSSE);
            var non = xmlDoc.CreateTextNode(encoded_nonce);
            var nonType = xmlDoc.CreateAttribute("EncodingType");
            nonType.Value = NONCETYPE;
            nonceNode.Attributes.Append(nonType);
            nonceNode.AppendChild(non);
            usernameTokenElement.AppendChild(nonceNode);

            var createdNode = xmlDoc.CreateNode(XmlNodeType.Element, "wsu:Created", NS_WSU);
            var cre = xmlDoc.CreateTextNode(created);
            createdNode.AppendChild(cre);
            usernameTokenElement.AppendChild(createdNode);

            return root.FirstChild;
        }

        public static byte[] pack(string fmt, string s)
        {
            switch (fmt)
            {
                case "a*":
                    return s.Select(x => (byte)x).ToArray();
                case "H*":
                    if (s.Length % 2 == 1) s += "0";
                    byte[] byteArray = new byte[s.Length / 2];
                    for (int i = 0, k = 0; i < s.Length; i += 2, k++)
                    {
                        byteArray[k] = Convert.ToByte(s.Substring(i, 2), 16);
                    }
                    return byteArray;
            }
            return s.Select(x => Convert.ToByte(x)).ToArray();
        }
        public static byte[] Sha1(byte[] str)
        {
            //建立SHA1对象 using System.Security.Cryptography;
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            return sha1.ComputeHash(str);
        }

        /// <summary>
        /// 调用ECShip的API
        /// </summary>
        /// <param name="uri">webservice路径</param>
        /// <param name="xml">调用webService所需的内容</param>
        /// <returns></returns>
        public static string SendECShipBySOAP(string uri, string xml)
        {

            try
            {
                //request
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(RemoteCertificateValidate);
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(RemoteCertificateValidate);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                Uri url = new Uri(uri);
                WebRequest webRequest = WebRequest.Create(uri);
                webRequest.Headers["SOAPAction"] = string.Empty;
                webRequest.ContentType = "text/xml; charset=utf-8";
                webRequest.Method = "POST";
                using (Stream requestStream = webRequest.GetRequestStream())
                {
                    byte[] paramBytes = Encoding.UTF8.GetBytes(xml);
                    requestStream.Write(paramBytes, 0, paramBytes.Length);
                }
                //response
                WebResponse webResponse = webRequest.GetResponse();
                using (StreamReader myStreamReader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8))
                {
                    string result = "";
                    return result = myStreamReader.ReadToEnd();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// 调用ECShip的API
        /// </summary>
        /// <param name="uri">webservice路径</param>
        /// <param name="xml">调用webService所需的内容</param>
        /// <returns></returns>
        public static string SendECShipBySOAPTest(string uri, string xml)
        {

            try
            {
                //request
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(RemoteCertificateValidate);
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(RemoteCertificateValidate);
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                Uri url = new Uri(uri);
                WebRequest webRequest = WebRequest.Create(uri);
                webRequest.Headers["SOAPAction"] = string.Empty;
                webRequest.ContentType = "text/xml; charset=utf-8";
                webRequest.Method = "POST";
                using (Stream requestStream = webRequest.GetRequestStream())
                {
                    byte[] paramBytes = Encoding.UTF8.GetBytes(xml);
                    requestStream.Write(paramBytes, 0, paramBytes.Length);
                }
                //response
                WebResponse webResponse = webRequest.GetResponse();
                using (StreamReader myStreamReader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8))
                {
                    string result = "";
                    return result = myStreamReader.ReadToEnd();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        

        private static bool RemoteCertificateValidate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            //if (sslPolicyErrors == SslPolicyErrors.None)
            //    return true;
            // 总是接受  
            return true;
        }
    }
}
