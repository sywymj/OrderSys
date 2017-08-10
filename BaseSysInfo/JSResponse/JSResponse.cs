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

        /// <summary>
        /// 返回错误提示，返回码默认500，返回消息默认空
        /// </summary>
        /// <param name="errcode"></param>
        /// <param name="errmsg"></param>
        public JSResponse(string errcode, string errmsg)
            : this("服务器繁忙，请重试！", errcode, errmsg)
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
        /// 响应结果是否错误
        /// </summary>
        public bool IsError
        {
            get
            {
                return !string.IsNullOrEmpty(this.ErrCode);
            }
        }
    }

    public enum ResponseType
    {
        Error=-1,
        None = 0,        // 0 消息。
        Message=1,
        Remind = 2,         // 1 提示。
        Warning = 3,        // 2 警示。
        WaitForAudit = 4,   // 3 待审核事项。
        Comment = 5,        // 4 评论。
        TodoList = 6,       // 5 待审核。
        Note = 7            // 6 备忘录。Error
    }
}
