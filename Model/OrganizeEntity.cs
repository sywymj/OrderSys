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
        /// OrganizeCategoryID
        /// </summary>		
        private string _organizecategoryid;
        public string OrganizeCategoryID
        {
            get { return _organizecategoryid; }
            set { _organizecategoryid = value; }
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
        /// ShortName
        /// </summary>		
        private string _shortname;
        public string ShortName
        {
            get { return _shortname; }
            set { _shortname = value; }
        }
        /// <summary>
        /// OuterPhone
        /// </summary>		
        private string _outerphone;
        public string OuterPhone
        {
            get { return _outerphone; }
            set { _outerphone = value; }
        }
        /// <summary>
        /// InnerPhone
        /// </summary>		
        private string _innerphone;
        public string InnerPhone
        {
            get { return _innerphone; }
            set { _innerphone = value; }
        }
        /// <summary>
        /// Fax
        /// </summary>		
        private string _fax;
        public string Fax
        {
            get { return _fax; }
            set { _fax = value; }
        }
        /// <summary>
        /// Email
        /// </summary>		
        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        /// <summary>
        /// Postalcode
        /// </summary>		
        private string _postalcode;
        public string Postalcode
        {
            get { return _postalcode; }
            set { _postalcode = value; }
        }
        /// <summary>
        /// Address
        /// </summary>		
        private string _address;
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }
        /// <summary>
        /// Web
        /// </summary>		
        private string _web;
        public string Web
        {
            get { return _web; }
            set { _web = value; }
        }
        /// <summary>
        /// Layer
        /// </summary>		
        private int? _layer;
        public int? Layer
        {
            get { return _layer; }
            set { _layer = value; }
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
        /// IsVisible
        /// </summary>		
        private int? _isvisible;
        public int? IsVisible
        {
            get { return _isvisible; }
            set { _isvisible = value; }
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
        /// FiledName:OrganizeCategoryID
        /// </summary>		
        public static string FieldOrganizeCategoryID
        {
            get { return "OrganizeCategoryID"; }
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
        /// FiledName:ShortName
        /// </summary>		
        public static string FieldShortName
        {
            get { return "ShortName"; }
        }
        /// <summary>
        /// FiledName:OuterPhone
        /// </summary>		
        public static string FieldOuterPhone
        {
            get { return "OuterPhone"; }
        }
        /// <summary>
        /// FiledName:InnerPhone
        /// </summary>		
        public static string FieldInnerPhone
        {
            get { return "InnerPhone"; }
        }
        /// <summary>
        /// FiledName:Fax
        /// </summary>		
        public static string FieldFax
        {
            get { return "Fax"; }
        }
        /// <summary>
        /// FiledName:Email
        /// </summary>		
        public static string FieldEmail
        {
            get { return "Email"; }
        }
        /// <summary>
        /// FiledName:Postalcode
        /// </summary>		
        public static string FieldPostalcode
        {
            get { return "Postalcode"; }
        }
        /// <summary>
        /// FiledName:Address
        /// </summary>		
        public static string FieldAddress
        {
            get { return "Address"; }
        }
        /// <summary>
        /// FiledName:Web
        /// </summary>		
        public static string FieldWeb
        {
            get { return "Web"; }
        }
        /// <summary>
        /// FiledName:Layer
        /// </summary>		
        public static string FieldLayer
        {
            get { return "Layer"; }
        }
        /// <summary>
        /// FiledName:SortCode
        /// </summary>		
        public static string FieldSortCode
        {
            get { return "SortCode"; }
        }
        /// <summary>
        /// FiledName:IsVisible
        /// </summary>		
        public static string FieldIsVisible
        {
            get { return "IsVisible"; }
        }
        /// <summary>
        /// FiledName:Description
        /// </summary>		
        public static string FieldDescription
        {
            get { return "Description"; }
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

        public OrganizeEntity GetFrom(System.Data.DataRow dataRow)
        {
            OrganizeEntity entity = new OrganizeEntity();
            entity.ID = CommonUtil.ConvertToInt(dataRow[this.PrimaryKey]);
            entity.ParentID = CommonUtil.ConvertToInt(dataRow[FieldParentID]);
            entity.OrganizeCategoryID = CommonUtil.ConvertToString(dataRow[FieldOrganizeCategoryID]);
            entity.Code = CommonUtil.ConvertToString(dataRow[FieldCode]);
            entity.FullName = CommonUtil.ConvertToString(dataRow[FieldFullName]);
            entity.ShortName = CommonUtil.ConvertToString(dataRow[FieldShortName]);
            entity.OuterPhone = CommonUtil.ConvertToString(dataRow[FieldOuterPhone]);
            entity.InnerPhone = CommonUtil.ConvertToString(dataRow[FieldInnerPhone]);
            entity.Fax = CommonUtil.ConvertToString(dataRow[FieldFax]);
            entity.Email = CommonUtil.ConvertToString(dataRow[FieldEmail]);
            entity.Postalcode = CommonUtil.ConvertToString(dataRow[FieldPostalcode]);
            entity.Address = CommonUtil.ConvertToString(dataRow[FieldAddress]);
            entity.Web = CommonUtil.ConvertToString(dataRow[FieldWeb]);
            entity.Layer = CommonUtil.ConvertToInt(dataRow[FieldLayer]);
            entity.SortCode = CommonUtil.ConvertToInt(dataRow[FieldSortCode]);
            entity.IsVisible = CommonUtil.ConvertToInt(dataRow[FieldIsVisible]);
            entity.Description = CommonUtil.ConvertToString(dataRow[FieldDescription]);
            entity.CreateOn = CommonUtil.ConvertToDateTime(dataRow[FieldCreateOn]);
            entity.CreateUserId = CommonUtil.ConvertToString(dataRow[FieldCreateUserId]);
            entity.CreateBy = CommonUtil.ConvertToString(dataRow[FieldCreateBy]);
            entity.ModifiedOn = CommonUtil.ConvertToDateTime(dataRow[FieldModifiedOn]);
            entity.ModifiedUserId = CommonUtil.ConvertToString(dataRow[FieldModifiedUserId]);
            entity.ModifiedBy = CommonUtil.ConvertToString(dataRow[FieldModifiedBy]);
            return entity;
        }


        public OrganizeEntity GetFrom(System.Data.IDataReader dataReader)
        {
            OrganizeEntity entity = new OrganizeEntity();
            entity.ID = CommonUtil.ConvertToInt(dataReader[this.PrimaryKey]);
            entity.ParentID = CommonUtil.ConvertToInt(dataReader[FieldParentID]);
            entity.OrganizeCategoryID = CommonUtil.ConvertToString(dataReader[FieldOrganizeCategoryID]);
            entity.Code = CommonUtil.ConvertToString(dataReader[FieldCode]);
            entity.FullName = CommonUtil.ConvertToString(dataReader[FieldFullName]);
            entity.ShortName = CommonUtil.ConvertToString(dataReader[FieldShortName]);
            entity.OuterPhone = CommonUtil.ConvertToString(dataReader[FieldOuterPhone]);
            entity.InnerPhone = CommonUtil.ConvertToString(dataReader[FieldInnerPhone]);
            entity.Fax = CommonUtil.ConvertToString(dataReader[FieldFax]);
            entity.Email = CommonUtil.ConvertToString(dataReader[FieldEmail]);
            entity.Postalcode = CommonUtil.ConvertToString(dataReader[FieldPostalcode]);
            entity.Address = CommonUtil.ConvertToString(dataReader[FieldAddress]);
            entity.Web = CommonUtil.ConvertToString(dataReader[FieldWeb]);
            entity.Layer = CommonUtil.ConvertToInt(dataReader[FieldLayer]);
            entity.SortCode = CommonUtil.ConvertToInt(dataReader[FieldSortCode]);
            entity.IsVisible = CommonUtil.ConvertToInt(dataReader[FieldIsVisible]);
            entity.Description = CommonUtil.ConvertToString(dataReader[FieldDescription]);
            entity.CreateOn = CommonUtil.ConvertToDateTime(dataReader[FieldCreateOn]);
            entity.CreateUserId = CommonUtil.ConvertToString(dataReader[FieldCreateUserId]);
            entity.CreateBy = CommonUtil.ConvertToString(dataReader[FieldCreateBy]);
            entity.ModifiedOn = CommonUtil.ConvertToDateTime(dataReader[FieldModifiedOn]);
            entity.ModifiedUserId = CommonUtil.ConvertToString(dataReader[FieldModifiedUserId]);
            entity.ModifiedBy = CommonUtil.ConvertToString(dataReader[FieldModifiedBy]);
            return entity;
        }

        public void SetEntity(NonQueryBuilder sqlBuilder, OrganizeEntity entity)
        {
            sqlBuilder.SetValue(FieldParentID, entity.ParentID);
            sqlBuilder.SetValue(FieldOrganizeCategoryID, entity.OrganizeCategoryID);
            sqlBuilder.SetValue(FieldCode, entity.Code);
            sqlBuilder.SetValue(FieldFullName, entity.FullName);
            sqlBuilder.SetValue(FieldShortName, entity.ShortName);
            sqlBuilder.SetValue(FieldOuterPhone, entity.OuterPhone);
            sqlBuilder.SetValue(FieldInnerPhone, entity.InnerPhone);
            sqlBuilder.SetValue(FieldFax, entity.Fax);
            sqlBuilder.SetValue(FieldEmail, entity.Email);
            sqlBuilder.SetValue(FieldPostalcode, entity.Postalcode);
            sqlBuilder.SetValue(FieldAddress, entity.Address);
            sqlBuilder.SetValue(FieldWeb, entity.Web);
            sqlBuilder.SetValue(FieldLayer, entity.Layer);
            sqlBuilder.SetValue(FieldSortCode, entity.SortCode);
            sqlBuilder.SetValue(FieldIsVisible, entity.IsVisible);
            sqlBuilder.SetValue(FieldDescription, entity.Description);
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