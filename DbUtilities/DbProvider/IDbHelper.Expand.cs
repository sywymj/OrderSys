using System.Data;

namespace JSNet.DbUtilities
{
    public interface IDbHelperExpand
    {
        /// <summary>
        /// 利用Net SqlBulkCopy 批量导入数据库,速度超快
        /// </summary>
        /// <param name="dataTable">源内存数据表</param>
        void SqlBulkCopyData(DataTable dataTable);
    }
}
