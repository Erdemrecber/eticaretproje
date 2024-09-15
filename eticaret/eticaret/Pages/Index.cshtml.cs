using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using eticaret.Data;
using eticaret.Models;
using eticaret.Services;

namespace eticaret.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly CartService _cartService;

        public IndexModel(ApplicationDbContext context, CartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        public IList<Product> Products { get; set; } = new List<Product>();

        public async Task OnGetAsync()
        {
            Products = await _context.Products.ToListAsync() ?? new List<Product>();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int id)
        {
            var userId = User.Identity.Name; 
            if (userId == null)
            {
                return RedirectToPage("/Account/Login");
            }

            await _cartService.AddToCartAsync(id, userId); 
            return RedirectToPage(); 
        }
    }
}
