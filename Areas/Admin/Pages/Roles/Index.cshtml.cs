using CS51_ASP.NET_Razor_EF_1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace App.Admin.Roles
{
    [Authorize(Roles = "Administrators")]
    public class IndexModel : RolePageModel
    {
        public List<IdentityRole> Roles {get;set;}
        public IndexModel(RoleManager<IdentityRole> roleManager, BlogContext blogContext) : base(roleManager, blogContext)
        {
        }

        public async Task OnGet()
        {
            Roles = await _roleManager.Roles.ToListAsync();
        }
        public void OnPost() =>  RedirectToPage();
    }
}
