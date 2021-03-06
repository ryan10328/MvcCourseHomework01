using System;
using System.Linq;
using System.Collections.Generic;

namespace MvcHomework01.Models
{
    public class 客戶銀行資訊Repository : EFRepository<客戶銀行資訊>, I客戶銀行資訊Repository
    {
        public 客戶銀行資訊 Get(int id)
        {
            return All().Where(x => x.Id == id).FirstOrDefault();
        }
    }

    public interface I客戶銀行資訊Repository : IRepository<客戶銀行資訊>
    {

    }
}