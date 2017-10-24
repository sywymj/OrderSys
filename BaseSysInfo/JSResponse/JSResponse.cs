using JSNet.Utilities;
using System;
using System.Xml.Serialization;

namespace JSNet.BaseSys
{
    [Serializable]
    public class JSResponse
    {

        /// <summary>
        /// 成功返回，返回码默认200，消息提示类型默认message
        /// </summary>
        /// <param name="msg">返回消息（展示给用户看）</param>
        public JSResponse(string msg)
            : this(ResponseType.Message, "200", msg, "", "", null)
        {

        }


        /// <summary>
        /// 成功返回数据（列表），返回码默认200，消息提示类型默认None
        /// </summary>
        /// <param name="data">数据</param>
        public JSResponse(object data)
            : this(ResponseType.None, "200", "数据加载成功", "", "", data)
        {

        }

        /// <summary>
        /// 成功返回，返回码默认200
        /// </summary>
        /// <param name="resType">消息提示类型</param>
        /// <param name="msg">返回消息</param>
        /// <param name="data">返回消息（展示给用户看）</param>
        public JSResponse(ResponseType resType, string msg)
            : this(resType, "200", msg, "", "", null)
        {

        }

        public JSResponse(ResponseType resType, string msg, object data)
            : this(resType, "200", msg, "", "", data)
        {

        }

        /// <summary>
        /// 返回错误提示，返回码默认500，返回消息为errmsg
        /// </summary>
        /// <param name="errcode"></param>
        /// <param name="errmsg"></param>
        public JSResponse(string errcode, string errmsg)
            : this(errmsg, errcode, errmsg)
        {

        }

        public JSResponse(string errcode, string errmsg,object data)
            : this(errmsg, errcode, errmsg, data)
        {

        }

        public JSResponse(string msg, string errcode, string errmsg,object data)
            : this("500", msg, errcode, errmsg,data)
        {

        }

        /// <summary>
        /// 返回错误提示，返回码默认500
        /// </summary>
        /// <param name="msg">返回消息（展示给用户看）</param>
        /// <param name="errcode">错误代码</param>
        /// <param name="errmsg">错误消息</param>
        public JSResponse(string msg, string errcode, string errmsg)
            : this("500", msg, errcode, errmsg)
        {

        }

        public JSResponse(string code, string msg, string errcode, string errmsg,object data)
            : this(ResponseType.Error, code, msg, errcode, errmsg, data)
        {

        }

        /// <summary>
        /// 返回错误提示
        /// </summary>
        /// <param name="code">返回码</param>
        /// <param name="msg">返回消息（展示给用户看）</param>
        /// <param name="errcode">错误代码</param>
        /// <param name="errmsg">错误消息</param>
        public JSResponse(string code, string msg, string errcode, string errmsg)
            : this(ResponseType.Error, code, msg, errcode, errmsg, null)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rspType">消息提示类型</param>
        /// <param name="code">返回码</param>
        /// <param name="msg">返回消息</param>
        /// <param name="errcode">错误代码</param>
        /// <param name="errmsg">错误消息</param>
        /// <param name="data">返回数据</param>
        public JSResponse(ResponseType rspType, string code, string msg, string errcode, string errmsg, object data)
        {
            RspTypeCode = (int)rspType;
            RspType = rspType;
            Code = code;
            Msg = msg;
            ErrCode = errcode;
            ErrMsg = errmsg;
            Data = data;
        }


        /// <summary>
        /// 返回码 200,401,500
        /// </summary>
        [XmlElement("code")]
        public string Code { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        [XmlElement("msg")]
        public string Msg { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        [XmlElement("err_code")]
        public string ErrCode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        [XmlElement("err_msg")]
        public string ErrMsg { get; set; }

        /// <summary>
        /// 响应原始内容
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 响应的类型
        /// </summary>
        [XmlElement("rsp_type")]
        public ResponseType RspType { get; set; }

        /// <summary>
        /// 相应的类型代码
        /// </summary>
        [XmlElement("rsp_typecode")]
        public int RspTypeCode { get; set; }
        
        /// <summary>
        /// 响应结果是否错误
        /// </summary>
        public bool IsError
        {
            get
            {
                return !string.IsNullOrEmpty(this.ErrCode);
            }
        }


        public static void WriteCookie(string key, string strValue)
        {
            CommonUtil.WriteCookie(key, strValue);
        }

        public static void WriteCookie(string key, string strValue, int expires)
        {
            CommonUtil.WriteCookie(key, strValue,expires);
        }
    }

    /// <summary>
    /// 前端消息提示方式
    /// </summary>
    public enum ResponseType
    {
        /// <summary>
        /// 错误消息
        /// </summary>
        Error=-1,
        /// <summary>
        /// 无，前端可以自定义提示方式
        /// </summary>
        None = 0,           
        /// <summary>
        /// 普通提示消息
        /// </summary>
        Message = 1,       
        /// <summary>
        /// 提示消息
        /// </summary>
        Remind = 2,         
        /// <summary>
        /// 警告消息
        /// </summary>
        Warning = 3,        
        /// <summary>
        /// 重定向
        /// </summary>
        Redict = 4,
        /// <summary>
        /// 提示没有数据
        /// </summary>
        NoData = 5,        
        /// <summary>
        /// 提示数据加载完
        /// </summary>
        NoMoreData = 6,
    }
}
