using System;
using System.Collections.Generic;

namespace ViewModels
{
    public class CartView
    {
        string pic;
        public int GoodsID { get; set; }
        public int BuyID { get; set; }
        public int TypeID { get; set; }
        public int Num { get; set; }
        public int Price { get; set; }
        public DateTime BuyDate { get; set; }
        public string GoodsPicture { get; set; }
        public string GoodsName { get; set; }
    }
    public class CartViews
    {
        public static IEnumerable<CartView> data
        {
            get;
            set;
        }
    }
}
