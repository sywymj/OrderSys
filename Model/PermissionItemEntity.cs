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
    //P_PermissionItem
    public class PermissionItemEntity : BaseEntity, IEntity<PermissionItemEntity>
    {
        /// <summary>
        /// TableName
        /// </summary>
        public string TableName
        {
            get { return "[P_PermissionItem]"; }
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
        /// Code
        /// </summary>		
        private string _code;
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }
        /// <summary>
        /// FullName
        /// </summary>		
        private string _fullname;
        public string FullName
        {
            get { return _fullname; }
            set { _fullname = value; }
        }
        /// <summary>
        /// SysCategory
        /// </summary>		
        private string _syscategory;
        public string SysCategory
        {
            get { return _syscategory; }
            set { _syscategory = value; }
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
        /// ActionName
        /// </summary>		
        private string _actionname;
        public string ActionName
        {
            get { return _actionname; }
            set { _actionname = value; }
        }
        /// <summary>
        /// ActionParameter
        /// </summary>		
        private string _actionparameter;
        public string ActionParameter
        {
            get { return _actionparameter; }
            set { _actionparameter = value; }
        }
        /// <summary>
        /// AllowEdit
        /// </summary>		
        private int? _allowedit;
        public int? AllowEdit
        {
            get { return _allowedit; }
            set { _allowedit = value; }
        }
        /// <summary>
        /// AllowDelete
        /// </summary>		
        private int? _allowdelete;
        public int? AllowDelete
        {
            get { return _allowdelete; }
            set { _allowdelete = value; }
        }
        /// <summary>
        /// IsEnable
        /// </summary>		
        private int? _isenable;
        public int? IsEnable
        {
            get { return _isenable; }
            set { _isenable = value; }
        }
        /// <summary>
        /// IsPublic
        /// </summary>		
        private int? _ispublic;
        public int? IsPublic
        {
            get { return _ispublic; }
            set { _ispublic = value; }
        }
        /// <summary>
        /// Description
        /// </summary>		
        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        /// <summary>
        /// SortCode
        /// </summary>		
        private int? _sortcode;
        public int? SortCode
        {
            get { return _sortcode; }
            set { _sortcode = value; }
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
        /// FiledName:ParentID
        /// </summary>		
        public static string FieldParentID
        {
            get { return "ParentID"; }
        }
        /// <summary>
        /// FiledName:Code
        /// </summary>		
        public static string FieldCode
        {
            get { return "Code"; }
        }
        /// <summary>
        /// FiledName:FullName
        /// </summary>		
        public static string FieldFullName
        {
            get { return "FullName"; }
        }
        /// <summary>
        /// FiledName:SysCategory
        /// </summary>		
        public static string FieldSysCategory
        {
            get { return "SysCategory"; }
        }
        /// <summary>
        /// FiledName:Controller
        /// </summary>		
        public static string FieldController
        {
            get { return "Controller"; }
        }
        /// <summary>
        /// FiledName:ActionName
        /// </summary>		
        public static string FieldActionName
        {
            get { return "ActionName"; }
        }
        /// <summary>
        /// FiledName:ActionParameter
        /// </summary>		
        public static string FieldActionParameter
        {
            get { return "ActionParameter"; }
        }
        /// <summary>
        /// FiledName:AllowEdit
        /// </summary>		
        public static string FieldAllowEdit
        {
            get { return "AllowEdit"; }
        }
        /// <summary>
        /// FiledName:AllowDelete
        /// </summary>		
        public static string FieldAllowDelete
        {
            get { return "AllowDelete"; }
        }
        /// <summary>
        /// FiledName:IsEnable
        /// </summary>		
        public static string FieldIsEnable
        {
            get { return "IsEnable"; }
        }
        /// <summary>
        /// FiledName:IsPublic
        /// </summary>		
        public static string FieldIsPublic
        {
            get { return "IsPublic"; }
        }
        /// <summary>
        /// FiledName:Description
        /// </summary>		
        public static string FieldDescription
        {
            get { return "Description"; }
        }
        /// <summary>
        /// FiledName:SortCode
        /// </summary>		
        public static string FieldSortCode
        {
            get { return "SortCode"; }
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

        public PermissionItemEntity GetFrom(System.Data.DataRow dataRow)
        {
            PermissionItemEntity entity = new PermissionItemEntity();
            entity.ID = CommonUtil.ConvertToInt(dataRow[this.PrimaryKey]);
            entity.ParentID = CommonUtil.ConvertToInt(dataRow[FieldParentID]);
            entity.Code = CommonUtil.ConvertToString(dataRow[FieldCode]);
            entity.FullName = CommonUtil.ConvertToString(dataRow[FieldFullName]);
            entity.SysCategory = CommonUtil.ConvertToString(dataRow[FieldSysCategory]);
            entity.Controller = CommonUtil.ConvertToString(dataRow[FieldController]);
            entity.ActionName = CommonUtil.ConvertToString(dataRow[FieldActionName]);
            entity.ActionParameter = CommonUtil.ConvertToString(dataRow[FieldActionParameter]);
            entity.AllowEdit = CommonUtil.ConvertToInt(dataRow[FieldAllowEdit]);
            entity.AllowDelete = CommonUtil.ConvertToInt(dataRow[FieldAllowDelete]);
            entity.IsEnable = CommonUtil.ConvertToInt(dataRow[FieldIsEnable]);
            entity.IsPublic = CommonUtil.ConvertToInt(dataRow[FieldIsPublic]);
            entity.Description = CommonUtil.ConvertToString(dataRow[FieldDescription]);
            entity.SortCode = CommonUtil.ConvertToInt(dataRow[FieldSortCode]);
            entity.DeletionStateCode = CommonUtil.ConvertToInt(dataRow[FieldDeletionStateCode]);
            entity.CreateOn = CommonUtil.ConvertToDateTime(dataRow[FieldCreateOn]);
            entity.CreateUserId = CommonUtil.ConvertToString(dataRow[FieldCreateUserId]);
            entity.CreateBy = CommonUtil.ConvertToString(dataRow[FieldCreateBy]);
            entity.ModifiedOn = CommonUtil.ConvertToDateTime(dataRow[FieldModifiedOn]);
            entity.ModifiedUserId = CommonUtil.ConvertToString(dataRow[FieldModifiedUserId]);
            entity.ModifiedBy = CommonUtil.ConvertToString(dataRow[FieldModifiedBy]);
            return entity;
        }


        public PermissionItemEntity GetFrom(System.Data.IDataReader dataReader)
        {
            PermissionItemEntity entity = new PermissionItemEntity();
            entity.ID = CommonUtil.ConvertToInt(dataReader[this.PrimaryKey]);
            entity.ParentID = CommonUtil.ConvertToInt(dataReader[FieldParentID]);
            entity.Code = CommonUtil.ConvertToString(dataReader[FieldCode]);
            entity.FullName = CommonUtil.ConvertToString(dataReader[FieldFullName]);
            entity.SysCategory = CommonUtil.ConvertToString(dataReader[FieldSysCategory]);
            entity.Controller = CommonUtil.ConvertToString(dataReader[FieldController]);
            entity.ActionName = CommonUtil.ConvertToString(dataReader[FieldActionName]);
            entity.ActionParameter = CommonUtil.ConvertToString(dataReader[FieldActionParameter]);
            entity.AllowEdit = CommonUtil.ConvertToInt(dataReader[FieldAllowEdit]);
            entity.AllowDelete = CommonUtil.ConvertToInt(dataReader[FieldAllowDelete]);
            entity.IsEnable = CommonUtil.ConvertToInt(dataReader[FieldIsEnable]);
            entity.IsPublic = CommonUtil.ConvertToInt(dataReader[FieldIsPublic]);
            entity.Description = CommonUtil.ConvertToString(dataReader[FieldDescription]);
            entity.SortCode = CommonUtil.ConvertToInt(dataReader[FieldSortCode]);
            entity.DeletionStateCode = CommonUtil.ConvertToInt(dataReader[FieldDeletionStateCode]);
            entity.CreateOn = CommonUtil.ConvertToDateTime(dataReader[FieldCreateOn]);
            entity.CreateUserId = CommonUtil.ConvertToString(dataReader[FieldCreateUserId]);
            entity.CreateBy = CommonUtil.ConvertToString(dataReader[FieldCreateBy]);
            entity.ModifiedOn = CommonUtil.ConvertToDateTime(dataReader[FieldModifiedOn]);
            entity.ModifiedUserId = CommonUtil.ConvertToString(dataReader[FieldModifiedUserId]);
            entity.ModifiedBy = CommonUtil.ConvertToString(dataReader[FieldModifiedBy]);
            return entity;
        }

        public void SetEntity(NonQueryBuilder sqlBuilder, PermissionItemEntity entity)
        {
            sqlBuilder.SetValue(FieldParentID, entity.ParentID);
            sqlBuilder.SetValue(FieldCode, entity.Code);
            sqlBuilder.SetValue(FieldFullName, entity.FullName);
            sqlBuilder.SetValue(FieldSysCategory, entity.SysCategory);
            sqlBuilder.SetValue(FieldController, entity.Controller);
            sqlBuilder.SetValue(FieldActionName, entity.ActionName);
            sqlBuilder.SetValue(FieldActionParameter, entity.ActionParameter);
            sqlBuilder.SetValue(FieldAllowEdit, entity.AllowEdit);
            sqlBuilder.SetValue(FieldAllowDelete, entity.AllowDelete);
            sqlBuilder.SetValue(FieldIsEnable, entity.IsEnable);
            sqlBuilder.SetValue(FieldIsPublic, entity.IsPublic);
            sqlBuilder.SetValue(FieldDescription, entity.Description);
            sqlBuilder.SetValue(FieldSortCode, entity.SortCode);
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