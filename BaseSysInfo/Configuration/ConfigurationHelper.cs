using JSNet.Utilities;
using System;
using System.Configuration;
using System.Globalization;

namespace JSNet.BaseSys
{
    public class ConfigurationHelper
    {
        #region public static void GetConfig()
        /// <summary>
        /// 从配置信息获取配置信息
        /// </summary>
        /// <param name="configuration">配置</param>
        public static void GetConfig()
        {
            // 对加密的数据库连接进行解密操作
            if (ConfigurationManager.AppSettings["EncryptDbConnection"] != null)
            {
                BaseSystemInfo.EncryptDbConnection = ConfigurationManager.AppSettings["EncryptDbConnection"].ToUpper().Equals(true.ToString().ToUpper());
            }


            #region 数据库相关的配置

            #region 主要数据库
            if (ConfigurationManager.AppSettings["CenterDbType"] != null)
            {
                BaseSystemInfo.CenterDbType = BaseConfiguration.GetDbType(ConfigurationManager.AppSettings["CenterDbType"]);
            }
            if (ConfigurationManager.AppSettings["CenterDbConnection"] != null)
            {
                BaseSystemInfo.CenterDbConnection = ConfigurationManager.AppSettings["CenterDbConnection"];
            }
            if (BaseSystemInfo.EncryptDbConnection)
            {
                BaseSystemInfo.CenterDbConnectionString = SecretUtil.Decrypt(BaseSystemInfo.CenterDbConnection);
            }
            else
            {
                BaseSystemInfo.CenterDbConnectionString = BaseSystemInfo.CenterDbConnection;
            } 
            #endregion


            #region 业务数据库
            if (ConfigurationManager.AppSettings["BusinessDbType"] != null)
            {
                BaseSystemInfo.BusinessDbType = BaseConfiguration.GetDbType(ConfigurationManager.AppSettings["BusinessDbType"]);
            }
            if (ConfigurationManager.AppSettings["BusinessDbConnection"] != null)
            {
                BaseSystemInfo.BusinessDbConnection = ConfigurationManager.AppSettings["BusinessDbConnection"];
            }
            if (BaseSystemInfo.EncryptDbConnection)
            {
                BaseSystemInfo.BusinessDbConnectionString = SecretUtil.Decrypt(BaseSystemInfo.BusinessDbConnection);
            }
            else
            {
                BaseSystemInfo.BusinessDbConnectionString = BaseSystemInfo.BusinessDbConnection;
            } 
            #endregion


            #region 工作流数据库
            if (ConfigurationManager.AppSettings["WorkFlowDbType"] != null)
            {
                BaseSystemInfo.WorkFlowDbType = BaseConfiguration.GetDbType(ConfigurationManager.AppSettings["WorkFlowDbType"]);
            }
            if (ConfigurationManager.AppSettings["WorkFlowDbConnection"] != null)
            {
                BaseSystemInfo.WorkFlowDbConnection = ConfigurationManager.AppSettings["WorkFlowDbConnection"];
            }
            if (BaseSystemInfo.EncryptDbConnection)
            {
                BaseSystemInfo.WorkFlowDbConnectionString = SecretUtil.Decrypt(BaseSystemInfo.WorkFlowDbConnection);
            }
            else
            {
                BaseSystemInfo.WorkFlowDbConnectionString = BaseSystemInfo.WorkFlowDbConnection;
            } 
            #endregion


            #region 卡务数据库

            if (ConfigurationManager.AppSettings["KawuDbType"] != null)
            {
                BaseSystemInfo.KawuDbType = BaseConfiguration.GetDbType(ConfigurationManager.AppSettings["KawuDbType"]);
            }
            if (ConfigurationManager.AppSettings["KawuDbConnection"] != null)
            {
                BaseSystemInfo.KawuDbConnection = ConfigurationManager.AppSettings["KawuDbConnection"];
            }
            if (BaseSystemInfo.EncryptDbConnection)
            {
                BaseSystemInfo.KawuDbConnectionString = SecretUtil.Decrypt(BaseSystemInfo.KawuDbConnection);
            }
            else
            {
                BaseSystemInfo.KawuDbConnectionString = BaseSystemInfo.KawuDbConnection;
            }  
            #endregion

            #endregion
        }
        #endregion
    }
}