using Autofac;
using BDMall.BLL;
using BDMall.Domain;
using BDMall.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Framework;
using Web.Jwt;
using Web.Mvc;

namespace BDMall.Admin.Controllers
{
    [LanguageResource]
    public class AccountController : Controller
    {      
        
        [HttpGet]
        public ActionResult User()
        {
            return View();
        }
        
        [HttpGet]
        public ActionResult Role()
        {
            return View();
        }
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            ViewBag.CopyRight = "2341234";
            ViewBag.ID = Guid.NewGuid().ToString();
            return View();
        }

        public async Task<IActionResult> LogOff()
        {           
            return RedirectToAction("Login", "Account");
        }

    }
}
