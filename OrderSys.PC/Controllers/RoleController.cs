using JSNet.Utilities;
using FastJSON;
using JSNet.BaseSys;
using JSNet.Model;
using JSNet.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.Mvc;
using System.Data;

namespace OrderSys.Admin.Controllers
{
    [ManagerAuthorize]
    public class RoleController : AdminBaseController
    {
        //
        // GET: /Role/
        MyRoleService service = new MyRoleService();

        #region INDEX
        [HttpGet]
        public ActionResult RoleIndex()
        {
            return View("~/Areas/Admin/Views/Role/Role_Index.cshtml");
        }

        [HttpGet]
        public string GetRoleDT()
        {
            DataTable dt = service.GetTreeRoleDTByRole(service.CurrentRole);

            string s = JSON.ToJSON(new JSResponse(new DataTableData(dt)), jsonParams);
            return s;
        } 
        #endregion

        #region INSERT
        [HttpGet]
        public ActionResult InsertRoleIndex()
        {
            return View("~/Areas/Admin/Views/Role/Role_InsertIndex.cshtml");
        }

        [HttpPost]
        public string AddRole()
        {
            string s = JSRequest.GetRequestFormParm("ViewModel");
            ViewRole viewModel = FastJSON.JSON.ToObject<ViewRole>(s);

            //TODO 数据验证。

            RoleEntity entity = new RoleEntity();
            viewModel.CopyTo(entity);
            service.AddRole(entity);
            return JSON.ToJSON(new JSResponse(ResponseType.Remind, "添加成功！"), jsonParams);
        } 
        #endregion

        #region EDIT
        [HttpGet]
        public ActionResult EditRoleIndex()
        {
            return View("~/Areas/Admin/Views/Role/Role_InsertIndex.cshtml");
        }

        [HttpPost]
        public string EditRole()
        {
            //获取参数
            string s = JSRequest.GetRequestFormParm("ViewModel");
            ViewRole viewModel = FastJSON.JSON.ToObject<ViewRole>(s);

            //TODO 数据验证。

            //ViewModel赋值
            RoleEntity entity = new RoleEntity();
            viewModel.CopyTo(entity);

            //调用Service
            service.EditRole(entity);
            return JSON.ToJSON(new JSResponse(ResponseType.Remind, "修改成功！"), jsonParams);
        }

        [HttpGet]
        public string GetSingleRole(int roleID)
        {
            ViewRole viewModel = new ViewRole();
            RoleEntity entity = service.GetRole(roleID);
            entity.CopyTo(viewModel);

            return JSON.ToJSON(new JSResponse(viewModel), jsonParams);
        } 
        #endregion

        [HttpGet]
        public string GetRoleDDL()
        {
            List<RoleEntity> list = service.GetRoleListByRole(service.CurrentRole);
            var re = list.Select(l =>
                new ViewRoleDDL()
                {
                    ID = l.ID.ToString(),
                    Title = l.FullName
                }).ToList();

            string s = JSON.ToJSON(new JSResponse(re), jsonParams);

            return s;
        }

    }
}
