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

        public int Delete(object idValue, string idField = null)
        {
            if (idField == null)
                idField = this._iEntity.PrimaryKey;
            WhereClause clause = new WhereClause(idField, Comparison.Equals, idValue);
            int rows = this.Delete(clause);
            return rows;
        }

        public int Delete(WhereClause whereClause)
        {
            WhereStatement statement = new WhereStatement(this.DbHelper);
            statement.Add(whereClause);
            int rows = this.Delete(statement);
            return rows;
        }

        public int Delete(WhereStatement whereStatement)
        {
            whereStatement.DbHelper = this.DbHelper;
            NonQueryBuilder sqlBuilder = new NonQueryBuilder(this.DbHelper, DbOperation.Delete);
            sqlBuilder.SetFromTable(this.CurrentTableName);
            sqlBuilder.AddWhere(whereStatement);

            DbParameter[] parameters = null;
            string sql = sqlBuilder.BuildSQL(out parameters);

            int rows = this.DbHelper.ExecuteNonQuery(sql, parameters);
            return rows;
        }

        public int SetDeleted(object idValue, string idField = null)
        {
            WhereClause whereClause;
            if (idField == null)
                whereClause = new WhereClause(this._iEntity.PrimaryKey, Comparison.Equals, idValue);
            else
                whereClause = new WhereClause(idField, Comparison.Equals, idValue);

            int rows = this.SetDeleted(whereClause);
            return rows;
        }

        public int SetDeleted(WhereClause whereClause)
        {
            WhereStatement statement = new WhereStatement(this.DbHelper);
            statement.Add(whereClause);
            int rows = this.SetDeleted(statement);
            return rows;
        }

        /// <summary>
        /// 假删除
        /// </summary>
        /// <returns></returns>
        public int SetDeleted(WhereStatement whereStatement)
        {
            if (this._iEntity.DeletionStateCode == null) { throw new ArgumentNullException("DeletionStateCode"); }

            KeyValuePair<string, object> kvp = new KeyValuePair<string, object>(this._iEntity.DeletionStateCode, 1);
            int rows = this.Update(kvp, whereStatement);
            return rows;
        }
    }
}