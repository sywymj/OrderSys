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
    public class StaffEntity : BaseEntity, IEntity<StaffEntity>
    {
        /// <summary>
        /// TableName
        /// </summary>
        public string TableName
        {
            get { return "[P_Staff]"; }
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
        /// OpenID
        /// </summary>		
        private string _openid;
        public string OpenID
        {
            get { return _openid; }
            set { _openid = value; }
        }
        /// <summary>
        /// Name
        /// </summary>		
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// Tel
        /// </summary>		
        private string _tel;
        public string Tel
        {
            get { return _tel; }
            set { _tel = value; }
        }
        /// <summary>
        /// Addr
        /// </summary>		
        private string _addr;
        public string Addr
        {
            get { return _addr; }
            set { _addr = value; }
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
        /// IsOnJob
        /// </summary>		
        private int? _isonjob;
        public int? IsOnJob
        {
            get { return _isonjob; }
            set { _isonjob = value; }
        }
        /// <summary>
        /// Sex
        /// </summary>		
        private int? _sex;
        public int? Sex
        {
            get { return _sex; }
            set { _sex = value; }
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
        /// FiledName:OpenID
        /// </summary>		
        public static string FieldOpenID
        {
            get { return "OpenID"; }
        }
        /// <summary>
        /// FiledName:Name
        /// </summary>		
        public static string FieldName
        {
            get { return "Name"; }
        }
        /// <summary>
        /// FiledName:Tel
        /// </summary>		
        public static string FieldTel
        {
            get { return "Tel"; }
        }
        /// <summary>
        /// FiledName:Addr
        /// </summary>		
        public static string FieldAddr
        {
            get { return "Addr"; }
        }
        /// <summary>
        /// FiledName:IsEnable
        /// </summary>		
        public static string FieldIsEnable
        {
            get { return "IsEnable"; }
        }
        /// <summary>
        /// FiledName:IsOnJob
        /// </summary>		
        public static string FieldIsOnJob
        {
            get { return "IsOnJob"; }
        }
        /// <summary>
        /// FiledName:Sex
        /// </summary>		
        public static string FieldSex
        {
            get { return "Sex"; }
        }
        #endregion

        public StaffEntity GetFrom(System.Data.DataRow dataRow)
        {
            StaffEntity entity = new StaffEntity();
            entity.ID = CommonUtil.ConvertToInt(dataRow[entity.PrimaryKey]);
            entity.OpenID = CommonUtil.ConvertToString(dataRow[FieldOpenID]);
            entity.Name = CommonUtil.ConvertToString(dataRow[FieldName]);
            entity.Tel = CommonUtil.ConvertToString(dataRow[FieldTel]);
            entity.Addr = CommonUtil.ConvertToString(dataRow[FieldAddr]);
            entity.IsEnable = CommonUtil.ConvertToInt(dataRow[FieldIsEnable]);
            entity.IsOnJob = CommonUtil.ConvertToInt(dataRow[FieldIsOnJob]);
            entity.Sex = CommonUtil.ConvertToInt(dataRow[FieldSex]);
            return entity;
        }


        public StaffEntity GetFrom(System.Data.IDataReader dataReader)
        {
            StaffEntity entity = new StaffEntity();
            entity.ID = CommonUtil.ConvertToInt(dataReader[entity.PrimaryKey]);
            entity.OpenID = CommonUtil.ConvertToString(dataReader[FieldOpenID]);
            entity.Name = CommonUtil.ConvertToString(dataReader[FieldName]);
            entity.Tel = CommonUtil.ConvertToString(dataReader[FieldTel]);
            entity.Addr = CommonUtil.ConvertToString(dataReader[FieldAddr]);
            entity.IsEnable = CommonUtil.ConvertToInt(dataReader[FieldIsEnable]);
            entity.IsOnJob = CommonUtil.ConvertToInt(dataReader[FieldIsOnJob]);
            entity.Sex = CommonUtil.ConvertToInt(dataReader[FieldSex]);
            return entity;
        }

        public void SetEntity(NonQueryBuilder sqlBuilder, StaffEntity entity)
        {
            sqlBuilder.SetValue(FieldOpenID, entity.OpenID);
            sqlBuilder.SetValue(FieldName, entity.Name);
            sqlBuilder.SetValue(FieldTel, entity.Tel);
            sqlBuilder.SetValue(FieldAddr, entity.Addr);
            sqlBuilder.SetValue(FieldIsEnable, entity.IsEnable);
            sqlBuilder.SetValue(FieldIsOnJob, entity.IsOnJob);
            sqlBuilder.SetValue(FieldSex, entity.Sex);
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