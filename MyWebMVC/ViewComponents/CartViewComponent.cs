using Microsoft.AspNetCore.Mvc;
using MyWebMVC.Helpers;
using MyWebMVC.ViewModels;

namespace MyWebMVC.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.Get <List<CartItem>>(MyConstant.CART_KEY) ?? new List<CartItem>();

            return View("CartPanel",new CartModel
            {
                quantity=cart.Sum(p=>p.SoLuong),
                total= cart.Sum(p=>p.ThanhTien)
            });
        }

        
    }
}
