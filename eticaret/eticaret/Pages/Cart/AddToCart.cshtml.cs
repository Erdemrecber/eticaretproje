using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using eticaret.Data;
using eticaret.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace eticaret.Pages.Cart
{
    public class AddToCartModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AddToCartModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnPostAsync(int productId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToPage("/Account/Login");
                }

                
                var cartItem = await _context.CartItems
                    .FirstOrDefaultAsync(c => c.ProductId == productId && c.UserId == user.Id);

                if (cartItem == null)
                {
                   
                    cartItem = new CartItem
                    {
                        UserId = user.Id,
                        ProductId = productId,
                        Quantity = 1
                    };
                    await _context.CartItems.AddAsync(cartItem);
                    Console.WriteLine($"Yeni ürün eklendi: ProductId = {productId}, Quantity = 1");
                }
                else
                {
                   
                    cartItem.Quantity++;
                    _context.CartItems.Update(cartItem);
                    Console.WriteLine($"Ürün zaten sepette. Miktar arttırıldı: ProductId = {productId}, Quantity = {cartItem.Quantity}");
                }

                await _context.SaveChangesAsync();

                
                return RedirectToPage("/Cart/Index");
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Hata: {ex.Message}");
                return RedirectToPage("/Error"); 
            }
        }
    }
}
