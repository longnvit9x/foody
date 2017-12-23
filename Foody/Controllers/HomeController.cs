using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foody.Models;
using PagedList;
namespace Foody.Controllers
{
    public class HomeController : Controller
    {
        private FoodDeliveryEntities db;
        int pageSize = 9;
        string sThongBao = "Không tìm kết quả nào phù hợp";
        public HomeController()
        {
            db = new FoodDeliveryEntities();
        }
      

        public ActionResult Index(int page = 1)
        {
            var model = db.Stores.OrderBy(x => x.StoreName).ToPagedList(page, pageSize);
            return View(model);
        }

        [HttpPost]
        public ActionResult TimKiem(FormCollection f=null, int page = 1)
        {
            string sStoreName ="";
            string sAddress="";
            string sStoreType="";
            if (f != null)
            {
                sStoreName = f["txtStoreName"].ToString();
                sAddress = f["cbxAddress"].ToString();
                sStoreType = f["cbxStoreType"].ToString();
            } 
            ViewBag.StoreName = sStoreName;
            ViewBag.Addres = sAddress;
            ViewBag.StoreType = sStoreType;
            List<Store> lstKQTK = db.Stores.Where(n => n.StoreName.Contains(sStoreName)
            && n.Address.Contains(sAddress)
            && n.StoreType.Contains(sStoreType)).ToList();
            if (lstKQTK.Count == 0)
            {
                ViewBag.ThongBao = sThongBao;
                return View(db.Stores.OrderBy(n => n.StoreName).ToPagedList(page, pageSize));
            }
            ViewBag.ThongBao = "Đã tìm thấy " + lstKQTK.Count + " kết quả!";
            return View(lstKQTK.OrderBy(n => n.StoreName).ToPagedList(page, pageSize));
        }

        public ActionResult TimKiem(int page = 1, string storeType = "")
        {
            string sStoreType = storeType;
            ViewBag.StoreType = sStoreType;
            List<Store> lstKQTK = db.Stores.Where(n => n.StoreType.Contains(sStoreType)).ToList();
            if (lstKQTK.Count == 0)
            {
                ViewBag.ThongBao = sThongBao;
                return View(db.Stores.OrderBy(n => n.StoreName).ToPagedList(page, pageSize));
            }
            ViewBag.ThongBao = "Đã tìm thấy " + lstKQTK.Count + " kết quả!";
            return View(lstKQTK.OrderBy(n => n.StoreName).ToPagedList(page, pageSize));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult SlideHome()
        {
            var model = db.Stores.OrderBy(x => x.StoreName).Take(4).ToList();
            return PartialView(model);
        }
        public ActionResult MenuTop()
        {
            if (Session["FullName"] != null)
            {
                ViewBag.FullName = Session["FullName"].ToString();
            }
            return PartialView();
        }

    }
}