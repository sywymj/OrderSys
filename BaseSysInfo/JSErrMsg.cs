using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSNet.BaseSys
{
    public class JSErrMsg
    {
        //4开头的是客户端问题
        public const string ERR_CODE_PARAM_MISSING = "40";
        public const string ERR_MSG_PARAM_MISSING = "缺少参数:{0}";
        public const string ERR_CODE_PARAM_INVALID = "41";
        public const string ERR_MSG_PARAM_INVALID = "参数不合法:{0}";
        public const string ERR_CODE_KEY_MISSING = "42";
        public const string ERR_MSG_KEY_MISSING = "client-error:Missing required key:{0}";
        public const string ERR_CODE_COOKIEKEY_MISSING = "43";
        public const string ERR_MSG_COOKIEKEY_MISSING = "client-error:Missing required cookie key:{0}";
        public const string ERR_CODE_NODATA_SELECT = "44";
        public const string ERR_MSG_NODATA_SELECT = "请选择至少一条记录。";
        //5开头的是服务端问题
        public const string ERR_CODE_SESSIONKEY_MISSING = "51";
        public const string ERR_MSG_SESSIONKEY_MISSING = "server-error:Missing required session key:{0}";

        //6开头的是业务问题
        public const string ERR_CODE_DATA_MISSING = "51";
        public const string ERR_MSG_DATA_MISSING = "{0}数据不存在";
        public const string ERR_CODE_NOLEADER = "52";
        public const string ERR_MSG_NOLEADER = "{0}工单没有领队";
        //7开头的是
    }
}
