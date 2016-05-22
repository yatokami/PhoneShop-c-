using System;
using System.Collections.Generic;

namespace ViewModels
{
    public class OrderView
    {
        public int OrderID { get; set; }
        public int TotalMoney { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public int Num { get; set; }
    }
    public class OrderViews
    {
        public static IEnumerable<OrderView> data
        {
            get;
            set;
        }
    }
}
