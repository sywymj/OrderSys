using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using CodeEngine.Framework.QueryBuilder.Enums;

namespace CodeEngine.Framework.QueryBuilder
{
    public class OrderByStatement : List<OrderByClause>
    {
        public OrderByStatement()
        {

        }
        public OrderByStatement(OrderByClause clause)
        {
            this.Add(clause);
        }

        public OrderByStatement(string field, Sorting order)
        {
            this.Add(new OrderByClause(field, order));
        }

        public OrderByStatement(string[] fields, Sorting order)
        {
            foreach (string field in fields)
            {
                this.Add(new OrderByClause(field, order));
            }
        }

        /// <summary>
        /// OrderBy从句数
        /// </summary>
        public int ClauseCount
        {
            get { return this.Count; }
        }

        public void Add(string field, Sorting order) { this.Add(new OrderByClause(field, order)); }

        public string BuildOrderByStatement()
        {
            string sql = string.Empty;
            // Output OrderBy statement
            if (this.Count > 0)
            {
                sql += " ORDER BY ";
                foreach (OrderByClause Clause in this)
                {
                    string OrderByClause = "";
                    switch (Clause.SortOrder)
                    {
                        case Sorting.Ascending:
                            OrderByClause = Clause.FieldName + " ASC"; break;
                        case Sorting.Descending:
                            OrderByClause = Clause.FieldName + " DESC"; break;
                    }
                    sql += OrderByClause + ',';
                }
                sql = sql.TrimEnd(','); // Trim de last AND inserted by foreach loop
                sql += ' ';
            }
            return sql;
        }
    }
}
