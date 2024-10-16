using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using Đồ_án_web_quản_lí_phòng_trọ.Models;

namespace Đồ_án_web_quản_lí_phòng_trọ.Controllers
{
    
    public class PhieuThueTraPhongController : Controller
    {
        QuanLyNhaTroEntities2 database = new QuanLyNhaTroEntities2();
        // GET: PhieuThueTraPhong
        //Danh sách phiếu 
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(database.PhieuThueTraPhongs.ToList());
        }
  
        // Thêm phiếu
        [Authorize(Roles = "Admin")]
        [HttpGet]
		public ActionResult GetThongTinPhong(int maPhong)
		{
			var phong = database.PhongTroes.FirstOrDefault(p => p.MaPhong == maPhong);
			if (phong != null)
			{
				return PartialView("_PhongDetailsPartial", phong);
			}
			else
			{
				// Nếu không tìm thấy phòng với mã phòng tương ứng, trả về một đối tượng JSON trống
				return Json(new { }, JsonRequestBehavior.AllowGet);
			}
		}
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public JsonResult GetThongTinKhachHang(string tenKhachHang)
        {
            var khachHang = database.KhachHangs.FirstOrDefault(kh => kh.TenKhachHang == tenKhachHang);

            if (khachHang != null)
            {
                // Trả về thông tin khách hàng dưới dạng JSON
                return Json(new
                {
                    DiaChi = khachHang.DiaChi,
                    CCCD = khachHang.CCCD,
                    SDT = khachHang.SDT,
                    MaTaiKhoan = khachHang.MaTaiKhoan
                    //Thêm các thông tin khác nếu cần
                }, JsonRequestBehavior.AllowGet);
            }

            // Trả về null nếu không tìm thấy thông tin khách hàng
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            using (var db = new QuanLyNhaTroEntities2())
            {
                // Truy vấn danh sách mã phòng từ cơ sở dữ liệu
                var danhSachMaPhong = db.PhongTroes.Select(p => p.MaPhong).ToList();
                var danhSachTenKH = db.KhachHangs.Select(p => p.TenKhachHang).ToList();

                // Đưa danh sách mã phòng vào ViewBag để hiển thị trong view
                ViewBag.DanhSachMaPhong = new SelectList(danhSachMaPhong);
                ViewBag.DanhSachTenKH = new SelectList(danhSachTenKH);

                // Khởi tạo phiếu thuê trả phòng mới
                var phieuThueTraPhong = new PhieuThueTraPhong();

                // Gắn thông tin phòng đầu tiên vào model nếu có
                if (danhSachMaPhong.Any())
                {
                    var firstMaPhong = danhSachMaPhong.First();
                    var phong = db.PhongTroes.FirstOrDefault(p => p.MaPhong == firstMaPhong);
                    if (phong != null)
                    {
                        phieuThueTraPhong.MaPhong = phong.MaPhong;
                        phieuThueTraPhong.MaPhong = phong.GiaThue;
                        phieuThueTraPhong.MaPhong = phong.DienTich;
                        // Gán các thông tin phòng khác tương tự tại đây (nếu có)
                    }
                }

                return View(phieuThueTraPhong);
            }

        }

        [HttpPost]
        public ActionResult Create(PhieuThueTraPhong phieuttp)
        {
            try
            {
                // Retrieve the room associated with the rental contract
                var room = database.PhongTroes.FirstOrDefault(p => p.MaPhong == phieuttp.MaPhong);

                // Check if the room exists
                if (room != null)
                {
                    // Check if the room is available for rent
                    if (room.TrangThai != "Đã thuê")
                    {
                        // Update the status of the room to "Đã thuê"
                        room.TrangThai = "Đã thuê";

                        // Generate a new MaPhieuTTP (assuming it's an identity column)
                        int maxMaPhieuTTP = database.PhieuThueTraPhongs.Max(t => (int?)t.MaPhieuTTP) ?? 0;
                        phieuttp.MaPhieuTTP = maxMaPhieuTTP + 1;

                        // Add the rental contract to the database
                        database.PhieuThueTraPhongs.Add(phieuttp);

                        // Save changes to the database
                        database.SaveChanges();

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        // Room is already rented, handle the situation accordingly
                        return Content("Phòng đã được thuê. Không thể tạo phiếu thuê mới.");
                    }
                }
                else
                {
                    // Room not found, handle the situation accordingly
                    return Content("Phòng không tồn tại!");
                }
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging
                // Consider using a logging framework or writing to a log file
                Console.WriteLine($"Error: {ex.Message}");
                return Content("Đã xảy ra lỗi khi tạo phiếu thuê trả phòng!");
            }
        }



        // Xem chi tiết phiếu thuê trả phòng Details
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
		{
			return View(database.PhieuThueTraPhongs.Where(s => s.MaPhieuTTP == id).FirstOrDefault());
		}

        //Chỉnh sửa thông tin phiếu thuê trả phòng
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
		{
			return View(database.PhieuThueTraPhongs.Where(s => s.MaPhieuTTP == id).FirstOrDefault());
		}
		[HttpPost]
		public ActionResult Edit(int id, PhieuThueTraPhong phieuttp)
		{
			database.Entry(phieuttp).State = EntityState.Modified;
			database.SaveChanges();
			return RedirectToAction("Index");
		}

        //Xoá phiếu thuê trả phòng 
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            return View(database.PhieuThueTraPhongs.Where(s => s.MaPhieuTTP == id).FirstOrDefault());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, PhieuThueTraPhong phieuttp)
        {
            try
            {
                phieuttp = database.PhieuThueTraPhongs.FirstOrDefault(s => s.MaPhieuTTP == id);

                if (phieuttp == null)
                {
                    return HttpNotFound();
                }

                // Retrieve the associated room
                var room = database.PhongTroes.FirstOrDefault(p => p.MaPhong == phieuttp.MaPhong);

                if (room != null)
                {
                    // Update the room status to "Trống"
                    room.TrangThai = "Trống";

                    // Mark the room as modified
                    database.Entry(room).State = EntityState.Modified;
                }

                // Remove the rental contract
                database.PhieuThueTraPhongs.Remove(phieuttp);
                database.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging
                Console.WriteLine($"Error: {ex.Message}");
                return Content("Dữ liệu này đang sử dụng trong bảng khác, xoá dữ liệu thất bại!");
            }
        }
    }
}