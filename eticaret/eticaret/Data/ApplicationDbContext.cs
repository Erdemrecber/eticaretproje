
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using eticaret.Models;
using Microsoft.AspNetCore.Identity;


namespace eticaret.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser> 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } // Ürünler tablosu
        public DbSet<CartItem> CartItems { get; set; } // Sepet tablosu
    }
}








public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    public string Description { get; internal set; }
    public int Stock { get; internal set; }
    public string ImageUrl { get; internal set; }
    public bool IsFeatured { get; internal set; }
}




