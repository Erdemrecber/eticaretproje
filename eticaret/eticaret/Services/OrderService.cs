using eticaret.Data;
using eticaret.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace eticaret.Services
{
    public class OrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateOrderAsync(string userName, List<CartItem> cartItems, string address, string city, string postalCode, string country)
        {
            
            var order = new Order
            {
                UserName = userName,
                Address = address,
                City = city,
                PostalCode = postalCode,
                Country = country,
                TotalPrice = cartItems.Sum(c => c.Quantity * c.Product.Price),
                OrderItems = new List<OrderItem>()
            };

            
            foreach (var cartItem in cartItems)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Product.Price
                });
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }
    }
}

