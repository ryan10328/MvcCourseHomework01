using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcHomework01.Models
{
    public class CustomerViewModel
    {
        public CustomerSearchParam CustomerSearchParam { get; set; }
        public IEnumerable<客戶資料> 客戶資料s { get; set; }
    }
}