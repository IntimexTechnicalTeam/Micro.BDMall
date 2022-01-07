using BDMall.Domain;
using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.BLL
{
    public class ProductImageBLL : BaseBLL, IProductImageBLL
    {
        public IAttributeBLL attributeBLL;

        public ProductImageBLL(IServiceProvider services) : base(services)
        {
            attributeBLL= Services.Resolve<IAttributeBLL>();
        }

        public List<ProductImageView> GetImageByProductId(Guid id)
        {
            var images = baseRepository.GetList<ProductImage>(x=>x.ProductId ==id && x.IsActive && !x.IsDeleted).ToList();

            var imageViews = images.Select(d => GenProductSkuImageView(d, Guid.Empty)).ToList();

            return imageViews;

        }

        private ProductImageView GenProductSkuImageView(ProductImage image, Guid defaultImageID)
        {
            ProductImageView view = new ProductImageView();

            //var sku = _productSkuRepository.GetByKey(image.Sku);

            view.Id = image.Id;
            view.ProductId = image.ProductId;
            //view.Sku = image.Sku;
            view.Type = image.Type;
            view.IsDefault = image.Id == defaultImageID ? true : false;
            view.IsDefaultName = view.IsDefault == true ? Resources.Label.True : Resources.Label.False;
            view.AttrValue1 = image.AttrValue1;
            view.AttrValue2 = image.AttrValue2;
            view.AttrValue3 = image.AttrValue3;

            view.AttrValues1Name = image.AttrValue1 == Guid.Empty ? "" : attributeBLL.GetAttributeValue(image.AttrValue1)?.Descs.FirstOrDefault(p => p.Language == CurrentUser.Lang)?.Desc ?? "";
            view.AttrValues2Name = image.AttrValue2 == Guid.Empty ? "" : attributeBLL.GetAttributeValue(image.AttrValue2)?.Descs.FirstOrDefault(p => p.Language == CurrentUser.Lang)?.Desc ?? "";
            view.AttrValues3Name = image.AttrValue3 == Guid.Empty ? "" : attributeBLL.GetAttributeValue(image.AttrValue3)?.Descs.FirstOrDefault(p => p.Language == CurrentUser.Lang)?.Desc ?? "";

            if (image.ImageItems != null)
            {
                List<ProductImageItemView> list = new List<ProductImageItemView>();
                if (image.ImageItems != null)
                {
                    var imageItems = image.ImageItems.OrderBy(o => o.Type).ToList();

                    foreach (var item in imageItems)
                    {
                        ProductImageItemView itemView = new ProductImageItemView();
                        itemView.Id = item.Id;
                        itemView.ImageID = image.Id;
                        itemView.Width = item.Width;
                        itemView.Length = item.Length;
                        itemView.OriginalPath = item.OriginalPath;
                        itemView.Path = item.Path;
                        itemView.Size = item.Size;
                        itemView.Type = item.Type;
                        itemView.ImageType = item.Type.ToString();
                        list.Add(itemView);
                    }
                }

                view.Items = list;

            }
            if (view.Items != null && view.Items.Count > 0)
            {
                view.Image = view.Items.First().Path;
            }
            else
            {
                view.Image = "";
            }
            return view;
        }
    }
}
