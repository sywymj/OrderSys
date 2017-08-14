using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace JSNet.BaseSys
{
    public class JSRequest
    {
        /// <summary>
        /// 获取URL参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetRequestUrlParm(string key,bool required = true)
        {
            if (!required)
            {
                return string.Empty;
            }
            if (!CheckRequestUrlParms(key))
            {
                throw new JSException(JSErrMsg.ERR_CODE_KEY_MISSING, string.Format(JSErrMsg.ERR_MSG_KEY_MISSING, key));
            }
            return HttpContext.Current.Request[key].ToString();
        }

        /// <summary>
        /// 获取Form参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetRequestFormParm(string key,bool required=true)
        {
            if (!required)
            {
                return string.Empty;
            }
            if (!CheckRequestFormParms(key))
            {
                throw new JSException(JSErrMsg.ERR_CODE_KEY_MISSING, string.Format(JSErrMsg.ERR_MSG_KEY_MISSING, key));
            }
            return HttpContext.Current.Request[key].ToString();
        }

        /// <summary>
        /// 获取Session参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSessionParm(string key)
        {
            if(!CheckSessionParms(key))
            {
                throw new JSException(JSErrMsg.ERR_CODE_SESSIONKEY_MISSING, string.Format(JSErrMsg.ERR_MSG_SESSIONKEY_MISSING, key));
            }
            return HttpContext.Current.Session[key].ToString();
        }

        //检查Request参数是否为null或为空
        private static bool CheckRequestUrlParms(string key)
        {
            if (HttpContext.Current.Request[key] != null && !string.IsNullOrWhiteSpace(HttpContext.Current.Request[key].ToString()))
            {
                return true;
            }
            return false;
        }

        //检查Request参数是否为null或为空
        private static bool CheckRequestFormParms(string key)
        {
            if (HttpContext.Current.Request.Form[key] != null && !string.IsNullOrWhiteSpace(HttpContext.Current.Request.Form[key].ToString()))
            {
                return true;
            }
            return false;
        }

        //检查Session参数是否为null或为空
        private static bool CheckSessionParms(string key)
        {
            if (HttpContext.Current.Session[key] != null && !string.IsNullOrWhiteSpace(HttpContext.Current.Session[key].ToString()))
            {
                return true;
            }
            return false;
        }
    }
}
