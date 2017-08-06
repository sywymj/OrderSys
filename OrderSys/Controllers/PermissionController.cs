using JSNet.BaseSys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderSys.Controllers
{
    public class PermissionController : Controller
    {
        //
        // GET: /Permission/

        public ActionResult Index()
        {
            //return View();

            JsonResult js = new JsonResult();
            js.Data = new JSResponse("成功删除");
            string s = "";
            return js;
        }

    }
}
