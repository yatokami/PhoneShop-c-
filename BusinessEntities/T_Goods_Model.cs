using System;
using System.Collections.Generic;
namespace BusinessEntities
{
    public class T_Goods
	{
		public int GoodsID { get; set; }
		public string GoodsName { get; set; }
		public int Price { get; set; }
		public string GoodsPicture { get; set; }
		public int TypeID { get; set; }
		public DateTime AddDate { get; set; }
	}

}
