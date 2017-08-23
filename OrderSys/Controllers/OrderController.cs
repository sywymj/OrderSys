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

namespace OrderSys.Controllers
{
    public class OrderController : BaseController
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
            return View();
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
            return PartialView("AppointOrder");
        }

        [HttpGet]
        public ActionResult OrderFlows(Guid id)
        {
            OrderEntity order = orderService.GetOrderEntity(id);

            ViewBag.OrderStatus = (OrderStatus)order.Status;
            ViewBag.OrderStatus1 = (OrderStatus)Math.Abs((int)order.Status);//处理过的状态，只保留主流程
            ViewBag.OrderID = order.ID;

            var list = orderService.GetOrderFlows(id);

            return PartialView("OrderFlows", list);
        }

        [HttpGet]
        public ActionResult OrderDetail(Guid id)
        {
            var handlers = orderService.GetOrderHandlers(id);
            ViewBag.Handlers = handlers;


            var dr = orderService.GetOrderDetail(id);
            return PartialView("OrderDetail", dr);
        }

        [HttpGet]
        public ActionResult HandleDetail(Guid orderID)
        {
            var list = orderService.GetOrderHandleDetails(orderID);

            if (list.Rows.Count > 0)
            {
                return PartialView("HandleDetail", list);
            }
            else
            {
                ContentResult res = new ContentResult();
                res.Content = JSON.ToJSON(new JSResponse(ResponseType.NoData, "- 暂无进度 -"), jsonParams);
                return res;
            }
        }

        [HttpGet]
        public ActionResult AddHandleDetail(Guid orderID)
        {
            ViewBag.OrderID = orderID;
            return PartialView("AddHandleDetail");
        }
        #endregion

        #region GetList
        [HttpGet]
        public ActionResult MyStartedOrders(int pageIndex, int pageSize)
        {
            int count = 0;
            var list = orderService.GetMyStartedOrders(pageIndex, pageSize, out count);

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
                    res.Content = JSON.ToJSON(new JSResponse(ResponseType.NoData, "-暂无数据-"), jsonParams);
                }
                return res;
            }
        }

        [HttpGet]
        public ActionResult MyReceivingOrders(int pageIndex, int pageSize)
        {
            int count = 0;
            var list = orderService.GetMyReceivingOrders(pageIndex, pageSize, out count);

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
            int count = 0;
            var list = orderService.GetMyHandlingOrders(pageIndex, pageSize, out count);

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
            int count = 0;
            var list = orderService.GetMyHandledOrders(pageIndex, pageSize, out count);

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
            int count = 0;
            var list = orderService.GetMyAppointingOrders(pageIndex, pageSize, out count);

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
            int count = 0;
            var list = orderService.GetMyAppointedOrders(pageIndex, pageSize, out count);

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
        public ActionResult DoStartOrder()
        {
            //获取参数
            string sBookingTime = JSRequest.GetRequestFormParm(OrderEntity.FieldBookingTime);
            string sAttn = JSRequest.GetRequestFormParm(OrderEntity.FieldAttn, false);
            string sAttnTel = JSRequest.GetRequestFormParm(OrderEntity.FieldAttnTel, false);
            string sPriority = JSRequest.GetRequestFormParm(OrderEntity.FieldPriority);
            string sContent = JSRequest.GetRequestFormParm(OrderEntity.FieldContent);
            string sRemark = JSRequest.GetRequestFormParm(OrderEntity.FieldRemark, false);

            //参数验证
            OrderEntity order = new OrderEntity();
            order.BookingTime = JSValidator.ValidateDateTime(OrderEntity.FieldBookingTime, sBookingTime, true);
            order.Attn = JSValidator.ValidateString(OrderEntity.FieldAttn, sAttn, false);
            order.AttnTel = JSValidator.ValidateString(OrderEntity.FieldAttnTel, sAttnTel, false);
            order.Priority = JSValidator.ValidateInt(OrderEntity.FieldPriority, sPriority, true);
            order.Content = JSValidator.ValidateString(OrderEntity.FieldContent, sContent, true);
            order.Remark = JSValidator.ValidateString(OrderEntity.FieldRemark, sRemark, false);

            orderService.StartOrder(order);

            ContentResult res = new ContentResult();
            res.Content = JSON.ToJSON(new JSResponse("成功发起报账单！"), jsonParams);
            return res;
        }

        [HttpPost]
        public ActionResult DoAppointOrder(string jsonHandlers, Guid orderID)
        {
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
        public ActionResult DoAddHandleDetail(Guid orderID, int handleType, string Remark)
        {
            OrderHandleDetailEntity orderHandleDetail = new OrderHandleDetailEntity();
            orderHandleDetail.OrderID = orderID;
            orderHandleDetail.HandleType = handleType;
            orderHandleDetail.Remark = Remark;
            orderService.AddHandleDetail(orderHandleDetail);

            ContentResult res = new ContentResult();
            res.Content = JSON.ToJSON(new JSResponse("操作成功！"), jsonParams);
            return res;
        }

        [HttpGet]
        public ActionResult HandledOrder(Guid orderID)
        {
            orderService.HandledOrder(orderID);

            ContentResult res = new ContentResult();
            res.Content = JSON.ToJSON(new JSResponse("验收成功！"), jsonParams);
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
            res.Content = JSON.ToJSON(new JSResponse("操作成功！"), jsonParams);
            return res;
        }

        [HttpGet]
        public ActionResult CancelOrder(Guid id)
        {
            orderService.CancelOrder(id);

            ContentResult res = new ContentResult();
            res.Content = JSON.ToJSON(new JSResponse("操作成功！"), jsonParams);
            return res;
        }

        #endregion

        
    }
}
