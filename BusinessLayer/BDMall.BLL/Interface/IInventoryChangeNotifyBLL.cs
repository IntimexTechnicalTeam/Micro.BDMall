﻿using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public interface IInventoryChangeNotifyBLL:IDependency
    {
        SystemResult AddInventoryChangeNotify(InventoryChangeNotify notify);

        void CheckAndNotifyAsync(IList<Guid> skuIds);
    }
}
