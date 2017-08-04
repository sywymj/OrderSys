using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSNet.Utilities
{
    /// <summary>  
    /// 分页接口类  
    /// </summary>  
    public interface IPaging
    {
        /// <summary>  
        /// 是否允许分页  
        /// </summary>  
        bool AllowPaging { get; set; }

        /// <summary>  
        /// 排序  
        /// </summary>  
        string OrderBy { get; set; }

        /// <summary>  
        /// 当前页码  
        /// </summary>  
        int PageIndex { get; set; }

        /// <summary>  
        /// 分页大小  
        /// </summary>  
        int PageSize { get; set; }

        /// <summary>  
        /// 当前页总数  
        /// </summary>  
        int TotalCount { get; set; }

        /// <summary>  
        /// 总页数  
        /// </summary>  
        int TotalPage { get; set; }
    }  
}
