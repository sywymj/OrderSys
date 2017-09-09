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
    public class PermissionController : WeixinBaseController
    {
        //
        // GET: /Permission/
        private PermissionService permissionService = new PermissionService();

        public ActionResult Index()
        {
            //return View();

            JsonResult js = new JsonResult();
            js.Data = new JSResponse("成功删除");
            return js;
        }

        [HttpGet]
        public ActionResult AllStaffs()
        {
            UserService userService = new UserService();

            var list = userService.GetAllStaffs();
            if (list.Count > 0)
            {
                return View("Staffs", list);
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
