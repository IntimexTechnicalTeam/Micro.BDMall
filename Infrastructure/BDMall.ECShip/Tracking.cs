using BDMall.ECShip;
using BDMall.ECShip.Model;
using BDMall.ECShip.Model.Posting;
using BDMall.ECShip.Model.SoazPosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WS.ECShip.Model.SoazPosting;
using WS.ECShip.Model.Tracking;

namespace WS.ECShip
{
    public static class Tracking
    {
        /// <summary>
        /// 创建ecship订单
        /// </summary>
        /// <param name="order"></param>
        /// <param name="deliveryInfo"></param>
        public static OrderTrackingInfo GetTrackingInfo(ECShipAccountInfo account, string trackingNo, string lang)
        {
            try
            {
                //IOrderBLL orderBLL = BLLFactory.Create(currentWebStore).CreateOrderBLL();
                string url = account.Url + "/" + "Tracking?wsdl";
                var xml = GenTrackingInfoXML(account, trackingNo, lang);
                
                var resultXML = ECShipSetting.SendECShipBySOAP(url, xml);

                OrderTrackingInfo result = new OrderTrackingInfo();

                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.LoadXml(resultXML);


                result.Message = xmlDoc.GetElementsByTagName("errMessage")[0].FirstChild?.Value ?? "";

                result.Status = xmlDoc.GetElementsByTagName("status")[0].FirstChild.Value;

                result.TrackingNo = xmlDoc.GetElementsByTagName("itemNo")[0].FirstChild?.Value;
                List<TrackingInfo> trackingInfos = new List<TrackingInfo>();
                foreach (XmlNode item in xmlDoc.GetElementsByTagName("trackingInfos")[0].ChildNodes)
                {
                    TrackingInfo trackingInfo = new TrackingInfo();

                    trackingInfo.TrackingNo = item.ChildNodes[0]?.FirstChild.Value;
                    trackingInfo.TrackingDate = item.ChildNodes[1]?.FirstChild.Value;
                    trackingInfo.TrackingDetail = item.ChildNodes[2]?.FirstChild.Value;

                    trackingInfos.Add(trackingInfo);
                }

                result.TrackingInfos = trackingInfos;

                return result;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string GenTrackingInfoXML(ECShipAccountInfo account, string trackingNo, string lang)
        {
            XmlDocument xmlDoc = new XmlDocument();

            var envelopeElement = xmlDoc.CreateElement("soapenv:Envelope", ECShipSetting.SOAPENV);
            envelopeElement.SetAttribute("xmlns:web", ECShipSetting.WEB);
            envelopeElement.SetAttribute("xmlns:obj", ECShipSetting.OBJ);


            envelopeElement.AppendChild(ECShipSetting.GenPostingXMLHeader(account, xmlDoc));

            var bodyElement = xmlDoc.CreateNode(XmlNodeType.Element, "soapenv:Body", ECShipSetting.SOAPENV);

            var actionElement = xmlDoc.CreateNode(XmlNodeType.Element, "web:getTTInfo", ECShipSetting.WEB);
            bodyElement.AppendChild(actionElement);

            var reqElement = xmlDoc.CreateNode(XmlNodeType.Element, "web:api05Req", ECShipSetting.WEB);
            actionElement.AppendChild(reqElement);

            var ecshipNameNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:ecshipUsername", ECShipSetting.OBJ);
            var ecshipName = xmlDoc.CreateTextNode(account.ECShipName);
            ecshipNameNode.AppendChild(ecshipName);
            reqElement.AppendChild(ecshipNameNode);

            var integratorNameNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:integratorUsername", ECShipSetting.OBJ);
            var integratorName = xmlDoc.CreateTextNode(account.IntegratorUserName);
            integratorNameNode.AppendChild(integratorName);
            reqElement.AppendChild(integratorNameNode);

            var itemNoNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:itemNo", ECShipSetting.OBJ);
            var itemNo = xmlDoc.CreateTextNode(trackingNo);
            itemNoNode.AppendChild(itemNo);
            reqElement.AppendChild(itemNoNode);

            var languageNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:language", ECShipSetting.OBJ);
            var language = xmlDoc.CreateTextNode(lang);
            languageNode.AppendChild(language);
            reqElement.AppendChild(languageNode);




            envelopeElement.AppendChild(bodyElement);

            return envelopeElement.OuterXml;
        }
    }
}
