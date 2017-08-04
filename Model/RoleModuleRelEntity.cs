using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using JSNet.Utilities;
using JSNet.Model;
using JSNet.DbUtilities;
namespace JSNet.Model
{
    //P_RoleModule_Rel
    public class RoleModuleEntity : BaseEntity, IEntity<RoleModuleEntity>
    {
        /// <summary>
        /// TableName
        /// </summary>
        public string TableName
        {
            get { return "[P_RoleModule_Rel]"; }
        }

        /// <summary>
        /// 主键
        /// </summary>
        public override string PrimaryKey
        {
            get { return "ID"; }
        }


        /// <summary>
        /// 主键自动递增
        /// </summary>
        public override bool IsIdenty
        {
            get { return true; }
        }


        #region 字段、属性
        /// <summary>
        /// ID
        /// </summary>		
        private int? _id;
        public int? ID
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// RoleID
        /// </summary>		
        private int? _roleid;
        public int? RoleID
        {
            get { return _roleid; }
            set { _roleid = value; }
        }
        /// <summary>
        /// ModuleID
        /// </summary>		
        private int? _moduleid;
        public int? ModuleID
        {
            get { return _moduleid; }
            set { _moduleid = value; }
        }
        #endregion

        #region  数据库字段名称
        /// <summary>
        /// FiledName:ID
        /// </summary>		
        public static string FieldID
        {
            get { return "ID"; }
        }
        /// <summary>
        /// FiledName:RoleID
        /// </summary>		
        public static string FieldRoleID
        {
            get { return "RoleID"; }
        }
        /// <summary>
        /// FiledName:ModuleID
        /// </summary>		
        public static string FieldModuleID
        {
            get { return "ModuleID"; }
        }
        #endregion

        public RoleModuleEntity GetFrom(System.Data.DataRow dataRow)
        {
            this.ID = CommonUtil.ConvertToInt(dataRow[this.PrimaryKey]);
            this.RoleID = CommonUtil.ConvertToInt(dataRow[FieldRoleID]);
            this.ModuleID = CommonUtil.ConvertToInt(dataRow[FieldModuleID]);
            return this;
        }


        public RoleModuleEntity GetFrom(System.Data.IDataReader dataReader)
        {
            this.ID = CommonUtil.ConvertToInt(dataReader[this.PrimaryKey]);
            this.RoleID = CommonUtil.ConvertToInt(dataReader[FieldRoleID]);
            this.ModuleID = CommonUtil.ConvertToInt(dataReader[FieldModuleID]);
            return this;
        }

        public void SetEntity(NonQueryBuilder sqlBuilder, RoleModuleEntity entity)
        {
            sqlBuilder.SetValue(FieldRoleID, entity.RoleID);
            sqlBuilder.SetValue(FieldModuleID, entity.ModuleID);
        }

        public void GetFromExpand(System.Data.DataRow dataRow)
        {
            throw new NotImplementedException();
        }

        public void GetFromExpand(System.Data.IDataReader dataReader)
        {
            throw new NotImplementedException();
        }
    }
}