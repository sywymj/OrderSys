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

namespace OrderSys.Admin.Controllers
{
    public class RoleController : AdminBaseController
    {
        //
        // GET: /Role/
        MyRoleService service = new MyRoleService();
        public ActionResult RoleIndex()
        {
            return View("~/Areas/Admin/Views/Role/Role_Index.cshtml");
        }

        [HttpGet]
        public string GetRoleDDL()
        {

            RoleEntity role = service.GetCurrentRole();

            List<RoleEntity> list = service.GetRoleList();

            var re = list.Select(l =>
                new ViewRoleDDL()
                {
                    ID = l.ID.ToString(),
                    Title = l.FullName
                }).ToList();

            string s = JSON.ToJSON(new JSResponse(re), jsonParams);

            return s;
        }

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
            RoleEntity Role = new RoleEntity();
            viewModel.CopyTo(Role);

            //调用Service
            service.EditRole(Role);
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

        [HttpGet]
        public string GetRoleListByPage(int pageIndex, int pageSize, string sortField, string sortOrder)
        {
            int count = 0;
            Paging paging = new Paging(pageIndex, pageSize, sortField, sortOrder);
            List<RoleEntity> re = service.GetRoleList(paging, out count);

            string s = JSON.ToJSON(new JSResponse(new ListData<RoleEntity>(re, count)), jsonParams);
            return s;
        }

        [HttpGet]
        public ActionResult GrantRoleModuleIndex()
        {
            return View("~/Areas/Admin/Views/Role/GrantRoleModule_Index.cshtml");
        }

        [HttpGet]
        public string GrantRoleModule()
        {
            string sRoleID = JSRequest.GetRequestUrlParm("RoleID", true);
            string sModuleIDs = JSRequest.GetRequestUrlParm("ModuleIDs", false);

            int roleID = (int)JSValidator.ValidateInt("角色ID", sRoleID, true);
            int[] moduleIDs = JSValidator.ValidateStrings("模块ID", sModuleIDs, false);

            service.GrantRoleModule(roleID, moduleIDs);

            string s = JSON.ToJSON(new JSResponse(ResponseType.Remind, "配置成功！"), jsonParams);
            return s;
        }

        [HttpGet]
        public string GetGrantedModuleIDs()
        {
            string sRoleID = JSRequest.GetRequestUrlParm("RoleID");
            int roleID = (int)JSValidator.ValidateInt("资源ID", sRoleID, true);

            int[] moduleIDs = service.GetGrantedModuleIDs(roleID);
            string re = string.Join(",", moduleIDs);
            string s = JSON.ToJSON(new JSResponse(data: re), jsonParams);
            return s;
        }


        [HttpGet]
        public ActionResult GrantRoleScopeIndex()
        {
            return View("~/Areas/Admin/Views/Role/GrantRoleScope_Index.cshtml");
        }

        [HttpGet]
        public string GrantRoleScope()
        {
            string sRoleID = JSRequest.GetRequestUrlParm("RoleID", true);
            string sScopeIDs = JSRequest.GetRequestUrlParm("ScopeIDs", false);

            int roleID = (int)JSValidator.ValidateInt("角色ID", sRoleID, true);
            int[] scopeIDs = JSValidator.ValidateStrings("资源ID", sScopeIDs, false);

            service.GrantRoleScope(roleID, scopeIDs);

            string s = JSON.ToJSON(new JSResponse(ResponseType.Remind, "配置成功！"), jsonParams);
            return s;
        }

        [HttpGet]
        public string GetGrantedScopeIDs()
        {
            string sRoleID = JSRequest.GetRequestUrlParm("RoleID");
            int roleID = (int)JSValidator.ValidateInt("资源ID", sRoleID, true);

            int[] scopeIDs = service.GetGrantedScopeIDs(roleID);
            string re = string.Join(",", scopeIDs);
            string s = JSON.ToJSON(new JSResponse(data: re), jsonParams);
            return s;
        }
    }
}
