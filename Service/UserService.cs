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
            int uid = 0;
            if (!Int32.TryParse(SecretUtil.Decrypt(JSRequest.GetCookie("UID", true)), out uid))
            {
                return null;
            }

            EntityManager<UserEntity> manager = new EntityManager<UserEntity>();
            UserEntity user = manager.GetSingle(uid);
            return user;
        }

        public UserEntity GetCurrentVXUser()
        {
            string openID = JSRequest.GetCookie("OpenID", true);
            if (string.IsNullOrEmpty(openID))
            {
                return null;
            }

            UserEntity user = GetUser(openID);
            return user;
        }

        public StaffEntity GetCurrentStaff()
        {
            EntityManager<StaffEntity> manager = new EntityManager<StaffEntity>();
            StaffEntity staff = manager.GetSingle(CurrentUser.ID, StaffEntity.FieldUserID);
            return staff;
        }

        #endregion

        #region User
        public void AddUser(UserEntity entity, StaffEntity staff, int[] roleIDs)
        {
            KawuService kawuService = new KawuService();
            kawuService.AddWeixinUser(staff.Tel, entity.UserName);

            //添加user
            if (string.IsNullOrEmpty(entity.Password))
            {
                entity.Password = "123456";// TODO 加盐转MD5加密保存
            }
            entity.OpenID = null;
            entity.AddedVXUser = (int)TrueFalse.True;
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

        public void AddWeiXinUser(int[] userIDs)
        {
            List<UserEntity> users = GetUserList(userIDs);

            KawuService kawuService = new KawuService();
            foreach(UserEntity user in users)
            {
                if (user.AddedVXUser == (int)TrueFalse.True) {
                    throw new JSException(JSErrMsg.ERR_CODE_AddedVXUser, JSErrMsg.ERR_MSG_AddedVXUser);
                }

                //增加微信用户
                StaffEntity staff = GetStaffByUserID((int)user.ID);
                kawuService.AddWeixinUser(staff.Tel, user.UserName);

                //修改用户状态为 已增加微信用户
                user.AddedVXUser = (int)TrueFalse.True;
                EditUserVXUserStatus(user);
            }
        }

        public void EditUser(UserEntity entity, StaffEntity staff, int[] roleIDs)
        {
            //0.0 修改卡务对应的手机号码
            StaffEntity originalStaff = GetStaffByUserID((int)entity.ID);
            KawuService kawuService = new KawuService();
            kawuService.ChangeUserData(originalStaff.Tel, staff.Tel, entity.UserName);

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

        public void EditUserOpenID(string tel,string openID)
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

        public void EditUserVXUserStatus(UserEntity user)
        {
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(UserEntity.FieldAddedVXUser, user.AddedVXUser));

            EntityManager<UserEntity> userManager = new EntityManager<UserEntity>();
            userManager.Update(kvps, user.ID);
        }

        public void ClearUserOpenID(string openID)
        {
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(UserEntity.FieldOpenID, ""));

            EntityManager<UserEntity> userManager = new EntityManager<UserEntity>();
            userManager.Update(kvps, openID, UserEntity.FieldOpenID);
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
            int count = 0;

            WhereStatement where = new WhereStatement();
            where.Add(UserEntity.FieldUserName, Comparison.Equals, userName);
            where.Add(UserEntity.FieldPassword, Comparison.Equals, pwd);
            
            EntityManager<UserEntity> manager = new EntityManager<UserEntity>();
            List<UserEntity> ls = manager.GetList(where, out count);
            return ls.FirstOrDefault();
        }

        public List<UserEntity> GetUserList(int[] userIDs)
        {
            int count = 0;
            WhereStatement where = new WhereStatement();
            where.Add(UserEntity.FieldID,Comparison.In,userIDs);

            EntityManager<UserEntity> manager = new EntityManager<UserEntity>();
            List<UserEntity> list = manager.GetList(where, out count);
            return list;
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

        public StaffEntity GetStaffByUserID(int userID)
        {
            EntityManager<StaffEntity> manager = new EntityManager<StaffEntity>();
            StaffEntity entity = manager.GetSingle(userID, StaffEntity.FieldUserID);
            return entity;
        }

        public StaffEntity GetStaff(int staffID)
        {
            EntityManager<StaffEntity> manager = new EntityManager<StaffEntity>();
            StaffEntity entity = manager.GetSingle(staffID, StaffEntity.FieldID);
            return entity;
        }

        public List<StaffEntity> GetWorkingStaffsByRole(RoleEntity role)
        {
            PermissionService permissionService = new PermissionService();
            List<string> organizeIDs = permissionService.GetAuthorizeOrganizeIDByRole(role, "OrderSys_Data.WorkingStaff");

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
