using System;
using System.Data;
using System.Data.Common;

namespace JSNet.DbUtilities
{
    public class DbHelper
    {
        /// <summary> 构造方法
        /// 构造方法
        /// </summary>
        private DbHelper()
        {
        }

        /// <summary>
        /// 按数据类型获取数据库访问实现类
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <returns>数据库访问实现类</returns>
        public static string GetDbHelperClass(DbTypeName dbType)
        {
            string returnValue = "DotNet.Utilities.SqlHelper";
            switch (dbType)
            {
                case DbTypeName.SqlServer:
                    returnValue = "DotNet.Utilities.SqlHelper";
                    break;
                case DbTypeName.Oracle:
                    returnValue = "DotNet.Utilities.MSOracleHelper";
                    break;
                case DbTypeName.Access:
                    returnValue = "DotNet.Utilities.OleDbHelper";
                    break;
                case DbTypeName.MySql:
                    returnValue = "DotNet.Utilities.MySqlHelper";
                    break;
                case DbTypeName.DB2:
                    returnValue = "DotNet.Utilities.DB2Helper";
                    break;
                case DbTypeName.SQLite:
                    returnValue = "DotNet.Utilities.SqLiteHelper";
                    break;
            }
            return returnValue;
        }
    }
}