using Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class T_User
    {
        [UnameValidation]
        public string Uname { get; set; }
        [StringLength(12, MinimumLength = 6, ErrorMessage = "密码长度错误")]
        public string Upwd { get; set; }
    }
}
