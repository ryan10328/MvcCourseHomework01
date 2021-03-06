﻿using System;
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
using System.Text;
using System.Security.Cryptography;

namespace MvcHomework01.Controllers
{
    public class CustomerController : Controller
    {
        客戶資料Repository repo = RepositoryHelper.Get客戶資料Repository();

        // GET: Customer
        public ActionResult Index()
        {
            var 客戶資料 = repo.All().Where(x => x.是否刪除 == false);

            CustomerViewModel model = new CustomerViewModel()
            {
                CustomerSearchParam = null,
                客戶資料s = 客戶資料.ToList(),
                CountryList = new SelectList(repo.GetCountries(), "Id", "Name")
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CustomerViewModel model)
        {
            model.CountryList = new SelectList(repo.GetCountries(), "Id", "Name", model.CustomerSearchParam.CountryId);

            model = repo.GetIndexData(model);

            return View(model);
        }

        // GET: Customer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo.Get(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            ViewBag.Countries = new SelectList(repo.GetCountries(), "Id", "Name");
            return View();
        }

        // POST: Customer/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,客戶國家Id")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                客戶資料.是否刪除 = false;
                repo.Add(客戶資料);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            return View(客戶資料);
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo.Get(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: Customer/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int Id, FormCollection form)
        {

            var db = repo.UnitOfWork.Context as CrmEntities;
            var 客戶資料 = repo.Get(Id);
            string pwd = 客戶資料.密碼;
            if (User.Identity.IsAuthenticated)
            {
                if (TryUpdateModel<客戶資料>(客戶資料, new string[] { "Id", "密碼", "電話", "傳真", "地址", "Email" }))
                {
                    if (!string.IsNullOrEmpty(客戶資料.密碼))
                    {
                        客戶資料.密碼 = EnCode(客戶資料.密碼);
                    }
                    else
                    {
                        客戶資料.密碼 = pwd;
                    }
                    db.SaveChanges();
                }
                else
                {
                    return View(客戶資料);
                }
            }
            else
            {
                if (TryUpdateModel<客戶資料>(客戶資料, new string[] { "Id", "客戶名稱", "統一編號", "電話", "密碼", "傳真", "地址", "Email", "是否刪除" }))
                {
                    if (!string.IsNullOrEmpty(客戶資料.密碼))
                    {
                        客戶資料.密碼 = EnCode(客戶資料.密碼);
                    }
                    else
                    {
                        客戶資料.密碼 = pwd;
                    }
                    db.SaveChanges();
                }
                else
                {
                    return View(客戶資料);
                }
            }

            TempData["Messages"] = "儲存成功";
            return RedirectToAction("Index", "Customer");
           
           
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo.Get(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶資料 客戶資料 = repo.Get(id);
            客戶資料.是否刪除 = true;

            repo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        public ActionResult ExportExcel()
        {
            XSSFWorkbook excel = new XSSFWorkbook();
            ISheet sheet = excel.CreateSheet("Export");

            var customers = repo.All().Where(x => x.是否刪除 == false).ToList();
            IRow firstRow = sheet.CreateRow(0);
            firstRow.CreateCell(0).SetCellValue("客戶名稱");
            firstRow.CreateCell(1).SetCellValue("統一編號");
            firstRow.CreateCell(2).SetCellValue("電話");
            firstRow.CreateCell(3).SetCellValue("傳真");
            firstRow.CreateCell(4).SetCellValue("地址");
            firstRow.CreateCell(5).SetCellValue("Email");

            for (int i = 0; i < customers.Count(); i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(customers[i].客戶名稱);
                row.CreateCell(1).SetCellValue(customers[i].統一編號);
                row.CreateCell(2).SetCellValue(customers[i].電話);
                row.CreateCell(3).SetCellValue(customers[i].傳真);
                row.CreateCell(4).SetCellValue(customers[i].地址);
                row.CreateCell(5).SetCellValue(customers[i].Email);
            }

            MemoryStream ms = new MemoryStream();
            excel.Write(ms);

            //ms.Seek(0, SeekOrigin.Begin);

            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "客戶資料.xlsx");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                var db = repo.UnitOfWork.Context as CrmEntities;
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public string EnCode(string EnString)  //將字串加密
        {
            byte[] Key = { 123, 123, 123, 123, 123, 123, 123, 123 };
            byte[] IV = { 123, 123, 123, 123, 123, 123, 123, 123 };

            byte[] b = Encoding.UTF8.GetBytes(EnString);

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            ICryptoTransform ict = des.CreateEncryptor(Key, IV);
            byte[] outData = ict.TransformFinalBlock(b, 0, b.Length);
            return Convert.ToBase64String(outData);  //回傳加密後的字串
        }

        public string DeCode(string DeString) //將字串解密
        {
            byte[] Key = { 123, 123, 123, 123, 123, 123, 123, 123 };
            byte[] IV = { 123, 123, 123, 123, 123, 123, 123, 123 };
            byte[] b = Convert.FromBase64String(DeString);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            ICryptoTransform ict = des.CreateDecryptor(Key, IV);
            byte[] outData = ict.TransformFinalBlock(b, 0, b.Length);
            return Encoding.UTF8.GetString(outData);//回傳解密後的字串
        }
    }
}
