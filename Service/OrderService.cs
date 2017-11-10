using CodeEngine.Framework.QueryBuilder;
using CodeEngine.Framework.QueryBuilder.Enums;
using JSNet.BaseSys;
using JSNet.DbUtilities;
using JSNet.Manager;
using JSNet.Model;
using JSNet.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace JSNet.Service
{
    public class OrderService:BaseService
    {
        //发起报障单
        public string StartOrder(OrderEntity order)
        {
            //1.0 添加工单实体
            order.ID = Guid.NewGuid();
            order.Status = (int)OrderStatus.Appointing;
            order.StarterID = CurrentStaff.ID;
            order.OperatorID = CurrentStaff.ID;
            order.StartTime = DateTime.Now;
            order.OperateTime = DateTime.Now;
            if (string.IsNullOrEmpty(order.Attn) || string.IsNullOrEmpty(order.AttnTel))
            {
                order.Attn = CurrentStaff.Name;
                order.AttnTel = CurrentStaff.Tel;
            }
            EntityManager<OrderEntity> orderManager = new EntityManager<OrderEntity>();
            string s = orderManager.Insert(order);

            //2.0 添加工作流实体
            OrderFlowEntity orderFlow = new OrderFlowEntity();
            orderFlow.OrderID = order.ID;
            orderFlow.OperatorID = CurrentStaff.ID;
            orderFlow.NextOperatorID = 0;
            orderFlow.Operation = (int)OperationEnum.Start;
            orderFlow.OperateTime = DateTime.Now;
            orderFlow.Remark = "";
            EntityManager<OrderFlowEntity> orderflowManager = new EntityManager<OrderFlowEntity>();
            orderflowManager.Insert(orderFlow);

            //3.0 微信推送
            ViewManager vmanager = new ViewManager("VO_Order");
            DataRow dr = vmanager.GetSingle(order.ID, "ID");

            //推送给委派人
            KawuService kawuService = new KawuService();
            List<int> staffIDs = GetAppointersStaffIDs();
            foreach (int staffid in staffIDs)
            {
                kawuService.CommonOrder_VXPushMsg(staffid, "您有新的工单，请及时委派！", dr);
            }

            return order.ID.ToString();
        }

        //委派工作
        public void AppointOrder(Guid orderID, List<OrderHandlerEntity> handlers)
        {
            //验证流程
            OrderEntity sourceOrder = GetOrderEntity(orderID);
            ValidateOrderFlows((OrderStatus)sourceOrder.Status, OrderStatus.Receving);

            //1.0 修改工单实体
            List<KeyValuePair<string,object>> kvps = new List<KeyValuePair<string,object>>();
            kvps.Add(new KeyValuePair<string,object>(OrderEntity.FieldStatus,(int)OrderStatus.Receving));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldAppointerID, CurrentStaff.ID));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldOperatorID, CurrentStaff.ID));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldNextOperatorID, GetLeaderHandlerID(handlers)));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldOperateTime,DateTime.Now));
            EntityManager<OrderEntity> orderManager = new EntityManager<OrderEntity>();
            int rows = orderManager.Update(kvps, orderID);
            if (rows == 0)
            {
                throw new Exception(string.Format(JSErrMsg.ERR_MSG_DATA_MISSING, "工单ID为" + orderID));
            }

            //2.0 添加工作流实体
            OrderFlowEntity orderFlow = new OrderFlowEntity();
            orderFlow.OrderID = orderID;
            orderFlow.OperatorID = CurrentStaff.ID;
            orderFlow.NextOperatorID = GetLeaderHandlerID(handlers);
            orderFlow.Operation = (int)OperationEnum.Appoint;
            orderFlow.OperateTime = DateTime.Now;
            orderFlow.Remark = "";
            EntityManager<OrderFlowEntity> orderflowManager = new EntityManager<OrderFlowEntity>();
            orderflowManager.Insert(orderFlow);

            //3.0 添加工单处理者
            EntityManager<OrderHandlerEntity> handlerManager = new EntityManager<OrderHandlerEntity>();
            handlerManager.Insert(handlers);

            //4.0 微信推送
            ViewManager vmanager = new ViewManager("VO_Order");
            DataRow dr = vmanager.GetSingle(orderID,"ID");

            KawuService kawuService = new KawuService();
            kawuService.CommonOrder_VXPushMsg((int)orderFlow.NextOperatorID, "您有一条新工单，请及时受理！", dr);

        }

        //受理报障单
        public void ReceiveOrder(Guid orderID)
        {
            //验证流程
            OrderEntity sourceOrder = GetOrderEntity(orderID);
            ValidateOrderFlows((OrderStatus)sourceOrder.Status, OrderStatus.Handling);

            //1.0 修改工单实体
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldStatus, (int)OrderStatus.Handling));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldOperatorID, CurrentStaff.ID));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldNextOperatorID, CurrentStaff.ID));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldHandlerID, CurrentStaff.ID));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldOperateTime, DateTime.Now));
            EntityManager<OrderEntity> orderManager = new EntityManager<OrderEntity>();
            int rows = orderManager.Update(kvps, orderID);
            if (rows == 0)
            {
                throw new Exception(string.Format(JSErrMsg.ERR_MSG_DATA_MISSING, "工单ID为" + orderID));
            }

            //2.0 添加工作流实体
            OrderFlowEntity orderflow = new OrderFlowEntity();
            orderflow.OrderID = orderID;
            orderflow.OperatorID = CurrentStaff.ID;
            orderflow.NextOperatorID = CurrentStaff.ID;
            orderflow.Operation = (int)OperationEnum.Receive;
            orderflow.OperateTime = DateTime.Now;
            orderflow.Remark = "";
            EntityManager<OrderFlowEntity> orderflowManager = new EntityManager<OrderFlowEntity>();
            orderflowManager.Insert(orderflow);

            //3.0 添加处理进度
            OrderHandleDetailEntity orderHandleDetail = new OrderHandleDetailEntity();
            orderHandleDetail.OrderID = orderID;
            orderHandleDetail.HandleType = (int)OrderHandleType.Others;
            orderHandleDetail.HandleDetail = "受理";
            orderHandleDetail.Remark = "已受理，准备处理。";
            AddHandleDetail(orderHandleDetail);

            //4.0 微信推送
            ViewManager vmanager = new ViewManager("VO_Order");
            DataRow dr = vmanager.GetSingle(orderID, "ID");

            //获取协助者
            List<OrderHandlerEntity> handlers = GetOrderHandlers(orderID,true);

            //推送给协助者
            KawuService kawuService = new KawuService();
            foreach(OrderHandlerEntity handler in handlers)
            {
                kawuService.CommonOrder_VXPushMsg((int)handler.HandlerID, "您有新的工单，请及时处理！", dr);
            }

            //推送给发起人
            kawuService.AcceptOrder_VXPushMsg(Convert.ToInt32(dr["StarterID"].ToString()), "您发起的工单已受理！", dr);
        }

        //增加处理明细
        public void AddHandleDetail(OrderHandleDetailEntity orderHandleDetail)
        {
            //2.0 添加工作处理明细实体
            orderHandleDetail.HandlerID = CurrentStaff.ID;
            orderHandleDetail.HandleTime = DateTime.Now;
            if (string.IsNullOrEmpty(orderHandleDetail.HandleDetail))
            {
                orderHandleDetail.HandleDetail = EnumExtensions.ToDescription((OrderHandleType)orderHandleDetail.HandleType);
            }
            orderHandleDetail.Progress = 0;
            EntityManager<OrderHandleDetailEntity> orderHandleDetailManager = new EntityManager<OrderHandleDetailEntity>();
            orderHandleDetailManager.Insert(orderHandleDetail);
        }

        public void AddHandledDetail(OrderHandleDetailEntity orderHandleDetail)
        {
            //4.1 添加处理进度
            orderHandleDetail.OrderID = orderHandleDetail.OrderID;
            orderHandleDetail.HandleType = (int)OrderHandleType.WanCheng;
            orderHandleDetail.Remark = orderHandleDetail.Remark;
            AddHandleDetail(orderHandleDetail);

            //4.2 添加处理进度
            OrderHandleDetailEntity orderHandleDetail1 = new OrderHandleDetailEntity();
            orderHandleDetail1.OrderID = orderHandleDetail.OrderID;
            orderHandleDetail1.HandleType = (int)OrderHandleType.DaiYanShou;
            orderHandleDetail1.Remark = "等待验收。";
            AddHandleDetail(orderHandleDetail1);
        }

        public void AddHandlingOrderDetail(OrderHandleDetailEntity orderHandleDetail)
        {
            //1.0 修改工单实体
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldStatus, (int)OrderStatus.Handling));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldOperatorID, CurrentStaff.ID));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldNextOperatorID, CurrentStaff.ID));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldHandlerID, CurrentStaff.ID));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldOperateTime, DateTime.Now));
            EntityManager<OrderEntity> orderManager = new EntityManager<OrderEntity>();
            int rows = orderManager.Update(kvps, orderHandleDetail.OrderID);
            if (rows == 0)
            {
                throw new Exception(string.Format(JSErrMsg.ERR_MSG_DATA_MISSING, "工单ID为" + orderHandleDetail.OrderID));
            }

            //2.0 添加工作流实体
            OrderFlowEntity orderflow = new OrderFlowEntity();
            orderflow.OrderID = orderHandleDetail.OrderID;
            orderflow.OperatorID = CurrentStaff.ID;
            orderflow.NextOperatorID = CurrentStaff.ID;
            orderflow.Operation = (int)OperationEnum.Receive;
            orderflow.OperateTime = DateTime.Now;
            orderflow.Remark = "";
            EntityManager<OrderFlowEntity> orderflowManager = new EntityManager<OrderFlowEntity>();
            orderflowManager.Insert(orderflow);

            //3.0 添加处理进度
            AddHandleDetail(orderHandleDetail);

        }

        public void AddOrderGoodsRel(List<OrderGoodsRelEntity> list)
        {
            EntityManager<OrderGoodsRelEntity> manager = new EntityManager<OrderGoodsRelEntity>();
            manager.Insert(list);
        }

        //报障处理完毕
        public void HandledOrder(Guid orderID, string handledPhotoPath, string handledPhotoPath1)
        {
            //1.1 //验证流程
            OrderEntity sourceOrder = GetOrderEntity(orderID);
            ValidateOrderFlows((OrderStatus)sourceOrder.Status, OrderStatus.Checking);

            //2.0 修改工单实体
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldOperatorID, CurrentStaff.ID));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldNextOperatorID, sourceOrder.StarterID));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldStatus, (int)OrderStatus.Checking));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldOperateTime, DateTime.Now));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldHandledPhotoPath, handledPhotoPath));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldHandledPhotoPath1, handledPhotoPath1));
            EntityManager<OrderEntity> orderManager = new EntityManager<OrderEntity>();
            int rows = orderManager.Update(kvps, orderID);
            if (rows == 0)
            {
                throw new Exception(string.Format(JSErrMsg.ERR_MSG_DATA_MISSING, "工单ID为" + orderID));
            }

            //3.0 添加工作流实体
            OrderFlowEntity orderflow = new OrderFlowEntity();
            orderflow.OrderID = orderID;
            orderflow.OperatorID = CurrentStaff.ID;
            orderflow.NextOperatorID = sourceOrder.StarterID;//传回发起人验收。
            orderflow.Operation = (int)OperationEnum.Handle;
            orderflow.OperateTime = DateTime.Now;
            orderflow.Remark = "";
            EntityManager<OrderFlowEntity> orderflowManager = new EntityManager<OrderFlowEntity>();
            orderflowManager.Insert(orderflow);

            //4.0 微信推送
            ViewManager vmanager = new ViewManager("VO_Order");
            DataRow dr = vmanager.GetSingle(orderID, "ID");

            //推送给发起人
            KawuService kawuService = new KawuService();
            kawuService.CheckOrder_VXPushMsg(Convert.ToInt32(dr["StarterID"].ToString()), "您发起的工单已处理完成，请及时验收！", dr);
        }

        //驳回报障，需继续处理
        public void RejectOrder(Guid orderID,string remark)
        {
            //验证流程
            OrderEntity sourceOrder = GetOrderEntity(orderID);
            ValidateOrderFlows((OrderStatus)sourceOrder.Status, OrderStatus.Rejected);

            //1.1 获取工单处理者列表，必备获取领队人的信息
            int count = 0;
            WhereStatement where = new WhereStatement();
            where.Add(OrderHandlerEntity.FieldOrderID, Comparison.Equals, orderID);
            EntityManager<OrderHandlerEntity> orderHandlerManager = new EntityManager<OrderHandlerEntity>();
            List<OrderHandlerEntity> orderHandlers = orderHandlerManager.GetList(where, out count);

            //2.0 修改工单实体
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldOperatorID, CurrentStaff.ID));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldNextOperatorID, GetLeaderHandlerID(orderHandlers)));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldStatus, (int)OrderStatus.Rejected));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldOperateTime, DateTime.Now));
            EntityManager<OrderEntity> orderManager = new EntityManager<OrderEntity>();
            int rows = orderManager.Update(kvps, orderID);
            if (rows == 0)
            {
                throw new Exception(string.Format(JSErrMsg.ERR_MSG_DATA_MISSING, "工单ID为" + orderID));
            }

            //3.0 添加工作流实体
            OrderFlowEntity orderflow = new OrderFlowEntity();
            orderflow.OrderID = orderID;
            orderflow.OperatorID = CurrentStaff.ID;
            orderflow.NextOperatorID = GetLeaderHandlerID(orderHandlers);
            orderflow.Operation = (int)OperationEnum.reject;
            orderflow.OperateTime = DateTime.Now;
            orderflow.Remark = remark;
            EntityManager<OrderFlowEntity> orderflowManager = new EntityManager<OrderFlowEntity>();
            orderflowManager.Insert(orderflow);

            OrderHandleDetailEntity orderHandleDetail = new OrderHandleDetailEntity();
            orderHandleDetail.OrderID = orderID;
            orderHandleDetail.HandleType = (int)OrderHandleType.BoHui;
            orderHandleDetail.HandleDetail = "工单验收不通过，被驳回。";
            orderHandleDetail.Remark = string.IsNullOrEmpty(remark) ? "" : "驳回原因：" + remark;
            AddHandleDetail(orderHandleDetail);

            //4.0 微信推送
            ViewManager vmanager = new ViewManager("VO_Order");
            DataRow dr = vmanager.GetSingle(orderID, "ID");

            //获取处理所有处理者
            List<OrderHandlerEntity> handlers = GetOrderHandlers(orderID);

            //推送给所有处理者
            KawuService kawuService = new KawuService();
            foreach (OrderHandlerEntity handler in handlers)
            {
                kawuService.CommonOrder_VXPushMsg((int)handler.HandlerID, "您处理的工单验收不通过，请重新处理！", dr);
            }
        }

        //报障验收完成
        public void FinishOrder(Guid orderID)
        {
            //验证流程
            OrderEntity sourceOrder = GetOrderEntity(orderID);
            ValidateOrderFlows((OrderStatus)sourceOrder.Status, OrderStatus.Finish);

            //1.0 修改工单实体
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldOperatorID, CurrentStaff.ID));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldNextOperatorID, null));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldStatus, (int)OrderStatus.Finish));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldFinishTime, DateTime.Now));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldOperateTime, DateTime.Now));
            EntityManager<OrderEntity> orderManager = new EntityManager<OrderEntity>();
            int rows = orderManager.Update(kvps, orderID);
            if (rows == 0)
            {
                throw new Exception(string.Format(JSErrMsg.ERR_MSG_DATA_MISSING, "工单ID为" + orderID));
            }

            //2.0 添加工作流实体
            OrderFlowEntity orderflow = new OrderFlowEntity();
            orderflow.OrderID = orderID;
            orderflow.OperatorID = CurrentStaff.ID;
            orderflow.NextOperatorID = 0;
            orderflow.Operation = (int)OperationEnum.Check;
            orderflow.OperateTime = DateTime.Now;
            orderflow.Remark = "";
            EntityManager<OrderFlowEntity> orderflowManager = new EntityManager<OrderFlowEntity>();
            orderflowManager.Insert(orderflow);

            //3.0 添加进度为完成
            OrderHandleDetailEntity orderHandleDetail = new OrderHandleDetailEntity();
            orderHandleDetail.OrderID = orderID;
            orderHandleDetail.HandleType = (int)OrderHandleType.WanCheng;
            orderHandleDetail.HandleDetail = "验收通过";
            orderHandleDetail.Remark = "工单验收通过，流程结束。";
            AddHandleDetail(orderHandleDetail);

            //4.0 微信推送
            ViewManager vmanager = new ViewManager("VO_Order");
            DataRow dr = vmanager.GetSingle(orderID, "ID");

            //推送给委托人
            KawuService kawuService = new KawuService();
            kawuService.FinishOrder_VXPushMsg(Convert.ToInt32(dr["AppointerID"].ToString()), "您委派的工单已处理完成！", dr);

            ////推送给所有处理人
            List<OrderHandlerEntity> handlers = GetOrderHandlers(orderID);
            foreach (OrderHandlerEntity handler in handlers)
            {
                kawuService.CommonOrder_VXPushMsg((int)handler.HandlerID, "您处理的工单已验收通过！", dr);
            }
        }

        //撤销报障单
        public void CancelOrder(Guid orderID)
        {
            //验证流程
            OrderEntity sourceOrder = GetOrderEntity(orderID);
            ValidateOrderFlows((OrderStatus)sourceOrder.Status, OrderStatus.Canceled);

            //1.0 修改工单实体
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldOperatorID, CurrentStaff.ID));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldStatus, (int)OrderStatus.Canceled));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldOperateTime, DateTime.Now));
            EntityManager<OrderEntity> orderManager = new EntityManager<OrderEntity>();
            int rows = orderManager.Update(kvps, orderID);
            if (rows == 0)
            {
                throw new Exception(string.Format(JSErrMsg.ERR_MSG_DATA_MISSING, "工单ID为" + orderID));
            }

            //2.0 添加工作流实体
            OrderFlowEntity orderflow = new OrderFlowEntity();
            orderflow.OrderID = orderID;
            orderflow.OperatorID = CurrentStaff.ID;
            orderflow.NextOperatorID = 0;
            orderflow.Operation = (int)OperationEnum.Cancel;
            orderflow.OperateTime = DateTime.Now;
            orderflow.Remark = "";
            EntityManager<OrderFlowEntity> orderflowManager = new EntityManager<OrderFlowEntity>();
            orderflowManager.Insert(orderflow);
        }

        public OrderEntity GetOrderEntity(Guid orderID)
        {
            EntityManager<OrderEntity> orderManager = new EntityManager<OrderEntity>();
            OrderEntity order = orderManager.GetSingle(orderID);
            return order;
        }

        public DataTable GetOrdersDTByRoles(RoleEntity role,JSDictionary dic, int pageIndex, int pageSize, out int count)
        {
            //1.0 构建资源对象
            PermissionService permissionService = new PermissionService();
            string scopeConstraint = "";
            List<string> scopeIDs = permissionService.GetAuthorizedScopeIDByRole(role, "OrderSys_Data.Orders", out scopeConstraint);

            //2.0 构建where从句
            WhereStatement where = new WhereStatement();
            if (dic.ContainsKey(OrderEntity.FieldStartTime + "1")
                && dic.ContainsKey(OrderEntity.FieldStartTime + "2"))
            {
                WhereClause clause = new WhereClause(OrderEntity.FieldStartTime, Comparison.GreaterOrEquals, dic[OrderEntity.FieldStartTime + "1"]);
                clause.AddClause(LogicOperator.And, Comparison.LessOrEquals, dic[OrderEntity.FieldStartTime + "2"]);
                where.Add(clause);
            }
            if (dic.ContainsKey(OrderEntity.FieldFinishTime + "1")
                && dic.ContainsKey(OrderEntity.FieldFinishTime + "2"))
            {
                WhereClause clause = new WhereClause(OrderEntity.FieldFinishTime, Comparison.GreaterOrEquals, dic[OrderEntity.FieldFinishTime + "1"]);
                clause.AddClause(LogicOperator.And, Comparison.LessOrEquals, dic[OrderEntity.FieldFinishTime + "2"]);
                where.Add(clause);
            }

            if (scopeIDs.Count > 0)
            {
                //显示指定部门的工单
                where.Add(scopeConstraint, Comparison.In, scopeIDs.ToArray());
            }
            else
            {
                if (CurrentRole.ID == 1)
                {
                    //超级管理员，显示所有内容
                    where.Add("1", Comparison.Equals, "1");
                }
                else
                {
                    //默认显示自己的内容
                    where.Add(OrderEntity.FieldStarterID, Comparison.Equals, CurrentStaff.ID);
                }
            }

            OrderByStatement orderby = new OrderByStatement();
            orderby.Add(OrderEntity.FieldStartTime, Sorting.Descending);

            //3.0 获取已发起的数据
            ViewManager manager = new ViewManager("VO_Order");
            DataTable dt = manager.GetDataTableByPage(where, out count, pageIndex, pageSize, orderby);
            return dt;
        }

        /// <summary>
        /// 获取我的已发起工单
        /// </summary>
        /// <returns></returns>
        public DataTable GetMyStartedOrders(JSDictionary dic,int pageIndex,int pageSize,out int count)
        {
            //1.0 构建资源对象
            PermissionService permissionService = new PermissionService();
            string scopeConstraint = "";
            List<string> scopeIDs = permissionService.GetAuthorizedScopeIDByRole(CurrentRole, "OrderSys_Data.StartedOrders",out scopeConstraint);

            //2.0 构建where从句
            WhereStatement where = new WhereStatement();
            if (!dic.ContainsKey(OrderEntity.FieldStatus))
            {
                where.Add(OrderEntity.FieldStatus, Comparison.NotEquals, (int)OrderStatus.Canceled);
            }
            else
            {
                where.Add(OrderEntity.FieldStatus, Comparison.Equals, dic[OrderEntity.FieldStatus]);
            }
            if (dic.ContainsKey(OrderEntity.FieldPriority))
            {
                where.Add(OrderEntity.FieldPriority, Comparison.Equals, dic[OrderEntity.FieldPriority]);
            }
            if (dic.ContainsKey(OrderEntity.FieldBookingTime))
            {
                where.Add(OrderEntity.FieldBookingTime, Comparison.Equals, dic[OrderEntity.FieldBookingTime]);
            }
            if (dic.ContainsKey(OrderEntity.FieldContent))
            {
                where.Add(OrderEntity.FieldContent, Comparison.Like, "%" + dic[OrderEntity.FieldContent] + "%");
            }
            if (dic.ContainsKey(OrderEntity.FieldWorkingLocation))
            {
                where.Add(OrderEntity.FieldWorkingLocation, Comparison.Like, "%" + dic[OrderEntity.FieldWorkingLocation] + "%");
            }


            if (scopeIDs.Count > 0)
            {
                //显示指定部门的工单
                where.Add(scopeConstraint, Comparison.In, scopeIDs.ToArray());
            }
            else
            {
                if (CurrentRole.ID == 1)
                {
                    //超级管理员，显示所有内容
                    where.Add("1", Comparison.Equals, "1");
                }
                else
                {
                    //默认显示自己的内容
                    where.Add(OrderEntity.FieldStarterID, Comparison.Equals, CurrentStaff.ID);
                }
            }

            OrderByStatement orderby = new OrderByStatement();
            orderby.Add(OrderEntity.FieldStartTime, Sorting.Descending);

            //3.0 获取已发起的数据
            ViewManager manager = new ViewManager("VO_Order");
            DataTable dt = manager.GetDataTableByPage(where, out count, pageIndex, pageSize,orderby);
            return dt;

        }

        /// <summary>
        /// 获取我的待委派工单
        /// </summary>
        /// <returns></returns>
        public DataTable GetMyAppointingOrders(JSDictionary dic,int pageIndex, int pageSize, out int count)
        {
            //1.0 构建资源对象
            PermissionService permissionService = new PermissionService();
            string scopeConstraint = "";
            List<string> scopeIDs = permissionService.GetAuthorizedScopeIDByRole(CurrentRole, "OrderSys_Data.AppointingOrders", out scopeConstraint);

            //2.0 构建where从句
            WhereStatement where = new WhereStatement();
            where.Add(OrderEntity.FieldStatus, Comparison.Equals, (int)OrderStatus.Appointing);
            if (dic.ContainsKey(OrderEntity.FieldPriority))
            {
                where.Add(OrderEntity.FieldPriority, Comparison.Equals, dic[OrderEntity.FieldPriority]);
            }
            if (dic.ContainsKey(OrderEntity.FieldBookingTime))
            {
                where.Add(OrderEntity.FieldBookingTime, Comparison.Equals, dic[OrderEntity.FieldBookingTime]);
            }
            if (dic.ContainsKey(OrderEntity.FieldContent))
            {
                where.Add(OrderEntity.FieldContent, Comparison.Like, "%" + dic[OrderEntity.FieldContent] + "%");
            }
            if (dic.ContainsKey(OrderEntity.FieldWorkingLocation))
            {
                where.Add(OrderEntity.FieldWorkingLocation, Comparison.Like, "%" + dic[OrderEntity.FieldWorkingLocation] + "%");
            }

            //显示指定部门的工单
            if (scopeIDs.Count > 0)
            {
                where.Add(scopeConstraint, Comparison.In, scopeIDs.ToArray());
            }
            else
            {
                if (CurrentRole.ID == 1)
                {
                    //超级管理员，显示所有内容
                    where.Add("1", Comparison.Equals, "1");
                }
                else
                {
                    where.Add("1", Comparison.Equals, "0");
                }
            }

            //2.1 构建orderby从句
            OrderByStatement orderby = new OrderByStatement();
            orderby.Add(OrderEntity.FieldPriority, Sorting.Descending);
            orderby.Add(OrderEntity.FieldBookingTime, Sorting.Ascending);

            //3.0 获取已发起的数据
            ViewManager manager = new ViewManager("VO_Order");
            DataTable dt = manager.GetDataTableByPage(where, out count, pageIndex, pageSize, orderby);
            return dt;
        }

        /// <summary>
        /// 获取我的已委派工单
        /// </summary>
        /// <returns></returns>
        public DataTable GetMyAppointedOrders(JSDictionary dic,int pageIndex, int pageSize, out int count)
        {
            //1.0 构建资源对象
            PermissionService permissionService = new PermissionService();
            string scopeConstraint = "";
            List<string> scopeIDs = permissionService.GetAuthorizedScopeIDByRole(CurrentRole, "OrderSys_Data.AppointedOrders", out scopeConstraint);

            //2.0 构建where从句
            WhereStatement where = new WhereStatement();
            if (!dic.ContainsKey(OrderEntity.FieldStatus))
            {
                where.Add(OrderEntity.FieldStatus, Comparison.GreaterOrEquals, (int)OrderStatus.Receving);
            }
            else
            {
                //只能搜索已委派的工单
                if (Convert.ToInt32(dic[OrderEntity.FieldStatus]) >= (int)OrderStatus.Receving)
                {
                    where.Add(OrderEntity.FieldStatus, Comparison.Equals, dic[OrderEntity.FieldStatus]);
                }
                else
                {
                    where.Add("1", Comparison.NotEquals, "0");
                }
            }
            if (dic.ContainsKey(OrderEntity.FieldPriority))
            {
                where.Add(OrderEntity.FieldPriority, Comparison.Equals, dic[OrderEntity.FieldPriority]);
            }
            if (dic.ContainsKey(OrderEntity.FieldBookingTime))
            {
                where.Add(OrderEntity.FieldBookingTime, Comparison.Equals, dic[OrderEntity.FieldBookingTime]);
            }
            if (dic.ContainsKey(OrderEntity.FieldContent))
            {
                where.Add(OrderEntity.FieldContent, Comparison.Like, "%" + dic[OrderEntity.FieldContent] + "%");
            }
            if (dic.ContainsKey(OrderEntity.FieldWorkingLocation))
            {
                where.Add(OrderEntity.FieldWorkingLocation, Comparison.Like, "%" + dic[OrderEntity.FieldWorkingLocation] + "%");
            }

            if (scopeIDs.Count > 0)
            {
                //显示指定部门的工单
                where.Add(scopeConstraint, Comparison.In, scopeIDs.ToArray());
            }
            else
            {
                if (CurrentRole.ID == 1)
                {
                    //超级管理员，显示所有内容
                    where.Add("1", Comparison.Equals, "1");
                }
                else
                {
                    where.Add(OrderEntity.FieldAppointerID, Comparison.Equals, CurrentStaff.ID);
                }
            }

            //2.1 构建orderby 从句
            OrderByStatement orderby = new OrderByStatement();
            orderby.Add(OrderEntity.FieldPriority, Sorting.Descending);
            orderby.Add(OrderEntity.FieldBookingTime, Sorting.Ascending);

            //3.0 获取已发起的数据
            ViewManager manager = new ViewManager("VO_Order");
            DataTable dt = manager.GetDataTableByPage(where, out count, pageIndex, pageSize,orderby);
            return dt;
        }

        /// <summary>
        /// 获取我的未接收工单
        /// </summary>
        /// <returns></returns>
        public DataTable GetMyReceivingOrders(JSDictionary dic ,int pageIndex, int pageSize, out int count)
        {
            //1.0 构建资源对象
            PermissionService permissionService = new PermissionService();
            string scopeConstraint = "";
            List<string> scopeIDs = permissionService.GetAuthorizedScopeIDByRole(CurrentRole, "OrderSys_Data.ReceivingOrders", out scopeConstraint);
            
            //2.0 构建where从句
            WhereStatement where = new WhereStatement();
            where.Add(OrderEntity.FieldStatus, Comparison.Equals, (int)OrderStatus.Receving);
            if (dic.ContainsKey(OrderEntity.FieldPriority))
            {
                where.Add(OrderEntity.FieldPriority, Comparison.Equals, dic[OrderEntity.FieldPriority]);
            }
            if (dic.ContainsKey(OrderEntity.FieldBookingTime))
            {
                where.Add(OrderEntity.FieldBookingTime, Comparison.Equals, dic[OrderEntity.FieldBookingTime]);
            }
            if (dic.ContainsKey(OrderEntity.FieldContent))
            {
                where.Add(OrderEntity.FieldContent, Comparison.Like, "%" + dic[OrderEntity.FieldContent] + "%");
            }
            if (dic.ContainsKey(OrderEntity.FieldWorkingLocation))
            {
                where.Add(OrderEntity.FieldWorkingLocation, Comparison.Like, "%" + dic[OrderEntity.FieldWorkingLocation] + "%");
            }

            if (scopeIDs.Count > 0)
            {
                //显示指定部门的工单
                where.Add(scopeConstraint, Comparison.In, scopeIDs.ToArray());
            }
            else
            {
                if (CurrentRole.ID == 1)
                {
                    //超级管理员，显示所有内容
                    where.Add("1", Comparison.Equals, "1");
                }
                else
                {
                    where.Add("NextOperatorID", Comparison.Equals, CurrentStaff.ID);
                }
            }

            //2.1 构建orderby 从句
            OrderByStatement orderby = new OrderByStatement();
            orderby.Add(OrderEntity.FieldPriority, Sorting.Descending);
            orderby.Add(OrderEntity.FieldBookingTime, Sorting.Ascending);

            //3.0 获取已发起的数据
            ViewManager manager = new ViewManager("VO_Order");
            DataTable dt = manager.GetDataTableByPage(where, out count, pageIndex, pageSize,orderby);
            return dt;
        }

        /// <summary>
        /// 获取我处理中的工单
        /// </summary>
        /// <returns></returns>
        public DataTable GetMyHandlingOrders(JSDictionary dic ,int pageIndex, int pageSize, out int count)
        {
            //1.0 构建资源对象
            PermissionService permissionService = new PermissionService();
            string scopeConstraint = "";
            List<string> scopeIDs = permissionService.GetAuthorizedScopeIDByRole(CurrentRole, "OrderSys_Data.HandlingOrders", out scopeConstraint);

            //2.0 构建where从句
            WhereStatement where = new WhereStatement();
            if (!dic.ContainsKey(OrderEntity.FieldStatus))
            {
                WhereClause clause = new WhereClause(OrderEntity.FieldStatus, Comparison.Equals, (int)OrderStatus.Handling);
                clause.AddClause(LogicOperator.Or, Comparison.Equals, (int)OrderStatus.Rejected);
                where.Add(clause);
            }
            else
            {
                //只能搜索处理中或被驳回的工单
                if (Convert.ToInt32(dic[OrderEntity.FieldStatus]) == (int)OrderStatus.Handling
                    || Convert.ToInt32(dic[OrderEntity.FieldStatus]) == (int)OrderStatus.Rejected)
                {
                    where.Add(OrderEntity.FieldStatus, Comparison.Equals, dic[OrderEntity.FieldStatus]);
                }
                else
                {
                    where.Add("1", Comparison.NotEquals, "0");
                }
            }
            if (dic.ContainsKey(OrderEntity.FieldPriority))
            {
                where.Add(OrderEntity.FieldPriority, Comparison.Equals, dic[OrderEntity.FieldPriority]);
            }
            if (dic.ContainsKey(OrderEntity.FieldBookingTime))
            {
                where.Add(OrderEntity.FieldBookingTime, Comparison.Equals, dic[OrderEntity.FieldBookingTime]);
            }
            if (dic.ContainsKey(OrderEntity.FieldContent))
            {
                where.Add(OrderEntity.FieldContent, Comparison.Like, "%" + dic[OrderEntity.FieldContent] + "%");
            }
            if (dic.ContainsKey(OrderEntity.FieldWorkingLocation))
            {
                where.Add(OrderEntity.FieldWorkingLocation, Comparison.Like, "%" + dic[OrderEntity.FieldWorkingLocation] + "%");
            }


            if (scopeIDs.Count > 0)
            {
                //显示指定部门的工单
                where.Add(scopeConstraint, Comparison.In, scopeIDs.ToArray());
                //只显示领队的，避免工单重复
                where.Add("IsLeader", Comparison.Equals,(int)TrueFalse.True);
            }
            else
            {
                if (CurrentRole.ID == 1)
                {
                    //超级管理员，显示所有内容
                    where.Add("1", Comparison.Equals, "1");
                    //只显示领队的，避免工单重复
                    where.Add("IsLeader", Comparison.Equals, (int)TrueFalse.True);
                }
                else
                {
                    where.Add("HandlerID", Comparison.Equals, CurrentStaff.ID);
                }
            }

            //2.1 构建orderby 从句
            OrderByStatement orderby = new OrderByStatement();
            orderby.Add(OrderEntity.FieldPriority, Sorting.Descending);
            orderby.Add(OrderEntity.FieldBookingTime, Sorting.Ascending);

            //3.0 获取已发起的数据
            ViewManager manager = new ViewManager("VO_OrderHandlers");
            DataTable dt = manager.GetDataTableByPage(where, out count, pageIndex, pageSize,orderby);
            return dt;
        }

        /// <summary>
        /// 获取我已处理的工单
        /// </summary>
        /// <returns></returns>
        public DataTable GetMyHandledOrders(JSDictionary dic,int pageIndex, int pageSize, out int count)
        {
            //1.0 构建资源对象
            PermissionService permissionService = new PermissionService();
            string scopeConstraint = "";
            List<string> scopeIDs = permissionService.GetAuthorizedScopeIDByRole(CurrentRole, "OrderSys_Data.HandledOrders", out scopeConstraint);

            //2.0 构建where从句
            WhereStatement where = new WhereStatement();
            if (!dic.ContainsKey(OrderEntity.FieldStatus))
            {
                where.Add(OrderEntity.FieldStatus, Comparison.GreaterOrEquals, (int)OrderStatus.Checking);
            }
            else
            {
                //只能搜索已委派的工单
                if (Convert.ToInt32(dic[OrderEntity.FieldStatus]) >= (int)OrderStatus.Checking)
                {
                    where.Add(OrderEntity.FieldStatus, Comparison.Equals, dic[OrderEntity.FieldStatus]);
                }
                else
                {
                    where.Add("1", Comparison.NotEquals, "0");
                }
            }
            if (dic.ContainsKey(OrderEntity.FieldPriority))
            {
                where.Add(OrderEntity.FieldPriority, Comparison.Equals, dic[OrderEntity.FieldPriority]);
            }
            if (dic.ContainsKey(OrderEntity.FieldBookingTime))
            {
                where.Add(OrderEntity.FieldBookingTime, Comparison.Equals, dic[OrderEntity.FieldBookingTime]);
            }
            if (dic.ContainsKey(OrderEntity.FieldContent))
            {
                where.Add(OrderEntity.FieldContent, Comparison.Like, "%" + dic[OrderEntity.FieldContent] + "%");
            }
            if (dic.ContainsKey(OrderEntity.FieldWorkingLocation))
            {
                where.Add(OrderEntity.FieldWorkingLocation, Comparison.Like, "%" + dic[OrderEntity.FieldWorkingLocation] + "%");
            }

            if (scopeIDs.Count > 0)
            {
                //显示指定部门的工单
                where.Add(scopeConstraint, Comparison.In, scopeIDs.ToArray());
            }
            else
            {
                if (CurrentRole.ID == 1)
                {
                    //超级管理员，显示所有内容
                    where.Add("1", Comparison.Equals, "1");
                }
                else
                {
                    where.Add("HandlerID", Comparison.Equals, CurrentStaff.ID);
                }
            }

            //2.1 构建orderby 从句
            OrderByStatement orderby = new OrderByStatement();
            orderby.Add(OrderEntity.FieldPriority, Sorting.Descending);
            orderby.Add(OrderEntity.FieldBookingTime, Sorting.Ascending);

            //3.0 获取已发起的数据
            ViewManager manager = new ViewManager("VO_Order");
            DataTable dt = manager.GetDataTableByPage(where, out count, pageIndex, pageSize,orderby);
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

        public DataTable GetOrderHandlerDT(Guid orderID)
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

        public List<OrderHandlerEntity> GetOrderHandlers(Guid orderID, bool onlyHelper = false)
        {
            WhereStatement where = new WhereStatement();
            where.Add(OrderHandlerEntity.FieldOrderID,Comparison.Equals,orderID);
            if (onlyHelper)
            {
                where.Add(OrderHandlerEntity.FieldIsLeader, Comparison.Equals, (int)TrueFalse.False);
            }

            int count = 0;
            EntityManager<OrderHandlerEntity> manager = new EntityManager<OrderHandlerEntity>();
            List<OrderHandlerEntity> handlers = manager.GetList(where,out count);

            return handlers;
        }

        #region 工作地点
        public OrderWorkingLocationEntity GetOrderWorkingLocation(int orderWorkingLocationID)
        {
            EntityManager<OrderWorkingLocationEntity> manager = new EntityManager<OrderWorkingLocationEntity>();
            OrderWorkingLocationEntity model = manager.GetSingle(orderWorkingLocationID);
            return model;
        } 

        public DataTable GetOrderWorkingLocationDTByRole(RoleEntity role, Paging paging, out int count)
        {
            PermissionService permissionService = new PermissionService();
            string scopeConstraint = "";
            List<string> scopeIDs = permissionService.GetAuthorizedScopeIDByRole(role, "OrderSys_Data.OrderWorkingLocation", out scopeConstraint);

            WhereStatement where = new WhereStatement();
            if (scopeIDs.Count > 0)
            {
                where.Add(scopeConstraint, Comparison.In, scopeIDs.ToArray());
            }
            else
            {
                if (role.ID == 1)
                {
                    //超级管理员，显示所有内容
                    where.Add("1", Comparison.Equals, "1");
                }
                else
                {
                    //非超级管理员，不显示内容
                    where.Add("1", Comparison.Equals, "0");
                }
            }

            OrderByStatement orderby = new OrderByStatement();
            orderby.Add(paging.SortField, ConvertToSort(paging.SortOrder));

            ViewManager vmanager = new ViewManager("VO_OrderWorkingLocation");
            DataTable dt = vmanager.GetDataTableByPage(where, out count, paging.PageIndex, paging.PageSize, orderby);
            return dt;
        }

        public List<OrderWorkingLocationEntity> GetOrderWorkingLocationListByRole(RoleEntity role)
        {
            PermissionService permissionService = new PermissionService();
            string scopeConstraint = "";
            List<string> scopeIDs = permissionService.GetAuthorizedScopeIDByRole(role, "OrderSys_Data.OrderWorkingLocation", out scopeConstraint);

            WhereStatement where = new WhereStatement();
            if (scopeIDs.Count > 0)
            {
                where.Add(scopeConstraint, Comparison.In, scopeIDs.ToArray());
            }
            else
            {
                if (role.ID == 1)
                {
                    //超级管理员，显示所有内容
                    where.Add("1", Comparison.Equals, "1");
                }
                else
                {
                    //非超级管理员，不显示内容
                    where.Add("1", Comparison.Equals, "0");
                }
            }

            int count = 0;
            EntityManager<OrderWorkingLocationEntity> manager = new EntityManager<OrderWorkingLocationEntity>();
            List<OrderWorkingLocationEntity> re = manager.GetList(where, out count);
            return re;
        }

        public void AddOrderWorkingLocation(OrderWorkingLocationEntity entity)
        {
            EntityManager<OrderWorkingLocationEntity> manager = new EntityManager<OrderWorkingLocationEntity>();
            manager.Insert(entity);
        }

        public void EditOrderWorkingLocation(OrderWorkingLocationEntity entity)
        {
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(OrderWorkingLocationEntity.FieldFirstLevel, entity.FirstLevel));
            kvps.Add(new KeyValuePair<string, object>(OrderWorkingLocationEntity.FieldScecondLevel, entity.ScecondLevel));
            kvps.Add(new KeyValuePair<string, object>(OrderWorkingLocationEntity.FieldOrganizeID, entity.OrganizeID));

            EntityManager<OrderWorkingLocationEntity> orderManager = new EntityManager<OrderWorkingLocationEntity>();
            int rows = orderManager.Update(kvps, entity.ID);
        }

        public void DeleteOrderWorkingLocation(int[] orderWorkingLocationIDs)
        {
            WhereStatement where = new WhereStatement();
            where.Add(OrderWorkingLocationEntity.FieldID, Comparison.In, orderWorkingLocationIDs);

            EntityManager<OrderWorkingLocationEntity> orderManager = new EntityManager<OrderWorkingLocationEntity>();
            int rows = orderManager.Delete(where);
        }

        public List<string> GetOrderWorkingLocationFirstLevelList(RoleEntity role)
        {
            List<OrderWorkingLocationEntity> list = GetOrderWorkingLocationListByRole(role);
            var re = list.GroupBy(x => x.FirstLevel).Select(x => x.First().FirstLevel).ToList();
            return re;
        }

        public List<string> GetOrderWorkingLocationSecondLevelList(RoleEntity role, string firstLevelList)
        {
            List<OrderWorkingLocationEntity> list = GetOrderWorkingLocationListByRole(role);
            var re = list.Where(x => x.FirstLevel == firstLevelList).Select(x => x.ScecondLevel).ToList();
            return re;
        } 
        #endregion

        #region 物品选择
        public OrderGoodsEntity GetOrderGoods(int OrderGoodsID)
        {
            EntityManager<OrderGoodsEntity> manager = new EntityManager<OrderGoodsEntity>();
            OrderGoodsEntity model = manager.GetSingle(OrderGoodsID);
            return model;
        }

        public DataTable GetOrderGoodsDTByRole(RoleEntity role, Paging paging, out int count)
        {
            PermissionService permissionService = new PermissionService();
            string scopeConstraint = "";
            List<string> scopeIDs = permissionService.GetAuthorizedScopeIDByRole(role, "OrderSys_Data.OrderGoods", out scopeConstraint);

            WhereStatement where = new WhereStatement();
            if (scopeIDs.Count > 0)
            {
                where.Add(scopeConstraint, Comparison.In, scopeIDs.ToArray());
            }
            else
            {
                if (role.ID == 1)
                {
                    //超级管理员，显示所有内容
                    where.Add("1", Comparison.Equals, "1");
                }
                else
                {
                    //非超级管理员，不显示内容
                    where.Add("1", Comparison.Equals, "0");
                }
            }

            OrderByStatement orderby = new OrderByStatement();
            orderby.Add(paging.SortField, ConvertToSort(paging.SortOrder));

            ViewManager vmanager = new ViewManager("VO_OrderGoods");
            DataTable dt = vmanager.GetDataTableByPage(where, out count, paging.PageIndex, paging.PageSize, orderby);
            return dt;
        }

        public List<OrderGoodsEntity> GetOrderGoodsListByRole(RoleEntity role)
        {
            PermissionService permissionService = new PermissionService();
            string scopeConstraint = "";
            List<string> scopeIDs = permissionService.GetAuthorizedScopeIDByRole(role, "OrderSys_Data.OrderGoods", out scopeConstraint);

            WhereStatement where = new WhereStatement();
            if (scopeIDs.Count > 0)
            {
                where.Add(scopeConstraint, Comparison.In, scopeIDs.ToArray());
            }
            else
            {
                if (role.ID == 1)
                {
                    //超级管理员，显示所有内容
                    where.Add("1", Comparison.Equals, "1");
                }
                else
                {
                    //非超级管理员，不显示内容
                    where.Add("1", Comparison.Equals, "0");
                }
            }

            int count = 0;
            EntityManager<OrderGoodsEntity> manager = new EntityManager<OrderGoodsEntity>();
            List<OrderGoodsEntity> re = manager.GetList(where, out count);
            return re;
        }

        public void AddOrderGoods(OrderGoodsEntity entity)
        {
            EntityManager<OrderGoodsEntity> manager = new EntityManager<OrderGoodsEntity>();
            manager.Insert(entity);
        }

        public void EditOrderGoods(OrderGoodsEntity entity)
        {
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(OrderGoodsEntity.FieldName, entity.Name));
            kvps.Add(new KeyValuePair<string, object>(OrderGoodsEntity.FieldOrganizeID, entity.OrganizeID));

            EntityManager<OrderGoodsEntity> orderManager = new EntityManager<OrderGoodsEntity>();
            int rows = orderManager.Update(kvps, entity.ID);
        }

        public void DeleteOrderGoods(int[] OrderGoodsIDs)
        {
            WhereStatement where = new WhereStatement();
            where.Add(OrderGoodsEntity.FieldID, Comparison.In, OrderGoodsIDs);

            EntityManager<OrderGoodsEntity> orderManager = new EntityManager<OrderGoodsEntity>();
            int rows = orderManager.Delete(where);
        }

        public List<string> GetOrderGoodsNameList(RoleEntity role)
        {
            List<OrderGoodsEntity> list = GetOrderGoodsListByRole(role);
            var re = list.GroupBy(x => x.Name).Select(x => x.First().Name).ToList();
            return re;
        }

        public List<OrderGoodsRelEntity> GetOrderGoodsRelList(Guid orderID)
        {
            WhereStatement where = new WhereStatement();
            where.Add(OrderGoodsRelEntity.FieldOrderID,Comparison.Equals,orderID);

            int count = 0;
            EntityManager<OrderGoodsRelEntity> manager = new EntityManager<OrderGoodsRelEntity>();
            List<OrderGoodsRelEntity> list = manager.GetList(where, out count);

            return list;
        }

        #endregion

        public string ExportOrders(RoleEntity role,JSDictionary dic)
        {
            Dictionary<int, string> statusDic = EnumExtensions.ConvertToDic<OrderStatus>();
            Dictionary<int, string> priorityDic = EnumExtensions.ConvertToDic<OrderPriority>();

            int count = 0;
            int exportCount = 10;
            DataTable dt = GetOrdersDTByRoles(role,dic, 1, exportCount, out count);
            if (count > exportCount) 
            { 
                throw new JSException(JSErrMsg.ERR_CODE_ExportTooMuch, string.Format(JSErrMsg.ERR_MSG_ExportTooMuch, exportCount)); 
            }
            
            dt.Columns.Add("StatusName");
            dt.Columns.Add("PriorityName");
            foreach (DataRow dr in dt.Rows)
            {
                if (!string.IsNullOrEmpty(dr["Status"].ToString()))
                {
                    dr["StatusName"] = statusDic[Convert.ToInt32(dr["Status"].ToString())];
                }
                if (!string.IsNullOrEmpty(dr["Priority"].ToString()))
                {
                    dr["PriorityName"] = priorityDic[Convert.ToInt32(dr["Priority"].ToString())];
                }
            }

            dt.Columns["OrderNo"].ColumnName = "工单号";
            dt.Columns["PriorityName"].ColumnName = "优先级";
            dt.Columns["StatusName"].ColumnName = "工单状态";
            dt.Columns["FinishTime"].ColumnName = "完成时间";
            dt.Columns["BookingTime"].ColumnName = "截止时间";
            dt.Columns["Content"].ColumnName = "内容";
            dt.Columns["Remark"].ColumnName = "备注";
            dt.Columns["Attn"].ColumnName = "联系人";
            dt.Columns["AttnTel"].ColumnName = "联系人电话";
            dt.Columns["StarterName"].ColumnName = "发起人";
            dt.Columns["StarterTel"].ColumnName = "发起人电话";
            dt.Columns["AppointerName"].ColumnName = "委派人";
            dt.Columns["AppointerTel"].ColumnName = "委派人电话";
            dt.Columns["HandlerName"].ColumnName = "处理人（领队）";
            dt.Columns["HandlerTel"].ColumnName = "处理人（领队）电话";
            dt.Columns["StartTime"].ColumnName = "发起时间";
            dt.Columns["WorkingLocation"].ColumnName = "维修地点";
            dt.Columns["FeeGoods"].ColumnName = "更换物品（收费）";
            dt.Columns["FreeGoods"].ColumnName = "更换物品（不收费）";

            DataTable re = dt.DefaultView.ToTable(false, 
                new string[] { 
                    "工单号", "优先级", "工单状态", "完成时间", "截止时间",
                "内容", "备注", "联系人", "联系人电话", "发起人",
                "发起人电话", "委派人", "委派人电话", "处理人（领队）", "处理人（领队）电话",
                "发起时间", "维修地点", "更换物品（收费）", "更换物品（不收费）"
                });

            ExportService exportService = new ExportService();
            string localpath = "";
            string webpath = exportService.GetExportFolderWebPath("order", out localpath);
            string fileName = exportService.GetFileName() + ".csv";
            
            BaseExportCSV.ExportCSV(re, localpath + fileName);
            return webpath + fileName;
        }

        private List<int> GetAppointersStaffIDs()
        {
            PermissionService permissionService = new PermissionService();
            string scopeConstraint = "";
            List<int> scopeIDs = permissionService.GetAuthorizedScopeIDByRole(CurrentRole, "OrderSys_Data.AppointingStaff", out scopeConstraint).ConvertAll(r => Convert.ToInt32(r));

            UserService userService = new UserService();
            DataTable dt = userService.GetUserDT(scopeIDs.ToArray());

            List<int> re = DataTableUtil.FieldToArray(dt, "Staff_ID").Distinct<string>().ToList().ConvertAll(r => Convert.ToInt32(r));
            return re;
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
            if (orderHandlers.Count == 0)
            {
                throw new JSException(JSErrMsg.ERR_CODE_NOHANDLERS, JSErrMsg.ERR_MSG_NOHANDLERS);
            }
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
                throw new JSException(JSErrMsg.ERR_CODE_NOLEADER, string.Format(JSErrMsg.ERR_MSG_NOLEADER,""));
            }
            return leaderID;
        }

        /// <summary>
        /// 返回工单号（不支持事务）
        /// </summary>
        /// <returns></returns>
        private string GetNewOrderNo()
        {
            //不调用这个了，直接用触发器
            List<IDbDataParameter> outDbParameters = new List<IDbDataParameter>();
            IDbHelper dbHelper = DbHelperFactory.GetHelper(BaseSystemInfo.CenterDbConnectionString);

            IDbDataParameter[] dbParameters = new IDbDataParameter[] { dbHelper.MakeOutParam("sn", SqlDbType.VarChar.ToString(), 14) };
            DataTable dt = dbHelper.ExecuteProcedureForDataTable("[uspSN]", "JSNet", dbParameters, out outDbParameters);

            string orderNo = outDbParameters.FirstOrDefault(p => p.ParameterName == dbHelper.GetParameter("sn")).Value.ToString();
            return orderNo;
        }

        private void ValidateOrderFlows(OrderStatus source, OrderStatus target)
        {
            if (source == target)
            {
                throw new JSException(JSErrMsg.ERR_CODE_WrongFlows, string.Format(JSErrMsg.ERR_MSG_WrongFlows, source.ToDescription()));
            }

            if ((int)target > 0)
            {
                //正常流程
                if ((int)source >= (int)target)
                {
                    throw new JSException(JSErrMsg.ERR_CODE_WrongFlows, string.Format(JSErrMsg.ERR_MSG_WrongFlows, source.ToDescription()));
                }
            }
            else if ((int)target == 0)
            {
                //撤销
                if ((int)source >= (int)OrderStatus.Handling)
                {
                    throw new JSException(JSErrMsg.ERR_CODE_NotAllowCancel, string.Format(JSErrMsg.ERR_MSG_NotAllowCancel));
                }
            }
            else if ((int)target < 0)
            {
                if (target == OrderStatus.Rejected)
                {
                    //驳回
                    if (target != OrderStatus.Handling)
                    {
                        throw new JSException(JSErrMsg.ERR_CODE_NotAllowReject, string.Format(JSErrMsg.ERR_MSG_NotAllowReject));
                    }
                }
            }
        }

    }
}
