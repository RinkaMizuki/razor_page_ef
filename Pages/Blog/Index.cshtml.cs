using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using razor_page_ef;

namespace CS51_ASP.NET_Razor_EF_1.Pages_Blog
{
    [Authorize(Policy = "IsGenZ")]
    public class IndexModel : PageModel
    {
        private readonly BlogContext _context;

        public IndexModel(BlogContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true, Name = "p")]
        public int currentPage { get; set; }
        public const int PER_PAGE = 10;
        public int totalPages { get; set; }
        public IList<Article> Article { get; set; } = default!;

        public async Task OnGetAsync(string search)
        {

            var countPage = await _context.articles.CountAsync();

            totalPages = (int)Math.Ceiling((double)(countPage / PER_PAGE));

            if (_context.articles != null)
            {
                Article = await (from a in _context.articles
                                 orderby a.PublishedDate ascending
                                 select a)
                                 .Skip(((currentPage <= 0 ? 1 : currentPage) - 1) * PER_PAGE)
                                 .Take(PER_PAGE)
                                 .ToListAsync();
            }
            if (!string.IsNullOrEmpty(search))
            {
                Article = Article.Where(a => a.Title.Contains(search)).ToList();
                if (Article.Count == 0)
                {
                    ViewData["NotFound"] = "Không tìm thấy";
                }
            }
        }
    }
}
