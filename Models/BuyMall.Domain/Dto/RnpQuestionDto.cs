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
    public class RnpQuestionDto
    {
        public Guid Id { get; set; }
        public Guid FormId { get; set; }

        public string Content { get; set; }

        public string DataType { get; set; }

        public int? Type { get; set; }

        public bool? IsRequired { get; set; }

        public int? Seq { get; set; }

    }
}
