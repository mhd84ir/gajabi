using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Context;
using gajabi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace gajabi.Areas.Admin.Controllers {
    
    [Area("Admin")]
    [Authorize]
       public class FinancialController : BaseController {
           public FinancialController(ContextHampadco _db,IWebHostEnvironment  env) :base(_db,env)
        {
        }
        public IActionResult Index () {
            return View ();
        }
        public IActionResult list () {
            return View ();
        }

    }
}