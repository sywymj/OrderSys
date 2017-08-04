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
            #region 数据库相关的配置
            // 数据库类型
            if (ConfigurationManager.AppSettings["CenterDbType"] != null)
            {
                BaseSystemInfo.CenterDbType = BaseConfiguration.GetDbType(ConfigurationManager.AppSettings["CenterDbType"]);
            }
            if (ConfigurationManager.AppSettings["BusinessDbType"] != null)
            {
                BaseSystemInfo.BusinessDbType = BaseConfiguration.GetDbType(ConfigurationManager.AppSettings["BusinessDbType"]);
            }
            if (ConfigurationManager.AppSettings["WorkFlowDbType"] != null)
            {
                BaseSystemInfo.WorkFlowDbType = BaseConfiguration.GetDbType(ConfigurationManager.AppSettings["WorkFlowDbType"]);
            }

            // 数据库连接字符串
            if (ConfigurationManager.AppSettings["BusinessDbConnection"] != null)
            {
                BaseSystemInfo.BusinessDbConnectionString = ConfigurationManager.AppSettings["BusinessDbConnection"];
            }
            if (ConfigurationManager.AppSettings["CenterDbConnection"] != null)
            {
                BaseSystemInfo.CenterDbConnectionString = ConfigurationManager.AppSettings["CenterDbConnection"];
            }
            if (ConfigurationManager.AppSettings["WorkFlowDbConnection"] != null)
            {
                BaseSystemInfo.WorkFlowDbConnectionString = ConfigurationManager.AppSettings["WorkFlowDbConnection"];
            }

            // 对加密的数据库连接进行解密操作
            if (ConfigurationManager.AppSettings["EncryptDbConnection"] != null)
            {
                BaseSystemInfo.EncryptDbConnection = ConfigurationManager.AppSettings["EncryptDbConnection"].ToUpper().Equals(true.ToString().ToUpper());
            }
            if (BaseSystemInfo.EncryptDbConnection)
            {
                BaseSystemInfo.BusinessDbConnection = SecretUtil.Decrypt(BaseSystemInfo.BusinessDbConnectionString);
                BaseSystemInfo.CenterDbConnection = SecretUtil.Decrypt(BaseSystemInfo.CenterDbConnectionString);
                BaseSystemInfo.WorkFlowDbConnection = SecretUtil.Decrypt(BaseSystemInfo.WorkFlowDbConnectionString);
            }
            else
            {
                BaseSystemInfo.BusinessDbConnection = BaseSystemInfo.BusinessDbConnectionString;
                BaseSystemInfo.CenterDbConnection = BaseSystemInfo.CenterDbConnectionString;
                BaseSystemInfo.WorkFlowDbConnection = BaseSystemInfo.WorkFlowDbConnectionString;
            } 
            #endregion
        }
        #endregion
    }
}