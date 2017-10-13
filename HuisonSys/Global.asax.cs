using FastJSON;
using JSNet.BaseSys;
using HuisonSys.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace HuisonSys
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //获取配置文件配置项
            BaseConfiguration.GetSetting();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var app = (MvcApplication)sender;
            var context = app.Context;
            var ex = app.Server.GetLastError();

            context.Response.Clear();
            context.ClearError();

            var routeData = new RouteData();
            routeData.Values["Exception"] = ex.Message;

            Type t = ex.GetType();
            if (t == typeof(HttpException))
            {
                var httpException = ex as HttpException;
                if (httpException != null)
                {
                    if (httpException.ErrorCode == 1001)
                    {
                        //微信端
                        switch (httpException.GetHttpCode())
                        {
                            case 404:
                                routeData.Values["Action"] = "Http404";
                                break;
                            case 401:  //没有登录
                                routeData.Values["Action"] = "VXHttp401";
                                break;
                            case 403:  //没有执行的权限
                                routeData.Values["Action"] = "VXHttp403";
                                break;
                        }
                    }
                    else
                    {
                        switch (httpException.GetHttpCode())
                        {
                            case 404:
                                routeData.Values["Action"] = "Http404";
                                break;
                            case 401:  //没有登录
                                routeData.Values["Action"] = "Http401";
                                break;
                            case 403:  //没有执行的权限
                                routeData.Values["Action"] = "Http403";
                                break;
                        }
                    }
                }
            }
            else if (t == typeof(JSException))
            {
                var jsException = ex as JSException;
                if (jsException != null)
                {
                    routeData.Values["ErrorCode"] = jsException.ErrorCode;
                    routeData.Values["ErrorMsg"] = jsException.ErrorMsg;
                    routeData.Values["Exception"] = jsException.Message;
                    routeData.Values["Action"] = "ShowErrorTips";
                }
            }
            else
            {
                var exception = ex;
                if (exception != null)
                {
                    routeData.Values["Exception"] = ex.ToString();
                    routeData.Values["Action"] = "Http500";
                }
            }

            IController controller = new ErrorController();
            controller.Execute(new RequestContext(new HttpContextWrapper(context), routeData));
        }
    }
}