using System.Collections;
using System.Collections.Generic;

namespace MvcHomework01.Models
{
    public class CustomerSearchParam
    {
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public int? CountryId { get; set; }
    }
}