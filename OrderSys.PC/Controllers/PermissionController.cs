using FastJSON;
using JSNet.BaseSys;
using JSNet.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderSys.Admin.Controllers
{
    public class PermissionController : AdminBaseController
    {
        //
        // GET: /Permission/
        private PermissionService service = new PermissionService();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ResourceIndex()
        {
            return View("~/Areas/Admin/Views/Permission/ResourceIndex.cshtml");
        }

        [HttpGet]
        public string GetResourceList()
        {
            int count = 0;
            DataTable re = service.GetResources(out count);

            string s = JSON.ToJSON(new JSResponse(new DataTableData(re, count)), jsonParams);
            return s;
        }


        
    }
}
