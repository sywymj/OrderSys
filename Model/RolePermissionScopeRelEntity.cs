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
    //P_RolePermissionScope_Rel
    public class RolePermissionScopeEntity : BaseEntity, IEntity<RolePermissionScopeEntity>
    {
        /// <summary>
        /// TableName
        /// </summary>
        public string TableName
        {
            get { return "[P_RolePermissionScope_Rel]"; }
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
        /// PermissionScopeID
        /// </summary>		
        private int? _permissionscopeid;
        public int? PermissionScopeID
        {
            get { return _permissionscopeid; }
            set { _permissionscopeid = value; }
        }
        /// <summary>
        /// DeletionStateCode
        /// </summary>		
        private int? _deletionstatecode;
        public int? DeletionStateCode
        {
            get { return _deletionstatecode; }
            set { _deletionstatecode = value; }
        }
        /// <summary>
        /// CreateOn
        /// </summary>		
        private DateTime? _createon;
        public DateTime? CreateOn
        {
            get { return _createon; }
            set { _createon = value; }
        }
        /// <summary>
        /// CreateUserId
        /// </summary>		
        private string _createuserid;
        public string CreateUserId
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }
        /// <summary>
        /// CreateBy
        /// </summary>		
        private string _createby;
        public string CreateBy
        {
            get { return _createby; }
            set { _createby = value; }
        }
        /// <summary>
        /// ModifiedOn
        /// </summary>		
        private DateTime? _modifiedon;
        public DateTime? ModifiedOn
        {
            get { return _modifiedon; }
            set { _modifiedon = value; }
        }
        /// <summary>
        /// ModifiedUserId
        /// </summary>		
        private string _modifieduserid;
        public string ModifiedUserId
        {
            get { return _modifieduserid; }
            set { _modifieduserid = value; }
        }
        /// <summary>
        /// ModifiedBy
        /// </summary>		
        private string _modifiedby;
        public string ModifiedBy
        {
            get { return _modifiedby; }
            set { _modifiedby = value; }
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
        /// FiledName:PermissionScopeID
        /// </summary>		
        public static string FieldPermissionScopeID
        {
            get { return "PermissionScopeID"; }
        }
        /// <summary>
        /// FiledName:DeletionStateCode
        /// </summary>		
        public static string FieldDeletionStateCode
        {
            get { return "DeletionStateCode"; }
        }
        /// <summary>
        /// FiledName:CreateOn
        /// </summary>		
        public static string FieldCreateOn
        {
            get { return "CreateOn"; }
        }
        /// <summary>
        /// FiledName:CreateUserId
        /// </summary>		
        public static string FieldCreateUserId
        {
            get { return "CreateUserId"; }
        }
        /// <summary>
        /// FiledName:CreateBy
        /// </summary>		
        public static string FieldCreateBy
        {
            get { return "CreateBy"; }
        }
        /// <summary>
        /// FiledName:ModifiedOn
        /// </summary>		
        public static string FieldModifiedOn
        {
            get { return "ModifiedOn"; }
        }
        /// <summary>
        /// FiledName:ModifiedUserId
        /// </summary>		
        public static string FieldModifiedUserId
        {
            get { return "ModifiedUserId"; }
        }
        /// <summary>
        /// FiledName:ModifiedBy
        /// </summary>		
        public static string FieldModifiedBy
        {
            get { return "ModifiedBy"; }
        }
        #endregion

        public RolePermissionScopeEntity GetFrom(System.Data.DataRow dataRow)
        {
            RolePermissionScopeEntity entity = new RolePermissionScopeEntity();
            entity.ID = CommonUtil.ConvertToInt(dataRow[this.PrimaryKey]);
            entity.RoleID = CommonUtil.ConvertToInt(dataRow[FieldRoleID]);
            entity.PermissionScopeID = CommonUtil.ConvertToInt(dataRow[FieldPermissionScopeID]);
            entity.DeletionStateCode = CommonUtil.ConvertToInt(dataRow[FieldDeletionStateCode]);
            entity.CreateOn = CommonUtil.ConvertToDateTime(dataRow[FieldCreateOn]);
            entity.CreateUserId = CommonUtil.ConvertToString(dataRow[FieldCreateUserId]);
            entity.CreateBy = CommonUtil.ConvertToString(dataRow[FieldCreateBy]);
            entity.ModifiedOn = CommonUtil.ConvertToDateTime(dataRow[FieldModifiedOn]);
            entity.ModifiedUserId = CommonUtil.ConvertToString(dataRow[FieldModifiedUserId]);
            entity.ModifiedBy = CommonUtil.ConvertToString(dataRow[FieldModifiedBy]);
            return entity;
        }


        public RolePermissionScopeEntity GetFrom(System.Data.IDataReader dataReader)
        {
            RolePermissionScopeEntity entity = new RolePermissionScopeEntity();
            entity.ID = CommonUtil.ConvertToInt(dataReader[this.PrimaryKey]);
            entity.RoleID = CommonUtil.ConvertToInt(dataReader[FieldRoleID]);
            entity.PermissionScopeID = CommonUtil.ConvertToInt(dataReader[FieldPermissionScopeID]);
            entity.DeletionStateCode = CommonUtil.ConvertToInt(dataReader[FieldDeletionStateCode]);
            entity.CreateOn = CommonUtil.ConvertToDateTime(dataReader[FieldCreateOn]);
            entity.CreateUserId = CommonUtil.ConvertToString(dataReader[FieldCreateUserId]);
            entity.CreateBy = CommonUtil.ConvertToString(dataReader[FieldCreateBy]);
            entity.ModifiedOn = CommonUtil.ConvertToDateTime(dataReader[FieldModifiedOn]);
            entity.ModifiedUserId = CommonUtil.ConvertToString(dataReader[FieldModifiedUserId]);
            entity.ModifiedBy = CommonUtil.ConvertToString(dataReader[FieldModifiedBy]);
            return entity;
        }

        public void SetEntity(NonQueryBuilder sqlBuilder, RolePermissionScopeEntity entity)
        {
            sqlBuilder.SetValue(FieldRoleID, entity.RoleID);
            sqlBuilder.SetValue(FieldPermissionScopeID, entity.PermissionScopeID);
            sqlBuilder.SetValue(FieldDeletionStateCode, entity.DeletionStateCode);
            sqlBuilder.SetValue(FieldCreateOn, entity.CreateOn);
            sqlBuilder.SetValue(FieldCreateUserId, entity.CreateUserId);
            sqlBuilder.SetValue(FieldCreateBy, entity.CreateBy);
            sqlBuilder.SetValue(FieldModifiedOn, entity.ModifiedOn);
            sqlBuilder.SetValue(FieldModifiedUserId, entity.ModifiedUserId);
            sqlBuilder.SetValue(FieldModifiedBy, entity.ModifiedBy);
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