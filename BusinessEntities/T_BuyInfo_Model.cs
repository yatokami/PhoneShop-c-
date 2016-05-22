using System;
namespace BusinessEntities
{
    public class T_BuyInfo
    {
        public int BuyID { get; set; }
        public int Uid { get; set; }
        public string Uname { get; set; }
        public int GoodsID { get; set; }
        public int Num { get; set; }
        public int Price { get; set; }
        public DateTime BuyDate { get; set; }
    }
}
