using System;
using System.Collections.Generic;
using System.Data;

namespace JSNet.Manager
{
    using CodeEngine.Framework.QueryBuilder;
    using CodeEngine.Framework.QueryBuilder.Enums;
    using JSNet.Utilities;
    using JSNet.DbUtilities;
    using JSNet.Model;
    using System.Data.Common;

    public partial class EntityManager<T>
        where T : BaseEntity, new()
    {
        public int Update(KeyValuePair<string, object> targetKVP, object idValue, string idField = null)
        {
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(targetKVP);

            int rows = this.Update(kvps, idValue, idField);
            return rows;
        }

        public int Update(List<KeyValuePair<string, object>> targetKVPs, object idValue, string idField = null)
        {
            WhereStatement whereStatement = new WhereStatement(this.DbHelper);
            if (idField == null)
                whereStatement.Add(this._iEntity.PrimaryKey, Comparison.Equals, idValue);
            else
                whereStatement.Add(idField, Comparison.Equals, idValue);

            int rows = this.Update(targetKVPs, whereStatement);
            return rows;
        }

        public int Update(KeyValuePair<string, object> targetKVP, WhereClause whereClause)
        {
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(targetKVP);
            int rows = this.Update(kvps, whereClause);
            return rows;
        }

        /// <summary>
        /// 更新实体数据，返回受影响的行数
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="this.CurrentTableName">数据表名</param>
        /// <param name="targetFields">更新的目标字段 数组</param>
        /// <param name="targetValues">更新的目标值 数组</param>
        /// <param name="whereClause">WHERE 从句</param>
        /// <returns></returns>
        public int Update(List<KeyValuePair<string, object>> targetKVPs, WhereClause whereClause)
        {
            NonQueryBuilder sqlBuilder = new NonQueryBuilder(this.DbHelper, DbOperation.Update);
            sqlBuilder.SetFromTable(this.CurrentTableName);
            sqlBuilder.AddWhere(whereClause);

            foreach (KeyValuePair<string, object> kv in targetKVPs)
                sqlBuilder.SetValue(kv.Key, kv.Value);

            DbParameter[] parameters = null;
            string sql = sqlBuilder.BuildSQL(out parameters);

            int rows = this.DbHelper.ExecuteNonQuery(sql, parameters);
            return rows;
        }

        public int Update(KeyValuePair<string, object> targetKVP, WhereStatement whereStatement)
        {
            whereStatement.DbHelper = this.DbHelper;
            List<KeyValuePair<string, object>> kvps = new List<KeyValuePair<string, object>>();
            kvps.Add(targetKVP);
            int rows = this.Update(kvps, whereStatement);
            return rows;
        }

        /// <summary>
        /// 更新实体数据，返回受影响的行数
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="this.CurrentTableName">数据表名</param>
        /// <param name="targetFields">更新的目标字段 数组</param>
        /// <param name="targetValues">更新的目标值 数组</param>
        /// <param name="whereClause">WHERE 从句</param>
        /// <returns></returns>
        public int Update(List<KeyValuePair<string, object>> targetKVPs, WhereStatement whereStatement)
        {
            whereStatement.DbHelper = this.DbHelper;
            NonQueryBuilder sqlBuilder = new NonQueryBuilder(this.DbHelper, DbOperation.Update);
            sqlBuilder.SetFromTable(this.CurrentTableName);

            foreach (KeyValuePair<string, object> kv in targetKVPs)
                sqlBuilder.SetValue(kv.Key, kv.Value);

            if (whereStatement.ClauseLevels > 0)
                sqlBuilder.AddWhere(whereStatement);
            else
                throw new Exception("whereClause contains 0 element");

            DbParameter[] parameters = null;
            string sql = sqlBuilder.BuildSQL(out parameters);

            int rows = this.DbHelper.ExecuteNonQuery(sql, parameters);
            return rows;
        }

        public int Update(T entity, object idValue, string idField = null)
        {
            WhereStatement whereStatement = new WhereStatement(this.DbHelper);
            if (idField == null)
                whereStatement.Add(this._iEntity.PrimaryKey, Comparison.Equals, idValue);
            else
                whereStatement.Add(idField, Comparison.Equals, idValue);

            int rows = this.Update(whereStatement, entity);
            return rows;
        }



        public int Update(WhereClause whereClause, T entity)
        {
            WhereStatement whereStatement = new WhereStatement(this.DbHelper);
            whereStatement.Add(whereClause);
            int rows = this.Update(whereStatement, entity);
            return rows;
        }

        /// <summary>
        /// 更新实体数据，返回受影响的行数
        /// </summary>
        /// <param name="dbHelper"></param>
        /// <param name="this.CurrentTableName">数据表名</param>
        /// <param name="whereStatement">WHERE 从句</param>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public int Update(WhereStatement whereStatement, T entity)
        {
            whereStatement.DbHelper = this.DbHelper;
            NonQueryBuilder sqlBuilder = new NonQueryBuilder(this.DbHelper, DbOperation.Update);
            sqlBuilder.SetFromTable(this.CurrentTableName);
            this._iEntity.SetEntity(sqlBuilder, entity);
            sqlBuilder.AddWhere(whereStatement);

            DbParameter[] parameters = null;
            string sql = sqlBuilder.BuildSQL(out parameters);

            int rows = this.DbHelper.ExecuteNonQuery(sql, parameters);
            return rows;
        }

    }
}
