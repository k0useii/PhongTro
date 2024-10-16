using Đồ_án_web_quản_lí_phòng_trọ.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Đồ_án_web_quản_lí_phòng_trọ.Controllers
{
    public class ThongKeController : Controller
    {
        // GET: ThongKe
        QuanLyNhaTroEntities2 database = new QuanLyNhaTroEntities2();

        public ActionResult DanhSachThongKe()
        {
            return View();
        }
        public ActionResult FilterAndCalculateByThang(int thang)
        {
            var filteredPhieuThuTien = database.PhieuThuTiens
                .Where(p => p.Thang == thang && p.TrangThai == "Đã thanh toán")
                .ToList();

            var tongDoanhThu = filteredPhieuThuTien.Sum(p => p.TongTien);

            ViewBag.Thang = thang;
            ViewBag.TongDoanhThu = tongDoanhThu;

            return View("FilteredAndCalculatedPhieuThuTien", filteredPhieuThuTien);
        }
        private int TinhTongDoanhThuCaNam()
        {
            var tongDoanhThuCaNam = database.PhieuThuTiens
                .Where(p => p.TrangThai == "Đã thanh toán")
                .Sum(p => p.TongTien);

            return tongDoanhThuCaNam;
        }

        public ActionResult ChuaThanhToan()
        {
            var chuaThanhToan = database.PhieuThuTiens
                .Where(p => p.TrangThai == "Chưa thanh toán")
                .ToList();

            return View("ChuaThanhToan", chuaThanhToan);
        }

        public ActionResult KhachHangThanThiet()
        {
            var khachHangs = database.KhachHangs
                .Where(kh => kh.PhieuThuTiens.Count >= 5)
                .ToList();
            ViewBag.KhachHangs = khachHangs;

            return View("KhachHangThanThiet");
        }
        public ActionResult ThongKeTongQuat()
        {
            int daThanhToanCount = database.PhieuThuTiens
                .Where(p => p.TrangThai == "Đã thanh toán")
                .Count();

            int chuaThanhToanCount = database.PhieuThuTiens
                .Where(p => p.TrangThai == "Chưa thanh toán")
                .Count();

            int daThanhToan = daThanhToanCount > 1
                ? database.PhieuThuTiens.Where(p => p.TrangThai == "Đã thanh toán").Sum(p => p.TongTien)
                : daThanhToanCount == 1
                    ? database.PhieuThuTiens.Single(p => p.TrangThai == "Đã thanh toán").TongTien
                    : 0;

            int chuaThanhToan = chuaThanhToanCount > 1
                ? database.PhieuThuTiens.Where(p => p.TrangThai == "Chưa thanh toán").Sum(p => p.TongTien)
                : chuaThanhToanCount == 1
                    ? database.PhieuThuTiens.Single(p => p.TrangThai == "Chưa thanh toán").TongTien
                    : 0;

            int tongDoanhThu = daThanhToan + chuaThanhToan;

            ViewBag.DaThanhToan = daThanhToan;
            ViewBag.ChuaThanhToan = chuaThanhToan;
            ViewBag.TongDoanhThu = tongDoanhThu;

            return View();
        }



    }
}