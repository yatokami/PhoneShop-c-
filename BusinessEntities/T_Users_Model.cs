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
        [StringLength(12, MinimumLength=6, ErrorMessage = "���볤�ȴ���")]
        public string Upwd { get; set; }
        [Required]
        [Display(Name = "�Ա�")]
        public string Sex { get; set; }
        [Required]
        [Display(Name = "��ʵ����")]
        public string RealName { get; set; }
        [StringLength(11, MinimumLength = 8, ErrorMessage ="�绰�������")]
        public string Tel { get; set; }
        [EmailValidation]
        public string Email { get; set; }
        [Required]
        [Display(Name = "��ַ")]
        public string Address { get; set; }
        [Required]
        [Display(Name = "�ʱ�")]
        public string PostCode { get; set; }
        public DateTime RegisterTime { get; set; }
    }
}
