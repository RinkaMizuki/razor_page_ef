using CS51_ASP.NET_Razor_EF_1;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using razor_page_ef;

namespace App.Admin.User
{
    public class IndexModel : PageModel
    {
        [TempData]
        public string StatusMessage { get; set; }
        [BindProperty(SupportsGet = true, Name = "p")]
        public int currentPage { get; set; }
        public const int PER_PAGE = 10;
        public int totalPages { get; set; }
        public int totalUser { get; set; }
        public class UserAuthenAndRole : AuthenUser
        {
            public string RoleNames { get; set; }
        }
        public List<UserAuthenAndRole> users { get; set; }
        public readonly UserManager<AuthenUser> _userManager;
        public readonly BlogContext _blogContext;
        public IndexModel(UserManager<AuthenUser> userManager, BlogContext blogContext)
        {
            _userManager = userManager;
            _blogContext = blogContext;
        }
        public async Task<IActionResult> OnGet()
        {
            var listUser = _userManager.Users.OrderBy(u => u.UserName);
            totalUser = listUser.Count();
            totalPages = (int)Math.Ceiling((double)(totalUser / PER_PAGE));

            var listUserPagination = listUser.Skip((currentPage - 1) * PER_PAGE).Take(PER_PAGE).Select(u => new UserAuthenAndRole
            {
                Id = u.Id,
                UserName = u.UserName,
            });
            users = await listUserPagination.ToListAsync();

            foreach (var user in users)
            {
                var roleNames = await _userManager.GetRolesAsync(user);
                user.RoleNames = string.Join(", ", roleNames);
            }

            if (users == null)
            {
                return NotFound("Không tìm thấy User");
            }
            return Page();
        }
    }
}
