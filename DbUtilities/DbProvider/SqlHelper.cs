﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace JSNet.DbUtilities
{
    public class SqlHelper : BaseDbHelper, IDbHelper, IDbHelperExpand
    {
        public override DbProviderFactory GetInstance()
        {
            return SqlClientFactory.Instance;
        }

        #region public override CurrentDbType CurrentDbType 获得当前数据库类型
        /// <summary>
        /// 获得当前数据库类型
        /// </summary>
        public override DbTypeName CurrentDbType
        {
            get
            {
                return DbTypeName.SqlServer;
            }
        }
        #endregion

        #region public SqlHelper() 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public SqlHelper()
        {
            FileName = "SqlHelper.txt"; // sql查询句日志
        }
        #endregion

        #region public SqlHelper(string connectionString) 构造函数,设置数据库连接
        /// <summary>
        /// 构造函数,设置数据库连接
        /// </summary>
        /// <param name="connectionString">数据连接</param>
        public SqlHelper(string connectionString)
        {
            this.ConnectionString = connectionString;
        }
        #endregion

        #region public string GetDBDateTime() 获得数据库日期时间 执行SQL后的结果
        /// <summary>
        /// 获得数据库日期时间
        /// </summary>
        /// <returns>日期时间</returns>
        public string GetDBDateTime()
        {
            string commandText = " SELECT " + this.GetDBNow();
            this.Open();
            string dateTime = this.ExecuteScalar(commandText, null, CommandType.Text).ToString();
            this.Close();
            return dateTime;
        }
        #endregion

        #region public string GetDBNow() 获得数据库日期时间 字符串
        /// <summary>
        /// 获得数据库当前日期时间
        /// </summary>
        /// <returns>日期时间</returns>
        public string GetDBNow()
        {
            return " Getdate() ";
        }
        #endregion

        #region string PlusSign(params string[] values) 获得Sql字符串相加符号
        /// <summary>
        ///  获得Sql字符串相加符号
        /// </summary>
        /// <param name="values">参数值</param>
        /// <returns>字符加</returns>
        public new string PlusSign(params string[] values)
        {
            string returnValue = string.Empty;
            for (int i = 0; i < values.Length; i++)
            {
                returnValue += values[i] + " + ";
            }
            if (!String.IsNullOrEmpty(returnValue))
            {
                returnValue = returnValue.Substring(0, returnValue.Length - 3);
            }
            else
            {
                returnValue = " + ";
            }
            return returnValue;
        }
        #endregion

        #region public string GetParameter(string parameter) 获得参数Sql表达式
        /// <summary>
        /// 获得参数Sql表达式
        /// </summary>
        /// <param name="parameter">参数名称</param>
        /// <returns>字符串</returns>
        public string GetParameter(string parameterName)
        {
            return "@" + parameterName + "";
        }
        #endregion

        #region string[] GetParameters(string[] parameterNames) 获得参数Sql表达式数组
        /// <summary>
        /// 获得参数Sql表达式
        /// </summary>
        /// <param name="parameter">参数名称</param>
        /// <returns>字符串</returns>
        public string[] GetParameters(string[] parameterNames)
        {
            string[] parameters = new string[parameterNames.Length];
            for (int i = 0; i < parameterNames.Length; i++)
            {
                parameters[i] = this.GetParameter(parameterNames[i]);
            }
            return parameters;
        }
        #endregion

        #region public IDbDataParameter MakeParameter(string targetFiled, object targetValue) 获取参数
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="targetFiled">目标字段</param>
        /// <param name="targetValue">值</param>
        /// <returns>参数集</returns>
        public IDbDataParameter MakeParameter(string targetFiled, object targetValue)
        {
            IDbDataParameter dbParameter = null;
            if (targetFiled != null)
            {
                dbParameter = this.MakeInParam(targetFiled, targetValue==null ? DBNull.Value : targetValue);
            }
            return dbParameter;
        }
        #endregion

        #region public IDbDataParameter MakeInParam(string targetFiled, object targetValue) 获取参数实际方法
        /// <summary>
        /// 获取参数实际方法
        /// </summary>
        /// <param name="targetFiled">目标字段</param>
        /// <param name="targetValue">值</param>
        /// <returns>参数</returns>
        public IDbDataParameter MakeInParam(string targetFiled, object targetValue)
        {
            return new SqlParameter(this.GetParameter(targetFiled), targetValue);
        }
        #endregion

        #region public IDbDataParameter[] MakeParameters(string[] targetFileds, Object[] targetValues) 获取参数数组集合
        /// <summary>
        /// 获取参数数组集合
        /// </summary>
        /// <param name="targetFiled">目标字段</param>
        /// <param name="targetValue">值</param>
        /// <returns>参数集</returns>
        public IDbDataParameter[] MakeParameters(string[] targetFileds, Object[] targetValues)
        {
            // 这里需要用泛型列表，因为有不合法的数组的时候
            List<IDbDataParameter> dbParameters = new List<IDbDataParameter>();
            if (targetFileds != null && targetValues != null)
            {
                for (int i = 0; i < targetFileds.Length; i++)
                {
                    if (targetFileds[i] != null && targetValues[i] != null && (!(targetValues[i] is Array)))
                    {
                        dbParameters.Add(this.MakeInParam(targetFileds[i], targetValues[i]));
                    }
                }
            }
            return dbParameters.ToArray();
        }
        #endregion

        #region public IDbDataParameter[] MakeParameters(List<KeyValuePair<string, object>> parameters) 获取参数泛型列表
        /// <summary>
        /// 获取参数泛型列表
        /// </summary>
        /// <param name="parameters">参数</param>
        /// <returns>参数集</returns>
        public IDbDataParameter[] MakeParameters(List<KeyValuePair<string, object>> parameters)
        {
            // 这里需要用泛型列表，因为有不合法的数组的时候
            List<IDbDataParameter> dbParameters = new List<IDbDataParameter>();
            if (parameters != null && parameters.Count > 0)
            {
                foreach (var parameter in parameters)
                {
                    if (parameter.Key != null && parameter.Value != null && (!(parameter.Value is Array)))
                    {
                        dbParameters.Add(MakeParameter(parameter.Key, parameter.Value));
                    }
                }
            }
            return dbParameters.ToArray();
        }
        #endregion

        #region public IDbDataParameter MakeOutParam(string paramName, DbType dbType, int size) 获取输出参数
        /// <summary>
        /// 获取输出参数
        /// </summary>
        /// <param name="paramName">参数</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="size">长度</param>
        /// <returns></returns>
        public IDbDataParameter MakeOutParam(string paramName, string sqlDbType, int size)
        {
            return MakeParameter(paramName, null, sqlDbType, size, ParameterDirection.Output);
        }
        #endregion

        #region public IDbDataParameter MakeInParam(string paramName, DbType dbType, int Size, object value) 获取输入参数
        /// <summary>
        /// 获取输入参数
        /// </summary>
        /// <param name="paramName">参数</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="Size">长度</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public IDbDataParameter MakeInParam(string paramName, string sqlDbType, int Size, object value)
        {
            return MakeParameter(paramName, value, sqlDbType, Size, ParameterDirection.Input);
        }
        #endregion

        #region 获取参数，包含详细参数设置 public IDbDataParameter MakeParameter(string parameterName, object parameterValue, DbType dbType, Int32 parameterSize, ParameterDirection parameterDirection)
        /// <summary>
        /// 获取参数，包含详细参数设置
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="parameterValue">值</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="parameterSize">长度</param>
        /// <param name="parameterDirection">参数类型</param>
        /// <returns>参数</returns>
        public IDbDataParameter MakeParameter(string parameterName, object parameterValue, string sqlDbType, int parameterSize, ParameterDirection parameterDirection)
        {
            SqlParameter parameter;

            if (parameterSize > 0)
            {
                parameter = new SqlParameter(this.GetParameter(parameterName), ConvertToSqlDbType(sqlDbType), parameterSize);
            }
            else
            {
                parameter = new SqlParameter(this.GetParameter(parameterName), ConvertToSqlDbType(sqlDbType));
            }

            parameter.Direction = parameterDirection;
            if (!(parameterDirection == ParameterDirection.Output && parameterValue == null))
            {
                parameter.Value = parameterValue;
            }

            return parameter;
        }
        #endregion

        #region   private System.Data.SqlDbType ConvertToSqlDbType(System.Data.DbType dbType) 类型转换
        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="dbType">数据类型</param>
        /// <returns>转换结果</returns>
        private System.Data.SqlDbType ConvertToSqlDbType(string sqlDbType)
        {
            SqlParameter sqlParameter = new SqlParameter();
            sqlParameter.SqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), sqlDbType);
            return sqlParameter.SqlDbType;
        }
        #endregion

        #region public void SqlBulkCopyData(DataTable dataTable) 利用Net SqlBulkCopy 批量导入数据库,速度超快
        /// <summary>
        /// 利用Net SqlBulkCopy 批量导入数据库,速度超快
        /// </summary>
        /// <param name="dataTable">源内存数据表</param>
        public void SqlBulkCopyData(DataTable dataTable)
        {
            // SQL 数据连接
            SqlConnection conn = null;
            // 打开数据库
            this.Open();

            // 获取连接
            conn = (SqlConnection)GetDbConnection();

            using (SqlTransaction tran = conn.BeginTransaction())
            {
                // 批量保存数据，只能用于Sql
                SqlBulkCopy sqlbulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, tran);
                // 设置源表名称
                sqlbulkCopy.DestinationTableName = dataTable.TableName;
                // 设置超时限制
                sqlbulkCopy.BulkCopyTimeout = 1000;

                foreach (DataColumn dtColumn in dataTable.Columns)
                {
                    sqlbulkCopy.ColumnMappings.Add(dtColumn.ColumnName, dtColumn.ColumnName);
                }
                try
                {
                    // 写入
                    sqlbulkCopy.WriteToServer(dataTable);
                    // 提交事务
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    sqlbulkCopy.Close();
                }
                finally
                {
                    sqlbulkCopy.Close();
                    this.Close();
                }

            }
        }
        #endregion
    }
}