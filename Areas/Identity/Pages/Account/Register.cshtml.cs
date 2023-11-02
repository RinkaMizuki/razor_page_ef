// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using razor_page_ef;

namespace CS51_ASP.NET_Razor_EF_1.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AuthenUser> _signInManager;
        private readonly UserManager<AuthenUser> _userManager;
        private readonly IUserStore<AuthenUser> _userStore;
        private readonly IUserEmailStore<AuthenUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        public class InputModel
        {
            [Required(ErrorMessage = "Trường {0} là bắt buộc.")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "{0} có độ dài từ {2} đến {1} kí tự.", MinimumLength = 3)]
            [DataType(DataType.Password)]
            [Display(Name = "Mật khẩu")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Nhập lại mật khẩu")]
            [Compare("Password", ErrorMessage = "{0} không trùng khớp.")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "Trường {0} là bắt buộc.")]
            [DisplayName("Tên người dùng")]
            [DataType(DataType.Text)]
            public string UserName { get; set; }
        }
        private IUserEmailStore<AuthenUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("Không hỗ trợ việc xác thực bằng Email");
            }
            return (IUserEmailStore<AuthenUser>)_userStore;
        }
        private AuthenUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<AuthenUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(AuthenUser)}'. " +
                    $"Ensure that '{nameof(AuthenUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
        public RegisterModel(
            UserManager<AuthenUser> userManager,
            IUserStore<AuthenUser> userStore,
            SignInManager<AuthenUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }
        //ExternalLogins nó chứa các Service login bên ngoài (FB, Google,...)
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                //khi Valid thành công thì tạo ra một User
                var user = CreateUser();

                //thêm các trường dữ liệu vào trong UserStore
                await _userStore.SetUserNameAsync(user, Input.UserName, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                //Tiến hành tạo một User với Password và email và username
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Đã tạo mới một User.");
                    //lấy ID của User
                    var userId = await _userManager.GetUserIdAsync(user);
                    //tạo mã thông báo Confirm gửi đến Email của User
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //Encode mã thông báo bằng Base64
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme); //protocol là lấy luôn phần hostname (http://localhost:5154)

                    await _emailSender.SendEmailAsync(Input.Email, "Xác nhận email của bạn cho trang Razor",
                        $"Hãy xác thực email của bạn thông qua <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Nhấn vào đây.</a>.");
                    // nếu có options SignIn.RequireConfirmedAccount thì xác nhận trước rồi mới login
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    //ngược lại cho vào trang login luôn rồi xác thực sau
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
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
