using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSNet.Utilities
{
    /// <summary>  
    /// 分页基类  
    /// </summary>  
    public class Paging : IPaging
    {
        //默认页码  
        private const int _pageIndex = 1;

        //默认页数  
        private const int _pageSize = 50;

        /// <summary>  
        /// 构造函数  
        /// </summary>  
        public Paging()
        {
            this.PageIndex = _pageIndex;
            this.PageSize = _pageSize;
        }

        public Paging(int pageIndex, int pageSize)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
        }

        public Paging(bool allowPaging) 
        {
            AllowPaging = allowPaging;
        }

        public Paging(int pageIndex, int pageSize, string orderby)
            :this(pageIndex,pageSize)
        {
            orderby = orderby.Trim();
            if (orderby.Split(' ').Length > 0)
            {
                this.SortField = orderby.Split(' ')[0];
                this.SortOrder = orderby.Split(' ')[1];
                this.OrderBy = orderby;
            }
        }

        public Paging(int pageIndex, int pageSize, string sortField, string sortOrder)
            : this(pageIndex, pageSize)
        {
            this.SortField = sortField;
            this.SortOrder = sortOrder;

            if (string.IsNullOrEmpty(this.SortField) || string.IsNullOrEmpty(this.SortOrder))
            {
                this.SortField = this.SortOrder = "";
            }
            string orderby = this.SortField + " " + this.SortOrder;
            this.OrderBy = orderby.Trim();
        }

        /// <summary>  
        /// 是否允许分页  
        /// </summary>  
        public bool AllowPaging { get; set; }

        public string SortField { get; set; }

        public string SortOrder { get; set; }
        /// <summary>  
        /// 排序  
        /// </summary>  
        public string OrderBy { get; set; }

        /// <summary>  
        /// 当前页码  
        /// </summary>  
        public int PageIndex { get; set; }

        /// <summary>  
        /// 分页大小  
        /// </summary>  
        public int PageSize { get; set; }

        /// <summary>  
        /// 当前页总数  
        /// </summary>  
        public int TotalCount { get; set; }

        /// <summary>  
        /// 总页数  
        /// </summary>  
        public int TotalPage { get; set; }
    }  
}
