using System;
using System.Collections.Generic;
using System.Reflection;

namespace JSNet.Utilities
{
    public static class EnumExtensions
    {
        public static string ToDescription(this Enum enumeration) 
        {
            Type type = enumeration.GetType();
            MemberInfo[] memInfo = type.GetMember(enumeration.ToString());
            if (null != memInfo && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(EnumDescription), false);
                if (null != attrs && attrs.Length > 0)
                {
                    return ((EnumDescription)attrs[0]).Text;
                }
            }
            return enumeration.ToString(); 
        }

        public static Dictionary<int, string> GetEnumDescription<T>() 
        {
            if (!typeof(T).IsEnum) throw new Exception("参数类型不正确");
            Dictionary<int, string> dic = new Dictionary<int, string>();
            FieldInfo[] fieldinfo = typeof(T).GetFields();
            foreach (FieldInfo item in fieldinfo)
            {
                Object[] obj = item.GetCustomAttributes(typeof(EnumDescription), false);
                if (obj != null && obj.Length != 0)
                {
                    EnumDescription des = (EnumDescription)obj[0];
                    T type = (T)Enum.Parse(typeof(T), item.Name);
                    dic.Add(Convert.ToInt32(type), des.Text);
                }
            }
            return dic;
        }
    }
}