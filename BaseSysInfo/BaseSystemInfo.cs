using System;

namespace JSNet.BaseSys
{
    public partial class BaseSystemInfo
    {
        

        /// <summary>
        /// 时间格式
        /// </summary>
        public static string TimeFormat = "HH:mm:ss";

        /// <summary>
        /// 日期短格式
        /// </summary>
        public static string DateFormat = "yyyy-MM-dd";

        /// <summary>
        /// 日期长格式
        /// </summary>
        public static string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// 是否登记异常
        /// </summary>
        public static bool LogException = true;

        /// <summary>
        /// 是否登记数据库操作
        /// </summary>
        public static bool LogSQL = false;

        /// <summary>
        /// 是否登记到 Windows 系统异常中
        /// </summary>
        public static bool EventLog = false;

        /// <summary>
        /// 是否进行日志记录
        /// </summary>
        public static bool RecordLog = true;

        /// <summary>
        /// 当前网站的安装地址
        /// </summary>
        public static string StartupPath = System.Web.HttpRuntime.AppDomainAppPath.ToString();

        /// <summary>
        /// Xml配置文件文件名
        /// </summary>
        public static string XmlFileName = "Config.xml";

        /// <summary>
        /// Xml配置文件文件夹路径
        /// </summary>
        public static string XmlDirectoryName = StartupPath + "Xml\\";


        /// <summary>
        /// RegistryKey、Configuration、UserConfig 注册表或者配置文件读取参数
        /// </summary>
        public static ConfigurationCategory ConfigurationFrom = ConfigurationCategory.Configuration;


        /// <summary>
        /// 当前版本
        /// </summary>
        public static string Version = XmlConfigHelper.GetValue("Version");

        /// <summary>
        /// 当前客户公司名称
        /// </summary>
        public static string CustomerCompanyName = XmlConfigHelper.GetValue("CustomerCompanyName");

        /// <summary>
        /// 当前软件Id
        /// </summary>
        public static string SoftName = XmlConfigHelper.GetValue("SoftName");

        /// <summary>
        /// 软件的名称
        /// </summary>
        public static string SoftFullName = XmlConfigHelper.GetValue("SoftFullName");

        /// <summary>
        /// 这里是设置，读取哪个系统的菜单
        /// </summary>
        public static string SystemCode = XmlConfigHelper.GetValue("SystemCode");

        /// <summary>
        /// 这里是设置，读取哪个子系统的菜单
        /// </summary>
        public static string RootMenuCode = XmlConfigHelper.GetValue("RootMenuCode");

        /// <summary>
        /// 上传文件路径
        /// </summary>
        public static string UploadDirectory = XmlConfigHelper.GetValue("UploadDirectory");

        /// <summary>
        /// 目前使用者選擇的語系
        /// </summary>
        public static string CurrentLanguage = XmlConfigHelper.GetValue("CurrentLanguage");

        /// <summary>
        /// 系统默认密码
        /// </summary>
        public static string DefaultPassword = XmlConfigHelper.GetValue("DefaultPassword");

        /// <summary>
        /// 遠端調用Service使用者名稱（提高系統安全性）
        /// </summary>
        public static string ServiceUserName = XmlConfigHelper.GetValue("ServiceUserName");

        /// <summary>
        /// 遠端調用Service密碼（提高系統安全性）
        /// </summary>
        public static string ServicePassword = XmlConfigHelper.GetValue("ServicePassword");

        /// <summary>
        /// 客戶端加密儲存密碼
        /// </summary>
        public static bool ClientEncryptPassword = XmlConfigHelper.GetBoolValue("ClientEncryptPassword");

        /// <summary>
        /// 服务器端加密存储密码
        /// </summary>
        public static bool ServerEncryptPassword = XmlConfigHelper.GetBoolValue("ServerEncryptPassword");
    }
}