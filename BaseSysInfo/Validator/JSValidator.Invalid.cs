using FastJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSNet.BaseSys
{
    public sealed partial class JSValidator
    {
        /// <summary>
        /// 验证参数字符串是否合法
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="require">是否必须</param>
        /// <returns></returns>
        /// 
        public static string ValidateTel(string name, string value, bool require = false) 
        {
            
            string re = string.Empty;
            if (!require && string.IsNullOrEmpty(value.Trim()))
            {
                return re;
            }
            if (require && string.IsNullOrEmpty(value))
            {
                throw new JSException(JSErrMsg.ERR_CODE_PARAM_MISSING, string.Format(JSErrMsg.ERR_MSG_PARAM_MISSING, name));
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^1[3|4|5|8][0-9]\d{4,8}$")) 
            {
                throw new JSException(JSErrMsg.ERR_CODE_WrongFormateTel, JSErrMsg.ERR_MSG_WrongFormateTel);
            }

            re = value;
            return re;
        }

        public static string ValidateString(string name, string value, bool require = false)
        {
            //TODO 过滤危险字符
            string re = string.Empty;
            if (!require && string.IsNullOrEmpty(value.Trim()))
            {
                return re;
            }
            if (require && string.IsNullOrEmpty(value))
            {
                throw new JSException(JSErrMsg.ERR_CODE_PARAM_MISSING, string.Format(JSErrMsg.ERR_MSG_PARAM_MISSING, name));
            }
            re = value;
            return re;
        }

        public static int? ValidateInt(string name, string value, bool require=false)
        {
            int re = 0;
            if (!require && string.IsNullOrEmpty(value.Trim()))
            {
                return null;
            }

            if (require && string.IsNullOrEmpty(value))
            {
                throw new JSException(JSErrMsg.ERR_CODE_PARAM_MISSING, string.Format(JSErrMsg.ERR_MSG_PARAM_MISSING, name));
            }

            if (!Int32.TryParse(value, out re))
            {
                throw new JSException(JSErrMsg.ERR_CODE_PARAM_INVALID, string.Format(JSErrMsg.ERR_MSG_PARAM_INVALID, name));
            }

            return re;
        }

        public static DateTime? ValidateDateTime(string name, string value, bool require = false)
        {
            DateTime re;
            if (!require && string.IsNullOrEmpty(value.Trim()))
            {
                return null;
            }
            if (require && string.IsNullOrEmpty(value))
            {
                throw new JSException(JSErrMsg.ERR_CODE_PARAM_MISSING, string.Format(JSErrMsg.ERR_MSG_PARAM_MISSING, name));
            }

            if (!DateTime.TryParse(value, out re))
            {
                throw new JSException(JSErrMsg.ERR_CODE_PARAM_INVALID, string.Format(JSErrMsg.ERR_MSG_PARAM_INVALID, name));
            }
            return re;
        }

        /// <summary>
        /// 验证参数是否为时间格式
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="require">是否必须</param>
        /// <returns>返回 短时间 {HH:mm}</returns>
        public static string ValidateShortTime(string name, string value, bool require = false)
        {
            if (!require && string.IsNullOrEmpty(value.Trim()))
            {
                return null;
            }
            DateTime? re = ValidateDateTime(name, value, require);
            return ((DateTime)re).ToString("HH:mm");
        }

        /// <summary>
        /// 验证参数是否为时间格式
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="isRequired">是否必须</param>
        /// <returns>返回 短时间 {HH:mm:ss}</returns>
        public static string ValidateTime(string name, string value, bool isRequired = false)
        {
            DateTime? re = ValidateDateTime(name, value, isRequired);
            return ((DateTime)re).ToString("HH:mm:ss");
        }

        /// <summary>
        /// 验证参数是否为日期格式
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        /// <param name="isRequired">是否必须</param>
        /// <returns>返回 日期 {yyyy-MM-dd}</returns>
        public static string ValidateDate(string name, string value, bool isRequired = false)
        {
            DateTime? re = ValidateDateTime(name, value, isRequired);
            return ((DateTime)re).ToString("yyyy-MM-dd");
        }

        public static bool? ValidateBoolen(string name, string value, bool require = false)
        {
            bool re;
            if (!require && string.IsNullOrEmpty(value.Trim()))
            {
                return null;
            }
            if (require && string.IsNullOrEmpty(value))
            {
                throw new JSException(JSErrMsg.ERR_CODE_PARAM_MISSING, string.Format(JSErrMsg.ERR_MSG_PARAM_MISSING, name));
            }

            if (!bool.TryParse(value, out re))
            {
                throw new JSException(JSErrMsg.ERR_CODE_PARAM_INVALID, string.Format(JSErrMsg.ERR_MSG_PARAM_INVALID, name));
            }
            return re;
        }

        public static Guid? ValidateGuid(string name, string value, bool require = false)
        {
            Guid re;
            if (!require && string.IsNullOrEmpty(value.Trim()))
            {
                return null;
            }
            if (require && string.IsNullOrEmpty(value))
            {
                throw new JSException(JSErrMsg.ERR_CODE_PARAM_MISSING, string.Format(JSErrMsg.ERR_MSG_PARAM_MISSING, name));
            }

            if (!Guid.TryParse(value, out re))
            {
                throw new JSException(JSErrMsg.ERR_CODE_PARAM_INVALID, string.Format(JSErrMsg.ERR_MSG_PARAM_INVALID, name));
            }
            return re;
        }

        public static int[] ValidateStrings(string name, string value, bool require = false)
        {
            if (!require && string.IsNullOrEmpty(value.Trim()))
            {
                return new int[0];
            }
            if (require && string.IsNullOrEmpty(value))
            {
                throw new JSException(JSErrMsg.ERR_CODE_PARAM_MISSING, string.Format(JSErrMsg.ERR_MSG_PARAM_MISSING, name));
            }

            string[] values = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return ValidateStrings(name, values, require);
        }

        public static int[] ValidateStrings(string name, string[] value, bool require = false)
        {
            if (!require && value.Length ==0)
            {
                return new int[0];
            }
            if (require && value.Length == 0)
            {
                throw new JSException(JSErrMsg.ERR_CODE_PARAM_MISSING, string.Format(JSErrMsg.ERR_MSG_PARAM_MISSING, name));
            }

            int[] re = new int[value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                int? n = ValidateInt(name, value[i], require);
                re[i] = (int)n;
            }
            return re;
        }

        public static string[] ValidateInts(string name, int[] value, bool require = false)
        {
            if (!require && value.Length == 0)
            {
                return new string[0];
            }
            if (require && value.Length == 0)
            {
                throw new JSException(JSErrMsg.ERR_CODE_PARAM_MISSING, string.Format(JSErrMsg.ERR_MSG_PARAM_MISSING, name));
            }

            string[] re = new string[value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                string s = ValidateString(name, value[i].ToString(), require);
                re[i] = s;
            }
            return re;
        }

        public static string[] ValidateToStrings(string name, string value, bool require = false)
        {
            if (!require && string.IsNullOrEmpty(value.Trim()))
            {
                return new string[0];
            }
            if (require && string.IsNullOrEmpty(value))
            {
                throw new JSException(JSErrMsg.ERR_CODE_PARAM_MISSING, string.Format(JSErrMsg.ERR_MSG_PARAM_MISSING, name));
            }

            string[] values = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return values;
        }


        /// <summary>
        /// 验证参数是否为空。
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        public static void ValidateRequired(string name, object value)
        {
            if (value == null)
            {
                throw new JSException(JSErrMsg.ERR_CODE_PARAM_MISSING, string.Format(JSErrMsg.ERR_MSG_PARAM_MISSING, name));
            }
            else
            {
                if (value.GetType() == typeof(string))
                {
                    string strValue = value as string;
                    if (string.IsNullOrEmpty(strValue))
                    {
                        throw new JSException(JSErrMsg.ERR_CODE_PARAM_MISSING, string.Format(JSErrMsg.ERR_MSG_PARAM_MISSING, name));
                    }
                }
            }
        }

        /// <summary>
        /// 验证字符串参数的最大长度。
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <param name="maxLength">最大长度</param>
        public static void ValidateMaxLength(string name, string value, int maxLength)
        {
            if (value != null && value.Length > maxLength)
            {
                throw new JSException(JSErrMsg.ERR_CODE_PARAM_INVALID, string.Format(JSErrMsg.ERR_MSG_PARAM_INVALID, name));
            }
        }

        /// <summary>
        /// 验证以逗号分隔的字符串参数的最大列表长度。
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <param name="maxSize">最大列表长度</param>
        public static void ValidateMaxListSize(string name, string value, int maxSize)
        {
            if (value != null)
            {
                string[] data = value.Split(',');
                if (data != null && data.Length > maxSize)
                {
                    throw new JSException(JSErrMsg.ERR_CODE_PARAM_INVALID, string.Format(JSErrMsg.ERR_MSG_PARAM_INVALID, name));
                }
            }
        }

        /// <summary>
        /// 验证复杂结构数组参数的最大列表长度。
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <param name="maxSize">最大列表长度</param>
        public static void ValidateObjectMaxListSize(string name, string value, int maxSize)
        {
            if (value != null)
            {
                IList list = JSON.Parse(value) as IList;
                if (list != null && list.Count > maxSize)
                {
                    throw new JSException(JSErrMsg.ERR_CODE_PARAM_INVALID, string.Format(JSErrMsg.ERR_MSG_PARAM_INVALID, name));
                }
            }
        }

        /// <summary>
        /// 验证字符串参数的最小长度。
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <param name="minLength">最小长度</param>
        public static void ValidateMinLength(string name, string value, int minLength)
        {
            if (value != null && value.Length < minLength)
            {
                throw new JSException(JSErrMsg.ERR_CODE_PARAM_INVALID, string.Format(JSErrMsg.ERR_MSG_PARAM_INVALID, name));
            }
        }

        /// <summary>
        /// 验证数字参数的最大值。
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <param name="maxValue">最大值</param>
        public static void ValidateMaxValue(string name, Nullable<long> value, long maxValue)
        {
            if (value != null && value > maxValue)
            {
                throw new JSException(JSErrMsg.ERR_CODE_PARAM_INVALID, string.Format(JSErrMsg.ERR_MSG_PARAM_INVALID, name));
            }
        }

        /// <summary>
        /// 验证数字参数的最小值。
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <param name="minValue">最小值</param>
        public static void ValidateMinValue(string name, Nullable<long> value, long minValue)
        {
            if (value != null && value < minValue)
            {
                throw new JSException(JSErrMsg.ERR_CODE_PARAM_INVALID, string.Format(JSErrMsg.ERR_MSG_PARAM_INVALID, name));
            }
        }
    }
}
