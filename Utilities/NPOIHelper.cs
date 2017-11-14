using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JSNet.Utilities
{
    public class NPOIHelper
    {
        /// <summary>
        /// 读取excel到datatable中
        /// </summary>
        /// <param name="excelPath">excel地址</param>
        /// <param name="sheetIndex">sheet索引</param>
        /// <returns>成功返回datatable，失败返回null</returns>
        public static DataTable ImportExcel(string excelPath, int sheetIndex)
        {
            IWorkbook workbook = null;//全局workbook
            ISheet sheet;//sheet
            DataTable table = null;
            try
            {
                FileInfo fileInfo = new FileInfo(excelPath);//判断文件是否存在
                if (fileInfo.Exists)
                {
                    FileStream fileStream = fileInfo.OpenRead();//打开文件，得到文件流
                    switch (fileInfo.Extension)
                    {
                        //xls是03，用HSSFWorkbook打开，.xlsx是07或者10用XSSFWorkbook打开
                        case ".xls": workbook = new HSSFWorkbook(fileStream); break;
                        case ".xlsx": workbook = new XSSFWorkbook(fileStream); break;
                        default: break;
                    }
                    fileStream.Close();//关闭文件流
                }
                if (workbook != null)
                {
                    sheet = workbook.GetSheetAt(sheetIndex);//读取到指定的sheet
                    table = new DataTable(sheet.SheetName);//初始化一个table

                    IRow headerRow = sheet.GetRow(0);//获取第一行，一般为表头
                    int cellCount = headerRow.LastCellNum;//得到列数

                    for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                    {
                        DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);//初始化table的列
                        table.Columns.Add(column);
                    }
                    //遍历读取cell
                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                    {
                        NPOI.SS.UserModel.IRow row = sheet.GetRow(i);//得到一行
                        DataRow dataRow = table.NewRow();//新建一个行

                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            ICell cell = row.GetCell(j);//得到cell
                            if (cell == null)//如果cell为null，则赋值为空
                            {
                                dataRow[j] = "";
                            }
                            else
                            {
                                dataRow[j] = row.GetCell(j).ToString().Trim();//否则赋值
                            }
                        }
                        if (dataRow[0].ToString().Equals(string.Empty)) continue;   // 首列为空的都不要了
                        table.Rows.Add(dataRow);//把行 加入到table中
                    }
                }
                return table;
            }
            catch (Exception e)
            {
                return table;
            }
            finally
            {
                //释放资源
                if (table != null) { table.Dispose(); }
                workbook = null;
                sheet = null;
            }
        }

        /// <summary>读取excel
        /// 默认第一行为标头
        /// </summary>
        /// <param name="strFileName">excel文档路径</param>
        /// <returns></returns>
        public static DataTable Import(string strFileName)
        {
            //HSSFWorkbook hssfworkbook;
            IWorkbook workbook = null;//全局workbook
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                FileInfo fileInfo = new FileInfo(strFileName);//判断文件是否存在
                switch (fileInfo.Extension)
                {
                    //xls是03，用HSSFWorkbook打开，.xlsx是07或者10用XSSFWorkbook打开
                    case ".xls": workbook = new HSSFWorkbook(file); break;
                    case ".xlsx": workbook = new XSSFWorkbook(file); break;
                    default: break;
                }
                //hssfworkbook = new HSSFWorkbook(file);
            }
            //var sheet = hssfworkbook.GetSheetAt(0);
            var sheet = workbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
            DataTable dt = new DataTable(sheet.SheetName);

            var headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            for (int j = 0; j < cellCount; j++)
            {
                var cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }

            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }

                dt.Rows.Add(dataRow);
            }
            return dt;
        }

        public static System.Data.DataTable IListToDataTable(IList list)
        {
            System.Data.DataTable Table = new System.Data.DataTable();
            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    Table.Columns.Add(pi.Name, pi.PropertyType);
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    Table.LoadDataRow(array, true);
                }
            }
            return Table;
        }
    }
}
