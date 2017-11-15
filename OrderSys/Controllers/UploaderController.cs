using FastJSON;
using JSNet.BaseSys;
using JSNet.Service;
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
            ContentResult res = new ContentResult();
            HttpFileCollectionBase files = Request.Files;
            if (files.Count == 0)
            {
                res.Content = JSON.ToJSON(new JSResponse("请选择文件后上传！"), jsonParams);
                return res;
            }

            string fileName = string.Empty;
            UploadService service = new UploadService();
            service.FileSaveAsImage(files[0], out fileName);

            res.Content = JSON.ToJSON(new JSResponse(data: fileName), jsonParams);
            return res;
        }

        public ActionResult RemovePhoto(string fileName)
        {
            UploadService service = new UploadService();
            service.RemoveFile(fileName);

            ContentResult res = new ContentResult();
            res.Content = JSON.ToJSON(new JSResponse(ResponseType.None,"删除成功！"), jsonParams);
            return res;
        }

    }
}
