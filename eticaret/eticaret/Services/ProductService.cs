using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eticaret.Data;
using eticaret.Models;
using Microsoft.EntityFrameworkCore;

namespace eticaret.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Ürünleri listeleme
        public async Task<List<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        // Ürün detaylarını alma
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        // Ürün ekleme
        public async Task AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        // Ürün güncelleme
        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        // Ürün silme
        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        // Öne çıkan ürünleri getirme
        public async Task<List<Product>> GetFeaturedProductsAsync()
        {
            
            return await _context.Products
                .Where(p => p.IsFeatured) 
                .ToListAsync();
        }
    }
}

