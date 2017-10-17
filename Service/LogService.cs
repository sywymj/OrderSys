using JSNet.BaseSys;
using JSNet.Manager;
using JSNet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSNet.Service
{
    public class LogService:BaseService
    {
        public void AddKawuApiLog(JSException ex, string requestUrl, string responseJson,string apiType)
        {
            ApiLogEntity log = new ApiLogEntity();
            log.RequestUrl= requestUrl;
            log.Response= responseJson;
            log.ApiType = apiType;
            AddApiLog(ex, log);
        }

        public void AddApiLog(JSException ex,ApiLogEntity log)
        {
            log.ErrorCode = ex.ErrorCode;
            log.ErrorMsg = ex.ErrorMsg;
            log.Message = ex.Message;
            AddApiLog(log);
        }

        public void AddApiLog(Exception ex,ApiLogEntity log)
        {
            log.ErrorCode = "500";
            log.ErrorMsg = ex.ToString();
            log.Message = "服务器出错！";
            AddApiLog(log);
        }

        public void AddApiLog(ApiLogEntity log)
        {
            log.DateTime = DateTime.Now;
            EntityManager<ApiLogEntity> manager = new EntityManager<ApiLogEntity>();
            manager.Insert(log);
        }
    }
}
