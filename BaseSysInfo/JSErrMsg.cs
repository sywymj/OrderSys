﻿using System;
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

        //9开头的是数据库问题
        public const string ERR_CODE_DATA_MISSING = "91";
        public const string ERR_MSG_DATA_MISSING = "{0}数据库数据丢失。";
        public const string ERR_CODE_DATA_REPETITION = "92";
        public const string ERR_MSG_DATA_REPETITION = "数据库{0}数据重复。";

        //6开头的是工单业务问题
        public const string ERR_CODE_NOLEADER = "62";
        public const string ERR_MSG_NOLEADER = "{0}工单没有领队";


        //7开头的是权限业务问题
        public const string ERR_CODE_NotGrantRole = "71";
        public const string ERR_MSG_NotGrantRole = "没有分配角色。";
        public const string ERR_CODE_NotGrantPermissionScope = "73";
        public const string ERR_MSG_NotGrantPermissionScope = "没有分配资源权限";
        public const string ERR_CODE_NotGrantMenuResource = "74";
        public const string ERR_MSG_NotGrantMenuResource = "没有分配菜单权限。";
        public const string ERR_CODE_ErrorFormatCode = "75";
        public const string ERR_MSG_ErrorFormatCode = "标识码格式有误，标识码格式为：{System}.{Data}";
        public const string ERR_CODE_NotAllowGrantItem = "76";
        public const string ERR_MSG_NotAllowGrantItem = "{0}不允许分配资源明细。";

    }
}
