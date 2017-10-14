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
using JSNet.Utilities;

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

            //Log4net配置
            string configPath = Server.MapPath("~/XML/Log4Net.xml");//指定配置文件路径
            Log4NetUtil.Init(configPath, "SystemLog");//初始化指定的logger
            Log4NetUtil.SetConnString("AdoNetAppender_SQLServer", BaseSystemInfo.CenterDbConnectionString, "SystemLog");//配置数据库路径
            Log4NetUtil.SetDefatultLog("SystemLog");//配置默认的logger
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var app = (MvcApplication)sender;
            var context = app.Context;
            var ex = app.Server.GetLastError();

            context.Response.Clear();
            context.ClearError();

            var routeData = new RouteData();
            routeData.Values["Tips"] = ex.Message;

            Type t = ex.GetType();
            if (t == typeof(HttpException))
            {
                var httpException = ex as HttpException;
                if (httpException != null)
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
            else if (t == typeof(JSException))
            {
                var jsException = ex as JSException;
                if (jsException != null)
                {
                    routeData.Values["ErrorCode"] = jsException.ErrorCode == null ? "" : jsException.ErrorCode;
                    routeData.Values["ErrorMsg"] = jsException.ErrorMsg == null ? "" : jsException.ErrorMsg;
                    routeData.Values["Tips"] = jsException.Message == null ? "" : jsException.Message;
                    routeData.Values["Action"] = "ShowErrorTips";
                }
            }
            else
            {
                var exception = ex;
                if (exception != null)
                {
                    Log4NetUtil.Error(ex.ToString());//使用default logger
                    routeData.Values["Tips"] = ex.ToString();
                    routeData.Values["Action"] = "Http500";
                }
            }

            IController controller = new ErrorController();
            controller.Execute(new RequestContext(new HttpContextWrapper(context), routeData));
        }
    }
}