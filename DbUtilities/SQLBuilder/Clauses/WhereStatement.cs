using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using CodeEngine.Framework.QueryBuilder.Enums;
using JSNet.DbUtilities;

//
// Class: WhereStatement
// Copyright 2006 by Ewout Stortenbeker
// Email: 4ewout@gmail.com
//
// This class is part of the CodeEngine Framework.
// You can download the framework DLL at http://www.code-engine.com/
//
namespace CodeEngine.Framework.QueryBuilder
{
    /// <summary>
    /// ���ӵ�WHERE�Ӿ䣺 [Level1]((Age<15 OR Age>=20) AND (strEmail LIKE 'e%')) OR [Level2](Age BETWEEN 15 AND 20)��ͬһ��LEVEL��AND���ӣ���ͬLEVEL��OR����
    /// </summary>
    public class WhereStatement : List<List<WhereClause>>
    {
        // The list in this container will contain lists of clauses, and 
        // forms a where statement alltogether!
        private IDbHelper _dbHelper = null;

        public IDbHelper DbHelper
        {
            get { return _dbHelper; }
            set { _dbHelper = value; }
        }



        public WhereStatement()
        {

        }
        public WhereStatement(IDbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }


        /// <summary>
        /// WHERE�Ӿ���
        /// </summary>
        public int ClauseLevels
        {
            get { return this.Count; }
        }

        private void AssertLevelExistance(int level)
        {
            if (this.Count < (level - 1))
            {
                throw new Exception("Level " + level + " not allowed because level " + (level - 1) + " does not exist.");
            }
            // Check if new level must be created
            else if (this.Count < level)
            {
                this.Add(new List<WhereClause>());
            }
        }

        /// <summary>
        /// ��� WHERE�Ӿ�
        /// </summary>
        /// <param name="clause">WHERE�Ӿ����</param>
        public void Add(WhereClause clause) { this.Add(clause, 1); }
        /// <summary>
        /// ��� WHERE�Ӿ�
        /// </summary>
        /// <param name="clause">WHERE�Ӿ����</param>
        /// <param name="level">����</param>
        public void Add(WhereClause clause, int level)
        {
            this.AddWhereClauseToLevel(clause, level);
        }
        /// <summary>
        /// ��� WHERE�Ӿ�
        /// </summary>
        /// <param name="field">�ֶ���</param>
        /// <param name="operator">�ȽϷ�</param>
        /// <param name="compareValue">�ֶ�ֵ</param>
        /// <returns></returns>
        public WhereClause Add(string field, Comparison @operator, object compareValue) { return this.Add(field, @operator, compareValue, 1); }
        /// <summary>
        /// ��� WHERE�Ӿ�
        /// </summary>
        /// <param name="field">�ֶ���ö��</param>
        /// <param name="operator">�ȽϷ�</param>
        /// <param name="compareValue">�ֶ�ֵ</param>
        /// <returns></returns>
        public WhereClause Add(Enum field, Comparison @operator, object compareValue) { return this.Add(field.ToString(), @operator, compareValue, 1); }
        /// <summary>
        /// ��� ��ӦLEVEL�� WHERE�Ӿ�
        /// </summary>
        /// <param name="field">�ֶ���</param>
        /// <param name="operator">�ȽϷ�</param>
        /// <param name="compareValue">�ֶ�ֵ</param>
        /// <param name="level">����</param>
        /// <returns></returns>
        public WhereClause Add(string field, Comparison @operator, object compareValue, int level)
        {
            WhereClause NewWhereClause = new WhereClause(field, @operator, compareValue);
            this.AddWhereClauseToLevel(NewWhereClause, level);
            return NewWhereClause;
        }

        /// <summary>
        /// ��� WHERE�Ӿ� 
        /// </summary>
        /// <param name="clause">WHERE�Ӿ�</param>
        private void AddWhereClause(WhereClause clause)
        {
            AddWhereClauseToLevel(clause, 1);
        }

        /// <summary>
        /// ��� ��ӦLEVEL�� WHERE�Ӿ� 
        /// </summary>
        /// <param name="clause">WHERE�Ӿ�</param>
        /// <param name="level">����</param>
        private void AddWhereClauseToLevel(WhereClause clause, int level)
        {
            // Add the new clause to the array at the right level
            AssertLevelExistance(level);
            this[level - 1].Add(clause);
        }

        /// <summary>
        /// ����WHERE�Ӿ䣨�ǲ�������
        /// </summary>
        /// <returns></returns>
        public string BuildWhereStatement()
        {
            List<DbParameter> parameters; // = DataAccess.UsedDbProviderFactory.CreateCommand();
            return BuildWhereStatement(false, out parameters);
        }

        /// <summary>
        /// ����WHERE�Ӿ䣬 [Level1]((Age >15 OR Age >=20) AND (strEmail LIKE 'e%')) OR [Level2] Age BETWEEN 15 AND 20 ÿ�� [Level] �� OR ������ÿ�� WhereClause �� AND ����
        /// </summary>
        /// <param name="useCommandObject">ʹ�ò�����</param>
        /// <param name="usedDbCommand">�������</param>
        /// <returns></returns>
        public string BuildWhereStatement(bool useCommandObject, out List<DbParameter> parameters)
        {
            string Result = "";
            parameters = new List<DbParameter>();
            foreach (List<WhereClause> WhereStatement in this) // Loop through all statement levels, OR them together
            {
                #region Loop through all statement levels, OR them together
                string LevelWhere = "";
                foreach (WhereClause Clause in WhereStatement) // Loop through all conditions, AND them together
                {
                    #region Loop through all conditions, AND them together
                    string WhereClause = "";

                    if (useCommandObject)
                    {
                        // Create a parameter
                        string parameterName = string.Format(
                                            "p{0}_{1}",
                                            parameters.Count + 1,
                                            Clause.FieldName.Replace('.', '_')
                                            );
                        //�������ֵ����������
                        if(Clause.Value is Array)
                        {
                            int index = 0;
                            Array array = Clause.Value as Array;
                            string[] parameterNames = new string[array.Length];
                            foreach (object obj in array)
                            {
                                parameterNames[index] = parameterName + "_" + (index + 1);
                                parameters.Add((DbParameter)_dbHelper.MakeParameter(parameterNames[index], obj));
                                index++;
                            }
                            // Create a where clause using the parameter, instead of its value
                            WhereClause += CreateComparisonClause(Clause.FieldName, Clause.ComparisonOperator, new SqlLiteral(_dbHelper.GetParameters(parameterNames)));
                        }
                        else
                        {
                            parameters.Add((DbParameter)_dbHelper.MakeParameter(parameterName, Clause.Value));
                            // Create a where clause using the parameter, instead of its value
                            WhereClause += CreateComparisonClause(Clause.FieldName, Clause.ComparisonOperator, new SqlLiteral(_dbHelper.GetParameter(parameterName)));
                        }
                    }
                    else
                    {
                        WhereClause = CreateComparisonClause(Clause.FieldName, Clause.ComparisonOperator, Clause.Value);
                    }

                    foreach (WhereClause.SubClause SubWhereClause in Clause.SubClauses)	// Loop through all subclauses, append them together with the specified logic operator
                    {
                        #region Loop through all subclauses, append them together with the specified logic operator
                        switch (SubWhereClause.LogicOperator)
                        {
                            case LogicOperator.And:
                                WhereClause += " AND "; break;
                            case LogicOperator.Or:
                                WhereClause += " OR "; break;
                        }

                        if (useCommandObject)
                        {
                            string parameterName = string.Format(
                                "p{0}_{1}",
                                parameters.Count + 1,
                                Clause.FieldName.Replace('.', '_')
                            );
                            //�������ֵ����������
                            if (SubWhereClause.Value is Array)
                            {
                                int index = 0;
                                Array array = SubWhereClause.Value as Array;
                                string[] parameterNames = new string[array.Length];
                                foreach (object obj in array)
                                {
                                    parameterNames[index] = parameterName + "_" + (index + 1);
                                    parameters.Add((DbParameter)_dbHelper.MakeParameter(parameterNames[index], obj));
                                    index++;
                                }
                                // Create a where clause using the parameter, instead of its value
                                WhereClause += CreateComparisonClause(Clause.FieldName, SubWhereClause.ComparisonOperator, new SqlLiteral(_dbHelper.GetParameters(parameterNames)));
                            }
                            else
                            {
                                parameters.Add((DbParameter)_dbHelper.MakeParameter(parameterName, SubWhereClause.Value));

                                // Create a where clause using the parameter, instead of its value
                                WhereClause += CreateComparisonClause(Clause.FieldName, SubWhereClause.ComparisonOperator, new SqlLiteral(_dbHelper.GetParameter(parameterName)));
                            }
                        }
                        else
                        {
                            WhereClause += CreateComparisonClause(Clause.FieldName, SubWhereClause.ComparisonOperator, SubWhereClause.Value);
                        }
                        #endregion
                    }
                    LevelWhere += "(" + WhereClause + ") AND ";
                    #endregion
                }
                LevelWhere = LevelWhere.Substring(0, LevelWhere.Length - 5); // Trim de last AND inserted by foreach loop
                if (WhereStatement.Count > 1)
                {
                    Result += " (" + LevelWhere + ") ";
                }
                else
                {
                    Result += " " + LevelWhere + " ";
                }
                Result += " OR"; 
                #endregion
            }
            Result = Result.Substring(0, Result.Length - 2); // Trim de last OR inserted by foreach loop
            return Result;
        }

        /// <summary>
        /// �����ӴӾ�
        /// </summary>
        /// <param name="fieldName">�ֶ���</param>
        /// <param name="comparisonOperator">�ȽϷ�</param>
        /// <param name="value">�ֶ�ֵ</param>
        /// <returns></returns>
        internal string CreateComparisonClause(string fieldName, Comparison comparisonOperator, object value)
        {
            string Output = "";
            if (value != null && value != System.DBNull.Value)
            {
                switch (comparisonOperator)
                {
                    case Comparison.Equals:
                        Output = fieldName + " = " + FormatSQLValue(value); break;
                    case Comparison.NotEquals:
                        Output = fieldName + " <> " + FormatSQLValue(value); break;
                    case Comparison.GreaterThan:
                        Output = fieldName + " > " + FormatSQLValue(value); break;
                    case Comparison.GreaterOrEquals:
                        Output = fieldName + " >= " + FormatSQLValue(value); break;
                    case Comparison.LessThan:
                        Output = fieldName + " < " + FormatSQLValue(value); break;
                    case Comparison.LessOrEquals:
                        Output = fieldName + " <= " + FormatSQLValue(value); break;
                    case Comparison.Like:
                        Output = fieldName + " LIKE " + FormatSQLValue(value); break;
                    case Comparison.NotLike:
                        Output = "NOT " + fieldName + " LIKE " + FormatSQLValue(value); break;
                    case Comparison.In:
                        Output = fieldName + " IN (" + FormatSQLValue(value) + ")"; break;
                }
            }
            else // value==null	|| value==DBNull.Value
            {
                if ((comparisonOperator != Comparison.Equals) && (comparisonOperator != Comparison.NotEquals))
                {
                    throw new Exception("Cannot use comparison operator " + comparisonOperator.ToString() + " for NULL values.");
                }
                else
                {
                    switch (comparisonOperator)
                    {
                        case Comparison.Equals:
                            Output = fieldName + " IS NULL"; break;
                        case Comparison.NotEquals:
                            Output = "NOT " + fieldName + " IS NULL"; break;
                    }
                }
            }
            return Output;
        }

        /// <summary>
        /// ��ʽ��SQL�ֶ�ֵ
        /// </summary>
        /// <param name="someValue"></param>
        /// <returns></returns>
        internal string FormatSQLValue(object someValue)
        {
            string FormattedValue = "";
            //				string StringType = Type.GetType("string").Name;
            //				string DateTimeType = Type.GetType("DateTime").Name;

            if (someValue == null)
            {
                FormattedValue = "NULL";
            }
            else
            {
                switch (someValue.GetType().Name)
                {
                    case "Int32[]": FormattedValue = ObjectsToList<Int32>(someValue); break;
                    case "String[]": FormattedValue = ObjectsToList<string>(someValue); break;
                    case "String": FormattedValue = "'" + ((string)someValue).Replace("'", "''") + "'"; break;
                    case "DateTime": FormattedValue = "'" + ((DateTime)someValue).ToString(_dbHelper.DateTimeFormat) + "'"; break;
                    case "DBNull": FormattedValue = "NULL"; break;
                    case "Boolean": FormattedValue = (bool)someValue ? "1" : "0"; break;
                    case "SqlLiteral": FormattedValue = ((SqlLiteral)someValue).Value; break;
                    default: FormattedValue = someValue.ToString(); break;
                }
            }
            return FormattedValue;
        }

        /// <summary>
        /// This static method combines 2 where statements with eachother to form a new statement�� ���� WhereStatement ����
        /// </summary>
        /// <param name="statement1"></param>
        /// <param name="statement2"></param>
        /// <returns></returns>
        public static WhereStatement CombineStatements(WhereStatement statement1, WhereStatement statement2)
        {
            // statement1: {Level1}((Age<15 OR Age>=20) AND (strEmail LIKE 'e%') OR {Level2}(Age BETWEEN 15 AND 20))
            // Statement2: {Level1}((Name = 'Peter'))
            // Return statement: {Level1}((Age<15 or Age>=20) AND (strEmail like 'e%') AND (Name = 'Peter'))

            // Make a copy of statement1
            WhereStatement result = WhereStatement.Copy(statement1);

            // Add all clauses of statement2 to result
            for (int i = 0; i < statement2.ClauseLevels; i++) // for each clause level in statement2
            {
                List<WhereClause> level = statement2[i];
                foreach (WhereClause clause in level) // for each clause in level i
                {
                    for (int j = 0; j < result.ClauseLevels; j++)  // for each level in result, add the clause
                    {
                        result.AddWhereClauseToLevel(clause, j);
                    }
                }
            }

            return result;
        }

        public static WhereStatement Copy(WhereStatement statement)
        {
            WhereStatement result = new WhereStatement();
            int currentLevel = 0;
            foreach (List<WhereClause> level in statement)
            {
                currentLevel++;
                result.Add(new List<WhereClause>());
                foreach (WhereClause clause in statement[currentLevel - 1])
                {
                    WhereClause clauseCopy = new WhereClause(clause.FieldName, clause.ComparisonOperator, clause.Value);
                    foreach (WhereClause.SubClause subClause in clause.SubClauses)
                    {
                        WhereClause.SubClause subClauseCopy = new WhereClause.SubClause(subClause.LogicOperator, subClause.ComparisonOperator, subClause.Value);
                        clauseCopy.SubClauses.Add(subClauseCopy);
                    }
                    result[currentLevel - 1].Add(clauseCopy);
                }
            }
            return result;
        }


        /// <summary>
        /// �ֶ�ֵ����ת��Ϊ�ַ����б�
        /// </summary>
        /// <param name="ids">�ֶ�ֵ</param>
        /// <returns>�ֶ�ֵ�ַ���</returns>
        public string ObjectsToList<T>(object obj)
        {
            T[] arry = null;
            if (obj is Array)
            {
                arry = obj as T[];
            }
            else
            {
                throw new Exception("obj is not Array");
            }

            string returnValue = string.Empty;

            string stringList = string.Empty;

            string splitor = ",";
            for (int i = 0; i < arry.Length; i++)
            {
                if (this._dbHelper.CurrentDbType == DbTypeName.Access)
                {
                    stringList += arry[i] + splitor;
                }
                else
                {
                    stringList += "'" + arry[i] + "'" + splitor;
                }
            }

            if (arry.Length == 0)
            {
                returnValue = " NULL ";
            }
            else
            {
                returnValue = stringList.Substring(0, stringList.Length - splitor.Length);
            }

            return returnValue;
        }

    }

}
