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
        public List<T> GetListByPage(WhereStatement whereStatement, out int recordCount, int pageIndex = 1, int pageSize = 50, OrderByStatement orderByStatement = null)
        {
            DataTable dataTable = this.GetDataTableByPage(whereStatement, out recordCount, pageIndex, pageSize, orderByStatement);
            List<T> list = ToList(dataTable);
            return list;
        }
    }
}
