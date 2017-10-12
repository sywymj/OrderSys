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
        private string _KawuAPIUrl = "/OrderPost.ashx";
        private string _KawuSecretKey = "huison88";

        /// <summary>
        /// 增加卡务系统微信用户
        /// </summary>
        public bool AddWeixinUser(string tel,string userName,out string message)
        {
            message = string.Empty;
            string sumbitdata = FastJSON.JSON.ToJSON(new
            {
                type = "login",
                mobile = tel,
                username = userName,
            });
            Dictionary<string,string> dic = new Dictionary<string,string>();
            dic.Add("sumbitdata", EncryptData(sumbitdata));
            string json = webUtil.DoGet(_KawuAPIUrl, dic);

            LoginResponse reponse = FastJSON.JSON.ToObject<LoginResponse>(json);
            if (reponse.Status == 1)
            {
                return true;
            }
            else
            {
                message = reponse.Message;
                return false;
            }
        }

        public bool ChangeUserData(string tel,string newTel,string userName,out string message)
        {
            message = string.Empty;
            string sumbitdata = FastJSON.JSON.ToJSON(new
            {
                type = "change",
                mobile = tel,
                newmobile = newTel,
                username = userName,
            });
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("sumbitdata", EncryptData(sumbitdata));
            string json = webUtil.DoGet(_KawuAPIUrl, dic);

            ChangeResponse reponse = FastJSON.JSON.ToObject<ChangeResponse>(json);
            if (reponse.Status == 1)
            {
                return true;
            }
            else
            {
                message = reponse.Message;
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
            catch
            {
                throw new JSException(JSErrMsg.ERR_CODE_APIDecryptFailed, JSErrMsg.ERR_MSG_APIDecryptFailed);
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
    }

    public class KawuResponse
    {
        public int Status { get; set; }

        public string Json { get; set; }

        public string Message { get; set; }
    }

    public class LoginResponse:KawuResponse
    {
        public string Mobile { get; set; }
    }

    public class ChangeResponse:KawuResponse
    {
        public string UserId { get; set; }
    }
}
