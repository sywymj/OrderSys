using System;
using System.Collections.Generic;
using System.Text;

//
// Class: SqlLiteral
// Copyright 2006 by Ewout Stortenbeker
// Email: 4ewout@gmail.com
//
// This class is part of the CodeEngine Framework.
// You can download the framework DLL at http://www.code-engine.com/
// 
namespace CodeEngine.Framework.QueryBuilder
{
    public class SqlLiteral
    {
        public static string StatementRowsAffected = "SELECT @@ROWCOUNT";

        private string[] _values;

        private string _value;
        public string Value
        {
            get 
            {
                string reValue = _value;
                if (_values == null || _values.Length == 0) {
                    return reValue;
                }
                foreach (string value in _values)
                {
                    reValue += value + ",";
                }
                return reValue.Substring(0, reValue.Length - 1);
            }
        }

        public SqlLiteral(string value)
        {
            _value = value;
        }

        public SqlLiteral(string[] values)
        {
            _values = values;
        } 
    }

}
