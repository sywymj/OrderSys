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
        public void StartOrder(OrderEntity order)
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
        }

        //委派工作
        public void AppointOrder(Guid orderID, List<OrderHandlerEntity> handlers)
        {
            //1.0 获取当前员工数据
            UserService userService = new UserService();
            EntityManager<OrderFlowEntity> orderflowManager = new EntityManager<OrderFlowEntity>();
            EntityManager<OrderEntity> orderManager = new EntityManager<OrderEntity>();
            EntityManager<OrderHandlerEntity> handlerManager = new EntityManager<OrderHandlerEntity>();

            StaffEntity staff = userService.GetCurrentStaff();

            //2.0 修改工单实体
           
            List<KeyValuePair<string,object>> kvps = new List<KeyValuePair<string,object>>();
            kvps.Add(new KeyValuePair<string,object>(OrderEntity.FieldStatus,(int)OrderStatus.Receving));
            kvps.Add(new KeyValuePair<string,object>(OrderEntity.FieldAppointerID,staff.ID));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldNextOperatorID, GetLeaderHandlerID(handlers)));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldOperateTime,DateTime.Now));
            int rows = orderManager.Update(kvps, orderID);
            if (rows == 0)
            {
                throw new JSException(JSErrMsg.ERR_CODE_DATA_MISSING, string.Format(JSErrMsg.ERR_MSG_DATA_MISSING, "工单实体"));
            }

            //3.0 添加工作流实体
            OrderFlowEntity orderFlow = new OrderFlowEntity();
            orderFlow.OrderID = orderID;
            orderFlow.OperatorID = staff.ID;
            orderFlow.NextOperatorID = GetLeaderHandlerID(handlers);
            orderFlow.Operation = (int)OperationEnum.Appoint;
            orderFlow.OperateTime = DateTime.Now;
            orderFlow.Remark = "";
            orderflowManager.Insert(orderFlow);

            //4.0 添加工单处理者
            handlerManager.Insert(handlers);

        }

        //受理报障单
        public void ReceiveOrder(Guid orderID)
        {
            //1.0 获取当前员工数据
            UserService userService = new UserService();
            EntityManager<OrderEntity> orderManager = new EntityManager<OrderEntity>();
            EntityManager<OrderFlowEntity> orderflowManager = new EntityManager<OrderFlowEntity>();
            StaffEntity staff = userService.GetCurrentStaff();

            //2.0 修改工单实体
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldStatus, (int)OrderStatus.Handling));
            kvps.Add(new KeyValuePair<string,object>(OrderEntity.FieldNextOperatorID, staff.ID));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldHandlerID, staff.ID));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldOperateTime, DateTime.Now));
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
            orderflowManager.Insert(orderflow);

            //4.0 添加处理进度
            OrderHandleDetailEntity orderHandleDetail = new OrderHandleDetailEntity();
            orderHandleDetail.OrderID = orderID;
            orderHandleDetail.HandleType = (int)OrderHandleType.Others;
            orderHandleDetail.HandleDetail = "受理";
            orderHandleDetail.Remark = "已受理，准备处理。";
            AddHandleDetail(orderHandleDetail);
        }

        //增加处理明细
        public void AddHandleDetail(OrderHandleDetailEntity orderHandleDetail)
        {
            //1.0 获取当前员工数据
            UserService userService = new UserService();
            StaffEntity currentStaff = userService.GetCurrentStaff();

            //2.0 添加工作处理明细实体
            orderHandleDetail.HandlerID = currentStaff.ID;
            orderHandleDetail.HandleTime = DateTime.Now;
            if (string.IsNullOrEmpty(orderHandleDetail.HandleDetail))
            {
                orderHandleDetail.HandleDetail = EnumExtensions.ToDescription((OrderHandleType)orderHandleDetail.HandleType);
            }
            orderHandleDetail.Progress = 0;
            EntityManager<OrderHandleDetailEntity> orderHandleDetailManager = new EntityManager<OrderHandleDetailEntity>();
            orderHandleDetailManager.Insert(orderHandleDetail);
        }

        //报障处理完毕
        public void HandledOrder(Guid orderID)
        {
            UserService userService = new UserService();
            EntityManager<OrderEntity> orderManager = new EntityManager<OrderEntity>();
            EntityManager<OrderFlowEntity> orderflowManager = new EntityManager<OrderFlowEntity>();

            //1.0 获取当前员工数据
            StaffEntity staff = userService.GetCurrentStaff();

            //1.1 获取工单实体，以备获取发起人信息
            OrderEntity order = orderManager.GetSingle(orderID);
            if (order == null)
            {
                throw new JSException(JSErrMsg.ERR_CODE_DATA_MISSING, string.Format(JSErrMsg.ERR_MSG_DATA_MISSING, "工单实体"));
            }

            //2.0 修改工单实体
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldOperatorID, staff.ID));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldNextOperatorID, order.StarterID));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldStatus, (int)OrderStatus.Checking));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldOperateTime, DateTime.Now));
            int rows = orderManager.Update(kvps, orderID);
            if (rows == 0)
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

            //4.0 添加处理进度
            OrderHandleDetailEntity orderHandleDetail = new OrderHandleDetailEntity();
            orderHandleDetail.OrderID = orderID;
            orderHandleDetail.HandleType = (int)OrderHandleType.WanCheng;
            orderHandleDetail.Remark = "已完成，送检。";
            AddHandleDetail(orderHandleDetail);
        }

        //驳回报障，需继续处理
        public void RejectOrder(Guid orderID,string remark)
        {
            UserService userService = new UserService();
            EntityManager<OrderEntity> orderManager = new EntityManager<OrderEntity>();
            EntityManager<OrderHandlerEntity> orderHandlerManager = new EntityManager<OrderHandlerEntity>();
            EntityManager<OrderFlowEntity> orderflowManager = new EntityManager<OrderFlowEntity>();

            //1.0 获取当前员工数据
            StaffEntity staff = userService.GetCurrentStaff();

            //1.1 获取工单处理者列表，必备获取领队人的信息
            int count = 0;
            WhereStatement where = new WhereStatement();
            where.Add(OrderHandlerEntity.FieldOrderID, Comparison.Equals, orderID);
            List<OrderHandlerEntity> orderHandlers = orderHandlerManager.GetList(where, out count);

            //2.0 修改工单实体
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldOperatorID, staff.ID));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldNextOperatorID, GetLeaderHandlerID(orderHandlers)));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldStatus, (int)OrderStatus.Rejected));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldOperateTime, DateTime.Now));
            int rows = orderManager.Update(kvps, orderID);
            if (rows == 0)
            {
                throw new JSException(JSErrMsg.ERR_CODE_DATA_MISSING, string.Format(JSErrMsg.ERR_MSG_DATA_MISSING, "工单实体"));
            }

            //3.0 添加工作流实体
            OrderFlowEntity orderflow = new OrderFlowEntity();
            orderflow.OrderID = orderID;
            orderflow.OperatorID = staff.ID;
            orderflow.NextOperatorID = GetLeaderHandlerID(orderHandlers);
            orderflow.Operation = (int)OperationEnum.reject;
            orderflow.OperateTime = DateTime.Now;
            orderflow.Remark = remark;
            orderflowManager.Insert(orderflow);

            OrderHandleDetailEntity orderHandleDetail = new OrderHandleDetailEntity();
            orderHandleDetail.OrderID = orderID;
            orderHandleDetail.HandleType = (int)OrderHandleType.Others;
            orderHandleDetail.HandleDetail = "驳回";
            orderHandleDetail.Remark = remark;
            AddHandleDetail(orderHandleDetail);
        }

        //报障验收完成
        public void FinishOrder(Guid orderID)
        {
            UserService userService = new UserService();
            EntityManager<OrderEntity> orderManager = new EntityManager<OrderEntity>();
            EntityManager<OrderFlowEntity> orderflowManager = new EntityManager<OrderFlowEntity>();

            //1.0 获取当前员工数据
            StaffEntity staff = userService.GetCurrentStaff();

            //2.0 修改工单实体
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldOperatorID, staff.ID));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldNextOperatorID, null));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldStatus, (int)OrderStatus.Finish));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldFinishTime, DateTime.Now));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldOperateTime, DateTime.Now));
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
            orderflow.Operation = (int)OperationEnum.Check;
            orderflow.OperateTime = DateTime.Now;
            orderflow.Remark = "";
            orderflowManager.Insert(orderflow);

            //4.0 添加进度为完成
            OrderHandleDetailEntity orderHandleDetail = new OrderHandleDetailEntity();
            orderHandleDetail.OrderID = orderID;
            orderHandleDetail.HandleType = (int)OrderHandleType.Others;
            orderHandleDetail.HandleDetail = "完成";
            orderHandleDetail.Remark = "完成，验收通过！";
            AddHandleDetail(orderHandleDetail);
        }

        //取消报障单
        public void CancelOrder(Guid orderID)
        {
            // TODO 增加判断，只能取消未处理完的工单
            UserService userService = new UserService();
            EntityManager<OrderEntity> orderManager = new EntityManager<OrderEntity>();
            EntityManager<OrderFlowEntity> orderflowManager = new EntityManager<OrderFlowEntity>();

            //1.0 获取当前员工数据
            StaffEntity staff = userService.GetCurrentStaff();

            //2.0 修改工单实体
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldStatus, (int)OrderStatus.Canceled));
            kvps.Add(new KeyValuePair<string, object>(OrderEntity.FieldOperateTime, DateTime.Now));
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
            orderflow.Operation = (int)OperationEnum.Cancel;
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
        public DataTable GetMyStartedOrders(JSDictionary dic,int pageIndex,int pageSize,out int count)
        {
            //1.0 构建资源对象
            PermissionService permissionService = new PermissionService();
            List<string> organizeIDs = permissionService.GetAuthorizeOrganizeIDByRole(CurrentRole, "OrderSys_Data.StartedOrders");

            //2.0 构建where从句
            WhereStatement where = new WhereStatement();
            if (!dic.ContainsKey(OrderEntity.FieldStatus))
            {
                where.Add(OrderEntity.FieldStatus, Comparison.NotEquals, (int)OrderStatus.Canceled);
                where.Add(OrderEntity.FieldStatus, Comparison.NotEquals, (int)OrderStatus.Finish);
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


            if (organizeIDs.Count > 0)
            {
                //显示指定部门的工单
                where.Add("StarterOrganizeID", Comparison.In, organizeIDs.ToArray());
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
            orderby.Add(OrderEntity.FieldPriority, Sorting.Descending);
            orderby.Add(OrderEntity.FieldBookingTime, Sorting.Ascending);

            //3.0 获取已发起的数据
            ViewManager manager = new ViewManager("VO_Order");
            DataTable dt = manager.GetDataTableByPage(where, out count, pageIndex, pageSize,orderby);
            return dt;

        }

        /// <summary>
        /// 获取我的待委派工单
        /// </summary>
        /// <returns></returns>
        public DataTable GetMyAppointingOrders(int pageIndex, int pageSize, out int count)
        {
            //1.0 构建资源对象
            PermissionService permissionService = new PermissionService();
            List<string> organizeIDs = permissionService.GetAuthorizeOrganizeIDByRole(CurrentRole, "OrderSys_Data.AppointingOrders");

            //2.0 构建where从句
            WhereStatement where = new WhereStatement();
            where.Add(OrderEntity.FieldStatus, Comparison.Equals, (int)OrderStatus.Appointing);
            //显示指定部门的工单
            if (organizeIDs.Count > 0)
            {
                where.Add("StarterOrganizeID", Comparison.In, organizeIDs.ToArray());
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
        public DataTable GetMyAppointedOrders(int pageIndex, int pageSize, out int count)
        {
            //1.0 构建资源对象
            PermissionService permissionService = new PermissionService();
            List<string> organizeIDs = permissionService.GetAuthorizeOrganizeIDByRole(CurrentRole, "OrderSys_Data.AppointedOrders");

            //2.0 构建where从句
            WhereStatement where = new WhereStatement();
            where.Add(OrderEntity.FieldStatus, Comparison.GreaterOrEquals, (int)OrderStatus.Receving);
            if (organizeIDs.Count > 0)
            {
                //显示指定部门的工单
                where.Add("AppointerOrganizeID", Comparison.In, organizeIDs.ToArray());
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
        public DataTable GetMyReceivingOrders(int pageIndex, int pageSize, out int count)
        {
            //1.0 构建资源对象
            PermissionService permissionService = new PermissionService();
            List<string> organizeIDs = permissionService.GetAuthorizeOrganizeIDByRole(CurrentRole, "OrderSys_Data.ReceivingOrders");
            
            //2.0 构建where从句
            WhereStatement where = new WhereStatement();
            where.Add(OrderEntity.FieldStatus, Comparison.Equals, (int)OrderStatus.Receving);
            if (organizeIDs.Count > 0)
            {
                //显示指定部门的工单
                where.Add("NextOperaterOrganizeID", Comparison.In, organizeIDs.ToArray());
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
        public DataTable GetMyHandlingOrders(int pageIndex, int pageSize, out int count)
        {
            //1.0 构建资源对象
            PermissionService permissionService = new PermissionService();
            List<string> organizeIDs = permissionService.GetAuthorizeOrganizeIDByRole(CurrentRole, "OrderSys_Data.HandlingOrders");

            //2.0 构建where从句
            WhereStatement where = new WhereStatement();
            WhereClause clause = new WhereClause(OrderEntity.FieldStatus, Comparison.Equals, (int)OrderStatus.Handling);
            clause.AddClause(LogicOperator.Or, Comparison.Equals, (int)OrderStatus.Rejected);
            where.Add(clause);
            if (organizeIDs.Count > 0)
            {
                //显示指定部门的工单
                where.Add("HandlerOrganizeID", Comparison.In, organizeIDs.ToArray());
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
        public DataTable GetMyHandledOrders(int pageIndex, int pageSize, out int count)
        {
            //1.0 构建资源对象
            PermissionService permissionService = new PermissionService();
            List<string> organizeIDs = permissionService.GetAuthorizeOrganizeIDByRole(CurrentRole, "OrderSys_Data.HandledOrders");

            //2.0 构建where从句
            WhereStatement where = new WhereStatement();
            where.Add(OrderEntity.FieldStatus, Comparison.GreaterOrEquals, (int)OrderStatus.Checking);
            if (organizeIDs.Count > 0)
            {
                //显示指定部门的工单
                where.Add("HandlerOrganizeID", Comparison.In, organizeIDs.ToArray());
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

    }
}
