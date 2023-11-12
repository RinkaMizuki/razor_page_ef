using CS51_ASP.NET_Razor_EF_1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace App.Admin.Roles
{
    [Authorize(Policy = "AllowEditRole")]
    public class IndexModel : RolePageModel
    {
        public class RoleModel : IdentityRole
        {
            public string[] Claims { get; set; }
        }
        public List<RoleModel> Roles { get; set; }
        public IndexModel(RoleManager<IdentityRole> roleManager, BlogContext blogContext) : base(roleManager, blogContext)
        {
        }

        public async Task OnGet()
        {
            var listRoles = await _roleManager.Roles.ToListAsync();
            List<RoleModel> listRoleClaims = new List<RoleModel>();
            foreach (var role in listRoles)
            {
                var listClaims = await _roleManager.GetClaimsAsync(role);
                var claimString = listClaims.Select(c => c.Type + "=" + c.Value).ToArray();
                var roleModel = new RoleModel()
                {
                    Id = role.Id,
                    Name = role.Name,
                    Claims = claimString,
                };
                listRoleClaims.Add(roleModel);
            }
            Roles = listRoleClaims;
        }
        public void OnPost() => RedirectToPage();
    }
}
