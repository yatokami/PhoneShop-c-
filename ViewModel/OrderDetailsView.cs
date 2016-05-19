using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class OrderDetailsView
    {
        public string picture;
        public int GoodsID { get; set; }
        public string GoodsName { get; set; }
        public string GoodsPicture { get { return picture + ".jpg"; } set { picture = value; } }
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
