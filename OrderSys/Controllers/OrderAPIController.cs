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
            //{\"tel\": \"13800138000\",\"openid\": \"123123\"}
            string re = string.Empty;
            string json = string.Empty;
            try
            {
                json = service.DecryptData(JSRequest.GetRequestUrlParm("submitdata").Replace(' ', '+'));
                UpdateOrderSysOpenID_APIViewModel vmodel = FastJSON.JSON.ToObject<UpdateOrderSysOpenID_APIViewModel>(json);

                string tel = JSValidator.ValidateTel("参数Tel", vmodel.Tel, true);
                string openID = JSValidator.ValidateString("参数OPENID", vmodel.OpenID, true);

                UserService usrService = new UserService();
                usrService.EditUserOpenID(tel, openID);

                re = JSON.ToJSON(new JSResponse(ResponseType.None, "更新成功！"), jsonParams);
            }
            catch (JSException jse)
            {
                re = JSON.ToJSON(new JSResponse(jse.ErrorCode, jse.ErrorMsg, data: json), jsonParams);
            }
            catch (Exception e)
            {
                re = JSON.ToJSON(new JSResponse("500", e.Message, data: json), jsonParams);
            }
            return re;
        }

        [HttpGet]
        public string LogoutOrderSys()
        {
            //{\"tel\": \"13800138000\",\"openid\": \"123123\"}
            string re = "";
            string json = "";
            try
            {
                json = service.DecryptData(JSRequest.GetRequestUrlParm("submitdata").Replace(' ', '+'));
                UpdateOrderSysOpenID_APIViewModel vmodel = FastJSON.JSON.ToObject<UpdateOrderSysOpenID_APIViewModel>(json);

                string tel = JSValidator.ValidateTel("参数Tel", vmodel.Tel, true);
                string openID = JSValidator.ValidateString("参数OPENID", vmodel.OpenID, true);

                LoginService loginService = new LoginService();
                loginService.VXLogout(openID);

                re = JSON.ToJSON(new JSResponse(ResponseType.None, "注销成功！"), jsonParams);
            }
            catch (JSException jse)
            {
                re = JSON.ToJSON(new JSResponse(jse.ErrorCode, jse.ErrorMsg, data: json), jsonParams);
            }
            catch (Exception e)
            {
                re = JSON.ToJSON(new JSResponse("500", e.Message, data: json), jsonParams);
            }
            return re;
        }

        [HttpGet]
        public string TestEncryptData()
        {
            //string s = service.EncryptData("{\"type\": \"orderaccepted\",\"openid\": \"1234567890\",\"first\": \"您好，你的维修单已处理完成，请您对本次服务进行评价\",\"remark\": \"点击评价，非常感谢\",\"orderno\": \"工单号\",\"treatment\": \"处理方案\"}");
            string s = service.EncryptData("{\"tel\": \"13620831234\"}");
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

        public string TestSubmitDataDecrypt()
        {

            string submitData = "/sBtPXbXE0B0YXFu7z/yJTgLnoD58l1bqbctZ0qeDL7yew1xPgqo1Lvw6BH3eOwY/ClHMupgyYWP+ZZdnDiOl77mP/JV5dGtcBJpu8wojmpTpf4j69cGMbMrliJ74R7vYWBl7VXc0c/Urt1lby82hsGNNfeTcs6+RrAftsomuVl2sMqvCu8r0ThXB8kUBHEHSAbYhsq8BKjcezkQ/BtRiBgc3fhspM/VbQkzjuLkjqsowKzkqb+laO0UtirMo7yhI34Dszbp4H17EHZu73RjmhlcXyHqSHACRadwumWo8DWhM7ooGg1kmOsMLzHczK3wNSb3MXv1PH/4nQBwZa/LUa9ZUhrglajVkc8bkYbMncR00hmnyRYr2gUq6bZd4Cv0fmKyRfkT6BrTwgzj93+lopdWfhFfOpSeZ/qaN3eoI4++pWFrIQHfT21gmtMlODkuFm60RYordCT5O7j5gbRcRujKjoOAUS+TYssDL3+lKfZ0hh40xiCyNkSMEMmXiGq1dmr/SiAAOVDkIXimhz5LDsjSeyajUJt/5kWbNhsoTPzPRUF/kLhdcA==";
            //string submitData = "/sBtPXbXE0BCF7D+gYX5Z+Dxbd5s34YUGtcvwArJaG2mz2tMt8qAw9ykJ/jB34WH2uhD8KDsQmprT8c6SWvNHHecsyE1OCF3Mw1M1Y9BqIjYt80J4aOacfqVoJp3pF+9SLCEq6BZWCDjoNJQj+RJTD4M2Nfiq98Hbx2BVw3j6aqSsRjqUOO4Ga3e7iCXSGfqwIPJ9t1ErzsS26tKfe3NS+dVuRpBPlGzRh7j/tSpnATE4s+qHj0UVFNFAomHzHx5cj1C0NJ07L4EKOGSPdPREt5dVGsoeW1rBuLRghHjNqUruOcOyc6Bhdrn2l6A4z3uugGn+9XKxscptTkQjeYBVwAw5vk//pZVKq0alVZI991Gx0ZrzLePgo6phJetd6OrY1XupTI2wMVA4mv+HtEsseqeNTBBThgMJcHhsZfJ+/UzkMauvA9fRt+ACd+RQssmJLiZ2T30WmUVFbiDU7gB5uk3MIjktsKOu2gccUmxlyC/JNQ5ayXIAQ==";
            KawuService service = new KawuService();
            string json = service.DecryptData(submitData);
            return json;
        }

        public string TestCallKawuApi()
        {
            string mess = "";
            KawuService service = new KawuService();
            bool b = service.Test_VXPushMsg();

            //LogService logService = new LogService();
            //JSException ex = new JSException("message", "errorcode", "errormsg");
            //logService.AddKawuApiLog(ex, "reqUrl", "resJson", "apitype");

            //LoginService loginService = new LoginService();
            //loginService.VXLogout("5");

            string re = JSON.ToJSON(new JSResponse(mess), jsonParams);
            return re;
        }

    }
}
