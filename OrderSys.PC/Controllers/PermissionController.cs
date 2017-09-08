using FastJSON;
using JSNet.BaseSys;
using JSNet.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderSys.Admin.Controllers
{
    public class PermissionController : AdminBaseController
    {
        //
        // GET: /Permission/
        private PermissionService permissionService = new PermissionService();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        
    }
}
