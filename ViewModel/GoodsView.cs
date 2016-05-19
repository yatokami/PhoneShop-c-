using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class GoodsView
    {
        public int GoodsID { get; set; }
        public string GoodsName { get; set; }
        public int Price { get; set; }
        public string GoodsPicture { get; set; }
        public int TypeID { get; set; }
    }
    public class GoodsViews
    {
        public static IEnumerable<GoodsView> data
        {
            get;
            set;
        }
    }
}
