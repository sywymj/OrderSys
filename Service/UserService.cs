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
using System.Web;

namespace JSNet.Service
{
    public class UserService : BaseService
    {

        #region Current
        public UserEntity GetCurrentUser()
        {
            string uid = SecretUtil.Decrypt(JSRequest.GetCookie("UID", true));
            UserEntity user = null;
            if(string.IsNullOrEmpty(uid))
            {
                //UID 空拿OPENID
               user =  GetCurrentVXUser();
            }
            else
            {
                EntityManager<UserEntity> manager = new EntityManager<UserEntity>();
                user = manager.GetSingle(Convert.ToInt32(uid));
            }

            if (user == null)
            {
                throw new HttpException(401, JSErrMsg.ERR_MSG_LoginOvertime);
            }

            return user;
        }

        public UserEntity GetCurrentVXUser()
        {
            string openID = JSRequest.GetCookie("OpenID", true);
            if (string.IsNullOrEmpty(openID))
            {
                throw new HttpException(401,JSErrMsg.ERR_MSG_LoginOvertime);
            }

            UserEntity user = GetUser(openID);
            if (user == null)
            {
                throw new HttpException(401,JSErrMsg.ERR_MSG_WrongOpenID);
            }

            return user;
        }

        public StaffEntity GetCurrentStaff()
        {
            EntityManager<StaffEntity> manager = new EntityManager<StaffEntity>();
            StaffEntity staff = manager.GetSingle(CurrentUser.ID, StaffEntity.FieldUserID);
            if (staff == null)
            {
                throw new Exception(string.Format(JSErrMsg.ERR_MSG_DATA_MISSING, "用户ID为" + CurrentUser.ID));
            }

            return staff;
        }

        #endregion

        #region User
        public void AddUser(UserEntity entity, StaffEntity staff, int[] roleIDs)
        {
            string errMessage = "";
            KawuService kawuService = new KawuService();
            bool b = kawuService.AddWeixinUser(staff.Tel, entity.UserName, out errMessage);
            if (!b)
            {
                throw new JSException(JSErrMsg.ERR_CODE_APIFailed, errMessage);
            }

            //添加user
            if (string.IsNullOrEmpty(entity.Password))
            {
                entity.Password = "123456";// TODO 加盐转MD5加密保存
            }
            entity.OpenID = null;
            entity.IsEnable = (int)TrueFalse.True;
            entity.OrganizeID = staff.OrganizeID;
            entity.DeletionStateCode = (int)TrueFalse.False;
            entity.CreateUserId = CurrentUser.ID.ToString();
            entity.CreateBy = CurrentUser.UserName;
            entity.CreateOn = DateTime.Now;
            EntityManager<UserEntity> userManager = new EntityManager<UserEntity>();
            string userID = userManager.Insert(entity);

            //添加staff
            staff.UserID = Convert.ToInt32(userID);
            string staffID = AddStaff(staff);
            
            //添加role-user-rel
            MyRoleService roleService = new MyRoleService();
            roleService.GrantRole(Convert.ToInt32(userID), roleIDs);
        }

        public void EditUser(UserEntity entity, StaffEntity staff, int[] roleIDs)
        {
            //1.0 修改用户信息
            EntityManager<UserEntity> userManager = new EntityManager<UserEntity>();
            List<KeyValuePair<string, object>> userTargetKVPs = new List<KeyValuePair<string, object>>();
            userTargetKVPs.Add(new KeyValuePair<string, object>(UserEntity.FieldUserName, entity.UserName));
            userTargetKVPs.Add(new KeyValuePair<string, object>(UserEntity.FieldPassword, entity.Password));
            userTargetKVPs.Add(new KeyValuePair<string, object>(UserEntity.FieldIsLogin, entity.IsLogin));
            userTargetKVPs.Add(new KeyValuePair<string, object>(UserEntity.FieldOrganizeID, staff.OrganizeID));
            userTargetKVPs.Add(new KeyValuePair<string, object>(UserEntity.FieldModifiedUserId, CurrentUser.ID.ToString()));
            userTargetKVPs.Add(new KeyValuePair<string, object>(UserEntity.FieldModifiedBy, CurrentUser.UserName));
            userTargetKVPs.Add(new KeyValuePair<string, object>(UserEntity.FieldModifiedOn, DateTime.Now));
            userManager.Update(userTargetKVPs, entity.ID);

            //2.0 修改员工信息
            EntityManager<StaffEntity> staffManager = new EntityManager<StaffEntity>();
            StaffEntity staff1 = staffManager.GetSingle(entity.ID, StaffEntity.FieldUserID);
            List<KeyValuePair<string, object>> staffTargetKVPs = new List<KeyValuePair<string, object>>();
            staffTargetKVPs.Add(new KeyValuePair<string, object>(StaffEntity.FieldName, staff.Name));
            staffTargetKVPs.Add(new KeyValuePair<string, object>(StaffEntity.FieldTel, staff.Tel));
            staffTargetKVPs.Add(new KeyValuePair<string, object>(StaffEntity.FieldAddr, staff.Addr));
            staffTargetKVPs.Add(new KeyValuePair<string, object>(StaffEntity.FieldSex, staff.Sex));
            staffTargetKVPs.Add(new KeyValuePair<string, object>(StaffEntity.FieldOrganizeID, staff.OrganizeID));
            staffTargetKVPs.Add(new KeyValuePair<string, object>(StaffEntity.FieldIsOnJob, staff.IsOnJob));
            staffTargetKVPs.Add(new KeyValuePair<string, object>(StaffEntity.FieldModifiedUserId, CurrentUser.ID.ToString()));
            staffTargetKVPs.Add(new KeyValuePair<string, object>(StaffEntity.FieldModifiedBy, CurrentUser.UserName));
            staffTargetKVPs.Add(new KeyValuePair<string, object>(StaffEntity.FieldModifiedOn, DateTime.Now));
            staffManager.Update(staffTargetKVPs, staff1.ID);

            //3.0 删除role-user-rel关系
            EntityManager<UserRoleEntity> userRoleManager = new EntityManager<UserRoleEntity>();
            WhereStatement where = new WhereStatement();
            where.Add(UserRoleEntity.FieldUserID, Comparison.Equals, entity.ID);
            userRoleManager.Delete(where);

            //3.1 增加role-user-rel关系
            foreach (int roleID in roleIDs)
            {
                UserRoleEntity userRole = new UserRoleEntity()
                {
                    RoleID = roleID,
                    UserID = entity.ID,
                    CreateUserId = CurrentUser.ID.ToString(),
                    CreateBy = CurrentUser.UserName,
                    CreateOn = DateTime.Now,
                };
                userRoleManager.Insert(userRole);
            }


        }

        public void EditUser(string tel,string openID)
        {
            EntityManager<StaffEntity> staffManager = new EntityManager<StaffEntity>();
            StaffEntity staff = staffManager.GetSingle(tel, StaffEntity.FieldTel);
            if (staff == null)
            {
                throw new JSException(JSErrMsg.ERR_CODE_WrongTel, JSErrMsg.ERR_MSG_WrongTel);
            }

            EntityManager<UserEntity> userManager = new EntityManager<UserEntity>();
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(UserEntity.FieldOpenID, openID));
            userManager.Update(kvps, staff.UserID);
        }
        public UserEntity GetUser(int userID)
        {
            EntityManager<UserEntity> manager = new EntityManager<UserEntity>();
            UserEntity entity = manager.GetSingle(userID);
            return entity;
        }

        public UserEntity GetUser(string openID)
        {
            EntityManager<UserEntity> manager = new EntityManager<UserEntity>();
            UserEntity entity = manager.GetSingle(openID, UserEntity.FieldOpenID);
            return entity;
        }

        public UserEntity GetUser(string userName, string pwd)
        {
            EntityManager<UserEntity> manager = new EntityManager<UserEntity>();
            WhereStatement where = new WhereStatement();
            where.Add(UserEntity.FieldUserName, Comparison.Equals, userName);
            where.Add(UserEntity.FieldPassword, Comparison.Equals, pwd);
            int count = 0;
            List<UserEntity> ls = manager.GetList(where, out count);
            return ls.FirstOrDefault();
        }

        public DataTable GetUserDTByRole(RoleEntity role, Paging paging, out int count)
        {
            PermissionService permissionService = new PermissionService();
            List<string> list = permissionService.GetAuthorizeOrganizeIDByRole(role, "OrderSys_Data.User");

            WhereStatement where = new WhereStatement();
            where.Add("Staff_IsEnable", Comparison.Equals, (int)TrueFalse.True);
            where.Add("Staff_IsOnJob", Comparison.Equals, (int)TrueFalse.True);
            if (list.Count > 0)
            {
                where.Add("Organize_ID", Comparison.In, list.ToArray());
            }
            else
            {
                if (role.ID == 1)
                {
                    //超级管理员，显示所有内容
                    where.Add("1", Comparison.Equals, "1");
                }
                else
                {
                    //非超级管理员，不显示内容
                    where.Add("1", Comparison.Equals, "0");
                }
            }

            OrderByStatement orderby = new OrderByStatement();
            orderby.Add(paging.SortField, ConvertToSort(paging.SortOrder));

            ViewManager vmanager = new ViewManager("VS_User_Show");
            DataTable dt = vmanager.GetDataTableByPage(where, out count, paging.PageIndex, paging.PageSize, orderby);
            return dt;
        }
        #endregion

        #region Staff

        public string AddStaff(StaffEntity entity)
        {
            entity.IsEnable = (int)TrueFalse.True;
            entity.DeletionStateCode = (int)TrueFalse.False;
            entity.CreateUserId = CurrentUser.ID.ToString();
            entity.CreateBy = CurrentUser.UserName;
            entity.CreateOn = DateTime.Now;

            EntityManager<StaffEntity> manager = new EntityManager<StaffEntity>();
            return manager.Insert(entity);
        }

        public StaffEntity GetStaff(int userID)
        {
            EntityManager<StaffEntity> manager = new EntityManager<StaffEntity>();
            StaffEntity entity = manager.GetSingle(userID, StaffEntity.FieldUserID);
            return entity;
        }

        public List<StaffEntity> GetStaffsByRole(RoleEntity role)
        {
            PermissionService permissionService = new PermissionService();
            List<string> organizeIDs = permissionService.GetAuthorizeOrganizeIDByRole(role,"OrderSys_Data.User");

            WhereStatement where = new WhereStatement();
            where.Add(StaffEntity.FieldIsEnable, Comparison.Equals, (int)TrueFalse.True);
            where.Add(StaffEntity.FieldIsOnJob, Comparison.Equals, (int)TrueFalse.True);
            if (organizeIDs.Count > 0)
            {
                where.Add(StaffEntity.FieldOrganizeID, Comparison.In, organizeIDs.ToArray());
            }
            else
            {
                if (role.ID == 1)
                {
                    //超级管理员，显示所有内容
                    where.Add("1", Comparison.Equals, "1");
                }
                else
                {
                    //默认不显示内容
                    where.Add("1", Comparison.Equals, "0");
                }
            }

            int count = 0;
            EntityManager<StaffEntity> manager = new EntityManager<StaffEntity>();
            List<StaffEntity> list = manager.GetList(where, out count);
            return list;
        }
        #endregion

        public bool ChkUserNameExist(string userName, string userID)
        {
            EntityManager<UserEntity> manager = new EntityManager<UserEntity>();

            WhereStatement where = new WhereStatement();
            where.Add(UserEntity.FieldUserName, Comparison.Equals, userName);
            if (!string.IsNullOrEmpty(userID))
            {
                //编辑时
                where.Add(UserEntity.FieldID, Comparison.NotEquals, userID);
            }

            bool b = manager.Exists(where);
            return b;
        }
    }
}
