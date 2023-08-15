using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Context;
using Extensions;
using gajabi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ViewModels.AdminViewModel.Order;
using Kavenegar;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace gajabi.Areas.Admin.Controllers {
    
    [Area("Admin")]
    [Authorize]
    public class OrderController : BaseController {
        public static string masage;
        public OrderController (ContextHampadco _db, IWebHostEnvironment env) : base (_db, env) { }
        // public IActionResult Index ()
        // {

        //     if (eror != null)
        //     {
        //         ViewBag.er=eror;
        //         eror=null;
        //     }

        //     List<Vm_Order> or = new List<Vm_Order> ();

        //     var qorder = db.Tbl_Order.Where (a => a.State == "save").OrderByDescending (a => a.Id).ToList ();
        //     if (qorder != null)
        //     {
        //         foreach (var item in qorder)
        //         {
        //             var qf = db.Tbl_Factors.Where (a => a.Id_Order == item.Id.ToString()).ToList();
                    
        //             var user=db.Tbl_User.Where(a=>a.Id.ToString()==item.Id_user).SingleOrDefault();
                    
        //             int sum = 0;

        //             if (qf.Count () != 0)
        //             {
        //                 foreach (var item1 in qf)
        //                 {
        //                     sum = sum + item1.Total_sum;
        //                 }
        //             }

        //             Vm_Order q = new Vm_Order () {
        //                 Id = item.Id,
        //                 datesave = item.Date_Order.ToPersianDateString (),
        //                 Total = sum,
        //                 Pay = item.Pay,
        //                 UserName=user.UserNameUs,
        //                 phone=user.PhoneUs
        //             };

        //             or.Add (q);
        //         }

        //         ViewBag.listp = or.OrderByDescending (a => a.Id).ToList ();

        //     }

        //     return View();
        // }
        // public IActionResult ongoing () {
        //     return View ();
        // }
        // public IActionResult sender () {
            
        //     List<Vm_Order> or = new List<Vm_Order> ();
        //     var qorder = db.Tbl_Order.Where (a => a.State == "send").OrderByDescending (a => a.Id).ToList ();
        //     if (qorder != null) {
        //         foreach (var item in qorder) {
        //             var qf = db.Tbl_Factors.Where (a => a.Id_Order == item.Id.ToString()).ToList ();
        //             var user=db.Tbl_User.Where(a=>a.Id.ToString()==item.Id_user).SingleOrDefault();
        //             int sum = 0;
        //             if (qf.Count () != 0) {
        //                 foreach (var item1 in qf) {
        //                     sum = sum + item1.Total_sum;
        //                 }
        //             }

        //             Vm_Order q = new Vm_Order () {
        //                 Id = item.Id,
        //                 datesave = item.Date_Order.ToPersianDateString (),
        //                 Total = sum,
        //                 Pay = item.Pay,
        //                 UserName=user.UserNameUs,
        //                 phone=user.PhoneUs
        //             };
        //             or.Add (q);

        //         }
        //         ViewBag.listp = or.OrderByDescending (a => a.Id).ToList ();

        //     }
        //     return View();
        // }

        //   public IActionResult show(int id)
        // {
         
        //     var qorder = db.Tbl_Order.Where (a => a.Id==id).SingleOrDefault ();
        //     ViewBag.listp = db.Tbl_Factors.Where (a => a.Id_Order == qorder.Id.ToString()).ToList ();
        //     int sum = 0;
        //     foreach (var item in ViewBag.listp) {
        //         sum = sum + item.Total_sum;
        //     }
        //     ViewBag.sum = sum;
        //     return View ();
        // }

        // public IActionResult del(int id)
        // {
        //   var qorder = db.Tbl_Order.Where (a => a.Id==id).SingleOrDefault ();
        //   db.Tbl_Order.Remove(qorder);
        //   db.SaveChanges();
        //   eror="سفارش با موفقیت حذف شد";

        //   return RedirectToAction("index");
        // }

        //  public IActionResult send(int id)
        // {
        //   var qorder = db.Tbl_Order.Where (a => a.Id==id).SingleOrDefault ();
        //   qorder.State="send";
        //   db.Tbl_Order.Update(qorder);
        //   db.SaveChanges();
        //   eror="سفارش با موفقیت ارسال شد";

        //   return RedirectToAction("index");
        // }

        public IActionResult index()
        {
            ViewBag.order=db.Tbl_Order.Where(a=>a.State=="OK").OrderByDescending(a=>a.Id).ToList();
            return View();
        }
        public IActionResult detailfactor(int id)
        {
            ViewBag.order=db.Tbl_Factors.Where(a=>a.RFactor==id).ToList();
            return View();
        }
        public IActionResult send(int id)
        {
            var order=db.Tbl_Order.Where(a=>a.RFactor==id).SingleOrDefault();
            order.State="OKK";
            order.Date_Send=DateTime.UtcNow;
            db.Tbl_Order.Update(order);
            db.SaveChanges();
            masage="ارسال باموفقیت انجام شد";
            
            return RedirectToAction("sender");
        }
        public IActionResult sender()
        {
            if (masage != null)
            {
               ViewBag.msg=masage;
               masage = null; 
            }
            ViewBag.order=db.Tbl_Order.Where(a=>a.State=="OKK").OrderByDescending(a=>a.Id).ToList();
            return View();
        }
        
        

    }
}