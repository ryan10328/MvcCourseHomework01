using System;
using System.Linq;
using System.Collections.Generic;

namespace MvcHomework01.Models
{
    public class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
    {

        public 客戶聯絡人 Get(int Id)
        {
            return All().Where(x => x.Id == Id).FirstOrDefault();
        }

    }

    public interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
    {

    }
}