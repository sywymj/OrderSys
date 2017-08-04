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
    public class ModuleEntity : BaseEntity, IEntity<ModuleEntity>
    {
        /// <summary>
        /// TableName
        /// </summary>
        public string TableName
        {
            get { return "[P_Module]"; }
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
        /// ParentID
        /// </summary>		
        private int? _parentid;
        public int? ParentID
        {
            get { return _parentid; }
            set { _parentid = value; }
        }
        /// <summary>
        /// Title
        /// </summary>		
        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        /// <summary>
        /// Controller
        /// </summary>		
        private string _controller;
        public string Controller
        {
            get { return _controller; }
            set { _controller = value; }
        }
        /// <summary>
        /// Action
        /// </summary>		
        private string _action;
        public string Action
        {
            get { return _action; }
            set { _action = value; }
        }
        /// <summary>
        /// JSFunName
        /// </summary>		
        private string _jsfunname;
        public string JSFunName
        {
            get { return _jsfunname; }
            set { _jsfunname = value; }
        }
        /// <summary>
        /// Type
        /// </summary>		
        private int? _type;
        public int? Type
        {
            get { return _type; }
            set { _type = value; }
        }
        /// <summary>
        /// Sort
        /// </summary>		
        private int? _sort;
        public int? Sort
        {
            get { return _sort; }
            set { _sort = value; }
        }
        /// <summary>
        /// Remark
        /// </summary>		
        private string _remark;
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
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
        /// FiledName:ParentID
        /// </summary>		
        public static string FieldParentID
        {
            get { return "ParentID"; }
        }
        /// <summary>
        /// FiledName:Title
        /// </summary>		
        public static string FieldTitle
        {
            get { return "Title"; }
        }
        /// <summary>
        /// FiledName:Controller
        /// </summary>		
        public static string FieldController
        {
            get { return "Controller"; }
        }
        /// <summary>
        /// FiledName:Action
        /// </summary>		
        public static string FieldAction
        {
            get { return "Action"; }
        }
        /// <summary>
        /// FiledName:JSFunName
        /// </summary>		
        public static string FieldJSFunName
        {
            get { return "JSFunName"; }
        }
        /// <summary>
        /// FiledName:Type
        /// </summary>		
        public static string FieldType
        {
            get { return "Type"; }
        }
        /// <summary>
        /// FiledName:Sort
        /// </summary>		
        public static string FieldSort
        {
            get { return "Sort"; }
        }
        /// <summary>
        /// FiledName:Remark
        /// </summary>		
        public static string FieldRemark
        {
            get { return "Remark"; }
        }
        #endregion

        public ModuleEntity GetFrom(System.Data.DataRow dataRow)
        {
            this.ID = CommonUtil.ConvertToInt(dataRow[this.PrimaryKey]);
            this.ParentID = CommonUtil.ConvertToInt(dataRow[FieldParentID]);
            this.Title = CommonUtil.ConvertToString(dataRow[FieldTitle]);
            this.Controller = CommonUtil.ConvertToString(dataRow[FieldController]);
            this.Action = CommonUtil.ConvertToString(dataRow[FieldAction]);
            this.JSFunName = CommonUtil.ConvertToString(dataRow[FieldJSFunName]);
            this.Type = CommonUtil.ConvertToInt(dataRow[FieldType]);
            this.Sort = CommonUtil.ConvertToInt(dataRow[FieldSort]);
            this.Remark = CommonUtil.ConvertToString(dataRow[FieldRemark]);
            return this;
        }


        public ModuleEntity GetFrom(System.Data.IDataReader dataReader)
        {
            this.ID = CommonUtil.ConvertToInt(dataReader[this.PrimaryKey]);
            this.ParentID = CommonUtil.ConvertToInt(dataReader[FieldParentID]);
            this.Title = CommonUtil.ConvertToString(dataReader[FieldTitle]);
            this.Controller = CommonUtil.ConvertToString(dataReader[FieldController]);
            this.Action = CommonUtil.ConvertToString(dataReader[FieldAction]);
            this.JSFunName = CommonUtil.ConvertToString(dataReader[FieldJSFunName]);
            this.Type = CommonUtil.ConvertToInt(dataReader[FieldType]);
            this.Sort = CommonUtil.ConvertToInt(dataReader[FieldSort]);
            this.Remark = CommonUtil.ConvertToString(dataReader[FieldRemark]);
            return this;
        }

        public void SetEntity(NonQueryBuilder sqlBuilder, ModuleEntity entity)
        {
            sqlBuilder.SetValue(FieldParentID, entity.ParentID);
            sqlBuilder.SetValue(FieldTitle, entity.Title);
            sqlBuilder.SetValue(FieldController, entity.Controller);
            sqlBuilder.SetValue(FieldAction, entity.Action);
            sqlBuilder.SetValue(FieldJSFunName, entity.JSFunName);
            sqlBuilder.SetValue(FieldType, entity.Type);
            sqlBuilder.SetValue(FieldSort, entity.Sort);
            sqlBuilder.SetValue(FieldRemark, entity.Remark);
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