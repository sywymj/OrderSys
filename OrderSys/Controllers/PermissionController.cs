using FastJSON;
using JSNet.BaseSys;
using JSNet.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderSys.Controllers
{
    public class PermissionController : BaseController
    {
        //
        // GET: /Permission/
        private PermissionService permissionService = new PermissionService();

        public ActionResult Index()
        {
            //return View();

            JsonResult js = new JsonResult();
            js.Data = new JSResponse("成功删除");
            string s = "";
            return js;
        }

        [HttpGet]
        public ActionResult GetAllStaffs()
        {
            var list = permissionService.GetAllStaffs();

            if (list.Count > 0)
            {
                return PartialView("Staffs", list);
            }
            else
            {
                ContentResult res = new ContentResult();
                res.Content = JSON.ToJSON(new JSResponse(ResponseType.NoData, "-暂无数据-"), jsonParams);
                return res;
            }
        }
        
    }
}
