using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace JSNet.BaseSys
{
    /// <summary>
    /// 访问用户XML配置文件的类
    /// </summary>
    public class XmlConfigHelper
    {
        public static string SelectPath = "//appSettings/add";

        public static string ConfigFilePath
        {
            get
            {
                return BaseSystemInfo.XmlDirectoryName + BaseSystemInfo.XmlFileName;
            }
        }

        #region public static Dictionary<String, String> GetLogOnTo() 获取配置文件选项
        /// <summary>
        /// 获取配置文件选项
        /// </summary>
        /// <returns>配置文件设置</returns>
        public static Dictionary<string, string> GetLogOnTo()
        {
            Dictionary<string, string> returnValue = new Dictionary<string, string>();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(ConfigFilePath);
            XmlNodeList xmlNodeList = xmlDocument.SelectNodes(SelectPath);
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (xmlNode.Attributes["key"].Value.ToUpper().Equals("LogOnTo".ToUpper()))
                {
                    returnValue.Add(xmlNode.Attributes["value"].Value, xmlNode.Attributes["dispaly"].Value);
                }
            }
            return returnValue;
        }
        #endregion      


        public static bool ExistsConfigFile()
        {
            bool returnValue = false;
            if (System.IO.File.Exists(ConfigFilePath))
            {
                returnValue = true;
            }
            return returnValue;
        }
        public static bool ExistsValue(string key)
        {
            return !string.IsNullOrEmpty(GetValue(key));
        }

        /// <summary>
        /// 读取配置项
        /// </summary>
        /// <param name="key"></param>
        /// <returns>返回字符串数组</returns>
        public static string[] GetOptions(string key)
        {
            string option = string.Empty;
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(ConfigFilePath);
            option = GetOption(xmlDocument, SelectPath, key);
            return option.Split(',');
        }

        #region public static string GetOption(XmlDocument xmlDocument, string selectPath, string key) 设置配置项
        /// <summary>
        /// 读取配置项
        /// </summary>
        /// <param name="xmlDocument">配置文件</param>
        /// <param name="selectPath">查询条件</param>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public static string GetOption(XmlDocument xmlDocument, string selectPath, string key)
        {
            string returnValue = string.Empty;
            XmlNodeList xmlNodeList = xmlDocument.SelectNodes(selectPath);
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (xmlNode.Attributes["key"].Value.ToUpper().Equals(key.ToUpper()))
                {
                    if (xmlNode.Attributes["Options"] != null)
                    {
                        returnValue = xmlNode.Attributes["Options"].Value;
                        break;
                    }
                }
            }
            return returnValue;
        }
        #endregion


        public static bool GetBoolValue(string key)
        {
            return (string.Compare(GetValue(key), "TRUE", true, CultureInfo.CurrentCulture) == 0);
        }

        #region public static string GetValue(string key) 读取配置项
        /// <summary>
        /// 读取配置项
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public static string GetValue(string key)
        {
            return GetValue(ConfigFilePath, SelectPath, key);
        }
        #endregion

        #region public static string GetValue(string fileName, string key) 读取配置项
        /// <summary>
        /// 读取配置项
        /// </summary>
        /// <param name="fileName">配置文件</param>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public static string GetValue(string fileName, string key)
        {
            return GetValue(fileName, SelectPath, key);
        }
        #endregion

        #region public static string GetValue(string fileName, string selectPath, string key) 设置配置项
        /// <summary>
        /// 读取配置项
        /// </summary>
        /// <param name="fileName">配置文件</param>
        /// <param name="selectPath">查询条件</param>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public static string GetValue(string fileName, string selectPath, string key)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);
            return GetValue(xmlDocument, selectPath, key);
        }
        #endregion

        #region public static string GetValue(XmlDocument xmlDocument, string key) 读取配置项
        /// <summary>
        /// 读取配置项
        /// </summary>
        /// <param name="xmlDocument">配置文件</param>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public static string GetValue(XmlDocument xmlDocument, string key)
        {
            return GetValue(xmlDocument, SelectPath, key);
        }
        #endregion

        #region public static string GetValue(XmlDocument xmlDocument, string selectPath, string key) 设置配置项
        /// <summary>
        /// 读取配置项
        /// </summary>
        /// <param name="xmlDocument">配置文件</param>
        /// <param name="selectPath">查询条件</param>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public static string GetValue(XmlDocument xmlDocument, string selectPath, string key)
        {
            string returnValue = string.Empty;
            XmlNodeList xmlNodeList = xmlDocument.SelectNodes(selectPath);
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (xmlNode.Attributes["key"].Value.ToUpper().Equals(key.ToUpper()))
                {
                    returnValue = xmlNode.Attributes["value"].Value;
                    break;
                }
            }
            return returnValue;
        }
        #endregion

        #region public static void GetConfig() 读取配置文件
        /// <summary>
        /// 读取配置文件
        /// </summary>
        public static void GetConfig()
        {
            if (ExistsConfigFile())
            {
                GetConfig(ConfigFilePath);
            }
        }
        #endregion

        #region public static void GetConfig(string fileName) 从指定的文件读取配置项
        /// <summary>
        /// 从指定的文件读取配置项
        /// </summary>
        /// <param name="fileName">配置文件</param>
        public static void GetConfig(string fileName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);

            #region 客户信息配置
            if (ExistsValue("Version"))
            {
                BaseSystemInfo.Version = GetValue(xmlDocument, "Version");
            }
            if (ExistsValue("CustomerCompanyName"))
            {
                BaseSystemInfo.CustomerCompanyName = GetValue(xmlDocument, "CustomerCompanyName");
            }
            if (ExistsValue("SoftName"))
            {
                BaseSystemInfo.SoftName = GetValue(xmlDocument, "SoftName");
            }
            if (ExistsValue("SoftFullName"))
            {
                BaseSystemInfo.SoftFullName = GetValue(xmlDocument, "SoftFullName");
            }
            if (ExistsValue("SystemCode"))
            {
                BaseSystemInfo.SystemCode = GetValue(xmlDocument, "SystemCode");
            }
            if (ExistsValue("RootMenuCode"))
            {
                BaseSystemInfo.RootMenuCode = GetValue(xmlDocument, "RootMenuCode");
            }
            if (ExistsValue("UploadDirectory"))
            {
                BaseSystemInfo.UploadDirectory = GetValue(xmlDocument, "UploadDirectory");
            }
            if (ExistsValue("CurrentLanguage"))
            {
                BaseSystemInfo.CurrentLanguage = GetValue(xmlDocument, "CurrentLanguage");
            }
            #endregion

            #region 安全相关的配置
            if (ExistsValue("DefaultPassword"))
            {
                BaseSystemInfo.DefaultPassword = GetValue(xmlDocument, "DefaultPassword");
            }
            if (ExistsValue("ServiceUserName"))
            {
                BaseSystemInfo.ServiceUserName = GetValue(xmlDocument, "ServiceUserName");
            }
            if (ExistsValue("ServicePassword"))
            {
                BaseSystemInfo.ServicePassword = GetValue(xmlDocument, "ServicePassword");
            }
            if (ExistsValue("ServerEncryptPassword"))
            {
                BaseSystemInfo.ServerEncryptPassword = (string.Compare(GetValue(xmlDocument, "ServerEncryptPassword"), "TRUE", true, CultureInfo.CurrentCulture) == 0);
            }
            if (ExistsValue("ClientEncryptPassword"))
            {
                BaseSystemInfo.ClientEncryptPassword = (string.Compare(GetValue(xmlDocument, "ClientEncryptPassword"), "TRUE", true, CultureInfo.CurrentCulture) == 0);
            } 
            #endregion

            #region 数据库相关的配置
            BaseSystemInfo.TestDbHelperConnectionString = GetValue(xmlDocument, "TestDbHelperConnection");
            #endregion
            
        }
        #endregion

        public static bool SetValue(XmlDocument xmlDocument, string key, string keyValue)
        {
            return SetValue(xmlDocument, SelectPath, key, keyValue);
        }

        public static bool SetValue(XmlDocument xmlDocument, string selectPath, string key, string keyValue)
        {
            bool returnValue = false;
            XmlNodeList xmlNodeList = xmlDocument.SelectNodes(selectPath);
            foreach (XmlNode xmlNode in xmlNodeList)
            {
                if (xmlNode.Attributes["key"].Value.ToUpper().Equals(key.ToUpper()))
                {
                    xmlNode.Attributes["value"].Value = keyValue;
                    returnValue = true;
                    break;
                }
            }
            return returnValue;
        }

        #region public static void SaveConfig() 保存配置文件
        /// <summary>
        /// 保存配置文件
        /// </summary>
        public static void SaveConfig()
        {
            if (ExistsConfigFile())
            {
                SaveConfig(ConfigFilePath);
            }
        }
        #endregion

        #region public static void SaveConfig(string fileName) 保存到指定的文件
        /// <summary>
        /// 保存到指定的文件
        /// </summary>
        /// <param name="fileName">配置文件</param>
        public static void SaveConfig(string fileName)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);

            #region 客户信息配置
            SetValue(xmlDocument, "Version", BaseSystemInfo.Version);
            SetValue(xmlDocument, "CustomerCompanyName", BaseSystemInfo.CustomerCompanyName);
            SetValue(xmlDocument, "SoftName", BaseSystemInfo.SoftName);
            SetValue(xmlDocument, "SoftFullName", BaseSystemInfo.SoftFullName);
            SetValue(xmlDocument, "SystemCode", BaseSystemInfo.SystemCode);
            SetValue(xmlDocument, "RootMenuCode", BaseSystemInfo.RootMenuCode);
            SetValue(xmlDocument, "UploadDirectory", BaseSystemInfo.UploadDirectory);

            SetValue(xmlDocument, "CurrentLanguage", BaseSystemInfo.CurrentLanguage);
            SetValue(xmlDocument, "TimeFormat", BaseSystemInfo.TimeFormat);
            SetValue(xmlDocument, "DateFormat", BaseSystemInfo.DateFormat);
            SetValue(xmlDocument, "DateTimeFormat", BaseSystemInfo.DateTimeFormat);
            SetValue(xmlDocument, "LogException", BaseSystemInfo.LogException.ToString());
            SetValue(xmlDocument, "LogSQL", BaseSystemInfo.LogSQL.ToString());
            SetValue(xmlDocument, "EventLog", BaseSystemInfo.EventLog.ToString());
            SetValue(xmlDocument, "RecordLog", BaseSystemInfo.RecordLog.ToString());
            #endregion

            #region 安全相关的配置
            SetValue(xmlDocument, "DefaultPassword", BaseSystemInfo.DefaultPassword.ToString());
            SetValue(xmlDocument, "ServiceUserName", BaseSystemInfo.ServiceUserName.ToString());
            SetValue(xmlDocument, "ServicePassword", BaseSystemInfo.ServicePassword.ToString());
            SetValue(xmlDocument, "ServerEncryptPassword", BaseSystemInfo.ServerEncryptPassword.ToString());
            SetValue(xmlDocument, "ClientEncryptPassword", BaseSystemInfo.ClientEncryptPassword.ToString()); 
            #endregion
            
            #region 保存数据库配置
            SetValue(xmlDocument, "TestDbHelperConnection", BaseSystemInfo.TestDbHelperConnection); 
            #endregion


            xmlDocument.Save(fileName);
        }
        #endregion
    }
}