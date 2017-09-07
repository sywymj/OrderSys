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
	public class OrderHandleDetailEntity:BaseEntity,IEntity<OrderHandleDetailEntity>
	{
		/// <summary>
		/// TableName
		/// </summary>
		public string TableName
        {
            get { return "[O_OrderHandleDetail]"; }
        }
	
		/// <summary>
        /// 主键
        /// </summary>
		public override string PrimaryKey
        {
            get {  return "ID"; }
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
            get{ return _id; }
            set{ _id = value; }
        }        
		/// <summary>
		/// OrderID
        /// </summary>		
		private Guid? _orderid;
        public Guid? OrderID
        {
            get{ return _orderid; }
            set{ _orderid = value; }
        }        
		/// <summary>
		/// HandlerID
        /// </summary>		
		private int? _handlerid;
        public int? HandlerID
        {
            get{ return _handlerid; }
            set{ _handlerid = value; }
        }        
		/// <summary>
		/// HandleType
        /// </summary>		
		private int? _handletype;
        public int? HandleType
        {
            get{ return _handletype; }
            set{ _handletype = value; }
        }        
		/// <summary>
		/// HandleDetail
        /// </summary>		
		private string _handledetail;
        public string HandleDetail
        {
            get{ return _handledetail; }
            set{ _handledetail = value; }
        }        
		/// <summary>
		/// Progress
        /// </summary>		
		private int? _progress;
        public int? Progress
        {
            get{ return _progress; }
            set{ _progress = value; }
        }        
		/// <summary>
		/// HandleTime
        /// </summary>		
		private DateTime? _handletime;
        public DateTime? HandleTime
        {
            get{ return _handletime; }
            set{ _handletime = value; }
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
				#endregion

		#region  数据库字段名称
      	/// <summary>
		/// FiledName:ID
        /// </summary>		
        public static string FieldID
        {
            get{ return "ID"; }
        }        
		/// <summary>
		/// FiledName:OrderID
        /// </summary>		
        public static string FieldOrderID
        {
            get{ return "OrderID"; }
        }        
		/// <summary>
		/// FiledName:HandlerID
        /// </summary>		
        public static string FieldHandlerID
        {
            get{ return "HandlerID"; }
        }        
		/// <summary>
		/// FiledName:HandleType
        /// </summary>		
        public static string FieldHandleType
        {
            get{ return "HandleType"; }
        }        
		/// <summary>
		/// FiledName:HandleDetail
        /// </summary>		
        public static string FieldHandleDetail
        {
            get{ return "HandleDetail"; }
        }        
		/// <summary>
		/// FiledName:Progress
        /// </summary>		
        public static string FieldProgress
        {
            get{ return "Progress"; }
        }        
		/// <summary>
		/// FiledName:HandleTime
        /// </summary>		
        public static string FieldHandleTime
        {
            get{ return "HandleTime"; }
        }        
		/// <summary>
		/// FiledName:Remark
        /// </summary>		
        public static string FieldRemark
        {
            get{ return "Remark"; }
        }        
		   		#endregion
   		
   		public OrderHandleDetailEntity GetFrom(System.Data.DataRow dataRow)
        {
        	OrderHandleDetailEntity entity = new OrderHandleDetailEntity();
	   		entity.ID = CommonUtil.ConvertToInt(dataRow[this.PrimaryKey]);
				entity.OrderID = CommonUtil.ConvertToGuid(dataRow[FieldOrderID]);
			entity.HandlerID = CommonUtil.ConvertToInt(dataRow[FieldHandlerID]);
			entity.HandleType = CommonUtil.ConvertToInt(dataRow[FieldHandleType]);
			entity.HandleDetail = CommonUtil.ConvertToString(dataRow[FieldHandleDetail]);
			entity.Progress = CommonUtil.ConvertToInt(dataRow[FieldProgress]);
			entity.HandleTime = CommonUtil.ConvertToDateTime(dataRow[FieldHandleTime]);
			entity.Remark = CommonUtil.ConvertToString(dataRow[FieldRemark]);
						return entity;
		}
		
		
   		public OrderHandleDetailEntity GetFrom(System.Data.IDataReader dataReader)
        {
        	OrderHandleDetailEntity entity = new OrderHandleDetailEntity();
	   		entity.ID = CommonUtil.ConvertToInt(dataReader[this.PrimaryKey]);
				entity.OrderID = CommonUtil.ConvertToGuid(dataReader[FieldOrderID]);
			entity.HandlerID = CommonUtil.ConvertToInt(dataReader[FieldHandlerID]);
			entity.HandleType = CommonUtil.ConvertToInt(dataReader[FieldHandleType]);
			entity.HandleDetail = CommonUtil.ConvertToString(dataReader[FieldHandleDetail]);
			entity.Progress = CommonUtil.ConvertToInt(dataReader[FieldProgress]);
			entity.HandleTime = CommonUtil.ConvertToDateTime(dataReader[FieldHandleTime]);
			entity.Remark = CommonUtil.ConvertToString(dataReader[FieldRemark]);
						return entity;
		}
		
		public void SetEntity(NonQueryBuilder sqlBuilder, OrderHandleDetailEntity entity)
        {
	   		sqlBuilder.SetValue(FieldOrderID, entity.OrderID);
			sqlBuilder.SetValue(FieldHandlerID, entity.HandlerID);
			sqlBuilder.SetValue(FieldHandleType, entity.HandleType);
			sqlBuilder.SetValue(FieldHandleDetail, entity.HandleDetail);
			sqlBuilder.SetValue(FieldProgress, entity.Progress);
			sqlBuilder.SetValue(FieldHandleTime, entity.HandleTime);
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