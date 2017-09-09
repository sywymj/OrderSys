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

namespace OrderSys.Admin.Controllers
{
    public class StaffController : AdminBaseController
    {
        //
        // GET: /Staff/
        UserService userService = new UserService();

        #region Index
        public ActionResult Index()
        {
            return View("~/Areas/Admin/Views/Staff/Index.cshtml");
        }

        [HttpGet]
        public ActionResult InsertIndex()
        {
            return View("~/Areas/Admin/Views/Staff/InsertIndex.cshtml");
        } 
        #endregion

        [HttpGet]
        public string GetList(int pageIndex, int pageSize, string sortField, string sortOrder)
        {
            int count = 0;

            Paging paging = new Paging(pageIndex, pageSize, sortField, sortOrder);
            DataTable re = userService.GetAllStaffs(paging, out count);

            string s = JSON.ToJSON(new JSResponse(new DataTableData(re, count)), jsonParams);
            return s;

        }




    }
}
