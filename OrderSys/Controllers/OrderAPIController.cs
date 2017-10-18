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
            string json = service.DecryptData(JSRequest.GetRequestUrlParm("submitdata").Replace(' ', '+'));
            UpdateOrderSysOpenID_APIViewModel vmodel = FastJSON.JSON.ToObject<UpdateOrderSysOpenID_APIViewModel>(json);

            string tel = JSValidator.ValidateString("参数Tel", vmodel.Tel, true);
            string openID = JSValidator.ValidateString("参数OPENID", vmodel.OpenID, true);

            UserService usrService = new UserService();
            usrService.EditUserOpenID(tel, openID);

            string re = JSON.ToJSON(new JSResponse(ResponseType.None,"更新成功！"), jsonParams);
            return re;
        }

        [HttpGet]
        public string LogoutOrderSys()
        {
            string json = service.DecryptData(JSRequest.GetRequestUrlParm("submitdata").Replace(' ', '+'));
            UpdateOrderSysOpenID_APIViewModel vmodel = FastJSON.JSON.ToObject<UpdateOrderSysOpenID_APIViewModel>(json);

            string tel = JSValidator.ValidateString("参数Tel", vmodel.Tel, true);
            string openID = JSValidator.ValidateString("参数OPENID", vmodel.OpenID, true);

            LoginService loginService = new LoginService();
            loginService.VXLogout(openID);

            string re = JSON.ToJSON(new JSResponse(ResponseType.None, "注销成功！"), jsonParams);
            return re;
        }

        [HttpGet]
        public string TestEncryptData()
        {
            string s = service.EncryptData("{\"type\": \"orderaccepted\",\"openid\": \"1234567890\",\"first\": \"您好，你的维修单已处理完成，请您对本次服务进行评价\",\"remark\": \"点击评价，非常感谢\",\"orderno\": \"工单号\",\"treatment\": \"处理方案\"}");
            string re = JSON.ToJSON(new JSResponse(data: s), jsonParams);
            return re;
        }

        [HttpGet]
        public string TestAddWeixinUser()
        {
            string mess = "";
            bool b = service.AddWeixinUser("13620834810", "曹操");
            string re = JSON.ToJSON(new JSResponse(mess), jsonParams);
            return re;
        }

        public string TestCallKawuApi()
        {
            string mess = "";
            //KawuService service = new KawuService();
            //bool b = service.AcceptOrder_VXPushMsg();

            //LogService logService = new LogService();
            //JSException ex = new JSException("message", "errorcode", "errormsg");
            //logService.AddKawuApiLog(ex, "reqUrl", "resJson", "apitype");

            LoginService loginService = new LoginService();
            loginService.VXLogout("5");

            string re = JSON.ToJSON(new JSResponse(mess), jsonParams);
            return re;
        }

    }
}
