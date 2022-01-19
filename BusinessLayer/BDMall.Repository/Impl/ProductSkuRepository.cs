
using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BDMall.Repository
{
    public class ProductSkuRepository : PublicBaseRepository, IProductSkuRepository
    {
        public ProductSkuRepository(IServiceProvider service) : base(service)
        {
        }

        public List<ProductSku> GetSkuByAttrValueId(string prodCode, Guid attrValueId)
        {
            var skus = baseRepository.GetList<ProductSku>(p => p.IsActive && !p.IsDeleted && p.ProductCode == prodCode && (p.AttrValue1 == attrValueId || p.AttrValue1 == attrValueId || p.AttrValue3 == attrValueId)).ToList();
            return skus;
        }

        public ProductSku GetActiveSkuByAttrValueId(string prodCode, Guid attrValueId1, Guid attrValueId2, Guid attrValueId3)
        {
            var sku = baseRepository.GetModel<ProductSku>(p => p.IsActive && !p.IsDeleted && p.ProductCode == prodCode && p.AttrValue1 == attrValueId1 && p.AttrValue2 == attrValueId2 && p.AttrValue3 == attrValueId3);
            return sku;
        }

        public ProductSku GetSkuByAttrValueId(string prodCode, Guid attrValueId1, Guid attrValueId2, Guid attrValueId3)
        {
            var sku = baseRepository.GetModel<ProductSku>(p => p.ProductCode == prodCode && p.AttrValue1 == attrValueId1 && p.AttrValue2 == attrValueId2 && p.AttrValue3 == attrValueId3);

            return sku;
        }

        public List<ProductSku> GetByProductCode(string prodCode)
        {
            var skus = baseRepository.GetList<ProductSku>(p => p.IsActive && !p.IsDeleted && p.ProductCode == prodCode).ToList();
            return skus;
        }

        /// <summary>
        /// 生成产品对应的sku
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public List<ProductSku> GenProduckSku(List<ProductAttr> attrs, string prodCode)
        {
            List<ProductSku> result = new List<ProductSku>();
            //List<List<ProductAttrValue>> listAttrValues = new List<List<ProductAttrValue>>();
            List<ProductAttrValue> attrValue1 = new List<ProductAttrValue>();
            List<ProductAttrValue> attrValue2 = new List<ProductAttrValue>();
            List<ProductAttrValue> attrValue3 = new List<ProductAttrValue>();

            if (attrs == null || !attrs.Any())
            {
                ProductSku sku = new ProductSku();
                sku.Id = Guid.NewGuid();
                sku.Attr1 = Guid.Empty;
                sku.Attr2 = Guid.Empty;
                sku.Attr3 = Guid.Empty;
                sku.AttrValue1 = Guid.Empty;
                sku.AttrValue2 = Guid.Empty;
                sku.AttrValue3 = Guid.Empty;
                sku.ProductCode = prodCode;
                result.Add(sku);
                return result;
            }

            //List<ProductAttrValue> listAttrValues = new List<ProductAttrValue>();
            //int attrCount = product.Attrs.Count();
            var attrID1 = Guid.Empty;
            var attrID2 = Guid.Empty;
            var attrID3 = Guid.Empty;
            var invAttrs = attrs.Where(p => p.IsInv == true).ToList();

            for (int i = 0; i < invAttrs.Count; i++)
            {
                if (i == 0)
                {
                    attrID1 = invAttrs[0].AttrId;
                    attrValue1 = invAttrs[0].AttrValues.Where(p => p.IsDeleted == false).ToList();
                }
                else if (i == 1)
                {
                    attrID2 = invAttrs[1].AttrId;
                    attrValue2 = invAttrs[1].AttrValues.Where(p => p.IsDeleted == false).ToList();
                }
                else if (i == 2)
                {
                    attrID3 = invAttrs[2].AttrId;
                    attrValue3 = invAttrs[2].AttrValues.Where(p => p.IsDeleted == false).ToList();
                }
            }

            if (attrValue1.Any())
            {
                foreach (var attr1 in attrValue1)
                {
                    if (attrValue2.Any())
                    {
                        foreach (var attr2 in attrValue2)
                        {
                            if (attrValue3.Any())
                            {
                                foreach (var attr3 in attrValue3)
                                {
                                    ProductSku sku = new ProductSku();
                                    sku.Id = Guid.NewGuid();
                                    sku.Attr1 = attrID1;
                                    sku.Attr2 = attrID2;
                                    sku.Attr3 = attrID3;
                                    sku.AttrValue1 = attr1.AttrValueId;
                                    sku.AttrValue2 = attr2.AttrValueId;
                                    sku.AttrValue3 = attr3.AttrValueId;
                                    sku.ProductCode = prodCode;
                                    result.Add(sku);
                                }
                            }
                            else
                            {
                                ProductSku sku = new ProductSku();
                                sku.Id = Guid.NewGuid();
                                sku.Attr1 = attrID1;
                                sku.Attr2 = attrID2;
                                sku.Attr3 = attrID3;
                                sku.AttrValue1 = attr1.AttrValueId;
                                sku.AttrValue2 = attr2.AttrValueId;
                                sku.AttrValue3 = Guid.Empty;
                                sku.ProductCode = prodCode;
                                result.Add(sku);
                            }
                        }
                    }
                    else if (attrValue3.Any())
                    {
                        foreach (var attr3 in attrValue3)
                        {
                            ProductSku sku = new ProductSku();
                            sku.Id = Guid.NewGuid();
                            sku.Attr1 = attrID1;
                            sku.Attr2 = attrID2;
                            sku.Attr3 = attrID3;
                            sku.AttrValue1 = attr1.AttrValueId;
                            sku.AttrValue2 = Guid.Empty;
                            sku.AttrValue3 = attr3.AttrValueId;
                            sku.ProductCode = prodCode;
                            result.Add(sku);
                        }
                    }
                    else
                    {
                        ProductSku sku = new ProductSku();
                        sku.Id = Guid.NewGuid();
                        sku.Attr1 = attrID1;
                        sku.Attr2 = attrID2;
                        sku.Attr3 = attrID3;
                        sku.AttrValue1 = attr1.AttrValueId;
                        sku.AttrValue2 = Guid.Empty;
                        sku.AttrValue3 = Guid.Empty;
                        sku.ProductCode = prodCode;
                        result.Add(sku);
                    }
                }
            }
            else
            {
                if (attrValue2.Any())
                {
                    foreach (var attr2 in attrValue2)
                    {
                        if (attrValue3.Any())
                        {
                            foreach (var attr3 in attrValue3)
                            {
                                ProductSku sku = new ProductSku();
                                sku.Id = Guid.NewGuid();
                                sku.Attr1 = attrID1;
                                sku.Attr2 = attrID2;
                                sku.Attr3 = attrID3; ;
                                sku.AttrValue1 = Guid.Empty;
                                sku.AttrValue2 = attr2.AttrValueId;
                                sku.AttrValue3 = attr3.AttrValueId;
                                sku.ProductCode = prodCode;
                                result.Add(sku);
                            }
                        }
                        else
                        {
                            ProductSku sku = new ProductSku();
                            sku.Id = Guid.NewGuid();
                            sku.Attr1 = attrID1;
                            sku.Attr2 = attrID2;
                            sku.Attr3 = attrID3;
                            sku.AttrValue1 = Guid.Empty;
                            sku.AttrValue2 = attr2.AttrValueId;
                            sku.AttrValue3 = Guid.Empty;
                            sku.ProductCode = prodCode;
                            result.Add(sku);
                        }
                    }
                }
                else
                {
                    if (attrValue3.Any())
                    {
                        foreach (var attr3 in attrValue3)
                        {
                            ProductSku sku = new ProductSku();
                            sku.Id = Guid.NewGuid();
                            sku.Attr1 = attrID1;
                            sku.Attr2 = attrID2;
                            sku.Attr3 = attrID3;
                            sku.AttrValue1 = Guid.Empty;
                            sku.AttrValue2 = Guid.Empty;
                            sku.AttrValue3 = attr3.AttrValueId;
                            sku.ProductCode = prodCode;
                            result.Add(sku);
                        }
                    }
                    else
                    {
                        ProductSku sku = new ProductSku();
                        sku.Id = Guid.NewGuid();
                        sku.Attr1 = Guid.Empty;
                        sku.Attr2 = Guid.Empty;
                        sku.Attr3 = Guid.Empty;
                        sku.AttrValue1 = Guid.Empty;
                        sku.AttrValue2 = Guid.Empty;
                        sku.AttrValue3 = Guid.Empty;
                        sku.ProductCode = prodCode;
                        result.Add(sku);
                    }
                }
            }

            return result;
        }
    }
}
