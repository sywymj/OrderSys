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
	public class OrderEntity:BaseEntity,IEntity<OrderEntity>
	{
		/// <summary>
		/// TableName
		/// </summary>
		public string TableName
        {
            get { return "[O_Order]"; }
        }
	
		public override string PrimaryKey
        {
            get {  return "ID"; }
        }
	
	
		
	
   		#region 字段、属性
      	/// <summary>
		/// ID
        /// </summary>		
		private Guid? _id;
        public Guid? ID
        {
            get{ return _id; }
            set{ _id = value; }
        }        
		/// <summary>
		/// OrderNo
        /// </summary>		
		private string _orderno;
        public string OrderNo
        {
            get{ return _orderno; }
            set{ _orderno = value; }
        }        
		/// <summary>
		/// StaffID
        /// </summary>		
		private int? _staffid;
        public int? StaffID
        {
            get{ return _staffid; }
            set{ _staffid = value; }
        }        
		/// <summary>
		/// OperatorID
        /// </summary>		
		private int? _operatorid;
        public int? OperatorID
        {
            get{ return _operatorid; }
            set{ _operatorid = value; }
        }        
		/// <summary>
		/// NextOperatorID
        /// </summary>		
		private int? _nextoperatorid;
        public int? NextOperatorID
        {
            get{ return _nextoperatorid; }
            set{ _nextoperatorid = value; }
        }        
		/// <summary>
		/// Content
        /// </summary>		
		private string _content;
        public string Content
        {
            get{ return _content; }
            set{ _content = value; }
        }        
		/// <summary>
		/// Remark
        /// </summary>		
		private string _remark;
        public string Remark
        {
            get{ return _remark; }
            set{ _remark = value; }
        }        
		/// <summary>
		/// AssignDpt
        /// </summary>		
		private int? _assigndpt;
        public int? AssignDpt
        {
            get{ return _assigndpt; }
            set{ _assigndpt = value; }
        }        
		/// <summary>
		/// Status
        /// </summary>		
		private int? _status;
        public int? Status
        {
            get{ return _status; }
            set{ _status = value; }
        }        
		/// <summary>
		/// FinishTime
        /// </summary>		
		private DateTime? _finishtime;
        public DateTime? FinishTime
        {
            get{ return _finishtime; }
            set{ _finishtime = value; }
        }        
		/// <summary>
		/// BookingTime
        /// </summary>		
		private DateTime? _bookingtime;
        public DateTime? BookingTime
        {
            get{ return _bookingtime; }
            set{ _bookingtime = value; }
        }        
		/// <summary>
		/// Attn
        /// </summary>		
		private string _attn;
        public string Attn
        {
            get{ return _attn; }
            set{ _attn = value; }
        }        
		/// <summary>
		/// AttnTel
        /// </summary>		
		private string _attntel;
        public string AttnTel
        {
            get{ return _attntel; }
            set{ _attntel = value; }
        }        
		/// <summary>
		/// Priority
        /// </summary>		
		private int? _priority;
        public int? Priority
        {
            get{ return _priority; }
            set{ _priority = value; }
        }        
				#endregion

		#region  数据库字段名称
      	/// <summary>
		/// FiledName:ID
        /// </summary>		
        public string FieldID
        {
            get{ return "ID"; }
        }        
		/// <summary>
		/// FiledName:OrderNo
        /// </summary>		
        public string FieldOrderNo
        {
            get{ return "OrderNo"; }
        }        
		/// <summary>
		/// FiledName:StaffID
        /// </summary>		
        public string FieldStaffID
        {
            get{ return "StaffID"; }
        }        
		/// <summary>
		/// FiledName:OperatorID
        /// </summary>		
        public string FieldOperatorID
        {
            get{ return "OperatorID"; }
        }        
		/// <summary>
		/// FiledName:NextOperatorID
        /// </summary>		
        public string FieldNextOperatorID
        {
            get{ return "NextOperatorID"; }
        }        
		/// <summary>
		/// FiledName:Content
        /// </summary>		
        public string FieldContent
        {
            get{ return "Content"; }
        }        
		/// <summary>
		/// FiledName:Remark
        /// </summary>		
        public string FieldRemark
        {
            get{ return "Remark"; }
        }        
		/// <summary>
		/// FiledName:AssignDpt
        /// </summary>		
        public string FieldAssignDpt
        {
            get{ return "AssignDpt"; }
        }        
		/// <summary>
		/// FiledName:Status
        /// </summary>		
        public string FieldStatus
        {
            get{ return "Status"; }
        }        
		/// <summary>
		/// FiledName:FinishTime
        /// </summary>		
        public string FieldFinishTime
        {
            get{ return "FinishTime"; }
        }        
		/// <summary>
		/// FiledName:BookingTime
        /// </summary>		
        public string FieldBookingTime
        {
            get{ return "BookingTime"; }
        }        
		/// <summary>
		/// FiledName:Attn
        /// </summary>		
        public string FieldAttn
        {
            get{ return "Attn"; }
        }        
		/// <summary>
		/// FiledName:AttnTel
        /// </summary>		
        public string FieldAttnTel
        {
            get{ return "AttnTel"; }
        }        
		/// <summary>
		/// FiledName:Priority
        /// </summary>		
        public string FieldPriority
        {
            get{ return "Priority"; }
        }        
		   		#endregion
   		
   		public OrderEntity GetFrom(System.Data.DataRow dataRow)
        {
	   		this.ID = CommonUtil.ConvertToGuid(dataRow[this.FieldID]);
			this.OrderNo = CommonUtil.ConvertToString(dataRow[this.FieldOrderNo]);
			this.StaffID = CommonUtil.ConvertToInt(dataRow[this.FieldStaffID]);
			this.OperatorID = CommonUtil.ConvertToInt(dataRow[this.FieldOperatorID]);
			this.NextOperatorID = CommonUtil.ConvertToInt(dataRow[this.FieldNextOperatorID]);
			this.Content = CommonUtil.ConvertToString(dataRow[this.FieldContent]);
			this.Remark = CommonUtil.ConvertToString(dataRow[this.FieldRemark]);
			this.AssignDpt = CommonUtil.ConvertToInt(dataRow[this.FieldAssignDpt]);
			this.Status = CommonUtil.ConvertToInt(dataRow[this.FieldStatus]);
			this.FinishTime = CommonUtil.ConvertToDateTime(dataRow[this.FieldFinishTime]);
			this.BookingTime = CommonUtil.ConvertToDateTime(dataRow[this.FieldBookingTime]);
			this.Attn = CommonUtil.ConvertToString(dataRow[this.FieldAttn]);
			this.AttnTel = CommonUtil.ConvertToString(dataRow[this.FieldAttnTel]);
			this.Priority = CommonUtil.ConvertToInt(dataRow[this.FieldPriority]);
						return this;
		}
		
		
   		public OrderEntity GetFrom(System.Data.IDataReader dataReader)
        {
	   		this.ID = CommonUtil.ConvertToGuid(dataReader[this.FieldID]);
			this.OrderNo = CommonUtil.ConvertToString(dataReader[this.FieldOrderNo]);
			this.StaffID = CommonUtil.ConvertToInt(dataReader[this.FieldStaffID]);
			this.OperatorID = CommonUtil.ConvertToInt(dataReader[this.FieldOperatorID]);
			this.NextOperatorID = CommonUtil.ConvertToInt(dataReader[this.FieldNextOperatorID]);
			this.Content = CommonUtil.ConvertToString(dataReader[this.FieldContent]);
			this.Remark = CommonUtil.ConvertToString(dataReader[this.FieldRemark]);
			this.AssignDpt = CommonUtil.ConvertToInt(dataReader[this.FieldAssignDpt]);
			this.Status = CommonUtil.ConvertToInt(dataReader[this.FieldStatus]);
			this.FinishTime = CommonUtil.ConvertToDateTime(dataReader[this.FieldFinishTime]);
			this.BookingTime = CommonUtil.ConvertToDateTime(dataReader[this.FieldBookingTime]);
			this.Attn = CommonUtil.ConvertToString(dataReader[this.FieldAttn]);
			this.AttnTel = CommonUtil.ConvertToString(dataReader[this.FieldAttnTel]);
			this.Priority = CommonUtil.ConvertToInt(dataReader[this.FieldPriority]);
						return this;
		}
		
		public void SetEntity(NonQueryBuilder sqlBuilder, OrderEntity entity)
        {
	   		sqlBuilder.SetValue(this.FieldID, entity.ID);
			sqlBuilder.SetValue(this.FieldOrderNo, entity.OrderNo);
			sqlBuilder.SetValue(this.FieldStaffID, entity.StaffID);
			sqlBuilder.SetValue(this.FieldOperatorID, entity.OperatorID);
			sqlBuilder.SetValue(this.FieldNextOperatorID, entity.NextOperatorID);
			sqlBuilder.SetValue(this.FieldContent, entity.Content);
			sqlBuilder.SetValue(this.FieldRemark, entity.Remark);
			sqlBuilder.SetValue(this.FieldAssignDpt, entity.AssignDpt);
			sqlBuilder.SetValue(this.FieldStatus, entity.Status);
			sqlBuilder.SetValue(this.FieldFinishTime, entity.FinishTime);
			sqlBuilder.SetValue(this.FieldBookingTime, entity.BookingTime);
			sqlBuilder.SetValue(this.FieldAttn, entity.Attn);
			sqlBuilder.SetValue(this.FieldAttnTel, entity.AttnTel);
			sqlBuilder.SetValue(this.FieldPriority, entity.Priority);
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