﻿using Intimex.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public interface ICountryBLL:IDependency
    {
        List<KeyValue> GetCountry();
    }
}