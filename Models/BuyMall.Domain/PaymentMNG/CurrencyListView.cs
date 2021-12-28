using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class CurrencyListView
    {
        public List<CurrencyExchangeRate> list { get; set; } = new List<CurrencyExchangeRate>();
    }
}
