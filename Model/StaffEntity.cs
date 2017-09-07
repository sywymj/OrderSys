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
	//P_Staff
	public class StaffEntity:BaseEntity,IEntity<StaffEntity>
	{
		/// <summary>
		/// TableName
		/// </summary>
		public string TableName
        {
            get { return "[P_Staff]"; }
        }
	
		/// <summary>
        /// 主键
        /// </summary>
		public override string PrimaryKey
        {
            get {  return "ID"; }
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
            get{ return _id; }
            set{ _id = value; }
        }        
		/// <summary>
		/// UserID
        /// </summary>		
		private int? _userid;
        public int? UserID
        {
            get{ return _userid; }
            set{ _userid = value; }
        }        
		/// <summary>
		/// OrganizeID
        /// </summary>		
		private int? _organizeid;
        public int? OrganizeID
        {
            get{ return _organizeid; }
            set{ _organizeid = value; }
        }        
		/// <summary>
		/// Name
        /// </summary>		
		private string _name;
        public string Name
        {
            get{ return _name; }
            set{ _name = value; }
        }        
		/// <summary>
		/// Tel
        /// </summary>		
		private string _tel;
        public string Tel
        {
            get{ return _tel; }
            set{ _tel = value; }
        }        
		/// <summary>
		/// Addr
        /// </summary>		
		private string _addr;
        public string Addr
        {
            get{ return _addr; }
            set{ _addr = value; }
        }        
		/// <summary>
		/// IsEnable
        /// </summary>		
		private int? _isenable;
        public int? IsEnable
        {
            get{ return _isenable; }
            set{ _isenable = value; }
        }        
		/// <summary>
		/// IsOnJob
        /// </summary>		
		private int? _isonjob;
        public int? IsOnJob
        {
            get{ return _isonjob; }
            set{ _isonjob = value; }
        }        
		/// <summary>
		/// Sex
        /// </summary>		
		private int? _sex;
        public int? Sex
        {
            get{ return _sex; }
            set{ _sex = value; }
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
		/// FiledName:UserID
        /// </summary>		
        public static string FieldUserID
        {
            get{ return "UserID"; }
        }        
		/// <summary>
		/// FiledName:OrganizeID
        /// </summary>		
        public static string FieldOrganizeID
        {
            get{ return "OrganizeID"; }
        }        
		/// <summary>
		/// FiledName:Name
        /// </summary>		
        public static string FieldName
        {
            get{ return "Name"; }
        }        
		/// <summary>
		/// FiledName:Tel
        /// </summary>		
        public static string FieldTel
        {
            get{ return "Tel"; }
        }        
		/// <summary>
		/// FiledName:Addr
        /// </summary>		
        public static string FieldAddr
        {
            get{ return "Addr"; }
        }        
		/// <summary>
		/// FiledName:IsEnable
        /// </summary>		
        public static string FieldIsEnable
        {
            get{ return "IsEnable"; }
        }        
		/// <summary>
		/// FiledName:IsOnJob
        /// </summary>		
        public static string FieldIsOnJob
        {
            get{ return "IsOnJob"; }
        }        
		/// <summary>
		/// FiledName:Sex
        /// </summary>		
        public static string FieldSex
        {
            get{ return "Sex"; }
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
   		
   		public StaffEntity GetFrom(System.Data.DataRow dataRow)
        {
        	StaffEntity entity = new StaffEntity();
	   		entity.ID = CommonUtil.ConvertToInt(dataRow[this.PrimaryKey]);
				entity.UserID = CommonUtil.ConvertToInt(dataRow[FieldUserID]);
			entity.OrganizeID = CommonUtil.ConvertToInt(dataRow[FieldOrganizeID]);
			entity.Name = CommonUtil.ConvertToString(dataRow[FieldName]);
			entity.Tel = CommonUtil.ConvertToString(dataRow[FieldTel]);
			entity.Addr = CommonUtil.ConvertToString(dataRow[FieldAddr]);
			entity.IsEnable = CommonUtil.ConvertToInt(dataRow[FieldIsEnable]);
			entity.IsOnJob = CommonUtil.ConvertToInt(dataRow[FieldIsOnJob]);
			entity.Sex = CommonUtil.ConvertToInt(dataRow[FieldSex]);
			entity.DeletionStateCode = CommonUtil.ConvertToInt(dataRow[FieldDeletionStateCode]);
			entity.CreateOn = CommonUtil.ConvertToDateTime(dataRow[FieldCreateOn]);
			entity.CreateUserId = CommonUtil.ConvertToString(dataRow[FieldCreateUserId]);
			entity.CreateBy = CommonUtil.ConvertToString(dataRow[FieldCreateBy]);
			entity.ModifiedOn = CommonUtil.ConvertToDateTime(dataRow[FieldModifiedOn]);
			entity.ModifiedUserId = CommonUtil.ConvertToString(dataRow[FieldModifiedUserId]);
			entity.ModifiedBy = CommonUtil.ConvertToString(dataRow[FieldModifiedBy]);
						return entity;
		}
		
		
   		public StaffEntity GetFrom(System.Data.IDataReader dataReader)
        {
        	StaffEntity entity = new StaffEntity();
	   		entity.ID = CommonUtil.ConvertToInt(dataReader[this.PrimaryKey]);
				entity.UserID = CommonUtil.ConvertToInt(dataReader[FieldUserID]);
			entity.OrganizeID = CommonUtil.ConvertToInt(dataReader[FieldOrganizeID]);
			entity.Name = CommonUtil.ConvertToString(dataReader[FieldName]);
			entity.Tel = CommonUtil.ConvertToString(dataReader[FieldTel]);
			entity.Addr = CommonUtil.ConvertToString(dataReader[FieldAddr]);
			entity.IsEnable = CommonUtil.ConvertToInt(dataReader[FieldIsEnable]);
			entity.IsOnJob = CommonUtil.ConvertToInt(dataReader[FieldIsOnJob]);
			entity.Sex = CommonUtil.ConvertToInt(dataReader[FieldSex]);
			entity.DeletionStateCode = CommonUtil.ConvertToInt(dataReader[FieldDeletionStateCode]);
			entity.CreateOn = CommonUtil.ConvertToDateTime(dataReader[FieldCreateOn]);
			entity.CreateUserId = CommonUtil.ConvertToString(dataReader[FieldCreateUserId]);
			entity.CreateBy = CommonUtil.ConvertToString(dataReader[FieldCreateBy]);
			entity.ModifiedOn = CommonUtil.ConvertToDateTime(dataReader[FieldModifiedOn]);
			entity.ModifiedUserId = CommonUtil.ConvertToString(dataReader[FieldModifiedUserId]);
			entity.ModifiedBy = CommonUtil.ConvertToString(dataReader[FieldModifiedBy]);
						return entity;
		}
		
		public void SetEntity(NonQueryBuilder sqlBuilder, StaffEntity entity)
        {
	   		sqlBuilder.SetValue(FieldUserID, entity.UserID);
			sqlBuilder.SetValue(FieldOrganizeID, entity.OrganizeID);
			sqlBuilder.SetValue(FieldName, entity.Name);
			sqlBuilder.SetValue(FieldTel, entity.Tel);
			sqlBuilder.SetValue(FieldAddr, entity.Addr);
			sqlBuilder.SetValue(FieldIsEnable, entity.IsEnable);
			sqlBuilder.SetValue(FieldIsOnJob, entity.IsOnJob);
			sqlBuilder.SetValue(FieldSex, entity.Sex);
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