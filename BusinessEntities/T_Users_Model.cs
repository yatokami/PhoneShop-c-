using Core;
using System;
using System.ComponentModel.DataAnnotations;
namespace BusinessEntities
{
    public class T_Users
    {
        public int Uid { get; set; }
        [UnameValidation]
        public string Uname { get; set; }
        [StringLength(12, MinimumLength=6, ErrorMessage = "密码长度错误")]
        public string Upwd { get; set; }
        [Required]
        [Display(Name = "性别")]
        public string Sex { get; set; }
        [Required]
        [Display(Name = "真实姓名")]
        public string RealName { get; set; }
        [StringLength(11, MinimumLength = 8, ErrorMessage ="电话号码错误")]
        public string Tel { get; set; }
        [EmailValidation]
        public string Email { get; set; }
        [Required]
        [Display(Name = "地址")]
        public string Address { get; set; }
        [Required]
        [Display(Name = "邮编")]
        public string PostCode { get; set; }
        public DateTime RegisterTime { get; set; }
    }
}
