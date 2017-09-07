using FastJSON;
using JSNet.Model;
using JSNet.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderSys.Admin.Controllers
{
    public class AdminBaseController : Controller
    {
        //protected RoleEntity role;
        public AdminBaseController()
        {
            //role = new PermissionService().GetCurrentRole();
        }

        protected JSONParameters jsonParams = new JSONParameters()
        {
            UseUTCDateTime = false,
            UsingGlobalTypes = false,
            UseExtensions = false,
        };
    }
}
