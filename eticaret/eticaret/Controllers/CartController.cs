using Microsoft.AspNetCore.Mvc;
using eticaret.Services;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace eticaret.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService _cartService;
        private readonly UserManager<IdentityUser> _userManager;

        public CartController(CartService cartService, UserManager<IdentityUser> userManager)
        {
            _cartService = cartService;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account"); 
            }

            await _cartService.AddToCartAsync(productId, user.Id);
            return RedirectToAction("Index", "Home"); 
        }

        public async Task<IActionResult> GetTotalPrice()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var totalPrice = await _cartService.GetTotalPriceAsync(user.Id);
            ViewBag.TotalPrice = totalPrice;
            return View(); 
        }
    }
}
