using CodeEngine.Framework.QueryBuilder;
using CodeEngine.Framework.QueryBuilder.Enums;
using JSNet.BaseSys;
using JSNet.DbUtilities;
using JSNet.Manager;
using JSNet.Model;
using JSNet.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace JSNet.Service
{
    public class PermissionService:BaseService
    {

        /// <summary>
        /// 添加操作权限
        /// </summary>
        public void AddPermission(UserEntity user,int permissionItemID, int resourceID, string permissionConstraint)
        {
            EntityManager<PermissionEntity> manager = new EntityManager<PermissionEntity>();
            
            PermissionEntity permission = new PermissionEntity();
            permission.PermissionItemID = permissionItemID;
            permission.ResourceID = resourceID;
            permission.CreateUserId = user.ID.ToString();
            permission.CreateBy = user.UserName;
            permission.CreateOn = DateTime.Now;
            manager.Insert(permission);

        }

        /// <summary>
        /// 添加资源权限
        /// </summary>
        public void AddPermissionScope(UserEntity user, int resourceID, int targetID, string permissionConstraint)
        {
            EntityManager<PermissionScopeEntity> manager = new EntityManager<PermissionScopeEntity>();

            PermissionScopeEntity scope = new PermissionScopeEntity();
            scope.PermissionItemID = 13;//资源访问权限，写死的，以后改成1
            scope.ResourceID = resourceID;
            scope.PermissionConstraint = permissionConstraint;
            scope.CreateUserId = user.ID.ToString();
            scope.CreateBy = user.UserName;
            scope.CreateOn = DateTime.Now;
            manager.Insert(scope);
        }

        /// <summary>
        /// 分配操作权限
        /// </summary>
        public void GrantResource(UserEntity user,int roleID,int[] resourceIDs)
        {
            //1.0 清空该角色原有的操作权限
            EntityManager<RoleResourceEntity> manager = new EntityManager<RoleResourceEntity>();
            WhereStatement where = new WhereStatement();
            where.Add(RoleResourceEntity.FieldRoleID, Comparison.Equals, roleID);
            manager.Delete(where);

            //2.0 添加当前选中的操作权限
            List<RoleResourceEntity> list = new List<RoleResourceEntity>();
            foreach (int resourceID in resourceIDs)
            {
                RoleResourceEntity entity = new RoleResourceEntity();
                entity.RoleID = roleID;
                entity.ResourceID = resourceID;
                entity.CreateUserId = user.ID.ToString();
                entity.CreateBy = user.UserName;
                entity.CreateOn = DateTime.Now;
                list.Add(entity);
            }
            manager.Insert(list);
        }

        /// <summary>
        /// 分配资源权限
        /// </summary>
        public void GrantScope(int resourceID, int[] scopeIDs)
        {
            //判断资源类型是否为Menu、button，只有Menu、button才能分配资源明细
            EntityManager<ResourceEntity> resourceManager = new EntityManager<ResourceEntity>();
            ResourceEntity resource = resourceManager.GetSingle(resourceID);
            if (!(resource.ResourceType == ResourceType.Data.ToString()))
            {
                throw new JSException(JSErrMsg.ERR_CODE_NotAllowGrantItem, string.Format(JSErrMsg.ERR_MSG_NotAllowGrantItem, "资源类型为" + resource.ResourceType + "，"));
            }

            UserService userService = new UserService();
            UserEntity currentUser = userService.GetCurrentUser();

            EntityManager<PermissionScopeEntity> manager = new EntityManager<PermissionScopeEntity>();
            manager.Delete(resourceID, PermissionEntity.FieldResourceID);

            List<PermissionScopeEntity> entitys = new List<PermissionScopeEntity>();
            foreach (int scopeID in scopeIDs)
            {
                PermissionScopeEntity entity = new PermissionScopeEntity();
                entity.ResourceID = resourceID;
                entity.TargetID = scopeID;
                entity.PermissionItemID = 2;
                entity.CreateUserId = currentUser.ID.ToString();
                entity.CreateBy = currentUser.UserName;
                entity.CreateOn = DateTime.Now;
                entitys.Add(entity);
            }
            manager.Insert(entitys);
        }

        public Dictionary<string, List<string>> GetRolePermissionScope(RoleEntity role, string resouceCode)
        {
            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
            ViewManager vmanager = new ViewManager("VP_UserRolePermissionScope");

            WhereStatement where = new WhereStatement();
            where.Add("Type", Comparison.Equals, ResourceType.Data.ToString());
            where.Add("RoleID", Comparison.Equals, role.ID);
            where.Add("Resource_Code", Comparison.Equals, resouceCode);

            int count = 0;
            DataTable dt = vmanager.GetDataTable(where, out count);// TODO 这里会有性能问题

            if (count == 0)
            {
                throw new JSException(JSErrMsg.ERR_CODE_NotGrantPermissionScope, JSErrMsg.ERR_MSG_NotGrantPermissionScope);
            }

            foreach(DataRow dr in dt.Rows)
            {
                if(!dic.ContainsKey(dr["OrganizeCategory_Code"].ToString()))
                {
                    List<string> list = new List<string>();
                    list.Add(dr["Organize_Code"].ToString());
                    dic.Add(dr["OrganizeCategory_Code"].ToString(),list);
                    continue;
                }
                dic[dr["OrganizeCategory_Code"].ToString()].Add(dr["Organize_Code"].ToString());
            }
            return dic;
        }

        public bool IsPermissionAuthorizedByRole(RoleEntity role,string controllerName,string actionName)
        {
            DataTable dt = GetAllPermissions();

            foreach (DataRow dr in dt.Rows)
            {
                if(dr["RolePermission_RoleID"].ToString() == role.ID.ToString()
                    &&dr["PermissionItem_Controller"].ToString() == controllerName 
                    && dr["PermissionItem_ActionName"].ToString() == actionName )
                {
                    return true;
                }
            }
            return false;

        }

        public DataTable GetAllPermissions()
        {
            // TODO 先从缓存里面拿，如果没有再从数据库拿
            ViewManager vmanager = new ViewManager("VP_RolePermission");
            WhereStatement where = new WhereStatement();
            where.Add("PermissionItem_IsEnable", Comparison.Equals, (int)TrueFalse.True);

            int count = 0;
            DataTable dt = vmanager.GetDataTable(where, out count);
            return dt;
        }

        #region Resource

        public void AddResource(ResourceEntity entity)
        {
            UserService userService = new UserService();
            UserEntity currentUser = userService.GetCurrentUser();

            EntityManager<ResourceEntity> manager = new EntityManager<ResourceEntity>();
            entity.CreateUserId = currentUser.ID.ToString();
            entity.CreateBy = currentUser.UserName;
            entity.CreateOn = DateTime.Now;
            manager.Insert(entity);
        }

        public void EditResource(ResourceEntity entity)
        {
            UserService userService = new UserService();
            UserEntity currentUser = userService.GetCurrentUser();

            EntityManager<ResourceEntity> manager = new EntityManager<ResourceEntity>();
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(ResourceEntity.FieldParentID, entity.ParentID));
            kvps.Add(new KeyValuePair<string, object>(ResourceEntity.FieldCode, entity.Code));
            kvps.Add(new KeyValuePair<string, object>(ResourceEntity.FieldFullName, entity.FullName));
            kvps.Add(new KeyValuePair<string, object>(ResourceEntity.FieldResourceType, entity.ResourceType));
            kvps.Add(new KeyValuePair<string, object>(ResourceEntity.FieldGroups, entity.Groups));
            kvps.Add(new KeyValuePair<string, object>(ResourceEntity.FieldTarget, entity.Target));
            kvps.Add(new KeyValuePair<string, object>(ResourceEntity.FieldNavigateUrl, entity.NavigateUrl));
            kvps.Add(new KeyValuePair<string, object>(ResourceEntity.FieldSortCode, entity.SortCode));
            kvps.Add(new KeyValuePair<string, object>(ResourceEntity.FieldIsVisible, entity.IsVisible));
            kvps.Add(new KeyValuePair<string, object>(ResourceEntity.FieldIsEnable, entity.IsEnable));
            kvps.Add(new KeyValuePair<string, object>(ResourceEntity.FieldIsPublic, entity.IsPublic));
            kvps.Add(new KeyValuePair<string, object>(ResourceEntity.FieldModifiedUserId, currentUser.ID.ToString()));
            kvps.Add(new KeyValuePair<string, object>(ResourceEntity.FieldModifiedBy, currentUser.UserName));
            kvps.Add(new KeyValuePair<string, object>(ResourceEntity.FieldModifiedOn, DateTime.Now));
            manager.Update(kvps, entity.ID);
        }

        public ResourceEntity GetResource(int resourceID)
        {
            EntityManager<ResourceEntity> manager = new EntityManager<ResourceEntity>();
            ResourceEntity entity = manager.GetSingle(resourceID);
            return entity;
        }

        public DataTable GetResources(out int count)
        {
            ViewManager vmanager = new ViewManager("VP_Resource_Show");
            WhereStatement where = new WhereStatement();

            DataTable dt = vmanager.GetDataTable(where, out count);
            return dt;
        }

        public List<ResourceEntity> GetResourceOfModuleList(string resouceCode, bool onlyChild = true)
        {
            List<ResourceEntity> list = GetResourceList(resouceCode, onlyChild);
            return list.Where(l => l.ResourceType != ResourceType.Data.ToString()).ToList();
        }

        public List<ResourceEntity> GetResourceList(string resouceCode, bool onlyChild = true)
        {
            EntityManager<ResourceEntity> manager = new EntityManager<ResourceEntity>();

            string[] ids = GetTreeResourceIDs(resouceCode);
            if (ids.Length == 0)
            {
                throw new JSException(JSErrMsg.ERR_CODE_DATA_MISSING, string.Format(JSErrMsg.ERR_MSG_DATA_MISSING, "resouceCode"));
            }

            WhereStatement where = new WhereStatement();
            where.Add(ResourceEntity.FieldID, Comparison.In, ids);

            int count = 0;
            List<ResourceEntity> list = manager.GetList(where, out count);

            if (onlyChild)
            {
                list.Where(l => l.Code != resouceCode);
            }

            return list;
        }

        public string[] GetTreeResourceIDs(string resouceCode)
        {
            string[] s = GetTreeIDs(
                "[VP_Resource]",
                "Resource_Code", resouceCode,
                "Resource_ID", "Resource_ParentID");
            return s;
        }

        public bool ChkResourceCodeExist(string resourceCode, string resourceID)
        {
            EntityManager<ResourceEntity> manager = new EntityManager<ResourceEntity>();
            bool b = ChkExist<ResourceEntity>(
                manager,
                ResourceEntity.FieldCode, resourceCode,
                ResourceEntity.FieldID, resourceID);
            return b;
        }

        #region 模块相关
        /// <summary>
        /// 获取对应code下一层的button
        /// </summary>
        /// <param name="role"></param>
        /// <param name="resourceCode"></param>
        /// <returns></returns>
        public DataTable GetButton(RoleEntity role, string resourceCode)
        {
            ViewManager vmanager = new ViewManager("VP_RolePermission");

            WhereStatement where = new WhereStatement();
            where.Add("Resource_SortCode", Comparison.Equals, resourceCode);

            int count = 0;
            DataTable dt = vmanager.GetDataTable(where, out count);

            if (count == 0)
            {
                throw new JSException(JSErrMsg.ERR_CODE_DATA_MISSING, string.Format(JSErrMsg.ERR_MSG_DATA_MISSING, resourceCode));
            }
            if (count > 1)
            {
                throw new JSException(JSErrMsg.ERR_CODE_DATA_REPETITION, string.Format(JSErrMsg.ERR_MSG_DATA_REPETITION, "Resource表的" + resourceCode));
            }

            WhereStatement where1 = new WhereStatement();
            where1.Add("Resource_ParentID", Comparison.Equals, dt.Rows[0]["Resource_ParentID"].ToString());
            where1.Add("RolePermission_RoleID", Comparison.Equals, role.ID);

            int count1 = 0;
            DataTable dt1 = vmanager.GetDataTable(where1, out count1);
            return dt1;

        }

        /// <summary>
        /// 获取 指定资源code 的整个树形menu
        /// </summary>
        /// <param name="role"></param>
        /// <param name="resourceCode">资源code</param>
        /// <returns></returns>
        public DataTable GetLeftMenu(RoleEntity role, string resourceCode)
        {
            //1.0 获取已授权的树形menu
            string[] ids = GetTreeMenuIds(role, resourceCode);
            if (ids.Length == 0)
            {
                throw new JSException(JSErrMsg.ERR_CODE_NotGrantMenuResource, JSErrMsg.ERR_MSG_NotGrantMenuResource);
            }

            //2.0 根据id 获取详细信息
            int count = 0;
            ViewManager vmanager = new ViewManager("VP_RoleResource");

            WhereStatement where = new WhereStatement();
            where.Add("Resource_ID", Comparison.In, ids);
            OrderByStatement order = new OrderByStatement("Resource_SortCode", Sorting.Ascending);

            DataTable dt = vmanager.GetDataTable(where, out count, order);
            return dt;

        }

        public string[] GetTreeMenuIds(RoleEntity role, string resourceCode)
        {
            IDbHelper dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.CenterDbConnectionString);
            IDbDataParameter[] dbParameters = new IDbDataParameter[] { dbHelper.MakeParameter("Resource_Code", resourceCode) };

            string sqlQuery = @" WITH TreeMenu AS (SELECT Resource_ID AS ID
                                        FROM [VP_RoleResource] 
                                        WHERE Resource_Code = " + dbHelper.GetParameter("Resource_Code") + @"
                                        UNION ALL
                                        SELECT ResourceTree.Resource_ID
                                            FROM [VP_RoleResource] AS ResourceTree INNER JOIN
                                                TreeMenu AS A ON A.ID = ResourceTree.Resource_ParentId
                                            WHERE Resource_ResourceType = '" + ResourceType.Menu.ToString() + @"'
                                                AND Resource_IsVisible = " + (int)TrueFalse.True + @"
                                                AND Resource_IsEnable = " + (int)TrueFalse.True + @"
                                                AND Role_ID = " + role.ID + @")
                                SELECT ID
                                    FROM TreeMenu ";
            DataTable dt = dbHelper.Fill(sqlQuery, dbParameters);
            return DataTableUtil.FieldToArray(dt, "ID");
        } 
        #endregion

        #endregion

        #region PermissionItem

        public void AddPermissionItem(PermissionItemEntity entity)
        {
            UserService userService = new UserService();
            UserEntity currentUser = userService.GetCurrentUser();

            EntityManager<PermissionItemEntity> manager = new EntityManager<PermissionItemEntity>();
            entity.AllowEdit = (int)TrueFalse.True;
            entity.AllowDelete = (int)TrueFalse.True;
            entity.CreateUserId = currentUser.ID.ToString();
            entity.CreateBy = currentUser.UserName;
            entity.CreateOn = DateTime.Now;
            manager.Insert(entity);
        }

        public void EditPermissionItem(PermissionItemEntity entity)
        {
            UserService userService = new UserService();
            UserEntity currentUser = userService.GetCurrentUser();

            EntityManager<PermissionItemEntity> manager = new EntityManager<PermissionItemEntity>();
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(PermissionItemEntity.FieldParentID, entity.ParentID));
            kvps.Add(new KeyValuePair<string, object>(PermissionItemEntity.FieldCode, entity.Code));
            kvps.Add(new KeyValuePair<string, object>(PermissionItemEntity.FieldFullName, entity.FullName));
            kvps.Add(new KeyValuePair<string, object>(PermissionItemEntity.FieldSysCategory, entity.SysCategory));
            kvps.Add(new KeyValuePair<string, object>(PermissionItemEntity.FieldController, entity.Controller));
            kvps.Add(new KeyValuePair<string, object>(PermissionItemEntity.FieldActionName, entity.ActionName));
            kvps.Add(new KeyValuePair<string, object>(PermissionItemEntity.FieldActionParameter, entity.ActionParameter));
            kvps.Add(new KeyValuePair<string, object>(PermissionItemEntity.FieldIsEnable, entity.IsEnable));
            kvps.Add(new KeyValuePair<string, object>(PermissionItemEntity.FieldIsPublic, entity.IsPublic));
            kvps.Add(new KeyValuePair<string, object>(PermissionItemEntity.FieldDescription, entity.Description));
            kvps.Add(new KeyValuePair<string, object>(PermissionItemEntity.FieldSortCode, entity.SortCode));
            kvps.Add(new KeyValuePair<string, object>(PermissionItemEntity.FieldModifiedUserId, currentUser.ID.ToString()));
            kvps.Add(new KeyValuePair<string, object>(PermissionItemEntity.FieldModifiedBy, currentUser.UserName));
            kvps.Add(new KeyValuePair<string, object>(PermissionItemEntity.FieldModifiedOn, DateTime.Now));
            manager.Update(kvps, entity.ID);
        }

        public PermissionItemEntity GetPermissionItem(int permissionItemID)
        {
            EntityManager<PermissionItemEntity> manager = new EntityManager<PermissionItemEntity>();
            PermissionItemEntity entity = manager.GetSingle(permissionItemID);
            return entity;
        }

        public void GrantItem(int resourceID,int[] permissionItemIDs)
        {
            //判断资源类型是否为Menu、button，只有Menu、button才能分配资源明细
            EntityManager<ResourceEntity> resourceManager = new EntityManager<ResourceEntity>();
            ResourceEntity resource = resourceManager.GetSingle(resourceID);
            if (!(resource.ResourceType == ResourceType.Menu.ToString() 
                || resource.ResourceType == ResourceType.Button.ToString()))
            {
                throw new JSException(JSErrMsg.ERR_CODE_NotAllowGrantItem, string.Format(JSErrMsg.ERR_MSG_NotAllowGrantItem, "资源类型为" + resource.ResourceType + "，"));
            }

            UserService userService = new UserService();
            UserEntity currentUser = userService.GetCurrentUser();

            EntityManager<PermissionEntity> manager = new EntityManager<PermissionEntity>();
            manager.Delete(resourceID, PermissionEntity.FieldResourceID);

            List<PermissionEntity> entitys = new List<PermissionEntity>();
            foreach (int permissionItemID in permissionItemIDs)
            {
                PermissionEntity entity = new PermissionEntity();
                entity.ResourceID = resourceID;
                entity.PermissionItemID = permissionItemID;
                entity.CreateUserId = currentUser.ID.ToString();
                entity.CreateBy = currentUser.UserName;
                entity.CreateOn = DateTime.Now;
                entitys.Add(entity);
            }
            manager.Insert(entitys);
        }

        public DataTable GetGrantPermissionItemsForShow(string resourceCode,string resourceType, out int count)
        {
            if (!(resourceType == ResourceType.Menu.ToString()
                || resourceType == ResourceType.Button.ToString()))
            {
                throw new JSException(JSErrMsg.ERR_CODE_NotAllowGrantItem, string.Format(JSErrMsg.ERR_MSG_NotAllowGrantItem, "资源类型为" + resourceType + "，"));
            }

            if (resourceCode.Split('.').Length < 2)
            {
                throw new JSException(JSErrMsg.ERR_CODE_ErrorFormatCode, JSErrMsg.ERR_MSG_ErrorFormatCode);
            }
            string parentPermissionItemCode = resourceCode.Split('.')[0];

            DataTable dt = GetPermissionItemsForShow(out count, parentPermissionItemCode);
            return dt;
        }

        public DataTable GetPermissionItemsForShow(out int count, string permissionItemCode = "Resource.ManagePermission")
        {
            string[] permissionItemIDs = GetTreePermissionItemIDs(permissionItemCode);

            ViewManager vmanager = new ViewManager("VP_PermissionItem_Show");
            WhereStatement where = new WhereStatement();
            where.Add("PermissionItem_ID", Comparison.In, permissionItemIDs);

            DataTable dt = vmanager.GetDataTable(where, out count);
            return dt;
        }

        public List<PermissionItemEntity> GetTreePermissionItemList(string permissionItemCode, bool onlyChild=true)
        {
            EntityManager<PermissionItemEntity> manager = new EntityManager<PermissionItemEntity>();

            string[] ids = GetTreePermissionItemIDs(permissionItemCode);
            if (ids.Length == 0)
            {
                throw new JSException(JSErrMsg.ERR_CODE_DATA_MISSING, string.Format(JSErrMsg.ERR_MSG_DATA_MISSING, "permissionItemCode"));
            }

            WhereStatement where = new WhereStatement();
            where.Add(PermissionItemEntity.FieldID, Comparison.In, ids);

            int count = 0;
            List<PermissionItemEntity> list = manager.GetList(where, out count);

            if (onlyChild)
            {
                list.Where(l => l.Code != permissionItemCode);
            }

            return list;
        }

        public string[] GetTreePermissionItemIDs(string permissionItemCode)
        {
            string[] s = GetTreeIDs(
                "[VP_PermissionItem]", 
                "PermissionItem_Code", permissionItemCode, 
                "PermissionItem_ID", "PermissionItem_ParentID");
            return s;
        }

        public bool ChkPermissionItemCodeExist(string permissionItemCode, string permissionItemID)
        {
            EntityManager<PermissionItemEntity> manager = new EntityManager<PermissionItemEntity>();
            bool b = ChkExist<PermissionItemEntity>(
                manager, 
                PermissionItemEntity.FieldCode, permissionItemCode, 
                PermissionItemEntity.FieldID, permissionItemID);
            return b;
        }

        #endregion

        public DataTable GetScopeTreeDT(string parentCode)
        {
            ViewManager vmanager = new ViewManager("VP_PermissionScope");

            string[] ids = GetTreeScopeIDs(parentCode);
            if (ids.Length == 0)
            {
                return new DataTable("JSNet");
            }

            WhereStatement where = new WhereStatement();
            where.Add("Resource_ID", Comparison.In, ids);

            int count = 0;
            DataTable dt = vmanager.GetDataTable(where, out count);
            return dt;
        }

        public string[] GetTreeScopeIDs(string parentCode)
        {
            string[] s = GetTreeIDs(
                "[VP_PermissionScope]",
                "Resource_Code", parentCode,
                "Resource_ID", "Resource_ParentID");
            return s;
        }

        public int[] GetGrantedItemIDs(int reourceID)
        {
            EntityManager<PermissionEntity> manager = new EntityManager<PermissionEntity>();
            WhereStatement where = new WhereStatement();
            where.Add(PermissionEntity.FieldResourceID, Comparison.Equals, reourceID);

            string[] sScopeIDs = manager.GetProperties(PermissionEntity.FieldPermissionItemID, where);
            int[] scopeIDs = CommonUtil.ConvertToIntArry(sScopeIDs);
            return scopeIDs;
        }

        public int[] GetGrantedScopeIDs(int reourceID)
        {
            EntityManager<PermissionScopeEntity> manager = new EntityManager<PermissionScopeEntity>();
            WhereStatement where = new WhereStatement();
            where.Add(PermissionScopeEntity.FieldResourceID, Comparison.Equals, reourceID);

            string[] sItems = manager.GetProperties(PermissionScopeEntity.FieldTargetID, where);
            int[] itemIDs = CommonUtil.ConvertToIntArry(sItems);
            return itemIDs;
        }
    }
}
