using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CS51_ASP.NET_Razor_EF_1.Pages_Blog
{
    public class EditModel : PageModel
    {
        private readonly BlogContext _context;
        private readonly IAuthorizationService _authorizationService;
        public EditModel(BlogContext context, IAuthorizationService authorizationService)
        {
            _context = context;
            _authorizationService = authorizationService;
        }

        [BindProperty]
        public Article Article { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.articles == null)
            {
                return NotFound();
            }

            var article = await _context.articles.FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }
            Article = article;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //Attach đính dữ liệu thay đổi vào phần tử được theo dõi bởi Entity và cho nó State = EntityState.Modified
            _context.Attach(Article).State = EntityState.Modified;

            try
            {
                if ((await _authorizationService.AuthorizeAsync(this.User, Article, "CanEditArticle")).Succeeded)
                {
                    await _context.SaveChangesAsync();
                }
                else {
                    return Content("Bạn không có quyền cập nhật hoặc bài viết này đã quá cũ");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(Article.Id))
                {
                    return this.Content("Không tìm thấy bài viết");
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ArticleExists(int id)
        {
            return _context.articles.Any(e => e.Id == id);
        }
    }
}
