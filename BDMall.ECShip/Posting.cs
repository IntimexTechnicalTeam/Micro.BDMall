using BDMall.ECShip.Model;
using BDMall.ECShip.Model.Posting;
using Intimex.Utility;
using System;
using System.Xml;
using WS.ECShip.Model.Posting;
using BDMall.Model.OrderMNG.View.Back;

namespace BDMall.ECShip
{
    public static class Posting
    {
        private static string GetECShipPostingAPI()
        {
            string api = "https://www.ec-ship.hk/API-trial/services/Posting?wsdl";
            return api;
        }



        /// <summary>
        /// 创建ecship订单
        /// </summary>
        /// <param name="order"></param>
        /// <param name="deliveryInfo"></param>
        public static CreateOrderReturnInfo CreateECShipOrder(ECShipAccountInfo account, ECShipOrderInfo order, ECShipDeliveryInfo deliveryInfo)
        {
            try
            {
                //IOrderBLL orderBLL = BLLFactory.Create(currentWebStore).CreateOrderBLL();
                string url = account.Url + "/" + "Posting?wsdl";
                var xml = GenCreateOrderXML(account, order, deliveryInfo);

                var resultXML = ECShipSetting.SendECShipBySOAP(url, xml);

                CreateOrderReturnInfo result = new CreateOrderReturnInfo();

                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.LoadXml(resultXML);

                result.AdditionalDocument = xmlDoc.GetElementsByTagName("additionalDocument")[0].FirstChild?.Value ?? "";
                result.ErrMessage = xmlDoc.GetElementsByTagName("errMessage")[0].FirstChild.Value;
                result.DeliveryCharge = decimal.Parse(xmlDoc.GetElementsByTagName("deliveryCharge")[0].FirstChild?.Value ?? "0");
                result.Status = xmlDoc.GetElementsByTagName("status")[0].FirstChild.Value;
                result.InsurPermFee = decimal.Parse(xmlDoc.GetElementsByTagName("insurPermFee")[0].FirstChild?.Value ?? "0");
                result.ExpressNo = xmlDoc.GetElementsByTagName("itemNo")[0].FirstChild?.Value;
                result.ECShipNo = xmlDoc.GetElementsByTagName("orderNo")[0].FirstChild?.Value;

                return result;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static MCNReturnInfo GetMCNInfo(ECShipAccountInfo account, string mcnCode)
        {
            try
            {
                MCNReturnInfo result = new MCNReturnInfo();
                string url = account.Url + "/" + "Posting?wsdl";

                var xml = GenMCNInfoXML(account, mcnCode);
                var resultXML = ECShipSetting.SendECShipBySOAP(url, xml);

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(resultXML);

                result.CCCode = xmlDoc.GetElementsByTagName("ccCode")[0].FirstChild?.Value ?? "";
                result.Message = xmlDoc.GetElementsByTagName("errMessage")[0].FirstChild.Value;
                result.PLCode = xmlDoc.GetElementsByTagName("plCode")[0].FirstChild?.Value ?? "";
                result.Status = xmlDoc.GetElementsByTagName("status")[0].FirstChild.Value;

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 获取ECShip Label的文件流
        /// </summary>
        /// <param name="expressNo"></param>
        /// <param name="printMode"></param>
        /// <returns></returns>
        public static GetAddressPackReturnInfo GetAddressPack(ECShipAccountInfo account, string trackingNo, string deliveryService)
        {
            try
            {
                string url = "";
                var printMode = "";
                var xml = "";
                var fileTagName = "";
                if (deliveryService == "EMS" || deliveryService == "SPGDE" || deliveryService == "SPGDEE")
                {
                    printMode = "A";
                    fileTagName = "COP";
                    url = account.Url + "/" + "SoazPosting?wsdl";
                    xml = GenGetSoazAddressPackXML(account, trackingNo, printMode);
                }
                else
                {
                    printMode = "0";
                    fileTagName = "ap";
                    url = account.Url + "/" + "Posting?wsdl";
                    xml = GenGetAddressPackXML(account, trackingNo, printMode);
                }



                var resultXML = ECShipSetting.SendECShipBySOAP(url, xml);

                GetAddressPackReturnInfo result = new GetAddressPackReturnInfo();

                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.LoadXml(resultXML);

                result.FileString = xmlDoc.GetElementsByTagName(fileTagName)[0].FirstChild?.Value ?? "";
                result.ErrMessage = xmlDoc.GetElementsByTagName("errMessage")[0].FirstChild?.Value ?? "";
                result.Status = xmlDoc.GetElementsByTagName("status")[0].FirstChild.Value;


                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// 生成获取MCN信息的XML
        /// </summary>
        /// <param name="account"></param>
        /// <param name="mcnCode"></param>
        /// <returns></returns>
        private static string GenMCNInfoXML(ECShipAccountInfo account, string mcnCode)
        {
            XmlDocument xmlDoc = new XmlDocument();

            var envelopeElement = xmlDoc.CreateElement("soapenv:Envelope", ECShipSetting.SOAPENV);
            envelopeElement.SetAttribute("xmlns:web", ECShipSetting.WEB);
            envelopeElement.SetAttribute("xmlns:obj", ECShipSetting.OBJ);


            envelopeElement.AppendChild(ECShipSetting.GenPostingXMLHeader(account, xmlDoc));

            var bodyElement = xmlDoc.CreateNode(XmlNodeType.Element, "soapenv:Body", ECShipSetting.SOAPENV);

            var actionElement = xmlDoc.CreateNode(XmlNodeType.Element, "web:getMcnInfo", ECShipSetting.WEB);
            bodyElement.AppendChild(actionElement);

            var reqElement = xmlDoc.CreateNode(XmlNodeType.Element, "web:api27Req", ECShipSetting.WEB);
            actionElement.AppendChild(reqElement);

            var ecshipNameNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:ecshipUsername", ECShipSetting.OBJ);
            var ecshipUsername = xmlDoc.CreateTextNode(account.ECShipName);
            ecshipNameNode.AppendChild(ecshipUsername);
            reqElement.AppendChild(ecshipNameNode);

            var integratorNameNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:integratorUsername", ECShipSetting.OBJ);
            var integratorname = xmlDoc.CreateTextNode(account.IntegratorUserName);
            integratorNameNode.AppendChild(integratorname);
            reqElement.AppendChild(integratorNameNode);


            var mcnNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:mcn", ECShipSetting.OBJ);
            var mcn = xmlDoc.CreateTextNode(mcnCode);
            mcnNode.AppendChild(mcn);
            reqElement.AppendChild(mcnNode);

            envelopeElement.AppendChild(bodyElement);

            return envelopeElement.OuterXml;

        }

        /// <summary>
        /// 生成获取ECShip label的XML
        /// </summary>
        /// <param name="expressNo"></param>
        /// <param name="printModel"></param>
        /// <returns></returns>
        private static string GenGetAddressPackXML(ECShipAccountInfo account, string expressNo, string printModel)
        {
            XmlDocument xmlDoc = new XmlDocument();

            var envelopeElement = xmlDoc.CreateElement("soapenv:Envelope", ECShipSetting.SOAPENV);
            envelopeElement.SetAttribute("xmlns:web", ECShipSetting.WEB);
            envelopeElement.SetAttribute("xmlns:obj", ECShipSetting.OBJ);


            envelopeElement.AppendChild(ECShipSetting.GenPostingXMLHeader(account, xmlDoc));
            var bodyElement = xmlDoc.CreateNode(XmlNodeType.Element, "soapenv:Body", ECShipSetting.SOAPENV);

            var actionElement = xmlDoc.CreateNode(XmlNodeType.Element, "web:getAddressPack", ECShipSetting.WEB);
            bodyElement.AppendChild(actionElement);

            var reqElement = xmlDoc.CreateNode(XmlNodeType.Element, "web:api11Req", ECShipSetting.WEB);
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
            reqElement.AppendChild(itemNoNode);

            var itemNode = xmlDoc.CreateNode(XmlNodeType.Element, "web:item", ECShipSetting.WEB);
            var item = xmlDoc.CreateTextNode(expressNo);
            itemNode.AppendChild(item);
            itemNoNode.AppendChild(itemNode);

            var printModeNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:printMode", ECShipSetting.OBJ);
            var printMode = xmlDoc.CreateTextNode(printModel);
            printModeNode.AppendChild(printMode);
            reqElement.AppendChild(printModeNode);

            envelopeElement.AppendChild(bodyElement);

            return envelopeElement.OuterXml;

        }


        private static string GenGetSoazAddressPackXML(ECShipAccountInfo account, string expressNo, string printModel)
        {
            XmlDocument xmlDoc = new XmlDocument();

            var envelopeElement = xmlDoc.CreateElement("soapenv:Envelope", ECShipSetting.SOAPENV);
            envelopeElement.SetAttribute("xmlns:web", ECShipSetting.WEB);
            envelopeElement.SetAttribute("xmlns:obj", ECShipSetting.OBJ);


            envelopeElement.AppendChild(ECShipSetting.GenPostingXMLHeader(account, xmlDoc));
            var bodyElement = xmlDoc.CreateNode(XmlNodeType.Element, "soapenv:Body", ECShipSetting.SOAPENV);

            var actionElement = xmlDoc.CreateNode(XmlNodeType.Element, "web:getSoazAddressPack", ECShipSetting.WEB);
            bodyElement.AppendChild(actionElement);

            var reqElement = xmlDoc.CreateNode(XmlNodeType.Element, "web:api31Req", ECShipSetting.WEB);
            actionElement.AppendChild(reqElement);

            var ecshipNameNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:soazUsername", ECShipSetting.OBJ);
            var ecshipName = xmlDoc.CreateTextNode(account.ECShipName);
            ecshipNameNode.AppendChild(ecshipName);
            reqElement.AppendChild(ecshipNameNode);

            var integratorNameNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:integratorUsername", ECShipSetting.OBJ);
            var integratorName = xmlDoc.CreateTextNode(account.IntegratorUserName);
            integratorNameNode.AppendChild(integratorName);
            reqElement.AppendChild(integratorNameNode);

            var cn22NoOfCopyNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:cn22NoOfCopy", ECShipSetting.OBJ);
            var cn22NoOfCopy = xmlDoc.CreateTextNode("0");
            cn22NoOfCopyNode.AppendChild(cn22NoOfCopy);
            reqElement.AppendChild(cn22NoOfCopyNode);

            var cn23NoOfCopyNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:cn23NoOfCopy", ECShipSetting.OBJ);
            var cn23NoOfCopy = xmlDoc.CreateTextNode("0");
            cn23NoOfCopyNode.AppendChild(cn23NoOfCopy);
            reqElement.AppendChild(cn23NoOfCopyNode);

            var createDateNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:createDate", ECShipSetting.OBJ);
            var createDate = xmlDoc.CreateTextNode(DateTime.Now.ToString("yyyy-MM-dd"));
            createDateNode.AppendChild(createDate);
            reqElement.AppendChild(createDateNode);


            var itemNode = xmlDoc.CreateNode(XmlNodeType.Element, "web:itemNo", ECShipSetting.WEB);
            var item = xmlDoc.CreateTextNode(expressNo);
            itemNode.AppendChild(item);
            reqElement.AppendChild(itemNode);

            var printModeNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:printMode", ECShipSetting.OBJ);
            var printMode = xmlDoc.CreateTextNode(printModel);
            printModeNode.AppendChild(printMode);
            reqElement.AppendChild(printModeNode);

            var signatureNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:signature", ECShipSetting.OBJ);
            var signature = xmlDoc.CreateTextNode("");
            signatureNode.AppendChild(signature);
            reqElement.AppendChild(signatureNode);

            envelopeElement.AppendChild(bodyElement);

            return envelopeElement.OuterXml;

        }


        /// <summary>
        /// 生成调用CreateOrder接口的XML
        /// </summary>
        /// <returns></returns>
        private static string GenCreateOrderXML(ECShipAccountInfo account, ECShipOrderInfo order, ECShipDeliveryInfo deliveryInfo)
        {
            XmlDocument xmlDoc = new XmlDocument();

            var envelopeElement = xmlDoc.CreateElement("soapenv:Envelope", ECShipSetting.SOAPENV);
            envelopeElement.SetAttribute("xmlns:web", ECShipSetting.WEB);
            envelopeElement.SetAttribute("xmlns:obj", ECShipSetting.OBJ);


            envelopeElement.AppendChild(ECShipSetting.GenPostingXMLHeader(account, xmlDoc));

            var bodyElement = xmlDoc.CreateNode(XmlNodeType.Element, "soapenv:Body", ECShipSetting.SOAPENV);

            var actionElement = xmlDoc.CreateNode(XmlNodeType.Element, "web:createOrder", ECShipSetting.WEB);
            bodyElement.AppendChild(actionElement);

            var reqElement = xmlDoc.CreateNode(XmlNodeType.Element, "web:api02Req", ECShipSetting.WEB);
            actionElement.AppendChild(reqElement);

            var ecshipNameNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:hkpId", ECShipSetting.OBJ);
            var ecshipName = xmlDoc.CreateTextNode(account.ECShipName);
            ecshipNameNode.AppendChild(ecshipName);
            reqElement.AppendChild(ecshipNameNode);

            var integratorNameNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:integratorUsername", ECShipSetting.OBJ);
            var integratorName = xmlDoc.CreateTextNode(account.IntegratorUserName);
            integratorNameNode.AppendChild(integratorName);
            reqElement.AppendChild(integratorNameNode);

            var countryNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:countryCode", ECShipSetting.OBJ);
            var country = xmlDoc.CreateTextNode(deliveryInfo.CountryCode);
            countryNode.AppendChild(country);
            reqElement.AppendChild(countryNode);

            var mailTypeNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:mailType", ECShipSetting.OBJ);
            var mailType = xmlDoc.CreateTextNode(deliveryInfo.MailType.ToString());
            mailTypeNode.AppendChild(mailType);
            reqElement.AppendChild(mailTypeNode);

            var shipCodeNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:shipCode", ECShipSetting.OBJ);
            var shipCode = xmlDoc.CreateTextNode(deliveryInfo.ShipCode);
            shipCodeNode.AppendChild(shipCode);
            reqElement.AppendChild(shipCodeNode);


            var noticeMethodNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:noticeMethod", ECShipSetting.OBJ);
            var noticeMethod = xmlDoc.CreateTextNode(deliveryInfo.NoticeMethod);
            noticeMethodNode.AppendChild(noticeMethod);
            reqElement.AppendChild(noticeMethodNode);

            var smsLangNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:smsLang", ECShipSetting.OBJ);
            var smsLang = xmlDoc.CreateTextNode(deliveryInfo.SmsLang);
            smsLangNode.AppendChild(smsLang);
            reqElement.AppendChild(smsLangNode);

            var mcnNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:mcn", ECShipSetting.OBJ);
            var mcn = xmlDoc.CreateTextNode(deliveryInfo.MCN);
            mcnNode.AppendChild(mcn);
            reqElement.AppendChild(mcnNode);




            var ipostalStationNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:ipostalStation", ECShipSetting.OBJ);
            var ipostalStation = xmlDoc.CreateTextNode(deliveryInfo.IPostStation);
            ipostalStationNode.AppendChild(ipostalStation);
            reqElement.AppendChild(ipostalStationNode);

            var pickupOfficeNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:pickupOffice", ECShipSetting.OBJ);
            var pickupOffice = xmlDoc.CreateTextNode(deliveryInfo.PickupOffice);
            pickupOfficeNode.AppendChild(pickupOffice);
            reqElement.AppendChild(pickupOfficeNode);

            var paidAmtNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:paidAmt", ECShipSetting.OBJ);
            var paidAmt = xmlDoc.CreateTextNode(deliveryInfo.PaidAmt.ToString());
            paidAmtNode.AppendChild(paidAmt);
            reqElement.AppendChild(paidAmtNode);

            var merchandiserEmailNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:merchandiserEmail", ECShipSetting.OBJ);
            var merchandiserEmail = xmlDoc.CreateTextNode(deliveryInfo.MerchandiserEmail);
            merchandiserEmailNode.AppendChild(merchandiserEmail);
            reqElement.AppendChild(merchandiserEmailNode);


            var recipientContactNoNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:recipientContactNo", ECShipSetting.OBJ);
            var recipientContactNo = xmlDoc.CreateTextNode(order.RecipientPhone);
            recipientContactNoNode.AppendChild(recipientContactNo);
            reqElement.AppendChild(recipientContactNoNode);

            var recipientEmailNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:recipientEmail", ECShipSetting.OBJ);
            var recipientEmail = xmlDoc.CreateTextNode(order.RecipientEmail);
            recipientEmailNode.AppendChild(recipientEmail);
            reqElement.AppendChild(recipientEmailNode);

            var recAddressNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:recipientAddress", ECShipSetting.OBJ);
            var recAddress = xmlDoc.CreateTextNode(order.RecipientAddress);
            recAddressNode.AppendChild(recAddress);
            reqElement.AppendChild(recAddressNode);

            var recCityNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:recipientCity", ECShipSetting.OBJ);
            var recCity = xmlDoc.CreateTextNode(order.RecipientCity);
            recCityNode.AppendChild(recCity);
            reqElement.AppendChild(recCityNode);

            var recNameNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:recipientName", ECShipSetting.OBJ);
            var recName = xmlDoc.CreateTextNode(order.RecipientName);
            recNameNode.AppendChild(recName);
            reqElement.AppendChild(recNameNode);

            var recPostalNoNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:recipientPostalNo", ECShipSetting.OBJ);
            var recPostalNo = xmlDoc.CreateTextNode(order.RecipientPostalNo);
            recPostalNoNode.AppendChild(recPostalNo);
            reqElement.AppendChild(recPostalNoNode);


            var refNoNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:refNo", ECShipSetting.OBJ);
            var refNo = xmlDoc.CreateTextNode(order.RefNo);
            refNoNode.AppendChild(refNo);
            reqElement.AppendChild(refNoNode);


            var allAddlress = (order.SenderAddress + " " + order.SenderAddress2 + " " + order.SenderAddress3 + " " + order.SenderAddress4);
            if (allAddlress.Length > 140)
            {
                allAddlress = allAddlress.Substring(0, 140);
            }
            var senderAddressNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:senderAddress", ECShipSetting.OBJ);
            var senderAddress = xmlDoc.CreateTextNode(allAddlress);
            senderAddressNode.AppendChild(senderAddress);
            reqElement.AppendChild(senderAddressNode);


            var senderCountryNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:senderCountry", ECShipSetting.OBJ);
            var senderCountry = xmlDoc.CreateTextNode(order.SenderCountry);
            senderCountryNode.AppendChild(senderCountry);
            reqElement.AppendChild(senderCountryNode);

            var senderContactNoNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:senderContactNo", ECShipSetting.OBJ);
            var senderContactNo = xmlDoc.CreateTextNode(order.SenderContactNo);
            senderContactNoNode.AppendChild(senderContactNo);
            reqElement.AppendChild(senderContactNoNode);

            var senderEmailNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:senderEmail", ECShipSetting.OBJ);
            var senderEmail = xmlDoc.CreateTextNode(order.SenderEmail);
            senderEmailNode.AppendChild(senderEmail);
            reqElement.AppendChild(senderEmailNode);

            var senderFaxNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:senderFax", ECShipSetting.OBJ);
            var senderFax = xmlDoc.CreateTextNode("");
            senderFaxNode.AppendChild(senderFax);
            reqElement.AppendChild(senderFaxNode);

            var senderNameNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:senderName", ECShipSetting.OBJ);
            var senderName = xmlDoc.CreateTextNode(order.SenderName);
            senderNameNode.AppendChild(senderName);
            reqElement.AppendChild(senderNameNode);

            if(!string.IsNullOrEmpty(deliveryInfo.ItemCategory))
            {
                var itemCategoryNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:itemCategory", ECShipSetting.OBJ);
                var itemCategory = xmlDoc.CreateTextNode(deliveryInfo.ItemCategory);
                itemCategoryNode.AppendChild(itemCategory);
                reqElement.AppendChild(itemCategoryNode);
            }

            var productNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:products", ECShipSetting.OBJ);
            reqElement.AppendChild(productNode);

            foreach (var orderItem in order.OrderItems)
            {
                var productItemNode = xmlDoc.CreateNode(XmlNodeType.Element, "web:item", ECShipSetting.WEB);
                productNode.AppendChild(productItemNode);

                var contentDescNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:contentDesc", ECShipSetting.OBJ);

                var isChinese = StringUtil.IsContainChinese(orderItem.Desc);
                var address = "";
                if (!isChinese)
                {
                    address = orderItem.Desc.Length > 60 ? orderItem.Desc.Substring(0, 60) : orderItem.Desc;
                }
                else
                {
                    address = orderItem.Desc.Length > 20 ? orderItem.Desc.Substring(0, 20) : orderItem.Desc;
                }


                var contentDesc = xmlDoc.CreateTextNode(address);
                contentDescNode.AppendChild(contentDesc);
                productItemNode.AppendChild(contentDescNode);

                var currencyCodeNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:currencyCode", ECShipSetting.OBJ);
                var currencyCode = xmlDoc.CreateTextNode(orderItem.CurrencyCode);
                currencyCodeNode.AppendChild(currencyCode);
                productItemNode.AppendChild(currencyCodeNode);

                var productCountryNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:productCountry", ECShipSetting.OBJ);
                var productCountry = xmlDoc.CreateTextNode("");
                productCountryNode.AppendChild(productCountry);
                productItemNode.AppendChild(productCountryNode);

                var productQtyNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:productQty", ECShipSetting.OBJ);
                var productQty = xmlDoc.CreateTextNode(orderItem.Qty.ToString());
                productQtyNode.AppendChild(productQty);
                productItemNode.AppendChild(productQtyNode);

                var productTariffCodeNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:productTariffCode", ECShipSetting.OBJ);
                var productTariffCode = xmlDoc.CreateTextNode("");
                productTariffCodeNode.AppendChild(productTariffCode);
                productItemNode.AppendChild(productTariffCodeNode);

                var productValueNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:productValue", ECShipSetting.OBJ);
                var productValue = xmlDoc.CreateTextNode(orderItem.TotalPrice.ToString());
                productValueNode.AppendChild(productValue);
                productItemNode.AppendChild(productValueNode);

                var productWeightNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:productWeight", ECShipSetting.OBJ);
                var productWeight = xmlDoc.CreateTextNode(orderItem.Weight.ToString());
                productWeightNode.AppendChild(productWeight);
                productItemNode.AppendChild(productWeightNode);
            }

            envelopeElement.AppendChild(bodyElement);

            return envelopeElement.OuterXml;

        }

        ///

    }
}
