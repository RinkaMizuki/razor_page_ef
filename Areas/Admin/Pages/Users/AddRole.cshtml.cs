using System.ComponentModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using razor_page_ef;

namespace App.Admin.User
{
    public class AddRoleModel : PageModel
    {
        public readonly RoleManager<IdentityRole> _roleManager;
        public readonly UserManager<AuthenUser> _userManager;
        [BindProperty]
        [DisplayName("Tên các role")]
        public string[] RoleNames { get; set; }
        public SelectList selectListItems { get; set; }
        public AuthenUser user { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        public AddRoleModel(UserManager<AuthenUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
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
