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
            string message = RouteData.Values["Tips"].ToString();

            string re = "";
            if (string.IsNullOrEmpty(errCode) || string.IsNullOrEmpty(errMessage))
            {
                re = JSON.ToJSON(new JSResponse(ResponseType.Error, "500", message), jsonParams);
                return re;
            }

            re = JSON.ToJSON(new JSResponse(message, errCode, errMessage), jsonParams);
            return re;
        }

        public string Http404()
        {
            bool isAjax = Request.Headers["x-requested-with"] == null ? false : true;//判断是否ajax请求

            string url = "/404.html";
            string area = Request.RequestContext.RouteData.DataTokens["area"] == null ? "" : Request.RequestContext.RouteData.DataTokens["area"].ToString().ToLower();
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
                Response.Redirect(url, true);
            }

            string message = RouteData.Values["Tips"].ToString();
            string re = JSON.ToJSON(new JSResponse("404"), jsonParams);
            return re;
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
                Response.Redirect(url, true);
            }

            string message = RouteData.Values["Tips"].ToString();
            string re = JSON.ToJSON(new JSResponse(ResponseType.Redict, message, data: url), jsonParams);
            return re;
        }

        public string Http401()
        {
            bool isAjax = Request.Headers["x-requested-with"] == null ? false : true;//判断是否ajax请求
            string area = Request.RequestContext.RouteData.DataTokens["area"] == null ? "" : Request.RequestContext.RouteData.DataTokens["area"].ToString().ToLower();
            
            string message = RouteData.Values["Tips"].ToString();

            string middleurl = "";//用来提示超时信息的中间页面。
            string loginurl = "";
            switch (area)
            {
                case "weixin":
                    loginurl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wxba12ba5cb2571169&redirect_uri=http://weixin.huison.com/weixin/wxcallback.aspx&response_type=code&scope=snsapi_base&state=my#wechat_redirect";
                    middleurl = Url.Action("LoginIndex", "Home", new { area = "Weixin", msg = message, url = loginurl });
                    break;
                case "admin":
                    loginurl = Url.Action("LoginIndex", "Home", new { area = "Admin" });
                    middleurl = Url.Action("LoginIndex", "Home", new { area = "Admin", msg = message, url = loginurl });
                    break;
                default:
                    break;
            }

            if (!isAjax)
            {
                //当用url访问出现无权限时，需要先跳转到一个中间页面来提示登录超时，再跳转到登录页面
                Response.Redirect(middleurl, true);
            }

            string re = JSON.ToJSON(new JSResponse(ResponseType.Redict, message, data: loginurl), jsonParams);
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
