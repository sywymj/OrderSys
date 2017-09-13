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
            EntityManager<OrganizeCategoryEntity> manager = new EntityManager<OrganizeCategoryEntity>();

            UserEntity user = userService.GetCurrentUser();

            entity.DeletionStateCode = (int)TrueFalse.True;
            entity.CreateUserId = user.ID.ToString();
            entity.CreateBy = user.UserName;
            entity.CreateOn = DateTime.Now;
            manager.Insert(entity);
        }

        public void AddOranize(OrganizeEntity entity)
        {
            UserService userService = new UserService();
            EntityManager<OrganizeEntity> manager = new EntityManager<OrganizeEntity>();

            UserEntity user = userService.GetCurrentUser();

            entity.CreateUserId = user.ID.ToString();
            entity.CreateBy = user.UserName;
            entity.CreateOn = DateTime.Now;
            manager.Insert(entity);
        }

        public string[] GetTreeOrganizeIDs(string organizeCode)
        {
            string[] s = GetTreeIDs(
                "[VOrg_Organize]",
                "Organize_Code", organizeCode,
                "Organize_ID", "Organize_ParentID");
            return s;
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

    }
}
