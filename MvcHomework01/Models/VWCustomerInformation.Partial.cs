using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcHomework01.Models
{

    [MetadataType(typeof(VWCustomerInformationMetaData))]
    public partial class VWCustomerInformation
    {
    }

    public partial class VWCustomerInformationMetaData
    {
        [Display(Name = "客戶名稱")]
        public string CustomerName { get; set; }
        [Display(Name = "銀行帳戶數量")]
        public Nullable<int> BankCount { get; set; }
        [Display(Name = "聯絡人數量")]
        public Nullable<int> ContactCount { get; set; }
    }
}