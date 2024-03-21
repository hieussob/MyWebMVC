using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebMVC.Data;
using MyWebMVC.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MyWebMVC.Controllers
{
    public class HangHoaController : Controller
    {
        private readonly TMHShopContext db;

        public HangHoaController(TMHShopContext context) {
            db = context;
        }
        
        public IActionResult Index(int? loai)
        {
            var hangHoa = db.HangHoas.AsQueryable();
            if(loai.HasValue)
            {
                hangHoa = hangHoa.Where(p=>p.MaLoai==loai);
            }

            var result = hangHoa.Select(p => new HangHoaVM
            {
                MaHangHoa = p.MaHh,
                TenHangHoa = p.TenHh,
                DonGia = p.DonGia ?? 0,
                Hinh = p.Hinh ?? "",
                MoTaNgan = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai
            });
            return View(result);
        }

        public IActionResult Seach()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Search(string query)
        {
            var hangHoa = db.HangHoas.AsQueryable();
            if (query!= null)
            {
                hangHoa = hangHoa.Where(p => p.TenHh.Contains(query));

            }

            var result = hangHoa.Select(p => new HangHoaVM
            {
                MaHangHoa = p.MaHh,
                TenHangHoa = p.TenHh,
                DonGia = p.DonGia ?? 0,
                Hinh = p.Hinh ?? "",
                MoTaNgan = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai
            });
            return View(result);
        }

        public IActionResult Detail(int maHangHoa)
        {
            var hangHoa = db.HangHoas.Include(p=>p.MaLoaiNavigation).SingleOrDefault(p => p.MaHh == maHangHoa);
            if(hangHoa == null)
            {
                return View();
            }
            var result = new ChiTietHangHoaVM
            {
                TenHangHoa = hangHoa.TenHh,
                TenLoai = hangHoa.MaLoaiNavigation.TenLoai,
                DonGia = hangHoa.DonGia ??0,
                MoTaNgan = hangHoa.MoTaDonVi ?? string.Empty,
                Hinh = hangHoa.Hinh ?? string.Empty,
                DiemDanhGia = 5,
                MoTaChiTiet= hangHoa.MoTa ?? string.Empty,
                SoLuongTon = 10,
                MaHangHoa=hangHoa.MaHh 

            };
            return View(result);
        }
    }
}
