using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using eticaret.Models; 
using System.Threading.Tasks;

namespace eticaret.Pages.Shared
{
    public class _LayoutModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public IdentityUser CurrentUser { get; set; }

        public _LayoutModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task OnGetAsync()
        {
            if (_signInManager.IsSignedIn(User))
            {
                CurrentUser = await _userManager.GetUserAsync(User);
            }
        }
    }
}
