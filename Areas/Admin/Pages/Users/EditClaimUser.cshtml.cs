using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CS51_ASP.NET_Razor_EF_1;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using razor_page_ef;

namespace App.Admin.User
{
    public class EditClaimUserModel : PageModel
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
        public EditClaimUserModel(RoleManager<IdentityRole> roleManager, BlogContext blogContext, UserManager<AuthenUser> userManager)
        {
            _blogContext = blogContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            if (userId == null)
            {
                return NotFound("Không tìm thấy user");
            }
            user = await _userManager.FindByIdAsync(userId);
            var claimOfUser = await (from uc in _blogContext.UserClaims
                                     where uc.UserId == userId
                                     select uc).FirstOrDefaultAsync();
            if (claimOfUser == null)
            {
                return NotFound("Không tìm thấy Claim của User");
            }
            Input = new InputModel()
            {
                UserClaimType = claimOfUser.ClaimType,
                UserClaimValue = claimOfUser.ClaimValue,
            };
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
            }
            user = await _userManager.FindByIdAsync(userId);

            if (_blogContext.UserClaims.Any(uc => uc.ClaimType == Input.UserClaimType && uc.ClaimValue == Input.UserClaimValue && uc.UserId == userId))
            {
                StatusMessage = "Claim riêng này đã tồn tại";
                return Page();
            }
            var editClaim = await (from uc in _blogContext.UserClaims
                                   where userId == uc.UserId
                                   select uc).FirstOrDefaultAsync();
            editClaim.ClaimType = Input.UserClaimType;
            editClaim.ClaimValue = Input.UserClaimValue;

            var result = await _blogContext.SaveChangesAsync();
            return RedirectToPage("./AddRole", new
            {
                userId = user.Id,
            });
        }
    }
}
