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
    public class OrganizeService:BaseService
    {
        public void AddOrganizeCategory(OrganizeCategoryEntity entity)
        {
            UserService userService = new UserService();
            UserEntity user = userService.GetCurrentUser();

            EntityManager<OrganizeCategoryEntity> manager = new EntityManager<OrganizeCategoryEntity>();
            entity.CreateUserId = user.ID.ToString();
            entity.CreateBy = user.UserName;
            entity.CreateOn = DateTime.Now;
            manager.Insert(entity);
        }

        public void EditOrganizeCategory(OrganizeCategoryEntity entity)
        {
            UserService userService = new UserService();
            UserEntity currentUser = userService.GetCurrentUser();

            EntityManager<OrganizeCategoryEntity> manager = new EntityManager<OrganizeCategoryEntity>();
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(OrganizeCategoryEntity.FieldCode, entity.Code));
            kvps.Add(new KeyValuePair<string, object>(OrganizeCategoryEntity.FieldFullName, entity.FullName));
            kvps.Add(new KeyValuePair<string, object>(OrganizeCategoryEntity.FieldDescription, entity.Description));
            kvps.Add(new KeyValuePair<string, object>(OrganizeCategoryEntity.FieldModifiedUserId, currentUser.ID.ToString()));
            kvps.Add(new KeyValuePair<string, object>(OrganizeCategoryEntity.FieldModifiedBy, currentUser.UserName));
            kvps.Add(new KeyValuePair<string, object>(OrganizeCategoryEntity.FieldModifiedOn, DateTime.Now));
            manager.Update(kvps, entity.ID);
        }

        public OrganizeCategoryEntity GetOrganizeCategory(int organizeCategoryID)
        {
            EntityManager<OrganizeCategoryEntity> manager = new EntityManager<OrganizeCategoryEntity>();
            OrganizeCategoryEntity entity = manager.GetSingle(organizeCategoryID);
            return entity;
        }



        public string[] GetTreeOrganizeIDs(string organizeCode)
        {
            string[] s = GetTreeIDs(
                "[VOrg_Organize]",
                "Organize_Code", organizeCode,
                "Organize_ID", "Organize_ParentID");
            return s;
        }

        public List<OrganizeEntity> GetTreeOrganizeListByUser(UserEntity user)
        {
            //如何处理管理员情况
            EntityManager<StaffEntity> manager = new EntityManager<StaffEntity>();
            StaffEntity staff = manager.GetSingle(user.ID, StaffEntity.FieldUserID);

            int deep = 0;
            if (user.ID == 1)
            {
                deep = 0;
            }
            else
            {
                deep = 1;
            }

            List<OrganizeEntity> list = GetTreeOrganizeList((int)staff.OrganizeID, deep);
            return list;
        }

        public List<OrganizeEntity> GetTreeOrganizeList(int organizeID, int deep, bool onlyChild = true)
        {
            EntityManager<OrganizeEntity> manager = new EntityManager<OrganizeEntity>();
            OrganizeEntity organize = manager.GetSingle(organizeID);

            string organizeCode = organize.Code.SubstringWithDeep('.', deep);//OrderSys.FSWGY
            List<OrganizeEntity> list = GetTreeOrganizeList(organizeCode, onlyChild);
            return list;
        }


        public List<OrganizeEntity> GetTreeOrganizeList(string organizeCode,bool onlyChild = true)
        {
            EntityManager<OrganizeEntity> manager = new EntityManager<OrganizeEntity>();

            string[] ids = GetTreeOrganizeIDs(organizeCode);
            if (ids.Length == 0)
            {
                throw new JSException(JSErrMsg.ERR_CODE_DATA_MISSING, string.Format(JSErrMsg.ERR_MSG_DATA_MISSING,"OrganizeCode"));
            }

            WhereStatement where = new WhereStatement();
            where.Add(OrganizeEntity.FieldID, Comparison.In, ids);
            
            int count= 0;
            List<OrganizeEntity> list = manager.GetList(where, out count);

            if (onlyChild)
            {
                list.Where(l => l.Code != organizeCode);
            }

            return list;
        }

        public DataTable GetGrantOrganizeDT(string resourceCode, string resourceType, out int count)
        {
            if (!(resourceType == ResourceType.Data.ToString()))
            {
                throw new JSException(JSErrMsg.ERR_CODE_NotAllowGrantItem, string.Format(JSErrMsg.ERR_MSG_NotAllowGrantItem, "资源类型为" + resourceType + "，"));
            }
            if (resourceCode.Split('.').Length < 2)
            {
                throw new JSException(JSErrMsg.ERR_CODE_ErrorFormatCode, JSErrMsg.ERR_MSG_ErrorFormatCode);
            }
            //resouceCode正确的格式 OrderSys_Data.XXXX
            string parentOrganizeCode = resourceCode.Split('.')[0].Split('_')[0];

            DataTable dt = GetTreeOrganizeDT(out count, parentOrganizeCode);
            return dt;
        }

        public DataTable GetTreeOrganizeDTByUser(UserEntity user,out int count)
        {
            EntityManager<StaffEntity> manager = new EntityManager<StaffEntity>();
            StaffEntity staff = manager.GetSingle(user.ID, StaffEntity.FieldUserID);

            int deep = 0;
            if (user.ID == 1)
            {
                deep = 0;
            }
            else
            {
                deep = 1;
            }

            DataTable dt = GetTreeOrganizeDT((int)staff.OrganizeID, deep,out count);
            return dt;
        }

        public DataTable GetTreeOrganizeDT(int organizeID, int deep,out int count)
        {
            EntityManager<OrganizeEntity> manager = new EntityManager<OrganizeEntity>();
            OrganizeEntity organize = manager.GetSingle(organizeID);

            string organizeCode = organize.Code.SubstringWithDeep('.', deep);//OrderSys.FSWGY
            DataTable dt = GetTreeOrganizeDT(out count, organizeCode);
            return dt;
        }

        public DataTable GetTreeOrganizeDT(out int count, string parentOrganizeCode = "BaseSys")
        {
            ViewManager vmanager = new ViewManager("VOrg_Organize");

            string[] ids = GetTreeOrganizeIDs(parentOrganizeCode);
            WhereStatement where = new WhereStatement();
            where.Add("Organize_ID", Comparison.In, ids);

            DataTable dt = vmanager.GetDataTable(where, out count);
            return dt;
        }


        public void AddOrganize(OrganizeEntity entity)
        {
            UserService userService = new UserService();
            UserEntity currentUser = userService.GetCurrentUser();

            EntityManager<OrganizeEntity> manager = new EntityManager<OrganizeEntity>();
            entity.Layer = 0;//TODO
            entity.CreateUserId = currentUser.ID.ToString();
            entity.CreateBy = currentUser.UserName;
            entity.CreateOn = DateTime.Now;
            manager.Insert(entity);
        }

        public void EditOrganize(OrganizeEntity entity)
        {
            UserService userService = new UserService();
            UserEntity currentUser = userService.GetCurrentUser();

            EntityManager<OrganizeEntity> manager = new EntityManager<OrganizeEntity>();
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(OrganizeEntity.FieldOrganizeCategoryID, entity.OrganizeCategoryID));
            kvps.Add(new KeyValuePair<string, object>(OrganizeEntity.FieldParentID, entity.ParentID));
            kvps.Add(new KeyValuePair<string, object>(OrganizeEntity.FieldCode, entity.Code));
            kvps.Add(new KeyValuePair<string, object>(OrganizeEntity.FieldFullName, entity.FullName));
            kvps.Add(new KeyValuePair<string, object>(OrganizeEntity.FieldShortName, entity.ShortName));
            kvps.Add(new KeyValuePair<string, object>(OrganizeEntity.FieldOuterPhone, entity.OuterPhone));
            kvps.Add(new KeyValuePair<string, object>(OrganizeEntity.FieldInnerPhone, entity.InnerPhone));
            kvps.Add(new KeyValuePair<string, object>(OrganizeEntity.FieldFax, entity.Fax));
            kvps.Add(new KeyValuePair<string, object>(OrganizeEntity.FieldEmail, entity.Email));
            kvps.Add(new KeyValuePair<string, object>(OrganizeEntity.FieldPostalcode, entity.Postalcode));
            kvps.Add(new KeyValuePair<string, object>(OrganizeEntity.FieldAddress, entity.Address));
            kvps.Add(new KeyValuePair<string, object>(OrganizeEntity.FieldWeb, entity.Web));
            kvps.Add(new KeyValuePair<string, object>(OrganizeEntity.FieldSortCode, entity.SortCode));
            kvps.Add(new KeyValuePair<string, object>(OrganizeEntity.FieldIsVisible, entity.IsVisible));
            kvps.Add(new KeyValuePair<string, object>(OrganizeEntity.FieldDescription, entity.Description));
            kvps.Add(new KeyValuePair<string, object>(OrganizeEntity.FieldModifiedUserId, currentUser.ID.ToString()));
            kvps.Add(new KeyValuePair<string, object>(OrganizeEntity.FieldModifiedBy, currentUser.UserName));
            kvps.Add(new KeyValuePair<string, object>(OrganizeEntity.FieldModifiedOn, DateTime.Now));
            manager.Update(kvps, entity.ID);
        }

        public OrganizeEntity GetOrganize(int organizeID)
        {
            EntityManager<OrganizeEntity> manager = new EntityManager<OrganizeEntity>();
            OrganizeEntity entity = manager.GetSingle(organizeID);
            return entity;
        }

        public bool ChkOrganizeCodeExist(string organizeCode, string organizeID)
        {
            EntityManager<OrganizeEntity> manager = new EntityManager<OrganizeEntity>();
            bool b = ChkExist<OrganizeEntity>(
                manager,
                ResourceEntity.FieldCode, organizeCode,
                ResourceEntity.FieldID, organizeID);
            return b;
        }

        public List<OrganizeCategoryEntity> GetOrganizeCategorys(Paging paging,out int count)
        {
            EntityManager<OrganizeCategoryEntity> manager = new EntityManager<OrganizeCategoryEntity>();
            WhereStatement where = new WhereStatement();

            List<OrganizeCategoryEntity> list = manager.GetListByPage(where, out count, paging.PageIndex, paging.PageSize);
            return list;
        }

        public List<OrganizeCategoryEntity> GetOrganizeCategorys()
        {
            int count = 0;
            EntityManager<OrganizeCategoryEntity> manager = new EntityManager<OrganizeCategoryEntity>();
            WhereStatement where = new WhereStatement();

            List<OrganizeCategoryEntity> list = manager.GetList(where, out count);
            return list;
        }

        public DataTable GetOrganizeCategoryDT(out int count, string p)
        {
            throw new NotImplementedException();
        }

        public bool ChkOrganizeCategoryCodeExist(string organizeCategoryCode, string organizeCategoryID)
        {
            EntityManager<OrganizeCategoryEntity> manager = new EntityManager<OrganizeCategoryEntity>();
            bool b = ChkExist<OrganizeCategoryEntity>(
                manager,
                OrganizeCategoryEntity.FieldCode, organizeCategoryCode,
                OrganizeCategoryEntity.FieldID, organizeCategoryID);
            return b;
        }
    }
}
