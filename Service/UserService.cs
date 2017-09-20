﻿using CodeEngine.Framework.QueryBuilder;
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
    public class UserService:BaseService
    {

        public UserEntity GetCurrentUser()
        {
            if (string.IsNullOrEmpty(JSRequest.GetCookie("AdminName", true))
               || string.IsNullOrEmpty(JSRequest.GetCookie("AdminPwd", true)))
            {
                throw new JSException(JSErrMsg.ERR_MSG_LoginOvertime);
            }

            string userName = JSRequest.GetCookie("AdminName");
            string password = JSRequest.GetCookie("AdminPwd");

            UserEntity user = GetUser(userName, password);
            if (user == null)
            {
                throw new JSException(JSErrMsg.ERR_MSG_WrongPwd);
            }
            if (user.IsEnable == (int)TrueFalse.False)
            {
                throw new JSException(JSErrMsg.ERR_MSG_UserUnable);
            }
            if (user.IsLogin == (int)TrueFalse.False)
            {
                throw new JSException(JSErrMsg.ERR_CODE_NotAllowLogin);
            }

            //写入cookie
            JSResponse.WriteCookie("AdminName", user.UserName, 60);
            JSResponse.WriteCookie("AdminPwd", user.Password, 60);
            return user;
        }

        public UserEntity GetCurrentUserByOpenID()
        {
            if (string.IsNullOrEmpty(JSRequest.GetCookie("OpenID", true)))
            {
                return null;
            }

            string openID = JSRequest.GetCookie("OpenID");
            UserEntity user = GetUser(openID);
            if (user == null)
            {
                throw new JSException(JSErrMsg.ERR_MSG_WrongOpenID);
            }
            if (user.IsEnable == (int)TrueFalse.False)
            {
                throw new JSException(JSErrMsg.ERR_MSG_UserUnable);
            }
            if (user.IsLogin == (int)TrueFalse.False)
            {
                throw new JSException(JSErrMsg.ERR_CODE_NotAllowLogin);
            }

            //写入cookie
            JSResponse.WriteCookie("OpenID", user.OpenID, 60);
            return user;
        }

        public void AddUser(UserEntity entity,StaffEntity staff, int[] roleIDs)
        {
            UserEntity currentUser = GetCurrentUser();

            //添加user
            if (string.IsNullOrEmpty(entity.Password))
            {
                entity.Password = "123456";// TODO 加盐转MD5加密保存
            }
            entity.OpenID = null;
            entity.IsEnable = (int)TrueFalse.True;
            entity.DeletionStateCode = (int)TrueFalse.False;
            entity.CreateUserId = currentUser.ID.ToString();
            entity.CreateBy = currentUser.UserName;
            entity.CreateOn = DateTime.Now;
            EntityManager<UserEntity> userManager = new EntityManager<UserEntity>();
            string userID = userManager.Insert(entity);

            //添加staff
            staff.UserID = Convert.ToInt32(userID);
            staff.IsEnable = (int)TrueFalse.True; ;
            staff.DeletionStateCode = (int)TrueFalse.False;
            staff.CreateUserId = currentUser.ID.ToString();
            staff.CreateBy = currentUser.UserName;
            staff.CreateOn = DateTime.Now;
            EntityManager<StaffEntity> staffManager = new EntityManager<StaffEntity>();
            string staffID = staffManager.Insert(staff);

            //添加role-user-rel
            EntityManager<UserRoleEntity> roleUserManager = new EntityManager<UserRoleEntity>();
            foreach (int roleID in roleIDs)
            {
                roleUserManager.Insert(new UserRoleEntity()
                {
                    RoleID = roleID,
                    UserID = Convert.ToInt32(userID),
                    CreateUserId = currentUser.ID.ToString(),
                    CreateBy = currentUser.UserName,
                    CreateOn = DateTime.Now,
                });
            }
        }

        public void EditUser(UserEntity entity, StaffEntity staff, int[] roleIDs)
        {
            UserEntity currentUser = GetCurrentUser();

            //1.0 修改用户信息
            EntityManager<UserEntity> userManager = new EntityManager<UserEntity>();
            List<KeyValuePair<string, object>> userTargetKVPs = new List<KeyValuePair<string, object>>();
            userTargetKVPs.Add(new KeyValuePair<string, object>(UserEntity.FieldUserName, entity.UserName));
            userTargetKVPs.Add(new KeyValuePair<string, object>(UserEntity.FieldPassword, entity.Password));
            userTargetKVPs.Add(new KeyValuePair<string, object>(UserEntity.FieldIsLogin, entity.IsLogin));
            userTargetKVPs.Add(new KeyValuePair<string, object>(UserEntity.FieldModifiedUserId, currentUser.ID.ToString()));
            userTargetKVPs.Add(new KeyValuePair<string, object>(UserEntity.FieldModifiedBy, currentUser.UserName));
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
            staffTargetKVPs.Add(new KeyValuePair<string, object>(StaffEntity.FieldModifiedUserId, currentUser.ID.ToString()));
            staffTargetKVPs.Add(new KeyValuePair<string, object>(StaffEntity.FieldModifiedBy, currentUser.UserName));
            staffTargetKVPs.Add(new KeyValuePair<string, object>(StaffEntity.FieldModifiedOn, DateTime.Now));
            staffManager.Update(staffTargetKVPs, staff1.ID);

            //3.0 删除role-user-rel关系
            EntityManager<UserRoleEntity> userRoleManager = new EntityManager<UserRoleEntity>();
            WhereStatement where = new WhereStatement();
            where.Add(UserRoleEntity.FieldUserID,Comparison.Equals,entity.ID);
            userRoleManager.Delete(where);

            //3.1 增加role-user-rel关系
            foreach (int roleID in roleIDs)
            {
                UserRoleEntity userRole = new UserRoleEntity()
                {
                    RoleID = roleID,
                    UserID = entity.ID,
                    CreateUserId = currentUser.ID.ToString(),
                    CreateBy = currentUser.UserName,
                    CreateOn = DateTime.Now,
                };
                userRoleManager.Insert(userRole);
            }


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
            where.Add(UserEntity.FieldUserName,Comparison.Equals,userName);
            where.Add(UserEntity.FieldPassword,Comparison.Equals,pwd);
            int count = 0;
            List<UserEntity> ls = manager.GetList(where, out count);
            return ls.FirstOrDefault();
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

        public void DeleteStaff(int id)
        {
            EntityManager<UserEntity> manamger = new EntityManager<UserEntity>();
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(StaffEntity.FieldDeletionStateCode, (int)TrueFalse.True));
            manamger.Update(kvps, id);
        }

        public StaffEntity GetStaff(int userID)
        {
            EntityManager<StaffEntity> manager = new EntityManager<StaffEntity>();
            StaffEntity entity = manager.GetSingle(userID, StaffEntity.FieldUserID);
            return entity;
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
            where.Add(StaffEntity.FieldIsEnable, Comparison.Equals, (int)TrueFalse.True);
            where.Add(StaffEntity.FieldIsOnJob, Comparison.Equals, (int)TrueFalse.True);

            int count = 0;
            List<StaffEntity> list = manager.GetList(where, out count);
            return list;
        }

        public DataTable GetAllUser(Paging paging, out int count)
        {
            ViewManager vmanager = new ViewManager("VS_User_Show");

            WhereStatement where = new WhereStatement();
            where.Add("Staff_IsEnable", Comparison.Equals, (int)TrueFalse.True);
            where.Add("Staff_IsOnJob", Comparison.Equals, (int)TrueFalse.True);

            OrderByStatement orderby = new OrderByStatement();
            orderby.Add(paging.SortField, ConvertToSort(paging.SortOrder));

            DataTable dt = vmanager.GetDataTableByPage(where, out count, paging.PageIndex, paging.PageSize, orderby);
            return dt;
        }

        public bool ChkUserNameExist(string userName,string userID)
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
