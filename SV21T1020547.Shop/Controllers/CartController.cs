using Microsoft.AspNetCore.Mvc;
using SV21T1020547.Shop.Models;

namespace SV21T1020547.Shop.Controllers
{
    public class CartController : Controller
    {
        private const string SHOPPING_CART = "ShoppingCart";

        public IActionResult Index()
        {
            return View();
        }

        private static List<CartItem> GetShoppingCart()
        {
            var shoppingCart = ApplicationContext.GetSessionData<List<CartItem>>(SHOPPING_CART);
            if (shoppingCart == null)
            {
                shoppingCart = new List<CartItem>();
                ApplicationContext.SetSessionData(SHOPPING_CART, shoppingCart);
            }

            return shoppingCart;
        }
    }
}
