﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Model
{
    public class Province : BaseEntity<int>
    {
        [StringLength(5)]
        public string Code { get; set; }

        [StringLength(20)]
        public string Name { get; set; }
        [StringLength(20)]
        public string Name_e { get; set; }
        [StringLength(20)]
        public string Name_c { get; set; }
        [StringLength(20)]
        public string Name_s { get; set; }
        [StringLength(20)]
        public string Name_j { get; set; }

        public int CountryId { get; set; }

    }
}