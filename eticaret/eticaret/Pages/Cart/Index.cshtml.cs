using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using eticaret.Data;
using eticaret.Models;
using eticaret.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eticaret.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly CartService _cartService;

        public List<CartItem> CartItems { get; set; } = new List<CartItem>(); 
        public decimal TotalPrice { get; set; }

        public IndexModel(ApplicationDbContext context, UserManager<IdentityUser> userManager, CartService cartService)
        {
            _context = context;
            _userManager = userManager;
            _cartService = cartService;
        }

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
                Console.WriteLine("Sepet boş.");
            }
            else
            {
                Console.WriteLine($"Sepetteki ürün sayısı: {CartItems.Count}");
            }

            TotalPrice = await _cartService.GetTotalPriceAsync(user.UserName); 

            return Page();
        }

        public async Task<decimal> GetTotalPriceAsync(string userId)
        {
            
            var cartItems = await _context.CartItems
                .Where(c => c.UserId == userId)
                .Include(c => c.Product) 
                .ToListAsync();

            return cartItems.Sum(c => c.Quantity * c.Product.Price); 
        }


        public async Task<IActionResult> OnPostAddToCartAsync(int productId, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            
            await _cartService.AddToCartAsync(productId, quantity, user.UserName);  

            Console.WriteLine($"Ürün ID: {productId} sepete eklendi. Miktar: {quantity}");

            return RedirectToPage();
        }







        public async Task<IActionResult> OnPostRemoveFromCartAsync(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            
            await _cartService.RemoveFromCartAsync(productId, user.UserName);

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostPurchaseAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            

            
            return RedirectToPage("/Cart/Purchase");  
        }



    }
}
