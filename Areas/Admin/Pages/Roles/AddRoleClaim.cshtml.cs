using System.Security.Claims;
using CS51_ASP.NET_Razor_EF_1;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace App.Admin.Roles
{
    public class AddRoleClaimModel : PageModel
    {
        public class InputModel
        {
            public string TypeClaim { get; set; }
            public string NameClaim { get; set; }
        }
        [BindProperty]
        public InputModel Input { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        public readonly RoleManager<IdentityRole> _roleManager;
        public readonly BlogContext _blogContext;
        public AddRoleClaimModel(RoleManager<IdentityRole> roleManager,
            BlogContext blogContext
        )
        {
            _roleManager = roleManager;
            _blogContext = blogContext;
        }
        public void OnGetAsync()
        {

        }
        public async Task<IActionResult> OnPostAsync(string roleId)
        {
            if (roleId == null)
            {
                return NotFound("Không tìm thấy role");
            }
            var role = await _roleManager.FindByIdAsync(roleId);
            if ((await _roleManager.GetClaimsAsync(role)).Any(c => c.Type == Input.TypeClaim && c.Value == Input.NameClaim))
            {
                StatusMessage = "Claim cho role này đã tồn tại";
                return Page();
            }
            var result = await _roleManager.AddClaimAsync(role, new Claim(Input.TypeClaim, Input.NameClaim));
            if (result.Succeeded)
            {
                StatusMessage = $"Thêm claim cho {role.Name} thành công";
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
