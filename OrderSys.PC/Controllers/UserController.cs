﻿using FastJSON;
using JSNet.BaseSys;
using JSNet.Model;
using JSNet.Service;
using JSNet.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderSys.Admin.Controllers
{
    [ManagerAuthorize]
    public class UserController : AdminBaseController
    {
        //
        // GET: /User/
        UserService service = new UserService();

        #region INDEX
        public ActionResult Index()
        {
            return View("~/Areas/Admin/Views/User/Index.cshtml");
        }
        [HttpGet]
        public string GetListByPage(int pageIndex, int pageSize, string sortField, string sortOrder)
        {
            int count = 0;
            Paging paging = new Paging(pageIndex, pageSize, sortField, sortOrder);
            DataTable re = service.GetUserDTForShow(service.CurrentRole, paging, out count);

            string s = JSON.ToJSON(new JSResponse(new DataTableData(re, count)), jsonParams);
            return s;

        }
        #endregion

        #region INSERT
        [HttpGet]
        public ActionResult InsertIndex()
        {
            return View("~/Areas/Admin/Views/User/InsertIndex.cshtml");
        }

        [HttpPost]
        public string Add()
        {
            string sRoleIDs = JSRequest.GetRequestFormParm("roleIDs");
            string s = JSRequest.GetRequestFormParm("viewModel");
            ViewUser viewModel = FastJSON.JSON.ToObject<ViewUser>(s);

            //TODO 数据验证。
            int[] roleIDs = JSValidator.ValidateStrings("角色ID", sRoleIDs, true);
            JSValidator.ValidateTel("手机号码", viewModel.Staff.Tel);

            UserEntity user = new UserEntity();
            StaffEntity staff = new StaffEntity();
            viewModel.CopyTo(user);
            viewModel.Staff.CopyTo(staff);

            service.AddUser(user, staff, roleIDs);
            return JSON.ToJSON(new JSResponse(ResponseType.Remind, "添加成功！"), jsonParams);
        } 
        #endregion

        #region EDIT
        [HttpGet]
        public ActionResult EditIndex()
        {
            return View("~/Areas/Admin/Views/User/InsertIndex.cshtml");
        }
        [HttpPost]
        public string Edit()
        {
            string sRoleIDs = JSRequest.GetRequestFormParm("roleIDs");
            string s = JSRequest.GetRequestFormParm("viewModel");
            ViewUser viewModel = FastJSON.JSON.ToObject<ViewUser>(s);

            //TODO 数据验证。
            int[] roleIDs = JSValidator.ValidateStrings("角色ID", sRoleIDs, true);
            JSValidator.ValidateTel("手机号码", viewModel.Staff.Tel);

            UserEntity user = new UserEntity();
            StaffEntity staff = new StaffEntity();
            viewModel.CopyTo(user);
            viewModel.Staff.CopyTo(staff);

            service.EditUser(user, staff, roleIDs);
            return JSON.ToJSON(new JSResponse(ResponseType.Remind, "修改成功！"), jsonParams);
        }

        [HttpGet]
        public string GetSingle(int userID)
        {
            ViewUser viewModel = new ViewUser();
            MyRoleService roleService = new MyRoleService();

            UserEntity user = service.GetUser(userID);
            StaffEntity staff = service.GetStaffByUserID((int)user.ID);
            int[] roleIDs = roleService.GetGrantedRoleIDs((int)user.ID);

            user.CopyTo(viewModel);
            viewModel.Staff = staff;
            viewModel.RoleIDs = string.Join(",", Array.ConvertAll<int, string>(roleIDs, s => s.ToString()));

            return JSON.ToJSON(new JSResponse(viewModel), jsonParams);
        } 
        #endregion

        [HttpGet]
        public string AddWeixinUser()
        {
            string sUserIDs = JSRequest.GetRequestUrlParm("UserIDs");
            int[] userIDs = JSValidator.ValidateStrings("用户ID",sUserIDs,true);

            service.AddWeiXinUser(userIDs);
            return JSON.ToJSON(new JSResponse(ResponseType.Remind, "操作成功！"), jsonParams);
        }


        #region
        [HttpPost]
        public string ImportUser()
        {
            HttpFileCollectionBase files = Request.Files;
            if (files.Count == 0)
            {
                return JSON.ToJSON(new JSResponse(ResponseType.Error, "请先选择文件！"), jsonParams);
            }

            string localFullName = string.Empty;
            UploadService uploadService = new UploadService();
            uploadService.FileSaveAsExcel(files[0], out localFullName);

            string result = string.Empty;
            DataTable dt = NPOIHelper.ImportExcel(localFullName, 0);
            service.Import(dt,out result);

            string url = "http://" + HttpContext.Request.Url.Authority + result;
            return JSON.ToJSON(new JSResponse(ResponseType.Message, "导入成功！", data: url), jsonParams);
        }
        #endregion

        #region VERIFY
        [AllowAnonymous]
        [HttpGet]
        public string VerifyUserName(string userName, string userID)
        {
            if (service.ChkUserNameExist(userName, userID))
            {
                return JSON.ToJSON(new JSResponse(false), jsonParams);
            }
            return JSON.ToJSON(new JSResponse(true), jsonParams);
        }

        [AllowAnonymous]
        [HttpGet]
        public string VerifyTel(string tel, string userID)
        {
            if (service.ValidateTel(tel, userID))
            {
                return JSON.ToJSON(new JSResponse(false), jsonParams);
            }
            return JSON.ToJSON(new JSResponse(true), jsonParams);

        }
        #endregion
    }
}
