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
	//O_OrderHandleDetail
    public class OrderHandleDetailEntity : BaseEntity, IEntity<OrderHandleDetailEntity>
    {
        /// <summary>
        /// TableName
        /// </summary>
        public string TableName
        {
            get { return "[O_OrderHandleDetail]"; }
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
        /// HandleType
        /// </summary>		
        private int? _handletype;
        public int? HandleType
        {
            get { return _handletype; }
            set { _handletype = value; }
        }
        /// <summary>
        /// HandleDetail
        /// </summary>		
        private string _handledetail;
        public string HandleDetail
        {
            get { return _handledetail; }
            set { _handledetail = value; }
        }
        /// <summary>
        /// Progress
        /// </summary>		
        private int? _progress;
        public int? Progress
        {
            get { return _progress; }
            set { _progress = value; }
        }
        /// <summary>
        /// HandleTime
        /// </summary>		
        private DateTime? _handletime;
        public DateTime? HandleTime
        {
            get { return _handletime; }
            set { _handletime = value; }
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
        public string FieldID
        {
            get { return "ID"; }
        }
        /// <summary>
        /// FiledName:OrderID
        /// </summary>		
        public string FieldOrderID
        {
            get { return "OrderID"; }
        }
        /// <summary>
        /// FiledName:HandlerID
        /// </summary>		
        public string FieldHandlerID
        {
            get { return "HandlerID"; }
        }
        /// <summary>
        /// FiledName:HandleType
        /// </summary>		
        public string FieldHandleType
        {
            get { return "HandleType"; }
        }
        /// <summary>
        /// FiledName:HandleDetail
        /// </summary>		
        public string FieldHandleDetail
        {
            get { return "HandleDetail"; }
        }
        /// <summary>
        /// FiledName:Progress
        /// </summary>		
        public string FieldProgress
        {
            get { return "Progress"; }
        }
        /// <summary>
        /// FiledName:HandleTime
        /// </summary>		
        public string FieldHandleTime
        {
            get { return "HandleTime"; }
        }
        /// <summary>
        /// FiledName:Remark
        /// </summary>		
        public string FieldRemark
        {
            get { return "Remark"; }
        }
        #endregion

        public OrderHandleDetailEntity GetFrom(System.Data.DataRow dataRow)
        {
            this.ID = CommonUtil.ConvertToInt(dataRow[this.PrimaryKey]);
            this.OrderID = CommonUtil.ConvertToGuid(dataRow[this.FieldOrderID]);
            this.HandlerID = CommonUtil.ConvertToInt(dataRow[this.FieldHandlerID]);
            this.HandleType = CommonUtil.ConvertToInt(dataRow[this.FieldHandleType]);
            this.HandleDetail = CommonUtil.ConvertToString(dataRow[this.FieldHandleDetail]);
            this.Progress = CommonUtil.ConvertToInt(dataRow[this.FieldProgress]);
            this.HandleTime = CommonUtil.ConvertToDateTime(dataRow[this.FieldHandleTime]);
            this.Remark = CommonUtil.ConvertToString(dataRow[this.FieldRemark]);
            return this;
        }


        public OrderHandleDetailEntity GetFrom(System.Data.IDataReader dataReader)
        {
            this.ID = CommonUtil.ConvertToInt(dataReader[this.PrimaryKey]);
            this.OrderID = CommonUtil.ConvertToGuid(dataReader[this.FieldOrderID]);
            this.HandlerID = CommonUtil.ConvertToInt(dataReader[this.FieldHandlerID]);
            this.HandleType = CommonUtil.ConvertToInt(dataReader[this.FieldHandleType]);
            this.HandleDetail = CommonUtil.ConvertToString(dataReader[this.FieldHandleDetail]);
            this.Progress = CommonUtil.ConvertToInt(dataReader[this.FieldProgress]);
            this.HandleTime = CommonUtil.ConvertToDateTime(dataReader[this.FieldHandleTime]);
            this.Remark = CommonUtil.ConvertToString(dataReader[this.FieldRemark]);
            return this;
        }

        public void SetEntity(NonQueryBuilder sqlBuilder, OrderHandleDetailEntity entity)
        {
            sqlBuilder.SetValue(this.FieldOrderID, entity.OrderID);
            sqlBuilder.SetValue(this.FieldHandlerID, entity.HandlerID);
            sqlBuilder.SetValue(this.FieldHandleType, entity.HandleType);
            sqlBuilder.SetValue(this.FieldHandleDetail, entity.HandleDetail);
            sqlBuilder.SetValue(this.FieldProgress, entity.Progress);
            sqlBuilder.SetValue(this.FieldHandleTime, entity.HandleTime);
            sqlBuilder.SetValue(this.FieldRemark, entity.Remark);
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