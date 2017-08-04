using JSNet.DbUtilities;
using System;
using System.Globalization;

namespace JSNet.BaseSys
{
    public class BaseConfiguration
    {
        #region public static void GetSetting() 读取配置信息
        /// <summary>
        /// 读取配置信息
        /// </summary>
        public static void GetSetting()
        {
            //读取 web.config 配置
            ConfigurationHelper.GetConfig();
        }
        #endregion

        #region public static DbType GetDbType(string dbType) 数据库连接的类型判断
        /// <summary>
        /// 数据库连接的类型判断
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <returns>数据库类型</returns>
        public static DbTypeName GetDbType(string dbType)
        {
            DbTypeName returnValue = DbTypeName.SqlServer;
            foreach (DbTypeName currentDbType in Enum.GetValues(typeof(DbTypeName)))
            {
                if (currentDbType.ToString().Equals(dbType))
                {
                    returnValue = currentDbType;
                    break;
                }
            }
            return returnValue;
        }
        #endregion




    }
}