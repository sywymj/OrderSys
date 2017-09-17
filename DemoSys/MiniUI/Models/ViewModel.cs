using JSNet.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace DemoSys.MiniUI.Models
{
    public abstract class BaseViewDDL
    {
        public string ID { get; set; }
        public string Title { get; set; }
    }
    public abstract class BaseTreeViewDDL : BaseViewDDL
    {
        public string ParentID { get; set; }
    }


    //普通 DDL
    public class ViewDemoDDL : BaseViewDDL { }

    public class ViewDemoCodeDDL : BaseViewDDL { }

    //Tree DDL
    public class ViewDemoTreeDDL : BaseTreeViewDDL { }

    public class ViewDemo : DemoEntity 
    {
        public SonEntity Son { get; set; }
        public string SonIDs { get; set; }
    }

    //ListData ViewModel
    #region 列表用的 ViewModel
    public class DataTableData
    {
        public DataTableData(DataTable dt)
        {
            this.DT = dt;
        }
        public DataTableData(DataTable dt, int count)
            : this(dt)
        {
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
    #endregion
}