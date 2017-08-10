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
        public void AddStaff(StaffEntity entity,int roleID)
        {
            entity.IsOnJob = 1;
            entity.IsEnable = 1;
            entity.OpenID = null;

            EntityManager<StaffEntity> staffManager = new EntityManager<StaffEntity>();
            string userID = staffManager.Insert(entity);

            EntityManager<RoleUserEntity> roleUserManager = new EntityManager<RoleUserEntity>();
            roleUserManager.Insert(new RoleUserEntity()
            {
                RoleID = roleID,
                UserID = Convert.ToInt32(userID),
            });
        }  

        public void EditStaff(StaffEntity entity)
        {
            EntityManager<StaffEntity> manamger = new EntityManager<StaffEntity>();

            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(StaffEntity.FieldAddr, entity.Addr));
            kvps.Add(new KeyValuePair<string, object>(StaffEntity.FieldName, entity.Name));
            kvps.Add(new KeyValuePair<string, object>(StaffEntity.FieldSex, entity.Sex));
            
            manamger.Update(entity, entity.ID);
        }

        public void DeleteStaff(int id)
        {

            EntityManager<StaffEntity> manamger = new EntityManager<StaffEntity>();
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(StaffEntity.FieldIsOnJob, 0));
            manamger.Update(kvps, id);
        }

        public StaffEntity GetCurrentStaff()
        {
            
            //string openID = JSRequest.GetSessionParm("OpenID").ToString();

            string openID = "1";//调试用

            StaffEntity staff = new StaffEntity();
            EntityManager<StaffEntity> manager = new EntityManager<StaffEntity>();

            staff =manager.GetSingle(openID,StaffEntity.FieldOpenID);
            if (staff == null)
            {
                throw new JSException(JSErrMsg.ERR_CODE_OBJECT_MISSING, string.Format(JSErrMsg.ERR_MSG_OBJECT_MISSING, "员工"));
            }

            return staff;
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

        
    }
}
