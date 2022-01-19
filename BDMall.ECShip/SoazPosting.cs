using BDMall.ECShip;
using BDMall.ECShip.Model;
using BDMall.ECShip.Model.Posting;
using BDMall.ECShip.Model.SoazPosting;
using Intimex.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WS.ECShip.Model.SoazPosting;

namespace WS.ECShip
{
    public static class SoazPosting
    {
        /// <summary>
        /// 创建ecship订单
        /// </summary>
        /// <param name="order"></param>
        /// <param name="deliveryInfo"></param>
        public static CreateOrderReturnInfo CreateSoazOrder(ECShipAccountInfo account, SoazOrderInfo order)
        {
            try
            {
                //IOrderBLL orderBLL = BLLFactory.Create(currentWebStore).CreateOrderBLL();
                string url = account.Url + "/" + "SoazPosting?wsdl";
                var xml = GenCreateSoazOrderXML(account, order);

                var resultXML = ECShipSetting.SendECShipBySOAP(url, xml);

                CreateOrderReturnInfo result = new CreateOrderReturnInfo();

                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.LoadXml(resultXML);


                result.ErrMessage = xmlDoc.GetElementsByTagName("errMessage")[0].FirstChild?.Value ?? "";

                result.Status = xmlDoc.GetElementsByTagName("status")[0].FirstChild.Value;

                result.ExpressNo = xmlDoc.GetElementsByTagName("itemNo")[0].FirstChild?.Value;


                return result;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string GenCreateSoazOrderXML(ECShipAccountInfo account, SoazOrderInfo order)
        {
            XmlDocument xmlDoc = new XmlDocument();

            var envelopeElement = xmlDoc.CreateElement("soapenv:Envelope", ECShipSetting.SOAPENV);
            envelopeElement.SetAttribute("xmlns:web", ECShipSetting.WEB);
            envelopeElement.SetAttribute("xmlns:obj", ECShipSetting.OBJ);


            envelopeElement.AppendChild(ECShipSetting.GenPostingXMLHeader(account, xmlDoc));

            var bodyElement = xmlDoc.CreateNode(XmlNodeType.Element, "soapenv:Body", ECShipSetting.SOAPENV);

            var actionElement = xmlDoc.CreateNode(XmlNodeType.Element, "web:createSoazOrder", ECShipSetting.WEB);
            bodyElement.AppendChild(actionElement);

            var reqElement = xmlDoc.CreateNode(XmlNodeType.Element, "web:api30Req", ECShipSetting.WEB);
            actionElement.AppendChild(reqElement);

            var ecshipNameNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:soazUsername", ECShipSetting.OBJ);
            var ecshipName = xmlDoc.CreateTextNode(account.ECShipName);
            ecshipNameNode.AppendChild(ecshipName);
            reqElement.AppendChild(ecshipNameNode);

            var integratorNameNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:integratorUsername", ECShipSetting.OBJ);
            var integratorName = xmlDoc.CreateTextNode(account.IntegratorUserName);
            integratorNameNode.AppendChild(integratorName);
            reqElement.AppendChild(integratorNameNode);

            var addresseeAddr1Node = xmlDoc.CreateNode(XmlNodeType.Element, "obj:addresseeAddr1", ECShipSetting.OBJ);
            var addresseeAddr1 = xmlDoc.CreateTextNode(order.RecipientAddress1);
            addresseeAddr1Node.AppendChild(addresseeAddr1);
            reqElement.AppendChild(addresseeAddr1Node);

            var addresseeAddr2Node = xmlDoc.CreateNode(XmlNodeType.Element, "obj:addresseeAddr2", ECShipSetting.OBJ);
            var addresseeAddr2 = xmlDoc.CreateTextNode(order.RecipientAddress2);
            addresseeAddr2Node.AppendChild(addresseeAddr2);
            reqElement.AppendChild(addresseeAddr2Node);

            var addresseeAddr3Node = xmlDoc.CreateNode(XmlNodeType.Element, "obj:addresseeAddr3", ECShipSetting.OBJ);
            var addresseeAddr3 = xmlDoc.CreateTextNode(order.RecipientAddress3);
            addresseeAddr3Node.AppendChild(addresseeAddr3);
            reqElement.AppendChild(addresseeAddr3Node);


            var addresseeAddr4Node = xmlDoc.CreateNode(XmlNodeType.Element, "obj:addresseeAddr4", ECShipSetting.OBJ);
            var addresseeAddr4 = xmlDoc.CreateTextNode(order.RecipientAddress4);
            addresseeAddr4Node.AppendChild(addresseeAddr4);
            reqElement.AppendChild(addresseeAddr4Node);

            var addresseeCityNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:addresseeCity", ECShipSetting.OBJ);
            var addresseeCity = xmlDoc.CreateTextNode(order.RecipientCity);
            addresseeCityNode.AppendChild(addresseeCity);
            reqElement.AppendChild(addresseeCityNode);

            var addresseeCompanyNameNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:addresseeCompanyName", ECShipSetting.OBJ);
            var addresseeCompanyName = xmlDoc.CreateTextNode(order.RecipientCompanyName);
            addresseeCompanyNameNode.AppendChild(addresseeCompanyName);
            reqElement.AppendChild(addresseeCompanyNameNode);

            var addresseeCountryNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:addresseeCountry", ECShipSetting.OBJ);
            var addresseeCountry = xmlDoc.CreateTextNode(order.DestinationCountry);
            addresseeCountryNode.AppendChild(addresseeCountry);
            reqElement.AppendChild(addresseeCountryNode);

            var addresseeEmailNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:addresseeEmail", ECShipSetting.OBJ);
            var addresseeEmail = xmlDoc.CreateTextNode(order.RecipientEmail);
            addresseeEmailNode.AppendChild(addresseeEmail);
            reqElement.AppendChild(addresseeEmailNode);

            var addresseeNameNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:addresseeName", ECShipSetting.OBJ);
            var addresseeName = xmlDoc.CreateTextNode(order.RecipientAddressName);
            addresseeNameNode.AppendChild(addresseeName);
            reqElement.AppendChild(addresseeNameNode);

            var addresseePostalCodeNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:addresseePostalCode", ECShipSetting.OBJ);
            var addresseePostalCode = xmlDoc.CreateTextNode(order.RecipientPostalNo);
            addresseePostalCodeNode.AppendChild(addresseePostalCode);
            reqElement.AppendChild(addresseePostalCodeNode);

            var addresseeTelNoNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:addresseeTelNo", ECShipSetting.OBJ);
            var addresseeTelNo = xmlDoc.CreateTextNode(order.RecipientPhone);
            addresseeTelNoNode.AppendChild(addresseeTelNo);
            reqElement.AppendChild(addresseeTelNoNode);


            var cn23FlagNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:cn23Flag", ECShipSetting.OBJ);
            var cn23Flag = xmlDoc.CreateTextNode("N");
            cn23FlagNode.AppendChild(cn23Flag);
            reqElement.AppendChild(cn23FlagNode);

            var cn23ItemTypeCodeNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:cn23ItemTypeCode", ECShipSetting.OBJ);
            var cn23ItemTypeCode = xmlDoc.CreateTextNode("O");
            cn23ItemTypeCodeNode.AppendChild(cn23ItemTypeCode);
            reqElement.AppendChild(cn23ItemTypeCodeNode);

            var cn23ItemTypeDescNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:cn23ItemTypeDesc", ECShipSetting.OBJ);
            var cn23ItemTypeDesc = xmlDoc.CreateTextNode("Other");
            cn23ItemTypeDescNode.AppendChild(cn23ItemTypeDesc);
            reqElement.AppendChild(cn23ItemTypeDescNode);


            var custOrdRefNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:custOrdRef", ECShipSetting.OBJ);
            var custOrdRef = xmlDoc.CreateTextNode(order.SubOrderNo);
            custOrdRefNode.AppendChild(custOrdRef);
            reqElement.AppendChild(custOrdRefNode);

            var reqURLNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:reqURL", ECShipSetting.OBJ);
            var reqURL = xmlDoc.CreateTextNode(order.ReqUrl);
            reqURLNode.AppendChild(reqURL);
            reqElement.AppendChild(reqURLNode);


            var insurAmtNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:insurAmt", ECShipSetting.OBJ);
            var insurAmt = xmlDoc.CreateTextNode("1");
            insurAmtNode.AppendChild(insurAmt);
            reqElement.AppendChild(insurAmtNode);


            var insurFlagNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:insurFlag", ECShipSetting.OBJ);
            var insurFlag = xmlDoc.CreateTextNode("N");
            insurFlagNode.AppendChild(insurFlag);
            reqElement.AppendChild(insurFlagNode);


            var insurRPlanNameNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:insurRPlanName", ECShipSetting.OBJ);
            var insurRPlanName = xmlDoc.CreateTextNode("0");
            insurRPlanNameNode.AppendChild(insurRPlanName);
            reqElement.AppendChild(insurRPlanNameNode);


            var sendAddrTypeCodeNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:sendAddrTypeCode", ECShipSetting.OBJ);
            var sendAddrTypeCode = xmlDoc.CreateTextNode("B");
            sendAddrTypeCodeNode.AppendChild(sendAddrTypeCode);
            reqElement.AppendChild(sendAddrTypeCodeNode);

            var isChinese = StringUtil.IsContainChinese(order.SenderAddress);
            var address = "";
            var address2 = "";
            var address3 = "";
            var address4 = "";
            if (!isChinese)
            {
                address = order.SenderAddress.Length > 35 ? order.SenderAddress.Substring(0, 35) : order.SenderAddress;
                address2 = order.SenderAddress2.Length > 35 ? order.SenderAddress2.Substring(0, 35) : order.SenderAddress2;
                address3 = order.SenderAddress3.Length > 35 ? order.SenderAddress3.Substring(0, 35) : order.SenderAddress3;
                address4 = order.SenderAddress4.Length > 35 ? order.SenderAddress4.Substring(0, 35) : order.SenderAddress4;

            }
            else
            {
                address = order.SenderAddress.Length > 11 ? order.SenderAddress.Substring(0, 11) : order.SenderAddress;
                address2 = order.SenderAddress2.Length > 11 ? order.SenderAddress2.Substring(0, 11) : order.SenderAddress2;
                address3 = order.SenderAddress3.Length > 11 ? order.SenderAddress3.Substring(0, 11) : order.SenderAddress3;
                address4 = order.SenderAddress4.Length > 11 ? order.SenderAddress4.Substring(0, 11) : order.SenderAddress4;

            }
            var sendAddress1Node = xmlDoc.CreateNode(XmlNodeType.Element, "obj:sendAddress1", ECShipSetting.OBJ);
            var sendAddress1 = xmlDoc.CreateTextNode(address);
            sendAddress1Node.AppendChild(sendAddress1);
            reqElement.AppendChild(sendAddress1Node);

            var sendAddress2Node = xmlDoc.CreateNode(XmlNodeType.Element, "obj:sendAddress2", ECShipSetting.OBJ);
            var sendAddress2 = xmlDoc.CreateTextNode(address2);
            sendAddress2Node.AppendChild(sendAddress2);
            reqElement.AppendChild(sendAddress2Node);

            var sendAddress3Node = xmlDoc.CreateNode(XmlNodeType.Element, "obj:sendAddress3", ECShipSetting.OBJ);
            var sendAddress3 = xmlDoc.CreateTextNode(address3);
            sendAddress3Node.AppendChild(sendAddress3);
            reqElement.AppendChild(sendAddress3Node);

            var sendAddress4Node = xmlDoc.CreateNode(XmlNodeType.Element, "obj:sendAddress4", ECShipSetting.OBJ);
            var sendAddress4 = xmlDoc.CreateTextNode(address4);
            sendAddress4Node.AppendChild(sendAddress4);
            reqElement.AppendChild(sendAddress4Node);


            var sendContactNameNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:sendContactName", ECShipSetting.OBJ);
            var sendContactName = xmlDoc.CreateTextNode(order.SenderName);
            sendContactNameNode.AppendChild(sendContactName);
            reqElement.AppendChild(sendContactNameNode);

            var sendCustRefNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:sendCustRef", ECShipSetting.OBJ);
            var sendCustRef = xmlDoc.CreateTextNode("");
            sendCustRefNode.AppendChild(sendCustRef);
            reqElement.AppendChild(sendCustRefNode);

            var sendEmailNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:sendEmail", ECShipSetting.OBJ);
            var sendEmail = xmlDoc.CreateTextNode(order.SenderEmail);
            sendEmailNode.AppendChild(sendEmail);
            reqElement.AppendChild(sendEmailNode);

            var sendFaxNoNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:sendFaxNo", ECShipSetting.OBJ);
            var sendFaxNo = xmlDoc.CreateTextNode("");
            sendFaxNoNode.AppendChild(sendFaxNo);
            reqElement.AppendChild(sendFaxNoNode);

            var sendTelNoNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:sendTelNo", ECShipSetting.OBJ);
            var sendTelNo = xmlDoc.CreateTextNode(order.SenderContactNo);
            sendTelNoNode.AppendChild(sendTelNo);
            reqElement.AppendChild(sendTelNoNode);

            var serviceNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:service", ECShipSetting.OBJ);
            var service = xmlDoc.CreateTextNode(order.Service);
            serviceNode.AppendChild(service);
            reqElement.AppendChild(serviceNode);


            if (order.OrderItems.Count > 0)
            {

                var currencyCodeNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:itemCurrCode1", ECShipSetting.OBJ);
                var currencyCode = xmlDoc.CreateTextNode(order.OrderItems[0].CurrencyCode);
                currencyCodeNode.AppendChild(currencyCode);
                reqElement.AppendChild(currencyCodeNode);
                for (int i = 0; i < order.OrderItems.Count; i++)
                {
                    var product = order.OrderItems[i];
                    var index = i + 1;

                    //if (index == 4 && order.OrderItems.Count > 4)
                    //{

                    //}
                    var itemCtyCode1Node = xmlDoc.CreateNode(XmlNodeType.Element, "obj:itemCtyCode" + index, ECShipSetting.OBJ);
                    var itemCtyCode1 = xmlDoc.CreateTextNode("");
                    itemCtyCode1Node.AppendChild(itemCtyCode1);
                    reqElement.AppendChild(itemCtyCode1Node);

                    var pIsChinese = StringUtil.IsContainChinese(product.Desc);
                    var name = "";
                    if (!pIsChinese)
                    {
                        name = product.Desc.Length > 60 ? product.Desc.Substring(0, 60) : product.Desc;
                    }
                    else
                    {
                        name = product.Desc.Length > 20 ? product.Desc.Substring(0, 20) : product.Desc;
                    }

                    var contentDescNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:itemDesc" + index, ECShipSetting.OBJ);

                    var contentDesc = xmlDoc.CreateTextNode(name);
                    contentDescNode.AppendChild(contentDesc);
                    reqElement.AppendChild(contentDescNode);



                    var itemHSCode1Node = xmlDoc.CreateNode(XmlNodeType.Element, "obj:itemHSCode" + index, ECShipSetting.OBJ);
                    var itemHSCode1 = xmlDoc.CreateTextNode("");
                    itemHSCode1Node.AppendChild(itemHSCode1);
                    reqElement.AppendChild(itemHSCode1Node);


                    var productQtyNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:itemQty" + index, ECShipSetting.OBJ);
                    var productQty = xmlDoc.CreateTextNode(product.Qty.ToString());
                    productQtyNode.AppendChild(productQty);
                    reqElement.AppendChild(productQtyNode);



                    var productValueNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:itemValue" + index, ECShipSetting.OBJ);
                    var productValue = xmlDoc.CreateTextNode(product.TotalPrice.ToString());
                    productValueNode.AppendChild(productValue);
                    reqElement.AppendChild(productValueNode);

                    var productWeightNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:itemWgt" + index, ECShipSetting.OBJ);
                    var productWeight = xmlDoc.CreateTextNode(product.Weight.ToString());
                    productWeightNode.AppendChild(productWeight);
                    reqElement.AppendChild(productWeightNode);
                }

                var productTariffCodeNode = xmlDoc.CreateNode(XmlNodeType.Element, "obj:itemTypeCode", ECShipSetting.OBJ);
                var productTariffCode = xmlDoc.CreateTextNode("M");
                productTariffCodeNode.AppendChild(productTariffCode);
                reqElement.AppendChild(productTariffCodeNode);


            }


            envelopeElement.AppendChild(bodyElement);

            return envelopeElement.OuterXml;
        }
    }
}
