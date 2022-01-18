using BDMall.Domain;
using BDMall.Model;
using BDMall.Repository;
using Intimex.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.Repository
{
    public class ExpressCompanyRepository : PublicBaseRepository, IExpressCompanyRepository
    {

        public ExpressCompanyRepository(IServiceProvider service) : base(service)
        {

        }

        public ExpressCompanyDto GetByCode(string code)
        {
            var result = new ExpressCompanyDto();
            var express = baseRepository.GetList<ExpressCompany>().FirstOrDefault(p => p.IsActive && !p.IsDeleted && p.Code == code);
            result = AutoMapperExt.MapTo<ExpressCompanyDto>(express);
            return result;
        }

        public List<ExpressCompanyDto> GetActiveExpress()
        {

            List<ExpressCompanyDto> result = new List<ExpressCompanyDto>();

            result = (from e in baseRepository.GetList<ExpressCompany>()
                      join t in baseRepository.GetList<Translation>() on new { a1 = e.NameTransId, a2 = CurrentUser.Lang } equals new { a1 = t.TransId, a2 = t.Lang } into tc
                      from tt in tc.DefaultIfEmpty()
                      where e.IsActive && !e.IsDeleted
                      select new ExpressCompanyDto
                      {
                          Id = e.Id,
                          Code = e.Code,
                          Name = tt == null ? "" : tt.Value,
                          NameTransId = e.NameTransId,
                      }).ToList();
            return result;
        }
    }
}
