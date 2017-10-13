using FastJSON;
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
        [HttpGet]
        public ActionResult Index(string openID)
        {
            UserService userService = new UserService();
            userService.VXLogin(openID);
            return View();
        }

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

        [HttpGet]
        public ActionResult GetOrderStatus()
        {
            Dictionary<int, string> dic = EnumExtensions.ConvertToDic<OrderStatus>();

            ContentResult res = new ContentResult();
            res.Content = JSON.ToJSON(new JSResponse(dic), jsonParams);
            return res;
        }

        [HttpGet]
        public ActionResult GetOrderHandleType()
        {
            Dictionary<int, string> dic = EnumExtensions.ConvertToDic<OrderHandleType>();

            

            ContentResult res = new ContentResult();
            res.Content = JSON.ToJSON(new JSResponse(dic), jsonParams);
            return res;

            //return PartialView("AddHandleDetail", dic);
        }

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
    }
}
