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
    [ManagerAuthorize]
    public class OrganizeController : AdminBaseController
    {
        //
        // GET: /Organize/
        OrganizeService service = new OrganizeService();

        [HttpGet]
        public ActionResult OrganizeIndex()
        {
            return View("~/Areas/Admin/Views/Organize/Organize_Index.cshtml");
        }

        [HttpGet]
        public ActionResult InsertOrganizeIndex()
        {
            return View("~/Areas/Admin/Views/Organize/Organize_InsertIndex.cshtml");
        }

        [HttpGet]
        public ActionResult EditOrganizeIndex()
        {
            return View("~/Areas/Admin/Views/Organize/Organize_InsertIndex.cshtml");
        }

        [HttpGet]
        public ActionResult OrganizeCategoryIndex()
        {
            return View("~/Areas/Admin/Views/Organize/OrganizeCategory_Index.cshtml");
        }

        [HttpGet]
        public ActionResult InsertOrganizeCategoryIndex()
        {
            return View("~/Areas/Admin/Views/Organize/OrganizeCategory_InsertIndex.cshtml");
        }

        [HttpGet]
        public ActionResult EditOrganizeCategoryIndex()
        {
            return View("~/Areas/Admin/Views/Organize/OrganizeCategory_InsertIndex.cshtml");
        }

        #region Organize
        [HttpPost]
        public string AddOrganize()
        {
            string s = JSRequest.GetRequestFormParm("viewModel");
            ViewOrganize viewModel = FastJSON.JSON.ToObject<ViewOrganize>(s);

            //TODO 数据验证。

            OrganizeEntity Organize = new OrganizeEntity();
            viewModel.CopyTo(Organize);

            service.AddOrganize(Organize);
            return JSON.ToJSON(new JSResponse(ResponseType.Remind, "添加成功！"), jsonParams);
        }

        [HttpPost]
        public string EditOrganize()
        {
            string s = JSRequest.GetRequestFormParm("viewModel");
            ViewOrganize viewModel = FastJSON.JSON.ToObject<ViewOrganize>(s);

            //TODO 数据验证。

            OrganizeEntity Organize = new OrganizeEntity();
            viewModel.CopyTo(Organize);

            service.EditOrganize(Organize);
            return JSON.ToJSON(new JSResponse(ResponseType.Remind, "修改成功！"), jsonParams);
        }

        [HttpGet]
        public string GetSingleOrganize(int OrganizeID)
        {
            ViewOrganize viewModel = new ViewOrganize();
            OrganizeEntity entity = service.GetOrganize(OrganizeID);
            entity.CopyTo(viewModel);

            return JSON.ToJSON(new JSResponse(viewModel), jsonParams);
        }

        [HttpGet]
        public string GetOrganizeList()
        {
            int count = 0;
            //string sParentCode = JSRequest.GetRequestUrlParm("ParentCode");

            DataTable re = service.GetTreeOrganizeDTForShow(out count, "OrderSys");
            string s = JSON.ToJSON(new JSResponse(new DataTableData(re, count)), jsonParams);
            return s;
        }

        [HttpGet]
        public string GetGrantOrganizeList()
        {
            int count = 0;
            string sResourceCode = JSRequest.GetRequestUrlParm("ResourceCode");
            string sResourceType = JSRequest.GetRequestUrlParm("ResourceType");

            DataTable re = service.GetGrantOrganizeDTForShow(sResourceCode, sResourceType, out count);
            string s = JSON.ToJSON(new JSResponse(new DataTableData(re, count)), jsonParams);
            return s;
        }

        [HttpGet]
        public string GetOrganizeDDL()
        {
            List<OrganizeEntity> list = service.GetTreeOrganizeList("OrderSys.FSWGY");

            var re = list.Select(l =>
                new ViewOrganizeDDL()
                {
                    ID = l.ID.ToString(),
                    ParentID = l.ParentID.ToString(),
                    Title = l.FullName
                }).ToList();

            string s = JSON.ToJSON(new JSResponse(re), jsonParams);

            return s;
        }

        [HttpGet]
        public string GetOrganizeCodeDDL()
        {
            List<OrganizeEntity> list = service.GetTreeOrganizeList("OrderSys.FSWGY", true);
            var re = list.Select(l => new ViewOrganizeCodeDDL()
            {
                ID = l.Code,
                Title = l.Code
            }).ToList();

            string s = JSON.ToJSON(new JSResponse(re), jsonParams);
            return s;
        }

        [HttpGet]
        public string GetOrganizeCategoryDDL()
        {
            List<OrganizeCategoryEntity> list = service.GetOrganizeCategorys();

            var re = list.Select(l =>
                new ViewOrganizeDDL()
                {
                    ID = l.ID.ToString(),
                    Title = l.FullName
                }).ToList();

            string s = JSON.ToJSON(new JSResponse(re), jsonParams);
            return s;
        }

        [HttpGet]
        public string GetOrganizeTreeDDL()
        {
            List<OrganizeEntity> list = service.GetTreeOrganizeList("OrderSys", true);
            var re = list.Select(l => new ViewOrganizeDDL()
            {
                ID = l.ID.ToString(),
                ParentID = l.ParentID.ToString(),
                Title = l.FullName
            }).ToList();

            string s = JSON.ToJSON(new JSResponse(re), jsonParams);
            return s;
        } 
        #endregion

        #region OrganizeCategory
        [HttpPost]
        public string AddOrganizeCategory()
        {
            string s = JSRequest.GetRequestFormParm("viewModel");
            ViewOrganizeCategory viewModel = FastJSON.JSON.ToObject<ViewOrganizeCategory>(s);

            //TODO 数据验证。

            OrganizeCategoryEntity OrganizeCategory = new OrganizeCategoryEntity();
            viewModel.CopyTo(OrganizeCategory);

            service.AddOrganizeCategory(OrganizeCategory);
            return JSON.ToJSON(new JSResponse(ResponseType.Remind, "添加成功！"), jsonParams);
        }

        [HttpPost]
        public string EditOrganizeCategory()
        {
            string s = JSRequest.GetRequestFormParm("viewModel");
            ViewOrganizeCategory viewModel = FastJSON.JSON.ToObject<ViewOrganizeCategory>(s);

            //TODO 数据验证。

            OrganizeCategoryEntity OrganizeCategory = new OrganizeCategoryEntity();
            viewModel.CopyTo(OrganizeCategory);

            service.EditOrganizeCategory(OrganizeCategory);
            return JSON.ToJSON(new JSResponse(ResponseType.Remind, "修改成功！"), jsonParams);
        }

        [HttpGet]
        public string GetSingleOrganizeCategory(int organizeCategoryID)
        {
            ViewOrganizeCategory viewModel = new ViewOrganizeCategory();
            OrganizeCategoryEntity entity = service.GetOrganizeCategory(organizeCategoryID);
            entity.CopyTo(viewModel);

            return JSON.ToJSON(new JSResponse(viewModel), jsonParams);
        }

        [HttpGet]
        public string GetOrganizeCategoryList(int pageIndex, int pageSize, string sortField, string sortOrder)
        {
            int count = 0;
            Paging paging = new Paging(pageIndex, pageSize, sortField, sortOrder);

            List<OrganizeCategoryEntity> re = service.GetOrganizeCategorys(paging,out count);
            string s = JSON.ToJSON(new JSResponse(new ListData<OrganizeCategoryEntity>(re, count)), jsonParams);
            return s;
        } 
        #endregion




        [HttpGet]
        public string VerifyOrganizeCode(string organizeCode, string organizeID)
        {
            bool re = false;

            if (service.ChkOrganizeCodeExist(organizeCode, organizeID))
            {
                return JSON.ToJSON(new JSResponse(re), jsonParams);
            }

            re = true;
            return JSON.ToJSON(new JSResponse(re), jsonParams);
        }

        [HttpGet]
        public string VerifyOrganizeCategoryCode(string organizeCategoryCode, string organizeCategoryID)
        {
            bool re = false;

            if (service.ChkOrganizeCategoryCodeExist(organizeCategoryCode, organizeCategoryID))
            {
                return JSON.ToJSON(new JSResponse(re), jsonParams);
            }

            re = true;
            return JSON.ToJSON(new JSResponse(re), jsonParams);
        }


    }

  
}
