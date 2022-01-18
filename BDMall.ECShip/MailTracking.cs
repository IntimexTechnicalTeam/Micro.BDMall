using BDMall.ECShip;
using BDMall.ECShip.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WS.ECShip.Model;
using WS.ECShip.Model.MailTracking;
using BDMall.Utility;
using BDMall.Runtime;

namespace WS.ECShip
{
    public static class MailTracking
    {        /// <summary>
             /// 创建ecship订单
             /// </summary>
             /// <param name="order"></param>
             /// <param name="deliveryInfo"></param>
        public static MailTrackingInfo GetMailTrackingInfo(MailTrackingAccountInfo account, string trackingNo, string lang)
        {
            try
            {
                string langPostfix = lang.ToLower();
                string url = account.Url + "/" + "EmteWebService?wsdl";
                var xml = GenMailTrackingInfoXML(account, trackingNo, lang);

                var resultXML = ECShipSetting.SendECShipBySOAP(url, xml);

                //                string resultXML = @"<?xml version='1.0' encoding='utf-8'?>

                //<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/'>
                //  <soapenv:Header/>
                //  <soapenv:Body>
                //    <ns:getItemResultResponse xmlns:ns='http://ws.emte.hkpost.com'>
                //      <ns:return xmlns:ax23='http://pojo.emte.hkpost.com/xsd' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xsi:type='ax23:ItemResult'>
                //        <ax23:item_array xsi:type='ax23:Item'>
                //            <ax23:dest_cnty_code>HK</ax23:dest_cnty_code>
                //            <ax23:detail_flag>N</ax23:detail_flag>
                //            <ax23:hkp_item_no>AY998015165HK</ax23:hkp_item_no>
                //            <ax23:item_no>AY998015165HK</ax23:item_no>
                //            <ax23:iolt_type>L</ax23:iolt_type>
                //            <ax23:item_status_code>1</ax23:item_status_code>

                //          <ax23:dest_cnty_c xsi:nil='true'/>
                //          <ax23:dest_cnty_e>HONG KONG</ax23:dest_cnty_e>
                //          <ax23:dest_cnty_s xsi:nil='true'/>

                //          <ax23:drop_off_c xsi:nil='true'/>
                //          <ax23:drop_off_e xsi:nil='true'/>
                //          <ax23:drop_off_s xsi:nil='true'/>          

                //          <ax23:item_type_c xsi:nil='true'/>
                //          <ax23:item_type_e>Smart Post</ax23:item_type_e>
                //          <ax23:item_type_s xsi:nil='true'/>


                //          <ax23:message_array xsi:type='ax23:Message'>
                //            <ax23:event_code>IT_RNR</ax23:event_code>
                //            <ax23:extra_message_code></ax23:extra_message_code>
                //            <ax23:location_cnty_code>HK</ax23:location_cnty_code>
                //            <ax23:message_code>R001</ax23:message_code>

                //            <ax23:extra_message_c xsi:nil='true'/>
                //            <ax23:extra_message_e></ax23:extra_message_e>
                //            <ax23:extra_message_s xsi:nil='true'/>

                //            <ax23:message_c>货物已到</ax23:message_c>
                //            <ax23:message_e>Item being processed</ax23:message_e>
                //            <ax23:message_s xsi:nil='true'/>

                //            <ax23:message_location_c xsi:nil='true'/>
                //            <ax23:message_location_e>HONG KONG</ax23:message_location_e>
                //            <ax23:message_location_s xsi:nil='true'/>

                //            <ax23:message_time>12-04-2018 10:44</ax23:message_time>
                //          </ax23:message_array>
                //            <ax23:message_array xsi:type='ax23:Message'>
                //            <ax23:event_code>IT_RNR</ax23:event_code>
                //            <ax23:extra_message_code></ax23:extra_message_code>
                //            <ax23:location_cnty_code>HK</ax23:location_cnty_code>
                //            <ax23:message_code>R001</ax23:message_code>

                //            <ax23:extra_message_c xsi:nil='true'/>
                //            <ax23:extra_message_e></ax23:extra_message_e>
                //            <ax23:extra_message_s xsi:nil='true'/>

                //            <ax23:message_c>货物派送中</ax23:message_c>
                //            <ax23:message_e>Item being processed</ax23:message_e>
                //            <ax23:message_s xsi:nil='true'/>

                //            <ax23:message_location_c xsi:nil='true'/>
                //            <ax23:message_location_e>HONG KONG</ax23:message_location_e>
                //            <ax23:message_location_s xsi:nil='true'/>

                //            <ax23:message_time>13-04-2018 10:44</ax23:message_time>
                //          </ax23:message_array>

                //            <ax23:milestone_code>4</ax23:milestone_code>
                //            <ax23:org_cnty_code>HK</ax23:org_cnty_code>
                //            <ax23:posting_date>13-04-2018</ax23:posting_date>
                //            <ax23:posting_time>10:44</ax23:posting_time>
                //            <ax23:service_code>LDS03</ax23:service_code>

                //            <ax23:milestone_c>testttt</ax23:milestone_c>
                //            <ax23:milestone_e>Delivered</ax23:milestone_e>
                //            <ax23:milestone_s xsi:nil='true'/>

                //            <ax23:org_cnty_c xsi:nil='true'/>
                //            <ax23:org_cnty_e>HONG KONG</ax23:org_cnty_e>
                //            <ax23:org_cnty_s xsi:nil='true'/>


                //        </ax23:item_array>
                //        <ax23:item_count>1</ax23:item_count>
                //        <ax23:status>000</ax23:status>
                //      </ns:return>
                //    </ns:getItemResultResponse>
                //  </soapenv:Body>
                //</soapenv:Envelope>";

                MailTrackingInfo info = new MailTrackingInfo();

                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.LoadXml(resultXML);


                info.ItemCount = int.Parse(xmlDoc.GetElementsByTagName("ax23:item_count")[0]?.FirstChild?.Value ?? "0");

                info.Status = xmlDoc.GetElementsByTagName("ax23:status")[0]?.FirstChild?.Value ?? "";



                info.DestCountryCode = xmlDoc.GetElementsByTagName("ax23:dest_cnty_code")[0]?.FirstChild?.Value ?? "";

                info.DetailFlag = xmlDoc.GetElementsByTagName("ax23:detail_flag")[0]?.FirstChild?.Value ?? "";

                info.HKPItemNo = xmlDoc.GetElementsByTagName("ax23:hkp_item_no")[0]?.FirstChild?.Value ?? "";
                info.ItemNo = xmlDoc.GetElementsByTagName("ax23:item_no")[0]?.FirstChild?.Value ?? "";

                info.IoltType = xmlDoc.GetElementsByTagName("ax23:iolt_type")[0]?.FirstChild?.Value ?? "";
                info.ItemStatusCode = xmlDoc.GetElementsByTagName("ax23:item_status_code")[0]?.FirstChild?.Value ?? "";

                info.DesctCountryName = xmlDoc.GetElementsByTagName("ax23:dest_cnty_" + langPostfix)[0]?.FirstChild?.Value ?? "";
                info.DropOff = xmlDoc.GetElementsByTagName("ax23:drop_off_" + langPostfix)[0]?.FirstChild?.Value ?? "";
                info.ItemType = xmlDoc.GetElementsByTagName("ax23:item_type_" + langPostfix)[0]?.FirstChild?.Value ?? "";

                info.MilestoneCode = xmlDoc.GetElementsByTagName("ax23:milestone_code")[0]?.FirstChild?.Value ?? "";
                info.OriginalCountryCode = xmlDoc.GetElementsByTagName("ax23:org_cnty_code")[0]?.FirstChild?.Value ?? "";
                try
                {
                    info.PostingDate = string.IsNullOrEmpty(xmlDoc.GetElementsByTagName("ax23:posting_date")[0]?.FirstChild?.Value) ? "" : DateUtil.DateTimeToString(DateUtil.ConvertoDateTime(xmlDoc.GetElementsByTagName("ax23:posting_date")[0]?.FirstChild?.Value ?? "", "dd-MM-yyyy"), Setting.ShortDateFormat);
                }
                catch (Exception ex)
                {
                    info.PostingDate = "";
                }

                info.PostingTime = xmlDoc.GetElementsByTagName("ax23:posting_time")[0]?.FirstChild?.Value ?? "";
                info.ServiceCode = xmlDoc.GetElementsByTagName("ax23:service_code")[0]?.FirstChild?.Value ?? "";

                info.MilestoneDesc = xmlDoc.GetElementsByTagName("ax23:milestone_" + langPostfix)[0]?.FirstChild?.Value ?? "";
                info.OriginalCountry = xmlDoc.GetElementsByTagName("ax23:org_cnty_" + langPostfix)[0]?.FirstChild?.Value ?? "";



                var extraMessageIndex = 0;
                var messageIndex = 0;
                var locationIndex = 0;

                switch (lang)
                {
                    case "C":
                        extraMessageIndex = 1;
                        messageIndex = 6;
                        locationIndex = 9;
                        break;
                    case "E":
                        extraMessageIndex = 3;
                        messageIndex = 8;
                        locationIndex = 10;
                        break;
                    case "S":
                        extraMessageIndex = 4;
                        messageIndex = 12;
                        locationIndex = 11;
                        break;
                }
                List<MailTrackingDetail> Details = new List<MailTrackingDetail>();
                int seq = 0;
                foreach (XmlNode item in xmlDoc.GetElementsByTagName("ax23:message_array"))
                {
                    var node = item.ChildNodes;
                    MailTrackingDetail detail = new MailTrackingDetail();

                    detail.EventCode = node[0]?.FirstChild?.Value ?? "";
                    detail.ExtraMessageCode = node[2]?.FirstChild?.Value ?? "";
                    detail.LocationCountryCode = node[5]?.FirstChild?.Value ?? "";
                    detail.MessageCode = node[7]?.FirstChild?.Value ?? "";
                    detail.ExtraMessage = node[extraMessageIndex]?.FirstChild?.Value ?? "";
                    detail.Message = node[messageIndex]?.FirstChild?.Value ?? "";
                    detail.MessageLocation = node[locationIndex]?.FirstChild?.Value ?? "";

                    detail.MessageTime = string.IsNullOrEmpty(node[13]?.FirstChild?.Value) ? "" : DateUtil.DateTimeToString(DateUtil.ConvertoDateTime(node[13]?.FirstChild?.Value ?? "", "dd-MM-yyyy HH:mm"), BDMall.Runtime.Setting.DefaultDateTimeFormat2);
                    detail.Seq = seq;
                    Details.Add(detail);
                    seq++;
                }
                info.Details = Details;

                return info;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetMailTrackingInfoForTest(MailTrackingAccountInfo account, string trackingNo, string lang)
        {
            try
            {
                string langPostfix = lang.ToLower();
                string url = account.Url + "/" + "EmteWebService?wsdl";
                var xml = GenMailTrackingInfoXML(account, trackingNo, lang);

                var resultXML = ECShipSetting.SendECShipBySOAP(url, xml);



                MailTrackingInfo info = new MailTrackingInfo();

                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.LoadXml(resultXML);


                //info.ItemCount = int.Parse(xmlDoc.GetElementsByTagName("ax23:item_count")[0]?.FirstChild?.Value ?? "0");

                //info.Status = xmlDoc.GetElementsByTagName("ax23:status")[0]?.FirstChild?.Value ?? "";



                //info.DestCountryCode = xmlDoc.GetElementsByTagName("ax23:dest_cnty_code")[0]?.FirstChild?.Value ?? "";

                //info.DetailFlag = xmlDoc.GetElementsByTagName("ax23:detail_flag")[0]?.FirstChild?.Value ?? "";

                //info.HKPItemNo = xmlDoc.GetElementsByTagName("ax23:hkp_item_no")[0]?.FirstChild?.Value ?? "";
                //info.ItemNo = xmlDoc.GetElementsByTagName("ax23:item_no")[0]?.FirstChild?.Value ?? "";

                //info.IoltType = xmlDoc.GetElementsByTagName("ax23:iolt_type")[0]?.FirstChild?.Value ?? "";
                //info.ItemStatusCode = xmlDoc.GetElementsByTagName("ax23:item_status_code")[0]?.FirstChild?.Value ?? "";

                //info.DesctCountryName = xmlDoc.GetElementsByTagName("ax23:dest_cnty_" + langPostfix)[0]?.FirstChild?.Value ?? "";
                //info.DropOff = xmlDoc.GetElementsByTagName("ax23:drop_off_" + langPostfix)[0]?.FirstChild?.Value ?? "";
                //info.ItemType = xmlDoc.GetElementsByTagName("ax23:item_type_" + langPostfix)[0]?.FirstChild?.Value ?? "";

                //info.MilestoneCode = xmlDoc.GetElementsByTagName("ax23:milestone_code")[0]?.FirstChild?.Value ?? "";
                //info.OriginalCountryCode = xmlDoc.GetElementsByTagName("ax23:org_cnty_code")[0]?.FirstChild?.Value ?? "";
                //info.PostingDate = string.IsNullOrEmpty(xmlDoc.GetElementsByTagName("ax23:posting_date")[0]?.FirstChild?.Value) ? "" : DateUtil.DateTimeToString(DateUtil.ConvertoDateTime(xmlDoc.GetElementsByTagName("ax23:posting_date")[0]?.FirstChild?.Value ?? "", "dd-MM-yyyy"), Runtime.Setting.ShortDateFormat);
                //info.PostingTime = xmlDoc.GetElementsByTagName("ax23:posting_time")[0]?.FirstChild?.Value ?? "";
                //info.ServiceCode = xmlDoc.GetElementsByTagName("ax23:service_code")[0]?.FirstChild?.Value ?? "";

                //info.MilestoneDesc = xmlDoc.GetElementsByTagName("ax23:milestone_" + langPostfix)[0]?.FirstChild?.Value ?? "";
                //info.OriginalCountry = xmlDoc.GetElementsByTagName("ax23:org_cnty_" + langPostfix)[0]?.FirstChild?.Value ?? "";



                //var extraMessageIndex = 0;
                //var messageIndex = 0;
                //var locationIndex = 0;

                //switch (lang)
                //{
                //    case "C":
                //        extraMessageIndex = 4;
                //        messageIndex = 7;
                //        locationIndex = 10;
                //        break;
                //    case "E":
                //        extraMessageIndex = 5;
                //        messageIndex = 8;
                //        locationIndex = 11;
                //        break;
                //    case "S":
                //        extraMessageIndex = 6;
                //        messageIndex = 9;
                //        locationIndex = 12;
                //        break;
                //}
                //List<MailTrackingDetail> Details = new List<MailTrackingDetail>();
                //foreach (XmlNode item in xmlDoc.GetElementsByTagName("ax23:message_array"))
                //{
                //    var node = item.ChildNodes;
                //    MailTrackingDetail detail = new MailTrackingDetail();

                //    detail.EventCode = node[0]?.FirstChild?.Value ?? "";
                //    detail.ExtraMessageCode = node[1]?.FirstChild?.Value ?? "";
                //    detail.LocationCountryCode = node[2]?.FirstChild?.Value ?? "";
                //    detail.MessageCode = node[3]?.FirstChild?.Value ?? "";
                //    detail.ExtraMessage = node[extraMessageIndex]?.FirstChild?.Value ?? "";
                //    detail.Message = node[messageIndex]?.FirstChild?.Value ?? "";
                //    detail.MessageLocation = node[locationIndex]?.FirstChild?.Value ?? "";
                //    detail.MessageTime = string.IsNullOrEmpty(node[13]?.FirstChild?.Value) ? "" : DateUtil.DateTimeToString(DateUtil.ConvertoDateTime(node[13]?.FirstChild?.Value ?? "", "dd-MM-yyyy HH:mm"), Runtime.Setting.DefaultDateTimeFormat2);
                //    Details.Add(detail);
                //}
                //info.Details = Details;

                return xmlDoc.OuterXml;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string GenMailTrackingInfoXML(MailTrackingAccountInfo account, string trackingNo, string lang)
        {
            XmlDocument xmlDoc = new XmlDocument();

            var envelopeElement = xmlDoc.CreateElement("soapenv:Envelope", ECShipSetting.SOAPENV);
            envelopeElement.SetAttribute("xmlns:ws", ECShipSetting.WS);


            //envelopeElement.AppendChild(ECShipSetting.GenPostingXMLHeader(account, xmlDoc));

            var headerElement = xmlDoc.CreateNode(XmlNodeType.Element, "soapenv:Header", ECShipSetting.SOAPENV);

            var bodyElement = xmlDoc.CreateNode(XmlNodeType.Element, "soapenv:Body", ECShipSetting.SOAPENV);

            var actionElement = xmlDoc.CreateNode(XmlNodeType.Element, "ws:getItemResult", ECShipSetting.WS);
            bodyElement.AppendChild(actionElement);


            var trackingNoNode = xmlDoc.CreateNode(XmlNodeType.Element, "ws:item_no_array", ECShipSetting.WS);
            var trackNo = xmlDoc.CreateTextNode(trackingNo);
            trackingNoNode.AppendChild(trackNo);
            actionElement.AppendChild(trackingNoNode);

            var idNode = xmlDoc.CreateNode(XmlNodeType.Element, "ws:ac_id", ECShipSetting.WS);
            var id = xmlDoc.CreateTextNode(account.ACId);
            idNode.AppendChild(id);
            actionElement.AppendChild(idNode);

            var passwordNode = xmlDoc.CreateNode(XmlNodeType.Element, "ws:ac_pw", ECShipSetting.WS);
            var password = xmlDoc.CreateTextNode(account.ACPassword);
            passwordNode.AppendChild(password);
            actionElement.AppendChild(passwordNode);

            var codeNode = xmlDoc.CreateNode(XmlNodeType.Element, "ws:sys_code", ECShipSetting.WS);
            var code = xmlDoc.CreateTextNode(account.SystemCode);
            codeNode.AppendChild(code);
            actionElement.AppendChild(codeNode);

            var languageNode = xmlDoc.CreateNode(XmlNodeType.Element, "ws:lang", ECShipSetting.WS);
            var language = xmlDoc.CreateTextNode(lang);
            languageNode.AppendChild(language);
            actionElement.AppendChild(languageNode);



            envelopeElement.AppendChild(headerElement);

            envelopeElement.AppendChild(bodyElement);


            return envelopeElement.OuterXml;
        }
    }
}
