using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Core
{
    public class UnameValidation:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(value == null)
            {
                return new ValidationResult("请输入用户名");
            }
            else
            {
                Regex re = new Regex(@"^[a-zA-Z][0-9a-zA-Z]{6,12}$");
                if (!re.IsMatch(value.ToString()))
                {
                    return new ValidationResult("用户名格式错误");
                }
            }
            return ValidationResult.Success;
        }
    }
}