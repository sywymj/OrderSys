using FastJSON;
using JSNet.BaseSys;
using JSNet.Model;
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
            }, "get", false);
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

        public bool FinishOrder_VXPushMsg(int staffid, string title, DataRow orderDR)
        {
            UserService userService = new UserService();
            StaffEntity staff = userService.GetStaff(staffid);
            UserEntity user = userService.GetUser((int)staff.UserID);

            PushVXMsgResponse reponse = CallKawuAPI<PushVXMsgResponse>(new OrderFinish_PushVXMsgRequest
            {
                type = "notice",
                noticetype ="ordercompleted",
                openid = user.OpenID,
                first = title,
                remark = "感谢您的使用。",
                url = "http://ordersys.huison.com/Weixin/Order/OrderFlows?OrderID=" + orderDR["ID"],
                jobdescription =orderDR["Content"].ToString() ,
                station = orderDR["WorkingLocation"].ToString(),
                jobstarttime = orderDR["StartTime"].ToString(),
                jobendtime = orderDR["FinishTime"].ToString(),
                collaborator = orderDR["HandlerName"].ToString(),
            }, "get");
            return true;
        }

        public bool AcceptOrder_VXPushMsg(int staffid,string title,DataRow orderDR)
        {
            UserService userService = new UserService();
            StaffEntity staff = userService.GetStaff(staffid);
            UserEntity user = userService.GetUser((int)staff.UserID);

            PushVXMsgResponse reponse = CallKawuAPI<PushVXMsgResponse>(new OrderAccept_PushVXMsgRequest
            {
                type = "notice",
                noticetype = "orderaccepted",
                openid = user.OpenID,
                first = title,                                                  //标题
                remark = "已受理您的工单。我们会及时解决您的问题。",            //备注
                url = "http://ordersys.huison.com/Weixin/Order/OrderFlows?OrderID=" + orderDR["ID"],
                ordername = orderDR["Content"].ToString(),                      //工单内容
                acceptanceengineer = orderDR["HandlerName"].ToString(),         //受理人
                accepttime = orderDR["OperateTime"].ToString(),                 //受理时间
            }, "get");
            return true;
        }

        public bool Test_VXPushMsg()
        {

            PushVXMsgResponse reponse = CallKawuAPI<PushVXMsgResponse>(new OrderAccept_PushVXMsgRequest
            {
                type = "notice",
                noticetype = "orderaccepted",
                openid = "o4xxqwOiP_DDyRBHcC68NZdcgV4I",
                first = "标题",                                                  //标题
                remark = "已受理您的工单。我们会及时解决您的问题。",            //备注
                url = "http://ordersys.huison.com/Weixin/Order/OrderFlows",
                ordername = "工单内容",                      //工单内容
                acceptanceengineer = "受理人",         //受理人
                accepttime = "受理时间",                 //受理时间
            }, "get");
            return true;
        }

        public bool CommonOrder_VXPushMsg(int staffid, string title, DataRow orderDR)
        {
            UserService userService = new UserService();
            StaffEntity staff = userService.GetStaff(staffid);
            UserEntity user = userService.GetUser((int)staff.UserID);

            PushVXMsgResponse reponse = CallKawuAPI<PushVXMsgResponse>(new OrderCommon_PushVXMsgRequest
            {
                type = "notice",
                noticetype = "dormitoryrepair",
                openid = user.OpenID,
                first = title,
                remark = string.IsNullOrEmpty(orderDR["Remark"].ToString()) ? "无" : orderDR["Remark"].ToString(),                               //备注
                url = "http://ordersys.huison.com/Weixin/Order/OrderFlows?OrderID=" + orderDR["ID"],
                orderno = orderDR["OrderNo"].ToString(),                                                                                         //工单号
                reportperson = orderDR["StarterName"].ToString(),                                                                                //发起人
                repairaddress = string.IsNullOrEmpty(orderDR["WorkingLocation"].ToString()) ? "无" : orderDR["WorkingLocation"].ToString(),      //工作地点
                orderdetail = orderDR["Content"].ToString(),                                                                                     //工单内容
                reporttime = orderDR["StartTime"].ToString(),                                                                                    //发起时间
            }, "get");
            return true;
        }

        public bool CheckOrder_VXPushMsg(int staffid, string title, DataRow orderDR)
        {
            UserService userService = new UserService();
            StaffEntity staff = userService.GetStaff(staffid);
            UserEntity user = userService.GetUser((int)staff.UserID);

            PushVXMsgResponse reponse = CallKawuAPI<PushVXMsgResponse>(new OrderCheck_PushVXMsgRequest
            {
                type = "notice",
                noticetype = "maintenanceserviceevaluation",
                openid = user.OpenID,
                first = title,
                remark = "点击验收，非常感谢",
                url = "http://ordersys.huison.com/Weixin/Order/OrderFlows?OrderID=" + orderDR["ID"],
                orderno = orderDR["OrderNo"].ToString(),
                treatment = "点击查看",
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
                throw new JSException(JSErrMsg.ERR_MSG_APIDecryptFailed,JSErrMsg.ERR_CODE_APIDecryptFailed, e.Message);
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
        public string url { get; set; }
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
