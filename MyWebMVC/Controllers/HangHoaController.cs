using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebMVC.Data;
using MyWebMVC.ViewModels;
using X.PagedList;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MyWebMVC.Controllers
{
    public class HangHoaController : Controller
    {
        private readonly TMHShopContext db;

        public HangHoaController(TMHShopContext context) {
            db = context;
        }
        
        public IActionResult Index(int? loai, int? page)
        {
            int pageSize = 9;
            int pageNumber = page ==null || page < 0 ? 1 : page.Value;
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
            PagedList<HangHoaVM> list = new PagedList<HangHoaVM>(result, pageNumber, pageSize);
            return View(list);
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
        
        public IActionResult SeachByPrice()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SearchByPrice(int ?rangeInput)
        {
            var hangHoa = db.HangHoas.AsQueryable();
            if (rangeInput!= null)
            {
                hangHoa = hangHoa.Where(p => p.DonGia <= rangeInput);
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

            var list = db.HangHoas.Include(p => p.MaLoaiNavigation).Where(p => p.MaLoai == hangHoa.MaLoai && p.MaHh != maHangHoa).Take(5).ToList();
            TempData["dsachLienQuan"] = list;
            return View(result);
        }
    }
}
