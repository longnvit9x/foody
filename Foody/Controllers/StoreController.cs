using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foody.Models;


namespace Foody.Controllers
{
    public class StoreController : Controller
    {
        private FoodDeliveryEntities db;
        public StoreController()
        {
            db = new FoodDeliveryEntities();
        }
        // GET: Store

        public ActionResult Index()
        {
            return View();
        }
        // GET: Store
        public ActionResult XemChiTiet(Guid StoreID)
        {

            Store store = db.Stores.SingleOrDefault(n => n.StoreID == StoreID);
            if (store == null)
            {
                //Trả về trang báo lỗi 
                Response.StatusCode = 404;
                return null;
            }
            else
            {
                if (Session["UserName"] != null)
                {
                    string user = Session["UserName"].ToString();
                    Customer cus = db.Customers.SingleOrDefault(n => n.UserName == user);
                    if (store.CustomerID.Equals(cus.CustomerID))
                    {
                        ViewBag.StoreAdmin = 1;
                        return View(store);
                    }
                }
            }
            return View(store);
        }

        public ActionResult ThongTin(Guid StoreID)
        {
            Store store = db.Stores.SingleOrDefault(n => n.StoreID == StoreID);
            if (store == null)
            {
                //Trả về trang báo lỗi 
                Response.StatusCode = 404;
                return null;
            }
            return View(store);
        }
        [HttpPost]
        public ActionResult ThongTin(Store store)
        {
            Store st = db.Stores.Where(n => n.StoreID == store.StoreID).SingleOrDefault();
            if (st == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            st.CustomerID = Guid.Parse(Session["CustomerID"].ToString());
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhận trong model
                db.Entry(st).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            ViewBag.Message = store.StoreName + "Cập nhật thành công";
            return View(st);
        }

        public ActionResult MenuStoreAdmin(string StoreID)
        {
            ViewBag.StoreID = StoreID;
            return PartialView();
        }

        public ActionResult Menuleft()
        {
            return PartialView();
        }

        public ActionResult StoreDetail(Guid StoreID)
        {
            var foods = db.Foods.Where(n => n.StoreID == StoreID).ToList();
            return PartialView(foods);
        }

        public ActionResult RegisterStore()
        {

            return View();
        }
    }
}