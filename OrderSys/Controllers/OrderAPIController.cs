using FastJSON;
using JSNet.BaseSys;
using JSNet.Service;
using JSNet.Utilities;
using OrderSys.Models;
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
        KawuService service = new KawuService();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string UpdateOrderSysOpenID()
        {
            string json = service.DecryptData(JSRequest.GetRequestUrlParm("sumbitdata"));
            UpdateOrderSysOpenID_APIViewModel vmodel = FastJSON.JSON.ToObject<UpdateOrderSysOpenID_APIViewModel>(json);

            string tel = JSValidator.ValidateString("手机号码", vmodel.Tel, true);
            string openID = JSValidator.ValidateString("OPENID", vmodel.OpenID, true);

            //UserService service = new UserService();
            //service.EditUser(tel, openID);

            string re = JSON.ToJSON(new JSResponse(ResponseType.None,"更新成功！"), jsonParams);
            return re;
        }

        [HttpGet]
        public string LogoutOrderSys()
        {
            //todo

            string re = JSON.ToJSON(new JSResponse(ResponseType.None, "注销成功！"), jsonParams);
            return re;
        }


    }
}
