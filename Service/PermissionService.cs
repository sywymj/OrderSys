using CodeEngine.Framework.QueryBuilder;
using CodeEngine.Framework.QueryBuilder.Enums;
using JSNet.BaseSys;
using JSNet.DbUtilities;
using JSNet.Manager;
using JSNet.Model;
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

        public JSDictionary GetRoleDDL ()
        {
            int count = 0;
            EntityManager<RoleEntity> roleManager = new EntityManager<RoleEntity>();
            WhereStatement where = new WhereStatement();
            List<RoleEntity> list = roleManager.GetList(where, out count);

            //list.Select(l=>new JSDictionary(l.ID,l.Title));

            //JSDictionary dic = new JSDictionary();
            //dic.
        }



        //private int GetUserID(string openID)
        //{
            
        //    IDbHelper dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.KawuDbConnectionString, BaseSystemInfo.KawuDbType);
        //    ViewManager manager = new ViewManager(dbHelper, "[T_WXUser]");

        //    DataRow dr = manager.GetSingle(openID, "OpenId");
        //    string s = dr["11"].ToString();

        //}
    }
}
