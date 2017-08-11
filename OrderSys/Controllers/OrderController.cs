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

        [HttpGet]
        public ActionResult StartOrder(OrderEntity order)
        {
            string sBookingTime = JSRequest.GetRequestFormParm(OrderEntity.FieldBookingTime);
            JSRequest.GetRequestFormParm(OrderEntity.FieldAttn);
            JSRequest.GetRequestFormParm(OrderEntity.FieldAttnTel);
            JSRequest.GetRequestFormParm(OrderEntity.FieldPriority);
            JSRequest.GetRequestFormParm(OrderEntity.FieldContent);
            JSRequest.GetRequestFormParm(OrderEntity.FieldRemark);



            orderService.StartOrder(order);

            ContentResult res = new ContentResult();
            res.Content = JSON.ToJSON(new JSResponse("操作成功！"), jsonParams);
            return res;
        }

        [HttpGet]
        public ActionResult AppointOrder()
        {
            string sOrderID = JSRequest.GetRequestUrlParm(OrderEntity.FieldID);
            string sHandlerIDs = JSRequest.GetRequestUrlParm(OrderHandlerEntity.FieldHandlerID + "s");

            Guid orderID = JSValidator.ValidateGuid(OrderEntity.FieldID,sOrderID,true);
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
            Guid orderID = JSValidator.ValidateGuid(OrderEntity.FieldID, sOrderID, true);

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

            Guid orderID = JSValidator.ValidateGuid(OrderEntity.FieldID, sOrderID, true);

            orderService.CompleteOrder(orderID);

            ContentResult res = new ContentResult();
            res.Content = JSON.ToJSON(new JSResponse("操作成功！"), jsonParams);
            return res;
        }

        [HttpGet]
        public ActionResult RejectOrder()
        {
            string sOrderID = JSRequest.GetRequestUrlParm(OrderEntity.FieldID);

            Guid orderID = JSValidator.ValidateGuid(OrderEntity.FieldID, sOrderID, true);

            orderService.RejectOrder(orderID);

            ContentResult res = new ContentResult();
            res.Content = JSON.ToJSON(new JSResponse("操作成功！"), jsonParams);
            return res;
        }

        [HttpGet]
        public ActionResult FinishOrder()
        {
            string sOrderID = JSRequest.GetRequestUrlParm(OrderEntity.FieldID);

            Guid orderID = JSValidator.ValidateGuid(OrderEntity.FieldID, sOrderID, true);

            orderService.FinishOrder(orderID);

            ContentResult res = new ContentResult();
            res.Content = JSON.ToJSON(new JSResponse("操作成功！"), jsonParams);
            return res;
        }

        [HttpGet]
        public ActionResult CancelOrder()
        {
            string sOrderID = JSRequest.GetRequestUrlParm(OrderEntity.FieldID);

            Guid orderID = JSValidator.ValidateGuid(OrderEntity.FieldID, sOrderID, true);

            orderService.CancelOrder(orderID);

            ContentResult res = new ContentResult();
            res.Content = JSON.ToJSON(new JSResponse("操作成功！"), jsonParams);
            return res;
        }

        [HttpGet]
        public ActionResult GetMyStartedOrders()
        {
            var list = orderService.GetMyStartedOrders();

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

            Guid orderID = JSValidator.ValidateGuid(OrderEntity.FieldID, sOrderID, true);

            DataRow dr = orderService.GetOrderDetail(orderID);

            return PartialView("OrderDetail", dr);
        }

    }
}
