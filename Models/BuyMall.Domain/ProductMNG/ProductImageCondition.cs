﻿using BDMall.Enums;
using Intimex.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class ProductImageCondition
    {
        public string SourceImage { get; set; }
        public Guid ProdId { get; set; }
        public Guid AttrValue1 { get; set; }
        public Guid AttrValue2 { get; set; }
        public Guid AttrValue3 { get; set; }
        public ImageType ImageType { get; set; }

        public List<KeyValue> ImagePaths { get; set; } = new List<KeyValue>();

    }
}
