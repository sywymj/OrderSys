using CodeEngine.Framework.QueryBuilder;
using CodeEngine.Framework.QueryBuilder.Enums;
using JSNet.BaseSys;
using JSNet.Manager;
using JSNet.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace JSNet.Service
{
    public class MyRoleService
    {
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
            // todo fix bug 其他用户登录会获取错误的roleid
            string roleID = JSRequest.GetCookie("RoleID", true);
            if (string.IsNullOrEmpty(roleID))
            {
                ViewManager vmanager = new ViewManager("[VP_UserRole]");

                WhereStatement where = new WhereStatement();
                where.Add("UserID", Comparison.Equals, user.ID);

                //OrderByStatement order = new OrderByStatement();
                //order.Add("UserID", Sorting.Ascending);

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
                throw new JSException(JSErrMsg.ERR_CODE_DATA_MISSING, string.Format(JSErrMsg.ERR_MSG_DATA_MISSING, "该角色的"));
            }

            //3.0 将当前roleID 写进cookie
            JSResponse.WriteCookie("RoleID", role.ID.ToString());
            return role;
        }

        public List<RoleEntity> GetRoleList()
        {
            EntityManager<RoleEntity> manager = new EntityManager<RoleEntity>();

            WhereStatement where = new WhereStatement();

            int count = 0;
            List<RoleEntity> roles = manager.GetList(where, out count);
            return roles;
        }
    }
}
