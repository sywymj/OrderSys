using CodeEngine.Framework.QueryBuilder;
using CodeEngine.Framework.QueryBuilder.Enums;
using JSNet.BaseSys;
using JSNet.DbUtilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace JSNet.Manager
{
    public partial class BaseManager
    {
        /// <summary>
        /// 当前操作的数据库表名称
        /// </summary>
        protected string CurrentTableName { get; set; }
        /// <summary>
        /// 当前操作的数据库连接字符串，默认使用 CenterDbConnection
        /// </summary>
        protected string DbConnection = BaseSystemInfo.CenterDbConnectionString;

        /// <summary>
        /// 当前操作的数据库类型，默认使用 CenterDbType
        /// </summary>
        protected DbTypeName DbType = BaseSystemInfo.CenterDbType;

        private static object locker = new Object();

        private IDbHelper _dbHelper = null;

        /// <summary>
        /// 当前的 数据库连接对象
        /// </summary>
        protected IDbHelper DbHelper
        {
            set
            {
                _dbHelper = value;
            }
            get
            {
                if (_dbHelper == null)
                {
                    lock (locker)
                    {
                        if (_dbHelper == null)
                        {
                            _dbHelper = DbHelperFactory.GetHelper(DbConnection, DbType);
                            // 是自动打开关闭数据库状态
                            _dbHelper.AutoOpenClose = true;
                        }
                    }
                }
                return _dbHelper;
            }
        }

        /// <summary>
        /// 默认使用 CenterDbType 数据库类型，CenterDbConnection 链接字符串
        /// </summary>
        public BaseManager()
        {
            if (this._dbHelper == null)
            {
                this._dbHelper = DbHelperFactory.GetHelper(DbConnection, DbType);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbHelper">数据库连接对象</param>
        public BaseManager(IDbHelper dbHelper)
            : this()
        {
            _dbHelper = dbHelper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName">数据库表名称</param>
        public BaseManager(string tableName)
            : this()
        {
            CurrentTableName = tableName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbHelper">数据库连接对象</param>
        /// <param name="tableName">数据表名称</param>
        public BaseManager(IDbHelper dbHelper, string tableName)
            : this(dbHelper)
        {
            CurrentTableName = tableName;
        }

        /// <summary>
        /// 执行存储过程返回数据表
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <returns>数据权限</returns>
        public virtual DataTable GetFromProcedure(string procedureName)
        {
            return this.DbHelper.ExecuteProcedureForDataTable(procedureName, "JsonL", null);
        }

        /// <summary>
        /// 根据条件，获取数据的数量
        /// </summary>
        /// <param name="whereStatement">SQL条件对象</param>
        /// <returns></returns>
        public virtual int GetCount(WhereStatement whereStatement)
        {
            int count = 0;
            whereStatement.DbHelper = this.DbHelper;
            SelectQueryBuilder sqlBuilder = new SelectQueryBuilder(this.DbHelper);
            sqlBuilder.SetSelectCount();
            sqlBuilder.SetFromTable(this.CurrentTableName);
            sqlBuilder.AddWhere(whereStatement);

            DbParameter[] parameters = null;
            string sql = sqlBuilder.BuildSQL(out parameters);

            count = (int)this.DbHelper.ExecuteScalar(sql, parameters);
            return count;

        }

        /// <summary>
        /// 根据条件，获取数据
        /// </summary>
        /// <param name="whereStatement">SQL条件对象</param>
        /// <param name="recordCount">数据数量</param>
        /// <param name="orderByStatement">排序对象</param>
        /// <returns></returns>
        public virtual DataTable GetDataTable(WhereStatement whereStatement, out int recordCount, OrderByStatement orderByStatement = null)
        {
            whereStatement.DbHelper = this.DbHelper;
            SelectQueryBuilder sqlBuilder = new SelectQueryBuilder(this.DbHelper);
            sqlBuilder.SetSelectAllColumns();
            sqlBuilder.SetFromTable(this.CurrentTableName);
            sqlBuilder.AddWhere(whereStatement);

            if (orderByStatement == null)
                orderByStatement = new OrderByStatement("ID", Sorting.Ascending);
            sqlBuilder.AddOrderBy(orderByStatement);

            DbParameter[] parameters = null;
            string sql = sqlBuilder.BuildSQL(out parameters);

            DataTable dataTable = new DataTable(this.CurrentTableName);
            dataTable = this.DbHelper.Fill(sql, parameters);
            recordCount = dataTable.Rows.Count;
            return dataTable;
        }

        /// <summary>
        /// 根据条件，获取数据（分页）
        /// </summary>
        /// <param name="whereStatement">条件对象</param>
        /// <param name="recordCount">数据数量</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="orderByStatement">排序对象</param>
        /// <returns></returns>
        public virtual DataTable GetDataTableByPage(WhereStatement whereStatement, out int recordCount, int pageIndex = 1, int pageSize = 50, OrderByStatement orderByStatement = null)
        {
            whereStatement.DbHelper = this.DbHelper;
            SelectQueryBuilder sqlBuilder = new SelectQueryBuilder(this.DbHelper);
            sqlBuilder.SetSelectAllColumns();
            sqlBuilder.SetFromTable(this.CurrentTableName);
            sqlBuilder.AddWhere(whereStatement);

            DbParameter[] parameters = null;
            string sql = sqlBuilder.BuildSQL(out parameters);
            recordCount = Convert.ToInt32(this.DbHelper.ExecuteScalar(PagingBuilder.CreateCountingSql(sql), parameters));

            if (orderByStatement == null)
                orderByStatement = new OrderByStatement("ID", Sorting.Ascending);

            string orderBySql = orderByStatement.BuildOrderByStatement();
            string pagedSql = PagingBuilder.CreatePagingSql(recordCount, pageSize, pageIndex, sql, orderBySql);

            DataTable dataTable = new DataTable(this.CurrentTableName);
            dataTable = this.DbHelper.Fill(pagedSql, parameters);
            return dataTable;
        }

        public virtual DataRow GetSingle(object idValue, string idField , WhereStatement whereStatement = null)
        {
            if(idValue==null)
            {
                throw new ArgumentException("idValue");
            }
            if(idField == null||idField==string.Empty)
            {
                throw new ArgumentException("idField");
            }

            string returnValue = string.Empty;
            SelectQueryBuilder sqlBuilder = new SelectQueryBuilder(this.DbHelper);
            sqlBuilder.SetSelectAllColumns();
            sqlBuilder.SetFromTable(this.CurrentTableName);

            if (whereStatement == null)
            {
                whereStatement = new WhereStatement(this.DbHelper);
            }
            whereStatement.DbHelper = this.DbHelper;
            whereStatement.Add(idField, Comparison.Equals, idValue);

            sqlBuilder.AddWhere(whereStatement);

            DbParameter[] parameters = null;
            string sql = sqlBuilder.BuildSQL(out parameters);

            DataTable dataTable = new DataTable(this.CurrentTableName);
            dataTable = this.DbHelper.Fill(sql, parameters);

            if (dataTable.Rows.Count > 1)
            {
                throw new Exception("there are more than 1 items");
            }
            else if (dataTable.Rows.Count < 1)
            {
                return null;
            }
            return dataTable.Rows[0];
        }

    }
}
