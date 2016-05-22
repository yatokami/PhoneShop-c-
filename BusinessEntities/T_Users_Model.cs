using System;
namespace BusinessEntities
{
    public class T_Users
    {
        public int Uid { get; set; }
        public string Uname { get; set; }
        public string Upwd { get; set; }
        public string Sex { get; set; }
        public string RealName { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public DateTime RegisterTime { get; set; }
    }
}
