using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Domain 
{
    public class RnpSubmitDto
    {
        public Guid Id { get; set; }
        public Guid FormId { get; set; }

        public decimal? TotalAmount { get; set; }

        public bool IsPayed { get; set; }

        public string GatewayResponse { get; set; }

        public string UpdatePayedBy { get; set; }

        public string Signature { get; set; }

        public List<RnpSubmitDataInfoView> RnpSubmitDataInfo { get; set; }
        public List<RnpSubmitDataDto> RnpSubmitDatas { get; set; }
    }
}
