using System;
namespace BusinessEntities
{
    public class T_Order
	{
		public int OrderID { get; set; }
		public int Uid { get; set; }
		public int TotalMoney { get; set; }
		public DateTime OrderDate { get; set; }
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
}
