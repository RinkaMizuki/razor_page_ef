using CS51_ASP.NET_Razor_EF_1;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Admin.Roles
{
    public class DeleteClaimModel : PageModel
    {
        [TempData]
        public string StatusMessage { get; set; }
        public readonly BlogContext _blogContext;
        public readonly RoleManager<IdentityRole> _roleManager;
        public DeleteClaimModel(RoleManager<IdentityRole> roleManager, BlogContext blogContext)
        {
            _roleManager = roleManager;
            _blogContext = blogContext;
        }
        public async Task<IActionResult> OnGetAsync(int? claimId)
        {
            if (claimId == null)
            {
                return NotFound("Không tìm thấy Claim để xóa");
            }
            var claim = await _blogContext.RoleClaims.FindAsync(claimId);
            var role = await _blogContext.Roles.FindAsync(claim.RoleId);
            var listClaim = await _roleManager.GetClaimsAsync(role);
            var deleteClaim = listClaim.Where(c => c.Type == claim.ClaimType && c.Value == claim.ClaimValue).FirstOrDefault();
            if (deleteClaim != null)
            {
                var result = await _roleManager.RemoveClaimAsync(role, deleteClaim);
                if (result.Succeeded)
                {
                    StatusMessage = $"Đã xóa Claim thành công cho Role : {role.Name}";
                }
                return RedirectToPage("./Index");
            }
            return Page();
        }
    }
}
