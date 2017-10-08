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
            routeData.Values["exception"] = ex.Message;

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
                                routeData.Values["action"] = "Http404";
                                break;
                            case 401:  //没有登录
                                routeData.Values["action"] = "VXHttp401";
                                break;
                            case 403:  //没有执行的权限
                                routeData.Values["action"] = "VXHttp403";
                                break;
                        }
                    }
                    else
                    {
                        switch (httpException.GetHttpCode())
                        {
                            case 404:
                                routeData.Values["action"] = "Http404";
                                break;
                            case 401:  //没有登录
                                routeData.Values["action"] = "Http401";
                                break;
                            case 403:  //没有执行的权限
                                routeData.Values["action"] = "Http403";
                                break;
                        }
                    }
                }
            }
            else if (t == typeof(JSException))
            {
                var oasysException = ex as JSException;
                if (oasysException != null)
                {
                    routeData.Values["action"] = "ShowErrorTips";
                }
            }
            else
            {
                var exception = ex;
                if (exception != null)
                {
                    routeData.Values["exception"] = ex.ToString();
                    routeData.Values["action"] = "Http500";
                }
            }

            IController controller = new ErrorController();
            controller.Execute(new RequestContext(new HttpContextWrapper(context), routeData));
        }
    }
}