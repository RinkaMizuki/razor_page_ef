using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CS51_ASP.NET_Razor_EF_1;
using Microsoft.AspNetCore.Authorization;

namespace CS51_ASP.NET_Razor_EF_1.Pages_Blog
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly CS51_ASP.NET_Razor_EF_1.BlogContext _context;

        public IndexModel(CS51_ASP.NET_Razor_EF_1.BlogContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true, Name = "p")]
        public int currentPage { get; set; }
        public const int PER_PAGE = 10;
        public int totalPages { get; set; }
        public IList<Article> Article { get; set; } = default!;

        public async Task OnGetAsync([FromQuery] string search)
        {
            // if (currentPage == 0)
            // {
            //     currentPage = 1;

            // }
            var countPage = await _context.articles.CountAsync();

            totalPages = (int)Math.Ceiling((double)(countPage / PER_PAGE));

            if (_context.articles != null)
            {
                Article = await (from a in _context.articles
                                 orderby a.PublishedDate ascending
                                 select a)
                                 .Skip((currentPage - 1) * PER_PAGE)
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
