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
    //O_Order
    public class OrderEntity : BaseEntity, IEntity<OrderEntity>
    {
        /// <summary>
        /// TableName
        /// </summary>
        public string TableName
        {
            get { return "[O_Order]"; }
        }

        /// <summary>
        /// 主键
        /// </summary>
        public override string PrimaryKey
        {
            get { return "ID"; }
        }




        #region 字段、属性
        /// <summary>
        /// ID
        /// </summary>		
        private Guid? _id;
        public Guid? ID
        {
            get { return _id; }
            set { _id = value; }
        }
        /// <summary>
        /// OrderNo
        /// </summary>		
        private string _orderno;
        public string OrderNo
        {
            get { return _orderno; }
            set { _orderno = value; }
        }
        /// <summary>
        /// StarterID
        /// </summary>		
        private int? _starterid;
        public int? StarterID
        {
            get { return _starterid; }
            set { _starterid = value; }
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
        /// AppointerID
        /// </summary>		
        private int? _appointerid;
        public int? AppointerID
        {
            get { return _appointerid; }
            set { _appointerid = value; }
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
        /// AssignDpt
        /// </summary>		
        private int? _assigndpt;
        public int? AssignDpt
        {
            get { return _assigndpt; }
            set { _assigndpt = value; }
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
        /// <summary>
        /// Priority
        /// </summary>		
        private int? _priority;
        public int? Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }
        /// <summary>
        /// Status
        /// </summary>		
        private int? _status;
        public int? Status
        {
            get { return _status; }
            set { _status = value; }
        }
        /// <summary>
        /// FinishTime
        /// </summary>		
        private DateTime? _finishtime;
        public DateTime? FinishTime
        {
            get { return _finishtime; }
            set { _finishtime = value; }
        }
        /// <summary>
        /// BookingTime
        /// </summary>		
        private DateTime? _bookingtime;
        public DateTime? BookingTime
        {
            get { return _bookingtime; }
            set { _bookingtime = value; }
        }
        /// <summary>
        /// Attn
        /// </summary>		
        private string _attn;
        public string Attn
        {
            get { return _attn; }
            set { _attn = value; }
        }
        /// <summary>
        /// AttnTel
        /// </summary>		
        private string _attntel;
        public string AttnTel
        {
            get { return _attntel; }
            set { _attntel = value; }
        }
        /// <summary>
        /// Content
        /// </summary>		
        private string _content;
        public string Content
        {
            get { return _content; }
            set { _content = value; }
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
        /// FiledName:OrderNo
        /// </summary>		
        public static string FieldOrderNo
        {
            get { return "OrderNo"; }
        }
        /// <summary>
        /// FiledName:StarterID
        /// </summary>		
        public static string FieldStarterID
        {
            get { return "StarterID"; }
        }
        /// <summary>
        /// FiledName:HandlerID
        /// </summary>		
        public static string FieldHandlerID
        {
            get { return "HandlerID"; }
        }
        /// <summary>
        /// FiledName:AppointerID
        /// </summary>		
        public static string FieldAppointerID
        {
            get { return "AppointerID"; }
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
        /// FiledName:AssignDpt
        /// </summary>		
        public static string FieldAssignDpt
        {
            get { return "AssignDpt"; }
        }
        /// <summary>
        /// FiledName:Remark
        /// </summary>		
        public static string FieldRemark
        {
            get { return "Remark"; }
        }
        /// <summary>
        /// FiledName:Priority
        /// </summary>		
        public static string FieldPriority
        {
            get { return "Priority"; }
        }
        /// <summary>
        /// FiledName:Status
        /// </summary>		
        public static string FieldStatus
        {
            get { return "Status"; }
        }
        /// <summary>
        /// FiledName:FinishTime
        /// </summary>		
        public static string FieldFinishTime
        {
            get { return "FinishTime"; }
        }
        /// <summary>
        /// FiledName:BookingTime
        /// </summary>		
        public static string FieldBookingTime
        {
            get { return "BookingTime"; }
        }
        /// <summary>
        /// FiledName:Attn
        /// </summary>		
        public static string FieldAttn
        {
            get { return "Attn"; }
        }
        /// <summary>
        /// FiledName:AttnTel
        /// </summary>		
        public static string FieldAttnTel
        {
            get { return "AttnTel"; }
        }
        /// <summary>
        /// FiledName:Content
        /// </summary>		
        public static string FieldContent
        {
            get { return "Content"; }
        }
        #endregion

        public OrderEntity GetFrom(System.Data.DataRow dataRow)
        {
            OrderEntity entity = new OrderEntity();
            entity.ID = CommonUtil.ConvertToGuid(dataRow[FieldID]);
            entity.OrderNo = CommonUtil.ConvertToString(dataRow[FieldOrderNo]);
            entity.StarterID = CommonUtil.ConvertToInt(dataRow[FieldStarterID]);
            entity.HandlerID = CommonUtil.ConvertToInt(dataRow[FieldHandlerID]);
            entity.AppointerID = CommonUtil.ConvertToInt(dataRow[FieldAppointerID]);
            entity.OperatorID = CommonUtil.ConvertToInt(dataRow[FieldOperatorID]);
            entity.NextOperatorID = CommonUtil.ConvertToInt(dataRow[FieldNextOperatorID]);
            entity.AssignDpt = CommonUtil.ConvertToInt(dataRow[FieldAssignDpt]);
            entity.Remark = CommonUtil.ConvertToString(dataRow[FieldRemark]);
            entity.Priority = CommonUtil.ConvertToInt(dataRow[FieldPriority]);
            entity.Status = CommonUtil.ConvertToInt(dataRow[FieldStatus]);
            entity.FinishTime = CommonUtil.ConvertToDateTime(dataRow[FieldFinishTime]);
            entity.BookingTime = CommonUtil.ConvertToDateTime(dataRow[FieldBookingTime]);
            entity.Attn = CommonUtil.ConvertToString(dataRow[FieldAttn]);
            entity.AttnTel = CommonUtil.ConvertToString(dataRow[FieldAttnTel]);
            entity.Content = CommonUtil.ConvertToString(dataRow[FieldContent]);
            return entity;
        }


        public OrderEntity GetFrom(System.Data.IDataReader dataReader)
        {
            OrderEntity entity = new OrderEntity();
            entity.ID = CommonUtil.ConvertToGuid(dataReader[FieldID]);
            entity.OrderNo = CommonUtil.ConvertToString(dataReader[FieldOrderNo]);
            entity.StarterID = CommonUtil.ConvertToInt(dataReader[FieldStarterID]);
            entity.HandlerID = CommonUtil.ConvertToInt(dataReader[FieldHandlerID]);
            entity.AppointerID = CommonUtil.ConvertToInt(dataReader[FieldAppointerID]);
            entity.OperatorID = CommonUtil.ConvertToInt(dataReader[FieldOperatorID]);
            entity.NextOperatorID = CommonUtil.ConvertToInt(dataReader[FieldNextOperatorID]);
            entity.AssignDpt = CommonUtil.ConvertToInt(dataReader[FieldAssignDpt]);
            entity.Remark = CommonUtil.ConvertToString(dataReader[FieldRemark]);
            entity.Priority = CommonUtil.ConvertToInt(dataReader[FieldPriority]);
            entity.Status = CommonUtil.ConvertToInt(dataReader[FieldStatus]);
            entity.FinishTime = CommonUtil.ConvertToDateTime(dataReader[FieldFinishTime]);
            entity.BookingTime = CommonUtil.ConvertToDateTime(dataReader[FieldBookingTime]);
            entity.Attn = CommonUtil.ConvertToString(dataReader[FieldAttn]);
            entity.AttnTel = CommonUtil.ConvertToString(dataReader[FieldAttnTel]);
            entity.Content = CommonUtil.ConvertToString(dataReader[FieldContent]);
            return entity;
        }

        public void SetEntity(NonQueryBuilder sqlBuilder, OrderEntity entity)
        {
            sqlBuilder.SetValue(FieldID, entity.ID);
            sqlBuilder.SetValue(FieldOrderNo, entity.OrderNo);
            sqlBuilder.SetValue(FieldStarterID, entity.StarterID);
            sqlBuilder.SetValue(FieldHandlerID, entity.HandlerID);
            sqlBuilder.SetValue(FieldAppointerID, entity.AppointerID);
            sqlBuilder.SetValue(FieldOperatorID, entity.OperatorID);
            sqlBuilder.SetValue(FieldNextOperatorID, entity.NextOperatorID);
            sqlBuilder.SetValue(FieldAssignDpt, entity.AssignDpt);
            sqlBuilder.SetValue(FieldRemark, entity.Remark);
            sqlBuilder.SetValue(FieldPriority, entity.Priority);
            sqlBuilder.SetValue(FieldStatus, entity.Status);
            sqlBuilder.SetValue(FieldFinishTime, entity.FinishTime);
            sqlBuilder.SetValue(FieldBookingTime, entity.BookingTime);
            sqlBuilder.SetValue(FieldAttn, entity.Attn);
            sqlBuilder.SetValue(FieldAttnTel, entity.AttnTel);
            sqlBuilder.SetValue(FieldContent, entity.Content);
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