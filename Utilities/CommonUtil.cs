using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace JSNet.Utilities
{
    public partial class CommonUtil
    {
        #region public static string NewGuid() 获得 Guid
        /// <summary>
        /// 获得 Guid
        /// </summary>
        /// <returns>主键</returns>
        public static string NewGuid()
        {
            return Guid.NewGuid().ToString();
        }
        #endregion

        //
        // WebService 传递参数的专用方法
        //

        #region public static byte[] GetBinaryFormatData(DataTable dataTable) 服务器上面取数据,填充数据权限,转换为二进制格式.
        /// <summary>
        /// 服务器上面取数据,填充数据权限,转换为二进制格式.
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <returns>二进制格式</returns>
        public static byte[] GetBinaryFormatData(DataTable dataTable)
        {
            byte[] ArrayResult = null;
            dataTable.RemotingFormat = SerializationFormat.Binary;
            MemoryStream memoryStream = new MemoryStream();
            IFormatter IFormatter = new BinaryFormatter();
            IFormatter.Serialize(memoryStream, dataTable);
            ArrayResult = memoryStream.ToArray();
            memoryStream.Close();
            memoryStream.Dispose();
            return ArrayResult;
        }
        #endregion

        #region public static DataTable RetrieveDataSet(byte[] ArrayResult) 客户端接收到byte[]格式的数据,对其进行反序列化,得到数据权限,进行客户端操作.
        /// <summary>
        /// 客户端接收到byte[]格式的数据,对其进行反序列化,得到数据权限,进行客户端操作.
        /// </summary>
        /// <param name="ArrayResult">二进制格式</param>
        /// <returns>数据表</returns>
        public static DataTable RetrieveDataTable(byte[] arrayResult)
        {
            DataTable dataTable = null;
            MemoryStream memoryStream = new MemoryStream(arrayResult);
            IFormatter IFormatter = new BinaryFormatter();
            Object targetObject = IFormatter.Deserialize(memoryStream);
            memoryStream.Close();
            memoryStream.Dispose();
            dataTable = (DataTable)targetObject;
            return dataTable;
        }
        #endregion

        #region public static string CutLastDot(string input) 字符串去掉结尾逗号
        /// <summary>
        /// 字符串去掉结尾逗号
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns>字符串</returns>
        public static string CutLastDot(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            else
            {
                if (input.IndexOf(",") > -1)
                {
                    int intLast = input.LastIndexOf(",");
                    if ((intLast + 1) == input.Length)
                    {
                        return input.Remove(intLast);
                    }
                    else
                    {
                        return input;
                    }
                }
                else
                {
                    return input;
                }
            }
        }
        #endregion

        ///
        ///  对象复制
        ///

        #region private static int SetClassValue(object sourceObject, string field, object targetObject) 设置对象的属性
        /// <summary>
        /// 设置对象的属性
        /// </summary>
        /// <param name="sourceObject">目标对象</param>
        /// <param name="field">属性名称</param>
        /// <param name="targetValue">目标值</param>
        /// <returns>影响的属性个数</returns>
        private static int SetClassValue(object sourceObject, string field, object targetValue)
        {
            int returnValue = 0;
            Type currentType = sourceObject.GetType();
            FieldInfo[] fieldInfo = currentType.GetFields(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo currentFieldInfo;
            for (int i = 0; i < fieldInfo.Length; i++)
            {
                if (field.Equals(fieldInfo[i].Name))
                {
                    currentFieldInfo = currentType.GetField(field);
                    currentFieldInfo.SetValue(sourceObject, targetValue);
                    // FieldInfo[i].SetValue(TargetObject, value);
                    returnValue++;
                    break;
                }
            }
            return returnValue;
        }
        #endregion

        #region public static object CopyObjectValue(object sourceObject, object targetObject) 复制类对象的对应的值
        /// <summary>
        /// 复制类对象的对应的值
        /// </summary>
        /// <param name="sourceObject">当前对象</param>
        /// <param name="targetObject">目标对象</param>
        /// <returns>对象</returns>
        public static object CopyObjectValue(object sourceObject, object targetObject)
        {
            int returnValue = 0;
            string name = string.Empty;
            Type type = sourceObject.GetType();
            FieldInfo[] fieldInfo = type.GetFields(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo currentFieldInfo;
            for (int i = 0; i < fieldInfo.Length; i++)
            {
                name = fieldInfo[i].Name;
                currentFieldInfo = fieldInfo[i];
                returnValue = SetClassValue(targetObject, name, currentFieldInfo.GetValue(sourceObject));
            }
            return targetObject;
        }
        #endregion

        #region public static object CopyObjectProperties(object sourceObject, object targetObject)
        /// <summary>
        /// 复制属性
        /// </summary>
        /// <param name="sourceObject">类</param>
        /// <param name="targetObject">目标类</param>
        /// <returns>类</returns>
        public static object CopyObjectProperties(object sourceObject, object targetObject)
        {
            Type typeSource = sourceObject.GetType();
            Type typeTarget = targetObject.GetType();
            PropertyInfo[] propertyInfoSource = typeSource.GetProperties();
            PropertyInfo[] propertyInfoTarget = typeTarget.GetProperties();
            for (int i = 0; i < propertyInfoTarget.Length; i++)
            {
                for (int j = 0; j < propertyInfoSource.Length; j++)
                {
                    if (propertyInfoTarget[i].Name.Equals(propertyInfoSource[j].Name))
                    {
                        if (propertyInfoTarget[i].CanWrite)
                        {
                            object pValue = propertyInfoSource[j].GetValue(sourceObject, null);
                            propertyInfoTarget[i].SetValue(targetObject, pValue, null);
                        }
                        break;
                    }
                }
            }
            return targetObject;
        }
        #endregion

        /// 
        ///  类型转换
        ///

        #region 各种类型转换


        /// <summary>
        /// 将字符类型主键转换为数值类型主键
        /// </summary>
        /// <param name="ids">字符类型主键</param>
        /// <returns>数值型主键数组</returns>
        public static int[] ConvertToIntArry(string[] ids)
        {
            List<int> keys = new List<int>();
            foreach (var key in ids)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    keys.Add(int.Parse(key));
                }
            }
            return keys.ToArray();
        }

        public static string ConvertToString(Object targetValue)
        {
            return targetValue != DBNull.Value ? Convert.ToString(targetValue) : null;
        }

        public static int? ConvertToInt(Object targetValue)
        {
            int? returnValue = null; 
            if (targetValue == DBNull.Value)
            {
                return returnValue;
            }

            int result = 0;
            int.TryParse(targetValue.ToString(), out result);
            returnValue = result;

            return returnValue;
        }

        public static Int32? ConvertToInt32(Object targetValue)
        {
            Int32? returnValue = null; 
            if (targetValue == DBNull.Value)
            {
                return returnValue;
            }

            Int32 result = 0;
            Int32.TryParse(targetValue.ToString(), out result);
            returnValue = result;

            return returnValue;
        }

        public static Int64? ConvertToInt64(Object targetValue)
        {
            Int64? returnValue = null; 
            if (targetValue == DBNull.Value)
            {
                return returnValue;
            }

            Int64 result = 0;
            Int64.TryParse(targetValue.ToString(), out result);
            returnValue = result;

            return returnValue;
        }

        public static long? ConvertToLong(Object targetValue)
        {
            long? returnValue = null; 
            if (targetValue == DBNull.Value)
            {
                return returnValue;
            }

            long result = 0;
            long.TryParse(targetValue.ToString(), out result);
            returnValue = result;

            return returnValue;
        }        

        public static Boolean ConvertIntToBoolean(Object targetValue)
        {
            return targetValue != DBNull.Value ? (targetValue.ToString().Equals("1") || targetValue.ToString().Equals(true.ToString())) : false;
        }

        public static Boolean ConvertToBoolean(Object targetValue)
        {
            return targetValue != DBNull.Value ? (targetValue.ToString().Equals(true.ToString())) : false;
        }

        public static Double? ConvertToDouble(Object targetValue)
        {
            Double? returnValue = null;
            if (targetValue == DBNull.Value)
            {
                return returnValue;
            }

            Double result = 0;
            Double.TryParse(targetValue.ToString(), out result);
            returnValue = result;

            return returnValue;
        }

        public static float? ConvertToFloat(Object targetValue)
        {
            float? returnValue = null;
            if (targetValue == DBNull.Value)
            {
                return returnValue;
            }

            float result = 0;
            float.TryParse(targetValue.ToString(), out result);
            returnValue = result;

            return returnValue;
        }

        public static decimal? ConvertToDecimal(Object targetValue)
        {
            decimal? returnValue = null;
            if (targetValue == DBNull.Value)
            {
                return returnValue;
            }

            decimal result = 0;
            decimal.TryParse(targetValue.ToString(), out result);
            returnValue = result;

            return returnValue;
        }

        public static DateTime? ConvertToDateTime(Object targetValue)
        {
            DateTime? returnValue = null;
            if (targetValue != DBNull.Value)
            {
                returnValue = Convert.ToDateTime(targetValue.ToString());
            }
            return returnValue;
        }

        public static Guid? ConvertToGuid(Object targetValue)
        {
            Guid? returnValue = null;
            if (targetValue != DBNull.Value)
            {
                returnValue = Guid.Parse(targetValue.ToString());
            }
            return returnValue;
        }

        public static string ConvertToDateToString(Object targetValue,string datetimeFormat = "yyyy-MM-dd HH:mm:ss")
        {
            string returnValue = string.Empty;
            returnValue = targetValue != DBNull.Value ? DateTime.Parse(targetValue.ToString()).ToString(datetimeFormat) : null;
            return returnValue;
        }

        public static byte[] ConvertToByte(Object targetValue)
        {
            return targetValue != DBNull.Value ? (byte[])targetValue : null;
        }
        #endregion

    }
}