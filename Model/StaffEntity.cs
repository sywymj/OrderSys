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
		/// Add
        /// </summary>		
		private string _add;
        public string Add
        {
            get{ return _add; }
            set{ _add = value; }
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
				#endregion

		#region  数据库字段名称
      	/// <summary>
		/// FiledName:ID
        /// </summary>		
        public string FieldID
        {
            get{ return "ID"; }
        }        
		/// <summary>
		/// FiledName:UserID
        /// </summary>		
        public string FieldUserID
        {
            get{ return "UserID"; }
        }        
		/// <summary>
		/// FiledName:Name
        /// </summary>		
        public string FieldName
        {
            get{ return "Name"; }
        }        
		/// <summary>
		/// FiledName:Tel
        /// </summary>		
        public string FieldTel
        {
            get{ return "Tel"; }
        }        
		/// <summary>
		/// FiledName:Add
        /// </summary>		
        public string FieldAdd
        {
            get{ return "Add"; }
        }        
		/// <summary>
		/// FiledName:IsEnable
        /// </summary>		
        public string FieldIsEnable
        {
            get{ return "IsEnable"; }
        }        
		/// <summary>
		/// FiledName:IsOnJob
        /// </summary>		
        public string FieldIsOnJob
        {
            get{ return "IsOnJob"; }
        }        
		/// <summary>
		/// FiledName:Sex
        /// </summary>		
        public string FieldSex
        {
            get{ return "Sex"; }
        }        
		   		#endregion
   		
   		public StaffEntity GetFrom(System.Data.DataRow dataRow)
        {
	   		this.ID = CommonUtil.ConvertToInt(dataRow[this.PrimaryKey]);
				this.UserID = CommonUtil.ConvertToInt(dataRow[this.FieldUserID]);
			this.Name = CommonUtil.ConvertToString(dataRow[this.FieldName]);
			this.Tel = CommonUtil.ConvertToString(dataRow[this.FieldTel]);
			this.Add = CommonUtil.ConvertToString(dataRow[this.FieldAdd]);
			this.IsEnable = CommonUtil.ConvertToInt(dataRow[this.FieldIsEnable]);
			this.IsOnJob = CommonUtil.ConvertToInt(dataRow[this.FieldIsOnJob]);
			this.Sex = CommonUtil.ConvertToInt(dataRow[this.FieldSex]);
						return this;
		}
		
		
   		public StaffEntity GetFrom(System.Data.IDataReader dataReader)
        {
	   		this.ID = CommonUtil.ConvertToInt(dataReader[this.PrimaryKey]);
				this.UserID = CommonUtil.ConvertToInt(dataReader[this.FieldUserID]);
			this.Name = CommonUtil.ConvertToString(dataReader[this.FieldName]);
			this.Tel = CommonUtil.ConvertToString(dataReader[this.FieldTel]);
			this.Add = CommonUtil.ConvertToString(dataReader[this.FieldAdd]);
			this.IsEnable = CommonUtil.ConvertToInt(dataReader[this.FieldIsEnable]);
			this.IsOnJob = CommonUtil.ConvertToInt(dataReader[this.FieldIsOnJob]);
			this.Sex = CommonUtil.ConvertToInt(dataReader[this.FieldSex]);
						return this;
		}
		
		public void SetEntity(NonQueryBuilder sqlBuilder, StaffEntity entity)
        {
	   		sqlBuilder.SetValue(this.FieldUserID, entity.UserID);
			sqlBuilder.SetValue(this.FieldName, entity.Name);
			sqlBuilder.SetValue(this.FieldTel, entity.Tel);
			sqlBuilder.SetValue(this.FieldAdd, entity.Add);
			sqlBuilder.SetValue(this.FieldIsEnable, entity.IsEnable);
			sqlBuilder.SetValue(this.FieldIsOnJob, entity.IsOnJob);
			sqlBuilder.SetValue(this.FieldSex, entity.Sex);
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