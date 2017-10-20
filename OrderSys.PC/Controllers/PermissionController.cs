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
    public class PermissionController : AdminBaseController
    {
        //
        // GET: /Permission/
        private PermissionService service = new PermissionService();

        #region Resource

        #region ResourceIndex
        [HttpGet]
        public ActionResult ResourceIndex()
        {
            return View("~/Areas/Admin/Views/Permission/Resource_Index.cshtml");
        }

        [HttpGet]
        public string GetResourceList()
        {
            int count = 0;
            DataTable re = service.GetResourceDT(out count);

            string s = JSON.ToJSON(new JSResponse(new DataTableData(re, count)), jsonParams);
            return s;
        }
        #endregion

        #region InsertResource
        [HttpGet]
        public ActionResult InsertResourceIndex()
        {
            return View("~/Areas/Admin/Views/Permission/Resource_InsertIndex.cshtml");
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
        #endregion

        #region EditResource
        [HttpGet]
        public ActionResult EditResourceIndex()
        {
            return View("~/Areas/Admin/Views/Permission/Resource_InsertIndex.cshtml");
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
        public string GetSingleResource(int resourceID)
        {
            ResourceEntity entity = service.GetResource(resourceID);

            ViewResource viewModel = new ViewResource();
            entity.CopyTo(viewModel);

            return JSON.ToJSON(new JSResponse(viewModel), jsonParams);
        } 
        #endregion

        #endregion

        #region PermissionItem

        #region ItemIndex
        [HttpGet]
        public ActionResult PermissionItemIndex()
        {
            return View("~/Areas/Admin/Views/Permission/PermissionItem_Index.cshtml");
        }

        [HttpGet]
        public string GetPermissionItemDT()
        {
            int count = 0;
            DataTable re = service.GetTreePermissionItemDT(out count);

            string s = JSON.ToJSON(new JSResponse(new DataTableData(re, count)), jsonParams);
            return s;
        } 
        #endregion

        #region InsertItem
        [HttpGet]
        public ActionResult InsertPermissionItemIndex()
        {
            return View("~/Areas/Admin/Views/Permission/PermissionItem_InsertIndex.cshtml");
        }

        [HttpPost]
        public string AddPermissionItem()
        {
            string s = JSRequest.GetRequestFormParm("viewModel");
            ViewPermissionItem viewModel = FastJSON.JSON.ToObject<ViewPermissionItem>(s);

            //TODO 数据验证。

            PermissionItemEntity entity = new PermissionItemEntity();
            viewModel.CopyTo(entity);

            service.AddPermissionItem(entity);
            return JSON.ToJSON(new JSResponse(ResponseType.Remind, "添加成功！"), jsonParams);
        } 
        #endregion

        #region EditItem
        [HttpGet]
        public ActionResult EditPermissionItemIndex()
        {
            return View("~/Areas/Admin/Views/Permission/PermissionItem_InsertIndex.cshtml");
        }

        [HttpPost]
        public string EditPermissionItem()
        {
            string s = JSRequest.GetRequestFormParm("viewModel");
            ViewPermissionItem viewModel = FastJSON.JSON.ToObject<ViewPermissionItem>(s);

            //TODO 数据验证。

            PermissionItemEntity entity = new PermissionItemEntity();
            viewModel.CopyTo(entity);

            service.EditPermissionItem(entity);
            return JSON.ToJSON(new JSResponse(ResponseType.Remind, "修改成功！"), jsonParams);
        }

        [HttpGet]
        public string GetSinglePermissionItem(int permissionItemID)
        {
            ViewPermissionItem viewModel = new ViewPermissionItem();
            PermissionItemEntity entity = service.GetPermissionItem(permissionItemID);
            entity.CopyTo(viewModel);

            return JSON.ToJSON(new JSResponse(viewModel), jsonParams);
        } 
        #endregion

        #endregion

        #region Grant_Item

        [HttpGet]
        public ActionResult GrantItemIndex()
        {
            return View("~/Areas/Admin/Views/Permission/GrantItem_Index.cshtml");
        }

        [HttpGet]
        public string GetGrantItemDT()
        {
            string resourceCode = JSRequest.GetRequestUrlParm("resourceCode");
            string resourceType = JSRequest.GetRequestUrlParm("resourceType");

            int count = 0;
            DataTable re = service.GetGrantItemsDT(resourceCode, resourceType, out count);

            string s = JSON.ToJSON(new JSResponse(new DataTableData(re, count)), jsonParams);
            return s;
        }

        [HttpGet]
        public string GrantItem()
        {
            string sReourceID = JSRequest.GetRequestUrlParm("reourceID", true);
            string sPermissionItemIDs = JSRequest.GetRequestUrlParm("permissionItemIDs", false);

            int resourceID = (int)JSValidator.ValidateInt("资源ID", sReourceID, true);
            int[] permissionItemIDs = JSValidator.ValidateStrings("资源明细ID", sPermissionItemIDs, false);

            service.GrantItem(resourceID, permissionItemIDs);

            string s = JSON.ToJSON(new JSResponse(ResponseType.Remind, "配置成功！"), jsonParams);
            return s;
        }

        [HttpGet]
        public string GetGrantedItemIDs()
        {
            string sReourceID = JSRequest.GetRequestUrlParm("resourceID");
            int resourceID = (int)JSValidator.ValidateInt("资源ID", sReourceID, true);

            int[] itemIDs = service.GetGrantedItemIDs(resourceID);
            string re = string.Join(",", itemIDs);
            string s = JSON.ToJSON(new JSResponse(data: re), jsonParams);
            return s;
        }

        #endregion

        #region Grant_Scope

        [HttpGet]
        public ViewResult GrantScopeIndex()
        {
            return View("~/Areas/Admin/Views/Permission/GrantScope_Index.cshtml");
        }

        [HttpGet]
        public string GetGrantScopeList()
        {
            string sResourceCode = JSRequest.GetRequestUrlParm("ResourceCode");
            string sResourceType = JSRequest.GetRequestUrlParm("ResourceType");

            int count = 0;
            DataTable re = service.GetGrantScopeDT(sResourceCode, sResourceType, out count);
            string s = JSON.ToJSON(new JSResponse(new DataTableData(re, count)), jsonParams);
            return s;
        }

        [HttpGet]
        public string GrantScope()
        {
            string sReourceID = JSRequest.GetRequestUrlParm("ResourceID", true);
            string sScopeIDs = JSRequest.GetRequestUrlParm("OrganizeIDs", false);

            int resourceID = (int)JSValidator.ValidateInt("资源ID", sReourceID, true);
            int[] scopeIDs = JSValidator.ValidateStrings("资源对象ID", sScopeIDs, false);

            service.GrantScope(resourceID, scopeIDs);

            string s = JSON.ToJSON(new JSResponse(ResponseType.Remind, "配置成功！"), jsonParams);
            return s;
        }

        [HttpGet]
        public string GetGrantedScopeIDs()
        {
            string sReourceID = JSRequest.GetRequestUrlParm("ResourceID");
            int resourceID = (int)JSValidator.ValidateInt("资源ID", sReourceID, true);

            int[] scopeIDs = service.GetGrantedScopeIDs(resourceID);
            string re = string.Join(",", scopeIDs);
            string s = JSON.ToJSON(new JSResponse(data: re), jsonParams);
            return s;
        }

        #endregion

        #region GRANT_MODULE

        [HttpGet]
        public ActionResult GrantPermissionModuleIndex()
        {
            return View("~/Areas/Admin/Views/Permission/GrantPermissionModule_Index.cshtml");
        }

        [HttpGet]
        public string GetGrantPermissionModuleList()
        {
            List<ResourceEntity> modules = service.GetModuleList("OrderSys");

            string s = JSON.ToJSON(new JSResponse(new ListData<ResourceEntity>(modules)), jsonParams);
            return s;
        }

        [HttpGet]
        public string GrantPermissionModule()
        {
            string sRoleID = JSRequest.GetRequestUrlParm("RoleID", true);
            string sModuleIDs = JSRequest.GetRequestUrlParm("ModuleIDs", false);

            int roleID = (int)JSValidator.ValidateInt("角色ID", sRoleID, true);
            int[] moduleIDs = JSValidator.ValidateStrings("模块ID", sModuleIDs, false);

            service.GrantModule(roleID, moduleIDs);

            string s = JSON.ToJSON(new JSResponse(ResponseType.Remind, "配置成功！"), jsonParams);
            return s;
        }

        [HttpGet]
        public string GetGrantedPermissionModuleIDs()
        {
            string sRoleID = JSRequest.GetRequestUrlParm("RoleID");
            int roleID = (int)JSValidator.ValidateInt("资源ID", sRoleID, true);

            int[] moduleIDs = service.GetGrantedModuleIDs(roleID);
            string re = string.Join(",", moduleIDs);
            string s = JSON.ToJSON(new JSResponse(data: re), jsonParams);
            return s;
        }

        #endregion

        #region GRANT_PermissionScope

        [HttpGet]
        public ActionResult GrantPermissionScopeIndex()
        {
            return View("~/Areas/Admin/Views/Permission/GrantPermissionScope_Index.cshtml");
        }

        [HttpGet]
        public string GetGrantPermissionScopeDT()
        {
            int count = 0;
            //获取当前登陆的角色所属的系统类型，从而获取属性列表

            DataTable dt = service.GetTreePermissionScopeDTByRole(service.CurrentRole);

            string s = JSON.ToJSON(new JSResponse(new DataTableData(dt, count)), jsonParams);
            return s;
        }

        [HttpGet]
        public string GetGrantDataResourceList()
        {
            string sRoleID = JSRequest.GetRequestUrlParm("RoleID", true);
            int roleID = (int)JSValidator.ValidateInt("角色ID", sRoleID, true);

            DataTable dt = service.GetGrantedDataResourceByRole(service.CurrentRole);
            int[] permissionScopeIDs = service.GetGrantedPermissionScopeIDs(roleID);//获取该角色已分配的资源对象

            List<ViewDataResource> dataResources = new List<ViewDataResource>();
            foreach(DataRow dr in dt.Rows)
            {
                ViewDataResource dataResource = dataResources.FirstOrDefault(r => r.ID == dr["Resource_ID"].ToString());
                if (dataResource == null)
                {
                    dataResource = new ViewDataResource
                    {
                        ID = dr["Resource_ID"].ToString(),
                        ParentID = dr["Resource_ParentID"].ToString(),
                        Title = dr["Resource_FullName"].ToString(),
                    };
                    dataResources.Add(dataResource);
                }
                if (dataResource.Expands == null)
                {
                    dataResource.Expands = new List<ViewPermissionScopeEntity>();
                }
                //添加扩展内容
                ViewPermissionScopeEntity permissionScope = dataResource.Expands.FirstOrDefault(ps => ps.ID == dr["PermissionScope_ID"].ToString());
                if (permissionScope == null)
                {
                    if (string.IsNullOrEmpty(dr["PermissionScope_ID"].ToString()))
                    {
                        continue;
                    }
                    permissionScope = new ViewPermissionScopeEntity()
                    {
                        ID = dr["PermissionScope_ID"].ToString(),
                        Title = dr["Organize_FullName"].ToString(),
                    };

                    if (permissionScopeIDs.Contains(Convert.ToInt32(dr["PermissionScope_ID"].ToString())))
                    {
                        permissionScope.Checked = true;
                    };
                    dataResource.Expands.Add(permissionScope);
                };
            }

            string s = JSON.ToNiceJSON(new JSResponse(dataResources), jsonParams);
            return s;
        }

        [HttpGet]
        public string GrantPermissionScope()
        {
            string sRoleID = JSRequest.GetRequestUrlParm("RoleID", true);
            string sScopeIDs = JSRequest.GetRequestUrlParm("ScopeIDs", false);

            int roleID = (int)JSValidator.ValidateInt("角色ID", sRoleID, true);
            int[] scopeIDs = JSValidator.ValidateStrings("资源ID", sScopeIDs, false);

            service.GrantPermissionScope(roleID, scopeIDs);

            string s = JSON.ToJSON(new JSResponse(ResponseType.Remind, "配置成功！"), jsonParams);
            return s;
        }

        [HttpGet]
        public string GetGrantedPermissionScopeIDs()
        {
            string sRoleID = JSRequest.GetRequestUrlParm("RoleID");
            int roleID = (int)JSValidator.ValidateInt("资源ID", sRoleID, true);

            int[] scopeIDs = service.GetGrantedPermissionScopeIDs(roleID);
            string re = string.Join(",", scopeIDs);
            string s = JSON.ToJSON(new JSResponse(data: re), jsonParams);
            return s;
        }

        #endregion

        #region DDL
        [HttpGet]
        public string GetResourceTreeDDL()
        {
            List<ResourceEntity> list = service.GetResourceList("OrderSys", true);
            var re = list.Select(l => new ViewResourceDDL()
            {
                ID = l.ID.ToString(),
                ParentID = l.ParentID.ToString(),
                Title = l.FullName
            }).ToList();

            string s = JSON.ToJSON(new JSResponse(re), jsonParams);
            return s;
        }
        
        /// <summary>
        /// 获取 Resource Code ，用作方便输入Code前缀
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetResourceCodeDDL()
        {
            List<ResourceEntity> list = service.GetResourceList("OrderSys", true);
            var re = list.Select(l => new ViewResourceCodeDDL()
            {
                ID = l.Code,
                Title = l.Code
            }).ToList();

            string s = JSON.ToJSON(new JSResponse(re), jsonParams);
            return s;
        }

        [HttpGet]
        public string GetPermissionItemTreeDDL()
        {
            List<PermissionItemEntity> list = service.GetTreePermissionItemList("OrderSys", true);
            var re = list.Select(l => new ViewPermissionItemDDL()
            {
                ID = l.ID.ToString(),
                ParentID = l.ParentID.ToString(),
                Title = l.FullName
            }).ToList();

            string s = JSON.ToJSON(new JSResponse(re), jsonParams);
            return s;
        }

        /// <summary>
        /// 获取 PermissionItem Code ，用作方便输入Code前缀
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetPermissionItemCodeDDL()
        {
            List<PermissionItemEntity> list = service.GetTreePermissionItemList("OrderSys", true);
            var re = list.Select(l => new ViewPermissionItemDDL()
            {
                ID = l.Code,
                Title = l.Code
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

        [HttpGet]
        public string GetSysCategoryDDL()
        {
            Dictionary<SysCategory, string> dic = EnumExtensions.ConvertToEnumDic<SysCategory>();
            var re = dic.Select(d => new ViewSysCategoryDDL()
            {
                ID = d.Key.ToString(),
                Title = d.Value
            }).ToList();

            string s = JSON.ToJSON(new JSResponse(re), jsonParams);
            return s;
        } 
        #endregion

        //验证
        [HttpGet]
        public string VerifyResourceCode(string resourceCode, string resourceID)
        {
            bool re = false;

            if (service.ChkResourceCodeExist(resourceCode, resourceID))
            {
                return JSON.ToJSON(new JSResponse(re), jsonParams);
            }

            re = true;
            return JSON.ToJSON(new JSResponse(re), jsonParams);
        }

        [HttpGet]
        public string VerifyPermissionItemCode(string permissionItemCode, string permissionItemID)
        {
            bool re = false;

            if (service.ChkPermissionItemCodeExist(permissionItemCode, permissionItemID))
            {
                return JSON.ToJSON(new JSResponse(re), jsonParams);
            }

            re = true;
            return JSON.ToJSON(new JSResponse(re), jsonParams);
        }
    }
}
