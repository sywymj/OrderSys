using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace JSNet.Utilities
{
    public static class CommonUtil
    {
        #region public static string NewGuid() 获得 Guid
        /// <summary>
        /// 获得 Guid
        /// </summary>
        /// <returns>主键</returns>
        public static string NewGuid()
        {
            return Guid.NewGuid().ToString("N");
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
        public static int[] ConvertToIntArry(this string[] ids)
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

        #region 文件操作
        /// <summary>
        /// 删除单个文件
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        public static bool DeleteFile(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return false;
            }
            string fullpath = GetMapPath(_filepath);
            if (File.Exists(fullpath))
            {
                File.Delete(fullpath);
            }
            return true;
        }

        /// <summary>
        /// 删除上传的文件(及缩略图)
        /// </summary>
        /// <param name="_filepath"></param>
        public static void DeleteUpFile(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return;
            }
            string fullpath = GetMapPath(_filepath); //原图
            if (File.Exists(fullpath))
            {
                File.Delete(fullpath);
            }
            if (_filepath.LastIndexOf("/") >= 0)
            {
                string thumbnailpath = _filepath.Substring(0, _filepath.LastIndexOf("/") + 1) + "s_" + _filepath.Substring(_filepath.LastIndexOf("/") + 1);
                string fullTPATH = GetMapPath(thumbnailpath); //宿略图
                if (File.Exists(fullTPATH))
                {
                    File.Delete(fullTPATH);
                }
            }
        }

        /// <summary>
        /// 删除Content内容的图片
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="startstr">匹配<img>标签src属性开头字符串</param>
        public static void DeleteContentPic(string content, string startstr)
        {
            if (string.IsNullOrEmpty(content))
            {
                return;
            }
            Regex reg = new Regex("IMG[^>]*?src\\s*=\\s*(?:\"(?<1>[^\"]*)\"|'(?<1>[^\']*)')", RegexOptions.IgnoreCase);//<1>表示的内容也会输出，所以后面用maths.group[1]来接收此段内容
            MatchCollection m = reg.Matches(content);
            foreach (Match math in m)
            {
                string imgUrl = math.Groups[1].Value;
                string fullPath = GetMapPath(imgUrl);
                try
                {
                    if (imgUrl.ToLower().StartsWith(startstr.ToLower()) && File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// 删除指定文件夹
        /// </summary>
        /// <param name="_dirpath">文件相对路径</param>
        public static bool DeleteDirectory(string _dirpath)
        {
            if (string.IsNullOrEmpty(_dirpath))
            {
                return false;
            }
            string fullpath = GetMapPath(_dirpath);
            if (Directory.Exists(fullpath))
            {
                Directory.Delete(fullpath, true);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 修改指定文件夹名称
        /// </summary>
        /// <param name="old_dirpath">旧相对路径</param>
        /// <param name="new_dirpath">新相对路径</param>
        /// <returns>bool</returns>
        public static bool MoveDirectory(string old_dirpath, string new_dirpath)
        {
            if (string.IsNullOrEmpty(old_dirpath))
            {
                return false;
            }
            string fulloldpath = GetMapPath(old_dirpath);
            string fullnewpath = GetMapPath(new_dirpath);
            if (Directory.Exists(fulloldpath))
            {
                Directory.Move(fulloldpath, fullnewpath);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 返回文件大小KB
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        /// <returns>int</returns>
        public static int GetFileSize(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return 0;
            }
            string fullpath = GetMapPath(_filepath);
            if (File.Exists(fullpath))
            {
                FileInfo fileInfo = new FileInfo(fullpath);
                return ((int)fileInfo.Length) / 1024;
            }
            return 0;
        }

        /// <summary>
        /// 返回文件扩展名，不含“.”
        /// </summary>
        /// <param name="_filepath">文件全名称</param>
        /// <returns>string</returns>
        public static string GetFileExt(string _filepath)
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                return "";
            }
            if (_filepath.LastIndexOf(".") > 0)
            {
                return _filepath.Substring(_filepath.LastIndexOf(".") + 1); //文件扩展名，不含“.”
            }
            return "";
        }

        /// <summary>
        /// 返回文件名，不含路径
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        /// <returns>string</returns>
        public static string GetFileName(string _filefullpath)
        {
            return _filefullpath.Substring(_filefullpath.LastIndexOf(@"/") + 1);
        }

        /// <summary>
        /// 返回文件路径，不含文件名
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        /// <returns>string</returns>
        public static string GetFilePath(string _filefullpath)
        {
            return _filefullpath.Substring(0, _filefullpath.LastIndexOf(@"/"));
        }
        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="_filepath">文件相对路径</param>
        /// <returns>bool</returns>
        public static bool FileExists(string _filepath)
        {
            string fullpath = GetMapPath(_filepath);
            if (File.Exists(fullpath))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region 获得当前绝对路径
        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string strPath)
        {
            //if (Path.IsPathRooted(strPath))
            //{
            //    return strPath;
            //}
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }
        #endregion

        #region 读取或写入cookie
        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = UrlEncode(strValue);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        public static void WriteCookie(string strName, string strValue,int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie.Value = UrlEncode(strValue);
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string key, string strValue)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie[key] = UrlEncode(strValue);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        public static void WriteCookie(string strName, string key, string strValue, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }
            cookie[key] = UrlEncode(strValue);
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
                return UrlDecode(HttpContext.Current.Request.Cookies[strName].Value.ToString());
            return "";
        }

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName, string key)
        {
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null && HttpContext.Current.Request.Cookies[strName][key] != null)
                return UrlDecode(HttpContext.Current.Request.Cookies[strName][key].ToString());

            return "";
        }
        #endregion

        #region URL处理
        /// <summary>
        /// URL字符编码
        /// </summary>
        public static string UrlEncode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            str = str.Replace("'", "");
            return HttpContext.Current.Server.UrlEncode(str);
        }

        /// <summary>
        /// URL字符解码
        /// </summary>
        public static string UrlDecode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            return HttpContext.Current.Server.UrlDecode(str);
        }

        /// <summary>
        /// 组合URL参数
        /// </summary>
        /// <param name="_url">页面地址</param>
        /// <param name="_keys">参数名称</param>
        /// <param name="_values">参数值</param>
        /// <returns>String</returns>
        public static string CombUrlTxt(string _url, string _keys, params string[] _values)
        {
            StringBuilder urlParams = new StringBuilder();
            try
            {
                string[] keyArr = _keys.Split(new char[] { '&' });
                for (int i = 0; i < keyArr.Length; i++)
                {
                    if (!string.IsNullOrEmpty(_values[i]) && _values[i] != "0")
                    {
                        _values[i] = UrlEncode(_values[i]);
                        urlParams.Append(string.Format(keyArr[i], _values) + "&");
                    }
                }
                if (!string.IsNullOrEmpty(urlParams.ToString()) && _url.IndexOf("?") == -1)
                    urlParams.Insert(0, "?");
            }
            catch
            {
                return _url;
            }
            return _url + DelLastChar(urlParams.ToString(), "&");
        }
        #endregion

        #region 删除最后结尾的指定字符后的字符
        /// <summary>
        /// 删除最后结尾的指定字符后的字符
        /// </summary>
        public static string DelLastChar(string str, string strchar)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            if (str.LastIndexOf(strchar) >= 0 && str.LastIndexOf(strchar) == str.Length - 1)
            {
                return str.Substring(0, str.LastIndexOf(strchar));
            }
            return str;
        }
        #endregion

        //public static T CopyTo<T>(this object source) where T : class, new()
        //{
        //    var result = new T();
        //    source.CopyTo(result);
        //    return result;
        //}

        public static void CopyTo<T>(this object source, T target)
            where T : class,new()
        {
            if (source == null)
                return;

            if (target == null)
            {
                throw new Exception("target 对象为空");
                //target = new T();
            }

            foreach (var property in target.GetType().GetProperties())
            {
                PropertyInfo pInfo  = source.GetType().GetProperty(property.Name);
                //↓editby Json 170911↓
                //var propertyValue = pInfo.GetValue(source, null);
                //过滤source没有的属性
                if (pInfo == null)
                {
                    continue;
                }
                //↑editby Json 170911↑
                var propertyValue = pInfo.GetValue(source, null);
                if (propertyValue != null)
                {
                    if (propertyValue.GetType().IsClass)
                    {

                    }
                    //↓editby Json 170911↓
                    if (!pInfo.CanWrite)
                    {
                        //过滤只读属性
                        continue;
                    }
                    //↑editby Json 170911↑
                    target.GetType().InvokeMember(property.Name, BindingFlags.SetProperty, null, target, new object[] { propertyValue });
                }

            }

            foreach (var field in target.GetType().GetFields())
            {
                var fieldValue = source.GetType().GetField(field.Name).GetValue(source);
                if (fieldValue != null)
                {
                    target.GetType().InvokeMember(field.Name, BindingFlags.SetField, null, target, new object[] { fieldValue });
                }
            }
        }

        /// <summary>
        /// 截取
        /// </summary>
        /// <param name="source"></param>
        /// <param name="deepIndex"></param>
        /// <returns></returns>
        public static string SubstringWithDeep(this string source ,char separator, int deepIndex)
        {
            StringBuilder sb = new StringBuilder();
            string[] arry = source.Split(separator);
            for (int i = 0; i < deepIndex + 1; i++)
            {
                sb.Append(arry[i]);
                sb.Append(separator);
            }
            string s = sb.ToString();
            return s.Remove(s.Length - 1, 1);
        }
    }
}