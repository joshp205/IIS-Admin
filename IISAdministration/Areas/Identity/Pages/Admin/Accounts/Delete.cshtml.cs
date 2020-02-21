using IISAdministration.Extensions;
using IISAdministration.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace IISAdministration.Areas.Identity.Pages.Admin.Accounts
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public DeleteModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public IdentityUser IdentityUser { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            IdentityUser = await _userManager.FindByIdAsync(id);

            if (IdentityUser == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
                return NotFound();
            IdentityUser = await _userManager.FindByIdAsync(id);

            if (IdentityUser == null)
                return NotFound();

            await _userManager.DeleteAsync(IdentityUser);
            TempData.AddAlert(Alert.Success($"Successfully Deleted User {IdentityUser.Email}"));
            return RedirectToPage("Index");
        }
    }
}