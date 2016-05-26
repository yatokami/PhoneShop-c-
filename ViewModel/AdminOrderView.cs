using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class AdminOrderView
    {
        public int OrderID { get; set; }
        public string Uname { get; set; }
        public int TotalMoney { get; set; }
        public string OrederDate { get; set; }
        public string OrderStatus { get; set; }
        public string PayType { get; set; }
        public string IsPayed { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverTel { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string Email { get; set; }
        public int Num { get; set; }
    }

    public class AdminOrderViews
    {
        public static IEnumerable<AdminOrderView> data
        {
            get; set;
        }
    }
}
