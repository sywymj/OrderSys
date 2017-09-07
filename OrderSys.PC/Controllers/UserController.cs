using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderSys.Admin.Controllers
{
    public class UserController : AdminBaseController
    {
        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }

    }
}
