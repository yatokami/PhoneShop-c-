﻿using System.Collections.Generic;

namespace ViewModels
{
    public class OrderDetailsView
    {
        public string picture;
        public int GoodsID { get; set; }
        public string GoodsName { get; set; }
        public string GoodsPicture { get ;  set ;  }
        public int Num { get; set; }
        public int OrderID { get; set; }
        public int TotalMoney { get; set; }
        public int TypeID { get; set; }
    }

    public class OrderDetailsViews
    {
        public static IEnumerable<OrderDetailsView> data
        {
            get;
            set;
        }
    }
}
