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
	public class OrderHandlerEntity:BaseEntity,IEntity<OrderHandlerEntity>
	{
		/// <summary>
		/// TableName
		/// </summary>
		public string TableName
        {
            get { return "[O_OrderHandler]"; }
        }
	
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
		/// Workload
        /// </summary>		
		private int? _workload;
        public int? Workload
        {
            get{ return _workload; }
            set{ _workload = value; }
        }        
		/// <summary>
		/// IsLeader
        /// </summary>		
		private int? _isleader;
        public int? IsLeader
        {
            get{ return _isleader; }
            set{ _isleader = value; }
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
		/// FiledName:OrderID
        /// </summary>		
        public string FieldOrderID
        {
            get{ return "OrderID"; }
        }        
		/// <summary>
		/// FiledName:HandlerID
        /// </summary>		
        public string FieldHandlerID
        {
            get{ return "HandlerID"; }
        }        
		/// <summary>
		/// FiledName:Workload
        /// </summary>		
        public string FieldWorkload
        {
            get{ return "Workload"; }
        }        
		/// <summary>
		/// FiledName:IsLeader
        /// </summary>		
        public string FieldIsLeader
        {
            get{ return "IsLeader"; }
        }        
		   		#endregion
   		
   		public OrderHandlerEntity GetFrom(System.Data.DataRow dataRow)
        {
	   		this.ID = CommonUtil.ConvertToInt(dataRow[this.PrimaryKey]);
				this.OrderID = CommonUtil.ConvertToGuid(dataRow[this.FieldOrderID]);
			this.HandlerID = CommonUtil.ConvertToInt(dataRow[this.FieldHandlerID]);
			this.Workload = CommonUtil.ConvertToInt(dataRow[this.FieldWorkload]);
			this.IsLeader = CommonUtil.ConvertToInt(dataRow[this.FieldIsLeader]);
						return this;
		}
		
		
   		public OrderHandlerEntity GetFrom(System.Data.IDataReader dataReader)
        {
	   		this.ID = CommonUtil.ConvertToInt(dataReader[this.PrimaryKey]);
				this.OrderID = CommonUtil.ConvertToGuid(dataReader[this.FieldOrderID]);
			this.HandlerID = CommonUtil.ConvertToInt(dataReader[this.FieldHandlerID]);
			this.Workload = CommonUtil.ConvertToInt(dataReader[this.FieldWorkload]);
			this.IsLeader = CommonUtil.ConvertToInt(dataReader[this.FieldIsLeader]);
						return this;
		}
		
		public void SetEntity(NonQueryBuilder sqlBuilder, OrderHandlerEntity entity)
        {
	   		sqlBuilder.SetValue(this.FieldOrderID, entity.OrderID);
			sqlBuilder.SetValue(this.FieldHandlerID, entity.HandlerID);
			sqlBuilder.SetValue(this.FieldWorkload, entity.Workload);
			sqlBuilder.SetValue(this.FieldIsLeader, entity.IsLeader);
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