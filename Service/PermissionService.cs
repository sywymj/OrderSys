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

            JSDictionary re = list.ToJSDictionary(Key => Key.ID, Value => Value.Title);
            return re;
        }

        //发起报障单
        public void StartOrder()
        {

        }

        //委派工作
        public void AppointOrder()
        {

        }

        //接收报障单
        public void ReceiveOrder()
        {

        }

        //增加处理明细
        public void AddHandleDetail()
        {

        }

        //报障处理完毕
        public void CompleteOrder()
        {

        }

        //驳回报障，需继续处理
        public void RejectOrder()
        {
        
        }

        //报障验收完成
        public void FinishOrder()
        {
            
        }

        //取消报障单
        public void CancelOrder()
        {

        }

        public List<OrderEntity> GetMyStartedOrders()
        {
            List<OrderEntity> list = new List<OrderEntity>();
            return list;
        }

        public List<OrderEntity> GetMyRecevingOrders()
        {
            List<OrderEntity> list = new List<OrderEntity>();
            return list;
        }

        public List<OrderEntity> GetMyHandlingOrders()
        {
            List<OrderEntity> list = new List<OrderEntity>();
            return list;
        }


    }
}
