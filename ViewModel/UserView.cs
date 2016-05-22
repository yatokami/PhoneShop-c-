using System;
using System.Collections.Generic;

namespace ViewModels
{
    public class UserView
    {
        public string Uname { get; set; }
        public string Sex { get; set; }
        public string RealName { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public DateTime RegisterTime { get; set; }
    }

    public class UserViews
    {
        public static IEnumerable<UserView> data
        {
            get;
            set;
        }
    }
}
