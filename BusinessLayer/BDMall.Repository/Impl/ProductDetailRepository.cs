using BDMall.Domain;
using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Repository
{
    public class ProductDetailRepository : PublicBaseRepository, IProductDetailRepository
    {
        public ProductDetailRepository(IServiceProvider service) : base(service)
        {
        }

        public List<MutiLanguage> GetMutiLanguage(Guid transId)
        {
            if (transId == Guid.Empty)
            {
                return new List<MutiLanguage>();
            }
            else
            {
                var supportLangs = GetSupportLanguage();
                var data = new List<MutiLanguage>();

                var translates = baseRepository.GetList<ProductDetail>(d => d.TransId == transId).Select(d => d).ToList();

                bool exist = false;
                foreach (var supportLang in supportLangs)
                {
                    exist = false;
                    foreach (var tran in translates)
                    {
                        if (supportLang.Code.Trim() == tran.Lang.ToString().Trim())
                        {
                            exist = true;
                            data.Add(new MutiLanguage { Desc = tran.Value, Lang = supportLang });
                        }

                    }
                    if (!exist)
                    {
                        data.Add(new MutiLanguage { Desc = "", Lang = supportLang });
                    }
                }

                return data;
            }

        }
    }
}
