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
	//P_UserRole_Rel
	public class UserRoleEntity:BaseEntity,IEntity<UserRoleEntity>
	{
		/// <summary>
		/// TableName
		/// </summary>
		public string TableName
        {
            get { return "[P_UserRole_Rel]"; }
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
		/// RoleID
        /// </summary>		
		private int? _roleid;
        public int? RoleID
        {
            get{ return _roleid; }
            set{ _roleid = value; }
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
		/// FiledName:RoleID
        /// </summary>		
        public static string FieldRoleID
        {
            get{ return "RoleID"; }
        }        
		/// <summary>
		/// FiledName:UserID
        /// </summary>		
        public static string FieldUserID
        {
            get{ return "UserID"; }
        }        
		   		#endregion
   		
   		public UserRoleEntity GetFrom(System.Data.DataRow dataRow)
        {
        	UserRoleEntity entity = new UserRoleEntity();
	   		entity.ID = CommonUtil.ConvertToInt(dataRow[this.PrimaryKey]);
				entity.RoleID = CommonUtil.ConvertToInt(dataRow[FieldRoleID]);
			entity.UserID = CommonUtil.ConvertToInt(dataRow[FieldUserID]);
						return this;
		}
		
		
   		public UserRoleEntity GetFrom(System.Data.IDataReader dataReader)
        {
        	UserRoleEntity entity = new UserRoleEntity();
	   		entity.ID = CommonUtil.ConvertToInt(dataReader[this.PrimaryKey]);
				entity.RoleID = CommonUtil.ConvertToInt(dataReader[FieldRoleID]);
			entity.UserID = CommonUtil.ConvertToInt(dataReader[FieldUserID]);
						return this;
		}
		
		public void SetEntity(NonQueryBuilder sqlBuilder, UserRoleEntity entity)
        {
	   		sqlBuilder.SetValue(FieldRoleID, entity.RoleID);
			sqlBuilder.SetValue(FieldUserID, entity.UserID);
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