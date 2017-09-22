using JSNet.BaseSys;
using JSNet.Model;
using JSNet.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderSys.Admin
{
    public class ManagerAuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //return true;
            var controllername = httpContext.Request.RequestContext.RouteData.Values["controller"].ToString().ToLower();
            var actionname = httpContext.Request.RequestContext.RouteData.Values["action"].ToString().ToLower();

            UserService userService = new UserService();
            UserEntity user = new UserEntity();
            RoleEntity role = new RoleEntity();
            try
            {
                userService.ChkLogin(out user, out role);
            }
            catch (JSException e)
            {
                httpContext.Response.StatusDescription = PermissionStatus.NoLogin.ToString();
                return false;
            }

            PermissionService permissionService = new PermissionService();
            bool b = permissionService.IsPermissionAuthorizedByRole(role, controllername, actionname);
            if (!b)
            {
                httpContext.Response.StatusDescription = PermissionStatus.NoRight.ToString();
                return false;
            }
            return true;
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