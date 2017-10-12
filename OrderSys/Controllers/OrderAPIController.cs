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
            string json = service.DecryptData(JSRequest.GetRequestUrlParm("submitdata"));
            List<UpdateOrderSysOpenID_APIViewModel> vmodel = FastJSON.JSON.ToObject<List<UpdateOrderSysOpenID_APIViewModel>>(json);

            if (vmodel.Count == 0)
            {
                throw new JSException("参数有误！");
            }
            string tel = JSValidator.ValidateString("手机号码", vmodel[0].Tel, true);
            string openID = JSValidator.ValidateString("OPENID", vmodel[0].OpenID, true);

            //UserService usrService = new UserService();
            //usrService.EditUser(tel, openID);

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


        public string TestEncryptData()
        {
            string s = service.EncryptData("[{\"tel1\": \"123\",\"openid1\": \"13800138000\"}]");
            string re = JSON.ToJSON(new JSResponse(data: s), jsonParams);
            return re;
        }

    }
}
