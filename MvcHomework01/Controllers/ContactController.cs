using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcHomework01.Models;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;

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

        public ActionResult ExportExcel()
        {
            XSSFWorkbook excel = new XSSFWorkbook();
            ISheet sheet = excel.CreateSheet("Export");

            var contactList = contactRepo.All().Include(x => x.客戶資料).Where(x => x.是否刪除 == false).ToList();
            IRow firstRow = sheet.CreateRow(0);
            firstRow.CreateCell(0).SetCellValue("職稱");
            firstRow.CreateCell(1).SetCellValue("姓名");
            firstRow.CreateCell(2).SetCellValue("Email");
            firstRow.CreateCell(3).SetCellValue("手機");
            firstRow.CreateCell(4).SetCellValue("電話");
            firstRow.CreateCell(5).SetCellValue("客戶名稱");

            for (int i = 0; i < contactList.Count(); i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(contactList[i].職稱);
                row.CreateCell(1).SetCellValue(contactList[i].姓名);
                row.CreateCell(2).SetCellValue(contactList[i].Email);
                row.CreateCell(3).SetCellValue(contactList[i].手機);
                row.CreateCell(4).SetCellValue(contactList[i].電話);
                row.CreateCell(5).SetCellValue(contactList[i].客戶資料.客戶名稱);
            }

            MemoryStream ms = new MemoryStream();
            excel.Write(ms);
            //ms.Seek(0, SeekOrigin.Begin);

            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "客戶聯絡人資料.xlsx");
        }

        //
        public ActionResult EditTable(int id) // 客戶ID
        {
            ViewBag.id = id;
            var 客戶聯絡人s = customerRepo.Get(id).客戶聯絡人;

            return PartialView("EditTable", 客戶聯絡人s);
        }
        //
        [HttpPost]
        public ActionResult EditTable(IList<客戶聯絡人> data, int id)
        {
            
            foreach (var item in data)
            {
                var contact = contactRepo.Get(item.Id);
                contact.職稱 = item.職稱;
                contact.電話 = item.電話;
                contact.手機 = item.手機;
            }

            contactRepo.UnitOfWork.Commit();
            TempData["Messages"] = "儲存成功";

            return RedirectToAction("Details", "Customer", new { id = id });
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
