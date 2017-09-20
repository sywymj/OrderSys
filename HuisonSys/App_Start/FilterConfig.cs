using JSNet.BaseSys;
using JSNet.Model;
using JSNet.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace HuisonSys
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
        }
    }


    public class ManagerAuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //return true;
            var controllername = httpContext.Request.RequestContext.RouteData.Values["controller"].ToString().ToLower();
            var actionname = httpContext.Request.RequestContext.RouteData.Values["action"].ToString().ToLower();

            UserService userService = new UserService();
            MyRoleService roleService = new MyRoleService();
            PermissionService permissionService = new PermissionService();

            UserEntity user = userService.GetCurrentUser();//获取当前用户

            RoleEntity role = roleService.GetCurrentRole(user);//获取当前用户角色
            bool b = permissionService.IsPermissionAuthorizedByRole(role, controllername, actionname);

            if (!b)
            {
                httpContext.Response.StatusDescription = PermissionStatus.NoLogin.ToString();
                return false;
            }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            string status = filterContext.HttpContext.Response.StatusDescription;
            if (status == PermissionStatus.NoLogin.ToString())
            {
                //没登录直接报错，返回json前端处理
                throw new HttpException(401, "登录超时请重新登录！");
            }
            else if (status == PermissionStatus.NoRight.ToString())
            {
                //无权限，返回json前端处理
                throw new HttpException(403, "没有操作权限！");
            }




            //if (filterContext.HttpContext.Response.StatusCode == 403)
            //{
            //    string status =filterContext.HttpContext.Response.StatusDescription;
            //    filterContext.HttpContext.Response.StatusCode = 200;
            //    if(status==Common.HtmlStatus.nologin.ToString())
            //    {
            //        //没登录直接报错，返回json前端处理
            //        throw new HttpException(401, "登录超时请重新登录！");
            //    }
            //    else if(status==Common.HtmlStatus.noright.ToString())
            //    {
            //        //无权限，返回json前端处理
            //        throw new HttpException(403, "没有操作权限！");
            //    }
            //}
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //问题，401貌似必须要重定向！返回的statuscode是200，返回前端是成功状态（因为如果是ajax请求，返回重定向即返回网页内容，会导致前端接收的不是Json格式）
            //最理想方案 返回401时，可以报错，直接调用Application_Error，返回前端是失败状态，通过前端来重定向

            //解决方案一：将html状态码全部改为403，抛出异常，返回json，目前的方案
            //base.HandleUnauthorizedRequest(filterContext);
            //if (filterContext == null)
            //{
            //    throw new ArgumentNullException("filterContext");
            //}
            //else
            //{
            //    if (filterContext.HttpContext.Response.StatusCode == 403)
            //    {
            //        //没登录直接报错，返回json前端处理
            //        throw new HttpException(403, "没有操作权限！");
            //    }
            //    filterContext.HttpContext.Response.StatusCode = 401;
            //    throw new HttpException(401, "登录超时请重新登录！");
            //}
        }
    }
}