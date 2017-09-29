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
    public class MyRoleService:BaseService
    {
        public RoleEntity GetCurrentRole()
        {
            string rid = SecretUtil.Decrypt(JSRequest.GetCookie("RID", true));
            if (string.IsNullOrEmpty(rid))
            {
                throw new JSException(JSErrMsg.ERR_MSG_LoginOvertime);
            }

            RoleEntity role = GetRole(Convert.ToInt32(rid));
            if (role == null)
            {
                throw new JSException(JSErrMsg.ERR_MSG_LoginOvertime);
            }
            return role;
        }

        public List<RoleEntity> GetRoleListByRole(RoleEntity role)
        {
            PermissionService permissionService = new PermissionService();
            List<string> organizeIDs = permissionService.GetAuthorizeOrganizeIDByRole(role, "OrderSys_Data.Role");

            WhereStatement where = new WhereStatement();
            if (organizeIDs.Count > 0)
            {
                where.Add(RoleEntity.FieldOrganizeID, Comparison.In, organizeIDs.ToArray());
            }

            int count = 0;
            EntityManager<RoleEntity> manager = new EntityManager<RoleEntity>();
            List<RoleEntity> roles = manager.GetList(where, out count);
            return roles;
        }

        public void AddRole(RoleEntity entity)
        {
            //获取系统名称
            OrganizeEntity organize = new OrganizeService().GetOrganize((int)entity.OrganizeID);
            entity.SysCategory = organize.Code.Split('.')[0];

            entity.CreateUserId = CurrentUser.ID.ToString();
            entity.CreateBy = CurrentUser.UserName;
            entity.CreateOn = DateTime.Now;

            EntityManager<RoleEntity> manager = new EntityManager<RoleEntity>();
            manager.Insert(entity);
        }

        public void EditRole(RoleEntity entity)
        {
            //获取系统名称
            OrganizeEntity organize = new OrganizeService().GetOrganize((int)entity.OrganizeID);
            entity.SysCategory = organize.Code.Split('.')[0];

            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(RoleEntity.FieldFullName, entity.FullName));
            kvps.Add(new KeyValuePair<string, object>(RoleEntity.FieldOrganizeID, entity.OrganizeID));
            kvps.Add(new KeyValuePair<string, object>(RoleEntity.FieldSysCategory,entity.SysCategory));
            kvps.Add(new KeyValuePair<string, object>(RoleEntity.FieldDescription, entity.Description));
            kvps.Add(new KeyValuePair<string, object>(RoleEntity.FieldModifiedUserId, CurrentUser.ID.ToString()));
            kvps.Add(new KeyValuePair<string, object>(RoleEntity.FieldModifiedBy, CurrentUser.UserName));
            kvps.Add(new KeyValuePair<string, object>(RoleEntity.FieldModifiedOn, DateTime.Now));

            EntityManager<RoleEntity> manager = new EntityManager<RoleEntity>();
            manager.Update(kvps, entity.ID);
        }

        public RoleEntity GetRole(int roleID)
        {
            EntityManager<RoleEntity> manager = new EntityManager<RoleEntity>();
            RoleEntity entity = manager.GetSingle(roleID);
            return entity;
        }

        public List<RoleEntity> GetRoleListByRole(RoleEntity role ,Paging paging,out int count)
        {
            PermissionService permissionService = new PermissionService();
            List<string> organizeIDs = permissionService.GetAuthorizeOrganizeIDByRole(role, "OrderSys_Data.Role");

            WhereStatement where = new WhereStatement();
            if (organizeIDs.Count > 0)
            {
                where.Add(RoleEntity.FieldOrganizeID, Comparison.In, organizeIDs.ToArray());//kvp.Value.ToArray();
            }

            EntityManager<RoleEntity> manager = new EntityManager<RoleEntity>();
            List<RoleEntity> list = manager.GetListByPage(where, out count, paging.PageIndex, paging.PageSize);
            return list;
        }

        #region Grant_Role
        public void GrantRole(int userID, int[] roleIDs)
        {
            //1.0 清空该用户原有的角色权限
            EntityManager<UserRoleEntity> manager = new EntityManager<UserRoleEntity>();
            WhereStatement where = new WhereStatement();
            where.Add(UserRoleEntity.FieldUserID, Comparison.Equals, userID);
            manager.Delete(where);

            //2.0 添加当前选中的角色
            foreach (int roleID in roleIDs)
            {
                UserRoleEntity entity = new UserRoleEntity();
                entity.UserID = userID;
                entity.RoleID = roleID;
                entity.CreateUserId = CurrentUser.ID.ToString();
                entity.CreateBy = CurrentUser.UserName;
                entity.CreateOn = DateTime.Now;
                manager.Insert(entity);
            }
        }

        public RoleEntity GetGrantedRole(UserEntity user)
        {
            int[] roleIDs = GetGrantedRoleIDs((int)user.ID);
            if (roleIDs.Length == 0)
            {
                return null;
            }

            RoleEntity role = GetRole(roleIDs[0]);
            return role;
        }

        public int[] GetGrantedRoleIDs(int userID)
        {
            WhereStatement where = new WhereStatement();
            where.Add(UserRoleEntity.FieldUserID, Comparison.Equals, userID);

            EntityManager<UserRoleEntity> manager = new EntityManager<UserRoleEntity>();
            string[] sRoleIDs = manager.GetProperties(UserRoleEntity.FieldRoleID, where);

            int[] roleIDs = CommonUtil.ConvertToIntArry(sRoleIDs);
            return roleIDs;
        } 
        #endregion

    }
}
