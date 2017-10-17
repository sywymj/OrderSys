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
    [ManagerAuthorize]
    public class StaffController : WeixinBaseController
    {
        //
        // GET: /Permission/
        private UserService userService = new UserService();

        [HttpGet]
        public ActionResult GetAllStaffs()
        {
            var list = userService.GetWorkingStaffsByRole(userService.CurrentRole);
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
