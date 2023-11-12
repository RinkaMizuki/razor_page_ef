@@Lưu ý là mỗi Request là một phiên làm việc => dữ liệu được set sẽ mất khi có một request mới

@Câu lệnh tạo template CRUD
    - dotnet aspnet-codegenerator razorpage -m CS51_ASP.NET_Razor_EF_1.Article -dc CS51_ASP.NET_Razor_EF_1.BlogContext -udl -outDir Pages/Blog --referenceScriptLibraries

@Lưu ý
    - asp-route-... : hãy nhớ nó dùng để đang tham số (RouteValue) vào cuối asp-page

@Các package quan trọng của Identity
    - dotnet add package Microsoft.AspNetCore.Identity
    - dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
    - dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
    - dotnet add package Microsoft.AspNetCore.Identity.UI

@Các packge phụ
    - dotnet add package Microsoft.AspNetCore.Authentication
    - dotnet add package Microsoft.AspNetCore.Http.Abstractions
    - dotnet add package Microsoft.AspNetCore.Authentication.Cookies
    - dotnet add package Microsoft.AspNetCore.Authentication.Facebook
    - dotnet add package Microsoft.AspNetCore.Authentication.Google
    - dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
    - dotnet add package Microsoft.AspNetCore.Authentication.MicrosoftAccount
    - dotnet add package Microsoft.AspNetCore.Authentication.oAuth
    - dotnet add package Microsoft.AspNetCore.Authentication.OpenIDConnect
    - dotnet add package Microsoft.AspNetCore.Authentication.Twitter

=> khi thêm package Identity thì nó sẽ phát sinh ra đối tượng IdentityUser là một lớp đối tượng
tương ứng với các Table liên quan đến User (User, UserRole, UserClaim,...) đối tượng cũng được tạo ra 
dựa trên DbSet và DbContext nên MyContext có thể kế thừa từ DbContext của IdentityUser và có thể custom
thêm các Property cho IdentityUser bằng cách tạo ra một class kế thừa từ IdentityUser và truyền class đó là kiểu dữ liệu của DbContext

- Bước 1 : Custom DbContext and IdentityUser
- Bước 2 : Thêm Identity vào serviceCollections
        => services.AddIdentity<IdentityUser, IdentityRole> (có Role nếu k sử dụng UI mặc định)
- Bước 3 : Thêm các options về Validate thông tin đăng nhập, đăng kí ,...
- Bước 4 : Lưu ý khi tạo bảng bởi Migrations mặc định tên bảng đều có chữ AspNet đầu tên bản nên ta sẽ 
config xóa nó đi (OnModelCreating) và update Database
- Bước 5 : truy cập vào URL mặc định là Identity/Account/Login, Identity/Account/Register
- Bước 6 : Khi có các bảng được tạo ra bởi Dbset Thì ta có thể truy cập Model Users ở bất kì đâu để check State của User (nêu ở view thì xài chỉ thị @inject) và để lấy ra các State và thông tin của User Identity cung cấp 2 Đối tượng là UserManaget và SignInManager để check
- Bước 7 : Tính năng confirm Email tạo một SendMailService kế thừa lớp IMailSender để cho 
ASP.NET hiểu và nó sẽ gửi Email để Confirm Account

- Sử dụng Command này để tạo ra các trang razor mặc định của UI 
=> dotnet aspnet-codegenerator identity -dc Album.Data.AppDbContext


@Tạo razor page Roles (Areas/Admin/Pages/Roles)
- dotnet new page -n Index -o Areas/Admin/Pages/Roles --namespace App.Admin.Roles
