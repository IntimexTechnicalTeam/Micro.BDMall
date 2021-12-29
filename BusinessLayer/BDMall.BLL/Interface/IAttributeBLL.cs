﻿using BDMall.Domain;

using Intimex.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public interface IAttributeBLL:IDependency
    {

        List<KeyValue> GetInveAttribute();

        List<KeyValue> GetNonInveAttribute();

        List<ProductAttributeValueDto> GetInveAttributeValueSummary();

        PageData<ProductAttributeDto> SearchAttribute(ProductAttributeCond attrCond);
    }
}
