using FastJSON;
using JSNet.BaseSys;
using JSNet.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
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
        public bool AddWeixinUser(string tel, string userName)
        {
            LoginResponse reponse = CallKawuAPI<LoginResponse>(new LoginRequest
            {
                type = "login",
                mobile = tel,
                username = userName,
            }, "get");
            return true;
        }

        /// <summary>
        /// 修改用户资料
        /// </summary>
        /// <param name="tel"></param>
        /// <param name="newTel"></param>
        /// <param name="userName"></param>
        /// <param name="errMessage"></param>
        /// <returns></returns>
        public bool ChangeUserData(string tel, string newTel, string userName)
        {
            ChangeResponse reponse = CallKawuAPI<ChangeResponse>(new ChangeRequest
            {
                type = "change",
                mobile = tel,
                newmobile = newTel,
                username = userName,
            }, "get");
            return true;
        }

        public bool FinishOrder_VXPushMsg()
        {
            PushVXMsgResponse reponse = CallKawuAPI<PushVXMsgResponse>(new OrderFinish_PushVXMsgRequest
            {
                type = "notice",
                noticetype ="ordercompleted",
                openid = "o4xxqwOiP_DDyRBHcC68NZdcgV4I",
                first = "您好，您已完成工单任务",
                remark = "感谢您的使用。",
                jobdescription = "湖南江华发现一株宋朝时期珍稀青檀树 树龄近千岁 ---10月16日,湖南省江华瑶族自治县农林部门考察人员在该县涔天河镇会合社区发现一株宋朝时期种植的",
                station="正门",
                jobstarttime = "2017-10-10 16:55:12",
                jobendtime = "2017-10-17 16:55:12",
                collaborator = "李白,杜甫"
            }, "get");
            return true;
        }

        public bool AcceptOrder_VXPushMsg()
        {
            PushVXMsgResponse reponse = CallKawuAPI<PushVXMsgResponse>(new OrderAccept_PushVXMsgRequest
            {
                type = "notice",
                noticetype = "orderaccepted",
                openid = "o4xxqwOiP_DDyRBHcC68NZdcgV4I",
                first = "尊敬的用户您好",
                remark = "已接受您的请求。我们会立即帮您查看您的问题。",
                ordername = "湖南江华发现一株宋朝时期珍稀青檀树 树龄近千岁 ---10月16日,湖南省江华瑶族自治县农林部门考察人员在该县涔天河镇会合社区发现一株宋朝时期种植的湖南江华发现一株宋朝时期珍稀青檀树 树龄近千岁 ---10月16日,湖南省江华瑶族自治县农林部门考察人员在该县涔天河镇会合社区发现一株宋朝时期种植的湖南江华发现一株宋朝时期珍稀青檀树 树龄近千岁 ---10月16日,湖南省江华瑶族自治县农林部门考察人员在该县涔天河镇会合社区发现一株宋朝时期种植的湖南江华发现一株宋朝时期珍稀青檀树 树龄近千岁 ---10月16日,湖南省江华瑶族自治县农林部门考察人员在该县涔天河镇会合社区发现一株宋朝时期种植的",
                acceptanceengineer = "李白,杜甫",
                accepttime = "2017-10-17 19:10:10",
            }, "get");
            return true;
        }

        public bool CommonOrder_VXPushMsg(string openID,string title,DataRow orderDR)
        {
            PushVXMsgResponse reponse = CallKawuAPI<PushVXMsgResponse>(new OrderCommon_PushVXMsgRequest
            {
                type = "notice",
                noticetype = "dormitoryrepair",
                openid = openID,
                first = title,
                remark = string.IsNullOrEmpty(orderDR["Remark"].ToString()) ? "无" : orderDR["Remark"].ToString(),
                orderno = orderDR["OrderNo"].ToString(),
                reportperson = orderDR["StarterName"].ToString(),
                repairaddress = string.IsNullOrEmpty(orderDR["WorkingLocation"].ToString()) ? "无" : orderDR["WorkingLocation"].ToString(),
                orderdetail = orderDR["Content"].ToString(),
                reporttime = orderDR["StartTime"].ToString(),
            }, "get");
            return true;
        }

        public bool CheckOrder_VXPushMsg()
        {
            PushVXMsgResponse reponse = CallKawuAPI<PushVXMsgResponse>(new OrderCheck_PushVXMsgRequest
            {
                type = "notice",
                noticetype = "maintenanceserviceevaluation",
                openid = "o4xxqwOiP_DDyRBHcC68NZdcgV4I",
                first = "您好，你的维修单已处理完成，请您对本次服务进行评价",
                remark = "点击评价，非常感谢",
                orderno = "201701120002",
                treatment = "简单处理。"
            }, "get");
            return true;
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

        private T CallKawuAPI<T>(object sumbitdata, string httpMethod, bool ignoreException = true)
            where T : KawuResponse, new()
        {
            string sumbitjson = EncryptData(FastJSON.JSON.ToJSON(sumbitdata, jsonParams), false);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("submitdata", sumbitjson);

            string requestUrl = WebUtils.BuildRequestUrl(_KawuAPIUrl,dic);
            string reponseJson = string.Empty;
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
                if (string.IsNullOrEmpty(reponseJson))
                {
                    throw new JSException(
                        JSErrMsg.ERR_MSG_APIFailed,
                        JSErrMsg.ERR_Code_APIFailedReason,
                        string.Format(JSErrMsg.ERR_MSG_APIFailedReason, requestUrl, "接口返回内容为空！"));
                }

                T response = FastJSON.JSON.ToObject<T>(reponseJson);
                if (response.Status == -1)
                {
                    throw new JSException(
                        JSErrMsg.ERR_MSG_APIFailed,
                        JSErrMsg.ERR_Code_APIFailedReason,
                        string.Format(JSErrMsg.ERR_MSG_APIFailedReason, requestUrl, response.Message));
                }
                return response;
            }
            catch (JSException ex)
            {
                LogService logService = new LogService();
                logService.AddKawuApiLog(ex, requestUrl, reponseJson, sumbitdata.ToString());
                if (!ignoreException)
                {
                    throw ex;
                }
            }
            catch (Exception e)
            {
                JSException ex = new JSException(
                        JSErrMsg.ERR_MSG_APIFailed,
                        JSErrMsg.ERR_Code_APIFailedReason,
                        string.Format(JSErrMsg.ERR_MSG_APIFailedReason, requestUrl, e.ToString()));

                LogService logService = new LogService();
                logService.AddKawuApiLog(ex, requestUrl, reponseJson, sumbitdata.ToString());
                if (!ignoreException)
                {
                    throw ex;
                }
            }
            return null;
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

    public class PushVXMsgRequest:KawuRequest
    {
        public string noticetype{get;set;}
        public string openid { get; set; }
        public string first { get; set; }
        public string remark { get; set; }
    }
    public class OrderFinish_PushVXMsgRequest : PushVXMsgRequest
    {
        public string jobdescription { get; set; }
        public string station { get; set; }
        public string jobstarttime { get; set; }
        public string jobendtime { get; set; }
        public string collaborator { get; set; }
    }

    public class OrderAccept_PushVXMsgRequest : PushVXMsgRequest
    {
        public string ordername { get; set; }
        public string acceptanceengineer { get; set; }
        public string accepttime { get; set; }
    }

    public class OrderCommon_PushVXMsgRequest:PushVXMsgRequest
    {
        public string orderno{get;set;}
        public string reportperson{get;set;}
        public string repairaddress{get;set;}
        public string orderdetail{get;set;}
        public string reporttime{get;set;}
    }

    public class OrderCheck_PushVXMsgRequest : PushVXMsgRequest
    {
        public string orderno{get;set;}
        public string treatment{get;set;}
    }

    #endregion


    #region KawuResponse
    public class KawuResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
    }

    public class LoginResponse : KawuResponse
    {
        public string Json { get; set; }
        public string Mobile { get; set; }
    }

    public class ChangeResponse : KawuResponse
    {
        public string Json { get; set; }
        public string UserId { get; set; }
    }

    public class PushVXMsgResponse : KawuResponse
    {
    }
    #endregion


}
