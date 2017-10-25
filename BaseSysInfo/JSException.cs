using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace JSNet.BaseSys
{
    /// <summary>
    /// JS客户端异常。
    /// </summary>
    public class JSException : Exception
    {
        private string errorCode;
        private string errorMsg;

        public JSException()
            : base()
        {
        }

        public JSException(string message)
            : base(message)
        {
        }

        protected JSException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public JSException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public JSException(string errorCode, string errorMsg)
            :base("")
        {
            this.errorCode = errorCode;
            this.errorMsg = errorMsg;
        }

        public JSException(string message, string errorCode, string errorMsg)
            : base(message)
        {
            this.errorCode = errorCode;
            this.errorMsg = errorMsg;
        }

        public string ErrorCode
        {
            get { return this.errorCode; }
        }

        public string ErrorMsg
        {
            get { return this.errorMsg; }
        }
    }
}
