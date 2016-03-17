using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcHomework01.Models;

namespace MvcHomework01.Controllers
{
    public class ContactController : Controller
    {
        //private CrmEntities db = new CrmEntities();
        客戶聯絡人Repository contactRepo = RepositoryHelper.Get客戶聯絡人Repository();
        客戶資料Repository customerRepo = RepositoryHelper.Get客戶資料Repository();

        ICRMService crmService = new CRMService();

        // GET: Contact
        public ActionResult Index()
        {
            var model = crmService.GetContactData();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            crmService.GetContactData(model);

            return View(model);
        }

        // GET: Contact/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            客戶聯絡人 客戶聯絡人 = crmService.GetContact(id.Value);

            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }

            return View(客戶聯絡人);
        }

        // GET: Contact/Create
        public ActionResult Create()
        {
            ViewBag.客戶Id = new SelectList(crmService.GetAllCustomer(), "Id", "客戶名稱");
            return View();
        }

        // POST: Contact/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                var customer = crmService.GetCustomer(客戶聯絡人.客戶Id);

                if (customer != null)
                {
                    // 若Email存在且還沒刪除 視為尚存在
                    if (customer.客戶聯絡人.Any(x => x.Email == 客戶聯絡人.Email && x.是否刪除 == false))
                    {
                        ModelState.AddModelError(string.Empty, "你指定的客戶中已存在此客戶聯絡人的Email!!!!");
                        ViewBag.客戶Id = new SelectList(crmService.GetAllCustomer(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
                        return View(客戶聯絡人);
                    }
                    contactRepo.Add(客戶聯絡人);
                }

                contactRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(crmService.GetAllCustomer(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: Contact/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = crmService.GetContact(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(crmService.GetAllCustomer(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // POST: Contact/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話,是否刪除")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                var db = contactRepo.UnitOfWork.Context as CrmEntities;
                db.Entry(客戶聯絡人).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.客戶Id = new SelectList(crmService.GetAllCustomer(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: Contact/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = crmService.GetContact(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // POST: Contact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var db = contactRepo.UnitOfWork.Context as CrmEntities;

            客戶聯絡人 客戶聯絡人 = contactRepo.Get(id);
            客戶聯絡人.是否刪除 = true;
            contactRepo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                var db = contactRepo.UnitOfWork.Context as CrmEntities;
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
