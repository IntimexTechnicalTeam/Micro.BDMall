
using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public class PreProductImageService : AbstractPreHeatService
    {
        public PreProductImageService(IServiceProvider services) : base(services)
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id">ProductId</param>
        /// <returns></returns>
        public override async Task<SystemResult> CreatePreHeat(Guid Id)
        {
            var result = new SystemResult();
            var query = await GetDataSourceAsync(Id);
            await SetDataToHashCache(query);
            return result;           
        }

        public async Task<IQueryable<HotProductImage>> GetDataSourceAsync(Guid Id)
        {
            var query = from pl in baseRepository.GetList<ProductImageList>()
                        join p in baseRepository.GetList<Product>() on pl.ImageID equals p.DefaultImage
                        where pl.IsActive && !pl.IsDeleted  //&& pl.Type == ImageSizeType.S3     //首页只拿S3 即 300*300
                        && p.IsActive && !p.IsDeleted                      
                        select new HotProductImage
                        {
                            Id = pl.Id,
                            ImageId = p.DefaultImage,           //可关联到Product.DefaultImage
                            ProductId = p.Id,
                            ProductCode = p.Code,
                            Type = pl.Type,
                            ImagePath = pl.Path,
                        };

            if (Id != Guid.Empty)
                query = query.Where(x => x.ProductId == Id);

            return query;
        }

        public async Task<SystemResult> SetDataToHashCache(IQueryable<HotProductImage> list)
        {
            var result = new SystemResult();
            string key = PreHotType.Hot_ProductImage.ToString();
            var hotList = list.OrderBy(o => o.Type).ToList();
            if (hotList != null && hotList.Any())
            {
                var proList = hotList.Select(s => new { s.ProductCode, s.ProductId }).Distinct().ToList();
                foreach (var pro in proList)
                {
                    var imgList = new List<HotProductImage>();
                    foreach (var item in hotList)
                    {
                        if (item.ProductCode == pro.ProductCode)
                        {
                            imgList.Add(new HotProductImage
                            {
                                ProductCode = item.ProductCode,
                                ProductId = item.ProductId,
                                ImageId = item.ImageId,
                                ImagePath = item.ImagePath,
                                Type = item.Type,
                            });
                        }
                    }
                    await RedisHelper.HSetAsync(key, pro.ProductCode, imgList);
                }
            }
            return result;
        }
    }
}