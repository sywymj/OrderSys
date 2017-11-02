using JSNet.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderSys.Models
{
    public class DDLViewModel
    {
        public string ID { get; set; }
        public string Title { get; set; }
    }

    public class OrderHandleDetailViewModel:OrderHandleDetailEntity
    {
        public List<OrderGoodsRelEntity> OrderGoods { get; set; }
    }

    public class OrderGoodsRelViewModel : OrderGoodsRelEntity { }
}