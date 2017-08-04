namespace JSNet.BaseSys
{
    public enum ConfigurationCategory
    {
        /// <summary>
        /// 从注册表 读取配置
        /// </summary>
        RegistryKey,    // 从注册表读取
        /// <summary>
        /// 从 XML配置文件 读取配置
        /// </summary>
        Configuration,  // 从配置文件读取
        /// <summary>
        /// 从 web.config 读取配置
        /// </summary>
        UserConfig      // 用户的配置文件
    }
}