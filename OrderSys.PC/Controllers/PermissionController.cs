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

namespace OrderSys.Admin.Controllers
{
    public class PermissionController : AdminBaseController
    {
        //
        // GET: /Permission/
        private PermissionService service = new PermissionService();

        #region Resource
        public ActionResult ResourceIndex()
        {
            return View("~/Areas/Admin/Views/Permission/ResourceIndex.cshtml");
        }

        [HttpGet]
        public ActionResult InsertResourceIndex()
        {
            return View("~/Areas/Admin/Views/Permission/InsertResourceIndex.cshtml");
        }

        [HttpGet]
        public ActionResult EditResourceIndex()
        {
            return View("~/Areas/Admin/Views/Permission/EditResourceIndex.cshtml");
        }

        [HttpPost]
        public string AddResource()
        {
            string s = JSRequest.GetRequestFormParm("viewModel");
            ViewResource viewModel = FastJSON.JSON.ToObject<ViewResource>(s);

            //TODO 数据验证。

            ResourceEntity resource = new ResourceEntity();
            viewModel.CopyTo(resource);

            service.AddResource(resource);
            return JSON.ToJSON(new JSResponse(ResponseType.Remind, "添加成功！"), jsonParams);
        }

        [HttpPost]
        public string EditResource()
        {
            string s = JSRequest.GetRequestFormParm("viewModel");
            ViewResource viewModel = FastJSON.JSON.ToObject<ViewResource>(s);

            //TODO 数据验证。

            ResourceEntity resource = new ResourceEntity();
            viewModel.CopyTo(resource);

            service.EditResource(resource);
            return JSON.ToJSON(new JSResponse(ResponseType.Remind, "修改成功！"), jsonParams);
        }
        
        [HttpGet]
        public string GetResourceList()
        {
            int count = 0;
            DataTable re = service.GetResources(out count);

            string s = JSON.ToJSON(new JSResponse(new DataTableData(re, count)), jsonParams);
            return s;
        }
        #endregion

        #region DDL
        [HttpGet]
        public string GetResourceDDL()
        {
            List<ResourceEntity> list = service.GetTreeResourceList("OrderSys", true);
            var re = list.Select(l => new ViewResourceDDL()
            {
                ID = l.ID.ToString(),
                ParentID = l.ParentID.ToString(),
                Title = l.FullName
            }).ToList();

            string s = JSON.ToJSON(new JSResponse(re), jsonParams);
            return s;
        }

        [HttpGet]
        public string GetResourceTypeDDL()
        {
            Dictionary<ResourceType, string> dic = EnumExtensions.ConvertToEnumDic<ResourceType>();
            var re = dic.Select(d => new ViewResourceTypeDDL()
            {
                ID = d.Key.ToString(),
                Title = d.Value
            }).ToList();

            string s = JSON.ToJSON(new JSResponse(re), jsonParams);
            return s;
        } 
        #endregion
        
    }
}
