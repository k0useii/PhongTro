using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Đồ_án_web_quản_lí_phòng_trọ.Models;

namespace Đồ_án_web_quản_lí_phòng_trọ.Controllers
{
    public class TaiKhoanController : Controller
    {
        // GET: TaiKhoan
        QuanLyNhaTroEntities2 database = new QuanLyNhaTroEntities2();
        public ActionResult Index()
        {
            return View(database.TaiKhoans.ToList());
        }

        //Thêm mới tài khoản

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(TaiKhoan tk)
        {
            try
            {
                database.TaiKhoans.Add(tk);
                database.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("Lỗi tạo mới tài khoản!!!");
            }
        }

        //Details
        public ActionResult Details(int id)
        {
            return View(database.TaiKhoans.Where(s => s.MaTaiKhoan == id).FirstOrDefault());
        }

        //Cập nhật tài khoản
        public ActionResult Edit(int id)
        {
            return View(database.TaiKhoans.Where(s => s.MaTaiKhoan == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Edit(int id, TaiKhoan tk)
        {
            database.Entry(tk).State = EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("Index");
        }

        //Delete

        public ActionResult Delete(int id)
        {
            return View(database.TaiKhoans.Where(s => s.MaTaiKhoan == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Delete(int id, TaiKhoan tk)
        {
            try
            {
                tk = database.TaiKhoans.Where(s => s.MaTaiKhoan == id).FirstOrDefault();
                database.TaiKhoans.Remove(tk);
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