using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class WellBadView
    {
        public string well;
        public string Well { get { return "赞" + well; } set { well = value; } }

        public string bad;
        public string Bad { get { return "踩" + bad; } set { bad = value; } }
    }
}
