using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foody.Models;
using PagedList;

namespace Foody.Controllers
{
    public class FoodController : Controller
    {
        int pageSize = 9;
        private FoodDeliveryEntities db;
        public FoodController()
        {
            db = new FoodDeliveryEntities();
        }
        // GET: Food
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult QuanLyMonAn(Guid StoreID, int page)
        {
           
            var foods = db.Foods.Where(n => n.StoreID == StoreID).OrderBy(x => x.FoodName).ToPagedList(page, pageSize);
            if (foods == null)
            {
                //Trả về trang báo lỗi 
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.StoreID = StoreID;
            return View(foods);
        }

        public ActionResult Edit(Guid StoreID,Guid FoodID)
        {

            var food = db.Foods.Where(n => n.FoodID == FoodID).SingleOrDefault();
            if (food == null)
            {
                //Trả về trang báo lỗi 
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.CategoryID = new SelectList(db.Categories.Where(n => n.StoreID == StoreID).OrderBy(n => n.CategoryName).ToList(), "CategoryID", "CategoryName",food.CategoryID);
            ViewBag.StoreID = StoreID;
            return View(food);
        }
        [HttpPost]
        public ActionResult Edit(Food food,Guid StoreID)
        {

            var f = db.Foods.Where(n => n.FoodID == food.FoodID).SingleOrDefault();
            if (food == null)
            {
                //Trả về trang báo lỗi 
                Response.StatusCode = 404;
                return null;
            }
            f.StoreID = StoreID;
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhận trong model
                db.Entry(f).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            ViewBag.Message = f.FoodName + " Cập nhật thành công";
            ViewBag.StoreID = StoreID;
            return View(food);
        }
    }
}