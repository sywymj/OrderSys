﻿using CodeEngine.Framework.QueryBuilder;
using CodeEngine.Framework.QueryBuilder.Enums;
using JSNet.BaseSys;
using JSNet.DbUtilities;
using JSNet.Manager;
using JSNet.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace JSNet.Service
{
    public class OrderService
    {
        //发起报障单
        public void StartOrder(OrderEntity order)
        {
            PermissionService permissionService = new PermissionService();
            EntityManager<OrderEntity> orderManager = new EntityManager<OrderEntity>();
            EntityManager<OrderFlowEntity> orderflowManager = new EntityManager<OrderFlowEntity>();

            //1.0 获取当前员工数据
            StaffEntity staff = permissionService.GetCurrentStaff();

            //2.0获取最新的工单号
            string orderNo = GetNewOrderNo();

            //2.0 添加工单实体
            order.ID = Guid.NewGuid();
            order.OrderNo = orderNo;
            order.Status = (int)OrderStatus.Appointing;
            order.StarterID = staff.ID;
            order.OperatorID = staff.ID;
            order.Status = (int)OrderStatus.Appointing;
            if (string.IsNullOrEmpty(order.Attn) || string.IsNullOrEmpty(order.AttnTel))
            {
                order.Attn = staff.Name;
                order.AttnTel = staff.Tel;
            }
            orderManager.Insert(order);

            //3.0 添加工作流实体
            OrderFlowEntity orderFlow = new OrderFlowEntity();
            orderFlow.OrderID = order.ID;
            orderFlow.OperatorID=staff.ID;
            orderFlow.NextOperatorID = 0;
            orderFlow.Operation = (int)OperationEnum.Start;
            orderFlow.OperateTime = DateTime.Now;
            orderFlow.Remark = "";
            orderflowManager.Insert(orderFlow);
        }

        //委派工作
        public void AppointOrder(Guid orderID, int[] handlerIDs)
        {
            //1.0 获取当前员工数据
            PermissionService permissionService = new PermissionService();
            StaffEntity staff = permissionService.GetCurrentStaff();

            //2.0 修改工单实体
            EntityManager<OrderEntity> orderManager = new EntityManager<OrderEntity>();
            List<KeyValuePair<string,object>> kvps = new List<KeyValuePair<string,object>>();
            kvps.Add(new KeyValuePair<string,object>(OrderEntity.FieldStatus,(int)OrderStatus.Receving));
            int rows = orderManager.Update(kvps, orderID);
            if (rows == 0)
            {
                throw new JSException(JSErrMsg.ERR_CODE_DATA_MISSING, string.Format(JSErrMsg.ERR_MSG_DATA_MISSING, "工单实体"));
            }

            //3.0 添加工作流实体
            OrderFlowEntity orderFlow = new OrderFlowEntity();
            orderFlow.OrderID = orderID;
            orderFlow.OperatorID = staff.ID;
            orderFlow.NextOperatorID = GetLeaderHandlerID(handlerIDs);
            orderFlow.Operation = (int)OperationEnum.Appoint;
            orderFlow.OperateTime = DateTime.Now;
            orderFlow.Remark = "";
            EntityManager<OrderFlowEntity> orderflowManager = new EntityManager<OrderFlowEntity>();
            orderflowManager.Insert(orderFlow);
        }

        //接收报障单
        public void ReceiveOrder(Guid orderID)
        {
            //1.0 获取当前员工数据
            PermissionService permissionService = new PermissionService();
            StaffEntity staff = permissionService.GetCurrentStaff();

            //2.0 修改工单实体
            EntityManager<OrderEntity> orderManager = new EntityManager<OrderEntity>();
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldStatus, (int)OrderStatus.Handling));
            kvps.Add(new KeyValuePair<string,object>(OrderEntity.FieldNextOperatorID, staff.ID));
            int rows = orderManager.Update(kvps, orderID);
            if (rows == 0)
            {
                throw new JSException(JSErrMsg.ERR_CODE_DATA_MISSING, string.Format(JSErrMsg.ERR_MSG_DATA_MISSING, "工单实体"));
            }

            //3.0 添加工作流实体
            OrderFlowEntity orderflow = new OrderFlowEntity();
            orderflow.OrderID = orderID;
            orderflow.OperatorID = staff.ID;
            orderflow.NextOperatorID = staff.ID;
            orderflow.Operation = (int)OperationEnum.Receive;
            orderflow.OperateTime = DateTime.Now;
            orderflow.Remark = "";
            EntityManager<OrderFlowEntity> orderflowManager = new EntityManager<OrderFlowEntity>();
            orderflowManager.Insert(orderflow);
        }

        //增加处理明细
        public void AddHandleDetail(OrderHandleDetailEntity orderHandleDetail)
        {
            //1.0 获取当前员工数据
            PermissionService permissionService = new PermissionService();
            StaffEntity currentStaff = permissionService.GetCurrentStaff();

            //2.0 添加工作处理明细实体
            orderHandleDetail.HandlerID = currentStaff.ID;
            orderHandleDetail.HandleTime = DateTime.Now;
            orderHandleDetail.Remark = "";
            EntityManager<OrderHandleDetailEntity> orderHandleDetailManager = new EntityManager<OrderHandleDetailEntity>();
            orderHandleDetailManager.Insert(orderHandleDetail);
        }

        //报障处理完毕
        public void CompleteOrder(Guid orderID)
        {
            PermissionService permissionService = new PermissionService();
            EntityManager<OrderEntity> orderManager = new EntityManager<OrderEntity>();
            
            EntityManager<OrderFlowEntity> orderflowManager = new EntityManager<OrderFlowEntity>();

            //1.0 获取当前员工数据
            StaffEntity staff = permissionService.GetCurrentStaff();

            //2.0 修改工单实体
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldStatus, (int)OrderStatus.Checking));
            int rows = orderManager.Update(kvps, orderID);
            if (rows == 0)
            {
                throw new JSException(JSErrMsg.ERR_CODE_DATA_MISSING, string.Format(JSErrMsg.ERR_MSG_DATA_MISSING, "工单实体"));
            }

            //2.1 获取工单实体，以备获取发起人信息
            OrderEntity order = orderManager.GetSingle(orderID);
            if (order == null)
            {
                throw new JSException(JSErrMsg.ERR_CODE_DATA_MISSING, string.Format(JSErrMsg.ERR_MSG_DATA_MISSING, "工单实体"));
            }

            //3.0 添加工作流实体
            OrderFlowEntity orderflow = new OrderFlowEntity();
            orderflow.OrderID = orderID;
            orderflow.OperatorID = staff.ID;
            orderflow.NextOperatorID = order.StarterID;//传回发起人验收。
            orderflow.Operation = (int)OperationEnum.Handle;
            orderflow.OperateTime = DateTime.Now;
            orderflow.Remark = "";
            orderflowManager.Insert(orderflow);
        }

        //驳回报障，需继续处理
        public void RejectOrder(Guid orderID)
        {
            PermissionService permissionService = new PermissionService();
            EntityManager<OrderEntity> orderManager = new EntityManager<OrderEntity>();
            EntityManager<OrderHandlerEntity> orderHandlerManager = new EntityManager<OrderHandlerEntity>();
            EntityManager<OrderFlowEntity> orderflowManager = new EntityManager<OrderFlowEntity>();

            //1.0 获取当前员工数据
            StaffEntity staff = permissionService.GetCurrentStaff();

            //2.0 修改工单实体
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldStatus, (int)OrderStatus.Rejected));
            int rows = orderManager.Update(kvps, orderID);
            if (rows == 0)
            {
                throw new JSException(JSErrMsg.ERR_CODE_DATA_MISSING, string.Format(JSErrMsg.ERR_MSG_DATA_MISSING, "工单实体"));
            }

            //2.1 获取工单处理者列表，必备获取领队人的信息
            int count = 0;
            WhereStatement where = new WhereStatement();
            where.Add(OrderHandlerEntity.FieldOrderID, Comparison.Equals, orderID);
            List<OrderHandlerEntity> orderHandlers = orderHandlerManager.GetList(where, out count);

            //3.0 添加工作流实体
            OrderFlowEntity orderflow = new OrderFlowEntity();
            orderflow.OrderID = orderID;
            orderflow.OperatorID = staff.ID;
            orderflow.NextOperatorID = GetLeaderHandlerID(orderHandlers);
            orderflow.Operation = (int)OperationEnum.reject;
            orderflow.OperateTime = DateTime.Now;
            orderflow.Remark = "";
            orderflowManager.Insert(orderflow);
        }

        //报障验收完成
        public void FinishOrder(Guid orderID)
        {
            PermissionService permissionService = new PermissionService();
            EntityManager<OrderEntity> orderManager = new EntityManager<OrderEntity>();
            EntityManager<OrderFlowEntity> orderflowManager = new EntityManager<OrderFlowEntity>();

            //1.0 获取当前员工数据
            StaffEntity staff = permissionService.GetCurrentStaff();

            //2.0 修改工单实体
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldStatus, (int)OrderStatus.Finish));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldFinishTime, DateTime.Now));
            int rows = orderManager.Update(kvps, orderID);
            if (rows == 0)
            {
                throw new JSException(JSErrMsg.ERR_CODE_DATA_MISSING, string.Format(JSErrMsg.ERR_MSG_DATA_MISSING, "工单实体"));
            }

            //3.0 添加工作流实体
            OrderFlowEntity orderflow = new OrderFlowEntity();
            orderflow.OrderID = orderID;
            orderflow.OperatorID = staff.ID;
            orderflow.NextOperatorID = 0;
            orderflow.Operation = (int)OperationEnum.reject;
            orderflow.OperateTime = DateTime.Now;
            orderflow.Remark = "";
            orderflowManager.Insert(orderflow);
        }

        //取消报障单
        public void CancelOrder(Guid orderID)
        {
            // TODO 增加判断，只能取消未处理完的工单
            PermissionService permissionService = new PermissionService();
            EntityManager<OrderEntity> orderManager = new EntityManager<OrderEntity>();
            EntityManager<OrderFlowEntity> orderflowManager = new EntityManager<OrderFlowEntity>();

            //1.0 获取当前员工数据
            StaffEntity staff = permissionService.GetCurrentStaff();

            //2.0 修改工单实体
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldStatus, (int)OrderStatus.Canceled));
            int rows = orderManager.Update(kvps, orderID);
            if (rows == 0)
            {
                throw new JSException(JSErrMsg.ERR_CODE_DATA_MISSING, string.Format(JSErrMsg.ERR_MSG_DATA_MISSING, "工单实体"));
            }

            //3.0 添加工作流实体
            OrderFlowEntity orderflow = new OrderFlowEntity();
            orderflow.OrderID = orderID;
            orderflow.OperatorID = staff.ID;
            orderflow.NextOperatorID = 0;
            orderflow.Operation = (int)OperationEnum.reject;
            orderflow.OperateTime = DateTime.Now;
            orderflow.Remark = "";
            orderflowManager.Insert(orderflow);
        }

        public OrderEntity GetOrderEntity(Guid orderID)
        {
            EntityManager<OrderEntity> orderManager = new EntityManager<OrderEntity>();
            OrderEntity order = orderManager.GetSingle(orderID);
            return order;
        }

        /// <summary>
        /// 获取我的已发起工单
        /// </summary>
        /// <returns></returns>
        public DataTable GetMyStartedOrders(int pageIndex,int pageSize,out int count)
        {
            PermissionService permissionService = new PermissionService();
            ViewManager manager = new ViewManager("VO_Order");
                        
            //1.0 获取当前员工数据
            StaffEntity staff = permissionService.GetCurrentStaff();

            //2.0 构建where从句
            WhereStatement where = new WhereStatement();
            where.Add(OrderEntity.FieldStatus, Comparison.GreaterOrEquals, (int)OrderStatus.Appointing);
            where.Add(OrderEntity.FieldStarterID, Comparison.Equals, staff.ID);

            //3.0 获取已发起的数据
            DataTable dt = manager.GetDataTableByPage(where, out count, pageIndex, pageSize);
            return dt;

        }

        /// <summary>
        /// 获取我的待委派工单
        /// </summary>
        /// <returns></returns>
        public DataTable GetMyAppointingOrders()
        {
            PermissionService permissionService = new PermissionService();
            ViewManager manager = new ViewManager("VO_Order");

            //1.0 获取当前员工数据
            StaffEntity staff = permissionService.GetCurrentStaff();

            //2.0 构建where从句
            WhereStatement where = new WhereStatement();
            where.Add(OrderEntity.FieldStatus, Comparison.Equals, (int)OrderStatus.Appointing);

            //3.0 获取已发起的数据
            int count = 0;
            DataTable dt = manager.GetDataTable(where, out count);
            return dt;
        }

        /// <summary>
        /// 获取我的已委派工单
        /// </summary>
        /// <returns></returns>
        public DataTable GetMyAppointedOrders()
        {
            PermissionService permissionService = new PermissionService();
            ViewManager manager = new ViewManager("VO_Order");

            //1.0 获取当前员工数据
            StaffEntity staff = permissionService.GetCurrentStaff();

            //2.0 构建where从句
            WhereStatement where = new WhereStatement();
            where.Add(OrderEntity.FieldStatus, Comparison.GreaterOrEquals, (int)OrderStatus.Receving);
            where.Add(OrderEntity.FieldAppointerID, Comparison.Equals, staff.ID);

            //3.0 获取已发起的数据
            int count = 0;
            DataTable dt = manager.GetDataTable(where, out count);
            return dt;
        }

        /// <summary>
        /// 获取我的未接收工单
        /// </summary>
        /// <returns></returns>
        public DataTable GetMyRecevingOrders()
        {
            PermissionService permissionService = new PermissionService();
            ViewManager manager = new ViewManager("VO_Order");

            //1.0 获取当前员工数据
            StaffEntity staff = permissionService.GetCurrentStaff();

            //2.0 构建where从句
            WhereStatement where = new WhereStatement();
            where.Add(OrderEntity.FieldStatus, Comparison.Equals, (int)OrderStatus.Receving);
            where.Add(OrderEntity.FieldNextOperatorID, Comparison.Equals, staff.ID);

            //3.0 获取已发起的数据
            int count = 0;
            DataTable dt = manager.GetDataTable(where, out count);
            return dt;
        }

        /// <summary>
        /// 获取我处理中的工单
        /// </summary>
        /// <returns></returns>
        public DataTable GetMyHandlingOrders()
        {
            PermissionService permissionService = new PermissionService();
            ViewManager manager = new ViewManager("VO_Order");

            //1.0 获取当前员工数据
            StaffEntity staff = permissionService.GetCurrentStaff();

            //2.0 构建where从句
            WhereStatement where = new WhereStatement();
            where.Add(OrderEntity.FieldStatus, Comparison.Equals, (int)OrderStatus.Handling);
            where.Add(OrderEntity.FieldHandlerID, Comparison.Equals, staff.ID);

            //3.0 获取已发起的数据
            int count = 0;
            DataTable dt = manager.GetDataTable(where, out count);
            return dt;
        }

        /// <summary>
        /// 获取我已处理的工单
        /// </summary>
        /// <returns></returns>
        public DataTable GetMyHandledOrders()
        {
            PermissionService permissionService = new PermissionService();
            ViewManager manager = new ViewManager("VO_Order");

            //1.0 获取当前员工数据
            StaffEntity staff = permissionService.GetCurrentStaff();

            //2.0 构建where从句
            WhereStatement where = new WhereStatement();
            where.Add(OrderEntity.FieldStatus, Comparison.GreaterOrEquals, (int)OrderStatus.Checking);
            where.Add(OrderEntity.FieldHandlerID, Comparison.Equals, staff.ID);

            //3.0 获取已发起的数据
            int count = 0;
            DataTable dt = manager.GetDataTable(where, out count);
            return dt;
        }

        /// <summary>
        /// 获取工单处理过程明细
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public DataTable GetOrderHandleDetails(Guid orderID)
        {
            //只能查视图
            ViewManager manager = new ViewManager("VO_OrderHandleDetails");

            WhereStatement where = new WhereStatement();
            where.Add(OrderHandleDetailEntity.FieldOrderID, Comparison.Equals, orderID.ToString());

            OrderByStatement orderby = new OrderByStatement();
            orderby.Add(OrderHandleDetailEntity.FieldHandleTime, Sorting.Descending);

            int count = 0;
            DataTable dt = manager.GetDataTable(where, out count, orderby);
            return dt;
        }

        /// <summary>
        /// 获取工单处理流程
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        public DataTable GetOrderFlows(Guid orderID)
        {
            //只能查视图
            ViewManager manager = new ViewManager("VO_OrderFlow");

            WhereStatement where = new WhereStatement();
            where.Add(OrderFlowEntity.FieldOrderID,Comparison.Equals,orderID.ToString());

            OrderByStatement orderby = new OrderByStatement();
            orderby.Add(OrderFlowEntity.FieldOperateTime,Sorting.Descending);

            int count = 0;
            DataTable dt = manager.GetDataTable(where,out count, orderby);
            return dt;
        }

        public DataRow GetOrderDetail(Guid orderID)
        {
            //只能查视图
            ViewManager manager = new ViewManager("VO_Order");

            DataRow dr = manager.GetSingle(orderID, OrderEntity.FieldID);
            return dr;
        }

        public DataTable GetOrderHandlers(Guid orderID)
        {
            //只能查视图
            ViewManager manager = new ViewManager("VO_OrderHandlers");

            WhereStatement where = new WhereStatement();
            where.Add(OrderHandlerEntity.FieldOrderID, Comparison.Equals, orderID.ToString());
            
            OrderByStatement orderby = new OrderByStatement();
            orderby.Add(OrderHandlerEntity.FieldIsLeader, Sorting.Descending);

            int count = 0;
            DataTable dt = manager.GetDataTable(where, out count, orderby);
            return dt;
        }

        private int GetLeaderHandlerID(int[] orderHandlerIDs)
        {
            int count = 0;
            EntityManager<OrderHandlerEntity> manager = new EntityManager<OrderHandlerEntity>();
            WhereStatement where = new WhereStatement();
            where.Add(OrderHandlerEntity.FieldID,Comparison.In,orderHandlerIDs);

            List<OrderHandlerEntity> list = manager.GetList(where, out count);
            int leaderID = GetLeaderHandlerID(list);
            return leaderID;
        }

        /// <summary>
        /// 获取工单处理的领队ID
        /// </summary>
        /// <param name="orderHandlers"></param>
        /// <returns></returns>
        private int GetLeaderHandlerID(List<OrderHandlerEntity> orderHandlers)
        {
            int leaderID = 0;
            foreach (OrderHandlerEntity orderHandler in orderHandlers)
            {
                if (orderHandler.IsLeader == (int)TrueFalse.True)
                {
                    leaderID = (int)orderHandler.HandlerID;
                    break;
                }
            }
            if (leaderID == 0)
            {
                throw new JSException(JSErrMsg.ERR_CODE_NOLEADER, string.Format(JSErrMsg.ERR_MSG_NOLEADER));
            }
            return leaderID;
        }

        /// <summary>
        /// 返回工单号（不支持事务）
        /// </summary>
        /// <returns></returns>
        private string GetNewOrderNo()
        {
            List<IDbDataParameter> outDbParameters = new List<IDbDataParameter>();
            IDbHelper dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.CenterDbConnectionString);

            IDbDataParameter[] dbParameters = new IDbDataParameter[] { dbHelper.MakeOutParam("sn", SqlDbType.VarChar.ToString(), 14) };
            DataTable dt = dbHelper.ExecuteProcedureForDataTable("[uspSN]", "JSNet", dbParameters, out outDbParameters);

            string orderNo = outDbParameters.FirstOrDefault(p => p.ParameterName == dbHelper.GetParameter("sn")).Value.ToString();
            return orderNo;
        }

    }
}
