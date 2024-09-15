using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using eticaret.Data;
using eticaret.Models;
using System.Threading.Tasks;


namespace eticaret.Pages.Admin
{
    public class AddProductModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AddProductModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ProductInputModel Input { get; set; }

        public class ProductInputModel
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string ImageUrl { get; set; }
            public string Description { get; set; }
            public int Stock { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var product = new Product
            {
                Name = Input.Name,
                Price = Input.Price,
                ImageUrl = Input.ImageUrl,
                Description = Input.Description,
                Stock = Input.Stock
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}
