using System;
namespace BusinessEntities
{
    public class T_Comment
	{
		public int CommentID { get; set; }
		public int GoodsID { get; set; }
		public string CommentContent { get; set; }
		public string Uname { get; set; }
		public DateTime CommentStarTime { get; set; }
	}
}
