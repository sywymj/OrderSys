using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSNet.BaseSys
{
    public class JSErrMsg
    {
        //4开头的是客户端问题
        public const string ERR_CODE_PARAM_MISSING = "4000";
        public const string ERR_MSG_PARAM_MISSING = "缺少参数:{0}";
        public const string ERR_CODE_PARAM_INVALID = "4001";
        public const string ERR_MSG_PARAM_INVALID = "参数不合法:{0}";
        public const string ERR_CODE_KEY_MISSING = "4002";
        public const string ERR_MSG_KEY_MISSING = "client-error:Missing required key:{0}";
        public const string ERR_CODE_COOKIEKEY_MISSING = "4003";
        public const string ERR_MSG_COOKIEKEY_MISSING = "client-error:Missing required cookie key:{0}";
        public const string ERR_CODE_NODATA_SELECT = "4004";
        public const string ERR_MSG_NODATA_SELECT = "请选择至少一条记录。";
        public const string ERR_CODE_AddedVXUser = "4005";
        public const string ERR_MSG_AddedVXUser = "微信用户已存在，请勿重复添加。";
        public const string ERR_CODE_WrongFormateTel = "4006";
        public const string ERR_MSG_WrongFormateTel = "手机号码格式不正确！";
        public const string ERR_CODE_ExportTooMuch = "4007";
        public const string ERR_MSG_ExportTooMuch = "单次导出不能超过{0}条记录，请筛选后重试。";

        //5开头的是服务端问题
        public const string ERR_CODE_SESSIONKEY_MISSING = "5001";
        public const string ERR_MSG_SESSIONKEY_MISSING = "server-error:Missing required session key:{0}";

        //这些错误，提示“服务器出错！”
        public const string ERR_MSG_DATA_MISSING = "{0}的数据丢失。";
        public const string ERR_MSG_DATA_REPETITION = "数据库{0}数据重复。";

        //6开头的是工单业务问题
        public const string ERR_CODE_NOLEADER = "6002";
        public const string ERR_MSG_NOLEADER = "{0}工单没有领队。";
        public const string ERR_CODE_NOHANDLERS = "6003";
        public const string ERR_MSG_NOHANDLERS = "工单至少要有一个处理者。";
        public const string ERR_CODE_NotAllowCancel = "6004";
        public const string ERR_MSG_NotAllowCancel = "只能取消受理前的工单。";
        public const string ERR_CODE_WrongFlows = "6005";
        public const string ERR_MSG_WrongFlows = "操作失败，工单状态为{0}。";
        public const string ERR_CODE_NotAllowReject = "6006";
        public const string ERR_MSG_NotAllowReject = "只能驳回处理中的工单。";

        //7开头的是权限业务问题
        public const string ERR_CODE_NotGrantRole = "7001";
        public const string ERR_MSG_NotGrantRole = "当前用户没分配角色，请联系管理员分配角色！";
        public const string ERR_CODE_NotGrantPermissionScope = "7003";
        public const string ERR_MSG_NotGrantPermissionScope = "没有分配资源权限";
        public const string ERR_CODE_NotGrantMenuResource = "7004";
        public const string ERR_MSG_NotGrantMenuResource = "没有分配菜单权限。";
        public const string ERR_CODE_ErrorFormatCode = "7005";
        public const string ERR_MSG_ErrorFormatCode = "标识码格式有误，标识码格式为：{System}.{Data}";
        public const string ERR_CODE_NotAllowGrantItem = "7006";
        public const string ERR_MSG_NotAllowGrantItem = "{0}不允许分配资源明细。";


        //8开头的是登陆、用户问题
        public const string ERR_MSG_LoginFaile = "登录失败，请重新登录！";
        public const string ERR_CODE_NotAllowLogin = "8001";
        public const string ERR_MSG_NotAllowLogin = "当前用户禁止登陆。";
        public const string ERR_CODE_LoginOvertime = "8002";
        public const string ERR_MSG_LoginOvertime = "登陆超时，请重新登陆。";
        public const string ERR_CODE_UserUnable = "8003";
        public const string ERR_MSG_UserUnable = "当前用户已失效。";
        public const string ERR_CODE_WrongPwd = "8004";
        public const string ERR_MSG_WrongPwd = "用户名或密码错误。";
        public const string ERR_CODE_WrongOpenID = "8005";
        public const string ERR_MSG_WrongOpenID = "登陆失败，用户OpenID有误！";
        public const string ERR_CODE_WrongUserID = "8006";
        public const string ERR_MSG_WrongUserID = "UID有误！";
        public const string ERR_CODE_WrongRoleID = "8007";
        public const string ERR_MSG_WrongRoleID = "RID有误！";
        public const string ERR_CODE_WrongTel = "8008";
        public const string ERR_MSG_WrongTel = "手机号码不存在！";
        public const string ERR_CODE_UserEditedl = "8009";
        public const string ERR_MSG_UserEdited = "用户验证失败，请重新登陆！";

        //9开头是API错误
        public const string ERR_MSG_APIFailed = "调用接口出错！";
        public const string ERR_Code_APIFailedReason = "9001";
        public const string ERR_MSG_APIFailedReason = "错误地址：{0}，错误原因：{1}";
        public const string ERR_CODE_APIDecryptFailed = "9002";
        public const string ERR_MSG_APIDecryptFailed = "参数解密失败。";

        //91开头，调用接口失败
        //public const string ERR_CODE_APIFailed = "91001";


    }
}
