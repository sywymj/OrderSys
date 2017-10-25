﻿using FastJSON;
using JSNet.BaseSys;
using JSNet.Service;
using JSNet.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderSys.Controllers
{
    
    public class HomeController : WeixinBaseController
    {
        //
        // GET: /Home/
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index(string openID)
        {
            LoginService loginService = new LoginService();
            loginService.VXLogin(openID);
            return View();
        }

        [ManagerAuthorize(Roles="public")]
        [HttpGet]
        public ActionResult GetHeader()
        {
            PermissionService permissionService = new PermissionService();
            DataTable dt = permissionService.GetLeftMenu(permissionService.CurrentRole, "OrderSys_VX", false);
            dt.Columns["Resource_ID"].ColumnName = "ID";
            dt.Columns["Resource_ParentID"].ColumnName = "ParentID";
            dt.Columns["Resource_FullName"].ColumnName = "Title";
            dt.Columns["Resource_NavigateUrl"].ColumnName = "Url";
            dt.Columns["Resource_ImagUrl"].ColumnName = "ImagUrl";

            DataTable re = dt.DefaultView.ToTable(false, new string[] { "ID", "ParentID", "Title", "Url", "ImagUrl" });
            ViewBag.StaffName = permissionService.CurrentStaff.Name;
            return PartialView("_Header", re);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetOrderStatus()
        {
            Dictionary<int, string> dic = EnumExtensions.ConvertToDic<OrderStatus>();

            ContentResult res = new ContentResult();
            res.Content = JSON.ToJSON(new JSResponse(dic), jsonParams);
            return res;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult GetOrderHandleType()
        {
            Dictionary<int, string> dic = EnumExtensions.ConvertToDic<OrderHandleType>();

            

            ContentResult res = new ContentResult();
            res.Content = JSON.ToJSON(new JSResponse(dic), jsonParams);
            return res;

            //return PartialView("AddHandleDetail", dic);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult FilterIndex(string tabid)
        {
            Dictionary<string, Dictionary<int, string>> dic = new Dictionary<string, Dictionary<int, string>>();
            Dictionary<int, string> statusDic = new Dictionary<int, string>();
            switch (tabid)
            {
                case "query_mystarted_btn":
                    statusDic.Add((int)OrderStatus.Appointing,EnumExtensions.ToDescription(OrderStatus.Appointing));
                    statusDic.Add((int)OrderStatus.Receving,EnumExtensions.ToDescription(OrderStatus.Receving));
                    statusDic.Add((int)OrderStatus.Handling,EnumExtensions.ToDescription(OrderStatus.Handling));
                    statusDic.Add((int)OrderStatus.Checking,EnumExtensions.ToDescription(OrderStatus.Checking));
                    statusDic.Add((int)OrderStatus.Finish,EnumExtensions.ToDescription(OrderStatus.Finish));
                    statusDic.Add((int)OrderStatus.Rejected,EnumExtensions.ToDescription(OrderStatus.Rejected));
                    statusDic.Add((int)OrderStatus.Canceled,EnumExtensions.ToDescription(OrderStatus.Canceled));
                    dic.Add("filterStatus", statusDic);
                    break;
                case "query_myappointed_btn":
                    statusDic.Add((int)OrderStatus.Receving, EnumExtensions.ToDescription(OrderStatus.Receving));
                    statusDic.Add((int)OrderStatus.Handling, EnumExtensions.ToDescription(OrderStatus.Handling));
                    statusDic.Add((int)OrderStatus.Checking, EnumExtensions.ToDescription(OrderStatus.Checking));
                    statusDic.Add((int)OrderStatus.Finish, EnumExtensions.ToDescription(OrderStatus.Finish));
                    statusDic.Add((int)OrderStatus.Rejected, EnumExtensions.ToDescription(OrderStatus.Rejected));
                    statusDic.Add((int)OrderStatus.Canceled, EnumExtensions.ToDescription(OrderStatus.Canceled));
                    dic.Add("filterStatus", statusDic);
                    break;
                case "query_myappointing_btn":
                    break;
                case "query_myreciving_btn":
                    break;
                case "query_myhandling_btn":
                    statusDic.Add((int)OrderStatus.Handling, EnumExtensions.ToDescription(OrderStatus.Handling));
                    statusDic.Add((int)OrderStatus.Rejected, EnumExtensions.ToDescription(OrderStatus.Rejected));
                    //statusDic.Add((int)OrderStatus.Canceled, EnumExtensions.ToDescription(OrderStatus.Canceled));
                    dic.Add("filterStatus", statusDic);
                    break;
                case "query_myhandled_btn":
                    statusDic.Add((int)OrderStatus.Checking, EnumExtensions.ToDescription(OrderStatus.Checking));
                    statusDic.Add((int)OrderStatus.Finish, EnumExtensions.ToDescription(OrderStatus.Finish));
                    dic.Add("filterStatus", statusDic);
                    break;
                
            }

            return PartialView("/Areas/Weixin/Views/Shared/_Filter.cshtml", dic);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult LoginIndex(string msg,string url)
        {
            if (string.IsNullOrEmpty(msg))
            {
                msg = "超时请重新登陆！";
            }
            ViewBag.ErrMsg = msg;
            ViewBag.Url = url;

            return View();
        }
    }
}
