using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CS51_ASP.NET_Razor_EF_1;

namespace CS51_ASP.NET_Razor_EF_1.Pages_Blog
{
    public class DeleteModel : PageModel
    {
        private readonly CS51_ASP.NET_Razor_EF_1.BlogContext _context;

        public DeleteModel(CS51_ASP.NET_Razor_EF_1.BlogContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Article Article { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.articles == null)
            {
                return this.Content("Không tìm thấy bài viết");
            }

            var article = await _context.articles.Where(m => m.Id == id).FirstOrDefaultAsync();

            if (article == null)
            {
                return this.Content("Không tìm thấy bài viết để xóa");
            }
            else
            {
                Article = article;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            var DeleteId = this.Request.Form["Article.Id"];
            if (string.IsNullOrEmpty(DeleteId) || _context.articles == null)
            {
                return this.Content("Không tìm thấy bài viết");
            }
            var article = await _context.articles.FindAsync(int.Parse(DeleteId.ToString()));

            if (article != null)
            {
                Article = article;
                _context.articles.Remove(Article);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
