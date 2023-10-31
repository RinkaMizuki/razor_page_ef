using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CS51_ASP.NET_Razor_EF_1.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private BlogContext _myBlogContext { get; set; }

    public IndexModel(ILogger<IndexModel> logger, BlogContext myBlogContext)
    {
        _logger = logger;
        _myBlogContext = myBlogContext;
        logger.LogInformation("Init Index Model!!!");
    }

    public async Task OnGet()
    {
        List<Article> listArticle = await (from a in _myBlogContext.articles
                                    select a).ToListAsync();
        ViewData["ListArticle"] = listArticle;
    }
}
