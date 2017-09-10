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
    public class UserController : AdminBaseController
    {
        //
        // GET: /User/
        UserService service = new UserService();

        public ActionResult Index()
        {
            return View();
        }

        public string VerifyUserName(string userName)
        {
            bool re = false;
            
            if (service.ChkUserNameExist(userName))
            {
                return JSON.ToJSON(new JSResponse(re), jsonParams);
            }

            re = true;
            return JSON.ToJSON(new JSResponse(re), jsonParams);
            
        }

    }
}
