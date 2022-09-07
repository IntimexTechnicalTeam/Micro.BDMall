namespace BDMall.Repository
{
    public class RoleRepository : PublicBaseRepository, IRoleRepository
    {
        ITranslationRepository translationRepository;

        public RoleRepository(IServiceProvider service) : base(service)
        {
            translationRepository =Services.Resolve<ITranslationRepository>();
        }

        public RoleDto GetRoleByEager(Guid id)
        {
            var dbRole =baseRepository.GetModelById<Role>(id);
            var role = AutoMapperExt.MapTo<RoleDto>(dbRole);

            var dbRolePermission = baseRepository.GetList<RolePermission>(x=>x.RoleId == id).ToList();
            role.RolePermission = AutoMapperExt.MapTo <List<RolePermissionDto>>(dbRolePermission);

            role.FullNames =baseRepository.GetList<Translation>().Where(d => d.TransId == role.FullNameTransId).Select(d => new MutiLanguage()
            {
                Desc = d.Value,
                Language = d.Lang
            }).ToList();
            role.Remarks = baseRepository.GetList<Translation>().Where(d => d.TransId == role.RemarkTransId).Select(d => new MutiLanguage()
            {
                Desc = d.Value,
                Language = d.Lang
            }).ToList();
            role.DisplayName = role.FullNames?.FirstOrDefault(d => d.Language == CurrentUser.Lang)?.Desc;
            return role;

        }

        public List<RoleDto> Search(RoleCondition cond)
        {
            if (cond == null)
            {
                throw new ArgumentNullException("查询条件不能为空。");
            }
            var joinQuery = (from r in baseRepository.GetList<Role>()
                            join t in baseRepository.GetList<Translation>() on r.FullNameTransId equals t.TransId into rts
                            from rt in rts.DefaultIfEmpty()
                            join t2 in baseRepository.GetList<Translation>() on new { a = r.RemarkTransId, b = rt.Lang } equals new { a = t2.TransId, b = t2.Lang } into rt2s
                            from rt2 in rt2s.DefaultIfEmpty()
                            where rt2.Lang == cond.Language || rt.Lang == cond.Language
                            select new
                            {
                                a = r,
                                b = rt,
                                c = rt2
                            }).ToList();

            var query = from q in joinQuery
                        group q by q.a into grt
                        select new 
                        {                           
                            r = new RoleDto {
                                 Id = grt.Key.Id,
                                IsActive = grt.Key.IsActive,
                                IsSystem = grt.Key.IsSystem,
                                IsDeleted = grt.Key.IsDeleted,
                                Name = grt.Key.Name,
                            },
                            ts = grt.Select(d => new MutiLanguage() { Desc = d.b != null ? d.b.Value : "", Language = d.b != null ? d.b.Lang : Language.E }),
                            Rs = grt.Select(d => new MutiLanguage() { Desc = d.c != null ? d.c.Value : "", Language = d.c != null ? d.c.Lang : Language.E })
                        };

            if (!string.IsNullOrEmpty(cond.Name))
            {
                query = query.Where(d => d.r.Name.Contains(cond.Name));
            }
            if (!string.IsNullOrEmpty(cond.DisplayName))

            {
                query = query.Where(d => d.ts.Any(c => c.Desc.Contains(cond.DisplayName)));
            }
            if (cond.IsActive.HasValue)
            {
                query = query.Where(d => d.r.IsActive == cond.IsActive);
            }
            if (cond.IsDeleted.HasValue)
            {
                query = query.Where(d => d.r.IsDeleted == cond.IsDeleted);
            }
            if (cond.IsSystem.HasValue)
            {
                query = query.Where(d => d.r.IsSystem == cond.IsSystem);
            }

            var data = query.ToList();
            foreach (var item in data)
            {
                item.r.FullNames = item.ts.ToList();
                item.r.Remarks = item.Rs.ToList();
            }

            var list = data.Select(s=> s.r).ToList();
            return list;
        }
    }
}
