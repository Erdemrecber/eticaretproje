using eticaret.Models;
using eticaret.Services; 
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using eticaret.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace eticaret.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly CartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(ApplicationDbContext context, CartService cartService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _cartService = cartService;
            _userManager = userManager;
        }

        public IList<Product> Products { get; set; } = default!;
        public decimal TotalPrice { get; set; }
        public IList<CartItem> CartItems { get; set; } = new List<CartItem>();

        public async Task OnGetAsync()
        {
            Products = await _context.Products.ToListAsync();

            var userId = User.Identity.Name;
            if (userId != null)
            {
                CartItems = await _cartService.GetCartItemsAsync(userId);
                TotalPrice = await _cartService.GetTotalPriceAsync(userId);
            }
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int productId, int quantity)
        {
            var userId = User.Identity.Name;
            if (userId == null)
            {
                return RedirectToPage("/Account/Login");
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            await _cartService.AddToCartAsync(product, quantity, userId);
            return RedirectToPage(); 
        }
    }
}
