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
    public class DetailsModel : PageModel
    {
        private readonly CS51_ASP.NET_Razor_EF_1.BlogContext _context;

        public DetailsModel(CS51_ASP.NET_Razor_EF_1.BlogContext context)
        {
            _context = context;
        }

      public Article Article { get; set; }

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
            else 
            {
                Article = article;
            }
            return Page();
        }
    }
}
