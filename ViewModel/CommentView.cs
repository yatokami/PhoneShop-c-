using System.Collections.Generic;

namespace ViewModels
{
    public class CommentView
    {
        public string CommentContent { get; set; }
        public string Uname { get; set; }
        public string CommentStartTime { get; set; }
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
