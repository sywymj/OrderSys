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
            if (required)
            {
                if (!CheckRequestUrlParms(key))
                {
                    throw new JSException(JSErrMsg.ERR_CODE_KEY_MISSING, string.Format(JSErrMsg.ERR_MSG_KEY_MISSING, key));
                }
            }
            else
            {
                if(!CheckRequestUrlParms(key))
                {
                    return string.Empty;
                }
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
            if (required)
            {
                if (!CheckRequestFormParms(key))
                {
                    throw new JSException(JSErrMsg.ERR_CODE_KEY_MISSING, string.Format(JSErrMsg.ERR_MSG_KEY_MISSING, key));
                }
            }
            else
            {
                if (!CheckRequestFormParms(key))
                {
                    return string.Empty;
                }
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

        /// <summary>
        /// 获取Cookie参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetCookieParm(string key)
        {
            if (!CheckCookieParms(key))
            {
                throw new JSException(JSErrMsg.ERR_CODE_COOKIEKEY_MISSING, string.Format(JSErrMsg.ERR_MSG_COOKIEKEY_MISSING, key));
            }
            return HttpContext.Current.Request.Cookies[key].ToString();
        }

        //检查Request参数是否为null或为空
        private static bool CheckRequestUrlParms(string key)
        {
            if (HttpContext.Current.Request[key] != null)
            {
                return true;
            }
            return false;
        }

        //检查Request参数是否为null或为空
        private static bool CheckRequestFormParms(string key)
        {
            if (HttpContext.Current.Request.Form[key] != null)
            {
                return true;
            }
            return false;
        }

        //检查Session参数是否为null或为空
        private static bool CheckSessionParms(string key)
        {
            if (HttpContext.Current.Session[key] != null)
            {
                return true;
            }
            return false;
        }

        private static bool CheckCookieParms(string key)
        {
            if (HttpContext.Current.Request.Cookies[key] != null)
            {
                return true;
            }
            return false;
        }
    }
}
