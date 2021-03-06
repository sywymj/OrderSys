﻿using System;
using System.Reflection;

namespace JSNet.DbUtilities
{
    public class DbHelperFactory
    {
        /// <summary>
        /// 获取指定的数据库连接
        /// </summary>
        /// <param name="connectionString">数据库连接串</param>
        /// <returns>数据库访问类</returns>
        public static IDbHelper GetHelper(string connectionString)
        {
            DbTypeName dbType = DbTypeName.SqlServer;
            return GetHelper(connectionString, dbType);
        }

        /// <summary>
        /// 获取指定的数据库连接
        /// </summary>
        /// <param name="dataBaseType">数据库类型（默认：SqlServer）</param>
        /// <param name="connectionString">数据库连接串（默认：CenterDbConnection）</param>
        /// <returns>数据库访问类</returns>
        public static IDbHelper GetHelper(string connectionString, DbTypeName dbType = DbTypeName.SqlServer)
        {
            // 这里是每次都获取新的数据库连接,否则会有并发访问的问题存在
            string dbHelperClass = GetDbHelperClass(dbType);
            Assembly assembly = Assembly.Load("JSNet.DbUtilities");//JSNet.DbUtilities.SqlHelper
            IDbHelper dbHelper = (IDbHelper)assembly.CreateInstance(dbHelperClass, true);
            if (!string.IsNullOrEmpty(connectionString))
            {
                dbHelper.ConnectionString = connectionString;
            }
            else
            {
                throw new Exception("connectionString is null");
            }
            return dbHelper;
        }

        /// <summary>
        /// 按数据类型获取数据库访问实现类
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <returns>数据库访问实现类</returns>
        public static string GetDbHelperClass(DbTypeName dbType)
        {
            string returnValue = "JSNet.DbUtilities.SqlHelper";
            switch (dbType)
            {
                case DbTypeName.SqlServer:
                    returnValue = "JSNet.DbUtilities.SqlHelper";
                    break;
                case DbTypeName.Oracle:
                    returnValue = "JSNet.DbUtilities.MSOracleHelper";
                    break;
                case DbTypeName.Access:
                    returnValue = "JSNet.DbUtilities.OleDbHelper";
                    break;
                case DbTypeName.MySql:
                    returnValue = "JSNet.DbUtilities.MySqlHelper";
                    break;
                case DbTypeName.DB2:
                    returnValue = "JSNet.DbUtilities.DB2Helper";
                    break;
                case DbTypeName.SQLite:
                    returnValue = "JSNet.DbUtilities.SqLiteHelper";
                    break;
            }
            return returnValue;
        }
    }
}