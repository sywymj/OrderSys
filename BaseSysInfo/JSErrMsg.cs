using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSNet.BaseSys
{
    public class JSErrMsg
    {
        public const string ERR_CODE_PARAM_MISSING = "40";
        public const string ERR_MSG_PARAM_MISSING = "client-error:Missing required arguments:{0}";
        public const string ERR_CODE_PARAM_INVALID = "41";
        public const string ERR_MSG_PARAM_INVALID = "client-error:Invalid arguments:{0}";
    }
}
