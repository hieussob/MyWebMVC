using Microsoft.AspNetCore.Mvc;
using MyWebMVC.Data;
using MyWebMVC.ViewModels;

namespace MyWebMVC.ViewComponents
{
    public class TopSalesViewComponent:ViewComponent
    {
        private readonly TMHShopContext db;
        public TopSalesViewComponent(TMHShopContext context)
        {
            db = context;
        }

        public IViewComponentResult Invoke()
        {
            var data = db.HoaDons
                .SelectMany(hd => hd.ChiTietHds)
                .GroupBy(ct => new { ct.MaHh, ct.MaHhNavigation.TenHh, ct.MaHhNavigation.Hinh,ct.MaHhNavigation.DonGia })
                .Select(g => new HangHoaVM
                {
                    MaHangHoa = g.Key.MaHh,
                    TenHangHoa = g.Key.TenHh,
                    Hinh = g.Key.Hinh,
                    DonGia = double.Parse(g.Key.DonGia.ToString()),
                    SolanXuatHien = g.Count()
                })
                .OrderByDescending(x => x.SolanXuatHien)
                .Take(3)
                .ToList();
            return View(data);
        }
    }
}
