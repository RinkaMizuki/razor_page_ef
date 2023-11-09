using CS51_ASP.NET_Razor_EF_1;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Admin.Roles
{
    public class DeleteModel : RolePageModel
    {
        public DeleteModel(RoleManager<IdentityRole> roleManager, BlogContext blogContext) : base(roleManager, blogContext)
        {
        }
        public IdentityRole role { get; set; }
        public async Task<IActionResult> OnGet(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
            {
                return NotFound($"Không tìm thấy {roleId}");
            }
            var findRole = await _roleManager.FindByIdAsync(roleId);
            if (findRole != null)
            {
                role = findRole;
            }
            else
            {
                return NotFound($"Không tìm thấy role");
            }
            return Page();
        }
        public async Task<IActionResult> OnPost(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
            {
                return NotFound($"Không tìm thấy {roleId}");
            }
            var findRole = await _roleManager.FindByIdAsync(roleId);
            if (findRole != null)
            {
                role = findRole;
            }
            else
            {
                return NotFound($"Không tìm thấy role");
            }
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                StatusMessage = $"Đã xóa thành công role : {role.Name}";
                return RedirectToPage("./Index");
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
