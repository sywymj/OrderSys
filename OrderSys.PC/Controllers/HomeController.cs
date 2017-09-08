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
    public class HomeController : AdminBaseController
    {
        //
        // GET: /Home/
        private PermissionService permissionService = new PermissionService();

        public ActionResult Index()
        {
            return View("~/Areas/Admin/Views/Home/Index.cshtml");
        }

        public string GetLeftTree()
        {
            RoleEntity role = new PermissionService().GetCurrentRole();
            DataTable dt = permissionService.GetMenu(role, "OrderSys_PC");
            dt.Columns["Resource_ID"].ColumnName = "ID";
            dt.Columns["Resource_ParentID"].ColumnName = "ParentID";
            dt.Columns["Resource_FullName"].ColumnName = "Title";
            dt.Columns["Resource_NavigateUrl"].ColumnName = "Url";
            dt.Columns["Resource_ImagUrl"].ColumnName = "ImagUrl";

            DataTable re = dt.DefaultView.ToTable(false, new string[] { "ID", "ParentID", "Title", "Url", "ImagUrl" });
            return JSON.ToJSON(new JSResponse(re),jsonParams);
        }

        public string GetButton(string resourceCode)
        {
            RoleEntity role = new PermissionService().GetCurrentRole();
            DataTable dt  = permissionService.GetButton(role, resourceCode);
            return JSON.ToJSON(new JSResponse(dt), jsonParams);
        }

        
    }
}
