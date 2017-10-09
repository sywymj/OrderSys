using FastJSON;
using JSNet.BaseSys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OrderSys.Controllers
{
    public class UploaderController : WeixinBaseController
    {
        //
        // GET: /Uploader/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UploadPhoto()
        {
            HttpFileCollectionBase files = Request.Files;

            ContentResult res = new ContentResult();
            res.Content = JSON.ToJSON(new JSResponse("撤销成功！"), jsonParams);
            return res;
        }

    }
}
