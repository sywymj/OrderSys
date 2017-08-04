using System;
using System.Data;

namespace JSNet.Utilities
{
    public class SortUtil
    {
        //
        // 排序操作在内存中的运算方式定义
        //
        #region  public static string GetNextId(DataView dataView, string id) 获取下一条记录主键
        /// <summary>
        /// 获取下一条记录主键
       /// </summary>
       /// <param name="dataView"></param>
       /// <param name="id"></param>
       /// <returns></returns>
        public static string GetNextIdDyn(dynamic lstT, string id, string field)
        {
            string returnValue = string.Empty;
            bool find = false;
            foreach (dynamic t in lstT)
            {
                if (find)
                {
                    returnValue = ReflectionUtil.GetProperty(t,field).ToString();
                    break;
                }
                if (ReflectionUtil.GetProperty(t, field).ToString().Equals(id))
                {
                    find = true;
                }
            }
            return returnValue;
        }
        #endregion

        #region public static string GetNextId(DataTable dataTable, string id, string field) 获取下一条记录主键
        /// <summary>
        /// 获取下一条记录主键
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <param name="tableName">当前表</param>
        /// <param name="id">当前主键Id</param>
        /// <param name="field">当前字段</param>
        /// <returns>主键</returns>
        public static string GetNextId(DataTable dataTable, string id, string field)
        {
            return GetNextId(dataTable.DefaultView, id, field);
        }
        #endregion

        #region public static string GetNextId(DataView dataView, string id, string field) 获取下一条记录 具体方法
        /// <summary>
        /// 获取下一条记录 具体方法
       /// </summary>
       /// <param name="dataView"></param>
       /// <param name="id"></param>
       /// <param name="field"></param>
       /// <returns></returns>
        public static string GetNextId(DataView dataView, string id, string field)
        {
            string returnValue = string.Empty;
            bool find = false;
            foreach (DataRowView dataRow in dataView)
            {
                if (find)
                {
                    returnValue = dataRow[field].ToString();
                    break;
                }
                if (dataRow[field].ToString().Equals(id))
                {
                    find = true;
                }
            }
            return returnValue;
        }
        #endregion

        #region public static string GetPreviousId(DataTable dataTable, string id) 获取上一条记录主键
        /// <summary>
        /// 获取上一条记录主键
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <param name="tableName">当前表</param>
        /// <param name="id">当前主键</param>
        /// <param name="field">当前字段</param>
        /// <returns>主键</returns>
        public static string GetPreviousId(DataTable dataTable, string id, string field)
        {
            return GetPreviousId(dataTable.DefaultView, id, field);
        }
        #endregion

        #region  public static string GetPreviousId(DataView dataView, string id, string field) 获取上一条记录主键 具体方法
        /// <summary>
        ///  获取上一条记录主键 具体方法
       /// </summary>
       /// <param name="dataView"></param>
       /// <param name="id"></param>
       /// <param name="field"></param>
       /// <returns></returns>
        public static string GetPreviousId(DataView dataView, string id, string field)
        {
            string returnValue = string.Empty;
            foreach (DataRowView dataRow in dataView)
            {
                if (dataRow[field].ToString().Equals(id))
                {
                    break;
                }
                returnValue = dataRow[field].ToString();
            }
            return returnValue;
        }
        #endregion

        public static string GetPreviousIdDyn(dynamic lstT, string id, string field)
        {
            string returnValue = string.Empty;
            foreach (dynamic t in lstT)
            {
                if (ReflectionUtil.GetProperty(t,field).ToString().Equals(id))
                {
                    break;
                }
                returnValue = ReflectionUtil.GetProperty(t, field).ToString().ToString();
            }
            return returnValue;
        }
    }
}