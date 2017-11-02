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

        #region 【查询】工作地点
        [HttpGet]
        public string GetOrderWorkingLocationDTByPage(int pageIndex, int pageSize, string sortField, string sortOrder)
        {
            int count = 0;
            Paging paging = new Paging(pageIndex, pageSize, sortField, sortOrder);
            DataTable re = service.GetOrderWorkingLocationDTByRole(service.CurrentRole, paging, out count);

            string s = JSON.ToJSON(new JSResponse(new DataTableData(re, count)), jsonParams);
            return s;
        }

        [HttpGet]
        public string GetSingleOrderWorkingLocation(int orderWorkingLocationID)
        {
            ViewOrderWorkingLocation viewModel = new ViewOrderWorkingLocation();
            OrderWorkingLocationEntity entity = service.GetOrderWorkingLocation(orderWorkingLocationID);
            entity.CopyTo(viewModel);

            return JSON.ToJSON(new JSResponse(viewModel), jsonParams);
        } 
        #endregion

        #region 【新增】工作地点
        [HttpGet]
        public ActionResult InsertOrderWorkingLocationIndex()
        {
            return View("~/Areas/Admin/Views/Order/OrderWorkingLocation_InsertIndex.cshtml");
        }

        [HttpPost]
        public string AddOrderWorkingLocation()
        {
            string s = JSRequest.GetRequestFormParm("ViewModel");
            ViewOrderWorkingLocation viewModel = FastJSON.JSON.ToObject<ViewOrderWorkingLocation>(s);

            //TODO 数据验证。
            ValidateWorkingLocation(viewModel);

            OrderWorkingLocationEntity entity = new OrderWorkingLocationEntity();
            viewModel.CopyTo(entity);
            service.AddOrderWorkingLocation(entity);
            return JSON.ToJSON(new JSResponse(ResponseType.Remind, "添加成功！"), jsonParams);
        }
        #endregion

        #region 【编辑】工作地点
        [HttpGet]
        public ActionResult EditOrderWorkingLocationIndex()
        {
            return View("~/Areas/Admin/Views/Order/OrderWorkingLocation_InsertIndex.cshtml");
        }

        [HttpPost]
        public string EditOrderWorkingLocation()
        {
            //获取参数
            string s = JSRequest.GetRequestFormParm("ViewModel");
            ViewOrderWorkingLocation viewModel = FastJSON.JSON.ToObject<ViewOrderWorkingLocation>(s);

            //TODO 数据验证。
            ValidateWorkingLocation(viewModel);

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

        #region 【删除】工作地点
        public string DeleteOrderWorkingLocation()
        {
            string sOrderWorkingLocation = JSRequest.GetRequestUrlParm("orderWorkingLocationIDs", true);
            int[] orderWorkingLocationIDs = JSValidator.ValidateStrings("ID格式有误！", sOrderWorkingLocation, false);

            service.DeleteOrderWorkingLocation(orderWorkingLocationIDs);
            return JSON.ToJSON(new JSResponse(ResponseType.Remind, "删除成功！"), jsonParams);
        } 
        #endregion

        public ActionResult OrderGoodsIndex()
        {
            return View("~/Areas/Admin/Views/Order/OrderGoods_Index.cshtml");
        }

        #region 【查询】物品
        [HttpGet]
        public string GetOrderGoodsDTByPage(int pageIndex, int pageSize, string sortField, string sortOrder)
        {
            int count = 0;
            Paging paging = new Paging(pageIndex, pageSize, sortField, sortOrder);
            DataTable re = service.GetOrderGoodsDTByRole(service.CurrentRole, paging, out count);

            string s = JSON.ToJSON(new JSResponse(new DataTableData(re, count)), jsonParams);
            return s;
        }

        [HttpGet]
        public string GetSingleOrderGoods(int OrderGoodsID)
        {
            ViewOrderGoods viewModel = new ViewOrderGoods();
            OrderGoodsEntity entity = service.GetOrderGoods(OrderGoodsID);
            entity.CopyTo(viewModel);

            return JSON.ToJSON(new JSResponse(viewModel), jsonParams);
        } 
        #endregion

        #region 【新增】物品
        [HttpGet]
        public ActionResult InsertOrderGoodsIndex()
        {
            return View("~/Areas/Admin/Views/Order/OrderGoods_InsertIndex.cshtml");
        }

        [HttpPost]
        public string AddOrderGoods()
        {
            string s = JSRequest.GetRequestFormParm("ViewModel");
            ViewOrderGoods viewModel = FastJSON.JSON.ToObject<ViewOrderGoods>(s);

            //TODO 数据验证。
            ValidateGoods(viewModel);

            OrderGoodsEntity entity = new OrderGoodsEntity();
            viewModel.CopyTo(entity);
            service.AddOrderGoods(entity);
            return JSON.ToJSON(new JSResponse(ResponseType.Remind, "添加成功！"), jsonParams);
        }
        #endregion

        #region 【编辑】物品
        [HttpGet]
        public ActionResult EditOrderGoodsIndex()
        {
            return View("~/Areas/Admin/Views/Order/OrderGoods_InsertIndex.cshtml");
        }

        [HttpPost]
        public string EditOrderGoods()
        {
            //获取参数
            string s = JSRequest.GetRequestFormParm("ViewModel");
            ViewOrderGoods viewModel = FastJSON.JSON.ToObject<ViewOrderGoods>(s);

            //TODO 数据验证。
            ValidateGoods(viewModel);

            //ViewModel赋值
            OrderGoodsEntity entity = new OrderGoodsEntity();
            viewModel.CopyTo(entity);

            //调用Service
            service.EditOrderGoods(entity);
            return JSON.ToJSON(new JSResponse(ResponseType.Remind, "修改成功！"), jsonParams);
        }
        #endregion

        #region 【删除】物品
        public string DeleteOrderGoods()
        {
            string sOrderGoods = JSRequest.GetRequestUrlParm("OrderGoodsIDs", true);
            int[] OrderGoodsIDs = JSValidator.ValidateStrings("ID格式有误！", sOrderGoods, false);

            service.DeleteOrderGoods(OrderGoodsIDs);
            return JSON.ToJSON(new JSResponse(ResponseType.Remind, "删除成功！"), jsonParams);
        } 
        #endregion

        private void ValidateWorkingLocation(ViewOrderWorkingLocation viewModel)
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

        private void ValidateGoods(ViewOrderGoods viewModel)
        {

        } 
    }
}
