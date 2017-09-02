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
                throw new JSException(JSErrMsg.ERR_CODE_DATA_MISSING, string.Format(JSErrMsg.ERR_MSG_DATA_MISSING, "员工"));
            }

            return user;
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

        }

        public void AddStaff(StaffEntity staff)
        {

        }

        public void GrantPermission()
        {

        }

        public void GrantPermissionScope()
        {

        }

        public void GrantRole()
        {

        }
        
    }
}
