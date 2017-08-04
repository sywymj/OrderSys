using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Collections.Generic;

namespace JSNet.DbUtilities
{
    public abstract class BaseDbHelper : IDisposable
    {
        #region 获取当前数据库类型
        /// <summary>
        /// 获取当前数据库类型
        /// </summary>
        public virtual DbTypeName CurrentDbType
        {
            get
            {
                return DbTypeName.SqlServer;
            }
        }
        #endregion

        #region 数据库连接必要条件参数
        //数据库连接
        private DbConnection _dbConnection = null;
        /// <summary>
        /// 数据库连接
        /// </summary>
        public virtual DbConnection DbConnection
        {
            get
            {
                if (this._dbConnection == null)
                {
                    // 若没打开，就变成自动打开关闭的
                    this.Open();
                    this.AutoOpenClose = true;
                }
                return this._dbConnection;
            }
            set
            {
                this._dbConnection = value;
            }
        }

        //命令
        private DbCommand _dbCommand = null;
        /// <summary>
        /// 命令
        /// </summary>
        public virtual DbCommand DbCommand
        {
            get
            {
                return this._dbCommand;
            }

            set
            {
                this._dbCommand = value;
            }
        }

        //数据库适配器
        private DbDataAdapter _dbDataAdapter = null;
        /// <summary>
        /// 数据库适配器
        /// </summary>
        public virtual DbDataAdapter DbDataAdapter
        {
            get
            {
                return this._dbDataAdapter;
            }

            set
            {
                this._dbDataAdapter = value;
            }
        }

        // 数据库连接，如果这里为空，会根据配置去读链接字符串
        private string _connectionString = string.Empty;
        /// <summary>
        /// 数据库连接
        /// </summary>
        public virtual string ConnectionString
        {
            get
            {
                return this._connectionString;
            }
            set
            {
                this._connectionString = value;
            }
        }

        private DbTransaction _dbTransaction = null;

        // 是否已在事务之中
        private bool _inTransaction = false;
        /// <summary>
        /// 是否已采用事务
        /// </summary>
        public virtual bool InTransaction
        {
            get
            {
                return this._inTransaction;
            }

            set
            {
                this._inTransaction = value;
            }
        }

        public string FileName = "BaseDbHelper.txt";    // sql查询句日志

        //默认打开关闭数据库选项（默认为否）
        private bool _autoOpenClose = false;
        /// <summary>
        /// 默认打开关闭数据库选项（默认为否）
        /// </summary>
        public virtual bool AutoOpenClose
        {
            get
            {
                return _autoOpenClose;
            }
            set
            {
                _autoOpenClose = value;
            }
        }

        //DbProviderFactory实例
        private DbProviderFactory _dbProviderFactory = null;
        /// <summary>
        /// DbProviderFactory实例
        /// </summary>
        public virtual DbProviderFactory GetInstance()
        {
            if (_dbProviderFactory == null)
            {
                _dbProviderFactory = DbHelperFactory.GetHelper(this.ConnectionString).GetInstance();
            }

            return _dbProviderFactory;
        }
        #endregion

        #region 配置选项
        private string _logPath = @"\\Log\\Query";
        public virtual string LogPath
        {
            get { return _logPath; }
            set { _logPath = value; }
        }

        private string _dateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        public virtual string DateTimeFormat
        {
            get { return _dateTimeFormat; }
            set { _dateTimeFormat = value; }
        }

        private bool _logSQL = false;

        public virtual bool LogSQL
        {
            get { return _logSQL; }
            set { _logSQL = value; }
        } 
        #endregion

        #region public virtual string SqlSafe(string value) 检查参数的安全性
        /// <summary>
        /// 检查参数的安全性
        /// </summary>
        /// <param name="value">参数</param>
        /// <returns>安全的参数</returns>
        public virtual string SqlSafe(string value)
        {
            value = value.Replace("'", "''");
            return value;
        }
        #endregion

        #region public virtual string PlusSign() 获得Sql字符串相加符号
        /// <summary>
        ///  获得Sql字符串相加符号
        /// </summary>
        /// <returns>字符加</returns>
        public virtual string PlusSign()
        {
            return " + ";
        }
        #endregion

        #region string PlusSign(params string[] values) 获得Sql字符串相加符号
        /// <summary>
        ///  获得Sql字符串相加符号
        /// </summary>
        /// <param name="values">参数值</param>
        /// <returns>字符加</returns>
        public virtual string PlusSign(params string[] values)
        {
            string returnValue = string.Empty;
            for (int i = 0; i < values.Length; i++)
            {
                returnValue += values[i] + PlusSign();
            }
            if (!String.IsNullOrEmpty(returnValue))
            {
                returnValue = returnValue.Substring(0, returnValue.Length - 3);
            }
            else
            {
                returnValue = PlusSign();
            }
            return returnValue;
        }
        #endregion

        #region public virtual IDbConnection Open() 获取数据库连接的方法
        /// <summary>
        /// 这时主要的获取数据库连接的方法（优先级： CenterDbConnection > BusinessDbConnection）
        /// </summary>
        /// <returns>数据库连接</returns>
        public virtual IDbConnection Open()
        {
            #if (DEBUG)
                int milliStart = Environment.TickCount;
            #endif

            // 这里是获取一个连接的详细方法
            if (String.IsNullOrEmpty(this.ConnectionString))
            {
                throw new Exception("ConnectionString属性为Null");
                // 默认打开业务数据库，而不是用户中心的数据库
                //if (String.IsNullOrEmpty(BaseSystemInfo.CenterDbConnection))
                //{
                //    BaseConfiguration.GetSetting();
                //}
                //if (String.IsNullOrEmpty(BaseSystemInfo.CenterDbConnection))
                //{
                //    this.ConnectionString = BaseSystemInfo.BusinessDbConnection;
                //}
                //else
                //{
                //    this.ConnectionString = BaseSystemInfo.CenterDbConnection;
                //}
            }

            this.Open(this.ConnectionString);

            // 写入调试信息
            #if (DEBUG)
                Trace.WriteLine(DateTime.Now.ToString(_dateTimeFormat) + MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);
            #endif

            return this._dbConnection;
        }
        #endregion

        #region public virtual IDbConnection Open(string connectionString) 获得新的数据库连接
        /// <summary>
        /// 获得新的数据库连接
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <returns>数据库连接</returns>
        public virtual IDbConnection Open(string connectionString)
        {
            // 写入调试信息
#if (DEBUG)
                int milliStart = Environment.TickCount;
                Trace.WriteLine(DateTime.Now.ToString(_dateTimeFormat) + " :Begin: " + 
                MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);
#endif

            // 若是空的话才打开
            if (this._dbConnection == null || this._dbConnection.State == ConnectionState.Closed)
            {
                this.ConnectionString = connectionString;
                this._dbConnection = GetInstance().CreateConnection();
                this._dbConnection.ConnectionString = this.ConnectionString;
                this._dbConnection.Open();

                // 创建对象
                // this.dbCommand = this.DbConnection.CreateCommand();
                // this.dbCommand.Connection = this.dbConnection;
                // this.dbDataAdapter = this.dbProviderFactory.CreateDataAdapter();
                // this.dbDataAdapter.SelectCommand = this.dbCommand;

                // 写入调试信息
#if (DEBUG)
                    int milliEnd = Environment.TickCount;
                    Trace.WriteLine(DateTime.Now.ToString(_dateTimeFormat) + " Ticks: " + 
                    TimeSpan.FromMilliseconds(milliEnd - milliStart).ToString() + " :End: " + 
                    MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);
#endif
            }

            this.AutoOpenClose = false;
            return this._dbConnection;
        }
        #endregion

        #region public virtual IDbConnection GetDbConnection() 获取数据库连接
        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <returns>数据库连接</returns>
        public virtual IDbConnection GetDbConnection()
        {
            return this._dbConnection;
        }
        #endregion

        #region public virtual IDbTransaction GetDbTransaction() 获取数据源上执行的事务
        /// <summary>
        /// 获取数据源上执行的事务
        /// </summary>
        /// <returns>数据源上执行的事务</returns>
        public virtual IDbTransaction GetDbTransaction()
        {
            return this._dbTransaction;
        }
        #endregion

        #region public virtual IDbCommand GetDbCommand() 获取数据源上命令
        /// <summary>
        /// 获取数据源上命令
        /// </summary>
        /// <returns>数据源上命令</returns>
        public virtual IDbCommand GetDbCommand()
        {
            return this.DbConnection.CreateCommand();
        }
        #endregion

        #region public virtual IDataReader ExecuteReader(string commandText) 执行查询
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="commandText">sql查询</param>
        /// <returns>结果集流</returns>
        public virtual IDataReader ExecuteReader(string commandText)
        {
            // 写入调试信息
#if (DEBUG)
            int milliStart = Environment.TickCount;
            Trace.WriteLine(DateTime.Now.ToString(_dateTimeFormat) + " :Begin: " + MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);
#endif

            // 自动打开
            if (this._dbConnection == null)
            {
                this.Open();
                this.AutoOpenClose = true;
            }
            else if (this._dbConnection.State == ConnectionState.Closed)
            {
                this.Open();
            }

            this._dbCommand = this.DbConnection.CreateCommand();
            this._dbCommand.CommandType = CommandType.Text;
            this._dbCommand.CommandText = commandText;

            DbDataReader dbDataReader = null;
            dbDataReader = this._dbCommand.ExecuteReader(CommandBehavior.CloseConnection);

            // 自动打开
            // ADD BY JSON 170424
            if (this.AutoOpenClose)
            {
                this.Close();
            }

            // 写入调试信息
#if (DEBUG)
            int milliEnd = Environment.TickCount;
            Trace.WriteLine(DateTime.Now.ToString(_dateTimeFormat) + " Ticks: " + TimeSpan.FromMilliseconds(milliEnd - milliStart).ToString() + " :End: " + MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);
#endif

            // 写入日志
            this.WriteLog(commandText);

            return dbDataReader;
        }
        #endregion

        #region public virtual IDataReader ExecuteReader(string commandText, IDbDataParameter[] dbParameters); 执行查询
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="commandText">sql查询</param>
        /// <param name="dbParameters">参数集</param>
        /// <returns>结果集流</returns>
        public virtual IDataReader ExecuteReader(string commandText, IDbDataParameter[] dbParameters)
        {
            return this.ExecuteReader(commandText, dbParameters, CommandType.Text);
        }
        #endregion

        #region public virtual IDataReader ExecuteReader(string commandText, IDbDataParameter[] dbParameters, CommandType commandType) 执行查询
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="commandText">sql查询</param>
        /// <param name="dbParameters">参数集</param>
        /// <param name="commandType">命令分类</param>
        /// <returns>结果集流</returns>
        public virtual IDataReader ExecuteReader(string commandText, IDbDataParameter[] dbParameters, CommandType commandType)
        {
            // 写入调试信息
#if (DEBUG)
            int milliStart = Environment.TickCount;
            Trace.WriteLine(DateTime.Now.ToString(_dateTimeFormat) + " :Begin: " + MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);
#endif

            // 自动打开
            if (this._dbConnection == null)
            {
                this.Open();
                this.AutoOpenClose = true;
            }
            else if (this._dbConnection.State == ConnectionState.Closed)
            {
                this.Open();
            }

            this._dbCommand = this.DbConnection.CreateCommand();
            this._dbCommand.CommandText = commandText;
            this._dbCommand.CommandType = commandType;
            if (this._dbTransaction != null)
            {
                this._dbCommand.Transaction = this._dbTransaction;
            }

            if (dbParameters != null)
            {
                this._dbCommand.Parameters.Clear();
                for (int i = 0; i < dbParameters.Length; i++)
                {
                    if (dbParameters[i] != null)
                    {
                        this._dbCommand.Parameters.Add(dbParameters[i]);
                    }
                }
            }

            // 这里要关闭数据库才可以的
            DbDataReader dbDataReader = null;
            dbDataReader = this._dbCommand.ExecuteReader(CommandBehavior.CloseConnection);
            this._dbCommand.Parameters.Clear();

            // 自动关闭
            // ADD BY JSON 170424
            if (this.AutoOpenClose)
            {
                this.Close();
            }

            // 写入调试信息
#if (DEBUG)
            int milliEnd = Environment.TickCount;
            Trace.WriteLine(DateTime.Now.ToString(_dateTimeFormat) + " Ticks: " + TimeSpan.FromMilliseconds(milliEnd - milliStart).ToString() + " :End: " + MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);
#endif

            // 写入日志
            this.WriteLog(commandText);

            return dbDataReader;
        }
        #endregion

        #region public virtual int ExecuteNonQuery(string commandText) 执行查询, SQL BUILDER 用了这个东西？参数需要保存, 不能丢失.
        /// <summary>
        /// 执行查询, SQL BUILDER 用了这个东西？参数需要保存, 不能丢失.
        /// </summary>
        /// <param name="commandText">sql查询</param>
        /// <returns>影响行数</returns>
        public virtual int ExecuteNonQuery(string commandText)
        {
            //>>>EDIT BY JSON 170424
            return this.ExecuteNonQuery(commandText, null);
            //<<<
        }
        #endregion

        #region public virtual int ExecuteNonQuery(string commandText, IDbDataParameter[] dbParameters) 执行查询
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="commandText">sql查询</param>
        /// <param name="dbParameters">参数集</param>
        /// <returns>影响行数</returns>
        public virtual int ExecuteNonQuery(string commandText, IDbDataParameter[] dbParameters)
        {
            return this.ExecuteNonQuery(commandText, dbParameters, CommandType.Text);
        }
        #endregion

        #region public virtual int ExecuteNonQuery(string commandText, CommandType commandType) 执行查询
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="commandText">sql查询</param>
        /// <param name="commandType">命令分类</param>
        /// <returns>影响行数</returns>
        public virtual int ExecuteNonQuery(string commandText, CommandType commandType)
        {
            return this.ExecuteNonQuery(commandText, null, commandType);
        }
        #endregion

        #region public virtual int ExecuteNonQuery(string commandText, IDbDataParameter[] dbParameters, CommandType commandType) 执行查询
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="transaction">数据库事务</param>
        /// <param name="commandText">sql查询</param>
        /// <param name="dbParameters">参数集</param>
        /// <param name="commandType">命令分类</param>
        /// <returns>影响行数</returns>
        public virtual int ExecuteNonQuery(string commandText, IDbDataParameter[] dbParameters, CommandType commandType)
        {
            // 写入调试信息
#if (DEBUG)
            int milliStart = Environment.TickCount;
            Trace.WriteLine(DateTime.Now.ToString(_dateTimeFormat) + " :Begin: " + MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);
#endif

            // 自动打开
            if (this._dbConnection == null)
            {
                this.Open();
                this.AutoOpenClose = true;
            }
            else if (this._dbConnection.State == ConnectionState.Closed)
            {
                this.Open();
            }

            this._dbCommand = this.DbConnection.CreateCommand();
            this._dbCommand.CommandText = commandText;
            this._dbCommand.CommandType = commandType;
            if (this._dbTransaction != null)
            {
                this._dbCommand.Transaction = this._dbTransaction;
            }
            if (dbParameters != null)
            {
                this._dbCommand.Parameters.Clear();
                for (int i = 0; i < dbParameters.Length; i++)
                {
                    // if (dbParameters[i] != null)
                    //{
                    this._dbCommand.Parameters.Add(dbParameters[i]);
                    //}
                }
            }
            int returnValue = this._dbCommand.ExecuteNonQuery();
            this._dbCommand.Parameters.Clear();

            // 自动关闭
            if (this.AutoOpenClose)
            {
                this.Close();
            }

            // 写入调试信息
#if (DEBUG)
            int milliEnd = Environment.TickCount;
            Trace.WriteLine(DateTime.Now.ToString(_dateTimeFormat) + " Ticks: " + TimeSpan.FromMilliseconds(milliEnd - milliStart).ToString() + " :End: " + MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);
#endif

            // 写入日志
            this.WriteLog(commandText);

            return returnValue;
        }
        #endregion

        #region public virtual object ExecuteScalar(string commandText) 执行查询
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="commandText">sql查询</param>
        /// <returns>object</returns>
        public virtual object ExecuteScalar(string commandText)
        {
            return this.ExecuteScalar(commandText, null, CommandType.Text);
        }
        #endregion

        #region public virtual object ExecuteScalar(string commandText, IDbDataParameter[] dbParameters) 执行查询
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="commandText">sql查询</param>
        /// <param name="dbParameters">参数集</param>
        /// <returns>Object</returns>
        public virtual object ExecuteScalar(string commandText, IDbDataParameter[] dbParameters)
        {
            return this.ExecuteScalar(commandText, dbParameters, CommandType.Text);
        }
        #endregion

        #region public virtual object ExecuteScalar(string commandText, IDbDataParameter[] dbParameters, CommandType commandType) 执行查询
        /// <summary>
        /// 执行查询
        /// </summary>
        /// <param name="transaction">数据库事务</param>
        /// <param name="commandText">sql查询</param>
        /// <param name="dbParameters">参数集</param>
        /// <param name="commandType">命令分类</param>
        /// <returns>Object</returns>
        public virtual object ExecuteScalar(string commandText, IDbDataParameter[] dbParameters, CommandType commandType)
        {
            // 写入调试信息
#if (DEBUG)
            int milliStart = Environment.TickCount;
            Trace.WriteLine(DateTime.Now.ToString(_dateTimeFormat) + " :Begin: " + MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);
#endif

            // 自动打开
            if (this._dbConnection == null)
            {
                this.Open();
                this.AutoOpenClose = true;
            }
            else if (this._dbConnection.State == ConnectionState.Closed)
            {
                this.Open();
            }

            this._dbCommand = this.DbConnection.CreateCommand();
            this._dbCommand.CommandText = commandText;
            this._dbCommand.CommandType = commandType;
            if (this._dbTransaction != null)
            {
                this._dbCommand.Transaction = this._dbTransaction;
            }
            if (dbParameters != null)
            {
                this._dbCommand.Parameters.Clear();
                for (int i = 0; i < dbParameters.Length; i++)
                {
                    if (dbParameters[i] != null)
                    {
                        this._dbCommand.Parameters.Add(dbParameters[i]);
                    }
                }
            }
            object returnValue = this._dbCommand.ExecuteScalar();
            this._dbCommand.Parameters.Clear();

            // 自动关闭
            if (this.AutoOpenClose)
            {
                this.Close();
            }

            // 写入调试信息
#if (DEBUG)
            int milliEnd = Environment.TickCount;
            Trace.WriteLine(DateTime.Now.ToString(_dateTimeFormat) + " Ticks: " + TimeSpan.FromMilliseconds(milliEnd - milliStart).ToString() + " :End: " + MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);
#endif

            // 写入日志
            this.WriteLog(commandText);
            return returnValue;
        }
        #endregion

        #region public virtual DataTable Fill(string commandText) 填充数据表
        /// <summary>
        /// 填充数据表
        /// </summary>
        /// <param name="commandText">查询</param>
        /// <returns>数据表</returns>
        public virtual DataTable Fill(string commandText)
        {
            DataTable dataTable = new DataTable("DotNet");
            return this.Fill(dataTable, commandText, null, CommandType.Text);
        }
        #endregion

        #region public virtual DataTable Fill(DataTable dataTable, string commandText) 填充数据表
        /// <summary>
        /// 填充数据表
        /// </summary>
        /// <param name="dataTable">目标数据表</param>
        /// <param name="commandText">查询</param>
        /// <returns>数据表</returns>
        public virtual DataTable Fill(DataTable dataTable, string commandText)
        {
            return this.Fill(dataTable, commandText, null, CommandType.Text);
        }
        #endregion

        #region public virtual DataTable Fill(string commandText, IDbDataParameter[] dbParameters) 填充数据表
        /// <summary>
        /// 填充数据表
        /// </summary>
        /// <param name="commandText">sql查询</param>
        /// <param name="dbParameters">参数集</param>
        /// <returns>数据表</returns>
        public virtual DataTable Fill(string commandText, IDbDataParameter[] dbParameters)
        {
            DataTable dataTable = new DataTable("DotNet");
            return this.Fill(dataTable, commandText, dbParameters, CommandType.Text);
        }
        #endregion

        #region public virtual DataTable Fill(DataTable dataTable, string commandText, IDbDataParameter[] dbParameters) 填充数据表
        /// <summary>
        /// 填充数据表
        /// </summary>
        /// <param name="dataTable">目标数据表</param>
        /// <param name="commandText">sql查询</param>
        /// <param name="dbParameters">参数集</param>
        /// <returns>数据表</returns>
        public virtual DataTable Fill(DataTable dataTable, string commandText, IDbDataParameter[] dbParameters)
        {
            return this.Fill(dataTable, commandText, dbParameters, CommandType.Text);
        }
        #endregion

        #region public virtual DataTable Fill(string commandText, IDbDataParameter[] dbParameters, CommandType commandType) 填充数据表
        /// <summary>
        /// 填充数据表
        /// </summary>
        /// <param name="commandText">sql查询</param>
        /// <param name="commandType">命令分类</param>
        /// <param name="dbParameters">参数集</param>
        /// <returns>数据表</returns>
        public virtual DataTable Fill(string commandText, IDbDataParameter[] dbParameters, CommandType commandType)
        {
            DataTable dataTable = new DataTable("DotNet");
            return this.Fill(dataTable, commandText, dbParameters, commandType);
        }
        #endregion

        #region public virtual DataTable Fill(DataTable dataTable, string commandText, IDbDataParameter[] dbParameters, CommandType commandType) 填充数据表
        /// <summary>
        /// 填充数据表
        /// </summary>
        /// <param name="dataTable">目标数据表</param>
        /// <param name="commandText">sql查询</param>
        /// <param name="dbParameters">参数集</param>
        /// <param name="commandType">命令分类</param>
        /// <returns>数据表</returns>
        public virtual DataTable Fill(DataTable dataTable, string commandText, IDbDataParameter[] dbParameters, CommandType commandType)
        {
            //>>>EDIT BY JSON 170424
            DataSet ds = new DataSet();
            dataTable.TableName = string.IsNullOrEmpty(dataTable.TableName) ? "DotNet" : dataTable.TableName;
            ds = this.Fill(ds, commandType, commandText, dataTable.TableName, dbParameters);
            if (ds.Tables.Count > 0)
            {
                dataTable= ds.Tables[0];
            }
            return dataTable;
            //<<<
        }
        #endregion

        #region public virtual DataSet Fill(DataSet dataSet, string commandText, string tableName) 填充数据权限
        /// <summary>
        /// 填充数据权限
        /// </summary>
        /// <param name="dataSet">目标数据权限</param>
        /// <param name="commandText">查询</param>
        /// <param name="tableName">填充表</param>
        /// <returns>数据权限</returns>
        public virtual DataSet Fill(DataSet dataSet, string commandText, string tableName)
        {
            return this.Fill(dataSet, CommandType.Text, commandText, tableName, null);
        }
        #endregion

        #region public virtual DataSet Fill(DataSet dataSet, string commandText, string tableName, IDbDataParameter[] dbParameters) 填充数据权限
        /// <summary>
        /// 填充数据权限
        /// </summary>
        /// <param name="dataSet">数据权限</param>
        /// <param name="commandText">sql查询</param>
        /// <param name="tableName">填充表</param>
        /// <param name="dbParameters">参数集</param>
        /// <returns>数据权限</returns>
        public virtual DataSet Fill(DataSet dataSet, string commandText, string tableName, IDbDataParameter[] dbParameters)
        {
            return this.Fill(dataSet, CommandType.Text, commandText, tableName, dbParameters);
        }
        #endregion

        #region public virtual DataSet Fill(DataSet dataSet, CommandType commandType, string commandText, string tableName, IDbDataParameter[] dbParameters) 填充数据权限
        /// <summary>
        /// 填充数据权限
        /// </summary>
        /// <param name="dataSet">数据权限</param>
        /// <param name="commandType">命令分类</param>
        /// <param name="commandText">sql查询</param>
        /// <param name="tableName">填充表</param>
        /// <param name="dbParameters">参数集</param>
        /// <returns>数据权限</returns>
        public virtual DataSet Fill(DataSet dataSet, CommandType commandType, string commandText, string tableName, IDbDataParameter[] dbParameters)
        {
            // 写入调试信息
#if (DEBUG)
            int milliStart = Environment.TickCount;
            Trace.WriteLine(DateTime.Now.ToString(_dateTimeFormat) + " :Begin: " + MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);
#endif

            // 自动打开
            if (this._dbConnection == null)
            {
                this.Open();
                this.AutoOpenClose = true;
            }
            else if (this._dbConnection.State == ConnectionState.Closed)
            {
                this.Open();
            }

            using (this._dbCommand = this.DbConnection.CreateCommand())
            {
                //this.dbCommand.Parameters.Clear();
                //if ((dbParameters != null) && (dbParameters.Length > 0))
                //{
                //    for (int i = 0; i < dbParameters.Length; i++)
                //    {
                //        if (dbParameters[i] != null)
                //        {
                //            this.dbDataAdapter.SelectCommand.Parameters.Add(dbParameters[i]);
                //        }
                //    }
                //}
                this._dbCommand.CommandText = commandText;
                this._dbCommand.CommandType = commandType;
                if (this.InTransaction)
                {
                    // 这个不这么写，也不行，否则运行不能通过的
                    this._dbCommand.Transaction = this._dbTransaction;
                }
                if ((dbParameters != null) && (dbParameters.Length > 0))
                {
                    this._dbCommand.Parameters.AddRange(dbParameters);
                }

                this._dbDataAdapter = this.GetInstance().CreateDataAdapter();
                this._dbDataAdapter.SelectCommand = this._dbCommand;
                this._dbDataAdapter.Fill(dataSet, tableName);
                this._dbDataAdapter.SelectCommand.Parameters.Clear();

                if (this.AutoOpenClose)
                {
                    this.Close();
                }
            }

            // 写入调试信息
#if (DEBUG)
            int milliEnd = Environment.TickCount;
            Trace.WriteLine(DateTime.Now.ToString(_dateTimeFormat) + " Ticks: " + TimeSpan.FromMilliseconds(milliEnd - milliStart).ToString() + " :End: " + MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);
#endif

            // 写入日志
            this.WriteLog(commandText);
            return dataSet;
        }
        #endregion

        #region public virtual int ExecuteProcedure(string procedureName) 执行存储过程
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procedureName">存储过程</param>
        /// <returns>int</returns>
        public virtual int ExecuteProcedure(string procedureName)
        {
            return this.ExecuteNonQuery(procedureName, null, CommandType.StoredProcedure);
        }
        #endregion

        #region public virtual int ExecuteProcedure(string procedureName, IDbDataParameter[] dbParameters) 执行代参数的存储过程
        /// <summary>
        /// 执行代参数的存储过程
        /// </summary>
        /// <param name="procedureName">存储过程名</param>
        /// <param name="dbParameters">参数集</param>
        /// <returns>影响行数</returns>
        public virtual int ExecuteProcedure(string procedureName, IDbDataParameter[] dbParameters)
        {
            return this.ExecuteNonQuery(procedureName, dbParameters, CommandType.StoredProcedure);
        }
        #endregion

        #region public virtual DataTable ExecuteProcedureForDataTable(string procedureName, string tableName, IDbDataParameter[] dbParameters) 执行存储过程返回数据表
        /// <summary>
        /// 执行存储过程返回数据表
        /// </summary>
        /// <param name="procedureName">存储过程</param>
        /// <param name="tableName">填充表</param>
        /// <param name="dbParameters">参数集</param>
        /// <returns>数据权限</returns>
        public virtual DataTable ExecuteProcedureForDataTable(string procedureName, string tableName, IDbDataParameter[] dbParameters)
        {
            DataTable dataTable = new DataTable(tableName);
            this.Fill(dataTable, procedureName, dbParameters, CommandType.StoredProcedure);
            return dataTable;
        }
        #endregion

        #region public IDbTransaction BeginTransaction() 事务开始
        /// <summary>
        /// 事务开始
        /// </summary>
        /// <returns>事务</returns>
        public IDbTransaction BeginTransaction()
        {
            // 写入调试信息
#if (DEBUG)
            int milliStart = Environment.TickCount;
            Trace.WriteLine(DateTime.Now.ToString(_dateTimeFormat) + " :Begin: " + MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);
#endif

            if (!this.InTransaction)
            {
                this.InTransaction = true;
                this._dbTransaction = this.DbConnection.BeginTransaction();
                // this.dbCommand.Transaction = this.dbTransaction;
            }

            // 写入调试信息
#if (DEBUG)
            int milliEnd = Environment.TickCount;
            Trace.WriteLine(DateTime.Now.ToString(_dateTimeFormat) + " Ticks: " + TimeSpan.FromMilliseconds(milliEnd - milliStart).ToString() + " :End: " + MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);
#endif

            return this._dbTransaction;
        }
        #endregion

        #region public void CommitTransaction() 提交事务
        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTransaction()
        {
            // 写入调试信息
#if (DEBUG)
            int milliStart = Environment.TickCount;
            Trace.WriteLine(DateTime.Now.ToString(_dateTimeFormat) + " :Begin: " + MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);
#endif

            if (this.InTransaction)
            {
                // 事务已经完成了，一定要更新标志信息
                this.InTransaction = false;
                this._dbTransaction.Commit();
            }

            // 写入调试信息
#if (DEBUG)
            int milliEnd = Environment.TickCount;
            Trace.WriteLine(DateTime.Now.ToString(_dateTimeFormat) + " Ticks: " + TimeSpan.FromMilliseconds(milliEnd - milliStart).ToString() + " :End: " + MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);
#endif
        }
        #endregion

        #region public void RollbackTransaction() 回滚事务
        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollbackTransaction()
        {
            // 写入调试信息
#if (DEBUG)
            int milliStart = Environment.TickCount;
            Trace.WriteLine(DateTime.Now.ToString(_dateTimeFormat) + " :Begin: " + MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);
#endif

            if (this.InTransaction)
            {
                this.InTransaction = false;
                this._dbTransaction.Rollback();
            }

            // 写入调试信息
#if (DEBUG)
            int milliEnd = Environment.TickCount;
            Trace.WriteLine(DateTime.Now.ToString(_dateTimeFormat) + " Ticks: " + TimeSpan.FromMilliseconds(milliEnd - milliStart).ToString() + " :End: " + MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);
#endif
        }
        #endregion

        #region public void Close() 关闭数据库连接
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void Close()
        {
            // 写入调试信息
#if (DEBUG)
            int milliStart = Environment.TickCount;
            Trace.WriteLine(DateTime.Now.ToString(_dateTimeFormat) + " :Begin: " + MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);
#endif

            if (this._dbConnection != null)
            {
                this._dbConnection.Close();
                this._dbConnection.Dispose();
            }

            this.Dispose();

            // 写入调试信息
#if (DEBUG)
            int milliEnd = Environment.TickCount;
            Trace.WriteLine(DateTime.Now.ToString(_dateTimeFormat) + " Ticks: " + TimeSpan.FromMilliseconds(milliEnd - milliStart).ToString() + " :End: " + MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);
#endif
        }
        #endregion

        #region public virtual void WriteLog(string commandText, string fileName = null) 写入sql查询句日志

        /// <summary>
        /// 写入sql查询句日志
        /// </summary>
        /// <param name="commandText"></param>
        public virtual void WriteLog(string commandText)
        {
            string fileName = DateTime.Now.ToString(_dateTimeFormat) + " _ " + this.FileName;
            WriteLog(commandText, fileName);
        }

        /// <summary>
        /// 写入sql查询句日志
        /// </summary>
        /// <param name="commandText">异常</param>
        /// <param name="fileName">文件名</param>
        public virtual void WriteLog(string commandText, string fileName = null)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = DateTime.Now.ToString(_dateTimeFormat) + " _ " + this.FileName;
            }
            string returnValue = string.Empty;
            // 系统里应该可以配置是否记录异常现象
            if (!_logSQL)
            {
                return;
            }
            // 将异常信息写入本地文件中
            string logDirectory = _logPath;
            if (!System.IO.Directory.Exists(logDirectory))
            {
                System.IO.Directory.CreateDirectory(logDirectory);
            }
            string writerFileName = logDirectory + "\\" + fileName;
            if (!File.Exists(writerFileName))
            {
                FileStream FileStream = new FileStream(writerFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
                FileStream.Close();
            }
            StreamWriter streamWriter = new StreamWriter(writerFileName, true, Encoding.Default);
            streamWriter.WriteLine(DateTime.Now.ToString(_dateTimeFormat) + " " + commandText);
            streamWriter.Close();
        }
        #endregion

        #region public void Dispose() 内存回收
        /// <summary>
        /// 内存回收
        /// </summary>
        public void Dispose()
        {
            string a = this._dbConnection.ConnectionString;
            if (this._dbCommand != null)
            {
                this._dbCommand.Dispose();
            }
            if (this._dbDataAdapter != null)
            {
                this._dbDataAdapter.Dispose();
            }
            if (this._dbTransaction != null)
            {
                this._dbTransaction.Dispose();
            }
            // 关闭数据库连接
            if (this._dbConnection != null)
            {
                if (this._dbConnection.State != ConnectionState.Closed)
                {
                    this._dbConnection.Close();
                    this._dbConnection.Dispose();
                }
            }
            this._dbConnection = null;

        }
        #endregion
    }
}