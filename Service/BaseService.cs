using CodeEngine.Framework.QueryBuilder.Enums;
using JSNet.BaseSys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JSNet.Service
{
    public class BaseService
    {
        public Sorting ConvertToSort(string sortOrder)
        {
            switch (sortOrder.ToLower())
            {
                case "asc":
                    return Sorting.Ascending;
                case "desc":
                    return Sorting.Descending;
                case "":
                    return Sorting.Ascending;
                default:
                    throw new JSException(string.Format("sortOrder值“{0}”不合法！", sortOrder));
            }
        }
    }
}
