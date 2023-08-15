using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using gajabi.Models;
using DataLayer.Context;
using Microsoft.AspNetCore.Hosting;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace gajabi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    
    public class  UsersController: BaseController
    {
        
            public UsersController(ContextHampadco _db,IWebHostEnvironment  env) :base(_db,env)
        {
        }
        public IActionResult Index()
        {
            if (err != null)
            {
                ViewBag.er = err;
                err = null;
            }
            ViewBag.list=db.Tbl_User.OrderByDescending(a=>a.Id).ToList();
            return View();
        }
         public IActionResult del(int id)
        {
            var selectdel = db.Tbl_User.Where(a => a.Id == id).SingleOrDefault();
            db.Tbl_User.Remove(selectdel);
            db.SaveChanges();
            err = "حذف با موفقیت انجام شد";
            return RedirectToAction("index");
        }

        public IActionResult shop(string phone)
        {
            var selectdel = db.Tbl_User.Where(a => a.PhoneUs == phone).SingleOrDefault();
            if (selectdel.Language == "shop")
            {
                 selectdel.Language = "noshop";
                    db.Tbl_User.Update(selectdel);
                    db.SaveChanges();
            }
            else
            {
                selectdel.Language = "shop";
                    db.Tbl_User.Update(selectdel);
                    db.SaveChanges();
                
            }
           
            return RedirectToAction("index");
        }

    }
}