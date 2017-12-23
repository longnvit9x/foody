using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foody.Models;
using PagedList;

namespace Foody.Controllers
{
    public class InvoiceController : Controller
    {
        int pageSize = 9;
        private FoodDeliveryEntities db;
        public InvoiceController()
        {
            db = new FoodDeliveryEntities();
        }
        // GET: Invoice
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult QuanLyHoaDon(Guid StoreID, int page=1)
        {

            var invoices = db.Invoices.Where(n => n.StoreID == StoreID).OrderBy(x => x.CustomerName).ToPagedList(page, pageSize);
            if (invoices == null)
            {
                //Trả về trang báo lỗi 
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.StoreID = StoreID;
            return View(invoices);
        }

        public ActionResult Edit(Guid StoreID, Guid InvoiceID)
        {

            var invoice = db.Invoices.Where(n => n.InvoiceID == InvoiceID).SingleOrDefault();
            if (invoice == null)
            {
                //Trả về trang báo lỗi 
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.StoreID = StoreID;
            return View(invoice);
        }
        [HttpPost]
        public ActionResult Edit(Food food, Guid StoreID)
        {

            var f = db.Foods.Where(n => n.FoodID == food.FoodID).SingleOrDefault();
            if (f == null)
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

        public ActionResult EditStt(Guid InvoiceID, int status, string StoreID)
        {

            var f = db.Invoices.Where(n => n.InvoiceID == InvoiceID).SingleOrDefault();
            if (f == null)
            {
                //Trả về trang báo lỗi 
                Response.StatusCode = 404;
                return null;
            }
            f.Status = status;
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhận trong model
                db.Entry(f).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            ViewBag.Message = f.InvoiceID + " cập nhật thành công";
            return RedirectToAction("QuanLyHoaDon", "Invoice", new { StoreID = StoreID });
        }

        public ActionResult InvoiceDetail(Guid StoreID, Guid InvoiceID,int page)
        {

            var invoiceDetails = db.InvoiceDetails.Where(n => n.InvoiceID == InvoiceID).OrderBy(x => x.FoodID).ToPagedList(page,pageSize);
            if (invoiceDetails == null)
            {
                //Trả về trang báo lỗi 
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.StoreID = StoreID;
            ViewBag.InvoiceId = InvoiceID;
            return View(invoiceDetails);
        }
    }
}