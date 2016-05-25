
using System.Collections.Generic;
namespace ViewModels
{
    public class AdminListView
    {
        public string AdminUser { get; set; }
        public string Uname { get; set; }
        public PageList<UserView> Users { get; set; }
        public List<GoodsTypeView> GoodsType { get; set; }
        public PageList<GoodsView> GoodsView { get; set; }
        public PageList<AdminOrderView> AdminOrderViews { get; set; }
    }
}
