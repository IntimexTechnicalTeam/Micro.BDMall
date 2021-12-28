using Autofac;
using BDMall.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Web.Mvc;

namespace BDMall.Admin.Controllers
{
    public class DesktopController : BaseMvcController
    {
        public DesktopController(IComponentContext service) : base(service)
        {
        }

        // GET: Desktop
        public async Task<ActionResult> Index()
        {
            if (CurrentUser == null)
                ViewBag.IsMerchant = false;
            else
                ViewBag.IsMerchant = CurrentUser.IsMerchant;

            return View();
        }
    }
}