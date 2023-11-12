using System.Security.Claims;
using CS51_ASP.NET_Razor_EF_1;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razor_page_ef;

namespace App.Admin.User
{
    public class DeleteClaimUserModel : PageModel
    {

        public readonly UserManager<AuthenUser> _userManager;

        public readonly BlogContext _blogContext;
        public DeleteClaimUserModel(UserManager<AuthenUser> userManager, BlogContext blogContext)
        {
            _userManager = userManager;
            _blogContext = blogContext;
        }
        [TempData]
        public string StatusMessage { get; set; }
        public async Task<IActionResult> OnGet(string userId)
        {
            if (userId == null)
            {
                return NotFound("Không tìm thấy User");
            }
            var user = await _userManager.FindByIdAsync(userId);
            var deletedClaimUser = await (from uc in _blogContext.UserClaims
                                          where userId == uc.UserId
                                          select uc).FirstOrDefaultAsync();
            var result = await _userManager.RemoveClaimAsync(user, new Claim(deletedClaimUser.ClaimType, deletedClaimUser.ClaimValue));
            if (result.Succeeded)
            {
                StatusMessage = "Đã xóa thành công Claim riêng của user";
            }
            else
            {
                result.Errors.ToList().ForEach(err =>
                {
                    ModelState.AddModelError(string.Empty, err.Description);
                });
            }
            return RedirectToPage("./AddRole", new
            {
                userId = user.Id,
            });
        }
    }
}
