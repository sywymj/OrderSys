using JSNet.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSNet.BaseSys
{
    public enum SysCategory
    {
        [EnumDescription("System")]
        System = 1,
        [EnumDescription("WinForm")]
        WinForm = 2,
        [EnumDescription("WebForm")]
        WebForm = 3,
        [EnumDescription("WebApi")]
        WebApi = 4,
        [EnumDescription("MVC")]
        MVC = 5,
    }

    public enum ResourceType
    {
        /// <summary>
        /// 系统
        /// </summary>
        ///
        [EnumDescription("系统")]
        System = 0,
        /// <summary>
        /// 数据资源
        /// </summary>
        ///
        [EnumDescription("数据")]
        Data = 1,
        /// <summary>
        /// 菜单
        /// </summary>
        /// 
        [EnumDescription("菜单")]
        Menu = 2,
        /// <summary>
        /// 内页菜单
        /// </summary>
        /// 
        [EnumDescription("内部菜单")]
        InnerMenu = 3,
        ///// <summary>
        ///// 弹出框
        ///// </summary>
        ///// 
        //[EnumDescription("弹出框")]
        //Popup = 4,
        /// <summary>
        /// 按钮
        /// </summary>
        /// 
        [EnumDescription("按钮")]
        Button=5,

    }

    public enum SexType
    {
        [EnumDescription("男")]
        Male = 1,
        [EnumDescription("女")]
        Female = 2,
    }

    public enum PermissionStatus
    {
        NoLogin,
        NoRight,
    }
}
