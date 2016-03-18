using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcHomework01.Models;
using Microsoft.Web.Mvc;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;

namespace MvcHomework01.Controllers
{
    public class BankController : Controller
    {
        //private CrmEntities db = new CrmEntities();
        客戶銀行資訊Repository repoBank = RepositoryHelper.Get客戶銀行資訊Repository();
        客戶資料Repository repoCustomer = RepositoryHelper.Get客戶資料Repository();
        // GET: Bank
        public ActionResult Index()
        {
            var 客戶銀行資訊 = repoBank.All().Include(客 => 客.客戶資料).Where(x => x.是否刪除 == false);

            BankViewModel model = new BankViewModel()
            {
                BankViewModelSearch = null,
                客戶銀行資訊s = 客戶銀行資訊.ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(BankViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var 客戶銀行資訊 = repoBank.All().Include(客 => 客.客戶資料).Where(x => x.是否刪除 == false);

            if (!string.IsNullOrEmpty(model.BankViewModelSearch.BankName))
            {
                客戶銀行資訊 = 客戶銀行資訊.Where(x => x.銀行名稱.Contains(model.BankViewModelSearch.BankName));
            }

            model.客戶銀行資訊s = 客戶銀行資訊;

            return View(model);
        }

        // GET: Bank/Details/5
        [HandleError(ExceptionType = typeof(InvalidOperationException),View = "MissingParam")]
        public ActionResult Details(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            客戶銀行資訊 客戶銀行資訊 = repoBank.Get(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // GET: Bank/Create
        public ActionResult Create()
        {
            ViewBag.客戶Id = new SelectList(repoCustomer.All(), "Id", "客戶名稱");
            return View();
        }

        // POST: Bank/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                客戶銀行資訊.是否刪除 = false;
                repoBank.Add(客戶銀行資訊);
                repoBank.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(repoCustomer.All(), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // GET: Bank/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = repoBank.Get(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(repoCustomer.All(), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // POST: Bank/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼,是否刪除")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                var db = repoBank.UnitOfWork.Context as CrmEntities;
                db.Entry(客戶銀行資訊).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.客戶Id = new SelectList(repoCustomer.All(), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // GET: Bank/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = repoBank.Get(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // POST: Bank/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        [AjaxOnly]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                客戶銀行資訊 客戶銀行資訊 = repoBank.Get(id);
                客戶銀行資訊.是否刪除 = true;
                repoBank.UnitOfWork.Commit();
                return Json(new { success = true, messages = "刪除成功。" });
            }
            catch (Exception ex)
            {
                return Json(new { success = true, messages = ex.Message });
            }
        }

        public ActionResult ExportExcel()
        {
            XSSFWorkbook excel = new XSSFWorkbook();
            ISheet sheet = excel.CreateSheet("Export");

            var banklist = repoBank.All().Include(x => x.客戶資料).Where(x => x.是否刪除 == false).ToList();
            IRow firstRow = sheet.CreateRow(0);
            firstRow.CreateCell(0).SetCellValue("銀行名稱");
            firstRow.CreateCell(1).SetCellValue("銀行代碼");
            firstRow.CreateCell(2).SetCellValue("分行代碼");
            firstRow.CreateCell(3).SetCellValue("帳戶名稱");
            firstRow.CreateCell(4).SetCellValue("帳戶號碼");
            firstRow.CreateCell(5).SetCellValue("客戶名稱");

            for (int i = 0; i < banklist.Count(); i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(banklist[i].銀行名稱);
                row.CreateCell(1).SetCellValue(banklist[i].銀行代碼);
                row.CreateCell(2).SetCellValue(banklist[i].分行代碼.Value);
                row.CreateCell(3).SetCellValue(banklist[i].帳戶名稱);
                row.CreateCell(4).SetCellValue(banklist[i].帳戶號碼);
                row.CreateCell(5).SetCellValue(banklist[i].客戶資料.客戶名稱);
            }

            MemoryStream ms = new MemoryStream();
            excel.Write(ms);
            //ms.Seek(0, SeekOrigin.Begin);

            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "客戶銀行資料.xlsx");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                var db = repoBank.UnitOfWork.Context as CrmEntities;
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
