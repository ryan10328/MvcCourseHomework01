using MvcHomework01.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcHomework01.Controllers
{
    public class MemberController : Controller
    {

        客戶資料Repository repo = RepositoryHelper.Get客戶資料Repository();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // has this account in db
            if (!repo.All().Where(x => x.帳號 == model.Account).Any())
            {
                ModelState.AddModelError(string.Empty, "該帳號不存在，請重新輸入。");
                return View(model);
            }

            var user = repo.All().Where(x => x.帳號 == model.Account).FirstOrDefault();

            if (user.密碼 == EnCode(model.Password))
            {
                FormsAuthentication.RedirectFromLoginPage(user.客戶名稱, model.RememberMe);
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        // 借用對稱式加密
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