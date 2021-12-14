using Autofac;
using BDMall.Enums;
using Microsoft.AspNetCore.Mvc;
using Web.Mvc;

namespace BDMall.Admin.Controllers
{
    public class DefaultController : Controller
    {
       
        public IActionResult Index()
        {
            ViewBag.Language = Language.C;


            return View();
        }
    }
}
