using BDMall.ECShip.Model.Posting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.ECShip.Model.SoazPosting
{
    public class SoazOrderInfo
    {
        /// <summary>
        /// 收件国家
        /// </summary>
        public string DestinationCountry { get; set; }

        public string SubOrderNo { get; set; }

        /// <summary>
        /// 收件人地址
        /// </summary>
        public string RecipientAddress1 { get; set; }
        public string RecipientAddress2 { get; set; }
        public string RecipientAddress3 { get; set; }
        public string RecipientAddress4 { get; set; }

        public string RecipientAddressName { get; set; }

        /// <summary>
        /// 收件人城市
        /// </summary>
        public string RecipientCity { get; set; }

        /// <summary>
        /// 收件人名称
        /// </summary>
        public string RecipientName { get; set; }

        /// <summary>
        /// 用落單人代替
        /// </summary>
        public string RecipientEmail { get; set; }

        /// <summary>
        /// 收件人邮编
        /// </summary>
        public string RecipientPostalNo { get; set; }
        public string RecipientCompanyName { get; set; }

        public string RecipientPhone { get; set; }


        /// 发件人地址
        /// </summary>
        public string SenderAddress { get; set; }
        /// 发件人地址
        /// </summary>
        public string SenderAddress2 { get; set; }
        /// 发件人地址
        /// </summary>
        public string SenderAddress3 { get; set; }
        /// 发件人地址
        /// </summary>
        public string SenderAddress4 { get; set; }
        

        /// <summary>
        /// 发件人联系电话
        /// </summary>
        public string SenderContactNo { get; set; }

        /// <summary>
        /// 发件人邮箱
        /// </summary>
        public string SenderEmail { get; set; }

        /// <summary>
        /// 发件人名称
        /// </summary>
        public string SenderName { get; set; }


        /// <summary>
        /// EMS-SPT_STD
        /// SPGDE-SPT_ECPOST
        /// SPGDEE-SPT_ECON
        /// </summary>
        public string Service { get; set; }


        /// <summary>
        /// for SPGDE 当前系统地址
        /// </summary>
        public string ReqUrl { get; set; }

        /// <summary>
        /// 货物信息
        /// </summary>
        public List<ECShipOrderItemInfo> OrderItems { get; set; }




    }
}
