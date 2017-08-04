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
    //P_Role
    public class RoleEntity : BaseEntity, IEntity<RoleEntity>
    {
        /// <summary>
        /// TableName
        /// </summary>
        public string TableName
        {
            get { return "[P_Role]"; }
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
        /// OrganizeID
        /// </summary>		
        private int? _organizeid;
        public int? OrganizeID
        {
            get { return _organizeid; }
            set { _organizeid = value; }
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
        /// FiledName:OrganizeID
        /// </summary>		
        public static string FieldOrganizeID
        {
            get { return "OrganizeID"; }
        }
        /// <summary>
        /// FiledName:Title
        /// </summary>		
        public static string FieldTitle
        {
            get { return "Title"; }
        }
        /// <summary>
        /// FiledName:Remark
        /// </summary>		
        public static string FieldRemark
        {
            get { return "Remark"; }
        }
        #endregion

        public RoleEntity GetFrom(System.Data.DataRow dataRow)
        {
            this.ID = CommonUtil.ConvertToInt(dataRow[this.PrimaryKey]);
            this.ParentID = CommonUtil.ConvertToInt(dataRow[FieldParentID]);
            this.OrganizeID = CommonUtil.ConvertToInt(dataRow[FieldOrganizeID]);
            this.Title = CommonUtil.ConvertToString(dataRow[FieldTitle]);
            this.Remark = CommonUtil.ConvertToString(dataRow[FieldRemark]);
            return this;
        }


        public RoleEntity GetFrom(System.Data.IDataReader dataReader)
        {
            this.ID = CommonUtil.ConvertToInt(dataReader[this.PrimaryKey]);
            this.ParentID = CommonUtil.ConvertToInt(dataReader[FieldParentID]);
            this.OrganizeID = CommonUtil.ConvertToInt(dataReader[FieldOrganizeID]);
            this.Title = CommonUtil.ConvertToString(dataReader[FieldTitle]);
            this.Remark = CommonUtil.ConvertToString(dataReader[FieldRemark]);
            return this;
        }

        public void SetEntity(NonQueryBuilder sqlBuilder, RoleEntity entity)
        {
            sqlBuilder.SetValue(FieldParentID, entity.ParentID);
            sqlBuilder.SetValue(FieldOrganizeID, entity.OrganizeID);
            sqlBuilder.SetValue(FieldTitle, entity.Title);
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