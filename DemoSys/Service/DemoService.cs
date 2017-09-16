using CodeEngine.Framework.QueryBuilder;
using CodeEngine.Framework.QueryBuilder.Enums;
using JSNet.BaseSys;
using JSNet.DbUtilities;
using JSNet.Manager;
using JSNet.Model;
using JSNet.Service;
using JSNet.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DemoSys.Service
{
    public class DemoService:BaseService
    {
        public void AddDemo(DemoEntity entity)
        {
            UserService userService = new UserService();
            UserEntity currentUser = userService.GetCurrentUser();

            EntityManager<DemoEntity> manager = new EntityManager<DemoEntity>();
            entity.CreateUserId = currentUser.ID.ToString();
            entity.CreateBy = currentUser.UserName;
            entity.CreateOn = DateTime.Now;
            manager.Insert(entity);
        }

        public void EditDemo(DemoEntity entity)
        {
            UserService userService = new UserService();
            UserEntity currentUser = userService.GetCurrentUser();

            EntityManager<DemoEntity> manager = new EntityManager<DemoEntity>();
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(DemoEntity.FieldFullName, entity.FullName));
            kvps.Add(new KeyValuePair<string, object>(DemoEntity.FieldDescription, entity.Description));
            kvps.Add(new KeyValuePair<string, object>(DemoEntity.FieldModifiedUserId, currentUser.ID.ToString()));
            kvps.Add(new KeyValuePair<string, object>(DemoEntity.FieldModifiedBy, currentUser.UserName));
            kvps.Add(new KeyValuePair<string, object>(DemoEntity.FieldModifiedOn, DateTime.Now));
            manager.Update(kvps, entity.ID);
        }

        public void DelDemo(int demoID)
        {
            //TODO
        }

        public DemoEntity GetDemo(int demoID)
        {
            EntityManager<DemoEntity> manager = new EntityManager<DemoEntity>();
            DemoEntity entity = manager.GetSingle(demoID);
            return entity;
        }

        public DataRow GetDemoDR(int demoID)
        {
            ViewManager vmanager = new ViewManager("V_Demo_Show");
            DataRow dr = vmanager.GetSingle(demoID, DemoEntity.FieldID);
            return dr;
        }

        public DataTable GetDemoDT(out int count)
        {
            ViewManager vmanager = new ViewManager("V_Demo_Show");
            WhereStatement where = new WhereStatement();

            DataTable dt = vmanager.GetDataTable(where, out count);
            return dt;
        }

        public DataTable GetDemoDT(Paging paging, out int count)
        {
            ViewManager vmanager = new ViewManager("V_Demo_Show");
            WhereStatement where = new WhereStatement();

            OrderByStatement orderby = new OrderByStatement();
            orderby.Add(paging.SortField, ConvertToSort(paging.SortOrder));

            DataTable dt = vmanager.GetDataTableByPage(where, out count, paging.PageIndex, paging.PageSize, orderby);
            return dt;
        }

        public List<DemoEntity> GetDemoList()
        {
            int count = 0;
            EntityManager<DemoEntity> manager = new EntityManager<DemoEntity>();
            WhereStatement where = new WhereStatement();

            List<DemoEntity> list = manager.GetList(where, out count);
            return list;
        }

        public List<DemoEntity> GetDemoList(Paging paging, out int count)
        {
            EntityManager<DemoEntity> manager = new EntityManager<DemoEntity>();
            WhereStatement where = new WhereStatement();

            List<DemoEntity> list = manager.GetListByPage(where, out count, paging.PageIndex, paging.PageSize);
            return list;
        }

        public List<DemoEntity> GetTreeDemoList(string demoCode, bool onlyChild = true)
        {
            EntityManager<DemoEntity> manager = new EntityManager<DemoEntity>();

            string[] ids = GetTreeDemoIDs(demoCode);
            if (ids.Length == 0)
            {
                throw new JSException(JSErrMsg.ERR_CODE_DATA_MISSING, string.Format(JSErrMsg.ERR_MSG_DATA_MISSING, "demoCode"));
            }

            WhereStatement where = new WhereStatement();
            where.Add(DemoEntity.FieldID, Comparison.In, ids);

            int count = 0;
            List<DemoEntity> list = manager.GetList(where, out count);

            if (onlyChild)
            {
                list.Where(l => l.Code != demoCode);
            }

            return list;
        }

        public string[] GetTreeDemoIDs(string demoCode)
        {
            string[] s = GetTreeIDs(
                "[V_Demo]",
                "Demo_Code", demoCode,
                "Demo_ID", "Demo_ParentID");
            return s;
        }

        public bool ChkDemoCodeExist(string demoCode, string demoID)
        {
            EntityManager<DemoEntity> manager = new EntityManager<DemoEntity>();
            bool b = ChkExist<DemoEntity>(
                manager,
                DemoEntity.FieldCode, demoCode,
                DemoEntity.FieldID, demoID);
            return b;
        }
    }
}