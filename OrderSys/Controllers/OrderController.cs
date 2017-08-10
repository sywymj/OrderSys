using FastJSON;
using JSNet.BaseSys;
using JSNet.Model;
using JSNet.Service;
using System;
using System.Collections.Generic;
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
            orderService.StartOrder(order);

            JsonResult res = new JsonResult();
            res.Data = new JSResponse("操作成功");

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

            JsonResult res = new JsonResult();
            res.Data = new JSResponse("操作成功");

            return res;
        }

        [HttpGet]
        public ActionResult ReceiveOrder()
        {
            string sOrderID = JSRequest.GetRequestUrlParm(OrderEntity.FieldID);

            Guid orderID = JSValidator.ValidateGuid(OrderEntity.FieldID, sOrderID, true);

            orderService.ReceiveOrder(orderID);

            JsonResult res = new JsonResult();
            res.Data = new JSResponse("操作成功");

            return res;

        }

        [HttpGet]
        public ActionResult AddHandleDetail(OrderHandleDetailEntity orderHandleDetail)
        {
            orderService.AddHandleDetail(orderHandleDetail);

            JsonResult res = new JsonResult();
            res.Data = new JSResponse("操作成功");

            return res;
        }

        [HttpGet]
        public ActionResult CompleteOrder()
        {
            string sOrderID = JSRequest.GetRequestUrlParm(OrderEntity.FieldID);

            Guid orderID = JSValidator.ValidateGuid(OrderEntity.FieldID, sOrderID, true);

            orderService.CompleteOrder(orderID);

            JsonResult res = new JsonResult();
            res.Data = new JSResponse("操作成功");

            return res;
        }

        [HttpGet]
        public ActionResult RejectOrder()
        {
            string sOrderID = JSRequest.GetRequestUrlParm(OrderEntity.FieldID);

            Guid orderID = JSValidator.ValidateGuid(OrderEntity.FieldID, sOrderID, true);

            orderService.RejectOrder(orderID);

            JsonResult res = new JsonResult();
            res.Data = new JSResponse("操作成功");

            return res;
        }

        [HttpGet]
        public ActionResult FinishOrder()
        {
            string sOrderID = JSRequest.GetRequestUrlParm(OrderEntity.FieldID);

            Guid orderID = JSValidator.ValidateGuid(OrderEntity.FieldID, sOrderID, true);

            orderService.FinishOrder(orderID);

            JsonResult res = new JsonResult();
            res.Data = new JSResponse("操作成功");

            return res;
        }

        [HttpGet]
        public ActionResult CancelOrder()
        {
            string sOrderID = JSRequest.GetRequestUrlParm(OrderEntity.FieldID);

            Guid orderID = JSValidator.ValidateGuid(OrderEntity.FieldID, sOrderID, true);

            orderService.CancelOrder(orderID);

            JsonResult res = new JsonResult();
            res.Data = new JSResponse("操作成功");

            return res;
        }

        [HttpGet]
        public ActionResult GetMyStartedOrders()
        {
            try
            {
                string s = BaseSystemInfo.XmlFileName;


                var list = orderService.GetMyStartedOrders();

                if (list.Rows.Count > 0)
                {
                    return PartialView("MyStartedOrders", list);
                }
                else
                {
                    ContentResult res = new ContentResult()
                    {
                        Content = JSON.ToJSON(new JSResponse("201", "没有数据可以加载了！"), jsonParameters)
                    };
                    return res;
                }
            }
            catch (Exception e)
            {
                throw e;
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
    }
}
