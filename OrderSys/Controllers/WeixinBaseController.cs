using FastJSON;
using JSNet.BaseSys;
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
    public class WeixinBaseController : Controller
    {
        protected JSONParameters jsonParams = new JSONParameters()
        {
            UseUTCDateTime = false,
            UsingGlobalTypes = false,
            UseExtensions = false,
        };
    }
}
