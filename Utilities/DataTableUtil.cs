using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace JSNet.Utilities
{
    public class DataTableUtil
    {
        #region public static string GetDateTime(DataRow dataRow, string name) 获取日期时间
        /// <summary>
        /// 获取日期时间
        /// </summary>
        /// <param name="dataRow">数据行</param>
        /// <param name="name">字段名</param>
        /// <returns>日期时间</returns>
        public static string GetDateTime(DataRow dataRow, string name,string datetimeFormat = "yyyy-MM-dd HH:mm:ss")
        {
            string returnValue = string.Empty;
            if (!dataRow.IsNull(name))
            {
                DateTime DateTime = DateTime.Parse(dataRow[name].ToString());
                returnValue = DateTime.ToString(datetimeFormat);
            }
            return returnValue;
        }
        #endregion

        #region public static string FieldToList(DataTable dataTable, string name) 表格字段转换为字符串列表
        /// <summary>
        /// 表格字段转换为字符串列表
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <param name="name">列名</param>
        /// <returns>字段值字符串</returns>
        public static string FieldToList(DataTable dataTable, string name)
        {
            int rowCount = 0;
            string returnValue = "'";
            foreach (DataRow dataRow in dataTable.Rows)
            {
                rowCount++;
                returnValue += dataRow[name].ToString() + "', '";
            }
            if (rowCount == 0)
            {
                returnValue = "''";
            }
            else
            {
                returnValue = returnValue.Substring(0, returnValue.Length - 3);
            }
            return returnValue;
        }
        #endregion

        #region public static string[] FieldToArray(DataTable dataTable, string name) dataTable某列转换为字符串数组
        /// <summary>
        /// dataTable某列转换为字符串数组
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <param name="name">列名</param>
        /// <returns>字段值数组</returns>
        public static string[] FieldToArray(DataTable dataTable, string name)
        {
            string[] returnValue = new string[0];
            int rowCount = 0;
            string stringList = string.Empty;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (!string.IsNullOrEmpty(dataRow[name].ToString()))
                {
                    rowCount++;
                    stringList += dataRow[name].ToString() + ",";
                }
            }
            if (rowCount > 0)
            {
                stringList = stringList.TrimEnd(',');
                returnValue = stringList.Split(',');
            }
            return returnValue;
        }
        #endregion

        #region public static DataTable SetFilter(DataTable dataTable, string fieldName, string fieldValue, bool equals = false) 对数据表进行过滤
        /// <summary>
        /// 对数据表进行过滤
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="fieldValue">字段值</param>
        /// <param name="equals">相等</param>
        /// <returns>数据权限</returns>
        public static DataTable SetFilter(DataTable dataTable, string fieldName, string fieldValue, bool equals = false)
        {
            foreach (DataRow dataRow in dataTable.Rows)
            {
                // 要求把相等的删除掉
                if (equals)
                {
                    if (string.IsNullOrEmpty(fieldValue))
                    {
                        if (string.IsNullOrEmpty(dataRow[fieldName].ToString()))
                        {
                            dataRow.Delete();
                        }
                    }
                    else
                    {
                        if (dataRow[fieldName].ToString().Equals(fieldValue))
                        {
                            dataRow.Delete();
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(fieldValue))
                    {
                        if (!string.IsNullOrEmpty(dataRow[fieldName].ToString()))
                        {
                            dataRow.Delete();
                        }
                    }
                    else
                    {
                        if (!dataRow[fieldName].ToString().Equals(fieldValue))
                        {
                            dataRow.Delete();
                        }
                    }
                }
            }
            dataTable.AcceptChanges();
            return dataTable;
        }
        #endregion

        #region public static string GetProperty(DataTable dataTable, string id, string targetField) 读取一个属性
        public static string GetPropertyDyn(dynamic lstT, string fieldName, string fieldValue, string targetField)
        {
            string returnValue = string.Empty;
            foreach (dynamic t in lstT)
            {
                if (ReflectionUtil.GetProperty(t, fieldName).ToString().Equals(fieldValue))
                {
                    returnValue =ReflectionUtil.GetProperty(t, targetField).ToString();
                    break;
                }
            }
            return returnValue;
        }
        #endregion

        #region public static string GetProperty(DataTable dataTable, string fieldName, string fieldValue, string targetField) 读取一个属性
        /// <summary>
        /// 读取一个属性
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <param name="fieldName">字段</param>
        /// <param name="fieldValue">值</param>
        /// <param name="targetField">目标字段</param>
        /// <returns>目标值</returns>
        public static string GetProperty(DataTable dataTable, string fieldName, string fieldValue, string targetField)
        {
            string returnValue = string.Empty;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (dataRow[fieldName].ToString().Equals(fieldValue))
                {
                    returnValue = dataRow[targetField].ToString();
                    break;
                }
            }
            return returnValue;
        }
        #endregion

        #region public static int SetProperty(DataTable dataTable, string fieldName, string fieldValue, string targetField, object targetValue) 设置一个属性
        /// <summary>
        /// 设置一个属性
        /// </summary>        
        /// <param name="dataTable">数据表</param>
        /// <param name="fieldName">字段</param>
        /// <param name="fieldValue">值</param>
        /// <param name="targetField">更新字段</param>
        /// <param name="targetValue">目标值</param>
        /// <returns>影响行数</returns>
        public static int SetProperty(DataTable dataTable, string fieldName, string fieldValue, string targetField, object targetValue)
        {
            int returnValue = 0;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (dataRow.RowState != DataRowState.Deleted)
                {
                    if (dataRow[fieldName].ToString().Equals(fieldValue))
                    {
                        dataRow[targetField] = targetValue;
                        returnValue++;
                        // break;
                    }
                }
            }
            return returnValue;
        }
        public static int SetPropertyDyn(dynamic lstT, string fieldName, string fieldValue, string targetField, object targetValue)
        {
            int returnValue = 0;
            for (int i = 0; i < lstT.Count; i++) {
                dynamic t = lstT[i];
                if (ReflectionUtil.GetProperty(t, fieldName).ToString().Equals(fieldValue))
                {
                    ReflectionUtil.SetProperty(t, targetField, targetValue);
                    lstT[i] = t;
                    returnValue++;
                    // break;
                }
            }            
            return returnValue;
        }
        #endregion

        #region public static int Delete(DataTable dataTable, string fieldName, string fieldValue) 删除记录
        /// <summary>
        /// 删除一条记录
        /// </summary>        
        /// <param name="dataTable">数据表名</param>
        /// <param name="fieldName">字段</param>
        /// <param name="fieldValue">值</param>
        /// <returns>影响行数</returns>
        public static int Delete(DataTable dataTable, string fieldName, string fieldValue)
        {
            int returnValue = 0;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (dataRow[fieldName].ToString().Equals(fieldValue))
                {
                    dataRow.Delete();
                    returnValue++;
                    break;
                }
            }
            return returnValue;
        }
        #endregion

        #region public static DataRow GetDataRow(DataTable dataTable, string fieldName, string fieldValue) 从数据权限读取一行数据
        /// <summary>
        /// 从数据权限读取一行数据
        /// </summary>        
        /// <param name="dataTable">数据表</param>
        /// <param name="fieldName">字段</param>
        /// <param name="fieldValue">值</param>
        /// <returns>数据行</returns>
        public static DataRow GetDataRow(DataTable dataTable, string fieldName, string fieldValue)
        {
            DataRow returnValue = null;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                if (dataRow.RowState != DataRowState.Deleted)
                {
                    if (dataRow[fieldName].ToString().Equals(fieldValue))
                    {
                        returnValue = dataRow;
                        break;
                    }
                }
            }
            return returnValue;
        }
        #endregion
    }
}