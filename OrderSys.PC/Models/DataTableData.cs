﻿using System;
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
}