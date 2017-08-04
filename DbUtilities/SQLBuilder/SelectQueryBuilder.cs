using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using CodeEngine.Framework.QueryBuilder.Enums;
using System.Data;
using JSNet.DbUtilities;

//
// Class: SelectQueryBuilder
// Copyright 2006 by Ewout Stortenbeker
// Email: 4ewout@gmail.com
//
// This class is part of the CodeEngine Framework. This framework also contains
// the UpdateQueryBuilder, InsertQueryBuilder and DeleteQueryBuilder.
// You can download the framework DLL at http://www.code-engine.com/
// 
namespace CodeEngine.Framework.QueryBuilder
{
    /// <summary>
    /// ��ѯ���//��Ҫ�ĳ�ʹ��IDBHelper
    /// </summary>
    public class SelectQueryBuilder :BaseSQLBuilder, ISQLBuilder
    {
        protected bool _distinct = false;
        protected TopClause _topClause = new TopClause(100, TopUnit.Percent);   // TOP �Ӿ�
        protected List<string> _selectedColumns = new List<string>();	// array of string
        protected List<JoinClause> _joins = new List<JoinClause>();	// JOIN �Ӿ�
        protected OrderByStatement _orderByStatement = new OrderByStatement();	// ORDER BY �Ӿ�
        protected List<string> _groupByColumns = new List<string>();		// GROUP BY �Ӿ�
        protected WhereStatement _havingStatement =null;// HAVING �Ӿ�

        public SelectQueryBuilder(IDbHelper dbHelper)
            : base(dbHelper)
        {
            _havingStatement = new WhereStatement(dbHelper);// HAVING �Ӿ�        
        }


        /// <summary>
        /// ʹ�� DISTINCT
        /// </summary>
        public bool Distinct
        {
            get { return _distinct; }
            set { _distinct = value; }
        }
        /// <summary>
        /// TOP ����
        /// </summary>
        public int TopRecords
        {
            get { return _topClause.Quantity; }
            set
            {
                _topClause.Quantity = value;
                _topClause.Unit = TopUnit.Records;
            }
        }
        /// <summary>
        /// TOP �Ӿ�
        /// </summary>
        public TopClause TopClause
        {
            get { return _topClause; }
            set { _topClause = value; }
        }

        /// <summary>
        /// SELECT �����飬�������ʾ SELECT *
        /// </summary>
        public string[] SelectedColumns
        {
            get
            {
                if (_selectedColumns.Count > 0)
                    return _selectedColumns.ToArray();
                else
                    return new string[1] { "*" };
            }
        }

        /// <summary>
        /// SELECT ����
        /// </summary>
        /// <param name="column"></param>
        public void SetSelectColumn(string column)
        {
            _selectedColumns.Clear();
            _selectedColumns.Add(column);
        }
        /// <summary>
        /// SELECT ��������
        /// </summary>
        /// <param name="columns">��������</param>
        public void SetSelectColumns(params string[] columns)
        {
            _selectedColumns.Clear();
            foreach (string column in columns)
            {
                _selectedColumns.Add(column);
            }
        }
        /// <summary>
        /// SELECT *
        /// </summary>
        public void SetSelectAllColumns()
        {
            _selectedColumns.Clear();
        }
        public void SetSelectCount()
        {
            SetSelectColumn("count(1)");
        }

        /// <summary>
        /// ��� JOIN �Ӿ�
        /// </summary>
        /// <param name="newJoin"></param>
        public void AddJoin(JoinClause newJoin)
        {
            _joins.Add(newJoin);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="join">JOIN����</param>
        /// <param name="toTableName"></param>
        /// <param name="toColumnName"></param>
        /// <param name="operator"></param>
        /// <param name="fromTableName"></param>
        /// <param name="fromColumnName"></param>
        public void AddJoin(JoinType join, string toTableName, string toColumnName, Comparison @operator, string fromTableName, string fromColumnName)
        {
            JoinClause NewJoin = new JoinClause(join, toTableName, toColumnName, @operator, fromTableName, fromColumnName);
            _joins.Add(NewJoin);
        }

        public void AddOrderBy(OrderByStatement statement)
        {
            _orderByStatement = statement;
        }
        public void AddOrderBy(OrderByClause clause)
        {
            _orderByStatement.Add(clause);
        }
        /// <summary>
        /// ��� ORDER BY �Ӿ�
        /// </summary>
        /// <param name="field">�ֶ���ö��</param>
        /// <param name="order">����</param>
        public void AddOrderBy(Enum field, Sorting order) { this.AddOrderBy(field.ToString(), order); }
        /// <summary>
        /// ��� ORDER BY �Ӿ�
        /// </summary>
        /// <param name="field">�ֶ���</param>
        /// <param name="order">����</param>
        public void AddOrderBy(string field, Sorting order)
        {
            OrderByClause NewOrderByClause = new OrderByClause(field, order);
            _orderByStatement.Add(NewOrderByClause);
        }
        /// <summary>
        /// GROUP BY �Ӿ�
        /// </summary>
        /// <param name="columns">GROUP BY ����</param>
        public void GroupBy(params string[] columns)
        {
            foreach (string Column in columns)
            {
                _groupByColumns.Add(Column);
            }
        }
        /// <summary>
        /// HAVING �Ӿ�
        /// </summary>
        public WhereStatement Having
        {
            get { return _havingStatement; }
            set { _havingStatement = value; }
        }
        /// <summary>
        /// ��� HAVING �Ӿ�
        /// </summary>
        /// <param name="clause">WhereClause ����</param>
        public void AddHaving(WhereClause clause) { AddHaving(clause, 1); }
        /// <summary>
        /// ��� HAVING �Ӿ�
        /// </summary>
        /// <param name="clause">WhereClause ����</param>
        /// <param name="level"></param>
        public void AddHaving(WhereClause clause, int level)
        {
            _havingStatement.Add(clause, level);
        }
        /// <summary>
        /// ��� HAVING �Ӿ� {Level1}((Age<15 OR Age>=20) AND (strEmail LIKE 'e%')) OR {Level2} Age BETWEEN 15 AND 20
        /// </summary>
        /// <param name="field">�ֶ���</param>
        /// <param name="operator">�ȽϷ�</param>
        /// <param name="compareValue">�ֶ�ֵ</param>
        /// <returns></returns>
        public WhereClause AddHaving(string field, Comparison @operator, object compareValue) { return AddHaving(field, @operator, compareValue, 1); }
        /// <summary>
        /// ��� HAVING �Ӿ� {Level1}((Age<15 OR Age>=20) AND (strEmail LIKE 'e%')) OR {Level2} Age BETWEEN 15 AND 20
        /// </summary>
        /// <param name="field">�ֶ���ö��</param>
        /// <param name="operator">�ȽϷ�</param>
        /// <param name="compareValue">�ֶ�ֵ</param>
        /// <returns></returns>
        public WhereClause AddHaving(Enum field, Comparison @operator, object compareValue) { return AddHaving(field.ToString(), @operator, compareValue, 1); }
        /// <summary>
        /// ��� HAVING �Ӿ�{Level1}((Age<15 OR Age>=20) AND (strEmail LIKE 'e%')) OR {Level2} Age BETWEEN 15 AND 20
        /// </summary>
        /// <param name="field">�ֶ���</param>
        /// <param name="operator">�ȽϷ�</param>
        /// <param name="compareValue">�ֶ�ֵ</param>
        /// <param name="level">OR���</param>
        /// <returns></returns>
        public WhereClause AddHaving(string field, Comparison @operator, object compareValue, int level)
        {
            WhereClause NewWhereClause = new WhereClause(field, @operator, compareValue);
            _havingStatement.Add(NewWhereClause, level);
            return NewWhereClause;
        }

        /// <summary>
        /// ���� ��ѯ��䣨����������DbCommand.CommandText��ȡSQL��䣩
        /// </summary>
        /// <returns></returns>
        public string BuildSQL(out DbParameter[] dbParameters)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            string sql =  this.BuildQuery(true, out parameters);
            dbParameters = parameters.ToArray();
            return sql;
        }
        /// <summary>
        /// ���� ��ѯ���
        /// </summary>
        /// <returns></returns>
        public string BuildSQL()
        {
            List<DbParameter> parameters = null;
            string sql = (string)this.BuildQuery(false, out parameters);
            return sql;
        }

        /// <summary>
        /// ���� ��ѯ���
        /// </summary>
        /// <returns>Returns a string containing the query, or a DbCommand containing a command with parameters</returns>
        private string BuildQuery(bool buildCommand, out List<DbParameter> dbParameters)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            string Query = "SELECT ";

            // Output Distinct
            if (_distinct)
            {
                Query += "DISTINCT ";
            }

            // Output Top clause
            if (!(_topClause.Quantity == 100 & _topClause.Unit == TopUnit.Percent))
            {
                Query += "TOP " + _topClause.Quantity;
                if (_topClause.Unit == TopUnit.Percent)
                {
                    Query += " PERCENT";
                }
                Query += " ";
            }

            // Output column names
            if (_selectedColumns.Count == 0)
            {
                if (_fromTables.Count == 1) 
                    Query += _fromTables[0] + "."; // By default only select * from the table that was selected. If there are any joins, it is the responsibility of the user to select the needed columns.

                Query += "*";
            }
            else
            {
                foreach (string ColumnName in _selectedColumns)
                {
                    Query += ColumnName + ',';
                }
                Query = Query.TrimEnd(','); // Trim de last comma inserted by foreach loop
                Query += ' ';
            }
            // Output table names
            if (_fromTables.Count > 0)
            {
                Query += " FROM ";
                foreach (string TableName in _fromTables)
                {
                    Query += TableName + ',';
                }
                Query = Query.TrimEnd(','); // Trim de last comma inserted by foreach loop
                Query += ' ';
            }

            // Output joins
            if (_joins.Count > 0)
            {
                foreach (JoinClause Clause in _joins)
                {
                    string JoinString = "";
                    switch (Clause.JoinType)
                    {
                        case JoinType.InnerJoin: JoinString = "INNER JOIN"; break;
                        case JoinType.OuterJoin: JoinString = "OUTER JOIN"; break;
                        case JoinType.LeftJoin: JoinString = "LEFT JOIN"; break;
                        case JoinType.RightJoin: JoinString = "RIGHT JOIN"; break;
                    }
                    JoinString += " " + Clause.ToTable + " ON ";
                    JoinString += WhereStatement.CreateComparisonClause(Clause.FromTable + '.' + Clause.FromColumn, Clause.ComparisonOperator, new SqlLiteral(Clause.ToTable + '.' + Clause.ToColumn));
                    Query += JoinString + ' ';
                }
            }

            // Output where statement
            if (_whereStatement.ClauseLevels > 0)
            {
                if (buildCommand)
                {
                    Query += " WHERE " + _whereStatement.BuildWhereStatement(true, out parameters); 
                    this._parameters.AddRange(parameters);
                }
                else
                    Query += " WHERE " + _whereStatement.BuildWhereStatement();
            }

            // Output GroupBy statement
            if (_groupByColumns.Count > 0)
            {
                Query += " GROUP BY ";
                foreach (string Column in _groupByColumns)
                {
                    Query += Column + ',';
                }
                Query = Query.TrimEnd(',');
                Query += ' ';
            }

            // Output having statement
            if (_havingStatement.ClauseLevels > 0)
            {
                // Check if a Group By Clause was set
                if (_groupByColumns.Count == 0)
                {
                    throw new Exception("Having statement was set without Group By");
                }
                if (buildCommand)
                {
                    Query += " HAVING " + _havingStatement.BuildWhereStatement(true, out parameters);
                    this._parameters.AddRange(parameters);
                } 
                else
                    Query += " HAVING " + _havingStatement.BuildWhereStatement();
            }

            // Output OrderBy statement
            Query += _orderByStatement.BuildOrderByStatement();
            
            // Return the build command
            dbParameters = this._parameters;
            return Query;
        }

    }

}
