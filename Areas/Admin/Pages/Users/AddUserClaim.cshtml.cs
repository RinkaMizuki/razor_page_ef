using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using CS51_ASP.NET_Razor_EF_1;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using razor_page_ef;

namespace App.Admin.User
{
    public class AddUserClaimModel : PageModel
    {
        public class InputModel
        {
            [DisplayName("Loại Claim")]
            [Required, StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phải có độ dài từ {2} đến {1} kí tự")]
            public string UserClaimType { get; set; }
            [DisplayName("Giá trị Claim")]
            [Required, StringLength(256, MinimumLength = 3, ErrorMessage = "{0} có độ dài từ {2} đến {1} kí tự")]
            public string UserClaimValue { get; set; }
        }
        [TempData]
        public string StatusMessage { get; set; }
        public readonly RoleManager<IdentityRole> _roleManager;
        public readonly BlogContext _blogContext;
        public readonly UserManager<AuthenUser> _userManager;
        [BindProperty]
        public InputModel Input { get; set; }
        public AuthenUser user { get; set; }
        public AddUserClaimModel(RoleManager<IdentityRole> roleManager, BlogContext blogContext, UserManager<AuthenUser> userManager)
        {
            _blogContext = blogContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGet(string userId)
        {
            user = await _userManager.FindByIdAsync(userId);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string userId)
        {
            if (userId == null)
            {
                return NotFound("Không tìm thấy User");
            }
            if (!ModelState.IsValid)
            {
                StatusMessage = "Dữ liệu không hợp lệ";
                return Page();
            }
            user = await _userManager.FindByIdAsync(userId);
            if ((await _userManager.GetClaimsAsync(user)).Any(uc => uc.Type == Input.UserClaimType && uc.Value == Input.UserClaimValue))
            {
                StatusMessage = "Claim này đã tồn tại trong user";
                return Page();
            }

            var result = await _userManager.AddClaimAsync(user, new Claim(Input.UserClaimType, Input.UserClaimValue));
            if (result.Succeeded)
            {
                StatusMessage = $"Đã thêm thành công Claim riêng cho {user.UserName}";
                return RedirectToPage("./AddRole", new
                {
                    userId = user.Id,
                });
            }
            else
            {
                result.Errors.ToList().ForEach(err =>
                {
                    ModelState.AddModelError(string.Empty, err.Description);
                });
            }
            return Page();
        }
    }
}
