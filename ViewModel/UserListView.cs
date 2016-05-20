using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace ViewModels
{
    public class UserListView
    {
        public string UserName { get; set; }
        public GoodsView Goods { get; set; }
        public WellBadView WellBad { get; set; }
        public List<UserView> Users { get; set; }
        public List<GoodsTypeView> GoodsTypeView { get; set; }
        public List<GoodsView> GoodView { get; set; }
        //PageList分页效果
        public PageList<GoodsView> GoodsView { get; set; }
        public PageList<CartView> CartViews { get; set; }
        public PageList<OrderView> OrderViews { get; set; }
        public PageList<OrderDetailsView> OrderDetailsViews { get; set; }
        public PageList<CommentView> CommentViews { get; set; }

    }
}
