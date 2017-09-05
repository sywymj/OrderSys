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
    public class PermissionService
    {
        public void AddUser(UserEntity entity, int roleID)
        {
            entity.IsEnable = 1;
            entity.IsEnable = 1;
            entity.OpenID = null;

            EntityManager<UserEntity> userManager = new EntityManager<UserEntity>();
            string userID = userManager.Insert(entity);

            EntityManager<RoleUserEntity> roleUserManager = new EntityManager<RoleUserEntity>();
            roleUserManager.Insert(new RoleUserEntity()
            {
                RoleID = roleID,
                UserID = Convert.ToInt32(userID),
            });
        }

        public void EditUser(UserEntity entity)
        {
        
        }

        public void DeleteStaff(int id)
        {

            EntityManager<UserEntity> manamger = new EntityManager<UserEntity>();
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(StaffEntity.FieldIsOnJob, 0));
            manamger.Update(kvps, id);
        }

        public UserEntity GetCurrentUser()
        {
            
            //string openID = JSRequest.GetSessionParm("OpenID").ToString();

            string openID = "1";//调试用

            UserEntity user = new UserEntity();
            EntityManager<UserEntity> manager = new EntityManager<UserEntity>();

            user = manager.GetSingle(openID, UserEntity.FieldOpenID);
            if (user == null)
            {
                throw new JSException(JSErrMsg.ERR_CODE_DATA_MISSING, string.Format(JSErrMsg.ERR_MSG_DATA_MISSING, "用户"));
            }

            return user;
        }

        public StaffEntity GetCurrentStaff()
        {
            EntityManager<StaffEntity> manager = new EntityManager<StaffEntity>();

            UserEntity user = GetCurrentUser();

            StaffEntity staff = manager.GetSingle(user.ID, StaffEntity.FieldID);
            if (staff == null)
            {
                throw new JSException(JSErrMsg.ERR_CODE_DATA_MISSING, string.Format(JSErrMsg.ERR_MSG_DATA_MISSING, "员工"));
            }

            return staff;
        }

        public List<StaffEntity> GetAllStaffs()
        {
            EntityManager<StaffEntity> manager = new EntityManager<StaffEntity>();

            WhereStatement where = new WhereStatement();
            where.Add(StaffEntity.FieldIsEnable, Comparison.Equals,(int)TrueFalse.True);
            where.Add(StaffEntity.FieldIsOnJob, Comparison.Equals,(int)TrueFalse.True);

            int count =0;
            List<StaffEntity> list = manager.GetList(where, out count);
            return list;
        }
        public JSDictionary GetRoleDDL ()
        {
            int count = 0;
            EntityManager<RoleEntity> roleManager = new EntityManager<RoleEntity>();
            WhereStatement where = new WhereStatement();
            List<RoleEntity> list = roleManager.GetList(where, out count);

            JSDictionary re = list.ToJSDictionary(Key => Key.ID, Value => Value.Title);
            return re;
        }


        public void AddPermissionItem(PermissionItemEntity entity)
        {
            EntityManager<PermissionItemEntity> manager = new EntityManager<PermissionItemEntity>();
            UserEntity user = GetCurrentUser();

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
            EntityManager<ResourceEntity> manager = new EntityManager<ResourceEntity>();
            UserEntity user = GetCurrentUser();

            entity.IsEnable = (int)TrueFalse.True;
            entity.DeletionStateCode = (int)TrueFalse.True;
            entity.CreateUserId = user.ID.ToString();
            entity.CreateBy = user.UserName;
            entity.CreateOn = DateTime.Now;
            manager.Insert(entity);
        }

        public void AddOrganizeCategory(OrganizeCategoryEntity entity)
        {
            EntityManager<OrganizeCategoryEntity> manager = new EntityManager<OrganizeCategoryEntity>();
            UserEntity user = GetCurrentUser();

            entity.DeletionStateCode = (int)TrueFalse.True;
            entity.CreateUserId = user.ID.ToString();
            entity.CreateBy = user.UserName;
            entity.CreateOn = DateTime.Now;
            manager.Insert(entity);
        }

        public void AddOranize(OrganizeEntity entity)
        {
            EntityManager<OrganizeEntity> manager = new EntityManager<OrganizeEntity>();
            UserEntity user = GetCurrentUser();

            entity.DeletionStateCode = (int)TrueFalse.True;
            entity.CreateUserId = user.ID.ToString();
            entity.CreateBy = user.UserName;
            entity.CreateOn = DateTime.Now;
            manager.Insert(entity);
        }

        public void AddRole(RoleEntity entity)
        {
            EntityManager<RoleEntity> manager = new EntityManager<RoleEntity>();
            UserEntity user = GetCurrentUser();

            entity.DeletionStateCode = (int)TrueFalse.True;
            entity.CreateUserId = user.ID.ToString();
            entity.CreateBy = user.UserName;
            entity.CreateOn = DateTime.Now;
            manager.Insert(entity);
        }

        public void AddStaff(StaffEntity entity)
        {
            EntityManager<StaffEntity> manager = new EntityManager<StaffEntity>();
            UserEntity user = GetCurrentUser();

            entity.DeletionStateCode = (int)TrueFalse.True;
            entity.CreateUserId = user.ID.ToString();
            entity.CreateBy = user.UserName;
            entity.CreateOn = DateTime.Now;
            manager.Insert(entity);
        }

        public void GrantPermission(UserEntity user)
        {

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

        /// <summary>
        /// 分配角色
        /// </summary>
        public void GrantRole(UserEntity user, int userID, int[] roleIDs)
        {
            //1.0 清空该用户原有的角色权限
            EntityManager<UserRoleEntity> manager = new EntityManager<UserRoleEntity>();
            WhereStatement where = new WhereStatement();
            where.Add(UserRoleEntity.FieldUserID, Comparison.Equals, userID);
            manager.Delete(where);

            //2.0 添加当前选中的角色
            foreach (int id in roleIDs)
            {
                UserRoleEntity entity = new UserRoleEntity();
                entity.UserID = userID;
                entity.RoleID = id;
                entity.DeletionStateCode = (int)TrueFalse.True;
                entity.CreateUserId = user.ID.ToString();
                entity.CreateBy = user.UserName;
                entity.CreateOn = DateTime.Now;
                manager.Insert(entity);
            }
        }

        public void GetOrganize()
        {
            
        }

        public RoleEntity GetCurrentRole(UserEntity user)
        {
            //1.0 先从cookie查，有没有相应的role，若没有，选择第一个role
            // todo fix bug 其他用户登录会获取错误的roleid
            string roleID = JSRequest.GetCookieParm("RoleID").ToString();
            if (roleID == null)
            {
                ViewManager vmanager = new ViewManager("VP_UserRole");

                WhereStatement where = new WhereStatement();
                where.Add(UserRoleEntity.FieldUserID, Comparison.Equals, user.ID);

                int count = 0;
                DataTable dt = vmanager.GetDataTable(where, out count);

                if (dt.Rows.Count == 0)
                {
                    throw new JSException(JSErrMsg.ERR_CODE_NotGrantRole, JSErrMsg.ERR_MSG_NotGrantRole);
                }
                roleID = dt.Rows[0][UserRoleEntity.FieldRoleID].ToString();
            }

            //2.0 根据roleID，获取role对象
            EntityManager<RoleEntity> manager = new EntityManager<RoleEntity>();
            RoleEntity role = manager.GetSingle(roleID);
            if (role == null)
            {
                throw new JSException(JSErrMsg.ERR_CODE_DATA_MISSING, string.Format(JSErrMsg.ERR_MSG_DATA_MISSING,"该角色的"));
            }

            //3.0 将当前roleID 写进cookie
            CommonUtil.WriteCookie("RoleID", role.ID.ToString());
            return role;
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
                throw new JSException(JSErrMsg.ERR_CODE_NotGrantResource, JSErrMsg.ERR_MSG_NotGrantResource);
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
        /// 获取整个树形menu
        /// </summary>
        /// <param name="role"></param>
        /// <param name="systemCode"></param>
        /// <returns></returns>
        public DataTable GetMenu(RoleEntity role,string systemCode)
        {

        }

        /// <summary>
        /// 获取对应code下一层的button
        /// </summary>
        /// <param name="role"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public DataTable GetButton(RoleEntity role,string code)
        {

        }



        public void IsPermissionAuthorizedByRole(RoleEntity role,string controllerName,string actionName,string actionParm = null)
        {

        }


        public DataTable GetAllPermissions()
        {
            //先从缓存里面拿，如果没有再从数据库拿
        }
        
    }
}
