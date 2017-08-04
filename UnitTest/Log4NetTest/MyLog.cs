using log4net.Core;
using log4net.Layout;
using log4net.Layout.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace UnitTest
{
    public class SmsLog
    {
        public SmsLog(string success,string mobiles,string message,string content )
        {
            Success = success;
            Mobiles = mobiles;
            Message = message;
            Content = content;
        }
        /// <summary>
        /// 短信发送是否成功
        /// </summary>
        public string Success { get; set; }

        /// <summary>
        /// 发送号码(可以多个，逗号分隔)
        /// </summary>
        public string Mobiles { get; set; }

        public string Message { get; set; }

        /// <summary>
        /// 发送内容
        /// </summary>
        public string Content { get; set; }
    }

    public class SystemLog
    {
        public SystemLog(string content)
        {
            Content = content;
        }
        /// <summary>
        /// 发送内容
        /// </summary>
        public string Content { get; set; }
    }

    public class MyLayout : PatternLayout
    {
        public MyLayout()
        {
            this.AddConverter("property", typeof(MyMessagePatternConverter));
        }
    }

    public class MyMessagePatternConverter : PatternLayoutConverter
    {
        protected override void Convert(System.IO.TextWriter writer, log4net.Core.LoggingEvent loggingEvent)
        {
            if (Option != null)
            {
                // Write the value for the specified key
                WriteObject(writer, loggingEvent.Repository, LookupProperty(Option, loggingEvent));
            }
            else
            {
                // Write all the key value pairs
                WriteDictionary(writer, loggingEvent.Repository, loggingEvent.GetProperties());
            }
        }

        /// <summary>
        /// 通过反射获取传入的日志对象的某个属性的值
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private object LookupProperty(string property, log4net.Core.LoggingEvent loggingEvent)
        {
            object propertyValue = string.Empty;

            PropertyInfo propertyInfo = loggingEvent.MessageObject.GetType().GetProperty(property);
            if (propertyInfo != null)
                propertyValue = propertyInfo.GetValue(loggingEvent.MessageObject, null);

            return propertyValue;
        }

    }
}
