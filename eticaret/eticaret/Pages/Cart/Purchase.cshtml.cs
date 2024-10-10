using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using eticaret.Models;
using eticaret.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eticaret.Pages.Cart
{
    public class PurchaseModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly CartService _cartService;
        private readonly OrderService _orderService;

        public PurchaseModel(UserManager<IdentityUser> userManager, CartService cartService, OrderService orderService)
        {
            _userManager = userManager;
            _cartService = cartService;
            _orderService = orderService;
        }

        [BindProperty]
        public string Address { get; set; }
        [BindProperty]
        public string City { get; set; }
        [BindProperty]
        public string PostalCode { get; set; }
        [BindProperty]
        public string Country { get; set; }

        public List<CartItem> CartItems { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            CartItems = await _cartService.GetCartItemsAsync(user.UserName);
            if (CartItems == null || CartItems.Count == 0)
            {
                return RedirectToPage("/Cart");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            
            CartItems = await _cartService.GetCartItemsAsync(user.UserName);
            if (CartItems == null || CartItems.Count == 0)
            {
                return RedirectToPage("/Cart");
            }

            
            await _orderService.CreateOrderAsync(user.UserName, CartItems, Address, City, PostalCode, Country);

            
            await _cartService.ClearCartAsync(user.UserName);

            return RedirectToPage("/Order/Success");  
        }
    }
}
