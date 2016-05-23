
using System.Collections.Generic;
namespace ViewModels
{
    public class AdminListView
    {
        public string AdminUser { get; set; }
        public PageList<UserView> Users { get; set; }
        public List<GoodsTypeView> GoodsType { get; set; }
    }
}
