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
    public class RnpSubmitDataDto
    {
        public Guid Id { get; set; }
        public Guid SubmitId { get; set; }

        public Guid QuestionId { get; set; }

        public Guid? AnswerId { get; set; }

        public string Display { get; set; }

        public string Result { get; set; }

        public int? Qty { get; set; }

        public decimal? Price { get; set; }
    }
}
