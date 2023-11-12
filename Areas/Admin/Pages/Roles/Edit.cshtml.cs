using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CS51_ASP.NET_Razor_EF_1;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Admin.Roles
{
    public class EditModel : RolePageModel
    {
        public class InputModel
        {
            [DisplayName("Tên role")]
            [Required, StringLength(256, MinimumLength = 3, ErrorMessage = "{0} phải có độ dài từ {2} đến {1} kí tự")]
            public string Name { get; set; }
        }
        public EditModel(RoleManager<IdentityRole> roleManager, BlogContext blogContext) : base(roleManager, blogContext)
        {
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public List<IdentityRoleClaim<string>> roleClaims { get; set; }
        public IdentityRole Role { get; set; }
        public async Task<IActionResult> OnGet(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
            {
                return NotFound("Không tìm thấy role");
            }
            Role = await _roleManager.FindByIdAsync(roleId);
            if (Role != null)
            {
                roleClaims = await (from c in _blogContext.RoleClaims
                                    where c.RoleId == Role.Id
                                    select c).ToListAsync();
                Input = new InputModel()
                {
                    Name = Role.Name,
                };
            }
            return Page();
        }
        public async Task<IActionResult> OnPost(string roleId)
        {
            if (string.IsNullOrEmpty(roleId))
            {
                return NotFound("Không tìm thấy role");
            }
            Role = await _roleManager.FindByIdAsync(roleId);
            if (Role == null)
            {
                return Page();
            }
            Role.Name = Input.Name;
            var result = await _roleManager.UpdateAsync(Role);
            if (result.Succeeded)
            {
                StatusMessage = $"Bạn vừa đổi tên role thành : {Input.Name}";
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
