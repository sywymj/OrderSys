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
            RoleEntity role = new MyRoleService().GetCurrentRole();

            List<OrganizeEntity> list = service.GetTreeOrganizeList("FSWGY");

            var re = list.Select(l => 
                new ViewOrganizeDDL(){
                    ID = l.ID.ToString(),
                    ParentID = l.ParentID.ToString(),
                    Title = l.FullName
            }).ToList();

            string s = JSON.ToJSON(new JSResponse(re), jsonParams);

            return s;
        }


    }

  
}
