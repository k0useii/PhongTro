using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Đồ_án_web_quản_lí_phòng_trọ.Models;

namespace Đồ_án_web_quản_lí_phòng_trọ.Controllers
{
    public class QuanLyController : Controller
    {
        // GET: QuanLy
        QuanLyNhaTroEntities2 database = new QuanLyNhaTroEntities2();
        public ActionResult Index()
        {
            return View(database.QuanLies.ToList());
        }

        //Thêm
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(QuanLy ql)
        {
            try
            {
                database.QuanLies.Add(ql);
                database.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("Dữ liệu này đang được sử dụng trong bảng khác. Tạo dữ liệu thất bại!!!");
            }
        }

        //Details

        public ActionResult Details(int id)
        {
            return View(database.QuanLies.Where(s => s.MaQuanLy == id).FirstOrDefault());
        }

        //Edit
        public ActionResult Edit(int? id)
        {
            return View(database.QuanLies.Where(s => s.MaQuanLy == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Edit(int id, QuanLy ql)
        {
            database.Entry(ql).State = EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("Index");
        }

        //Delete

        public ActionResult Delete(int id)
        {
            return View(database.QuanLies.Where(s => s.MaQuanLy == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Delete(int id, QuanLy ql)
        {
            try
            {
                ql = database.QuanLies.Where(s => s.MaQuanLy == id).FirstOrDefault();
                database.QuanLies.Remove(ql);
                database.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("Dữ liệu này đang sử dụng trong bảng khác, xoá dữ liệu thất bại!");
            }
        }
    }
}