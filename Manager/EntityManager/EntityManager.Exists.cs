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

        public bool Exists(string targetField, object targetValue)
        {
            WhereStatement whereStatement = new WhereStatement(this.DbHelper);
            whereStatement.Add(targetField, Comparison.Equals, targetValue);

            bool b = Exists(whereStatement);
            return b;
        }

        public bool Exists(WhereStatement whereStatement)
        {
            int count = GetCount(whereStatement);
            if (count > 0)
                return true;
            else
                return false;
        }
    }
}
