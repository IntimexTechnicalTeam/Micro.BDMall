﻿namespace BDMall.Repository
{
    public class TranslationRepository : PublicBaseRepository, ITranslationRepository
    {
        public TranslationRepository(IServiceProvider service) : base(service)
        {
        }

        public void DeleteByTransId(Guid transId)
        {
            var data = baseRepository.GetList<Translation>().Where(d => d.TransId == transId).ToList();
            if (data.Any())
            {
                baseRepository.Delete(data);                
            }
            string key = $"{CacheKey.Translations}_E";
            RedisHelper.HDel(key, transId.ToString());

            key = $"{CacheKey.Translations}_C";
            RedisHelper.HDel(key, transId.ToString());

            key = $"{CacheKey.Translations}_S";
            RedisHelper.HDel(key, transId.ToString());
        }

        public List<MutiLanguage> GetMutiLanguage(Guid transId)
        {
            var data = new List<MutiLanguage>();
            var supportLangs = GetSupportLanguage();
            if (transId == Guid.Empty)
            {
                foreach (var supportLang in supportLangs)
                {
                    data.Add(new MutiLanguage { Desc = "", Lang = supportLang });
                }
                return data;
            }

            //var translates = baseRepository.GetList<Translation>().Where(d => d.TransId == transId && d.IsActive && !d.IsDeleted).Select(d => d).ToList();
            var translates = GetTranslation(transId);
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

        public string GetDescForLang(Guid transId, Language lang)
        {
            string key = $"{CacheKey.Translations}_{lang}";
            var trans =  RedisHelper.HGet<Translation>(key, transId.ToString());
            if (trans == null)
            {             
                trans =  baseRepository.GetModel<Translation>(d => d.TransId == transId && d.Lang == lang && d.IsActive && !d.IsDeleted);
                if (trans != null)
                     RedisHelper.HSet(key, trans.TransId.ToString(), trans);
            }         
            return trans?.Value ?? "";
        }

        public List<Translation> GetTranslation(Guid transId)
        {
            var trans = GetTranslationFromCache(transId);
            if (trans == null || !trans.Any())
            {
                trans = baseRepository.GetList<Translation>().Where(d => d.TransId == transId && d.IsActive && !d.IsDeleted).Select(d => d).ToList();
                if (trans != null && trans.Any())
                {
                    foreach (var item in trans)
                    {
                        string key = $"{CacheKey.Translations}_{item.Lang}";
                        RedisHelper.HSet(key, item.TransId.ToString(), item);
                    }
                }
            }
            return trans;
        }

        List<Translation> GetTranslationFromCache(Guid transId)
        {
            var list = new List<Translation>();

            string key = $"{CacheKey.Translations}_E";
            var trans = RedisHelper.HGet<Translation>(key, transId.ToString());
            if (trans != null)
                list.Add(trans);

            key = $"{CacheKey.Translations}_C";
            trans = RedisHelper.HGet<Translation>(key, transId.ToString());
            if (trans != null)
                list.Add(trans);

            key = $"{CacheKey.Translations}_S";
            trans = RedisHelper.HGet<Translation>(key, transId.ToString());
            if (trans != null)
                list.Add(trans);

            return list;
        }

        public Translation GetTranslation(Guid transId, Language lang)
        {
            string key = $"{CacheKey.Translations}_{lang}";
            var trans = RedisHelper.HGet<Translation>(key, transId.ToString());
            if (trans == null)
            {
                trans = baseRepository.GetModel<Translation>(d => d.TransId == transId && d.Lang == lang && d.IsActive && !d.IsDeleted);
                if (trans != null)
                    RedisHelper.HSet(key, trans.TransId.ToString(), trans);
            }
            return trans;
        }

        public Guid InsertMutiLanguage(List<MutiLanguage> items, TranslationType type)
        {
            if (items != null && items.Any())
            {
                var transId = Guid.NewGuid();
                foreach (var item in items)
                {
                    Translation trans = new Translation();
                    trans.Id = Guid.NewGuid();
                    trans.TransId = transId;
                    trans.Lang = item.Language;
                    trans.Value = item.Desc ?? string.Empty;
                    trans.Module = type.ToString();
                    baseRepository.Insert(trans);

                    string key = $"{CacheKey.Translations}_{trans.Lang}";
                    RedisHelper.HSet(key, trans.TransId.ToString(), trans);
                }
                return transId;
            }
            return Guid.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <param name="type"></param>
        /// <param name="transId"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<Translation> GenTranslations(List<MutiLanguage> items, TranslationType type, Guid transId,ActionTypeEnum actionTypeEnum = ActionTypeEnum.Add)
        {
            var list = new List<Translation>();

            foreach (var item in items)
            {
                Translation trans = new Translation();
                if (actionTypeEnum == ActionTypeEnum.Add)
                {
                    trans.Id = Guid.NewGuid();
                    trans.TransId = transId;
                    trans.Lang = item.Language;
                    trans.Value = item.Desc ?? string.Empty;
                    trans.Module = type.ToString();                                   
                }
                else if (actionTypeEnum == ActionTypeEnum.Modify)
                {
                    trans  = baseRepository.GetModel<Translation>(x => x.TransId == transId && x.Lang == item.Language);
                    trans.Value = item.Desc ?? string.Empty;
                    trans.UpdateDate = DateTime.Now;
                }

                list.Add(trans);
                string key = $"{CacheKey.Translations}_{item.Lang.Code}";
                RedisHelper.HSet(key, trans.TransId.ToString(), trans);
            }
            return list;
        }

        public Guid UpdateMutiLanguage(Guid transId, List<MutiLanguage> items, TranslationType type)
        {
            if (items != null && items.Any())
            {
                if (transId == Guid.Empty)
                {
                    return InsertMutiLanguage(items, type);
                }
                foreach (var item in items)
                {
                    Translation trans = baseRepository.GetModel<Translation>(d => d.TransId == transId && d.Lang == item.Language && d.IsActive && !d.IsDeleted);
                    if (trans == null)
                    {
                        trans = new Translation();
                        trans.Id = Guid.NewGuid();
                        trans.TransId = transId;
                        trans.Lang = item.Language;
                        trans.Value = item.Desc ?? "";
                        //  trans.Module = module;
                        baseRepository.Insert(trans);
                    }
                    else
                    {
                        trans.Value = item.Desc ?? "";
                        baseRepository.Update(trans);
                    }

                    string key = $"{CacheKey.Translations}_{ trans.Lang }";
                    RedisHelper.HSet(key, trans.TransId.ToString(), trans);
                }
                return transId;
            }
            return Guid.Empty;
        }

        public async Task<Translation> GetTranslationAsync(Guid transId, Language lang)
        {
            string key = $"{CacheKey.Translations}_{lang}";
            var trans = await RedisHelper.HGetAsync<Translation>(key, transId.ToString());
            if (trans == null)
            {
                //trans = _unitWork.DataContext.Translations.FirstOrDefault(d => d.TransId == transId && d.Lang == lang && d.IsActive && !d.IsDeleted);
                trans = await baseRepository.GetModelAsync<Translation>(d => d.TransId == transId && d.Lang == lang && d.IsActive && !d.IsDeleted);
                if (trans != null)
                    await RedisHelper.HSetAsync(key, trans.TransId.ToString(), trans);
            }

            return trans;
        }
    }
}
