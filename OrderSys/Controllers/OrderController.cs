using FastJSON;
using JSNet.BaseSys;
using JSNet.Model;
using JSNet.Service;
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

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult MyStartIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StartOrder()
        {
            //获取参数
            string sBookingTime = JSRequest.GetRequestFormParm(OrderEntity.FieldBookingTime);
            string sAttn =  JSRequest.GetRequestFormParm(OrderEntity.FieldAttn,false);
            string sAttnTel = JSRequest.GetRequestFormParm(OrderEntity.FieldAttnTel,false);
            string sPriority = JSRequest.GetRequestFormParm(OrderEntity.FieldPriority);
            string sContent = JSRequest.GetRequestFormParm(OrderEntity.FieldContent);
            string sRemark = JSRequest.GetRequestFormParm(OrderEntity.FieldRemark,false);

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

        [HttpGet]
        public ActionResult AppointOrder()
        {
            string sOrderID = JSRequest.GetRequestUrlParm(OrderEntity.FieldID);
            string sHandlerIDs = JSRequest.GetRequestUrlParm(OrderHandlerEntity.FieldHandlerID + "s");

            Guid orderID = (Guid)JSValidator.ValidateGuid(OrderEntity.FieldID, sOrderID, true);
            int[] handlerIDs =  JSValidator.ValidateStrings(OrderHandlerEntity.FieldHandlerID + "s",sHandlerIDs,true);

            orderService.AppointOrder(orderID, handlerIDs);

            ContentResult res = new ContentResult();
            res.Content = JSON.ToJSON(new JSResponse("操作成功！"), jsonParams);
            return res;
        }

        [HttpGet]
        public ActionResult ReceiveOrder()
        {
            string sOrderID = JSRequest.GetRequestUrlParm(OrderEntity.FieldID);
            Guid orderID = (Guid)JSValidator.ValidateGuid(OrderEntity.FieldID, sOrderID, true);

            orderService.ReceiveOrder(orderID);

            ContentResult res = new ContentResult();
            res.Content = JSON.ToJSON(new JSResponse("操作成功！"), jsonParams);
            return res;

        }

        [HttpGet]
        public ActionResult AddHandleDetail(OrderHandleDetailEntity orderHandleDetail)
        {
            orderService.AddHandleDetail(orderHandleDetail);

            ContentResult res = new ContentResult();
            res.Content = JSON.ToJSON(new JSResponse("操作成功！"), jsonParams);
            return res;
        }

        [HttpGet]
        public ActionResult CompleteOrder()
        {
            string sOrderID = JSRequest.GetRequestUrlParm(OrderEntity.FieldID);

            Guid orderID = (Guid)JSValidator.ValidateGuid(OrderEntity.FieldID, sOrderID, true);

            orderService.CompleteOrder(orderID);

            ContentResult res = new ContentResult();
            res.Content = JSON.ToJSON(new JSResponse("操作成功！"), jsonParams);
            return res;
        }

        [HttpGet]
        public ActionResult RejectOrder()
        {
            string sOrderID = JSRequest.GetRequestUrlParm(OrderEntity.FieldID);

            Guid orderID = (Guid)JSValidator.ValidateGuid(OrderEntity.FieldID, sOrderID, true);

            orderService.RejectOrder(orderID);

            ContentResult res = new ContentResult();
            res.Content = JSON.ToJSON(new JSResponse("操作成功！"), jsonParams);
            return res;
        }

        [HttpGet]
        public ActionResult FinishOrder()
        {
            string sOrderID = JSRequest.GetRequestUrlParm(OrderEntity.FieldID);

            Guid orderID = (Guid)JSValidator.ValidateGuid(OrderEntity.FieldID, sOrderID, true);

            orderService.FinishOrder(orderID);

            ContentResult res = new ContentResult();
            res.Content = JSON.ToJSON(new JSResponse("操作成功！"), jsonParams);
            return res;
        }

        [HttpGet]
        public ActionResult CancelOrder()
        {
            string sOrderID = JSRequest.GetRequestUrlParm(OrderEntity.FieldID);

            Guid orderID = (Guid)JSValidator.ValidateGuid(OrderEntity.FieldID, sOrderID, true);

            orderService.CancelOrder(orderID);

            ContentResult res = new ContentResult();
            res.Content = JSON.ToJSON(new JSResponse("操作成功！"), jsonParams);
            return res;
        }

        [HttpGet]
        public ActionResult GetMyStartedOrders(int pageIndex,int pageSize)
        {
            int count = 0;
            var list = orderService.GetMyStartedOrders(pageIndex,pageSize,out count);

            if (list.Rows.Count > 0)
            {
                return PartialView("MyStartedOrders", list);
            }
            else
            {
                ContentResult res = new ContentResult();
                res.Content = JSON.ToJSON(new JSResponse(ResponseType.None,"没有数据了！"), jsonParams);
                return res;
            }
        }

        [HttpGet]
        public ActionResult GetMyRecevingOrders()
        {
            var list = orderService.GetMyRecevingOrders();

            JsonResult res = new JsonResult();
            res.Data = new JSResponse(list);

            return res;
        }

        [HttpGet]
        public ActionResult GetMyHandlingOrders()
        {
            var list = orderService.GetMyHandlingOrders();

            JsonResult res = new JsonResult();
            res.Data = new JSResponse(list);

            return res;
        }

        [HttpGet]
        public ActionResult GetMyHandledOrders()
        {
            var list = orderService.GetMyHandledOrders();

            JsonResult res = new JsonResult();
            res.Data = new JSResponse(list);

            return res;
        }

        [HttpGet]
        public ActionResult GetMyAppointingOrders()
        {
            var list = orderService.GetMyAppointingOrders();

            JsonResult res = new JsonResult();
            res.Data = new JSResponse(list);

            return res;
        }

        [HttpGet]
        public ActionResult GetMyAppointedOrders()
        {
            var list = orderService.GetMyAppointedOrders();

            JsonResult res = new JsonResult();
            res.Data = new JSResponse(list);

            return res;
        }

        [HttpGet]
        public ActionResult GetOrderDetail()
        {
            string sOrderID = JSRequest.GetRequestUrlParm(OrderEntity.FieldID);

            Guid orderID = (Guid)JSValidator.ValidateGuid(OrderEntity.FieldID, sOrderID, true);

            DataRow dr = orderService.GetOrderDetail(orderID);

            return PartialView("OrderDetail", dr);
        }

    }
}
