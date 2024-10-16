using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Đồ_án_web_quản_lí_phòng_trọ.Models;



namespace Đồ_án_web_quản_lí_phòng_trọ.Controllers
{
    public class KhachHangController : Controller
    {
		QuanLyNhaTroEntities2 database = new QuanLyNhaTroEntities2();
        // GET: KhachHang
        public ActionResult Index()
        {
            return View(database.KhachHangs.ToList());
        }
        public ActionResult LienHe()
        {
            return View();
        }
        //Them moi khach hàng
        public ActionResult Create()
        {
            return View();
        }
		[HttpPost]
		public ActionResult Create(KhachHang khachhang)
		{
			try
			{
				database.KhachHangs.Add(khachhang);
				database.SaveChanges();
				return RedirectToAction("Index");
			}
			catch
			{
				return Content("Lỗi tạo mới khách hàng!!!");
			}
		}
		//Details
		public ActionResult Details(int id)
		{
			return View(database.KhachHangs.Where(s => s.MaKhachHang == id).FirstOrDefault());
		}
		//Edit
		public ActionResult Edit(int? id)
		{
			return View(database.KhachHangs.Where(s => s.MaKhachHang == id).FirstOrDefault());
		}
		[HttpPost]
		public ActionResult Edit(int id, KhachHang khachhang)
		{
			database.Entry(khachhang).State = EntityState.Modified;
			database.SaveChanges();
			return RedirectToAction("Index");
		}
        public ActionResult EditProfile()
        {
            // Get the username of the currently logged-in user
            string loggedInUsername = User.Identity.Name;

            // Find the user account based on the username
            var userAccount = database.TaiKhoans.SingleOrDefault(u => u.UserName == loggedInUsername);

            if (userAccount != null)
            {
                // Get the IDTaiKhoan from the user account
                int idTaiKhoan = userAccount.MaTaiKhoan;

                // Find the customer based on the IDTaiKhoan
                var khachHang = database.KhachHangs.SingleOrDefault(kh => kh.MaTaiKhoan == idTaiKhoan);

                if (khachHang != null)
                {
                    // Pass the customer object to the view for editing
                    return View(khachHang);
                }
            }

            // Handle the case when user account or customer information is not found
            return View("NotFound");
        }

        [HttpPost]
        public ActionResult EditProfile(KhachHang khachhang)
        {
            try
            {
                // Update the customer information in the database
                database.Entry(khachhang).State = EntityState.Modified;
                database.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                // Handle errors appropriately
                return Content("Lỗi chỉnh sửa thông tin khách hàng!!!");
            }
        }



        //Delete

        public ActionResult Delete(int id)
		{
			return View(database.KhachHangs.Where(s => s.MaKhachHang == id).FirstOrDefault());
		}
		[HttpPost]
		public ActionResult Delete(int id, KhachHang khachhang)
		{
			try
			{
				khachhang = database.KhachHangs.Where(s => s.MaKhachHang == id).FirstOrDefault();
				database.KhachHangs.Remove(khachhang);
				database.SaveChanges();
				return RedirectToAction("Index");
			}
			catch
			{
				return Content("Dữ liệu này đang sử dụng trong bảng khác, xoá dữ liệu thất bại!");
			}
		}
        public ActionResult HistoryRentRoom()
        {
            // Xác định Username của tài khoản đang đăng nhập
            string loggedInUsername = User.Identity.Name;

            // Kết nối đến cơ sở dữ liệu
            using (var dbContext = new QuanLyNhaTroEntities2())
            {
                // Tìm tài khoản trong database dựa trên Username
                var userAccount = dbContext.TaiKhoans.SingleOrDefault(u => u.UserName == loggedInUsername);

                if (userAccount != null)
                {
                    // Truy xuất IDTaiKhoan từ tài khoản
                    int idTaiKhoan = userAccount.MaTaiKhoan;

                    // Lấy khách hàng tương ứng với tài khoản
                    var khachHang = dbContext.KhachHangs.SingleOrDefault(kh => kh.MaTaiKhoan == idTaiKhoan);

                    if (khachHang != null)
                    {
                        // Lấy IDKhachHang của khách hàng
                        String tenKhachHang = khachHang.TenKhachHang;

                        // Lọc ra các phiếu thuê trả phòng dựa trên IDKhachHang
                        var rentalHistory = dbContext.PhieuThueTraPhongs
                            .Where(pttp => pttp.TenKhachHang == tenKhachHang)
                            .ToList();

                        // Trả về view và truyền dữ liệu lịch sử thuê phòng
                        return View("HistoryRentRoom", rentalHistory);
                    }
                }

                // Xử lý khi không tìm thấy tài khoản hoặc thông tin khách hàng
                return View("NotFound");
            }
        }
        public ActionResult GetNotificationCount()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Get the current user's UserName
                string userName = User.Identity.Name;

                // Retrieve the user's associated KhachHang entity
                var khachHang = database.KhachHangs.FirstOrDefault(kh => kh.TaiKhoan.UserName == userName);

                if (khachHang != null)
                {
                    // Count the number of PhieuThuTien records related to the KhachHang
                    int notificationCount = database.PhieuThuTiens
                        .Where(ptt => ptt.MaKhachHang == khachHang.MaKhachHang)
                        .Count();

                    return Json(notificationCount, JsonRequestBehavior.AllowGet);
                }
            }

            // If the user is not authenticated or KhachHang is not found, return 0
            return Json(0, JsonRequestBehavior.AllowGet);
        }


    }
}