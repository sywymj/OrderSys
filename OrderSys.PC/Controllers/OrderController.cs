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


        public ActionResult OrdersIndex()
        {
            ViewBag.StatusNames = JSON.ToJSON(EnumExtensions.ConvertToDic<OrderStatus>(), jsonParams); ;
            ViewBag.PriorityNames = JSON.ToJSON(EnumExtensions.ConvertToDic<OrderPriority>(), jsonParams);
            return View("~/Areas/Admin/Views/Order/Orders_Index.cshtml");
        }

        #region 【查询】工单
        [HttpGet]
        public string GetOrdersDTByPage(int pageIndex, int pageSize)
        {
            JSDictionary dic = MakeFilterOfOrders();

            int count = 0;
            DataTable re = service.GetOrdersDTByRoles(service.CurrentRole, dic, pageIndex, pageSize, out count);

            string s = JSON.ToJSON(new JSResponse(new DataTableData(re, count)), jsonParams);
            return s;
        }
        #endregion

        #region 【导出】工单
        [HttpGet]
        public string ExportOrders()
        {
            JSDictionary dic = MakeFilterOfOrders();

            string path = service.ExportOrders(service.CurrentRole, dic);
            string url = "http://" + HttpContext.Request.Url.Authority + path;
            return JSON.ToJSON(new JSResponse(ResponseType.Redict, "导出成功！", data: url), jsonParams);
        } 
        #endregion

        #region 【统计】工单

        [HttpGet]
        public ActionResult StartStatisticsIndex()
        {
            return View("~/Areas/Admin/Views/Order/OrderStartStatistics_Index.cshtml");
        }

        [HttpGet]
        public string StartStatistics()
        {
            JSDictionary dic = MakeFilterOfStartStatistics();
            DataTable dt = service.StartStatistics(dic);

            string s = JSON.ToJSON(new JSResponse(new DataTableData(dt, 0)), jsonParams);
            return s;
        }


        [HttpGet]
        public ActionResult HandleStatisticsIndex()
        {
            return View("~/Areas/Admin/Views/Order/OrderHandleStatistics_Index.cshtml");
        }

        [HttpGet]
        public string HandleStatistics()
        {
            JSDictionary dic = MakeFilterOfHandleStatistics();
            DataTable dt = service.HandleStatistics(dic);

            string s = JSON.ToJSON(new JSResponse(new DataTableData(dt, 0)), jsonParams);
            return s;
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

        private JSDictionary MakeFilterOfOrders()
        {
            #region 获取参数
            string sStartTime1 = JSRequest.GetRequestUrlParm("StartTime1", false);
            string sStartTime2 = JSRequest.GetRequestUrlParm("StartTime2", false);
            string sFinishTime1 = JSRequest.GetRequestUrlParm("FinishTime1", false);
            string sFinishTime2 = JSRequest.GetRequestUrlParm("FinishTime2", false);
            #endregion

            #region 验证参数
            DateTime? startTime1 = JSValidator.ValidateDateTime("发起时间", sStartTime1);
            DateTime? startTime2 = JSValidator.ValidateDateTime("发起时间", sStartTime2);
            DateTime? finishTime1 = JSValidator.ValidateDateTime("完成时间", sFinishTime1);
            DateTime? finishTime2 = JSValidator.ValidateDateTime("完成时间", sFinishTime2);
            #endregion

            #region 构造搜索条件
            JSDictionary dic = new JSDictionary();

            if (startTime1 != null) { dic.Add(OrderEntity.FieldStartTime + "1", startTime1); }
            if (startTime2 != null) { dic.Add(OrderEntity.FieldStartTime + "2", startTime2); }
            if (finishTime1 != null) { dic.Add(OrderEntity.FieldFinishTime + "1", finishTime1); }
            if (finishTime2 != null) { dic.Add(OrderEntity.FieldFinishTime + "2", finishTime2); }
            #endregion

            return dic;
        }

        private JSDictionary MakeFilterOfStartStatistics() 
        {
            #region 获取参数
            string sStartTime1 = JSRequest.GetRequestUrlParm("StartTime1", false);
            string sStartTime2 = JSRequest.GetRequestUrlParm("StartTime2", false);
            string sStarterOrganizeID = JSRequest.GetRequestUrlParm("StarterOrganizeID", true);
            #endregion

            #region 验证参数
            DateTime? startTime1 = JSValidator.ValidateDateTime("发起时间", sStartTime1,false);
            DateTime? startTime2 = JSValidator.ValidateDateTime("发起时间", sStartTime2,false);
            int? starterOrganizeID = JSValidator.ValidateInt("部门", sStarterOrganizeID, true);
            #endregion

            #region 构造搜索条件
            JSDictionary dic = new JSDictionary();

            if (startTime1 != null) { dic.Add("StartTime1", startTime1); }
            if (startTime2 != null) { dic.Add("StartTime2", startTime2); }
            if (starterOrganizeID != null) { dic.Add("StarterOrganizeID", starterOrganizeID); }
            #endregion

            return dic;
        }

        private JSDictionary MakeFilterOfHandleStatistics()
        {
            #region 获取参数
            string sHandleTime1 = JSRequest.GetRequestUrlParm("HandleTime1", false);
            string sHandleTime2 = JSRequest.GetRequestUrlParm("HandleTime2", false);
            string sHandlerOrganizeID = JSRequest.GetRequestUrlParm("HandlerOrganizeID", true);
            #endregion

            #region 验证参数
            DateTime? handleTime1 = JSValidator.ValidateDateTime("处理时间", sHandleTime1, false);
            DateTime? handleTime2 = JSValidator.ValidateDateTime("处理时间", sHandleTime2, false);
            int? handlerOrganizeID = JSValidator.ValidateInt("部门", sHandlerOrganizeID, true);
            #endregion

            #region 构造搜索条件
            JSDictionary dic = new JSDictionary();

            if (handleTime1 != null) { dic.Add("HandleTime1", handleTime1); }
            if (handleTime2 != null) { dic.Add("HandleTime2", handleTime2); }
            if (handlerOrganizeID != null) { dic.Add("HandlerOrganizeID", handlerOrganizeID); }
            #endregion

            return dic;
        }
    }
}
