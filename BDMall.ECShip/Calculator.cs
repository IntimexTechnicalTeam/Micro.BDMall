using BDMall.ECShip.Model;
using BDMall.ECShip.Model.Calculator;
using System.Xml;

namespace BDMall.ECShip
{
    public class Calculator
    {
        private static string GetECShipCalculatorAPI()
        {

            string api = "https://service.hongkongpost.hk/API-trial/services/Calculator?wsdl";
            //string api = "https://ec-ship-uat.hongkongpost.hk/APIUAT/services/Calculator?wsdl";

            return api;
        }

        /// <summary>
        /// 获取运费
        /// </summary>
        /// <param name="countryCode"></param>
        /// <param name="shipCode"></param>
        /// <param name="weight"></param>
        /// <param name="mailType"></param>
        /// <returns></returns>
        public static TotalPostageReturnInfo GetTotalPostage(ECShipAccountInfo account, PostageInfo postage)
        {
            try
            {

                var url = account.Url + "/" + "Calculator?wsdl";
                var xml = GenTotalPostageXML(account, postage);
                var resultXML = ECShipSetting.SendECShipBySOAP(url, xml);

                TotalPostageReturnInfo result = new TotalPostageReturnInfo();

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(resultXML);

                result.AdditionalCharge = decimal.Parse(xmlDoc.GetElementsByTagName("additionalCharge")[0].FirstChild?.Value ?? "0");
                result.ErrMessage = xmlDoc.GetElementsByTagName("errMessage")[0].FirstChild.Value;
                result.InsurancePremiumFee = decimal.Parse(xmlDoc.GetElementsByTagName("insurancePremiumFee")[0].FirstChild?.Value ?? "0");
                result.Status = xmlDoc.GetElementsByTagName("status")[0].FirstChild.Value;
                result.TotalPostage = decimal.Parse(xmlDoc.GetElementsByTagName("totalPostage")[0].FirstChild?.Value ?? "0");


                return result;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        /// <summary>
        /// 生成调用GetPostage接口的XML
        /// </summary>
        /// <returns></returns>
        private static string GenTotalPostageXML(ECShipAccountInfo account, PostageInfo postage)
        {
            XmlDocument xmlDoc = new XmlDocument();

            var envelopeElement = xmlDoc.CreateElement("soapenv:Envelope", ECShipSetting.SOAPENV);
            envelopeElement.SetAttribute("xmlns:web", ECShipSetting.WEB);
            envelopeElement.SetAttribute("xmlns:obj", ECShipSetting.OBJ);


            envelopeElement.AppendChild(ECShipSetting.GenPostingXMLHeader(account, xmlDoc));

            var bodyElement = xmlDoc.CreateNode(XmlNodeType.Element, "soapenv:Body", ECShipSetting.SOAPENV);

            var actionElement = xmlDoc.CreateNode(XmlNodeType.Element, "web:getTotalPostage", ECShipSetting.WEB);
            bodyElement.AppendChild(actionElement);

            var reqElement = xmlDoc.CreateNode(XmlNodeType.Element, "web:api01Req", ECShipSetting.WEB);
            actionElement.AppendChild(reqElement);

            var ecshipNameNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:hkpId", ECShipSetting.OBJ);
            var ecshipUsername = xmlDoc.CreateTextNode(account.ECShipName);
            ecshipNameNode.AppendChild(ecshipUsername);
            reqElement.AppendChild(ecshipNameNode);

            var integratorNameNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:integratorUsername", ECShipSetting.OBJ);
            var integratorname = xmlDoc.CreateTextNode(account.IntegratorUserName);
            integratorNameNode.AppendChild(integratorname);
            reqElement.AppendChild(integratorNameNode);

            var countryNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:countryCode", ECShipSetting.OBJ);
            var country = xmlDoc.CreateTextNode(postage.CountryCode);
            countryNode.AppendChild(country);
            reqElement.AppendChild(countryNode);


            //var mailTypeNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:insuranceAmount", ECShipSetting.OBJ);
            //reqElement.AppendChild(mailTypeNode);
            //var recAddressNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:insuranceTypeCode", ECShipSetting.OBJ);
            //reqElement.AppendChild(recAddressNode);

            var mailTypeNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:mailType", ECShipSetting.OBJ);
            var mialType = xmlDoc.CreateTextNode(postage.MailType);
            mailTypeNode.AppendChild(mialType);
            reqElement.AppendChild(mailTypeNode);

            var shipCodeNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:shipCode", ECShipSetting.OBJ);
            var sc = xmlDoc.CreateTextNode(postage.ShipCode);
            shipCodeNode.AppendChild(sc);
            reqElement.AppendChild(shipCodeNode);


            var weightNoNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:weight", ECShipSetting.OBJ);
            var w = xmlDoc.CreateTextNode(postage.Weight.ToString());
            weightNoNode.AppendChild(w);
            reqElement.AppendChild(weightNoNode);


            envelopeElement.AppendChild(bodyElement);

            return envelopeElement.OuterXml;

        }
    }
}
