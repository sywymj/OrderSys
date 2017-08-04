using JSNet.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTest
{
    public enum TestEnum
    {
        [EnumDescription("测试1")]
        test1=0,
        [EnumDescription("测试2")]
        test2 = 2,
    }
}
