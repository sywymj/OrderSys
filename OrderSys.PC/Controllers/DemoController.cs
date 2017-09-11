using FastJSON;
using JSNet.BaseSys;
using JSNet.Model;
using JSNet.Service;
using JSNet.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderSys.Admin.Controllers
{
    public class DemoController : AdminBaseController
    {
        //
        // GET: /Demo/
        
        public ActionResult Index()
        {
            return View("~/Areas/Admin/Views/XXX/XXX.cshtml");
        }

        public string GetXXXDDL()
        {
            OrganizeService service = new OrganizeService();
            RoleEntity role = new MyRoleService().GetCurrentRole();

            List<OrganizeEntity> list = service.GetTreeOrganizeList("XXX");

            list.Select(l => new
            {
                ID = l.ID,
                ParentID = l.ParentID,
                Title = l.FullName
            });

            string s = JSON.ToJSON(new JSResponse(new ListData<OrganizeEntity>(list)), jsonParams);
            return s;
        }

        //GetDataTableList
        [HttpGet]
        public string GetList(int pageIndex, int pageSize, string sortField, string sortOrder)
        {
            UserService userService = new UserService();

            int count = 0;
            Paging paging = new Paging(pageIndex, pageSize, sortField, sortOrder);
            DataTable re = userService.GetAllStaffs(paging, out count);

            string s = JSON.ToJSON(new JSResponse(new DataTableData(re, count)), jsonParams);
            return s;

        }
    }
}
