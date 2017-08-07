using CodeEngine.Framework.QueryBuilder;
using CodeEngine.Framework.QueryBuilder.Enums;
using JSNet.BaseSys;
using JSNet.Manager;
using JSNet.Model;
using System;
using System.Collections.Generic;
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

            //2.0 添加工单实体
            order.Status = (int)OrderStatus.Started;
            string orderID = orderManager.Insert(order);

            //3.0 添加工作流实体
            OrderFlowEntity orderFlow = new OrderFlowEntity();
            orderFlow.OrderID = Guid.Parse(orderID);
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
            kvps.Add(new KeyValuePair<string,object>(OrderEntity.FieldStatus,(int)OrderStatus.Appointed));
            int rows = orderManager.Update(kvps, orderID);
            if (rows == 0)
            {
                throw new JSException(JSErrMsg.ERR_CODE_OBJECT_MISSING, string.Format(JSErrMsg.ERR_MSG_OBJECT_MISSING, "工单实体"));
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
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldStatus, (int)OrderStatus.Received));
            int rows = orderManager.Update(kvps, orderID);
            if (rows == 0)
            {
                throw new JSException(JSErrMsg.ERR_CODE_OBJECT_MISSING, string.Format(JSErrMsg.ERR_MSG_OBJECT_MISSING, "工单实体"));
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
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldStatus, (int)OrderStatus.Handled));
            int rows = orderManager.Update(kvps, orderID);
            if (rows == 0)
            {
                throw new JSException(JSErrMsg.ERR_CODE_OBJECT_MISSING, string.Format(JSErrMsg.ERR_MSG_OBJECT_MISSING, "工单实体"));
            }

            //2.1 获取工单实体，以备获取发起人信息
            OrderEntity order = orderManager.GetSingle(orderID);
            if (order == null)
            {
                throw new JSException(JSErrMsg.ERR_CODE_OBJECT_MISSING, string.Format(JSErrMsg.ERR_MSG_OBJECT_MISSING, "工单实体"));
            }

            //3.0 添加工作流实体
            OrderFlowEntity orderflow = new OrderFlowEntity();
            orderflow.OrderID = orderID;
            orderflow.OperatorID = staff.ID;
            orderflow.NextOperatorID = order.StaffID;//传回发起人验收。
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
                throw new JSException(JSErrMsg.ERR_CODE_OBJECT_MISSING, string.Format(JSErrMsg.ERR_MSG_OBJECT_MISSING, "工单实体"));
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
                throw new JSException(JSErrMsg.ERR_CODE_OBJECT_MISSING, string.Format(JSErrMsg.ERR_MSG_OBJECT_MISSING, "工单实体"));
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
                throw new JSException(JSErrMsg.ERR_CODE_OBJECT_MISSING, string.Format(JSErrMsg.ERR_MSG_OBJECT_MISSING, "工单实体"));
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

        public List<OrderEntity> GetMyStartedOrders()
        {
            List<OrderEntity> list = new List<OrderEntity>();
            return list;
        }

        public List<OrderEntity> GetMyRecevingOrders()
        {
            List<OrderEntity> list = new List<OrderEntity>();
            return list;
        }

        public List<OrderEntity> GetMyHandlingOrders()
        {
            List<OrderEntity> list = new List<OrderEntity>();
            return list;
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

    }
}
