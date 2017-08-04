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
        /// StaffID
        /// </summary>		
        private int? _staffid;
        public int? StaffID
        {
            get { return _staffid; }
            set { _staffid = value; }
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
        /// Content
        /// </summary>		
        private string _content;
        public string Content
        {
            get { return _content; }
            set { _content = value; }
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
        /// AssignDpt
        /// </summary>		
        private int? _assigndpt;
        public int? AssignDpt
        {
            get { return _assigndpt; }
            set { _assigndpt = value; }
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
        /// Priority
        /// </summary>		
        private int? _priority;
        public int? Priority
        {
            get { return _priority; }
            set { _priority = value; }
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
        /// FiledName:StaffID
        /// </summary>		
        public static string FieldStaffID
        {
            get { return "StaffID"; }
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
        /// FiledName:Content
        /// </summary>		
        public static string FieldContent
        {
            get { return "Content"; }
        }
        /// <summary>
        /// FiledName:Remark
        /// </summary>		
        public static string FieldRemark
        {
            get { return "Remark"; }
        }
        /// <summary>
        /// FiledName:AssignDpt
        /// </summary>		
        public static string FieldAssignDpt
        {
            get { return "AssignDpt"; }
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
        /// FiledName:Priority
        /// </summary>		
        public static string FieldPriority
        {
            get { return "Priority"; }
        }
        #endregion

        public OrderEntity GetFrom(System.Data.DataRow dataRow)
        {
            this.ID = CommonUtil.ConvertToGuid(dataRow[FieldID]);
            this.OrderNo = CommonUtil.ConvertToString(dataRow[FieldOrderNo]);
            this.StaffID = CommonUtil.ConvertToInt(dataRow[FieldStaffID]);
            this.OperatorID = CommonUtil.ConvertToInt(dataRow[FieldOperatorID]);
            this.NextOperatorID = CommonUtil.ConvertToInt(dataRow[FieldNextOperatorID]);
            this.Content = CommonUtil.ConvertToString(dataRow[FieldContent]);
            this.Remark = CommonUtil.ConvertToString(dataRow[FieldRemark]);
            this.AssignDpt = CommonUtil.ConvertToInt(dataRow[FieldAssignDpt]);
            this.Status = CommonUtil.ConvertToInt(dataRow[FieldStatus]);
            this.FinishTime = CommonUtil.ConvertToDateTime(dataRow[FieldFinishTime]);
            this.BookingTime = CommonUtil.ConvertToDateTime(dataRow[FieldBookingTime]);
            this.Attn = CommonUtil.ConvertToString(dataRow[FieldAttn]);
            this.AttnTel = CommonUtil.ConvertToString(dataRow[FieldAttnTel]);
            this.Priority = CommonUtil.ConvertToInt(dataRow[FieldPriority]);
            return this;
        }


        public OrderEntity GetFrom(System.Data.IDataReader dataReader)
        {
            this.ID = CommonUtil.ConvertToGuid(dataReader[FieldID]);
            this.OrderNo = CommonUtil.ConvertToString(dataReader[FieldOrderNo]);
            this.StaffID = CommonUtil.ConvertToInt(dataReader[FieldStaffID]);
            this.OperatorID = CommonUtil.ConvertToInt(dataReader[FieldOperatorID]);
            this.NextOperatorID = CommonUtil.ConvertToInt(dataReader[FieldNextOperatorID]);
            this.Content = CommonUtil.ConvertToString(dataReader[FieldContent]);
            this.Remark = CommonUtil.ConvertToString(dataReader[FieldRemark]);
            this.AssignDpt = CommonUtil.ConvertToInt(dataReader[FieldAssignDpt]);
            this.Status = CommonUtil.ConvertToInt(dataReader[FieldStatus]);
            this.FinishTime = CommonUtil.ConvertToDateTime(dataReader[FieldFinishTime]);
            this.BookingTime = CommonUtil.ConvertToDateTime(dataReader[FieldBookingTime]);
            this.Attn = CommonUtil.ConvertToString(dataReader[FieldAttn]);
            this.AttnTel = CommonUtil.ConvertToString(dataReader[FieldAttnTel]);
            this.Priority = CommonUtil.ConvertToInt(dataReader[FieldPriority]);
            return this;
        }

        public void SetEntity(NonQueryBuilder sqlBuilder, OrderEntity entity)
        {
            sqlBuilder.SetValue(FieldID, entity.ID);
            sqlBuilder.SetValue(FieldOrderNo, entity.OrderNo);
            sqlBuilder.SetValue(FieldStaffID, entity.StaffID);
            sqlBuilder.SetValue(FieldOperatorID, entity.OperatorID);
            sqlBuilder.SetValue(FieldNextOperatorID, entity.NextOperatorID);
            sqlBuilder.SetValue(FieldContent, entity.Content);
            sqlBuilder.SetValue(FieldRemark, entity.Remark);
            sqlBuilder.SetValue(FieldAssignDpt, entity.AssignDpt);
            sqlBuilder.SetValue(FieldStatus, entity.Status);
            sqlBuilder.SetValue(FieldFinishTime, entity.FinishTime);
            sqlBuilder.SetValue(FieldBookingTime, entity.BookingTime);
            sqlBuilder.SetValue(FieldAttn, entity.Attn);
            sqlBuilder.SetValue(FieldAttnTel, entity.AttnTel);
            sqlBuilder.SetValue(FieldPriority, entity.Priority);
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