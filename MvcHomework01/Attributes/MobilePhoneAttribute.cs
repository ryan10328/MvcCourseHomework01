using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MvcHomework01
{
    class MobilePhoneAttribute : DataTypeAttribute
    {
        public MobilePhoneAttribute() : base(DataType.Text) { }

        public override bool IsValid(object value)
        {
            return Regex.IsMatch(value.ToString(), @"\d{4}-\d{6}");
        }
    }
}