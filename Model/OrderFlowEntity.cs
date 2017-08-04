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
    //O_OrderFlow
    public class OrderFlowEntity : BaseEntity, IEntity<OrderFlowEntity>
    {
        /// <summary>
        /// TableName
        /// </summary>
        public string TableName
        {
            get { return "[O_OrderFlow]"; }
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
        /// OperatorID
        /// </summary>		
        private int? _operatorid;
        public int? OperatorID
        {
            get { return _operatorid; }
            set { _operatorid = value; }
        }
        /// <summary>
        /// NextOperatorID
        /// </summary>		
        private int? _nextoperatorid;
        public int? NextOperatorID
        {
            get { return _nextoperatorid; }
            set { _nextoperatorid = value; }
        }
        /// <summary>
        /// Operation
        /// </summary>		
        private int? _operation;
        public int? Operation
        {
            get { return _operation; }
            set { _operation = value; }
        }
        /// <summary>
        /// OperateTime
        /// </summary>		
        private DateTime? _operatetime;
        public DateTime? OperateTime
        {
            get { return _operatetime; }
            set { _operatetime = value; }
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
        /// FiledName:OrderID
        /// </summary>		
        public static string FieldOrderID
        {
            get { return "OrderID"; }
        }
        /// <summary>
        /// FiledName:OperatorID
        /// </summary>		
        public static string FieldOperatorID
        {
            get { return "OperatorID"; }
        }
        /// <summary>
        /// FiledName:NextOperatorID
        /// </summary>		
        public static string FieldNextOperatorID
        {
            get { return "NextOperatorID"; }
        }
        /// <summary>
        /// FiledName:Operation
        /// </summary>		
        public static string FieldOperation
        {
            get { return "Operation"; }
        }
        /// <summary>
        /// FiledName:OperateTime
        /// </summary>		
        public static string FieldOperateTime
        {
            get { return "OperateTime"; }
        }
        /// <summary>
        /// FiledName:Remark
        /// </summary>		
        public static string FieldRemark
        {
            get { return "Remark"; }
        }
        #endregion

        public OrderFlowEntity GetFrom(System.Data.DataRow dataRow)
        {
            this.ID = CommonUtil.ConvertToInt(dataRow[this.PrimaryKey]);
            this.OrderID = CommonUtil.ConvertToGuid(dataRow[FieldOrderID]);
            this.OperatorID = CommonUtil.ConvertToInt(dataRow[FieldOperatorID]);
            this.NextOperatorID = CommonUtil.ConvertToInt(dataRow[FieldNextOperatorID]);
            this.Operation = CommonUtil.ConvertToInt(dataRow[FieldOperation]);
            this.OperateTime = CommonUtil.ConvertToDateTime(dataRow[FieldOperateTime]);
            this.Remark = CommonUtil.ConvertToString(dataRow[FieldRemark]);
            return this;
        }


        public OrderFlowEntity GetFrom(System.Data.IDataReader dataReader)
        {
            this.ID = CommonUtil.ConvertToInt(dataReader[this.PrimaryKey]);
            this.OrderID = CommonUtil.ConvertToGuid(dataReader[FieldOrderID]);
            this.OperatorID = CommonUtil.ConvertToInt(dataReader[FieldOperatorID]);
            this.NextOperatorID = CommonUtil.ConvertToInt(dataReader[FieldNextOperatorID]);
            this.Operation = CommonUtil.ConvertToInt(dataReader[FieldOperation]);
            this.OperateTime = CommonUtil.ConvertToDateTime(dataReader[FieldOperateTime]);
            this.Remark = CommonUtil.ConvertToString(dataReader[FieldRemark]);
            return this;
        }

        public void SetEntity(NonQueryBuilder sqlBuilder, OrderFlowEntity entity)
        {
            sqlBuilder.SetValue(FieldOrderID, entity.OrderID);
            sqlBuilder.SetValue(FieldOperatorID, entity.OperatorID);
            sqlBuilder.SetValue(FieldNextOperatorID, entity.NextOperatorID);
            sqlBuilder.SetValue(FieldOperation, entity.Operation);
            sqlBuilder.SetValue(FieldOperateTime, entity.OperateTime);
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