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
    public class  SourcehelpController: BaseController
    {
          public SourcehelpController(ContextHampadco _db,IWebHostEnvironment  env) :base(_db,env)
        {
        }
        public IActionResult UploadImageSingle()
        {
            return View();
        }
        public IActionResult UploadImageMulti()
        {
            return View();
        }
        public IActionResult list()
        {
            return View();
        }
        public IActionResult DropdownSearch()
        {
            return View();
        }



    }
}