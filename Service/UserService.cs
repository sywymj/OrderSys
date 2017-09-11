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
            entity.ModifiedUserId = currentUser.ID.ToString();
            entity.ModifiedBy = currentUser.UserName;
            entity.ModifiedOn = DateTime.Now;
            userManager.Update(entity, entity.ID);

            //2.0 修改员工信息
            EntityManager<StaffEntity> staffManager = new EntityManager<StaffEntity>();
            StaffEntity staff1 = staffManager.GetSingle(entity.ID, StaffEntity.FieldUserID);
            staff.ModifiedUserId = currentUser.ID.ToString();
            staff.ModifiedBy = currentUser.UserName;
            staff.ModifiedOn = DateTime.Now;
            staffManager.Update(staff, staff1.ID);

            //3.0 删除role-user-rel关系
            EntityManager<UserRoleEntity> roleUserManager = new EntityManager<UserRoleEntity>();
            WhereStatement where = new WhereStatement();
            where.Add(UserRoleEntity.FieldUserID,Comparison.Equals,entity.ID);
            roleUserManager.Delete(where);

            //3.1 增加role-user-rel关系
            foreach (int roleID in roleIDs)
            {
                roleUserManager.Insert(new UserRoleEntity()
                {
                    RoleID = roleID,
                    UserID = entity.ID,
                    CreateUserId = currentUser.ID.ToString(),
                    CreateBy = currentUser.UserName,
                    CreateOn = DateTime.Now,
                });
            }


        }

        public UserEntity GetUser(int userID)
        {
            EntityManager<UserEntity> manager = new EntityManager<UserEntity>();
            UserEntity entity = manager.GetSingle(userID);
            return entity;
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

        public DataTable GetAllStaffs(Paging paging, out int count)
        {
            ViewManager vmanager = new ViewManager("VS_Staff_Show");

            WhereStatement where = new WhereStatement();
            where.Add("Staff_IsEnable", Comparison.Equals, (int)TrueFalse.True);
            where.Add("Staff_IsOnJob", Comparison.Equals, (int)TrueFalse.True);

            OrderByStatement orderby = new OrderByStatement();
            orderby.Add(paging.SortField, ConvertToSort(paging.SortOrder));

            DataTable dt = vmanager.GetDataTableByPage(where, out count, paging.PageIndex, paging.PageSize, orderby);
            return dt;
        }

        public bool ChkUserNameExist(string userName)
        {
            EntityManager<UserEntity> manager = new EntityManager<UserEntity>();

            WhereStatement where = new WhereStatement();
            where.Add(UserEntity.FieldUserName, Comparison.Equals, userName);

            bool b = manager.Exists(where);
            return b;
        }
    }
}
