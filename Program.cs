using CS51_ASP.NET_Razor_EF_1;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using razor_page_ef;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using App.Authorize.Requiremnts;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

var httpsConnectionAdapterOptions = new HttpsConnectionAdapterOptions
{
    SslProtocols = System.Security.Authentication.SslProtocols.Tls12,
    ClientCertificateMode = ClientCertificateMode.AllowCertificate,
    ServerCertificate = new System.Security.Cryptography.X509Certificates.X509Certificate2("./certificate.pfx", "0867706538a")

};
builder.WebHost.ConfigureKestrel(options =>
options.ConfigureEndpointDefaults(listenOptions =>
listenOptions.UseHttps(httpsConnectionAdapterOptions)));

builder.WebHost.UseUrls("https://localhost:5118/");

// Add services to the container.
builder.Services.AddRazorPages();

//Add service SendMailService into DI container
builder.Services.AddSingleton<IEmailSender, SendMailService>();

//Add servicer of Identity
builder.Services.AddIdentity<AuthenUser, IdentityRole>()
                .AddEntityFrameworkStores<BlogContext>()
                .AddDefaultTokenProviders();
//sử dụng UI mặc định
// builder.Services.AddDefaultIdentity<AuthenUser>()
//                 .AddEntityFrameworkStores<BlogContext>()
//                 .AddDefaultTokenProviders();

//Configure for Identity

//Change Path Authorize(phải gọi sau AddIdentity)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";
    options.AccessDeniedPath = "/access-denied";
});
builder.Services.AddAuthorization(options =>
{
    //Để thỏa mãn được chính sách AllowEditUser thì phải 
    //1. Login, Có role : Administrators, Vjp
    options.AddPolicy("AllowEditRole", policyBuilder =>
    {
        policyBuilder.RequireAuthenticatedUser();
        //phải có role Administrators
        policyBuilder.RequireRole("Administrators");
        // policyBuilder.RequireRole("Vjp");
        //phải có Claim manage.role thuộc role hoặc thuộc Claim riêng của user
        policyBuilder.RequireClaim("manage.role", new string[] { "all", "tatca" });
    });

    options.AddPolicy("IsGenZ", policyBuilder =>
    {
        policyBuilder.RequireAuthenticatedUser();
        policyBuilder.Requirements.Add(new AuthorizationRequimentGenZ());
    });
    
    options.AddPolicy("Manager", policyBuilder => {
        policyBuilder.RequireAuthenticatedUser();
        policyBuilder.RequireRole("Administrators");
    });

    options.AddPolicy("CanEditArticle", policyBuilder => {
        policyBuilder.RequireAuthenticatedUser();
        policyBuilder.Requirements.Add(new AuthorizationRequimentEdit());
    });

});

//Add service AppAuthorizationHandler into DI container
builder.Services.AddTransient<IAuthorizationHandler, AppAuthorizationHandler>();

builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    //Khoảng thời gian sau khi thay đổi quyền và phải chờ để hệ thống nạp lại quyền
    options.ValidationInterval = TimeSpan.FromSeconds(5);
});
builder.Services.Configure<IdentityOptions>(options =>
{
    // Thiết lập về Password
    options.Password.RequireDigit = false; //không bắt buộc phải có số
    options.Password.RequireLowercase = false; //không bắt buộc phải viết in thường
    options.Password.RequiredLength = 3; //bắt buộc tối thiểu phải có 3 kí tự
    options.Password.RequireNonAlphanumeric = false; //không bắt buộc phải có kí tự đặc biệt
    options.Password.RequireUppercase = false; //không bắt buộc phải viết In hoa
    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

    // Cấu hình Ban - khóa user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
    options.Lockout.MaxFailedAccessAttempts = 3; // Thất bại 5 lần thì khóa
    options.Lockout.AllowedForNewUsers = true; // Mới được tạo cũng sẽ bị khóa

    // Thiết lập về User
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"; // các ký tự đặt tên user
    options.User.RequireUniqueEmail = true;  // Email là duy nhất

    // Cấu hình đăng nhập.
    options.SignIn.RequireConfirmedEmail = true;// Cấu hình xác thực địa chỉ email (email phải tồn tại)
    options.SignIn.RequireConfirmedAccount = true;
    options.SignIn.RequireConfirmedPhoneNumber = false;// Xác thực số điện thoại
});

var configuration = builder.Configuration;

//Add configuration MailSettings

builder.Services.Configure<MailSettings>(configuration.GetSection("MailSettings"));

builder.Services.AddDbContext<BlogContext>(options =>
{
    var ConnectionString = configuration.GetConnectionString("BlogContextConnection");
    if (ConnectionString != null)
    {
        options.UseSqlServer(ConnectionString);
    }
    else
    {
        Console.WriteLine("Something went wrong when connecting to DB");
    }
});

//config Authentication via Provider
builder.Services.AddAuthentication()
                .AddGoogle(googleOptions =>
                {
                    var configOptions = configuration.GetSection("Authentication:Google");
                    googleOptions.ClientId = configOptions["ClientId"];
                    googleOptions.ClientSecret = configOptions["ClientSecret"];
                    googleOptions.CallbackPath = "/dang-nhap-google";
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

app.UseAuthentication(); // phục hồi thông tin User đã xác thực
app.UseAuthorization(); // phục hồi thông tin User đã phân quyền

app.MapRazorPages();

app.Run();
