using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Owlvey.Falcon.Authority.Infra.Settings;
using Owvley.Falcon.Authority.Domain.Models;
using TheRoostt.Authority.Domain.Interfaces;
using static TheRoostt.Authority.Domain.DomainConstants;

namespace Owlvey.Falcon.Authority.Presentation.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        readonly ICustomerRepository _customerRepository;
        readonly IUserRepository _userRepository;
        readonly WebSettings _webSettings;
        
        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            ICustomerRepository customerRepository,
            IUserRepository userRepository,
            IOptions<WebSettings> webSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _customerRepository = customerRepository;
            _userRepository = userRepository;
            _webSettings = webSettings.Value;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

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

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new User { UserName = Input.Email, Email = Input.Email, FirstName = Input.FirstName, LastName = Input.LastName };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    var existingUser = await _userRepository.GetUserById(user.Id);
                    existingUser.AddPreference(Preferences.DateFormat.Name, Preferences.DateFormat.DefaultValue);
                    existingUser.AddPreference(Preferences.TimeFormat.Name, Preferences.TimeFormat.DefaultValue);
                    existingUser.AddPreference(Preferences.TimeZone.Name, Preferences.TimeZone.DefaultValue);
                    existingUser.AddPreference(Preferences.Theme.Name, Preferences.Theme.DefaultValue);
                    _userRepository.Update(existingUser);
                    await _userRepository.SaveChanges();

                    await _userManager.AddClaimAsync(user, new Claim("givenname", Input.FirstName));
                    await _userManager.AddClaimAsync(user, new Claim("fullname", $"{Input.FirstName} {Input.LastName}"));
                    await _userManager.AddClaimAsync(user, new Claim(Preferences.DateFormat.ClaimName, Preferences.DateFormat.DefaultValue));
                    await _userManager.AddClaimAsync(user, new Claim(Preferences.TimeFormat.ClaimName, Preferences.TimeFormat.DefaultValue));
                    await _userManager.AddClaimAsync(user, new Claim(Preferences.TimeZone.ClaimName, Preferences.TimeZone.DefaultValue));
                    await _userManager.AddClaimAsync(user, new Claim(Preferences.Theme.ClaimName, Preferences.Theme.DefaultValue));
                    await _userManager.AddClaimAsync(user, new Claim(Preferences.Customer.ClaimName, Preferences.Customer.DefaultValue));

                    await _userManager.AddToRoleAsync(user, Roles.Guest);

                    _logger.LogInformation("User created a new account with password.");
                    
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
