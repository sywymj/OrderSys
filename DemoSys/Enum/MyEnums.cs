using JSNet.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoSys
{

    public enum DemoType
    {
        /// <summary>
        /// 系统
        /// </summary>
        ///
        [EnumDescription("测试1")]
        Test1 = 0,
        /// <summary>
        /// 数据资源
        /// </summary>
        ///
        [EnumDescription("测试2")]
        Test2 = 1,
    }
}