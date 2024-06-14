using Microsoft.AspNetCore.Mvc;
using MyWebMVC.Data;

namespace MyWebMVC.ViewComponents
{
    public class TopLastestViewComponent: ViewComponent
    {
        private readonly TMHShopContext db;
        public TopLastestViewComponent(TMHShopContext context)
        {
            db= context;
        }
        public IViewComponentResult Invoke()
        {
            var data = db.HangHoas.OrderByDescending(p=>p.NgaySx).Take(5).ToList();
            return View(data);
        }
    }
}
