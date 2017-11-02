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
    //O_OrderGoods_Rel
    public class OrderGoodsRelEntity : BaseEntity, IEntity<OrderGoodsRelEntity>
    {
        /// <summary>
        /// TableName
        /// </summary>
        public string TableName
        {
            get { return "[O_OrderGoods_Rel]"; }
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
        /// OrderID
        /// </summary>		
        private Guid? _orderid;
        public Guid? OrderID
        {
            get { return _orderid; }
            set { _orderid = value; }
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
        /// IsFee
        /// </summary>		
        private int? _isfee;
        public int? IsFee
        {
            get { return _isfee; }
            set { _isfee = value; }
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
        /// FiledName:OrderID
        /// </summary>		
        public static string FieldOrderID
        {
            get { return "OrderID"; }
        }
        /// <summary>
        /// FiledName:Name
        /// </summary>		
        public static string FieldName
        {
            get { return "Name"; }
        }
        /// <summary>
        /// FiledName:IsFee
        /// </summary>		
        public static string FieldIsFee
        {
            get { return "IsFee"; }
        }
        #endregion

        public OrderGoodsRelEntity GetFrom(System.Data.DataRow dataRow)
        {
            OrderGoodsRelEntity entity = new OrderGoodsRelEntity();
            entity.ID = CommonUtil.ConvertToInt(dataRow[this.PrimaryKey]);
            entity.OrderID = CommonUtil.ConvertToGuid(dataRow[FieldOrderID]);
            entity.Name = CommonUtil.ConvertToString(dataRow[FieldName]);
            entity.IsFee = CommonUtil.ConvertToInt(dataRow[FieldIsFee]);
            return entity;
        }


        public OrderGoodsRelEntity GetFrom(System.Data.IDataReader dataReader)
        {
            OrderGoodsRelEntity entity = new OrderGoodsRelEntity();
            entity.ID = CommonUtil.ConvertToInt(dataReader[this.PrimaryKey]);
            entity.OrderID = CommonUtil.ConvertToGuid(dataReader[FieldOrderID]);
            entity.Name = CommonUtil.ConvertToString(dataReader[FieldName]);
            entity.IsFee = CommonUtil.ConvertToInt(dataReader[FieldIsFee]);
            return entity;
        }

        public void SetEntity(NonQueryBuilder sqlBuilder, OrderGoodsRelEntity entity)
        {
            sqlBuilder.SetValue(FieldOrderID, entity.OrderID);
            sqlBuilder.SetValue(FieldName, entity.Name);
            sqlBuilder.SetValue(FieldIsFee, entity.IsFee);
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