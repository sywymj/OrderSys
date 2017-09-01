using JSNet.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSNet.BaseSys
{
    public enum SysCategory
    {
        System = 1,
        WinForm = 2,
        WebForm = 3,
        WebApi = 4,
        Mvc = 5,
    }

    public enum ResourceType
    {
        /// <summary>
        /// 数据资源
        /// </summary>
        Data = 1,
        /// <summary>
        /// 菜单
        /// </summary>
        Menu = 2,
        /// <summary>
        /// 内页菜单
        /// </summary>
        InnerMenu = 3,
        /// <summary>
        /// 弹出框
        /// </summary>
        Popup = 4,
        /// <summary>
        /// 按钮
        /// </summary>
        Button=5,

    }
}
