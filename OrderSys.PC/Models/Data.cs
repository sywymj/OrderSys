using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace OrderSys.Admin
{
    public class DataTableData
    {
        public DataTableData(DataTable dt, int count)
        {
            this.DT = dt;
            this.Count = count;
        }
        public DataTable DT { get; set; }
        public int Count { get; set; }
    }

    public class ListData<T>
    {
        public ListData(List<T> list)
        {
            this.List = list;
        }
    
        public ListData(List<T> list, int count)
        {
            this.List = list;
            this.Count = count;
        }
        public List<T> List { get; set; }
        public int Count { get; set; }
    }
}