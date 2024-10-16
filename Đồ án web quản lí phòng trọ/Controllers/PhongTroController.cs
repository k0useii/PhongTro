using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Đồ_án_web_quản_lí_phòng_trọ.Models;

namespace Đồ_án_web_quản_lí_phòng_trọ.Controllers
{
    public class PhongTroController : Controller
    {
        QuanLyNhaTroEntities2 database = new QuanLyNhaTroEntities2();
        // GET: PhongTro

        public ActionResult Index()
        {
            return View(database.PhongTroes.ToList());
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Index2()
        {
            return View(database.PhongTroes.ToList());
        }
        //Tạo mới phòng trọ
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Create()
		{
			PhongTro phongTro = new PhongTro();
			return View(phongTro);
		}
        [HttpPost]
        public ActionResult Create(PhongTro phongtro, HttpPostedFileBase[] UploadImage)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string fileName = Path.GetFileNameWithoutExtension(phongtro.UploadImage.FileName);
                    string extension = Path.GetExtension(phongtro.UploadImage.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    phongtro.Hinh = "~/images/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/images/"), fileName);
                    phongtro.UploadImage.SaveAs(fileName);
                    using (QuanLyNhaTroEntities2 db = new QuanLyNhaTroEntities2())
                    {
                        db.PhongTroes.Add(phongtro);
                        db.SaveChanges();

                    }
                    return RedirectToAction("Index2");
                }
                catch(Exception e)
                {
                    ModelState.AddModelError("", "Đã xảy ra lỗi trong quá trình thêm phòng trọ.");

                }
            }
            return View(phongtro);

        }


        //



        // Xem chi tiết phòng trọ
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
		{
			return View(database.PhongTroes.Where(s => s.MaPhong == id).FirstOrDefault());
		}

        public ActionResult Details1(int id)
        {
            return View(database.PhongTroes.Where(s => s.MaPhong == id).FirstOrDefault());
        }
        //Chỉnh sửa phòng trọ
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            return View(database.PhongTroes.Where(s => s.MaPhong == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Edit(int id, PhongTro phongTro)
        {
            database.Entry(phongTro).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("Index2");
        }

        //Xoá phòng trọ
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
		{
			return View(database.PhongTroes.Where(s => s.MaPhong == id).FirstOrDefault());
		}
		[HttpPost]
		public ActionResult Delete(int id, PhongTro phongtro)
		{
			try
			{
				phongtro = database.PhongTroes.Where(s => s.MaPhong == id).FirstOrDefault();
				database.PhongTroes.Remove(phongtro);
				database.SaveChanges();
				return RedirectToAction("Index2");
			}
			catch
			{
				return Content("Dữ liệu này đang sử dụng trong bảng khác, xoá dữ liệu thất bại!");
			}
		}

        //Tìm kiếm phòng
        //Search OPtion
        public ActionResult SearchOption(int? min, int? max)
        {
            // Get all "PhongTro" entities from the database
            var phongTroList = database.PhongTroes.ToList();

            // Filter the "PhongTro" entities based on the provided price range
            if (min.HasValue && max.HasValue)
            {
                phongTroList = phongTroList.Where(p => p.GiaThue >= min.Value && p.GiaThue <= max.Value).ToList();
            }

            // Return the filtered list of "PhongTro" entities to the view
            return View(phongTroList.ToList());
        }




    }
}