﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Model
{
    public class RnpSubmitData : BaseEntity<Guid>
    {
        public Guid SubmitId { get; set; }

        public Guid QuestionId { get; set; }

        public Guid? AnswerId { get; set; }

        [StringLength(100)]
        public string Display { get; set; }

        [Required]
        [StringLength(300)]
        public string Result { get; set; }

        public int? Qty { get; set; }

        [Column(TypeName = "money")]
        public decimal? Price { get; set; }
    }
}
