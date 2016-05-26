using System;
namespace BusinessEntities
{
    public class T_Bulletin
    {
        public int BulletinId { get; set; }
        public string BulletinTitle { get; set; }
        public string BulletinContent { get; set; }
        public DateTime BulletinStarTime { get; set; }
        public string BulletinImg { get; set; }
    }
}
