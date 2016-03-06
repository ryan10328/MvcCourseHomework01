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
        private CrmEntities db = new CrmEntities();

        // GET: Contact
        public ActionResult Index()
        {
            var 客戶聯絡人 = db.客戶聯絡人.Include(客 => 客.客戶資料).Where(x => x.是否刪除 == false);

            ContactViewModel model = new ContactViewModel()
            {
                ContactViewModelSearch = null,
                客戶聯絡人s = 客戶聯絡人.ToList(),
                CustomerList = new SelectList(db.客戶資料.Where(x => x.是否刪除 == false), "Id", "客戶名稱")
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ContactViewModel model)
        {
            if (model.ContactViewModelSearch.CustomerId.HasValue)
            {
                model.CustomerList = new SelectList(db.客戶資料.Where(x => x.是否刪除 == false), "Id", "客戶名稱", model.ContactViewModelSearch.CustomerId.Value);
            }
            else
            {
                model.CustomerList = new SelectList(db.客戶資料.Where(x => x.是否刪除 == false), "Id", "客戶名稱");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var 客戶聯絡人 = db.客戶聯絡人.Include(客 => 客.客戶資料).Where(x => x.是否刪除 == false);
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



            return View(model);
        }

        // GET: Contact/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // GET: Contact/Create
        public ActionResult Create()
        {
            ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱");
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
                var customer = db.客戶資料.Where(x => x.Id == 客戶聯絡人.客戶Id).FirstOrDefault();
                if (customer != null)
                {
                    // 若Email存在且還沒刪除 視為尚存在
                    if (customer.客戶聯絡人.Any(x => x.Email == 客戶聯絡人.Email && x.是否刪除 == false))
                    {
                        ModelState.AddModelError(string.Empty, "你指定的客戶中已存在此客戶聯絡人的Email!!!!");
                        ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶聯絡人.客戶Id);
                        return View(客戶聯絡人);
                    }
                    db.客戶聯絡人.Add(客戶聯絡人);
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: Contact/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶聯絡人.客戶Id);
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
                db.Entry(客戶聯絡人).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.客戶Id = new SelectList(db.客戶資料, "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: Contact/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
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
            客戶聯絡人 客戶聯絡人 = db.客戶聯絡人.Find(id);
            客戶聯絡人.是否刪除 = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
