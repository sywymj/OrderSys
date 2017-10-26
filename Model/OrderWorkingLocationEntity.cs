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
    //O_OrderWorkingLocation
    public class OrderWorkingLocationEntity : BaseEntity, IEntity<OrderWorkingLocationEntity>
    {
        /// <summary>
        /// TableName
        /// </summary>
        public string TableName
        {
            get { return "[O_OrderWorkingLocation]"; }
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
        /// OrganizeID
        /// </summary>		
        private int? _organizeid;
        public int? OrganizeID
        {
            get { return _organizeid; }
            set { _organizeid = value; }
        }
        /// <summary>
        /// FirstLevel
        /// </summary>		
        private string _firstlevel;
        public string FirstLevel
        {
            get { return _firstlevel; }
            set { _firstlevel = value; }
        }
        /// <summary>
        /// ScecondLevel
        /// </summary>		
        private string _scecondlevel;
        public string ScecondLevel
        {
            get { return _scecondlevel; }
            set { _scecondlevel = value; }
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
        /// FiledName:OrganizeID
        /// </summary>		
        public static string FieldOrganizeID
        {
            get { return "OrganizeID"; }
        }
        /// <summary>
        /// FiledName:FirstLevel
        /// </summary>		
        public static string FieldFirstLevel
        {
            get { return "FirstLevel"; }
        }
        /// <summary>
        /// FiledName:ScecondLevel
        /// </summary>		
        public static string FieldScecondLevel
        {
            get { return "ScecondLevel"; }
        }
        #endregion

        public OrderWorkingLocationEntity GetFrom(System.Data.DataRow dataRow)
        {
            OrderWorkingLocationEntity entity = new OrderWorkingLocationEntity();
            entity.ID = CommonUtil.ConvertToInt(dataRow[this.PrimaryKey]);
            entity.OrganizeID = CommonUtil.ConvertToInt(dataRow[FieldOrganizeID]);
            entity.FirstLevel = CommonUtil.ConvertToString(dataRow[FieldFirstLevel]);
            entity.ScecondLevel = CommonUtil.ConvertToString(dataRow[FieldScecondLevel]);
            return entity;
        }


        public OrderWorkingLocationEntity GetFrom(System.Data.IDataReader dataReader)
        {
            OrderWorkingLocationEntity entity = new OrderWorkingLocationEntity();
            entity.ID = CommonUtil.ConvertToInt(dataReader[this.PrimaryKey]);
            entity.OrganizeID = CommonUtil.ConvertToInt(dataReader[FieldOrganizeID]);
            entity.FirstLevel = CommonUtil.ConvertToString(dataReader[FieldFirstLevel]);
            entity.ScecondLevel = CommonUtil.ConvertToString(dataReader[FieldScecondLevel]);
            return entity;
        }

        public void SetEntity(NonQueryBuilder sqlBuilder, OrderWorkingLocationEntity entity)
        {
            sqlBuilder.SetValue(FieldOrganizeID, entity.OrganizeID);
            sqlBuilder.SetValue(FieldFirstLevel, entity.FirstLevel);
            sqlBuilder.SetValue(FieldScecondLevel, entity.ScecondLevel);
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