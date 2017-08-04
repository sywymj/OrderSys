using System.Data;
using System.IO;
using System.Text;
using System.Web;

namespace JSNet.Utilities
{
    public class BaseExportCSV
    {
        #region public static StringBuilder GetCSVFormatData(DataTable dataTable) 通过DataTable获得CSV格式数据
        /// <summary>
        /// 通过DataTable获得CSV格式数据
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <returns>CSV字符串数据</returns>
        public static StringBuilder GetCSVFormatData(DataTable dataTable)
        {
            StringBuilder StringBuilder = new StringBuilder();
            // 写出表头
            foreach (DataColumn DataColumn in dataTable.Columns)
            {
                StringBuilder.Append(DataColumn.ColumnName.ToString() + ",");
            }
            StringBuilder.Append("\n");
            // 写出数据
            foreach (DataRowView dataRowView in dataTable.DefaultView)
            {
                foreach (DataColumn DataColumn in dataTable.Columns)
                {
                    StringBuilder.Append(dataRowView[DataColumn.ColumnName].ToString() + ",");
                }
                StringBuilder.Append("\n");
            }
            return StringBuilder;
        }
        #endregion

        #region public static StringBuilder GetCSVFormatData(DataSet dataSet) 通过DataSet获得CSV格式数据
        /// <summary>
        /// 通过DataSet获得CSV格式数据
        /// </summary>
        /// <param name="dataSet">数据权限</param>
        /// <returns>CSV字符串数据</returns>
        public static StringBuilder GetCSVFormatData(DataSet dataSet)
        {
            StringBuilder StringBuilder = new StringBuilder();
            foreach (DataTable dataTable in dataSet.Tables)
            {
                StringBuilder.Append(GetCSVFormatData(dataTable));
            }
            return StringBuilder;
        }
        #endregion

        #region public static void ExportCSV(DataTable dataTable, string fileName) 导出CSV格式文件
        /// <summary>
        /// 导出CSV格式文件
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <param name="fileName">文件名</param>
        public static void ExportCSV(DataTable dataTable, string fileName)
        {
            StreamWriter streamWriter = null;
            streamWriter = new StreamWriter(fileName, false, System.Text.Encoding.GetEncoding("utf-8"));
            streamWriter.WriteLine(GetCSVFormatData(dataTable).ToString());
            streamWriter.Flush();
            streamWriter.Close();
        }
        #endregion

        #region public static void ExportCSV(DataSet dataSet, string fileName) 导出CSV格式文件
        /// <summary>
        /// 导出CSV格式文件
        /// </summary>
        /// <param name="dataSet">数据权限</param>
        /// <param name="fileName">文件名</param>
        public static void ExportCSV(DataSet dataSet, string fileName)
        {
            StreamWriter streamWriter = null;
            streamWriter = new StreamWriter(fileName, false, System.Text.Encoding.GetEncoding("utf-8"));
            streamWriter.WriteLine(GetCSVFormatData(dataSet).ToString());
            streamWriter.Flush();
            streamWriter.Close();
        }
        #endregion

        #region public static void GetResponseCSV(DataTable dataTable, string fileName) 在浏览器中获得CSV格式文件
        /// <summary>
        /// 在浏览器中获得CSV格式文件
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <param name="fileName">输出文件名</param>
        public static void GetResponseCSV(DataTable dataTable, string fileName)
        {
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
            HttpContext.Current.Response.AppendHeader("Content-disposition", "attachment;filename=" + fileName);
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.Write(GetCSVFormatData(dataTable).ToString());
            HttpContext.Current.Response.End();
        }
        #endregion

        #region public static void GetResponseCSV(DataSet dataSet, string fileName) 在浏览器中获得CSV格式文件
        /// <summary>
        /// 在浏览器中获得CSV格式文件
        /// </summary>
        /// <param name="dataSet">数据权限</param>
        /// <param name="fileName">输出文件名</param>
        public static void GetResponseCSV(DataSet dataSet, string fileName)
        {
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
            HttpContext.Current.Response.AppendHeader("Content-disposition", "attachment;filename=" + fileName);
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.Write(GetCSVFormatData(dataSet).ToString());
            HttpContext.Current.Response.End();
            // 读取文件下载
            //String OutTemplateCSV = Server.MapPath("~/DownLoadFiles/ExcelExport/Common/Log/LogGeneral.csv");
            //StreamWriter StreamWriter = new StreamWriter(OutTemplateCSV, false, System.Text.Encoding.GetEncoding("gb2312"));
            //StreamWriter.WriteLine(this.GetCSVFormatData(dataSet).ToString());
            //StreamWriter.Flush();
            //StreamWriter.Close();
            //Response.Redirect("../../../DownLoadFiles/ExcelExport/Common/Log/LogGeneral.csv");
        }
        #endregion 
    }
}