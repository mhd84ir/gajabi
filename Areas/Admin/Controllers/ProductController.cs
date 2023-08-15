using System.Data.Common;
using System.Numerics;
using System.Security.AccessControl;
using System.Dynamic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.AdminEntities.Category;
using DataLayer.AdminEntities.Product;
using DataLayer.Context;
using gajabi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using ViewModels.AdminViewModel.Product;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using DataLayer.AdminEntities.Order;

namespace gajabi.Areas.Admin.Controllers {
  
    [Area("Admin")]
    [Authorize]
    public class ProductController : BaseController {
        public ProductController(ContextHampadco _db,IWebHostEnvironment  env) :base(_db,env)
        {
        }
        public IActionResult Index () {
            //////////////////////////////////////////////////////////////////////////////////list

            return View ();
        }
        public IActionResult addproduct () {

            //////////////////////////////////////////////////////////////////////////////////list
            var qlist = db.tbl_category.Where (a => a.FatherIdCat == 0).ToList ();
            var listtb = new List<Tb_Category> ();
            if (qlist != null)
            {
                foreach (var item in qlist)
                {
                    var qselect = db.tbl_category.Where (a => a.FatherIdCat == item.Id).ToList ();
                    if (qselect.Count () != 0)
                    {
                        foreach (var item1 in qselect) 
                        {
                            var qselect1 = db.tbl_category.Where (a => a.FatherIdCat == item1.Id).ToList ();
                            if (qselect1.Count () != 0)
                            {
                                foreach (var item2 in qselect1)
                                {
                                    var b = new Tb_Category ()
                                    {
                                    Id = item2.Id,
                                    NameCat = item2.NameCat + "  " + "--" + "  " + item1.NameCat + "  " + "--" + "  " +item.NameCat,
                                    FatherIdCat = item1.FatherIdCat
                                    };
                                        listtb.Add (b);
                                }
                            }
                        }
                    }
                    else
                    {
                        listtb.Add (item);
                    }

                }
                ViewBag.list = new SelectList (listtb, "Id", "NameCat");
            } else
            {
                ViewBag.list = null;
            }

            //////////////////////////////////////////////////////////////////////////////////list
            if (err != null)
            {
                ViewBag.er = err;
                err = null;
            }
            return View ();
        }
        public IActionResult Arshive()
        {
            if (err != null) {
                ViewBag.er = err;
                err = null;
            }
            var list = db.Tbl_Product.Where(a=>a.TypeCarPro == "0").OrderByDescending (a => a.Id).ToList ();
            if (list.Count != 0 )
            {
                List<Vm_Product> p = new List<Vm_Product> ();
                foreach (var item in list) {
                    var qcat = db.tbl_category.Where (a => a.Id == Convert.ToInt32(item.CategoryChildIdPro)).SingleOrDefault ();
                    Vm_Product product = new Vm_Product () 
                    {
                        Id = item.Id,
                        TitleProductPro = item.TitleProductPro,
                        catname = qcat.NameCat,
                        ImageMainPro = item.ImageMainPro,
                        PricePro = item.PricePro,
                        Language = item.Language,
                        OfferPro = item.OfferPro,
                        TypeCarPro=item.TypeCarPro,
                    };
                    p.Add (product);
                }
                ViewBag.listp = p.OrderByDescending (a => a.Id).ToList ();
            }
            return View();
        }
        

        public IActionResult list () {
            if (err != null) {
                ViewBag.er = err;
                err = null;
            }

            var list = db.Tbl_Product.Where(a=>a.TypeCarPro == "1").OrderByDescending (a => a.Id).ToList ();
            if (list.Count != 0 )
            {              
            
                    List<Vm_Product> p = new List<Vm_Product> ();
                    foreach (var item in list) {
                        var qcat = db.tbl_category.Where (a => a.Id == Convert.ToInt32(item.CategoryChildIdPro)).SingleOrDefault ();
                        Vm_Product product = new Vm_Product () 
                        {
                            Id = item.Id,
                            TitleProductPro = item.TitleProductPro,
                            catname = qcat.NameCat,
                            ImageMainPro = item.ImageMainPro,
                            PricePro = item.PricePro,
                            Language = item.Language,
                            OfferPro = item.OfferPro,
                            TypeCarPro=item.TypeCarPro,

                        };
                        p.Add (product);

                    }
                        ViewBag.listp = p.OrderByDescending (a => a.Id).ToList ();
            }
            return View ();
        }

        public async Task<IActionResult> add (Vm_Product pro) {
            ////////////////////////////////////////////////////////////////////////////start upload main imge
            if (pro.mainimg != null)
            {
                ///////////////upload file
                string FileExtension1 = Path.GetExtension (pro.mainimg.FileName);
                NewFileName = String.Concat (Guid.NewGuid ().ToString (), FileExtension1);
                var path = $"{_env.WebRootPath}\\fileupload\\{NewFileName}";
                using (var stream = new FileStream (path, FileMode.Create))
                {
                    await pro.mainimg.CopyToAsync (stream);
                }
                //////////////////////////end upload file 
            }

            //////////////////////////////////////////////////////////////////////////////////////end upload multi image
            var fcat=db.tbl_category.Where(s=>s.Id.ToString()==pro.CategoryChildIdPro).SingleOrDefault().FatherIdCat;
            var cat=db.tbl_category.Where(d=>d.Id== fcat).SingleOrDefault().Id;
            var off=Convert.ToDouble(pro.OfferPro)/100;
            var offp=pro.PricePro*off;
            var op=pro.PricePro-offp;
            Tb_Product p = new Tb_Product () {
                TitleProductPro = pro.TitleProductPro,
                CategoryIdPro =Convert.ToString(cat),
                CategoryChildIdPro=pro.CategoryChildIdPro,
                PricePro = pro.PricePro,
                Price_Pro=Convert.ToInt32(op),
                OfferPro = pro.OfferPro,
                SizePro = pro.SizePro,
                ColorPro = pro.ColorPro,
                BrandPro = pro.BrandPro,
                TypeCarPro = "1",
                MaterialPro = pro.MaterialPro,
                TotalPro = pro.TotalPro,
                DescreptionPro = pro.DescreptionPro,
                Language = pro.Language,
                ImageMainPro = NewFileName,
            };
            db.Tbl_Product.Add(p);
            db.SaveChanges ();
            var q = db.Tbl_Product.OrderByDescending (a => a.Id).Take (1).SingleOrDefault ();
            ////////////////////////////////////////////////////////upload multi image
            if (pro.upload_imgs != null)
            {
                foreach (var item in pro.upload_imgs)
                {
                    ///////////////upload file
                    string FileExtension1 = Path.GetExtension (item.FileName);
                    NewFileName = String.Concat (Guid.NewGuid ().ToString (), FileExtension1);
                    var path = $"{_env.WebRootPath}\\fileupload\\{NewFileName}";
                    using (var stream = new FileStream (path, FileMode.Create))
                    {
                        await item.CopyToAsync (stream);
                    }
                    Tb_GalleryProduct g = new Tb_GalleryProduct ()
                    {
                        Language = pro.Language,
                        ImagePath = NewFileName,
                        IdProduct = q.Id
                    };
                    db.tb_GalleryProducts.Add(g);
                    db.SaveChanges();
                    //////////////////////////end upload file 
                }
            }
            if ( pro.Color != null )
            {
                var colornames = pro.Color.Split(",");
                var qpro = db.Tbl_Product.OrderByDescending( a => a.Id ).Take(1).SingleOrDefault();

                foreach ( var item in colornames )
                {
                    Tbl_ColorProduct tcp = new Tbl_ColorProduct()
                    {
                        ColorName = item ,
                        IdProduct = qpro.Id ,
                    };
                    db.Tbl_ColorProducts.Add(tcp);
                }
                db.SaveChanges();

            }
            err = "اطلاعات با موفقیت ثبت شد";
            //////////////////////////////////////////////////////////////////////////////////////end upload multi image
            return RedirectToAction ("addproduct");
        }

        public IActionResult del (int id) {
            var q = db.Tbl_Product.Where (a => a.Id == id).SingleOrDefault ();
            db.Tbl_Product.Remove (q);
            db.SaveChanges ();
            var favorate=db.Tbl_MyFavorites.Where(a => a.IdKala == id).ToList();
            foreach (var item in favorate)
            {
                db.Tbl_MyFavorites.Remove (item);
                db.SaveChanges ();  
            }
           err = "اطلاعات با موفقیت حذف شد";
          return RedirectToAction ("list");
        }
        public IActionResult hiden (int id) {
            var qa = db.Tbl_Product.Where (a => a.Id == id).SingleOrDefault ();
            qa.TypeCarPro = "0";
            db.Tbl_Product.Update(qa);
            db.SaveChanges();
            err = "اطلاعات با موفقیت مخفی شد";
            return RedirectToAction ("list");
        }
        public IActionResult show (int id) {
            var q = db.Tbl_Product.Where (a => a.Id == id).SingleOrDefault ();
            q.TypeCarPro = "1";
            db.Tbl_Product.Update(q);
            db.SaveChanges();
            err = "اطلاعات با موفقیت نمایان شد";
            return RedirectToAction ("Arshive");
        }
        

        public IActionResult update (int id) {

               //////////////////////////////////////////////////////////////////////////////////list
            var qlist = db.tbl_category.Where (a => a.FatherIdCat == 0).ToList ();
            var color = db.Tbl_ColorProducts.Where(a => a.IdProduct == id).ToList();
            string colorlist=null;
            if(color != null)
            {
                foreach (var item in color)
                {
                    colorlist = colorlist + item.ColorName+",";
                }

            }
            var listtb = new List<Tb_Category> ();

            if (qlist != null) {
                foreach (var item in qlist) {
                    var qselect = db.tbl_category.Where (a => a.FatherIdCat == item.Id).ToList ();
                    if (qselect.Count () != 0) {
                        foreach (var item1 in qselect) {
                            var qselect1 = db.tbl_category.Where (a => a.FatherIdCat == item1.Id).ToList ();
                            if (qselect1.Count () != 0)
                            {
                                foreach (var item2 in qselect1)
                                {
                                    var b = new Tb_Category () {
                                    Id = item2.Id,
                                    NameCat = item2.NameCat + "  " + "--" + "  " + item1.NameCat + "  " + "--" + "  " +item.NameCat,
                                    FatherIdCat = item1.FatherIdCat
                                        };
                                        listtb.Add (b);
                                    }
                                }
                            }
                    }
                    else
                    {
                        listtb.Add (item);
                    }

                }
                ViewBag.list = new SelectList (listtb, "Id", "NameCat");
            } else
            {
                ViewBag.list = null;
            }

            //////////////////////////////////////////////////////////////////////////////////list
            var qproduct = db.Tbl_Product.Where (a => a.Id == id).SingleOrDefault ();
            var qgallery = db.tb_GalleryProducts.Where (a => a.IdProduct == id).ToList ();
            List<string> h=new List<string>();
            foreach (var item in qgallery)
            {

                h.Add(item.ImagePath);
            }
            Vm_Product qp = new Vm_Product () {
                Id = qproduct.Id,
                TitleProductPro = qproduct.TitleProductPro,
                CategoryChildIdPro=qproduct.CategoryChildIdPro,
                ImageMainPro = qproduct.ImageMainPro,
                CategoryIdPro = qproduct.CategoryIdPro,
                PricePro = qproduct.PricePro,
                Price_Pro=qproduct.Price_Pro,
                OfferPro = qproduct.OfferPro,
                SizePro = qproduct.SizePro,
                ColorPro = colorlist,
                BrandPro = qproduct.BrandPro,
                MaterialPro = qproduct.MaterialPro,
                TotalPro = qproduct.TotalPro,
                DescreptionPro = qproduct.DescreptionPro,
                Language = qproduct.Language,

            };
           ViewBag.img=h;
            return View (qp);
        }



       public async Task<IActionResult> updatepro(Vm_Product pro)
       {
           var qlist=db.Tbl_Product.Where(a=>a.Id==pro.Id).SingleOrDefault();
           if (pro.upload_imgs != null)
           {
               var qgallery=db.tb_GalleryProducts.Where(a=>a.IdProduct==pro.Id).ToList();
               foreach (var item in qgallery)
               {
                   db.tb_GalleryProducts.Remove(item);
                   db.SaveChanges();

               }

               ////////////////////////////////////////////////////////upload multi image
            
                foreach (var item in pro.upload_imgs) {
                    ///////////////upload file
                    string FileExtension1 = Path.GetExtension (item.FileName);
                    NewFileName = String.Concat (Guid.NewGuid ().ToString (), FileExtension1);
                    var path = $"{_env.WebRootPath}\\fileupload\\{NewFileName}";
                    using (var stream = new FileStream (path, FileMode.Create)) {

                        await item.CopyToAsync (stream);

                    }
                    Tb_GalleryProduct g = new Tb_GalleryProduct () {
                        Language = pro.Language,
                        ImagePath = NewFileName,
                        IdProduct = pro.Id
                    };
                    db.tb_GalleryProducts.Add (g);
                    db.SaveChanges ();

                    //////////////////////////end upload file 
                }

          
           
           }
           
               ////////////////////////////////////////////////////////////////////////////start upload main imge
            var fcat=db.tbl_category.Where(s=>s.Id.ToString()==pro.CategoryChildIdPro).SingleOrDefault().FatherIdCat;
            var cat=db.tbl_category.Where(d=>d.Id== fcat).SingleOrDefault().Id;
            if (pro.mainimg != null)
            {


               

                ///////////////upload file
                string FileExtension1 = Path.GetExtension (pro.mainimg.FileName);
                    NewFileName = String.Concat (Guid.NewGuid ().ToString (), FileExtension1);
                    var path = $"{_env.WebRootPath}\\fileupload\\{NewFileName}";
                    using (var stream = new FileStream (path, FileMode.Create)) {

                        await pro.mainimg.CopyToAsync (stream);

                    }
                    // /        محاسبه قیمت بعد تخفیف        
                    
                    var off=Convert.ToDouble(pro.OfferPro)/100;
                    var offp=pro.PricePro*off;
                    var op=pro.PricePro-offp;
                      
                    //////////////////////////end upload file 
                qlist.Id = pro.Id;
                qlist.TitleProductPro = pro.TitleProductPro;
                qlist.ImageMainPro = NewFileName;
                qlist.CategoryIdPro =Convert.ToString(cat);
                qlist.CategoryChildIdPro=pro.CategoryChildIdPro;
                qlist.PricePro = pro.PricePro;
                qlist.Price_Pro=Convert.ToInt32(op);
                qlist.OfferPro = pro.OfferPro;
                qlist.SizePro = pro.SizePro;
                qlist.TypeCarPro = "1";
                qlist.ColorPro = pro.ColorPro;
                qlist.BrandPro = pro.BrandPro;
                qlist.MaterialPro = pro.MaterialPro;
                qlist.TotalPro = pro.TotalPro;
                qlist.DescreptionPro = pro.DescreptionPro;
                qlist.Language = pro.Language;

           
                 db.Tbl_Product.Update(qlist);
                db.SaveChanges();
                err="اطلاعات با موفقیت به روز رسانی شد.";
                
            }
            else
            {
                    var off=Convert.ToDouble(pro.OfferPro)/100;
                    var offp=pro.PricePro*off;
                    var op=pro.PricePro-offp;

                qlist.Id = pro.Id;
                qlist.TitleProductPro = pro.TitleProductPro;
                qlist.CategoryIdPro =Convert.ToString(cat);
                qlist.CategoryChildIdPro=pro.CategoryChildIdPro;
                qlist.PricePro = pro.PricePro;
                qlist.Price_Pro=Convert.ToInt32(op);
                qlist.OfferPro = pro.OfferPro;
                qlist.SizePro = pro.SizePro;
                qlist.ColorPro = pro.ColorPro;
                qlist.TypeCarPro = "1";
                qlist.BrandPro = pro.BrandPro;
                qlist.MaterialPro = pro.MaterialPro;
                qlist.TotalPro = pro.TotalPro;
                qlist.DescreptionPro = pro.DescreptionPro;
                qlist.Language = pro.Language;

                db.Tbl_Product.Update(qlist);
                db.SaveChanges();
                err="اطلاعات با موفقیت به روز رسانی شد.";
                
            }
            


            // colorrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrsssssssssssssssssss
             if (pro.ColorPro != null )
                {
                    var colornames = pro.ColorPro.Split(",");
                    var qpro = qlist;
                    var qcolor = db.Tbl_ColorProducts.Where(a => a.IdProduct == qpro.Id).ToList();

                    foreach (var item in qcolor)
                    {
                        db.Tbl_ColorProducts.Remove(item);
                         db.SaveChanges();
                    }
                    

                    foreach (var item in colornames)
                    {
                        Tbl_ColorProduct tcp = new Tbl_ColorProduct()
                        {
                            ColorName = item,
                            IdProduct = qpro.Id,
                        };
                        db.Tbl_ColorProducts.Add(tcp);
                        db.SaveChanges();
                    }
                }else{
                    
                    
                    var qcolor = db.Tbl_ColorProducts.Where(a => a.IdProduct == pro.Id).ToList();

                    foreach (var item in qcolor)
                    {
                        db.Tbl_ColorProducts.Remove(item);
                         db.SaveChanges();
                    }
                    
                }



            // end coloressssssssssssssssssssssss
               
               
               
               return RedirectToAction("list");


           
       }


    }
}