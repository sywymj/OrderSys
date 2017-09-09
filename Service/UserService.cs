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
    public class UserService:BaseService
    {
        public void AddUser(UserEntity entity, int roleID)
        {
            entity.IsEnable = 1;
            entity.IsEnable = 1;
            entity.OpenID = null;

            EntityManager<UserEntity> userManager = new EntityManager<UserEntity>();
            string userID = userManager.Insert(entity);

            EntityManager<UserRoleEntity> roleUserManager = new EntityManager<UserRoleEntity>();
            roleUserManager.Insert(new UserRoleEntity()
            {
                RoleID = roleID,
                UserID = Convert.ToInt32(userID),
            });
        }

        public void EditUser(UserEntity entity)
        {

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
            kvps.Add(new KeyValuePair<string, object>(StaffEntity.FieldIsOnJob, 0));
            manamger.Update(kvps, id);
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
    }
}
