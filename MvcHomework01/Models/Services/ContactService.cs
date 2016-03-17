using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Web.Mvc;
using System.Collections.Generic;
using System;

namespace MvcHomework01.Models
{
    public interface ICRMService
    {
        ContactViewModel GetContactData();
        ContactViewModel GetContactData(ContactViewModel model);
        客戶聯絡人 GetContact(int id);
        IEnumerable<客戶聯絡人> GetAllContact();
        客戶資料 GetCustomer(int id);
        IEnumerable<客戶資料> GetAllCustomer();
    }

    public class CRMService : ICRMService
    {

        客戶聯絡人Repository ContactRepo = RepositoryHelper.Get客戶聯絡人Repository();
        客戶資料Repository CustomerRepo = RepositoryHelper.Get客戶資料Repository();

        public ContactViewModel GetContactData()
        {
            var 客戶聯絡人 = ContactRepo.All().Include(x => x.客戶資料).Where(x => x.是否刪除 == false);

            ContactViewModel model = new ContactViewModel()
            {
                ContactViewModelSearch = null,
                客戶聯絡人s = 客戶聯絡人.ToList(),
                CustomerList = new SelectList(CustomerRepo.All().Where(x => x.是否刪除 == false), "Id", "客戶名稱")
            };

            return model;
        }

        public ContactViewModel GetContactData(ContactViewModel model)
        {
            if (model.ContactViewModelSearch.CustomerId.HasValue)
            {
                model.CustomerList = new SelectList(CustomerRepo.All().Where(x => x.是否刪除 == false), "Id", "客戶名稱", model.ContactViewModelSearch.CustomerId.Value);
            }
            else
            {
                model.CustomerList = new SelectList(CustomerRepo.All().Where(x => x.是否刪除 == false), "Id", "客戶名稱");
            }

            var 客戶聯絡人 = ContactRepo.All().Include(客 => 客.客戶資料).Where(x => x.是否刪除 == false);
            if (!string.IsNullOrEmpty(model.ContactViewModelSearch.Name))
            {
                客戶聯絡人 = 客戶聯絡人.Where(x => x.姓名.Contains(model.ContactViewModelSearch.Name));
            }
            if (!string.IsNullOrEmpty(model.ContactViewModelSearch.Mobile))
            {
                客戶聯絡人 = 客戶聯絡人.Where(x => x.手機.Contains(model.ContactViewModelSearch.Mobile));
            }
            if (model.ContactViewModelSearch.CustomerId.HasValue)
            {
                客戶聯絡人 = 客戶聯絡人.Where(x => x.客戶Id == model.ContactViewModelSearch.CustomerId.Value);
            }

            model.客戶聯絡人s = 客戶聯絡人;

            return model;
        }

        public 客戶聯絡人 GetContact(int id)
        {
            return ContactRepo.Get(id);
        }

        public IEnumerable<客戶聯絡人> GetAllContact()
        {
            return ContactRepo.All();
        }

        public 客戶資料 GetCustomer(int id)
        {
            return CustomerRepo.Get(id);
        }

        public IEnumerable<客戶資料> GetAllCustomer()
        {
            return CustomerRepo.All();
        }

    }
}