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
    //O_OrderGoods
    public class OrderGoodsEntity : BaseEntity, IEntity<OrderGoodsEntity>
    {
        /// <summary>
        /// TableName
        /// </summary>
        public string TableName
        {
            get { return "[O_OrderGoods]"; }
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
        /// Name
        /// </summary>		
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
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
        /// FiledName:Name
        /// </summary>		
        public static string FieldName
        {
            get { return "Name"; }
        }
        #endregion

        public OrderGoodsEntity GetFrom(System.Data.DataRow dataRow)
        {
            OrderGoodsEntity entity = new OrderGoodsEntity();
            entity.ID = CommonUtil.ConvertToInt(dataRow[this.PrimaryKey]);
            entity.OrganizeID = CommonUtil.ConvertToInt(dataRow[FieldOrganizeID]);
            entity.Name = CommonUtil.ConvertToString(dataRow[FieldName]);
            return entity;
        }


        public OrderGoodsEntity GetFrom(System.Data.IDataReader dataReader)
        {
            OrderGoodsEntity entity = new OrderGoodsEntity();
            entity.ID = CommonUtil.ConvertToInt(dataReader[this.PrimaryKey]);
            entity.OrganizeID = CommonUtil.ConvertToInt(dataReader[FieldOrganizeID]);
            entity.Name = CommonUtil.ConvertToString(dataReader[FieldName]);
            return entity;
        }

        public void SetEntity(NonQueryBuilder sqlBuilder, OrderGoodsEntity entity)
        {
            sqlBuilder.SetValue(FieldOrganizeID, entity.OrganizeID);
            sqlBuilder.SetValue(FieldName, entity.Name);
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