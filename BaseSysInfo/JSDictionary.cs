﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSNet.BaseSys
{
    /// <summary>
    /// 符合JS习惯的纯字符串字典结构。
    /// </summary>
    public class JSDictionary : Dictionary<string, string>
    {
        public JSDictionary() { }

        public JSDictionary(IDictionary<string, object> dictionary)
        {
            this.AddAll(dictionary);
        }

        public JSDictionary(string key,object value)
        {
            this.Add(key, value);
        }

        /// <summary>
        /// 添加一个新的键值对。空键或者空值的键值对将会被忽略。
        /// </summary>
        /// <param name="key">键名称</param>
        /// <param name="value">键对应的值，目前支持：string, int, long, double, bool, DateTime类型</param>
        public void Add(string key, object value)
        {
            string strValue;

            if (value == null)
            {
                strValue = null;
            }
            else if (value is string)
            {
                strValue = (string)value;
            }
            else if (value is Nullable<DateTime>)
            {
                Nullable<DateTime> dateTime = value as Nullable<DateTime>;
                strValue = dateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else if (value is Nullable<int>)
            {
                strValue = (value as Nullable<int>).Value.ToString();
            }
            else if (value is Nullable<long>)
            {
                strValue = (value as Nullable<long>).Value.ToString();
            }
            else if (value is Nullable<double>)
            {
                strValue = (value as Nullable<double>).Value.ToString();
            }
            else if (value is Nullable<bool>)
            {
                strValue = (value as Nullable<bool>).Value.ToString().ToLower();
            }
            else
            {
                strValue = value.ToString();
            }

            this.Add(key, strValue);
        }

        public new void Add(string key, string value)
        {
            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
            {
                base[key] = value;
            }
        }

        public void AddAll(IDictionary<string, object> dict)
        {
            if (dict != null && dict.Count > 0)
            {
                IEnumerator<KeyValuePair<string, object>> kvps = dict.GetEnumerator();
                while (kvps.MoveNext())
                {
                    KeyValuePair<string, object> kvp = kvps.Current;
                    Add(kvp.Key, kvp.Value);
                }
            }
        }
    }

    public static class ListTools
    {
        public static JSDictionary ToJSDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            JSDictionary dic = new JSDictionary();
            foreach (TSource element in source)
            {
                dic.Add(keySelector(element).ToString(), elementSelector(element));
            }
            return dic;
        }
    } 
}
