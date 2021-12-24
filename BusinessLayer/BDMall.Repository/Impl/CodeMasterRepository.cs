using BDMall.Domain;
using BDMall.Enums;
using BDMall.Model;
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

        public CodeMasterRepository(IServiceProvider service) : base(service)
        {
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
                        join t in UnitOfWork.DataContext.Translations on m.DescTransId equals t.TransId into mts
                        from mt in mts.DefaultIfEmpty()
                        where m.IsActive && !m.IsDeleted
                        select new
                        {
                            m = m,
                            t = mt
                        };

            if (!string.IsNullOrEmpty(module))
            {
                query = query.Where(d => d.m.Module == module);
            }
            if (!string.IsNullOrEmpty(function))
            {
                query = query.Where(d => d.m.Function == function);
            }
            if (!string.IsNullOrEmpty(key))
            {
                query = query.Where(d => d.m.Key == key);
            }
            if (!string.IsNullOrEmpty(value))
            {
                query = query.Where(d => d.m.Value == value);
            }

            var queryGroup = query.ToList().GroupBy(g => g.m).Select(d => new { m = d.Key, Trans = d.Select(a => a.t).ToList() });


            var supportLang = GetSupportLanguage();
            foreach (var item in queryGroup)
            {
                CodeMasterDto dto = new CodeMasterDto();
                dto = AutoMapperExt.MapTo<CodeMasterDto>(item.m);
                dto.Descriptions = LangUtil.GetMutiLangFromTranslation(item.Trans, supportLang);
                dto.Description = dto.Descriptions.FirstOrDefault(d => d.Language == CurrentUser.Lang)?.Desc ?? "";
                data.Add(dto);
            }

            //if (module == CodeMasterModule.System.ToString() || function == CodeMasterFunction.SupportLanguage.ToString())
            //{
            //    CacheManager.Insert(cacheKey, data);
            //}

            return data;
        }

        public PageData<CodeMasterDto> GetCodeMastersByPage(CodeMasterCondition cond)
        {
            PageData<CodeMasterDto> result = new PageData<CodeMasterDto>();
            var query = from m in UnitOfWork.DataContext.CodeMasters
                        join t in UnitOfWork.DataContext.Translations on m.DescTransId equals t.TransId into mts
                        from mt in mts.DefaultIfEmpty()
                        select new
                        {
                            m = m,
                            t = mt
                        };

            if (cond.IsActive != -1)
            {
                bool a = cond.IsActive == 0 ? false : true;
                query = query.Where(d => d.m.IsActive == a);
            }
            if (cond.IsDeleted != -1)
            {
                bool a = cond.IsDeleted == 0 ? false : true;
                query = query.Where(d => d.m.IsDeleted == a);
            }

            if (!string.IsNullOrEmpty(cond.Module) && cond.Module != "-1")
            {
                query = query.Where(d => d.m.Module == cond.Module);
            }
            if (!string.IsNullOrEmpty(cond.Function) && cond.Function != "-1")
            {
                query = query.Where(d => d.m.Function == cond.Function);
            }
            if (!string.IsNullOrEmpty(cond.Key))
            {
                query = query.Where(d => d.m.Key == cond.Key);
            }
            if (!string.IsNullOrEmpty(cond.Value))
            {
                query = query.Where(d => d.m.Value == cond.Value);
            }

            var queryGroup = query.GroupBy(g => g.m).Select(d => new { m = d.Key, Trans = d.Select(a => a.t).ToList() });
            {
                if (!string.IsNullOrEmpty(cond.PageInfo.SortName))
                {
                    if (cond.PageInfo.SortName == "Module")
                    {
                        if (cond.PageInfo.SortOrder.ToUpper() == "DESC")
                        {
                            queryGroup = queryGroup.OrderByDescending(o => o.m.Module);
                        }
                        else
                        {
                            queryGroup = queryGroup.OrderBy(o => o.m.Module);
                        }
                    }
                    if (cond.PageInfo.SortName == "Function")
                    {
                        if (cond.PageInfo.SortOrder.ToUpper() == "DESC")
                        {
                            queryGroup = queryGroup.OrderByDescending(o => o.m.Function);
                        }
                        else
                        {
                            queryGroup = queryGroup.OrderBy(o => o.m.Function);
                        }
                    }
                    if (cond.PageInfo.SortName == "Key")
                    {
                        if (cond.PageInfo.SortOrder.ToUpper() == "DESC")
                        {
                            queryGroup = queryGroup.OrderByDescending(o => o.m.Key);
                        }
                        else
                        {
                            queryGroup = queryGroup.OrderBy(o => o.m.Key);
                        }
                    }
                    if (cond.PageInfo.SortName == "Value")
                    {
                        if (cond.PageInfo.SortOrder.ToUpper() == "DESC")
                        {
                            queryGroup = queryGroup.OrderByDescending(o => o.m.Value);
                        }
                        else
                        {
                            queryGroup = queryGroup.OrderBy(o => o.m.Value);
                        }
                    }
                    if (cond.PageInfo.SortName == "Remark")
                    {
                        if (cond.PageInfo.SortOrder.ToUpper() == "DESC")
                        {
                            queryGroup = queryGroup.OrderByDescending(o => o.m.Remark);
                        }
                        else
                        {
                            queryGroup = queryGroup.OrderBy(o => o.m.Remark);
                        }
                    }
                    if (cond.PageInfo.SortName == "IsActive")
                    {
                        if (cond.PageInfo.SortOrder.ToUpper() == "DESC")
                        {
                            queryGroup = queryGroup.OrderByDescending(o => o.m.IsActive);
                        }
                        else
                        {
                            queryGroup = queryGroup.OrderBy(o => o.m.IsActive);
                        }
                    }
                    if (cond.PageInfo.SortName == "IsDeleted")
                    {
                        if (cond.PageInfo.SortOrder.ToUpper() == "DESC")
                        {
                            queryGroup = queryGroup.OrderByDescending(o => o.m.IsDeleted);
                        }
                        else
                        {
                            queryGroup = queryGroup.OrderBy(o => o.m.IsDeleted);
                        }
                    }
                }
                else
                {
                    queryGroup = queryGroup.OrderBy(o => o.m.Key);
                }
            }



            result.TotalRecord = queryGroup.Count();

            var skipRecord = queryGroup.Skip(cond.PageInfo.Offset).Take(cond.PageInfo.PageSize).ToList();

            List<CodeMasterDto> data = new List<CodeMasterDto>();
            var supportLang = GetSupportLanguage();
            foreach (var item in skipRecord)
            {
                CodeMasterDto dto = new CodeMasterDto();
                dto = AutoMapperExt.MapTo<CodeMasterDto>(item.m);
                dto.Descriptions = LangUtil.GetMutiLangFromTranslation(item.Trans, supportLang);
                dto.Description = dto.Descriptions.FirstOrDefault(d => d.Language == CurrentUser.Lang)?.Desc ?? "";
                data.Add(dto);
            }
            result.Data = data;

            return result;
        }
    }
}
