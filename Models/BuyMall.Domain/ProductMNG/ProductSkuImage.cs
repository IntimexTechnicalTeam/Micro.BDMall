﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public  class ProductSkuImage
    {
        public Guid ProductId { get; set; }

        public Guid Attr1 { get; set; }

        public Guid Attr2 { get; set; }

        public Guid Attr3 { get; set; }

        public string Path { get; set; }
    }
}
