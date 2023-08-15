using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using gajabi.Models;
using Microsoft.AspNetCore.Hosting;
using DataLayer.Context;
using DataLayer.AdminEntities.Gallery;
using ViewModels.AdminViewModel.Gallery;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace gajabi.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]

    public class GalleryController : BaseController
    {
        public GalleryController(ContextHampadco _db, IWebHostEnvironment env) : base(_db, env)
        {
        }
        public IActionResult Index()
        {
            if (err != null)
            {
                ViewBag.er = err;
                err = null;
            }
            return View();
        }
        public IActionResult List()
        {
            if (err != null)
            {
                ViewBag.er = err;
                err = null;
            }
            ViewBag.list = db.Tbl_Gallery.OrderByDescending(a => a.Id).ToList();
            return View();
        }

        public async Task<IActionResult> add(Vm_Gallery gallery)
        {

            ///////////////upload file
            string FileExtension1 = Path.GetExtension(gallery.img.FileName);
            NewFileName = String.Concat(Guid.NewGuid().ToString(), FileExtension1);
            var path = $"{_env.WebRootPath}\\fileupload\\{NewFileName}";
            using (var stream = new FileStream(path, FileMode.Create))
            {

                await gallery.img.CopyToAsync(stream);

            }
            //////////////////////////end upload file  
            /// 

            if (db.Tbl_Gallery.Any(a => a.TitleGal == gallery.TitleGal))
            {

                var check = db.Tbl_Gallery.Where(a => a.TitleGal == gallery.TitleGal)?.SingleOrDefault();

                check.Language = gallery.Language;
                check.TitleGal = gallery.TitleGal;
                check.pathImage = NewFileName;



                db.Tbl_Gallery.Update(check);
                db.SaveChanges();

            }
            else
            {
                Tb_Gallery g = new Tb_Gallery()
                {
                    Language = gallery.Language,
                    TitleGal = gallery.TitleGal,
                    pathImage = NewFileName,


                };
                db.Tbl_Gallery.Add(g);
                db.SaveChanges();
            }


            return RedirectToAction("index");
        }


        public IActionResult del(int id)
        {
            var qselect = db.Tbl_Gallery.Where(a => a.Id == id).SingleOrDefault();
            db.Tbl_Gallery.Remove(qselect);
            db.SaveChanges();
            var qgallery = db.tb_ImageGalleries.Where(a => a.IdGallery == id).ToList();
            foreach (var item in qgallery)
            {
                db.tb_ImageGalleries.Remove(item);
                db.SaveChanges();
            }
            err = "رکورد با موفقیت حذف شد";
            return RedirectToAction("list");


        }

    }
}