﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace KoiOrderingSystemInJapan.MVCWebApp.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly SignInManager<ApplicationUser> _signInManager;
        //private readonly RoleManager<IdentityRole> _roleManager;
        //private readonly ILogger<RegisterModel> _logger;
        //private readonly IEmailService _emailService;

        //public RegisterModel(
        //    UserManager<ApplicationUser> userManager,
        //    SignInManager<ApplicationUser> signInManager,
        //    RoleManager<IdentityRole> roleManager,
        //    ILogger<RegisterModel> logger,
        //    IEmailService emailService)
        //{
        //    _userManager = userManager;
        //    _signInManager = signInManager;
        //    _roleManager = roleManager;
        //    _logger = logger;
        //    _emailService = emailService;
        //}

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<IdentityRole> Roles { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(50, ErrorMessage = "Max 50 characters allowed")]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(50, ErrorMessage = "Max 50 characters allowed")]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [StringLength(12, ErrorMessage = "IDNumber cannot be longer than 13 characters.")]
            [RegularExpression(@"^\d{12}$", ErrorMessage = "IDNumber must be exactly 13 digits.")]
            public string IDNumber { get; set; }

            [Required]
            public string SelectedRole { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            //ReturnUrl = returnUrl;
            //Roles = await _roleManager.Roles.ToListAsync();
            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            return Page();
        }

        //public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        //{
        //    returnUrl ??= Url.Content("~/");
        //    Roles = _roleManager.Roles.ToList();
        //    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        //    if (ModelState.IsValid)
        //    {
        //        var user = new ApplicationUser
        //        {
        //            UserName = Input.Email,
        //            Email = Input.Email,
        //            FirstName = Input.FirstName,
        //            LastName = Input.LastName,
        //            IDNumber = Input.IDNumber,
        //             EmailConfirmed = true   //Marking email confirmation true for now.
        //        };

        //        var result = await _userManager.CreateAsync(user, Input.Password);

        //        if (result.Succeeded)
        //        {
        //            // Assign the selected role to the user
        //            if (!string.IsNullOrEmpty(Input.SelectedRole))
        //            {
        //                await _userManager.AddToRoleAsync(user, Input.SelectedRole);
        //            }

        //            _logger.LogInformation("User created a new account with password.");

        //            var userId = await _userManager.GetUserIdAsync(user);
        //            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        //            var callbackUrl = Url.Page(
        //                "/Account/ConfirmEmail",
        //                pageHandler: null,
        //                values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
        //                protocol: Request.Scheme);

        //            await _emailService.SendConfirmationEmailAsync(Input.Email, callbackUrl);

        //            var qrCodeImage = _emailService.GenerateQrCode(user.Email);
        //            var qrCodeBase64 = Convert.ToBase64String(qrCodeImage);

        //            await _emailService.SendQrCodeEmailAsync(Input.Email, qrCodeBase64);

        //            if (_userManager.Options.SignIn.RequireConfirmedAccount)
        //            {
        //                return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
        //            }
        //            else
        //            {
        //                await _signInManager.SignInAsync(user, isPersistent: false);
        //                return LocalRedirect(returnUrl);
        //            }
        //        }

        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError(string.Empty, error.Description);
        //        }
        //    }

        //    // Redisplay the form if we got this far (meaning something failed)
        //    return Page();
        //}
    }
}
