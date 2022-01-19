using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Repository
{
    public class MerchantFreeChargeRepository : PublicBaseRepository, IMerchantFreeChargeRepository
    {
        public MerchantFreeChargeRepository(IServiceProvider service) : base(service)
        {
        }

        public List<MerchantFreeCharge> GetByMerchantId(Guid id)
        {
            return baseRepository.GetList<MerchantFreeCharge>().Where(p => p.IsActive && !p.IsDeleted && p.MerchantId == id).ToList();
        }

        public MerchantFreeChargeView GetMerchantFreeChargeInfo(Guid id, List<string> shipCodes)
        {

            MerchantFreeChargeView result = new MerchantFreeChargeView();

            var view = (from f in baseRepository.GetList<MerchantFreeCharge>()
                        where f.IsActive && !f.IsDeleted && f.MerchantId == id
                        select new MerchantFreeChargeView
                        {
                            Id = f.Id,
                            MerchantId = f.MerchantId,
                        }).FirstOrDefault();

            if (view != null)
            {
                view.ShipCodes = baseRepository.GetList<MerchantFreeCharge>().Where(p => p.IsActive && !p.IsDeleted && p.MerchantId == id).Select(d => d.ShipCode).Distinct().ToList();

                view.Products = (from f in baseRepository.GetList<MerchantFreeCharge>()
                                 join p in baseRepository.GetList<Product>() on new { a1 = f.ProductCode, a2 = ProductStatus.OnSale } equals new { a1 = p.Code, a2 = p.Status }
                                 join t in baseRepository.GetList<Translation>() on new { a1 = p.NameTransId, a2 = CurrentUser.Lang } equals new { a1 = t.TransId, a2 = t.Lang } into tc
                                 from tt in tc.DefaultIfEmpty()
                                 join i in baseRepository.GetList<ProductImageList>() on new { a1 = p.DefaultImage, a2 = ImageSizeType.S1 } equals new { a1 = i.ImageID, a2 = i.Type } into ic
                                 from ii in ic.DefaultIfEmpty()
                                 where f.MerchantId == id && f.ProductCode != ""
                                 && shipCodes.Contains(f.ShipCode)
                                 select new MerchantFreeChargeProductView
                                 {
                                     Id = p.Id,
                                     Code = p.Code,
                                     Name = tt == null ? "" : tt.Value,
                                     Image = ii == null ? "" : ii.Path,
                                     IsDeleted = false

                                 }).Distinct().ToList();

                result = view;
            }
            else
            {
                result.Id = Guid.Empty;
                result.MerchantId = id;
                result.Products = new List<MerchantFreeChargeProductView>();
                result.ShipCodes = new List<string>();
            }

            return result;
        }


        public List<MerchantFreeCharge> GetByCode(string code)
        {
            var query = baseRepository.GetList<MerchantFreeCharge>().Where(p => p.ProductCode == code.Trim()).ToList();

            return query;
        }
    }
}
