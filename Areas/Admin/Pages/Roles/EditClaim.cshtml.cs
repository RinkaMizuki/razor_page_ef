using System.Security.Claims;
using CS51_ASP.NET_Razor_EF_1;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Admin.Roles
{
    public class EditClaimModel : PageModel
    {
        public readonly RoleManager<IdentityRole> _roleManager;
        public readonly BlogContext _blogContext;
        public IdentityRoleClaim<string> claim { get; set; }
        public EditClaimModel(RoleManager<IdentityRole> roleManager, BlogContext blogContext)
        {
            _blogContext = blogContext;
            _roleManager = roleManager;
        }
        [TempData]
        public string StatusMessage { get; set; }
        public class InputModel
        {
            public string NameClaim { get; set; }
            public string TypeClaim { get; set; }
        }
        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync(int? claimId)
        {
            if (claimId == null)
            {
                return NotFound("Không tìm thấy Claim");
            }
            claim = await _blogContext.RoleClaims.FindAsync(claimId);
            if (claim != null)
            {
                Input = new InputModel()
                {
                    NameClaim = claim.ClaimValue,
                    TypeClaim = claim.ClaimType,
                };
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int? claimId)
        {
            if (claimId == null)
            {
                return NotFound("Không tìm thấy Claim");
            }
            var claim = await _blogContext.RoleClaims.FindAsync(claimId);
            var role = await _blogContext.Roles.FindAsync(claim.RoleId);

            if (_blogContext.RoleClaims.Any(c => c.RoleId == role.Id && c.ClaimValue == Input.NameClaim && c.ClaimType == Input.TypeClaim && c.Id != claimId))
            {
                StatusMessage = "Role này đã tồn tại";
                return Page();
            }
            claim.ClaimType = Input.TypeClaim;
            claim.ClaimValue = Input.NameClaim;
            await _blogContext.SaveChangesAsync();
            return RedirectToPage("./Edit", new {
                roleId = role.Id
            });
        }
    }
}
