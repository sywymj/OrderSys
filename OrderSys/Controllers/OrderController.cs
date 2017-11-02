using FastJSON;
using JSNet.BaseSys;
using JSNet.Model;
using JSNet.Service;
using JSNet.Utilities;
using OrderSys.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderSys.Controllers
{
    [ManagerAuthorize]
    public class OrderController : WeixinBaseController
    {
        private OrderService orderService = new OrderService();

        #region GetIndex
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult MyStartIndex()
        {
            OrderEntity order = new OrderEntity();
            order.Attn = orderService.CurrentStaff.Name;
            order.AttnTel = orderService.CurrentStaff.Tel;
            return View(order);
        }

        [HttpGet]
        public ActionResult MyAppointIndex()
        {
            return View();
        }

        [HttpGet]
        public ActionResult MyHandleIndex()
        {
            return View();
        }



        #endregion

        #region GetPartialView
        [HttpGet]
        public ActionResult AppointOrder(Guid orderID)
        {
            ViewBag.OrderID = orderID;
            return View("AppointOrder");
        }

        [HttpGet]
        public ActionResult OrderFlows(Guid orderID)
        {
            OrderEntity order = orderService.GetOrderEntity(orderID);

            ViewBag.OrderStatus = (OrderStatus)order.Status;
            ViewBag.OrderStatus1 = (OrderStatus)Math.Abs((int)order.Status);//处理过的状态，只保留主流程
            ViewBag.OrderID = order.ID;

            var list = orderService.GetOrderFlows(orderID);

            return View("OrderFlows", list);
        }

        [HttpGet]
        public ActionResult OrderDetail(Guid orderID)
        {
            var handlers = orderService.GetOrderHandlerDT(orderID);
            ViewBag.Handlers = handlers;


            var dr = orderService.GetOrderDetail(orderID);
            return View("OrderDetail", dr);
        }

        [HttpGet]
        public ActionResult HandleDetail(Guid orderID)
        {
            var list = orderService.GetOrderHandleDetails(orderID);

            if (list.Rows.Count > 0)
            {
                return View("HandleDetail", list);
            }
            else
            {
                return View("_NoData", model: "- 工单没受理，暂无进度。 -");
            }
        }

        [HttpGet]
        public ActionResult AddHandleDetail(Guid orderID)
        {
            ViewBag.OrderID = orderID;
            var list = orderService.GetOrderHandleDetails(orderID);
            return View("AddHandleDetail",list);
        }
        #endregion

        #region GetList
        [HttpGet]
        public ActionResult MyStartedOrders(int pageIndex, int pageSize)
        {
            #region 获取参数
            string sStatus = JSRequest.GetRequestUrlParm("Status", false);
            string sPriority = JSRequest.GetRequestUrlParm("Priority", false);
            string sBookingTime = JSRequest.GetRequestUrlParm("BookingTime", false);
            string sContent = JSRequest.GetRequestUrlParm("Content", false);
            string sWorkingLocation = JSRequest.GetRequestUrlParm("WorkingLocation", false); 
            #endregion

            #region 验证参数
            int? status = JSValidator.ValidateInt("工单状态", sStatus);
            int? priority = JSValidator.ValidateInt("紧急程度", sPriority);
            DateTime? bookingTime = JSValidator.ValidateDateTime("紧急程度", sBookingTime);
            string content = JSValidator.ValidateString("工单内容", sContent);
            string workingLocation = JSValidator.ValidateString("维修地点", sWorkingLocation); 
            #endregion

            #region 构造搜索条件
            JSDictionary dic = new JSDictionary();
            if (status != null) { dic.Add(OrderEntity.FieldStatus, (int)status); }
            if (priority != null) { dic.Add(OrderEntity.FieldPriority, (int)priority); }
            if (bookingTime != null) { dic.Add(OrderEntity.FieldBookingTime, bookingTime); }
            if (!string.IsNullOrEmpty(content)) { dic.Add(OrderEntity.FieldContent, content); }
            if (!string.IsNullOrEmpty(workingLocation)) { dic.Add(OrderEntity.FieldWorkingLocation, workingLocation); } 
            #endregion
            
            int count = 0;
            var list = orderService.GetMyStartedOrders(dic, pageIndex, pageSize, out count);

            if (list.Rows.Count > 0)
            {
                return PartialView("MyStartedOrders", list);
            }
            else
            {
                ContentResult res = new ContentResult();
                if (pageIndex > 1)
                {
                    res.Content = JSON.ToJSON(new JSResponse(ResponseType.NoMoreData, "— 数据已加载完毕 —"), jsonParams);
                }
                else
                {
                    res.Content = JSON.ToJSON(new JSResponse(ResponseType.NoData, "- 暂无数据 -"), jsonParams);
                }
                return res;
            }
        }

        [HttpGet]
        public ActionResult MyReceivingOrders(int pageIndex, int pageSize)
        {
            #region 获取参数
            string sStatus = JSRequest.GetRequestUrlParm("Status", false);
            string sPriority = JSRequest.GetRequestUrlParm("Priority", false);
            string sBookingTime = JSRequest.GetRequestUrlParm("BookingTime", false);
            string sContent = JSRequest.GetRequestUrlParm("Content", false);
            string sWorkingLocation = JSRequest.GetRequestUrlParm("WorkingLocation", false);
            #endregion

            #region 验证参数
            int? status = JSValidator.ValidateInt("工单状态", sStatus);
            int? priority = JSValidator.ValidateInt("紧急程度", sPriority);
            DateTime? bookingTime = JSValidator.ValidateDateTime("紧急程度", sBookingTime);
            string content = JSValidator.ValidateString("工单内容", sContent);
            string workingLocation = JSValidator.ValidateString("维修地点", sWorkingLocation);
            #endregion

            #region 构造搜索条件
            JSDictionary dic = new JSDictionary();
            if (status != null) { dic.Add(OrderEntity.FieldStatus, (int)status); }
            if (priority != null) { dic.Add(OrderEntity.FieldPriority, (int)priority); }
            if (bookingTime != null) { dic.Add(OrderEntity.FieldBookingTime, bookingTime); }
            if (!string.IsNullOrEmpty(content)) { dic.Add(OrderEntity.FieldContent, content); }
            if (!string.IsNullOrEmpty(workingLocation)) { dic.Add(OrderEntity.FieldWorkingLocation, workingLocation); }
            #endregion


            int count = 0;
            var list = orderService.GetMyReceivingOrders(dic,pageIndex, pageSize, out count);

            if (list.Rows.Count > 0)
            {
                return PartialView("MyReceivingOrders", list);
            }
            else
            {
                ContentResult res = new ContentResult();
                if (pageIndex > 1)
                {
                    res.Content = JSON.ToJSON(new JSResponse(ResponseType.NoMoreData, "— 数据已加载完毕 —"), jsonParams);
                }
                else
                {
                    res.Content = JSON.ToJSON(new JSResponse(ResponseType.NoData, "— 暂无数据 —"), jsonParams);
                }
                return res;
            }
        }

        [HttpGet]
        public ActionResult MyHandlingOrders(int pageIndex, int pageSize)
        {
            #region 获取参数
            string sStatus = JSRequest.GetRequestUrlParm("Status", false);
            string sPriority = JSRequest.GetRequestUrlParm("Priority", false);
            string sBookingTime = JSRequest.GetRequestUrlParm("BookingTime", false);
            string sContent = JSRequest.GetRequestUrlParm("Content", false);
            string sWorkingLocation = JSRequest.GetRequestUrlParm("WorkingLocation", false);
            #endregion

            #region 验证参数
            int? status = JSValidator.ValidateInt("工单状态", sStatus);
            int? priority = JSValidator.ValidateInt("紧急程度", sPriority);
            DateTime? bookingTime = JSValidator.ValidateDateTime("紧急程度", sBookingTime);
            string content = JSValidator.ValidateString("工单内容", sContent);
            string workingLocation = JSValidator.ValidateString("维修地点", sWorkingLocation);
            #endregion

            #region 构造搜索条件
            JSDictionary dic = new JSDictionary();
            if (status != null) { dic.Add(OrderEntity.FieldStatus, (int)status); }
            if (priority != null) { dic.Add(OrderEntity.FieldPriority, (int)priority); }
            if (bookingTime != null) { dic.Add(OrderEntity.FieldBookingTime, bookingTime); }
            if (!string.IsNullOrEmpty(content)) { dic.Add(OrderEntity.FieldContent, content); }
            if (!string.IsNullOrEmpty(workingLocation)) { dic.Add(OrderEntity.FieldWorkingLocation, workingLocation); }
            #endregion

            int count = 0;
            var list = orderService.GetMyHandlingOrders(dic,pageIndex, pageSize, out count);

            if (list.Rows.Count > 0)
            {
                return PartialView("MyHandlingOrders", list);
            }
            else
            {
                ContentResult res = new ContentResult();
                if (pageIndex > 1)
                {
                    res.Content = JSON.ToJSON(new JSResponse(ResponseType.NoMoreData, "— 数据已加载完毕 —"), jsonParams);
                }
                else
                {
                    res.Content = JSON.ToJSON(new JSResponse(ResponseType.NoData, "— 暂无数据 —"), jsonParams);
                }
                return res;
            }
        }

        [HttpGet]
        public ActionResult MyHandledOrders(int pageIndex, int pageSize)
        {
            #region 获取参数
            string sStatus = JSRequest.GetRequestUrlParm("Status", false);
            string sPriority = JSRequest.GetRequestUrlParm("Priority", false);
            string sBookingTime = JSRequest.GetRequestUrlParm("BookingTime", false);
            string sContent = JSRequest.GetRequestUrlParm("Content", false);
            string sWorkingLocation = JSRequest.GetRequestUrlParm("WorkingLocation", false);
            #endregion

            #region 验证参数
            int? status = JSValidator.ValidateInt("工单状态", sStatus);
            int? priority = JSValidator.ValidateInt("紧急程度", sPriority);
            DateTime? bookingTime = JSValidator.ValidateDateTime("紧急程度", sBookingTime);
            string content = JSValidator.ValidateString("工单内容", sContent);
            string workingLocation = JSValidator.ValidateString("维修地点", sWorkingLocation);
            #endregion

            #region 构造搜索条件
            JSDictionary dic = new JSDictionary();
            if (status != null) { dic.Add(OrderEntity.FieldStatus, (int)status); }
            if (priority != null) { dic.Add(OrderEntity.FieldPriority, (int)priority); }
            if (bookingTime != null) { dic.Add(OrderEntity.FieldBookingTime, bookingTime); }
            if (!string.IsNullOrEmpty(content)) { dic.Add(OrderEntity.FieldContent, content); }
            if (!string.IsNullOrEmpty(workingLocation)) { dic.Add(OrderEntity.FieldWorkingLocation, workingLocation); }
            #endregion

            int count = 0;
            var list = orderService.GetMyHandledOrders(dic ,pageIndex, pageSize, out count);

            if (list.Rows.Count > 0)
            {
                return PartialView("MyHandledOrders", list);
            }
            else
            {
                ContentResult res = new ContentResult();
                if (pageIndex > 1)
                {
                    res.Content = JSON.ToJSON(new JSResponse(ResponseType.NoMoreData, "— 数据已加载完毕 —"), jsonParams);
                }
                else
                {
                    res.Content = JSON.ToJSON(new JSResponse(ResponseType.NoData, "— 暂无数据 —"), jsonParams);
                }
                return res;
            }
        }

        [HttpGet]
        public ActionResult MyAppointingOrders(int pageIndex, int pageSize)
        {
            #region 获取参数
            string sStatus = JSRequest.GetRequestUrlParm("Status", false);
            string sPriority = JSRequest.GetRequestUrlParm("Priority", false);
            string sBookingTime = JSRequest.GetRequestUrlParm("BookingTime", false);
            string sContent = JSRequest.GetRequestUrlParm("Content", false);
            string sWorkingLocation = JSRequest.GetRequestUrlParm("WorkingLocation", false);
            #endregion

            #region 验证参数
            int? status = JSValidator.ValidateInt("工单状态", sStatus);
            int? priority = JSValidator.ValidateInt("紧急程度", sPriority);
            DateTime? bookingTime = JSValidator.ValidateDateTime("紧急程度", sBookingTime);
            string content = JSValidator.ValidateString("工单内容", sContent);
            string workingLocation = JSValidator.ValidateString("维修地点", sWorkingLocation);
            #endregion

            #region 构造搜索条件
            JSDictionary dic = new JSDictionary();
            if (status != null) { dic.Add(OrderEntity.FieldStatus, (int)status); }
            if (priority != null) { dic.Add(OrderEntity.FieldPriority, (int)priority); }
            if (bookingTime != null) { dic.Add(OrderEntity.FieldBookingTime, bookingTime); }
            if (!string.IsNullOrEmpty(content)) { dic.Add(OrderEntity.FieldContent, content); }
            if (!string.IsNullOrEmpty(workingLocation)) { dic.Add(OrderEntity.FieldWorkingLocation, workingLocation); }
            #endregion


            int count = 0;
            var list = orderService.GetMyAppointingOrders(dic,pageIndex, pageSize, out count);

            if (list.Rows.Count > 0)
            {
                return PartialView("MyAppointingOrders", list);
            }
            else
            {
                ContentResult res = new ContentResult();
                if (pageIndex > 1)
                {
                    res.Content = JSON.ToJSON(new JSResponse(ResponseType.NoMoreData, "— 数据已加载完毕 —"), jsonParams);
                }
                else
                {
                    res.Content = JSON.ToJSON(new JSResponse(ResponseType.NoData, "— 暂无数据 —"), jsonParams);
                }
                return res;
            }
        }

        [HttpGet]
        public ActionResult MyAppointedOrders(int pageIndex, int pageSize)
        {
            #region 获取参数
            string sStatus = JSRequest.GetRequestUrlParm("Status", false);
            string sPriority = JSRequest.GetRequestUrlParm("Priority", false);
            string sBookingTime = JSRequest.GetRequestUrlParm("BookingTime", false);
            string sContent = JSRequest.GetRequestUrlParm("Content", false);
            string sWorkingLocation = JSRequest.GetRequestUrlParm("WorkingLocation", false);
            #endregion

            #region 验证参数
            int? status = JSValidator.ValidateInt("工单状态", sStatus);
            int? priority = JSValidator.ValidateInt("紧急程度", sPriority);
            DateTime? bookingTime = JSValidator.ValidateDateTime("紧急程度", sBookingTime);
            string content = JSValidator.ValidateString("工单内容", sContent);
            string workingLocation = JSValidator.ValidateString("维修地点", sWorkingLocation);
            #endregion

            #region 构造搜索条件
            JSDictionary dic = new JSDictionary();
            if (status != null) { dic.Add(OrderEntity.FieldStatus, (int)status); }
            if (priority != null) { dic.Add(OrderEntity.FieldPriority, (int)priority); }
            if (bookingTime != null) { dic.Add(OrderEntity.FieldBookingTime, bookingTime); }
            if (!string.IsNullOrEmpty(content)) { dic.Add(OrderEntity.FieldContent, content); }
            if (!string.IsNullOrEmpty(workingLocation)) { dic.Add(OrderEntity.FieldWorkingLocation, workingLocation); }
            #endregion

            int count = 0;
            var list = orderService.GetMyAppointedOrders(dic,pageIndex, pageSize, out count);

            if (list.Rows.Count > 0)
            {
                return PartialView("MyAppointedOrders", list);
            }
            else
            {
                ContentResult res = new ContentResult();
                if (pageIndex > 1)
                {
                    res.Content = JSON.ToJSON(new JSResponse(ResponseType.NoMoreData, "— 数据已加载完毕 —"), jsonParams);
                }
                else
                {
                    res.Content = JSON.ToJSON(new JSResponse(ResponseType.NoData, "— 暂无数据 —"), jsonParams);
                }
                return res;
            }
        }

        #endregion

        #region DoAction
        [HttpPost]
        public ActionResult StartOrder()
        {
            DoStartOrder();

            ContentResult res = new ContentResult();
            res.Content = JSON.ToJSON(new JSResponse("成功发起"), jsonParams);
            return res;
        }

        [HttpPost]
        public ActionResult StartAndAppointOrder()
        {
            string orderID = DoStartOrder();

            ContentResult res = new ContentResult();
            res.Content = JSON.ToJSON(new JSResponse(ResponseType.Redict, "成功发起，现请进行委派！", data: Url.Action("AppointOrder", "Order", new { area = "Weixin", OrderID = orderID })));
            return res;
        }

        private string DoStartOrder()
        {
            //获取参数
            string sBookingTime = JSRequest.GetRequestFormParm(OrderEntity.FieldBookingTime, false);
            string sAttn = JSRequest.GetRequestFormParm(OrderEntity.FieldAttn, false);
            string sAttnTel = JSRequest.GetRequestFormParm(OrderEntity.FieldAttnTel, false);
            string sPriority = JSRequest.GetRequestFormParm(OrderEntity.FieldPriority);
            string sContent = JSRequest.GetRequestFormParm(OrderEntity.FieldContent);
            string sRemark = JSRequest.GetRequestFormParm(OrderEntity.FieldRemark, false);
            string sWorkingLocation = JSRequest.GetRequestFormParm(OrderEntity.FieldWorkingLocation, false);
            string sPhotoPath = JSRequest.GetRequestFormParm(OrderEntity.FieldPhotoPath, false);
            string sPhotoPath1 = JSRequest.GetRequestFormParm(OrderEntity.FieldPhotoPath1, false);

            //参数验证
            OrderEntity order = new OrderEntity();
            order.BookingTime = JSValidator.ValidateDateTime(OrderEntity.FieldBookingTime, sBookingTime, false);
            order.Attn = JSValidator.ValidateString(OrderEntity.FieldAttn, sAttn, false);
            order.AttnTel = JSValidator.ValidateString(OrderEntity.FieldAttnTel, sAttnTel, false);
            order.Priority = JSValidator.ValidateInt(OrderEntity.FieldPriority, sPriority, true);
            order.Content = JSValidator.ValidateString(OrderEntity.FieldContent, sContent, true);
            order.Remark = JSValidator.ValidateString(OrderEntity.FieldRemark, sRemark, false);
            order.WorkingLocation = JSValidator.ValidateString(OrderEntity.FieldWorkingLocation, sWorkingLocation, false);
            order.PhotoPath = JSValidator.ValidateString(OrderEntity.FieldPhotoPath, sPhotoPath, false);
            order.PhotoPath1 = JSValidator.ValidateString(OrderEntity.FieldPhotoPath1, sPhotoPath1, false);
            string orderID = orderService.StartOrder(order);

            return orderID;
        }

        [HttpPost]
        public ActionResult DoAppointOrder(string jsonHandlers, Guid orderID)
        {
            //json demo = "[{\"OrderID\":\"e8026b86-c3bf-4a8c-8566-6b053d4d80e4\",\"HandlerID\":11,\"Workload\":1,\"IsLeader\":0},{\"OrderID\":\"e8026b86-c3bf-4a8c-8566-6b053d4d80e4\",\"HandlerID\":7,\"Workload\":1,\"IsLeader\":0},{\"OrderID\":\"e8026b86-c3bf-4a8c-8566-6b053d4d80e4\",\"HandlerID\":1,\"Workload\":1,\"IsLeader\":1}]"
            List<OrderHandlerEntity> handlers = FastJSON.JSON.ToObject<List<OrderHandlerEntity>>(jsonHandlers);

            orderService.AppointOrder(orderID, handlers);

            ContentResult res = new ContentResult();
            res.Content = JSON.ToJSON(new JSResponse("委派成功！"), jsonParams);
            return res;
        }

        [HttpGet]
        public ActionResult DoReceiveOrder(Guid orderID)
        {

            orderService.ReceiveOrder(orderID);

            ContentResult res = new ContentResult();
            res.Content = JSON.ToJSON(new JSResponse("接单成功！"), jsonParams);
            return res;

        }

        [HttpPost]
        public ActionResult DoAddHandleDetail()
        {
            string s = JSRequest.GetRequestFormParm("ViewModel");
            OrderHandleDetailViewModel viewModel = FastJSON.JSON.ToObject<OrderHandleDetailViewModel>(s);

            //TODO 数据验证。

            OrderHandleDetailEntity handleDetail = new OrderHandleDetailEntity();
            viewModel.CopyTo(handleDetail);
            List<OrderGoodsRelEntity> orderGoodsRels = new List<OrderGoodsRelEntity>();
            foreach (OrderGoodsRelEntity good in viewModel.OrderGoods)
            {
                OrderGoodsRelEntity rel = new OrderGoodsRelEntity();
                good.CopyTo(rel);
                orderGoodsRels.Add(rel);
            }

            //添加处理进度
            orderService.AddHandleDetail(handleDetail);
            //添加更换的物品
            orderService.AddOrderGoodsRel(orderGoodsRels);

            ContentResult res = new ContentResult();
            res.Content = JSON.ToJSON(new JSResponse("操作成功！"), jsonParams);
            return res;
        }

        [HttpPost]
        public ActionResult HandledOrder()
        {
            string sHandledPhotoPath = JSRequest.GetRequestFormParm("HandledPhotoPath");
            string handledPhotoPath = JSValidator.ValidateString("图片地址", sHandledPhotoPath, false);
            string sHandledPhotoPath1 = JSRequest.GetRequestFormParm("HandledPhotoPath1");
            string handledPhotoPath1 = JSValidator.ValidateString("图片地址", sHandledPhotoPath1, false);

            string s = JSRequest.GetRequestFormParm("ViewModel");
            OrderHandleDetailViewModel viewModel = FastJSON.JSON.ToObject<OrderHandleDetailViewModel>(s);

            OrderHandleDetailEntity handleDetail = new OrderHandleDetailEntity();
            viewModel.CopyTo(handleDetail);

            //完成工单
            orderService.HandledOrder((Guid)handleDetail.OrderID, handledPhotoPath, handledPhotoPath1);
            //添加处理进度
            orderService.AddHandledDetail(handleDetail);

            ContentResult res = new ContentResult();
            res.Content = JSON.ToJSON(new JSResponse("操作成功！"), jsonParams);
            return res;
        }

        [HttpGet]
        public ActionResult RejectOrder(Guid orderID, string remark)
        {
            orderService.RejectOrder(orderID, remark);

            ContentResult res = new ContentResult();
            res.Content = JSON.ToJSON(new JSResponse("驳回成功！"), jsonParams);
            return res;
        }

        [HttpGet]
        public ActionResult FinishOrder(Guid orderID)
        {

            orderService.FinishOrder(orderID);

            ContentResult res = new ContentResult();
            res.Content = JSON.ToJSON(new JSResponse("验收成功！"), jsonParams);
            return res;
        }

        [HttpGet]
        public ActionResult CancelOrder(Guid orderID)
        {
            orderService.CancelOrder(orderID);

            ContentResult res = new ContentResult();
            res.Content = JSON.ToJSON(new JSResponse("撤销成功！"), jsonParams);
            return res;
        }

        #endregion

        
    }
}
