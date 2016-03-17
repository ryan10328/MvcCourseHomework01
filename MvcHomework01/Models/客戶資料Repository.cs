using System;
using System.Linq;
using System.Collections.Generic;

namespace MvcHomework01.Models
{
    public class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
    {
        public 客戶資料 Get(int Id)
        {
            return All().Where(x => x.Id == Id).FirstOrDefault();
        }

        public IEnumerable<Country> GetCountries()
        {
            yield return new Country() { Id = 0, Name = "台灣" };
            yield return new Country() { Id = 1, Name = "日本" };
            yield return new Country() { Id = 2, Name = "韓國" };
            yield return new Country() { Id = 3, Name = "中國" };
            yield return new Country() { Id = 4, Name = "美國" };
            yield return new Country() { Id = 5, Name = "英國" };
        }

        public CustomerViewModel GetIndexData(CustomerViewModel model)
        {
            var 客戶資料 = All().Where(x => x.是否刪除 == false);

            if (!string.IsNullOrEmpty(model.CustomerSearchParam.CustomerName))
            {
                客戶資料 = 客戶資料.Where(x => x.客戶名稱.Contains(model.CustomerSearchParam.CustomerName));
            }
            if (!string.IsNullOrEmpty(model.CustomerSearchParam.Address))
            {
                客戶資料 = 客戶資料.Where(x => x.地址.Contains(model.CustomerSearchParam.Address));
            }
            if (model.CustomerSearchParam.CountryId.HasValue)
            {
                客戶資料 = 客戶資料.Where(x => x.客戶國家Id == model.CustomerSearchParam.CountryId.Value);
            }

            model.客戶資料s = 客戶資料;

            return model;
        }
    }

    public interface I客戶資料Repository : IRepository<客戶資料>
    {

    }
}