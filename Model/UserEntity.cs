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
	//P_User
	public class UserEntity:BaseEntity,IEntity<UserEntity>
	{
		/// <summary>
		/// TableName
		/// </summary>
		public string TableName
        {
            get { return "[P_User]"; }
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
		/// UserName
        /// </summary>		
		private string _username;
        public string UserName
        {
            get{ return _username; }
            set{ _username = value; }
        }        
		/// <summary>
		/// Password
        /// </summary>		
		private string _password;
        public string Password
        {
            get{ return _password; }
            set{ _password = value; }
        }        
		/// <summary>
		/// OpenID
        /// </summary>		
		private int? _openid;
        public int? OpenID
        {
            get{ return _openid; }
            set{ _openid = value; }
        }        
		/// <summary>
		/// IsLogin
        /// </summary>		
		private int? _islogin;
        public int? IsLogin
        {
            get{ return _islogin; }
            set{ _islogin = value; }
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
		/// Description
        /// </summary>		
		private string _description;
        public string Description
        {
            get{ return _description; }
            set{ _description = value; }
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
		/// FiledName:UserName
        /// </summary>		
        public static string FieldUserName
        {
            get{ return "UserName"; }
        }        
		/// <summary>
		/// FiledName:Password
        /// </summary>		
        public static string FieldPassword
        {
            get{ return "Password"; }
        }        
		/// <summary>
		/// FiledName:OpenID
        /// </summary>		
        public static string FieldOpenID
        {
            get{ return "OpenID"; }
        }        
		/// <summary>
		/// FiledName:IsLogin
        /// </summary>		
        public static string FieldIsLogin
        {
            get{ return "IsLogin"; }
        }        
		/// <summary>
		/// FiledName:IsEnable
        /// </summary>		
        public static string FieldIsEnable
        {
            get{ return "IsEnable"; }
        }        
		/// <summary>
		/// FiledName:Description
        /// </summary>		
        public static string FieldDescription
        {
            get{ return "Description"; }
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
   		
   		public UserEntity GetFrom(System.Data.DataRow dataRow)
        {
        	UserEntity entity = new UserEntity();
	   		entity.ID = CommonUtil.ConvertToInt(dataRow[this.PrimaryKey]);
				entity.UserName = CommonUtil.ConvertToString(dataRow[FieldUserName]);
			entity.Password = CommonUtil.ConvertToString(dataRow[FieldPassword]);
			entity.OpenID = CommonUtil.ConvertToInt(dataRow[FieldOpenID]);
			entity.IsLogin = CommonUtil.ConvertToInt(dataRow[FieldIsLogin]);
			entity.IsEnable = CommonUtil.ConvertToInt(dataRow[FieldIsEnable]);
			entity.Description = CommonUtil.ConvertToString(dataRow[FieldDescription]);
			entity.DeletionStateCode = CommonUtil.ConvertToInt(dataRow[FieldDeletionStateCode]);
			entity.CreateOn = CommonUtil.ConvertToDateTime(dataRow[FieldCreateOn]);
			entity.CreateUserId = CommonUtil.ConvertToString(dataRow[FieldCreateUserId]);
			entity.CreateBy = CommonUtil.ConvertToString(dataRow[FieldCreateBy]);
			entity.ModifiedOn = CommonUtil.ConvertToDateTime(dataRow[FieldModifiedOn]);
			entity.ModifiedUserId = CommonUtil.ConvertToString(dataRow[FieldModifiedUserId]);
			entity.ModifiedBy = CommonUtil.ConvertToString(dataRow[FieldModifiedBy]);
            return entity;
		}
		
		
   		public UserEntity GetFrom(System.Data.IDataReader dataReader)
        {
        	UserEntity entity = new UserEntity();
	   		entity.ID = CommonUtil.ConvertToInt(dataReader[this.PrimaryKey]);
				entity.UserName = CommonUtil.ConvertToString(dataReader[FieldUserName]);
			entity.Password = CommonUtil.ConvertToString(dataReader[FieldPassword]);
			entity.OpenID = CommonUtil.ConvertToInt(dataReader[FieldOpenID]);
			entity.IsLogin = CommonUtil.ConvertToInt(dataReader[FieldIsLogin]);
			entity.IsEnable = CommonUtil.ConvertToInt(dataReader[FieldIsEnable]);
			entity.Description = CommonUtil.ConvertToString(dataReader[FieldDescription]);
			entity.DeletionStateCode = CommonUtil.ConvertToInt(dataReader[FieldDeletionStateCode]);
			entity.CreateOn = CommonUtil.ConvertToDateTime(dataReader[FieldCreateOn]);
			entity.CreateUserId = CommonUtil.ConvertToString(dataReader[FieldCreateUserId]);
			entity.CreateBy = CommonUtil.ConvertToString(dataReader[FieldCreateBy]);
			entity.ModifiedOn = CommonUtil.ConvertToDateTime(dataReader[FieldModifiedOn]);
			entity.ModifiedUserId = CommonUtil.ConvertToString(dataReader[FieldModifiedUserId]);
			entity.ModifiedBy = CommonUtil.ConvertToString(dataReader[FieldModifiedBy]);
            return entity;
		}
		
		public void SetEntity(NonQueryBuilder sqlBuilder, UserEntity entity)
        {
	   		sqlBuilder.SetValue(FieldUserName, entity.UserName);
			sqlBuilder.SetValue(FieldPassword, entity.Password);
			sqlBuilder.SetValue(FieldOpenID, entity.OpenID);
			sqlBuilder.SetValue(FieldIsLogin, entity.IsLogin);
			sqlBuilder.SetValue(FieldIsEnable, entity.IsEnable);
			sqlBuilder.SetValue(FieldDescription, entity.Description);
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