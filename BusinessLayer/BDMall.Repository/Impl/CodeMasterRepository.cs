using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
using BDMall.Utility;
using Intimex.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Repository
{
    public class CodeMasterRepository : PublicBaseRepository, ICodeMasterRepository
    {
        public const string CacheKeyFormat = "{0}_{1}_{2}_{3}";
        ITranslationRepository _translationRpo;
        public CodeMasterRepository(IServiceProvider service) : base(service)
        {
            _translationRpo = Services.Resolve<ITranslationRepository>();
        }

        public CodeMasterDto GetCodeMaster(string module, string function, string key)
        {
            var model = GetCodeMasters(module, function, key, null)?.FirstOrDefault();
            return model;
        }

        public CodeMasterDto GetCodeMaster(string module, string function, string key, string value)
        {
            var model = GetCodeMasters(module, function, key, value)?.FirstOrDefault();
            return model;
        }


        public List<CodeMasterDto> GetCodeMasters(CodeMasterModule module, CodeMasterFunction function)
        {
            var list = GetCodeMasters(module.ToString(), function.ToString(), null, null);
            return list;
        }

        public List<CodeMasterDto> GetCodeMasters(string module, string function)
        {
            var list = GetCodeMasters(module, function, null, null);
            return list;
        }

        public List<CodeMasterDto> GetCodeMasters(Guid clientId, string module, string function, string key)
        {
            var list = GetCodeMasters(module, function, null, null);
            return list;
        }

        private List<CodeMasterDto> GetCodeMasters(string module, string function, string key, string value)
        {
            //string cacheKey = string.Format(CacheKeyFormat, module, function, key, CurrentUser.Lang.ToString());
            List<CodeMasterDto> data = new List<CodeMasterDto>();
            //if (data != null && data.Count > 0)
            //{
            //    // CacheLog.Debug("Exist " + cacheKey);
            //    return data;
            //}
            //else
            //{
            //    data = new List<CodeMasterDto>();
            //    //  CacheLog.Debug("Empty " + cacheKey);
            //}

            var query = from m in UnitOfWork.DataContext.CodeMasters
                        join t in UnitOfWork.DataContext.Translations on new { a1 = m.DescTransId, a2 = CurrentUser.Lang } equals new { a1 = t.TransId, a2 = t.Lang } into mts
                        from mt in mts.DefaultIfEmpty()
                        where m.IsActive && !m.IsDeleted
                        select new CodeMasterDto
                        {
                            Id = m.Id,
                            Function = m.Function,
                            Module = m.Module,
                            Key = m.Key,
                            Value = m.Value,
                            DescTransId = m.DescTransId,
                            Remark = m.Remark,
                            Description = mt.Value,
                            CreateDate = m.CreateDate,
                            UpdateDate = m.UpdateDate

                        };

            if (!string.IsNullOrEmpty(module))
            {
                query = query.Where(d => d.Module == module);
            }
            if (!string.IsNullOrEmpty(function))
            {
                query = query.Where(d => d.Function == function);
            }
            if (!string.IsNullOrEmpty(key))
            {
                query = query.Where(d => d.Key == key);
            }
            if (!string.IsNullOrEmpty(value))
            {
                query = query.Where(d => d.Value == value);
            }

            //var queryGroup = query.Distinct().ToList().GroupBy(g => g.m).Select(d => new { m = d.Key, Trans = d.Select(a => a.t).ToList() });


            //var supportLang = GetSupportLanguage();
            //foreach (var item in queryGroup)
            //{
            //    CodeMasterDto dto = new CodeMasterDto();
            //    dto = AutoMapperExt.MapTo<CodeMasterDto>(item.m);
            //    //dto.Descriptions = LangUtil.GetMutiLangFromTranslation(item.Trans, supportLang);
            //    dto.Description = dto.Descriptions.FirstOrDefault(d => d.Language == CurrentUser.Lang)?.Desc ?? "";
            //    data.Add(dto);
            //}
            //if (module == CodeMasterModule.System.ToString() || function == CodeMasterFunction.SupportLanguage.ToString())
            //{
            //    CacheManager.Insert(cacheKey, data);
            //}
            data = query.ToList();
            return data;
        }

        public PageData<CodeMasterDto> GetCodeMastersByPage(CodeMasterCondition cond)
        {
            PageData<CodeMasterDto> result = new PageData<CodeMasterDto>();
            var query = from m in UnitOfWork.DataContext.CodeMasters
                            //join t in UnitOfWork.DataContext.Translations on m.DescTransId equals t.TransId into mts
                            //from mt in mts.DefaultIfEmpty()
                        select m
                        ;

            if (cond.IsActive != -1)
            {
                bool a = cond.IsActive == 0 ? false : true;
                query = query.Where(d => d.IsActive == a);
            }
            if (cond.IsDeleted != -1)
            {
                bool a = cond.IsDeleted == 0 ? false : true;
                query = query.Where(d => d.IsDeleted == a);
            }

            if (!string.IsNullOrEmpty(cond.Module) && cond.Module != "-1")
            {
                query = query.Where(d => d.Module == cond.Module);
            }
            if (!string.IsNullOrEmpty(cond.Function) && cond.Function != "-1")
            {
                query = query.Where(d => d.Function == cond.Function);
            }
            if (!string.IsNullOrEmpty(cond.Key))
            {
                query = query.Where(d => d.Key == cond.Key);
            }
            if (!string.IsNullOrEmpty(cond.Value))
            {
                query = query.Where(d => d.Value == cond.Value);
            }
            if (!string.IsNullOrEmpty(cond.PageInfo.SortName))
            {
                query = query.SortBy(cond.PageInfo.SortName, cond.PageInfo.SortOrder.ToUpper().ToEnum<SortType>());
            }
            else
            {
                query = query.SortBy("Key", cond.PageInfo.SortOrder.ToUpper().ToEnum<SortType>());
            }
            

            result.TotalRecord = query.Count();

            var skipRecord = query.Skip(cond.PageInfo.Offset).Take(cond.PageInfo.PageSize).ToList();

            List<CodeMasterDto> data = new List<CodeMasterDto>();
            var supportLang = GetSupportLanguage();
            foreach (var item in skipRecord)
            {
                CodeMasterDto dto = new CodeMasterDto();
                dto = AutoMapperExt.MapTo<CodeMasterDto>(item);
                dto.Descriptions = _translationRpo.GetMutiLanguage(item.DescTransId);
                dto.Description = dto.Descriptions.FirstOrDefault(d => d.Language == CurrentUser.Lang)?.Desc ?? "";
                data.Add(dto);
            }
            result.Data = data;

            return result;
        }
    }
}
