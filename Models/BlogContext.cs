using Microsoft.EntityFrameworkCore;

namespace CS51_ASP.NET_Razor_EF_1
{
    public class BlogContext : DbContext
    {
        //Do BlogContext tạo ra bởi DbContext và Inject nó vào Service của DI container nên khi tạo ra BlogContext cần tạo ra Constructor với tham số là một DbContextOptions<MyContext> và truyền tham số vào Constructor lớp base() để khi tạo ra các Connection thì những Options mà mình config cho DbContext được Inject vào và truyền sang cho DbContext khởi tạo
        public BlogContext(DbContextOptions<BlogContext> dbContextOptions) : base(dbContextOptions)
        {
        }
        public DbSet<Article> articles { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}