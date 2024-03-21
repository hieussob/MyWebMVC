using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebMVC.Data;
using MyWebMVC.Models;
using MyWebMVC.ViewModels;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace MyWebMVC.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
		private readonly TMHShopContext db;

		//public HomeController(ILogger<HomeController> logger)
  //      {
  //          _logger = logger;
  //      }

        public HomeController(TMHShopContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            var dsLoai = db.Loais.ToList();
            return View(dsLoai);
        }

        public IActionResult Privacy(int maSanPham)
        {
            var sanPham = db.HangHoas.Include(p => p.MaLoaiNavigation).SingleOrDefault(p => p.MaHh == maSanPham);

            if (sanPham == null)
            {
                return Redirect("/Index");
            }

            var result = new HangHoa
            {
                MaHh=sanPham.MaHh,
                TenHh=sanPham.TenHh,
                DonGia=sanPham.DonGia,
            };


            return View(sanPham);
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}