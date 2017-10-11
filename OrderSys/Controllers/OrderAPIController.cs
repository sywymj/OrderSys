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
    public class OrderAPIController : WeixinBaseController
    {
        //
        // GET: /OrderAPI/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string UpdateOrderSysOpenID()
        {
            string sTel = JSRequest.GetRequestUrlParm("tel");
            string sOpen = JSRequest.GetRequestUrlParm("openid");

            string tel = JSValidator.ValidateString("手机号码", sTel, true);
            string openID = JSValidator.ValidateString("OPENID", sOpen, true);

            UserService service = new UserService();
            service.EditUser(tel, openID);

            string re = JSON.ToJSON(new JSResponse("更新成功！"), jsonParams);
            return re;
        }

        [HttpGet]
        public string LogoutOrderSys()
        {
            //todo

            string re = JSON.ToJSON(new JSResponse("更新成功！"), jsonParams);
            return re;
        }
    }
}
