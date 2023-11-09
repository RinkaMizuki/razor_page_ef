using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using razor_page_ef;

namespace App.Admin.User
{
    public class SetPasswordModel : PageModel
    {
        private readonly UserManager<AuthenUser> _userManager;
        private readonly SignInManager<AuthenUser> _signInManager;
        public AuthenUser user { get; set; }
        public SetPasswordModel(
            UserManager<AuthenUser> userManager,
            SignInManager<AuthenUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        public class InputModel
        {

            [Required(ErrorMessage = "{0} là bắt buộc")]
            [StringLength(100, ErrorMessage = "{0} phải có độ dài từ {2} đến {1} kí tự.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Mật khẩu")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Nhập lại mật khẩu")]
            [Compare("NewPassword", ErrorMessage = "Nhập lại mật khẩu không trùng khớp.")]
            public string ConfirmPassword { get; set; }
        }
        public async Task<IActionResult> OnGet(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                StatusMessage = "Không tìm thấy User";
                return RedirectToPage("./Index");
            }
            user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Không tìm thấy User");
            }
            return Page();
        }
        public async Task<IActionResult> OnPost(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                StatusMessage = "Không tìm thấy User";
                return RedirectToPage("./Index");
            }
            if (!ModelState.IsValid)
            {
                StatusMessage = "Dữ liệu không hợp lệ";
                return Page();
            }
            //Lưu ý Password sẽ đặt được khi PassHash là null
            user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.RemovePasswordAsync(user);
            }
            var result = await _userManager.AddPasswordAsync(user, Input.NewPassword);
            if (result.Succeeded)
            {
                StatusMessage = $"Đã thay đổi mật khẩu thành công : {user.UserName}";
                return Page();
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
