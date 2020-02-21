using IISAdministration.Extensions;
using IISAdministration.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace IISAdministration.Areas.Identity.Pages.Admin.Accounts
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(UserManager<IdentityUser> userManager, IEmailSender emailSender, ILogger<CreateModel> logger)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        [BindProperty]
        public string Email { get; set; }


        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Email == null)
                return BadRequest();

            var user = new IdentityUser()
            {
                UserName = Email,
                Email = Email,
            };

            var identityResult = await _userManager.CreateAsync(user);

            if (identityResult.Succeeded)
            {
                _logger.LogInformation("User created a new account.");
                await SendActivationEmail(user.Email);
                TempData.AddAlert(Alert.Success($"Successfully created user {user.Email}. Have the user check his/her email to create a password"));
                return RedirectToPage("./Index");
            }
            else
            {
                TempData.AddAlert(Alert.Error($"Failed to create user. {identityResult.ToString()}"));
                return Page();
            }
        }

        private async Task SendActivationEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return;
            }

            // For more information on how to enable account confirmation and password reset please 
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/Register",
                pageHandler: null,
                values: new { area = "Identity", code, email },
                protocol: Request.Scheme);
            var subject = "Welcome to Warehouse OS Metrics!";
            var body = $"<h4>Welcome to IIS Administration!</h4><p>To get started, please <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>activate</a> your account.</p><p>The account must be activated within 24 hours from receving this mail.</p>";
            await _emailSender.SendEmailAsync(email, subject, body);
        }
    }
}