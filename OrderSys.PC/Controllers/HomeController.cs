﻿using FastJSON;
using JSNet.BaseSys;
using JSNet.Model;
using JSNet.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderSys.Admin.Controllers
{
    [ManagerAuthorize]
    public class HomeController : AdminBaseController
    {
        //
        // GET: /Home/
        private PermissionService permissionService = new PermissionService();

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index()
        {
            return View("~/Areas/Admin/Views/Home/Index.cshtml");
        }

        [HttpGet]
        public string GetLeftTree()
        {
            RoleEntity role = new MyRoleService().GetCurrentRole();
            DataTable dt = permissionService.GetLeftMenu(role, "OrderSys_PC");
            dt.Columns["Resource_ID"].ColumnName = "ID";
            dt.Columns["Resource_ParentID"].ColumnName = "ParentID";
            dt.Columns["Resource_FullName"].ColumnName = "Title";
            dt.Columns["Resource_NavigateUrl"].ColumnName = "Url";
            dt.Columns["Resource_ImagUrl"].ColumnName = "ImagUrl";

            DataTable re = dt.DefaultView.ToTable(false, new string[] { "ID", "ParentID", "Title", "Url", "ImagUrl" });
            return JSON.ToJSON(new JSResponse(re),jsonParams);
        }

        [HttpGet]
        public string GetButton()
        {
            //获取当前请求的URL地址
            //HttpRequest
            string url = JSRequest.GetRequestUrlParm("NavigateUrl");


            RoleEntity role = new MyRoleService().GetCurrentRole();
            DataTable re  = permissionService.GetButtons(role,url);

            string s = JSON.ToJSON(new JSResponse(new DataTableData(re)), jsonParams);
            return s;
        }

        [AllowAnonymous]
        [HttpGet]
        public ViewResult LoginIndex()
        {
            return View("~/Areas/Admin/Views/Home/Login_Index.cshtml");
        }

        [AllowAnonymous]
        [HttpGet]
        public ViewResult NoRightIndex()
        {
            return View("~/Areas/Admin/Views/Home/NoRight_Index.cshtml");
        }

        [AllowAnonymous]
        [HttpPost]
        public string Login()
        {
            string sUserName = JSRequest.GetRequestFormParm("UserName");
            string sPassword = JSRequest.GetRequestFormParm("Password");
            string userName = JSValidator.ValidateString("用户名", sUserName, true);
            string pwd = JSValidator.ValidateString("密码", sPassword, true);

            UserService userService = new UserService();
            userService.Login(userName, pwd);

            string s = JSON.ToJSON(new JSResponse(ResponseType.Remind, "登陆成功！"), jsonParams);
            return s;
        }

        [AllowAnonymous]
        [HttpGet]
        public string Logout()
        {
            UserService userService = new UserService();
            userService.Logout();

            string s = JSON.ToJSON(new JSResponse(ResponseType.Remind, "登出成功！"), jsonParams);
            return s;
        }
        
    }
}
