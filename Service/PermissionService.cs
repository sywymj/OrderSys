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
        public JSDictionary GetRoleDDL ()
        {
            int count = 0;
            EntityManager<RoleEntity> roleManager = new EntityManager<RoleEntity>();
            WhereStatement where = new WhereStatement();
            List<RoleEntity> list = roleManager.GetList(where, out count);

            JSDictionary re = list.ToJSDictionary(Key => Key.ID, Value => Value.FullName);
            return re;
        }

        public void AddPermissionItem(PermissionItemEntity entity)
        {
            UserService userService = new UserService();
            EntityManager<PermissionItemEntity> manager = new EntityManager<PermissionItemEntity>();

            UserEntity user = userService.GetCurrentUser();

            entity.IsEnable = (int)TrueFalse.True;
            entity.AllowDelete = 1;
            entity.AllowEdit = 1;
            entity.DeletionStateCode = (int)TrueFalse.True;
            entity.CreateUserId = user.ID.ToString();
            entity.CreateBy = user.UserName;
            entity.CreateOn = DateTime.Now;
            manager.Insert(entity);
        }

        public void AddResource(ResourceEntity entity)
        {
            UserService userService = new UserService();
            EntityManager<ResourceEntity> manager = new EntityManager<ResourceEntity>();

            UserEntity user = userService.GetCurrentUser();

            entity.IsEnable = (int)TrueFalse.True;
            entity.DeletionStateCode = (int)TrueFalse.True;
            entity.CreateUserId = user.ID.ToString();
            entity.CreateBy = user.UserName;
            entity.CreateOn = DateTime.Now;
            manager.Insert(entity);
        }

        public void AddRole(RoleEntity entity)
        {
            UserService userService = new UserService();
            EntityManager<RoleEntity> manager = new EntityManager<RoleEntity>();

            UserEntity user = userService.GetCurrentUser();

            entity.DeletionStateCode = (int)TrueFalse.True;
            entity.CreateUserId = user.ID.ToString();
            entity.CreateBy = user.UserName;
            entity.CreateOn = DateTime.Now;
            manager.Insert(entity);
        }

        /// <summary>
        /// 添加操作权限
        /// </summary>
        public void AddPermission(UserEntity user,int permissionItemID, int resourceID, string permissionConstraint)
        {
            EntityManager<PermissionEntity> manager = new EntityManager<PermissionEntity>();
            
            PermissionEntity permission = new PermissionEntity();
            permission.PermissionItemID = permissionItemID;
            permission.ResourceID = resourceID;
            permission.PermissionConstraint = permissionConstraint;
            permission.DeletionStateCode = (int)TrueFalse.True;
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
            scope.DeletionStateCode = (int)TrueFalse.True;
            scope.CreateUserId = user.ID.ToString();
            scope.CreateBy = user.UserName;
            scope.CreateOn = DateTime.Now;
            manager.Insert(scope);
        }

        /// <summary>
        /// 分配操作权限
        /// </summary>
        public void GrantPermission(UserEntity user,int roleID,int[] permissionIDs)
        {
            //1.0 清空该角色原有的操作权限
            EntityManager<RolePermissionEntity> manager = new EntityManager<RolePermissionEntity>();
            WhereStatement where = new WhereStatement();
            where.Add(RolePermissionEntity.FieldRoleID,Comparison.Equals,roleID);
            manager.Delete(where);

            //2.0 添加当前选中的操作权限
            foreach (int id in permissionIDs)
            {
                RolePermissionEntity entity = new RolePermissionEntity();
                entity.RoleID = roleID;
                entity.PermissionID = id;
                entity.DeletionStateCode = (int)TrueFalse.True;
                entity.CreateUserId = user.ID.ToString();
                entity.CreateBy = user.UserName;
                entity.CreateOn = DateTime.Now;
                manager.Insert(entity);
            }
        }

        /// <summary>
        /// 分配资源权限
        /// </summary>
        public void GrantPermissionScope(UserEntity user, int roleID, int[] permissionScopeIDs)
        {
            //1.0 清空该角色原有的操作权限
            EntityManager<RolePermissionScopeEntity> manager = new EntityManager<RolePermissionScopeEntity>();
            WhereStatement where = new WhereStatement();
            where.Add(RolePermissionScopeEntity.FieldRoleID, Comparison.Equals, roleID);
            manager.Delete(where);

            //2.0 添加当前选中的操作权限
            foreach (int id in permissionScopeIDs)
            {
                RolePermissionScopeEntity entity = new RolePermissionScopeEntity();
                entity.RoleID = roleID;
                entity.PermissionScopeID = id;
                entity.DeletionStateCode = (int)TrueFalse.True;
                entity.CreateUserId = user.ID.ToString();
                entity.CreateBy = user.UserName;
                entity.CreateOn = DateTime.Now;
                manager.Insert(entity);
            }
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

        /// <summary>
        /// 获取 指定资源code 的整个树形menu
        /// </summary>
        /// <param name="role"></param>
        /// <param name="resourceCode">资源code</param>
        /// <returns></returns>
        public DataTable GetMenu(RoleEntity role,string resourceCode)
        {
            //1.0 获取已授权的树形menu
            string[] ids = GetTreeMenuIds(role,resourceCode);
            if (ids.Length == 0)
            {
                throw new JSException(JSErrMsg.ERR_CODE_NotGrantMenuResource, JSErrMsg.ERR_MSG_NotGrantMenuResource);
            }

            //2.0 根据id 获取详细信息
            ViewManager vmanager = new ViewManager("VP_RolePermission");

            WhereStatement where = new WhereStatement();
            where.Add("Resource_ID", Comparison.In, ids);

            OrderByStatement order = new OrderByStatement("Resource_SortCode", Sorting.Ascending);

            int count = 0;
            DataTable dt = vmanager.GetDataTable(where, out count, order);

            return dt;
            
        }

        public string[] GetTreeMenuIds(RoleEntity role,string resourceCode)
        {
            IDbHelper dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.CenterDbConnectionString);
            IDbDataParameter[] dbParameters = new IDbDataParameter[] { dbHelper.MakeParameter("Resource_Code", resourceCode) };

            string sqlQuery = @" WITH TreeMenu AS (SELECT Resource_ID AS ID
                                        FROM [VP_RolePermission] 
                                        WHERE Resource_Code = " + dbHelper.GetParameter("Resource_Code") + @"
                                        UNION ALL
                                        SELECT ResourceTree.Resource_ID
                                            FROM [VP_RolePermission] AS ResourceTree INNER JOIN
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
            where.Add("Resource_SortCode",Comparison.Equals, resourceCode);

            int count = 0;
            DataTable dt = vmanager.GetDataTable(where, out count);

            if (count == 0)
            {
                throw new JSException(JSErrMsg.ERR_CODE_DATA_MISSING, string.Format(JSErrMsg.ERR_MSG_DATA_MISSING, resourceCode));
            }
            if (count > 1)
            {
                throw new JSException(JSErrMsg.ERR_CODE_DATA_REPETITION, string.Format(JSErrMsg.ERR_MSG_DATA_REPETITION,"Resource表的"+ resourceCode));
            }

            WhereStatement where1 = new WhereStatement();
            where1.Add("Resource_ParentID", Comparison.Equals, dt.Rows[0]["Resource_ParentID"].ToString());
            where1.Add("RolePermission_RoleID", Comparison.Equals, role.ID);

            int count1 = 0;
            DataTable dt1 = vmanager.GetDataTable(where1, out count1);
            return dt1;

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
        
    }
}
