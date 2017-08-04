using JSNet.DbUtilities;
using System;

namespace JSNet.BaseSys
{
    public partial class BaseSystemInfo
    {
        /// <summary>
        /// 是否加数据库连接
        /// </summary>
        public static bool EncryptDbConnection = false;

        /// <summary>
        /// 用户数据库类别
        /// </summary>
        public static DbTypeName CenterDbType = DbTypeName.SqlServer;

        /// <summary>
        /// 数据库连接（连接串，可能是加密的）
        /// </summary>
        internal static string CenterDbConnection = string.Empty;//"Data Source=localhost;Initial Catalog=Demo_Framework;Integrated Security=SSPI";

        /// <summary>
        /// 数据库连接
        /// </summary>
        public static string CenterDbConnectionString = string.Empty;


        /// <summary>
        /// 业务数据库类别
        /// </summary>
        public static DbTypeName BusinessDbType = DbTypeName.SqlServer;

        /// <summary>
        /// 业务数据库（连接串，可能是加密的）
        /// </summary>
        internal static string BusinessDbConnection = string.Empty;//"Data Source=localhost;Initial Catalog=Demo_Framework1;Integrated Security=SSPI;";

        /// <summary>
        /// 业务数据库
        /// </summary>
        public static string BusinessDbConnectionString = string.Empty;

        /// <summary>
        /// 工作流数据库类别
        /// </summary>
        public static DbTypeName WorkFlowDbType = DbTypeName.SqlServer;

        /// <summary>
        /// 工作流数据库（连接串，可能是加密的）
        /// </summary>
        internal static string WorkFlowDbConnection = string.Empty;//"Data Source=localhost;Initial Catalog=Demo_Framework2;Integrated Security=SSPI;";

        /// <summary>
        /// 工作流数据库
        /// </summary>
        public static string WorkFlowDbConnectionString = string.Empty;

        /// <summary>
        /// 卡务数据库类别
        /// </summary>
        public static DbTypeName KawuDbType = DbTypeName.SqlServer;

        /// <summary>
        /// 卡务数据库（连接串，可能是加密的）
        /// </summary>
        internal static string KawuDbConnection = string.Empty;

        /// <summary>
        /// 卡务数据库
        /// </summary>
        public static string KawuDbConnectionString = string.Empty;


        /// <summary>
        /// 测试数据库类别
        /// </summary>
        public static DbTypeName TestDbType = DbTypeName.SqlServer;

        /// <summary>
        /// 测试数据库（连接串，可能是加密的）
        /// </summary>
        internal static string TestDbHelperConnection = string.Empty;//"Data Source=localhost;Initial Catalog=Demo_Framework;Integrated Security=SSPI;Max Pool Size=5;Connect Timeout=1";

        /// <summary>
        /// 测试数据库
        /// </summary>
        public static string TestDbHelperConnectionString = string.Empty;




    }
}