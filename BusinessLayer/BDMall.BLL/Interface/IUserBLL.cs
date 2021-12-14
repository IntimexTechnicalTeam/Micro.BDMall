using BDMall.Domain;
using BDMall.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Framework;

namespace BDMall.BLL
{
    public interface IUserBLL:IDependency
    {

        Task<SystemResult> ChangeLang(CurrentUser currentUser, Language Lang);
    }
}
