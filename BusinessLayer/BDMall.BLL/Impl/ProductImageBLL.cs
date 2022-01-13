using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using BDMall.Repository;
using BDMall.Utility;
using Intimex.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public class ProductImageBLL : BaseBLL, IProductImageBLL
    {
        public IAttributeBLL attributeBLL;
        public IProductImageRepository productImageRepository;
        public ISettingBLL settingBLL;

        public ProductImageBLL(IServiceProvider services) : base(services)
        {
            attributeBLL = Services.Resolve<IAttributeBLL>();
            productImageRepository = Services.Resolve<IProductImageRepository>();
            settingBLL = Services.Resolve<ISettingBLL>();   
        }

        public List<ProductImageView> GetImageByProductId(Guid id)
        {
            var images = baseRepository.GetList<ProductImage>(x => x.ProductId == id && x.IsActive && !x.IsDeleted).ToList();

            var imageViews = images.Select(d => GenProductSkuImageView(d, Guid.Empty)).ToList();

            return imageViews;

        }

        public void SaveProductImage(ProductImageCondition cond)
        {
            var product = baseRepository.GetModelById<Product>(cond.ProdId);
            if (product != null)
            {
                switch (cond.ImageType)
                {
                    case ImageType.SkuImage:
                        SaveProductAttributeImage(cond);
                        break;
                    case ImageType.AdditionImage:
                        SaveProductAdditionalImage(cond);
                        break;
                }
            }
        }

        public List<ProductImageView> GetProductSkuImageList(Guid prodID)
        {
            List<ProductImageView> imgs = GetProductImageList(prodID, ImageType.SkuImage);
            return imgs;

        }

        public List<ProductImageView> GetAdditionalImgs(Guid prodID)
        {
            List<ProductImageView> imgs = GetProductImageList(prodID, ImageType.AdditionImage);
            return imgs;
        }

        public List<ProductImageView> GetProductImageList(Guid prodID, ImageType imageType)
        {
            List<ProductImageView> imgs = new List<ProductImageView>();
            var datas = productImageRepository.GetImageByType(prodID, imageType).OrderBy(o => o.CreateDate).ToList();

            if (datas != null && datas.Any())
            {
                var product = baseRepository.GetModelById<Product>(prodID);
                foreach (var item in datas)
                {
                    imgs.Add(GenProductSkuImageView(item, product.DefaultImage));
                }
            }
            return imgs;
        }

        public async Task<SystemResult> SaveProductSkuImage(ProductSkuImage model)
        {
            var result = new SystemResult();
            List<KeyValue> dbImagePaths = new List<KeyValue>();
            List<string> delPaths = new List<string>();
            ProductImageView prodImage = new ProductImageView();

            var product = baseRepository.GetModelById<Product>(model.ProductId);
            var imageSizes = settingBLL.GetProductImageSize();
            var dbProductImage = productImageRepository.GetImageBySku(model.ProductId, model.Attr1, model.Attr2, model.Attr3, ImageType.SkuImage);
            if (dbProductImage !=null)
                prodImage = GenProductSkuImageView(dbProductImage, product.DefaultImage);

            GenSkuImagePath(product, model.Path, delPaths, dbImagePaths, imageSizes, prodImage);

            ProductImageCondition cond = new ProductImageCondition();
            cond.AttrValue1 = model.Attr1;
            cond.AttrValue2 = model.Attr2;
            cond.AttrValue3 = model.Attr3;
            cond.ImageType = ImageType.SkuImage;
            cond.ProdId = model.ProductId;
            cond.ImagePaths = dbImagePaths;

            SaveProductImage(cond);
            result.Succeeded = true;

            //处理图片
            if (result.Succeeded == true)
            {
                string tempPath = PathUtil.GetPhysicalPath(Globals.Configuration["UploadPath"], product.MerchantId.ToString(), FileFolderEnum.TempPath);
                string targetPath = PathUtil.GetPhysicalPath(Globals.Configuration["UploadPath"], product.MerchantId.ToString(), FileFolderEnum.Product) + "\\" + product.Id;
                List<string> insertImgs = new List<string>();

                foreach (var item in delPaths)
                {
                    FileUtil.DeleteFile(targetPath + "\\" + Path.GetFileName(item));
                }
                for (int i = 0; i < imageSizes.Count; i++)
                {
                    var img = dbImagePaths[i];
                    var imgSize = imageSizes[i];
                    insertImgs.Add(img.Text);
                    ImageUtil.CreateImg(Path.Combine(tempPath, model.Path), targetPath, Path.GetFileName(img.Text), imgSize.Width, imgSize.Length);
                }
            }
            return result;
        }

        public void SetDefaultImage(Guid prodID, Guid imageID)
        {
            var product = baseRepository.GetModelById<Product>(prodID);
            if (product != null)
            {
                product.DefaultImage = imageID;
                baseRepository.Update(product);
            }
        }

        public void DeleteImage(Guid imageID)
        {
            var productImage = baseRepository.GetModelById<ProductImage>(imageID);
            if (productImage != null)
            {
                productImage.IsDeleted = true;
                baseRepository.Update(productImage);

                //如果是默认图片，则清空产品的默认图片ID
                var product = baseRepository.GetModel<Product>(p => p.DefaultImage == imageID);
                if (product != null)
                {
                    product.DefaultImage = Guid.Empty;
                    baseRepository.Update(product);
                }
               
            }
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

            var imageItems  = baseRepository.GetList<ProductImageList>(x=>x.ImageID == image.Id).OrderBy(o => o.Type).ToList();
            view.Items = imageItems.Select(item => new ProductImageItemView
            {
                Id = item.Id,
                ImageID = image.Id,
                Width = item.Width,
                Length = item.Length,
                OriginalPath = item.OriginalPath,
                Path = item.Path,
                Size = item.Size,
                Type = item.Type,
                ImageType = item.Type.ToString()
            }).ToList();

            view.Image = view.Items?.FirstOrDefault()?.Path ?? "";
            return view;
        }

        private void SaveProductAttributeImage(ProductImageCondition cond)
        {
            var productImage = productImageRepository.GetImageBySku(cond.ProdId, cond.AttrValue1, cond.AttrValue2, cond.AttrValue3, cond.ImageType);
            UnitOfWork.IsUnitSubmit = true;

            var listImageSize = settingBLL.GetProductImageSize();
            if (productImage == null)//新增skuimage
            {
                var imageID = Guid.NewGuid();
                productImage = new ProductImage();
                productImage.Id = imageID;
                productImage.ProductId = cond.ProdId;
                productImage.Side = 0;
                productImage.AttrValue1 = cond.AttrValue1;
                productImage.AttrValue2 = cond.AttrValue2;
                productImage.AttrValue3 = cond.AttrValue3;
                productImage.Type = cond.ImageType;

                var images = GetProductImageList(cond.ProdId, cond.ImageType);
                if (images == null || !images.Any())
                    SetDefaultImage(cond.ProdId, imageID);

                List<ProductImageList> list = new List<ProductImageList>();
                for (int i = 0; i < cond.ImagePaths.Count; i++)
                {
                    var imagePath = cond.ImagePaths[i];
                    var item = listImageSize[i];
                    ProductImageList imageItem = new ProductImageList();
                    imageItem.Id = Guid.Parse(imagePath.Id);

                    imageItem.ImageID = imageID;
                    imageItem.OriginalPath = "";
                    imageItem.Path = imagePath.Text;
                    imageItem.Size = 0;
                    imageItem.Width = item.Width.ToString();
                    imageItem.Length = item.Length.ToString();
                    imageItem.Type = (ImageSizeType)i;
                    list.Add(imageItem);
                }
                productImage.ImageItems = list;
                baseRepository.Insert(productImage);
                baseRepository.Insert(list);
            }
            else//更新SKU Image
            {
                if (cond.ImagePaths.Any())
                {
                    var dbImages = baseRepository.GetList<ProductImageList>(x => x.ImageID == productImage.Id).OrderBy(o => o.Type).ToList();
                    if (dbImages.Any())
                    {
                        for (int i = 0; i < dbImages.Count; i++)
                        {
                            var item = dbImages.ToList()[i];
                            var imagePath = cond.ImagePaths[i];     //替换掉

                            item.OriginalPath = "";
                            item.Path = imagePath.Text;
                            item.Size = 0;
                            item.UpdateDate = DateTime.Now;
                        }
                    }
                    else
                    {
                        List<ProductImageList> list = new List<ProductImageList>();
                        for (int i = 0; i < cond.ImagePaths.Count; i++)
                        {
                            var imagePath = cond.ImagePaths[i];
                            var item = listImageSize[i];

                            ProductImageList imageItem = new ProductImageList();
                            imageItem.Id = Guid.Parse(imagePath.Id);

                            imageItem.ImageID = productImage.Id;
                            imageItem.OriginalPath = "";
                            imageItem.Path = imagePath.Text;
                            imageItem.Size = 0;
                            imageItem.Width = item.Width.ToString();
                            imageItem.Length = item.Length.ToString();
                            imageItem.Type = (ImageSizeType)i;
                            list.Add(imageItem);
                        }
                        baseRepository.Insert(list);
                    }
                    baseRepository.Update(productImage);
                    baseRepository.Update(dbImages);
                }
            }
            UnitOfWork.Submit();
        }

        private void SaveProductAdditionalImage(ProductImageCondition cond)
        {         
            var listImageSize = settingBLL.GetProductAdditionalImageSize();

            UnitOfWork.IsUnitSubmit = true;
            var imageID = Guid.NewGuid();
            ProductImage image = new ProductImage();
            image.Id = imageID;
            image.ProductId = cond.ProdId;
            image.Side = 0;
            image.AttrValue1 = cond.AttrValue1;
            image.AttrValue2 = cond.AttrValue2;
            image.AttrValue3 = cond.AttrValue3;
            image.Type = cond.ImageType;

            List<ProductImageList> list = new List<ProductImageList>();
            for (int i = 0; i < cond.ImagePaths.Count; i++)
            {
                var imagePath = cond.ImagePaths[i];
                var item = listImageSize[i];               
                ProductImageList imageItem = new ProductImageList();
                imageItem.Id = Guid.Parse(imagePath.Id);
               
                imageItem.ImageID = imageID;
                imageItem.OriginalPath = "";
                imageItem.Path = imagePath.Text;
                imageItem.Size = 0;
                imageItem.Width = item.Width.ToString();
                imageItem.Length = item.Length.ToString();
                imageItem.Type = (ImageSizeType)i;
                list.Add(imageItem);                
                image.ImageItems = list;              
            }

            baseRepository.Insert(image);
            UnitOfWork.Submit();
        }

        private void GenSkuImagePath(Product product, string sourcePath, List<string> delImagePaths, List<KeyValue> dbImagePaths, List<ImageSize> imageSizes, ProductImageView prodImage)
        {
            string tempPath = PathUtil.GetPhysicalPath(Globals.Configuration["UploadPath"], product.MerchantId.ToString(), FileFolderEnum.TempPath);
            string targetPath = PathUtil.GetPhysicalPath(Globals.Configuration["UploadPath"], product.MerchantId.ToString(), FileFolderEnum.Product) + "\\" + product.Id;
            string relativePath = PathUtil.GetRelativePath(product.MerchantId.ToString(), FileFolderEnum.Product) + "/" + product.Id;

            if (prodImage != null)
            {
                if (prodImage.Items != null)
                {
                    foreach (var item in prodImage.Items)
                    {
                        delImagePaths.Add(relativePath + "/" + Path.GetFileName(item.Path));
                    }
                }
            }

            if (File.Exists(tempPath + "\\" + Path.GetFileName(sourcePath)))
            {
                foreach (var item in imageSizes)
                {
                    var imageItemId = Guid.NewGuid();
                    dbImagePaths.Add(new KeyValue { Id = imageItemId.ToString(), Text = relativePath + "/" + imageItemId + Path.GetExtension(sourcePath) });
                }
            }
        }

    }
}
