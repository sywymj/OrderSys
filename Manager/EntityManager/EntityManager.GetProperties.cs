using System;
using System.Collections.Generic;
using System.Data;

namespace JSNet.Manager
{
    using CodeEngine.Framework.QueryBuilder;
    using CodeEngine.Framework.QueryBuilder.Enums;
    using JSNet.Utilities;
    using JSNet.Model;
    using System.Data.Common;

    public partial class EntityManager<T>
        where T : BaseEntity, new()
    {

        public virtual string[] GetIds(WhereStatement whereStatement, OrderByStatement orderByStatement = null)
        {
            return this.GetProperties(this._iEntity.PrimaryKey, whereStatement, orderByStatement);
        }

        public virtual string[] GetIdsByPage(WhereStatement whereStatement, out int recordCount, int pageIndex = 1, int pageSize = 50, OrderByStatement orderByStatement = null)
        {
            return this.GetPropertiesByPage(this._iEntity.PrimaryKey, whereStatement, out recordCount, pageIndex, pageSize, orderByStatement);
        }

        public virtual string[] GetProperties(string selectFieldName, WhereStatement whereStatement, OrderByStatement orderByStatement = null)
        {
            whereStatement.DbHelper = this.DbHelper;
            SelectQueryBuilder sqlBuilder = new SelectQueryBuilder(this.DbHelper);
            sqlBuilder.SetSelectColumns(selectFieldName);
            sqlBuilder.SetFromTable(this.CurrentTableName);
            sqlBuilder.AddWhere(whereStatement);

            if (orderByStatement == null)
                orderByStatement = new OrderByStatement(this._iEntity.PrimaryKey, Sorting.Ascending);
            sqlBuilder.AddOrderBy(orderByStatement);

            DbParameter[] parameters = null;
            string sql = sqlBuilder.BuildSQL(out parameters);

            DataTable dataTable = new DataTable(this.CurrentTableName);
            dataTable = this.DbHelper.Fill(sql, parameters);

            return DataTableUtil.FieldToArray(dataTable, selectFieldName);
        }

        public virtual string[] GetPropertiesByPage(string selectFieldName, WhereStatement whereStatement, out int recordCount, int pageIndex = 1, int pageSize = 50, OrderByStatement orderByStatement = null)
        {
            whereStatement.DbHelper = this.DbHelper;
            SelectQueryBuilder sqlBuilder = new SelectQueryBuilder(this.DbHelper);
            sqlBuilder.SetSelectColumns(selectFieldName);
            sqlBuilder.SetFromTable(this.CurrentTableName);
            sqlBuilder.AddWhere(whereStatement);

            DbParameter[] parameters = null;
            string sql = sqlBuilder.BuildSQL(out parameters);
            recordCount = Convert.ToInt32(this.DbHelper.ExecuteScalar(PagingBuilder.CreateCountingSql(sql), parameters));

            if (orderByStatement == null)
                orderByStatement = new OrderByStatement(this._iEntity.PrimaryKey, Sorting.Ascending);

            string orderBySql = orderByStatement.BuildOrderByStatement();
            string pagedSql = PagingBuilder.CreatePagingSql(recordCount, pageSize, pageIndex, sql, orderBySql);

            DataTable dataTable = new DataTable(this.CurrentTableName);
            dataTable = this.DbHelper.Fill(pagedSql, parameters);

            return DataTableUtil.FieldToArray(dataTable, selectFieldName);
        }
    }
}
