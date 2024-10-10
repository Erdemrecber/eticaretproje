using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using eticaret.Data;
using eticaret.Models;
using System.Threading.Tasks;

namespace eticaret.Pages.Admin
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _context.Products.FindAsync(id);

            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            
            Console.WriteLine("POST isteği geldi!");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("Model doğrulaması başarısız.");

                foreach (var modelState in ViewData.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }

                return Page();
            }

            
            var productToUpdate = await _context.Products.FindAsync(Product.Id);

            if (productToUpdate == null)
            {
                Console.WriteLine("Ürün bulunamadı.");
                return NotFound();
            }

            productToUpdate.Name = Product.Name;
            productToUpdate.Price = Product.Price;
            productToUpdate.Description = Product.Description;
            productToUpdate.Stock = Product.Stock;
            productToUpdate.ImageUrl = Product.ImageUrl;

            try
            {
                
                await _context.SaveChangesAsync();
                Console.WriteLine($"Ürün güncellendi: {Product.Name}, Fiyat: {Product.Price}");
            }
            catch (DbUpdateException ex)
            {
                
                Console.WriteLine($"Güncelleme hatası: {ex.Message}");
                ModelState.AddModelError(string.Empty, "Ürünü güncellerken bir hata oluştu.");
                return Page();
            }

            return RedirectToPage("/Admin/Dashboard");
        }
    }
}
