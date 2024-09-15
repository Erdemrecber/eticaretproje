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

        public List<CartItem> CartItems { get; set; } = new List<CartItem>(); // Varsayılan boş liste
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

            // Sepet öğelerini getir
            CartItems = await _cartService.GetCartItemsAsync(user.Id);

            
            if (CartItems == null || CartItems.Count == 0)
            {
                Console.WriteLine("Sepet boş.");
            }
            else
            {
                Console.WriteLine($"Sepetteki ürün sayısı: {CartItems.Count}");
                foreach (var item in CartItems)
                {
                    Console.WriteLine($"Ürün: {item.Product.Name}, Miktar: {item.Quantity}, Fiyat: {item.Product.Price}");
                }
            }

            // Toplam fiyatı hesapla
            TotalPrice = await _cartService.GetTotalPriceAsync(user.Id);

            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int productId, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            // Ürünü sepete ekle
            await _cartService.AddToCartAsync(productId, user.Id, quantity);

            
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

            // Ürünü sepetten kaldır
            await _cartService.RemoveFromCartAsync(productId, user.Id);

            
            Console.WriteLine($"Ürün ID: {productId} sepetten kaldırıldı.");

            
            return RedirectToPage();
        }
    }
}
