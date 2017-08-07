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
    public class OrderController : Controller
    {
        //
        // GET: /Order/
        private OrderService orderService = new OrderService();
        public ActionResult Index()
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
    }
}
