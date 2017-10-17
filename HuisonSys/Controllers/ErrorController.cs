using FastJSON;
using JSNet.BaseSys;
using JSNet.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HuisonSys.Controllers
{
    public class ErrorController : BaseController
    {
        //
        // GET: /Error/

        public ActionResult Index(string errMsg)
        {
            return View("/Views/Shared/Error.cshtml", errMsg);
        }

        public string ShowErrorTips()
        {
            string errCode = RouteData.Values["ErrorCode"].ToString();
            string errMessage = RouteData.Values["ErrorMsg"].ToString();
            string message = RouteData.Values["Tips"].ToString() == "" ? errMessage : RouteData.Values["Tips"].ToString();//错误消息

            string re = JSON.ToJSON(new JSResponse(message, errCode, errMessage), jsonParams);
            return re;
        }

        public string Http404()
        {
            return "貌似URL不在~~";
        }

        /// <summary>
        /// 
        /// </summary>
        public string Http403()
        {
            bool isAjax = Request.Headers["x-requested-with"] == null ? false : true;//判断是否ajax请求
            string area = Request.RequestContext.RouteData.DataTokens["area"] == null ? "" : Request.RequestContext.RouteData.DataTokens["area"].ToString().ToLower();

            string url = "/NoRight.html";
            switch (area)
            {
                case "weixin":
                    break;
                case "admin":
                    break;
                default:
                    break;
            }
            
            if (!isAjax)
            {
                Response.Redirect(url);
                Response.End();
            }

            string message = RouteData.Values["Tips"].ToString();
            string re = JSON.ToJSON(new JSResponse("403", message), jsonParams);
            return re;
        }

        public string Http401()
        {
            bool isAjax = Request.Headers["x-requested-with"] == null ? false : true;//判断是否ajax请求
            string area = Request.RequestContext.RouteData.DataTokens["area"] == null ? "" : Request.RequestContext.RouteData.DataTokens["area"].ToString().ToLower();
            
            string message = RouteData.Values["Tips"].ToString();

            string url = "/Login.html";
            string redirectUrl = "";
            switch (area)
            {
                case "weixin":
                    redirectUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxba12ba5cb2571169&redirect_uri=http://weixin.huison.com/weixin/wxcallback.aspx&response_type=code&scope=snsapi_base&state=my#wechat_redirect";
                    url = Url.Action("LoginIndex", "Home", new { area = "Weixin", errMsg = message, url = redirectUrl });
                    break;
                case "admin":
                    redirectUrl = Url.Action("LoginIndex", "Home", new { area = "Admin" });
                    url = Url.Action("LoginIndex", "Home", new { area = "Admin", errMsg = message, url = redirectUrl });
                    break;
                default:
                    break;
            }

            if (!isAjax)
            {
                Response.Redirect(url);
                Response.End();
            }

            string re = JSON.ToJSON(new JSResponse("401", message, data: url), jsonParams);
            return re;
        }

        public string Http500()
        {
            bool isAjax = Request.Headers["x-requested-with"] == null ? false : true;//判断是否ajax请求
            string area = Request.RequestContext.RouteData.DataTokens["area"] == null ? "" : Request.RequestContext.RouteData.DataTokens["area"].ToString().ToLower();

            string message = RouteData.Values["Tips"].ToString();

            string url = Url.Action("Index", "Error");
            switch (area)
            {
                case "weixin":
                    break;
                case "admin":
                    break;
                default:
                    break;
            }

            if (!isAjax)
            {
                Response.Redirect(url);
                Response.End();
            }

            string re = JSON.ToJSON(new JSResponse("服务器出错！", "500", message), jsonParams);
            return re;
        }
    }
}
