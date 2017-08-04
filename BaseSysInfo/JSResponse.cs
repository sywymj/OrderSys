using System;
using System.Xml.Serialization;

namespace JSNet.BaseSys
{
    [Serializable]
    public class JSResponse
    {
        /// <summary>
        /// 错误码
        /// </summary>
        [XmlElement("code")]
        public string ErrCode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        [XmlElement("msg")]
        public string ErrMsg { get; set; }

        /// <summary>
        /// 子错误码
        /// </summary>
        [XmlElement("sub_code")]
        public string SubErrCode { get; set; }

        /// <summary>
        /// 子错误信息
        /// </summary>
        [XmlElement("sub_msg")]
        public string SubErrMsg { get; set; }

        /// <summary>
        /// 响应原始内容
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 响应的类型
        /// </summary>
        [XmlElement("rsp_type")]
        public MsgType RspType { get; set; }

        /// <summary>
        /// 响应结果是否错误
        /// </summary>
        public bool IsError
        {
            get
            {
                return !string.IsNullOrEmpty(this.ErrCode) || !string.IsNullOrEmpty(this.SubErrCode);
            }
        }
    }

    public enum MsgType
    {
        Error=-1,
        Message = 0,        // 0 消息。
        Remind = 1,         // 1 提示。
        Warning = 2,        // 2 警示。
        WaitForAudit = 3,   // 3 待审核事项。
        Comment = 4,        // 4 评论。
        TodoList = 5,       // 5 待审核。
        Note = 6            // 6 备忘录。Error
    }
}
