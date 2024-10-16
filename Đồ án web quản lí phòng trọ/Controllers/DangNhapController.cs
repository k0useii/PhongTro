using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Đồ_án_web_quản_lí_phòng_trọ.Models;

namespace Đồ_án_web_quản_lí_phòng_trọ.Controllers
{
    public class DangNhapController : Controller
    {
        QuanLyNhaTroEntities2 database = new QuanLyNhaTroEntities2();
        // GET: DangNhap
        public ActionResult DangNhap()
        {
            return View();
        }
        //Xử lí hàm đăng nhập
        [HttpPost]
      
        public ActionResult LoginAcount(TaiKhoan _taikhoan)
        {
            using (var context = new QuanLyNhaTroEntities2())
            {
                bool isValid = context.TaiKhoans.Any(x => x.UserName == _taikhoan.UserName && x.MatKhau == _taikhoan.MatKhau);
                if (isValid)
                {
                    FormsAuthentication.SetAuthCookie(_taikhoan.UserName, false);
                    if (_taikhoan.UserName == "vvthanh1606")
                    {
                        return RedirectToAction("Index2", "PhongTro");

                    }
                    else
                    {

                        return RedirectToAction("Index", "PhongTro");
                    }
                }
                ModelState.AddModelError("", "Username hoặc mật khẩu không trùng khớp");
            }
            return View();
        }
      
        //Đăng ký
        public ActionResult DangKy()
        {
            return View(new DangKyViewModel { TaiKhoan = new TaiKhoan(), KhachHang = new KhachHang() });
        }


        [HttpPost]
        public ActionResult RegisterUser(DangKyViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem tài khoản có tồn tại trong database chưa
                var existingAccount = database.TaiKhoans.FirstOrDefault(s => s.UserName == viewModel.TaiKhoan.UserName);
                if (existingAccount == null)
                {


                    // Lấy mã tài khoản tự động tăng bằng cách lấy mã tài khoản lớn nhất trong database, sau đó tăng lên 1
                    int maxMaTaiKhoan = database.TaiKhoans.Max(t => (int?)t.MaTaiKhoan) ?? 0;
                    viewModel.TaiKhoan.MaTaiKhoan = maxMaTaiKhoan + 1;

                    // Lấy mã khách hàng tự động tăng bằng cách lấy mã khách hàng lớn nhất trong database, sau đó tăng lên 
                    int maxMaKhachHang = database.KhachHangs.Max(kh => (int?)kh.MaKhachHang) ?? 0;
                    viewModel.KhachHang.MaKhachHang = maxMaKhachHang + 1;

                    // Thêm thông tin khách hàng vào bảng KhachHang
                    viewModel.KhachHang.MaTaiKhoan = viewModel.TaiKhoan.MaTaiKhoan; // Gán mã tài khoản mới cho khách hàng
                    database.KhachHangs.Add(viewModel.KhachHang); // Thêm khách hàng vào database
                    int newTaiKhoanId = viewModel.TaiKhoan.MaTaiKhoan;
                    // Tạo role trong model cho tài khoản mới đăng ký
                    var role = new Role
                    {
                        // Automatically increase the ID for the new Role
                        ID = database.Roles.Max(r => r.ID) + 1,
                        MaTaiKhoan = newTaiKhoanId, // Set the IDTaiKhoan
                        TenRole = "User" // Set the default role to "User"
                    };
                    database.Roles.Add(role);

                    database.Configuration.ValidateOnSaveEnabled = false; // Tạm thời tắt validate để không bị lỗi khi thêm dữ liệu
                    database.TaiKhoans.Add(viewModel.TaiKhoan); // Thêm tài khoản vào database
                    database.SaveChanges(); // Lưu thay đổi vào database

                    // Đăng ký thành công, chuyển hướng về trang đăng nhập
                    return RedirectToAction("DangNhap", "DangNhap");
                }

                else
                {
                    // Tài khoản đã tồn tại, hiển thị thông báo lỗi
                    ViewBag.ErrorRegister = "Tài khoản đã đăng nhập";
                    return View(viewModel);
                }
            }

            // Model không hợp lệ, hiển thị lại view đăng ký với thông báo lỗi
            return View(viewModel);
        }
        public ActionResult LogOutUser()
        {
              FormsAuthentication.SignOut();
            return RedirectToAction("DangNhap", "DangNhap");
        }
    }

}
