using JSNet.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSNet.BaseSys
{
    public enum OperationEnum
    {
        [EnumDescription("驳回")]
        reject = -3,

        [EnumDescription("发起")]
        Start = 1,

        [EnumDescription("委派")]
        Appoint = 2,

        [EnumDescription("受理")]
        Receive = 3,

        [EnumDescription("处理")]
        Handle = 4,

        [EnumDescription("验收")]
        Check = 99,

        [EnumDescription("撤销")]
        Cancel = 0,
    }

    public enum OrderStatus
    {
        /// <summary>
        /// 驳回为负数，该值的绝对值为被驳回到的状态
        /// </summary>
        [EnumDescription("被驳回")]
        Rejected = -3,

        [EnumDescription("待委派")]
        Appointing = 1,

        [EnumDescription("待受理")]
        Receving = 2,

        [EnumDescription("处理中")]
        Handling = 3,

        [EnumDescription("待验收")]
        Checking = 4,

        [EnumDescription("完成")]
        Finish = 99,

        [EnumDescription("已撤销")]
        Canceled = 0,
    }

    public enum OrderPriority
    {
        [EnumColorStyle("blue")]
        [EnumDescription("一般")]
        Normal = 0,
        [EnumColorStyle("red")]
        [EnumDescription("紧急")]
        Urgent = 1,
    }

    /// <summary>
    /// 工单维修方法
    /// </summary>
    public enum OrderHandleType
    {
        [EnumDescription("简易维护")]
        Normal = 0,
        [EnumDescription("等待零件")]
        DengJian = 1,
        [EnumDescription("换件")]
        HuanJian=2,
        [EnumDescription("外包")]
        WaiBao = 3,
    }

    public enum TrueFalse
    {
        [EnumDescription("否")]
        False = 0,
        [EnumDescription("是")]
        True = 1,
    }
}
