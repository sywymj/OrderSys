using JSNet.BaseSys;
using JSNet.Model;
using JSNet.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderSys
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
                userService.ChkVXLogin(out user, out role);
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
                throw new HttpException(401, "登录超时请重新登录！", 1001);
            }
            else if (status == PermissionStatus.NoRight.ToString())
            {
                //无权限，返回json前端处理
                throw new HttpException(403, "没有操作权限！",1001);
            }
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
        }
    }
}