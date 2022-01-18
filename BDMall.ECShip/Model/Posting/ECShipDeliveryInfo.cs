using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BDMall.Model.Enums;

namespace BDMall.ECShip.Model.Posting
{
    public class ECShipDeliveryInfo
    {
        public ECShipDeliveryInfo()
        {
            this.ShipCode = "";
            this.CountryCode = "";
            this.CurrencyCode = "";
            this.ItemCategory = "";
            this.InsurTypeCode = "";
            this.NonDeliveryOptions = "";
            this.PickupOffice = "";
            this.IPostStation = "";
            this.SmsLang = "";
            this.NoticeMethod = "";
            this.PaidAmt = 0;
            this.MerchandiserEmail = "";
        }
        public string ShipCode { get; set; }
        public DeliveryMailType MailType { get; set; }
        public string CountryCode { get; set; }
        public string CurrencyCode { get; set; }

        public string ItemCategory { get; set; }

        public string InsurTypeCode { get; set; }

        public string NonDeliveryOptions { get; set; }

        /// <summary>
        /// 櫃檯code
        /// </summary>
        public string PickupOffice { get; set; }

        /// <summary>
        /// 智郵站站點code
        /// </summary>
        public string IPostStation { get; set; }

        public string SmsLang { get; set; }

        public string NoticeMethod { get; set; }

        public decimal PaidAmt { get; set; }

        public string MerchandiserEmail { get; set; }

        public string MCN { get; set; }
    }
}
