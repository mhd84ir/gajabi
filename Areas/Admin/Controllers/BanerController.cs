using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.AdminEntities.Baner;
using DataLayer.Context;
using gajabi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using ViewModels.AdminViewModel.Baner;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace gajabi.Areas.Admin.Controllers {
    [Area("Admin")]
    [Authorize]
   
    public class BanerController : BaseController {
       public BanerController(ContextHampadco _db,IWebHostEnvironment  env) :base(_db,env)
        {
        }
        public IActionResult Index () {
            if (err != null) {
                ViewBag.er = err;
                err = null;
            }
            //////////////////////////////////////////////////selector
            var qlist = db.Tbl_Product.ToList ();
            ViewBag.list = new SelectList (qlist, "Id", "TitleProductPro");
            ///////////////////////////////////////////////////////////list
            return View ();
        }
        public IActionResult list () {
            if (err != null) {
                ViewBag.er = err;
                err = null;
            }

            ////////////////////////////////list
            var d = db.Tbl_Baner.ToList ();
            if (d != null) {
                List<Vm_Baner> s = new List<Vm_Baner> ();
                foreach (var item in d) {
                    var q = db.Tbl_Product.Where (a => a.Id == item.CategoryProductIdSlid).SingleOrDefault ();
                    if (q ==null)
                    {
                        prname="محصولی ثبت نشده";
                    }else
                    {
                        prname=q.TitleProductPro;
                    }
                    Vm_Baner f = new Vm_Baner () {
                        Id = item.Id,
                        ImageMainSlid = item.ImageMainSlid,
                        Language = item.Language,
                        nameproduct = prname,
                        IdBaner=item.IdBaner
                    };
                    s.Add (f);

                }
                ViewBag.list = s.OrderByDescending (a => a.Id).ToList ();
            }else
            {
                 ViewBag.list=null;
            }

            return View ();
        }

        public async Task<IActionResult> add (Vm_Baner ex) {
            ///////////////upload file

            var cc = db.Tbl_Baner.Where(a=>a.IdBaner==ex.IdBaner).SingleOrDefault();

            if (cc != null)
            {
                err = "بنر با این شماره موجود میباشد بنر را حذف نمایید";
                return RedirectToAction("index");
            }
            else
            {
                string FileExtension1 = Path.GetExtension(ex.img.FileName);
                NewFileName = String.Concat(Guid.NewGuid().ToString(), FileExtension1);
                var path = $"{_env.WebRootPath}\\fileupload\\{NewFileName}";
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await ex.img.CopyToAsync(stream);
                }
                Tb_Baner tb = new Tb_Baner()
                {
                    ImageMainSlid = NewFileName,
                    CategoryProductIdSlid = ex.CategoryProductIdSlid,
                    Language = ex.Language,
                    IdBaner = ex.IdBaner
                };
                db.Tbl_Baner.Add(tb);
                db.SaveChanges();
                err = "اطلاعات با موفقیت ثبت شد";
                return RedirectToAction("index");
            }







            //////////////////////////end upload file 
        }

        public IActionResult del (int id) {
            var qdel = db.Tbl_Baner.Where (add => add.Id == id).SingleOrDefault ();
            db.Tbl_Baner.Remove (qdel);
            db.SaveChanges ();
            err = "حذف رکورد مورد نظر با موفقیت انجام شد";
            return RedirectToAction ("list");
        }

    }
}