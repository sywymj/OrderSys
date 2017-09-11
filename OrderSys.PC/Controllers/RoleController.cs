using FastJSON;
using JSNet.BaseSys;
using JSNet.Model;
using JSNet.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.Mvc;

namespace OrderSys.Admin.Controllers
{
    public class RoleController : AdminBaseController
    {
        //
        // GET: /Role/
        MyRoleService service = new MyRoleService();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string GetRoleDDL()
        {

            RoleEntity role = service.GetCurrentRole();

            List<RoleEntity> list = service.GetRoleList();

            var re = list.Select(l =>
                new ViewRoleDDL()
                {
                    ID = l.ID.ToString(),
                    Title = l.FullName
                }).ToList();

            string s = JSON.ToJSON(new JSResponse(re), jsonParams);

            return s;
        }

    }
}
