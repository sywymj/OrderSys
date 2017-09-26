using FastJSON;
using JSNet.BaseSys;
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

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowErrorTips()
        {
            string message = RouteData.Values["exception"].ToString();
            ContentResult res = new ContentResult()
            {
                Content = JSON.ToJSON(new JSResponse(message,"500", message), jsonParams)
            };
            return res;
        }

        public ActionResult Http404()
        {
            return Content("貌似URL不在~~", "text/plain");
        }

        /// <summary>
        /// 
        /// </summary>
        public string Http403()
        {
            string message = RouteData.Values["exception"].ToString();
            bool isAjax = Request.Headers["x-requested-with"] == null ? false : true;//判断是否ajax请求
            if (!isAjax)
            {
                Response.Redirect(Url.Action("NoRightIndex", "Home", new { area = "Admin" }));
                Response.End();
                //return Redirect("/Login.html");
            }

            string re = JSON.ToJSON(new JSResponse("403", message), jsonParams);
            return re;
        }

        /// <summary>
        /// 没有登录
        /// </summary>
        /// <returns>ActionResult.</returns>
        public string Http401()
        {
            string message = RouteData.Values["exception"].ToString();
            bool isAjax = Request.Headers["x-requested-with"] == null ? false : true;//判断是否ajax请求
            if (!isAjax)
            {
                Response.Redirect(Url.Action("LoginIndex", "Home", new { area = "Admin" }));
                Response.End();
                //return Redirect("/Login.html");
            }

            string re = JSON.ToJSON(new JSResponse("401", message), jsonParams);
            return re;
        }


        public ActionResult Http500()
        {
            string message = RouteData.Values["exception"].ToString();
            ContentResult res = new ContentResult()
            {
                Content = JSON.ToJSON(new JSResponse("服务器出错！", "500", message), jsonParams)
            };
            return res;
        }
    }
}
