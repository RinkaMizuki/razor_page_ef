using System.ComponentModel;
using CS51_ASP.NET_Razor_EF_1;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using razor_page_ef;

namespace App.Admin.User
{
    public class AddRoleModel : PageModel
    {
        public readonly RoleManager<IdentityRole> _roleManager;
        public readonly UserManager<AuthenUser> _userManager;
        public readonly BlogContext _blogContext;
        [BindProperty]
        [DisplayName("Tên các role")]
        public string[] RoleNames { get; set; }
        public SelectList selectListItems { get; set; }
        public List<IdentityRoleClaim<string>> RoleClaims { get; set; } = new List<IdentityRoleClaim<string>>();
        public List<IdentityUserClaim<string>> UserClaims { get; set; } = new List<IdentityUserClaim<string>>();
        public AuthenUser user { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        public AddRoleModel(UserManager<AuthenUser> userManager, RoleManager<IdentityRole> roleManager, BlogContext blogContext)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _blogContext = blogContext;
        }
        public async Task<IActionResult> OnGet(string userId)
        {
            if (userId == null)
            {
                return NotFound($"Không tìm thấy user có Id : {userId}");
            }
            user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Không tìm thấy User");
            }

            var listRole = from ur in _blogContext.UserRoles
                           join r in _blogContext.Roles
                           on ur.RoleId equals r.Id
                           where ur.UserId == userId
                           select r;

            RoleClaims = await (from c in _blogContext.RoleClaims
                                join lr in listRole
                                on c.RoleId equals lr.Id
                                select c).ToListAsync();

            UserClaims = await (from uc in _blogContext.UserClaims
                                where uc.UserId == userId
                                select uc).ToListAsync();

            RoleNames = (await _userManager.GetRolesAsync(user)).ToArray();
            var allRole = _roleManager.Roles.Select(r => r.Name).ToList();
            selectListItems = new SelectList(allRole);

            return Page();
        }
        public async Task<IActionResult> OnPost(string userId)
        {
            if (userId == null)
            {
                return NotFound($"Không tìm thấy user có Id : {userId}");
            }
            user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Không tìm thấy User");
            }

            //render lại danh sách tất cả các role
            var allRole = _roleManager.Roles.Select(r => r.Name).ToList();
            selectListItems = new SelectList(allRole);

            //lấy ra các role hiện tại
            var OldRoleNames = await _userManager.GetRolesAsync(user);

            //xóa những role k tồn tại trong các role đã thay đổi
            var deleteRole = OldRoleNames.Where(r => !RoleNames.Contains(r));

            //thêm những role đã thay đổi
            var addRole = RoleNames.Where(r => !OldRoleNames.Contains(r));
            //giữ các role mà nó không thay đổi
            var resultDeleted = await _userManager.RemoveFromRolesAsync(user, deleteRole);

            if (resultDeleted.Succeeded)
            {
                var resultAdded = await _userManager.AddToRolesAsync(user, addRole);
                if (resultAdded.Succeeded)
                {
                    StatusMessage = $"Đã cập nhật role thành công cho : {user.UserName}";
                    return RedirectToPage("./Index", new { p = 1 });
                }
                else
                {
                    resultAdded.Errors.ToList().ForEach(err =>
                    {
                        ModelState.AddModelError(string.Empty, err.Description);
                    });
                }
            }
            else
            {
                resultDeleted.Errors.ToList().ForEach(err =>
                {
                    ModelState.AddModelError(string.Empty, err.Description);
                });
            }
            StatusMessage = "Thêm role thất bại";
            return Page();
        }
    }
}
