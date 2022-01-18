using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.ECShip.Model.Posting
{
    public class ECShipOrderInfo
    {
        /// <summary>
        /// 收件国家
        /// </summary>
        public string DestinationCountry { get; set; }


        public string RecipientPhone { get; set; }
        /// <summary>
        /// 收件人地址
        /// </summary>
        public string RecipientAddress { get; set; }

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

        /// <summary>
        /// 订单编号
        /// </summary>
        public string RefNo { get; set; }
        /// <summary>
        /// 發件人地區
        /// </summary>
        public string SenderCountry { get; set; }

        /// <summary>
        /// 发件人地址
        /// </summary>
        public string SenderAddress { get; set; }
        public string SenderAddress2 { get; set; }
        public string SenderAddress3 { get; set; }
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
        public string SenderName { get; set;}


        /// <summary>
        /// 货物信息
        /// </summary>
        public List<ECShipOrderItemInfo> OrderItems { get; set; }




    }
}
