using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace JSNet.DbUtilities
{
    public interface IDbHelper : IDisposable
    {
        string DateTimeFormat { get; set; }

        /// <summary>
        /// 创建提供程序对数据源类的实现的实例。
        /// </summary>
        /// <returns>数据源类的实现的实例</returns>
        DbProviderFactory GetInstance();

        /// <summary>
        /// 当前数据库类型
        /// </summary>
        DbTypeName CurrentDbType { get; }

        /// <summary>
        /// 默认打开关闭数据库选项（默认为否）
        /// </summary>
        bool AutoOpenClose { get; set; }

        string ConnectionString { get; set; }

        /// <summary>
        /// 获得数据库当前日期
        /// </summary>
        /// <returns>当前日期</returns>
        string GetDBNow();

        /// <summary>
        /// 获得数据库当前日期
        /// </summary>
        /// <returns>当前日期</returns>
        string GetDBDateTime();

        /// <summary>
        /// 是否已在事务之中
        /// </summary>
        /// <returns></returns>
        bool InTransaction
        {
            get;
            set;
        }

        /// <summary>
        /// 检查参数的安全性
        /// </summary>
        /// <param name="value">参数</param>
        /// <returns>安全的参数</returns>
        string SqlSafe(string value);

        /// <summary>
        ///  获得Sql字符串相加符号
        /// </summary>
        /// <returns>字符加</returns>
        string PlusSign();

        /// <summary>
        ///  获得Sql字符串相加符号
        /// </summary>
        /// <param name="values">参数值</param>
        /// <returns>字符加</returns>
        string PlusSign(params string[] values);

        /// <summary>
        /// 获得Sql参数表达式
        /// </summary>
        /// <param name="parameter">参数名称</param>
        /// <returns>字符串</returns>
        string GetParameter(string parameter);

        /// <summary>
        /// 获得Sql参数表达式数组
        /// </summary>
        /// <param name="parameter">参数名称数组</param>
        /// <returns>字符串数组</returns>
        string[] GetParameters(string[] parameters);

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="targetFiled">目标字段</param>
        /// <param name="targetValue">值</param>
        /// <returns>参数</returns>
        IDbDataParameter MakeParameter(string targetFiled, object targetValue);

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="targetFiled">目标字段</param>
        /// <param name="targetValue">值</param>
        /// <returns>参数集</returns>
        IDbDataParameter[] MakeParameters(string[] targetFileds, Object[] targetValues);

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="parameters">参数</param>
        /// <returns>参数集</returns>
        IDbDataParameter[] MakeParameters(List<KeyValuePair<string, object>> parameters);

        /// <summary>
        /// 生成参数
        /// </summary>
        /// <param name="parameterName">目标字段</param>
        /// <param name="parameterValue">值</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="parameterSize">长度</param>
        /// <param name="parameterDirection">输入输出类型</param>
        /// <returns>参数</returns>
        IDbDataParameter MakeParameter(string parameterName, object parameterValue, string dbType, int parameterSize, ParameterDirection parameterDirection);

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <returns>数据库连接</returns>
        IDbConnection GetDbConnection();

        /// <summary>
        /// 获取数据源上执行的事务
        /// </summary>
        /// <returns>数据源上执行的事务</returns>
        IDbTransaction GetDbTransaction();

        /// <summary>
        /// 获取数据源上命令
        /// </summary>
        /// <returns>数据源上命令</returns>
        IDbCommand GetDbCommand();

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <returns>数据库连接</returns>
        IDbConnection Open();

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <returns>数据库连接</returns>
        IDbConnection Open(string connectionString);

        /// <summary>
        /// 事务开始
        /// </summary>
        /// <returns>事务</returns>
        IDbTransaction BeginTransaction();

        /// <summary>
        /// 提交事务
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// 回滚事务
        /// </summary>
        void RollbackTransaction();

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        void Close();

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="commandText">sql查询</param>
        /// <returns>结果集流</returns>
        IDataReader ExecuteReader(string commandText);

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="commandText">sql查询</param>
        /// <param name="dbParameters">参数集</param>
        /// <returns>结果集流</returns>
        IDataReader ExecuteReader(string commandText, IDbDataParameter[] dbParameters);

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="commandText">sql查询</param>
        /// <param name="dbParameters">参数集</param>
        /// <param name="commandType">命令分类</param>
        /// <returns>结果集流</returns>
        IDataReader ExecuteReader(string commandText, IDbDataParameter[] dbParameters, CommandType commandType);

        /// <summary>
        /// 执行查询, SQL BUILDER 用了这个东西？参数需要保存, 不能丢失.
        /// </summary>
        /// <param name="commandText">sql查询</param>
        /// <returns>影响行数</returns>
        int ExecuteNonQuery(string commandText);

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="commandText">sql查询</param>
        /// <param name="dbParameters">参数集</param>
        /// <returns>影响行数</returns>
        int ExecuteNonQuery(string commandText, IDbDataParameter[] dbParameters);

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="commandText">sql查询</param>
        /// <param name="commandType">命令分类</param>
        /// <returns>影响行数</returns>
        int ExecuteNonQuery(string commandText, CommandType commandType);

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="commandText">sql查询</param>
        /// <param name="dbParameters">参数集</param>
        /// <param name="commandType">命令分类</param>
        /// <returns>影响行数</returns>
        int ExecuteNonQuery(string commandText, IDbDataParameter[] dbParameters, CommandType commandType);

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="commandText">sql查询</param>
        /// <returns>Object</returns>
        object ExecuteScalar(string commandText);

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="commandText">sql查询</param>
        /// <param name="dbParameters">参数集</param>
        /// <returns>Object</returns>
        object ExecuteScalar(string commandText, IDbDataParameter[] dbParameters);

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="commandText">sql查询</param>
        /// <param name="dbParameters">参数集</param>
        /// <param name="commandType">命令分类</param>
        /// <returns>Object</returns>
        object ExecuteScalar(string commandText, IDbDataParameter[] dbParameters, CommandType commandType);

        /// <summary>
        /// 填充数据表
        /// </summary>
        /// <param name="commandText">查询</param>
        /// <returns>数据表</returns>
        DataTable Fill(string commandText);

        /// <summary>
        /// 填充数据表
        /// </summary>
        /// <param name="dataTable">目标数据表</param>
        /// <param name="commandText">查询</param>
        /// <returns>数据表</returns>
        DataTable Fill(DataTable dataTable, string commandText);

        /// <summary>
        /// 填充数据表
        /// </summary>
        /// <param name="commandText">sql查询</param>
        /// <param name="dbParameters">参数集</param>
        /// <returns>数据表</returns>
        DataTable Fill(string commandText, IDbDataParameter[] dbParameters);

        /// <summary>
        /// 填充数据表
        /// </summary>
        /// <param name="dataTable">目标数据表</param>
        /// <param name="commandText">sql查询</param>
        /// <param name="dbParameters">参数集</param>
        /// <returns>数据表</returns>
        DataTable Fill(DataTable dataTable, string commandText, IDbDataParameter[] dbParameters);

        /// <summary>
        /// 填充数据表
        /// </summary>
        /// <param name="commandText">sql查询</param>
        /// <param name="dbParameters">参数集</param>
        /// <param name="CommandType">命令分类</param>
        /// <returns>数据表</returns>
        DataTable Fill(string commandText, CommandType commandType, IDbDataParameter[] dbParameters);

        /// <summary>
        /// 填充数据表
        /// </summary>
        /// <param name="dataTable">目标数据表</param>
        /// <param name="commandText">sql查询</param>
        /// <param name="commandType">命令分类</param>
        /// <param name="dbParameters">参数集</param>
        /// <returns>数据表</returns>
        DataTable Fill(DataTable dataTable, string commandText, CommandType commandType, IDbDataParameter[] dbParameters);

        /// <summary>
        /// 填充数据权限
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="dbParameters"></param>
        /// <param name="outDbParameters"></param>
        /// <returns></returns>
        DataTable Fill(DataTable dataTable, string commandText, CommandType commandType, IDbDataParameter[] dbParameters, out DbParameterCollection outDbParameters);

        /// <summary>
        /// 填充数据权限
        /// </summary>
        /// <param name="dataSet">目标数据权限</param>
        /// <param name="commandText">查询</param>
        /// <param name="tableName">填充表</param>
        /// <returns>数据权限</returns>
        DataSet Fill(string tableName,DataSet dataSet, string commandText );

        /// <summary>
        /// 填充数据权限
        /// </summary>
        /// <param name="tableName">填充表</param>
        /// <param name="dataSet">数据权限</param>
        /// <param name="commandText">sql查询</param>
        /// <param name="dbParameters">参数集</param>
        /// <returns>数据权限</returns>
        DataSet Fill(string tableName, DataSet dataSet, string commandText, IDbDataParameter[] dbParameters);

        /// <summary>
        /// 填充数据权限
        /// </summary>
        /// <param name="tableName">填充表</param>
        /// <param name="dataSet">数据权限</param>
        /// <param name="commandText">sql查询</param>
        /// <param name="commandType">命令分类</param>
        /// <param name="dbParameters">参数集</param>
        /// <returns>数据权限</returns>
        DataSet Fill(string tableName, DataSet dataSet, string commandText, CommandType commandType, IDbDataParameter[] dbParameters);

        /// <summary>
        /// 填充数据权限
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dataSet"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="dbParameters"></param>
        /// <param name="outDbParameters"></param>
        /// <returns></returns>
        DataSet Fill(string tableName, DataSet dataSet, string commandText, CommandType commandType, IDbDataParameter[] dbParameters, out DbParameterCollection outDbParameters);

        /// <summary>
        /// 执行数据库查询
        /// </summary>
        /// <param name="procedureName">存储过程</param>
        /// <returns>影响行数</returns>
        int ExecuteProcedure(string procedureName);

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procedureName">存储过程名</param>
        /// <param name="dbParameters">参数集</param>
        /// <returns>影响行数</returns>
        int ExecuteProcedure(string procedureName, IDbDataParameter[] dbParameters);

        /// <summary>
        /// 执行数据库脚本
        /// </summary>
        /// <param name="procedureName">存储过程</param>
        /// <param name="tableName">填充表</param>
        /// <param name="dbParameters">参数集</param>
        /// <returns>数据权限</returns>
        DataTable ExecuteProcedureForDataTable(string procedureName, string tableName, IDbDataParameter[] dbParameters);

        /// <summary>
        /// 执行数据库脚本
        /// </summary>
        /// <param name="procedureName">存储过程</param>
        /// <param name="tableName">填充表</param>
        /// <param name="dbParameters">参数集</param>
        /// <param name="outDbParameters">用于返回输出参数</param>
        /// <returns>数据权限</returns>
        DataTable ExecuteProcedureForDataTable(string procedureName, string tableName, IDbDataParameter[] dbParameters,out DbParameterCollection outDbParameters);

        /// <summary>
        /// 获取输出参数
        /// </summary>
        /// <param name="paramName">参数</param>
        /// <param name="dbType">数据类型</param>
        /// <returns></returns>
        IDbDataParameter MakeOutParam(string paramName, string dbType);

        /// <summary>
        /// 获取输出参数
        /// </summary>
        /// <param name="paramName">参数</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="size">长度</param>
        /// <returns></returns>
        IDbDataParameter MakeOutParam(string paramName, string dbType, int size);

        /// <summary>
        /// 获取输入参数
        /// </summary>
        /// <param name="paramName">参数</param>
        /// <param name="dbType">数据类型</param>
        /// <param name="Size">长度</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        IDbDataParameter MakeInParam(string paramName, string dbType, int Size, object value);

        /// <summary>
        /// 写入sql查询日志
        /// </summary>
        /// <param name="commandText">sql查询</param>
        void WriteLog(string commandText);

        /// <summary>
        /// 内存回收，撤销
        /// </summary>
        new void Dispose();
    }
}