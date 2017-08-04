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
        /// FiledName:OrganizeName
        /// </summary>		
        public static string FieldOrganizeName
        {
            get { return "OrganizeName"; }
        }
        /// <summary>
        /// FiledName:OrganizeCode
        /// </summary>		
        public static string FieldOrganizeCode
        {
            get { return "OrganizeCode"; }
        }
        /// <summary>
        /// FiledName:IsEnable
        /// </summary>		
        public static string FieldIsEnable
        {
            get { return "IsEnable"; }
        }
        #endregion

        public OrganizeEntity GetFrom(System.Data.DataRow dataRow)
        {
            this.ID = CommonUtil.ConvertToInt(dataRow[this.PrimaryKey]);
            this.ParentID = CommonUtil.ConvertToInt(dataRow[FieldParentID]);
            this.OrganizeName = CommonUtil.ConvertToString(dataRow[FieldOrganizeName]);
            this.OrganizeCode = CommonUtil.ConvertToString(dataRow[FieldOrganizeCode]);
            this.IsEnable = CommonUtil.ConvertToInt(dataRow[FieldIsEnable]);
            return this;
        }


        public OrganizeEntity GetFrom(System.Data.IDataReader dataReader)
        {
            this.ID = CommonUtil.ConvertToInt(dataReader[this.PrimaryKey]);
            this.ParentID = CommonUtil.ConvertToInt(dataReader[FieldParentID]);
            this.OrganizeName = CommonUtil.ConvertToString(dataReader[FieldOrganizeName]);
            this.OrganizeCode = CommonUtil.ConvertToString(dataReader[FieldOrganizeCode]);
            this.IsEnable = CommonUtil.ConvertToInt(dataReader[FieldIsEnable]);
            return this;
        }

        public void SetEntity(NonQueryBuilder sqlBuilder, OrganizeEntity entity)
        {
            sqlBuilder.SetValue(FieldParentID, entity.ParentID);
            sqlBuilder.SetValue(FieldOrganizeName, entity.OrganizeName);
            sqlBuilder.SetValue(FieldOrganizeCode, entity.OrganizeCode);
            sqlBuilder.SetValue(FieldIsEnable, entity.IsEnable);
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