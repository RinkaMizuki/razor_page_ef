using CS51_ASP.NET_Razor_EF_1;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace App.Admin.Roles {
    public class RolePageModel : PageModel {
        protected RoleManager<IdentityRole> _roleManager {get;set;}
        protected BlogContext _blogContext {get;set;}
        [TempData]
        public string StatusMessage {get;set;}
        public RolePageModel(RoleManager<IdentityRole> roleManager, BlogContext blogContext) {
            _roleManager = roleManager;
            _blogContext = blogContext;
        }
    }
}