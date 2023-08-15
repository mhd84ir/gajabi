using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using gajabi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using DataLayer.Context;
using Microsoft.AspNetCore.Hosting;
using ViewModels.AdminViewModel.Users;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using DataLayer.AdminEntities.User;
using DataLayer.AdminEntities.Users;

namespace gajabi.Areas.Admin.Controllers
{
    [Area("Admin")]
   
    public class HomeController : BaseController
    {
        public static string msg ;
        public HomeController(ContextHampadco _db,IWebHostEnvironment  env) :base(_db,env)
        {
        }
        public IActionResult Index()
        {
            var user = db.Tbl_User.Where(a => a.UserNameUs=="admin").SingleOrDefault();
            if (user != null)
            {
                menu.img=user.ProfileImageUs;
            }
             
            menu.resiver=db.Tbl_Message.Where(a=>a.StateMess==false && a.SenderMess!="admin").Count();
            menu.sender=db.Tbl_Message.Where(a=>a.StateMess==false && a.SenderMess=="admin").Count();
          
            return View();
        }
        public IActionResult exit()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("index","home");
        }

        public IActionResult RayChat()
        {
          return View();
        }

        public IActionResult login()
        {
            if ( msg != null )
            {
                ViewBag.msg = msg ;
                msg = null ;   
            }

            return View();
        }

        public IActionResult checklogin(Vm_User vu)
        {
            var user = db.Tbl_User.Where(a => a.UserNameUs=="admin").SingleOrDefault();

            //if user is null then add admin and password is 1234

            if (user ==null && vu.UserNameUs=="admin")
            {
                Tb_User me = new Tb_User();

                me.UserNameUs="admin";
                me.UserFamilyUs="مدیریت";
                me.PasswordUs="1234";
                me.PhoneUs="09100000000";

                db.Tbl_User.Add(me);
                db.SaveChanges();

                user = db.Tbl_User.Where(a => a.UserNameUs=="admin").SingleOrDefault();


            }




            


            if ( vu.UserNameUs == user.UserNameUs && vu.PasswordUs == user.PasswordUs )
            {
                var claims = new List<Claim> () {
                new Claim (ClaimTypes.NameIdentifier,user.PhoneUs),
                new Claim (ClaimTypes.Name, user.UserFamilyUs)
                };
                HttpContext.Session.SetString("mobile",user.PhoneUs);
    
                var identity = new ClaimsIdentity (claims, CookieAuthenticationDefaults.AuthenticationScheme);
    
                var principal = new ClaimsPrincipal (identity);
    
                var properties = new AuthenticationProperties {
                    ExpiresUtc = DateTime.UtcNow.AddYears(1),
                    IsPersistent = true
                };
                HttpContext.SignInAsync (principal, properties);
                return RedirectToAction( "index" , "home" );
            }
            else
            {
                msg = "نام کاربری یا رمز اشتباه است" ;
                return RedirectToAction( "login" , "home" ); 
            }

        }
    }
}