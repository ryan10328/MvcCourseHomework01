using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcHomework01.Models
{
    public class BankViewModel
    {
        public string BankName { get; set; }
        public IPagedList<客戶銀行資訊> 客戶銀行資訊s { get; set; }
        public int page { get; set; } = 1;
        public string SortDirection { get; set; }
        public string SortColumn { get; set; }
    }
}