using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Đồ_án_web_quản_lí_phòng_trọ.Models
{
    public class TongDoanhThuModel
    {
        [Key]
        public int Thang { get; set; }

        public int TongDoanhThu { get; set; }
    }
}