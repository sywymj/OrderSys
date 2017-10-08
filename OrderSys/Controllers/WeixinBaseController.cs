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
    public class WeixinBaseController : Controller
    {
        protected JSONParameters jsonParams = new JSONParameters()
        {
            UseUTCDateTime = false,
            UsingGlobalTypes = false,
            UseExtensions = false,
        };

        //[HttpGet]
        //public ActionResult GetHeader()
        //{
        //    PermissionService permissionService = new PermissionService();
        //    DataTable dt = permissionService.GetLeftMenu(permissionService.CurrentRole, "OrderSys_VX",false);
        //    dt.Columns["Resource_ID"].ColumnName = "ID";
        //    dt.Columns["Resource_ParentID"].ColumnName = "ParentID";
        //    dt.Columns["Resource_FullName"].ColumnName = "Title";
        //    dt.Columns["Resource_NavigateUrl"].ColumnName = "Url";
        //    dt.Columns["Resource_ImagUrl"].ColumnName = "ImagUrl";

        //    DataTable re = dt.DefaultView.ToTable(false, new string[] { "ID", "ParentID", "Title", "Url", "ImagUrl" });
        //    ViewBag.StaffName = permissionService.CurrentStaff.Name;
        //    return PartialView("_Header", re);
        //}

        //[HttpGet]
        //public ActionResult GetOrderStatus()
        //{
        //    Dictionary<int, string> dic = EnumExtensions.ConvertToDic<OrderStatus>();

        //    ContentResult res = new ContentResult();
        //    res.Content = JSON.ToJSON(new JSResponse(dic), jsonParams);
        //    return res;
        //}

        //[HttpGet]
        //public ActionResult GetOrderHandleType()
        //{
        //    Dictionary<int, string> dic = EnumExtensions.ConvertToDic<OrderHandleType>();

        //    ContentResult res = new ContentResult();
        //    res.Content = JSON.ToJSON(new JSResponse(dic), jsonParams);
        //    return res;

        //    //return PartialView("AddHandleDetail", dic);
        //}
    }
}
