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
    //O_OrderHandler
    public class OrderHandlerEntity : BaseEntity, IEntity<OrderHandlerEntity>
    {
        /// <summary>
        /// TableName
        /// </summary>
        public string TableName
        {
            get { return "[O_OrderHandler]"; }
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
        /// HandlerID
        /// </summary>		
        private int? _handlerid;
        public int? HandlerID
        {
            get { return _handlerid; }
            set { _handlerid = value; }
        }
        /// <summary>
        /// Workload
        /// </summary>		
        private int? _workload;
        public int? Workload
        {
            get { return _workload; }
            set { _workload = value; }
        }
        /// <summary>
        /// IsLeader
        /// </summary>		
        private int? _isleader;
        public int? IsLeader
        {
            get { return _isleader; }
            set { _isleader = value; }
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
        /// FiledName:HandlerID
        /// </summary>		
        public static string FieldHandlerID
        {
            get { return "HandlerID"; }
        }
        /// <summary>
        /// FiledName:Workload
        /// </summary>		
        public static string FieldWorkload
        {
            get { return "Workload"; }
        }
        /// <summary>
        /// FiledName:IsLeader
        /// </summary>		
        public static string FieldIsLeader
        {
            get { return "IsLeader"; }
        }
        #endregion

        public OrderHandlerEntity GetFrom(System.Data.DataRow dataRow)
        {
            OrderHandlerEntity entity = new OrderHandlerEntity();
            entity.ID = CommonUtil.ConvertToInt(dataRow[entity.PrimaryKey]);
            entity.OrderID = CommonUtil.ConvertToGuid(dataRow[FieldOrderID]);
            entity.HandlerID = CommonUtil.ConvertToInt(dataRow[FieldHandlerID]);
            entity.Workload = CommonUtil.ConvertToInt(dataRow[FieldWorkload]);
            entity.IsLeader = CommonUtil.ConvertToInt(dataRow[FieldIsLeader]);
            return entity;
        }


        public OrderHandlerEntity GetFrom(System.Data.IDataReader dataReader)
        {
            OrderHandlerEntity entity = new OrderHandlerEntity();
            entity.ID = CommonUtil.ConvertToInt(dataReader[entity.PrimaryKey]);
            entity.OrderID = CommonUtil.ConvertToGuid(dataReader[FieldOrderID]);
            entity.HandlerID = CommonUtil.ConvertToInt(dataReader[FieldHandlerID]);
            entity.Workload = CommonUtil.ConvertToInt(dataReader[FieldWorkload]);
            entity.IsLeader = CommonUtil.ConvertToInt(dataReader[FieldIsLeader]);
            return entity;
        }

        public void SetEntity(NonQueryBuilder sqlBuilder, OrderHandlerEntity entity)
        {
            sqlBuilder.SetValue(FieldOrderID, entity.OrderID);
            sqlBuilder.SetValue(FieldHandlerID, entity.HandlerID);
            sqlBuilder.SetValue(FieldWorkload, entity.Workload);
            sqlBuilder.SetValue(FieldIsLeader, entity.IsLeader);
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