using FastJSON;
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
    public class OrderController :AdminBaseController
    {
        //
        // GET: /Order/
        OrderService service = new OrderService();
        public ActionResult OrderWorkingLocationIndex()
        {
            return View("~/Areas/Admin/Views/Order/OrderWorkingLocation_Index.cshtml");
        }

        [HttpGet]
        public string GetOrderWorkingLocationDTByPage(int pageIndex, int pageSize, string sortField, string sortOrder)
        {
            int count = 0;
            Paging paging = new Paging(pageIndex, pageSize, sortField, sortOrder);
            DataTable re = service.GetOrderWorkingLocationDTByRole(service.CurrentRole, paging, out count);

            string s = JSON.ToJSON(new JSResponse(new DataTableData(re, count)), jsonParams);
            return s;
        }

        public string GetSingleOrderWorkingLocation(int orderWorkingLocationID)
        {
            ViewOrderWorkingLocation viewModel = new ViewOrderWorkingLocation();
            OrderWorkingLocationEntity entity = service.GetOrderWorkingLocation(orderWorkingLocationID);
            entity.CopyTo(viewModel);

            return JSON.ToJSON(new JSResponse(viewModel), jsonParams);
        }


        [HttpGet]
        public ActionResult InsertOrderWorkingLocationIndex()
        {
            return View("~/Areas/Admin/Views/Order/OrderWorkingLocation_InsertIndex.cshtml");
        }

        #region 【最简单】的新增
        [HttpPost]
        public string AddOrderWorkingLocation()
        {
            string s = JSRequest.GetRequestFormParm("ViewModel");
            ViewOrderWorkingLocation viewModel = FastJSON.JSON.ToObject<ViewOrderWorkingLocation>(s);

            //TODO 数据验证。
            Validate(viewModel);

            OrderWorkingLocationEntity entity = new OrderWorkingLocationEntity();
            viewModel.CopyTo(entity);
            service.AddOrderWorkingLocation(entity);
            return JSON.ToJSON(new JSResponse(ResponseType.Remind, "添加成功！"), jsonParams);
        }
        #endregion

        [HttpGet]
        public ActionResult EditOrderWorkingLocationIndex()
        {
            return View("~/Areas/Admin/Views/Order/OrderWorkingLocation_InsertIndex.cshtml");
        }

        #region 【最简单】的新增
        [HttpPost]
        public string EditOrderWorkingLocation()
        {
            //获取参数
            string s = JSRequest.GetRequestFormParm("ViewModel");
            ViewOrderWorkingLocation viewModel = FastJSON.JSON.ToObject<ViewOrderWorkingLocation>(s);

            //TODO 数据验证。
            Validate(viewModel);

            //ViewModel赋值
            OrderWorkingLocationEntity entity = new OrderWorkingLocationEntity();
            viewModel.CopyTo(entity);

            //调用Service
            service.EditOrderWorkingLocation(entity);
            return JSON.ToJSON(new JSResponse(ResponseType.Remind, "修改成功！"), jsonParams);
        }


        public string GetOrderWorkingLocationFirstLevelDDL()
        {
            List<string> list = service.GetOrderWorkingLocationFirstLevelList(service.CurrentRole);

            var re = list.Select(l =>
                new OrderWorkingLocationDDL()
                {
                    ID = l,
                    Title = l
                }).ToList();

            string s = JSON.ToJSON(new JSResponse(re), jsonParams);
            return s;
        } 
        #endregion

        public string DeleteOrderWorkingLocation()
        {
            string sOrderWorkingLocation = JSRequest.GetRequestUrlParm("orderWorkingLocationIDs", true);
            int[] orderWorkingLocationIDs = JSValidator.ValidateStrings("ID格式有误！", sOrderWorkingLocation, false);

            service.DeleteOrderWorkingLocation(orderWorkingLocationIDs);
            return JSON.ToJSON(new JSResponse(ResponseType.Remind, "删除成功！"), jsonParams);
        }


        private void Validate(ViewOrderWorkingLocation viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.FirstLevel))
            {
                throw new JSException("一级分类不能为空！");
            }
            if (string.IsNullOrEmpty(viewModel.ScecondLevel))
            {
                viewModel.ScecondLevel = string.Empty;
            }
            if (viewModel.OrganizeID == 0)
            {
                throw new JSException("请选择所属机构！");
            }
        }
    }
}
