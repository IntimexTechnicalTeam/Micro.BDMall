namespace BDMall.BLL
{
    public class PaymentBLL : BaseBLL, IPaymentBLL
    {

        ISettingBLL _settingBLL;
        ITranslationRepository _translationRepo;

        public PaymentBLL(IServiceProvider services) : base(services)
        {
            _settingBLL = Services.Resolve<ISettingBLL>();
            _translationRepo = Services.Resolve<ITranslationRepository>();
        }

        public List<PaymentMethodView> GetPaymentTypes()
        {

            var query = baseRepository.GetList<PaymentMethod>().Where(p => p.IsActive == true && p.IsDeleted == false).ToList();

            var data = query.Select(d => new PaymentMethodView()
            {
                Id = d.Id,
                Name = _translationRepo.GetTranslation(d.NameTransId).FirstOrDefault(t => t.Lang == CurrentUser.Lang)?.Value,
                Image = d.Image,//PathUtil.GetFileServer(CurrentUser.ComeFrom) + d.Image,
                Code = d.Code,
                ServRate = d.ServRate
            });
            return data.ToList();

        }

        public PaymentMethodView GetPaymentType(Guid id)
        {

            var type = baseRepository.GetModel<PaymentMethod>(p => p.Id == id);
            var data = new PaymentMethodView()
            {
                Id = type.Id,
                Name = _translationRepo.GetTranslation(type.NameTransId).FirstOrDefault(t => t.Lang == CurrentUser.Lang)?.Value,
                Image = type.Image
            };
            return data;

        }
        public string GetPaymentName(Guid id, Language lang)
        {

            var type = baseRepository.GetModel<PaymentMethod>(p => p.Id == id);
            var data = _translationRepo.GetTranslation(type.NameTransId).FirstOrDefault(t => t.Lang == lang)?.Value ?? "";
            return data;

        }




        /// <summary>
        /// 獲取支付方式列表
        /// </summary>
        /// <param name="lang"></param>
        /// <returns></returns>
        public List<PaymentMethodDto> GetPaymentMenthods()
        {

            List<PaymentMethodDto> dataList = new List<PaymentMethodDto>();
            dataList = baseRepository.GetList<PaymentMethod>().Where(p => p.IsDeleted == false).ToList().Select(p => new PaymentMethodDto()
            {
                Id = p.Id,
                BankAccount = p.BankAccount,
                Code = p.Code,
                CreateBy = p.CreateBy,
                CreateDate = p.CreateDate,
                Image = p.Image,
                ImgPath = p.Image,
                IsActive = p.IsActive,
                IsDeleted = p.IsDeleted,
                Names = _translationRepo.GetMutiLanguage(p.NameTransId),
                Remarks = _translationRepo.GetMutiLanguage(p.RemarkTransId),
                NameTransId = p.NameTransId,
                RemarkTransId = p.RemarkTransId,
                ServRate = p.ServRate,
                UpdateBy = p.UpdateBy,
                UpdateDate = p.UpdateDate
            }).ToList();
            if (dataList?.Count > 0)
            {
                foreach (var data in dataList)
                {
                    data.Name = data.Names.FirstOrDefault(d => d.Language == CurrentUser.Lang)?.Desc ?? "";
                    data.Remark = data.Remarks.FirstOrDefault(d => d.Language == CurrentUser.Lang)?.Desc ?? "";
                }
            }
            return dataList;
        }

        /// <summary>
        /// 獲取支付方式詳細信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="systenLang"></param>
        /// <returns></returns>
        public PaymentMethodDto GetPaymentMenthod(Guid id)
        {
            var langs = _settingBLL.GetSupportLanguages();

            var dbModel = baseRepository.GetModel<PaymentMethod>(p => p.Id == id);
            var viewModel = AutoMapperExt.MapTo<PaymentMethodDto>(dbModel);
            if (viewModel != null)
            {
                viewModel.ImgPath = viewModel.Image;
                viewModel.Names = _translationRepo.GetMutiLanguage(dbModel.NameTransId);
                viewModel.Name = viewModel.Names.FirstOrDefault(x => x.Language == CurrentUser.Lang)?.Desc;
                viewModel.Remarks = _translationRepo.GetMutiLanguage(dbModel.RemarkTransId);
                viewModel.Remark = viewModel.Remarks.FirstOrDefault(x => x.Language == CurrentUser.Lang)?.Desc;
            }
            else
            {
                viewModel = new PaymentMethodDto();
                viewModel.Names = LangUtil.GetMutiLangFromTranslation(null, langs);
                viewModel.Remarks = LangUtil.GetMutiLangFromTranslation(null, langs);
            }

            return viewModel;
        }

        /// <summary>
        /// 保存支付方式信息
        /// </summary>
        /// <param name="model"></param>
        public SystemResult SavePayMethodItem(PaymentMethodDto model)
        {
            Guid merchId = Guid.Empty;
            string imgPhysicalPath = PathUtil.GetPhysicalPath(Globals.Configuration["UploadPath"], merchId.ToString(), FileFolderEnum.PaymentMehod);
            string imgRelativePath = PathUtil.GetRelativePath(merchId.ToString(), FileFolderEnum.PaymentMehod);

            var newImage = model.Image;

            if (newImage != null && !newImage.Contains("/ClientResources/"))
            {
                string tempImgPath = PathUtil.GetPhysicalPath(Globals.Configuration["UploadPath"], merchId.ToString(), FileFolderEnum.TempPath);//臨時文件夾路徑

                //移動原圖
                var tempBImgPath = tempImgPath + Path.DirectorySeparatorChar + newImage;
                FileUtil.MoveFile(tempBImgPath, imgPhysicalPath, newImage);
                string newFilePath = imgRelativePath + "/" + newImage;
                model.ImgPath = newFilePath;
                model.Image = newFilePath;
            }
            else
            {
                model.Image = newImage ?? "";
                model.ImgPath = newImage ?? "";
            }

            SystemResult result = new SystemResult();
            UnitOfWork.IsUnitSubmit = true;

            if (model.Id == Guid.Empty)
            {
                InserPayMethod(model);
            }
            else
            {
                UpdatePaymentMethod(model);
            }
            UnitOfWork.Submit();

            result.Succeeded = true;
            return result;

        }

        #region Remark 上傳圖片舊邏輯

        ///// <summary>
        ///// 上传图片
        ///// </summary>
        ///// <param name="image"></param>
        ///// <returns></returns>
        //private string UpLoadImage(string image)
        //{
        //    string tempFolder = PathUtil.GetPhysicalPath(UnitOfWork.Operator.ClientId, UnitOfWork.Operator.MerchantId, FileFolderEnum.TempPath);
        //    string methodImageUrl = null;
        //    if (!string.IsNullOrEmpty(image))
        //    {
        //        string uploadedFilePath = Path.Combine(tempFolder, image);

        //        methodImageUrl = Guid.NewGuid() + Path.GetExtension(image);

        //        string imageFolder = PathUtil.GetPhysicalPath(UnitOfWork.Operator.ClientId, UnitOfWork.Operator.MerchantId, FileFolderEnum.PaymentMehod);
        //        if (File.Exists(uploadedFilePath))
        //        {
        //            if (Directory.Exists(imageFolder) == false)//如果不存在就创建file文件夹
        //            {
        //                Directory.CreateDirectory(imageFolder);
        //            }

        //            //将原图从Temp文件夹移动到newPath
        //            Intimex.Utility.FileUtil.MoveFile(uploadedFilePath, imageFolder, methodImageUrl);
        //        }
        //        string relativeFolder = PathUtil.GetRelativePath(UnitOfWork.Operator.ClientId, UnitOfWork.Operator.MerchantId, FileFolderEnum.PaymentMehod);
        //        image = PathUtil.Combine(relativeFolder, methodImageUrl);
        //    }
        //    else
        //    {
        //        image = "";
        //    }
        //    return image;
        //}

        #endregion

        /// <summary>
        /// 更新內容信息
        /// </summary>
        /// <param name="model"></param>
        private void UpdatePaymentMethod(PaymentMethodDto model)
        {
            var dbModel = baseRepository.GetModel<PaymentMethod>(p => p.Id == model.Id);

            if (dbModel != null)
            {
                _translationRepo.UpdateMutiLanguage(dbModel.RemarkTransId, GetDesc(model.Remarks), TranslationType.PaymentMethod);
                _translationRepo.UpdateMutiLanguage(dbModel.NameTransId, GetDesc(model.Names), TranslationType.PaymentMethod);
                dbModel.Image = model.ImgPath ?? string.Empty;
                dbModel.BankAccount = model.BankAccount;
                dbModel.IsActive = model.IsActive;
                dbModel.Code = model.Code;
                dbModel.ServRate = model.ServRate;
                dbModel.UpdateDate = DateTime.Now;
                dbModel.UpdateBy = Guid.Parse(CurrentUser.UserId);
                baseRepository.Update(dbModel);
            }
        }
        private List<MutiLanguage> GetDesc(List<MutiLanguage> list)
        {
            foreach (var item in list)
            {
                item.Desc = item.Desc ?? "";
            }
            return list;
        }
        /// <summary>
        /// 插入內容
        /// </summary>
        /// <param name="model"></param>
        private void InserPayMethod(PaymentMethodDto model)
        {
            var dbModel = AutoMapperExt.MapTo<PaymentMethod>(model);
            dbModel.NameTransId = _translationRepo.InsertMutiLanguage(model.Names, TranslationType.PaymentMethod);
            dbModel.RemarkTransId = _translationRepo.InsertMutiLanguage(model.Remarks, TranslationType.PaymentMethod);
            dbModel.Id = Guid.NewGuid();
            dbModel.CreateBy = Guid.Parse(CurrentUser.UserId);
            dbModel.CreateDate = DateTime.Now;
            dbModel.IsActive = true;
            dbModel.IsDeleted = false;
            dbModel.Image = model.Image ?? "";
            baseRepository.Insert(dbModel);
        }

        /// <summary>
        /// 刪除支付方式
        /// </summary>
        /// <param name="id">內容ID</param>
        public void DeletePayMethods(Guid[] id)
        {

            UnitOfWork.IsUnitSubmit = true;
            foreach (var item in id)
            {
                var pay = baseRepository.GetModel<PaymentMethod>(p => p.Id == item);

                if (pay != null)
                {
                    pay.IsDeleted = true;
                    pay.UpdateBy = Guid.Parse(CurrentUser.UserId);
                    pay.UpdateDate = new DateTime();
                    baseRepository.Update(pay);
                }
            }
            UnitOfWork.Submit();
        }



        public void DeleteMothodImage(Guid id)
        {
            var method = baseRepository.GetModel<PaymentMethod>(p => p.Id == id);

            if (method != null)
            {
                method.Image = null;
                baseRepository.Update(method);
            }

        }
    }
}
