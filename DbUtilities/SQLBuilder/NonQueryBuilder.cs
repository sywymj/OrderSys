using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;

namespace JSNet.DbUtilities
{
    using CodeEngine.Framework.QueryBuilder;
    using CodeEngine.Framework.QueryBuilder.Enums;
    using System.Data.Common;

    public partial class NonQueryBuilder : BaseSQLBuilder, ISQLBuilder
    {
        private string _commandText = string.Empty;
        private string _tableName = string.Empty;
        private string _insertValue = string.Empty;
        private string _insertField = string.Empty;
        private string _updateSql = string.Empty;
        private string _selectSql = string.Empty;
        private string _whereSql = string.Empty;

        /// <summary>
        /// 默认主键列名为Id，采用自增量，新增时主键字段不会赋值
        /// </summary>
        /// <param name="dbHelper"></param>
        public NonQueryBuilder(IDbHelper dbHelper, DbOperation sqlOperation)
            : base(dbHelper,sqlOperation)
        {
            PrepareCommand();
        }


        #region public void SetFormula(string targetFiled, string formula, string relation) 设置公式
        /// <summary>
        /// 设置公式
        /// </summary>
        /// <param name="targetFiled">目标字段</param>
        /// <param name="targetFiled">目标字段</param>
        public void SetFormula(string targetFiled, string formula)
        {
            string relation = " = ";
            this.SetFormula(targetFiled, formula, relation);
        }
        /// <summary>
        /// 设置公式
        /// </summary>
        /// <param name="targetFiled">目标字段</param>
        /// <param name="targetFiled">目标字段</param>
        public void SetFormula(string targetFiled, string formula, string relation)
        {
            if (_sqlOperation == DbOperation.Insert)
            {
                _insertField += targetFiled + ", ";
                _insertValue += formula + ", ";
            }
            if (_sqlOperation == DbOperation.Update)
            {
                _updateSql += targetFiled + relation + formula + ", ";
            }
        }
        #endregion

        #region public void SetDBNow(string targetFiled) 设置为当前时间
        /// <summary>
        /// 设置为当前时间
        /// </summary>
        /// <param name="targetFiled">目标字段</param>
        public void SetDBNow(string targetFiled)
        {
            if (_sqlOperation == DbOperation.Insert)
            {
                _insertField += targetFiled + ", ";
                _insertValue += _dbHelper.GetDBNow() + ", ";
            }
            if (_sqlOperation == DbOperation.Update)
            {
                _updateSql += targetFiled + " = " + _dbHelper.GetDBNow() + ", ";
            }
        }
        #endregion

        #region public void SetNull(string targetFiled) 设置为Null值
        /// <summary>
        /// 设置为Null值
        /// </summary>
        /// <param name="targetFiled">目标字段</param>
        public void SetNull(string targetFiled)
        {
            this.SetValue(targetFiled, null);
        }
        #endregion

        #region public void SetValue(string targetFiled, object targetValue,bool ignore = false,string targetFiledName = null) 设置值

        /// <summary>
        /// 构造 insertField 或 updateField 部分
        /// </summary>
        /// <param name="targetFiled">字段名称</param>
        /// <param name="targetValue">字段值</param>
        /// <param name="ignore">增加时是否忽略字段（一般是数据库有默认值的字段或自增主键）</param>
        /// <param name="targetFiledName">Parameter参数的名称</param>
        public void SetValue(string targetFiled, object targetValue, bool ignore = false, string targetFiledName = null)
        {
            if (ignore)
            {
                return;
            }
            if (targetFiledName == null)
            {
                targetFiledName = targetFiled;
            }
            switch (this._sqlOperation)
            {
                case DbOperation.Update:
                    if (targetValue == null)
                    {
                        this._updateSql += targetFiled + " = Null, ";
                    }
                    else
                    {
                        if (targetValue.ToString().Length > 0)
                        {
                            // 判断数据库连接类型
                            this._updateSql += targetFiled + " = " + _dbHelper.GetParameter(targetFiledName) + ", ";
                            this.AddParameter(targetFiledName, targetValue);
                        }
                        else
                        {
                            this._updateSql += targetFiled + " = '', ";
                        }
                    }
                    break;
                case DbOperation.Insert:
                    if (_dbHelper.CurrentDbType == DbTypeName.SqlServer)
                    {
                        this._insertField += targetFiled + ", ";
                    }
                    else if (_dbHelper.CurrentDbType == DbTypeName.Access)
                    {
                        this._insertField += targetFiled + ", ";
                    }
                    else
                    {
                        this._insertField += targetFiled + ", ";
                    }

                    if (targetValue == null)
                    {
                        if (_dbHelper.CurrentDbType == DbTypeName.SqlServer)
                        {
                            this._insertValue += " Null, ";
                        }
                        else if (_dbHelper.CurrentDbType == DbTypeName.Access)
                        {
                            this._insertValue += " Null, ";
                        }
                        else
                        {
                            this._insertValue += " Null, ";
                        }
                    }
                    else
                    {
                        this._insertValue += _dbHelper.GetParameter(targetFiledName) + ", ";
                        this.AddParameter(targetFiledName, targetValue);

                    }
                    break;
            }
        }
        #endregion

        /// <summary>
        /// 构造 SQL语句
        /// </summary>
        /// <returns>SQL语句</returns>
        public string BuildSQL()
        {
            string sql = string.Empty;
            List<DbParameter> parameters = new List<DbParameter>();
            switch (_sqlOperation)
            {
                case DbOperation.Insert:
                    sql = (string)BuildInsert(false, out parameters);
                    break;
                case DbOperation.Update:
                    sql = (string)BuildUpdate(false, out parameters);
                    break;
                case DbOperation.Delete:
                    sql = (string)BuildDelete(false, out parameters);
                    break;
            }
            return sql;
        }

        /// <summary>
        /// 构造 带参数化 的SQL语句
        /// </summary>
        /// <param name="dbParameters">out 参数对象数组</param>
        /// <returns>SQL语句</returns>
        public string BuildSQL(out DbParameter[] dbParameters)
        {
            string sql = string.Empty;
            List<DbParameter> parameters = new List<DbParameter>();
            switch(_sqlOperation)
            {
                case DbOperation.Insert:
                    sql = this.BuildInsert(true, out parameters);
                    break;
                case DbOperation.Update:
                    sql = this.BuildUpdate(true, out parameters);
                    break;
                case DbOperation.Delete:
                    sql = this.BuildDelete(true, out parameters);
                    break;
            }
            dbParameters = parameters.ToArray();
            return sql;
        }


        #region private void AddParameter(string targetFiled, object targetValue) 添加参数
        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="targetFiled">目标字段</param>
        /// <param name="targetValue">值</param>
        private void AddParameter(string targetFiled, object targetValue)
        {
            IDbDataParameter parameter = _dbHelper.MakeParameter(targetFiled, targetValue);
            this._parameters.Add((DbParameter)parameter);
        }
        #endregion

        /// <summary>
        /// 构造 新增语句
        /// </summary>
        /// <returns>Returns a string containing the query, or a DbCommand containing a command with parameters</returns>
        private string BuildInsert(bool buildCommand, out List<DbParameter> dbParameters)
        {
            this._tableName = _fromTables[0];
            this._insertField = this._insertField.Substring(0, _insertField.Length - 2);
            this._insertValue = this._insertValue.Substring(0, _insertValue.Length - 2);
            this._commandText = " INSERT INTO " + this._tableName + " (" + _insertField + ") VALUES (" + _insertValue + ") ";

            dbParameters = this._parameters;
            return this._commandText;
        }
        /// <summary>
        /// 构造 更新语句
        /// </summary>
        /// <returns>Returns a string containing the update, or a DbCommand containing a command with parameters</returns>
        private string BuildUpdate(bool buildCommand, out List<DbParameter> dbParameters)
        {
            List<DbParameter> parameters = new List<DbParameter>();

            this._tableName = _fromTables[0];
            this._updateSql = this._updateSql.Substring(0, _updateSql.Length - 2);
            if (_whereStatement.ClauseLevels > 0)
            {
                this._whereSql = " WHERE " + _whereStatement.BuildWhereStatement(buildCommand, out parameters);
            }
            this._commandText = " UPDATE " + this._tableName + " SET " + this._updateSql + this._whereSql;
            this._parameters.AddRange(parameters);

            dbParameters = this._parameters;
            return this._commandText;
        }

        /// <summary>
        /// 构造 删除语句
        /// </summary>
        /// <returns>Returns a string containing the delete, or a DbCommand containing a command with parameters</returns>
        private string BuildDelete(bool buildCommand, out List<DbParameter> dbParameters)
        {
            List<DbParameter> parameters = new List<DbParameter>();

            this._tableName = _fromTables[0];
            if (_whereStatement.ClauseLevels > 0)
            {
                this._whereSql = " WHERE " + _whereStatement.BuildWhereStatement(buildCommand, out parameters);
            }
            this._commandText = " DELETE FROM " + this._tableName + this._whereSql;
            this._parameters.AddRange(parameters);

            dbParameters = this._parameters;
            return this._commandText;
        }

        #region private void PrepareCommand() 
        /// <summary>
        /// 获得数据库连接
        /// </summary>
        private void PrepareCommand()
        {
            // 写入调试信息
#if (DEBUG)
            int milliStart = Environment.TickCount;
            Trace.WriteLine(DateTime.Now.ToString(base._dbHelper.DateTimeFormat) + " :Begin: " + MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);
#endif

            this._commandText = string.Empty;
            this._tableName = string.Empty;
            this._insertValue = string.Empty;
            this._insertField = string.Empty;
            this._updateSql = string.Empty;
            this._whereSql = string.Empty;
            this._parameters.Clear();

            // 写入调试信息
#if (DEBUG)
            int milliEnd = Environment.TickCount;
            Trace.WriteLine(DateTime.Now.ToString(base._dbHelper.DateTimeFormat) + " Ticks: " + TimeSpan.FromMilliseconds(milliEnd - milliStart).ToString() + " :End: " + MethodBase.GetCurrentMethod().ReflectedType.Name + "." + MethodBase.GetCurrentMethod().Name);
#endif
        }
        #endregion

    }
}