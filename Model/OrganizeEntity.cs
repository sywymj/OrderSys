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
	//P_Organize
    public class OrganizeEntity : BaseEntity, IEntity<OrganizeEntity>
    {
        /// <summary>
        /// TableName
        /// </summary>
        public string TableName
        {
            get { return "[P_Organize]"; }
        }

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
        /// OrganizeName
        /// </summary>		
        private string _organizename;
        public string OrganizeName
        {
            get { return _organizename; }
            set { _organizename = value; }
        }
        /// <summary>
        /// OrganizeCode
        /// </summary>		
        private string _organizecode;
        public string OrganizeCode
        {
            get { return _organizecode; }
            set { _organizecode = value; }
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
        #endregion

        #region  数据库字段名称
        /// <summary>
        /// FiledName:ID
        /// </summary>		
        public string FieldID
        {
            get { return "ID"; }
        }
        /// <summary>
        /// FiledName:ParentID
        /// </summary>		
        public string FieldParentID
        {
            get { return "ParentID"; }
        }
        /// <summary>
        /// FiledName:OrganizeName
        /// </summary>		
        public string FieldOrganizeName
        {
            get { return "OrganizeName"; }
        }
        /// <summary>
        /// FiledName:OrganizeCode
        /// </summary>		
        public string FieldOrganizeCode
        {
            get { return "OrganizeCode"; }
        }
        /// <summary>
        /// FiledName:IsEnable
        /// </summary>		
        public string FieldIsEnable
        {
            get { return "IsEnable"; }
        }
        #endregion

        public OrganizeEntity GetFrom(System.Data.DataRow dataRow)
        {
            this.ID = CommonUtil.ConvertToInt(dataRow[this.PrimaryKey]);
            this.ParentID = CommonUtil.ConvertToInt(dataRow[this.FieldParentID]);
            this.OrganizeName = CommonUtil.ConvertToString(dataRow[this.FieldOrganizeName]);
            this.OrganizeCode = CommonUtil.ConvertToString(dataRow[this.FieldOrganizeCode]);
            this.IsEnable = CommonUtil.ConvertToInt(dataRow[this.FieldIsEnable]);
            return this;
        }


        public OrganizeEntity GetFrom(System.Data.IDataReader dataReader)
        {
            this.ID = CommonUtil.ConvertToInt(dataReader[this.PrimaryKey]);
            this.ParentID = CommonUtil.ConvertToInt(dataReader[this.FieldParentID]);
            this.OrganizeName = CommonUtil.ConvertToString(dataReader[this.FieldOrganizeName]);
            this.OrganizeCode = CommonUtil.ConvertToString(dataReader[this.FieldOrganizeCode]);
            this.IsEnable = CommonUtil.ConvertToInt(dataReader[this.FieldIsEnable]);
            return this;
        }

        public void SetEntity(NonQueryBuilder sqlBuilder, OrganizeEntity entity)
        {
            sqlBuilder.SetValue(this.FieldParentID, entity.ParentID);
            sqlBuilder.SetValue(this.FieldOrganizeName, entity.OrganizeName);
            sqlBuilder.SetValue(this.FieldOrganizeCode, entity.OrganizeCode);
            sqlBuilder.SetValue(this.FieldIsEnable, entity.IsEnable);
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