using FastJSON;
using JSNet.BaseSys;
using JSNet.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSNet.Service
{
    public class KawuService:BaseService
    {
        private WebUtils webUtil = new WebUtils();
        private string _KawuAPIUrl = "http://ecard.huison.com/OrderSys/OrderPost.ashx";
        private string _KawuSecretKey = "huison88";

        /// <summary>
        /// 增加卡务系统微信用户
        /// </summary>
        public bool AddWeixinUser(string tel, string userName, out string errMessage)
        {
            errMessage = string.Empty;
            LoginResponse reponse = CallKawuAPI<LoginResponse>(new LoginRequest
            {
                type = "login",
                mobile = tel,
                username = userName,
            }, "get");

            if (reponse.Status == 1)
            {
                return true;
            }
            else
            {
                // TODO 失败记录log
                errMessage = reponse.Message;
                return false;
            }
        }

        /// <summary>
        /// 修改用户资料
        /// </summary>
        /// <param name="tel"></param>
        /// <param name="newTel"></param>
        /// <param name="userName"></param>
        /// <param name="errMessage"></param>
        /// <returns></returns>
        public bool ChangeUserData(string tel, string newTel, string userName, out string errMessage)
        {
            errMessage = string.Empty;
            ChangeResponse reponse = CallKawuAPI<ChangeResponse>(new ChangeRequest
            {
                type = "change",
                mobile = tel,
                newmobile = newTel,
                username = userName,
            }, "get");

            if (reponse.Status == 1)
            {
                return true;
            }
            else
            {
                // TODO 失败记录log
                errMessage = reponse.Message;
                return false;
            }
        }

        public bool WeixinPushMessage(out string errMessage)
        {
            errMessage = string.Empty;
            ChangeResponse reponse = CallKawuAPI<ChangeResponse>(new
            {
                type = "change",
            }, "get");

            if (reponse.Status == 1)
            {
                return true;
            }
            else
            {
                errMessage = reponse.Message;
                return false;
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="pToDecrypt"></param>
        /// <returns></returns>
        public string DecryptData(string pToDecrypt)
        {
            try
            {
                string json = SecretUtil.KawuDecrypt(pToDecrypt, _KawuSecretKey);
                return json;
            }
            catch(Exception e)
            {
                throw new JSException(JSErrMsg.ERR_CODE_APIDecryptFailed, e.Message);
            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="pToEncrypt"></param>
        /// <returns></returns>
        public string EncryptData(string pToEncrypt,bool urlEncode = true)
        {
            string json = SecretUtil.KawuEncrypt(pToEncrypt, _KawuSecretKey);
            return urlEncode ? CommonUtil.UrlEncode(json) : json;
        }

        private T CallKawuAPI<T>(object sumbitdata, string httpMethod)
            where T : KawuResponse, new()
        {
            string sumbitjson = FastJSON.JSON.ToJSON(sumbitdata, jsonParams);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("sumbitdata", EncryptData(sumbitjson, false));

            string reponseJson;
            try
            {
                switch (httpMethod.ToUpper())
                {
                    case "POST":
                        reponseJson = webUtil.DoPost(_KawuAPIUrl, dic);
                        break;
                    case "GET":
                        reponseJson = webUtil.DoGet(_KawuAPIUrl, dic);
                        break;
                    default:
                        throw new Exception(httpMethod + "，参数httpMethod有误，必须是post或get");
                }
            }
            catch(Exception ex)
            {
                throw new Exception("请求地址：" + _KawuAPIUrl + "出错，错误内容：" + ex.Message);
            }
            if (string.IsNullOrEmpty(reponseJson))
            {
                throw new Exception("请求地址：" + _KawuAPIUrl + "出错，返回内容为空。");
            }
            
            T reponse = FastJSON.JSON.ToObject<T>(reponseJson);
            return reponse;
        }


    }

    #region KawuRequest
    public class KawuRequest
    {
        public string type { get; set; }
    }

    public class LoginRequest : KawuRequest
    {
        public string mobile { get; set; }
        public string username { get; set; }
    }

    public class ChangeRequest : KawuRequest
    {
        public string mobile { get; set; }
        public string newmobile { get; set; }
        public string username { get; set; }
    } 
    #endregion


    #region KawuResponse
    public class KawuResponse
    {
        public int Status { get; set; }

        public string Json { get; set; }

        public string Message { get; set; }
    }

    public class LoginResponse : KawuResponse
    {
        public string Mobile { get; set; }
    }

    public class ChangeResponse : KawuResponse
    {
        public string UserId { get; set; }
    } 
    #endregion


}
