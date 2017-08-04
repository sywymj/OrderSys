using CodeEngine.Framework.QueryBuilder;
using CodeEngine.Framework.QueryBuilder.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace JSNet.DbUtilities
{
    public class BaseSQLBuilder
    {
        protected IDbHelper _dbHelper = null;
        protected DbOperation _sqlOperation;
        protected WhereStatement _whereStatement = null;// WHERE 从句
        protected List<DbParameter> _parameters = null;
        protected List<string> _fromTables = new List<string>();	// array of string

        private BaseSQLBuilder()
        {
            this._parameters = new List<DbParameter>();
        }

        protected BaseSQLBuilder(IDbHelper dbHelper)
            :this()
        {
            this._dbHelper = dbHelper;
            this._whereStatement = new WhereStatement(dbHelper);
            this._sqlOperation = DbOperation.Select;
        }

        protected BaseSQLBuilder(IDbHelper dbHelper,DbOperation sqlOperation)
            :this(dbHelper)
        {
            _sqlOperation = sqlOperation;
        }

        /// <summary>
        /// Select From 表名
        /// </summary>
        /// <param name="table"></param>
        public void SetFromTable(string table)
        {
            _fromTables.Clear();
            _fromTables.Add(table);
        }
        /// <summary>
        /// Select From 表名数组
        /// </summary>
        /// <param name="tables"></param>
        public void SetFromTables(params string[] tables)
        {
            _fromTables.Clear();
            foreach (string Table in tables)
            {
                _fromTables.Add(Table);
            }
        }
        /// <summary>
        /// FROM表 数组
        /// </summary>
        public string[] FromTables
        {
            get { return _fromTables.ToArray(); }
        }

        internal WhereStatement WhereStatement
        {
            get { return _whereStatement; }
            set { _whereStatement = value; }
        }
        public WhereStatement Where
        {
            get { return _whereStatement; }
            set { _whereStatement = value; }
        }
        /// <summary>
        /// 添加 WHERE 从句，AND
        /// </summary>
        /// <param name="statement"></param>
        public void AddWhere(WhereStatement statement) { _whereStatement = statement; }

        /// <summary>
        /// 添加 WHERE 从句，AND
        /// </summary>
        /// <param name="clause"></param>
        public void AddWhere(WhereClause clause) { AddWhere(clause, 1); }
        /// <summary>
        /// 添加 WHERE 从句，OR
        /// </summary>
        /// <param name="clause"></param>
        /// <param name="level"></param>
        public void AddWhere(WhereClause clause, int level)
        {
            _whereStatement.Add(clause, level);
        }
        /// <summary>
        /// 添加 WHERE 从句
        /// </summary>
        /// <param name="field">字段名</param>
        /// <param name="operator">比较符</param>
        /// <param name="compareValue">字段值</param>
        /// <returns></returns>
        public WhereClause AddWhere(string field, Comparison @operator, object compareValue) { return AddWhere(field, @operator, compareValue, 1); }
        /// <summary>
        /// 添加 WHERE 从句
        /// </summary>
        /// <param name="field">字段名枚举</param>
        /// <param name="operator">比较符</param>
        /// <param name="compareValue">字段值</param>
        /// <returns></returns>
        public WhereClause AddWhere(Enum field, Comparison @operator, object compareValue) { return AddWhere(field.ToString(), @operator, compareValue, 1); }

        /// <summary>
        /// 添加 WHERE 从句 {Level1}((Age<15 OR Age>=20) AND (strEmail LIKE 'e%')) OR {Level2} Age BETWEEN 15 AND 20
        /// </summary>
        /// <param name="field">字段名</param>
        /// <param name="operator">比较符</param>
        /// <param name="compareValue">字段值</param>
        /// <param name="level">OR组别</param>
        /// <returns></returns>
        public WhereClause AddWhere(string field, Comparison @operator, object compareValue, int level)
        {
            WhereClause NewWhereClause = new WhereClause(field, @operator, compareValue);
            _whereStatement.Add(NewWhereClause, level);
            return NewWhereClause;
        }
    }
}
