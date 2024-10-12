using JpsStreet.Web.Service;
using JpsStreet.Web.Service.IService;
using JpsStreet.Web.Utility;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// For configure HttpClient need to call this Services
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
// Finally configure HttpClient
builder.Services.AddHttpClient<ICouponService, CouponService>();
builder.Services.AddHttpClient<IAuthService, AuthService>();

// For populate Base Api from SD
// For populate Base Api from SD
SD.CouponApiBase = builder.Configuration["ServiceUrls:CouponApiBase"];
SD.AuthApiBase = builder.Configuration["ServiceUrls:AuthApiBase"];


// Register for lifetime
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
