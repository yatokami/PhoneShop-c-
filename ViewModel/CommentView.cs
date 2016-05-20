using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class CommentView
    {
        public string CommentContent { get; set; }
        public string Uname { get; set; }
        public DateTime CommentStartTime { get; set; }
    }

    public class CommentViews
    {
        public static IEnumerable<CommentView> data
        {
            get;
            set;
        }
    }
}
