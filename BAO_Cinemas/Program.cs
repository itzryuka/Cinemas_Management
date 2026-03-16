using BAO_Cinemas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// 1. Kết nối database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning)));

// 2. Cấu hình Identity (Dịch vụ quản lý Tài khoản)
builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    options.SignIn.RequireConfirmedAccount = false; // Không cần xác thực Email cho đỡ phiền
    options.Password.RequireDigit = false;          // Tắt các quy tắc mật khẩu khó nhằn để test cho nhanh
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddRoles<IdentityRole>() // Cho phép phân quyền Admin/User
.AddEntityFrameworkStores<ApplicationDbContext>();

// 3. Đăng ký các dịch vụ MVC
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // Rất quan trọng: Phải có dòng này để chạy trang Login/Register
builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
var app = builder.Build();

// 4. Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Dùng cái này cho ổn định thay vì MapStaticAssets nếu gặp lỗi

app.UseRouting();

// 5. THỨ TỰ CỰC KỲ QUAN TRỌNG: Authentication (Xác thực) phải trước Authorization (Phân quyền)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 6. Cho phép hệ thống tìm thấy các trang Login/Register mặc định của Identity
app.MapRazorPages();

app.Run();