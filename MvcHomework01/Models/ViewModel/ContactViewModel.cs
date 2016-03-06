using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcHomework01.Models
{
    public class ContactViewModel
    {
        public ContactViewModelSearch ContactViewModelSearch { get; set; }
        public IEnumerable<客戶聯絡人> 客戶聯絡人s { get; set; }
        public SelectList CustomerList { get; set; }
    }
}