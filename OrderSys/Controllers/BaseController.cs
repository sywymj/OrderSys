using FastJSON;
using JSNet.BaseSys;
using JSNet.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderSys.Controllers
{
    public class BaseController : Controller
    {
        protected JSONParameters jsonParams = new JSONParameters()
        {
            UseUTCDateTime = false,
            UsingGlobalTypes = false,
            UseExtensions = false,
        };

        public ActionResult GetOrderStatus()
        {
            Dictionary<int, string> dic = EnumExtensions.GetEnumDescription<OrderStatus>();

            ContentResult res = new ContentResult();
            res.Content = JSON.ToJSON(new JSResponse(dic), jsonParams);
            return res;
        }

        
    }
}
