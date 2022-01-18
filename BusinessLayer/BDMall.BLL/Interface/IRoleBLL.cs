using BDMall.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public interface IRoleBLL:IDependency
    {
        List<RoleDto> Search(RoleCondition cond);

        RoleDto GetById(Guid id);

        SystemResult Save(RoleDto model);

        SystemResult Update(RoleDto model);

        SystemResult Remove(RoleDto model);

        SystemResult Restore(RoleDto model);
    }
}
