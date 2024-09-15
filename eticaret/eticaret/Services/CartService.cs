using eticaret.Data;
using eticaret.Models;
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

        // Sepete ürün ekleme
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

        // Toplam fiyatı hesaplama
        public async Task<decimal> GetTotalPriceAsync(string userId)
        {
            var cartItems = await _context.CartItems
                .Where(c => c.UserId == userId)
                .Include(c => c.Product)
                .ToListAsync();

            return cartItems.Sum(c => c.Quantity * c.Product.Price);
        }

        // Sepetteki ürünleri listeleme
        public async Task<List<CartItem>> GetCartItemsAsync(string userId)
        {
            var cartItems = await _context.CartItems
                .Where(c => c.UserId == userId)
                .Include(c => c.Product)  
                .ToListAsync();

            
            Console.WriteLine("Sepetteki Ürünler:");
            foreach (var item in cartItems)
            {
                Console.WriteLine($"Ürün: {item.Product.Name}, Miktar: {item.Quantity}, Fiyat: {item.Product.Price}");
            }

            return cartItems;
        }

        // Sepetten ürün kaldırma
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

        internal async Task AddToCartAsync(Product product, int quantity, string userId)
        {
            var cartItem = await _context.CartItems
        .FirstOrDefaultAsync(c => c.ProductId == product.Id && c.UserId == userId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ProductId = product.Id,
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
    }
}
