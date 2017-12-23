using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foody.Models;
namespace Foody.Controllers
{
    public class AccountController : Controller
    {
        private FoodDeliveryEntities db;
        public AccountController()
        {
            db = new FoodDeliveryEntities();
        }
        // GET: Account
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Customer cus, HttpPostedFileBase fileUpload)
        {
            cus.CustomerID =Guid.NewGuid();
            db.Customers.Add(cus);
            db.SaveChanges();
            ModelState.Clear();
            ViewBag.Message = cus.FullName + "đăng kí thành công";
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            string userName = f["txtUserName"].ToString();
            string password = f["txtPassword"].ToString();

            var Cus=db.Customers.Single(u => u.UserName == userName && u.Password == password);
            if (Cus != null)
            {
                Session["FullName"] = Cus.FullName.ToString();
                Session["UserName"] = Cus.UserName.ToString();
                Session["CustomerID"] = Cus.CustomerID.ToString();
                return RedirectToAction("LoggedIn");
            }
            else
            {
                ModelState.AddModelError("", "Tài khoản và mật khẩu không đúng...!");
            }
            return View();
        }
        public ActionResult LoggedIn()
        {
            if (Session["FullName"] != null)
            {
                return RedirectToAction("Index","Home");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult AccountDetail()
        {
            if (Session["FullName"] != null)
            {
                ViewBag.FullName = Session["FullName"].ToString();
            }
            return PartialView();
        }
        public ActionResult Logout()
        {
            if (Session["FullName"] != null)
            {
                Session.Clear();
                ViewBag.FullName = null;
            }
            return RedirectToAction("Index","Home");
        }
    }
}