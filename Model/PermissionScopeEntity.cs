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
	//P_PermissionScope
	public class PermissionScopeEntity:BaseEntity,IEntity<PermissionScopeEntity>
	{
		/// <summary>
		/// TableName
		/// </summary>
		public string TableName
        {
            get { return "[P_PermissionScope]"; }
        }
	
				
	
		
	
   		#region 字段、属性
      	/// <summary>
		/// ID
        /// </summary>		
		private int? _id;
        public int? ID
        {
            get{ return _id; }
            set{ _id = value; }
        }        
		/// <summary>
		/// ResourceCategory
        /// </summary>		
		private string _resourcecategory;
        public string ResourceCategory
        {
            get{ return _resourcecategory; }
            set{ _resourcecategory = value; }
        }        
		/// <summary>
		/// ResourceId
        /// </summary>		
		private string _resourceid;
        public string ResourceId
        {
            get{ return _resourceid; }
            set{ _resourceid = value; }
        }        
		/// <summary>
		/// TargetCategory
        /// </summary>		
		private string _targetcategory;
        public string TargetCategory
        {
            get{ return _targetcategory; }
            set{ _targetcategory = value; }
        }        
		/// <summary>
		/// TargetId
        /// </summary>		
		private string _targetid;
        public string TargetId
        {
            get{ return _targetid; }
            set{ _targetid = value; }
        }        
		/// <summary>
		/// PermissionId
        /// </summary>		
		private int? _permissionid;
        public int? PermissionId
        {
            get{ return _permissionid; }
            set{ _permissionid = value; }
        }        
		/// <summary>
		/// PermissionConstraint
        /// </summary>		
		private string _permissionconstraint;
        public string PermissionConstraint
        {
            get{ return _permissionconstraint; }
            set{ _permissionconstraint = value; }
        }        
		/// <summary>
		/// DeletionStateCode
        /// </summary>		
		private int? _deletionstatecode;
        public int? DeletionStateCode
        {
            get{ return _deletionstatecode; }
            set{ _deletionstatecode = value; }
        }        
		/// <summary>
		/// CreateOn
        /// </summary>		
		private DateTime? _createon;
        public DateTime? CreateOn
        {
            get{ return _createon; }
            set{ _createon = value; }
        }        
		/// <summary>
		/// CreateUserId
        /// </summary>		
		private string _createuserid;
        public string CreateUserId
        {
            get{ return _createuserid; }
            set{ _createuserid = value; }
        }        
		/// <summary>
		/// CreateBy
        /// </summary>		
		private string _createby;
        public string CreateBy
        {
            get{ return _createby; }
            set{ _createby = value; }
        }        
		/// <summary>
		/// ModifiedOn
        /// </summary>		
		private DateTime? _modifiedon;
        public DateTime? ModifiedOn
        {
            get{ return _modifiedon; }
            set{ _modifiedon = value; }
        }        
		/// <summary>
		/// ModifiedUserId
        /// </summary>		
		private string _modifieduserid;
        public string ModifiedUserId
        {
            get{ return _modifieduserid; }
            set{ _modifieduserid = value; }
        }        
		/// <summary>
		/// ModifiedBy
        /// </summary>		
		private string _modifiedby;
        public string ModifiedBy
        {
            get{ return _modifiedby; }
            set{ _modifiedby = value; }
        }        
				#endregion

		#region  数据库字段名称
      	/// <summary>
		/// FiledName:ID
        /// </summary>		
        public static string FieldID
        {
            get{ return "ID"; }
        }        
		/// <summary>
		/// FiledName:ResourceCategory
        /// </summary>		
        public static string FieldResourceCategory
        {
            get{ return "ResourceCategory"; }
        }        
		/// <summary>
		/// FiledName:ResourceId
        /// </summary>		
        public static string FieldResourceId
        {
            get{ return "ResourceId"; }
        }        
		/// <summary>
		/// FiledName:TargetCategory
        /// </summary>		
        public static string FieldTargetCategory
        {
            get{ return "TargetCategory"; }
        }        
		/// <summary>
		/// FiledName:TargetId
        /// </summary>		
        public static string FieldTargetId
        {
            get{ return "TargetId"; }
        }        
		/// <summary>
		/// FiledName:PermissionId
        /// </summary>		
        public static string FieldPermissionId
        {
            get{ return "PermissionId"; }
        }        
		/// <summary>
		/// FiledName:PermissionConstraint
        /// </summary>		
        public static string FieldPermissionConstraint
        {
            get{ return "PermissionConstraint"; }
        }        
		/// <summary>
		/// FiledName:DeletionStateCode
        /// </summary>		
        public static string FieldDeletionStateCode
        {
            get{ return "DeletionStateCode"; }
        }        
		/// <summary>
		/// FiledName:CreateOn
        /// </summary>		
        public static string FieldCreateOn
        {
            get{ return "CreateOn"; }
        }        
		/// <summary>
		/// FiledName:CreateUserId
        /// </summary>		
        public static string FieldCreateUserId
        {
            get{ return "CreateUserId"; }
        }        
		/// <summary>
		/// FiledName:CreateBy
        /// </summary>		
        public static string FieldCreateBy
        {
            get{ return "CreateBy"; }
        }        
		/// <summary>
		/// FiledName:ModifiedOn
        /// </summary>		
        public static string FieldModifiedOn
        {
            get{ return "ModifiedOn"; }
        }        
		/// <summary>
		/// FiledName:ModifiedUserId
        /// </summary>		
        public static string FieldModifiedUserId
        {
            get{ return "ModifiedUserId"; }
        }        
		/// <summary>
		/// FiledName:ModifiedBy
        /// </summary>		
        public static string FieldModifiedBy
        {
            get{ return "ModifiedBy"; }
        }        
		   		#endregion
   		
   		public PermissionScopeEntity GetFrom(System.Data.DataRow dataRow)
        {
        	PermissionScopeEntity entity = new PermissionScopeEntity();
	   		entity.ID = CommonUtil.ConvertToInt(dataRow[FieldID]);
			entity.ResourceCategory = CommonUtil.ConvertToString(dataRow[FieldResourceCategory]);
			entity.ResourceId = CommonUtil.ConvertToString(dataRow[FieldResourceId]);
			entity.TargetCategory = CommonUtil.ConvertToString(dataRow[FieldTargetCategory]);
			entity.TargetId = CommonUtil.ConvertToString(dataRow[FieldTargetId]);
			entity.PermissionId = CommonUtil.ConvertToInt(dataRow[FieldPermissionId]);
			entity.PermissionConstraint = CommonUtil.ConvertToString(dataRow[FieldPermissionConstraint]);
			entity.DeletionStateCode = CommonUtil.ConvertToInt(dataRow[FieldDeletionStateCode]);
			entity.CreateOn = CommonUtil.ConvertToDateTime(dataRow[FieldCreateOn]);
			entity.CreateUserId = CommonUtil.ConvertToString(dataRow[FieldCreateUserId]);
			entity.CreateBy = CommonUtil.ConvertToString(dataRow[FieldCreateBy]);
			entity.ModifiedOn = CommonUtil.ConvertToDateTime(dataRow[FieldModifiedOn]);
			entity.ModifiedUserId = CommonUtil.ConvertToString(dataRow[FieldModifiedUserId]);
			entity.ModifiedBy = CommonUtil.ConvertToString(dataRow[FieldModifiedBy]);
						return this;
		}
		
		
   		public PermissionScopeEntity GetFrom(System.Data.IDataReader dataReader)
        {
        	PermissionScopeEntity entity = new PermissionScopeEntity();
	   		entity.ID = CommonUtil.ConvertToInt(dataReader[FieldID]);
			entity.ResourceCategory = CommonUtil.ConvertToString(dataReader[FieldResourceCategory]);
			entity.ResourceId = CommonUtil.ConvertToString(dataReader[FieldResourceId]);
			entity.TargetCategory = CommonUtil.ConvertToString(dataReader[FieldTargetCategory]);
			entity.TargetId = CommonUtil.ConvertToString(dataReader[FieldTargetId]);
			entity.PermissionId = CommonUtil.ConvertToInt(dataReader[FieldPermissionId]);
			entity.PermissionConstraint = CommonUtil.ConvertToString(dataReader[FieldPermissionConstraint]);
			entity.DeletionStateCode = CommonUtil.ConvertToInt(dataReader[FieldDeletionStateCode]);
			entity.CreateOn = CommonUtil.ConvertToDateTime(dataReader[FieldCreateOn]);
			entity.CreateUserId = CommonUtil.ConvertToString(dataReader[FieldCreateUserId]);
			entity.CreateBy = CommonUtil.ConvertToString(dataReader[FieldCreateBy]);
			entity.ModifiedOn = CommonUtil.ConvertToDateTime(dataReader[FieldModifiedOn]);
			entity.ModifiedUserId = CommonUtil.ConvertToString(dataReader[FieldModifiedUserId]);
			entity.ModifiedBy = CommonUtil.ConvertToString(dataReader[FieldModifiedBy]);
						return this;
		}
		
		public void SetEntity(NonQueryBuilder sqlBuilder, PermissionScopeEntity entity)
        {
	   		sqlBuilder.SetValue(FieldID, entity.ID);
			sqlBuilder.SetValue(FieldResourceCategory, entity.ResourceCategory);
			sqlBuilder.SetValue(FieldResourceId, entity.ResourceId);
			sqlBuilder.SetValue(FieldTargetCategory, entity.TargetCategory);
			sqlBuilder.SetValue(FieldTargetId, entity.TargetId);
			sqlBuilder.SetValue(FieldPermissionId, entity.PermissionId);
			sqlBuilder.SetValue(FieldPermissionConstraint, entity.PermissionConstraint);
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