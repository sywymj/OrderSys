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

        public virtual List<T> GetList(WhereStatement whereStatement, out int recordCount, OrderByStatement orderByStatement = null)
        {
            DataTable dataTable = this.GetDataTable(whereStatement, out recordCount, orderByStatement);
            List<T> list = ToList(dataTable);
            return list;
        }

    }
}
