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
	//P_Module
	public class ModuleEntity:BaseEntity,IEntity<ModuleEntity>
	{
		/// <summary>
		/// TableName
		/// </summary>
		public string TableName
        {
            get { return "[P_Module]"; }
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
		/// ParentID
        /// </summary>		
		private int? _parentid;
        public int? ParentID
        {
            get{ return _parentid; }
            set{ _parentid = value; }
        }        
		/// <summary>
		/// Title
        /// </summary>		
		private string _title;
        public string Title
        {
            get{ return _title; }
            set{ _title = value; }
        }        
		/// <summary>
		/// Controller
        /// </summary>		
		private string _controller;
        public string Controller
        {
            get{ return _controller; }
            set{ _controller = value; }
        }        
		/// <summary>
		/// Action
        /// </summary>		
		private string _action;
        public string Action
        {
            get{ return _action; }
            set{ _action = value; }
        }        
		/// <summary>
		/// JSFunName
        /// </summary>		
		private string _jsfunname;
        public string JSFunName
        {
            get{ return _jsfunname; }
            set{ _jsfunname = value; }
        }        
		/// <summary>
		/// Type
        /// </summary>		
		private int? _type;
        public int? Type
        {
            get{ return _type; }
            set{ _type = value; }
        }        
		/// <summary>
		/// Sort
        /// </summary>		
		private int? _sort;
        public int? Sort
        {
            get{ return _sort; }
            set{ _sort = value; }
        }        
		/// <summary>
		/// Remark
        /// </summary>		
		private string _remark;
        public string Remark
        {
            get{ return _remark; }
            set{ _remark = value; }
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
		/// FiledName:ParentID
        /// </summary>		
        public string FieldParentID
        {
            get{ return "ParentID"; }
        }        
		/// <summary>
		/// FiledName:Title
        /// </summary>		
        public string FieldTitle
        {
            get{ return "Title"; }
        }        
		/// <summary>
		/// FiledName:Controller
        /// </summary>		
        public string FieldController
        {
            get{ return "Controller"; }
        }        
		/// <summary>
		/// FiledName:Action
        /// </summary>		
        public string FieldAction
        {
            get{ return "Action"; }
        }        
		/// <summary>
		/// FiledName:JSFunName
        /// </summary>		
        public string FieldJSFunName
        {
            get{ return "JSFunName"; }
        }        
		/// <summary>
		/// FiledName:Type
        /// </summary>		
        public string FieldType
        {
            get{ return "Type"; }
        }        
		/// <summary>
		/// FiledName:Sort
        /// </summary>		
        public string FieldSort
        {
            get{ return "Sort"; }
        }        
		/// <summary>
		/// FiledName:Remark
        /// </summary>		
        public string FieldRemark
        {
            get{ return "Remark"; }
        }        
		   		#endregion
   		
   		public ModuleEntity GetFrom(System.Data.DataRow dataRow)
        {
	   		this.ID = CommonUtil.ConvertToInt(dataRow[this.PrimaryKey]);
				this.ParentID = CommonUtil.ConvertToInt(dataRow[this.FieldParentID]);
			this.Title = CommonUtil.ConvertToString(dataRow[this.FieldTitle]);
			this.Controller = CommonUtil.ConvertToString(dataRow[this.FieldController]);
			this.Action = CommonUtil.ConvertToString(dataRow[this.FieldAction]);
			this.JSFunName = CommonUtil.ConvertToString(dataRow[this.FieldJSFunName]);
			this.Type = CommonUtil.ConvertToInt(dataRow[this.FieldType]);
			this.Sort = CommonUtil.ConvertToInt(dataRow[this.FieldSort]);
			this.Remark = CommonUtil.ConvertToString(dataRow[this.FieldRemark]);
						return this;
		}
		
		
   		public ModuleEntity GetFrom(System.Data.IDataReader dataReader)
        {
	   		this.ID = CommonUtil.ConvertToInt(dataReader[this.PrimaryKey]);
				this.ParentID = CommonUtil.ConvertToInt(dataReader[this.FieldParentID]);
			this.Title = CommonUtil.ConvertToString(dataReader[this.FieldTitle]);
			this.Controller = CommonUtil.ConvertToString(dataReader[this.FieldController]);
			this.Action = CommonUtil.ConvertToString(dataReader[this.FieldAction]);
			this.JSFunName = CommonUtil.ConvertToString(dataReader[this.FieldJSFunName]);
			this.Type = CommonUtil.ConvertToInt(dataReader[this.FieldType]);
			this.Sort = CommonUtil.ConvertToInt(dataReader[this.FieldSort]);
			this.Remark = CommonUtil.ConvertToString(dataReader[this.FieldRemark]);
						return this;
		}
		
		public void SetEntity(NonQueryBuilder sqlBuilder, ModuleEntity entity)
        {
	   		sqlBuilder.SetValue(this.FieldParentID, entity.ParentID);
			sqlBuilder.SetValue(this.FieldTitle, entity.Title);
			sqlBuilder.SetValue(this.FieldController, entity.Controller);
			sqlBuilder.SetValue(this.FieldAction, entity.Action);
			sqlBuilder.SetValue(this.FieldJSFunName, entity.JSFunName);
			sqlBuilder.SetValue(this.FieldType, entity.Type);
			sqlBuilder.SetValue(this.FieldSort, entity.Sort);
			sqlBuilder.SetValue(this.FieldRemark, entity.Remark);
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