namespace BDMall.BLL
{
    public class AttributeBLL : BaseBLL, IAttributeBLL
    {
        IAttributeRepository attributeRepository;
        ITranslationRepository translationRepository;
        ICodeMasterRepository codeMasterRepository;     
        IProductAttrValueRepository productAttrValueRepository;
        IProductRepository productRepository;
        IProductAttrRepository productAttrRepository;

        public AttributeBLL(IServiceProvider services) : base(services)
        {
            attributeRepository = Services.Resolve<IAttributeRepository>();
            translationRepository = Services.Resolve<ITranslationRepository>();           
            codeMasterRepository = Services.Resolve<ICodeMasterRepository>();
            productAttrValueRepository = Services.Resolve<IProductAttrValueRepository>();
            productRepository = Services.Resolve<IProductRepository>();
            productAttrRepository = Services.Resolve<IProductAttrRepository>();
        }

        public List<KeyValue> GetInveAttribute()
        {
            var inveAttributes = (from e in baseRepository.GetList<ProductAttribute>()
                         join t in baseRepository.GetList<Translation>() on new { a1 = e.DescTransId, a2 = CurrentUser.Lang } equals new { a1 = t.TransId, a2 = t.Lang } into tc
                         from tt in tc.DefaultIfEmpty()
                         where e.IsActive && !e.IsDeleted && e.IsInvAttribute
                         select new KeyValue
                         {
                             Id = e.Id.ToString().ToLower(),
                             Text = tt.Value
                         }).ToList();

            return inveAttributes;
        }

        public List<KeyValue> GetNonInveAttribute()
        {           
            var nonInveAttributes = (from e in baseRepository.GetList<ProductAttribute>()
                                     join t in baseRepository.GetList<Translation>() on new { a1 = e.DescTransId, a2 = CurrentUser.Lang } equals new { a1 = t.TransId, a2 = t.Lang } into tc
                                     from tt in tc.DefaultIfEmpty()
                                     where e.IsActive && !e.IsDeleted && !e.IsInvAttribute
                                     select new KeyValue
                                     {
                                         Id = e.Id.ToString().ToLower(),
                                         Text = tt.Value
                                     }).ToList();

            return nonInveAttributes;
        }

        public List<KeyValue> GetAttrLayout()
        {
            List<KeyValue> list = new List<KeyValue>();
            var attrLayouts = codeMasterRepository.GetCodeMasters(CodeMasterModule.System, CodeMasterFunction.AttrLayout);
            if (attrLayouts == null || !attrLayouts.Any())
            {
                list.Add(new KeyValue { Id = AttrLayout.Select.ToInt().ToString(), Text = AttrLayout.Select.ToString() });
                list.Add(new KeyValue { Id = AttrLayout.List.ToInt().ToString(), Text = AttrLayout.List.ToString() });
                return list;
            }

            list = attrLayouts.Select(item => new KeyValue { Id = item.Value, Text = item.Description }).ToList();
            return list;
        }

        public List<ProductAttributeValueDto> GetInveAttributeValueSummary()
        {
            var attrValues = (from d in UnitOfWork.DataContext.ProductAttributes
                              join v in UnitOfWork.DataContext.ProductAttributeValues on d.Id equals v.AttrId
                              join t in UnitOfWork.DataContext.Translations on new { a1 = v.DescTransId, a2 = CurrentUser.Lang } equals new { a1 = t.TransId, a2 = t.Lang } into tc
                              from tt in tc.DefaultIfEmpty()
                              where d.IsInvAttribute && !d.IsDeleted && d.IsActive
                              select new ProductAttributeValueDto
                              {
                                  AttrId = v.AttrId,
                                  Id = v.Id,
                                  Desc = tt.Value,
                                  Code = v.Code,
                                  MerchantId = v.MerchantId,
                                  Image = v.Image,
                                  DescTransId = v.DescTransId,

                              }).ToList();

            //foreach (var item in attrValues)
            //{
            //    item.MerchantName = baseRepository.GetModelById<Merchant>(item.MerchantId).n;
            //}
            return attrValues;
        }

        public PageData<ProductAttributeDto> SearchAttribute(ProductAttributeCond attrCond)
        {
            var list = attributeRepository.SearchAttribute(attrCond);
            foreach (var item in list.Data)
            {
                 GenProductAttribute(item);
            }
            return list;
        }

        public ProductAttributeDto GetAttribute(Guid id)
        {
            if (id == Guid.Empty)
            { 
                var attr = new ProductAttributeDto() {  };
                attr.Descs =translationRepository.GetMutiLanguage(Guid.Empty);
                return attr;
            }

            var dbAttribute = attributeRepository.GetAttribute(id);
            var attribute = AutoMapperExt.MapTo<ProductAttributeDto>(dbAttribute);
            attribute = GenProductAttribute(attribute);
            return attribute;
        }

        public bool CheckAttrIsUsed(string ids)
        {
            var attrIds = ids.Split(',').Select(s=>Guid.Parse(s)).ToList();
            var flag = baseRepository.Any<ProductCatalogAttr>(x => x.IsActive && !x.IsDeleted && attrIds.Contains(x.AttrId));
            return flag;            
        }

        public SystemResult DeleteAttribute(Guid[] ids)
        {
            SystemResult result = new SystemResult();
            var AttributeValues = new List<ProductAttributeValue>();
            var attrValueList = new List<ProductAttrValue>();
            var proAttrList = new List<ProductAttribute>();
            foreach (var item in ids)
            {                
                var attribute = baseRepository.GetModelById<ProductAttribute>(item);
                if (attribute == null) throw new BLException(Resources.Message.NoRecord);

                var hasInvRecord = productAttrValueRepository.CheckHasInvRecordByAttrValueId(attribute.Id);
                if (attribute.IsInvAttribute  && hasInvRecord) throw new BLException(Resources.Message.ProductInStock);

                //是非库存属性，或者没有库存记录时才能删除属性
                attribute.IsDeleted = true;
                attribute.UpdateDate= DateTime.Now;
                proAttrList.Add(attribute);

                AttributeValues = baseRepository.GetList<ProductAttributeValue>(x=>x.AttrId == attribute.Id).ToList();               
                foreach (var value in AttributeValues)
                {
                    value.IsDeleted = true;
                    value.UpdateDate = DateTime.Now;
                }

                var attrProducts = productRepository.GetProductByAttrValueId(attribute.Id);
                foreach (var product in attrProducts)
                {
                    var prodAttrList = baseRepository.GetList<ProductAttr>(x => x.ProductId == product.Id).ToList();
                    var attrValue = baseRepository.GetModel<ProductAttrValue>(x => prodAttrList.Select(s => s.Id).Contains(x.ProdAttrId) && x.AttrValueId == attribute.Id);
                    attrValue.IsDeleted = true;
                    attrValue.UpdateDate = DateTime.Now;
                    attrValueList.Add(attrValue);
                }
            }

            using var tran = baseRepository.CreateTransation();
            baseRepository.Update(proAttrList);
            baseRepository.Update(attrValueList);
            //baseRepository.Update(AttributeValues);

            tran.Commit();
            result.Succeeded = true;

            return result;
        }

        public ProductAttributeValueDto GetAttributeValue(Guid id)
        {
            if (id == Guid.Empty)
            { 
                var attrValue =new ProductAttributeValueDto() { };  
                attrValue.Descs = translationRepository.GetMutiLanguage(Guid.Empty);
                return attrValue;
            }

            var dbAttributeValue = baseRepository.GetModelById<ProductAttributeValue>(id);
            var attributeValue = AutoMapperExt.MapTo<ProductAttributeValueDto>(dbAttributeValue);               
            attributeValue = GenProductAttrValue(attributeValue);           
            return attributeValue;
        }

        public SystemResult Save(ProductAttributeDto attributeObj)
        { 
            var result = new SystemResult();

            foreach (var item in attributeObj.AttributeValues)
            {
                if (item.Status == RecordStatus.Add) item.Id = Guid.NewGuid();
                //item.ImagePath = item.Image;                            //临时目录
                item.Image = GetAttributeImage(item);               
            }

            ProductAttributeDto attributeOld = null;
            if (attributeObj.Id == Guid.Empty)
                result = InsertAttribute(attributeObj);
            else
            {
                attributeOld = GetAttribute(attributeObj.Id);
                result = UpdateAttribute(attributeObj);
            }

            //处理图片
            if (result.Succeeded)
            {               
                var updateValues = attributeObj.AttributeValues.Where(p => p.Status == RecordStatus.Update).ToList();//更新的属性值列表
                var insertValues = attributeObj.AttributeValues.Where(p => p.Status == RecordStatus.Add).ToList();//新增的属性值列表
                var deleteValues = attributeObj.AttributeValues.Where(p=>p.Status == RecordStatus.Delete).ToList(); 

                foreach (var item in insertValues)
                {
                    GenAttributeImage(item.ImagePath, item.Id.ToString());
                }
                foreach (var item in updateValues)
                {
                    GenAttributeImage(item.ImagePath, item.Id.ToString());
                }

                string path = PathUtil.GetPhysicalPath(Globals.Configuration["UploadPath"], CurrentUser.MerchantId.ToString(), FileFolderEnum.Attribute);
                foreach (var item in deleteValues)
                {
                    string targetPath = Path.Combine(path, Path.GetFileName(item.Image));
                    if (File.Exists(targetPath)) 
                        FileUtil.DeleteFile(targetPath);
                }
            }

            return result;
        }

        public List<AttributeObjectView> GetInvAttributeByCatId(Guid catId)
        {
            List<AttributeObjectView> list = new List<AttributeObjectView>();
            var dbAttrs = attributeRepository.GetAttributeItemsByCatID(catId).Where(p => p.IsInvAttribute == true).ToList();
            list = GenAttributeObjectView(dbAttrs);
            return list;
        }

        public List<AttributeObjectView> GetNonInvAttributeByCatId(Guid catId)
        {
            List<AttributeObjectView> list = new List<AttributeObjectView>();       
            var dbAttrs = attributeRepository.GetAttributeItemsByCatID(catId).Where(p => p.IsInvAttribute == false).ToList();
            list = GenAttributeObjectView(dbAttrs);
            return list;
        }

        public List<AttributeObjectView> GetNonInvAttributeByProduct(Guid prodId)
        {
            List<AttributeObjectView> list = new List<AttributeObjectView>();           
            var dbAttrs = attributeRepository.GetAttributeItemsByProductId(prodId).Where(p => p.IsInvAttribute == false).ToList();
            list = GenAttributeObjectView(dbAttrs);
            return list;
        }

        public List<AttributeObjectView> GetInvAttributeByProduct(Guid prodId)
        {
            List<AttributeObjectView> list = new List<AttributeObjectView>();
            var mapAttrs = productAttrRepository.GetAttributeItemsMappByProductId(prodId).Where(p => p.IsInv == true).OrderBy(o => o.Seq).ToList();
            var produt = baseRepository.GetModel<Product>(x => x.Id == prodId);

            if (mapAttrs != null && mapAttrs.Any())
            {
                foreach (var item in mapAttrs)
                {
                    var dbAttr = baseRepository.GetModel<ProductAttribute>(x => x.Id == item.AttrId);
                    var attr = AutoMapperExt.MapTo<ProductAttributeDto>(dbAttr);
                    var modelAttr = GenProductAttribute(attr);
                    AttributeObjectView obj = new AttributeObjectView();
                    obj.Id = modelAttr.Id;
                    obj.Desc = modelAttr.Desc;
                    obj.SubItems = modelAttr.AttributeValues.Where(x => x.IsActive && !x.IsDeleted && (x.MerchantId == produt.MerchantId || x.MerchantId == Guid.Empty))
                                            .Select(s => new AttributeValueView
                                            {
                                                Id = s.Id.ToString(),
                                                Text = s.Desc,
                                                Price = item.AttrValues.FirstOrDefault(p => p.AttrValueId == s.Id)?.AdditionalPrice ?? 0
                                            }).ToList();
                    list.Add(obj);
                }
            }

            return list;
        }

        public List<AttributeObjectView> GetInvAttributeByProductWithMap(Guid prodId)
        {
            List<AttributeObjectView> list = new List<AttributeObjectView>();

            var mapAttrs = productAttrRepository.GetAttributeItemsMappByProductId(prodId).Where(p => p.IsInv == true).OrderBy(o => o.Seq).ToList();
            var dbAttrs = attributeRepository.GetAttributeItemsByProductId(prodId).Where(p => p.IsInvAttribute == true).ToList();

            if (dbAttrs != null && dbAttrs.Any())
            {
                var attrs = AutoMapperExt.MapTo<List<ProductAttributeDto>>(dbAttrs);
                foreach (var item in mapAttrs)
                {
                    var attr = attrs.FirstOrDefault(p => p.Id == item.AttrId);
                    if (attr != null)
                    {
                        var modelAttr = GenProductAttribute(attr);
                        AttributeObjectView obj = new AttributeObjectView();
                        obj.Id = modelAttr.Id;
                        obj.Desc = modelAttr.Desc;                     
                        obj.SubItems = modelAttr.AttributeValues.Where(x => x.IsActive && !x.IsDeleted && x.AttrId ==attr.Id)
                                            .Select(s => new AttributeValueView
                                            {
                                                Id = s.Id.ToString(),
                                                Text = s.Desc,
                                                Price = item.AttrValues.FirstOrDefault(p => p.AttrValueId == s.Id)?.AdditionalPrice ?? 0
                                            }).ToList();

                        list.Add(obj);
                    }
                }
            }
            return list;
        }

        public List<ProdAtt> GetFrontAttribute(ProdAttCond cond)
        {
            List<ProdAtt> list = new List<ProdAtt>();
             
            var attrs = (from a in baseRepository.GetList<ProductAttribute>()
                         join m in baseRepository.GetList<ProductCatalogAttr>() on a.Id equals m.AttrId
                         where a.IsActive == true && a.IsDeleted == false                        
                         select new { a,m.CatalogId}
                        );

            if (!cond.CatalogIds?.Any() ?? false)
                attrs = attrs.Where(x => cond.CatalogIds.Contains(x.CatalogId));

            if (cond.Type == 1)          
                attrs = attrs.Where(p => p.a.IsInvAttribute == true);            
            else if (cond.Type == 2)
                attrs = attrs.Where(p => p.a.IsInvAttribute == false);
            
            list = (from d in attrs
                    join t in baseRepository.GetList<Translation>() on new { a1 = d.a.DescTransId, a2 = CurrentUser.Lang } equals new { a1 = t.TransId, a2 = t.Lang }
                    into tc
                    from tt in tc.DefaultIfEmpty()
                    select new ProdAtt
                    {
                        Id = d.a.Id,
                        Layout = d.a.Layout,
                        Name = tt.Value,
                    }).ToList();
            foreach (var item in list)
            {

                var imagePerfix = string.Empty;
                var attrValues = (from d in baseRepository.GetList <ProductAttributeValue>()
                                  join t in baseRepository.GetList<Translation>() on new { a1 = d.DescTransId, a2 = CurrentUser.Lang } equals new { a1 = t.TransId, a2 = t.Lang } into tc
                                  from tt in tc.DefaultIfEmpty()
                                  where d.AttrId == item.Id
                                  && d.IsActive == true && d.IsDeleted == false
                                  select new ProdAttValue
                                  {
                                      Id = d.Id,
                                      Name = tt.Value,
                                      //AddPrice = d.AddPrice,
                                      Image = d.Image
                                  }).ToList();

                foreach (var attrValue in attrValues)
                {
                    if (!string.IsNullOrEmpty(attrValue.Image))
                    {
                        attrValue.Image = imagePerfix + attrValue.Image;
                    }
                }

                item.Values = attrValues;
            }
            return list;
        }

        private List<AttributeObjectView> GenAttributeObjectView(List<ProductAttribute> dbAttrs)
        {
            List<AttributeObjectView> list = new List<AttributeObjectView>();

            if (dbAttrs != null && dbAttrs.Any())
            {
                var attrs = AutoMapperExt.MapTo<List<ProductAttributeDto>>(dbAttrs);
                foreach (var item in attrs)
                {
                    var modelAttr = GenProductAttribute(item);
                    AttributeObjectView obj = new AttributeObjectView();
                    obj.Id = modelAttr.Id;
                    obj.Desc = modelAttr.Desc;
                    obj.SubItems = modelAttr.AttributeValues.Where(x => !x.IsDeleted && (x.MerchantId == CurrentUser.MerchantId || x.MerchantId == Guid.Empty))
                                                .Select(s => new AttributeValueView { Id = s.Id.ToString(), Text = s.Desc }).ToList();

                    list.Add(obj);
                }
            }

            return list;
        }

        public ProductAttributeDto GenProductAttribute(ProductAttributeDto attribute)
        {         
            attribute.Descs = translationRepository.GetMutiLanguage(attribute.DescTransId);
            attribute.Layout = attribute.Layout;
            attribute.Desc = attribute.Descs.FirstOrDefault(x=>x.Language == CurrentUser.Lang)?.Desc ?? "";
            var attrValuelist = baseRepository.GetList<ProductAttributeValue>(x => x.AttrId == attribute.Id && x.IsActive && !x.IsDeleted);
            //if (CurrentUser.IsMerchant)
            //    attrValuelist = attrValuelist.Where(x => x.MerchantId == CurrentUser.MerchantId);

            attribute.AttributeValues = AutoMapperExt.MapTo<List<ProductAttributeValueDto>>(attrValuelist.ToList());
            foreach (var item in attribute.AttributeValues)
            {
                var mch = baseRepository.GetModelById<Merchant>(item.MerchantId);    
                item.MerchantName = translationRepository.GetDescForLang(mch?.NameTransId ?? Guid.Empty, CurrentUser.Lang) ?? "";
                item.Descs = translationRepository.GetMutiLanguage(item.DescTransId);
                item.ImagePath = item.Image;
                item.Status = item.MerchantName == string.Empty ? RecordStatus.Add : RecordStatus.Update;
                item.Desc = item.Descs.FirstOrDefault(x=>x.Language== CurrentUser.Lang)?.Desc ?? string.Empty;
            }
            return attribute;
        }

        public ProductAttributeDto GetProductAttribute(Guid attrId, Language lang)
        {
            var dbAttribute = baseRepository.GetModel<ProductAttribute>(p => p.Id == attrId && !p.IsDeleted  && p.IsActive);
            if (dbAttribute == null) return null;
            var attribute = AutoMapperExt.MapTo<ProductAttributeDto>(dbAttribute);
            return GenProductAttribute(attribute);
        }

        public InvAttributeLst GetInvAttributeItemsByCatID(Guid catID)
        {
            InvAttributeLst inven = new InvAttributeLst();
            var attr1 = new List<KeyValue>();
            var attr2 = new List<KeyValue>();
            var attr3 = new List<KeyValue>();

            var dbAttributes = attributeRepository.GetAttributeItemsByCatID(catID).Where(p => p.IsInvAttribute == true).ToList();
            var attributes = AutoMapperExt.MapTo<List<ProductAttributeDto>>(dbAttributes);

            if (attributes != null && attributes.Any())
            {
                var supportLangs = GetSupportLanguage();
                var emptyLangs = LangUtil.GetMutiLangFromTranslation(null, supportLangs);

                foreach (var attribute in attributes)
                {                   
                    var dbAttValues = baseRepository.GetList<ProductAttributeValue>(x => x.AttrId == attribute.Id && x.IsActive && !x.IsDeleted).ToList();
                    attribute.AttributeValues = AutoMapperExt.MapTo<List<ProductAttributeValueDto>>(dbAttValues);
                    attribute.Descs = translationRepository.GetMutiLanguage(attribute.DescTransId) ??  emptyLangs;                   
                    attribute.Desc = attribute.Descs.FirstOrDefault(x => x.Language == CurrentUser.Lang)?.Desc;
                }
                {
                    //if (attributes[0].AttributeValues != null && attributes[0].AttributeValues.Any())
                    //{
                    //    foreach (var item in attributes[0].AttributeValues)
                    //    {
                    //        item.Descs = translationRepository.GetMutiLanguage(item.DescTransId) ?? emptyLangs;
                    //        item.Desc = item.Descs.FirstOrDefault(x => x.Language == CurrentUser.Lang)?.Desc;

                    //        attr1.Add(new KeyValue { Id = item.Id.ToString(), Text = item.Desc });
                    //    }
                    //}

                    //if (attributes.Count > 1 && attributes[1].AttributeValues != null && attributes[1].AttributeValues.Any())
                    //{
                    //    foreach (var item in attributes[1].AttributeValues)
                    //    {
                    //        item.Descs = translationRepository.GetMutiLanguage(item.DescTransId) ?? emptyLangs;                        
                    //        item.Desc = item.Descs.FirstOrDefault(x => x.Language == CurrentUser.Lang)?.Desc;

                    //        attr2.Add(new KeyValue { Id = item.Id.ToString(), Text = item.Desc });
                    //    }
                    //}

                    //if (attributes.Count > 2 && attributes[2].AttributeValues != null && attributes[2].AttributeValues.Any())
                    //{
                    //    foreach (var item in attributes[2].AttributeValues)
                    //    {
                    //        item.Descs = translationRepository.GetMutiLanguage(item.DescTransId) ?? emptyLangs;                      
                    //        item.Desc = item.Descs.FirstOrDefault(x => x.Language == CurrentUser.Lang)?.Desc;

                    //        attr3.Add(new KeyValue { Id = item.Id.ToString(), Text = item.Desc });
                    //    }
                    //}
                }

                inven.AttrIList = GenAttrOption(attributes);
                inven.AttrIIList = GenAttrOption(attributes,1);
                inven.AttrIIIList = GenAttrOption(attributes,2);
            }

            return inven;
        }

        public List<ProdAtt> GetInvAttributeByProductWithMapForFront(Guid prodId)
        {
            List<ProdAtt> list = new List<ProdAtt>();

            var mapAttrs = productAttrRepository.GetAttributeItemsMappByProductId(prodId).Where(p => p.IsInv == true).OrderBy(o => o.Seq).ToList();
            var dbAttrs = attributeRepository.GetAttributeItemsByProductId(prodId).Where(p => p.IsInvAttribute == true).ToList();

            if (dbAttrs?.Any() ?? false)
            {
                var attrs = AutoMapperExt.MapTo<List<ProductAttributeDto>>(dbAttrs);
                foreach (var item in mapAttrs)
                {
                    var attr = attrs.FirstOrDefault(p => p.Id == item.AttrId);
                    if (attr != null)
                    {
                        var modelAttr = GenProductAttribute(attr);
                        ProdAtt obj = new ProdAtt();
                        obj.Id = modelAttr.Id;
                        obj.Name = modelAttr.Desc;
                        obj.Values = new List<ProdAttValue>();
                        obj.Layout = modelAttr.Layout;
                        obj.Seq = item.Seq;
                        foreach (var attrValue in modelAttr.AttributeValues)
                        {
                            var mapAttrValue = item.AttrValues.FirstOrDefault(p => p.AttrValueId == attrValue.Id);
                            if (mapAttrValue != null)
                            {
                                obj.Values.Add(new ProdAttValue
                                {
                                    Id = attrValue.Id,
                                    Name = attrValue.Desc,
                                    Image = attrValue.Image,
                                    //Image = attrValue.Image,
                                    AddPrice = mapAttrValue.AdditionalPrice// TODO
                                });
                            }
                        }

                        if (obj.Values.Count() > 0)//沒有屬性值，不顯示屬性
                        {
                            list.Add(obj);
                        }
                    }
                }
            }

            return list;
        }

        public async Task<List<ProdAtt>> GetFrontAttributeAsync(ProdAttCond cond)
        {         
            cond.Type = 2;

            string key = CacheKey.ProdAttr.ToString();
            string field = $"{CacheField.Atts}_{cond.Type}_{CurrentUser.Lang}";
            var data = await RedisHelper.HGetAsync<List<ProdAtt>>(key,field);
            if (!data?.Any() ?? false)
            {
                data = GetFrontAttribute(cond);
                await RedisHelper.HSetAsync(key,field,data);
            }
            return data;
        }

        private ProductAttributeValueDto GenProductAttrValue(ProductAttributeValueDto attributeValue)
        {
            attributeValue.Descs = translationRepository.GetMutiLanguage(attributeValue.DescTransId);
            attributeValue.Desc = attributeValue.Descs.FirstOrDefault(x=>x.Language == CurrentUser.Lang)?.Desc ?? string.Empty;
            attributeValue.Status = attributeValue.Desc == string.Empty ? RecordStatus.Add : RecordStatus.Update;
            return attributeValue;
        }

        /// <summary>
        /// 生成相对路径
        /// </summary>
        /// <param name="attrValue"></param>
        /// <returns></returns>
        private string GetAttributeImage(ProductAttributeValueDto attrValue)
        {
            var image = Path.GetFileName(attrValue.Image);

            if (image.IsEmpty()) return string.Empty;

            string tempPath = PathUtil.GetPhysicalPath(Globals.Configuration["UploadPath"], CurrentUser.MerchantId.ToString(), FileFolderEnum.TempPath);
            if (File.Exists(Path.Combine(tempPath, image)))
            {
                string relativePath = PathUtil.GetRelativePath(CurrentUser.MerchantId.ToString(), FileFolderEnum.Attribute);

                string extension = Path.GetExtension(image);
                string fileName = attrValue.Id.ToString() + extension;

                return relativePath + "/" + fileName;
            }
            else
            {
                string oldRelativePath = PathUtil.GetRelativePath(attrValue.MerchantId.ToString(), FileFolderEnum.Attribute);
                return oldRelativePath + "/" + image;
            }
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="tempImageName"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GenAttributeImage(string tempImageName, string name)
        {
            string relativePath = PathUtil.GetRelativePath(CurrentUser.MerchantId.ToString(), FileFolderEnum.Attribute);

            string tempPath = PathUtil.GetPhysicalPath(Globals.Configuration["UploadPath"], CurrentUser.MerchantId.ToString(), FileFolderEnum.TempPath);

            string path = PathUtil.GetPhysicalPath(Globals.Configuration["UploadPath"], CurrentUser.MerchantId.ToString(), FileFolderEnum.Attribute);

            string tmpFile = Path.Combine(tempPath, Path.GetFileName(tempImageName));
            if (File.Exists(tmpFile))
            {
                string extension = Path.GetExtension(tempImageName);
                string fileName = name + extension;
                FileUtil.MoveFile(tmpFile, path, fileName);

                return relativePath + "/" + fileName;
            }
            else
            {
                return relativePath + "/" + tempImageName;
            }
        }

        /// <summary>
        /// 插入属性信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private SystemResult InsertAttribute(ProductAttributeDto obj)
        {
            SystemResult result = new SystemResult();

            List<ProductAttributeValue> attrValues = new List<ProductAttributeValue>();          
            var attrId = Guid.NewGuid();
            var attrTransId = Guid.NewGuid();
            var TranList = translationRepository.GenTranslations(obj.Descs, TranslationType.Attribute, attrTransId);
            obj.DescTransId = attrTransId;
            obj.Id = attrId;

            foreach (var item in obj.AttributeValues)//遍历属性下的属性值
            {
                var attrValueLangId = Guid.NewGuid();
                var attrValueTransList = translationRepository.GenTranslations(item.Descs, TranslationType.AttributeValue, attrValueLangId);
                item.AttrId = attrId;
                if (item.Id == Guid.Empty)
                {
                    item.Id = Guid.NewGuid();
                }
                item.DescTransId = attrValueLangId;
                item.MerchantId = CurrentUser.MerchantId;
                TranList.AddRange(attrValueTransList);
            }

            attrValues = AutoMapperExt.MapTo<List<ProductAttributeValue>>(obj.AttributeValues);
            var dbAttr = AutoMapperExt.MapTo<ProductAttribute>(obj);
            dbAttr.CreateDate = DateTime.Now;
            dbAttr.CreateBy = Guid.Parse(CurrentUser.UserId);

            using var tran = baseRepository.CreateTransation();

            baseRepository.Insert(dbAttr);
            baseRepository.Insert(TranList);
            baseRepository.Insert(attrValues);

            tran.Commit();
            result.ReturnValue = obj.Id;
            result.Succeeded = true;

            return result;
        }

        /// <summary>
        /// 保存屬性修改
        /// </summary>
        /// <param name="obj">屬性對象</param>
        private SystemResult UpdateAttribute(ProductAttributeDto obj)
        {
            SystemResult result = new SystemResult();

            List<string> newImages = new List<string>();
            List<string> oldImages = new List<string>();

            var transInsertList = new List<Translation>();
            var newProAttrList = new List<ProductAttributeValue>();
            var updateProAttrList = new List<ProductAttributeValue>();
            var attrValueList = new List<ProductAttrValue>();
            var transUpdateLists = translationRepository.GenTranslations(obj.Descs, TranslationType.Attribute, obj.DescTransId, ActionTypeEnum.Modify);

            var updateValues = obj.AttributeValues.Where(p => p.Status == RecordStatus.Update).ToList();//更新的属性值列表
            var insertValues = obj.AttributeValues.Where(p => p.Status == RecordStatus.Add).ToList();//新增的属性值列表
            var deleteValues = obj.AttributeValues.Where(p => p.Status == RecordStatus.Delete).ToList();//删除的属性值列表

            var dbAttr = AutoMapperExt.MapTo<ProductAttribute>(obj);
            dbAttr.UpdateDate = DateTime.Now;
            dbAttr.UpdateBy =Guid.Parse(CurrentUser.UserId);

            foreach (var item in updateValues)//遍历属性下的修改的属性值
            {
                item.MerchantId = CurrentUser.MerchantId;
                item.UpdateBy = Guid.Parse(CurrentUser.UserId);
                item.UpdateDate = DateTime.Now;
                var valueList = translationRepository.GenTranslations(item.Descs, TranslationType.AttributeValue, item.DescTransId, ActionTypeEnum.Modify);
                transUpdateLists.AddRange(valueList);
            }
            updateProAttrList = AutoMapperExt.MapTo<List<ProductAttributeValue>>(updateValues);

            //2--如果有新属性
            if (insertValues !=null && insertValues.Any())//需要新增属性值时判断该属性是否有产品和是否有库存
            {
                var attrProducts = productRepository.GetProductByAttrId(obj.Id);
                List<ProductAttrValue> productAttrValues = new List<ProductAttrValue>();
                List<ProductSku> productSkus = new List<ProductSku>();
                foreach (var item in insertValues)//遍历属性下的新增的属性值
                {
                    var attrValueLangId = Guid.NewGuid();
                    var newTranList = translationRepository.GenTranslations(item.Descs, TranslationType.AttributeValue, attrValueLangId);
                    item.AttrId = obj.Id;
                    item.DescTransId = attrValueLangId;
                    item.MerchantId = CurrentUser.MerchantId;

                    transInsertList.AddRange(newTranList);
                  
                }
                //_attributeValueRepository.Insert(insertValues); insert  transInsertList
                newProAttrList = AutoMapperExt.MapTo<List<ProductAttributeValue>>(insertValues);
            }

            //3--如果有删除的属性
            foreach (var item in deleteValues)//遍历属性下的已经删除的属性值
            {
                var attrProducts = productRepository.GetProductByAttrValueId(item.Id);
                var hasInvRecord = productAttrValueRepository.CheckHasInvRecordByAttrValueId(item.Id);

                if (obj.IsInvAttribute && hasInvRecord) throw new BLException(Resources.Message.ProductInStock);

                item.IsDeleted = true;
                item.UpdateBy =Guid.Parse(CurrentUser.UserId);
                foreach (var product in attrProducts)
                {
                    var prodAttrList = baseRepository.GetList<ProductAttr>(x => x.ProductId == product.Id).ToList();
                    var attrValue = baseRepository.GetModel<ProductAttrValue>(x => prodAttrList.Select(s => s.Id).Contains(x.ProdAttrId) && x.AttrValueId == item.Id);
                    attrValue.IsDeleted = true;
                    attrValue.UpdateDate = DateTime.Now;
                    attrValueList.Add(attrValue);

                }
            }
            var deleteList = AutoMapperExt.MapTo<List<ProductAttributeValue>>(deleteValues);
            var delTranList = baseRepository.GetList<Translation>(x=>deleteList.Select(s=>s.DescTransId).Contains(x.TransId)).ToList();
           
            using var tran = baseRepository.CreateTransation();

            baseRepository.Update(dbAttr);
            baseRepository.Update(updateProAttrList);
            baseRepository.Insert(transInsertList);
            baseRepository.Insert(newProAttrList);
            baseRepository.Update(attrValueList);
            baseRepository.Update(transUpdateLists);
            baseRepository.Update(deleteList);
            baseRepository.Delete(delTranList);    //硬删除

            tran.Commit();
 
            result.Succeeded = true;
            result.ReturnValue = obj.Id;

            return result;

        }

        private List<KeyValue> GenAttrOption(List<ProductAttributeDto> attributes, int index = 0)
        {
            var result = new List<KeyValue>();

            if (index < 0) return result;

            if ((index == 0 || attributes.Count > index) && attributes[index].AttributeValues != null && attributes[index].AttributeValues.Any())
            {
                result = attributes[index].AttributeValues.Select(s => new KeyValue { 
                            Id = s.Id.ToString(), 
                            Text = translationRepository.GetDescForLang(s.DescTransId, CurrentUser.Lang) })
                .ToList();
            }

            return result;
        }
    }
}
