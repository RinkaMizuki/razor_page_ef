using CS51_ASP.NET_Razor_EF_1;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
var configuration = builder.Configuration;
builder.Services.AddDbContext<BlogContext>(options =>
{
    var ConnectionString = configuration.GetConnectionString("ArticleContext");
    if (ConnectionString != null)
    {
        options.UseSqlServer(ConnectionString);
    } else 
    {
        Console.WriteLine("Something went wrong when connecting to DB");
    }
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
