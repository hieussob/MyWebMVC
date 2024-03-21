using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyWebMVC.ViewModels;
using System.ComponentModel;
using MyWebMVC.Helpers;

namespace MyWebMVC.ViewComponents
{
    public class CartDetailViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>(MyConstant.CART_KEY) ?? new List<CartItem>();
            return View(cart);
        }
        
    }
}
