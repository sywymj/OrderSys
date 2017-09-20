using CodeEngine.Framework.QueryBuilder;
using CodeEngine.Framework.QueryBuilder.Enums;
using JSNet.BaseSys;
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
    public class MyRoleService
    {

        public RoleEntity GetCurrentRole()
        {
            UserService userService = new UserService();
            MyRoleService roleService = new MyRoleService();

            UserEntity user = userService.GetCurrentUser();
            RoleEntity role = roleService.GetCurrentRole(user);
            return role;
        }

        public RoleEntity GetCurrentRole(UserEntity user)
        {
            //1.0 先从cookie查，有没有相应的role，若没有，选择第一个role
            string roleID = JSRequest.GetCookie("RoleID", true);

            if (string.IsNullOrEmpty(roleID))
            {
                ViewManager vmanager = new ViewManager("[VP_UserRole]");

                WhereStatement where = new WhereStatement();
                where.Add("User_ID", Comparison.Equals, user.ID);

                int count = 0;
                DataTable dt = vmanager.GetDataTable(where, out count);

                if (dt.Rows.Count == 0)
                {
                    throw new JSException(JSErrMsg.ERR_CODE_NotGrantRole, JSErrMsg.ERR_MSG_NotGrantRole);
                }
                roleID = dt.Rows[0]["Role_ID"].ToString();

            }

            //2.0 根据roleID，获取role对象
            EntityManager<RoleEntity> manager = new EntityManager<RoleEntity>();
            RoleEntity role = manager.GetSingle(roleID);

            //3.0 将当前roleID 写进cookie
            JSResponse.WriteCookie("RoleID", role.ID.ToString());
            return role;
        }

        public int[] GetRoleIDs(int userID)
        {
            EntityManager<UserRoleEntity> manager = new EntityManager<UserRoleEntity>();
            WhereStatement where = new WhereStatement();
            where.Add(UserRoleEntity.FieldUserID,Comparison.Equals,userID);
            string[] sRoleIDs = manager.GetProperties(UserRoleEntity.FieldRoleID, where);

            int[] roleIDs = CommonUtil.ConvertToIntArry(sRoleIDs);
            return roleIDs;
        }

        public List<RoleEntity> GetRoleList()
        {
            EntityManager<RoleEntity> manager = new EntityManager<RoleEntity>();

            WhereStatement where = new WhereStatement();

            int count = 0;
            List<RoleEntity> roles = manager.GetList(where, out count);
            return roles;
        }

        public void AddRole(RoleEntity entity)
        {
            UserService userService = new UserService();
            UserEntity currentUser = userService.GetCurrentUser();

            EntityManager<RoleEntity> manager = new EntityManager<RoleEntity>();
            entity.CreateUserId = currentUser.ID.ToString();
            entity.CreateBy = currentUser.UserName;
            entity.CreateOn = DateTime.Now;
            manager.Insert(entity);
        }

        public void EditRole(RoleEntity entity)
        {
            UserService userService = new UserService();
            UserEntity currentUser = userService.GetCurrentUser();

            EntityManager<RoleEntity> manager = new EntityManager<RoleEntity>();
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(RoleEntity.FieldFullName, entity.FullName));
            kvps.Add(new KeyValuePair<string, object>(RoleEntity.FieldDescription, entity.Description));
            kvps.Add(new KeyValuePair<string, object>(RoleEntity.FieldModifiedUserId, currentUser.ID.ToString()));
            kvps.Add(new KeyValuePair<string, object>(RoleEntity.FieldModifiedBy, currentUser.UserName));
            kvps.Add(new KeyValuePair<string, object>(RoleEntity.FieldModifiedOn, DateTime.Now));
            manager.Update(kvps, entity.ID);
        }

        public RoleEntity GetRole(int roleID)
        {
            EntityManager<RoleEntity> manager = new EntityManager<RoleEntity>();
            RoleEntity entity = manager.GetSingle(roleID);
            return entity;
        }

        public List<RoleEntity> GetRoleList(Paging paging, out int count)
        {
            EntityManager<RoleEntity> manager = new EntityManager<RoleEntity>();
            WhereStatement where = new WhereStatement();

            List<RoleEntity> list = manager.GetListByPage(where, out count, paging.PageIndex, paging.PageSize);
            return list;
        }

        public void GrantRoleModule(int roleID, int[] moduleIDs)
        {
            //判断资源类型是否为Menu、button，只有Menu、button才能分配资源明细
            EntityManager<RoleEntity> roleManager = new EntityManager<RoleEntity>();
            RoleEntity role = roleManager.GetSingle(roleID);


            UserService userService = new UserService();
            UserEntity currentUser = userService.GetCurrentUser();

            EntityManager<RoleResourceEntity> manager = new EntityManager<RoleResourceEntity>();
            manager.Delete(roleID, RoleResourceEntity.FieldRoleID);

            List<RoleResourceEntity> entitys = new List<RoleResourceEntity>();
            foreach (int moduleID in moduleIDs)
            {
                RoleResourceEntity entity = new RoleResourceEntity();
                entity.RoleID = roleID;
                entity.ResourceID = moduleID;
                entity.CreateUserId = currentUser.ID.ToString();
                entity.CreateBy = currentUser.UserName;
                entity.CreateOn = DateTime.Now;
                entitys.Add(entity);
            }
            manager.Insert(entitys);
        }

        public int[] GetGrantedModuleIDs(int roleID)
        {
            EntityManager<RoleResourceEntity> manager = new EntityManager<RoleResourceEntity>();
            WhereStatement where = new WhereStatement();
            where.Add(RoleResourceEntity.FieldRoleID, Comparison.Equals, roleID);

            string[] sIDs = manager.GetProperties(RoleResourceEntity.FieldResourceID, where);
            int[] ids = CommonUtil.ConvertToIntArry(sIDs);
            return ids;
        }

        public void GrantRoleScope(int roleID, int[] scopeIDs)
        {
            //判断资源类型是否为Menu、button，只有Menu、button才能分配资源明细
            EntityManager<RoleEntity> roleManager = new EntityManager<RoleEntity>();
            RoleEntity role = roleManager.GetSingle(roleID);


            UserService userService = new UserService();
            UserEntity currentUser = userService.GetCurrentUser();

            EntityManager<RolePermissionScopeEntity> manager = new EntityManager<RolePermissionScopeEntity>();
            manager.Delete(roleID, RolePermissionScopeEntity.FieldRoleID);

            List<RolePermissionScopeEntity> entitys = new List<RolePermissionScopeEntity>();
            foreach (int scopeID in scopeIDs)
            {
                RolePermissionScopeEntity entity = new RolePermissionScopeEntity();
                entity.RoleID = roleID;
                entity.PermissionScopeID = scopeID;
                entity.CreateUserId = currentUser.ID.ToString();
                entity.CreateBy = currentUser.UserName;
                entity.CreateOn = DateTime.Now;
                entitys.Add(entity);
            }
            manager.Insert(entitys);
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
                entity.CreateUserId = user.ID.ToString();
                entity.CreateBy = user.UserName;
                entity.CreateOn = DateTime.Now;
                manager.Insert(entity);
            }
        }

        public int[] GetGrantedRoleScopeIDs(int roleID)
        {
            EntityManager<RolePermissionScopeEntity> manager = new EntityManager<RolePermissionScopeEntity>();
            WhereStatement where = new WhereStatement();
            where.Add(RolePermissionScopeEntity.FieldRoleID, Comparison.Equals, roleID);

            string[] sIDs = manager.GetProperties(RolePermissionScopeEntity.FieldPermissionScopeID, where);
            int[] ids = CommonUtil.ConvertToIntArry(sIDs);
            return ids;
        }

    }
}
