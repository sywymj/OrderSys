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

        public new T GetSingle(object idValue, string idField = null,WhereStatement whereStatement = null)
        {
            string returnValue = string.Empty;
            SelectQueryBuilder sqlBuilder = new SelectQueryBuilder(this.DbHelper);
            sqlBuilder.SetSelectAllColumns();
            sqlBuilder.SetFromTable(this.CurrentTableName);

            if (whereStatement == null)
                whereStatement = new WhereStatement(this.DbHelper);
            if (idField == null)
                whereStatement.Add(this._iEntity.PrimaryKey, Comparison.Equals, idValue);
            else
                whereStatement.Add(idField, Comparison.Equals, idValue);
            whereStatement.DbHelper = this.DbHelper;
            sqlBuilder.AddWhere(whereStatement);

            DbParameter[] parameters = null;
            string sql = sqlBuilder.BuildSQL(out parameters);

            DataTable dataTable = new DataTable(this.CurrentTableName);
            dataTable = this.DbHelper.Fill(sql, parameters);

            T t = null;
            if (dataTable.Rows.Count > 0)
            {
                //t = this.iEntity.GetFrom(dataTable.Rows[0]);
                t = this.ToObject(dataTable.Rows[0]);
            }
            return t;
        }
    }
}