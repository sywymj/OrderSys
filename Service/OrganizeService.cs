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
    /*
     * OrganizeCode 命名规范：{系统名}.{机构...}
     * ResourceCode 命名规范：{系统名}_{类别}.XXX => 例子：OrderSys_Data.Role
     * 
     */
    public class OrganizeService:BaseService
    {
        #region Organize
        public void AddOrganize(OrganizeEntity entity)
        {
            entity.Layer = 0;//TODO
            entity.CreateUserId = CurrentUser.ID.ToString();
            entity.CreateBy = CurrentUser.UserName;
            entity.CreateOn = DateTime.Now;

            EntityManager<OrganizeEntity> manager = new EntityManager<OrganizeEntity>();
            manager.Insert(entity);
        }

        public void EditOrganize(OrganizeEntity entity)
        {
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
            kvps.Add(new KeyValuePair<string, object>(OrganizeEntity.FieldModifiedUserId, CurrentUser.ID.ToString()));
            kvps.Add(new KeyValuePair<string, object>(OrganizeEntity.FieldModifiedBy, CurrentUser.UserName));
            kvps.Add(new KeyValuePair<string, object>(OrganizeEntity.FieldModifiedOn, DateTime.Now));

            EntityManager<OrganizeEntity> manager = new EntityManager<OrganizeEntity>();
            manager.Update(kvps, entity.ID);
        }

        public OrganizeEntity GetOrganize(int organizeID)
        {
            EntityManager<OrganizeEntity> manager = new EntityManager<OrganizeEntity>();
            OrganizeEntity entity = manager.GetSingle(organizeID);
            return entity;
        }
        #endregion

        #region OrganizeCategory
        public void AddOrganizeCategory(OrganizeCategoryEntity entity)
        {
            entity.CreateUserId = CurrentUser.ID.ToString();
            entity.CreateBy = CurrentUser.UserName;
            entity.CreateOn = DateTime.Now;

            EntityManager<OrganizeCategoryEntity> manager = new EntityManager<OrganizeCategoryEntity>();
            manager.Insert(entity);
        }
        public void EditOrganizeCategory(OrganizeCategoryEntity entity)
        {
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(OrganizeCategoryEntity.FieldCode, entity.Code));
            kvps.Add(new KeyValuePair<string, object>(OrganizeCategoryEntity.FieldFullName, entity.FullName));
            kvps.Add(new KeyValuePair<string, object>(OrganizeCategoryEntity.FieldDescription, entity.Description));
            kvps.Add(new KeyValuePair<string, object>(OrganizeCategoryEntity.FieldModifiedUserId, CurrentUser.ID.ToString()));
            kvps.Add(new KeyValuePair<string, object>(OrganizeCategoryEntity.FieldModifiedBy, CurrentUser.UserName));
            kvps.Add(new KeyValuePair<string, object>(OrganizeCategoryEntity.FieldModifiedOn, DateTime.Now));

            EntityManager<OrganizeCategoryEntity> manager = new EntityManager<OrganizeCategoryEntity>();
            manager.Update(kvps, entity.ID);
        }
        public OrganizeCategoryEntity GetOrganizeCategory(int organizeCategoryID)
        {
            EntityManager<OrganizeCategoryEntity> manager = new EntityManager<OrganizeCategoryEntity>();
            OrganizeCategoryEntity entity = manager.GetSingle(organizeCategoryID);
            return entity;
        }
        public List<OrganizeCategoryEntity> GetOrganizeCategorys(Paging paging, out int count)
        {
            WhereStatement where = new WhereStatement();

            EntityManager<OrganizeCategoryEntity> manager = new EntityManager<OrganizeCategoryEntity>();
            List<OrganizeCategoryEntity> list = manager.GetListByPage(where, out count, paging.PageIndex, paging.PageSize);
            return list;
        }
        public List<OrganizeCategoryEntity> GetOrganizeCategorys()
        {
            WhereStatement where = new WhereStatement();

            int count = 0;
            EntityManager<OrganizeCategoryEntity> manager = new EntityManager<OrganizeCategoryEntity>();
            List<OrganizeCategoryEntity> list = manager.GetList(where, out count);
            return list;
        }

        #endregion

        public List<OrganizeEntity> GetTreeOrganizeListByUser(UserEntity user,bool onlyChild = true)
        {
            //如何处理管理员情况
            UserService userService = new UserService();
            StaffEntity staff = userService.GetStaff((int)user.ID);

            int deep = 0;
            if (user.ID == 1)
            {
                deep = 0;
            }
            else
            {
                deep = 1;
            }

            List<OrganizeEntity> list = GetTreeOrganizeList((int)staff.OrganizeID, deep, onlyChild);
            return list;
        }
        private List<OrganizeEntity> GetTreeOrganizeList(int organizeID, int deep, bool onlyChild = true)
        {
            EntityManager<OrganizeEntity> manager = new EntityManager<OrganizeEntity>();
            OrganizeEntity organize = manager.GetSingle(organizeID);

            //OrderSys.FSWGY
            string parentOrganizeCode = organize.Code.SubstringWithDeep('.', deep);
            List<OrganizeEntity> list = GetTreeOrganizeList(parentOrganizeCode, onlyChild);
            return list;
        }
        private List<OrganizeEntity> GetTreeOrganizeList(string parentOrganizeCode,bool onlyChild = true)
        {
            string[] ids = GetTreeOrganizeIDs(parentOrganizeCode);
            if (ids.Length == 0)
            {
                throw new JSException(JSErrMsg.ERR_CODE_DATA_MISSING, string.Format(JSErrMsg.ERR_MSG_DATA_MISSING,"OrganizeCode"));
            }

            WhereStatement where = new WhereStatement();
            where.Add(OrganizeEntity.FieldID, Comparison.In, ids);
            
            int count= 0;
            EntityManager<OrganizeEntity> manager = new EntityManager<OrganizeEntity>();
            List<OrganizeEntity> list = manager.GetList(where, out count);

            if (onlyChild)
            {
                list.Where(l => l.Code != parentOrganizeCode);
            }

            return list;
        }

        /// <summary>
        /// 获取该系统下的所有组织机构
        /// </summary>
        /// <param name="resourceCode"></param>
        /// <param name="resourceType"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public DataTable GetTreeOrganizeDT(string resourceCode, string resourceType, out int count)
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
        public DataTable GetTreeOrganizeDTByUser(UserEntity user, out int count)
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

            DataTable dt = GetTreeOrganizeDT((int)staff.OrganizeID, deep, out count);
            return dt;
        }
        private DataTable GetTreeOrganizeDT(int organizeID, int deep,out int count)
        {
            EntityManager<OrganizeEntity> manager = new EntityManager<OrganizeEntity>();
            OrganizeEntity organize = manager.GetSingle(organizeID);

            //OrderSys.FSWGY
            string organizeCode = organize.Code.SubstringWithDeep('.', deep);
            DataTable dt = GetTreeOrganizeDT(out count, organizeCode);
            return dt;
        }
        private DataTable GetTreeOrganizeDT(out int count, string parentOrganizeCode)
        {
            string[] ids = GetTreeOrganizeIDs(parentOrganizeCode);
            WhereStatement where = new WhereStatement();
            where.Add("Organize_ID", Comparison.In, ids);

            ViewManager vmanager = new ViewManager("VOrg_Organize");
            DataTable dt = vmanager.GetDataTable(where, out count);
            return dt;
        }

        public string[] GetTreeOrganizeIDs(string parentOrganizeCode)
        {
            string[] s = GetTreeIDs(
                "[VOrg_Organize]",
                "Organize_Code", parentOrganizeCode,
                "Organize_ID", "Organize_ParentID");
            return s;
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
