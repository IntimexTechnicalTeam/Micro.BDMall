using Autofac;
using BDMall.BLL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Mvc;

namespace BDMall.Admin.Controllers
{
    public class HomeController : Controller
    {
       
        public IActionResult Index()
        {
            

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
