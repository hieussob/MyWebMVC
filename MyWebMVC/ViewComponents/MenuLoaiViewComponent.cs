using Microsoft.AspNetCore.Mvc;
using MyWebMVC.Data;
using MyWebMVC.ViewModels;

namespace MyWebMVC.ViewComponents
{
    public class MenuLoaiViewComponent : ViewComponent
    {
        private readonly TMHShopContext db;
        public MenuLoaiViewComponent(TMHShopContext context)
        {
            db = context;
        }

        public IViewComponentResult Invoke()
        {
            var data = db.Loais.Select(loai => new MenuLoaiVM {
                MaLoai= loai.MaLoai,
                TenLoai= loai.TenLoai,
                SoLuong = loai.HangHoas.Count
            }).OrderByDescending(p=>p.SoLuong);
            return View(data); //Default.cshtml
        }
    }
}
