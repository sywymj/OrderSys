using FastJSON;
using JSNet.Model;
using JSNet.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoSys.MiniUI
{
    public class BaseController:Controller
    {
        public BaseController()
        {
            //role = new PermissionService().GetCurrentRole();
        }

        protected JSONParameters jsonParams = new JSONParameters()
        {
            UseUTCDateTime = false,
            UsingGlobalTypes = false,
            UseExtensions = false,
        };
    }
}