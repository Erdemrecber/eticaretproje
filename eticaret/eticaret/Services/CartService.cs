using eticaret.Data;
using eticaret.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace eticaret.Services
{
    public class CartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

       
        public async Task AddToCartAsync(int productId, string userId, int quantity = 1)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.ProductId == productId && c.UserId == userId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ProductId = productId,
                    UserId = userId,
                    Quantity = quantity
                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
                _context.CartItems.Update(cartItem);
            }

            
            await _context.SaveChangesAsync();
        }

        
        public async Task<decimal> GetTotalPriceAsync(string userId)
        {
            var cartItems = await _context.CartItems
                .Where(c => c.UserId == userId)
                .Include(c => c.Product)
                .ToListAsync();

            return cartItems.Sum(c => c.Quantity * c.Product.Price);
        }

        
        public async Task<List<CartItem>> GetCartItemsAsync(string userName)
        {
            
            var cartItems = await _context.CartItems
                .Where(c => c.UserId == userName)  
                .Include(c => c.Product)  
                .ToListAsync();

            return cartItems;
        }



        
        public async Task RemoveFromCartAsync(int productId, string userId)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.ProductId == productId && c.UserId == userId);

            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();  
            }
        }

        public async Task AddToCartAsync(int productId, int quantity, string userName)
        {
            
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.ProductId == productId && c.UserId == userName);  

            if (cartItem == null)
            {
                
                cartItem = new CartItem
                {
                    ProductId = productId,
                    UserId = userName,  
                    Quantity = quantity
                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                
                cartItem.Quantity += quantity;
                _context.CartItems.Update(cartItem);
            }

            await _context.SaveChangesAsync();  
        }

        public async Task ClearCartAsync(string userId)
        {
            var cartItems = await _context.CartItems
                .Where(c => c.UserId == userId)
                .ToListAsync();

            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }




    }
}
