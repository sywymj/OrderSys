using FastJSON;
using JSNet.BaseSys;
using JSNet.Model;
using JSNet.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderSys.Admin.Controllers
{
    public class OrganizeController : AdminBaseController
    {
        //
        // GET: /Organize/
        OrganizeService service = new OrganizeService();
        public ActionResult Index()
        {
            return View("~/Areas/Admin/Views/Home/Index.cshtml");
        }

        public string GetOrganizeDDL()
        {
            RoleEntity role = new PermissionService().GetCurrentRole();

            List<OrganizeEntity> list = service.GetTreeOrganizeList("FSWGY");

            list.Select(l => new
            {
                ID = l.ID,
                ParentID = l.ParentID,
                Title = l.FullName
            });

            string s = JSON.ToJSON(new JSResponse(new ListData<OrganizeEntity>(list)), jsonParams);
            return s;
        }

    }
}
